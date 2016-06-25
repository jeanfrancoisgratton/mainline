// comptabilite : compilefactures
// Écrit par jfgratton (Jean-François Gratton), 2013.07.04 @ 19:34
// 
// aboutDlg.xaml.cs : boite "a propos..."

using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace JFG.Comptes
{
    public partial class AboutDlg : Window
    {
        public AboutDlg()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

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

            var showDialog = base.ShowDialog();
            if (showDialog != null) bRes = (bool) showDialog;
            return bRes;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "CompileFactures v" + Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}