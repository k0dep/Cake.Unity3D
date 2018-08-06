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
        }

        /// <summary>
        /// The platform to build for.
        /// Default: StandaloneWindows64
        /// </summary>
        public Unity3DBuildPlatform Platform { get; set; }

        /// <summary>
        /// The target path for the build project.
        /// Default: null
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// A custom string used as the build version of the Unity3D project.
        /// This will be used as the bundle version in the built application.
        /// </summary>
        public string BuildVersion { get; set; }
    }
}
