using System;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Devart.Data.MySql;
using JFG.dialogs;
using listeBoitesVocales;

namespace uOne.listeBoitesVocales
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthInfoStruct _aisInfo = new AuthInfoStruct();
        private int _uid;
        private MySqlConnection _mconn;
        string[] args = Environment.GetCommandLineArgs();
        bool bLogin = false;

        #region INIT + CLEANUP

        public MainWindow()
        {
            _aisInfo = new AuthInfoStruct();
            LoginDialog lDlg = null;
            string longname = "";

            InitializeComponent();

            _aisInfo.Server = "172.26.51.186";
            _aisInfo.Database = "uone";
            _aisInfo.Port = 3306;
            _aisInfo.ControlUser = "uone";
            _aisInfo.ControlPassword = "uone";
            _aisInfo.TableNameUser = "sysadmins";
            _aisInfo.ColumnNameUser = "uLogin";
            _aisInfo.ColumnNamePassword = "uPassword";
            _aisInfo.ColumnNameGroup = "";
            _aisInfo.AllowedBadLogins = 3;
            _aisInfo.PasswordIsEncrypted = true;

            lDlg = new LoginDialog(SkinSelectionEnum.acision);

            bLogin = args.Length > 1 ? (bool)lDlg.ShowDialog(args, ref _aisInfo) : (bool)lDlg.ShowDialog(ref _aisInfo);

            if (bLogin == false)
                Environment.Exit(0);

            lblTitre.Content = "listeBoitesVocales v" + Assembly.GetExecutingAssembly().GetName().Version;

            string mConnString = @"User Id=" + _aisInfo.ControlUser + ";Password=" + _aisInfo.ControlPassword + ";Host=" +
                                 _aisInfo.Server + ";Port=" + _aisInfo.Port + ";Database=" + _aisInfo.Database + ";";
            _mconn = new MySqlConnection(mConnString);
            try
            {
                _mconn.Open();
            }
            catch (MySqlException mEx)
            {
                MessageBox.Show(mEx.Message, "Erreur à la connexion");
            }

            if (getAdditionalInfo(ref _uid, ref longname) == false)
            {
                MessageBox.Show("Votre uID n'a pas été retrouvé dans la BD.", "Erreur fatale");
                Environment.Exit(0);
            }

            lblUser.Content = "Bonjour, " + longname + " (" + _aisInfo.LoginUser + ") .";
        }

        private void Window_IsClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_mconn != null) _mconn.Close();
        }

        #endregion INIT + CLEANUP

        //+     getAddtionalInfo : helper function to get user's uID and long name
        //!+    In:
        //      ref id, ref name : the ID + long name to be returned
        //!+    Out:
        //      boolean indicating if there were an error or anything during the op
        private bool getAdditionalInfo(ref int id, ref string name)
        {
            bool bGotResult;
            MySqlCommand mCmd = new MySqlCommand("SELECT uID, uLongName FROM sysadmins WHERE uLogin = '" +
                _aisInfo.LoginUser + "'", _mconn);
            MySqlDataReader rdr = null;

            try
            {
                rdr = mCmd.ExecuteReader(CommandBehavior.SingleRow);
                rdr.Read();
                id = rdr.GetInt32(rdr.GetOrdinal("uID"));
                name = rdr.GetString(rdr.GetOrdinal("uLongName"));
            }
            catch (MySqlException mEx)
            {
                MessageBox.Show(mEx.Message, "Erreur à l'exécution du SELECT");
                if (rdr != null) rdr.Close();
            }

            bGotResult = rdr != null && rdr.HasRows;

            if (rdr != null) rdr.Close();

            return bGotResult;
        }

        #region BOUTONS

        private void btnPasteNumber_Click(object sender, RoutedEventArgs e) { tbMSISDN.Text = Clipboard.GetText(); }

        private void btnPasteMS_Click(object sender, RoutedEventArgs e) { tbMailHost.Text = Clipboard.GetText(); }

        private void btnPasteGreeting_Click(object sender, RoutedEventArgs e) { tbGreeting.Text = Clipboard.GetText(); }

        private void btnPasteBillet_Click(object sender, RoutedEventArgs e) { tbBillet.Text = Clipboard.GetText(); }

        private void btnList_Click(object sender, RoutedEventArgs e)
        {
            listeBVs lbv = new listeBVs(_mconn);
            lbv.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbMSISDN.Text))
            {
                MessageBox.Show("Vous devez au moins entrer un numéro de téléphone", "Données invalides");
                return;
            }

            string cmdString = "INSERT INTO boitesvocales (rMSISDN, rGreetingAdm, rMailStore, rBillet, rTimestamp, uID)" +
                " VALUES ('" + tbMSISDN.Text + "', '" + tbGreeting.Text + "', '" + tbMailHost.Text +
                "', '" + tbBillet.Text + "', '" + DateTime.Now.ToString() + "', " + _uid + ")";

            MySqlCommand mcmd = new MySqlCommand(cmdString, _mconn);

            try
            {
                mcmd.ExecuteNonQuery();
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur à l'exécution du INSERT");
                return;
            }
            tbMSISDN.Clear(); tbGreeting.Clear(); tbMailHost.Clear(); tbBillet.Clear();
        }

        #endregion BOUTONS
    }
}