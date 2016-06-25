// Comptabilite : comptes
// Écrit par J.F. Gratton, 2012.08.29
// 
// CreateDB.xaml.cs : Création de la Base de données dans le cas où elle n'existerait pas encore

using System;
using System.Data;
using System.Globalization;
using System.Windows;
using MySql.Data.MySqlClient;

// ReSharper disable CheckNamespace
namespace JFG.Comptes
// ReSharper restore CheckNamespace
{
    public partial class CreateDB
    {
        private DbStruct _dbStruct;
        private string _sControlUserName;
        private string _sControlUserPassword;
        private bool? _bCreatedOk;

        public CreateDB()
        {
            _dbStruct = new DbStruct();
            _bCreatedOk = null;
            _sControlUserName = _sControlUserPassword = "";
            InitializeComponent();
        }

        public bool? ShowDialog(ref DbStruct dbs)
        {
            _dbStruct = dbs;
            tbDB.Text = dbs.DB;
            tbHost.Text = dbs.Host;
            tbPort.Text = dbs.Port.ToString(CultureInfo.InvariantCulture);
            tbUsername.Text = dbs.User;
            pwdbx.Password = dbs.Password;
            ShowDialog();
            if (_bCreatedOk == true)
                dbs = _dbStruct;

            return _bCreatedOk;
        }

        private void BtnCreateClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbDB.Text) || String.IsNullOrEmpty(tbHost.Text) ||
                String.IsNullOrEmpty(tbPort.Text) || String.IsNullOrEmpty(tbUsername.Text) ||
                String.IsNullOrEmpty(pwdbx.Password) || String.IsNullOrEmpty(pwdbxControlUser.Password) ||
                String.IsNullOrEmpty(tbControlUser.Text)) return;

            _dbStruct.DB = tbDB.Text;
            _dbStruct.Host = tbHost.Text;
            _dbStruct.Password = pwdbx.Password;
            Int32.TryParse(tbPort.Text, out _dbStruct.Port);
            _dbStruct.User = tbUsername.Text;
            _sControlUserPassword = pwdbxControlUser.Password;
            _sControlUserName = tbControlUser.Text;
                
            _bCreatedOk = GoCreateDB();
            Close();
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            _bCreatedOk = false;
            Close();
        }

        private bool? GoCreateDB()
        {
            // Setting up variables
            string connString = @"User Id=" + _sControlUserName + ";Password=" + _sControlUserPassword + ";Host=" +
                                _dbStruct.Host + ";Port=" + _dbStruct.Port + ";";
            MySqlConnection conn;
            MySqlCommand cmd;
            MySqlTransaction transaction;
            // Setting up the connection

            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
                cmd = new MySqlCommand();
                transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = transaction;
                cmd.Connection = conn;
            }
            catch (MySqlException mEx)
            {
                MessageBox.Show(mEx.Message, "Erreur SQL");
                return false;
            }

            #region TRANSACTIONS
            try
            {
                // Transactions
                cmd.CommandText = "CREATE DATABASE " + _dbStruct.DB;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "USE " + _dbStruct.DB;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE __MONTHLY_TEMPLATE (mtID int(10) unsigned NOT NULL AUTO_INCREMENT," +
                                  "mtPoste varchar(45) NOT NULL, mtDepense decimal(2,0) unsigned NOT NULL, "+
                                  "mtPayeur varchar(45) NOT NULL, mtIndivisible bit(1) NOT NULL,PRIMARY KEY (mtID),"+
                                  "UNIQUE KEY mtID_UNIQUE (mtID),UNIQUE KEY mtPoste_UNIQUE (mtPoste)) ENGINE=InnoDB DEFAULT CHARSET=utf8";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE _groupes (gid int(10) unsigned NOT NULL AUTO_INCREMENT,gName" +
                    " varchar(20) NOT NULL,PRIMARY KEY (gid),UNIQUE KEY gid_UNIQUE (gid),UNIQUE KEY" +
                    " gName_UNIQUE (gName)) ENGINE=InnoDB DEFAULT CHARSET=utf8";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO _groupes (gID, gName) VALUES (1, 'dba'),(2,'usagers'),(3, 'stats'),(4,'readonly')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE _usagers (uID int(10) unsigned NOT NULL AUTO_INCREMENT,uName" +
                    " varchar(25) NOT NULL,uLongName varchar(45) DEFAULT NULL,gID int(11) unsigned NOT NULL," +
                    "uPassword text NOT NULL,uLastLogin char(19) DEFAULT 'jamais',PRIMARY KEY (uID),"+
                    "UNIQUE KEY uID_UNIQUE (uID),UNIQUE KEY uName_UNIQUE (uName))" +
                    "ENGINE=InnoDB DEFAULT CHARSET=utf8";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO _usagers (uID, uName, uLongName, gID, uPassword) VALUES " +
                                  "(1, 'dba', 'DB Administrator', 1, PASSWORD('jiefgroot')), " +
                                  "(2,'vicky','Vicky Desrosiers',2,PASSWORD('vicky')), " +
                                  "(3, 'jfg', 'J.F.Gratton',2,PASSWORD('jiefg000')), " +
                                  "(4, 'stats','Calcul de statistiques',3,PASSWORD('stats'))";
                cmd.ExecuteNonQuery();
                cmd.CommandText =
                    "INSERT INTO _usagers (uID, uName, uLongName, gID) VALUES (5, 'guest', 'Anonyme (Lecture seulement)', 4)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE _postesbudgetaires (pbID int(10) unsigned NOT " +
                                  "NULL AUTO_INCREMENT,pbPoste varchar(45) NOT NULL,PRIMARY KEY (pbID),"+
                                  "UNIQUE KEY pbID_UNIQUE (pbID),UNIQUE KEY pbPoste_UNIQUE (pbPoste))"+
                                  "ENGINE=InnoDB DEFAULT CHARSET=utf8";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO _postesbudgetaires (pbID, pbPoste) VALUES (1, 'loyer'),(2,'essence')," +
                                  "(3, 'hydro-québec'),(4,'vidéotron')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "GRANT ALL ON " + _dbStruct.DB + ".* TO '" + _dbStruct.User + "' IDENTIFIED BY '" +
                                  _dbStruct.Password + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "FLUSH PRIVILEGES";
                cmd.ExecuteNonQuery();
                transaction.Commit();
                _bCreatedOk = true;
            }

            catch (Exception x)
            {
                transaction.Rollback();
                MessageBox.Show(x.Message, "Échec de la transaction");
                _bCreatedOk = false;
            }
            #endregion TRANSACTIONS

            finally
            {
                conn.Close();
            }

            return _bCreatedOk;
        }
    }
}