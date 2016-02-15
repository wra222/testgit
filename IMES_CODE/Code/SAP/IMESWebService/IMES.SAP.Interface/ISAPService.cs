using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.SAP.Interface
{
    public enum enumMOStatus
    {
        Release,
        Start,
        Run,
        Close
    }

    public interface ISAPService
    {
        bool UpdateMO(string moId, out string errorText);
        bool UpdateMOHeader(string moId, out string errorText);
        bool UpdateMaterial(string component, out string errorText);
        MaterialInfo QueryMaterialInfo(string component, out string errorText);
        bool QuerySAPMOStatus(string moId, out string erorText);
        bool CloseSAPMO(string moId, string comment, string Editor, out string errText);
        bool ChangeIMESMOStatus(string moId, enumMOStatus status, string comment, string Editor, out string errText);
        bool OpenSAPMO(string moId, string Editor, out string errText);
        bool ConfirmMO(string isParent, out string errorText);
        bool AdjustConfirmMO(out string errorText);
        string CancelBindDN(string Plant, string DN, out string errorText);
        string CancelBindDNbyItem(string Plant, string DN, string dnItem, string dnType, out string errorText);
        string UploadMasterWeight(string Plant, string Model, string GrossWeight, string NetWeight, string Unit, out string errorText);
        ReplyUploadSAPSN UploadSapCustSN(IList<UploadSAPSN> uploadSAPSNList);
        ResponseSAPPo GetSAPPOInfo(RequestSAPPo poInfo);

    }

    public class MaterialInfo:MarshalByRefObject
    {
       public string Material;
       public string Serilnumber;
       public string Plant;
       public string Materialgroup;
       public string Materialtype;
       public string Materialstatus;
       public string CustomerMaterial;
       public string Cutsomer;
       public string Materialdescription;         
    }

    /*
    public class CancelBindDNResponse : MarshalByRefObject
    {
        public string Serilnumber;
        public string Plant;
        public string DN;
        public string Result;
        public string ErrorText;
    }
    */
    [Serializable]
    public class UploadSAPSN{
        public string SERIALNUMBER="";
        public string DN = "";
        public string DLV_ITM = "";
        public string PO_NO = "";
        public string PALLET_NO = "";
        public string CARTON_NO = "";
        public string SERNO = "";
        public string BOX_ID = "";
        public string QTY = "";
        public string TYPE = "";
        public string PBOX_ID = "";
        public string ASSET_NO = "";
        public string WLMAT = "";
        public string ODMAT = "";
        public string ACNUM = "";
        public string FDA = "";
        public string FCCID = "";
        public string WARRANTY_ID = "";
        public string WARRANTY_DT = "";
        public string MACADDRESS = "";
        public string CUST_PALLET = "";
        public string PALLET_ORDINAL = "";
        public string SERNO2 = "";
        public string SOFTWARE_PN = "";
        public string HEX_MEID = "";
        public string MANUFACTURE_DATE = "";
        public string UUID = "";
        public string MACADDRESS2 = "";
        public string IMEI_CODE = "";
        public string BUILDER_VER = "";
        public string SIMLOCK_CODE = "";
        public string SOFTWARE_VER = "";
        public string ICCID = "";
        public string ERAC = "";
        public string WMAC = "";
        public string BMAC = "";
    }

    [Serializable]
    public class ReplyUploadSAPSNHeader
    {
         public string SERIALNUMBER;
         public string DLV_NO;
         public string DLV_ITM;
         public string RESULT;
         public string MESSAGE;

    }

    [Serializable]
    public class ReplyUploadSAPSNItem
    {
            public string  SERIALNUMBER;
            public string  DLV_NO;
            public string  DLV_ITM;
            public string  SERNO;
            public string  RESULT;
            public string MESSAGE;
    }

    [Serializable]
    public class ReplyUploadSAPSN
    {
        public IList<ReplyUploadSAPSNHeader> HeaderList;
        public IList<ReplyUploadSAPSNItem> ItemList;
    }

    [Serializable]
    public class RequestSAPPo
    {
        public string TxnId;
        public string Type;
        public string PoNo;
        public string PoItem;
        public string Remark;
    }

    [Serializable]
    public class ResponseSAPPo
    {
        public string TxnId;
        public string Data;
        public string Result;
        public string ErrorText;      
    }

}
