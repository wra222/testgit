using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.CancelBindDN
{
    public class CancelBindDN 
    {
        public string SerialNumber ;
        public string Plant;
        public string DN;
        public string Remark1;
        public string Remark2;
    }

    public class CancelBindDNResponse
    {
        public string SerialNumber;
        public string Plant;
        public string DN;
        public string Result;
        public string ErrorText;       
    }

}
