using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using Devart.Data.MySql;
using MessageBox = System.Windows.MessageBox;

namespace uOne.mboxlist
{
    /// <summary>
    /// Interaction logic for sqlLoader.xaml
    /// </summary>
    public partial class sqlLoader : Window
    {
        private string _file2Load;
        private bool _truncateTableB4, _deleteFileAfterwards;
        private MySqlConnection _mconn;
        private DialogResult _dlgResult;

        public sqlLoader(MySqlConnection c)
        {
            _mconn = c;
            InitializeComponent();
            _file2Load = "";
            _dlgResult = System.Windows.Forms.DialogResult.OK;
            _truncateTableB4 = _deleteFileAfterwards = false;
        }
        
        public new DialogResult ShowDialog()
        {
            base.ShowDialog();
            return _dlgResult;
        }

        private void ptbxFile2Load_LostFocus(object sender, RoutedEventArgs e)
        {
            if (File.Exists(ptbxFile2Load.Text) == false)
                ptbxFile2Load.Foreground = Brushes.Red;
            else
                ptbxFile2Load.Foreground = Brushes.LightGreen;
        }

        private void chkTruncate_Checked(object sender, RoutedEventArgs e)
        {
            if (chkTruncate.IsChecked != null)
                _truncateTableB4 = (bool) chkTruncate.IsChecked;
        }

        private void chkDelete_Checked(object sender, RoutedEventArgs e)
        {
            if (chkDelete.IsChecked != null)
                _deleteFileAfterwards = (bool) chkDelete.IsChecked;
        }

        #region BOUTONS
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofDlg = new OpenFileDialog();
            ofDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofDlg.Filter = Properties.Resources.sqlLoader_FileFilterSTRresource;
            ofDlg.RestoreDirectory = true;

            if (ofDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                _file2Load = ptbxFile2Load.Text = ofDlg.FileName;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            /*
             * load data local infile "c:/utils/sql/allms.txt" into table multiplegreetings fields terminated by " "
             * (mgNombreGreetings, mgLastAccessed, mgGreetingAdm, mgBoiteVocale, mgMailStore, mgDateInserted);
             * delete from multiplegreetings where mgnombregreetings < 3;
             * delete from multiplegreetings where mglastaccessed="-";
            */

            if (String.IsNullOrWhiteSpace(_file2Load))
            {
                MessageBox.Show("Aucun fichier à charger n'a été spécifié", "Impossible de procéder");
                return;
            }

            string sqlCmd1 = @"LOAD DATA LOCAL INFILE '" + _file2Load.Replace('\\', '/') +
                             "' INTO TABLE multiplegreetings FIELDS TERMINATED BY \" \"" +
                             "(mgNombreGreetings, mgLastAccessed, mgGreetingAdm, mgBoiteVocale, mgMailStore, mgDateInserted)";
            const string sqlCmd2 = "DELETE FROM multiplegreetings WHERE mgnombregreetings < 3 || mglastaccessed='-'";

            try
            {
                MySqlCommand c1 = new MySqlCommand(sqlCmd1, _mconn), c2 = new MySqlCommand(sqlCmd2, _mconn);

                c1.ExecuteNonQuery();
                c2.ExecuteNonQuery();
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur au chargement");
                _dlgResult = System.Windows.Forms.DialogResult.Abort;
            }

            _dlgResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _dlgResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        #endregion
    }
}