<%@ WebService Language="C#" Class="DTPalletControlWebService" %>

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
public class DTPalletControlWebService : System.Web.Services.WebService
{

    [WebMethod]
    public ArrayList DT(string palletNo, string editor, string line, string station, string customer)
    {
        ArrayList result = new ArrayList();
        try
        {
            IDTPalletControl currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IDTPalletControl>(com.inventec.iMESWEB.WebConstant.DTPalletControlObject);
            IList<WhPltLogInfo> dtList = currentService.DT(palletNo, editor, line, station, customer);
            result.Add(dtList);
            result.Add(WebConstant.SUCCESSRET);
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
    public ArrayList Query(string palletNo, string from, string to)
    {
        ArrayList result = new ArrayList();
        try
        {
            IDTPalletControl currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IDTPalletControl>(com.inventec.iMESWEB.WebConstant.DTPalletControlObject);
            IList<WhPltLogInfo> dtList = currentService.Query(palletNo, from, to);
            result.Add(dtList);
            result.Add(WebConstant.SUCCESSRET);
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

}

