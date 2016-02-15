<%@ WebService Language="C#" Class="WebServiceWHInspection" %>


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
public class WebServiceWHInspection : System.Web.Services.WebService
{
    //ICheckDoorSn iCheckDoorSn = ServiceAgent.getInstance().GetObjectByName<ICheckDoorSn>(WebConstant.ICheckDoorSn);
    IWHInspection iWHInspection = ServiceAgent.getInstance().GetObjectByName<IWHInspection>(WebConstant.WHInspectionObject);
    [WebMethod]
    public ArrayList InputMaterialCT(string materialct, string materialtype, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList ar = iWHInspection.InputMaterialCT(materialct, materialtype , pdLine, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(ar[0].ToString());
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
  
    //[WebMethod]
    //public ArrayList Save(string custsn)
    //{
    //    ArrayList ret = new ArrayList();

    //    try
    //    {
    //        iCheckDoorSn.Save(custsn);
    //        ret.Add(WebConstant.SUCCESSRET);
    //        return ret;
    //    }
    //    catch (FisException ex)
    //    {
    //        throw new Exception(ex.mErrmsg);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
   
    [WebMethod]
    public void Cancel(string key)
    {
        try
        {
            iWHInspection.Cancel(key);
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
