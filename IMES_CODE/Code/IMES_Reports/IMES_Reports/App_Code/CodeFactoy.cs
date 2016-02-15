using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Net;
//using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Collections;
namespace IMES_Reports
{
    public class Data_Factory
    {

        public DataSet Conn(string ConStr, string ComStr)
        {

            //Str.Str_ s=new Str.Str_();


            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = ComStr;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            //return (string[])result.ToArray(typeof(string));
            return ds;

        }
        public string Conn(string ConStr, string ComStr,string Sprit)
        {

            //Str.Str_ s=new Str.Str_();
            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = ComStr;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            DataTable dt = ds.Tables[0];
            string result = "";
            //ArrayList result = new ArrayList();
            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //result.Add(ds.Tables[0].Rows[i][0].ToString());
                    result = result + ds.Tables[0].Rows[i][j].ToString() + Sprit;
                }
            }
            //return (string[])result.ToArray(typeof(string));
            return result;

        }
    }
    public class StringSprit
    {
        Data_Factory DF = new Data_Factory();
        public string[] StringSprint(string ConStr, string ComStr, string Sprit)
        {
            string[] TargetString=new string[]{""};

            if (ComStr.Length == 0 || Sprit.Length == 0)
            {
                TargetString[0] = "No Data";
            }
            else
            {
                string Temp = DF.Conn(ConStr, ComStr, Sprit);
                if (Temp.Length == 0)
                {
                    TargetString[0] = "No Data";
                }
                else
                {
                    TargetString = Temp.Split(Sprit.ToCharArray());
                }
            }

            return TargetString;
        }
    }
    #region 數據圖表繪製
    public class Func
    {
        private static AChart chart = new AChart();
        private static Object[] ChartColor = { Color.Red, Color.Blue, Color.Orange, Color.Green, Color.Cyan, Color.Purple, Color.Coral, Color.Chocolate, Color.Gray, Color.Gold, Color.Lavender, Color.Linen, Color.Magenta, Color.Moccasin, Color.Navy, Color.Olive, Color.Peru, Color.Plum, Color.Purple, Color.Salmon, Color.Sienna, Color.Silver, Color.Tan, Color.Tomato, Color.Violet, Color.Turquoise, Color.Transparent };
        /// <summary>
        /// 填充饼图、直方图、曲线图到容器里
        /// </summary>
        /// <param name="chartTitle">标题</param>
        /// <param name="control">容器(Panel,Form,TabPage)</param>
        /// <param name="dataSet">对Table[0]进行操作，饼图取最前两列，第一列为名字，第二列为值。单数据直方图取最前两列，第一列为横轴每列名称，第二列为值。多数据直方图第一列为横轴父项名称，然后依次取前一列为横轴每列名称，后一列为值。曲线图取第一列为横轴每列名称，往后每列都代表一条曲线，列名为曲线名称。</param>
        /// <param name="chartType">图表类型</param>
        /// <param name="minNumber">刻度最小值，此参数对饼图无效</param>
        /// <param name="maxNumber">刻度最大值，此参数对饼图无效</param>
        /// <param name="scale">刻度值，此参数对饼图无效</param>
        /// <param name="unit">值的单位，此参数对饼图无效</param>
        public static void DrawingChart(string chartTitle, Control control, DataSet dataSet, ChartType chartType, int minNumber, int maxNumber, int scale, string unit)
        {
            DrawingChartInclude(chartTitle, control, dataSet, chartType, minNumber, maxNumber, scale, unit);
        }
        /// <summary>
        /// 填充饼图到容器里
        /// </summary>
        /// <param name="chartTitle">标题</param>
        /// <param name="control">容器(Panel,Form,TabPage)</param>
        /// <param name="dataSet">对Table[0]进行操作，取最前两列，第一列为名字，第二列为值</param>
        public static void DrawingChart(string chartTitle, Control control, DataSet dataSet)
        {
            DrawingChartInclude(chartTitle, control, dataSet, ChartType.Piegraph, 0, 0, 0, "");
        }
        private static void DrawingChartInclude(string chartTitle, Control control, DataSet dataSet, ChartType chartType, int minNumber, int maxNumber, int scale, string unit)
        {
            if (control.Height < 230 || control.Width < 230)
            {
                Func.Msg("容器宽度或长度设置过小！（宽度和长度必须>=230）");
                return;
            }
            if (control.GetType().Name.Equals("Form") || control.GetType().Name.Equals("TabPage") || control.GetType().Name.Equals("GroupBox") || control.GetType().Name.Equals("Panel"))
            {
                for (int i = 0; i < chart.Count; i++)
                {
                    if (control == (Control)chart[i].crtObj)
                    {
                        chart.RemoveChart(i);
                        break;
                    }
                }
                chart.AddChart(chartTitle, control, chartType, dataSet, minNumber, maxNumber, scale, unit);
                control.Paint += new PaintEventHandler(control_Paint);
                control.Disposed += new EventHandler(control_Disposed);
                control.Refresh();
            }
            else
            {
                Func.Msg("该容器不是Form,TabPage,GroupBox,Panel类型！");
                return;
            }
        }
        static void control_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                for (int i = 0; i < chart.Count; i++)
                {
                    if (chart[i].crtType == ChartType.None)
                    {
                        break;
                    }
                    else if (chart[i].crtObj != sender)
                    {
                        continue;
                    }
                    int x = 0;
                    int y = 0;
                    int diameter = Math.Min(((Control)chart[i].crtObj).Height, ((Control)chart[i].crtObj).Width) - 100;
                    if (((Control)chart[i].crtObj).Width >= ((Control)chart[i].crtObj).Height)
                    {
                        x = (((Control)chart[i].crtObj).Width - ((Control)chart[i].crtObj).Height) / 2 + 50;
                        y = 50;
                    }
                    else
                    {
                        x = 50;
                        y = (((Control)chart[i].crtObj).Height - ((Control)chart[i].crtObj).Width) / 2 + 50;
                    }
                    Pen line = new Pen(Color.Black, 2);
                    SolidBrush linesb = new SolidBrush(Color.DarkGreen);
                    Rectangle rect = new Rectangle(x, y, diameter, diameter);
                    Pen title = new Pen(Color.Blue, 2);
                    SolidBrush B = new SolidBrush(Color.Blue);
                    SolidBrush BL = new SolidBrush(Color.Black);
                    StringFormat Ll = new StringFormat();
                    Ll.Alignment = System.Drawing.StringAlignment.Near;
                    int height = ((Control)chart[i].crtObj).Height - 20;
                    //写标题
                    RectangleF RectTitle;
                    if (chart[i].crtType == ChartType.Other)
                    {
                         RectTitle = new RectangleF(diameter / 2 - 20, 5, diameter, diameter);
                        e.Graphics.DrawString(chart[i].crtTitle, new Font("Arial", 12), B, RectTitle, Ll);
                    }
                    else
                    {
                         RectTitle = new RectangleF(diameter / 2 + 50, 5, diameter, diameter);
                        e.Graphics.DrawString(chart[i].crtTitle, new Font("Arial", 12), B, RectTitle, Ll);
                    }

                    #region 饼图
                    if (chart[i].crtType == ChartType.Piegraph)
                    {
                        float total = 0;
                        float fontWidth = 0;
                        for (int j = 0; j < chart[i].crtDataSet.Tables[0].Rows.Count; j++)
                        {
                            total += (float)Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][1].ToString());
                            if (fontWidth < e.Graphics.MeasureString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10)).Width)
                            {
                                fontWidth = e.Graphics.MeasureString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10)).Width;
                            }
                        }
                        float next = 0;
                        for (int j = 0; j < chart[i].crtDataSet.Tables[0].Rows.Count; j++)
                        {
                            SolidBrush sb = new SolidBrush((Color)ChartColor[j % ChartColor.Length]);
                            float tmp = (float)(Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][1].ToString()) / total * 100 * 3.6);
                            e.Graphics.FillPie(sb, rect, next, tmp);
                            RectTitle = new RectangleF(5, height, diameter, diameter);
                            B = new SolidBrush(Color.Black);
                            e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10), B, RectTitle, Ll);
                            RectTitle = new RectangleF(5 + (int)fontWidth + 2, height, 10, 12);
                            e.Graphics.FillRectangle(sb, RectTitle);
                            RectTitle = new RectangleF(x + diameter + 2, height, 10, 12);
                            e.Graphics.FillRectangle(sb, RectTitle);
                            RectTitle = new RectangleF(x + diameter + 14, height, diameter, diameter);
                            e.Graphics.DrawString(Convert.ToString(Math.Round((Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][1].ToString()) / total * 100), 0)) + "%", new Font("Arial", 10), B, RectTitle, Ll);
                            height -= 20;
                            next += tmp;
                        }
                    }
                    #endregion
                    #region 直方图
                    else if (chart[i].crtType == ChartType.Histogram)
                    {
                        e.Graphics.DrawLine(line, x, y, x, y + diameter);
                        height = y + diameter - 12;
                        int scaleBS = diameter / ((chart[i].crtMaxNumber - chart[i].crtMinNumber) / chart[i].crtScale);
                        float fontWidth = e.Graphics.MeasureString(chart[i].crtMaxNumber.ToString(), new Font("Arial", 10)).Width;
                        for (int j = chart[i].crtMinNumber; j <= chart[i].crtMaxNumber; j += chart[i].crtScale)
                        {
                            RectangleF RectScaleHeightLine = new RectangleF(x - fontWidth - 1, height, diameter, diameter);
                            e.Graphics.DrawString(j.ToString(), new Font("Arial", 10), linesb, RectScaleHeightLine, Ll);
                            height -= scaleBS;
                        }
                        height = y + diameter;
                        int width = x + 10; // 控制靠近最左边轴的位置
                        for (int j = 0; j < chart[i].crtDataSet.Tables[0].Rows.Count; j++)
                        {
                            if (chart[i].crtDataSet.Tables[0].Columns.Count <= 2)
                            {
                                SolidBrush sb = new SolidBrush((Color)ChartColor[j % ChartColor.Length]);
                                RectangleF RectScale = new RectangleF(width, height, diameter, diameter);
                                e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10), sb, RectScale, Ll);
                                double aa = Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][1].ToString()) / chart[i].crtScale * scaleBS;
                                RectScale = new RectangleF(width, y + diameter - (int)aa, 20, (int)aa);
                                e.Graphics.FillRectangle(sb, RectScale);
                                RectScale = new RectangleF(width, y + diameter - (int)aa - 15, diameter, diameter);
                                e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][1].ToString() + chart[i].crtUnit, new Font("Arial", 10), sb, RectScale);
                            }
                            else
                            {
                                StringFormat StrF = new StringFormat();
                                StrF.FormatFlags = StringFormatFlags.DirectionVertical;
                                e.Graphics.DrawString("Rate(%)", new Font("宋体", 20), BL, new Point(x-55, diameter/2), StrF);
                                int sbnum = 0;
                                RectangleF RectScale = new RectangleF(width, height + 24, diameter, diameter);

                                if (chart[i].crtDataSet.Tables[0].Columns.Count > 5)
                                {
                                    e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10), B, RectScale, Ll);
                                    for (int k = 6; k < chart[i].crtDataSet.Tables[0].Columns.Count; k += 1)
                                    {
                                       
                                        SolidBrush sb = new SolidBrush((Color)ChartColor[sbnum % ChartColor.Length]);
                                        sbnum++;
                                        RectScale = new RectangleF(width, height + (sbnum % 2) * 12, diameter, diameter);
                                        //写下标
                                        // e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString(), new Font("Arial", 8), sb, RectScale, Ll);
                                        //写柱状图
                                        double aa = Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString()) / chart[i].crtScale * scaleBS;
                                        RectScale = new RectangleF(width, y + diameter - (int)aa, 28, (int)aa);
                                        e.Graphics.FillRectangle(sb, RectScale);
                                        RectScale = new RectangleF(width, y + diameter - (int)aa - 15, diameter, diameter);
                                        //写上标
                                        e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString() + chart[i].crtUnit, new Font("Arial", 8), sb, RectScale);
                                        width += 28;
                                    }
                                }
                                else
                                {
                                    
                                    for (int k = 0; k < chart[i].crtDataSet.Tables[0].Columns.Count; k += 1)
                                    {
                                        RectangleF RectScale_1 = new RectangleF(width, height + 24, diameter, diameter);
                                        e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Columns[k].Caption.ToString(), new Font("Arial", 10), B, RectScale_1, Ll);
                                        SolidBrush sb = new SolidBrush((Color)ChartColor[sbnum % ChartColor.Length]);
                                        sbnum++;
                                        RectScale = new RectangleF(width, height + (sbnum % 2) * 12, diameter, diameter);
                                        //写下标
                                        // e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString(), new Font("Arial", 8), sb, RectScale, Ll);
                                        //写柱状图
                                        double aa = Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString()) / chart[i].crtScale * scaleBS;
                                        RectScale = new RectangleF(width, y + diameter - (int)aa, 28, (int)aa);
                                        e.Graphics.FillRectangle(sb, RectScale);
                                        RectScale = new RectangleF(width, y + diameter - (int)aa - 15, diameter, diameter);
                                        //写上标
                                        e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString() + chart[i].crtUnit, new Font("Arial", 8), sb, RectScale);
                                        width += 60;
                                    }
                                }
                            }
                            if ((fontWidth + e.Graphics.MeasureString(chart[i].crtUnit, new Font("Arial", 10)).Width) >= 30)
                            {
                                width += (int)fontWidth + (int)e.Graphics.MeasureString(chart[i].crtUnit, new Font("Arial", 10)).Width - 10;
                            }
                            else
                            {
                                width += 30;
                            }
                        }
                        e.Graphics.DrawLine(line, x, y + diameter, width, y + diameter);
                    }
                    #endregion
                    #region 曲线图
                    else if (chart[i].crtType == ChartType.Polygram)
                    {
                        int width = x;
                        int scaleBS = diameter / ((chart[i].crtMaxNumber - chart[i].crtMinNumber) / chart[i].crtScale);
                        int scaleWidthBS = diameter / chart[i].crtDataSet.Tables[0].Rows.Count;
                        height = y + diameter - ((chart[i].crtMaxNumber - chart[i].crtMinNumber) / chart[i].crtScale) * scaleBS;
                        float fontWidth = e.Graphics.MeasureString(chart[i].crtMaxNumber.ToString(), new Font("Arial", 10)).Width;
                        for (int j = 0; j <= chart[i].crtDataSet.Tables[0].Rows.Count; j++)
                        {
                            e.Graphics.DrawLine(line, width, height, width, y + diameter);
                            width += scaleWidthBS;
                        }
                        height = y + diameter - 12;
                        for (int j = chart[i].crtMinNumber; j <= chart[i].crtMaxNumber; j += chart[i].crtScale)
                        {
                            e.Graphics.DrawLine(line, x, height + 12, x + scaleWidthBS * chart[i].crtDataSet.Tables[0].Rows.Count, height + 12);
                            RectangleF RectScaleHeightLine = new RectangleF(x - fontWidth - 1, height, diameter, diameter);
                            e.Graphics.DrawString(j.ToString(), new Font("Arial", 10), linesb, RectScaleHeightLine, Ll);
                            height -= scaleBS;
                        }
                        width = x;
                        height = y + diameter;
                        double nowheight = 0;
                        for (int j = 0; j < chart[i].crtDataSet.Tables[0].Rows.Count; j++)
                        {
                            height = y + diameter;
                            SolidBrush textsb = new SolidBrush(Color.Black);
                            RectangleF rectText = new RectangleF(width + scaleWidthBS, height + 2 + (j % 2) * 12, diameter, diameter);
                            e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10), textsb, rectText);
                            //
                            for (int k = 1; k < chart[i].crtDataSet.Tables[0].Columns.Count; k++)
                            {
                                Pen polygramLine = new Pen((Color)ChartColor[(k - 1) % ChartColor.Length]);
                                SolidBrush valueSB = new SolidBrush((Color)ChartColor[(k - 1) % ChartColor.Length]);
                                double aa = y + diameter;
                                if (j > 0)
                                {
                                    aa = y + diameter - Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j - 1][k].ToString()) / chart[i].crtScale * scaleBS;
                                }
                                nowheight = y + diameter - Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString()) / chart[i].crtScale * scaleBS;
                                e.Graphics.DrawLine(polygramLine, width, (int)aa, width + scaleWidthBS, (int)nowheight);
                                RectangleF RectScale = new RectangleF(width + scaleWidthBS, (int)nowheight - 12, diameter, diameter);
                                e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString() + chart[i].crtUnit, new Font("Arial", 10), valueSB, RectScale);
                            }
                            width += scaleWidthBS;
                        }
                        fontWidth = 0;
                        for (int j = 1; j < chart[i].crtDataSet.Tables[0].Columns.Count; j++)
                        {
                            if (fontWidth < e.Graphics.MeasureString(chart[i].crtDataSet.Tables[0].Columns[j].ColumnName.ToString(), new Font("Arial", 10)).Width)
                            {
                                fontWidth = e.Graphics.MeasureString(chart[i].crtDataSet.Tables[0].Columns[j].ColumnName.ToString(), new Font("Arial", 10)).Width;
                            }
                        }
                        height = y + diameter;
                        for (int j = 1; j < chart[i].crtDataSet.Tables[0].Columns.Count; j++)
                        {
                            RectTitle = new RectangleF(5, height, diameter, diameter);
                            B = new SolidBrush(Color.Black);
                            SolidBrush valueSB = new SolidBrush((Color)ChartColor[(j - 1) % ChartColor.Length]);
                            e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Columns[j].ColumnName.ToString(), new Font("Arial", 10), B, RectTitle, Ll);
                            RectTitle = new RectangleF(5 + (int)fontWidth + 2, height, 15, 12);
                            e.Graphics.FillRectangle(valueSB, RectTitle);
                            height -= 20;
                        }
                    }
                    #endregion
                    #region 圖包圖
                    else if (chart[i].crtType == ChartType.Other)
                    {
                        //x += ;
                        e.Graphics.DrawLine(line, x, y, x, y + diameter);
                        height = y + diameter - 12;
                        int scaleBS = diameter / ((chart[i].crtMaxNumber - chart[i].crtMinNumber) / chart[i].crtScale);
                        float fontWidth = e.Graphics.MeasureString(chart[i].crtMaxNumber.ToString(), new Font("Arial", 10)).Width;
                        for (int j = chart[i].crtMinNumber; j <= chart[i].crtMaxNumber; j += chart[i].crtMaxNumber)
                        {
                            RectangleF RectScaleHeightLine = new RectangleF(x - fontWidth - 1, height, diameter, diameter);
                            e.Graphics.DrawString(j.ToString(), new Font("Arial", 10), linesb, RectScaleHeightLine, Ll);
                            //height -= scaleBS;
                            height -= y + diameter -60 ; 
                            //for (int y_height = chart[i].crtMinNumber; y_height <= chart[i].crtMaxNumber; y_height += chart[i].crtScale)

                            //{ height -= y + diameter-12; }
                        }
                        height = y + diameter;
                        int width = x + 30; // 控制靠近最左边轴的位置
                        for (int j = 0; j < chart[i].crtDataSet.Tables[0].Rows.Count; j++)
                        {
                            if (chart[i].crtDataSet.Tables[0].Columns.Count <= 2)
                            {
                                SolidBrush sb = new SolidBrush((Color)ChartColor[j % ChartColor.Length]);
                                RectangleF RectScale = new RectangleF(width, height, diameter, diameter);
                                e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10), sb, RectScale, Ll);
                                double aa = Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][1].ToString()) / chart[i].crtScale * scaleBS;
                                RectScale = new RectangleF(width, y + diameter - (int)aa, 20, (int)aa);
                                e.Graphics.FillRectangle(sb, RectScale);
                                RectScale = new RectangleF(width, y + diameter - (int)aa - 15, diameter, diameter);
                                e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][1].ToString() + chart[i].crtUnit, new Font("Arial", 10), sb, RectScale);
                            }
                            else
                            {
                                StringFormat StrF = new StringFormat();
                                StrF.FormatFlags = StringFormatFlags.DirectionVertical;
                                //e.Graphics.DrawString("Rate(%)", new Font("宋体", 20), BL, new Point(x - 55, diameter / 2), StrF);
                                int sbnum = 0;
                                RectangleF RectScale = new RectangleF(width, height + 24, diameter, diameter);

                                if (chart[i].crtDataSet.Tables[0].Columns.Count > 5)
                                {
                                    e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][0].ToString(), new Font("Arial", 10), B, RectScale, Ll);
                                    for (int k = 6; k < chart[i].crtDataSet.Tables[0].Columns.Count; k += 1)
                                    {

                                        SolidBrush sb = new SolidBrush((Color)ChartColor[sbnum % ChartColor.Length]);
                                        sbnum++;
                                        RectScale = new RectangleF(width, height + (sbnum % 2) * 12, diameter, diameter);
                                        //写下标
                                        // e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString(), new Font("Arial", 8), sb, RectScale, Ll);
                                        //写柱状图
                                        double aa = Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString()) / chart[i].crtScale * scaleBS;
                                        RectScale = new RectangleF(width, y + diameter - (int)aa, 38, (int)aa);
                                        e.Graphics.FillRectangle(sb, RectScale);
                                        RectScale = new RectangleF(width, y + diameter - (int)aa - 15, diameter, diameter);
                                        //写上标
                                        e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString() + chart[i].crtUnit, new Font("Arial", 8), sb, RectScale);
                                        width += 28;
                                    }
                                }
                                else
                                {

                                    for (int k = 0; k < chart[i].crtDataSet.Tables[0].Columns.Count; k += 1)
                                    {
                                        RectangleF RectScale_1 = new RectangleF(width, height + 24, diameter, diameter);
                                       // e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Columns[k].Caption.ToString(), new Font("Arial", 10), B, RectScale_1, Ll);
                                        SolidBrush sb = new SolidBrush((Color)ChartColor[sbnum % ChartColor.Length]);
                                        sbnum++;
                                        RectScale = new RectangleF(width, height + (sbnum % 2) * 12, diameter, diameter);
                                        //写下标
                                        // e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString(), new Font("Arial", 8), sb, RectScale, Ll);
                                        //写柱状图
                                        double aa = Convert.ToDouble(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString()) / chart[i].crtScale * scaleBS;
                                        RectScale = new RectangleF(width, y + diameter - (int)aa, 28, (int)aa);
                                        e.Graphics.FillRectangle(sb, RectScale);
                                        if (k % 2 == 0)
                                        {
                                            RectScale = new RectangleF(width-30, y + diameter - (int)aa - 15, diameter, diameter);
                                        }
                                        else
                                        {
                                            RectScale = new RectangleF(width+30, y + diameter - (int)aa - 15, diameter, diameter);
                                        }
                                        //写上标
                                        e.Graphics.DrawString(chart[i].crtDataSet.Tables[0].Rows[j][k].ToString() + chart[i].crtUnit, new Font("Arial", 8), sb, RectScale);
                                        if (k == 1)
                                        {
                                            width += 65;
                                        }
                                    }
                                }
                            }
                            if ((fontWidth + e.Graphics.MeasureString(chart[i].crtUnit, new Font("Arial", 10)).Width) >= 30)
                            {
                                width += (int)fontWidth + (int)e.Graphics.MeasureString(chart[i].crtUnit, new Font("Arial", 10)).Width - 10;
                            }
                            else
                            {
                                width += 30;
                            }
                        }
                        e.Graphics.DrawLine(line, x, y + diameter, width+30, y + diameter);
                    }
                    #endregion
                    break;
                }
            }
            catch
            {
                return;
            }
        }
        static void control_Disposed(object sender, EventArgs e)
        {
            for (int i = 0; i < chart.Count; i++)
            {
                if ((Control)sender == (Control)chart[i].crtObj)
                {
                    chart.RemoveChart(i);
                    break;
                }
            }
        }
        static void Msg(string str)
        {
            MessageBox.Show(str);
        }
        public static DataSet DBbind(string str)
        {
            Data_Factory D_F = new Data_Factory();
            string Str_Conn;
            Str_Conn = "data source=10.96.183.202,998;Initial Catalog=HPIMES_Rep;User ID=imes_qry;Password=imes_qry";//;App=HPEDITS105;
            DataSet ds = D_F.Conn(Str_Conn, str);
            return ds;
        }


    }
    public class pChart
    {
        public Object crtObj;
        public ChartType crtType;
        public DataSet crtDataSet;
        public string crtTitle = "";
        public int crtMinNumber = 0;
        public int crtMaxNumber = 0;
        public int crtScale = 0;
        public string crtUnit = "";
        public pChart(string chartTitle, Object chartObject, ChartType chartType, DataSet chartDataSet, int minNumber, int maxNumber, int scale, string unit)
        {
            crtObj = chartObject;
            crtType = chartType;
            crtDataSet = chartDataSet;
            crtTitle = chartTitle;
            crtMinNumber = minNumber;
            crtMaxNumber = maxNumber;
            crtScale = scale;
            crtUnit = unit;
        }
    }
    public class AChart : System.Collections.CollectionBase
    {
        public AChart()
        { }
        public void AddChart(string chartTitle, Object chartObject, ChartType chartType, DataSet chartDataSet, int minNumber, int maxNumber, int scale, string unit)
        {
            List.Add(new pChart(chartTitle, chartObject, chartType, chartDataSet, minNumber, maxNumber, scale, unit));
        }
        public void RemoveChart(int index)
        {
            if (index > Count - 1 || index < 0)
            {
                MessageBox.Show("Index not valid!");
            }
            else
            {
                List.RemoveAt(index);
            }
        }
        public AChart Item
        {
            get
            {
                return this;
            }
        }
        [System.Runtime.CompilerServices.IndexerName("item")]
        public pChart this[int index]
        {
            get
            {
                return (pChart)List[index];
            }
        }
    }
    /// <summary>
    /// 图表类型
    /// </summary>
    public enum ChartType
    {
        /// <summary>
        /// 直方图
        /// </summary>
        Histogram,
        /// <summary>
        /// 饼图
        /// </summary>
        Piegraph,
        /// <summary>
        /// 曲线图
        /// </summary>
        Polygram,
        /// <summary>
        /// Other
        /// </summary>
        Other,
        /// <summary>
        /// None
        /// </summary>
        None
    }
    public static class Extensions
    {
        public static void SafeCall(this Control ctrl, Action<Object> callback, Object obj)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(callback, obj);
            else
                callback(obj);
        }
    }

    //3
    //测试用例：
    /*
    private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = Func.DBbind("select '甲' as name,50 as value union all select '乙',60 union all select '丙',70 union all select '丁',80");
            Func.DrawingChart("直方图一测试", this.panel1, ds, ChartType.Histogram, 0, 100, 10, "万");
        }
     */
    #endregion
    public class IsInOANet
    {
        public  bool IsCanConnect_1(string url)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            bool CanCn = true;   //设成可以连接； 
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.Timeout = 1;
                res = (HttpWebResponse)req.GetResponse();
                Thread.Sleep(5000);
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Close();
                }
                CanCn = false;   //无法连接 
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return CanCn;
        }
        public bool IsCanConnect(string ip)
        {
            IPAddress myIP = IPAddress.Parse(ip);
            string HostName = "";
            bool Result=false;
            try
            {
                IPHostEntry myHost = Dns.GetHostByAddress(myIP);
                HostName = myHost.HostName.ToString();
            }
            catch
            {
                HostName = "";
            }
            if (HostName == "")
            {
                //HostName = "连接不通";
                Result = false;
            }
            else if (HostName == "bogon")
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
            return Result;
        }
    }
}
