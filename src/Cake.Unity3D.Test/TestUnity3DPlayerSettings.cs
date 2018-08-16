using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Unity3D;

namespace Cake.Unity3D.Test
{
    [TestClass]
    public class TestUnity3DPlayerSettings
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void TestComplexSettings()
        {
            var settings = CreateComplexSettings();
            
            settings.DumpOptions();

            Dictionary<string, string> args = new Dictionary<string, string>();
            settings.WriteArgs(args);
            StringBuilder sb = new StringBuilder();
            if (args != null)
            {
                foreach (KeyValuePair<string, string> arg in args)
                {
                    sb.Append($"--{arg.Key}={arg.Value} ");
                }
            }

            string outString = sb.ToString();
            TestContext.WriteLine(outString);
        }

        Unity3DPlayerSettings CreateComplexSettings()
        {
            var settings = new Unity3DPlayerSettings
            {
                // General Settings
                CompanyName = "Wolpertingergames",
                ProductName = "Test\\ App\\ Come\\ on",
                ApplicationIdentifier = "com.woplertingertgame.testapp",
                BundleVersion = "0.1",

                // DotNet / Code / Script Settings
                ApiCompatibilityLevel = PlayerApiCompatibilityLevel.NET_4_6,
                ActionOnDotNetUnhandledException = PlayerActionOnDotNetUnhandledException.Crash,
                LogObjCUncaughtExceptions = true,
                EnableCrashReportAPI = true,
                StrippingLevel = PlayerStrippingLevel.UseMicroMSCorlib,
                StripEngineCode = true,
                UsePlayerLog = true,
                StackTraceLogType = new System.Collections.Generic.Dictionary<PlayerLogType, PlayerStackTraceLogType>()
                {
                    { PlayerLogType.Exception, PlayerStackTraceLogType.Full },
                    { PlayerLogType.Assert, PlayerStackTraceLogType.Full },
                    { PlayerLogType.Error, PlayerStackTraceLogType.Full },
                    { PlayerLogType.Warning, PlayerStackTraceLogType.ScriptOnly },
                    { PlayerLogType.Log, PlayerStackTraceLogType.None }
                },
                Il2CppCompilerConfiguration = new System.Collections.Generic.Dictionary<PlayerBuildTargetGroup, PlayerIl2CppCompilerConfiguration>()
                {
                    { PlayerBuildTargetGroup.Standalone, PlayerIl2CppCompilerConfiguration.Debug },
                    { PlayerBuildTargetGroup.iOS, PlayerIl2CppCompilerConfiguration.Release },
                    { PlayerBuildTargetGroup.Android, PlayerIl2CppCompilerConfiguration.Release }
                },
                IncrementalIl2CppBuild = new System.Collections.Generic.Dictionary<PlayerBuildTargetGroup, bool>()
                {
                    { PlayerBuildTargetGroup.Standalone, true },
                    { PlayerBuildTargetGroup.iOS, false },
                    { PlayerBuildTargetGroup.Android, false }
                },
                ScriptingBackend = new System.Collections.Generic.Dictionary<PlayerBuildTargetGroup, PlayerScriptingImplementation>()
                {
                    { PlayerBuildTargetGroup.Standalone, PlayerScriptingImplementation.IL2CPP },
                    { PlayerBuildTargetGroup.iOS, PlayerScriptingImplementation.IL2CPP },
                    { PlayerBuildTargetGroup.Android, PlayerScriptingImplementation.Mono2x }
                },
                ScriptingDefineSymbolsForGroup = new System.Collections.Generic.Dictionary<PlayerBuildTargetGroup, string>()
                {
                    { PlayerBuildTargetGroup.Standalone, "CUSTOM_STANDALONE" },
                    { PlayerBuildTargetGroup.iOS, "CUSTOM_IOS" },
                    { PlayerBuildTargetGroup.Android, "CUSTOM_ANDROID" }
                },

                // Window Settings
                ResizableWindow = false,
                DefaultIsFullScreen = true,
                DefaultIsNativeResolution = true,
                DefaultScreenWidth = 1920,
                DefaultScreenHeight = 1080,
                DefaultWebScreenWidth = 1208,
                DefaultWebScreenHeight = 720,
                CaptureSingleScreen = true,
                RunInBackground = false,
                AspectRatio = new System.Collections.Generic.Dictionary<PlayerAspectRatio, bool>()
                {
                    { PlayerAspectRatio.AspectOthers, false },
                    { PlayerAspectRatio.Aspect4by3, true },
                    { PlayerAspectRatio.Aspect5by4, true },
                    { PlayerAspectRatio.Aspect16by10, true },
                    { PlayerAspectRatio.Aspect16by9, true }
                },

                // Spalsh / Startup Settings
                PreloadedAssets = new string[]
                {
                    "Assets/PreLoad0.txt",
                    "Assets/PreLoad1.txt"
                },

                DisplayResolutionDialog = PlayerResolutionDialogSetting.Disabled,
                VirtualRealitySplashScreen = "Assets/Splash/VR.png",
                ShowUnitySplashScreen = false,
                SplashScreenStyle = PlayerSplashScreenStyle.Dark,
                IconsForTargetGroup = new System.Collections.Generic.Dictionary<PlayerBuildTargetGroup, string[]>()
                {
                    { PlayerBuildTargetGroup.Standalone, new string [] {"Assets/Icons/Standalone/Icon.png",
                        "Assets/Icons/Standalone/Icon.png",
                        "Assets/Icons/Standalone/Icon.png",
                        "Assets/Icons/Standalone/Icon.png",
                        "Assets/Icons/Standalone/Icon.png",
                        "Assets/Icons/Standalone/Icon.png",
                        "Assets/Icons/Standalone/Icon.png"} },
                    { PlayerBuildTargetGroup.iOS, new string [] { "Assets/Icons/iOS/Icon1.png", "Assets/Icons/iOS/Icon2.png" } },
                    { PlayerBuildTargetGroup.Android, new string [] {"Assets/Icons/Android/Icon.png" } }
                },

                SplashScreenAnimationMode = SplashAnimationMode.Dolly,
                SplashScreenAnimationBackgroundZoom = 1,
                SplashScreenAnimationLogoZoom = 1,
                SplashScreenBackground = "Assets/Splash/Backgrounbd.png",
                SplashScreenBackgroundPortrait = "Assets/Splash/Portrait.png",
                SplashScreenBackgroundColor = "#99ccff",
                SplashScreenDrawMode = SplashDrawMode.AllSequential,
                SplashScreenLogos = new PlayerSplashScreenLogo[]
                {
                    new PlayerSplashScreenLogo() {Logo = "Assets/Splash/Logo1.png", Duration = 0.5f },
                    new PlayerSplashScreenLogo() {Logo = "Assets/Splash/Logo2.png", Duration = 0.5f }
                },
                SplashScreenOverlayOpacity = 0.75f,
                SplashScreenShow = true,
                SplashScreenShowUnityLogo = false,
                SplashScreenUnityLogoStyle = SplashUnityLogoStyle.LightOnDark,

                // Graphics
                Use32BitDisplayBuffer = true,
                ColorSpace = PlayerColorSpace.Linear,
                GraphicsAPIs = new System.Collections.Generic.Dictionary<PlayerBuildTarget, PlayerGraphicsDeviceType>()
                {
                    { PlayerBuildTarget.StandaloneWindows, PlayerGraphicsDeviceType.UseDefault |
                        PlayerGraphicsDeviceType.Direct3D9 |
                        PlayerGraphicsDeviceType.Direct3D11 |
                        PlayerGraphicsDeviceType.Direct3D12 |
                        PlayerGraphicsDeviceType.Vulkan},
                    { PlayerBuildTarget.StandaloneWindows64, PlayerGraphicsDeviceType.UseDefault |
                        PlayerGraphicsDeviceType.Direct3D9 |
                        PlayerGraphicsDeviceType.Direct3D11 |
                        PlayerGraphicsDeviceType.Direct3D12 |
                        PlayerGraphicsDeviceType.Vulkan},
                    { PlayerBuildTarget.iOS, PlayerGraphicsDeviceType.DoNotUseDefault | PlayerGraphicsDeviceType.Metal },
                    { PlayerBuildTarget.Android, PlayerGraphicsDeviceType.UseDefault |
                        PlayerGraphicsDeviceType.OpenGLES3 |
                        PlayerGraphicsDeviceType.Vulkan}
                },

                // Perofrmance
                StripUnusedMeshComponents = false,
                BakeCollisionMeshes = false,

                // Mobile
                DefaultInterfaceOrientation = PlayerUIOrientation.LandscapeRight,
                AllowedAutorotateToPortrait = true,
                AllowedAutorotateToPortraitUpsideDown = true,
                AllowedAutorotateToLandscapeRight = false,
                AllowedAutorotateToLandscapeLeft = false,
                UseAnimatedAutorotation = true,
                StatusBarHidden = true,

                // macOS
                UseMacAppStoreValidation = false,
                MacRetinaSupport = true,
                MacOSBuildNumber = "0.1-mac",

                // iOS
                iOSApplicationDisplayName = "App on iOS",
                iOSBuildNumber = "0.1-ios",
                iOSTargetDevice = PlayeriOSTargetDevice.iPhoneAndiPad,
                iOSTargetOSVersionString = "7",
                iOSSdkVersion = PlayeriOSSdkVersion.DeviceSDK,
                iOSAppleDeveloperTeamID = "Wolpertinger Games UG (haftungsbeschraenkt)",

                iOSTvOSManualProvisioningProfileID = "Some Manual Provision",
                iOSManualProvisioningProfileID = "Another Manual Provision",

                iOSMicrophoneUsageDescription = "Microphone Not Used",
                iOSLocationUsageDescription = "Location Not Used",
                iOSCameraUsageDescription = "Camera Not Used",

                iOSAllowHTTPDownload = false,
                iOSForceHardShadowsOnMetal = false,
                iOSExitOnSuspend = false,
                iOSUseOnDemandResources = false,
                iOSAppleEnableAutomaticSigning = true,
                iOSRequiresFullScreen = true,
                iOSRequiresPersistentWiFi = false,
                iOSPrerenderedIcon = true,
                iOSOverrideIPodMusic = false,

                iOSShowActivityIndicatorOnLoading = PlayeriOSShowActivityIndicatorOnLoading.Gray,
                iOSBackgroundModes = PlayeriOSBackgroundMode.None,
                iOSTargetResolution = PlayeriOSTargetResolution.ResolutionAutoPerformance,
                iOSAppInBackgroundBehavior = PlayeriOSAppInBackgroundBehavior.Suspend,
                iOSScriptCallOptimization = PlayerScriptCallOptimizationLevel.FastButNoExceptions,
                iOSStatusBarStyle = PlayeriOSStatusBarStyle.BlackTranslucent,

                iOSiPadLaunchScreenType = PlayeriOSLaunchScreenType.Default,
                iOSiPhoneLaunchScreenType = PlayeriOSLaunchScreenType.Default,
                LaunchScreenImage = new System.Collections.Generic.Dictionary<PlayeriOSLaunchScreenImageType, string>()
                {
                    { PlayeriOSLaunchScreenImageType.iPhonePortraitImage, "Assets/Splash/iPhonePort.png" },
                    { PlayeriOSLaunchScreenImageType.iPhoneLandscapeImage, "Assets/Splash/iPhoneLand.png" },
                    { PlayeriOSLaunchScreenImageType.iPadImage, "Assets/Splash/iPad.png" },
                },

                // Android
                AndroidBundleVersionCode = 1,
                AndroidTargetSdkVersion = 28,
                AndroidMinSdkVersion = 16,
                AndroidTargetDevice = PlayerAndroidTargetDevice.FAT,

                AndroidKeystoreName = "Asset/keystore.keys",
                AndroidKeystorePass = "123456",
                AndroidKeyaliasName = "MainApp",
                AndroidKeyaliasPass = "123456",

                AndroidUseAPKExpansionFiles = true,
                AndroidUse24BitDepthBuffer = true,
                AndroidDisableDepthAndStencilBuffers = false,
                AndroidIsGame = true,
                AndroidTVCompatibility = false,
                AndroidForceSDCardPermission = true,
                AndroidForceInternetPermission = false,

                AndroidMaxAspectRatio = 1.8f,

                AndroidPreferredInstallLocation = PlayerAndroidPreferredInstallLocation.Auto,
                AndroidShowActivityIndicatorOnLoading = PlayerAndroidShowActivityIndicatorOnLoading.Large,
                AndroidSplashScreenScale = PlayerAndroidSplashScreenScale.ScaleToFit,
                AndroidBlitType = PlayerAndroidBlitType.Auto
            };

            return settings;
        }
    }
}
