using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using IMES.WS.Common;
namespace IMES.WS.MOPullRelease
{

    public class MoPullReleaseResponse
    {
        public string MoNumber;
        public string SerialNumber;
        public string Result;
        public string ErrorMessage;
    }

    public class MoPullHeader
    {
        public string SerialNumber;
        public string Plant;
        public string MoNumber;       
        public string ProductionVer;
        public string IssuedQty;
        public string CurrentyIssueQty;
     
        public MoPullHeader() { }
        public MoPullHeader(MoPullHeader header)
        {
            ObjectTool.CopyObject(header, this);

        }

        
    }

   


}
