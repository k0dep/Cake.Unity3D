using Cake.Common.Diagnostics;
using Cake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace Cake.Unity3D.Helpers
{
    /// <summary>
    /// Some helper methods for interacting with Unity3D.
    /// </summary>
    public static class Unity3DEditor
    {
        /// <summary>
        /// Locate all installed version of Unity3D.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LocateUnityInstalls()
        {
            if(Environment.OSVersion.Platform == PlatformID.MacOSX ||
                Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return LocateUnityInstallsMacOS();
            }
            else
            {
                return LocateUnityInstallsWindows();
            }
        }

        /// <summary>
        /// Locate all installed version of Unity3D on Windows.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LocateUnityInstallsWindows()
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (!System.IO.Directory.Exists(programData))
            {
                throw new Exception($"Failed to find any installed Unity3d versions. 'ProgramData' folder '{programData}' does not exist.");
            }

            var startMenuProgramsDirectory = System.IO.Path.Combine(programData, "Microsoft", "Windows", "Start Menu", "Programs");
            if (!System.IO.Directory.Exists(startMenuProgramsDirectory))
            {
                throw new Exception($"Failed to find any installed Unity3d versions. Start menu programs folder '{startMenuProgramsDirectory}' does not exist.");
            }

            var installs = new Dictionary<string, string>();
            foreach (var unityFolder in System.IO.Directory.EnumerateDirectories(startMenuProgramsDirectory, "Unity*"))
            {
                var unityShortcut = System.IO.Path.Combine(unityFolder, "Unity.lnk");
                if (!System.IO.File.Exists(unityShortcut))
                {
                    continue;
                }

                installs.Add(new System.IO.DirectoryInfo(unityFolder).Name, WindowsShortcut.GetShortcutTarget(unityShortcut));
            }
            return installs;
        }
        /// <summary>
        /// Locate all installed version of Unity3D on macOS.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LocateUnityInstallsMacOS()
        {
            var installs = new Dictionary<string, string>();
            
            var find = new System.Diagnostics.Process();
            find.StartInfo.FileName = "find";
            find.StartInfo.Arguments = "/Applications/ -name Unity.app";
            find.StartInfo.RedirectStandardOutput = true;
            find.StartInfo.UseShellExecute = false;
            find.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            find.Start();
            string output = find.StandardOutput.ReadToEnd();
            find.WaitForExit();

            foreach(string line in output.SplitLines())
            {
                if (line.EndsWith("Unity.app"))
                {
                    string appPath = line;                     
                    string version = "";
                    if (!TryGetUnityVersion(appPath, "Contents", out version))
                    {
                        version = "Unity";
                    }
                    if (!installs.ContainsKey(version))
                    {
                        installs.Add(version, appPath);
                    }
                }
            }

            return installs;
        }

        public static bool TryGetUnityVersion(string unityPath, string subPath, out string version)
        {
            string unityDir = unityPath;
            if(!Directory.Exists(unityDir))
            {
                if(!File.Exists(unityDir))
                {
                    version = "";
                    return false;
                }

                unityDir = Path.GetDirectoryName(unityDir);
            }

            string versionDirPath = Path.Combine(unityDir, subPath, "PackageManager/Unity/PackageManager");
            foreach(var versionDir in Directory.GetDirectories(versionDirPath))
            {
                string ivyPath = Path.Combine(versionDir, "ivy.xml");
                if(File.Exists(ivyPath))
                {
                    return ReadVersionFromIvy(ivyPath, out version);
                }
            }
            version = "";
            return false;
        }

        public static bool ReadVersionFromIvy(string path, out string version)
        {
            XDocument xDoc = XDocument.Load(path);
            XElement module = xDoc.Element("ivy-module");
            if (module != null)
            {
                XElement info = module.Element("info");
                if (info != null)
                {
                    XAttribute eVersion = info.Attribute("{http://ant.apache.org/ivy/extra}unityVersion");
                    if (eVersion != null)
                    {
                        version = eVersion.Value;
                        return true;
                    }
                }
            }
            version = "";
            return false;
        }

        /// <summary>
        /// Gets the absolute path to the Unity3D editor log.
        /// This presumes that Unity3D is running as a user and not a service.
        /// </summary>
        /// <returns>The absolute path to the Unity3D editor log.</returns>
        public static string GetEditorLogLocation()
        {
            if (Environment.OSVersion.Platform == PlatformID.MacOSX ||
                Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return "~/Library/Logs/Unity/Editor.log";
            }
            else
            {
                var localAppdata = Environment.GetEnvironmentVariable("LocalAppData");
                if (string.IsNullOrEmpty(localAppdata))
                {
                    throw new Exception("Failed to find the 'LocalAppData' directory.");
                }
                return System.IO.Path.Combine(localAppdata, "Unity", "Editor", "Editor.log");
            }
        }

        /// <summary>
        /// Output all new log lines to the console for the specified log.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="outputEditorLog">Should the editor log produced by Unity3D whilst building.</param>
        /// <param name="logLocation">The location of the log file to redirect.</param>
        /// <param name="currentLine">The line of the log of which we have already redirected.</param>
        /// <returns></returns>
        public static bool ProcessEditorLog(ICakeContext context, bool outputEditorLog, string logLocation, ref int currentLine, DateTime startTime)
        {
            // The log doesn't exist, so we can't output its contents
            // to the console.
            if (!System.IO.File.Exists(logLocation))
            {
                return false;
            }
            
            if (System.IO.File.GetLastWriteTime(logLocation) < startTime)
            {
                return false;
            }

            // Read all lines from the log.
            var lines = SafeReadAllLines(logLocation);

            // If the log output is less that what we have already
            // redirected, return what we have outputted presuming
            // something went wrong when reading the file.
            if (lines.Length <= currentLine)
            {
                return false;
            }

            var hasError = false;

            // Output all new lines of the log.
            foreach (var line in lines.Skip(currentLine))
            {
                var logType = Unity3DEditorLog.ProcessLogLine(line);
                if (outputEditorLog)
                {
                    switch (logType)
                    {
                        case Unity3DEditorLog.MessageType.Debug:
                            context.Debug(line);
                            break;
                        case Unity3DEditorLog.MessageType.Info:
                            context.Information(line);
                            break;
                        case Unity3DEditorLog.MessageType.Warning:
                            context.Warning(line);
                            break;
                        case Unity3DEditorLog.MessageType.Error:
                            context.Error(line);
                            hasError = true;
                            break;
                    }
                }
                else if (logType == Unity3DEditorLog.MessageType.Error)
                {
                    hasError = true;
                }
            }

            // Return the new number of lines we have redirected.
            currentLine = lines.Length;
            return hasError;
        }
        
        /// <summary>
        /// Return build status from the specified log file after unity process exited.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="logLocation">The location of the log file to redirect.</param>
        /// <returns> true, if report found and success, or false </returns>
        public static bool BuildStatusFromLogs(ICakeContext context, string logLocation)
        {
            // The log doesn't exist, so we can't output its contents
            // to the console.
            if (!System.IO.File.Exists(logLocation))
            {
                return false;
            }

            // Read all lines from the log.
            var lines = SafeReadAllLines(logLocation);

            var buildSuccess = lines.Any(line => line.Contains("[Cake.Unity3D] Automated build completed"));

            return buildSuccess;
        }


        /// <summary>
        /// Read all lines from a given file.
        /// Using read/write file share access.
        /// </summary>
        /// <param name="path">The absolute path to the file to read.</param>
        /// <returns>An array of all lines in the log file.</returns>
        private static string[] SafeReadAllLines(string path)
        {
            try
            {
                using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
                {
                    using (var reader = new System.IO.StreamReader(fileStream))
                    {
                        var file = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            file.Add(reader.ReadLine());
                        }
                        return file.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                // Something went wrong, return an empty array
                return new string[0];
            }
        }
    }
}
