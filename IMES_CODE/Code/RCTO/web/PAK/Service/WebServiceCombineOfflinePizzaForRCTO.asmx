<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombineOfflinePizzaForRCTO
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceCombineOfflinePizzaForRCTO" %>


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
public class WebServiceCombineOfflinePizzaForRCTO : System.Web.Services.WebService
{
	ICombineOfflinePizzaForRCTO iCombineOfflinePizzaForRCTO = ServiceAgent.getInstance().GetObjectByName<ICombineOfflinePizzaForRCTO>(WebConstant.CombineOfflinePizzaForRCTO);

    [WebMethod]
    public ArrayList InputCartonId(string cartonSN, IList<PrintItem> printItems, string pdLine, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList tmpList = iCombineOfflinePizzaForRCTO.InputCartonId(cartonSN, printItems, pdLine, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//prodList.Count
            ret.Add(tmpList[1]);//bomItemList
            ret.Add(tmpList[2]);//IList<PrintItem>
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
    public ArrayList InputPizzaId(string cartonSN, string pizzaId, string pdLine, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList tmpList = iCombineOfflinePizzaForRCTO.InputPizzaId(cartonSN, pizzaId, pdLine, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//Finish?
			ret.Add(tmpList[1]);//idxPizza
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
    public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList arr = iCombineOfflinePizzaForRCTO.RePrint(sn, reason, line, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]);
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
    public void cancel(string productId)
    {
        try
        {
            iCombineOfflinePizzaForRCTO.Cancel(productId);
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
