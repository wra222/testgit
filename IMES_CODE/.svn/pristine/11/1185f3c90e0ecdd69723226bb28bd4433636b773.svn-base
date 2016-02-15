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
using com.inventec.system;


public partial class webroot_aspx_dashboard_dashboardShowData : System.Web.UI.Page
{
    public String resultDataString="";
    protected void Page_Load(object sender, EventArgs e)
    {
        //String windowId = HttpContext.Current.Request.QueryString["windowId"];

        string windowId = Request["windowId"];
        string stageType = Request["stageType"];

        //windowId = windowId.Replace("+", "%2B");
        if (stageType == Constants.FA_STAGE.ToString())
        {
            resultDataString = com.inventec.portal.dashboard.Fa.DashboardShowManager.GetShowInfoDashboardStringByWinId(windowId);
        }
        else
        {
            resultDataString = com.inventec.portal.dashboard.Smt.DashboardShowManager.GetShowInfoDashboardStringByWinId(windowId);
        }

        Response.Clear();
        Response.Write(resultDataString);

    }
}
