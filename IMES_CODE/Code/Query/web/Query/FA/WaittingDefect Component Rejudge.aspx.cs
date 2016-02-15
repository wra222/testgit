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
//using IMES.Station.Interface.CommonIntf;
using System.Data;
using System.IO;

public partial class Query_FA_WaittingDefect_Component_Rejudge : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IFA_WaittingDefectComponentRejudge WaittingDefectComponentRejudge = ServiceAgent.getInstance().GetObjectByName<IFA_WaittingDefectComponentRejudge>(WebConstant.IFA_WaittingRejudge);


    public string[] GvQueryColumnName = { "BatchID","Customer","Model","Family","DefectCode","DefectDescr",
                                            "ReturnLine","PartSn","PartNo","PartType","BomNodeType","IECPn","CustomerPn","Vendor",
                                            "CheckItemType","Comment","Status","Editor","Cdt","Udt"};
    public int[] GvQueryColumnNameWidth = {  80, 50, 100, 150, 100,200,100,200,200,100,50,100,100,100,100,100,50,100,200,200 };

    string customer = "";
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DBConnection = CmbDBType.ddlGetConnection();

            customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
               // InitPage();
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

   

    private void InitCondition()
    {

       
        if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
        {
            txtPeriodFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
            txtPeriodToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
        }
        else if (DateTime.Now.Hour >= 20)
        {
            txtPeriodFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
            txtPeriodToDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 08:00");
        }
        else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
        {
            txtPeriodFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:30");
            txtPeriodToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
        }
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
    }
    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
        {
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            foreach (string columnname in GvQueryColumnName)
            {
                newRow[columnname] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvQueryColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }

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
        string Connection = CmbDBType.ddlGetConnection();

       
        try
        {
            String partype=drpparttype.SelectedValue;
            String status=dropstatus.SelectedValue;
            DataTable dt = new DataTable();
            //int sum = 0;
            dt = WaittingDefectComponentRejudge.GetDefectComponentRejudgeQueryResult(DBConnection, DateTime.Parse(txtPeriodFromDate.Text), DateTime.Parse(txtPeriodToDate.Text), partype, status);
            if (dt.Rows.Count > 0)
            {
                gvResult.DataSource = dt;
                gvResult.DataBind();
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
        finally
        {
            endWaitingCoverDiv(this);
        }
        
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
