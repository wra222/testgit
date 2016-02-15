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

    //private DataTable getNullDataTable(DataTable infodt)
    //{
    //    DataTable dt = initTable();
    //    DataRow newRow = null;
    //    float TotalInputQty = 0;
    //    float TotalDefectQty = 0;
    //    float TotalFPF = 0;
    //    for (int i = 0; i < infodt.Rows.Count; i++)
    //    {
    //        newRow = dt.NewRow();
    //        newRow["No"] = (i + 1).ToString();
    //        newRow["Family"] = infodt.Rows[i]["Family"].ToString();
    //        newRow["Model"] = infodt.Rows[i]["Model"].ToString();
    //        newRow["Line"] = infodt.Rows[i]["Line"].ToString();
    //        newRow["Station"] = infodt.Rows[i]["Station"].ToString();
    //        newRow["InputQty"] = infodt.Rows[i]["InputQty"].ToString();
    //        newRow["DefectQty"] = infodt.Rows[i]["DefectQty"].ToString();
    //        float InputQty = float.Parse(infodt.Rows[i]["InputQty"].ToString());
    //        float DefectQty = float.Parse(infodt.Rows[i]["DefectQty"].ToString());
    //        float FPF = 0;
    //        if (DefectQty > 0)
    //        {
    //            FPF = (DefectQty / InputQty);
    //        }
    //        newRow["FPF Rate"]  = Math.Round(FPF, 5).ToString("0.00%");
    //        newRow["FPY Rate"] = (1 - Math.Round(FPF, 5)).ToString("0.00%");
    //        TotalInputQty += InputQty;
    //        TotalDefectQty += DefectQty;
    //        dt.Rows.Add(newRow);
    //    }
    //    newRow = dt.NewRow();
    //    newRow["No"] = "";
    //    newRow["Family"] = "";
    //    newRow["Model"] = "";
    //    newRow["Line"] = "";
    //    newRow["Station"] = "";
    //    newRow["InputQty"] = TotalInputQty.ToString();
    //    newRow["DefectQty"] = TotalDefectQty.ToString();
    //    if (TotalDefectQty > 0)
    //    {
    //        TotalFPF = (TotalDefectQty / TotalInputQty);
    //    }
    //    newRow["FPF Rate"] = Math.Round(TotalFPF, 5).ToString("0.00%");
    //    newRow["FPY Rate"] = (1 - Math.Round(TotalFPF, 5)).ToString("0.00%");
    //    dt.Rows.Add(newRow);
    //    return dt;
    //}

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
            List<string> lstStation = new List<string>();
            for (int i = 0; i < ddlStation.Items.Count; i++)
            {
                if (ddlStation.Items[i].Selected)
                {
                    lstStation.Add(ddlStation.Items[i].Value);
                }
            }
           //string prdType = defaultSelectDB.ToUpper().Equals("HPDOCKING") ? "" : ChxLstProductType1.GetCheckList();
            string prdType = iConfigDB.CheckDockingDB(defaultSelectDB) ? "" : ChxLstProductType1.GetCheckList();
       
            DataTable dt = ProductYield.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text),
                            lstStation, ddlFamily.SelectedValue, lstPdLine, txtModel.Text.Trim(), station, prdType);
            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();
                float TotalInputQty = 0;
                float TotalDefectQty = 0;
                float TotalFPF = 0;
                string r_Family = "";
                string r_Model = "";
                string r_Line = "";
                string r_Station = "";

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResult.Rows[i].Cells[0].Text = (i + 1).ToString();
                    gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Family"].ToString();
                    gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Model"].ToString();
                    gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Line"].ToString();
                    gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["Station"].ToString();

                    r_Family = "";
                    r_Model = ""; 
                    r_Line = "";
                    r_Station = "";
                    r_Family = gvResult.Rows[i].Cells[1].Text.Trim() == "ALL" ? "" : gvResult.Rows[i].Cells[1].Text.Trim();
                    r_Model = gvResult.Rows[i].Cells[2].Text.Trim() == "ALL" ? "" : gvResult.Rows[i].Cells[2].Text.Trim();
                    r_Line = gvResult.Rows[i].Cells[3].Text.Trim() == "ALL" ? "" : gvResult.Rows[i].Cells[3].Text.Trim();
                    r_Station = gvResult.Rows[i].Cells[4].Text.Trim();
                    r_Station = r_Station.Split(' ')[0];

                    for (int j = 5; j < 7; j++)
                    {
                        string typeflag = j == 5 ? "InputQty" : "DefectQty";
                        gvResult.Rows[i].Cells[j].Text = dt.Rows[i][typeflag].ToString();
                        if (gvResult.Rows[i].Cells[j].Text != "0")
                        {
                            gvResult.Rows[i].Cells[j].CssClass = "querycell";
                            gvResult.Rows[i].Cells[j].Attributes.Add("onclick", "SelectDetail('" + typeflag + "','" + r_Family + "','" + r_Model + "','" + r_Line + "','" + r_Station + "','" + prdType + "')");
                            gvResult.Rows[i].Cells[j].BackColor = System.Drawing.Color.Yellow;
                        }
                    }
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

                for (int i = 5; i < 7; i++)
                {
                    if (gvResult.FooterRow.Cells[i].Text != "0")
                    {
                        string typeflag = i == 5 ? "InputQty" : "DefectQty";
 
                        gvResult.FooterRow.Cells[i].CssClass = "querycell";

                        gvResult.FooterRow.Cells[i].Attributes.Add("onclick", "SelectDetail('" + typeflag + "','','','','" + YieldStation + "','" + prdType + "')");
                        gvResult.FooterRow.Cells[i].BackColor = System.Drawing.Color.Yellow;
                    }
                }
                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID);
                this.gvDetailResult.DataSource = null;
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

    protected void btnQueryDetail_Click(object sender, EventArgs e)
    {
        string type = this.hidType.Value;
        string family = this.hidFamily.Value;
        string model = this.hidModel.Value;
        string line = this.hidLine.Value;
        string station = this.hidStation.Value;
        string category = this.hidprdType.Value;

        DataTable dt = new DataTable();

        if (type == "InputQty")
        {
            dt = ProductYield.GetDetailQueryByInput(DBConnection, txtFromDate.Text, txtToDate.Text,family, model,line,station,category);
        }
        else
        {
            dt = ProductYield.GetDetailQueryByDefect(DBConnection, txtFromDate.Text, txtToDate.Text, family, model, line, station, category);
        }
        this.gvDetailResult.DataSource = dt;
        this.gvDetailResult.DataBind();
        if (dt.Rows.Count > 0)
        {
            EnableBtnExcel(this, true, btnExport.ClientID);
            EnableBtnExcel2(this, true, btnExportDetail.ClientID);
            InitDetailGridView();
        }
    }

    private void InitDetailGridView()
    {
        gvDetailResult.HeaderRow.Cells[0].Width = Unit.Pixel(10);
        gvDetailResult.HeaderRow.Cells[1].Width = Unit.Pixel(40);
        gvDetailResult.HeaderRow.Cells[2].Width = Unit.Pixel(40);
        gvDetailResult.HeaderRow.Cells[3].Width = Unit.Pixel(40);
        gvDetailResult.HeaderRow.Cells[4].Width = Unit.Pixel(40);
        gvDetailResult.HeaderRow.Cells[5].Width = Unit.Pixel(50);
        gvDetailResult.HeaderRow.Cells[6].Width = Unit.Pixel(30);
        gvDetailResult.HeaderRow.Cells[7].Width = Unit.Pixel(80);
        gvDetailResult.HeaderRow.Cells[8].Width = Unit.Pixel(20);
        //gvDetailResult.HeaderRow.Cells[9].Width = Unit.Pixel(30);
        gvDetailResult.HeaderRow.Cells[9].Width = Unit.Pixel(80);
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
    protected void btnExportDetail_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvDetailResult, Page.Title, Page);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    //protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //@InputBegDate, @InputEndDate, @Family, @Model, @Line, @Station, @Category
    //    string InputBegDate = "";
    //    string InputEndDate = "";
    //    string Family = "";
    //    string Model = "";
    //    string Line = "";
    //    string Station = "";
    //    string Category = "";

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        Family = e.Row.Cells[1].Text.Trim() == "ALL" ? "" : e.Row.Cells[1].Text.Trim();
    //        Model = e.Row.Cells[2].Text.Trim() == "ALL" ? "" : e.Row.Cells[2].Text.Trim();
    //        Line = e.Row.Cells[3].Text.Trim() == "ALL" ? "" : e.Row.Cells[3].Text.Trim();
    //        //inputPdLine = e.Row.Cells[3].Text.Trim();
            
    //        for (int i =5; i < e.Row.Cells.Count; i++)
    //        {
    //            if (i == 5)
    //            {
    //                if (e.Row.Cells[i].Text == "&nbsp;" || e.Row.Cells[i].Text == "0")
    //                {
    //                }
    //                else
    //                {
    //                    e.Row.Cells[i].CssClass = "querycell";
    //                    e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail()");
    //                    e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
    //                }
    //            }

    //            if (i == 6)
    //            {
    //                if (e.Row.Cells[i].Text == "&nbsp;" || e.Row.Cells[i].Text == "0")
    //                {
    //                }
    //                else
    //                {
    //                    e.Row.Cells[i].CssClass = "querycell";
    //                    e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail()");
    //                    e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
    //                }
    //            }
    //        }
    //    }
    //}
}
