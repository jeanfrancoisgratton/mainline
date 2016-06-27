/* Solution : PuppetP4
 * Projet : CreateUser
 * File : createUser.cs . main file
*/

using System;

namespace JFG.BLC.puppet
{
	partial class UserAdd
	{
		public static void Main (string[] args)
		{
			string username, sshkey, sshkeyfile, directory;
			Console.Write ("Entrez le username: ");
			username = Console.ReadLine ();
			Console.WriteLine ("[1] : Sysadmin CGI/BLC\t[2]: B2B Adm");
			Console.WriteLine ("[3]: B2B Dev\t[4]: B2B Autre");
			Console.WriteLine ("[5]: DBA\t[6]: Autre");
			Console.Write ("Entrez le type de compte (1-6) : ");
			while (true)
			{
				ConsoleKeyInfo result = Console.ReadKey();
				if ((result.KeyChar >= '1') || (result.KeyChar < '7'))
				{
					directory = fetchGroupDirectory (result.KeyChar);
					break;
				}
			}

			Console.Write ("Indiquez le chemin + nom de la cle SSH, ENTER si aucune: ");
			sshkeyfile = Console.ReadLine ();
			if (!String.IsNullOrWhiteSpace (sshkeyfile))
				sshkey = fetchSSHkey ();

			GeneratePPfile (username, sshkey, directory);

			Console.WriteLine ("Complet. N'oubliez pas de créer un password pour l'usager à l'aide de genPass.");
			Environment.Exit (0);
		}

		private static string fetchGroupDirectory (char groupe)
		{
			switch (groupe)
			{
			case '1':
				fetchGroupDirectory = "sysadmins";
				break;
			case '2':
				fetchGroupDirectory = "b2badm";
				break;
			case '3':
				fetchGroupDirectory="b2bdev";
				break;
			case '4':
				fetchGroupDirectory="b2bother";
				break;
			case '5':
				fetchGroupDirectory = "dba";
				break;
			case '6':
				fetchGroupDirectory = "other_users";
				break;
			}
			return fetchGroupDirectory;
		}
	}
}
