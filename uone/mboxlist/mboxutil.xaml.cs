using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Devart.Data.MySql;
using JFG.dialogs;
using SysUtils;

namespace uOne.mboxlist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthInfoStruct _aisInfo;
        private MySqlConnection _mconn;

        #region INIT + CLEANUP
        public MainWindow()
        {
            _aisInfo = new AuthInfoStruct();
            LoginDialog lDlg = null;
            string[] args = Environment.GetCommandLineArgs();
            bool bLogin = false;

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

            bLogin = args.Length > 1 ? (bool) lDlg.ShowDialog(args, ref _aisInfo) : (bool) lDlg.ShowDialog(ref _aisInfo);

            if (bLogin == false)
                Environment.Exit(0);

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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_mconn != null) _mconn.Close();
        }

        private void Window_Loading(object sender, RoutedEventArgs e)
        {
            cboSortOrder.Items.Add("#greetings (défaut)");
            cboSortOrder.Items.Add("#greetings + lastAccessedTime");
            cboSortOrder.Items.Add("#greetings + greetingAdm");
            cboSortOrder.Items.Add("#greetings + mailStore");
            getDates();
            getGreetings();
            populateGrid();

            //btnDeleteRow.IsEnabled = false;
        }
        #endregion

        

        #region COMBOS
        private void cboDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            populateGrid();
            //btnDeleteRow.IsEnabled = true;
        }

        private void cboMinGreetings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboDates.SelectedIndex == -1)
            {
                cboMinGreetings.SelectedItem = -1;
                return;
            }
            string sqlCmd = "SELECT mgBoiteVocale, mgNombreGreetings, mgLastAccessed, mgGreetingAdm, mgMailStore " +
                     "FROM multiplegreetings WHERE mgNombreGreetings >='" + cboMinGreetings.SelectedItem + "' ORDER BY mgNombreGreetings DESC";

            populateGrid(sqlCmd);
        }

        private void cboSortOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fullSort;
            string sqlBase = "SELECT mgBoiteVocale, mgNombreGreetings, mgLastAccessed, mgGreetingAdm, " +
                "mgMailStore FROM multiplegreetings WHERE mgDateInserted='" + cboDates.SelectedItem +
                "' ORDER BY mgNombreGreetings DESC";

            switch (cboSortOrder.SelectedIndex)
            {
                case 1:
                    fullSort = sqlBase + ", mgLastAccessed DESC";
                    break;
                case 2:
                    fullSort = sqlBase + ", mgGreetingAdm DESC";
                    break;
                case 3:
                    fullSort = sqlBase + ", mgMailStore DESC";
                    break;
                default:
                    fullSort = sqlBase;
                    break;
            }

            populateGrid(fullSort);
        }
        #endregion

        #region INJECTION DATA

        //!+ getDates()
        // Popule le combobox des dates d'extraction des mboxlist
        private void getDates()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT mgDateInserted FROM multiplegreetings " +
                                       " ORDER BY mgDateInserted DESC", _mconn);
            MySqlDataReader rdr = null;

            try
            {
                cboDates.Items.Clear();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    cboDates.Items.Add(rdr[0].ToString());
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "MySQL exception");
            }

            finally
            {
                if (rdr != null) rdr.Close();
            }
        }

        //!+ getGreetings()
        // Popule le combobox pour sorter par #greetings
        private void getGreetings()
        {
            MySqlDataReader rdr = null;
            try
            {
                cboMinGreetings.Items.Clear();
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT mgNombreGreetings from multiplegreetings" +
                " ORDER BY mgNombreGreetings DESC", _mconn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    cboMinGreetings.Items.Add(rdr[0].ToString());
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "MySQL exception");
            }

            finally
            {
                if (rdr != null) rdr.Close();
            }
        }

        //!+ populateGrid()
        // Popule le datagrid avec le data de la requete SQL passée en paramètre
        private void populateGrid(string sqlCmd = null)
        {
            DataTable dt = new DataTable();
            MySqlDataReader rdr = null;
            MySqlCommand cmd = null;

            dt.Columns.Add("MSISDN");
            dt.Columns.Add("# greetings");
            dt.Columns.Add("Last accessed");
            dt.Columns.Add("Greeting adm");
            dt.Columns.Add("MailStore");
            dgGreetings.DataContext = dt;

            try
            {
                string commandeSQL;
                if (sqlCmd != null)
                    commandeSQL = sqlCmd;
                else
                {
                    commandeSQL = "SELECT mgBoiteVocale, mgNombreGreetings, mgLastAccessed, mgGreetingAdm, " +
                                  "mgMailStore FROM multiplegreetings WHERE mgDateInserted='" + cboDates.SelectedItem +
                                  "' ORDER BY mgNombreGreetings DESC";
                }
                cmd = new MySqlCommand(commandeSQL, _mconn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    dt.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4]);
            }
            catch (MySqlException mex)
            {
                if (mex.Message.StartsWith("Commands out of sync") == false)
                    MessageBox.Show(mex.Message, "MySQL exception");
                
                if (rdr != null) rdr.Close();
            }
            finally
            {
                if (rdr != null) rdr.Close();
            }

            showRecordCount();
        }

        //!+ showRecordCount()
        // Affiche le nombre total de BV pour une extraction donnée, et facultativement, en triant avec cboMinGreetings
        private void showRecordCount()
        {
            MySqlCommand cmd1 = null, cmd2 = null;
            MySqlDataReader rdr1 = null, rdr2 = null;

            try
            {
                string commandeSQLall = "SELECT COUNT(*) FROM multiplegreetings WHERE mgDateInserted='" +
                                        cboDates.SelectedItem + "'";
                string commandeSQLgrt = commandeSQLall + " AND mgNombregreetings >=" + cboMinGreetings;
                cmd1 = new MySqlCommand(commandeSQLall, _mconn);
                cmd2 = new MySqlCommand(commandeSQLgrt, _mconn);

                rdr1 = cmd1.ExecuteReader(CommandBehavior.SingleRow);
                rdr1.Read();
                tbTotal.Text = rdr1[0].ToString();

                if (cboMinGreetings.SelectedIndex != -1)
                {
                    rdr2 = cmd2.ExecuteReader(CommandBehavior.SingleRow);
                    rdr2.Read();
                    tbAffected.Text = rdr2[0].ToString();
                }
            }
            catch (MySqlException mex)
            {
                if (mex.Message.StartsWith("Commands out of sync") == false)
                    MessageBox.Show(mex.Message, "MySQL exception");
                if (rdr1 != null) rdr1.Close();
                if (rdr2 != null) rdr2.Close();
            }
            finally
            {
                if (rdr1 != null) rdr1.Close();
                if (rdr2 != null) rdr2.Close();
            }
        }
        #endregion

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            sqlLoader sqlL = new sqlLoader(_mconn);

            if (sqlL.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                getDates();
                getGreetings();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}