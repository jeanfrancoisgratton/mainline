// perforceDB : gis
// gis.cs : main file
// 2013.02.28.09:55, Jean-François Gratton

//gis [{-l like | -p port | -i instance | -ldm}] [-s server] | [-v version] [-o owner]

/*
 * public string Name, Alias, Server, Version, Owner;
        public int Port;

        // other fields
        public string Description, Estimated_finish, Dudb, Location, Fpp, Note, Restore_date, DbstorageType;
        public int Hide, Allocated, Estimated_size, Archive, Journal_size;
 */

using System;
using System.Collections;

namespace JFG.Ubisoft.Perforce
{
    public partial class gis
    {
        private static string _sXmlFile, _connectionString;
        //private static ResultatStruct RS = new ResultatStruct();
        
        static void Main(string[] args)
        {
            ArrayList alRS = new ArrayList();
            //XMLfileHandler xmlhandler;
            DBConnectionStruct? dbcs = null;
            
            alRS.Clear();
            _sXmlFile = "";
            string sSelect = ParseCommandLine(args);

            //TODO : on est rendus ici
            if (String.IsNullOrWhiteSpace(_sXmlFile) == false)
            {
                // xmlhandler = new XMLfileHandler();
                //dbcs = XMLfileHandler.readXMLFile(_sXmlFile);
                //if (dbcs != null)
                   // _connectionString = XMLfileHandler.parseConnectionStruct((DBConnectionStruct)dbcs);
            }

            alRS = FetchData(sSelect);

            if (sSelect.StartsWith("SELECT name, port"))
                ShowDML(alRS);
            else
                ShowFullInfo(alRS);
        }
    }
}