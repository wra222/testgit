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

public partial class Query_FA_ProductStatement: IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string defaultSelectDB;
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);
    IFA_ProductStatement ProductStatement = ServiceAgent.getInstance().GetObjectByName<IFA_ProductStatement>(WebConstant.ProductStatement);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    

    public string[] GvQueryColumnName = { "No", "ProductID", "Family", "CUSTSN", "Desc", "Model", "Station", "Status", "Line", "Editor", "Udt" };
    public int[] GvQueryColumnNameWidth = { 50, 80, 100, 80, 60, 90, 150, 80, 80, 120, 150 };

    public string[] GvQueryByModelColumnName = { "Model", "Line", "F0InputSum", "BoardInputSum", "EPIAInput", "EPIAFail", "EPIAFRate", "PIAInput", "PIAFail", "PIAFRate", "PAQCInput", "PAQCFail", "PAQCFRate" };
    public int[] GvQueryByModelColumnNameWidth = { 100, 60, 100, 100, 80, 80, 80, 80, 80, 80, 80, 80, 80 };
    //public int[] GvQueryByModelfootcellindex = { 2, 3, 4, 5, 7, 8, 10, 11 }; //for PIA
    public int[] GvQueryByModelfootcellindex = { 2, 3, 4, 5, 10, 11 };

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
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblTotalCount.Text = this.GetLocalResourceObject(Pre + "_lblTotalCount").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }

    private void InitCondition()
    {
        //this.CmbPdLine.Customer = customer;
        //this.CmbPdLine.Stage = "FA";
        if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
        {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
        }
        else if (DateTime.Now.Hour >= 20)
        {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
            txtToDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 08:00");
        }
        else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
        {
            txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:30");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
        }


        GetPdline();
        GetStation();

        DataTable dtPno = Family.GetFamily(DBConnection);

        if (dtPno.Rows.Count > 0)
        {
            ddlFamily.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dtPno.Rows)
            {
                ddlFamily.Items.Add(new ListItem(dr["Family"].ToString().Trim(), dr["Family"].ToString().Trim()));
            }
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
    private void InitGridViewByModel()
    {
        for (int i = 0; i < GvQueryByModelColumnNameWidth.Length; i++)
		{
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryByModelColumnNameWidth[i]);
		}     
    }

    private void InitGridViewStyleByModel() {
        for (int i = 0; i < gvResult.Rows.Count; i++)
        {
            if (double.Parse(gvResult.Rows[i].Cells[6].Text.ToString()) > 0) {                
                gvResult.Rows[i].Cells[6].CssClass = "querycell";
            }

            if (double.Parse(gvResult.Rows[i].Cells[12].Text.ToString()) > 0) {                
                gvResult.Rows[i].Cells[12].CssClass = "querycell";
            }
            gvResult.Rows[i].Cells[6].Text = string.Format("{0:P}", double.Parse(gvResult.Rows[i].Cells[6].Text));
            gvResult.Rows[i].Cells[12].Text = string.Format("{0:P}", double.Parse(gvResult.Rows[i].Cells[12].Text));
        }


        if (double.Parse(gvResult.FooterRow.Cells[6].Text.ToString()) > 0)
        {
            gvResult.FooterRow.Cells[6].CssClass = "querycell";
        }

        if (double.Parse(gvResult.FooterRow.Cells[12].Text.ToString()) > 0)
        {
            gvResult.FooterRow.Cells[12].CssClass = "querycell";
        }
        gvResult.FooterRow.Cells[6].Text = string.Format("{0:P}", double.Parse(gvResult.FooterRow.Cells[6].Text));
        gvResult.FooterRow.Cells[12].Text = string.Format("{0:P}", double.Parse(gvResult.FooterRow.Cells[12].Text));
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
            
            if (rbDetail.Checked)
            {
             
                dt = ProductStatement.GetQueryResult(Connection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                                lstPdLine, ddlFamily.SelectedValue, ddlStation.SelectedValue, Model, bool.Parse(hfLineShife.Value), prdType);
                sum = dt.Rows.Count;
                this.gvResult.DataSource = dt;
                this.gvResult.DataBind();
                lblTotalQtyCount.Text = sum.ToString();
                InitGridView();
            }
            else if (rbModel.Checked)
            {
                dt = ProductStatement.GetQueryResultByModel(Connection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                                lstPdLine, ddlFamily.SelectedValue, ddlStation.SelectedValue, Model, bool.Parse(hfLineShife.Value), prdType);

                double[] footcell = new double[dt.Columns.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine(i);
                    foreach (int j in GvQueryByModelfootcellindex)
                    {
                        double tmp = 0;
                        footcell[j] += double.TryParse(dt.Rows[i][j].ToString(), out tmp) ? tmp : 0;
                    }
                    sum += int.Parse(dt.Rows[i]["BoardInputSum"].ToString());
                    dt.Rows[i]["EPIAFRate"] = (dt.Rows[i]["EPIAInput"].ToString() == "0" || dt.Rows[i]["EPIAInput"].ToString() == "") ? 0 : Math.Round(float.Parse(dt.Rows[i]["EPIAFail"].ToString()) / float.Parse(dt.Rows[i]["EPIAInput"].ToString()), 4, MidpointRounding.AwayFromZero);
                    dt.Rows[i]["PIAFRate"] = "--";
                    dt.Rows[i]["PAQCFRate"] = (dt.Rows[i]["PAQCInput"].ToString() == "0" || dt.Rows[i]["PAQCInput"].ToString() == "") ? 0 : Math.Round(float.Parse(dt.Rows[i]["PAQCFail"].ToString()) / float.Parse(dt.Rows[i]["PAQCInput"].ToString()), 4, MidpointRounding.AwayFromZero);
                }

                this.gvResult.DataSource = dt;
                this.gvResult.DataBind();

                footcell[6] = footcell[4] == 0 ? 0 : footcell[5] / footcell[4];
                //footcell[9] = footcell[7] == 0 ?0: footcell[8] / footcell[7] ;
                footcell[12] = footcell[10] == 0 ? 0 : footcell[11] / footcell[10];

                gvResult.FooterRow.Cells[1].Text = "TOTAL";
                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    gvResult.FooterRow.Cells[j].Text = footcell[j].ToString();
                    gvResult.FooterRow.Cells[j].Font.Bold = true;
                    gvResult.FooterRow.Cells[j].Font.Size = FontUnit.Parse("16px");
                }
                gvResult.FooterStyle.Font.Bold = true;
                gvResult.FooterRow.Visible = true;
                gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;

                lblTotalQtyCount.Text = sum.ToString();
                InitGridViewByModel();
                InitGridViewStyleByModel();

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
        finally {

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
    protected void GetStation() {
        List<string> station = new List<string>();
        station.AddRange(FAOnlineStation.Split(','));
        station.AddRange(PAKOnlineStation.Split(','));
        DataTable dtStation = QueryCommon.GetStationName(station,DBConnection);
        if (dtStation.Rows.Count > 0)
        {
            ddlStation.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtStation.Rows)
            {
                ddlStation.Items.Add(new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim()));
            }
        }
    }

    protected void Process_CheckedChanged(object sender, EventArgs e)
    {
        GetPdline();
    }
    protected void ddlStation_SelectedIndexChanged(object sender, EventArgs e)
    {
        hidModelList.Value = "";
    }
    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        hidModelList.Value = "";

    }
}
