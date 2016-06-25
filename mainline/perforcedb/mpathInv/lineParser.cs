// perforceDB : mpathInv
// Écrit par : jfgratton (), 2013.08.13 @ 07:11
// 
// lineParser.cs : Découpage de chaque ligne du fichier de multipath

using System.Collections;
using System.Collections.Generic;
using System.IO;

//using JFG.SysUtils;

namespace JFG.Ubisoft.Perforce
{
    partial class mpathInv
    {
        private static void FileParser(List<string> lignes)
        {
            int nCounter = 0;
            while (nCounter < lignes.Count)
            {
                if (lignes[nCounter].StartsWith("SERVEUR="))
                    SERVEUR = lignes[nCounter].Replace("SERVEUR=", "");
                if (lignes[nCounter].StartsWith("2") || lignes[nCounter].StartsWith("3") ||
                    lignes[nCounter].StartsWith("mp_")) // we have a valid block
                    BlockParser(nCounter, lignes);
                ++nCounter;
            }
        }

        private static void BlockParser(int counter, List<string> lignes)
        {
            PathInfoStruct pathInfo = new PathInfoStruct();
            pathInfo.alDisks = new ArrayList();
            pathInfo.alUnreadyDisks = new ArrayList();
            if (lignes[counter][0] == '2' || lignes[counter][0] == '3' || lignes[counter].StartsWith("mp_"))
                GetWWWIDsanDesc(lignes[counter], ref pathInfo);
            ++counter;

            while (counter < lignes.Count || lignes[counter][0] == '2' || lignes[counter][0] == '3' ||
                   lignes[counter].StartsWith("mp_"))
            {
                if (lignes[counter].StartsWith("[size="))
                    GetSize(lignes[counter], ref pathInfo);
                if (lignes[counter].StartsWith(@" \_"))
                    GetLunIDdiskStatus(lignes[counter], ref pathInfo);
                ++counter;
            }
            CommitPathInfoToDB(pathInfo);
        }

        private static void GetFileList(string workDir)
        {
            Directory.SetCurrentDirectory(workDir);

        }
    }
}