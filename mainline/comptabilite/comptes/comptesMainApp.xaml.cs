// Comptabilite : comptes
// Écrit par J.F. Gratton, 2012.08.22
// 
// comptesMainApp.xaml.cs : point d'entrée au logiciel

using System;
using System.Data;
using System.Windows;
using JFG.Comptes.DB_TableHelpers;
using MySql.Data.MySqlClient;
//using JFG.SysUtils;


//select table_name from tables where table_schema='comptabilite'

namespace JFG.Comptes
{
    public struct DbStruct
    {
        public string Host, DB, User, Password;
        public int Port;
    };

    public partial class Bilan
    {
        private DbStruct _dbS;
        private MySqlConnection _mconn;
        private bool _bConnexionExiste;
            
        #region INIT + CLEANUP
        public Bilan()
        {
            _dbS = ParseCommandLine(Environment.GetCommandLineArgs());
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            TesteBDExiste();

            //LoginDialog l = new LoginDialog(_dbS);
            //l.ShowDialog(ref _dbS);
            
            sbiConnectionString.Content = _dbS.User + "@" + _dbS.Host + ":" + _dbS.Port;
            _mconn = new MySqlConnection("User="+_dbS.User+"; password="+_dbS.Password+"; server="+_dbS.Host+"; port="+
            _dbS.Port + "; database="+_dbS.DB+";");
            
            _mconn.Open();
            sbiDernierMois.Content = TableName2HumanReadable(GetLatestTable());
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_mconn != null && _mconn.State == ConnectionState.Open)
                _mconn.Close();
        }

        #endregion

        #region BOUTONS
        private void BtnAboutClick(object sender, RoutedEventArgs e)
        {
            var about = new aboutDlg();
            about.ShowDialog();
        }

        private void BtnPostesClick(object sender, RoutedEventArgs e)
        {
            var postes = new PostesBudgetaires(_mconn);
            postes.Show();
        }

        private void BtnNouveauMoisClick(object sender, RoutedEventArgs e)
        {
            string dernierMois = TrouveMoisAAfficher();

            if (MessageBox.Show("Création de la table " + dernierMois, "Confirmation de la création",
                MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            if (CreateTable(dernierMois) == false)
                MessageBox.Show("La table " + dernierMois + " n'a pu être créée", "Erreur à la création de table");
            else
                sbiDernierMois.Content = TableName2HumanReadable(GetLatestTable());
        }

        private void BtnShowCreateTableClick(object sender, RoutedEventArgs e)
        {
            var sct = new ShowCreateTable(_dbS, _mconn);
            sct.ShowDialog();
        }

        private void BtnUsagersClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLoginClick(object sender, RoutedEventArgs e)
        {
            //LoginDialog l = new LoginDialog(_dbS);
            //l.ShowDialog(ref _dbS);
        }

        #endregion
    }
}