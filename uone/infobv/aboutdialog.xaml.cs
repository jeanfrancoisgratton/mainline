// uOne : infoBV
// aboutDialog.xaml.cs
// 
// Écrit par J.F. Gratton, 2012.03.13

// Gestion de la boite de dialog "about this app"

using System;
using System.Reflection;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace uOne.infoBV
{
    /// <summary>
    /// Interaction logic for aboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
            lblAboutApp.Content = "InfoBV v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public new bool? ShowDialog()
        {
            bool bRes = true;
            StreamReader sr = null;
            
            try
            {
                Assembly _assembly = Assembly.GetExecutingAssembly();
                sr = new StreamReader(_assembly.GetManifestResourceStream("uOne.infoBV.Resources.CHANGELOG.txt"));
                string sLine;

                while ((sLine = sr.ReadLine()) != null)
                    tbChangeLog.Text += sLine + Environment.NewLine;
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

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}