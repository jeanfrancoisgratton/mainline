// Comptabilite : comptes
// Écrit par J.F. Gratton, 2012.09.18
// 
// modifierPoste.xaml.cs : modification d'un poste budgétaire

using System;
using System.Collections;
using System.Windows;
using MySql.Data.MySqlClient;

// ReSharper disable CheckNamespace
namespace JFG.Comptes
// ReSharper restore CheckNamespace
{
    public partial class ModifierPoste
    {
        private readonly MySqlConnection _connection;
        private readonly ArrayList _alPostes;
        private readonly string _sPosteOriginal;

        #region INIT + CLEANUP
        public ModifierPoste(string itemToMod, ArrayList al, MySqlConnection s)
        {
            _sPosteOriginal = itemToMod;
            _alPostes = al;
            _alPostes.Sort();
            _connection = s;
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            lblPoste.Content = _sPosteOriginal;
        }

        private void BtnFermerClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        private void BtnModifierClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbNewPoste.Text))
                return;

            if (_alPostes.Contains(tbNewPoste.Text.ToLower()))
            {
                MessageBox.Show(tbNewPoste.Text + " est déjà dans la BD", "Duplicata d'entrée");
                return;
            }

            string sCmd = "UPDATE postesbudgetaires SET pbPoste='" + tbNewPoste.Text +
                "' WHERE pbposte='" + lblPoste.Content + "'";
            var cmd = new MySqlCommand(sCmd, _connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }

            finally
            {
                Close();
            }
        }
    }
}