// Comptabilite : comptes
// Écrit par J.F.Gratton, 2012.09.23
// 
// tableHelpers.cs : fonctions d'aide à la gestion des tables

using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

//using JFG.SysUtils;


//select table_name from tables where table_schema='comptabilite'

// ReSharper disable CheckNamespace
namespace JFG.Comptes
// ReSharper restore CheckNamespace
{
    public partial class Bilan
    {
        // CreateTable() :
        // Le template de table mensuelle est appliqué pour ensuite créer la nouvelle tableur
        private bool CreateTable(string mois)
        {
            string sMonthlyTable = ReadTemplateFromDB();
            if (String.IsNullOrWhiteSpace(sMonthlyTable) == false)
                return CreateNewMonth(mois, sMonthlyTable);
            return false;
        }

        // readTemplateFromDB():
        // Lit le show create table __MONTHLY_TEMPLATE
        private string ReadTemplateFromDB()
        {
            string sMonthlyTableTemplate = "";
            var cmd = new MySqlCommand("SHOW CREATE TABLE __MONTHLY_TEMPLATE", _mconn);
            MySqlDataReader rdr = null;

            try
            {
                rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (rdr.Read())
                {
                    sMonthlyTableTemplate = rdr[1].ToString().Replace('`', '\'');
                }
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }

            if (rdr != null) rdr.Close();
            return sMonthlyTableTemplate;
        }

        // createNewMonth() :
        // Crée le nouveau mois
        private bool CreateNewMonth (string mois, string template)
        {
            bool bOk = true;
            string s = template.Replace("__MONTHLY_TEMPLATE", "_" + mois).Replace("\n", Environment.NewLine).Replace
                ("'", null).Replace("__MONTHLY_TEMPLATE", "_" + mois);
            var cmd = new MySqlCommand(s, _mconn);
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
                bOk = false;
            }

            return bOk;
        }
    }
}