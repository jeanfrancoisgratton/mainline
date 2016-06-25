using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Devart.Data.MySql;


namespace listeBoitesVocales
{
    /// <summary>
    /// Interaction logic for listeBVs.xaml
    /// </summary>
    public partial class listeBVs : Window
    {
        private MySqlConnection _mconn;
        private int _uid;
        

        public listeBVs(MySqlConnection sqlcon)
        {
            _mconn = sqlcon;
            InitializeComponent();
        }

        private void ListWindow_IsLoaded(object sender, RoutedEventArgs e)
        {
            if (populateSysAdminComboBox() == false)
                Close();
        }

        private bool populateSysAdminComboBox()
        {
            var cmd = new MySqlCommand("SELECT uID, uLongName from sysadmins", _mconn);
            MySqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    _uid = rdr.GetInt32(rdr.GetOrdinal("uID"));
                    cboSysadmins.Items.Add(_uid.ToString() + " - " + rdr.GetString(rdr.GetOrdinal("uLongName")));
                }
            }

            catch (MySqlException mEx)
            {
                cboSysadmins.Items.Clear();
                if (rdr != null) rdr.Close();
                MessageBox.Show(mEx.Message, "mySQL Exception");
                return false;
            }

            cboSysadmins.Items.Add("Toutes les boîtes vocales");
            return true;
        }

        private void sysadminSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sUid;
            
            sUid = cboSysadmins.SelectedIndex == cboSysadmins.Items.Count - 1 ? "" : cboSysadmins.SelectedItem.ToString().Substring(0, cboSysadmins.SelectedItem.ToString().IndexOf('-'));
            afficheCorrections(sUid);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // SELECT bv.rMSISDN, bv.rGreetingAdm, bv.rMailStore, bv.rBillet, sa.uLongName
        // FROM boitesvocales bv, sysadmins sa
        // WHERE bv.uID = sa.uID;
        private void afficheCorrections(string whereClause)
        {
            string sCommand =
                "SELECT bv.rMSISDN, bv.rGreetingAdm, bv.rMailStore, bv.rBillet, sa.uLongName, bv.rTimestamp FROM boitesvocales bv, sysadmins sa WHERE bv.uID = sa.uID";
            if (String.IsNullOrWhiteSpace(whereClause) == false)
                sCommand += " AND bv.uID = " + whereClause;
            var cmd = new MySqlCommand(sCommand, _mconn);
            MySqlDataReader rdr = null;
            DataTable dt = new DataTable();

            dt.Columns.Add("MSISDN");
            dt.Columns.Add("Greeting admin");
            dt.Columns.Add("mailStore");
            dt.Columns.Add("billet");
            dt.Columns.Add("sysadmin");
            dt.Columns.Add("timestamp");

            try
            {
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    dt.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5]);
                dgBV.DataContext = dt;
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
    }
}
