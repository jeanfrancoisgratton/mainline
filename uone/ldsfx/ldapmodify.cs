/* dsFx : Directory Server Functions
 * v1.02.00
 * Écrit par J.F.Gratton © 2011-2012
 * Projet: dsFx
 * Solution: uOne
 * Fichier: ldapmodify.cs
 *      Fonction de modification (ldapmodify) du LDAP
*/

using System;
using System.DirectoryServices.Protocols;

namespace JFG.ldap.dsFx
{
    public partial class LdapFunctions : IDisposable
    {
        //! ldapModify v1
        // Effectue le ldapmodify (attribut à une seule valeur)
        // In: le dn où effectuer la modif, le nom de l'attribut, nouvelle valeur de l'attribut
        // Out : Le code de retour dans la réponse du LDAP
        public ResultCode ldapModify(string dn, string nomAttribut, string valeurAttribut)
        {
            ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                nomAttribut, valeurAttribut);
            //modRequest.Controls.Add(new PermissiveModifyControl());
            ModifyResponse modResponse = (ModifyResponse) ldc.SendRequest(modRequest,
                new TimeSpan(0,0,30));  // 30secs delay before timing-out

            return modResponse.ResultCode;
        }

        //! ldapModify v2
        // Effectue le ldapmodify (attribut à plusieurs valeurs)
        // In: le dn où effectuer la modif, le nom de l'attribut, nouvelle valeur de l'attribut
        // Out : Le code de retour dans la réponse du LDAP
        public ResultCode ldapModify(string dn, string nomAttribut, string[] valeursAttribut)
        {
            ModifyRequest modRequest = new ModifyRequest(dn, DirectoryAttributeOperation.Replace,
                nomAttribut, valeursAttribut);
            //modRequest.Controls.Add(new PermissiveModifyControl());
            ModifyResponse modResponse = (ModifyResponse)ldc.SendRequest(modRequest,
                new TimeSpan(0, 0, 30));  // 30secs delay before timing-out

            return modResponse.ResultCode;
        }
    }
}