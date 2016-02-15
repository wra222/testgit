<%@ WebService Language="C#" Class="WebServiceVGALabelPrint" %>
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
using log4net;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceVGALabelPrint : System.Web.Services.WebService
{
    IVGALabelPrint iVGALabelPrint = ServiceAgent.getInstance().GetObjectByName<IVGALabelPrint>(WebConstant.VGALabelPrintObject);

    [WebMethod]
    public ArrayList print(string pdLine, string month, string mbCode, string mo, string qty, string datecode, string editor, string station, string customer, string _111, string factor, IList<PrintItem> printItems)
    {
        IList<String> snLst = new List<String>();
        IList<PrintItem> retPrintItemLst = null;
       // ArrayList retPrintItemLstArray = new ArrayList();
        ArrayList retLst = new ArrayList();
        
        try
        {
            retPrintItemLst = iVGALabelPrint.Print(pdLine, Convert.ToBoolean(month), mbCode, mo, int.Parse(qty), datecode, editor, station, customer, out snLst, _111, factor, printItems);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retPrintItemLst);
            retLst.Add(snLst);
            //retLst.Add(snLst[0]);
            //retLst.Add(snLst[1]);
            //retLst.Add(pdLine);
            //retLst.Add(_111);
            //retLst.Add(snLst);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            retLst.Add(ex.Message);
            //throw ex;
        }
        return retLst;
    }

    [WebMethod]
    public ArrayList RePrint(string mbSno, string customerId, string reason, string editor, string stationId, IList<PrintItem> printItems)
    {
        //List<string> lstMBno = null;
        //List<string> lstPartNo = null;
        IList<PrintItem> retPrintItemLst = null;
        ArrayList retLst = new ArrayList();
        try
        {
            retPrintItemLst = iVGALabelPrint.RePrint(mbSno, customerId, reason, editor, stationId, printItems);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(mbSno);
            retLst.Add(retPrintItemLst);
            //retLst.Add(lstMBno);
            //retLst.Add(lstPartNo);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            retLst.Add(ex.Message);
            //throw ex;
        }
        return retLst;        
    }


    [WebMethod]
    public void cancel(string mo)
    {
        try
        {
            iVGALabelPrint.Cancel(mo);
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
