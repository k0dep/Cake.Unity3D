using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Common.IO;
using Cake.Unity3D.Helpers;
using Cake.Common.Tools.MSBuild;

namespace Cake.Unity3D
{
    /// <summary>
    /// A build context for building Unity3D Dependencys
    /// </summary>
    public class Unity3DBuildDependencyContext : Unity3DContext<Unity3DBuildDependencyOptions>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The current cake context.</param>
        /// <param name="projectOptions">The Unity3d Project options to use when building the project.</param>
        /// <param name="options">The build options to use when building the project.</param>
        public Unity3DBuildDependencyContext(ICakeContext context, Unity3DProjectOptions projectOptions, Unity3DBuildDependencyOptions options) 
            : base(context, projectOptions, options)
        {            
            if (!System.IO.File.Exists(projectOptions.UnityEditorLocation))
            {
                throw new Exception($"The Unity Editor location '{projectOptions.UnityEditorLocation}' does not exist.");
            }

            if (options.Dependencies == null || options.Dependencies.Count <= 0)
            {
                throw new Exception($"You need to set at least one TargetDependencieNames");
            }
        }

        /// <summary>
        /// Outputs all current options for this build context.
        /// </summary>
        public override void DumpOptions()
        {
            base.DumpOptions();
            Console.WriteLine($"Build Dependencies:");
            foreach(var dependency in m_buildOptions.Dependencies)
            {
                Console.WriteLine($"\t{dependency.Source} -> {dependency.Target}");
            }
        }

        /// <summary>
        /// Perform a build using the contexts project directory and build options.
        /// </summary>
        public override void Build()
        {
            base.Build();

            // 1. Force Unity to emit VisualStudio Project's for all enabled Plugins
            Console.WriteLine($"::  Force Unity to emit VisualStudio Project");
            RunUnityCommand("Cake.Unity3D.AutomatedBuild.SyncVS", null);

            string projectFolder = m_projectOptions.ProjectFolder.ToString();

            // 2. Locate all asmDef files
            Console.WriteLine($"::  Locate all asmDef files");
            List<Dependency> dependencies = new List<Dependency>();
            foreach(var dependency in m_buildOptions.Dependencies)
            {
                string dependencyPath = System.IO.Path.Combine(projectFolder, "Packages", dependency.Source);
                string pattern = dependencyPath + "/**/*.asmdef";

                Console.WriteLine($"Dependency {dependency.Source} => {dependencyPath}");
                Dependency dep = new Dependency
                {
                    SourceInfo = dependency,
                };
                dependencies.Add(dep);

                foreach (var asmDefPath in m_cakeContext.GetFiles(pattern))
                {
                    AssemblyDefinition definition = AssemblyDefinition.ReadFile(asmDefPath.ToString());

                    DependencyAssambly depAsm = new DependencyAssambly()
                    {
                        DependencyName = dependency.Source,
                        Name = definition.name,
                        AsmDefPath = asmDefPath,
                    };
                    Console.WriteLine($"Assambly {depAsm.Name} => {depAsm.AsmDefPath}");
                    dep.AsmList.Add(depAsm);
                }
            }

            // 3. Create or Update Dependency Solution
            Console.WriteLine($":: Create or Update Dependency Solution");
            string dependenciySolutionPath = System.IO.Path.Combine(projectFolder, "PacklageAssembly.sln");
            VisualStudioSolution solution;
            if (!VisualStudioSolution.TryReadFile(dependenciySolutionPath, out solution))
            {
                solution = new VisualStudioSolution();
            }
            foreach (var dep in dependencies)
            {
                foreach (var depAsm in dep.AsmList)
                {
                    Console.WriteLine($"+Project {depAsm.Name}.csproj");
                    solution.AddProject(System.IO.Path.Combine(projectFolder, depAsm.Name + ".csproj"));
                }
            }
            VisualStudioSolution.WriteFile(dependenciySolutionPath, solution);

            // 4. Build Dependency Solution
            Console.WriteLine($":: Build Dependency Solution");
            m_cakeContext.MSBuild(dependenciySolutionPath, (settings) => 
            {
                foreach (var dep in dependencies)
                {
                    foreach (var depAsm in dep.AsmList)
                    {
                        settings.Targets.Add(depAsm.Name);
                    }
                }
            });

            // 5. Copy Assets to Target
            Console.WriteLine($":: Copy Assets to Target");
            string asmBinDir = System.IO.Path.Combine(projectFolder, "Temp/bin/Debug");
            foreach (var dep in dependencies)
            {
                string depenencySourcePath = System.IO.Path.Combine(projectFolder, "Packages", dep.SourceInfo.Source);
                string depenencyTargetPath = System.IO.Path.Combine(projectFolder, "Packages", dep.SourceInfo.Target);

                Console.WriteLine($"Copy: {dep.SourceInfo.Source} => {dep.SourceInfo.Target}");
                Console.WriteLine($"\t{depenencyTargetPath}");
                Console.WriteLine($"\t{depenencySourcePath}");

                foreach (var depAsm in dep.AsmList)
                {
                    // Copy compiled asm files into target dependency
                    string asmBinPath = System.IO.Path.Combine(asmBinDir, depAsm.Name + ".dll");
                    m_cakeContext.CopyFile(asmBinPath, depenencyTargetPath);
                }
                // Copy all the assets
                string pattern = depenencySourcePath + "/**/*&exclude=/**/*.cs&exclude==/**/*.asmdef";
                m_cakeContext.CopyFiles(pattern, depenencyTargetPath, true);
            }
        }

        class Dependency
        {
            public Unity3DBuildDependencyOptions.Dependency SourceInfo { get; set; }
            public List<DependencyAssambly> AsmList { get; set; } = new List<DependencyAssambly>();
        }

        class DependencyAssambly
        {
            public string DependencyName { get; set; }
            public string Name { get; set; }
            public FilePath AsmDefPath { get; set; }
        }
    }
}
