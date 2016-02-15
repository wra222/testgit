<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Print Service
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-02-01  itc207013     Create 
 2010-02-10  itc207013     Modify: ITC-1122-0083
 
 Known issues:
 --%>
<%@ WebService Language="C#" Class="SessionService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using IMES.Infrastructure;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class SessionService  : System.Web.Services.WebService {

    [WebMethod]
    public string ClearSession(string key,string type,string serviceName) {
    try
    {
        ISession isessionObj = ServiceAgent.getInstance().GetObjectByName<ISession>(WebConstant.CommonObject, serviceName);
       int sessionType = int.Parse(type);
       isessionObj.TerminateSession(key, (SessionType)sessionType);
       return "";
       }
       catch(FisException ex)
       {
          return ex.mErrmsg;
       }
       catch (Exception ex)
        {
           return ex.Message;
        }
    }
    
}

