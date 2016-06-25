// perforceDB : mpathInv
// Écrit par jfgratton (Jean-François Gratton), 2013.07.06 @ 07:53
// 
// mpathInv.cs : main file

using System;
using System.Collections;
using System.Data;
using System.IO;
using JFG.mySQLhelper;
using MySql.Data.MySqlClient;

// mpathInv { -d working_dir | -f file1[...file2...fileN] }
// mpathInv { -h | -cl }

namespace JFG.Ubisoft.Perforce
{
    public struct PathInfoStruct
    {
        public string Desc, WWID, SAN, Size;
        public int LunID;
        public ArrayList alDisks, alUnreadyDisks;
    };

    partial class mpathInv
    {
        private static ArrayList alFiles;
        private static string sCurrentDir, sWorkingDir;
        private static string SERVEUR;
        private static string _connectString;
        private static MySqlConnection _conn;

        private static void Main(string[] args)
        {
            _connectString = @"User Id=p4user;Password=p4user;Host=localhost;Port=3306;Database=perforce;";
            ConnectionHelper _connHelp;
            sCurrentDir = Directory.GetCurrentDirectory();
            ClParser(args);
            _connHelp = new ConnectionHelper(_connectString);
            try
            {
                _conn = _connHelp.GetConnection();
                Console.WriteLine(FileParser());
                _connHelp.CloseConnection();
                _connHelp.Dispose();
            }
            catch (MySqlException mEx)
            {
                Console.WriteLine(mEx.Message);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _connHelp.CloseConnection();
                    _connHelp.Dispose();
                }
            }
        }

        private static void CommitPathInfoToDB(PathInfoStruct pi)
        {
            ConnectionHelper _connHelp = new ConnectionHelper(_connectString);
            _conn = _connHelp.GetConnection();
            try
            {
                _connHelp.OpenConnection();
                string sRows, sValues, sTable = "INSERT INTO multipath_status";
                
                _connHelp.Execute("DELETE FROM multipath_status WHERE mpsServeur='" + SERVEUR + "'");
                sRows = "(mpsServeur, mpsLunDesc, mpsLunWWID, mpsLunID, mpsLunSAN, mpsLunSize, mpsDisks, mpsUnreadyDisks)";
                sValues = " VALUES ('" + SERVEUR + "', '" + pi.Desc + "', '" + pi.WWID + "', " + pi.LunID + ", '" + pi.SAN +
                    "', " + GetNumeralForSize(pi.Size) + ", '" + pi.alDisks + ", '" + pi.alUnreadyDisks + "')";
                _connHelp.Execute(sTable + sRows + sValues);
            }
            catch (Exception X)
            {
                
            }
        }

        private static int GetNumeralForSize(string sz)
        {
            int nSz, nMultiplier = 1;
            if (sz.EndsWith("T"))
            {
                nMultiplier = 1024;
                sz.Replace("T", "");
            }
            else
                sz.Replace("G", "");

            if (Int32.TryParse(sz, out nSz) == false)
                nMultiplier = 0;
            return nSz*nMultiplier;
        }
    }
}