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

public partial class RedirectErrMsg : IMESBasePage
{
    public String strTitle = Resources.iMESGlobalDisplay.ResourceManager.GetString(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_titbErrMessage");

    protected void Page_Load(object sender, EventArgs e)
    {
        String strMsg = Request["Message"];
        divMsg.InnerText = strMsg;
    }
}
