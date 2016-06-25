// perforceDB : gis
// dataHandling.cs : fetch and display results
// 2013.03.08.15:20, Jean-Francois Gratton


using System;
using System.Collections;
using MySql.Data.MySqlClient;
using JFG.Ubisoft.Perforce;


// DML :   SELECT name, port, version, owner
// OTHER : SELECT name, alias, port, server, version, owner, restore_date 
namespace JFG.Ubisoft.Perforce
{
    public partial class gis
    {
        private static ArrayList FetchData (string selectClause)
        {
            var alRS = new ArrayList();
            //var _conn = new MySqlConnection("User Id=p4utils_read;Password=perforceUtils;Host=p4db.ubisoft.org;Database=perforce_web;");
            var _conn = new MySqlConnection(_connectionString);

            bool bDMLfetch = selectClause.StartsWith("SELECT name, port, version, owner");
            
            bool bRet = true;
            var rs = new ResultatStruct();
            var cmd = new MySqlCommand(selectClause, _conn);
            MySqlDataReader rdr = null;

            alRS.Clear();

            try
            {
                _conn.Open();
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (bDMLfetch)
                    {
                        rs = perfUtils.ClearRS();
                        rs.Name = rdr[0].ToString();
                        if (Int32.TryParse(rdr[1].ToString(), out rs.Port) == false)
                            rs.Port = 1666;
                        rs.Version = rdr[2].ToString();
                        rs.Owner = rdr[3].ToString();
                    }
                    else
                    {
                        rs.Name = rdr[0].ToString();
                        rs.Alias = rdr[1].ToString();
                        if (Int32.TryParse(rdr[2].ToString(), out rs.Port) == false)
                            rs.Port = 1666;
                        rs.Server = rdr[3].ToString();
                        rs.Version = rdr[4].ToString();
                        rs.Owner = rdr[5].ToString();
                        rs.Restore_date = rdr[6].ToString();
                    }
                    alRS.Add(rs);
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

            if (bRet == false)
                alRS.Clear();

            return alRS;
        }
        
        private static void ShowDML (ArrayList alRS)
        {
            ResultatStruct RS = new ResultatStruct();
            int i = 0;
            //int page = 1, pages = alRS.Count/15 + 1;
            string s;
            Console.Clear();
            //Console.WriteLine("List data managers. {0} records.    Page {1} of {2}", alRS.Count, page, pages);
            Console.WriteLine("Liste des data managers ({0} records).", alRS.Count);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("+" + perfUtils.PadOutPut("-------------", 32, '-') + "+" + perfUtils.PadOutPut("----", 10, '-') +
                "+" + perfUtils.PadOutPut("-------", 10, '-') + "+" + perfUtils.PadOutPut("------------", 23, '-') + "+");

            s = "| " + perfUtils.PadOutPut("Instance name", 31) + "| ";
            s += perfUtils.PadOutPut("Port", 9) + "| ";
            s += perfUtils.PadOutPut("Version", 9) + "| ";
            s += perfUtils.PadOutPut("Data Manager", 22) + "|";
            Console.WriteLine(s);
            Console.WriteLine("+" + perfUtils.PadOutPut("-------------", 32, '-') + "+" + perfUtils.PadOutPut("----", 10, '-') +
                "+" + perfUtils.PadOutPut("-------", 10, '-') + "+" + perfUtils.PadOutPut("------------", 23, '-') + "+");

            while (i < alRS.Count)
            {
                RS = (ResultatStruct)alRS[i];
                s = "| " + perfUtils.PadOutPut(RS.Name, 31) + "| ";
                s += perfUtils.PadOutPut(RS.Port.ToString(), 9) + "| ";
                s += perfUtils.PadOutPut(perfUtils.StripVersion(RS.Version), 9) + "| ";
                s += perfUtils.PadOutPut(RS.Owner, 22) + "|";
                Console.WriteLine(s);
                ++i;
            }
            Console.WriteLine("+" + perfUtils.PadOutPut("-------------", 32, '-') + "+" + perfUtils.PadOutPut("----", 10, '-') +
                "+" + perfUtils.PadOutPut("-------", 10, '-') + "+" + perfUtils.PadOutPut("------------", 23, '-') + "+");
        }

        // OTHER : SELECT name, alias, port, server, version, owner, restore_date 
        private static void ShowFullInfo (ArrayList alRS)
        {
            ResultatStruct RS = new ResultatStruct();
            for (int i = 0; i < alRS.Count; i++)
            {
                RS = (ResultatStruct) alRS[i];
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Row #{0}", i + 1);
                Console.WriteLine("Instance (alias) : {0} ({1})", RS.Name, RS.Alias);
                Console.WriteLine("Port             : {0}", RS.Port);
                Console.WriteLine("Server           : {0}", RS.Server);
                Console.WriteLine("Version          : {0}", RS.Version);
                Console.WriteLine("Owner            : {0}", RS.Owner);
                Console.WriteLine("Restore date     : {0}", RS.Restore_date);
            }
        }
    }
}