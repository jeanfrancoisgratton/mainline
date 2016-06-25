// perforceDB : getInstanceInfo
// gii.cs : main entry file
// 2013.01.31.10:56, Jean-Francois Gratton

using System;
using System.Collections;
using MySql.Data.MySqlClient;

// Usage : gii {-p port | {-l | -i} instance}
// -p port : donner le #port de l'instance plutot que son nom
// [-l] donner le nom approximatif de l'instance (like '%instance%')
// [-i] donner le nom exact de l'instance

namespace JFG.Ubisoft.Perforce
{
    public static partial class Gii
    {
        private static MySqlConnection _conn;

        static void Main(string[] args)
        {
            int nPort = -1;
            var alResultats = new ArrayList();
            string sInstance = "NULL", sSelect = "SELECT name,alias,port,server,version,owner FROM p4_instances WHERE ";

            if (ParseCommandLine(args, ref nPort, ref sInstance) == false)
                Environment.Exit(0);

            _conn = new MySqlConnection("User Id=perforce_write;Password=gouranga;Host=p4db.ubisoft.org;Database=perforce_web;");
            if (nPort != -1)
                sSelect += "port = " + nPort;
            else
            {
                if (sInstance[0] == '%')
                    sSelect += "name LIKE '" + sInstance + "'";
                else
                    sSelect += "name = '" + sInstance + "'";
            }

            if (GoFetchData(sSelect, ref alResultats))
                GoPrintData(alResultats);
        }
    }
}