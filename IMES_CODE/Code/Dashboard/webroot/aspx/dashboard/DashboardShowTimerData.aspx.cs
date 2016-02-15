using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.inventec.portal.dashboard.common;


public partial class webroot_aspx_dashboard_DashboardShowTimerData : System.Web.UI.Page
{
    public String resultDataString = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        resultDataString = DashboardCommon .GetCurrentTimeInfoString();
        Response.Clear();
        Response.Write(resultDataString);
    }

}
