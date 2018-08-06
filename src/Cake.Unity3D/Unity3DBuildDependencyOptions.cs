using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Unity3D
{
    /// <summary>
    /// All build options available for building Unity3d dependencies.
    /// </summary>
    public class Unity3DBuildDependencyOptions : IUnity3DBuildOptions
    {
        /// <summary>
        /// The location of the Unity.exe to use.
        /// Default: null
        /// </summary>
        public string UnityEditorLocation { get; set; }

        /// <summary>
        /// Should the editor log produced by Unity3D whilst building
        /// be output to the console.
        /// Default: true
        /// </summary>
        public bool OutputEditorLog { get; set; }

        /// <summary>
        /// Should we install the automated build script
        /// even if we find an existing one.
        /// Default: false
        /// </summary>
        public bool ForceScriptInstall { get; set; }

        // TOOD: this needs to be a source -> Target thing

        /// <summary>
        /// The names of all the dependencies to build
        /// Default: empty list
        /// </summary>
        public List<string> TargetDependencieNames { get; set; } = new List<string>();
    }
}
