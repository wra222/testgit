﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4971
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMES.Service.MO.SAPSubmitConfirmWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", ConfigurationName="SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WS")]
    public interface Z_PRODORDCONF_IMES_SUBMIT_WS {
        
        // CODEGEN: Parameter 'RESPONSE' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [return: System.ServiceModel.MessageParameterAttribute(Name="RESPONSE")]
        IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMITResponse Z_PRODORDCONF_IMES_SUBMIT(IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMITRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZMO_MES_CF_R1 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string sERIALNUMBERField;
        
        private string rESULTCNFField;
        
        private string eRRMSGField;
        
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
        public string RESULTCNF {
            get {
                return this.rESULTCNFField;
            }
            set {
                this.rESULTCNFField = value;
                this.RaisePropertyChanged("RESULTCNF");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string ERRMSG {
            get {
                return this.eRRMSGField;
            }
            set {
                this.eRRMSGField = value;
                this.RaisePropertyChanged("ERRMSG");
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="Z_PRODORDCONF_IMES_SUBMIT", WrapperNamespace="urn:sap-com:document:sap:rfc:functions", IsWrapped=true)]
    public partial class Z_PRODORDCONF_IMES_SUBMITRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SERIALNUMBER;
        
        public Z_PRODORDCONF_IMES_SUBMITRequest() {
        }
        
        public Z_PRODORDCONF_IMES_SUBMITRequest(string SERIALNUMBER) {
            this.SERIALNUMBER = SERIALNUMBER;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Z_PRODORDCONF_IMES_SUBMITResponse", WrapperNamespace="urn:sap-com:document:sap:rfc:functions", IsWrapped=true)]
    public partial class Z_PRODORDCONF_IMES_SUBMITResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:rfc:functions", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IMES.Service.MO.SAPSubmitConfirmWS.ZMO_MES_CF_R1 RESPONSE;
        
        public Z_PRODORDCONF_IMES_SUBMITResponse() {
        }
        
        public Z_PRODORDCONF_IMES_SUBMITResponse(IMES.Service.MO.SAPSubmitConfirmWS.ZMO_MES_CF_R1 RESPONSE) {
            this.RESPONSE = RESPONSE;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface Z_PRODORDCONF_IMES_SUBMIT_WSChannel : IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WS, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class Z_PRODORDCONF_IMES_SUBMIT_WSClient : System.ServiceModel.ClientBase<IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WS>, IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WS {
        
        public Z_PRODORDCONF_IMES_SUBMIT_WSClient() {
        }
        
        public Z_PRODORDCONF_IMES_SUBMIT_WSClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Z_PRODORDCONF_IMES_SUBMIT_WSClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Z_PRODORDCONF_IMES_SUBMIT_WSClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Z_PRODORDCONF_IMES_SUBMIT_WSClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMITResponse IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WS.Z_PRODORDCONF_IMES_SUBMIT(IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMITRequest request) {
            return base.Channel.Z_PRODORDCONF_IMES_SUBMIT(request);
        }
        
        public IMES.Service.MO.SAPSubmitConfirmWS.ZMO_MES_CF_R1 Z_PRODORDCONF_IMES_SUBMIT(string SERIALNUMBER) {
            IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMITRequest inValue = new IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMITRequest();
            inValue.SERIALNUMBER = SERIALNUMBER;
            IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMITResponse retVal = ((IMES.Service.MO.SAPSubmitConfirmWS.Z_PRODORDCONF_IMES_SUBMIT_WS)(this)).Z_PRODORDCONF_IMES_SUBMIT(inValue);
            return retVal.RESPONSE;
        }
    }
}