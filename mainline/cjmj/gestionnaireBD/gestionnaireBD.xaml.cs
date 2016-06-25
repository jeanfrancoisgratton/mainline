// CJMontJoli : gestionnaireBD
// Écrit par J.F. Gratton, 2012.07.29
// 
// gestionnaireBD.xaml.cs : page principale du gestionnaire de la base de données

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cjmj.gestionnaireBD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class gestionnaireBD : Window
    {
        public gestionnaireBD()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Menus

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            aboutDialog about = new aboutDialog();
            about.ShowDialog();
        }

        private void mnuLoadXML_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void mnuSaveXML_Click(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion Menus
    }
}