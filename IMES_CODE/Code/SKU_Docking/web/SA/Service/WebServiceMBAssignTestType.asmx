<%@ WebService Language="C#" Class="WebServiceMBAssignTestType" %>

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
public class WebServiceMBAssignTestType  : System.Web.Services.WebService
{
    IMBAssignTestType iMBAssignType = ServiceAgent.getInstance().GetObjectByName<IMBAssignTestType>(WebConstant.MBAssignTestType);

    [WebMethod]
    public ArrayList mbsnoSFC(string pdLine, string MB_SNo, string editor, string stationId, string customerId)
    {
        ArrayList re = new ArrayList();
        try
        {
            re = iMBAssignType.MBCombineInputMBSN(pdLine, MB_SNo, editor, stationId, customerId);
            return re;
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
    public string Cancel(string MB_SNo, string stationId)
    {
        try
        {
            iMBAssignType.CancelMB(MB_SNo);
            return (WebConstant.SUCCESSRET);
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

