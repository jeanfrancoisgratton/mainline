using System;
using JFG.SysUtils;

namespace JFG.Ubisoft.Perforce
{
    partial class mpathInv
    {
        // deux possibilités:
        // mp_petzfantasy (3600508b400068c380000c00002780000) dm-26 HP,HSV210
        // 3600508b400068c3c0001000000170000 dm-16 HP,HSV210
        private static void GetWWWIDsanDesc(string ligne, ref PathInfoStruct pathInfo)
        {
            if (ligne[0] == '2' || ligne[0] == '3')
                pathInfo.Desc = pathInfo.WWID = sysutils.Left(ligne.IndexOf(' '), ligne);
            if (ligne.StartsWith("mp_"))
                pathInfo.Desc = sysutils.Left(ligne.IndexOf(' '), ligne);

            // to get the SAN value, we need to find the first whitespace after dm-XXX
            string l = ligne.Substring(ligne.IndexOf("dm-"));
            pathInfo.SAN = l.Substring(l.IndexOf(' '));
        }

        // [size=300G][features=1 queue_if_no_path][hwhandler=0][rw]
        private static void GetSize(string ligne, ref PathInfoStruct pathInfo)
        {
            string l = sysutils.Left(ligne.IndexOf(']'), ligne);
            pathInfo.Size = l.Replace("[size=", "");
        }

        // \_ 4:0:5:4   sdbq 68:64   [active][ready]
        private static void GetLunIDdiskStatus(string ligne, ref PathInfoStruct pathInfo)
        {
            string m = ligne.Replace(@" \_ ", "");
            string[] l = m.Split(' ');
            string[] lun = l[0].Split(':');
            if (Int32.TryParse(lun[3], out pathInfo.LunID) == false)
                pathInfo.LunID = -1;
            if (ligne.Contains("[active][ready]") == false)
                pathInfo.alUnreadyDisks.Add(l[1]);
            else
                pathInfo.alDisks.Add(l[1]);
        }
    }
}