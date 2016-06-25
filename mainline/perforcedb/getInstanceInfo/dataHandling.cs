// perforceDB : getInstanceInfo
// dataHandling.cs : select + print data
// 2013.02.01.11:37, Jean-Francois Gratton


using System;
using System.Collections;
using MySql.Data.MySqlClient;


// SELECT name,alias,port,server,version,owner FROM p4_instances WHERE name LIKE %NAME%
namespace JFG.Ubisoft.Perforce
{
    public static partial class Gii
    {
        private static bool GoFetchData(string sSelect, ref ArrayList alResultats)
        {
            bool bRet = true;
            var rs = new ResultatStruct();
            var cmd = new MySqlCommand(sSelect, _conn);
            MySqlDataReader rdr = null;

            alResultats.Clear();
            
            try
            {
                _conn.Open();
                rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    rs.Name = rdr[0].ToString();
                    rs.Alias = rdr[1].ToString();
                    if (Int32.TryParse(rdr[2].ToString(), out rs.Port) == false)
                        rs.Port = 1666;
                    rs.Server = rdr[3].ToString();
                    rs.Version = rdr[4].ToString();
                    rs.Owner = rdr[5].ToString();
                    alResultats.Add(rs);
                }
            }

            catch (MySqlException mex)
            {
                Console.WriteLine("Erreur SQL:");
                Console.WriteLine("-----------");
                Console.WriteLine(mex.Message);
                bRet = false;
            }

            catch (Exception x)
            {
                Console.WriteLine("Erreur:");
                Console.WriteLine("-------");
                Console.WriteLine(x.Message);
                bRet = false;
            }
            finally
            {
                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                    rdr.Dispose();
                }
            }

            if (alResultats.Count == 0)
                bRet = false;
            return bRet;
        }

        private static void GoPrintData(ArrayList alResultats)
        {
            var rs = new ResultatStruct();
            for (int i = 0; i < alResultats.Count;i++)
            {
                rs = (ResultatStruct)alResultats[i];
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Row {0}", i+1);
                Console.WriteLine("Instance (alias) : {0} ({1})", rs.Name, rs.Alias);
                Console.WriteLine("Port             : {0}", rs.Port);
                Console.WriteLine("Serveur          : {0}", rs.Server.Replace(".ubisoft.org", ""));
                Console.WriteLine("Version          : {0}", rs.Version);
                Console.WriteLine("Owner            : {0}", rs.Owner);
            }
        }
    }
}