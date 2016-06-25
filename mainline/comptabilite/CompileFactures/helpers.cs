// comptabilite : CompileFacturesUI
// Écrit par jfgratton (Jean-François Gratton), 2013.06.13 @ 02:45
// 
// helpers.cs : diverses commandes d'aide

using System;
using System.Data;
using System.Windows;
using JFG.mySQLhelper;
using MySql.Data.MySqlClient;

namespace JFG.Comptes
{
    public partial class CompileFactures
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        
        private int CheckIfItemExists(string item)
        {
            bool bFound = false;
            var cmd = new MySqlCommand("SELECT pNom FROM _postesbudgetaires WHERE pNom LIKE '%" + item.ToLower() + "%'", _mconn);
            MySqlDataReader rdr = null;

            try
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                }
                bFound = rdr.RecordsAffected > 0;
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }
            catch (Exception x)
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
            if (bFound == false)
                InsereNouveauPosteBudgetaire(TxBxAutreTypeAchat.Text);

            return IndexPosteBudgetaire(TxBxAutreTypeAchat.Text);
        }

        private void ResetForm()
        {
            _sCommentaires = "";
            ++_nEntree;
            rbEpicerie.IsChecked = rbVD.IsChecked = true;
            sbiNombreEntrees.Content = "Entrée # " + _nEntree;
            txbxAmount.Text = txbxDate.Text = String.Empty;
        }

        private void ParseCommandLine(string[] args)
        {
            int n = 0;
            bool bCompleted = false, bCompleteString = false;
            string host = "oslo", port = "3306", user = "comptes", password = "comptes", database = "comptabilite";
            //ConnectString=@"User Id=comptes;Password=comptes;Host=oslo;Port=3306;Database=comptabilite;";

            while (n < args.Length && bCompleted == false)
            {
                switch (args[n])
                {
                    case "-?":
                        _bHelpNeeded = bCompleted = true;
                        break;
                    case "-X":
                        switch (args[n + 1].ToLower())
                        {
                            case "remote":
                                _connectString =
                                    @"User Id=C280972_accnt;Password=JfG!2937;Host=comptes.famillegratton.net;Port=3306;Database=C280972_comptabilite;";
                                _nTargetDB = 0;
                                rbHE.IsChecked = bCompleted = true;
                                break;
                            case "oslo-pub":
                                _connectString = @"User Id=comptes;Password=comptes;Host=oslo.famillegratton.net;Port=3360;Database=comptabilite;";
                                _nTargetDB = 1;
                                rbOsloPub.IsChecked = bCompleted = true;
                                break;
                            case "oslo":
                            case "oslo-lan":
                                _connectString = @"User Id=comptes;Password=comptes;Host=oslo;Port=3306;Database=comptabilite;";
                                _nTargetDB = 2;
                                rbOsloLAN.IsChecked = bCompleted = true;
                                break;
                            case "local":
                            case "localhost":
                            case "127.0.0.1":
                                _connectString = @"User Id=comptes;Password=comptes;Host=localhost;Port=3306;Database=comptabilite;";
                                _nTargetDB = 3;
                                rbLocalhost.IsChecked = bCompleted = true;
                                break;
                        }
                        if (n + 2 < args.Length && args[n + 2] == "-L")
                            _bLockConnection = true;
                        break;
                    case "-u":
                        user = args[n + 1];
                        ++n;
                        break;
                    case "-p":
                        password = args[n + 1];
                        ++n;
                        break;
                    case "-d":
                        database = args[n + 1];
                        ++n;
                        break;
                    case "-h":
                        host = args[n + 1].ToLower();
                        ++n;
                        break;
                    case "-P":
                        port = args[n + 1];
                        ++n;
                        break;
                    case "-x":
                        _connectString = args[n + 1];
                        ++n;
                        bCompleteString = true;
                        break;
                    default:
                        ++n;
                        break;
                }
            }
            if (bCompleted)
                return;
            if (bCompleteString == false)
            {
                _connectString = @"User Id=" + user + ";Password=" + password + ";Host=" + host + ";Port=" + port +
                                 ";Database=" + database + ";";
                rbOsloLAN.IsChecked = rbOsloPub.IsChecked = rbHE.IsChecked = rbLocalhost.IsChecked = false;
            }
        }

        private bool Reconnect(string cstring)
        {
            bool bOK;

            var sqlConn = new ConnectionHelper(cstring);
            if (sqlConn.OpenConnection() == false)
            {
                MessageBox.Show(sqlConn.GetExceptionMessage(), "Échec à la reconnexion");
                bOK = false;
            }
            else
            {
                sqlConn.CloseConnection();
                _connectString = cstring;
                _nEntree = 0;
                bOK = InitSQLConnection();
            }
            return bOK;
        }

        private void InsereNouveauPosteBudgetaire(string poste)
        {
            _sqlConn.Execute("INSERT INTO _postesbudgetaires (pNow) VALUES ('" + poste + "')");
            _sqlConn.ResetAutoIncrement("comptabilite", "_postesbudgetaires", "pID");
        }

        private int IndexPosteBudgetaire(string poste)
        {
            int nIndex = -1;
            bool bFound = false;
            var cmd = new MySqlCommand("SELECT pID FROM _postesbudgetaires WHERE pNom='" + poste + "'", _mconn);
            MySqlDataReader rdr = null;

            try
            {
                rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (rdr.Read())
                    nIndex = rdr.GetInt32(0);
                bFound = rdr.RecordsAffected > 0;
            }
            catch (MySqlException mex)
            {
                MessageBox.Show(mex.Message, "Erreur SQL");
            }
            catch (Exception x)
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
            if (bFound == false)
                nIndex = -1;

            return nIndex;
        }
    }
}