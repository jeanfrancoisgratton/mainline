namespace;/* DockerUtils : switchDocker
 * switchDocker : Utility to fetch the proper parameters to pass to docker to launch a specific container
 * (c) 2015, J.F.Gratton (jean-francois.gratton@videotron.ca, jean-francois.gratton@fxinnovation.com)
 * 
 * cliHelper.cs : argument parser
*/

using System;

// switchDocker [-f configFile] {-l | -a | -e | -r containerID }

namespace JFG.Docker.Utils
{
    partial class switchDocker
    {
        public static void Help()
        {
            Environment.Exit (0);
        }

        public static bool ParseCommandLine (string []args, ref string cfgFile)
        {
            bool bRes = true, bEoL = false;
            int n = 0;
            cfgFile = "";

            while (n < args.Length)
            {
                switch (args[n].ToLower())
                {
                    case "-f":
                        if (n == args.Length -1)
                            Help();
                        else
                        {
                            ReadConfigFile (args[n+1]);
                            n+=2;
                        }
                        break;
                    case "-l":
                        ListDockerInstances();
                        ++n;
                        break;
                    case "-a":
                        AddDockerInstance();
                        break;
                    case "-e":
                        EditDockerInstance();
                        break;
                    case "-r":
                        RemoveDockerInstance(args[n + 1]);
                        n+=2;
                        break;
                    default:
                        ++n;
                        break;
                }
            }
        }
    }
}