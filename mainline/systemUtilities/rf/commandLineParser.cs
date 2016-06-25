using System;
using System.IO;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using JFG.SysUtils;
using JFG.SysUtils.Files;

namespace renamer
{
    internal partial class renommePhotos
    {
        private static void parseCLI(string[] args, ref RenameEnum rE, ref bool bCamInfo, ref string sSuffix)
        {
            int i = 0;
            while (i < args.Length)
            {
                switch (args[i].ToLower())
                {
                    case "-changelog":
                        ShowChangeLog();
                        break;

                    case "-pix":
                    case "-photos":
                        rE = RenameEnum.photos;
                        break;

                    case "-mov":
                    case "-films":
                        rE = RenameEnum.films;
                        break;

                    case "-all":
                    case "-tout":
                        rE = RenameEnum.tout;
                        break;

                    case "-nocaminfo":
                        bCamInfo = false;
                        break;

                    case "-nosuffix":
                        sSuffix = "";
                        break;

                    default:
                        ++i;
                        break;
                }
            }
        }

        
    }
}