<%@ WebService Language="C#" Class="WebServiceCOARemoval" %>


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

public class WebServiceCOARemoval : System.Web.Services.WebService
{
    ICOARemoval iCOARemoval = ServiceAgent.getInstance().GetObjectByName<ICOARemoval>(WebConstant.COARemovalObject);
    
    [WebMethod]
    public string HelloWorld() {
        //throw new FieldAccessException("errormess");
        return "Hello World3";
    }


    [WebMethod]
    public ArrayList inputCOANumber(/*string sessionKey, */string COANumber, string pdLine, string prodId, string editor, string station, string customer, string action, string cause)
    {
        ArrayList retLst = new ArrayList();
#if false        
        retLst.Add("OK!");
        retLst.Add(COANumber);
        //Rework node = new Rework();
        //node.ReworkCode = "test";
        //node.Cdt = DateTime.Now;
        //node.Udt = DateTime.UtcNow;
        //retLst.Add(node);
        return retLst;
#else        
        try
        {
            retLst = iCOARemoval.InputCOANumber(/*sessionKey, */COANumber, pdLine, prodId, editor, station, customer, action, cause);
            //retLst.Add(WebConstant.SUCCESSRET);
            //retLst.Add(prodId);
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
#endif
    }

    [WebMethod]
    public ArrayList saveProc(string pdLine, string prodId, string editor, string stationId, string customerId, string[] COANoList, string[] ActionList, string[] CauseList, string action, string cause)
        //saveProc(string[] defectInTable)//string sessionKey)//string COANumber, string pdLine, string prodId, string editor, string stationId, string customerId)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            iCOARemoval.SaveProc(pdLine, prodId, editor, stationId, customerId, COANoList, ActionList, CauseList, action, cause); //COANumber, pdLine, prodId, editor, stationId, customerId);
            retLst.Add(WebConstant.SUCCESSRET);
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
    }
    
    [WebMethod]
    public ArrayList cancel(string MB)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iCOARemoval.Cancel(MB);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(MB);

        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
  
}

