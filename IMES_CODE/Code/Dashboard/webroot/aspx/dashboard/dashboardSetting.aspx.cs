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
using com.inventec.portal.dashboard.Fa;
using com.inventec.system;

public partial class webroot_aspx_dashboard_dashboardSetting : System.Web.UI.Page
{
    public string stageName;

    protected void Page_Load(object sender, EventArgs e)
    {
   
        AjaxPro.Utility.RegisterTypeForAjax(typeof(DashboardManager));
        stageName=com.inventec.portal.dashboard.common.DashboardCommon.GetStageNameByType(Constants.FA_STAGE.ToString());

    }
}
