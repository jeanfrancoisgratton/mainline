// uOne : BVadm
// commandLineParser.cs
// 
// Écrit par J.F. Gratton, 2012.03.01

using System.Windows;
using JFG.dialogs;

// arguments possibles:
// -theme {yellow|black|acision|random}
// -xml xml file
// -dbh database host
// -dbp database port
// -dbn database name
// -dsh ds host
// -dsp ds port
// -bdn ds base dn

namespace uOne.BVadm
{
    public partial class SearchBV
    {
        public SkinSelectionEnum parseCommandLineArgs(string[] args)
        {
            return SkinSelectionEnum.none;
        }
    }
}