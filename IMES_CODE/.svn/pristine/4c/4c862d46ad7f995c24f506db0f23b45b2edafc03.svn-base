<%@ WebService Language="C#" Class="UpdateCacheWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using com.inventec.iMESWEB;
using IMES.Infrastructure;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class UpdateCacheWebService  : System.Web.Services.WebService {
    [WebMethod]
    public string UpdateCache(string serviceName) 
    {
        try
        {
            ICache iCacheObj = ServiceAgent.getInstance().GetObjectByName<ICache>(WebConstant.CommonObject, serviceName);
            iCacheObj.UpdateCacheNow();
            return "";
        }
        catch (FisException ex)
        {
            return ex.mErrmsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    [WebMethod]
    public string UpdatePartMatch(string serviceName)
    {
        try
        {
            ICache iCacheObj = ServiceAgent.getInstance().GetObjectByName<ICache>(WebConstant.CommonObject, serviceName);
            iCacheObj.RefreshPartMatchAssembly();
            return "";
        }
        catch (FisException ex)
        {
            return ex.mErrmsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    [WebMethod]
    public string ClearCache(string serviceName)
    {
        try
        {
            ICache iCacheObj = ServiceAgent.getInstance().GetObjectByName<ICache>(WebConstant.CommonObject, serviceName);
            iCacheObj.RefreshAllCache();
            return "";
        }
        catch (FisException ex)
        {
            return ex.mErrmsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}

