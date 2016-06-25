// uOne : consoleInfoBV
// conInfoBV.cs
// 
// Écrit par J.F. Gratton, 2012.03.02

using System;
using System.DirectoryServices.Protocols;
using System.Collections;
using JFG.ldap.dsFx;
using uOne.consoleInfoBV.Properties;

/* ARGS:
 * -h host
 * -p port (default = 389)
 * -o output file
 * {-m msisdn|-M msisdn_file}
 * -a attrib_list (string[])
*/

namespace uOne.consoleInfoBV
{
    internal partial class conInfoBV
    {
        private static LdapFunctions _ldf;
        private static string _msisdn, _msisdnFile;
        private static string[] _attributs;
        private static string _basedn = "";
        private static string _host = "";
        private static int _port = 389;
        private static string _outFile = null;

        private static void Main(string[] args)
        {
            if (parseCommandLine(args) == false)
                help();

            try
            {
                _ldf = new LdapFunctions(_host, _port, _basedn);
                ArrayList ldapResults = _ldf.ldapSearch(_msisdn, _attributs);
                if (ldapResults == null || ldapResults.Count == 0)
                    Console.WriteLine(Resources.main_AucunResultat);
                else
                {
                    if (String.IsNullOrWhiteSpace(_outFile))
                        AfficheResultats(ldapResults);
                    else
                        SauvegardeResultats(ldapResults);
                }
            }
            catch (DirectoryException dEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Resources.main_DirectoryException);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(dEx.Message);
            }
        }
    }
}