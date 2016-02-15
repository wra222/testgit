﻿<%@ WebHandler Language="C#" Class="Commn_GetUPHDara" %>

using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using UPH.Interface;
using com.inventec.iMESWEB;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using UPH.Entity.Repository.Meta.IMESSKU;
using com.inventec.system;



public class Commn_GetUPHDara : IHttpHandler {
    IUPH uph = ServiceAgent.getInstance().GetObjectByName<IUPH>(WebConstant.UPHIUPHALL);
    public void ProcessRequest (HttpContext context) {
        NameValueCollection query = context.Request.QueryString;
        if (query.HasKeys())
        {
            string type = query["TYPE"].Trim();
           
            
            switch (type)
            {
                case "GetPdline":

                    EffPdline(context);
                    break;
                case "Efficiency_Hour":
                    GetEfficiency_Hour(context);
                    break;
                case "Efficiency_Hour_Echart":
                    GetEfficiency_Hour_Echart(context);
                    break;
                case "Efficiency_Hour_Table":
                    GetEfficiency_Hour_EchartTable(context);
                    break;
                case "Efficiency_Day":
                    GetEfficiency_Day(context);
                    break;
                case "Efficiency_Day_Echart":
                    GetEfficiency_Day_Echart(context);
                    break;
                case "Efficiency_Process_Day":
                    GetEfficiency_Process_Day(context);
                    break;
                case "Efficiency_Process_Day_Echart":
                    GetEfficiency_Process_Day_Echart(context);
                    break;
                case "Efficiency_Process_Hour":
                    GetEfficiency_Process_Hour(context);
                    break;
                case "Efficiency_Process_Hour_Echart":
                    GetEfficiency_Process_Hour_Echart(context);
                    break;
                case "GetProductQty":
                    GetProductQty(context);
                    break;
                 
                 
            }
        }
    }
    /// <summary>
    /// 线别下拉
    /// </summary>
    /// <param name="context"></param>
    private void EffPdline( HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        List<PdLine> ret = uph.EffPdline(process);
        context.Response.Write(ChangeToJson<List<PdLine>>(ret));
        context.Response.End();

    }
    /// <summary>
    /// 效率表格显示
    /// </summary>
    /// <param name="context"></param>
    private void GetEfficiency_Hour(HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        string line = context.Request.QueryString["Line"].Trim();
        string begintime = context.Request.QueryString["BeginTime"].Trim();
        string endtime = context.Request.QueryString["EndTime"].Trim();
        List<Efficiency_Hour> reteff = uph.GetEfficiency_Hour(line, process, Convert.ToDateTime(begintime), Convert.ToDateTime(endtime));
        context.Response.Write(ChangeToJson<List<Efficiency_Hour>>(reteff));
        context.Response.End();
    }
    private void GetEfficiency_Hour_EchartTable(HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        string line = context.Request.QueryString["Line"].Trim();
        DateTime begintime = Convert.ToDateTime(context.Request.QueryString["BeginTime"].Trim());
        DateTime endtime = Convert.ToDateTime(context.Request.QueryString["EndTime"].Trim());
        DataTable dt = uph.GetEfficiency_Hour_Echart(line, process, begintime, endtime);
           List<Efficiency_Hour> reteff = new List<Efficiency_Hour>();
        foreach (System.Data.DataRow dr in dt.Rows)
        {
             Efficiency_Hour eff=new Efficiency_Hour();
              eff.Dt=dr["Dt"].ToString();
              eff.Hour= dr["Hour"].ToString().Trim().PadLeft(2, '0');  
              eff.PdLine= dr["PdLine"].ToString();
              eff.Qty= Convert.ToInt32( dr["Qty"].ToString());
             eff.Efficiency= Convert.ToDecimal( dr["Efficiency"].ToString());
              eff.UPH_achieve=Convert.ToDecimal(dr["UPH_achieve"].ToString());
             reteff.Add(eff);
            
        }
        context.Response.Write(ChangeToJson<List<Efficiency_Hour>>(reteff));
        context.Response.End();
    
    }
    private void GetEfficiency_Hour_Echart(HttpContext context)
    {
      
        string process = context.Request.QueryString["Process"].Trim();
        string line = context.Request.QueryString["Line"].Trim();
        DateTime begintime = Convert.ToDateTime(context.Request.QueryString["BeginTime"].Trim());
        DateTime endtime = Convert.ToDateTime(context.Request.QueryString["EndTime"].Trim());
        DataTable dt= uph.GetEfficiency_Hour_Echart(line, process, begintime, endtime);

        ChartItem chart = new ChartItem();
        List<SeriesType> SeriesAll = new List<SeriesType>();
        List<string> Series1 = new List<string>();//定义线条数据
        List<string> XList = new List<string>();//定义线条的X轴
        List<string> qtyList = new List<string>();
        List<string> EfficiencyList = new List<string>();
        List<string> RealUPHList = new List<string>();
        string title = "区间：" + begintime + "~" + endtime;
        
        // X轴数据
        
        Dictionary<string, string> diclist = new Dictionary<string, string>();
        int hours = Convert.ToInt32((endtime - begintime).TotalHours);
        if (hours == 0)//如果begin 和end 时间一样，就去当天的0点到endtime
        {
            hours = DateTime.Now.Hour;
        }
        for (int i = 0; i < hours; i++)
        {

            DateTime xbegin = new DateTime(begintime.Year, begintime.Month, begintime.Day, begintime.Hour, 0, 0);
                string xvalue = xbegin.AddHours(i).ToString("MM-dd HH:mm");
               string uph_qty=string.Empty;
                string uph_Efficiency =string.Empty;
               string uph_UPH_achieve = string.Empty;
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    DateTime xdt = Convert.ToDateTime(dr["Dt"]);
                    string dd = xdt.ToString("MM-dd");
                    string hh = dr["Hour"].ToString().Trim().PadLeft(2, '0');
                    string ddhh = dd + " " + hh + ":00";
                    if (ddhh.Trim() == xvalue.Trim())
                    {
                         uph_qty=dr["Qty"].ToString();
                         uph_Efficiency=dr["Efficiency"].ToString();
                         uph_UPH_achieve=dr["UPH_achieve"].ToString();
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(uph_qty))
                {
                    XList.Add(xvalue);
                    qtyList.Add(uph_qty);
                    EfficiencyList.Add(uph_Efficiency);
                    RealUPHList.Add(uph_UPH_achieve);
                }
                else
                {
                    XList.Add(xvalue);
                    qtyList.Add("0");
                    EfficiencyList.Add("0");
                    RealUPHList.Add("0");
                }  
        }
       
        
        
        
        SeriesType Seriesqty = new SeriesType();
        Seriesqty.Name = "产出";
        Seriesqty.SeriesValues = qtyList;
        Seriesqty.XValue = XList;
        Seriesqty.Subtext = title;
            
            
        SeriesType SeriesEfficiency = new SeriesType();
        SeriesEfficiency.Name = "生产效率";
        SeriesEfficiency.SeriesValues=EfficiencyList;
        SeriesEfficiency.XValue = XList;
        
        
        SeriesType SeriesRealUPH = new SeriesType();
        SeriesRealUPH.Name = "UPH达成率";
        SeriesRealUPH.SeriesValues=RealUPHList;
        SeriesRealUPH.XValue = XList;

        List<SeriesType> ret = new List<SeriesType>();
        ret.Add(Seriesqty);
        ret.Add(SeriesEfficiency);
        ret.Add(SeriesRealUPH);

        context.Response.Write(ChangeToJson<List<SeriesType>>(ret));
        context.Response.End();
        
        
        
    }

