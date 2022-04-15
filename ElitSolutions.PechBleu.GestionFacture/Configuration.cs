using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Configuration.JsonAdaptor;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElitSolutions.PechBleu.GestionFacture
{
    [DataContract]
    public class Configuration
    {
        /// <summary>
        /// A <see cref="MFIdentifier"/> for the class
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFClass(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Classe des fournisseurs PFO")]
        public MFIdentifier ClassFournisseurPFO { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the class
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFClass(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Classe des fournisseurs YEDRA")]
        public MFIdentifier ClassFournisseurYEDRA { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the class
        /// with alias "MF.Class.Fac".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFClass(Required = true)]
        [DataMember]
        [JsonConfEditor (Label = "Classe des factures")]
        public MFIdentifier ClassFacture { get; set; }
            = "MFiles.Class.Facture";

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the class
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFClass(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Classe des EC")]
        public MFIdentifier ClassEC { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Prop EC")]
        public MFIdentifier PropDefEC { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// with alias "MF.PD.Fou".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Prop Fournisseur PFO")]
        public MFIdentifier PropDefFournisseurPFO { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// with alias "MF.PD.Fou".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Prop Fournisseur YEDRA")]
        public MFIdentifier PropDefFournisseurYEDRA { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// with alias "MF.PD.NumFac".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Prop Numéro de facture")]
        public MFIdentifier PropDefNumFac { get; set; }
            = "MF.PD.NumFac";

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Prop Date Facture")]
        public MFIdentifier PropDefDateFact { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Prop Société")]
        public MFIdentifier PropDefSociete { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Id Société PFO")]
        public int idSocietePFO { get; set; }

        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Id Société YEDRA")]
        public int idSocieteYEDRA { get; set; }

        //
        [MFWorkflow(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Workflow EC")]
        public MFIdentifier WFEC { get; set; }

        //
        [MFState(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Première étape du WF EC")]
        public MFIdentifier StateEC { get; set; }
    }
}