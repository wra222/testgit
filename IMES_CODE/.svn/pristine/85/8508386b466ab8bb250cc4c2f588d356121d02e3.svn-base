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
using com.inventec.portal.dashboard.Smt;

public partial class webroot_aspx_dashboard_dashboardSmtShow : System.Web.UI.Page
{

    public string dashboardWindowBottomRemain;
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(DashboardShowManager));
        if (ConfigurationManager.AppSettings["DashboardShowBottomRemain"] == null)
        {
            dashboardWindowBottomRemain = "0";
        }
        else
        {
            dashboardWindowBottomRemain = ConfigurationManager.AppSettings["DashboardShowBottomRemain"].ToString();
        }
    }

}
