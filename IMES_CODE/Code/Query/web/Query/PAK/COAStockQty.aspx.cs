using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
using System.Data;

public partial class Query_PAK_COAStockQty : IMESQueryBasePage
{

    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_COAStockQty COAStockQty = ServiceAgent.getInstance().GetObjectByName<IPAK_COAStockQty>(WebConstant.COAStockQty);

    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    private string DBConnection;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DBConnection = CmbDBType.ddlGetConnection();
            if (!this.IsPostBack)
            {
                InitPage();
                InitCondition();
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private void InitPage()
    {
       

    }
    private void InitCondition()
    {        
      //  txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        
    //    this.gvResult.DataSource = getNullDataTable(1);
    //    this.gvResult.DataBind();
     //   InitGridView();
        InitStatusLine();
    }
    private void InitStatusLine()
    {
        DataTable dt = PAK_Common.GetCoaStatusLine(DBConnection);
        DataRow[] drArr = dt.Select("Type='Status'");
        droLine.Items.Add(new ListItem("ALL", "ALL"));
        foreach (DataRow dr in drArr)
        { 
           ListItem it=new ListItem(dr["Value"].ToString().Trim(),dr["Value"].ToString().Trim());
        //    if(dr["Value"].ToString().Trim()=="P0)
           droStatus.Items.Add(it);
        }
        DataRow[] drArr2 = dt.Select("Type='Line'"," Value ASC");

        foreach (DataRow dr in drArr2)
        {
            ListItem it = new ListItem(dr["Value"].ToString(), dr["Value"].ToString());
            droLine.Items.Add(it);
        }
      
        droStatus.Text = "A0";
    }

    private void InitGridView()
    {
        if (chkSummary.Checked)
        {

            gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(50);
            gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(50);
          
        }
        else
        {
            gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(33);
            gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(33);
            gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(33);
        }
      
           
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["MinID"] = "";
            newRow["MaxID"] = "";
            newRow["Qty"] = "";            

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MinID", Type.GetType("System.String"));
        retTable.Columns.Add("MaxID", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));   
        return retTable;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //beginWaitingCoverDiv();     
        string Connection = CmbDBType.ddlGetConnection();
        try
        {
          
            string line=droLine.SelectedValue.Trim();
          //  if(droStatus.SelectedValue.Trim() =="A0" || droStatus.SelectedValue.Trim() =="P0" || droLine.SelectedValue.Trim()=="ALL")
            if (droLine.SelectedValue.Trim() == "ALL")
            
            {line="";}

            DataTable dt = COAStockQty.GetQueryResultExt(DBConnection, droStatus.SelectedValue.Trim(), line,chkSummary.Checked);
            if (dt.Rows.Count > 0)
            {
                 this.gvResult.DataSource = dt;
                //this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();

                InitGridView();
               EnableBtnExcel(this, true, btnExport.ClientID);
               gvResult.Visible = true;
            }
            else
            {
                gvResult.Visible = false;
                  EnableBtnExcel(this, false, btnExport.ClientID);
                 showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }

        endWaitingCoverDiv();
    }
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void ShowTotal(int Count)
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowTotal();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "ShowTotal", script, false);
    }
    private void BindNoData()
    {
        
        EnableBtnExcel(this, false, btnExport.ClientID);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
   
        int i =chkSummary.Checked?1:2;

        ExcelTool.SaveToExcel(this, gvResult, "data", "COA Stock Qty", false,i);

    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!chkSummary.Checked)
            {
                if (e.Row.Cells[1].Text != "&nbsp;" && e.Row.Cells[1].Text != "")
                {
                    e.Row.CssClass = "data1";
                }
                else
                {
                    e.Row.Cells[1].Text = e.Row.Cells[2].Text;
                    e.Row.Cells[2].Text = "";
                    e.Row.Font.Size = FontUnit.Parse("16px"); //count
                
                }
            }
          
        }
    }

}
