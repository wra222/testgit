using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.PGIStatus
{
    public class PGIStatus 
    {
        public string SerialNumber ;
        public string Plant;
        public string EventType;
        public string ID;
        public string Type;
        public string PGI_Date;
        public string Remark1;
    }

    public class PGIStatusResponse
    {
        public string SerialNumber;
        public string Result;
       
    }

}
