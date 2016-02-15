<%@ WebService Language="C#" Class="WebServiceIECLabelPrint" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceIECLabelPrint : System.Web.Services.WebService
{
    IIECLabelPrint iIECLabelPrint = ServiceAgent.getInstance().GetObjectByName<IIECLabelPrint>(WebConstant.IECLabelPrintObject);

    [WebMethod]
    public ArrayList print(string dateCode, string config, string rev, string qty, string line, string editor, string station, string customer, List<PrintItem> printItems)
    {
        ArrayList retLst = new ArrayList();
        ArrayList ret = new ArrayList();
       //List<PrintItem> printList = new List<PrintItem>();
        try
        {
            ret = iIECLabelPrint.Print(dateCode, config, rev, qty, line, editor, station, customer, printItems);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(ret);
            retLst.Add(rev);
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
    public ArrayList Reprint(string vendorCT, string reason, string line, string editor, string station, string customer, List<PrintItem> printItems)
    {
        ArrayList retLst = new ArrayList();
        //IList<PrintItem> printList = new List<PrintItem>();
        try
        {
            var printList = iIECLabelPrint.ReprintLabel(vendorCT, reason, line, editor, station, customer, printItems);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(printList);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
            //return retLst;
        }
        catch (Exception ex)
        {
            retLst.Add(ex.Message);
            //throw ex;
        }

        return retLst;
    }
}
