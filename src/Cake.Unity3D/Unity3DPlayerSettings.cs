using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Unity3D.Helpers;

namespace Cake.Unity3D
{
    // TODO : Handle Splash Image
    // TODO : Handle Icons
    public class Unity3DPlayerSettings
    {
        Dictionary<string, object> values = new Dictionary<string, object>();

        public T GetValue<T>(string key)
        {
            object value;
            if(values.TryGetValue(key, out value))
            {
                return (T)value;
            }
            return default(T);
        }

        public void SetValue<T>(string key, T value)
        {
            if (values.ContainsKey(key))
            {
                values[key] = value.ToString();
            }
            else
            {
                values.Add(key, value.ToString());
            }
        }

        const string DumpPrefix = "--";
        public void FillIntoArgs(Dictionary<string,string> args)
        {
            foreach(KeyValuePair<string, object> setting in values)
            {
                string argsKey = $"{DumpPrefix}{setting.Key}";
                args.Add(argsKey, EncodeValue(setting.Value));
            }

            FillIntoArgs(StackTraceLogType, args);
            FillIntoArgs(AspectRatio, args);
            FillIntoArgs(GraphicsAPIs, args);
        }

        void FillIntoArgs<K,V>(IEnumerable<KeyValuePair<K,V>> dic, Dictionary<string, string> args)
        {
            if (dic != null)
            {
                foreach (var keyValue in GraphicsAPIs)
                {
                    string argsKey = $"{DumpPrefix}GraphicsAPIs:{keyValue.Key}";
                    args.Add(argsKey, EncodeValue(keyValue.Value));
                }
            }
        }

        string EncodeValue(object value)
        {
            Type t = value.GetType();
            if(t.IsEnum)
            {
                Enum enumValue = (Enum)value;
                if(enumValue.HasFlag(enumValue))
                {
                    return string.Join(",", enumValue.GetIndividualFlags());
                }
            }
            return value.ToString();
        }

        public void DumpOptions()
        {
            foreach (KeyValuePair<string, object> setting in values)
            {
                Console.WriteLine($"{setting.Key}: {EncodeValue(setting.Value)}");
            }

            DumpOptions(StackTraceLogType, "StackTraceLogType");
            DumpOptions(AspectRatio, "AspectRatio");
            DumpOptions(GraphicsAPIs, "GraphicsAPIs");
        }

        void DumpOptions<K, V>(IEnumerable<KeyValuePair<K, V>> dic, string Title)
        {
            if (dic != null)
            {
                Console.WriteLine($"{Title}->");
                foreach (var keyValue in dic)
                {
                    Console.WriteLine($"\t{keyValue.Key}: {EncodeValue(keyValue.Value)}");
                }
            }
        }

        // General Settings
        public string CompanyName { get { return GetValue<string>("companyName"); } set { SetValue("companyName", value); } }
        public string ProductName { get { return GetValue<string>("productName"); } set { SetValue("productName", value); } }
        public string ApplicationIdentifier { get { return GetValue<string>("applicationIdentifier"); } set { SetValue("applicationIdentifier", value); } }
        public string BundleVersion { get { return GetValue<string>("bundleVersion"); } set { SetValue("bundleVersion", value); } }

        // Unity Services Settings
        public string CloudProjectId { get { return GetValue<string>("cloudProjectId"); } set { SetValue("cloudProjectId", value); } }
        public Guid ProductGUID { get { return GetValue<Guid>("productGUID"); } set { SetValue("productGUID", value); } }

