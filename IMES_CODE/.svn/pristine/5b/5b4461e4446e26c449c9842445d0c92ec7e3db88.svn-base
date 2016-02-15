<%@ WebService Language="C#" Class="WebServiceUpdateConsolidate" %>


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
public class WebServiceUpdateConsolidate : System.Web.Services.WebService
{
    IUpdateConsolidate iUpdateConsolidate = ServiceAgent.getInstance().GetObjectByName<IUpdateConsolidate>(WebConstant.UpdateConsolidateObject);

    [WebMethod]
    public ArrayList Update(string pdline, string station, string editor, string customer, string consolidate, string actqty)
    {
        ArrayList retService = new ArrayList();
        ArrayList ret = new ArrayList();
        
        try
        {
            retService = iUpdateConsolidate.Update(pdline, station, editor, customer, consolidate, actqty);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retService);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            return ret;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
