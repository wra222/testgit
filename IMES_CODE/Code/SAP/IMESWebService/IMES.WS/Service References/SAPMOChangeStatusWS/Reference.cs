﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5448
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMES.WS.SAPMOChangeStatusWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", ConfigurationName="SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WS")]
    public interface Z_SET_PRODORD_TO_IMES_TECO_WS {
        
        // CODEGEN: Parameter 'OUTPUT' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [return: System.ServiceModel.MessageParameterAttribute(Name="OUTPUT")]
        IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECOResponse Z_SET_PRODORD_TO_IMES_TECO(IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECORequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZMO_MES_TECO_I : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string mONUMBERField;
        
        private string dELIVEREDQTYField;
        
        private string sTATUSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string MONUMBER {
            get {
                return this.mONUMBERField;
            }
            set {
                this.mONUMBERField = value;
                this.RaisePropertyChanged("MONUMBER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string DELIVEREDQTY {
            get {
                return this.dELIVEREDQTYField;
            }
            set {
                this.dELIVEREDQTYField = value;
                this.RaisePropertyChanged("DELIVEREDQTY");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZMO_MES_TECO_E : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string mONUMBERField;
        
        private string dELIVEREDQTYField;
        
        private string sTATUSField;
        
        private string rESULTField;
        
        private string eRRORMESSAGEField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string MONUMBER {
            get {
                return this.mONUMBERField;
            }
            set {
                this.mONUMBERField = value;
                this.RaisePropertyChanged("MONUMBER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string DELIVEREDQTY {
            get {
                return this.dELIVEREDQTYField;
            }
            set {
                this.dELIVEREDQTYField = value;
                this.RaisePropertyChanged("DELIVEREDQTY");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string STATUS {
            get {
                return this.sTATUSField;
            }
            set {
                this.sTATUSField = value;
                this.RaisePropertyChanged("STATUS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string RESULT {
            get {
                return this.rESULTField;
            }
            set {
                this.rESULTField = value;
                this.RaisePropertyChanged("RESULT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string ERRORMESSAGE {
            get {
                return this.eRRORMESSAGEField;
            }
            set {
                this.eRRORMESSAGEField = value;
                this.RaisePropertyChanged("ERRORMESSAGE");
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="Z_SET_PRODORD_TO_IMES_TECO", WrapperNamespace="urn:sap-com:document:sap:rfc:functions", IsWrapped=true)]
    public partial class Z_SET_PRODORD_TO_IMES_TECORequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IMES.WS.SAPMOChangeStatusWS.ZMO_MES_TECO_I INPUT;
        
        public Z_SET_PRODORD_TO_IMES_TECORequest() {
        }
        
        public Z_SET_PRODORD_TO_IMES_TECORequest(IMES.WS.SAPMOChangeStatusWS.ZMO_MES_TECO_I INPUT) {
            this.INPUT = INPUT;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Z_SET_PRODORD_TO_IMES_TECOResponse", WrapperNamespace="urn:sap-com:document:sap:rfc:functions", IsWrapped=true)]
    public partial class Z_SET_PRODORD_TO_IMES_TECOResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IMES.WS.SAPMOChangeStatusWS.ZMO_MES_TECO_E OUTPUT;
        
        public Z_SET_PRODORD_TO_IMES_TECOResponse() {
        }
        
        public Z_SET_PRODORD_TO_IMES_TECOResponse(IMES.WS.SAPMOChangeStatusWS.ZMO_MES_TECO_E OUTPUT) {
            this.OUTPUT = OUTPUT;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface Z_SET_PRODORD_TO_IMES_TECO_WSChannel : IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WS, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class Z_SET_PRODORD_TO_IMES_TECO_WSClient : System.ServiceModel.ClientBase<IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WS>, IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WS {
        
        public Z_SET_PRODORD_TO_IMES_TECO_WSClient() {
        }
        
        public Z_SET_PRODORD_TO_IMES_TECO_WSClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Z_SET_PRODORD_TO_IMES_TECO_WSClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Z_SET_PRODORD_TO_IMES_TECO_WSClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Z_SET_PRODORD_TO_IMES_TECO_WSClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECOResponse IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WS.Z_SET_PRODORD_TO_IMES_TECO(IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECORequest request) {
            return base.Channel.Z_SET_PRODORD_TO_IMES_TECO(request);
        }
        
        public IMES.WS.SAPMOChangeStatusWS.ZMO_MES_TECO_E Z_SET_PRODORD_TO_IMES_TECO(IMES.WS.SAPMOChangeStatusWS.ZMO_MES_TECO_I INPUT) {
            IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECORequest inValue = new IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECORequest();
            inValue.INPUT = INPUT;
            IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECOResponse retVal = ((IMES.WS.SAPMOChangeStatusWS.Z_SET_PRODORD_TO_IMES_TECO_WS)(this)).Z_SET_PRODORD_TO_IMES_TECO(inValue);
            return retVal.OUTPUT;
        }
    }
}
