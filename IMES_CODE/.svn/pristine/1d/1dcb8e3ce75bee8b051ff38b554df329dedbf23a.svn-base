<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: 
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 Known issues:
 --%>
 
<%@ WebService Language="C#" Class="WebserviceUnpackOfflinePizza" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebserviceUnpackOfflinePizza: System.Web.Services.WebService
{

    IUnpackOfflinePizza iUnpack = ServiceAgent.getInstance().GetObjectByName<IUnpackOfflinePizza>(WebConstant.UnpackOfflinePizzaObject); 

    [WebMethod]
    public string Unpack(string pizzaId, string editor, string stationId, string customer)
    {
        string line = string.Empty;

        try
        {
            iUnpack.Unpack(pizzaId, line, editor, stationId, customer);
            return WebConstant.SUCCESSRET;
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
    public void cancel(string productId)
    {
        try
        {
            iUnpack.Cancel(productId);
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

