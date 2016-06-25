// uOne : infoBV
// infoBV.xaml.cs
// 
// Écrit par J.F. Gratton, 2012.03.01
//
// Fichier d'entrée 

using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using JFG.ldap.dsFx;

namespace uOne.infoBV
{
    public partial class SearchBv : Window
    {
        private string[] _attributs;
        private int _nBranche; // 0 = telephonenumber, 1 = umbillingnumber
        private string sNum;

        public SearchBv()
        {
            _nBranche = 0; // branche choisie = telephonenumber
            string[] args = Environment.GetCommandLineArgs();
            sNum = args.Length > 1 ? args[1] : "";
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            tbMSISDN.Text = sNum;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string sE = ValidationDesChamps();
            if (String.IsNullOrWhiteSpace(sE) == false)
            {
                MessageBox.Show(sE);
                return;
            }

            int nePort = Int32.Parse(tbPort.Text);

            string recherche = (_nBranche == 0 || _nBranche == 2) ? "telephonenumber=" : "umbillingnumber=";
            LdapFunctions ldf = new LdapFunctions(tbDS.Text, nePort, "o=isp");
            ArrayList al = ldf.ldapSearch(recherche + tbMSISDN.Text, _attributs);

            ldapsearchResults ldrWindow = new ldapsearchResults();
            ldrWindow.Show(al);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog abtdlg = new AboutDialog();

            abtdlg.ShowDialog();
        }

        private string FormatteMSISDN()
        {
            tbMSISDN.Text = tbMSISDN.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "");
            return tbMSISDN.Text;
        }
        
        private string ValidationDesChamps()
        {
            string sErreur = null;
            int nPort;

            if (String.IsNullOrWhiteSpace(tbDS.Text) || String.IsNullOrWhiteSpace(FormatteMSISDN()) || String.IsNullOrWhiteSpace(tbPort.Text))
                sErreur = "Les champs DS, Port et MSISDN doivent contenir des infos";

            if (Int32.TryParse(tbPort.Text, NumberStyles.None & NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out nPort) && (nPort < 1 || nPort > 65535))
                sErreur = "Le numéro de port doit être compris entre 1 et 65535";

            return sErreur;
        }
    }
}