// Comptabilite : comptes
// Écrit par J.F. Gratton, 2012.09.19
// 
// helpers.cs : fonctions de parsing du mois

using System;
using System.Data;
using System.Globalization;
using System.Windows;
using MySql.Data.MySqlClient;
using JFG.SysUtils;

//select table_name from tables where table_schema='comptabilite'

// ReSharper disable CheckNamespace
namespace JFG.Comptes
// ReSharper restore CheckNamespace
{
    public partial class Bilan
    {
        // GetLatestTable() :
        // Toutes les tables mensuelles sont nommées sous la forme _YYYY_MM
        // Nous prenons donc la dernière de ces tables, enlevons le _ et retournons la valeur
        private string GetLatestTable()
        {
            string latestMth = "";
            var cmd = new MySqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema ='" + _dbS.DB +
                "' AND table_name LIKE '20%' ORDER BY table_name DESC", _mconn);
            MySqlDataReader rdr = null;
            bool bBdHasRows = false;
            
            try
            {
                rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (rdr.Read())
                {
                    latestMth = rdr[0].ToString();
                    bBdHasRows = rdr.HasRows;
                }
            }
            
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }
            
            if (rdr != null) rdr.Close();

            if (bBdHasRows == false)
                return "";
            
            return latestMth;
        }

        // TableName2HumanReadable():
        // Prend le nom de la table (_2012_09) et le transforme en lisible (Septembre 2012)
        private string TableName2HumanReadable (string tblName)
        {
            if (String.IsNullOrWhiteSpace(tblName))
                return "Aucun";

            // obligé de mettre "" à l'indice 0 car le 0ème mois n'existe pas :-)
            string[] sMois =
            {
                "", "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
                "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
            };
            return sMois[Int32.Parse(sysutils.Right(2, tblName))] + " " + sysutils.Left(4, tblName);
        }

        // TrouveMoisAAfficher():
        // Additionne 1 au mois
        private string TrouveMoisAAfficher()
        {
            string dernierMois;

            if (String.IsNullOrEmpty((dernierMois = GetLatestTable())))
                dernierMois = DateTime.Now.ToString("yyyy_MM");
            else
            {
                int aa = Int32.Parse(sysutils.Left(4, dernierMois)), mm = Int32.Parse(sysutils.Right(2, dernierMois));
                if (mm == 12)
                {
                    ++aa;
                    mm = 1;
                }
                else
                    ++mm;
                dernierMois = aa.ToString(CultureInfo.InvariantCulture);
                if (mm > 9)
                    dernierMois += mm;
                else
                    dernierMois += "0" + mm;
            }

            return dernierMois;
        }

        //  TesteBDExiste()
        //  Vérifie que la base de donnée est existante ou non
        private void TesteBDExiste()
        {
            _bConnexionExiste = false;
            while (_bConnexionExiste == false && TesteConnexion(_dbS) == false)
            {
                MessageBoxResult dlgR = MessageBox.Show(
                    "La BD est inaccessible. Souhaitez-vous créer une nouvelle BD ?",
                    "BD non-joignable", MessageBoxButton.YesNo);

                if (dlgR == MessageBoxResult.Yes)
                {
                    var dbCreator = new CreateDB();
                    bool? showDialog = dbCreator.ShowDialog(ref _dbS);
                    if (showDialog != null && (bool) showDialog)
                        _bConnexionExiste = true;
                }
                else
                    Environment.Exit(0);
            }
        }
    }
}