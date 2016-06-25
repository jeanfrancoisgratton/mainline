// uOne : consoleInfoBV
// commandLineParser.cs
// 
// Écrit par J.F. Gratton, 2012.03.02

using System;
using System.IO;
using System.Reflection;
using uOne.consoleInfoBV.Properties;

/* ARGS:
 * {-m msisdn|-M msisdn_file}
 * -a attrib_list (string[])
*/

namespace uOne.consoleInfoBV
{
    internal partial class conInfoBV
    {
        private static bool parseCommandLine(string[] options)
        {
            int nOptions = options.Length, i = 0;
            bool[] bReqs = {false, false, false};
            
            // Cas spéciaux : -about|-version et -help|--help
            if (options[0]== "-about" || options[0] == "-version")
                about();

            if (options[0] == "-help" || options[0] == "--help")
                help();

            // on doit au moins avoir -m / -M suivi d'un msisdn ou un fichier de msisdn
            // on doit aussi avoir -a suivi d'une liste d'attributs (1 ou plusieurs)
            if (nOptions < 4)
                return false;

            while (i < nOptions)
            {
                switch (options[i])
                {
                    case "-o":
                        if (i < nOptions - 1)
                            _outFile = options[i + 1];
                        ++i;
                        break;
                    case "-h":
                        if (i < nOptions - 1)
                            _host = options[i + 1];
                        ++i;
                        break;
                    case "-p":
                        if (i < nOptions - 1)
                        {
                            if (Int32.TryParse(options[i + 1], out _port))
                                if (_port < 1 || _port > 65535)
                                    _port = 389;
                        }
                        ++i;
                        break;
                    case "-m":
                        if (i < nOptions - 1) // au cas où le paramètre aurait été placé en fin de ligne
                        {
                            _msisdn = options[i + 1];
                            if (_msisdn.Contains("."))
                                _msisdn = _msisdn.Replace(".", "");
                            bReqs[0] = true;
                        }
                        ++i;
                        break;
                    case "-M":
                        if (i < nOptions - 1)
                        {
                            _msisdnFile = options[i + 1];
                            bReqs[1] = true;
                        }
                        ++i;
                        break;
                    case "-a":
                        if (i < nOptions - 1)
                        {
                            _attributs = new string[nOptions - i - 1];
                            for (int x = i + 1; x < nOptions; x++)
                                _attributs[x - i - 1] = options[x];
                            bReqs[2] = true;
                        }
                        i = nOptions;
                        break;
                    default:
                        ++i;
                        break;
                }
            }

            if (bReqs[0] && bReqs[1])
                return false;
            return bReqs[2] && (bReqs[0] || bReqs[1]);
        }

        private static void help()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(Resources.consoleInfoBV);
            Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine(Resources.help_Options);
            Console.WriteLine(Resources.help_About);
            Console.WriteLine(Resources.help_Hostname);
            Console.WriteLine(Resources.help_Port);
            Console.WriteLine(Resources.help_OutputFile);
            Console.WriteLine(Resources.help_MSISDN);
            Console.WriteLine(Resources.help_Attributs);
            Console.WriteLine();
            Environment.Exit(0);
        }

        private static void about()
        {
            StreamReader sr = null;
            Console.Clear(); Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Resources.consoleInfoBV +" v" + Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine(Resources.about_EcritPar);
            Console.WriteLine(); Console.ForegroundColor = ConsoleColor.Gray;

            try
            {
                Assembly _a = Assembly.GetExecutingAssembly();
                sr = new StreamReader(_a.GetManifestResourceStream("uOne.consoleInfoBV.Resources.CHANGELOG.txt"));
                //sr = new StreamReader(_a.GetManifestResourceStream("Resources.CHANGELOG.txt"));
                //sr = new StreamReader(_a.GetManifestResourceStream("CHANGELOG.txt"));
                string ligne;

                while ((ligne = sr.ReadLine()) != null)
                {
                    if (ligne.StartsWith("v"))
                        Console.ForegroundColor = ConsoleColor.Gray;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(ligne);
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }

            finally
            {
                if (sr != null) sr.Close();
            }
            Environment.Exit(3);
        }
    }
}