/* Solution : PuppetP4
 * Projet : ppParser
 * File : ppParser.cs . Parses the .pp file (main file)
*/

using System;
using System.IO;

/*
 * ppParser : ./ppParser --user USAGER --file FICHIER.PP
*/


namespace JFG.BLC.puppet
{
	partial class PPParser
	{
		/// <summary>
		/// Main entry point
		/// </summary>
		/// <param name="table"> Table to get the count</param>
		/// <param name="db"> Database containing the table</param>
		/// <returns> The number of rows (int) on success, -1 on failure (check _mExceptionString for the error message)</returns>
		/// <exception cref="MySqlException"></exception>
		public static int Main (string[] args)
		{
			string usager, fichier, commentaire, hashedpwd, passwdfile;
			int nRet = 0;

			usager = fichier = passwdfile = "";
			
			if (ParseCLI (args, ref usager, ref fichier, ref passwdfile) == false || String.IsNullOrWhiteSpace(usager) || String.IsNullOrWhiteSpace(fichier))
			{
				ShowUsage();
				Environment.Exit (2);
			}

			// 1. get new hashedpassword from /var/users/$USER.user if passwdfile is empty
			if (String.IsNullOrWhiteSpace (passwdfile))
				hashedpwd = ReadPasswordFile ("/var/users/" + usager + "/.user");
			else
				hashedpwd = ReadPasswordFile(passwdfile);
			// 2. get comment attribute from current .pp file
			commentaire = ReadCommentFromFile(fichier);
			// 3. build a new .pp file
			GenPP(usager, fichier, commentaire, hashedpwd);
			// 4. we're done
			
			Console.WriteLine("Complet. N'oubliez pas de pousser le fichier {0} vers Perforce !", fichier);

            return nRet;
		}
	}
}