        // DotNet / Code / Script Settings
        public PlayerApiCompatibilityLevel ApiCompatibilityLevel { get { return GetValue<PlayerApiCompatibilityLevel>("apiCompatibilityLevel"); } set { SetValue("apiCompatibilityLevel", value); } }
        public bool ActionOnDotNetUnhandledException { get { return GetValue<bool>("actionOnDotNetUnhandledException"); } set { SetValue("actionOnDotNetUnhandledException", value); } }
        public bool LogObjCUncaughtExceptions { get { return GetValue<bool>("logObjCUncaughtExceptions"); } set { SetValue("logObjCUncaughtExceptions", value); } }
        public bool EnableCrashReportAPI { get { return GetValue<bool>("enableCrashReportAPI"); } set { SetValue("enableCrashReportAPI", value); } }
        public PlayerStrippingLevel StrippingLevel { get { return GetValue<PlayerStrippingLevel>("strippingLevel"); } set { SetValue("strippingLevel", value); } }
        public bool StripEngineCode { get { return GetValue<bool>("stripEngineCode"); } set { SetValue("stripEngineCode", value); } }
        public bool UsePlayerLog { get { return GetValue<bool>("usePlayerLog"); } set { SetValue("usePlayerLog", value); } }
        public string AdditionalIl2CppArgs { get { return GetValue<string>("additionalIl2CppArgs"); } set { SetValue("additionalIl2CppArgs", value); } }
        public IEnumerable<KeyValuePair<PlayerLogType, PlayerStackTraceLogType>> StackTraceLogType { get; set; }

        // Window Settings
        public bool ResizableWindow { get { return GetValue<bool>("resizableWindow"); } set { SetValue("resizableWindow", value); } }
        public bool DefaultIsFullScreen { get { return GetValue<bool>("defaultIsFullScreen"); } set { SetValue("defaultIsFullScreen", value); } }
        public bool DefaultIsNativeResolution { get { return GetValue<bool>("defaultIsNativeResolution"); } set { SetValue("defaultIsNativeResolution", value); } }
        public int DefaultScreenWidth { get { return GetValue<int>("defaultScreenWidth"); } set { SetValue("defaultScreenWidth", value); } }
        public int DefaultScreenHeight { get { return GetValue<int>("defaultScreenHeight"); } set { SetValue("defaultScreenHeight", value); } }
        public int DefaultWebScreenWidth { get { return GetValue<int>("defaultWebScreenWidth"); } set { SetValue("defaultWebScreenWidth", value); } }
        public int DefaultWebScreenHeight { get { return GetValue<int>("defaultWebScreenHeight"); } set { SetValue("defaultWebScreenHeight", value); } }
        public bool CaptureSingleScreen { get { return GetValue<bool>("captureSingleScreen"); } set { SetValue("captureSingleScreen", value); } }
        public bool RunInBackground { get { return GetValue<bool>("runInBackground"); } set { SetValue("runInBackground", value); } }
        public IEnumerable<KeyValuePair<PlayerAspectRatio, bool>> AspectRatio { get; set; }

        // Spalsh / Startup Settings
        public bool DisplayResolutionDialog { get { return GetValue<bool>("displayResolutionDialog"); } set { SetValue("displayResolutionDialog", value); } }
        public string VirtualRealitySplashScreen { get { return GetValue<string>("virtualRealitySplashScreen"); } set { SetValue("virtualRealitySplashScreen", value); } }
        public bool ShowUnitySplashScreen { get { return GetValue<bool>("showUnitySplashScreen"); } set { SetValue("showUnitySplashScreen", value); } }
        public PlayerSplashScreenStyle SplashScreenStyle { get { return GetValue<PlayerSplashScreenStyle>("splashScreenStyle"); } set { SetValue("splashScreenStyle", value); } }

        // Graphics
        public bool Use32BitDisplayBuffer { get { return GetValue<bool>("use32BitDisplayBuffer"); } set { SetValue("use32BitDisplayBuffer", value); } }
        public PlayerColorSpace ColorSpace { get { return GetValue<PlayerColorSpace>("colorSpace"); } set { SetValue("colorSpace", value); } }
        public IEnumerable<KeyValuePair<PlayerBuildTarget, PlayerGraphicsDeviceType>> GraphicsAPIs { get; set; }

        // Perofrmance
        public bool StripUnusedMeshComponents { get { return GetValue<bool>("stripUnusedMeshComponents"); } set { SetValue("stripUnusedMeshComponents", value); } }
        public bool BakeCollisionMeshes { get { return GetValue<bool>("bakeCollisionMeshes"); } set { SetValue("bakeCollisionMeshes", value); } }

