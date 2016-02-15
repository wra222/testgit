﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4984
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMES.WS.SAPFisBomCompareWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", ConfigurationName="SAPFisBomCompareWS.Z_RFC_FIS_BOM_WS")]
    public interface Z_RFC_FIS_BOM_WS {
        
        // CODEGEN: Parameter 'ZIMES_FIS_BOM_H_R' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [return: System.ServiceModel.MessageParameterAttribute(Name="ZIMES_FIS_BOM_H_R")]
        IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOMResponse Z_RFC_FIS_BOM(IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOMRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZIMES_FIS_BOM_H : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string sERIALNUMBERField;
        
        private string wERKSField;
        
        private string mATNRField;
        
        private string cNTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SERIALNUMBER {
            get {
                return this.sERIALNUMBERField;
            }
            set {
                this.sERIALNUMBERField = value;
                this.RaisePropertyChanged("SERIALNUMBER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string WERKS {
            get {
                return this.wERKSField;
            }
            set {
                this.wERKSField = value;
                this.RaisePropertyChanged("WERKS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string MATNR {
            get {
                return this.mATNRField;
            }
            set {
                this.mATNRField = value;
                this.RaisePropertyChanged("MATNR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string CNT {
            get {
                return this.cNTField;
            }
            set {
                this.cNTField = value;
                this.RaisePropertyChanged("CNT");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZIMES_FIS_BOM_H_R : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string sERIALNUMBERField;
        
        private string wERKSField;
        
        private string mATNRField;
        
        private string cNTField;
        
        private string sTATUSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SERIALNUMBER {
            get {
                return this.sERIALNUMBERField;
            }
            set {
                this.sERIALNUMBERField = value;
                this.RaisePropertyChanged("SERIALNUMBER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string WERKS {
            get {
                return this.wERKSField;
            }
            set {
                this.wERKSField = value;
                this.RaisePropertyChanged("WERKS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string MATNR {
            get {
                return this.mATNRField;
            }
            set {
                this.mATNRField = value;
                this.RaisePropertyChanged("MATNR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string CNT {
            get {
                return this.cNTField;
            }
            set {
                this.cNTField = value;
                this.RaisePropertyChanged("CNT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string STATUS {
            get {
                return this.sTATUSField;
            }
            set {
                this.sTATUSField = value;
                this.RaisePropertyChanged("STATUS");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZIMES_FIS_BOM_I_R : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string sERIALNUMBERField;
        
        private string iTEMNOField;
        
        private string eRRORLOGField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SERIALNUMBER {
            get {
                return this.sERIALNUMBERField;
            }
            set {
                this.sERIALNUMBERField = value;
                this.RaisePropertyChanged("SERIALNUMBER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ITEMNO {
            get {
                return this.iTEMNOField;
            }
            set {
                this.iTEMNOField = value;
                this.RaisePropertyChanged("ITEMNO");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string ERRORLOG {
            get {
                return this.eRRORLOGField;
            }
            set {
                this.eRRORLOGField = value;
                this.RaisePropertyChanged("ERRORLOG");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZIMES_FIS_BOM_I : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string sERIALNUMBERField;
        
        private string iTEMNOField;
        
        private string aLPGRField;
        
        private string iDNRKField;
        
        private string mENGEField;
        
        private string mEINSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SERIALNUMBER {
            get {
                return this.sERIALNUMBERField;
            }
            set {
                this.sERIALNUMBERField = value;
                this.RaisePropertyChanged("SERIALNUMBER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ITEMNO {
            get {
                return this.iTEMNOField;
            }
            set {
                this.iTEMNOField = value;
                this.RaisePropertyChanged("ITEMNO");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string ALPGR {
            get {
                return this.aLPGRField;
            }
            set {
                this.aLPGRField = value;
                this.RaisePropertyChanged("ALPGR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string IDNRK {
            get {
                return this.iDNRKField;
            }
            set {
                this.iDNRKField = value;
                this.RaisePropertyChanged("IDNRK");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string MENGE {
            get {
                return this.mENGEField;
            }
            set {
                this.mENGEField = value;
                this.RaisePropertyChanged("MENGE");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string MEINS {
            get {
                return this.mEINSField;
            }
            set {
                this.mEINSField = value;
                this.RaisePropertyChanged("MEINS");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Z_RFC_FIS_BOM", WrapperNamespace="urn:sap-com:document:sap:rfc:functions", IsWrapped=true)]
    public partial class Z_RFC_FIS_BOMRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IMES.WS.SAPFisBomCompareWS.ZIMES_FIS_BOM_H ZIMES_FIS_BOM_H;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=1)]
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZIMES_FIS_BOM_I[] ZIMES_FIS_BOM_I;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=2)]
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZIMES_FIS_BOM_I_R[] ZIMES_FIS_BOM_I_R;
        
        public Z_RFC_FIS_BOMRequest() {
        }
        
        public Z_RFC_FIS_BOMRequest(IMES.WS.SAPFisBomCompareWS.ZIMES_FIS_BOM_H ZIMES_FIS_BOM_H, ZIMES_FIS_BOM_I[] ZIMES_FIS_BOM_I, ZIMES_FIS_BOM_I_R[] ZIMES_FIS_BOM_I_R) {
            this.ZIMES_FIS_BOM_H = ZIMES_FIS_BOM_H;
            this.ZIMES_FIS_BOM_I = ZIMES_FIS_BOM_I;
            this.ZIMES_FIS_BOM_I_R = ZIMES_FIS_BOM_I_R;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Z_RFC_FIS_BOMResponse", WrapperNamespace="urn:sap-com:document:sap:rfc:functions", IsWrapped=true)]
    public partial class Z_RFC_FIS_BOMResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IMES.WS.SAPFisBomCompareWS.ZIMES_FIS_BOM_H_R ZIMES_FIS_BOM_H_R;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=1)]
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZIMES_FIS_BOM_I[] ZIMES_FIS_BOM_I;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=2)]
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZIMES_FIS_BOM_I_R[] ZIMES_FIS_BOM_I_R;
        
        public Z_RFC_FIS_BOMResponse() {
        }
        
        public Z_RFC_FIS_BOMResponse(IMES.WS.SAPFisBomCompareWS.ZIMES_FIS_BOM_H_R ZIMES_FIS_BOM_H_R, ZIMES_FIS_BOM_I[] ZIMES_FIS_BOM_I, ZIMES_FIS_BOM_I_R[] ZIMES_FIS_BOM_I_R) {
            this.ZIMES_FIS_BOM_H_R = ZIMES_FIS_BOM_H_R;
            this.ZIMES_FIS_BOM_I = ZIMES_FIS_BOM_I;
            this.ZIMES_FIS_BOM_I_R = ZIMES_FIS_BOM_I_R;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface Z_RFC_FIS_BOM_WSChannel : IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOM_WS, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class Z_RFC_FIS_BOM_WSClient : System.ServiceModel.ClientBase<IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOM_WS>, IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOM_WS {
        
        public Z_RFC_FIS_BOM_WSClient() {
        }
        
        public Z_RFC_FIS_BOM_WSClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Z_RFC_FIS_BOM_WSClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Z_RFC_FIS_BOM_WSClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Z_RFC_FIS_BOM_WSClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOMResponse IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOM_WS.Z_RFC_FIS_BOM(IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOMRequest request) {
            return base.Channel.Z_RFC_FIS_BOM(request);
        }
        
        public IMES.WS.SAPFisBomCompareWS.ZIMES_FIS_BOM_H_R Z_RFC_FIS_BOM(IMES.WS.SAPFisBomCompareWS.ZIMES_FIS_BOM_H ZIMES_FIS_BOM_H, ref ZIMES_FIS_BOM_I[] ZIMES_FIS_BOM_I, ref ZIMES_FIS_BOM_I_R[] ZIMES_FIS_BOM_I_R) {
            IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOMRequest inValue = new IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOMRequest();
            inValue.ZIMES_FIS_BOM_H = ZIMES_FIS_BOM_H;
            inValue.ZIMES_FIS_BOM_I = ZIMES_FIS_BOM_I;
            inValue.ZIMES_FIS_BOM_I_R = ZIMES_FIS_BOM_I_R;
            IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOMResponse retVal = ((IMES.WS.SAPFisBomCompareWS.Z_RFC_FIS_BOM_WS)(this)).Z_RFC_FIS_BOM(inValue);
            ZIMES_FIS_BOM_I = retVal.ZIMES_FIS_BOM_I;
            ZIMES_FIS_BOM_I_R = retVal.ZIMES_FIS_BOM_I_R;
            return retVal.ZIMES_FIS_BOM_H_R;
        }
    }
}