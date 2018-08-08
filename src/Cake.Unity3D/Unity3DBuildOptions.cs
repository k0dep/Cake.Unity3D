using System.Collections.Generic;

namespace Cake.Unity3D
{
    /// <summary>
    /// All build options available when performing a Unity3D build.
    /// </summary>
    public class Unity3DBuildOptions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Unity3DBuildOptions()
        {
            Platform = Unity3DBuildPlatform.StandaloneWindows64;
            Options = Unity3DBuildPlayerOptions.None;
            Scenes = null;
        }

        /// <summary>
        /// The platform to build for.
        /// Default: StandaloneWindows64
        /// </summary>
        public Unity3DBuildPlatform Platform { get; set; }

        /// <summary>
        /// Additional BuildOptions, like whether to run the built player.
        /// Default: None
        /// </summary>
        public Unity3DBuildPlayerOptions Options { get; set; }

        /// <summary>
        /// The target path for the build project.
        /// Default: null
        /// </summary>
        public string OutputPath { get; set; } = "";

        /// <summary>
        /// A custom string used as the build version of the Unity3D project.
        /// This will be used as the bundle version in the built application.
        /// </summary>
        public string BuildVersion { get; set; } = "";

        /// <summary>
        /// The path to an manifest file describing all of the asset bundles used in the build (optional).     
        /// </summary>
        public string AssetBundleManifestPath { get; set; } = "";

        /// <summary>
        /// The scenes to be included in the build. If empty, the currently open scene will
        /// be built. Paths are relative to the project folder (AssetsMyLevelsMyScene.unity).
        /// If null is proved the scenes enabled in the editor will be used.
        /// Default: null list 
        /// </summary>
        public IEnumerable<string> Scenes { get; set; }
    }
}
