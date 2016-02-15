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

public partial class Query_SA_PCBTestReport : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string defaultSelectDB = "";
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    ISA_PCBTestReport SA_PCBTestReport = ServiceAgent.getInstance().GetObjectByName<ISA_PCBTestReport>(WebConstant.ISA_PCBTestReport);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    public string[] GvQueryColumnName = { "ActionName", "PCBNo", "MAC", "Type", "Status", "FixtureID", "Station", 
                                            "PdLine", "PCBModelID","ErrorCode", "Descr", "Editor","Remark","Cdt" };
    public int[] GvQueryColumnNameWidth = {60, 55, 65, 25, 30, 60, 35, 
                                              30, 60, 60, 45, 45, 45, 100 };
    public string[] GvDetailColumnName = { "PCBNo","PdLine","Station","Defect","Cause","Location",
                                             "Remark","Obligation","Repairer","Editor","TestedDate","RepairedDate" };
    public int[] GvDetailColumnNameWidth = { 50, 30, 30, 30, 30, 40, 
                                               50, 45, 25, 40, 80, 80 };
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
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblOP.Text = this.GetLocalResourceObject(Pre + "_lblOP").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
        
        InitFixtureID();
        InitOP();
    }

    protected void InitFamily()
    {
        DateTime dtFrom = DateTime.Parse(txtFromDate.Text);
        DateTime dtTo = DateTime.Parse(txtToDate.Text);
        DataTable dtFamily = QueryCommon.GetFamily(DBConnection,dtFrom,dtTo);
        
        if (dtFamily.Rows.Count > 0)
        {
            foreach (DataRow dr in dtFamily.Rows)
            {
                lboxFamily.Items.Add(new ListItem(dr["Family"].ToString().Trim(), dr["Family"].ToString().Trim()));
            }
        }
    }

    protected void btnchangetime_Click(object sender, EventArgs e)
    {
        lboxFamily.Items.Clear();
        try
        {
            InitFamily();
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }

    protected void InitFixtureID()
    {
        DataTable dtFixtureID = QueryCommon.GetFixtureID(DBConnection);
        if (dtFixtureID.Rows.Count > 0)
        {
            foreach (DataRow dr in dtFixtureID.Rows)
            {
                lboxFixtureID.Items.Add(new ListItem(dr["FixtureID"].ToString().Trim()));
            }
        }
    }

    protected void InitOP()
    {
        DataTable dtOP = QueryCommon.GetOP(DBConnection);
        if (dtOP.Rows.Count > 0)
        {
            foreach (DataRow dr in dtOP.Rows)
            {
                lboxOP.Items.Add(new ListItem(dr["Editor"].ToString().Trim()));
            }
        }
    }



    private void InitCondition()
    {

        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        InitFamily();
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        this.grvDetail.DataSource = getNullDetailDataTable(1);
        this.grvDetail.DataBind();
        InitGridView();
    }

    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
        {
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
        for (int i = 0; i < GvDetailColumnNameWidth.Length; i++)
        {
            grvDetail.HeaderRow.Cells[i].Width = Unit.Pixel(GvDetailColumnNameWidth[i]);
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
    private DataTable getNullDetailDataTable(int j)
    {
        DataTable dt = initDetailTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            foreach (string columnname in GvDetailColumnName)
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
    private DataTable initDetailTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvDetailColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }
        return retTable; 
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        List<int> selList = new List<int>();
        string cmd = "";
        string PCBNo = "";
        PCBNo = e.Row.Cells[1].Text;

        cmd = string.Format("ShowDetail('{0}')", PCBNo);
        e.Row.Attributes.Add("onclick", cmd);
    }
    
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {

        try
        { 
            string PCBNo = "";
            PCBNo = hidCol.Value;
            DataTable dt = new DataTable();

            dt = SA_PCBTestReport.GetRepairDetailResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text), PCBNo);

            if (dt.Rows.Count > 0)
            {
                grvDetail.DataSource = dt;
                grvDetail.DataBind();
                InitGridView();
                EnableBtnExcel(this, true, btnDetailExport.ClientID);
            }
            else
            {
                BindNoDetailData();
                return;
            }
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoDetailData();        
        }
        finally
        {
            endWaitingCoverDiv(this);
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

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string Connection = CmbDBType.ddlGetConnection();
        try
        {
            IList<string> lstFamily = new List<string>();
            for (int i = 0; i < lboxFamily.Items.Count; i++)
            {
                if (lboxFamily.Items[i].Selected)
                {
                    lstFamily.Add(lboxFamily.Items[i].Value);
                }
            }
            IList<string> lstTestItem = new List<string>();
            for (int i = 0; i < lboxTestItem.Items.Count; i++)
            {
                if (lboxTestItem.Items[i].Selected)
                {
                    lstTestItem.Add(lboxTestItem.Items[i].Value);
                }
            }
            IList<string> lstFixtureID = new List<string>();
            for (int i = 0; i < lboxFixtureID.Items.Count; i++)
            {
                if (lboxFixtureID.Items[i].Selected)
                {
                    lstFixtureID.Add(lboxFixtureID.Items[i].Value);
                }
            }
            IList<string> lstSation = new List<string>();
            for (int i = 0; i < lboxStation.Items.Count; i++)
            {
                if (lboxStation.Items[i].Selected)
                {
                    lstSation.Add(lboxStation.Items[i].Value);
                }
            }
            IList<string> lstOP = new List<string>();
            for (int i = 0; i < lboxOP.Items.Count; i++)
            {
                if (lboxOP.Items[i].Selected)
                {
                    lstOP.Add(lboxOP.Items[i].Value);
                }
            }
            DataTable dt = new DataTable();
            dt = SA_PCBTestReport.GetPCBTestReportResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                                lstFamily, lstTestItem, lstFixtureID, lstSation, lstOP);


            //DataColumn colstring = new DataColumn("StringCol");
            //colstring.DataType = System.Type.GetType("System.String");
            //dt.Columns.Add(colstring);
            //dt.Columns[13].ColumnName = "Status1";

            //for (int i = 0; i < dt.Rows.Count - 1; i++)
            //{
            //    if (dt.Rows[i]["Status"].ToString() == "1")
            //    {
            //        dt.Rows[i]["Status1"] = "Pass";
            //    }
            //    else if (dt.Rows[i]["Status"].ToString() == "0")
            //    {
            //        dt.Rows[i]["Status1"] = "Fail";
            //    }
            //}

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Family");
                //dt.Columns.Remove("Status");
                gvResult.DataSource = dt;
                gvResult.DataBind();
                InitGridView();
                BindNoDetailData();
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();
                BindNoDetailData();
                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
            BindNoDetailData();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }
    
    private void BindNoDetailData()
    {
        this.grvDetail.DataSource = getNullDetailDataTable(1);
        this.grvDetail.DataBind();
        InitGridView();
        EnableBtnExcel(this, false, btnDetailExport.ClientID);
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
    protected void btnDetailExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(grvDetail, Page.Title, Page);
    }
}