        // Mobile
        public UIOrientation DefaultInterfaceOrientation { get { return GetValue<UIOrientation>("defaultInterfaceOrientation"); } set { SetValue("defaultInterfaceOrientation", value); } }
        public bool AllowedAutorotateToPortrait { get { return GetValue<bool>("allowedAutorotateToPortrait"); } set { SetValue("allowedAutorotateToPortrait", value); } }
        public bool AllowedAutorotateToPortraitUpsideDown { get { return GetValue<bool>("allowedAutorotateToPortraitUpsideDown"); } set { SetValue("allowedAutorotateToPortraitUpsideDown", value); } }
        public bool AllowedAutorotateToLandscapeRight { get { return GetValue<bool>("allowedAutorotateToLandscapeRight"); } set { SetValue("allowedAutorotateToLandscapeRight", value); } }
        public bool AllowedAutorotateToLandscapeLeft { get { return GetValue<bool>("allowedAutorotateToLandscapeLeft"); } set { SetValue("allowedAutorotateToLandscapeLeft", value); } }
        public bool UseAnimatedAutorotation { get { return GetValue<bool>("useAnimatedAutorotation"); } set { SetValue("useAnimatedAutorotation", value); } }
        public bool StatusBarHidden { get { return GetValue<bool>("statusBarHidden"); } set { SetValue("statusBarHidden", value); } }

        // macOS
        public bool UseMacAppStoreValidation { get { return GetValue<bool>("useMacAppStoreValidation"); } set { SetValue("useMacAppStoreValidation", value); } }
        public bool MacRetinaSupport { get { return GetValue<bool>("macRetinaSupport"); } set { SetValue("macRetinaSupport", value); } }
        public bool MacOSBuildNumber { get { return GetValue<bool>("mac.OSBuildNumber"); } set { SetValue("mac.OSBuildNumber", value); } }

        // iOS
        public string iOSApplicationDisplayName { get { return GetValue<string>("iOS.applicationDisplayName"); } set { SetValue("iOS.applicationDisplayName", value); } }
        public string iOSBuildNumber { get { return GetValue<string>("iOS.buildNumber"); } set { SetValue("iOS.buildNumber", value); } }
        public PlayeriOSTargetDevice iOSTargetDevice { get { return GetValue<PlayeriOSTargetDevice>("iOS.targetDevice"); } set { SetValue("iOS.targetDevice", value); } }
        public string iOSTargetOSVersionString { get { return GetValue<string>("iOS.targetOSVersionString"); } set { SetValue("iOS.targetOSVersionString", value); } }
        public PlayeriOSSdkVersion iOSSdkVersion { get { return GetValue<PlayeriOSSdkVersion>("iOS.sdkVersion"); } set { SetValue("iOS.sdkVersion", value); } }
        public string iOSAppleDeveloperTeamID { get { return GetValue<string>("iOS.appleDeveloperTeamID"); } set { SetValue("iOS.appleDeveloperTeamID", value); } }

        public string iOSTvOSManualProvisioningProfileID { get { return GetValue<string>("iOS.tvOSManualProvisioningProfileID"); } set { SetValue("iOS.tvOSManualProvisioningProfileID", value); } }
        public string iOSManualProvisioningProfileID { get { return GetValue<string>("iOS.manualProvisioningProfileID"); } set { SetValue("iOS.manualProvisioningProfileID", value); } }

        public string iOSMicrophoneUsageDescription { get { return GetValue<string>("iOS.microphoneUsageDescription"); } set { SetValue("iOS.microphoneUsageDescription", value); } }
        public string iOSLocationUsageDescription { get { return GetValue<string>("iOS.locationUsageDescription"); } set { SetValue("iOS.locationUsageDescription", value); } }
        public string iOSCameraUsageDescription { get { return GetValue<string>("iOS.cameraUsageDescription"); } set { SetValue("iOS.cameraUsageDescription", value); } }

