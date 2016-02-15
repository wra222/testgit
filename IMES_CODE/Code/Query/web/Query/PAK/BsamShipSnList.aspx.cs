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
public partial class Query_PAK_BsamShipSnList : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string defaultSelectDB = "";
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_BsamShipSnList PAK_BsamShipSnList = ServiceAgent.getInstance().GetObjectByName<IPAK_BsamShipSnList>(WebConstant.IPAK_BsamShipSnList);

    public string[] GvQueryColumnName = { "Consolidate/DN", "Model", "Qty", "OK_Qty", "NG_Qty", "AvailableLocation" };
    public int[] GvQueryColumnNameWidth = { 80, 50, 30, 30, 30 ,100};


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
        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
    }//width
    
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
    }//nulldatatable
   

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvQueryColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }

        return retTable;
    }//empty data
    
    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
        EnableBtnExcel(this, false, btnExport.ClientID);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            string ShipDate = txtFromDate.Text.ToString();
            string Type = this.SelectType.SelectedValue.ToString();
            if (Type == "DN")
            { dt = PAK_BsamShipSnList.GetBsamShipSnListByDN(DBConnection, ShipDate); }
            else
            { dt = PAK_BsamShipSnList.GetBsamShipSnListByConsolidate(DBConnection, ShipDate); }
            
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
