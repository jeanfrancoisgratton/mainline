using System;
using System.IO;
using JFG.SysUtils.Files;

namespace batchRenamer
{
    partial class batchRenamer
    {
        public static void Main(string[] args)
        {
            string suffixe = "";
            string sCurDir = Directory.GetCurrentDirectory();

            if (parseCLI(args, ref suffixe) == false)
                ShowHelp();

            LowerizeDirectory(args[0], sCurDir);
            batchRename(args[0], suffixe);
        }

        private static void LowerizeDirectory(string newdir, string currentdir)
        {
            if (Directory.Exists(newdir) == false)
            {
                Console.WriteLine("Le repertoire {0} n'existe pas.", newdir);
                Environment.Exit(3);
            }
            Directory.SetCurrentDirectory(newdir);
            FileUtils.LowerDir(newdir);
            Directory.SetCurrentDirectory(currentdir);
        }

        private static void batchRename(string repertoire, string suffixe)
        {
            string sDirname = "";
            int newIndex = 0, index;

            if (Directory.Exists(repertoire) == false)
            {
                Console.WriteLine("Le repertoire {0} n'existe pas.", repertoire);
                Environment.Exit(3);
            }
            Directory.SetCurrentDirectory(repertoire);
            index = repertoire.LastIndexOf(Path.DirectorySeparatorChar);
            if (index == -1)
                sDirname = repertoire;
            else
                sDirname = repertoire.Substring(++index);

            rename(ref newIndex, sDirname);
        }

        private static void rename(ref int index, string dir)
        {
            string[] files = Directory.GetFiles(".", "*.*");
        }
    }
}