        public bool iOSAllowHTTPDownload { get { return GetValue<bool>("iOS.allowHTTPDownload"); } set { SetValue("iOS.allowHTTPDownload", value); } }
        public bool iOSForceHardShadowsOnMetal { get { return GetValue<bool>("iOS.forceHardShadowsOnMetal"); } set { SetValue("iOS.forceHardShadowsOnMetal", value); } }
        public bool iOSExitOnSuspend { get { return GetValue<bool>("iOS.exitOnSuspend"); } set { SetValue("iOS.exitOnSuspend", value); } }
        public bool iOSUseOnDemandResources { get { return GetValue<bool>("iOS.useOnDemandResources"); } set { SetValue("iOS.useOnDemandResources", value); } }
        public bool iOSAppleEnableAutomaticSigning { get { return GetValue<bool>("iOS.appleEnableAutomaticSigning"); } set { SetValue("iOS.appleEnableAutomaticSigning", value); } }
        public bool iOSRequiresFullScreen { get { return GetValue<bool>("iOS.requiresFullScreen"); } set { SetValue("iOS.requiresFullScreen", value); } }
        public bool iOSRequiresPersistentWiFi { get { return GetValue<bool>("iOS.requiresPersistentWiFi"); } set { SetValue("iOS.requiresPersistentWiFi", value); } }
        public bool iOSPrerenderedIcon { get { return GetValue<bool>("iOS.prerenderedIcon"); } set { SetValue("iOS.prerenderedIcon", value); } }
        public bool iOSOverrideIPodMusic { get { return GetValue<bool>("iOS.OverrideIPodMusic"); } set { SetValue("iOS.OverrideIPodMusic", value); } }

        public PlayeriOSShowActivityIndicatorOnLoading iOSShowActivityIndicatorOnLoading { get { return GetValue<PlayeriOSShowActivityIndicatorOnLoading>("iOS.showActivityIndicatorOnLoading"); } set { SetValue("iOS.showActivityIndicatorOnLoading", value); } }
        public PlayeriOSBackgroundMode iOSBackgroundModes { get { return GetValue<PlayeriOSBackgroundMode>("iOS.backgroundModes"); } set { SetValue("iOS.backgroundModes", value); } }
        public PlayeriOSTargetResolution iOSTargetResolution { get { return GetValue<PlayeriOSTargetResolution>("iOS.targetResolution"); } set { SetValue("iOS.targetResolution", value); } }
        public PlayeriOSAppInBackgroundBehavior iOSAppInBackgroundBehavior { get { return GetValue<PlayeriOSAppInBackgroundBehavior>("iOS.appInBackgroundBehavior"); } set { SetValue("iOS.appInBackgroundBehavior", value); } }
        public PlayerScriptCallOptimizationLevel iOSScriptCallOptimization { get { return GetValue<PlayerScriptCallOptimizationLevel>("iOS.scriptCallOptimization"); } set { SetValue("iOS.scriptCallOptimization", value); } }
        public PlayeriOSStatusBarStyle iOSStatusBarStyle { get { return GetValue<PlayeriOSStatusBarStyle>("iOS.statusBarStyle"); } set { SetValue("iOS.statusBarStyle", value); } }

        public PlayeriOSLaunchScreenType iOSiPadLaunchScreenType { get { return GetValue<PlayeriOSLaunchScreenType>("iOS.iPadLaunchScreenType"); } set { SetValue("iOS.iPadLaunchScreenType", value); } }
        public PlayeriOSLaunchScreenType iOSiPhoneLaunchScreenType { get { return GetValue<PlayeriOSLaunchScreenType>("iOS.iPhoneLaunchScreenType"); } set { SetValue("iOS.iPhoneLaunchScreenType", value); } }
        // TODO 
        //public static void SetLaunchScreenImage(Texture2D image, iOSLaunchScreenImageType type);

        // Android
        public int AndroidBundleVersionCode { get { return GetValue<int>("android.bundleVersionCode"); } set { SetValue("android.bundleVersionCode", value); } }
        public int AndroidTargetSdkVersion { get { return GetValue<int>("android.targetSdkVersion"); } set { SetValue("android.targetSdkVersion", value); } }
        public int AndroidMinSdkVersion { get { return GetValue<int>("android.minSdkVersion"); } set { SetValue("android.minSdkVersion", value); } }
        public PlayerAndroidTargetDevice AndroidTargetDevice { get { return GetValue<PlayerAndroidTargetDevice>("android.targetDevice"); } set { SetValue("android.targetDevice", value); } }

