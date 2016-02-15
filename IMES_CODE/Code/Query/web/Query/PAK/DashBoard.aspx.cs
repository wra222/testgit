using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
using log4net;

using System.Data;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;


public partial class DashBoard : IMESQueryBasePage
{
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        hidDbName.Value = Request["DBName"] ?? configDefaultDB;
        hidConnection.Value = CmbDBType.ddlGetConnection();
        hidImgPath.Value = MapPath(@"~\tmp\");
    }




    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetMain_WebMethod(string Connection, string imgPath, List<String> lineLst)
    {
        return GetHtmlTableString(Connection, imgPath, lineLst);


    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string[] GetModelDetail_WebMethod(string Connection, string line, string beginTime, string endTime)
    {
        IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
        DateTime dFrom = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") +" " +beginTime);
        DateTime dTo = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + endTime);

        DataTable dt = PAK_Common.GetDashBoardData_Detail(Connection, dFrom, dTo, line);
        string[] r = GetModelDetailHtmlTableString(dt,line, beginTime);
        return r;

    }
    public static string[] GetModelDetailHtmlTableString(DataTable dt,string line, string beginTime)
    {
        List<string> lst = new List<string>();
 //       { sb.AppendFormat("<tr  class='{0}' style='display:block;color: #00FF00'>", line + "_Data" + beginTime.Replace(":", "")); }
       
        string model;
        string data;
        float fpy;
        for (int j = 0; j < dt.Rows.Count; j++)
        {
            StringBuilder sb = new StringBuilder();
            model=dt.Rows[j]["Model"].ToString().Trim();
            sb.AppendFormat("<tr class='{0}' style='display:block;color: #FFE87C'>",  "Model"+line + "_Data" + beginTime.Replace(":", "") );
            for (int colIdx = 0; colIdx < dt.Columns.Count; colIdx++)
            {
                data = dt.Rows[j][colIdx].ToString().Trim().Replace("&nbsp;", "");

                if (colIdx == 3) { continue; }
                if (colIdx == 0 || colIdx == 1)
                { sb.Append("<td></td>"); }
                else if (colIdx == 2)
                {
                    sb.AppendFormat("<td colspan='2'>{0}</td>", model);
                }
                else
                {
                    if (colIdx == 13 || colIdx == 14)
                    {
                        fpy = float.Parse(data);
                        data = fpy.ToString("#0.00%");
                        sb.AppendFormat("<td>{0}</td>", data);
                    }
                    else
                    {
                        sb.AppendFormat("<td>{0}</td>", data);
                    }
               
                
                }
            }
            sb.Append("</tr>");
            lst.Add(sb.ToString());
        
        }
        return lst.ToArray();
    
    }

