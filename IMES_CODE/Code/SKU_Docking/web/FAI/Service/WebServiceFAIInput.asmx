<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* Known issues:
* TODO：
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceFAIInput" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;


//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceFAIInput : System.Web.Services.WebService
{
    IFAIInput iFAIInput = ServiceAgent.getInstance().GetObjectByName<IFAIInput>(WebConstant.FAIInputObject);

    [WebMethod]
    public ArrayList checkProdId(string custsn, string pdline, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            ArrayList lst = iFAIInput.CheckProdId(custsn, pdline, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
			ret.Add(lst[0]);
			ret.Add(lst[1]);
			ret.Add(lst[2]);
			ret.Add(lst[3]);
            return ret;
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
    public ArrayList Save(string custsn, string pdline, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iFAIInput.Save(custsn, pdline, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
            return ret;
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
    public string Cancel(string key, string station)
    {
        try
        {
            iFAIInput.Cancel(key);

            return "";

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
