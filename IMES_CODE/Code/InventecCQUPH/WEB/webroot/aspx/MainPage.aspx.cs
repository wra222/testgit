using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.DataVisualization.Charting;
using com.inventec.imes.dao;
using System.Drawing;
using System.Collections.Generic;

public partial class webroot_aspx_MainPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // Chart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);
         
        Timer1.Enabled = false;
        Timer1.Interval = 5000;
        ClearChart();
       // refreshchar("FA");
       // refreshchar("PAK");
        InitializeChart("FA");
        InitializeChart("PAK");
    }
    

    protected void Timer1_Tick(object sender, EventArgs e)
    {

    }
  
    private void  ClearChart()
    {
       Chart1.Series.Clear(); //清除图表集合
        Chart1.ChartAreas.Clear();//清除绘图区域
        Chart1.Titles.Clear();//清除标题
       Chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

       Chart1.Legends.Clear();//图表使用的图例名称
       #region 设置图表的属性
       //图表的背景色
       Chart1.BackColor = Color.FromArgb(211, 223, 240);
       //图表背景色的渐变方式
       Chart1.BackGradientStyle = GradientStyle.TopBottom;
       //图表的边框颜色、
       Chart1.BorderlineColor = Color.FromArgb(26, 59, 105);
       //图表的边框线条样式
       Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
       //图表边框线条的宽度
       Chart1.BorderlineWidth = 2;
       //图表边框的皮肤
       Chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
       #endregion

       #region 设置图表的Title
       Title title = new Title();
       //标题内容
       title.Text = "FA & PAK 效率";
       //标题的字体
       title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
       //标题字体颜色
       title.ForeColor = Color.FromArgb(26, 59, 105);
       //标题阴影颜色
       title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
       //标题阴影偏移量
       title.ShadowOffset = 3;

       Chart1.Titles.Add(title);
       #endregion
    }
   
    /// <summary>
    /// 绘图区域名称
    /// </summary>
    /// <param name="ChartAreasname"></param>
    private void refreshchar(string ChartAreasname)
    {
     

        Chart1.Legends.Add(ChartAreasname+"_Gaol");
        Chart1.ChartAreas.Add(ChartAreasname);//绘图区域名称
        //设置坐标轴  
        Chart1.ChartAreas[ChartAreasname].AxisX.LineColor = Color.Blue;
        Chart1.ChartAreas[ChartAreasname].AxisY.LineColor = Color.Blue;
        Chart1.ChartAreas[ChartAreasname].AxisX.LineWidth = 2;
        Chart1.ChartAreas[ChartAreasname].AxisX.Title = "TimeRange";


        Chart1.ChartAreas[ChartAreasname].AxisY.LineWidth = 2;
        Chart1.ChartAreas[ChartAreasname].AxisY.Title = "ProductRatio";
        //设置网格线  
        Chart1.ChartAreas[ChartAreasname].AxisX.MajorGrid.LineColor = Color.Blue;
        Chart1.ChartAreas[ChartAreasname].AxisY.MajorGrid.LineColor = Color.Blue;

        Chart1.ChartAreas[ChartAreasname].AxisY.Minimum = 0;

        Chart1.ChartAreas[ChartAreasname].AxisX.Interval = 1;   //设置X轴坐标的间隔为1
        Chart1.ChartAreas[ChartAreasname].AxisX.IntervalOffset = 1;  //设置X轴坐标偏移为1
        Chart1.ChartAreas[ChartAreasname].AxisX.LabelStyle.IsStaggered = true;   //设置是否交错显示,比如数据多的时间分成两行来显示 
        Chart1.Titles.Add(ChartAreasname +"达成率");
      
        Chart1.BackColor = Color.Azure; //图片背景色  
        IList<string> linelist = AuthorityDao.Instance.GetUPHLine();
        foreach (string line in linelist)
        {
            Series series = Chart1.Series.Add(line + ChartAreasname);
         //   Series series = new Series(line);
            series.ChartType = SeriesChartType.Line;               //图标集类型，Line为直线，SpLine为曲线  
          //  series.Color = Color.Green;                              //线条颜色  
            series.ChartArea = ChartAreasname;
            series.LegendText = ChartAreasname+" Line: "+line;
            series.BorderWidth = 2;                                  //线条宽度  
            series.ShadowOffset = 1;                                 //阴影宽度  
            series.IsVisibleInLegend = true;                         //是否显示数据说明  
            series.IsValueShownAsLabel = true;
            series.MarkerStyle = MarkerStyle.Diamond;               //线条上的数据点标志类型  
            series.MarkerSize = 8;
            series.LabelFormat = "0.00%";
            series.XValueType = ChartValueType.Time;
            DataTable dt = AuthorityDao.Instance.GetUPH(line);
            DataView dv = new DataView(dt);
            series.Points.DataBindXY(dv, "TimeRange", dv, "ProductRatio");
            series.ToolTip = "时间：#VALX\\n达成率：#VAL";
            
        }
       

      






    }

    

   /// <summary>
        /// 初始化Char控件样式
        /// </summary>
   public void InitializeChart(string stage)
        {
            Chart1.BackColor = Color.Azure; //图片背景色  


            #region 设置图表区属性
            //图表区的名字
            ChartArea chartArea = new ChartArea(stage);
            //背景色
            chartArea.BackColor = Color.FromArgb(64, 165, 191, 228);
            //背景渐变方式
            chartArea.BackGradientStyle = GradientStyle.TopBottom;
            //渐变和阴影的辅助背景色
            chartArea.BackSecondaryColor = Color.White;
            //边框颜色
            chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            //阴影颜色
            chartArea.ShadowColor = Color.Transparent;

            //设置X轴和Y轴线条的颜色和宽度
            chartArea.AxisX.LineColor = Color.Blue;
            chartArea.AxisX.LineWidth = 4;
            chartArea.AxisY.LineColor = Color.Blue;
            chartArea.AxisY.LineWidth = 4;

            //设置X轴和Y轴的标题
            chartArea.AxisX.Title = stage+"TimeRange";
            chartArea.AxisY.Title = stage+"ProductRatio";

            //设置图表区网格横纵线条的颜色和宽度
            chartArea.AxisX.MajorGrid.LineColor = Color.Blue;
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineColor = Color.Blue;
            chartArea.AxisY.MajorGrid.LineWidth = 1;

            chartArea.AxisX.Interval = 1;   //设置X轴坐标的间隔为1
            chartArea.AxisX.IntervalOffset = 1;  //设置X轴坐标偏移为1
            chartArea.AxisX.LabelStyle.IsStaggered = true;   //设置是否交错显示,比如数据多的时间分成两行来显示 



            Chart1.ChartAreas.Add(chartArea);
            #endregion

            #region 图例及图例的位置
            Legend legend = new Legend(stage);
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Top;
      
      
            this.Chart1.Legends.Add(legend);
            #endregion


            #region 绑定数据
            IList<string> linelist = AuthorityDao.Instance.GetUPHLine();
            foreach (string line in linelist)
            {
                Series series = Chart1.Series.Add(stage + line);
                //Series series = new Series(line);
                series.ChartType = SeriesChartType.Line;               //图标集类型，Line为直线，SpLine为曲线  
                //  series.Color = Color.Green;                              //线条颜色  
                series.ChartArea = stage;
                series.Legend = stage;
                series.LegendText = "Line: " + stage+"_" + line;
                series.BorderWidth = 2;                                  //线条宽度  
                series.ShadowOffset = 1;                                 //阴影宽度  
                series.IsVisibleInLegend = true;                         //是否显示数据说明  
                series.IsValueShownAsLabel = false;
                series.MarkerStyle = MarkerStyle.Diamond;               //线条上的数据点标志类型  
                series.MarkerSize = 8;
                series.LabelFormat = "0.00%";
                series.XValueType = ChartValueType.Time;
                DataTable dt = AuthorityDao.Instance.GetUPH(line);
                DataView dv = new DataView(dt);
                series.Points.DataBindXY(dv, "TimeRange", dv, "ProductRatio");
                series.ToolTip = "时间：#VALX\\n达成率：#VAL";

            }
            #endregion
        }

        //设置Series样式
    private Series SetSeriesStyle(int i)
        {
            Series series = new Series(string.Format("第{0}条数据", i + 1));

            //Series的类型
            series.ChartType = SeriesChartType.Line;
            //Series的边框颜色
            series.BorderColor = Color.FromArgb(180, 26, 59, 105);
            //线条宽度
            series.BorderWidth = 3;
            //线条阴影颜色
            series.ShadowColor = Color.Black;
            //阴影宽度
            series.ShadowOffset = 2;
            //是否显示数据说明
            series.IsVisibleInLegend = true;
            //线条上数据点上是否有数据显示
            series.IsValueShownAsLabel = false;
            //线条上的数据点标志类型
            series.MarkerStyle = MarkerStyle.Circle;
            //线条数据点的大小
            series.MarkerSize = 8;
            //线条颜色
            switch (i)
            {
                case 0:
                    series.Color = Color.FromArgb(220, 65, 140, 240);
                    break;
                case 1:
                    series.Color = Color.FromArgb(220, 224, 64, 10);
                    break;
                case 2:
                    series.Color = Color.FromArgb(220, 120, 150, 20);
                    break;
            }
            return series;
        }
 }

