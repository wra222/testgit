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

public partial class Query_FA_IMG_2PPQuery : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string defaultSelectDB;
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);
    IFA_IMG_2PPQuery FA_IMG_2PPQuery = ServiceAgent.getInstance().GetObjectByName<IFA_IMG_2PPQuery>(WebConstant.IFA_IMG_2PPQuery);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);

    public string[] GvQueryColumnName = { "ProductID","CUSTSN", "IMG/2PPCL","Line" };
    public int[] GvQueryColumnNameWidth = { 80, 80,80,80 };

    public string[] GvQueryByModelColumnName = { "ProductID", "CUSTSN", "IMG/2PPCL", "Line" };
    public int[] GvQueryByModelColumnNameWidth = { 80,80,80,80 };
    //public int[] GvQueryByModelfootcellindex = { 2, 3, 4, 5, 7, 8, 10, 11 }; //for PIA
    public int[] GvQueryByModelfootcellindex = { 2, 3 };
    string customer = "";
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
            DBConnection = CmbDBType.ddlGetConnection();
            defaultSelectDB = this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
            //lblModelCategory.Visible = !defaultSelectDB.ToUpper().Equals("HPDOCKING");
            //ChxLstProductType1.IsHide = defaultSelectDB.ToUpper().Equals("HPDOCKING"); HPDocking_Rep
            lblModelCategory.Visible = !iConfigDB.CheckDockingDB(defaultSelectDB);
            ChxLstProductType1.IsHide = iConfigDB.CheckDockingDB(defaultSelectDB);
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
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblTotalCount.Text = this.GetLocalResourceObject(Pre + "_lblTotalCount").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    
    }
    private void InitCondition()
    {
        //this.CmbPdLine.Customer = customer;
        //this.CmbPdLine.Stage = "FA";
       
        GetPdline();

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
        lblTotalQtyCount.Text = "0";
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
            IList<string> lstPdLine = new List<string>();
            foreach (ListItem item in lboxPdLine.Items)
            {
                if (item.Selected)
                {
                    lstPdLine.Add(item.Value);
                }
            }
            IList<string> Model = new List<string>();

            hidModelList.Value = hidModelList.Value.Replace("'", "");
            txtModel.Text = txtModel.Text.Replace("'", "");
            if (txtModel.Text != "")
            {
                Model.Add(txtModel.Text.Trim());
            }
            else
            {
                Model = hidModelList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }

            DataTable dt = new DataTable();
            int sum = 0;
            //  string prdType = defaultSelectDB.ToUpper().Equals("HPDOCKING") ? "" : ChxLstProductType1.GetCheckList();
            string prdType = iConfigDB.CheckDockingDB(defaultSelectDB) ? "" : ChxLstProductType1.GetCheckList();

            if (rbIMG.Checked)
            {

                dt = FA_IMG_2PPQuery.GetQueryResult(Connection,
                                lstPdLine, Model, bool.Parse(hfLineShife.Value), prdType);
                sum = dt.Rows.Count;
                this.gvResult.DataSource = dt;
                this.gvResult.DataBind();
                lblTotalQtyCount.Text = sum.ToString();
                InitGridView();
            }
            else if (rb2PP.Checked)
            {
                dt = FA_IMG_2PPQuery.GetQueryResultByModel(Connection, 
                                lstPdLine, Model, bool.Parse(hfLineShife.Value), prdType);

                sum = dt.Rows.Count;
                this.gvResult.DataSource = dt;
                this.gvResult.DataBind();
                lblTotalQtyCount.Text = sum.ToString();
                InitGridView();

            }
            else
            {
                BindNoData();
                showErrorMessage("Please choose query type");
            }

            if (dt.Rows.Count > 0)
            {
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();
                lblTotalQtyCount.Text = "0";
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
    protected void btnChangeLine_Click(object sender, EventArgs e)
    {
        if (bool.Parse(hfLineShife.Value))
        {
            hfLineShife.Value = "false";
        }
        else
        {
            hfLineShife.Value = "true";
        }
        GetPdline();
    }

    protected void GetPdline()
    {
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");

        DataTable dtPdLine = QueryCommon.GetLine(Process, customer, bool.Parse(hfLineShife.Value), DBConnection);
        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
   

    protected void Process_CheckedChanged(object sender, EventArgs e)
    {
        GetPdline();
    }
}
