using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace JFG.Cardio
{
    /// <summary>
    /// This is the "about" dialog box
    /// </summary>
    public partial class AboutDlg : Window
    {
        public AboutDlg()
        {
            InitializeComponent();
        }

        public new bool? ShowDialog()
        {
            StreamReader sr = null;
            bool bRes = true;

            try
            {
                //Assembly _assembly = Assembly.GetExecutingAssembly();
                sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("JFG.Cardio.Resources.changelog.txt"));
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
            if (showDialog != null) bRes = (bool)showDialog;
            return bRes;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Gestion des appels v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
