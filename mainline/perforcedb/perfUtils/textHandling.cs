// perforceDB : perfUtils
// textHandling.cs : text formatting of SELECTs
// 2013.03.11.08:11, Jean-Francois Gratton

namespace JFG.Ubisoft.Perforce
{
    public static partial class perfUtils
    {
        public static string PadOutPut (string phrase, int columnLength, char c=' ')
        {
            bool b = true;
            if (phrase.Length > columnLength)
                return phrase.Substring(0, columnLength - 3) + "...";

            return phrase += new string(c, columnLength - phrase.Length);
        }

        public static string StripVersion(string versionString)
        {
            return versionString.Replace("P4D/LINUX26X86_64/", "").Substring(0, 6);
        }
    }
}
