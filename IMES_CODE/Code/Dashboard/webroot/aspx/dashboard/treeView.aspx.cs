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


public partial class webroot_aspx_dashboard_treeView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_dashboard_treeView));
        AjaxPro.Utility.RegisterTypeForAjax(typeof(com.inventec.portal.dashboard.AccountUserInfo));
        AjaxPro.Utility.RegisterTypeForAjax(typeof(com.inventec.portal.dashboard.common.DashboardCommon));
        
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public DataTable getNodeData(string type, string uuid)
    {
        //log.Debug("type--->" + type);
        //log.Debug("uuid--->" + uuid);

        //DashboardManager manager = new DashboardManager();
        DataTable dt = new DataTable();
        if (type.Equals("NODE_ROOT"))
        {

            dt = com.inventec.portal.dashboard.common.DashboardCommon.GetDashboardWindowNameList();
            //否则有html标签
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                dt.Rows[i]["name"] = HttpContext.Current.Server.HtmlEncode(dt.Rows[i]["name"].ToString());
            }

            return dt;

        }

        //Response.Write("<Script> alert('" + allInfo + "')");

        return dt;
    }


    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public DataTable getEmptyNodeData()
    {        
        DataTable dt = new DataTable();
        return dt;
    }


    private bool checkStringPara(string obj)
    {
        if (null == obj || obj.Trim().Equals(""))
        {
            return false;
        }

        return true;
    }
}
