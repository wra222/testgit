﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5485
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UPS.CNRS.UPSWS {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://hp.com/", ConfigurationName="UPSWS.ATRPSoap")]
    public interface ATRPSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSGetTags", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ATRPStruct UPSGetTags(string SN, string Location);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSGetRange", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ATRPStruct UPSGetRange(string PartNumber, string AssetNumber, string HPPO);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSGetImages", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ATRPStruct UPSGetImages(string SN, string Location);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSGetATRP", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ATRPStruct UPSGetATRP(string SN, string Location);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSGetBTWFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ATRPStruct UPSGetBTWFile(string PartNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSResetSN", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ResetStruct UPSResetSN(string SN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSUpdateSN", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ResetStruct UPSUpdateSN(string SN, string newSN);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSUpdateHPPO", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ResetStruct UPSUpdateHPPO(string SN, string PartNumber, string AssetNumber, string HPPO, string newHPPO);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSUpdatePrintInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ATMStruct UPSUpdatePrintInfo(string SN, string PartNumber, string AssetNumber, string HPPO, string MAC, string MAC2, string SystemID, string Placement, string TagInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSGetATM", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.ATMStruct UPSGetATM(string SN, string PartNumber, string AssetNumber, string HPPO, string AssetTagNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/UPSGetUSI", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        UPS.CNRS.UPSWS.USIStruct UPSGetUSI(string SN, string HPPO, string PartNumber, string FGPartNum);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5485")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hp.com/")]
    public partial class ATRPStruct : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string versionField;
        
        private int retcodeField;
        
        private string messageField;
        
        private System.Data.DataSet tagdataField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
                this.RaisePropertyChanged("version");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int retcode {
            get {
                return this.retcodeField;
            }
            set {
                this.retcodeField = value;
                this.RaisePropertyChanged("retcode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public System.Data.DataSet tagdata {
            get {
                return this.tagdataField;
            }
            set {
                this.tagdataField = value;
                this.RaisePropertyChanged("tagdata");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5485")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hp.com/")]
    public partial class USIStruct : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string versionField;
        
        private int retcodeField;
        
        private string messageField;
        
        private string unattendField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
                this.RaisePropertyChanged("version");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int retcode {
            get {
                return this.retcodeField;
            }
            set {
                this.retcodeField = value;
                this.RaisePropertyChanged("retcode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string unattend {
            get {
                return this.unattendField;
            }
            set {
                this.unattendField = value;
                this.RaisePropertyChanged("unattend");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5485")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hp.com/")]
    public partial class ATMStruct : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string versionField;
        
        private int retcodeField;
        
        private string messageField;
        
        private string assetTagNumField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
                this.RaisePropertyChanged("version");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int retcode {
            get {
                return this.retcodeField;
            }
            set {
                this.retcodeField = value;
                this.RaisePropertyChanged("retcode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string assetTagNum {
            get {
                return this.assetTagNumField;
            }
            set {
                this.assetTagNumField = value;
                this.RaisePropertyChanged("assetTagNum");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5485")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://hp.com/")]
    public partial class ResetStruct : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string versionField;
        
        private int retcodeField;
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
                this.RaisePropertyChanged("version");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int retcode {
            get {
                return this.retcodeField;
            }
            set {
                this.retcodeField = value;
                this.RaisePropertyChanged("retcode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ATRPSoapChannel : UPS.CNRS.UPSWS.ATRPSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class ATRPSoapClient : System.ServiceModel.ClientBase<UPS.CNRS.UPSWS.ATRPSoap>, UPS.CNRS.UPSWS.ATRPSoap {
        
        public ATRPSoapClient() {
        }
        
        public ATRPSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ATRPSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ATRPSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ATRPSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public UPS.CNRS.UPSWS.ATRPStruct UPSGetTags(string SN, string Location) {
            return base.Channel.UPSGetTags(SN, Location);
        }
        
        public UPS.CNRS.UPSWS.ATRPStruct UPSGetRange(string PartNumber, string AssetNumber, string HPPO) {
            return base.Channel.UPSGetRange(PartNumber, AssetNumber, HPPO);
        }
        
        public UPS.CNRS.UPSWS.ATRPStruct UPSGetImages(string SN, string Location) {
            return base.Channel.UPSGetImages(SN, Location);
        }
        
        public UPS.CNRS.UPSWS.ATRPStruct UPSGetATRP(string SN, string Location) {
            return base.Channel.UPSGetATRP(SN, Location);
        }
        
        public UPS.CNRS.UPSWS.ATRPStruct UPSGetBTWFile(string PartNumber) {
            return base.Channel.UPSGetBTWFile(PartNumber);
        }
        
        public UPS.CNRS.UPSWS.ResetStruct UPSResetSN(string SN) {
            return base.Channel.UPSResetSN(SN);
        }
        
        public UPS.CNRS.UPSWS.ResetStruct UPSUpdateSN(string SN, string newSN) {
            return base.Channel.UPSUpdateSN(SN, newSN);
        }
        
        public UPS.CNRS.UPSWS.ResetStruct UPSUpdateHPPO(string SN, string PartNumber, string AssetNumber, string HPPO, string newHPPO) {
            return base.Channel.UPSUpdateHPPO(SN, PartNumber, AssetNumber, HPPO, newHPPO);
        }
        
        public UPS.CNRS.UPSWS.ATMStruct UPSUpdatePrintInfo(string SN, string PartNumber, string AssetNumber, string HPPO, string MAC, string MAC2, string SystemID, string Placement, string TagInfo) {
            return base.Channel.UPSUpdatePrintInfo(SN, PartNumber, AssetNumber, HPPO, MAC, MAC2, SystemID, Placement, TagInfo);
        }
        
        public UPS.CNRS.UPSWS.ATMStruct UPSGetATM(string SN, string PartNumber, string AssetNumber, string HPPO, string AssetTagNumber) {
            return base.Channel.UPSGetATM(SN, PartNumber, AssetNumber, HPPO, AssetTagNumber);
        }
        
        public UPS.CNRS.UPSWS.USIStruct UPSGetUSI(string SN, string HPPO, string PartNumber, string FGPartNum) {
            return base.Channel.UPSGetUSI(SN, HPPO, PartNumber, FGPartNum);
        }
    }
}
