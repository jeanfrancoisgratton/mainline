// uOne : consoleInfoBV
// output.cs
// 
// Écrit par J.F. Gratton, 2012.03.02

// AFFICHAGE DES RESULTATS

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security;
using JFG.ldap.dsFx;

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
        //+ AfficheResultats():
        // Shows ldapsearch's output to console
        private static void AfficheResultats(ArrayList resultats)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("ldapsearch -h {0} -p {1} -b {2} msisdn={3} ", _host, _port, _basedn, _msisdn);
            if (_attributs != null) Console.WriteLine(String.Join(" ", _attributs));
            Console.WriteLine();

            // Première loupe : on énumère toutes les entrées trouvées
            for (int i = 0; i < resultats.Count; i++)
            {
                LdapReturnStruct ldr = (LdapReturnStruct) resultats[i];
                Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("dn: {0}", ldr.dn);
                Console.BackgroundColor = ConsoleColor.Black;
                // Seconde loupe : tout les attributs trouvés pour l'entrée
                for (int j = 0; j < ldr.alAttributs.Count; j++)
                {
                    string[] k = (string[]) ldr.alAttributs[j];
                    // Dernière loupe: toutes les valeurs de cet attribut
                    for (int m = 1; m < k.Length; m++)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("{0}: ", k[0]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("{0}", k[m]);
                    }
                }
                Console.WriteLine();
            }
        }

        //+ SauvegardeResultats():
        // Sends ldapsearch's output to file
        private static void SauvegardeResultats(ArrayList resultats)
        {
            try
            {

                FileStream fs = new FileStream(_outFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter sw = new StreamWriter(fs);
                // Première loupe : on énumère toutes les entrées trouvées
                for (int i = 0; i < resultats.Count; i++)
                {
                    string s;
                    LdapReturnStruct ldr = (LdapReturnStruct) resultats[i];
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    sw.WriteLine(String.Format("dn: {0}", ldr.dn));

                    // Seconde loupe : tout les attributs trouvés pour l'entrée
                    for (int j = 0; j < ldr.alAttributs.Count; j++)
                    {
                        string[] k = (string[]) ldr.alAttributs[j];
                        // Dernière loupe: toutes les valeurs de cet attribut
                        for (int m = 1; m < k.Length; m++)
                            sw.WriteLine("{0}: {1}", k[0], k[m]);
                    }
                    Console.WriteLine();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}