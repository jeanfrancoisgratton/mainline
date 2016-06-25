// comptabilite : CompileFacturesUI
// Écrit par jfgratton (Jean-François Gratton), 2013.06.02 @ 11:49
// 
// CompileFactures.xaml.cs : fenêtre principale

using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows;
using MySql.Data.MySqlClient;
using JFG.mySQLhelper;

namespace JFG.Comptes
{
    public partial class CompileFactures : IDisposable
    {
        private string _connectString;
        private string _tableName;
        private string _sCommentaires;
        private int _nTypeAchat, _nEntree, _nTargetDB;
        private ConnectionHelper _sqlConn;
        private MySqlConnection _mconn;
        private ArrayList _alListeTables;
        private bool _bHelpNeeded;
        private bool _bLockConnection;

        #region INIT
        public CompileFactures()
        {
            InitializeComponent();
            _nTypeAchat = 0;
            _nEntree = 1;
            _nTargetDB = -1;
            _alListeTables = new ArrayList();
            _tableName = "";
            _bHelpNeeded = _bLockConnection = false;
            ParseCommandLine(Environment.GetCommandLineArgs());
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            rbEpicerie.IsChecked = true;

            if (_bHelpNeeded)
            {
                helpDlg helpDialog = new helpDlg();
                helpDialog.ShowDialog();
                Environment.Exit(0);
            }

            if (InitSQLConnection() == false)
            {
                MessageBox.Show(_sqlConn.GetExceptionMessage(), "Échec de connexion");
                Environment.Exit(0);
            }
            //ReloadComboBox();
            rbHE.IsEnabled = rbOsloPub.IsEnabled = rbOsloLAN.IsEnabled = rbLocalhost.IsEnabled = !_bLockConnection;
            if (_bLockConnection)
                sbiConnectionString.Content += "  *** LOCKED ***";
        }

        private bool InitSQLConnection()
        {
            _sqlConn = new ConnectionHelper(_connectString);
            if (_sqlConn.OpenConnection() == false)
                return false;

            _mconn = _sqlConn.GetConnection();
            string[] sAr = _connectString.Split(';');

            if (_sqlConn.ShowTables(ref _alListeTables, "comptabilite", "201%") == false)
                return false;

            sbiNombreEntrees.Content = "Entrée # " + _nEntree;
            sbiConnectionString.Content = "mysql://" + sAr[0].Replace("User Id=", "") + "@" + sAr[2].Replace("Host=", "") + ":" +
                sAr[3].Replace("Port=", "") + "/" + sAr[4].Replace("Database=", "");
            return true;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_mconn.State != ConnectionState.Closed)
                _sqlConn.CloseConnection();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _sqlConn.Dispose();
        }
        #endregion

