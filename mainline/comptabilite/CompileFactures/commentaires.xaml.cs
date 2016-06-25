// comptabilite : compilefactures
// CommentWindow.xaml.cs : ajout de commentaires à l'entrée
// 2013.07.24 21:22, Jean-Francois Gratton

using System.Windows;

namespace JFG.Comptes
{
    /// <summary>
    /// Interaction logic for CommentWindow.xaml
    /// </summary>
    public partial class CommentWindow : Window
    {
        private string _commentaires;
        private bool _resultat;

        public CommentWindow()
        {
            InitializeComponent();
            _commentaires = "";
        }

        private void btnAddCmt_Click(object sender, RoutedEventArgs e)
        {
            _resultat = true;
            _commentaires = TxbxCommentaires.Text.Length > 1024 ? TxbxCommentaires.Text.Substring(0, 1024) : TxbxCommentaires.Text;
            Close();
        }

        private void btnCancelCmt_Click(object sender, RoutedEventArgs e)
        {
            _resultat = false;
            _commentaires = "";
            Close();
        }

        public bool? ShowDialog(ref string commentaires)
        {
            base.ShowDialog();
            commentaires = _commentaires;

            return _resultat;
        }
    }
}