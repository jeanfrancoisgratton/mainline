// uOne : infoBV
// ldapsearchResults.xaml.cs
// 
// Écrit par J.F. Gratton, 2012.03.09

// ldapSearchResults : "dissection" de la structure de données
// LdapReturnStruct pour en afficher le contenu de façon "human-readable"

using System;
using System.Collections;
using System.Windows;
using JFG.ldap.dsFx;

namespace uOne.infoBV
{
    /// <summary>
    /// Interaction logic for ldapsearchResults.xaml
    /// </summary>
    public partial class ldapsearchResults : Window
    {
        public ldapsearchResults()
        {
            InitializeComponent();
        }

        public void Show(ArrayList al)
        {
            afficheResultats(al);
            Show();
        }

        private void afficheResultats(ArrayList resultats)
        {
            // Première loupe : on énumère toutes les entrées trouvées
            for (int i = 0; i < resultats.Count; i++)
            {
                LdapReturnStruct ldr = (LdapReturnStruct) resultats[i];
                tbResults.Text = "dn: " + ldr.dn + Environment.NewLine;
                // Seconde loupe : tout les attributs trouvés pour l'entrée
                for (int j = 0; j < ldr.alAttributs.Count; j++)
                {
                    string[] k = (string[]) ldr.alAttributs[j];
                    // Dernière loupe: toutes les valeurs de cet attribut
                    for (int m = 1; m < k.Length; m++)
                        tbResults.Text += k[0] + ": " + k[m] + Environment.NewLine;
                }
                Console.WriteLine();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}