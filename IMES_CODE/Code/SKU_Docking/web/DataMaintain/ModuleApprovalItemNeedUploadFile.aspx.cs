using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using System.Web.UI.WebControls;
using System.Web;
using System.Text.RegularExpressions;

public partial class ModuleApprovalItemNeedUploadFile : System.Web.UI.Page
{
    private const int COL_NUM = 4;
    private const int DEFAULT_ROWS = 36;
    String FamilyTop = "";
    String approvalItemID = "";
    
    private IModuleApprovalItem iModuleApprovalItem;

    protected void Page_Load(object sender, EventArgs e)
    {
        iModuleApprovalItem = ServiceAgent.getInstance().GetMaintainObjectByName<IModuleApprovalItem>(WebConstant.ModuleApprovalItemObject);
        approvalItemID = Request.QueryString["approvalItemID"].ToString();
        FamilyTop = Request.QueryString["familyTop"].ToString();
        if (string.IsNullOrEmpty(approvalItemID))
        {
            showErrorMessage("請選擇一筆資料後再查詢");
            return;
        }
        getFamilyList();
        this.lblfamily.Text = FamilyTop;
    }

    private void getFamilyList()
    {
        bindTable(iModuleApprovalItem.GetApprovalItemAttrList(approvalItemID),DEFAULT_ROWS);
    }

    private void bindTable(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Family");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");

        if (list != null && list.Rows.Count != 0)
        {
            foreach (DataRow item in list.Rows)
            {
                dr = dt.NewRow();
                dr[0] = item["Family"].ToString().Trim();
                dr[1] = item["Editor"].ToString().Trim();
                var time = DateTime.Parse(item["Cdt"].ToString().Trim());
                dr[2] = time.ToString("yyyy-MM-dd HH:mm:ss");
                time = DateTime.Parse(item["Udt"].ToString().Trim());
                dr[3] = time.ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);
            }

            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            this.hidRecordCount.Value = list.Rows.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.hidRecordCount.Value = "";
        }
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
    }
    
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[0].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
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

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

}