    private void GetEfficiency_Day(HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        string line = context.Request.QueryString["Line"].Trim();
        string begintime = context.Request.QueryString["BeginTime"].Trim();
        string endtime = context.Request.QueryString["EndTime"].Trim();
        List<Efficiency_Day> reteff = uph.GetEfficiency_Day(line, process, Convert.ToDateTime(begintime), Convert.ToDateTime(endtime));
        context.Response.Write(ChangeToJson<List<Efficiency_Day>>(reteff));
        context.Response.End();
    }
    private void GetEfficiency_Day_Echart(HttpContext context)
    {

        string process = context.Request.QueryString["Process"].Trim();
        string line = context.Request.QueryString["Line"].Trim();
        string begintime = context.Request.QueryString["BeginTime"].Trim();
        string endtime = context.Request.QueryString["EndTime"].Trim();
        List<Efficiency_Day> reteff = uph.GetEfficiency_Day(line, process, Convert.ToDateTime(begintime), Convert.ToDateTime(endtime));

        string title = "区间：" + begintime + "~" + endtime;
        List<string> XList = new List<string>();//定义线条的X轴
        List<string> qtyList = new List<string>();
        //List<string> EfficiencyList = new List<string>();
        //List<string> RealUPHList = new List<string>();
        List<string> PerSTUPHList = new List<string>();//开线率
        List<string> PerEffUPHList = new List<string>();//效率
        List<string> PerOutUPHList = new List<string>();//整体产出率

        foreach (Efficiency_Day efftable in reteff)//把X轴 和Y轴数据变为数组给Echart
        {

            XList.Add(efftable.Dt);
            qtyList.Add(efftable.Qty.ToString());
            PerSTUPHList.Add(efftable.PerST.ToString());
            PerEffUPHList.Add(efftable.PerEff.ToString());
            PerOutUPHList.Add(efftable.PerOut.ToString());

        }
        SeriesType Seriesqty = new SeriesType();
        Seriesqty.Name = "产出";
        Seriesqty.SeriesValues = qtyList;
        Seriesqty.XValue = XList;
        Seriesqty.Subtext = title;

        SeriesType SeriesEfficiency = new SeriesType();
        SeriesEfficiency.Name = "效率";
        SeriesEfficiency.SeriesValues = PerEffUPHList;
        SeriesEfficiency.XValue = XList;


        SeriesType SeriesRealUPH = new SeriesType();
        SeriesRealUPH.Name = "整体产出率";
        SeriesRealUPH.SeriesValues = PerOutUPHList;
        SeriesRealUPH.XValue = XList;


        SeriesType SeriesPerST = new SeriesType();
        SeriesPerST.Name = "开线率";
        SeriesPerST.SeriesValues = PerSTUPHList;
        SeriesPerST.XValue = XList;
        

        List<SeriesType> ret = new List<SeriesType>();
        ret.Add(Seriesqty);
        ret.Add(SeriesEfficiency);
        ret.Add(SeriesRealUPH);
        ret.Add(SeriesPerST);

        context.Response.Write(ChangeToJson<List<SeriesType>>(ret));
        context.Response.End();



    }

    
    

