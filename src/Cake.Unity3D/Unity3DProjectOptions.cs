using Cake.Core;
using Cake.Core.IO;

namespace Cake.Unity3D
{
    /// <summary>
    /// Base options required for any Unity3D Command
    /// </summary>
    public class Unity3DProjectOptions
    {
        /// <summary>
        /// The location of the Unity.exe to use.
        /// </summary>
        public string UnityEditorLocation { get; set; }

        /// <summary>
        /// The absolute path to the Unity3D project to build.
        /// </summary>
        public FilePath ProjectFolder { get; set; }

        /// <summary>
        /// Should the editor log produced by Unity3D whilst building
        /// be output to the console.
        /// </summary>
        public bool OutputEditorLog { get; set; } = true;

        /// <summary>
        /// Should we install the automated build script
        /// even if we find an existing one.
        /// </summary>
        public bool ForceScriptInstall { get; set; } = false;
    }
}
