/* Solution : PuppetP4
 * Projet : ppParser
 * File : fileio.cs : file i-o methods
*/
using System;
using System.IO;

namespace JFG.BLC.puppet
{
	partial class PPParser
	{
		private static string ReadCommentFromFile (string fichier)
		{
            string ligne, comment = "";

            try
            {
                using (StreamReader sr = new StreamReader (fichier))
                {
                    ligne = sr.ReadLine();
                    if (ligne.ToLower().Contains("comment") && ligne.Contains ("=>"))
                    	comment = ligne;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
				Environment.Exit(2);
            }

            return comment;
        }
		
		private static void GenPP(string usager, string fichier, string commentaire, string hashedpwd)
		{
			try
			{
				// Compose a string that consists of three lines.
				// string lines = "First line.\r\nSecond line.\r\nThird line.";

				// Write the string to a file.
				StreamWriter sw = new StreamWriter (fichier, false);
				sw.WriteLine("# Usager: " + usager +" (" + commentaire + ") DEBUG");
				sw.WriteLine("@user\t{");
				sw.WriteLine("\t\t'{0}' :",usager);
				sw.WriteLine("\t\tensure => present,");
				if (!String.IsNullOrWhiteSpace(commentaire))
					sw.WriteLine("\t\t{0},", commentaire.Trim());
				sw.WriteLine("\t\tmanagehome => true,");
				sw.WriteLine("\t\tshell => '/bin/bash',");
				sw.WriteLine("\t\tpassword => '{0}',", hashedpwd);
				sw.WriteLine("\t}");
				
				sw.Close();
			}
			
			catch (Exception e)
            {
                Console.WriteLine(e.Message);
				Environment.Exit(2);
            }
		}

		/// <summary>
		/// ReadPasswordFile
		/// </summary>
		/// <param name="usager"> Nom de l'usager</param>
		/// <returns> Le password encrypté</returns>
		/// <exception cref="Exception"></exception>
		private static string ReadPasswordFile (string userfile)
		{
			string hashedPwd = "";

			try
			{
				using (StreamReader sr = new StreamReader (userfile))
				{
					hashedPwd = sr.ReadLine();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Environment.Exit(2);
			}

			return hashedPwd;
		}
	}
}