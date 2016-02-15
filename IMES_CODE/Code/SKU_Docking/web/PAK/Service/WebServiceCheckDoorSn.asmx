<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceCheckDoorSn" %>


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
public class WebServiceCheckDoorSn : System.Web.Services.WebService
{
    ICheckDoorSn iCheckDoorSn = ServiceAgent.getInstance().GetObjectByName<ICheckDoorSn>(WebConstant.ICheckDoorSn);

    [WebMethod]
    public ArrayList InputCustSN(string custsn, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        
        try
        {

            ArrayList ar = iCheckDoorSn.InputCustSN(custsn, pdLine, editor, stationId, customerId);
            if (ar.Count != 0)
            {
                ret.Add(WebConstant.SUCCESSRET);
                //ret.Add(ar[0].ToString());
                for (int i = 0; i < ar.Count; i++)
                {
                    ret.Add(ar[i].ToString());
                }
            }
            else
            {
                ret.Add("Not Combine");
            }
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
    public ArrayList Save(string custsn)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iCheckDoorSn.Save(custsn);
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
    public void Cancel(string key)
    {
        try
        {
            iCheckDoorSn.Cancel(key);
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
