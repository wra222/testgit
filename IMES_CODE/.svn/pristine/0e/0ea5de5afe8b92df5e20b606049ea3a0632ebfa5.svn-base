<%@ WebService Language="C#" Class="WebServicePilotRunMO" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
//using IMES.FisObject.Common.Model;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePilotRunMO : System.Web.Services.WebService
{
    private IReleaseProductIDHold iReleaseProductIDHold = ServiceAgent.getInstance().GetObjectByName<IReleaseProductIDHold>(WebConstant.ReleaseProductIDHoldObject);
    private IPilotRunMO iPilotRunMO = ServiceAgent.getInstance().GetObjectByName<IPilotRunMO>(WebConstant.PilotRunMOObject);
    

    
    
    [WebMethod]
    public void cancel(string guid)
    {
        try
        {
            iPilotRunMO.Cancel(guid);
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
    public  ArrayList Print(string pilotrunmo, string qty, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            ArrayList retPrintItemLst = iPilotRunMO.Print(pilotrunmo, qty, line, editor, station, customer, printItems);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retPrintItemLst);
            retLst.Add(pilotrunmo);
            retLst.Add(qty);
            
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }
}