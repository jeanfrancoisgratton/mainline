// perforceDB : mpathInv
// Écrit par jfgratton (Jean-François Gratton), 2013.07.06 @ 08:26
// 
// fileHelper.cs : gestion des fichiers d'output de multipath -ll générés sur le serveur


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// mpathInv [{ -d working_dir | -f file1[...file2...fileN] }]
// mpathInv { -h | -cl }

namespace JFG.Ubisoft.Perforce
{
    partial class mpathInv
    {
        private static string FileParser()
        {
            string sExceptionMessage = "";
            StreamReader sr = null;

            try
            {
                foreach (string s in alFiles)
                {
                    string ligne;
                    List<string> lignes = new List<string>();
                    sr = new StreamReader(new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.Read));

                    while ((ligne = sr.ReadLine()) != null)
                        lignes.Add(ligne);

                    //BlockParser(lignes);
                    FileParser(lignes);
                }
            }
            catch (Exception ex)
            {
                sExceptionMessage = ex.Message;
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
            
            return sExceptionMessage;
        }

        private static ArrayList GetFileList()
        {
            ArrayList alF = new ArrayList();
            string[] sArray = Directory.GetFiles(sWorkingDir, "mpll.??");

            foreach (string s in sArray)
                alF.Add(s);

            return alF;
        }
    }
}