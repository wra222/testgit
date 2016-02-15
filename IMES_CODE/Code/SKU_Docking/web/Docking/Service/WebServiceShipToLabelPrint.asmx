
<%@ WebService Language="C#" Class="WebServiceShipToLabelPrint" %>

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
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceShipToLabelPrint : System.Web.Services.WebService
{
    IShipToLabelPrintForDocking iShipToLabelPrint = ServiceAgent.getInstance().GetObjectByName<IShipToLabelPrintForDocking>(WebConstant.ShipToLabelPrintForDocking);
    
    [WebMethod]
    public ArrayList ShipToPrint(string pdline, string editor, string station, string customer, string id, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {

            ret = iShipToLabelPrint.Print(pdline, editor, station, customer, id, printItems);
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
    public ArrayList ShipToRePrint(string pdline, string editor, string station, string customer, string id, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {

            ret = iShipToLabelPrint.RePrint(pdline, editor, station, customer, id, printItems);
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
}


