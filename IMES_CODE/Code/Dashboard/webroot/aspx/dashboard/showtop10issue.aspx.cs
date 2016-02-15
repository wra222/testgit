using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.inventec.portal.dashboard.common;
using System.Web.UI.DataVisualization.Charting;
using System.Linq;
using System.Collections.Generic;
using com.inventec.portal.dashboard.Fa;


public partial class webroot_aspx_dashboard_top10: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String stageType = HttpContext.Current.Request.QueryString["stageType"];
        this.Chart1.Click += new ImageMapEventHandler(Chart1_Click);
        IList<string> stage = new List<string>();
        if (stageType == "1")//FA
        {
            stage.Add("FA");
            stage.Add("PAK");
        }
        if (stageType == "2")
        {
            stage.Add("SA");
        }
        if (stageType == "3")
        {
            stage.Add("SMT");
        }
        BindChartAllStatge_X(stage);
        foreach (Series series in this.Chart1.Series)
        {
            series.PostBackValue = series.Name + ",#INDEX";
        }
        if (!this.IsPostBack)
        {
            Bindstage(stage);
        }
       
        

    }
    private DataTable GetDashboard_Defect()
    {
        DataTable dt = DashboardCommon.GetTop10Defect();
        dt.DefaultView.Sort = "DefectQty";
        return dt;
    }
    private void BindChartAllStatge(IList<string> stagelist)
    {
        DataTable dt = default(DataTable);
        dt = GetDashboard_Defect();  
        Chart1.Series.Clear(); //清除图表集合
        Chart1.ChartAreas.Clear();//清除绘图区域
        Chart1.Titles.Clear();//清除标题
        Chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

        Chart1.Legends.Clear();//图表使用的图例名称
        Chart1.Legends.Add("Gaol");

        ChartArea ca = Chart1.ChartAreas.Add("Main");//绘图区域名称
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
        string cause;
        float maxDefect = 1;
        ca.AxisX.LineWidth = 0;
        DataRow[] drArr;
        foreach (string stage in stagelist)
        {
            drArr = dt.Select("Stage='" + stage + "'"+" and Line='ALL'", " DefectQty");
          
            if (drArr.Length > 0)
            {
                string line = "ALL";
                Series serDefect = Chart1.Series.Add(stage);
                serDefect.ChartType = SeriesChartType.Column;
                //   serDefect.ChartType = SeriesChartType.Line;

                serDefect.ChartArea = "Main";
                //  serDefect.ToolTip = "xx12345";
                serDefect.LegendText = stage + " Defect";
                // serDefect.PostBackValue = "#INDEX";
                Series serFPFRate = Chart1.Series.Add(stage + "FPFRate");
                serFPFRate.ChartType = SeriesChartType.Line;
                serFPFRate.BorderWidth = 3;
                serFPFRate.LabelFormat = "0%";
                serFPFRate.YAxisType = AxisType.Secondary;
                serFPFRate.IsValueShownAsLabel = false;
                serFPFRate.ChartArea = "Main";
                serFPFRate.LegendText = stage + " FPF Rate";

                int i = 0;
                foreach (DataRow dr in drArr)
                {
                    cause = dr["Cause"].ToString().Trim();
                    station = dr["Station"].ToString().Trim();
                    DefectQty = int.Parse(dr["DefectQty"].ToString());
                    InputQty = float.Parse(dr["InputQty"].ToString());
                    if (DefectQty > 0)
                    {
                        FPF = (DefectQty / InputQty);
                    }
                    else
                    { FPF = 0; }
                    serFPFRate.Points.AddXY(cause, FPF);

                    if (FPF > max) { max = FPF; }
                    if (DefectQty > maxDefect) { maxDefect = DefectQty; }
                    serDefect.Points.AddXY( cause, DefectQty);
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

            foreach (Legend L in Chart1.Legends)
            {
                L.Docking = Docking.Top;

            }
        }
    private void BindGrid()
    {
        this.gvResult.DataSource = GetDashboard_Defect();
        this.gvResult.DataBind();
        InitGridView();
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Line"] = "";
            newRow["Station"] = "";
            newRow["Cause"] = "";
            newRow["DefectQty"] = "";
            newRow["InputQty"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Cause", Type.GetType("System.String"));
        retTable.Columns.Add("DefectQty", Type.GetType("System.String"));
        retTable.Columns.Add("InputQty", Type.GetType("System.String"));
        return retTable;
    }
    private void InitGridView()
    {
        int i = 10;

        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(i);

    }
    protected void Chart1_Click(object sender, ImageMapEventArgs e)
    {
        string[] input = e.PostBackValue.Split(',');
        BindGrid();
        
    }
    private void Bindstage(IList<string> stage)
    {
        stagelist.Items.Clear();
        stagelist.Items.Add("");
        foreach (string st in stage)
        {
            stagelist.Items.Add(st);
        }
      //  stagelist.Items.Add(stage);
        // DataTable dt = DashboardCommon.GetAllStage();
        //if (dt.Rows.Count>0)
        //{
        //  for(int i=0;i<dt.Rows.Count;i++)
        //  {
        //      stagelist.Items.Add(dt.Rows[i][0].ToString());
        //  }
        //}
        stagelist.SelectedIndex = 0;
       
    }
    protected void stagelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        inpdline();
    }
    protected void pdlinelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        instation();
    }
    private void inpdline()
    {
        pdlinelist.Items.Clear();
        pdlinelist.Items.Add("");
        string stage = stagelist.SelectedItem.Text;
        DataTable dt = com.inventec.portal.dashboard.Fa.DashboardManager.GetLineListByStage(stage);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pdlinelist.Items.Add(dt.Rows[i][0].ToString());

            }
            pdlinelist.SelectedIndex = 0;
        }
    }
    private void instation()
    {
        stationlist.Items.Clear();
        stationlist.Items.Add("");
        string line = pdlinelist.SelectedItem.Text;
        DataTable dt = com.inventec.portal.dashboard.Fa.DashboardManager.GetStationListByLine(line);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // stationlist.Items.Add(new ListItem(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString()));
                stationlist.Items.Add(dt.Rows[i][0].ToString());
            }
        }
    }
   
    private void BindChart(string line, string stationlist)
    {
        DataTable dt = default(DataTable);
        dt = GetDashboard_Defect();
        Chart1.Series.Clear(); //清除图表集合
        Chart1.ChartAreas.Clear();//清除绘图区域
        Chart1.Titles.Clear();//清除标题
        Chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

        Chart1.Legends.Clear();//图表使用的图例名称
        Chart1.Legends.Add("Gaol");

        ChartArea ca = Chart1.ChartAreas.Add("Main");//绘图区域名称
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
        string cause;
        float maxDefect = 1;
        ca.AxisX.LineWidth = 0;
        DataRow[] drArr;
        drArr = dt.Select();
      
            drArr = dt.Select("Line='" + line + "'" +" and Station='" +stationlist+"'", " DefectQty");
            if (drArr.Length > 0)
            {
                Series serDefect = Chart1.Series.Add(line);
                serDefect.ChartType = SeriesChartType.Column;
                //   serDefect.ChartType = SeriesChartType.Line;

                serDefect.ChartArea = "Main";
                //  serDefect.ToolTip = "xx12345";
                serDefect.LegendText = "Line:"+line + " Station:"+stationlist + " Defect";
                // serDefect.PostBackValue = "#INDEX";
                Series serFPFRate = Chart1.Series.Add(line + "FPFRate");
                serFPFRate.ChartType = SeriesChartType.Line;
                serFPFRate.BorderWidth = 3;
                serFPFRate.LabelFormat = "0%";
                serFPFRate.YAxisType = AxisType.Secondary;
                serFPFRate.IsValueShownAsLabel = false;
                serFPFRate.ChartArea = "Main";
                serFPFRate.LegendText = "Line:"+line + " Station:"+stationlist + " FPF Rate";

                int i = 0;
                foreach (DataRow dr in drArr)
                {

                    cause = dr["Cause"].ToString();
                    DefectQty = int.Parse(dr["DefectQty"].ToString());
                    InputQty = float.Parse(dr["InputQty"].ToString());
                    if (DefectQty > 0)
                    {
                        FPF = (DefectQty / InputQty);
                    }
                    else
                    { FPF = 0; }
                    serFPFRate.Points.AddXY(cause, FPF);

                    if (FPF > max) { max = FPF; }
                    if (DefectQty > maxDefect) { maxDefect = DefectQty; }
                    serDefect.Points.AddXY(cause, DefectQty);
                    if (DefectQty == 0)
                    { serDefect.Points[i].IsValueShownAsLabel = false; }
                    else
                    { serDefect.Points[i].IsValueShownAsLabel = true; }

                    i++;
                }
           
            ca.AxisX.Interval = 1;
            int t = (int)Math.Ceiling(max * 100);
            double d = Math.Ceiling((double)t / 5);
            d = d * 0.05;

            ca.AxisY2.Maximum = d;

            double d3 = Math.Ceiling(maxDefect / 5) * 5;

            ca.AxisY.Interval = d3 / 5;
            ca.AxisY.Maximum = d3;

            foreach (Legend L in Chart1.Legends)
            {
                L.Docking = Docking.Top;

            }
        }
    }
    protected void stationlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.query.Enabled = true;
    }
    protected void query_Click(object sender, EventArgs e)
    {
        string line = pdlinelist.SelectedItem.Value;
        string station = stationlist.SelectedItem.Value;
        if (line != "" && station != "")
        {
            BindChart(line, station);
        }
    }

    private void BindChartAllStatge_X(IList<string> stagelist)
    {
        DataTable dt = default(DataTable);
        dt = GetDashboard_Defect();
        Chart1.Series.Clear(); //清除图表集合
        Chart1.ChartAreas.Clear();//清除绘图区域
        Chart1.Titles.Clear();//清除标题
        Chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

        Chart1.Legends.Clear();//图表使用的图例名称
        Chart1.Legends.Add("Gaol");


        foreach (string stage in stagelist)
        {
           
            ChartArea ca = Chart1.ChartAreas.Add(stage + "Main");//绘图区域名称
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
            string cause;
            float maxDefect = 1;
            ca.AxisX.LineWidth = 0;
            DataRow[] drArr;
            drArr = dt.Select("Stage='" + stage + "'" + " and Line='ALL'", " DefectQty");

            if (drArr.Length > 0)
            {
                string line = "ALL";
                Series serDefect = Chart1.Series.Add(stage);
                serDefect.ChartType = SeriesChartType.Column;
                //   serDefect.ChartType = SeriesChartType.Line;

                serDefect.ChartArea = stage + "Main";// 绘图区域
                //  serDefect.ToolTip = "xx12345";
                serDefect.LegendText = stage + " Defect";
                // serDefect.PostBackValue = "#INDEX";
                Series serFPFRate = Chart1.Series.Add(stage + "FPFRate");
                serFPFRate.ChartType = SeriesChartType.Line;
                serFPFRate.BorderWidth = 3;
                serFPFRate.LabelFormat = "0%";
                serFPFRate.YAxisType = AxisType.Secondary;
                serFPFRate.IsValueShownAsLabel = false;
                serFPFRate.ChartArea = stage + "Main";// 绘图区域
                serFPFRate.LegendText = stage + " FPF Rate";

                int i = 0;
                foreach (DataRow dr in drArr)
                {
                    cause = dr["Cause"].ToString().Trim();
                    station = dr["Station"].ToString().Trim();
                    DefectQty = int.Parse(dr["DefectQty"].ToString());
                    InputQty = float.Parse(dr["InputQty"].ToString());
                    if (DefectQty > 0)
                    {
                        FPF = (DefectQty / InputQty);
                    }
                    else
                    { FPF = 0; }
                    serFPFRate.Points.AddXY(station+":"+cause, FPF);

                    if (FPF > max) { max = FPF; }
                    if (DefectQty > maxDefect) { maxDefect = DefectQty; }
                    serDefect.Points.AddXY(station+":"+cause, DefectQty);
                    if (DefectQty == 0)
                    { serDefect.Points[i].IsValueShownAsLabel = false; }
                    else
                    { serDefect.Points[i].IsValueShownAsLabel = true; }

                    i++;
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

            foreach (Legend L in Chart1.Legends)
            {
                L.Docking = Docking.Top;

            }
        }
    }
}


