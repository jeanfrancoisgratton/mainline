/* dsFx : Directory Server Functions
 * v1.02.00
 * Écrit par J.F.Gratton © 2011-2012
 * Projet: dsFx
 * Solution: uOne
 * Fichier: ldapadd.cs
 *      Fonction d'ajout(ldapmodify) du LDAP
*/

using System;
using System.DirectoryServices.Protocols;

//using System.IO;

namespace JFG.ldap.dsFx
{
	public partial class LdapFunctions : IDisposable
	{
		//! ldapAdd
		// Effectue le ldapadd (attribut à une seule valeur)
		// In: fichier LDIF contenant la ou les entrées à ajouter
		// Out : Le code de retour dans la réponse du LDAP
		public ResultCode ldapAdd(string ldif)
		{
			/*
			FileStream rs = new FileStream(ldif, FileMode.Open, FileAccess.Read);
			StreamReader r = new StreamReader(rs);
			*/
			throw (new NotImplementedException("JFG.ldap.dsFx.LdapFunctions.ldapAdd not implemented... yet :-)"));
		}
	}
}