        #region BOUTONS

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutDlg();
            about.ShowDialog();
        }
        
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string insCmd;
            if (String.IsNullOrWhiteSpace(txbxAmount.Text) || String.IsNullOrWhiteSpace(txbxDate.Text))
            {
                MessageBox.Show("La date et le montant ne peuvent demeurer vides");
                return;
            }
            if (_nTypeAchat == 11 && String.IsNullOrWhiteSpace(TxBxAutreTypeAchat.Text))
            {
                MessageBox.Show("Le champs autre type d'achat ne peut demeurer vide");
                return;
            }
            CheckIfItemExists(TxBxAutreTypeAchat.Text);
            _tableName = txbxDate.Text.Substring(0, 7).Replace('/', '_');
            if (txbxAmount.Text.EndsWith("."))
                txbxAmount.Text += "00";
            if (_alListeTables.Contains(_tableName) == false)
            {
                var rm = new ResourceManager("JFG.Comptes.Properties.Resources", Assembly.GetExecutingAssembly());
                if (_sqlConn.CreateTable(_tableName, rm.GetString("IDS_SHOWCREATETABLE")) == false)
                    MessageBox.Show(_sqlConn.GetExceptionMessage(), "Erreur à la création de la table");
                else
                {
                    _alListeTables.Add(_tableName);
                    _alListeTables.Sort();
                    string payeur = rbVD.IsChecked != null && (bool) rbVD.IsChecked ? "Vicky Desrosiers" : "Jean-Francois Gratton";

                    insCmd = String.IsNullOrWhiteSpace(_sCommentaires) ?
                        "INSERT INTO " + _tableName + " (mtPoste, mtDepense, mtPayeur) VALUES (" +
                        _nTypeAchat + ", " + txbxAmount.Text.Replace("$", "") + ", '" + payeur + "')" :
                        "INSERT INTO " + _tableName + " (mtPoste, mtDepense, mtPayeur, mtCommentaires) VALUES (" +
                        _nTypeAchat + ", " + txbxAmount.Text.Replace("$", "") + ", '" + payeur + "', '" + _sCommentaires + "')";
                    if (_sqlConn.Execute(insCmd) == false)
                        MessageBox.Show(_sqlConn.GetExceptionMessage(), "Erreur à l'insertion");
                    else
                        ResetForm();
                }
            }
        }

        private void btnComment_Click(object sender, RoutedEventArgs e)
        {
            string sComment = "";
            CommentWindow cmtWnd = new CommentWindow();
            if (cmtWnd.ShowDialog(ref sComment) == true)
                _sCommentaires = sComment;

        }
        #endregion

        #region RADIO BUTTONS
        #region Type d'achat
        private void rbEpicerie_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 0;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbEssence_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 1;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbVL_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 2;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbBCE_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 3;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbHQ_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 4;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbResto_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 5;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbPharmacie_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 6;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbHouseStuff_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 7;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbLoyer_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 8;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbEnfants_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 9;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbMisc_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 10;
            TxBxAutreTypeAchat.Visibility = Visibility.Hidden;
        }

        private void rbAutre_Checked(object sender, RoutedEventArgs e)
        {
            _nTypeAchat = 11;
            TxBxAutreTypeAchat.Visibility = Visibility.Visible;
        }
        #endregion

        #region Selection de BD
        private void rbHE_Checked(object sender, RoutedEventArgs e)
        {
            if (_nTargetDB != 0)
            {
                if (_nEntree > 1 && MessageBox.Show("Souhaitez-vous vraiment changer de BD ?", "Changement de BD",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                if (Reconnect(@"User Id=C280972_accnt;Password=JfG!2937;Host=comptes.famillegratton.net;Port=3306;Database=C280972_comptabilite;"))
                    _nTargetDB = 0;
                else
                {
                    MessageBox.Show(_sqlConn.GetExceptionMessage(), "Échec à la  reconnexion");
                    Environment.Exit(0);
                }
            }
        }

        private void rbOsloPub_Checked(object sender, RoutedEventArgs e)
        {
            if (_nTargetDB != 1)
            {
                if (_nEntree > 1 && MessageBox.Show("Souhaitez-vous vraiment changer de BD ?", "Changement de BD",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                if (Reconnect(@"User Id=C280972_accnt;Password=JfG!2937;Host=comptes.famillegratton.net;Port=3306;Database=C280972_comptabilite;"))
                    _nTargetDB = 1;
                else
                {
                    MessageBox.Show(_sqlConn.GetExceptionMessage(), "Échec à la  reconnexion");
                    Environment.Exit(0);
                }
            }
        }

        private void rbOsloLAN_Checked(object sender, RoutedEventArgs e)
        {
            if (_nTargetDB != 2)
            {
                if (_nEntree > 1 && MessageBox.Show("Souhaitez-vous vraiment changer de BD ?", "Changement de BD",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                if (Reconnect(@"User Id=C280972_accnt;Password=JfG!2937;Host=comptes.famillegratton.net;Port=3306;Database=C280972_comptabilite;"))
                    _nTargetDB = 2;
                else
                {
                    MessageBox.Show(_sqlConn.GetExceptionMessage(), "Échec à la  reconnexion");
                    Environment.Exit(0);
                }
            }
        }

        private void rbLocalhost_Checked(object sender, RoutedEventArgs e)
        {
            if (_nTargetDB != 3)
            {
                if (_nEntree > 1 && MessageBox.Show("Souhaitez-vous vraiment changer de BD ?", "Changement de BD",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                if (Reconnect(@"User Id=C280972_accnt;Password=JfG!2937;Host=comptes.famillegratton.net;Port=3306;Database=C280972_comptabilite;"))
                    _nTargetDB = 3;
                else
                {
                    MessageBox.Show(_sqlConn.GetExceptionMessage(), "Échec à la  reconnexion");
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #endregion

        private void txbxAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txbxAmount.Text))
                return;
            if (txbxAmount.Text.EndsWith("."))
                txbxAmount.Text += "00";
        }
    }
}