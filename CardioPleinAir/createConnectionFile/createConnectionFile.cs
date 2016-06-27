using System;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace JFG.Cardio
{
    class CreateConnection
    {
        private static void Main()
        {
            StorageHelpers storagehelpers = new StorageHelpers();
            IsolatedStorageFileStream ist = storagehelpers.GetIStoreStream("connstruct.xml");

            XDocument connfile = new XDocument(new XDeclaration ("1.0", "utf-8","yes"), new XElement("Databases",
                new XElement("Database", new XAttribute("RoleID", "MainDB"),
                    new XElement("DBInfo", new XAttribute("Server", "cardio.famillegratton.net"), new XAttribute("Port", 3306), new XAttribute("Name", "cardio")),
                    new XElement("Credentials", new XElement("User", "C280972CARDIO"), new XElement("Password", "*** PUT HASH CODE HERE ***"))),
                new XComment(""), new XComment(" AUTRE BD "), new XComment(""),
                new XElement("Database", new XAttribute("RoleID", "DevDB"),
                    new XElement("DBInfo", new XAttribute("Server", "oslo"), new XAttribute("Port", 3306), new XAttribute("Name", "cardio_dev"),
                    new XElement("Credentials", new XElement("User", "cardio"), new XElement("Password", "*** PUT HASH CODE HERE ***"))))));
            
            connfile.Save(ist);
        }
    }
}