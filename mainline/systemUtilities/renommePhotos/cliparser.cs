using System;
using System.IO;
using System.Reflection;
//using JFG.SysUtils.Files;

namespace batchRenamer
{
    partial class batchRenamer
    {
        private static bool parseCLI(string []args, ref string suffixe)
        {
            bool bRet = true;
            int i = 0;

            while (i < args.Length)
            {
                switch (args[i].ToLower())
                {
                    case "-help":
                    case "-h":
                    case "-?":
                        ShowHelp();
                        break;
                    case "-changelog":
                        ShowChangeLog();
                        break;
                    case "-suffix":
                        if (i < args.Length - 1)
                            suffixe = args[++i];
                        else
                            bRet = false;
                        break;
                    default:
                        ++i;
                        break;
                }
            }

            return bRet;
        }

        private static void ShowChangeLog()
        {
            StreamReader sr = null;
            try
            {
                Assembly _assembly = Assembly.GetExecutingAssembly();
                sr = new StreamReader(_assembly.GetManifestResourceStream("batchRenamer.Resources.changelog.txt"));
                string ligne;

                Console.Clear();
                Console.WriteLine("Renamer - Ecrit par J.F.Gratton, 2011 - 2016");

                while ((ligne = sr.ReadLine()) != null)
                    Console.WriteLine(ligne);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
                Environment.Exit(0);
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("PLACEHOLDER");
        }
    }
}