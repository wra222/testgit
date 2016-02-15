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

public partial class Query_FA_PIADefectList : IMESQueryBasePage
{
    public string defaultSelectDB ;
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IStation iStation = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
    IFA_PIADefectList PIADefectList = ServiceAgent.getInstance().GetObjectByName<IFA_PIADefectList>(WebConstant.PIADefectList);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    //IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    ICause Cause = ServiceAgent.getInstance().GetObjectByName<ICause>(WebConstant.QueryCommon);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    public string PAKOnlineStation = WebCommonMethod.getConfiguration("PAKOnlineStation");
    public string FAOnlineStation = WebCommonMethod.getConfiguration("FAOnlineStation");

    public string[] GvQueryColumnName = { "ProductID", "Family", "Model", "Month","Week","Line", "Shift" , "Station" , 
                                          "Defect", "Cause" , "MajorPart","Remark" ,"Obligation",
                                          "OldPart", "OldPartSno", "NewPart", "NewPartSno" , 
                                           "FailureDescription","PartCategory","Catergy","CUSTSN","BIOS","IMAGE","ComponentDesignator","Editor",  "Udt" ,};
    public int[] GvQueryColumnNameWidth = { 100, 120, 120, 50, 50, 50, 50 , 50,
                                            80, 50, 80, 230,50, 
                                            100, 100, 100, 100 ,
                                            230, 120, 120, 80, 80, 80, 120,80, 180 };
    String DBConnection = "";

    string customer = "";
    bool isiqdc = false;
      protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        DBConnection = CmbDBType.ddlGetConnection();
        customer = Master.userInfo.Customer;
        defaultSelectDB = this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
          string urliqdc=this.Page.Request["IQDC"]  ?? "";
          if (urliqdc=="Y")
          {
            isiqdc=true;
          }
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
        this.lblRD.Text = this.GetLocalResourceObject(Pre + "_lblRD").ToString();
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
        this.lblDefect.Text = this.GetLocalResourceObject(Pre + "_lblDefect").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }
    private void InitCondition()
    {       
        GetPdline();
        GetStation();
        GetDefect();
        GetCause();

        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd 00:00");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); 
        
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
        try
        {
            DataTable dt = GetQuery();  

            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataSource = dt;
                this.gvResult.DataBind();

                //for (int i = 0; i <= dt.Rows.Count - 1; i++)
                //{
                //    gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["ProductID"].ToString();
                //    gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Model"].ToString();
                //    gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Line"].ToString();                    
                //    gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Station"].ToString();
                //    gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["OldPart"].ToString();
                //    gvResult.Rows[i].Cells[5].Text = dt.Rows[i]["OldPartSno"].ToString();
                //    gvResult.Rows[i].Cells[6].Text = dt.Rows[i]["NewPart"].ToString();
                //    gvResult.Rows[i].Cells[7].Text = dt.Rows[i]["NewPartSno"].ToString();
                //    gvResult.Rows[i].Cells[8].Text = dt.Rows[i]["Editor"].ToString();
                //    gvResult.Rows[i].Cells[9].Text = dt.Rows[i]["Defect"].ToString();
                //    gvResult.Rows[i].Cells[10].Text = dt.Rows[i]["Cause"].ToString();
                //    gvResult.Rows[i].Cells[11].Text = dt.Rows[i]["Remark"].ToString();
                //    gvResult.Rows[i].Cells[12].Text = dt.Rows[i]["Cdt"].ToString();
                //}
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

    protected DataTable GetQuery() 
    {
        string Connection = CmbDBType.ddlGetConnection();

        DataTable dt = new DataTable();
        List<string> PdLine = new List<string>();
        List<string> Station = new List<string>();
        List<string> Cause = new List<string>();
        List<string> Defect = new List<string>();

        foreach (ListItem item in lboxPdLine.Items)
        {
            if (item.Selected)
            {
                PdLine.Add(item.Value);
            }

        }

        foreach (ListItem item in lboxStaion.Items)
        {
            if (item.Selected)
            {
                Station.Add(item.Value);
            }

        }

        foreach (ListItem item in lboxCause.Items)
        {
            if (item.Selected)
            {
                Cause.Add(item.Value);
            }

        }

        foreach (ListItem item in lboxDefect.Items)
        {
            if (item.Selected)
            {
                Defect.Add(item.Value);
            }

        }
        //string prdType = defaultSelectDB.ToUpper().Equals("HPDOCKING") ? "" : ChxLstProductType1.GetCheckList();
        string prdType = iConfigDB.CheckDockingDB(defaultSelectDB) ? "" : ChxLstProductType1.GetCheckList();
        if (isiqdc)
        {
            dt = PIADefectList.GetQueryResult_IQDC(Connection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                       Station, Defect, Cause, PdLine, prdType);
        }
        else
        {
            dt = PIADefectList.GetQueryResult(Connection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                       Station, Defect, Cause, PdLine, prdType);
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
    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void GetPdline()
    {
        DataTable dtPdLine = QueryCommon.GetLine(GetProcess(), customer, false, DBConnection);
        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
    protected void GetStation()
    {
        List<string> type = new List<string>();
        type.Add("FATest");
        DataTable dtStation = QueryCommon.GetStation(type,DBConnection);
        if (dtStation.Rows.Count > 0)
        {
            foreach (DataRow dr in dtStation.Rows)
            {
                lboxStaion.Items.Add(new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim()));
            }
        }
    }

    private List<string> GetProcess()
    {
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        return Process;
    }
    private void GetDefect()
    { 
        List<string> defecttype= new List<string>();
        defecttype.Add("PRD");
        DataTable dt = QueryCommon.GetDefect(defecttype,DBConnection);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                lboxDefect.Items.Add(new ListItem(dr["Defect"].ToString().Trim() + " - " + dr["Descr"].ToString().Trim(), dr["Defect"].ToString().Trim()));
            }
        }
    }
    private void GetCause()
    {
        List<string> infotype = new List<string>();
        infotype.Add("FACause");
        DataTable dt = QueryCommon.GetDefectInfo(infotype, customer,DBConnection);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                lboxCause.Items.Add(new ListItem(dr["Code"].ToString().Trim() + " - " + dr["Description"].ToString().Trim(), dr["Code"].ToString().Trim()));
            }
        }
    }
}
