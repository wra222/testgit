<%@ WebService Language="C#" Class="WebServiceTravelCardWithCollectKP" %>


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
public class WebServiceTravelCardWithCollectKP : System.Web.Services.WebService
{

    ITravelCardWithCollectKP iTravelCardWithCollectKP = ServiceAgent.getInstance().GetObjectByName<ITravelCardWithCollectKP>(WebConstant.ITravelCardWithCollectKP);

    [WebMethod]
    public ArrayList InputCT(string ctNo, string pdLine, string editor, string stationId, string customerId, string model, IList<PrintItem> printItemLst)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            ArrayList retLst2 = iTravelCardWithCollectKP.InputCT(ctNo, pdLine, editor, stationId, customerId, model, printItemLst);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retLst2[0]);
            retLst.Add(retLst2[1]);
            retLst.Add(retLst2[2]);
            
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;


    }
    
    
    
    

   
    [WebMethod]
    public ArrayList PrintCustsnLabel(IList<PrintItem> printItemLst,string custsn)
    {

        ArrayList ret = new ArrayList();
        try
        {
            IList<PrintItem> retLst = iTravelCardWithCollectKP.PrintCustsnLabel(printItemLst, custsn);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
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
 
    [WebMethod]
    public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
           
            ArrayList arr = iTravelCardWithCollectKP.RePrint(sn, reason, line, editor, station, customer, printItems);
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
    public ArrayList Cancel(string prodId, string station)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iTravelCardWithCollectKP.Cancel(prodId);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(prodId);

        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);

        }
        catch (Exception ex)
        {
            throw ex; ;
        }
        return ret;
    }
  
}


