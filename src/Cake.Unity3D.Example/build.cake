#reference "../Cake.Unity3D/bin/Debug/net46/Cake.Unity3D.dll"
//#reference "../Cake.Unity3D/bin/Release/net46/Cake.Unity3D.dll"
//#addin nuget:?package=Cake.Unity3D

var target = Argument("target", "Build-Win64");

Task("Build-Win64")
  .Does(() =>
{
	var projectPath = System.IO.Path.GetFullPath("./");
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "x64", "example.exe");
	var unityEditorLocation = @"C:\Program Files\Unity\Hub\Editor\2018.1.0f1\Editor\Unity.exe";
	
	var options = new Unity3DBuildOptions()
	{
		Platform = Unity3DBuildPlatform.StandaloneWindows64,
		OutputPath = outputPath,
		UnityEditorLocation = unityEditorLocation,
		ForceScriptInstall = true
	};
	
	BuildUnity3DProject(projectPath, options);
});

Task("Build-WebGL")
  .Does(() =>
{
	var projectPath = System.IO.Path.GetFullPath("./");
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "webgl");
	var unityEditorLocation = @"C:\Program Files\Unity\Hub\Editor\2018.1.0f1\Editor\Unity.exe";
	
	var options = new Unity3DBuildOptions()
	{
		Platform = Unity3DBuildPlatform.WebGL,
		OutputPath = outputPath,
		UnityEditorLocation = unityEditorLocation,
		ForceScriptInstall = true
	};
	
	BuildUnity3DProject(projectPath, options);
});

RunTarget(target);