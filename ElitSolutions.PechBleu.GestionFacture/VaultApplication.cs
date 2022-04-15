using MFiles.VAF;
using MFiles.VAF.AppTasks;
using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Core;
using MFilesAPI;
using System;
using System.Diagnostics;
using System.IO;

namespace ElitSolutions.PechBleu.GestionFacture
{
    /// <summary>
    /// The entry point for this Vault Application Framework application.
    /// </summary>
    /// <remarks>Examples and further information available on the developer portal: http://developer.m-files.com/. </remarks>
    public class VaultApplication
        : ConfigurableVaultApplicationBase<Configuration>
    {
        /// <summary>
        /// Executed when an object is moved into a workflow state
        /// with alias "WFS.WfTest.001".
        /// </summary>
        /// <param name="env">The vault/object environment.</param>
        [StateAction("WFS.WfTest.001")]
        public void MyWorkflowStateHandler(StateEnvironment env)
        {

            //
            SysUtils.ReportInfoToEventLog("Toto");
            SysUtils.ReportToEventLog("Tata", EventLogEntryType.Warning);

            // Récupère le filever de l'objet.
            ObjectFiles objFiles = env.Vault.ObjectFileOperations.GetFiles(env.ObjVer);
            ObjectFile objFile = objFiles[0];
            FileVer fileVer = objFile.FileVer;

            // Récupère le contenu texte.
            string contenu = env.Vault.ObjectFileOperations.GetTextContentForFile(env.ObjVer, fileVer);

            // Copie les lignes du contenu dans un tableau .
            string[] lignes = contenu.Split('\n');

            //
            int numFac;
            int numTiers;
            int numLot;
            int numLotPrec = -1;

            //
            string ec = "";

            // Pour chaque ligne du contenu texte.
            foreach ( string ligne in lignes )
            {
                // Enregistre la ligne.
                ec += ligne + '\n';

                // Copie les chaînes séparées par tabulation dans un tableau.
                string[] valeurs = ligne.Split('\t');

                // Récupère le numéro de facture.
                numFac = int.Parse(valeurs[3]);

                // Récupère le numéro de tiers.
                numTiers = int.Parse(valeurs[6]);

                // Récupère le numéro de lot.
                numLot = int.Parse(valeurs[0]);

                // Passe à la ligne suivante si le numéro de lot a changé.
                if ( numLotPrec == numLot ) { continue; }

                // Mémorise le numéro de lot.
                numLotPrec = numLot;

                // Cherche le fournisseur.
                var searchBuilder = new MFSearchBuilder(env.Vault);
                searchBuilder.Class(this.Configuration.ClassFournisseur);
                searchBuilder.Property(this.Configuration.PropDefNumFou, MFDataType.MFDatatypeText, numTiers);
                //// Only items with an external ID (display ID) of "SUP12345".
                //searchBuilder.Status(MFStatusType.MFStatusTypeExtID, MFDataType.MFDataTypeText, "SUP12345")
                searchBuilder.Deleted(false);
                var searchResults = searchBuilder.FindEx();

                // Passe à la ligne suivante si aucun résultat.
                if ( searchResults.Count <= 0) { continue; }
                    
                // Récupère l'ID du fournisseur trouvé.
                int idFou = searchResults[0].ID;

                // Cherche la facture.
                searchBuilder = new MFSearchBuilder(env.Vault);
                searchBuilder.Class(this.Configuration.ClassFacture);
                searchBuilder.Property(this.Configuration.PropDefFournisseur, MFDataType.MFDatatypeLookup, idFou);
                searchBuilder.Property(this.Configuration.PropDefNumFac, MFDataType.MFDatatypeText, numFac);
                searchBuilder.Deleted(false);
                searchResults = searchBuilder.FindEx();

                // Passe à la ligne suivante si aucun résultat.
                if (searchResults.Count <= 0) { continue; }

                //// Crée le fichier temporaire.
                //string tempFileName = Path.GetTempFileName();

                //// Ajoute les écritures au fichier temporaire.
                //StreamWriter streamWriter = File.AppendText(tempFileName);
                //streamWriter.Write(ec);
                //streamWriter.Flush();
                //streamWriter.Close();

                //// Crée le document.

                //// Supprime le fichier temporaire s'il existe.
                //if ( File.Exists(tempFileName) ) { File.Delete(tempFileName); }
            }
            //throw new System.NotImplementedException();
        }
    }
}