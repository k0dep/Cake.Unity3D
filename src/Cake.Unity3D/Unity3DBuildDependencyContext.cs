using System;
using System.Diagnostics;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;
using Cake.Unity3D.Helpers;

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

            // 1. Force Unity to emit vs Project
            //      UnityEditor.SyncVS.SyncSolution // Static Internal Type
            RunUnityCommand("Cake.Unity3D.AutomatedBuild.SyncVS", null);

            // 2. Create or Update Dependency Solution


            // 3. Build Dependency Solution
            

            // 4. Copy Assets to Target


        }
    }
}
