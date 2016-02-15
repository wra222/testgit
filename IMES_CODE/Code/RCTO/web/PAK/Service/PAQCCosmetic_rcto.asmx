<%@ WebService Language="C#" Class="WebServicePAQCCosmetic" %>


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
public class WebServicePAQCCosmetic: System.Web.Services.WebService
{
    IPAQCCosmetic_rcto iPAQCCosmetic = ServiceAgent.getInstance().GetObjectByName<IPAQCCosmetic_rcto>(WebConstant.PAQCCosmetic_rctoObject);

    [WebMethod]
    public ArrayList ProcessInput(string prodid, string flag, string pdline, string station, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService = new ArrayList();
        
        try
        {
            retService = iPAQCCosmetic.ProcessInput(prodid, flag, pdline, station, editor, customer);       
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retService);
            retLst.Add(flag);            
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
    public ArrayList ProcessDefect(string code)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serviceLst = new ArrayList();

        try
        {
            serviceLst = iPAQCCosmetic.GetDefectInfo(code);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(code);
            retLst.Add(serviceLst);
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
    public ArrayList save(string key, IList<string> list)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serLst = new ArrayList();
        try
        {
            serLst = iPAQCCosmetic.Save(key, list);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(key);
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
            iPAQCCosmetic.Cancel(prodid);
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
