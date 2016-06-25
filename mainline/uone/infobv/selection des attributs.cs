// uOne : infoBV
// selection des attributs.cs
// 
// Écrit par J.F. Gratton, 2012.03.05

// Sélection de quels attributs afficher pour les filtres, selon le radio button choisi


using System;
using System.Collections;
using System.Windows;

namespace uOne.infoBV
{
    public partial class SearchBv
    {
        //! rbTelephonenumber_Checked():
        // Les attributs associés à telephonenumber seront affichés
        private void rbTelephonenumber_Checked(object sender, RoutedEventArgs e)
        {
            _nBranche = 0;
            _attributs = null;
        }

        //! rbUmbillingnumber_Checked():
        // Les attributs associés à umbillingnumber seront affichés
        private void rbUmbillingnumber_Checked(object sender, RoutedEventArgs e)
        {
            _nBranche = 1;
            _attributs = null;
        }

        private void rbTelephonenumberMost_Checked(object sender, RoutedEventArgs e)
        {
            _nBranche = 2;
            _attributs = new string[] { "ummsghost", "umcallanswerlang", "ummwienabled", "mailforwardingaddress", "umcosdn", "umgreetinfo", "mailHost", "uid" };
        }


        private void rbUmbillingnumberMost_Checked(object sender, RoutedEventArgs e)
        {
            _nBranche = 3;
            _attributs = new string[] { "umuserlevel", "ummwiswitch", "umsmsccenterid", "umbadlogincount", "umlockouttimestamp", "umgreetings", "umactivegreetingid", "umphonetype" };
        }

        
    }
}