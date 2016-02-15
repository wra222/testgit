using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.DefectComponentUpdate
{
    public class DCHeader
    {
        public string TxnId;
        public string BatchID;
        public string Remark1;
        public string Remark2;
    }

    public class DCDetail
    {
        public string TxnId;
        public string BatchID;
        //public string PartSerialNo;
        public string PartSn;
        public string Status;
        public string Remark1;
        public string Remark2;
    }

    public class RequestDC
    {
        public DCHeader dcheader;
        public DCDetail[] dcDetail ;
    }

    public class ResponseDC
    {
        public string TxnId;
        public string BatchID;
        public string Status;
        public string Result;
        public string ErrorText;

    }
}
