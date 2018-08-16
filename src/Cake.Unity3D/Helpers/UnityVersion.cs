using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Unity3D.Helpers
{
    public enum UnityDevelopmentState
    {
        Any = 0,
        Alpha = 1,
        Beta = 2,
        ReleaseCandidate = 3,
        Final = 4,
    }

    public class UnityVersion
    {
        public string Major { get; set; } = "";
        public string Minor { get; set; } = "";
        public string Revision { get; set; } = "";
        public UnityDevelopmentState DevelopmentState { get; set; } = UnityDevelopmentState.Any;
        public string ReleaseVersion { get; set; } = "";

        public UnityVersion()
        {

        }

        public UnityVersion(string version)
        {
            UnityVersion parsed = Parse(version);
            Major = parsed.Major;
            Minor = parsed.Minor;
            Revision = parsed.Revision;
            DevelopmentState = parsed.DevelopmentState;
            ReleaseVersion = parsed.ReleaseVersion;
        }

        public int CompareTo(string version)
        {
            return CompareTo(Parse(version));
        }

        public int CompareTo(UnityVersion version)
        {
            int comp = 0;
            comp += CompareValue(Major, version.Major) * 32 * 32 * 32 * 32;
            comp += CompareValue(Minor, version.Minor) * 32 * 32 * 32;
            comp += CompareValue(Revision, version.Revision) * 32 * 32;
            comp += CompareValue(DevelopmentState, version.DevelopmentState) * 32;
            comp += CompareValue(ReleaseVersion, version.ReleaseVersion);
            return comp;
        }

        int CompareValue(string lhs, string rhs)
        {
            int lhsNum = 0;
            bool lhsINum = int.TryParse(lhs, out lhsNum);
            int rhsNum = 0;
            bool rhsINum = int.TryParse(rhs, out rhsNum);
            if(lhsINum && rhsINum)
            {
                return lhsNum - rhsNum;
            }

            if(lhs == "*" || lhs == "" ||
                rhs == "*" || rhs == "")
            {
                return 0;
            }
            return lhs.CompareTo(rhs);
        }

        int CompareValue(UnityDevelopmentState lhs, UnityDevelopmentState rhs)
        {
            if(lhs == UnityDevelopmentState.Any || rhs == UnityDevelopmentState.Any)
            {
                return 0;
            }
            return ((int)rhs).CompareTo((int)lhs);
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ReleaseVersion))
            {
                return $"{Major}.{Minor}.{Revision}{DevelopmentStateToString(DevelopmentState)}{ReleaseVersion}";
            }
            if (DevelopmentState != UnityDevelopmentState.Any)
            {
                return $"{Major}.{Minor}.{Revision}{DevelopmentStateToString(DevelopmentState)}";
            }
            if (!string.IsNullOrEmpty(Revision))
            {
                return $"{Major}.{Minor}.{Revision}";
            }
            if (!string.IsNullOrEmpty(Minor))
            {
                return $"{Major}.{Minor}";
            }
            return $"{Major}";
        }

        public static UnityVersion Parse(string input)
        {
            string[] parts = input.Split('.');
            var version = new UnityVersion();
            version.Major = parts.Length >= 1 ? parts[0] : "";
            version.Minor = parts.Length >= 2 ? parts[1] : "";
            if (parts.Length >= 3)
            {
                UnityDevelopmentState state;
                string revString;
                string releaseString;
                if (TryGetDevelopmentState(parts[2].ToLower(), out state, out revString, out releaseString))
                {
                    version.Revision = revString;
                    version.DevelopmentState = state;
                    version.ReleaseVersion = releaseString;
                }
                else
                {
                    version.Revision = parts[2];
                    version.DevelopmentState = UnityDevelopmentState.Any;
                    version.ReleaseVersion = "";
                }
            }
            return version;
        }

        static bool TryGetDevelopmentState(string input, out UnityDevelopmentState state, out string prefix, out string surfix)
        {
            int pos = 0;
            do
            {
                // scan for state char of the state
                pos = input.IndexOfAny(new char[] { '*', 'a', 'b', 'r', 'f' }, pos);                
                if(pos < 0)
                {
                    break;
                }
                // Get the char we found
                char c = input[pos];
                // move the head to next char in case we continue
                pos += 1;
                // if its a r it may be a rc so we need to check
                if (c == 'r')
                {
                    if(input.Length < pos || input[pos] != 'c')
                    {
                        // this is not a 'rc' so move on
                        continue;
                    }
                }

                // subPos will be the pos we will cut the surfix, rc needs to move it +1 so we decalare it ahead of state setting
                int subPos = pos;
                switch(c)
                {
                    case '*':
                        // Detect a "*f1" or "***" pattern and ignore the first "*"
                        if(pos == 1)
                        {
                            continue;
                        }
                        state = UnityDevelopmentState.Any;
                        break;
                    case 'a':
                        state = UnityDevelopmentState.Alpha;
                        break;
                    case 'b':
                        state = UnityDevelopmentState.Beta;
                        break;
                    case 'r':
                        state = UnityDevelopmentState.ReleaseCandidate;
                        subPos += 1;
                        break;
                    case 'f':
                        state = UnityDevelopmentState.Final;
                        break;
                    default:
                        // This is imposible to reach
                        throw new NotImplementedException();
                }

                // cut input before the state into prefix
                prefix = input.Substring(0, pos-1);
                // cur input after state into surfix
                if (subPos < input.Length)
                {
                    surfix = input.Substring(subPos, input.Length - subPos);
                }
                else
                {
                    surfix = "";
                }
                return true;
            }
            while (pos < input.Length);
            state = UnityDevelopmentState.Any;
            prefix = input;
            surfix = "";
            return false;
        }

        static string DevelopmentStateToString(UnityDevelopmentState state)
        {
            switch(state)
            {
                case UnityDevelopmentState.Any:
                    return "*";
                case UnityDevelopmentState.Alpha:
                    return "a";
                case UnityDevelopmentState.Beta:
                    return "b";
                case UnityDevelopmentState.ReleaseCandidate:
                    return "rc";
                case UnityDevelopmentState.Final:
                    return "f";
            }
            throw new NotImplementedException();
        }

        public static UnityVersion GetNewest(IEnumerable<UnityVersion> versions)
        {
            UnityVersion selected = null;

            foreach (var version in versions)
            {
                if (selected == null || selected.CompareTo(version) < 0)
                {
                    selected = version;
                }
            }
            return selected;
        }

        public static UnityVersion GetNewest(IEnumerable<UnityVersion> versions, UnityVersion match)
        {
            UnityVersion selected = null;

            foreach (var version in versions)
            {
                Console.WriteLine($"{match.ToString()}: {version.ToString()}");
                if(match.CompareTo(version) >= 0)
                {
                    if(selected == null || selected.CompareTo(version) < 0)
                    {
                        selected = version;
                    }
                }
            }
            return selected;
        }

        static IEnumerable<UnityVersion> EnumVersions(IEnumerable<string> list)
        {
            foreach(var elem in list)
            {
                yield return new UnityVersion(elem);
            }
        }

        public static UnityVersion GetNewest(IEnumerable<string> versions)
        {
            return GetNewest(EnumVersions(versions));
        }

        public static UnityVersion GetNewest(IEnumerable<string> versions, string mach)
        {
            return GetNewest(EnumVersions(versions), new UnityVersion(mach));
        }
    }
}
