// Comptabilite : comptes
// Écrit par J.F. Gratton, 2012.08.27
// 
// connexion.cs : gestion des paramètres au commandline et de la connexion

using System.Data;
using MySql.Data.MySqlClient;

namespace JFG.Comptes
{
	public partial class Bilan
	{
		// ParseCommandLine():
		//  Permet de changer de BD au commandline
		private DbStruct ParseCommandLine(string[] a)
		{
			DbStruct db;
			//_db.Host = "oslo.famillegratton.net";
			//_db.DB = "compta";
			//_db.Port = 3360;
			db.Host = "localhost";
			db.User = "compta";
			db.Password = "compta";
			db.DB = "comptabilite";
			db.Port = 3306;

			int n = 1;

			while (n < a.Length)
			{
				switch (a[n])
				{
					case "-h":
					case "--host":
						db.Host = a[n + 1];
						++n;
						break;

					case "-P":
					case "--port":
						int.TryParse(a[n + 1], out db.Port);
						++n;
						break;
					case "-p":
					case "--password":
						db.Password = a[n + 1];
						++n;
						break;
					case "-u":
					case "--user":
						db.User = a[n + 1];
						++n;
						break;
					case "-d":
					case "--database":
						db.DB = a[n + 1];
						++n;
						break;
					default:
						++n;
						break;
				}
			}
			return db;
		}

		// TesteConnexion():
		// Vérifie que la connexion est valide et ouverte
		private bool TesteConnexion(DbStruct dbs)
		{
			bool bOpen = true;
			string connectString = @"User Id=" + dbs.User + ";Password=" + dbs.Password + ";Host=" +
				dbs.Host + ";Port=" + dbs.Port + ";Database=" + dbs.DB + ";";

			_mconn = new MySqlConnection(connectString);

			try
			{
				_mconn.Open();
			}
			catch (MySqlException)
			{
				bOpen = false;
			}

			finally
			{
				if (_mconn.State == ConnectionState.Open)
					_mconn.Close();
			}

			return bOpen;
		}
	}
}