using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Unity3D.Helpers
{
    public class VisualStudioProject
    {
        public string FileName;
        public string Ident;

        public static VisualStudioProject ReadFile(string path)
        {
            VisualStudioProject project = new VisualStudioProject();
            project.FileName = System.IO.Path.GetFileName(path);

            XDocument xDoc = XDocument.Load(path);
            XElement xProject = xDoc.Element("{http://schemas.microsoft.com/developer/msbuild/2003}Project");
            foreach (var xPropertyGroup in xProject.Elements("{http://schemas.microsoft.com/developer/msbuild/2003}PropertyGroup"))
            {
                foreach (var yProjectGuid in xPropertyGroup.Elements("{http://schemas.microsoft.com/developer/msbuild/2003}ProjectGuid"))
                {
                    project.Ident = yProjectGuid.Value;
                }
            }

            return project;
        }

        public static bool TryReadFile(string path, out VisualStudioProject project)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    project = ReadFile(path);
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception: {0}\n\r{1}", ex.Message, ex.StackTrace);
                }
            }
            project = null;
            return false;
        }
    }
}
