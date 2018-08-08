using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Unity3D
{
    [System.Flags]
    public enum Unity3DBuildPlayerOptions
    {
        
        None = 0,
        Development = 1,
        /// <summary>
        /// Run the built player.
        /// </summary>
        AutoRunPlayer = 4,
        /// <summary>
        /// Show the built player.
        /// </summary>
        ShowBuiltPlayer = 8,
        /// <summary>
        /// Build a compressed asset bundle that contains streamed scenes loadable with the
        /// UnityWebRequest class.
        /// </summary>
        BuildAdditionalStreamedScenes = 16,
        /// <summary>
        /// Used when building Xcode (iOS) or Eclipse (Android) projects.
        /// </summary>
        AcceptExternalModificationsToPlayer = 32,
        InstallInBuildFolder = 64,
        /// <summary>
        /// Copy UnityObject.js alongside Web Player so it wouldn't have to be downloaded
        /// from internet.
        /// </summary>
        WebPlayerOfflineDeployment = 128,
        /// <summary>
        /// Start the player with a connection to the profiler in the editor.
        /// </summary>
        ConnectWithProfiler = 256,
        /// <summary>
        /// Allow script debuggers to attach to the player remotely.
        /// </summary>
        AllowDebugging = 512,
        /// <summary>
        /// Symlink runtime libraries when generating iOS Xcode project. (Faster iteration
        /// time).
        /// </summary>
        SymlinkLibraries = 1024,
        /// <summary>
        /// Don't compress the data when creating the asset bundle.
        /// </summary>
        UncompressedAssetBundle = 2048,
        /// <summary>
        /// Sets the Player to connect to the Editor.
        /// </summary>
        ConnectToHost = 4096,
        /// <summary>
        /// Build headless Linux standalone.
        /// </summary>
        EnableHeadlessMode = 16384,
        /// <summary>
        /// Build only the scripts of a project.
        /// </summary>
        BuildScriptsOnly = 32768,
        /// <summary>
        /// Include assertions in the build. By default, the assertions are only included
        /// in development builds.
        /// </summary>
        ForceEnableAssertions = 131072,
        /// <summary>
        /// Use chunk-based LZ4 compression when building the Player.
        /// </summary>
        CompressWithLz4 = 262144,
        /// <summary>
        /// Use chunk-based LZ4 high-compression when building the Player.
        /// </summary>
        CompressWithLz4HC = 524288,
        ComputeCRC = 1048576,
        /// <summary>
        /// Do not allow the build to succeed if any errors are reporting during it.
        /// </summary>
        StrictMode = 2097152,
        /// <summary>
        /// Build will include Assemblies for testing.
        /// </summary>
        IncludeTestAssemblies = 4194304,
        /// <summary>
        /// Will force the buildGUID to all zeros.
        /// </summary>
        NoUniqueIdentifier = 8388608
    }
}
