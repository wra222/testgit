<%@ WebService Language="C#" Class="WebServiceCNCardReceive" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCNCardReceive : System.Web.Services.WebService
{
    ICNCardReceive iCNCardReceive = ServiceAgent.getInstance().GetObjectByName<ICNCardReceive>(WebConstant.CNCardReceiveObject);

    [WebMethod]
    public ArrayList CheckPartNo(string partNo)
    {
        ArrayList retLst = new ArrayList();
        IList<string> res = new List<string>();
        
        try
        {                        
            iCNCardReceive.CheckPartNo(partNo);                
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(partNo);                                                        
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg); 
        }
        catch (Exception ex)
        {            
            throw ex;
        }
        return retLst;
    }
    
    [WebMethod]
    public ArrayList CheckBegNo(string partNo, string begNo)
    {
        ArrayList retLst = new ArrayList();
        
        try
        {   
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(begNo);            
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg); 
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }


    [WebMethod]
    //ITC-1360-0134
    public ArrayList CheckEndNo(string partNo, string beg, string end)
    {
        ArrayList retLst = new ArrayList();

        try
        {
            string begNo = beg;
            string endNo = end;            
            long count;
            string tmp;
            long ibeg5 = Convert.ToInt64(begNo.Substring(8));
            long iend5 = Convert.ToInt64(endNo.Substring(8));

            if (ibeg5 >= iend5)
            {                
                tmp = begNo;
                begNo = endNo;
                endNo = tmp;             
            }
            long ibeg8 = Convert.ToInt64(begNo.Substring(8));
            long iend8 = Convert.ToInt64(endNo.Substring(8));   
            if (ibeg8 >= iend8)
            {
                count = ibeg8 - iend8 + 1;
            }
            else
            {
                count = iend8 - ibeg8 + 1;
            }
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(begNo);
            retLst.Add(endNo);
            retLst.Add(Convert.ToString(count));
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }


    [WebMethod]
    public ArrayList Save_Click(string partNo, string begNo, string endNo, string station, string editor, string pdline, string customer)
    {
        ArrayList retLst = new ArrayList();
        IList<string> res = new List<string>();
        try
        {
            iCNCardReceive.Save(partNo, begNo, endNo, station, editor, pdline, customer);            
            retLst.Add(WebConstant.SUCCESSRET);            
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
    }    
}
