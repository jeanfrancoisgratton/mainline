/* DockerUtils : switchDocker
 * containerhelper : Helper lib to add, remove, edit container specs
 * (c) 2015, J.F.Gratton (jean-francois.gratton@videotron.ca, jean-francois.gratton@fxinnovation.com)
 * 
 * containerhelper.cs : entry point
*/

using System;

namespace JFG.Docker.Utils
{
    public class ContainerHelper
    {
        /// <summary> Connection information </summary>
        public struct ConnectionStruct
        {
            public string DB;
            public string Password;
            public int Port;
            public string Server;
            public string Username;
        };

        /// <summary> Main constructor </summary>
        public ContainerHelper()
        {
        }
    }
}

