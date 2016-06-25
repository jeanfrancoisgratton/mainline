/* DockerUtils : switchDocker
 * switchDocker : Utility to fetch the proper parameters to pass to docker to launch a specific container
 * (c) 2015, J.F.Gratton (jean-francois.gratton@videotron.ca, jean-francois.gratton@fxinnovation.com)
 * 
 * switchdocker.cs : entry point
*/
using System;

// switchDocker [-f configFile] {-l | -a | -e | -r containerID }

namespace JFG.Docker.Utils
{
    partial class switchDocker
    {
        public static void Main (string[] args)
        {
            string configFile;
            if (ParseCommandLine (args, ref configFile) == false)
                Help ();
            Console.WriteLine ("Hello World!");
        }





    }
}
