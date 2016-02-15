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
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
using System.Text;
using System.Drawing;

public partial class SMT_TestDataReport : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    ISMT_TestDataReport intfSMT_TestDataReport = ServiceAgent.getInstance().GetObjectByName<ISMT_TestDataReport>(WebConstant.ISMT_TestDataReport);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.Chart2.Click += new ImageMapEventHandler(Chart2_Click);
        foreach (Series series in this.Chart2.Series)
        {
            series.PostBackValue = series.Name+ ",#INDEX";
        }
        foreach (Series series in this.Chart3.Series)
        {
            series.PostBackValue = series.Name + ",#INDEX";
        }
        DBConnection = CmbDBType.ddlGetConnection();
        if (!Page.IsPostBack)
        {
          
            InitStation();
            InitPdLine();
           txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InitFamily();
        }

    }
    public void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    public void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    protected void InitPdLine()
    {
        //CmbPdLine.Stage = "SA"; //PCA = SA
        //CmbPdLine.Customer = Master.userInfo.Customer;
        List<string> Process = new List<string>();
        Process.Add("AOI");

        DataTable dtPdLine = QueryCommon.GetLine(Process,"HP", false, DBConnection);
        DL_Pdline.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                DL_Pdline.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }

    }
    protected void InitStation()
    {

   
        //List<string> lstFamily = new List<string>();
        ////lstFamily.Add(ddlFamily.SelectedValue);

        //DataTable Station = intfPCBStationQuery.GetStation(DBConnection, lstFamily);

        //DL_Station.Items.Clear();
        //for (int i = 0; i < Station.Rows.Count; i++)
        //{
        //    DL_Station.Items.Add(new ListItem(Station.Rows[i]["Station"].ToString() + " - " + Station.Rows[i]["Descr"].ToString(),
        //                                    Station.Rows[i]["Station"].ToString()));

        //}
    }
    protected void InitFamily()
    {
      
        string TableName = "Part";
        DataTable dtFamily = intfFamily.GetPCBFamily(DBConnection, TableName);
        lboxStation.Items.Clear();
        //lboxStation.Items.Add(new ListItem("All", ""));
        if (dtFamily.Rows.Count > 0)
        {
            for (int i = 0; i < dtFamily.Rows.Count; i++)
            {
                lboxStation.Items.Add(new ListItem(dtFamily.Rows[i]["Family"].ToString(), dtFamily.Rows[i]["Family"].ToString()));
            }
        }
         
        #region mark
        /*
        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        IList<string> ListPdline = new List<string>();
        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                ListPdline.Add(DL_Pdline.Items[i].Value);
            }
        }
        IList<string> ListStation = new List<string>();
        for (int i = 0; i < DL_Station.Items.Count; i++)
        {
            if (DL_Station.Items[i].Selected)
            {
                ListStation.Add(DL_Station.Items[i].Value);
            }
        }

        DataTable Result = intfSMT_TestDataReport.GetModel(DBConnection, ListPdline, ListStation, timeStart, timeEnd);
        //DataView ds = new DataView();
        //ds.Table = Result;
        //DG1.DataSource = ds;
        //DG1.DataBind();
        if (Result.Rows.Count > 0)
        {
            for (int i = 0; i < Result.Rows.Count; i++)
            {
                lboxStation.Items.Add(new ListItem(Result.Rows[i]["PCBModel"].ToString(), Result.Rows[i]["PCBModel"].ToString()));
            }
        }
         */
        #endregion

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
    protected void BT_Query_Click(object sender, EventArgs e)
    {
        if (Data_Check())
        {

            //showErrorMessage(DL_Model.Text + DL_Pdline.SelectedValue + DL_Station.SelectedValue + DL_Type.SelectedValue); 
            switch ( DL_Type.SelectedValue)
            {

                    case "Day":
                    DateTime timeStart = DateTime.Parse(txtFromDate.Text);
                    DateTime timeEnd = DateTime.Parse(txtToDate.Text);
                    if (timeEnd.Day - timeStart.Day > 1)
                    {
                        showErrorMessage("選擇的日期太長，只能查詢當天的數據！");
                    }
                    else
                    {
                        InitChart1("Day");
                        InitChart2();
                        InitChart3();
                        InitGrid();
                    }
                    break;
                    case "Week":
                    DateTime timeStart1 = DateTime.Parse(txtFromDate.Text);
                    DateTime timeEnd1 = DateTime.Parse(txtToDate.Text);
                    if (timeEnd1.Day - timeStart1.Day > 7)
                    {
                        showErrorMessage("選擇的日期太長，只能查詢7天的數據！");
                    }
                    else
                    {
                        InitChart1("Week");
                        InitChart2();
                        InitChart3();
                        InitGrid();
                    }
                    break;
                    case "Month":
                    DateTime timeStart2 = DateTime.Parse(txtFromDate.Text);
                    DateTime timeEnd2 = DateTime.Parse(txtToDate.Text);
                    if (timeEnd2.Day - timeStart2.Day > 90)
                    {
                        showErrorMessage("選擇的日期太長，只能查詢90天的數據！");
                    }
                    else
                    {
                        InitChart1("Month");
                        InitChart2();
                        InitChart3();
                        InitGrid();
                    }
                    break;
                    default: break;
            }
               

                 
       
        }
        else
        {
            showErrorMessage("Please Select The Conditions"); 
        }
    }
     public bool Data_Check()
    {
        bool Result=true;

        if (lboxStation.SelectedValue == "")
        {
            Result = false;
        }
        else if (DL_Pdline.SelectedValue == "")
        {
            Result = false;
        }
        else if (DL_Station.SelectedValue == "")
        {
            Result = false;
        }
        else if (DL_Type.SelectedValue == "")
        {
            Result = false;
        }
        else
        {
            Result = true;
        }
            return Result;
    }
    public void InitGrid()
    {
        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        IList <string> ListFamily = new List<string>();
        for (int i = 0; i < lboxStation.Items.Count; i++)
        {
            if (lboxStation.Items[i].Selected)
            {
                ListFamily.Add(lboxStation.Items[i].Value);
            }
        }
        IList<string> ListPdline = new List<string>();
        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                ListPdline.Add(DL_Pdline.Items[i].Value);
            }
        }
        IList<string> ListStation = new List<string>();
        for (int i = 0; i < DL_Station.Items.Count; i++)
        {
            if (DL_Station.Items[i].Selected)
            {
                ListStation.Add(DL_Station.Items[i].Value);
            }
        }

        DataTable Result = intfSMT_TestDataReport.GetTestCount(DBConnection, ListFamily, ListPdline, ListStation, timeStart, timeEnd);
        if(Result.Rows.Count==0)
        {      showErrorMessage("該Family 無測試數據！");
        }
        else
        {
        DataView ds = new DataView();
        ds.Table = Result;
        //DG1.DataSource = ds;
        //DG1.DataBind();
        GridView1.DataSource = ds;
        GridView1.DataBind();
        }
    }
    public void InitChart1(string Type)
    {
        ////////////////////ChartArea1属性设置///////////////////////////

        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        IList<string> ListFamily = new List<string>();
        for (int i = 0; i < lboxStation.Items.Count; i++)
        {
            if (lboxStation.Items[i].Selected)
            {
                ListFamily.Add(lboxStation.Items[i].Value);
            }
        }
        IList<string> ListPdline = new List<string>();
        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                ListPdline.Add(DL_Pdline.Items[i].Value);
            }
        }
        IList<string> ListStation = new List<string>();
        for (int i = 0; i < DL_Station.Items.Count; i++)
        {
            if (DL_Station.Items[i].Selected)
            {
                ListStation.Add(DL_Station.Items[i].Value);
            }
        }

        DataTable Result = intfSMT_TestDataReport.Get_Defect_Rate(DBConnection, ListFamily, ListPdline, ListStation, timeStart, timeEnd, Type);
        DataView ds = new DataView();
        ds.Table = Result;
        

        //设置网格的颜色
        chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
        chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

        //设置坐标轴名称
        //chart1.ChartAreas["ChartArea1"].AxisX.Title = "a";
        chart1.ChartAreas["ChartArea1"].AxisY.Title = "Rate(%)";
        chart1.Legends.Add("");
        //启用3D显示
        chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;


        //////////////////////Series属性设置///////////////////////////

        //设置显示类型-线型
        chart1.Series["Rate"].ChartType = SeriesChartType.Column;

        //设置坐标轴Value显示类型
        chart1.Series["Rate"].XValueType = ChartValueType.String;
      
        //是否显示标签的数值
        chart1.Series["Rate"].IsValueShownAsLabel = true;

        //设置标记图案
        chart1.Series["Rate"].MarkerStyle = MarkerStyle.Circle;

        //设置图案颜色
        chart1.Series["Rate"].Color = Color.Coral;

        //设置图案的宽度
        chart1.Series["Rate"].BorderWidth =1;


        //添加随机数
        //Random rd = new Random();
        //for (int i = 1; i < 20; i++)
        //{
        //    chart1.Series["a"].Points.AddXY(i, rd.Next(100));
        //}
        chart1.DataSource = ds;
        chart1.Series["Rate"].XValueMember = "PCBModel";
        chart1.Series["Rate"].YValueMembers = "Rate";
        chart1.DataBind();
         
      
    }
    public void InitChart3()
    {
        ////////////////////ChartArea1属性设置///////////////////////////

        //设置网格的颜色
        Chart3.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
        Chart3.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

        //设置坐标轴名称
        Chart3.ChartAreas["ChartArea1"].AxisX.Title = "a";
        Chart3.ChartAreas["ChartArea1"].AxisY.Title = "不良數";
        Chart3.Legends.Add("");
        //启用3D显示
        Chart3.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;


        //////////////////////Series属性设置///////////////////////////

        //设置显示类型-线型
        //Chart3.Series["Rate"].ChartType = SeriesChartType.Line;
        Chart3.Series["Cause_Top"].ChartType = SeriesChartType.Column;
        //设置坐标轴Value显示类型
        Chart3.Series["Cause_Top"].XValueType = ChartValueType.String;

        //是否显示标签的数值
        Chart3.Series["Cause_Top"].IsValueShownAsLabel = true;
        //Chart3.Series["Rate"].IsValueShownAsLabel = true;
        //设置标记图案
        Chart3.Series["Cause_Top"].MarkerStyle = MarkerStyle.Circle;
        //Chart3.Series["Rate"].MarkerStyle = MarkerStyle.Circle;
        //设置图案颜色
        Chart3.Series["Cause_Top"].Color = Color.SkyBlue;
        //Chart3.Series["Rate"].Color = Color.Red;
        //设置图案的宽度
        Chart3.Series["Cause_Top"].BorderWidth = 3;
        //Chart3.Series["Rate"].BorderWidth = 3;

        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        IList<string> ListFamily = new List<string>();
        for (int i = 0; i < lboxStation.Items.Count; i++)
        {
            if (lboxStation.Items[i].Selected)
            {
                ListFamily.Add(lboxStation.Items[i].Value);
            }
        }
        IList<string> ListPdline = new List<string>();
        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                ListPdline.Add(DL_Pdline.Items[i].Value);
            }
        }
        IList<string> ListStation = new List<string>();
        for (int i = 0; i < DL_Station.Items.Count; i++)
        {
            if (DL_Station.Items[i].Selected)
            {
                ListStation.Add(DL_Station.Items[i].Value);
            }
        }

        DataTable Result = intfSMT_TestDataReport.Get_Defect_Analysis(DBConnection, ListFamily, ListPdline, ListStation, timeStart, timeEnd);
        DataView ds = new DataView();
        ds.Table = Result;
        Chart3.DataSource = ds;
        Chart3.Series["Cause_Top"].XValueMember = "Cause";
        Chart3.Series["Cause_Top"].YValueMembers = "Count";
        //Chart3.Series["Rate"].XValueMember = "Cause";
        //Chart3.Series["Rate"].YValueMembers = "Rate";
        Chart3.DataBind();
        //添加随机数
        //Random rd = new Random();
        //for (int i = 1; i < 5; i++)
        //{
        //    Chart3.Series["Top"].Points.AddXY(i, rd.Next(100));
        //    Chart3.Series["Rate"].Points.AddXY(i, rd.Next(100));
        //}


    }
    public void InitChart2()
    {
        ////////////////////ChartArea1属性设置///////////////////////////

        //设置网格的颜色
        Chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;

        //设置坐标轴名称
        //Chart2.ChartAreas["ChartArea1"].AxisX.Title = "a";
        Chart2.ChartAreas["ChartArea1"].AxisY.Title = "不良數";
        Chart2.Legends.Add("");
        //启用3D显示
        Chart2.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;


        //////////////////////Series属性设置///////////////////////////

        //设置显示类型-线型

        Chart2.Series["Location_Top"].ChartType = SeriesChartType.Column;
        //设置坐标轴Value显示类型
        Chart2.Series["Location_Top"].XValueType = ChartValueType.String;

        //是否显示标签的数值
        Chart2.Series["Location_Top"].IsValueShownAsLabel = true;
        
        //设置标记图案
        Chart2.Series["Location_Top"].MarkerStyle = MarkerStyle.Circle;
       
        //设置图案颜色
        Chart2.Series["Location_Top"].Color = Color.YellowGreen;
       
        //设置图案的宽度
        Chart2.Series["Location_Top"].BorderWidth = 3;

        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        IList<string> ListFamily = new List<string>();
        for (int i = 0; i < lboxStation.Items.Count; i++)
        {
            if (lboxStation.Items[i].Selected)
            {
                ListFamily.Add(lboxStation.Items[i].Value);
            }
        }
        IList<string> ListPdline = new List<string>();
        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                ListPdline.Add(DL_Pdline.Items[i].Value);
            }
        }
        IList<string> ListStation = new List<string>();
        for (int i = 0; i < DL_Station.Items.Count; i++)
        {
            if (DL_Station.Items[i].Selected)
            {
                ListStation.Add(DL_Station.Items[i].Value);
            }
        }

        DataTable Result = intfSMT_TestDataReport.Get_Defect_Top(DBConnection, ListFamily, ListPdline, ListStation, timeStart, timeEnd);
        DataView ds = new DataView();
        ds.Table = Result;
        Chart2.DataSource = ds;
        Chart2.Series["Location_Top"].XValueMember = "Location";
        Chart2.Series["Location_Top"].YValueMembers = "Count";
        Chart2.DataBind();
        //添加随机数
        //Random rd = new Random();
        //for (int i = 1; i < 5; i++)
        //{
        //    Chart2.Series["Top"].Points.AddXY(i, rd.Next(1000));
          
        //}


    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitChart1(DL_Type.SelectedValue);
    }

    private void InitializeComponent()
    {

    }
    protected void Chart2_Click(object sender, ImageMapEventArgs e)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        beginWaitingCoverDiv();
        switch (DL_Type.SelectedValue)
        {
            case "Day":
                InitChart1("Day");
                break;
            case "Week":
                InitChart1("Week");
                break;
            case "Month":
                InitChart1("Month");
                break;
            default:
                break;
        }
        InitChart2();
        InitChart3();
        string[] input = e.PostBackValue.Split(',');
        Series series=Chart2.Series[input[0]];
        DataPoint datapoint = series.Points[Convert.ToInt16(input[1])];
        //showErrorMessage(datapoint.AxisLabel);
      
         // showErrorMessage("你选中了" + input[0] + "的第 " + (Convert.ToInt16(input[1]) + 1).ToString() + " 点");
        string message = "";
        string Model = "";
        string Pdline = "";
        string Station = "";
        //DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        //DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        message += DBConnection + "~";

        for (int i = 0; i < lboxStation.Items.Count; i++)
        {
            if (lboxStation.Items[i].Selected)
            {
                Model = lboxStation.Items[i].Value;
            }
        }

        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                Pdline = DL_Pdline.Items[i].Value;
            }
        }

        for (int i = 0; i < DL_Station.Items.Count; i++)
        {
            if (DL_Station.Items[i].Selected)
            {
                Station = DL_Station.Items[i].Value;
            }
        }
        message += Model + "~";
        message += Pdline + "~";
        message += Station + "~";
        message += txtFromDate.Text + "~";
        message += txtToDate.Text + "~";
        message += datapoint.AxisLabel + "~";
        message += "Location";
        //showErrorMessage(datapoint.AxisLabel);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine(@"var url= ""DetialShow.aspx?msg=" + message + "\"" + " ;");
        scriptBuilder.AppendLine(@"window.showModalDialog(url," + "\"" + message + "\"" + ",'dialogWidth:850px;dialogHeight:650px;status:no;help:no;menubar:no;toolbar:no;resize:no;');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        endWaitingCoverDiv();
    }

    protected void Chart3_Click(object sender, ImageMapEventArgs e)
    {
        beginWaitingCoverDiv();
        StringBuilder scriptBuilder = new StringBuilder();
        InitChart2();
        InitChart3();
        switch (DL_Type.SelectedValue)
        {
            case "Day":
                InitChart1("Day");
                break;
            case "Week":
                InitChart1("Week");
                break;
            case "Month":
                InitChart1("Month");
                break;
            default:
                break;
        }
        string[] input = e.PostBackValue.Split(',');
        Series series = Chart3.Series[input[0]];
        DataPoint datapoint = series.Points[Convert.ToInt16(input[1])];
        string message =  "";
        string Model="";
        string Pdline = "";
        string Station = "";
        //DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        //DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        message += DBConnection + "~";
         
        for (int i = 0; i < lboxStation.Items.Count; i++)
        {
            if (lboxStation.Items[i].Selected)
            {
                Model=lboxStation.Items[i].Value;
            }
        }
         
        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                Pdline=DL_Pdline.Items[i].Value;
            }
        }
      
        for (int i = 0; i < DL_Station.Items.Count; i++)
        {
            if (DL_Station.Items[i].Selected)
            {
               Station=DL_Station.Items[i].Value;
            }
        }
        message += Model + "~";
        message += Pdline + "~";
        message += Station + "~";
        message += txtFromDate.Text + "~";
        message += txtToDate.Text + "~";
        message += datapoint.AxisLabel + "~";
        message += "DefectCode";
        //showErrorMessage(datapoint.AxisLabel);
         scriptBuilder.AppendLine("<script language='javascript'>");
         scriptBuilder.AppendLine(@"var url= ""DetialShow.aspx?msg=" + message + "\""+" ;");
        scriptBuilder.AppendLine(@"window.showModalDialog(url,"+"\""+message+"\""+",'dialogWidth:850px;dialogHeight:650px;status:no;help:no;menubar:no;toolbar:no;resize:no;');");
        scriptBuilder.AppendLine("</script>");
          ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
          endWaitingCoverDiv();
    }
    protected void DL_Pdline_SelectedIndexChanged(object sender, EventArgs e)
    {
        string TableName = "Part";
          IList<string> ListPdline = new List<string>();
        for (int i = 0; i < DL_Pdline.Items.Count; i++)
        {
            if (DL_Pdline.Items[i].Selected)
            {
                ListPdline.Add(DL_Pdline.Items[i].Value);
            }
        }
         DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        DataTable dtFamily = intfSMT_TestDataReport.GetModel(DBConnection,ListPdline,timeStart,timeEnd);
        lboxStation.Items.Clear();
        //lboxStation.Items.Add(new ListItem("All", ""));
        if (dtFamily.Rows.Count > 0)
        {
            for (int i = 0; i < dtFamily.Rows.Count; i++)
            {
                lboxStation.Items.Add(new ListItem(dtFamily.Rows[i]["Family"].ToString(), dtFamily.Rows[i]["Family"].ToString()));
            }
        }
    }
}
