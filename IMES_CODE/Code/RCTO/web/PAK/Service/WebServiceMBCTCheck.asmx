<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
* Known issues:
* TODO：
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceMBCTCheck" %>


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
public class WebServiceMBCTCheck : System.Web.Services.WebService
{
    IMBCTCheck iMBCTCheck = ServiceAgent.getInstance().GetObjectByName<IMBCTCheck>(WebConstant.MBCTCheckObject);

    [WebMethod]
    public ArrayList InputProduct(string product, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();

        try
        {
            IList<string> list = iMBCTCheck.InputProduct(product, line, editor, station, customer);
            //if ("" == pcbno)
            //    throw new Exception("该MBCT不存在，请确认");
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(list);
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
    public ArrayList Save(string product, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            string result = iMBCTCheck.Save(product, pdLine, editor, stationId, customerId);
            if (result != "OK")
				throw new Exception("检查Fail,请核实！");
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
            iMBCTCheck.Cancel(key);

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
