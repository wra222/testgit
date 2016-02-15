<%@ WebService Language="C#" Class="WebServicePrDelete" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePrDelete : System.Web.Services.WebService
{
    IPrDelete iPrDelete = ServiceAgent.getInstance().GetObjectByName<IPrDelete>(WebConstant.PrDeleteObject);

    [WebMethod]
    public ArrayList FindInfoForPrSN(string prsn)
    {
        ArrayList retLst = new ArrayList();
        IList<string> res = new List<string>();
        
        try
        {
            res = iPrDelete.FindInfoForPrSN(prsn);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(res);

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
    public ArrayList DelPrSN(string inputSN, string productID, string productSN, string checkProductSN, string partNo,
                                string model, string station, string editor, string pdline, string customer)
    {
        ArrayList retLst = new ArrayList();
        
        try
        {
            iPrDelete.DelPrSN(inputSN, productID, productSN, checkProductSN, partNo, model, station, editor, pdline, customer);
            retLst.Add(WebConstant.SUCCESSRET);
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
}
