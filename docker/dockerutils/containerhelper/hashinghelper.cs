// CardioPleinAir : isolatedStorageHelpers
// Écrit par : jfgratton (), 2014.10.18 @ 23:29
// 
// hashingHelper.cs : hash functions helper
// COPIED TO //docker//dockerutils/containerhelper/... le 7 janvier 2015

using System;
using System.Security.Cryptography;
using System.Text;

//namespace JFG.Cardio
namespace JFG.Docker.Utils
{
    public struct ConnectionStruct
    {
        public string DB;
        public string Password;
        public int Port;
        public string Server;
        public string Username;
    };

    public partial class StorageHelpers : IDisposable
    {

        public string CalculateHashForPassword(string pwd)
        {
            HashAlgorithm ha = HashAlgorithm.Create("SHA512Managed");
            if (ha != null)
            {
                byte[] hash = ha.ComputeHash(Encoding.Default.GetBytes(pwd));

                return BitConverter.ToString(hash);
            }

            return pwd; // seulement si ha == null
        }

        // ComparePasswordHashes()
        //! Please note :
        // it's impossible to deduce a password from its hash
        // What we do here is compare the password hash (parameter) with the one password hash from the database
        // returns true if both are identical, false otherwise.
        public bool ComparePasswordHashes(string hashedPwd, ConnectionStruct connInfo)
        {
            //TODO: Connect to DB, fetch the password, return it in some string var
            //MySqlConnection conn = new MySqlConnection()
            string dbPwd = "";

            return String.Compare(hashedPwd, dbPwd) == 0;
        }
    }
}