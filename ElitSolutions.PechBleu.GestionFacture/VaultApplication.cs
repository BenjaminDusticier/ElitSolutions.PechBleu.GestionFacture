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

            // R�cup�re le filever de l'objet.
            ObjectFiles objFiles = env.Vault.ObjectFileOperations.GetFiles(env.ObjVer);
            ObjectFile objFile = objFiles[0];
            FileVer fileVer = objFile.FileVer;

            // R�cup�re le contenu texte.
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

            // Initialise la premi�re ligne du tableau
            string ec = lignes[0] + '\n';

            // Pour chaque ligne du contenu texte.
            for (int i = 1; i < lignes.Length; i++)
            {
                // Enregistre la ligne.
                ec += lignes[i] + '\n';
            
                // Copie les cha�nes s�par�es par tabulation dans un tableau.
                string[] valeurs = lignes[i].Split('\t');

                // M�morise le tiers si pas vide
                if(valeurs[6] != "")
                {
                    // R�cup�re le num�ro de tiers.
                    numTiers = int.Parse(valeurs[6]);
                }
                
                // R�cup�re le num�ro de pi�ce.
                numPiece = int.Parse(valeurs[0]);

                // Passe � la ligne suivante si le num�ro de pi�ce n'a pas chang� ou si on est � la premi�re ligne
                if ( numPiecePrec == numPiece || i == 1)
                {   
                    // M�morise le num�ro de pi�ce.
                    numPiecePrec = numPiece; 
                    
                    // Passe � la ligne suivante
                    continue; 
                }

                // R�cup�re le num�ro de facture.
                numFac = int.Parse(valeurs[3]);

                // R�cup�re la date de la facture
                dateFacture = DateTime.Parse(valeurs[2]);

                // Cherche le fournisseur PFO.
                var searchBuilder = new MFSearchBuilder(env.Vault);
                searchBuilder.Class(this.Configuration.ClassFournisseurPFO);
                searchBuilder.Status(MFStatusType.MFStatusTypeExtID, MFDataType.MFDatatypeInteger, numTiers);
                searchBuilder.Deleted(false);
                var searchResults = searchBuilder.FindEx();

                // Si au moins un r�sultat.
                if ( searchResults.Count > 0) 
                {
                    // R�cup�re l'ID du fournisseur trouv�.
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

                    // Si au moins un r�sultat.
                    if (searchResults.Count > 0)
                    {
                        // R�cup�re l'ID de la facture trouv�e.
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

                // Si au moins un r�sultat.
                if (searchResults.Count > 0)
                {
                    // R�cup�re l'ID du fournisseur trouv�.
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

                    // Si au moins un r�sultat.
                    if (searchResults.Count > 0)
                    {
                        if (idFac == -1)
                        {
                            // R�cup�re l'ID de la facture trouv�e.
                            idFac = searchResults[0].ID;
                            oFac = searchResults[0];
                        }
                        else
                        {
                            idFac = -1;
                        }
                    }
                }

                // Cr�e le fichier temporaire.
                string tempFileName = Path.GetTempFileName();

                // Ajoute les �critures au fichier temporaire.
                StreamWriter streamWriter = File.AppendText(tempFileName);
                streamWriter.Write(ec);
                streamWriter.Flush();
                streamWriter.Close();

                // Cr�e le document.

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

                // R�initialise l'id
                idFac = -1;

            }
            //throw new System.NotImplementedException();
        }
    }
}