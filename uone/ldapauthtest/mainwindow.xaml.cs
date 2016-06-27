using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using JFG.ldap.dsFx;

namespace ldapAuthTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int nPort;
        public MainWindow()
        {
            InitializeComponent();
            nPort = 389;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (validateFields() == false)
            {
                MessageBox.Show("Les champs ne sont pas remplis correctement");
                return;
            }

            string s = "";
            bool b = LdapFunctions.CheckAuth(textBox1.Text, nPort, textBox3.Text, textBox4.Text, passwordBox1.Password, ref s);

            if (b)
                MessageBox.Show("TRUE");
            else
                MessageBox.Show(s, "FALSE");
        }

        private bool validateFields()
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(textBox2.Text) || String.IsNullOrWhiteSpace(textBox3.Text)
                || String.IsNullOrWhiteSpace(textBox4.Text) || String.IsNullOrWhiteSpace(passwordBox1.Password))
                return false;
            if (Int32.TryParse(textBox2.Text, out nPort) == false)
                return false;

            if (nPort < 1 || nPort > 65535)
                return false;

            return true;
        }
    }
}
