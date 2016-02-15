<%@ WebService Language="C#" Class="WebServiceRCTOLabelPrint" %>


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
public class WebServiceRCTOLabelPrint: System.Web.Services.WebService
{
    IRCTOLabelPrint iRCTOLabelPrint = ServiceAgent.getInstance().GetObjectByName<IRCTOLabelPrint>(WebConstant.RCTOLabelPrintObject);

    [WebMethod]
    public ArrayList ProcessProd(string prodid, string existProd, string pdline, string station, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService = new ArrayList();
        
        try
        {
            retService = iRCTOLabelPrint.ProcessProd(prodid, existProd, pdline, station, editor, customer);       
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
    public ArrayList ProcessMB(string key, string mbsn, IList<PrintItem> printItemLst)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serLst = new ArrayList();
        try
        {
            serLst = iRCTOLabelPrint.ProcessMB(key, mbsn, printItemLst);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(mbsn);
            retLst.Add(serLst);
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
    public void wfcancel(string prodid)
    {
        try
        {
            iRCTOLabelPrint.Cancel(prodid);
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
            ArrayList arr = iRCTOLabelPrint.RePrint(sn, reason, line, editor, station, customer, printItems);
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
