# Cake.Unity3D

## About

Unity3D build support for Cake (https://github.com/cake-build/cake).

## Example

```csharp
#addin nuget:?package=Cake.Unity3D

var target = Argument("target", "Build");

Task("Build")
  .Does(() =>
{
	// Presuming the build.cake file is within the Unity3D project folder.
	var projectPath = System.IO.Path.GetFullPath("./");
	
	// The location we want the build application to go
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "x64", "example.exe");
	
	// Get the absolute path to the 2018.1.0f1 Unity3D editor.
	string unityEditorLocation;
	if (!TryGetUnityInstall("Unity 2018.1.0f1 (64-bit)", out unityEditorLocation)) 
	{
		Error("Failed to find 'Unity 2018.1.0f1 (64-bit)' install location");
		return;
	}
	
	// Create our build options.
	var options = new Unity3DBuildOptions()
	{
		Platform = Unity3DBuildPlatform.StandaloneWindows64,
		OutputPath = outputPath,
		UnityEditorLocation = unityEditorLocation,
		ForceScriptInstall = true,
		BuildVersion = "1.0.0"
	};
	
	// Perform the Unity3d build.
	BuildUnity3DProject(projectPath, options);
});

RunTarget(target);
```

## Supported Platforms

### Executing Platforms

Currently the addon only works on Windows and has only been tested on Windows 10.
This is due to how the 'TryGetUnityInstall' works. What it does is look at all shortcuts in your Windows start menu and locate
the Unity3D installs from that. In theory, if you just set the 'UnityEditorLocation' build option to the absolute path to
your Unity3D install on Mac or Linux, this should also work.

### Build Platforms

* StandaloneWindows64
* StandaloneWindows
* WebGL

In theory, all platforms that the executing platform supports should work. However, I haven't test any apart from the above list.