    private void GetEfficiency_Process_Day(HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        string begintime = context.Request.QueryString["BeginTime"].Trim();
        string endtime = context.Request.QueryString["EndTime"].Trim();
        List<Efficiency_Process_Day> reteff = uph.GetEfficiency_Process_Day(process, Convert.ToDateTime(begintime), Convert.ToDateTime(endtime));
        context.Response.Write(ChangeToJson<List<Efficiency_Process_Day>>(reteff));
        context.Response.End();
    }
    private void GetEfficiency_Process_Day_Echart(HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        string begintime = context.Request.QueryString["BeginTime"].Trim();
        string endtime = context.Request.QueryString["EndTime"].Trim();
        List<Efficiency_Process_Day> reteff = uph.GetEfficiency_Process_Day(process, Convert.ToDateTime(begintime), Convert.ToDateTime(endtime));
        string title = "区间：" + begintime + "~" + endtime;
        List<string> XList = new List<string>();//定义线条的X轴
        List<string> qtyList = new List<string>();
        List<string> PerST = new List<string>();
        List<string> PerEff = new List<string>();
        List<string> PerOut = new List<string>();


        foreach (Efficiency_Process_Day efftable in reteff)//把X轴 和Y轴数据变为数组给Echart
        {

            XList.Add(efftable.Dt);
            qtyList.Add(efftable.Qty.ToString());
            PerST.Add(efftable.PerST.ToString());
            PerEff.Add(efftable.PerEff.ToString());
            PerOut.Add(efftable.PerOut.ToString());

        }
        SeriesType Seriesqty = new SeriesType();
        Seriesqty.Name = "产出";
        Seriesqty.SeriesValues = qtyList;
        Seriesqty.XValue = XList;
        Seriesqty.Subtext = title;

        SeriesType Series1 = new SeriesType();
        Series1.Name = "开线率";
        Series1.SeriesValues = PerST;
        Series1.XValue = XList;

        SeriesType SeriesEfficiency = new SeriesType();
        SeriesEfficiency.Name = "生产效率";
        SeriesEfficiency.SeriesValues = PerEff;
        SeriesEfficiency.XValue = XList;


        SeriesType SeriesRealUPH = new SeriesType();
        SeriesRealUPH.Name = "整体产出率";
        SeriesRealUPH.SeriesValues = PerOut;
        SeriesRealUPH.XValue = XList;

        List<SeriesType> ret = new List<SeriesType>();
        ret.Add(Seriesqty);//产出
        ret.Add(Series1);//开线率
        ret.Add(SeriesEfficiency);//生产效率
        ret.Add(SeriesRealUPH);//整体产出率

        context.Response.Write(ChangeToJson<List<SeriesType>>(ret));
        context.Response.End();
    }

