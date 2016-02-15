<%@ WebService Language="C#" Class="PalletCollectionWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.StationIntf;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Collections;
using System.Collections.Generic;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PalletCollectionWebService : System.Web.Services.WebService
{

    [WebMethod]
    public ArrayList InputCarton(string carton, string floor, string editor, string line, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList result = new ArrayList();
        try
        {
            IPalletCollection currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IPalletCollection>(com.inventec.iMESWEB.WebConstant.PalletCollectionObject);
            ArrayList dtList = currentService.InputCarton(carton,floor, editor, line, station, customer,printItems);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(dtList);
            
        }
        catch (FisException fe)
        {
            result.Add(fe.Message);
        }
        catch (Exception e)
        {
            result.Add(e.Message);
        }
        return result;
    }

    [WebMethod]
    public ArrayList Reprint(string carton, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            IPalletCollection currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IPalletCollection>(com.inventec.iMESWEB.WebConstant.PalletCollectionObject);
            ret = currentService.Reprint(carton, reason, editor, line, station, customer, printItems);

            ret.Insert(0,WebConstant.SUCCESSRET);
            return ret;
        }
        catch (FisException ex)
        {
            ret.Add(ex.Message);
        }
        catch (Exception ex)
        {
            ret.Add(ex.Message);
        }
        return ret;
    }

}

