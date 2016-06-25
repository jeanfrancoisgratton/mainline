// CardioPleinAir : gestionAppels
// Écrit par : jfgratton (), 2014.10.02 @ 13:24
// 
// gestionAppels.xaml.cs : Main code file

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace JFG.Cardio
{
    public struct ConnectionStruct
    {
        public string DB;
        public string Password;
        public int Port;
        public string Server;
        public string Username;
    };
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GestionAppels : Window
    {
        /// <summary>
        /// </summary>
        public static RoutedCommand rcCommandI = new RoutedCommand(),
            rcCommandR = new RoutedCommand(),
            rcCommandH = new RoutedCommand();
        private ConnectionStruct ConnectionInfo;
        private string _notConnectedMessage;
        private string _validationMessage;
        private bool _bConnected;

        #region INIT

        /// <summary>
        ///     Gestion des nouveaux abonnements
        /// </summary>
        public GestionAppels()
        {
            InitializeComponent();
            _bConnected = false;
            _notConnectedMessage = "NON-CONNECTÉ. CLIQUEZ POUR VOUS CONNECTER.";
            lblUserBD.Content = @"cardio@oslo:3306/cardio_dev";
            rcCommandI.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Alt)); // Insert
            rcCommandR.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Alt)); // Reset form
            rcCommandH.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Alt)); // Help
            txtblkConn.Background = new SolidColorBrush(Colors.Red);
            txtblkConn.Foreground = new SolidColorBrush(Colors.White);
            txtblkConn.Text = _notConnectedMessage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion INIT

        #region BOUTONS

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (EmailValidator(txtbxEmail.Text) == false)
                MessageBox.Show(_validationMessage, "Invalid email");

            Environment.Exit(0); //! TEMP
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutdlg = new AboutDlg();

            aboutdlg.ShowDialog();
        }

        private void btnConnTest_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txbxServer.Text) || String.IsNullOrWhiteSpace(txbxPort.Text) ||
                String.IsNullOrWhiteSpace(txbxBD.Text) || String.IsNullOrWhiteSpace(txbxUser.Text) ||
                String.IsNullOrWhiteSpace(pwdbxPasswd.Password))
            {
                MessageBox.Show("Tout les champs doivent être remplis");
                _bConnected = false;
            }
            else
            {
                ConnectionInfo.Server = txbxServer.Text;
                if (Int32.TryParse(txbxPort.Text, out ConnectionInfo.Port) == false)
                    ConnectionInfo.Port = 3306;
                ConnectionInfo.DB = txbxBD.Text;
                ConnectionInfo.Username = txbxUser.Text;
                ConnectionInfo.Password = pwdbxPasswd.Password;

                _bConnected = TryConnection(ConnectionInfo);
            }
        }

        #endregion BOUTONS

        #region VALIDATORS

        private bool EmailValidator(string emailAddress)
        {
            if (emailAddress == "")
                return true;

            if (emailAddress.IndexOf("@", StringComparison.Ordinal) > -1)
            {
                if (
                    emailAddress.IndexOf(".", emailAddress.IndexOf("@", StringComparison.Ordinal),
                        StringComparison.Ordinal) > emailAddress.IndexOf("@", StringComparison.Ordinal))
                {
                    _validationMessage = "";
                    return true;
                }
            }
            _validationMessage = "email address is invalid";
            return false;
        }

        #endregion VALIDATORS

        #region HOTKEYS

        private void InsertData(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("INSERT");
        }

        private void ResetForm(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("RESET");
        }

        private void Help(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("HELP");
        }

        #endregion HOTKEYS

        private new void ConnectionTabMouseDown(object sender, MouseButtonEventArgs e)
        {
            tabctrl1.SelectedIndex = 1;
        }
    }
}