/* dsFx : Directory Server Functions
 * v1.02.00
 * Écrit par J.F.Gratton © 2011-2012
 * Projet: dsFx
 * Solution: uOne
 * Fichier: ldapdelete.cs
 *      Fonction de suppression (ldapdelete) d'une entrée
*/

using System;
using System.DirectoryServices.Protocols;

namespace JFG.ldap.dsFx
{
	public partial class LdapFunctions : IDisposable
	{
		//! ldapDelete
		// Effectue le ldapdelete
		// In: dn à effacer
		// Out : Le code de retour dans la réponse du LDAP
		public ResultCode ldapDelete(string dn)
		{
			DeleteRequest delRequest = new DeleteRequest(dn);
			DeleteResponse delResponse = (DeleteResponse)ldc.SendRequest(delRequest);

			return delResponse.ResultCode;
		}
	}
}