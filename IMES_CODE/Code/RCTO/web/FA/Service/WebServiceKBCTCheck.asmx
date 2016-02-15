<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for KB CT Check Page
 * UI:CI-MES12-SPEC-FA-UI KB CT Check.docx –2012/6/12 
 * UC:CI-MES12-SPEC-FA-UC KB CT Check.docx –2012/6/12            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-6-12   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceKBCTCheck" %>


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
public class WebServiceKBCTCheck : System.Web.Services.WebService
{
    IKBCTCheck iKBCTCheck = ServiceAgent.getInstance().GetObjectByName<IKBCTCheck>(WebConstant.KBCTCheckObject);

    [WebMethod]
    public ArrayList checkProdId(string prodid, string pdline, string editor, string stationId, string customerId)
    {
		return checkProdId(prodid, pdline, editor, stationId, customerId, "");
	}
	
	[WebMethod]
    public ArrayList checkProdId(string prodid, string pdline, string editor, string stationId, string customerId, string CheckModel)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            iKBCTCheck.CheckProdId(prodid, pdline, editor, stationId, customerId, CheckModel);
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
    public ArrayList checkAndSave(string prodid, string kbct, string pdline, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iKBCTCheck.CheckAndSave(prodid, kbct, pdline, editor, stationId, customerId);
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
            iKBCTCheck.Cancel(key);

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
