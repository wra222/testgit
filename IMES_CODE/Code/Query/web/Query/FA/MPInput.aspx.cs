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

public partial class Query_FA_MPInput : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);    
    IFA_MPInput MPInput = ServiceAgent.getInstance().GetObjectByName<IFA_MPInput>(WebConstant.MPInput);
    IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    //public string PAKOnlineStation = WebCommonMethod.getConfiguration("PAKOnlineStation");
    //public string FAOnlineStation = WebCommonMethod.getConfiguration("FAOnlineStation");
    public string StationList = "";
    public string defaultSelectDB = "";

    string customer = "";
    public int FixedColCount = 3;
    public string plusColumnName = "MVS,ITCND,COA";
    String DBConnection = "";

    public string[] GvQueryColumnName = { "No", "ProductID", "Model", "Station", "Status", "Line", "Editor", "Udt" };
    public int[] GvQueryColumnNameWidth = { 50, 80, 80, 150, 80, 80, 120, 150 };
    
    protected void Page_Load(object sender, EventArgs e)
    {
        customer = Master.userInfo.Customer;
        DBConnection = CmbDBType.ddlGetConnection();
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        defaultSelectDB = this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
        if (iConfigDB.CheckDockingDB(defaultSelectDB))
        {
            lblModelCategory.Visible = false;
            ChxLstProductType1.IsHide = true;
        }
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
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
        this.lblProcess.Text = this.GetLocalResourceObject(Pre + "_lblProcess").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();        
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
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

        hfMVS_SUM.Value = MVS_SUM;
        hfITCND_SUM.Value = ITCND_SUM;
        hfCOA_SUM.Value = COA_SUM;

        hfProcess_All.Value = FAOnlineStation + "," + PAKOnlineStation;
        hfProcess_FA.Value = FAOnlineStation;
        hfProcess_PAK.Value = PAKOnlineStation;

        //this.gvResult.DataSource = getNullDataTable(1);
        //this.gvResult.DataBind();
        //InitGridView();        
    }
    private void InitGridViewHeader() {
        DataTable dtStation = Station.GetStation(DBConnection);
        for (int r = FixedColCount; r <= StationList.Split(',').Length + (FixedColCount - 1); r++)
        {
            ToolUtility tool = new ToolUtility();
            string descr = tool.GetStationDescr(dtStation, gvResult.HeaderRow.Cells[r].Text.Trim());
            string tip = tool.GetTipString(descr);
            gvResult.HeaderRow.Cells[r].Attributes.Add("onmouseover",tip);
            gvResult.HeaderRow.Cells[r].Attributes.Add("onmouseout", "UnTip()");
        
        }
    }

    private void InitGridView()
    {
        int i = 100;
        int j = 50;        
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(i);

        for (int r = FixedColCount; r <= StationList.Split(',').Length + (FixedColCount - 1); r++)
        {
            gvResult.HeaderRow.Cells[r].Width = Unit.Pixel(70);
        }        
    }
    private void InitDetailGridView()
    {
        gvStationDetail.HeaderRow.Cells[0].Width = Unit.Pixel(40);
        gvStationDetail.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        gvStationDetail.HeaderRow.Cells[2].Width = Unit.Pixel(100);
        gvStationDetail.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        gvStationDetail.HeaderRow.Cells[4].Width = Unit.Pixel(100);
        gvStationDetail.HeaderRow.Cells[5].Width = Unit.Pixel(120);
        gvStationDetail.HeaderRow.Cells[6].Width = Unit.Pixel(80);
        gvStationDetail.HeaderRow.Cells[7].Width = Unit.Pixel(60);
        gvStationDetail.HeaderRow.Cells[8].Width = Unit.Pixel(60);
        gvStationDetail.HeaderRow.Cells[9].Width = Unit.Pixel(200);
        gvStationDetail.HeaderRow.Cells[10].Width = Unit.Pixel(50);
        gvStationDetail.HeaderRow.Cells[11].Width = Unit.Pixel(160);
        gvStationDetail.HeaderRow.Cells[12].Width = Unit.Pixel(160);
        gvStationDetail.HeaderRow.Cells[13].Width = Unit.Pixel(100);
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();                                    
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Family", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        foreach (string STN in StationList.Split(','))
        {
            retTable.Columns.Add(STN, Type.GetType("System.String"));            
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
        GetProcess();
        try
        {

            DateTime ToDate = DateTime.Now;
            hfFromDate.Value = DateTime.Parse(txtFromDate.Text.ToString()).ToString();
            hfToDate.Value = DateTime.Parse(txtToDate.Text.ToString()).ToString();

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

            string prdType;

            prdType = defaultSelectDB.Equals("Docking") ? "" : ChxLstProductType1.GetCheckList();
         

          
         
        

            DataTable dt = MPInput.GetQueryResult(DBConnection, DateTime.Parse(hfFromDate.Value), DateTime.Parse(hfToDate.Value),
                            lstPdLine, ddlFamily.SelectedValue, Model, StationList, bool.Parse(hfLineShife.Value), ddlStation.SelectedValue, false, prdType);

            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();

                int[] sum = new int[dt.Columns.Count];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["Line"].ToString();
                    gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Family"].ToString();
                    gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Model"].ToString();

                    string r_Family = "";
                    string r_Line = "";
                    string r_Model = "";

                    r_Line = gvResult.Rows[i].Cells[0].Text.Trim();
                    r_Family = gvResult.Rows[i].Cells[1].Text.Trim();
                    r_Model = gvResult.Rows[i].Cells[2].Text.Trim();



                    int MVS = 0 ;
                    int ITCND = 0 ;
                    int COA = 0 ;
                    
                    for (int j = FixedColCount; j <= dt.Columns.Count - 1; j++)
                    {
                        string r_Station = "";
                        r_Station = dt.Columns[j].ColumnName.ToString();

                       gvResult.Rows[i].Cells[j].Text = string.IsNullOrEmpty(dt.Rows[i][j].ToString()) ? "0" : dt.Rows[i][j].ToString();
                        if (gvResult.Rows[i].Cells[j].Text != "0")
                        {
                            gvResult.Rows[i].Cells[j].CssClass = "querycell";
                            gvResult.Rows[i].Cells[j].Attributes.Add("onclick", "SelectDetail('" + r_Station + "','" + r_Line + "','" + r_Model + "','" + r_Family + "','" + hfLineShife.Value + "')");

                            if (Array.IndexOf(MVS_SUM.Split(','), r_Station) > -1) {
                                MVS += int.Parse(gvResult.Rows[i].Cells[j].Text);                                
                            }
                            if (Array.IndexOf(ITCND_SUM.Split(','), r_Station) > -1)
                            {
                                ITCND += int.Parse(gvResult.Rows[i].Cells[j].Text);
                            }
                            if (Array.IndexOf(COA_SUM.Split(','), r_Station) > -1)
                            {
                                COA += int.Parse(gvResult.Rows[i].Cells[j].Text);
                            }
                            sum[j] += int.Parse(gvResult.Rows[i].Cells[j].Text);
                        }
                    }

                    gvResult.Rows[i].Cells[3].Text = (-1 * MVS).ToString();                    
                    gvResult.Rows[i].Cells[3].CssClass = MVS > 0 ? "querycell nopointer" : "";
                    sum[3] += (-1 *MVS);

                    gvResult.Rows[i].Cells[4].Text = (-1 * ITCND).ToString();                    
                    gvResult.Rows[i].Cells[4].CssClass = ITCND > 0 ? "querycell nopointer" : "";
                    sum[4] += (-1 *ITCND);

                    gvResult.Rows[i].Cells[5].Text = (-1 * COA).ToString();
                    gvResult.Rows[i].Cells[5].CssClass = COA > 0 ? "querycell nopointer" : "";
                    sum[5] += (-1 *COA);
                }

                //加入Header
                for (int j = FixedColCount; j <= dt.Columns.Count - 1; j++)
                {
                    gvResult.HeaderRow.Cells[j].Attributes.Add("onclick", "SelectDetail('" + gvResult.HeaderRow.Cells[j].Text + "','All','All','All','true')");
                }

                //加入Footer
                if (dt.Rows.Count > 0)
                {
                    gvResult.FooterStyle.Font.Bold = true;
                    gvResult.FooterRow.Cells[0].Text = "";
                    gvResult.FooterRow.Cells[1].Text = "";
                    gvResult.FooterRow.Cells[2].Text = "TOTAL";


                    for (int j = FixedColCount; j <= dt.Columns.Count - 1; j++)
                    {
                        gvResult.FooterRow.Cells[j].Text = sum[j].ToString();
                        gvResult.FooterRow.Cells[j].Font.Bold = true;
                        gvResult.FooterRow.Cells[j].Font.Size = FontUnit.Parse("16px");
                        if (sum[j] > 0)
                        {
                            gvResult.HeaderRow.Cells[j].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='blue';this.className='rowclient';  ");
                            gvResult.HeaderRow.Cells[j].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                            gvResult.HeaderRow.Cells[j].Attributes.Add("onclick", "SelectDetail('" + gvResult.HeaderRow.Cells[j].Text + "','All','All','All','true')");
                        }
                    }
                    gvResult.FooterRow.Visible = true;
                    gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;
                }


                InitGridViewHeader();
                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID);
                //gvResult.GvExtHeight = "370px";
                gvStationDetail.Visible = false;
                gvStationDetail.DataSource = null;
                gvStationDetail.DataBind();

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
    public void btnQueryDetail_Click(object sender, System.EventArgs e)
    {
        try
        {
            string Station = hfStation.Value;
            string Line = hfLine.Value;
            string Model = hfModel.Value;
            List<string> lstModel = new List<string>(Model.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            string Family = hfFamily.Value;
            IList<string> lstPdLine = new List<string>();

            DataTable dt = MPInput.GetSelectDetail(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text), lstPdLine, Family, lstModel, Line, Station, bool.Parse(hfLineShife.Value), ddlStation.SelectedValue);
            gvStationDetail.DataSource = dt;
            gvStationDetail.DataBind();
            gvStationDetail.Visible = true;
            if (dt.Rows.Count > 0) {
                gvStationDetail.HeaderRow.Cells[0].Width = 30;
                gvStationDetail.HeaderRow.Cells[7].Width = 50;
                gvStationDetail.HeaderRow.Cells[8].Width = 120;
                gvStationDetail.HeaderRow.Cells[9].Width = 30;
                EnableBtnExcel(this, true, btnExport.ClientID);
                EnableBtnExcel2(this, true, btnDetailExport.ClientID);
                //gvResult.GvExtHeight = "200px";
                gvStationDetail.Visible = true;
                InitDetailGridView();
            }
            else { }
            
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
        tu.ExportExcel(gvResult,Page.Title,Page);
    }

    protected void btnDetailExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvStationDetail, Page.Title, Page);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        
    }
 
    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        hidModelList.Value = "";        
    }
     
    private List<string> GetProcess()
    {
        List<string> Process = new List<string>();
        if (rbProcess_All.Checked)
        {
            Process.Add("FA");
            Process.Add("PAK");
            StationList = plusColumnName + "," + FAOnlineStation + "," + PAKOnlineStation;            
        }
        else if (rbProcess_FA.Checked)
        {
            Process.Add("FA");
            StationList = FAOnlineStation;
        
        }
        else if (rbProcess_PAK.Checked)
        {
            Process.Add("PAK");
            StationList = PAKOnlineStation;        
        }
        return Process;
    }

    protected void btnChangeLine_Click(object sender, EventArgs e)
    {
        if (bool.Parse(hfLineShife.Value))
        {
            hfLineShife.Value = "false";
        }
        else {
            hfLineShife.Value = "true";
        }
        GetPdline();    
    }

    protected void GetPdline() {

        List<string> Process = GetProcess();

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

    protected void  Process_CheckedChanged(object sender, EventArgs e)
    {
        GetPdline();
    }

    protected void GetStation()
    {
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
    protected void ddlStation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
