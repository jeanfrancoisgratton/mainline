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
        private static void showHelp()
        {
            Console.Clear();
            Console.WriteLine("USAGE: renommePhotos DIRECTORY {-photos|-films|-tout}");
            Environment.Exit(0);
        }

        private static void ShowChangeLog()
        {
            StreamReader sr = null;
            try
            {
                Assembly _assembly = Assembly.GetExecutingAssembly();
                sr = new StreamReader(_assembly.GetManifestResourceStream("rf.Resources.changelog.txt"));
                string sLine;

                Console.Clear();
                Console.WriteLine("Renamer - Ecrit par J.F.Gratton, 2011 - 2015");

                while ((sLine = sr.ReadLine()) != null)
                    Console.WriteLine(sLine);
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
    }
}