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
using System.IO;
using System.Configuration;

[System.Runtime.InteropServices.GuidAttribute("5C65E95A-72B7-456C-BF68-18D106E5CD1B")]
public partial class Query_PAK_BsamLocationQuery : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string defaultSelectDB = "";
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_BsamLocationQuery PAK_BsamLocationQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_BsamLocationQuery>(WebConstant.IPAK_BsamLocationQuery);
    //private static DBInfo objDbInfo;
    //private string defaultSelectDBType;
    //private static string _onLinesConnString;
    //private static string _hisLinesConnString;
    public string[] GvQueryColumnNameForSummary = { "LocationID","Model","Qty" };
    public int[] GvQueryColumnNameWidthForSummary = { 50,50,50};
    public string[] GvQueryColumnNameForDetail = { "LocationID", "Model", "CUSTSM", "Line", "Station", "LocationAssignedTime" };
    public int[] GvQueryColumnNameWidthForDetail = { 50, 50, 50, 50, 50, 50 };

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
        this.gvResult.DataSource = getNullDataTableForSummary(1);
        this.gvResult.DataBind();
        InitGridViewForSummary();
    }

    private void InitGridViewForSummary()
    {
        for (int i = 0; i < GvQueryColumnNameWidthForSummary.Length; i++)
        {
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidthForSummary[i]);
        }
    }//width
    private void InitGridViewForDetail()
    {
        for (int i = 0; i < GvQueryColumnNameWidthForDetail.Length; i++)
        {
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidthForDetail[i]);
        }
    }

    private DataTable getNullDataTableForSummary(int j)
    {
        DataTable dt = initTableForSummary();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            foreach (string columnname in GvQueryColumnNameForSummary)
            {
                newRow[columnname] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }//nulldatatable
    private DataTable getNullDataTableForDetail(int j)
    {
        DataTable dt = initTableForDetail();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            foreach (string columnname in GvQueryColumnNameForDetail)
            {
                newRow[columnname] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTableForSummary()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvQueryColumnNameForSummary)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }

        return retTable;
    }//empty data
    private DataTable initTableForDetail()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvQueryColumnNameForDetail)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }

        return retTable;
    }

    private void GetSummaryReport()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PAK_BsamLocationQuery.GetResultForSummary(DBConnection);
            if (dt.Rows.Count > 0)
            {
                gvResult.DataSource = dt;
                gvResult.DataBind();
                InitGridViewForSummary();
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoDataForSummary();
                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoDataForSummary();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
        
    }

    private void GetDetailReport()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PAK_BsamLocationQuery.GetResultForDetail(DBConnection);
            if (dt.Rows.Count > 0)
            {
                gvResult.DataSource = dt;
                gvResult.DataBind();
                InitGridViewForDetail();
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoDataForDetail();
                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoDataForSummary();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }

    private void BindNoDataForSummary()
    {
        this.gvResult.DataSource = getNullDataTableForSummary(1);
        this.gvResult.DataBind();
        InitGridViewForSummary();
        EnableBtnExcel(this, false, btnExport.ClientID);
    }

    private void BindNoDataForDetail()
    {
        this.gvResult.DataSource = getNullDataTableForDetail(1);
        this.gvResult.DataBind();
        InitGridViewForDetail();
        EnableBtnExcel(this, false, btnExport.ClientID);
    }


    protected void btnQuery_Click(object sender, EventArgs e)
    {
            DataTable dt = new DataTable();
            string Type = this.SelectType.SelectedValue.ToString();
            switch (Type)
            {
                case "Summary" :
                    GetSummaryReport();
                    break;
                case "Detail":
                    GetDetailReport();
                    break;
            }
    }
    
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, Page.Title, Page);
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int j = 1; j < e.Row.Cells.Count; j++)
            {
                e.Row.Cells[j].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.className='rowclient' ");
                e.Row.Cells[j].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
            }
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
