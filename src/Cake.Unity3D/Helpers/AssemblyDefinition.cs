using System;
using Newtonsoft.Json;

namespace Cake.Unity3D.Helpers
{
    public class AssemblyDefinition
    {
        public string name;

        public static AssemblyDefinition ReadFile(string path)
        {
            string json = System.IO.File.ReadAllText(path);
            return JsonConvert.DeserializeObject<AssemblyDefinition>(json);
        }
    }
}
