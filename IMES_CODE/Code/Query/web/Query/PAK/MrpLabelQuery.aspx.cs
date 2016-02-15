using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;



public partial class Query_PAK_MrpLabelQuery : System.Web.UI.Page
{

   // public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_MrpQuery PAKMRPQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_MrpQuery>(WebConstant.IPAKMRPLabelQuery);
    string DBConnection = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
            
    }

   
   
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string dnorsn=hidProduct.Value.ToString();
        string ret = "";
         ret= PAKMRPQuery.GetMrpLabelByDN(DBConnection, dnorsn);
         Textarea1.InnerText = ret;
        
    }
}
