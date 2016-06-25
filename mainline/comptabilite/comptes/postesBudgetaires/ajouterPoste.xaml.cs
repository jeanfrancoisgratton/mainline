// Comptabilite : comptes
// Écrit par J.F.Gratton, 2012.09.15
// 
// ajouterPoste.xaml.cs : ajout d'un poste budgétaire à la BD

using System;
using System.Collections;
using System.Windows;
using MySql.Data.MySqlClient;

// ReSharper disable CheckNamespace
namespace JFG.Comptes
// ReSharper restore CheckNamespace
{
    public partial class AjouterPoste
    {
        private readonly MySqlConnection _connection;
        private readonly ArrayList _alPostes;
        
        public AjouterPoste(ArrayList al, MySqlConnection s)
        {
            _alPostes = al;
            _alPostes.Sort();
            _connection = s;
            InitializeComponent();
        }

        private void BtnInsertClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbNouveauPoste.Text))
                return;

            if (_alPostes.Contains(tbNouveauPoste.Text.ToLower()))
            {
                MessageBox.Show(tbNouveauPoste.Text + " est déjà dans la BD", "Duplicata d'entrée");
                return;
            }

            var cmd = new MySqlCommand("INSERT INTO postesbudgetaires (pbPoste) VALUES ('" +
                tbNouveauPoste.Text + "')", _connection);

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

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}