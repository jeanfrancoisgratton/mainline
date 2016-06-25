// Comptabilite : comptes
// Écrit par J.F.Gratton, 2012.09.09
// 
// postesBudgetaires.xaml.cs : ajout/retrait/modification des postes budgétaires

using System;
using System.Collections;
using System.Windows;
using MySql.Data.MySqlClient;

// ReSharper disable CheckNamespace
namespace JFG.Comptes
// ReSharper restore CheckNamespace
{
    public partial class PostesBudgetaires
    {
        //public enum PosteOperationEnum
        //{
        //    supprime = -1,
        //    modifie = 0,
        //    ajoute = 1
        //};
        //private PosteOperationEnum op;

        private readonly MySqlConnection _sqlConn;
        private readonly ArrayList _alPostes;
        

        #region INIT + CLEANUP
        public PostesBudgetaires(MySqlConnection conn)
        {
            _sqlConn = conn;
            _alPostes = new ArrayList();
            InitializeComponent();
        }

        private void PostesWindowLoaded(object sender, RoutedEventArgs e)
        {
            RafraichitListePostes();
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion INIT + CLEANUP

        //!+    RafraichitListePostes():
        //!     Rafraichit le listbox contenant les postes budgétaires
        private void RafraichitListePostes()
        {
            var cmd = new MySqlCommand("SELECT pbPoste FROM _postesbudgetaires ORDER BY pbPoste ASC", _sqlConn);
            MySqlDataReader rdr = null;
            try
            {
                _alPostes.Clear();
                lbPostes.Items.Clear();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    _alPostes.Add(rdr[0].ToString().ToLower());
                    lbPostes.Items.Add(rdr[0].ToString());
                }
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }

            catch(Exception x)
            {
                MessageBox.Show(x.Message, "Erreur");
            }
            finally
            {
                if (rdr != null && rdr.IsClosed == false)
                {
                    rdr.Close();
                    rdr.Dispose();
                }
            }
        }

        #region BOUTONS

        //!+    btnSupprimerPoste_Click():
        //!     Supprime le poste sélectionné
        private void BtnSupprimerPosteClick(object sender, RoutedEventArgs e)
        {
            if (lbPostes.SelectedIndex == -1)
                return;
            string sDelString = "DELETE FROM _postesbudgetaires WHERE pbPoste = '" + lbPostes.SelectedItem + "'";
            var cmd = new MySqlCommand(sDelString, _sqlConn);

            try
            {
                cmd.ExecuteNonQuery();
                lbPostes.Items.Clear();
                RafraichitListePostes();
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }
        }

        //!+    btnModifierPoste_Click():
        //!     Modifie le poste sélectionné
        private void BtnModifierPosteClick(object sender, RoutedEventArgs e)
        {
            if (lbPostes.SelectedIndex == -1)
                return;
            var modPos = new ModifierPoste(lbPostes.SelectedItem.ToString(), _alPostes, _sqlConn);
            modPos.ShowDialog();
            RafraichitListePostes();
        }

        //!+    btnAjouterPoste_Click():
        //!     Ajoute un nouveau poste à la BD
        private void BtnAjouterPosteClick(object sender, RoutedEventArgs e)
        {
            var addP = new AjouterPoste(_alPostes, _sqlConn);
            addP.ShowDialog();
            RafraichitListePostes();
        }
        #endregion BOUTONS
    }
}