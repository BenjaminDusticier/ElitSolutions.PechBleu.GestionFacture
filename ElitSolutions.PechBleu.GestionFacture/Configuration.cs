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
        /// with alias "MF.Class.Fou".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFClass(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Classe des fournisseurs")]
        public MFIdentifier ClassFournisseur { get; set; }
            = "MFiles.Class.Fournisseur";


        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// with alias "MF.PD.NumFou".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Numéro du fournisseur")]
        public MFIdentifier PropDefNumFou { get; set; }
            = "MFiles.PropertyDef.NumFou";

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
        /// A <see cref="MFIdentifier"/> for the property definition
        /// with alias "MF.PD.Fou".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Fournisseur des factures")]
        public MFIdentifier PropDefFournisseur { get; set; }
            = "MFiles.PropertyDef.Fournisseur";


        /// <summary>
        /// A <see cref="MFIdentifier"/> for the property definition
        /// with alias "MF.PD.NumFac".
        /// </summary>
        /// <remarks>If this alias cannot be resolved then the ID will be -1.</remarks>
        [MFPropertyDef(Required = true)]
        [DataMember]
        [JsonConfEditor(Label = "Numéro de facture")]
        public MFIdentifier PropDefNumFac { get; set; }
            = "MF.PD.NumFac";

    }
}