    private void GetEfficiency_Process_Hour(HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        string begintime = context.Request.QueryString["BeginTime"].Trim();
        string endtime = context.Request.QueryString["EndTime"].Trim();
        List<Efficiency_Process_Hour> reteff = uph.GetEfficiency_Process_Hour( process, Convert.ToDateTime(begintime), Convert.ToDateTime(endtime));
        context.Response.Write(ChangeToJson<List<Efficiency_Process_Hour>>(reteff));
        context.Response.End();
    }
    private void GetEfficiency_Process_Hour_Echart(HttpContext context)
    {
        string process = context.Request.QueryString["Process"].Trim();
        string begintime = context.Request.QueryString["BeginTime"].Trim();
        string endtime = context.Request.QueryString["EndTime"].Trim();
        List<Efficiency_Process_Hour> reteff = uph.GetEfficiency_Process_Hour(process, Convert.ToDateTime(begintime), Convert.ToDateTime(endtime));
        string title = "区间：" + begintime + "~" + endtime;
        List<string> XList = new List<string>();//定义线条的X轴
        List<string> qtyList = new List<string>();
        List<string> PerST = new List<string>();
        List<string> PerEff = new List<string>();
        List<string> PerOut = new List<string>();


        foreach (Efficiency_Process_Hour efftable in reteff)//把X轴 和Y轴数据变为数组给Echart
        {
            DateTime xdt = Convert.ToDateTime(efftable.Dt);
            string dd = xdt.ToString("MM-dd");
            string hh = efftable.Hour.ToString().Trim().PadLeft(2, '0');
            string ddhh = dd + " " + hh + ":00";
            XList.Add(ddhh);
            qtyList.Add(efftable.Qty.ToString());
            PerST.Add(efftable.PerST.ToString());
            PerEff.Add(efftable.PerEff.ToString());
            PerOut.Add(efftable.PerOut.ToString());

        }
        SeriesType Seriesqty = new SeriesType();
        Seriesqty.Name = "产出";
        Seriesqty.SeriesValues = qtyList;
        Seriesqty.XValue = XList;
        Seriesqty.Subtext = title;

        SeriesType Series1 = new SeriesType();
        Series1.Name = "开线率";
        Series1.SeriesValues = PerST;
        Series1.XValue = XList;

        SeriesType SeriesEfficiency = new SeriesType();
        SeriesEfficiency.Name = "生产效率";
        SeriesEfficiency.SeriesValues = PerEff;
        SeriesEfficiency.XValue = XList;


        SeriesType SeriesRealUPH = new SeriesType();
        SeriesRealUPH.Name = "整体产出率";
        SeriesRealUPH.SeriesValues = PerOut;
        SeriesRealUPH.XValue = XList;

        List<SeriesType> ret = new List<SeriesType>();
        ret.Add(Seriesqty);//产出
        ret.Add(Series1);//开线率
        ret.Add(SeriesEfficiency);//生产效率
        ret.Add(SeriesRealUPH);//整体产出率

        context.Response.Write(ChangeToJson<List<SeriesType>>(ret));
        context.Response.End();
    }
    private void GetProductQty(HttpContext context)
    {

        List<ProductQty> reteff = uph.GetProductQty();
        context.Response.Write(ChangeToJson<List<ProductQty>>(reteff));
        context.Response.End();
    }
    
    

    public bool IsReusable {
        get {
            return false;
        }
    }
    private string ChangeToJson<T>(object obj)
    {
         
        string ProductAnalyzeDataJson;
        DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
        using (MemoryStream stream = new MemoryStream())
        {
            json.WriteObject(stream, obj);
             ProductAnalyzeDataJson = Encoding.UTF8.GetString(stream.ToArray());
        }
        return ProductAnalyzeDataJson.ToString();
    }
   

}