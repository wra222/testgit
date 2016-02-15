<%@ WebService Language="C#" Class="ShipToCartonLabel" %>


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
public class ShipToCartonLabel : System.Web.Services.WebService
{
    IShipToCartonLabel iShipToCartonLabel =
                        ServiceAgent.getInstance().GetObjectByName<IShipToCartonLabel>(WebConstant.ShipToCartonLabelObject);
    
    [WebMethod]
    public ArrayList InputProcess(string inputData, int type, string key, IList<PrintItem> printItemLst, string pdline, string station, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serLst = new ArrayList();

        try
        {                        
            serLst = iShipToCartonLabel.InputProcess(inputData, type, key, printItemLst, pdline, station, editor, customer);

            retLst.Add(WebConstant.SUCCESSRET);
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
    public ArrayList ClearUI(string none)
    {
        ArrayList retLst = new ArrayList();

        retLst.Add(WebConstant.SUCCESSRET);
        return retLst;
    }

    [WebMethod]
    public ArrayList ServiceChangeLabel(string key)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serLst = new ArrayList();

        try
        {
            serLst = iShipToCartonLabel.ChangePrintLabel(key);
            retLst.Add(WebConstant.SUCCESSRET);
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
    public ArrayList Reprint(string prodid, int type, string editor, string station, string customer, string pCode, string reason, IList<PrintItem> printItems)
    {
        ArrayList serRet = new ArrayList();
        ArrayList ret = new ArrayList();
        try
        {

            serRet = iShipToCartonLabel.Reprint(prodid, type, editor, station, customer, reason, pCode, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(serRet);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            return ret;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    [WebMethod]
    public ArrayList ServiceReprintChangeLabel(string key)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serLst = new ArrayList();

        try
        {
            serLst = iShipToCartonLabel.ChangeRePrintLabel(key);
            retLst.Add(WebConstant.SUCCESSRET);
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
}
