// CardioPleinAir : isolatedStorageHelpers
// Écrit par : jfgratton (), 2014.10.13 @ 17:21
// 
// i-storageHelpers.cs : Isolated Storage helper methods

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

namespace JFG.Cardio
{
    public struct ConnectionStruct
    {
        public string DB;
        public string PwdHash;
        public int Port;
        public string Server;
        public string Username;
    };

    //TODO: classe incomplete
    public partial class StorageHelpers
    {
        private bool _Dispose;
        private string _file;
        private IsolatedStorageFileStream _istoreFileStream;

        #region INIT + CLEANUP

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_Dispose)
                return;

            if (disposing && _istoreFileStream != null) _istoreFileStream.Dispose();

            _Dispose = true;
        }

        #endregion // INIT + CLEANUP

        #region GET HANDLES

        public IsolatedStorageFileStream GetIStoreStream(string filename = "")
        {
            _file = filename;
            IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForAssembly();
            _istoreFileStream = new IsolatedStorageFileStream(_file, FileMode.OpenOrCreate, FileAccess.ReadWrite, f);

            return _istoreFileStream;
        }

        private ConnectionStruct ReadConnectionInfoFromStorage(IsolatedStorageFileStream fs)
        {
            _istoreFileStream = fs;
            ConnectionStruct connInfo = new ConnectionStruct();

            StreamReader sr = new StreamReader(_istoreFileStream);

            // Do your XML reading stuff here


            return connInfo;
        }

        #endregion GET HANDLES

        #region XML

        private ConnectionStruct[] ReadConnectionFile()
        {
            ConnectionStruct[] conn = new ConnectionStruct[2];
            return conn;
        }

        private string WriteConnectionFile(ConnectionStruct[] conn)
        {
            string sRes = "";
            IsolatedStorageFileStream ist = GetIStoreStream("connstruct.xml");

            try
            {
                XDocument connfile = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Databases",
                        new XElement("Database", new XAttribute("RoleID", "MainDB"),
                            new XElement("DBInfo", new XAttribute("Server", conn[0].Server), new XAttribute("Port", conn[0].Port), new XAttribute("Name", conn[0].DB)),
                            new XElement("Credentials", new XElement("User", conn[0].Username),
                                new XElement("Password",conn[0].PwdHash))),
                        new XComment(""), new XComment(" AUTRE BD "), new XComment(""),
                        new XElement("Database", new XAttribute("RoleID", "FailoverDB"),
                            new XElement("DBInfo", new XAttribute("Server", conn[1].Server), new XAttribute("Port", conn[1].Port), new XAttribute("Name", conn[1].DB),
                                new XElement("Credentials", new XElement("User", conn[1].Username),
                                    new XElement("Password", conn[1].PwdHash))))));

                connfile.Save(ist);
            }
            catch (Exception ex)
            {
                sRes = ex.Message;
            }

            return sRes;
        }

        #endregion
    }
}