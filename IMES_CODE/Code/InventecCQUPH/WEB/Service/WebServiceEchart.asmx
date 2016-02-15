﻿<%@ WebService Language="C#" Class="WebServiceEchart" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.system;
using System.Collections.Generic;
using com.inventec.imes.dao;
using UPH.Interface;
using System.Data;
using com.inventec.iMESWEB;
using UPH.Interface;
using System.Collections;
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceEchart  : System.Web.Services.WebService {

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }


    [WebMethod]
    public ChartItem GetData()
    {
        IUPH uph = ServiceAgent.getInstance().GetObjectByName<IUPH>(WebConstant.UPHIUPHALL);
        ChartItem chart = new ChartItem();
        List<SeriesType> SeriesAll = new List<SeriesType>();
        
        chart.Title = "HJT 自定义控件";
        IList<string> linelist = uph.GetUPHLine();
       
        foreach (string line in linelist)
        {
            System.Data.DataTable dt = uph.GetUPH(line);

            SeriesType aeriestype1 = new SeriesType();//定义线条模型
            List<string> Series1 = new List<string>();//定义线条数据
            List<string> XList = new List<string>();//定义线条的X轴

            foreach (System.Data.DataRow dr in dt.Rows)//把X轴 和Y轴数据变为数组给Echart
            {
                XList.Add(((DateTime)dr["TimeRange"]).ToString("yyyy-MM-dd HH:mm"));
                Series1.Add(dr["ProductRatio"].ToString());
                
            }
            aeriestype1.Name = line;//线条的名字
            aeriestype1.SeriesValues = Series1;//线条赋值
            aeriestype1.XValue = XList;//线条的X轴
            SeriesAll.Add(aeriestype1);

        }
        chart.Series = SeriesAll;//存放多个线条
       

        return chart;


    }
 
    [WebMethod]
    public ChartItem GetData_Daily(string process)
    {
        try
        {
           // List<string> dailylist = new List<string> { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11",
           //                                  "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"};
            //List<string> xlist = new List<string>();
            //for (int i = 12; i > 0; i--)
            //{
            //    DateTime now = DateTime.Now;
            //    DateTime xbegin = new DateTime(now.Year, now.Month, now.Day, now.Hour,0,0);
            //    xlist.Add(xbegin.AddHours(-i).ToString("MM-dd HH:mm"));
            //}
            
            IUPH uph = ServiceAgent.getInstance().GetObjectByName<IUPH>(WebConstant.UPHIUPHALL);
            ChartItem chart = new ChartItem();
            List<SeriesType> SeriesAll = new List<SeriesType>();

            // chart.Title = "SMT_Daily";
            IList<string> linelist = uph.GetUPHLine(process);
            foreach (string line in linelist)
            {
                int queryhours = 24;
                DateTime now = DateTime.Now;
                DateTime before=now.AddHours(-queryhours);

                DateTime begin = new DateTime(before.Year, before.Month, before.Day, 0, 0, 0);
                DateTime end = DateTime.Now;

                System.Data.DataTable dt = uph.GetUPH(line, process, begin, end);
                SeriesType aeriestype1 = new SeriesType();//定义线条模型
                List<string> Series1 = new List<string>();//定义线条数据
                List<string> Series2 = new List<string>();//定义线条数据
                List<string> XList = new List<string>();//定义线条的X轴
             
                Dictionary<string, string> diclist = new Dictionary<string, string>();
                for (int i = queryhours; i > -1; i--)
                {
                   
                      DateTime xbegin = new DateTime(now.Year, now.Month, now.Day, now.Hour,0,0);
                      diclist.Add(xbegin.AddHours(-i).ToString("MM-dd HH:mm"),"0");
                }
                
                foreach (System.Data.DataRow dr in dt.Rows)//把X轴 和Y轴数据变为数组给Echart
                {
                    DateTime xdt = Convert.ToDateTime(dr["Dt"]);
                    string dd = xdt.ToString("MM-dd");
                    string hh = dr["Hour"].ToString().Trim().PadLeft(2, '0');
                    string ddhh = dd +" " + hh+":00";
                    
                    string qty = dr["Qty"].ToString();
                    if (diclist.ContainsKey(ddhh))
                    {
                        diclist[ddhh] = qty;
                    }
                }
                foreach (var key in diclist)
                {
                    Series1.Add(key.Value);
                    Series2.Add(key.Value);
                    XList.Add(key.Key);

                }
                aeriestype1.Name = line;//线条的名字
                aeriestype1.Name2 = line + "Output";
                aeriestype1.SeriesValues = Series1;//线条赋值
                aeriestype1.SeriesValues2 = Series2;//第2Y轴
                aeriestype1.XValue = XList;
                SeriesAll.Add(aeriestype1);
                chart.XValue = XList;

            }
            int index = DateTime.Now.Hour;
            chart.Series = SeriesAll;//存放多个线条


            return chart;
        }
        catch (Exception e)
        {
           
            throw;
        }
    
    }

    [WebMethod]
    public ChartItem GetData_Month(string process)
    {
        IUPH uph = ServiceAgent.getInstance().GetObjectByName<IUPH>(WebConstant.UPHIUPHALL);
        ChartItem chart = new ChartItem();
        List<SeriesType> SeriesAll = new List<SeriesType>();
       // List<string> xlist = new List<string>();
        DateTime now = DateTime.Now;
        
            
        
        
        // chart.Title = "SMT_Daily";
        IList<string> linelist = uph.GetUPHLine(process);
        foreach (string line in linelist)
        {
            //DateTime now = DateTime.Now;
            int querybeforeday = now.Day + 7;
            DateTime beforedate = DateTime.Now.AddDays(-querybeforeday);
            
            DateTime begin = new DateTime(beforedate.Year, beforedate.Month, beforedate.Day, 0, 0, 0);
            DateTime end = now;

            System.Data.DataTable dt = uph.GetUPH_Month(line, process, begin, end);
            SeriesType aeriestype1 = new SeriesType();//定义线条模型
            List<string> Series1 = new List<string>();//定义线条数据
            List<string> XList = new List<string>();//定义线条的X轴
            
           
            Dictionary<string, string> diclist = new Dictionary<string, string>();
            for (int i = querybeforeday; i > -1; i--)
            {
                diclist.Add(now.AddDays(-i).ToString("yyyy-MM-dd"), "0");
            }
            
            foreach (System.Data.DataRow dr in dt.Rows)//把X轴 和Y轴数据变为数组给Echart
            {
                string mm = dr["Dt"].ToString();
                string qty = dr["Qty"].ToString();
                if (diclist.ContainsKey(mm))
                {
                    diclist[mm] = qty;
                }

                //XList.Add(mm);
                //Series1.Add(qty);

            }
            foreach (var key in diclist)
            {
                Series1.Add(key.Value);
                XList.Add(key.Key);
               
            }
            aeriestype1.Name = line;//线条的名字
            aeriestype1.SeriesValues = Series1;//线条赋值
            aeriestype1.XValue = XList;//线条的X轴
            SeriesAll.Add(aeriestype1);
            chart.XValue = XList;

        }
        chart.Series = SeriesAll;//存放多个线条
     


        return chart;

    }


    [WebMethod]
    public ArrayList GetData_OutPut(string process)
    {
        ArrayList ret = new ArrayList();
        IUPH uph = ServiceAgent.getInstance().GetObjectByName<IUPH>(WebConstant.UPHIUPHALL);
     
        DateTime now = DateTime.Now;
        DateTime begin = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        string title = "时间段：" + begin.ToString("yyyy-MM-dd HH:mm") + "~" + now.ToString("yyyy-MM-dd HH:mm");
        List<PieType> Series1 = new List<PieType>();//定义线条数据
        List<string> XList = new List<string>();//定义线条的X轴

        IList<string> linelist = uph.GetUPHLine_OutPut(process, begin);
        foreach (string line in linelist)
        {
            XList.Add(line);
         
            System.Data.DataTable dt = uph.GetUPH_OutPut(line, process, begin);
            PieType ptye = new PieType();
            foreach (System.Data.DataRow dr in dt.Rows)//把X轴 和Y轴数据变为数组给Echart
            {
                string qty = dr["Qty"].ToString();
                ptye.Name = line;
                ptye.Value = qty;
                break;
            }
            Series1.Add(ptye);
        }
        ret.Add(XList);
        ret.Add(Series1);
        ret.Add(title);


        return ret;
    
    }
   
    
}

