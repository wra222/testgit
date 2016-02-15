using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.SAPPO
{
   
    public class NotifyPoInfoResponse
    {
        public string TxnId;
        public string CUST_PO_NO;
        public string CUST_PO_NO_ITEM;
        public string Result;
        public string ErrorText;
       
    }

    public class NotifyPackageInfoResponse
    {
        public string TxnId;
        public string DN;
        public string DN_ITEM;
        public string Result;
        public string ErrorText;

    }

    public class NotifyCancelPoDnResponse
    {
        public string TxnId;
        public string ID;
        public string ITEM;
        public string Result;
        public string ErrorText;

    }

    

}
