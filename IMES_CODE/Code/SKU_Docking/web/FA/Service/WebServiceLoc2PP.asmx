<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: 2PP Input
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceLoc2PP" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceLoc2PP : System.Web.Services.WebService
{
    ILoc2PP iLoc2PP = ServiceAgent.getInstance().GetObjectByName<ILoc2PP>(WebConstant.Loc2PPObject);
    
    [WebMethod]
    public string ChkProdId(string prodId)
    {
        try
        {
            return iLoc2PP.ChkProdId(prodId);
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
    public string inputLoc(string prodId, string loc, string editor, string station, string customer)
    {
        try
        {
            string line = string.Empty;
            return iLoc2PP.InputLoc(prodId, loc, line, editor, station, customer);
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
    public void cancel(string prodId)
    {
        try
        {
            iLoc2PP.cancel(prodId);
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
