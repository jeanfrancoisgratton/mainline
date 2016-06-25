using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace JFG.Comptes.DB_TableHelpers
{
    /// <summary>
    /// Interaction logic for showCreateTable.xaml
    /// </summary>
    public partial class ShowCreateTable
    {
        private readonly MySqlConnection _conn;
        private DbStruct _dbs;
        public ShowCreateTable(DbStruct dbs, MySqlConnection conn)
        {
            _conn = conn;
            _dbs = dbs;
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            FillCbo();
            cboTables.SelectedIndex = 0;

        }

        private void CboTablesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillTextBox();
        }


        private void FillCbo()
        {
            var cmd = new MySqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema ='" +
                _dbs.DB + "' ORDER BY table_name ASC", _conn);
            MySqlDataReader rdr = null;

            try
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    cboTables.Items.Add(rdr[0].ToString());
                }
            }

            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }

            if (rdr != null) rdr.Close();
        }

        private void FillTextBox()
        {
            var cmd = new MySqlCommand("SHOW CREATE TABLE " + cboTables.SelectedItem, _conn);
            MySqlDataReader rdr = null;

            tbxShowCreate.Clear();
            try
            {
                rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (rdr.Read())
                {
                    string sShowCreateTable = rdr[1].ToString();
                    tbxShowCreate.Text = sShowCreateTable.Replace("`", null);
                }
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }

            if (rdr != null) rdr.Close();
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
