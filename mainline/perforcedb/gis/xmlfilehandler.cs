// perforceDB : gis
// XMLFileHandler.cs : gestion du fichier XML de connexion
// 2013.05.15 06:36, Jean-Francois Gratton

using System;
using System.IO;
using System.Text;
using System.Xml;
using JFG.SysUtils.CryptoLib;

namespace JFG.Ubisoft.Perforce
{
    public class XMLfileHandler
    {
        // writeXMLFile() :
        // Ecrit les entrees dans un fichier XML
        // Params:
        //  string: fichier XML,
        //  out string : message d'erreur si necessaire
        //  DBConnectionStruct : infos sur la connex
        // Retourne : true/false si tout est OK
        public string writeXMLFile(string xmlFile, DBConnectionStruct dbcs)
        {
            string sErreur = "";
            try
            {
                XmlWriterSettings xmlSettings = new XmlWriterSettings();
                xmlSettings.Indent = xmlSettings.NewLineOnAttributes = true;
                FileStream fs = new FileStream(xmlFile, FileMode.Create, FileAccess.Write);
                XmlWriter w = XmlWriter.Create(fs, xmlSettings);

                w.WriteStartDocument();
                w.WriteStartElement("connectionString");
                w.WriteAttributeString("user", dbcs.Username);
                w.WriteAttributeString("password", CryptoUtils.EncryptString(dbcs.Password));
                w.WriteAttributeString("database", dbcs.Database);
                w.WriteAttributeString("host", dbcs.Server);
                w.WriteAttributeString("port", dbcs.Port.ToString());
                w.WriteEndElement();
                w.WriteEndDocument();
                w.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                sErreur = ex.Message;
            }
            return sErreur;
        }

        public DBConnectionStruct? readXMLFile(string xmlFile)
        {
            DBConnectionStruct? dbcs = null;
            DBConnectionStruct d = new DBConnectionStruct();
            XmlTextReader rdr = null;

            if (File.Exists(xmlFile) == false)
                return null;

            try
            {
                rdr = new XmlTextReader(xmlFile);
                while (rdr.Read())
                {
                    while (rdr.MoveToNextAttribute())
                    {
                        switch (rdr.Name)
                        {
                            case "user":
                                d.Username = rdr.Value;
                                break;
                            case "password":
                                d.Password = CryptoUtils.DecryptString(rdr.Value);
                                break;
                            case "server":
                                d.Server = rdr.Value;
                                break;
                            case "database":
                                d.Database = rdr.Value;
                                break;
                            case "port":
                                if (Int32.TryParse(rdr.Value, out d.Port) == false)
                                    d.Port = 3306;
                                break;
                            default:
                                break;
                        }
                    }
                }
                dbcs = d;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dbcs = null;
            }

            finally
            {
                rdr.Close();
            }
            return dbcs;
        }

        public string parseConnectionStruct(DBConnectionStruct d)
        {
            //User Id=p4utils_read;Password=perforceUtils;Host=p4db.ubisoft.org;Database=perforce_web;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("User Id={0};Password={1};Host={2};Port={3};Database={4};",
                            d.Username, d.Password, d.Server, d.Port, d.Database);

            return sb.ToString();
        }
    }
}