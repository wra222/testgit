
<%@ WebService Language="C#" Class="WebServiceRULabelPrint" %>

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
public class WebServiceRULabelPrint : System.Web.Services.WebService
{
    IRULabelPrint iRULabelReprint = ServiceAgent.getInstance().GetObjectByName<IRULabelPrint>(WebConstant.RULabelPrintObject);


    [WebMethod]
    public ArrayList CheckPalletNo(string palletNo)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService;

        try
        {
            retService = iRULabelReprint.CheckPalletNo(palletNo);
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
    public ArrayList Print(string palletNo, string RUNoQty, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {
            ret = iRULabelReprint.Print(palletNo, RUNoQty, editor, station, customer, pCode, printItems);
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
    public void wfcancel(string palletNo)
    {
        try
        {
            iRULabelReprint.Cancel(palletNo);
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
    public ArrayList CheckRePrintPalletNo(string palletNo)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService;

        try
        {
            retService = iRULabelReprint.CheckRePrintPalletNo(palletNo);
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
    public ArrayList RePrint(string palletNo, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList arr = iRULabelReprint.RePrint(palletNo, line, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]);
            ret.Add(palletNo);
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


