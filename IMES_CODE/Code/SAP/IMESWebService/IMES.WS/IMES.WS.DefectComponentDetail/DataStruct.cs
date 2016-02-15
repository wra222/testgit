using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.DefectComponentDetail
{
    public class DCItem 
    {
        public string TxnId ;
        public string BatchID;
        public string Type;
        public string Family;
        public string IECPn;
        public string Vendor;
        public string DefectCode;
        public string Remark1;
        public string Remark2;
    }

    public class DCHeader
    {
        public string TxnId;
        public string BatchID;
        public string Status;
        public string Result;
        public string ErrorText;
        //List<DCDetail> DCDetail;
    }

    public class DCDetail
    {
        public string TxnId;
        public string BatchID;
        public string Family;
        public string IECPn;
        public string PartType;
        public string Vendor;
        public string DefectCode;
        public string DefectDescr;
        public string PartSn;
        public string PartSerialNo;

    }

    public class ResponseDC
    {
        public DCHeader dcHeader;
        public DCDetail[] dcDetail ;
    }

}
