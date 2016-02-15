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
using UPH.Interface;
using com.inventec.iMESWEB;

public partial class webroot_UPHMaintain_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string User=Master.userInfo.UserName;
        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd 00:00");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        IAlarmMaintain IAM = ServiceAgent.getInstance().GetObjectByName<IAlarmMaintain>(WebConstant.UPHAlarm);
       DataTable dt= IAM.GetAlarmALL();
       this.gd.DataSource = dt;
       gd.DataBind();
        //txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("00:00");
        //txtToDate.Text = DateTime.Now.ToString("HH:mm");

    }
}
