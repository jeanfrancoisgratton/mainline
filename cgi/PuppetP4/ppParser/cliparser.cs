/* Solution : PuppetP4
 * Projet : ppParser
 * File : cliparser.cs . Parses the command line
*/

using System;

namespace JFG.BLC.puppet
{
	partial class PPParser
	{

		public static bool ParseCLI (string[] a, ref string usager, ref string fichier, ref string passwdfile)
		{
			int n = 0;
			bool bRet = true;
			usager = fichier = "";

			while (n < a.Length)
			{
				switch (a [n])
				{
					case "-f":
					case "--fichier":
					case "--file":
						if (n + 1 < a.Length)
							fichier = a [++n];
						else
							bRet = false;
						++n;
						break;
					case "-u":
					case "--usager":
					case "--user":
						if (n + 1 < a.Length)
							usager = a [++n];
						else
							bRet = false;
						++n;
						break;
					case "-w":
						if (n + 1 < a.Length)
							passwdfile = a [++n];
						break;
					default:
						++n;
						break;
				}
			}
			return bRet;
		}
		public static void ShowUsage()
		{
			Console.WriteLine("Vous devez entrer le nom de l'usager --usager USAGER) et le chemin (--fichier FICHIER)");
			Console.WriteLine("You must enter the username (--user USER) and the filepath (--file FILE)");
		}
	}
}