using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace IMES.LD
{
    /// <summary>
    /// 雷雕回传的信息：SN、PN、Warranty、MN 
    /// Status>0 =>success 
    /// Status<0 => fail
    /// </summary>
    public class PrintResponse
    {
        public string SN { get; set; }
        public string PN { get; set; }
        public string Warranty { get; set; }
        public string MN { get; set; }
        public string ErrorMsg { get; set; }
        public int Status { get; set; }

        public override string ToString()
        {
            return string.Format("SN:{0} PN:{1} Warranty:{2} MN:{3} ErrorMsg:{4} Status:{5}", SN,PN,Warranty,MN,ErrorMsg,Status.ToString()) ;
        }
    }

    /// <summary>
    /// EoLA回传的信息
    /// </summary>
    public class ModelResponse
    {
        public string CustSN { get; set; }
        public string ProductID { get; set; }
        public string Model { get; set; }
        public string Family { get; set; }
        public string Customer { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("CustSN:{0} ProductID:{1} Model:{2} Family:{3} Customer:{4} ErrorCode:{5} Message:{6}", 
                                     CustSN, ProductID,Model,Family, Customer,ErrorCode.ToString(), Message);
        }
    }

    /// <summary>
    /// 自动贴标签回传信息
    /// </summary>
    public class LocationResponse
    {
        public string SN { get; set; }
        public string Location { get; set; }
        public string Coordinate { get; set; }
        public int Status { get; set; }
        public string ErrorMsg { get; set; }

        public override string ToString()
        {
            return string.Format("SN:{0} Location:{1} Coordinate:{2} Status:{3} ErrorMsg:{4}",
                                     SN,Location,Coordinate,Status.ToString(),ErrorMsg);
        }
    }
}
