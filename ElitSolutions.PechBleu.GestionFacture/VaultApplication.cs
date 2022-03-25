using MFiles.VAF;
using MFiles.VAF.AppTasks;
using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Core;
using MFilesAPI;
using System;
using System.Diagnostics;

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
            // Récupère le filever de l'objet.
            ObjectFiles objfiles = env.Vault.ObjectFileOperations.GetFiles(env.ObjVer);
            ObjectFile objfile = objfiles[1];
            FileVer filever = objfile.FileVer;

            // Récupère le contenu texte.
            env.Vault.ObjectFileOperations.GetTextContentForFile(env.ObjVer, filever);

            //
            int numlot = -1;

            // Pour chaque ligne du contenu texte.
            foreach (var item in collection)
            {

            }

            //throw new System.NotImplementedException();
        }

    }
}