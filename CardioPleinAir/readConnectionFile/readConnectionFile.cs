using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;

namespace JFG.Cardio
{
    class ReadConnFile
    {
        static void Main(string[] args)
        {
            StorageHelpers storagehelpers = new StorageHelpers();
            IsolatedStorageFileStream ist = storagehelpers.GetIStoreStream("connstruct.xml");

            XDocument xdoc = XDocument.Load(ist);
            
            //if (File.Exists("d:\\foo.xml"))
            //    File.Delete("d:\\foo.xml");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = settings.IgnoreProcessingInstructions = settings.IgnoreWhitespace = true;

            //using (XmlWriter w = XmlWriter.Create("d:\\foo.xml", settings))
            using (XmlReader r = XmlReader.Create(ist, settings))
            {
                //TODO : A MODIFIER
                //! Pas le choix, je dois convertir RoleID en Element
                r.MoveToContent();
                r.ReadStartElement("Databases");
                r.ReadStartElement("Database");
                r.MoveToFirstAttribute();
                Console.WriteLine(r.Value);
                //r.MoveToAttribute("RoleID");
                r.MoveToNextAttribute();
                string sRole = r.ReadElementContentAsString();
                //Console.WriteLine("RoleID: {0}", r.ReadElementContentAsString());
                
                r.ReadStartElement("DBInfo");
                Console.WriteLine("Server: {0}",r["Server"]);
                Console.WriteLine("Port: {0}", r["Port"]);
                Console.WriteLine("Name: {0}", r["Name"]);
                r.ReadEndElement(); // db info
                r.ReadStartElement("Credentials");
                Console.WriteLine("User: {0}", r.ReadElementContentAsString());
                Console.WriteLine("Password: {0}", r.ReadElementContentAsString());
                r.ReadEndElement(); // credentials
                r.ReadEndElement(); // database

                //r.MoveToContent();
                //r.ReadStartElement("Database");
                //Console.WriteLine("RoleID: {0}", r.ReadElementContentAsString());
                //r.ReadStartElement("DBInfo");
                //Console.WriteLine("Server: {0}", r.ReadElementContentAsString());
                //Console.WriteLine("Port: {0}", r.ReadElementContentAsString());
                //Console.WriteLine("Name: {0}", r.ReadElementContentAsString());
                //r.ReadEndElement(); // db info
                //r.ReadStartElement("Credentials");
                //Console.WriteLine("User: {0}", r.ReadElementContentAsString());
                //Console.WriteLine("Password: {0}", r.ReadElementContentAsString());
                //r.ReadEndElement(); // credentials
                //r.ReadEndElement(); // database

                r.ReadEndElement(); // databases
            }
        }
    }
}
