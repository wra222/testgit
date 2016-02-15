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
using System.Web.UI.DataVisualization.Charting;

public partial class Query_FA_ProductYield : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);    
    IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);
    IFA_ProductYield ProductYield = ServiceAgent.getInstance().GetObjectByName<IFA_ProductYield>(WebConstant.ProductYield);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    public string defaultSelectDB = "";
    
    string DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        DBConnection = CmbDBType.ddlGetConnection();
        defaultSelectDB = this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
        //lblModelCategory.Visible = !defaultSelectDB.ToUpper().Equals("HPDOCKING");
        //ChxLstProductType1.IsHide = defaultSelectDB.ToUpper().Equals("HPDOCKING");
        lblModelCategory.Visible = !iConfigDB.CheckDockingDB(defaultSelectDB);
        ChxLstProductType1.IsHide = iConfigDB.CheckDockingDB(defaultSelectDB);
        try
        {            
            if (!this.IsPostBack)
            {
                List<string> lst = new List<string> { "YieldStation" };
                DataTable dt = GetSysSetting(lst, DBConnection);
                YieldStation = dt.Select(string.Format("name = '{0}'", "YieldStation"))[0]["Value"].ToString();
                

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
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }

    private void InitCondition()
    {
        List<string> station = new List<string>();
        station.AddRange(YieldStation.Split(','));
        
        DataTable dtStation = QueryCommon.GetStationName(station,DBConnection);
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
        DataTable dtPdLine = PdLine.GetPdLine(customer, Process,DBConnection);
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
        
        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd 00:00");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
       
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
        int k = 150;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(30);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[8].Width = Unit.Pixel(j);
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
            newRow["Station"] = "";
            newRow["InputQty"] = "";
            newRow["DefectQty"] = "";
            newRow["FPF Rate"] = "";
            newRow["FPY Rate"] = "";

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
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("InputQty", Type.GetType("System.String"));
        retTable.Columns.Add("DefectQty", Type.GetType("System.String"));
        retTable.Columns.Add("FPF Rate", Type.GetType("System.String"));
        retTable.Columns.Add("FPY Rate", Type.GetType("System.String"));

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
            List<string> station = new List<string>();
            station.AddRange(YieldStation.Split(','));
            

            IList<string> lstPdLine = new List<string>();
            foreach (ListItem item in lboxPdLine.Items)
            {
                if (item.Selected)
                {
                    lstPdLine.Add(item.Value);
                }
            }
           //string prdType = defaultSelectDB.ToUpper().Equals("HPDOCKING") ? "" : ChxLstProductType1.GetCheckList();
            string prdType = iConfigDB.CheckDockingDB(defaultSelectDB) ? "" : ChxLstProductType1.GetCheckList();
       
            DataTable dt = ProductYield.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                            ddlStation.SelectedValue, ddlFamily.SelectedValue, lstPdLine, txtModel.Text.Trim(), station, prdType);
            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();
                float TotalInputQty = 0;
                float TotalDefectQty = 0;
                float TotalFPF = 0;

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResult.Rows[i].Cells[0].Text = (i + 1).ToString();
                    gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Family"].ToString();
                    gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Model"].ToString();
                    gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Line"].ToString();
                    gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["Station"].ToString();
                    gvResult.Rows[i].Cells[5].Text = dt.Rows[i]["InputQty"].ToString();
                    gvResult.Rows[i].Cells[6].Text = dt.Rows[i]["DefectQty"].ToString();
                    float InputQty = float.Parse(gvResult.Rows[i].Cells[5].Text);
                    float DefectQty = float.Parse(gvResult.Rows[i].Cells[6].Text);
                    float FPF = 0;
                    if (DefectQty > 0)
                    {
                        FPF = (DefectQty / InputQty);
                    }
                    gvResult.Rows[i].Cells[7].Text = Math.Round(FPF, 5).ToString("0.00%");
                    gvResult.Rows[i].Cells[8].Text = (1 - Math.Round(FPF, 5)).ToString("0.00%");
                    TotalInputQty += InputQty;
                    TotalDefectQty += DefectQty;
                }

                gvResult.FooterStyle.Font.Bold = true;
                gvResult.FooterRow.Cells[5].Text = TotalInputQty.ToString();
                gvResult.FooterRow.Cells[6].Text = TotalDefectQty.ToString();
                if (TotalDefectQty > 0)
                {
                    TotalFPF = (TotalDefectQty / TotalInputQty);
                }
                gvResult.FooterRow.Cells[7].Text = Math.Round(TotalFPF, 5).ToString("0.00%");
                gvResult.FooterRow.Cells[8].Text = (1 - Math.Round(TotalFPF, 5)).ToString("0.00%");


                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID);
                chart1.Visible = true;
                chart1.ImageStorageMode = ImageStorageMode.UseImageLocation;
                BindChart(dt, lstPdLine);


            }
            else
            {
                chart1.Visible = false;
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
            endWaitingCoverDiv();
        }
      
    }
    private void BindChart(DataTable dt, IList<string> lstPdLine)
    {
        

        chart1.Series.Clear();
        chart1.ChartAreas.Clear();
        chart1.Titles.Clear();
        chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

        chart1.Legends.Clear();
        chart1.Legends.Add("Gaol");
        ChartArea ca = chart1.ChartAreas.Add("Main");
        LabelStyle ls = new LabelStyle();
        ls.Format = "0%";
        ca.AxisY2.Minimum = 0;
        ca.AxisY2.LabelStyle = ls;
        ca.AxisY.Title = "Defect Qty";
        System.Drawing.Font titleFont = new System.Drawing.Font("Times New Roman", 12);
        ca.AxisY.TitleFont = titleFont;
        ca.AxisY.TextOrientation = TextOrientation.Horizontal;
        ca.AxisY2.Title = "FPF Rate";
        ca.AxisY2.TitleFont = titleFont;
        ca.AxisY2.TextOrientation = TextOrientation.Horizontal;
        ca.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
         ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
        ca.AxisY.Minimum = 0;
        ca.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;

        float InputQty;
        float DefectQty;
        float FPF = 0;
        float max = 0.1F;
        string station;
        float maxDefect = 1;
        ca.AxisX.LineWidth = 0;
    
        DataRow[] drArr;

        if (lstPdLine.Count == 0)
        {
            lstPdLine.Add("ALL");
        }

        foreach (string line in lstPdLine)
        {
            drArr = dt.Select("Line='" + line + "'"," Station");
            if (drArr.Length > 0)
            {
                var serDefect = chart1.Series.Add(line);
                serDefect.ChartType = SeriesChartType.Column;
             //   serDefect.ChartType = SeriesChartType.Line;
             
                serDefect.ChartArea = "Main";
              //  serDefect.ToolTip = "xx12345";
                serDefect.LegendText = line + " Defect";
               
                var serFPFRate = chart1.Series.Add(line + "FPFRate");
                serFPFRate.ChartType = SeriesChartType.Line;
                serFPFRate.BorderWidth = 3;
                serFPFRate.LabelFormat = "0%";
                serFPFRate.YAxisType = AxisType.Secondary;
                serFPFRate.IsValueShownAsLabel = false;
                serFPFRate.ChartArea = "Main";
                serFPFRate.LegendText = line +" FPF Rate";

                int i = 0;
                foreach (DataRow dr in drArr)
                {
                    
                    station = dr["Station"].ToString().Substring(0, 2);
                    DefectQty = int.Parse(dr["DefectQty"].ToString());
                    InputQty = float.Parse(dr["InputQty"].ToString());
                    if (DefectQty > 0)
                    {
                        FPF = (DefectQty / InputQty);
                    }
                    else
                    { FPF = 0; }
                  serFPFRate.Points.AddXY(station, FPF);
                    if (FPF > max) { max = FPF; }
                    if (DefectQty > maxDefect) { maxDefect = DefectQty; }
                     serDefect.Points.AddXY(station, DefectQty);
                    if (DefectQty == 0)
                    { serDefect.Points[i].IsValueShownAsLabel = false; }
                    else
                    { serDefect.Points[i].IsValueShownAsLabel = true; }

                    i++;
                }
            }
            
        
        }

     ca.AxisX.Interval = 1;
      int t = (int)Math.Ceiling(max * 100);
      double d = Math.Ceiling((double)t / 5);
      d = d * 0.05;

      ca.AxisY2.Maximum = d;

      double d3 = Math.Ceiling(maxDefect / 5) * 5;

      ca.AxisY.Interval = d3 / 5;
      ca.AxisY.Maximum = d3;
 
      foreach (Legend L in chart1.Legends)
      {
          L.Docking = Docking.Top;
    
      }

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