    private static void SetTableHeaderString(DataTable dt, StringBuilder sb, string connection, int planQ)
    {
        string totalInput = dt.Rows[dt.Rows.Count - 1]["Input"].ToString();
        string faOut = dt.Rows[dt.Rows.Count - 1]["FA Output"].ToString();
        string packOut = dt.Rows[dt.Rows.Count - 1]["PACK Out"].ToString();

        sb.AppendFormat(@"<DIV id='DIVmainTable' style='width: 100%; height: 370px;  overflow: auto;'>  
                                         <table id='mainTable'  class='iMes_grid_TableGvExt' value1='{0}' value2='{1}'   value3='{2}'  value4='{3}'
                                  style='width: 98%;border-width:0px;height:1px;table-layout:fixed;background-color: #000000;'>
                                       ", totalInput, faOut, packOut, planQ.ToString());

    }

    public static string GetHtmlTableString(string Connection, string imgPath, List<String> lineLst)
    {

        int planQ = 0;
        IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
        DataTable dt2 = PAK_Common.GetDashBoardData(Connection, out planQ);

        StringBuilder sb = new StringBuilder();
        SetTableHeaderString(dt2, sb, Connection, planQ);
        string data;
        string line;
        float fpy;
        string beginTime;
        string endTime;
        for (int j = 0; j < dt2.Rows.Count; j++)
        {
            line = dt2.Rows[j]["Line"].ToString().Trim();
            beginTime = dt2.Rows[j]["BeginTime"].ToString().Trim();
            endTime = dt2.Rows[j]["EndTime"].ToString().Trim();
            
            if (dt2.Rows[j]["BeginTime"].ToString().Trim() == "-")
            {
                sb.Append("<tr style='color: #00FF00'>");
            }
            else
            {
                if (lineLst.IndexOf(line + "_Data") > -1)
                { sb.AppendFormat("<tr  class='{0}' style='display:block;color: #00FF00'>", line + "_Data" + beginTime.Replace(":","")); }
                else
                { sb.AppendFormat("<tr  class='{0}' style='display:none;color: #00FF00'>", line + "_Data" +  beginTime.Replace(":","")); }
            }

            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                data = dt2.Rows[j][i].ToString().Trim().Replace("&nbsp;", "");
                if (dt2.Rows[j]["BeginTime"].ToString().Trim() == "-" && i == 1 && j != dt2.Rows.Count - 1)
                {
                    if (lineLst.IndexOf(line + "_Data") > -1)
                    {
                        sb.AppendFormat("<td>    <img onClick=\"ShowD('{0}',this)\" src='../../Images/minus.png' />{1}</td>", line, data);

                    }
                    else
                    {
                        sb.AppendFormat("<td >    <img onClick=\"ShowD('{0}',this)\" src='../../Images/plus.png' />{1}</td>", line, data);

                    }
                }
                else if (dt2.Rows[j]["BeginTime"].ToString().Trim() != "-" && i == 1 && j != dt2.Rows.Count - 1) //2nd layer date
                {

                    sb.AppendFormat("<td align='right' >    <img class='{3}' onClick=\"ShowModel(this,'{0}','{1}','{2}')\"  src='../../Images/plus.png' />{0}</td>", line, beginTime, endTime, line+"_Icon");
                
                }
                else 
                {
                    if (i == 13 || i == 14)
                    {
                        fpy = float.Parse(data);
                        data = fpy.ToString("#0.00%");
                        sb.AppendFormat("<td>{0}</td>", data);
                    }
                    else if (i == 1)
                    {
                        sb.AppendFormat("<td align='center'>{0}</td>", data);
                    }
                    else
                    { sb.AppendFormat("<td >{0}</td>", data); } //align='right'

                }
            }
            sb.Append("</tr>");
        }
        sb.Replace("zzTotal", "Total");
        sb.Append("</table></div> ");
        string[] arr = BindChart(dt2, imgPath);
      //  string imgId = BindChart(dt2, imgPath);
        sb.AppendFormat("<img  src='../../tmp/{0}.png'  usemap='#chartMap'/>{1} ", arr[0],arr[1]);
        BindChart(dt2, imgPath);
        DeleteOldFile(imgPath);
        return sb.ToString();

    }
    private static List<string> GetTimeRange()
    {
        int h = DateTime.Now.Hour;
        List<string> lst = new List<string>();
        if (h >= 8 && h <= 20)
        {
            for (int i = 8; i <= h; i++)
            {
                lst.Add(i.ToString() + ":00-" + (i + 1).ToString() + ":00");
            }
        }
        else
        {
            for (int i = 20; i <= h; i++)
            {
                if (i == 23)
                { lst.Add("23:00-00:00"); }
                else if (i == 24)
                { lst.Add("00:00-01:00"); }
                else
                { lst.Add(i.ToString() + ":00-" + (i + 1).ToString() + ":00"); }


            }

        }
        return lst;

    }
    private static void DeleteOldFile(string path)
    {
        try
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] fi;
            fi = d.GetFiles();
            foreach (FileInfo f in fi)
            {
                if (f.CreationTime < DateTime.Now.AddMinutes(-5))
                {
                    f.Delete();
                }
            }
        }
        catch
        {
            return;
        }
    }


    private static string[] BindChart(DataTable dt, string imgPath)
    {
        string[] arrReturn = new string[2];
        Chart chart1 = new Chart();
        chart1 = new Chart();
      //  chart1.RenderType = RenderType.BinaryStreaming;
        chart1.Width = Unit.Parse("1220px");
        chart1.Height = Unit.Parse("550px");


        chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

        chart1.Legends.Clear();
        chart1.Legends.Add("Gaol");
        ChartArea ca = chart1.ChartAreas.Add("Main");
        LabelStyle ls = new LabelStyle();
        ls.Format = "00.00%";
    
        ca.AxisY.LabelStyle = ls;
        ca.AxisY.Title = "FPF Rate";
        System.Drawing.Font titleFont = new System.Drawing.Font("Verdana", 16);
        ca.AxisY.TitleFont = titleFont;
        //   ca.AxisY.TextOrientation = TextOrientation.Horizontal;
        //    ca.AxisY.Crossing = 3;
        //chart1.ChartAreas[0].AxisX.IntervalOffset = -5;
        //chart1.ChartAreas[0].AxisY.RoundAxisValues();
        //   chart1.ChartAreas("Default").AxisX.IsMarginVisible = False
        ca.AxisX.IsMarginVisible = false;
        //    ca.AxisX.IsStartedFromZero = false;
        //chart1.ChartAreas[0].AxisX.IntervalOffset =-1;
        //ca.AxisY2.TitleFont = titleFont;
        //ca.AxisY2.TextOrientation = TextOrientation.Horizontal;
        ca.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
        ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
        ca.AxisY.Minimum = 0;
        ca.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
        //   ca.AxisX.LabelStyle.Angle = 90;

        //  ca.AxisX.Minimum = -1;
        DataView dvView = dt.DefaultView;
        // ToTable的第一個變數即為是否Distinct
        DataTable dtLine = dvView.ToTable(true, "Line");
        chart1.ChartAreas[0].AxisX.Interval = 1;
        // chart1.ChartAreas[0].AxisX.IntervalOffset =-1;

        DataRow[] drDataArray;
        string line = "";
        string beginTime;
        string endTime;
        float FPF = 0;
        float FPF_1 = 0;
        string select;
        string timeSpn;
        List<string> lstTime = GetTimeRange();
       // chart1.Series[0].ToolTip = "#VALY, #VALX";
        int count ;
        string tipRate;
        foreach (DataRow drLine in dtLine.Select("Line<>'zzTotal'"))
        {
            line = drLine[0].ToString().Trim();
      
            Series serFPFRate = new Series(line);
        //    serFPFRate.IsValueShownAsLabel = true;
           // serFPFRate.ToolTip = "#VALY, #VALX";
            serFPFRate.ToolTip = "Line: " + line;

            chart1.Series.Add(serFPFRate);
        
            serFPFRate.LabelFormat = "0%";
            serFPFRate.ChartType = SeriesChartType.Line;
            serFPFRate.BorderWidth = 3;
          //  serFPFRate.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No;
            serFPFRate.ChartArea = "Main";
            serFPFRate.YAxisType = AxisType.Primary;
            serFPFRate.MarkerStyle = MarkerStyle.Circle;
            serFPFRate.MarkerSize = 8;
            count = 0;
            
            foreach (string time in lstTime)
            {

                select = "Line='{0}' and BeginTime='{1}' and EndTime='{2}'";
                beginTime = time.Split('-')[0].Trim();
                endTime = time.Split('-')[1].Trim();
                timeSpn = beginTime + "-" + endTime;
                select = string.Format(select, line, beginTime, endTime);
                drDataArray = dt.Select(select);
                if (drDataArray.Length == 0)
                {
                    serFPFRate.Points.AddXY(timeSpn, FPF_1);
                    tipRate="0";
                }
                else
                {
                 
                    FPF = float.Parse(drDataArray[0]["FPF"].ToString());
                    tipRate = string.Format("{0:0.00%}", FPF);
                  
                   serFPFRate.Points.AddXY(timeSpn, FPF);
                //    serFPFRate.Points[count].ToolTip = "FPF : " + tipRate + "\nLine: " + line;
                   // serFPFRate.Points[count].ToolTip = "Line: " + line;

                }
       
                count++;
            }
        }

       // chart1.Series["B1D"].Points[3].ToolTip = "xxxxxxxxx";
       //double d1= chart1.Series["B1D"].Points[3].XValue;
       //double[] d2 = chart1.Series["B1D"].Points[3].YValues;

        string guid = System.Guid.NewGuid().ToString();
        chart1.SaveImage(imgPath + guid + ".png", ChartImageFormat.Png);
        string map = chart1.GetHtmlImageMap("chartMap");
        
    
        
        arrReturn[0] = guid;
        arrReturn[1] = map;

        return arrReturn;



    }
}