        public string AndroidKeystoreName { get { return GetValue<string>("android.keystoreName"); } set { SetValue("android.keystoreName", value); } }
        public string AndroidKeystorePass { get { return GetValue<string>("android.keystorePass"); } set { SetValue("android.keystorePass", value); } }
        public string AndroidKeyaliasName { get { return GetValue<string>("android.keyaliasName"); } set { SetValue("android.keyaliasName", value); } }
        public string AndroidKeyaliasPass { get { return GetValue<string>("android.keyaliasPass"); } set { SetValue("android.keyaliasPass", value); } }

        public bool AndroidUseAPKExpansionFiles { get { return GetValue<bool>("android.useAPKExpansionFiles"); } set { SetValue("android.useAPKExpansionFiles", value); } }
        public bool AndroidLicenseVerification { get { return GetValue<bool>("android.licenseVerification"); } set { SetValue("android.licenseVerification", value); } }
        public bool AndroidUse24BitDepthBuffer { get { return GetValue<bool>("android.use24BitDepthBuffer"); } set { SetValue("android.use24BitDepthBuffer", value); } }
        public bool AndroidDisableDepthAndStencilBuffers { get { return GetValue<bool>("android.disableDepthAndStencilBuffers"); } set { SetValue("android.disableDepthAndStencilBuffers", value); } }
        public bool AndroidIsGame { get { return GetValue<bool>("android.androidIsGame"); } set { SetValue("android.androidIsGame", value); } }
        public bool AndroidTVCompatibility { get { return GetValue<bool>("android.androidTVCompatibility"); } set { SetValue("android.androidTVCompatibility", value); } }
        public bool AndroidForceSDCardPermission { get { return GetValue<bool>("android.forceSDCardPermission"); } set { SetValue("android.forceSDCardPermission", value); } }
        public bool AndroidForceInternetPermission { get { return GetValue<bool>("android.forceInternetPermission"); } set { SetValue("android.forceInternetPermission", value); } }

        public float AndroidMaxAspectRatio { get { return GetValue<float>("android.maxAspectRatio"); } set { SetValue("android.maxAspectRatio", value); } }

        public PlayerAndroidPreferredInstallLocation AndroidPreferredInstallLocation { get { return GetValue<PlayerAndroidPreferredInstallLocation>("android.preferredInstallLocation"); } set { SetValue("android.preferredInstallLocation", value); } }
        public PlayerAndroidShowActivityIndicatorOnLoading AndroidShowActivityIndicatorOnLoading { get { return GetValue<PlayerAndroidShowActivityIndicatorOnLoading>("android.showActivityIndicatorOnLoading"); } set { SetValue("android.showActivityIndicatorOnLoading", value); } }
        public PlayerAndroidSplashScreenScale AndroidSplashScreenScale { get { return GetValue<PlayerAndroidSplashScreenScale>("android.splashScreenScale"); } set { SetValue("android.splashScreenScale", value); } }
        public PlayerAndroidBlitType AndroidBlitType { get { return GetValue<PlayerAndroidBlitType>("android.blitType"); } set { SetValue("android.blitType", value); } }
    }

    public enum PlayerApiCompatibilityLevel
    {
        NET_2_0,
        NET_2_0_Subset,
        NET_4_6,
        NET_Web,
        NET_Micro
    }

    public enum PlayerStrippingLevel
    {
        Disabled,
        StripAssemblies,
        StripByteCode,
        UseMicroMSCorlib
    }

    public enum PlayerSplashScreenStyle
    {
        Light,
        Dark
    }

    public enum PlayerColorSpace
    {
        Uninitialized,
        Gamma,
        Linear
    }

    public enum UIOrientation
    {
        Portrait,
        PortraitUpsideDown,
        LandscapeRight,
        LandscapeLeft,
        AutoRotation
    }

    public enum PlayeriOSTargetDevice
    {
        iPhoneOnly,
        iPadOnly,
        iPhoneAndiPad
    }

