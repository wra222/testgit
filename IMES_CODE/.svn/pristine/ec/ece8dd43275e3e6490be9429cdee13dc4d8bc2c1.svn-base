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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;
using System.Web.Services;


public partial class CommonControl_1234567 : System.Web.UI.Page
{
    static IMaterialRequest imaterialrequest = ServiceAgent.getInstance().GetObjectByName<IMaterialRequest>(WebConstant.SAMBRepairControl);

    public String UserId;
    public String Customer;
    public String Station;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.ServerClick += new EventHandler(btnSave_ServerClick);
    }
    //protected void Button1_Click(object sender, EventArgs e)
    //{

    //   string txt = MBSno2.Text;       

    //   IList<MBRepairControlInfo> ControlInfo = imaterialrequest.GeMaterialRequest(new MBRepairControlInfo { PartNo = txt });
    //   if (ControlInfo != null && ControlInfo.Count > 0)
    //   {
    //       string ID = ControlInfo[0].ID.ToString();
    //       int delID = int.Parse(ID);

    //       if (delID != null)
    //       {
    //           imaterialrequest.DelMBRepairControl(new MBRepairControlInfo { ID = delID });
    //           showSuccessMessage("刪除成功");
    //           MBSno3.Text = MBSno2.Text;
    //           MBSno2.Text = "";
    //           MBSno2.Focus();
    //           return;
    //       }
    //   }
    //   else
    //   {
    //       showErrorMessage("此 MB:" + txt + "不存在!");
    //       return;
    //   } 
    //}
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string mbn = this.HMBSno.Value.Trim();
        IList<MBRepairControlInfo> ControlInfo = imaterialrequest.GeMaterialRequest(new MBRepairControlInfo { PartNo = mbn });
        if (ControlInfo != null && ControlInfo.Count > 0)
        {
            MBRepairControlInfo MBNO = ControlInfo[0];
            if (MBNO.Status == "Input" && MBNO.MaterialType == "MB")
            {
                int delID = ControlInfo[0].ID;
                imaterialrequest.DelMBRepairControl(new MBRepairControlInfo { ID = delID });
                showSuccessMessage("刪除成功");
                
            }
            else
            {
                showErrorMessage("此 MB:" + mbn + "的狀態不能刪除!");
            }
        }
        else
        {
            showErrorMessage("此 MB:" + mbn + "不存在!");
            return;
        } 
    }
    private void showSuccessMessage(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        msg = replaceSpecialChart(msg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("onSaveSucess('" + msg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
