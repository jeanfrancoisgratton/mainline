// Comptabilite : comptes
// Écrit par J.F.Gratton, 2012.08.24
// 
// aboutDlg.xaml.cs : boîte de dialogue "à propos de ..."

using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace JFG.Comptes
{
    public partial class aboutDlg : Window
    {
        #region INIT + CLEANUP
        public aboutDlg()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Title += "Comptabilité v" + Assembly.GetExecutingAssembly().GetName().Version;
        }
        #endregion
        public new bool? ShowDialog()
        {
            StreamReader sr = null;
            bool bRes = true;

            try
            {
                Assembly _assembly = Assembly.GetExecutingAssembly();
                sr = new StreamReader(_assembly.GetManifestResourceStream("JFG.Comptes.Resources.changelog.txt"));
                string sLine;

                while ((sLine = sr.ReadLine()) != null)
                    tbChangelog.Text += sLine + Environment.NewLine;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bRes = false;
            }

            finally
            {
                if (sr != null) sr.Close();
            }

            bRes = (bool) base.ShowDialog();
            return bRes;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}