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
        /// <param name="projectFolder">The absolute path to the Unity3D project to build.</param>
        /// <param name="options">The build options to use when building the project.</param>
        public Unity3DBuildDependencyContext(ICakeContext context, FilePath projectFolder, Unity3DBuildDependencyOptions options) 
            : base(context, projectFolder, options)
        {            
            if (!System.IO.File.Exists(options.UnityEditorLocation))
            {
                throw new Exception($"The Unity Editor location '{options.UnityEditorLocation}' does not exist.");
            }

            if (options.TargetDependencieNames == null || options.TargetDependencieNames.Count <= 0)
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
            Console.WriteLine($"ForceScriptInstall: {string.Join(",", m_buildOptions.TargetDependencieNames)}");
        }

        /// <summary>
        /// Perform a build using the contexts project directory and build options.
        /// </summary>
        public override void Build()
        {
            base.Build();

            // 1. Force Unity to emit vs Project
            //      UnityEditor.SyncVS.SyncSolution // Static Internal Type

            // 2. Create or Update Dependency Solution
            // 3. Build Dependency Solution
            // 4. Copy Assets to Target
        }
    }
}
