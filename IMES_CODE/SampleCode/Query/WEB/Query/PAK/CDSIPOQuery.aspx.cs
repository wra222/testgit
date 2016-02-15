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

public partial class Query_PAK_CDSIPOQuery : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_CDSIPOQuery CDSIPOQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_CDSIPOQuery>(WebConstant.CDSIPOQuery);

    public string[] GvQueryColumnName = { "PO", "PasswordQty", "POQty", "EDIPOQty" };
    public int[] GvQueryColumnNameWidth = { 150, 150, 150, 150 };

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
            showErrorMessage(ex.mErrmsg, this);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message, this);
        }
    }
    
    private void InitPage()
    {
        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblHPPO.Text = this.GetLocalResourceObject(Pre + "_lblHPPO").ToString();
    }

    private void InitCondition()
    {
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

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string Connection = CmbDBType.ddlGetConnection();

        try
        {
            String PO;

            hidModelList.Value = hidModelList.Value.Replace("'", "");
            txtHPPO.Text = txtHPPO.Text.Replace("'", "");
            if (txtHPPO.Text != "")
            {
                PO = txtHPPO.Text;
            }
            else
            {
                PO = hidModelList.Value;
            }

            DataTable dt = new DataTable();
            dt = CDSIPOQuery.GetQueryResult(Connection,PO);  
                          
            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = dt;
                this.gvResult.DataBind();
                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();

                showErrorMessage("Not Found Any Information!!",this);
            }
            
            
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message, this);       
        }

        endWaitingCoverDiv();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, Page.Title, Page);
    }
    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
        EnableBtnExcel(this, false, btnExport.ClientID);
    }
}
