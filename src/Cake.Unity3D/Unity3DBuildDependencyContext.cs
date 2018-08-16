using System;
using System.Linq;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Common.IO;
using Cake.Unity3D.Helpers;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Solution.Project;

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

            // 3. Build Dependency Solution
            Console.WriteLine($":: Build Dependency Solution");
            foreach (var dep in dependencies)
            {
                foreach (var depAsm in dep.AsmList)
                {
                    string projPath = System.IO.Path.Combine(projectFolder, depAsm.Name + ".csproj");
                    m_cakeContext.MSBuild(projPath, (settings) =>
                    {
                        settings.Configuration = m_buildOptions.DebugBuild ? "Debug" : "Release";
                    });
                }
            }

            // 4. Copy Assets to Target
            Console.WriteLine($":: Copy Assets to Target");
            string asmBinDir = System.IO.Path.Combine(projectFolder,
                m_buildOptions.DebugBuild ? "Temp/bin/Debug" : "Temp/bin/Release");
            foreach (var dep in dependencies)
            {
                string depenencySourcePath = System.IO.Path.Combine(projectFolder, "Packages", dep.SourceInfo.Source);
                string depenencyTargetPath = System.IO.Path.Combine(projectFolder, "Packages", dep.SourceInfo.Target);

                Information($"Copy: {dep.SourceInfo.Source} => {dep.SourceInfo.Target}");
                Information($"\t{depenencySourcePath}");
                Information($"\t{depenencyTargetPath}");

                // Make Target Dir
                m_cakeContext.EnsureDirectoryExists(depenencyTargetPath);

                // Clear Target Dir form all files not present in Source
                var existringFiles = m_cakeContext.GetFiles(depenencyTargetPath + "/**/*");
                string clearPath = System.IO.Path.GetFullPath(depenencyTargetPath).Replace("\\", "/") + "/";
                foreach (var file in existringFiles)
                {
                    if (!file.FullPath.EndsWith(".meta"))
                    {
                        string relPath = file.FullPath.Replace(clearPath, "");
                        string sourcePath = System.IO.Path.Combine(depenencySourcePath, relPath);
                        if (!m_cakeContext.FileExists(sourcePath))
                        {
                            m_cakeContext.DeleteFile(file);
                        }
                    }
                }

                // Copy all Assamblys
                foreach (var depAsm in dep.AsmList)
                {
                    // Copy compiled asm files into target dependency
                    string asmFileName = depAsm.Name + ".dll";
                    string asmBinPath = System.IO.Path.Combine(asmBinDir, asmFileName);
                    Console.WriteLine($"\t{asmBinPath} => {depenencyTargetPath}");
                    m_cakeContext.CopyFile(asmBinPath, System.IO.Path.Combine(depenencyTargetPath, asmFileName));
                    if(m_buildOptions.CopyPdb)
                    {
                        string pdbFileName = depAsm.Name + ".pdb";
                        string pdbBinPath = System.IO.Path.Combine(asmBinDir, pdbFileName);
                        Console.WriteLine($"\t{pdbBinPath} => {depenencyTargetPath}");
                        m_cakeContext.CopyFile(pdbBinPath, System.IO.Path.Combine(depenencyTargetPath, pdbFileName));
                    }
                }

                // Copy all the assets
                IEnumerable<FilePath> fileList = null;
                if (m_buildOptions.UseSourceMetaFiles)
                {
                    fileList = m_cakeContext.GetFiles(depenencySourcePath + "/**/*").Where(f =>
                    {
                        return !(f.FullPath.EndsWith(".cs") ||
                            f.FullPath.EndsWith(".asmdef"));
                    });
                }
                else
                {
                    fileList = m_cakeContext.GetFiles(depenencySourcePath + "/**/*").Where(f =>
                    {
                        return !(f.FullPath.EndsWith(".cs") ||
                            f.FullPath.EndsWith(".asmdef") ||
                            f.FullPath.EndsWith(".meta"));
                    });
                }
                m_cakeContext.CopyFiles(fileList, depenencyTargetPath, true);
            }
        }

        /// <summary>
        /// Utility function to Log in the same way as in CakeBuild script
        /// </summary>
        /// <param name="msg"></param>
        void Information(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Build informazion for a single Dependency
        /// </summary>
        class Dependency
        {
            /// <summary>
            /// The Finformation from the BuildOptions for this Dependency
            /// </summary>
            public Unity3DBuildDependencyOptions.Dependency SourceInfo { get; set; }

            /// <summary>
            /// A list of all Assamblys present inside this Dependency
            /// </summary>
            public List<DependencyAssambly> AsmList { get; set; } = new List<DependencyAssambly>();
        }

        /// <summary>
        /// Build information for a single Depndency Assambly
        /// </summary>
        class DependencyAssambly
        {
            /// <summary>
            /// The name of the Dependency this Assambly resides in
            /// </summary>
            public string DependencyName { get; set; }

            /// <summary>
            /// The name of the Assambly iteself
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The Path to the asmdef file of this Assambly
            /// </summary>
            public FilePath AsmDefPath { get; set; }
        }
    }
}
