namespace Cake.Unity3D
{
    /// <summary>
    /// All support build target platforms.
    /// </summary>
    /// <remarks>
    /// Outdated and deprecated Targets are included as somon may want to build a old Unity Version.
    /// </remarks>
    public enum Unity3DBuildPlatform
    {
        /// <summary>
        /// Build a macOS standalone (Intel 64-bit).
        /// </summary>
        StandaloneOSX,
        /// <summary>
        /// Build a macOS standalone Universal.
        /// </summary>
        StandaloneOSXUniversal,
        /// <summary>
        ///  Build a macOS Intel 32-bit standalone.
        /// </summary>
        StandaloneOSXIntel,
        /// <summary>
        /// Build a standalone windows x86 build.
        /// </summary>
        StandaloneWindows,
        /// <summary>
        /// Build a web player.
        /// </summary>
        WebPlayer,
        /// <summary>
        /// Build a streamed web player.
        /// </summary>
        WebPlayerStreamed,
        /// <summary>
        /// Build an iOS player.
        /// </summary>
        iOS,
        /// <summary>
        /// Build an PS3 player.
        /// </summary>
        PS3,
        /// <summary>
        /// Build an Xbox 360 player.
        /// </summary>
        XBOX360,
        /// <summary>
        /// Build an Android .apk standalone app.
        /// </summary>
        Android,
        /// <summary>
        /// Build a Linux standalone.
        /// </summary>
        StandaloneLinux,
        /// <summary>
        /// Build a standalone windows x64 build.
        /// </summary>
        StandaloneWindows64,
        /// <summary>
        /// Build a WebGL build.
        /// </summary>
        WebGL,
        /// <summary>
        /// Build an Windows Store Apps player.
        /// </summary>
        WSAPlayer,
        /// <summary>
        /// Build a Linux 64-bit standalone.
        /// </summary>
        StandaloneLinux64,
        /// <summary>
        /// Build a Linux universal standalone.
        /// </summary>
        StandaloneLinuxUniversal,
        /// <summary>
        /// Build a Windows Phone 8 player.
        /// </summary>
        WP8Player,
        /// <summary>
        /// Build a macOS Intel 64-bit standalone.
        /// </summary>
        StandaloneOSXIntel64,
        /// <summary>
        /// Build a BlackBerry player.
        /// </summary>
        BlackBerry,
        /// <summary>
        /// Build a Tizen player.
        /// </summary>
        Tizen,
        /// <summary>
        /// Build a PS Vita Standalone.
        /// </summary>
        PSP2,
        /// <summary>
        /// Build a PS4 Standalone.
        /// </summary>
        PS4,
        /// <summary>
        /// Build a PSM player.
        /// </summary>
        PSM,
        /// <summary>
        /// Build a Xbox One Standalone.
        /// </summary>
        XboxOne,
        /// <summary>
        /// Build a SamsungTV player.
        /// </summary>
        SamsungTV,
        /// <summary>
        /// Build to Nintendo 3DS platform.
        /// </summary>
        N3DS,
        /// <summary>
        /// Build a Nintendo WiiU player.
        /// </summary>
        WiiU,
        /// <summary>
        /// Build to Apple's tvOS platform.
        /// </summary>
        tvOS,
        /// <summary>
        /// Build a Nintendo Switch player.
        /// </summary>
        Switch
    }
}
