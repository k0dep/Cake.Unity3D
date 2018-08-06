using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;
using Cake.Unity3D.Helpers;

namespace Cake.Unity3D
{
    /// <summary>
    /// The core application build context for the a Unity3D project.
    /// </summary>
    public class Unity3DBuildContext : Unity3DContext<Unity3DBuildOptions>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The current cake context.</param>
        /// <param name="projectFolder">The absolute path to the Unity3D project to build.</param>
        /// <param name="options">The build options to use when building the project.</param>
        public Unity3DBuildContext(ICakeContext context, FilePath projectFolder, Unity3DBuildOptions options) 
            : base(context, projectFolder, options)
        {
            if (string.IsNullOrEmpty(options.OutputPath))
            {
                throw new Exception("The output path build option must be set.");
            }

            if (options.OutputPath.Contains(" "))
            {
                throw new Exception("The output path can not contain any spaces.");
            }

            if (options.BuildVersion.Contains(" "))
            {
                throw new Exception("The build version can not contain any spaces.");
            }

            if (!System.IO.File.Exists(options.UnityEditorLocation))
            {
                throw new Exception($"The Unity Editor location '{options.UnityEditorLocation}' does not exist.");
            }
        }

        /// <summary>
        /// Outputs all current options for this build context.
        /// </summary>
        public override void DumpOptions()
        {
            base.DumpOptions();
            Console.WriteLine($"Platform: {m_buildOptions.Platform}");
            Console.WriteLine($"OutputPath: {m_buildOptions.OutputPath}");
            Console.WriteLine($"BuildVersion: {m_buildOptions.BuildVersion}");
        }

        /// <summary>
        /// Perform a build using the contexts project directory and build options.
        /// </summary>
        public override void Build()
        {
            base.Build();

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("--output-path", m_buildOptions.OutputPath);
            args.Add("--platform", m_buildOptions.Platform.ToString());

            if (!string.IsNullOrEmpty(m_buildOptions.BuildVersion))
            {
                args.Add("--version", m_buildOptions.BuildVersion);
            }

            RunUnityCommand("Cake.Unity3D.AutomatedBuild.Build", args);
        }
    }
}
