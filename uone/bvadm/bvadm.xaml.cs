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
using JFG.dialogs;

namespace uOne.BVadm
{
    public partial class SearchBV : Window
    {
        private SkinSelectionEnum _skin;

        public SearchBV()
        {
            InitializeComponent();
            //string[] args = Environment.GetCommandLineArgs();
            _skin = parseCommandLineArgs(Environment.GetCommandLineArgs());
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
