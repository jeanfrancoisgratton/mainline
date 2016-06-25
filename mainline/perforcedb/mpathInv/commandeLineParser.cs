// perforceDB : mpathInv
// Écrit par jfgratton (Jean-François Gratton), 2013.07.06 @ 08:21
// 
// commandeLineParser.cs : self-explanatory :-)

using System;
using System.IO;
using System.Reflection;

// mpathInv [-x connection string] [{ -d working_dir | -f file1[...file2...fileN] }]
// mpathInv { -h | -cl }

namespace JFG.Ubisoft.Perforce
{
    partial class mpathInv
    {
        private static void ClParser(string[] arguments)
        {
            int nn = 0;
            while (nn < arguments.Length)
            {
                switch (arguments[nn].ToLower())
                {
                    case "-d":
                        sWorkingDir = arguments[nn + 1].ToLower();
                        if (Directory.Exists(sWorkingDir) == false)
                        {
                            Console.WriteLine("Directory {0} does not exist. Aborting.", sWorkingDir);
                            Environment.Exit(-1);
                        }
                        else
                        {
                            Directory.SetCurrentDirectory(sWorkingDir);
                            alFiles = GetFileList();
                            Directory.SetCurrentDirectory(sCurrentDir);
                        }
                        ++nn;
                        break;
                    case "-f":
                        int n = nn + 1;
                        while (n < arguments.Length)
                            alFiles.Add(arguments[n++]);
                        break;
                    case "-x":
                        _connectString = arguments[nn + 1];
                        ++nn;
                        break;
                    case "-h":
                        throw new NotImplementedException("you dumbass, forgot that one didn't ya ? :p");
                        break;
                    case "-cl":
                        ShowChangeLog();
                        Directory.SetCurrentDirectory(sCurrentDir);
                        Environment.Exit(0);
                        break;
                    default:
                        ++nn;
                        break;
                }
            }
        }

        private static void ShowChangeLog()
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            StreamReader sr = new StreamReader(_assembly.GetManifestResourceStream("JFG.Ubisoft.Perforce.changelog.txt"));
            string clLigne;
            Console.Clear();
            Console.WriteLine("mpathInv v" + Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine(Environment.NewLine);

            while ((clLigne = sr.ReadLine()) != null)
                Console.WriteLine(clLigne);
        }
    }
}