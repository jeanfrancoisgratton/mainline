// comptabilite : compilefactures
// helpDlg.xaml.cs : boite d'aide
// 2013.07.07 21:30, Jean-Francois Gratton

using System;
using System.Windows;

namespace JFG.Comptes
{
    /// <summary>
    /// Interaction logic for helpDlg.xaml
    /// </summary>
    public partial class helpDlg : Window
    {
        public helpDlg()
        {
            InitializeComponent();
        }

        public new bool? ShowDialog()
        {
            TxbxHelp.Clear();
            TxbxHelp.Text = "Paramètres:" + Environment.NewLine + Environment.NewLine;
            TxbxHelp.Text += "-X remote : Connexion à comptes.famillegratton.net" + Environment.NewLine;
            TxbxHelp.Text += "-X oslo-pub : Connexion à oslo via l'adresse publique" + Environment.NewLine;
            TxbxHelp.Text += "-X {oslo | oslo-pub} : Connexion à oslo via l'adresse privée" + Environment.NewLine;
            TxbxHelp.Text += "-X {local | localhost | 127.0.0.1} : Connexion locale" + Environment.NewLine;
            TxbxHelp.Text += "-u username" + Environment.NewLine;
            TxbxHelp.Text += "-p password" + Environment.NewLine;
            TxbxHelp.Text += "-d database" + Environment.NewLine;
            TxbxHelp.Text += "-h host" + Environment.NewLine;
            TxbxHelp.Text += "-P port" + Environment.NewLine;
            TxbxHelp.Text += "-x connectString" + Environment.NewLine + Environment.NewLine;
            TxbxHelp.Text += "connectString :" + Environment.NewLine +
                             "'User ID=_user_;Password=_passwd_;Host=_host_;Port=_port_;Database=_db_;'";
            return base.ShowDialog();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}