using System.Collections.Generic;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core;
using Cake.Unity3D.Helpers;

namespace Cake.Unity3D
{
    /// <summary>
    /// Adds the ability to build Unity3D projects using cake.
    /// </summary>
    [CakeAliasCategory("Unity3D")]
    public static class Unity3DAliases
    {
        /// <summary>
        /// Build a provided Unity3D project with the specified build options.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="projectOptions">The Unity3d Project options to use when building the project.</param>
        /// <param name="options">The build options to use when building the project.</param>
        [CakeMethodAlias]
        public static void BuildUnity3DProject(this ICakeContext context, Unity3DProjectOptions projectOptions, Unity3DBuildOptions options)
        {
            try
            {
                var unityBuildContext = new Unity3DBuildContext(context, projectOptions, options);
                unityBuildContext.DumpOptions();
                unityBuildContext.Build();
            }
            catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Build a provided Unity3D project with the specified build options.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="projectOptions">The Unity3d Project options to use when building the project.</param>
        /// <param name="options">The build options to use when building the project.</param>
        [CakeMethodAlias]
        public static void BuildUnity3DDependency(this ICakeContext context, Unity3DProjectOptions projectOptions, Unity3DBuildDependencyOptions options)
        {
            var unityBuildContext = new Unity3DBuildDependencyContext(context, projectOptions, options);
            unityBuildContext.DumpOptions();
            unityBuildContext.Build();
        }

        /// <summary>
        /// Enable or Dissable Unity3D Project Dependencies
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="projectOptions">The Unity3d Project options to use when building the project.</param>
        /// <param name="dependencyName">Name of the Depenendency. in com.org.packet format</param>
        /// <param name="value">Version of file:{fileName} of the dependency</param>
        [CakeMethodAlias]
        public static void SetUnity3DDependency(this ICakeContext context, Unity3DProjectOptions projectOptions, string dependencyName, string value)
        {
            string path = System.IO.Path.Combine(projectOptions.ProjectFolder.ToString(), "Packages/manifest.json");
            var manifest = PackageManifest.ReadFile(path);
            bool hasChange = false;

            if(string.IsNullOrEmpty(value))
            {
                if(manifest.dependencies.ContainsKey(dependencyName))
                {
                    // Clear Entry
                    manifest.dependencies.Remove(dependencyName);
                    hasChange = true;
                    System.Console.WriteLine($"- {dependencyName}");
                }
            }
            else
            {
                string currentValue;
                if(!manifest.dependencies.TryGetValue(dependencyName, out currentValue))
                {
                    // Add Entry
                    manifest.dependencies.Add(dependencyName, value);
                    hasChange = true;
                    System.Console.WriteLine($"+ {dependencyName} => {value}");
                }
                else if(currentValue != value)
                {
                    // Update Entry
                    manifest.dependencies[dependencyName] = value;
                    hasChange = true;
                    System.Console.WriteLine($"  {dependencyName} => {value}");
                }
            }

            if (hasChange)
            {
                PackageManifest.WriteFile(path, manifest);
            }
        }

        [CakeMethodAlias]
        public static void SetUnity3DPlayerSettings(this ICakeContext context, Unity3DProjectOptions projectOptions, Unity3DPlayerSettings settings)
        {
            var jobContext = new Unity3DPlayerContext(context, projectOptions, settings);
            jobContext.DumpOptions();
            jobContext.Build();
        }

        /// <summary>
        /// Locate all installed version of Unity3D.
        /// Warning: This currently only works for Windows and has only been tested on Windows 10.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <returns>A dictionary containing 'key' Unity version, 'value' absolute install path</returns>
        [CakeMethodAlias]
        public static Dictionary<string, string> GetAllUnityInstalls(this ICakeContext context)
        {
            return Helpers.Unity3DEditor.LocateUnityInstalls();
        }

        /// <summary>
        /// Try and get the absolute install path for a specific Unity3D version.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="version">The version to try and locate.</param>
        /// <param name="installPath">If found the absolute install path will be written to this out variable</param>
        /// <returns>True if the editor version was found, false otherwise.</returns>
        [CakeMethodAlias]
        public static bool TryGetUnityInstall(this ICakeContext context, string version, out string installPath)
        {
            var installs = context.GetAllUnityInstalls();
            return installs.TryGetValue(version, out installPath);
        }
    }
}

