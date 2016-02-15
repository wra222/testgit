
<%@ WebService Language="C#" Class="WebServiceSKULabelPrint" %>

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
public class WebServiceSKULabelPrint : System.Web.Services.WebService
{
    ISKULabelPrint iSKULabelReprint = ServiceAgent.getInstance().GetObjectByName<ISKULabelPrint>(WebConstant.SKULabelPrintObject);


    [WebMethod]
    public ArrayList GetPCBID(string prodid)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService;

        try
        {
            retService = iSKULabelReprint.GetPCBID(prodid);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retService);
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
    public ArrayList Print(string prodid, string PCBID, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {
            ret = iSKULabelReprint.Print(prodid, PCBID, reason, editor, station, customer, pCode, printItems);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(ret);
            
            return result; 
        }
        catch (FisException e)
        {
            result.Add(e.mErrmsg);
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }                       
    }

    [WebMethod]
    public void wfcancel(string prodid)
    {
        try
        {
            iSKULabelReprint.Cancel(prodid);
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
    public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList arr = iSKULabelReprint.RePrint(sn, reason, line, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]);
            return ret;
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


