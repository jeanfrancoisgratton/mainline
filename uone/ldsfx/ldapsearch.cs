/* dsFx : Directory Server Functions
 * v1.02.00
 * Écrit par J.F.Gratton © 2011-2012
 * Projet: dsFx
 * Solution: uOne
 * Fichier: ldapSearch.cs
 *      Fonction de recherche (ldapsearch) du LDAP
*/

using System;
using System.Collections;
using System.DirectoryServices.Protocols;

namespace JFG.ldap.dsFx
{
	public partial class LdapFunctions : IDisposable
	{
		//! ldapSearch
		// Effectue le ldapsearch
		// In: le filtre (telephonenumber=5146999820), les attributs à lister (ummwienabled, mailforwardingaddress, etc)
		// Out: Un ArrayList de struc LdapReturnStruct contenant le dn et les attributs demandés (nom + valeur(s)) en paramètre
		public ArrayList ldapSearch(string filter, string[] attributs)
		{
			LdapReturnStruct ldapReturn = new LdapReturnStruct();
			ArrayList ldapresults = new ArrayList();
			//ldapReturn.alAttributs.Capacity = attributs.Length;

			SearchRequest searchRequest = new SearchRequest(_baseDN, filter, SearchScope.Subtree, attributs);
			SearchResponse searchResponse = (SearchResponse)ldc.SendRequest(searchRequest);

			if (searchResponse != null)
			{
				foreach (SearchResultEntry entry in searchResponse.Entries)
				{
					ldapReturn.dn = entry.DistinguishedName;
					ldapReturn.alAttributs = new ArrayList();
					SearchResultAttributeCollection attributeCollection = entry.Attributes;
					if (attributeCollection.Values != null)
					{
						foreach (DirectoryAttribute attribute in attributeCollection.Values)
						{
							string[] attribs = new string[attribute.Count + 1];
							attribs[0] = attribute.Name;
							for (int i = 0; i < attribute.Count; i++)
								attribs[i + 1] = attribute[i].ToString();
							ldapReturn.alAttributs.Add(attribs);
						}
						ldapReturn.alAttributs.TrimToSize();
					}
					ldapresults.Add(ldapReturn);
				}
			}
			return ldapresults;
		}
	}
}