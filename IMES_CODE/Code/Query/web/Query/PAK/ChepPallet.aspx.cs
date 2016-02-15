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

public partial class Query_PAK_ChepPallet : IMESQueryBasePage
{

    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
     IPAK_ChepPallet ChepPallet = ServiceAgent.getInstance().GetObjectByName<IPAK_ChepPallet>(WebConstant.ChepPallet);        

    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
              InitPage();
              InitCondition();
            }
        }
        catch (FisException ex)
        {
           // showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
           // showErrorMessage(ex.Message);
        }
    }

    private void InitPage()
    {
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblDNDate.Text = this.GetLocalResourceObject(Pre + "_lblDNDate").ToString();
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();        
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");

    }
    private void InitCondition()
    {
        txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        txtEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
    }

    private void InitGridView()
    {
        int i = 100;
        int j = 70;
        int k = 150;
        //gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        //gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(j);
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Carrier"] = "";
            newRow["Truck No"] = "";
            newRow["Driver"] = "";
            newRow["Pallet No"] = "";
            newRow["RFID Pallet No"] = "";
     
            newRow["Cdt"] = "";
            newRow["Udt"] = "";
            newRow["Editor"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        //Carrier/Truck No./Driver/Pallet No./RFID Pallet No./Cdt/Udt/Editor
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Carrier", Type.GetType("System.String"));
        retTable.Columns.Add("Truck No", Type.GetType("System.String"));
        retTable.Columns.Add("Driver", Type.GetType("System.String"));
        retTable.Columns.Add("Pallet No", Type.GetType("System.String"));
        retTable.Columns.Add("RFID Pallet No", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        retTable.Columns.Add("Udt", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
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

            DateTime ToDate = DateTime.Now;
            string toStrDate = txtEndDate.Text.Trim() + " 23:59:59.999";
            ToDate = DateTime.ParseExact(toStrDate, "yyyy-MM-dd HH:mm:ss.fff", null);

            DataTable dt = ChepPallet.GetQueryResult(Connection, DateTime.Parse(txtStartDate.Text), ToDate);                
            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();
                //   //Carrier/Truck No./Driver/Pallet No./RFID Pallet No./Cdt/Udt/Editor
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["Carrier"].ToString();
                    gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Truck No"].ToString();
                    gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Driver"].ToString();
                    
                    gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Pallet No"].ToString();
                    gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["RFID Pallet No"].ToString();
                     gvResult.Rows[i].Cells[5].Text = dt.Rows[i]["Cdt"].ToString();
                    gvResult.Rows[i].Cells[6].Text = dt.Rows[i]["Udt"].ToString();
                    gvResult.Rows[i].Cells[7].Text = dt.Rows[i]["Editor"].ToString();
                }
                InitGridView();                
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();
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
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
        EnableBtnExcel(this, false, btnExport.ClientID);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, Page.Title, Page);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }    
}
