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
        /// <param name="projectOptions">The Unity3d Project options to use when building the project.</param>
        /// <param name="options">The build options to use when building the project.</param>
        public Unity3DBuildContext(ICakeContext context, Unity3DProjectOptions projectOptions, Unity3DBuildOptions options) 
            : base(context, projectOptions, options)
        {
            if (string.IsNullOrEmpty(options.OutputPath))
            {
                throw new Exception("The output path build option must be set.");
            }

            if (string.IsNullOrEmpty(options.OutputPath))
            {
                throw new Exception("The output path must be set.");
            }
            else if (options.OutputPath.Contains(" "))
            {
                throw new Exception("The output path can not contain any spaces.");
            }

            if (!string.IsNullOrEmpty(options.BuildVersion) && options.BuildVersion.Contains(" "))
            {
                throw new Exception("The build version can not contain any spaces.");
            }

            if (!System.IO.File.Exists(projectOptions.UnityEditorLocation))
            {
                throw new Exception($"The Unity Editor location '{projectOptions.UnityEditorLocation}' does not exist.");
            }
        }

        /// <summary>
        /// Outputs all current options for this build context.
        /// </summary>
        public override void DumpOptions()
        {
            base.DumpOptions();
            Console.WriteLine($"Platform: {m_buildOptions.Platform}");
            Console.WriteLine($"Options: {m_buildOptions.Options}");
            Console.WriteLine($"OutputPath: {m_buildOptions.OutputPath}");
            Console.WriteLine($"BuildVersion: {m_buildOptions.BuildVersion}");
            Console.WriteLine($"AssetBundleManifestPath: {m_buildOptions.AssetBundleManifestPath}");
            if (m_buildOptions.Scenes != null)
            {
                Console.WriteLine($"Scenes: {string.Join(";", m_buildOptions.Scenes)}");
            }
            else
            {
                Console.WriteLine($"Scenes: null");
            }
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
            if(!string.IsNullOrEmpty(m_buildOptions.AssetBundleManifestPath))
            {
                args.Add("--asset-bundle-manifest-path", m_buildOptions.AssetBundleManifestPath);
            }
            if(m_buildOptions.Options != Unity3DBuildPlayerOptions.None)
            {
                args.Add("--options", string.Join(",", m_buildOptions.Options.GetIndividualFlags()));
            }
            if (m_buildOptions.Scenes != null)
            {
                args.Add("--scenes", string.Join(";", m_buildOptions.Scenes ));
            }
            if (!string.IsNullOrEmpty(m_buildOptions.BuildVersion))
            {
                args.Add("--version", m_buildOptions.BuildVersion);
            }

            RunUnityCommand("Cake.Unity3D.AutomatedBuild.Build", args);
        }
    }
}
