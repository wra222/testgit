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
 
<%@ WebService Language="C#" Class="WebServiceFAIOutput" %>


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
public class WebServiceFAIOutput : System.Web.Services.WebService
{
    IFAIOutput iFAIOutput = ServiceAgent.getInstance().GetObjectByName<IFAIOutput>(WebConstant.FAIOutputObject);

    [WebMethod]
    public ArrayList checkProdId(string custsn, string pdline, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            ArrayList lst = iFAIOutput.CheckProdId(custsn, pdline, editor, stationId, customerId);
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
    public ArrayList Save(string custsn, IList<string> defectList, string pdline, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            ArrayList lst = iFAIOutput.Save(custsn, defectList, pdline, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
			ret.Add(lst[0]);
			ret.Add(lst[1]);
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
    public ArrayList CheckDefect(string custsn, string defectCode, string pdline, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            ArrayList lst = iFAIOutput.CheckDefect(custsn, defectCode, pdline, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
			ret.Add(lst[0]);
			ret.Add(lst[1]);
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
            iFAIOutput.Cancel(key);

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
