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
    public class Unity3DBuildDependencyOptions
    {
        /// <summary>
        /// List of all Depenedencies to build
        /// </summary>
        public List<Dependency> Dependencies { get; set; } = new List<Dependency>();
        
        /// <summary>
        /// Set to true in order to Compile Assamblys as Debug version
        /// </summary>
        public bool DebugBuild { get; set; } = false;

        /// <summary>
        /// Set to true in order to copy the pdb Debug Information into the Target
        /// </summary>
        public bool CopyPdb { get; set; } = false;

        /// <summary>
        /// Set to true in order to use and copy the meta information from the Source
        /// </summary>
        public bool UseSourceMetaFiles { get; set; } = false;

        /// <summary>
        /// Configuration for a single Dependency to build
        /// </summary>
        public struct Dependency
        {
            /// <summary>
            /// The source name of the dependency to build
            /// </summary>
            public string Source;
            /// <summary>
            /// The target name of the dependency to build
            /// </summary>
            public string Target;
        }
    }
}
