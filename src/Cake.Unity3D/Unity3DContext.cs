using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Unity3D.Helpers;

namespace Cake.Unity3D
{
    /// <summary>
    /// The core context for a Unity3D project.
    /// </summary>
    public class Unity3DContext<T>
    {
        /// <summary>
        /// The cake context being used.
        /// </summary>
        protected readonly ICakeContext m_cakeContext;

        /// <summary>
        /// The Unity3d Project options to use when building the project.
        /// </summary>
        protected readonly Unity3DProjectOptions m_projectOptions;

        /// <summary>
        /// The build options to use when building the project.
        /// </summary>
        protected readonly T m_buildOptions;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The current cake context.</param>
        /// <param name="projectOptions">The Unity3d Project options to use when building the project.</param>
        /// <param name="options">The build options to use when building the project.</param>
        public Unity3DContext(ICakeContext context, Unity3DProjectOptions projectOptions, T options)
        {
            if (projectOptions == null)
            {
                throw new Exception("Project Options must be set.");
            }

            if (options == null)
            {
                throw new Exception("Build Options mus be set.");
            }

            m_cakeContext = context;
            m_projectOptions = projectOptions;
            m_buildOptions = options;
        }

        /// <summary>
        /// Outputs all current options for this build context.
        /// </summary>
        public virtual void DumpOptions()
        {
            Console.WriteLine($"ProjectFolder: {m_projectOptions.ProjectFolder}");
            Console.WriteLine($"UnityEditorLocation: {m_projectOptions.UnityEditorLocation}");
            Console.WriteLine($"OutputEditorLog: {m_projectOptions.OutputEditorLog}");
            Console.WriteLine($"ForceScriptInstall: {m_projectOptions.ForceScriptInstall}");
        }

        /// <summary>
        /// Perform a build using the contexts project directory and build options.
        /// </summary>
        public virtual void Build()
        {
            // Make sure the automated build script has been copied to the Unity project.
            // The build script is a Unity script that actually invokes the build.
            if (!ProjectHasAutomatedBuildScript() || m_projectOptions.ForceScriptInstall)
            {
                InstallAutomatedBuildScript();
            }
        }

        /// <summary>
        /// Gets the path to the automated build script within a provided Unity3D project path.
        /// </summary>
        /// <returns>The absolute path to the automated build script.</returns>
        private string GetAutomatedBuildScriptPath()
        {
            return System.IO.Path.Combine(m_projectOptions.ProjectFolder.FullPath, "Assets", "Cake.Unity3D", "Editor", "AutomatedBuild.cs");
        }

        /// <summary>
        /// Checks to see if the provided project has the automated build script already.
        /// </summary>
        /// <returns>True if the build script already exists.</returns>
        private bool ProjectHasAutomatedBuildScript()
        {
            return System.IO.File.Exists(GetAutomatedBuildScriptPath());
        }

        /// <summary>
        /// Extract the embedded automated build script resource to the Unity3D project.
        /// </summary>
        private void InstallAutomatedBuildScript()
        {
            Console.WriteLine("Installing AutomatedBuild Script...");

            var path = GetAutomatedBuildScriptPath();

            // Make sure the directories required for the script exist.
            var installDirectory = System.IO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(installDirectory) && !System.IO.Directory.Exists(installDirectory))
            {
                System.IO.Directory.CreateDirectory(installDirectory);
            }

            // Extract the embedded resource to the Unity3D project provided.
            var resourceName = "Cake.Unity3D.Resources.AutomatedBuild.template";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (resource == null)
                {
                    throw new Exception($"Failed to find the embedded resource '{resourceName}'");
                }

                using (var file = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }

            Console.WriteLine($"AutomatedBuild Script installed to \"{path}\"");
        }

        /// <summary>
        /// Invoke the Unity3D editor
        /// </summary>
        /// <param name="method">The Unity scipt method to call</param>
        /// <param name="args">arguments for the command line</param>
        protected void RunUnityCommand(string method, Dictionary<string, string> args)
        {
            // The command line arguments to use.
            // All options which start with duel hyphens are used internally by
            // the automated build script.
            var buildArguments =
                "-batchmode " +
                "-quit " +
                $"-projectPath \"{System.IO.Path.GetFullPath(m_projectOptions.ProjectFolder.FullPath)}\" " +
                $"-executeMethod {method} ";

            if (args != null)
            {
                foreach (KeyValuePair<string, string> arg in args)
                {
                    buildArguments += $"--{arg.Key}={EncodeCmdLineValue(arg.Value)} ";
                }
            }

            string fileName = m_projectOptions.UnityEditorLocation;
            if(fileName.EndsWith(".app"))
            {
                // Use open from command line to invoke unity
                buildArguments = $"{fileName} -W --args {buildArguments}";
                fileName = "open";
            }

            // Create the process using the Unity editor and arguments above.
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = fileName,
                    Arguments = buildArguments,
                    CreateNoWindow = true,
                    UseShellExecute = true,
                }
            };

            Console.WriteLine($"Running: \"{m_projectOptions.UnityEditorLocation}\" {buildArguments}");

            // Unity will output to a log, and not to the console.
            // So we have to periodically parse the log and redirect the output to the console.
            // We do this by storing how far through the log we have already seen and outputting
            // the remaining lines. This works because Unity flushes in full lines, so we should
            // always have a full line to output.
            var outputLineIndex = 0;
            var logLocation = Unity3DEditor.GetEditorLogLocation();

            // Start the process.
            process.Start();

            // Whilst the process is still running, periodically redirect the editor log
            // to the console if required.
            while (!process.HasExited)
            {
                System.Threading.Thread.Sleep(100);
                Unity3DEditor.ProcessEditorLog(m_cakeContext, m_projectOptions.OutputEditorLog, logLocation, ref outputLineIndex);
            }

            if (!Unity3DEditor.BuildStatusFromLogs(m_cakeContext, logLocation))
            {
                throw new Exception("An error was reported in the Unity3D editor log.");
            }
        }

        string EncodeCmdLineValue(string input)
        {
            // Check if input has spaces and needs to be encoded
            if(input.IndexOf(" ") >= 0)
            {
                return "'" + input + "'";
            }
            return input;
        }
    }
}
