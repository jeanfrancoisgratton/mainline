/* dsFx : Directory Server Functions
 * v1.02.00
 * Écrit par J.F.Gratton © 2011-2012
 * Projet: dsFx
 * Solution: uOne
 * Fichier: ldsFunctions.cs
 *      Fichier principal, déclaration de la classe principale de la librairie
*/

using System;
using System.Collections;
using System.DirectoryServices.Protocols;
using System.Net;

namespace JFG.ldap.dsFx
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
	public struct LdapReturnStruct
	{
	    public string dn { get; set; }

	    public ArrayList alAttributs { get; set; }
	};

	public partial class LdapFunctions : IDisposable
	{
		private string _host, _baseDN;
		private int _port;
		private LdapConnection ldc;

        //+ ldapFunctions
		// Constructeur acceptant comme paramètres le nom du serveur, son port, le baseDN, et optionnellement username+password
		public LdapFunctions(string server, int port, string dn, string user, string password)
		{
			_host = server;
			_port = port;
			_baseDN = dn;
			NetworkCredential cred = new NetworkCredential(user, password);

			ldc = new LdapConnection(_host + ":" + _port);    // PROD via SSH);
			ldc.Credential = cred;
			ldc.AuthType = AuthType.Basic;
			ldc.SessionOptions.ProtocolVersion = 3;
		}

		//+ ldapFunctions
		// Constructeur acceptant comme paramètres le nom du serveur, son port, et le baseDN
		public LdapFunctions(string server, int port, string dn)
		{
			_host = server;
			_port = port;
			_baseDN = dn;
			ldc = new LdapConnection(_host + ":" + _port);    // PROD via SSH);
			ldc.Credential = null;
			ldc.AuthType = AuthType.Anonymous;
			ldc.SessionOptions.ProtocolVersion = 3;
		}

		//+ Dispose():
		// Permet de fermer la connexion LDAP
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
				if (ldc != null)
				{
					ldc.Dispose();
					ldc = null;
				}
			}
		}
	}
}