using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using JFG.SysUtils.CryptoLib;

namespace xmlTester
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = xmlSettings.NewLineOnAttributes = true;
            FileStream fs = new FileStream("test.xml", FileMode.Create, FileAccess.Write);
            XmlWriter w = XmlWriter.Create(fs, xmlSettings);
            
            w.WriteStartDocument();
            w.WriteStartElement("connectionString");
            w.WriteAttributeString("connectionID", "101");
            w.WriteAttributeString("user", "perforce_read");
            w.WriteAttributeString("password", CryptoUtils.EncryptString("jiefgroot"));
            w.WriteAttributeString("database", "pweb");
            w.WriteAttributeString("host", "localhost");
            w.WriteAttributeString("port", "3306");
            w.WriteEndElement();
            w.WriteEndDocument();
            w.Flush();
            fs.Close();

            Console.WriteLine("done");
        }
    }
}
