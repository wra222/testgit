<%@ WebService Language="C#" Class="WebServiceCombineCartonInDN_BIRCH" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.BSamIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
//using IMES.FisObject.Common.Model;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCombineCartonInDN_BIRCH : System.Web.Services.WebService
{
    private ICombineCartonInDN_BIRCH iCombineCartonInDN = ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDN_BIRCH>(WebConstant.ICombineCartonInDN_BIRCH);
     
    [WebMethod]
    public ArrayList InputCustSn(string firstSN, string custsn, IList<PrintItem> items)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
       //    iCombineCartonInDN = ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDN>(WebConstant.ICombineCartonInDN);
           tmpList = iCombineCartonInDN.InputCustSn(firstSN, custsn);
           retList.Add(WebConstant.SUCCESSRET);
           retList.Add(tmpList);

           return retList;
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public ArrayList Save(string firstSN, string cartonSn,string editor, IList<PrintItem> items)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
  //         iCombineCartonInDN = ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDN>(WebConstant.ICombineCartonInDN);
            tmpList = iCombineCartonInDN.Save(firstSN, cartonSn,editor, items);
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]); //dn
            retList.Add(tmpList[1]); // LocMsg
            retList.Add(tmpList[2]); //createPDF
            retList.Add(tmpList[3]); //templatename
            retList.Add(tmpList[4]); //PrintItem
            //SUCCESSRET,db,locmsg,createpdf,templatename,PrintItem
            return retList;
        
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public ArrayList ReprintCartonLabel(string sn, string editor, string station, string customer, string reason, IList<PrintItem> printItems)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
         
            tmpList = iCombineCartonInDN.RePrintCartonLabel(sn, editor, station, customer, reason, printItems);
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]); // custsn
            retList.Add(tmpList[1]); // dn
            retList.Add(tmpList[2]); //CartonSN
            retList.Add(tmpList[3]); //templatename
            retList.Add(tmpList[4]); //line
            retList.Add(tmpList[5]);//flag
            retList.Add(tmpList[6]);//print item
            return retList;
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    
    }
    [WebMethod]
    public void Cancel(string key, string cartonSn,string editor)
    {
        try
        {
       //     iCombineCartonInDN = ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDN>(WebConstant.ICombineCartonInDN);
            iCombineCartonInDN.Cancel(key, cartonSn, editor);
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } 
}