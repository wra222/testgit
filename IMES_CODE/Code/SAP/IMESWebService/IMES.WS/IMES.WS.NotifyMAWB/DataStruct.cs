using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.NotifyMAWB
{
    public class NotifyMAWB 
    {
        public string SerialNumber ;
        public string MAWB; 
        public string Plant;
        public string DN;
        public string Items;
        public string Declaration;
        public string Containerid;
        public string Remark1;
    }

    public class NotifyMAWBResponse
    {
        public string SerialNumber;
        public string MAWB;
        public string Result;
        public string ErrorText;       
    }

}
