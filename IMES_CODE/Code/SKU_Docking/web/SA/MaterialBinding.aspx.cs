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
using System.ComponentModel;


public partial class SA_MaterialBinding : IMESBasePage
{
    private const int DEFAULT_ROWS = 15;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    IMaterialRequest imaterialrequest = ServiceAgent.getInstance().GetObjectByName<IMaterialRequest>(WebConstant.SAMBRepairControl);

    protected void Page_Load(object sender, EventArgs e)
    {
        UserId = Master.userInfo.UserId;
    }
    //protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    //{


    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        for (int i = 0; i < e.Row.Cells.Count; i++)
    //        {
    //            if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
    //            {
    //                e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
    //            }
    //        }

    //    }
    //}
    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string AssignUser = txtAssignUser.Text.ToUpper();
            string PCBNo = txtPCBNo.Text.ToUpper();
            IList<MBRepairControlInfo> ControlInfo = imaterialrequest.GeMaterialRequest(new MBRepairControlInfo { PartNo = PCBNo});
            if (ControlInfo.Count == 0)
            {

                MBRepairControlInfo info = new MBRepairControlInfo();
                info.AssignUser = AssignUser;
                info.PartNo = PCBNo;
                info.MaterialType = "MB";
                info.Stage = "SA";
                info.Family = "";
                info.PCBModelID = "";
                info.Line = "";
                info.Location = "";
                info.Editor = UserId;
                info.Qty = 1;
                info.Status = "Input";
                info.Cdt = DateTime.Now;
                info.Udt = DateTime.Now;
                imaterialrequest.AddMBRepairControl(info);
                showSuccessMessage("保存成功");
            }
            else 
            {
                showErrorMessage("MB号:" + PCBNo + "已存在！");
                //MB号:"+PCBNo+"已存在！
                return;
            }

        }
        finally
        {
          
        }
    }
    //protected void bindgd(string status)
    //{
    //    IList<MBRepairControlInfo> ControlInfo = imaterialrequest.GeMaterialRequest(new MBRepairControlInfo { Status = status, MaterialType = "Component" });
    //}
    //private void setColumnWidth()
    //{
    //    gd.HeaderRow.Cells[0].Width = Unit.Parse("30");
    //    gd.HeaderRow.Cells[1].Width = Unit.Parse("90");
    //    gd.HeaderRow.Cells[2].Width = Unit.Parse("50");
    //    gd.HeaderRow.Cells[3].Width = Unit.Parse("90");
    //    gd.HeaderRow.Cells[4].Width = Unit.Parse("90");
    //    gd.HeaderRow.Cells[5].Width = Unit.Parse("50");
    //    gd.HeaderRow.Cells[6].Width = Unit.Parse("90");
    //    gd.HeaderRow.Cells[7].Width = Unit.Parse("50");
    //    gd.HeaderRow.Cells[8].Width = Unit.Parse("50");
    //    gd.HeaderRow.Cells[9].Width = Unit.Parse("50");
    //    gd.HeaderRow.Cells[10].Width = Unit.Parse("50");
    //    gd.HeaderRow.Cells[11].Width = Unit.Parse("100");
    //    gd.HeaderRow.Cells[12].Width = Unit.Parse("50");
    //    gd.HeaderRow.Cells[13].Width = Unit.Parse("130");
    //    gd.HeaderRow.Cells[14].Width = Unit.Parse("130");

    //}
    private void showErrorMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("senderrorcallback();ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private void showSuccessMessage(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        msg = replaceSpecialChart(msg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("onSaveSucess('" + msg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showSuccessMessage", scriptBuilder.ToString(), false);
    }
}
