
using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace JFG.Ubisoft.Perforce
{
	partial class gis
	{
		private static void GetVersion()
		{
			Console.WriteLine("gis v" + Assembly.GetExecutingAssembly().GetName().Version);
			Console.WriteLine("(impossible d'obtenir le changelog)");
		}

		private static void DisplayChangeLog(ArrayList alv)
		{
			for (int i = 0; i < alv.Count; i++)
				Console.WriteLine((string) alv[i]);
			Environment.Exit(0);
		}

		private static ArrayList ShowChangeLog()
		{
			StreamReader sr = null;
			ArrayList alVersions = new ArrayList();

			try
			{
				Assembly _assembly = Assembly.GetExecutingAssembly();
				sr = new StreamReader(_assembly.GetManifestResourceStream("JFG.Ubisoft.Perforce.Resources.CHANGELOG.txt"));
				string sLine;

				while ((sLine = sr.ReadLine()) != null)
					alVersions.Add(sLine);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				alVersions.Clear();
			}

			finally
			{
				if (sr != null) sr.Close();
			}

			return alVersions;
		}

		private static void Help()
		{
			Console.Clear();
			Console.WriteLine("gis [{-i instance |-l instance | -p port number | -dml}] [-v version] [-s server] [-o owner]");
			Console.WriteLine("-i instance: nom exact de l'instance");
			Console.WriteLine("-l instance: nom incomplet de l'instance");
			Console.WriteLine("-p port: port de l'instance");
			Console.WriteLine("-dml : liste des data managers (owners)");
			Console.Write(Environment.NewLine);
			Console.WriteLine("-v version : Liste des instances de cette version");
			Console.WriteLine("-s server : Liste des instances sur ce serveur");
			Console.WriteLine("-o owner : Liste des instances sous cet owner");
		}
	}
}