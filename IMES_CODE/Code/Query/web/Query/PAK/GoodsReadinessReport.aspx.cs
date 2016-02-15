using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using com.inventec.imes.DBUtility;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

public partial class GoodsReadinessReport : IMESQueryBasePage
{

    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
  
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
      
        try
        { 
            if (!this.IsPostBack)
            {
                InitPage();
                InitCondition();
            }
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
          txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); 
      //  gvResult.DataSource = getNullDataTable("PartNo,Descr,Qty,Line");
        //gvResult.DataBind();
      //  InitGridView();
    }

    private void InitGridView()
    {
        //int i = 100;
        //int j = 70;
        //int k = 150;
        for (int i = 0; i < 11; i++)
        {
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(100);
        }
        //gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
         gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(60);
        //gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(150);
        gvResult.HeaderRow.Cells[10].Width = Unit.Pixel(150);
        //gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(j);
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
      
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
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void ShowTotal(int Count)
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowTotal();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "ShowTotal", script, false);
    }
    private void BindNoData()
    {

        this.gvResult.DataBind();
        InitGridView();

    }

  
 
    
    

  
   
    protected void btnExcel_Click(object sender, EventArgs e)
    {

        ExcelTool.SaveToExcel(this, gvResult, "data", "GoodsReadinessReport", false, 10);
    }
   
    protected void btnQuery_Click(object sender, EventArgs e)
    {

        hidDNList.Value = hidDNList.Value.Replace("'", "");
        txtDN.Text = txtDN.Text.Replace("'", "");
        string[] dnArr = hidDNList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        string dnList = "";
        if (dnArr.Length > 0)
        { dnList = string.Join(",", dnArr); }


        if (!string.IsNullOrEmpty(txtDN.Text))
        {
            dnList = txtDN.Text;
        }
                
        if (string.IsNullOrEmpty(dnList))
        {
            showErrorMessage("Please Input DN!!");
            endWaitingCoverDiv(this);

            return;
        }
        DateTime ShipDate = DateTime.Parse(txtShipDate.Text.Trim());
        try
        {

            DataTable dt = PAK_Common.ReadinessReport(DBConnection, dnList, ShipDate, radItem.SelectedValue.Trim());
            gvResult.DataSource = dt;
            gvResult.DataBind();
            if (dt.Rows.Count > 0)
            {
                InitGridView();
                EnableBtnExcel(this, true, btnExcel.ClientID);
            }
            else
            {
                showErrorMessage("No Data!");
                EnableBtnExcel(this, false, btnExcel.ClientID);
            }

        }
        catch (Exception exp)
        {
            showErrorMessage(exp.Message);

        }
        finally
        {
            endWaitingCoverDiv(this);
        }

    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
}
