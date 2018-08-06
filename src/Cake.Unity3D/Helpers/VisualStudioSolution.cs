using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Unity3D.Helpers
{
    public class VisualStudioSolution
    {
        enum Element
        {
            Unknown,
            Header,
            ProjectStart,
            ProjectEnd,
            GlobalStart,
            GlobalEnd,
            SolutionConfigurationPlatformsStart,
            SolutionConfigurationPlatforms,
            SolutionConfigurationPlatformsEnd,
            ProjectConfigurationPlatformsStart,
            ProjectConfigurationPlatforms,
            ProjectConfigurationPlatformsEnd,
            SolutionPropertiesStart,
            SolutionProperties,
            SolutionPropertiesEnd,
        }

        public static string[] BuildConfigurations = new string[] { "Debug", "Release" };
        public static string[] CPUConfigurations = new string[] { "ActiveCfg", "Build.0" };

        public string Ident { get; set; }
        public Dictionary<string, VisualStudioProject> Projects = new Dictionary<string, VisualStudioProject>();

        public VisualStudioSolution()
        {
            Ident = "{" + Guid.NewGuid().ToString() + "}";
        }

        public static VisualStudioSolution ReadFile(string path)
        {
            Element lastElement = Element.Header;

            var sr = new System.IO.StreamReader(path, true);
            VisualStudioSolution solution = new VisualStudioSolution();

            while (!sr.EndOfStream)
            {
                Element element;
                string line = sr.ReadLine();
                if (!TryGetElementType(line, lastElement, out element))
                {
                    switch (lastElement)
                    {
                        case Element.SolutionConfigurationPlatformsStart:
                        case Element.SolutionConfigurationPlatforms:
                            element = Element.SolutionConfigurationPlatforms;
                            break;
                        case Element.ProjectConfigurationPlatformsStart:
                        case Element.ProjectConfigurationPlatforms:
                            element = Element.ProjectConfigurationPlatforms;
                            break;
                        case Element.SolutionPropertiesStart:
                        case Element.SolutionProperties:
                            element = Element.SolutionProperties;
                            break;
                        default:
                            element = Element.Unknown;
                            break;
                    }
                }

                switch (element)
                {
                    case Element.ProjectStart:
                        string solutionIdent;
                        VisualStudioProject project = ReadProjectLine(line, out solutionIdent);
                        solution.Projects.Add(project.Ident, project);
                        solution.Ident = solutionIdent;
                        break;
                }
            }

            sr.Close();

            return solution;
        }

        static bool TryGetElementType(string line, Element lastElement, out Element element)
        {
            if (line.StartsWith("Project"))
            {
                element = Element.ProjectStart;
                return true;
            }
            else if (line.StartsWith("EndProject"))
            {
                element = Element.ProjectEnd;
                return true;
            }

            else if (line.StartsWith("Global"))
            {
                element = Element.GlobalStart;
                return true;
            }
            else if (line.StartsWith("EndGlobal"))
            {
                element = Element.GlobalEnd;
                return true;
            }

            else if (line.StartsWith("	GlobalSection"))
            {
                int start = line.IndexOf("(") + 1;
                int end = line.IndexOf(")");

                string sectionType = line.Substring(start, end - start);
                switch (sectionType)
                {
                    case "SolutionConfigurationPlatforms":
                        element = Element.SolutionConfigurationPlatforms;
                        return true;
                    case "ProjectConfigurationPlatforms":
                        element = Element.ProjectConfigurationPlatforms;
                        return true;
                    case "SolutionProperties":
                        element = Element.SolutionProperties;
                        return true;
                }
            }
            else if (line.StartsWith("  EndGlobalSection"))
            {
                switch (lastElement)
                {
                    case Element.SolutionConfigurationPlatformsStart:
                    case Element.SolutionConfigurationPlatforms:
                        element = Element.SolutionConfigurationPlatformsEnd;
                        return true;
                    case Element.ProjectConfigurationPlatformsStart:
                    case Element.ProjectConfigurationPlatforms:
                        element = Element.ProjectConfigurationPlatformsEnd;
                        return true;
                    case Element.SolutionPropertiesStart:
                    case Element.SolutionProperties:
                        element = Element.SolutionPropertiesEnd;
                        return true;
                }
            }
            element = Element.Unknown;
            return false;
        }

        static VisualStudioProject ReadProjectLine(string line, out string SolutionIdent)
        {
            // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "client", "Assembly-CSharp.csproj", "{A2859A56-AB4A-51F9-60BB-1B6FC96E8C70}"

            string[] parts = line.Split('"');
            VisualStudioProject project = new VisualStudioProject();
            //project.Name = parts[5].Replace(".csproj", "");
            project.FileName = parts[5];
            project.Ident = parts[7];

            SolutionIdent = parts[1];

            return project;
        }

        public static bool TryReadFile(string path, out VisualStudioSolution solution)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    solution = ReadFile(path);
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception: {0}\n\r{1}", ex.Message, ex.StackTrace);
                }
            }
            solution = null;
            return false;
        }

        public static void WriteFile(string path, VisualStudioSolution solution)
        {
            var sw = new System.IO.StreamWriter(path, false, Encoding.Unicode);

            sw.WriteLine("Microsoft Visual Studio Solution File, Format Version 12.00");
            sw.WriteLine("# Visual Studio 15");
            foreach (var project in solution.Projects.Values)
            {
                sw.WriteLine(String.Format("Project(\"{0}\") = \"{1}\", \"{1}\", \"{2}\"", solution.Ident, project.FileName, project.Ident));
                sw.WriteLine("EndProject");
            }
            sw.WriteLine("Global");
            sw.WriteLine("	GlobalSection(SolutionConfigurationPlatforms) = preSolution");
            sw.WriteLine("		Debug|Any CPU = Debug|Any CPU");
            sw.WriteLine("		Release|Any CPU = Release|Any CPU");
            sw.WriteLine("	EndGlobalSection");
            sw.WriteLine("	GlobalSection(ProjectConfigurationPlatforms) = postSolution");
            foreach (var project in solution.Projects.Values)
            {
                foreach (var buildConfig in BuildConfigurations)
                {
                    foreach (var cpuConfig in CPUConfigurations)
                    {
                        sw.WriteLine(String.Format("        {0}.{1}|Any CPU.{2} = {1}|Any CPU", project.Ident, buildConfig, cpuConfig));
                    }
                }
            }
            sw.WriteLine("	EndGlobalSection");
            sw.WriteLine("	GlobalSection(SolutionProperties) = preSolution");
            sw.WriteLine("		HideSolutionNode = FALSE");
            sw.WriteLine("	EndGlobalSection");
            sw.WriteLine("EndGlobal");

            sw.Flush();
            sw.Close();
        }

        public void AddProject(string projectPath)
        {
            VisualStudioProject project = VisualStudioProject.ReadFile(projectPath);
            if (!Projects.ContainsKey(project.Ident))
            {
                Projects.Add(project.Ident, project);
            }
        }
    }
}
