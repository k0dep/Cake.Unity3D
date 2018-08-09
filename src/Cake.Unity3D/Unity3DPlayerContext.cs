using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;
using Cake.Unity3D.Helpers;

namespace Cake.Unity3D
{
    public class Unity3DPlayerContext : Unity3DContext<Unity3DPlayerSettings>
    {
        public Unity3DPlayerContext(ICakeContext context, Unity3DProjectOptions projectOptions, Unity3DPlayerSettings settings)
            : base(context, projectOptions, settings)
        {
        }

        public override void DumpOptions()
        {
            base.DumpOptions();
            m_buildOptions.DumpOptions();
        }

        public override void Build()
        {
            base.Build();

            Dictionary<string, string> args = new Dictionary<string, string>();
            m_buildOptions.FillIntoArgs(args);

            RunUnityCommand("Cake.Unity3D.AutomatedBuild.SetPlayerSettings", args);
        }
    }
}
