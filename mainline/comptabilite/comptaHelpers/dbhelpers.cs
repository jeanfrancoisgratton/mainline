// comptabilite : comptaHelpers
// Écrit par : J.F.Gratton, 2013.01.01 12:10
// dbhelpers.cs : helpers BD

using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace JFG.Comptes.Helpers
{
    //DBInfoStruct: database creds, info, etc
    public struct DBInfoStruct
    {
        public string DbisAdmin, DbisAdminPasswd;
        public string DbisDB;
        public string DbisHost;
        public string DbisPasswd;
        public int DbisPort;
        public string DbisUser;
    }

// ReSharper disable ClassNeverInstantiated.Global
    public partial class DBHelpers
// ReSharper restore ClassNeverInstantiated.Global
    {
        // getGID() : Trouve le gid à partir du gname
        public static string GetGID(MySqlConnection mconn, string g)
        {
            string id = "";
            var cmd = new MySqlCommand("SELECT gID FROM _groupes WHERE gName='" + g + "'", mconn);

            MySqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    id = rdr[0].ToString();
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }

            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Erreur");
            }
            finally
            {
                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            return id;
        }
    }
}