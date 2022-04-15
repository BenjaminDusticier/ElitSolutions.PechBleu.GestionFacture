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
            ObjVerEx oFac = new ObjVerEx();
            int idFac = -1;
            DateTime dateFacture;
            int numTiers = -1;
            int numPiece;
            int numPiecePrec = -1;

            // Initialise la première ligne du tableau
            string ec = lignes[0] + '\n';

            // Pour chaque ligne du contenu texte.
            for (int i = 1; i < lignes.Length; i++)
            {
                // Enregistre la ligne.
                ec += lignes[i] + '\n';
            
                // Copie les chaînes séparées par tabulation dans un tableau.
                string[] valeurs = lignes[i].Split('\t');

                // Mémorise le tiers si pas vide
                if(valeurs[6] != "")
                {
                    // Récupère le numéro de tiers.
                    numTiers = int.Parse(valeurs[6]);
                }
                
                // Récupère le numéro de pièce.
                numPiece = int.Parse(valeurs[0]);

                // Passe à la ligne suivante si le numéro de pièce n'a pas changé ou si on est à la première ligne
                if ( numPiecePrec == numPiece || i == 1)
                {   
                    // Mémorise le numéro de pièce.
                    numPiecePrec = numPiece; 
                    
                    // Passe à la ligne suivante
                    continue; 
                }

                // Récupère le numéro de facture.
                numFac = int.Parse(valeurs[3]);

                // Récupère la date de la facture
                dateFacture = DateTime.Parse(valeurs[2]);

                // Cherche le fournisseur PFO.
                var searchBuilder = new MFSearchBuilder(env.Vault);
                searchBuilder.Class(this.Configuration.ClassFournisseurPFO);
                searchBuilder.Status(MFStatusType.MFStatusTypeExtID, MFDataType.MFDatatypeInteger, numTiers);
                searchBuilder.Deleted(false);
                var searchResults = searchBuilder.FindEx();

                // Si au moins un résultat.
                if ( searchResults.Count > 0) 
                {
                    // Récupère l'ID du fournisseur trouvé.
                    int idFou = searchResults[0].ID;

                    // Cherche la facture.
                    searchBuilder = new MFSearchBuilder(env.Vault);
                    searchBuilder.Class(this.Configuration.ClassFacture);
                    searchBuilder.Property(this.Configuration.PropDefFournisseurPFO, MFDataType.MFDatatypeLookup, idFou);
                    searchBuilder.Property(this.Configuration.PropDefNumFac, MFDataType.MFDatatypeText, numFac);
                    searchBuilder.Property(this.Configuration.PropDefDateFact, MFDataType.MFDatatypeDate, dateFacture);
                    searchBuilder.Property(this.Configuration.PropDefSociete, MFDataType.MFDatatypeLookup, this.Configuration.idSocietePFO);
                    searchBuilder.Deleted(false);
                    searchResults = searchBuilder.FindEx();

                    // Si au moins un résultat.
                    if (searchResults.Count > 0)
                    {
                        // Récupère l'ID de la facture trouvée.
                        idFac = searchResults[0].ID;
                        oFac = searchResults[0];
                    }
                }

                // Cherche le fournisseur YEDRA.
                searchBuilder = new MFSearchBuilder(env.Vault);
                searchBuilder.Class(this.Configuration.ClassFournisseurYEDRA);
                searchBuilder.Status(MFStatusType.MFStatusTypeExtID, MFDataType.MFDatatypeInteger, numTiers);
                searchBuilder.Deleted(false);
                searchResults = searchBuilder.FindEx();

                // Si au moins un résultat.
                if (searchResults.Count > 0)
                {
                    // Récupère l'ID du fournisseur trouvé.
                    int idFou = searchResults[0].ID;

                    // Cherche la facture.
                    searchBuilder = new MFSearchBuilder(env.Vault);
                    searchBuilder.Class(this.Configuration.ClassFacture);
                    searchBuilder.Property(this.Configuration.PropDefFournisseurYEDRA, MFDataType.MFDatatypeLookup, idFou);
                    searchBuilder.Property(this.Configuration.PropDefNumFac, MFDataType.MFDatatypeText, numFac);
                    searchBuilder.Property(this.Configuration.PropDefDateFact, MFDataType.MFDatatypeDate, dateFacture);
                    searchBuilder.Property(this.Configuration.PropDefSociete, MFDataType.MFDatatypeLookup, this.Configuration.idSocieteYEDRA);
                    searchBuilder.Deleted(false);
                    searchResults = searchBuilder.FindEx();

                    // Si au moins un résultat.
                    if (searchResults.Count > 0)
                    {
                        if (idFac == -1)
                        {
                            // Récupère l'ID de la facture trouvée.
                            idFac = searchResults[0].ID;
                            oFac = searchResults[0];
                        }
                        else
                        {
                            idFac = -1;
                        }
                    }
                }

                // Crée le fichier temporaire.
                string tempFileName = Path.GetTempFileName();

                // Ajoute les écritures au fichier temporaire.
                StreamWriter streamWriter = File.AppendText(tempFileName);
                streamWriter.Write(ec);
                streamWriter.Flush();
                streamWriter.Close();

                // Crée le document.

                // Create a property values builder.
                var oPropValBuilder = new MFPropertyValuesBuilder(env.Vault)
                    .SetClass(this.Configuration.ClassEC.ID)
                    .SetWorkflowState(this.Configuration.WFEC, this.Configuration.StateEC)
                    .SetTitle("EC " + numFac + " " + numTiers + " " + dateFacture.ToString("dd/MM/yyyy"));

                // Use the values to create an object.
                int idEC = env.Vault.ObjectOperations.CreateNewObjectExQuick((int)MFBuiltInObjectType.MFBuiltInObjectTypeDocument, oPropValBuilder.Values);

                //
                if(idFac != -1)
                {
                    //
                    oFac.SaveProperty(this.Configuration.PropDefEC, MFDataType.MFDatatypeLookup, idEC);
                }

                // Supprime le fichier temporaire s'il existe.
                if (File.Exists(tempFileName)) { File.Delete(tempFileName); }

                // Réinitialise l'id
                idFac = -1;

            }
            //throw new System.NotImplementedException();
        }
    }
}