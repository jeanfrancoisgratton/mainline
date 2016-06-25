// perforceDB : gis
// commandLineParser.cs : self-explanatory :)
// 2013.02.28.09:58, Jean-François Gratton

using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Text;
//using JFG.SysUtils.CryptoLib;

//gis [-x xmlfile] [{-l like | -p port | -i instance | -ldm}] [-s server] | [-v version] [-o owner]
//bCommands = like, instance, port, ldm, server, version, owner

namespace JFG.Ubisoft.Perforce
{
    partial class gis
    {
        private static string ParseCommandLine (string[]args)
        {
            var alVer = new ArrayList();
            var bSingleArgument = false;
            var n = 0;
            var sSelectClause = "";

            alVer.Clear();
            while (n < args.Length && bSingleArgument == false)
            {
                switch (args[n].ToLower())
                {
                    case "-w":
                        CreateConnectionFile();
                        break;
                    case "-x":
                        _sXmlFile = args[n + 1];
                        ++n;
                        break;
                    case "-cl":
                        alVer = ShowChangeLog();
                        Console.Clear();
                        if (alVer.Count == 0)
                            GetVersion();
                        else
                            DisplayChangeLog(alVer);
                        bSingleArgument = true;
                        break;
                    case "-dml":
                        bSingleArgument = true;
                        sSelectClause = "SELECT name, port, version, owner FROM p4_instances ORDER BY owner";
                        break;
                    case "-h":
                        Help();
                        break;
                    case "-o":
                        if (n == args.Length - 1)
                            Help();
                        if (String.IsNullOrWhiteSpace(sSelectClause))
                            sSelectClause = @"SELECT name, alias, port, server, version, owner, restore_date FROM p4_instances WHERE owner LIKE '%" + args[n + 1] + "%'";
                        else
                            sSelectClause += " AND owner LIKE '%" + args[n + 1] + "%'";
                        ++n;
                        break;
                    case "-l":
                        if (n == args.Length - 1)
                            Help();
                        if (String.IsNullOrWhiteSpace(sSelectClause))
                            sSelectClause =
                                @"SELECT name, alias, port, server, version, owner, restore_date FROM p4_instances WHERE name LIKE '%" + args[n + 1] + "%'";
                        else
                            sSelectClause += " ANd name LIKE '%" + args[n + 1] + "'";
                        ++n;
                        break;
                    case "-i":
                        if (n == args.Length - 1)
                            Help();
                        if (String.IsNullOrWhiteSpace(sSelectClause))
                            sSelectClause = @"SELECT name, alias, port, server, version, owner, restore_date FROM p4_instances WHERE name ='" + args[n + 1] + "'";
                        else
                            sSelectClause += " AND name ='" + args[n + 1] + "'";
                        ++n;
                        break;
                    case "-p":
                        if (n == args.Length - 1)
                            Help();
                        int p;
                        if (Int32.TryParse(args[n + 1], out p) == false || p < 0 || p > 65535)
                            p = 1666;
                        if (String.IsNullOrWhiteSpace(sSelectClause))
                            sSelectClause = @"SELECT name, alias, port, server, version, owner, restore_date FROM p4_instances WHERE port =" + p;
                        else
                            sSelectClause += " AND port =" + p;
                        ++n;
                        break;
                    case "-s":
                        if (n == args.Length - 1)
                            Help();
                        if (String.IsNullOrWhiteSpace(sSelectClause))
                            sSelectClause = @"SELECT name, alias, port, server, version, owner, restore_date FROM p4_instances WHERE server LIKE '%" + args[n + 1] + "%'";
                        else
                            sSelectClause += " AND server LIKE '%" + args[n + 1] + "%'";
                        ++n;
                        break;
                    case "-v":
                        if (n == args.Length - 1)
                            Help();
                        if (String.IsNullOrWhiteSpace(sSelectClause))
                            sSelectClause = @"SELECT name, alias, port, server, version, owner, restore_date FROM p4_instances WHERE version LIKE '%" + args[n + 1] + "%'";
                        else
                            sSelectClause += " AND version LIKE '%" + args[n + 1] + "%'";
                        ++n;
                        break;
                    default:
                        ++n;
                        break;
                }
            }
            return sSelectClause;
        }

        private static void CreateConnectionFile()
        {
            DBConnectionStruct dbcs = new DBConnectionStruct();
            string s, xmlFile;
            //XMLfileHandler xmlFH = new XMLfileHandler();

            //Console.Clear();
            Console.WriteLine("Global instances stats v"+Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine(Environment.NewLine);
            Console.Write("Nom du fichier: ");
            xmlFile = Console.ReadLine();
            Console.Write("Serveur: ");
            dbcs.Server = Console.ReadLine();
            Console.Write("Port: ");
            s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s) || Int32.TryParse(s, out dbcs.Port) == false)
                dbcs.Port = 3306;
            Console.Write("Username: ");
            dbcs.Username = Console.ReadLine();
            Console.Write("Password: ");
            dbcs.Password = ReadPasswd();
            Console.Write("Database: ");
            dbcs.Database = Console.ReadLine();
            
            Console.WriteLine(Environment.NewLine);
            //s = XMLfileHandler.writeXMLFile(xmlFile, dbcs);
            if (string.IsNullOrWhiteSpace(s))
                Console.WriteLine("Fichier {0} enregistre", xmlFile);
            else
                Console.WriteLine("Erreur a l'ecriture de {0}: {1}", xmlFile, s);

            Environment.Exit(0);
        }

        private static string ReadPasswd()
        {
            StringBuilder pwd = new StringBuilder();
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            // Prevent example from ending if CTL+C is pressed.
            //Console.TreatControlCAsInput = true;

            do
            {
                cki = Console.ReadKey(true);
                if (cki.Key != ConsoleKey.Enter)
                    pwd.Append(cki.KeyChar);
            } while (cki.Key != ConsoleKey.Enter);

            return pwd.ToString();
        }
    }
}