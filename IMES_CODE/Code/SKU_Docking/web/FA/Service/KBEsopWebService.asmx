<%@ WebService Language="C#" Class="KBEsopWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.StationIntf;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class KBEsopWebService  : System.Web.Services.WebService {

    [WebMethod]
    public ArrayList InputKey(string key, string keyType, string editor, string line, string station, string customer)
    {
        ArrayList result = new ArrayList();
        try
        {
            IKBEsop currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IKBEsop>(com.inventec.iMESWEB.WebConstant.KBEsopObject);
            result = currentService.InputKey(key, editor, line, station, customer);
            result.Insert(0,WebConstant.SUCCESSRET);
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

