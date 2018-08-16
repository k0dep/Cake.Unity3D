using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.Unity3D.Test
{
    [TestClass]
    public class TestUnityInstall
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void ListAllInstalls()
        {
            var installs = Helpers.Unity3DEditor.LocateUnityInstalls();

            foreach(var install in installs)
            {
                TestContext.WriteLine($"{install.Key} => {install.Value}");
            }
        }

        [TestMethod]
        public void TestUnityVersionParse()
        {
            foreach(string version in versions)
            {
                var info = new Helpers.UnityVersion(version);
                TestContext.WriteLine($"{version} => {info.ToString()}");
                Assert.AreEqual(info.ToString(), version);
            }
        }

        [TestMethod]
        public void TestUnityVersionCompare()
        {
            Action<string,string, int> compare = (string vl, string vr, int target) =>
            {
                var comp = new Helpers.UnityVersion(vl).CompareTo(vr);
                int compCut = comp > 0 ? 1 : comp < 0 ? -1 : 0;
                TestContext.WriteLine($"{vl} : {vr} = {comp}");
                Assert.AreEqual(compCut, target);
            };

            compare("2018.2.3f1", "2018.1.3f1", 1);
            compare("2018.2.3f1", "2018.2.3f1", 0);
            compare("2018.2.3f1", "2018.3.3f1", -1);

            compare("2018.2.3f1", "2018.*", 0);
            compare("2018.2.3f1", "2018.2.*", 0);
            compare("2018.2.3f1", "2018.3.3f*", -1);
        }

        readonly string[] versions = new[]
        {
            "2017.4.1rc1",
            "2018.3.5f2",
            "2018.2.2f1",
            "2018.2.1b1",
            "2018.2.9f1",
            "2017.4.1rc1",
            "2017.3.1f1",
            "2017.2.1a1",
            "5.4.3f1",
            "5.3.5rc2",
            "4.6.3f2",
            "2018.3.5f",
            "2018.3.5",
            "2018.3",
            "2018.3.5*1",
            "2018.*.5",
            "2018"
        };

        [TestMethod]
        public void TestUnityVersionGetNewest()
        {
            List<Helpers.UnityVersion> versionsParsed = new List<Helpers.UnityVersion>();
            foreach (string version in versions)
            {
                var info = new Helpers.UnityVersion(version);
                versionsParsed.Add(info);
            }

            Action<string, string> getNewest = (string mach, string target) =>
            {
                Helpers.UnityVersion info = Helpers.UnityVersion.GetNewest(versionsParsed, new Helpers.UnityVersion(mach));
                Assert.IsNotNull(info);
                TestContext.WriteLine($"{mach} => {info.ToString()}");
                Assert.AreEqual(info.ToString(), target);
            };

            getNewest("2018.*", "2018.3.5f2");
            getNewest("2018.2.*", "2018.2.9f1");
            getNewest("2018", "2018.3.5f2");
            getNewest("2017.*.*f*", "2017.3.1f1");
            getNewest("2017.2.1", "2017.2.1a1");
            getNewest("5.4", "5.4.3f1");
        }
    }
}
