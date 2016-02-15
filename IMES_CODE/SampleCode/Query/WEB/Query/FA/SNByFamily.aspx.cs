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

public partial class Query_FA_SNByFamily : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);
    IFA_SNByFamily SNByFamily = ServiceAgent.getInstance().GetObjectByName<IFA_SNByFamily>(WebConstant.SNByFamily);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    string DBConnection = "";
    public string defaultSelectDB;

    protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        DBConnection =  CmbDBType.ddlGetConnection();
        defaultSelectDB = this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
        //lblModelCategory.Visible = !defaultSelectDB.ToUpper().Equals("HPDOCKING");
        //ChxLstProductType1.IsHide = defaultSelectDB.ToUpper().Equals("HPDOCKING");
        lblModelCategory.Visible = !iConfigDB.CheckDockingDB(defaultSelectDB);
        ChxLstProductType1.IsHide = iConfigDB.CheckDockingDB(defaultSelectDB);
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
        this.lblShipDate.Text = this.GetLocalResourceObject(Pre + "_lblShipDate").ToString();
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");

    }
    private void InitCondition()
    {
        DataTable dtFamily = Family.GetFamily(DBConnection);

        if (dtFamily.Rows.Count > 0)
        {
            ddlFamily.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtFamily.Rows)
            {
                ddlFamily.Items.Add(new ListItem(dr["Family"].ToString().Trim(), dr["Family"].ToString().Trim()));
            }
        }

        string customer = Master.userInfo.Customer;
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        DataTable dtPdLine = PdLine.GetPdLine(customer, Process, DBConnection);
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd 00:00:00");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");        
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
    }

    private void InitGridView()
    {
        int i = 100;
        int j = 70;
        int k = 150;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(30);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(30);
        gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(j);
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["No"] = "";
            newRow["Family"] = "";
            newRow["Model"] = "";
            newRow["Line"] = "";
            newRow["Product"] = "";
            newRow["Sno"] = "";
            newRow["Station"] = "";
            newRow["Udt"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("No", Type.GetType("System.String"));
        retTable.Columns.Add("Family", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));        
        retTable.Columns.Add("Product", Type.GetType("System.String"));
        retTable.Columns.Add("Sno", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Udt", Type.GetType("System.String"));
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
        int Count = 0;
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
            DateTime tmp = new DateTime();
            DataTable dt;
           // string prdType = defaultSelectDB.ToUpper().Equals("HPDOCKING") ? "" : ChxLstProductType1.GetCheckList();
            string prdType = iConfigDB.CheckDockingDB(defaultSelectDB) ? "" : ChxLstProductType1.GetCheckList();
            
            if(DateTime.TryParse(txtShipDate.Text.Trim(),out tmp)){
                dt = SNByFamily.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                        ddlFamily.SelectedValue, txtModel.Text.Trim(), lstPdLine, DateTime.Parse(txtShipDate.Text.Trim()), prdType);

            }
            else{
                dt = SNByFamily.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                        ddlFamily.SelectedValue, txtModel.Text.Trim(), lstPdLine, tmp, prdType);
            }
            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResult.Rows[i].Cells[0].Text = (i+1).ToString();
                    gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Family"].ToString();
                    gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Model"].ToString();
                    gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Line"].ToString();                    
                    gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["ProductID"].ToString();
                    gvResult.Rows[i].Cells[5].Text = dt.Rows[i]["CUSTSN"].ToString();
                    gvResult.Rows[i].Cells[6].Text = dt.Rows[i]["Station"].ToString();
                    gvResult.Rows[i].Cells[7].Text = dt.Rows[i]["Udt"].ToString();
                }
                InitGridView();
                Count = dt.Rows.Count;
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();                
                showErrorMessage("Not Found Any Information!!");
                Count = 0;
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }

        endWaitingCoverDiv(this);
        ShowTotal(Count);
    }
   

    private void ShowTotal(int Count)
    {
        htmlHidden.Value = Count.ToString();
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
