// uOne : dsFx
// Écrit par J.F. Gratton, 2012.03.28
// 
// statics.cs : helper functions, fonctions utilisables statiquement, etc

using System;
using System.DirectoryServices.AccountManagement;

namespace JFG.ldap.dsFx
{
    public partial class LdapFunctions : IDisposable
    {
        public static bool CheckAuth(string serveur, int port, string baseDN, string uid, string userpassword, ref string exceptionMessage)
        {
            bool loginAllowed;
            PrincipalContext pc = null;
            string srvString = String.Format("{0}:{1}", serveur, port);
            
            try
            {
                pc = new PrincipalContext(ContextType.ApplicationDirectory, srvString, baseDN,
                    "cn=directory manager", "jiefgadm");
                
            }
            catch (Exception ex)
            {
                loginAllowed = false;
                exceptionMessage = ex.Message;
            }

            try
            {
                loginAllowed = pc.ValidateCredentials(uid, userpassword);
            }
            catch (Exception ex)
            {
                loginAllowed = false;
                exceptionMessage = ex.Message;
            }
            
            
            return loginAllowed;
        }
    }
}


/*
//Search for user
                   DirectoryEntry deSystem = new DirectoryEntry("LDAP://" + strServerName + "/" + strUserDN + "," 
                                                       + strBaseDN);
                   deSystem.AuthenticationType=AuthenticationTypes.Secure;
                   deSystem.Username=txtUserName.Text;
                   deSystem.Password =txtPassword.Text;
                   //Search for account name
                   string strSearch=strAccountFilter + "=" + txtUserName.Text;
                   DirectorySearcher dsSystem = new DirectorySearcher(deSystem,strSearch);
                   //Search subtree of UserDN
                   dsSystem.SearchScope= SearchScope.Subtree;
                   //Find the user data
                   SearchResult srSystem = dsSystem.FindOne();
*/