    public enum PlayeriOSSdkVersion
    {
        DeviceSDK,
        SimulatorSDK
    }

    public enum PlayeriOSShowActivityIndicatorOnLoading
    {
        DontShow,
        WhiteLarge,
        White,
        Gray
    }

    [System.Flags]
    public enum PlayeriOSBackgroundMode : uint
    {
        None = 0,
        Audio = 1,
        Location = 2,
        VOIP = 4,
        NewsstandContent = 8,
        ExternalAccessory = 16,
        BluetoothCentral = 32,
        BluetoothPeripheral = 64,
        Fetch = 128,
        RemoteNotification = 256
    }

    public enum PlayeriOSTargetResolution
    {
        Native,
        ResolutionAutoPerformance,
        ResolutionAutoQuality,
        Resolution320p,
        Resolution640p,
        Resolution768p
    }

    public enum PlayeriOSAppInBackgroundBehavior
    {
        Custom,
        Suspend,
        Exit
    }

    public enum PlayerScriptCallOptimizationLevel
    {
        SlowAndSafe,
        FastButNoExceptions
    }

    public enum PlayeriOSStatusBarStyle
    {
        BlackTranslucent,
        BlackOpaque,
        Default,
        LightContent
    }

    public enum PlayeriOSLaunchScreenType
    {
        Default,
        ImageAndBackgroundRelative,
        CustomXib,
        None,
        ImageAndBackgroundConstant
    }

    public enum PlayerAndroidTargetDevice
    {
        FAT,
        ARMv7,
        x86
    }

    public enum PlayerAndroidPreferredInstallLocation
    {
        Auto,
        PreferExternal,
        ForceInternal
    }

    public enum PlayerAndroidShowActivityIndicatorOnLoading
    {
        DontShow,
        Large,
        InversedLarge,
        Small,
        InversedSmall
    }

    public enum PlayerAndroidSplashScreenScale
    {
        Center,
        ScaleToFit,
        ScaleToFill
    }

    public enum PlayerAndroidBlitType
    {
        Always,
        Never,
        Auto
    }

    public enum PlayerStackTraceLogType
    {
        None = 0,
        ScriptOnly = 1,
        Full = 2
    }

    [System.Flags]
    public enum PlayerGraphicsDeviceType
    {
        None = 0,
        UseDefault = 1,
        DoNotUseDefault = 524288,
        OpenGL2 = 2,
        OpenGLES2 = 4,
        OpenGLES3 = 8,
        OpenGLCore = 16,
        Vulkan = 32,
        Direct3D9 = 64,
        Direct3D11 = 128,
        Direct3D12 = 256,        
        Metal = 512,
        Switch = 1024,
        N3DS = 2048,
        PlayStationVita = 4096,
        PlayStationMobile = 8192,
        PlayStation3 = 16384,
        PlayStation4 = 32768,
        Xbox360 = 65536,
        XboxOne = 131072,
        XboxOneD3D12 = 262144
    }

    public enum PlayerBuildTarget
    {
        NoTarget,
        iPhone,
        BB10,
        MetroPlayer,
        StandaloneOSX,
        StandaloneOSXUniversal,
        StandaloneOSXIntel,
        StandaloneWindows,
        WebPlayer,
        WebPlayerStreamed,
        iOS,
        PS3,
        XBOX360,
        Android,
        StandaloneLinux,
        StandaloneWindows64,
        WebGL,
        WSAPlayer,
        StandaloneLinux64,
        StandaloneLinuxUniversal,
        WP8Player,
        StandaloneOSXIntel64,
        BlackBerry,
        Tizen,
        PSP2,
        PS4,
        PSM,
        XboxOne,
        SamsungTV,
        N3DS,
        WiiU,
        tvOS,
        Switch
    }

    public enum PlayerLogType
    {
        Error,
        Assert,
        Warning,
        Log,
        Exception
    }

    public enum PlayerAspectRatio
    {
        AspectOthers,
        Aspect4by3,
        Aspect5by4,
        Aspect16by10,
        Aspect16by9
    }
}
