// perforceDB : getInstanceInfo
// commandlineHelpers.cs : command line parser, help method
// 2013.01.31.11:13, Jean-Francois Gratton


using System;
using System.Collections;
using System.IO;
using System.Reflection;

// Usage : gii {-p port | {-l | -i} instance}
// -p port : donner le #port de l'instance plutot que son nom
// [-l] donner le nom approximatif de l'instance (like '%instance%')
// [-i] donner le nom exact de l'instance

namespace JFG.Ubisoft.Perforce
{
    public static partial class Gii
    {
        private static void Help()
        {
            Console.WriteLine("gii {-p port | {-l |-i} instance}");
            Console.WriteLine("-p port : donner le #port de l'instance plutot que son nom");
            Console.WriteLine("[-l] donner le nom approximatif de l'instance (like '%instance%')");
            Console.WriteLine("[-i] donner le nom exact de l'instance");
            Environment.Exit(0);
        }

        private static bool ParseCommandLine(string[] args, ref int port, ref string instance)
        {
            bool bRet = true;
            int n = 0;
            ArrayList alVer = new ArrayList();
            alVer.Clear();

            while (n < args.Length && bRet)
            {
                switch (args[n])
                {
                    case "-p":
                        int.TryParse(args[n + 1], out port);
                        if (port != 0 && port < 65536)
                            instance = "NULL";
                        ++n;
                        break;
                    case "-l":
                        instance = "%" + args[n + 1] + "%";
                        port = -1;
                        ++n;
                        break;
                    case "-i":
                        instance = args[n + 1];
                        port = -1;
                        ++n;
                        break;
                    case "-h":
                        Help();
                        bRet = false;
                        break;
                    case "-cl":
                        alVer = ShowChangeLog("JFG.Ubisoft.Perforce.Resources.CHANGELOG.txt");
                        Console.Clear();
                        if (alVer.Count == 0)
                            GetVersion();
                        else
                            DisplayChangeLog(alVer);
                        bRet = false;
                        break;
                    case "-v":
                        Console.Clear();
                        Console.WriteLine(Assembly.GetExecutingAssembly().GetName() + ". Version actuelle = v"+Assembly.GetExecutingAssembly().GetName().Version);
                        Environment.Exit(-1);
                        break;
                    default:
                        ++n;
                        break;
                }
            }
            return bRet;
        }

        private static void GetVersion()
        {
            Console.WriteLine("gii v" + Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("(impossible d'obtenir le changelog)");
        }

        private static void DisplayChangeLog(ArrayList alv)
        {
            for (int i = 0; i < alv.Count;i++)
                Console.WriteLine((string)alv[i]);
        }

        private static ArrayList ShowChangeLog(string res)
        {
            StreamReader sr = null;
            ArrayList alVersions = new ArrayList();
            
            try
            {
                Assembly _assembly = Assembly.GetExecutingAssembly();
                sr = new StreamReader(_assembly.GetManifestResourceStream(res));
                string sLine;

                while ((sLine = sr.ReadLine()) != null)
                    alVersions.Add(sLine);
            }
            catch (Exception)
            {
                alVersions.Clear();
            }

            finally
            {
                if (sr != null) sr.Close();
            }

            return alVersions;
        }
    }
}