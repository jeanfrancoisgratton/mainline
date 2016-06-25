using System;
using System.IO;
//using System.Security.Authentication.ExtendedProtection;
using JFG.SysUtils;
using JFG.SysUtils.Files;

// l:620, s:55, j:85, f:245

// usage:
// rf DIRECTORY {pix|mov|all} [-nocaminfo] [-suffix SUFFIX]

namespace renamer
{
    public enum RenameEnum
    {
        undefined = -1,
        extension = 0,
        photos = 1,
        films = 2,
        tout = 3
    };

    internal partial class renommePhotos
    {
        private static void Main(string[] args)
        {
	        bool bCamInfo = true;
            RenameEnum rE = RenameEnum.undefined;
            string sSuffix = null;
            string sCurrentDirectory = Directory.GetCurrentDirectory();


	        parseCLI(args, ref rE, ref bCamInfo, ref sSuffix);
            if (args.Length < 2)
                showHelp();

			
            LowerizeDirectory(args[0], sCurrentDirectory);
            go(args[0], rE, bCamInfo, sSuffix);
            Directory.SetCurrentDirectory(sCurrentDirectory);
        }

        private static void go(string repertoire, RenameEnum re, bool camInfo, string suffixe)
        {
            string sDirname;
            int newIndex = 0;

            if (Directory.Exists(repertoire) == false)
            {
                Console.WriteLine("Le repertoire {0} n'existe pas", repertoire);
                return;
            }
            Directory.SetCurrentDirectory(repertoire);
            int index = repertoire.LastIndexOf(@"\");
            if (index == -1)
                sDirname = repertoire;
            else
                sDirname = repertoire.Substring(++index);

            switch (re)
            {
                case RenameEnum.films:
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "mov");
                    batchRename(ref newIndex, sDirname, "mpeg");
                    batchRename(ref newIndex, sDirname, "mp4");
                    batchRename(ref newIndex, sDirname, "mpg");
                    batchRename(ref newIndex, sDirname, "avi");
                    break;

                case RenameEnum.photos:
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "jpeg");
                    batchRename(ref newIndex, sDirname, "jpg");
                    break;

                case RenameEnum.tout:
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "jpeg");
                    batchRename(ref newIndex, sDirname, "jpg");
                    newIndex = 0;
                    batchRename(ref newIndex, sDirname, "mov");
                    batchRename(ref newIndex, sDirname, "mpeg");
                    batchRename(ref newIndex, sDirname, "mp4");
                    batchRename(ref newIndex, sDirname, "mpg");
                    batchRename(ref newIndex, sDirname, "avi");
                    break;
            }
        }

        private static void batchRename(ref int newIndex, string dir, string ext)
        {
            //Directory.SetCurrentDirectory(dir);
            string[] files = Directory.GetFiles(".", "*." + ext.ToLower());

            Array.Sort(files);
            foreach (string f in files)
            {
                string intermediateName = getDigits(ref newIndex, f);
                if (intermediateName == null)
                    continue;
                //if (f.ToLower().StartsWith(".\\img_") == true)
	            {
                    string sNewName = dir.ToLower() + " - " + intermediateName + " " + GetCameraMake (f) + "." + ext;
                    Console.WriteLine(f.ToLower().Substring(2) + " -> " + sNewName);
		            if (File.Exists(sNewName))
			            incrementTargetFileName(ref sNewName);
                    File.Move(f, sNewName);
                }
            }
        }

        private static string getDigits(ref int index, string file)
        {
            int nPos = 0, nExtPos = file.LastIndexOf("."), nRealIndex;
            string ff = file, strDigits = "";
            bool charEncountered = false;

            // stripping off of the extension
            if (nExtPos > 0) // has an extension
                ff = file.Substring(0, nExtPos);

            // ok, on commence le travail...
            string reverseFile = SysUtils.Reverse(ff);

            while (nPos < reverseFile.Length && !charEncountered)
            {
                if (reverseFile[nPos] >= '0' && reverseFile[nPos] <= '9')
                    strDigits += reverseFile[nPos++];
                else
                    charEncountered = true;
            }

            if (nPos == 0)
                nRealIndex = ++index;
            else
                return SysUtils.Reverse(strDigits);

            return nRealIndex.ToString();
        }

	    private static void incrementTargetFileName(ref string fname)
	    {
		    int nCount = 1;
		    while (File.Exists(fname + "-" + nCount))
			    ++nCount;
		    fname += "-" + nCount;
	    }

        private static string GetCameraMake (string file)
        {
            if (file.ToLower().Contains("dsc") || file.ToLower().Contains("csc"))
                return "Nikon";
            if (file.ToLower().Contains("img"))
                return "Canon";
            return "";
        }

        private static void LowerizeDirectory(string newDir, string sCurrentDirectory)
        {
            if (Directory.Exists(newDir) == false)
            {
                Console.WriteLine("Le repertoire {0} n'existe pas", newDir);
                Environment.Exit(0);
            }
            Directory.SetCurrentDirectory(newDir);
            FileUtils.LowerDir(newDir);            
            Directory.SetCurrentDirectory(sCurrentDirectory);
        }
    }
}