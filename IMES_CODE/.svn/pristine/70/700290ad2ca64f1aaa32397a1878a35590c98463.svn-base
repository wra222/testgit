
<%@ WebService Language="C#" Class="WebServiceTravelCardRePrint" %>

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
public class WebServiceTravelCardRePrint : System.Web.Services.WebService
{
    ITravelCardPrint2012 iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrint2012>(WebConstant.TravelCardPrint2012Object);
    private const string SUCCESS = "SUCCESSRET";
    [WebMethod]
    public ArrayList PrintTravelCard(string prodid, string reason, string editor,  string station, string customer, string pCode, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {
       
            ret = iTravelCardPrint.ReprintTravelCard(prodid, reason, editor, station, customer, pCode, printItems);
            result.Add(SUCCESS);
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
    public string GetProductID(string LCMCT)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {

            return iTravelCardPrint.GetProductID(LCMCT);
            //result.Add(SUCCESS);
            //result.Add(ret);

            //return result;
        }
        catch (FisException e)
        {
            //result.Add(e.mErrmsg);
            return e.mErrmsg;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}


