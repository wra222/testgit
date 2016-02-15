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

public partial class Query_FA_PrdIdOnLine : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string defaultSelectDB = "";
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IFA_PrdIdOnLine PrdIdOnLine = ServiceAgent.getInstance().GetObjectByName<IFA_PrdIdOnLine>(WebConstant.PrdIdOnLine);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
    string DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        defaultSelectDB = this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;

        lblModelCategory.Visible = !iConfigDB.CheckDockingDB(defaultSelectDB);
        ChxLstProductType1.IsHide = iConfigDB.CheckDockingDB(defaultSelectDB);
       
        DBConnection = CmbDBType.ddlGetConnection();
        try
        {
            if (!this.IsPostBack)
            {
                InitPage();
                InitCondition();
            }
            
            /*StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("<script language='javascript'>");
            string Model = lboxModel.ClientID;
            scriptBuilder.AppendLine("$('#" + Model + "').dropdownchecklist({ icon: {}, maxDropHeight: 150, width: 220, emptyText: 'Please select ...' });");
            scriptBuilder.AppendLine("</script>");

            ScriptManager.RegisterStartupScript(UpdatePanel2, typeof(System.Object), "ListModel", scriptBuilder.ToString(), false);*/


            //StringBuilder scriptBuilder = new StringBuilder();
            //scriptBuilder.AppendLine("<script language='javascript'>");
            //scriptBuilder.AppendLine("$('.CheckBoxList').multiselect( { selectedList: 5, position: { my: 'left bottom', at: 'left top'}, noneSelectedText: 'Please PdLine ' } ).multiselectfilter();");
            //scriptBuilder.AppendLine("</script>");
            //ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "beginWaitingCoverDiv", scriptBuilder.ToString(), false);

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
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblPno.Text = this.GetLocalResourceObject(Pre + "_lblPno").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }
    private void InitCondition()
    {
        /*DataTable dtStation = Station.GetStation("2");

        if (dtStation.Rows.Count > 0)
        {
            ddlStation.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtStation.Rows)
            {
                ddlStation.Items.Add(new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Descr"].ToString().Trim(), dr["Station"].ToString().Trim()));
            }
        }*/
      

        List<string> station = new List<string>();
        station.AddRange(FAOnlineStation.Split(','));        
        DataTable dtStation = QueryCommon.GetStationName(station, DBConnection);
        if (dtStation.Rows.Count > 0)
        {
            ddlStation.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtStation.Rows)
            {
                ddlStation.Items.Add(new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim()));
            }
        }

        string customer = Master.userInfo.Customer;        
        //this.CmbPdLine.Customer = customer;
        //this.CmbPdLine.Stage = "FA";
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
        
        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd 00:00");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); 

        /*DataTable dtPno = Model.GetModel();

        if (dtPno.Rows.Count > 0)
        {
            ddlPno.Items.Add(new ListItem("", ""));
         
            foreach (DataRow dr in dtPno.Rows)
            {
                ddlPno.Items.Add(new ListItem(dr["Model"].ToString().Trim(), dr["Model"].ToString().Trim()));
            }
        }*/

        DataTable dtFamily = Family.GetFamily(DBConnection);

        if (dtFamily.Rows.Count > 0)
        {
            ddlFamily.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtFamily.Rows)
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
        int i = 100;
        int j = 70;
        int k = 160;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(30);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(50);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(200);
        gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[8].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[9].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[10].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[11].Width = Unit.Pixel(k);
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["No"] = "";
            newRow["Line"] = "";
            newRow["Family"] = "";
            newRow["Model"] = "";
            newRow["ProductID"] = "";
            newRow["CPQ"] = "";
            newRow["Station"] = "";
            newRow["DN"] = "";
            newRow["PLT"] = "";
            newRow["ShipDate"] = "";
            newRow["Editor"] = "";
            newRow["Udt"] = "";  

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("No", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Family", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("CPQ", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("PLT", Type.GetType("System.String"));
        retTable.Columns.Add("ShipDate", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
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
            string prdType;
            prdType = iConfigDB.CheckDockingDB(defaultSelectDB) ? "" : ChxLstProductType1.GetCheckList();
            DataTable dt = PrdIdOnLine.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                            lstPdLine, ddlStation.SelectedValue, txtModel.Text.Trim(),
                            ddlFamily.SelectedValue, prdType);//txtPno.Text.Trim());                
            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResult.Rows[i].Cells[0].Text = (i + 1).ToString();
                    gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Line"].ToString();
                    gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Family"].ToString();
                    gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Model"].ToString();
                    gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["ProductID"].ToString();
                    gvResult.Rows[i].Cells[5].Text = dt.Rows[i]["CUSTSN"].ToString();
                    gvResult.Rows[i].Cells[6].Text = dt.Rows[i]["Station"].ToString();
                    gvResult.Rows[i].Cells[7].Text = dt.Rows[i]["DeliveryNo"].ToString();
                    gvResult.Rows[i].Cells[8].Text = dt.Rows[i]["PalletNo"].ToString();
                    gvResult.Rows[i].Cells[9].Text = dt.Rows[i]["ShipDate"].ToString();
                    gvResult.Rows[i].Cells[10].Text = dt.Rows[i]["Editor"].ToString();
                    gvResult.Rows[i].Cells[11].Text = dt.Rows[i]["Udt"].ToString();
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
}
