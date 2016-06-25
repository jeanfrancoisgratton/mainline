// comptabilite : comptes
// Écrit par : J.F.Gratton (jfgratton), 2012.11.04
// 
// LoginDialog.xaml.cs : fenêtre de connexion

using System.Windows;

namespace JFG.Comptes
{
    public partial class LoginDialog : Window
    {
        private DbStruct _dbstructure;

        public LoginDialog(DbStruct dbs)
        {
            _dbstructure = dbs;
            InitializeComponent();
        }

        private void BtnConnectClick(object sender, RoutedEventArgs e)
        {
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
        }

        public void ShowDialog(ref DbStruct dbs)
        {
            if (string.IsNullOrWhiteSpace(tbBD.Text) || string.IsNullOrWhiteSpace(tbHost.Text) ||
                string.IsNullOrWhiteSpace(tbPort.Text) || string.IsNullOrWhiteSpace(tbUser.Text) ||
                string.IsNullOrWhiteSpace(pwdbx.Password))
                return;

        }
    }
}