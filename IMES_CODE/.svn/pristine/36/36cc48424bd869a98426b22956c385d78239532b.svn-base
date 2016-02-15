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

public partial class Query_ReworkQuery : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string[] GvQueryColumnName = { "Line", "Family", "Model", "CUSTSN", "Descr", "Editor", "Cdt", "Status","Fdt" };
    public int[] GvQueryColumnNameWidth = { 40, 80, 80, 80, 100, 140, 80, 80,80 };
    IFA_ReworkQuery FA_ReworkQuery = ServiceAgent.getInstance().GetObjectByName<IFA_ReworkQuery>(WebConstant.IFA_ReworkQuery);

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
               //InitPage();
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

  /* private void InitPage()
    {
        throw new NotImplementedException();
    }*/
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

    //private void InitPage()
    //{
    //    this.lblDB.Text = this.GetLocalResourceObject("eng_lblDB").ToString();
    //    this.lblDate.Text = this.GetLocalResourceObject("eng_lblDate").ToString();
    //    this.lblFrom.Text = this.GetLocalResourceObject("eng_lblFrom").ToString();
    //    this.lblTo.Text = this.GetLocalResourceObject("eng_lblTo").ToString();
    //    this.btnQuery.InnerText = this.GetLocalResourceObject("eng_btnQuery").ToString();
    //    this.btnExport.InnerText = this.GetLocalResourceObject("eng_btnExcel").ToString();

    //}

    private void InitCondition()
    {

        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string Connection = CmbDBType.ddlGetConnection();
        try
        {
            DataTable dt = new DataTable();
            //int sum = 0;
            dt = FA_ReworkQuery.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text));

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


}
