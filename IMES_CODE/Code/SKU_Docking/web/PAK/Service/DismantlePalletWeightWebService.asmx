<%@ WebService Language="C#" Class="DismantlePalletWeightWebService" %>

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
public class DismantlePalletWeightWebService : System.Web.Services.WebService
{

    [WebMethod]
    public ArrayList Dismantle(string palletOrDn, string editor, string line, string station, string customer)
    {
        ArrayList result = new ArrayList();
        try
        {
            IDismantlePalletWeight currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IDismantlePalletWeight>(com.inventec.iMESWEB.WebConstant.DismantlePalletWeightObject);
            IList<DNPalletWeight> weightList = currentService.Dismantle(palletOrDn, editor, line, station, customer);
            result.Add(weightList);
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
    public ArrayList Query(string palletOrDn)
    {
        ArrayList result = new ArrayList();
        try
        {
            IDismantlePalletWeight currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IDismantlePalletWeight>(com.inventec.iMESWEB.WebConstant.DismantlePalletWeightObject);
            IList<DNPalletWeight> weightList = currentService.Query(palletOrDn);
            result.Add(weightList);
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

