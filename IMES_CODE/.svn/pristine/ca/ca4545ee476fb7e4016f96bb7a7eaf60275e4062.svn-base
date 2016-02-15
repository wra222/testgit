
<%@ WebService Language="C#" Class="WebServiceOfflineLcdCtPrint" %>

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
public class WebServiceOfflineLcdCtPrint : System.Web.Services.WebService
{
    IOfflineLcdCtPrint iOfflineLcdCtPrint = ServiceAgent.getInstance().GetObjectByName<IOfflineLcdCtPrint>(WebConstant.OfflineLcdCtPrint);


    [WebMethod]
    public ArrayList CheckModel(string model, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService;

        try
        {
            if (iOfflineLcdCtPrint.CheckModel(model, customer))
            {
				retLst.Add(WebConstant.SUCCESSRET);
			}
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
    public ArrayList Print(string model, string ct, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {
            ret = iOfflineLcdCtPrint.Print(model, ct, editor, station, customer, pCode, printItems);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(ret);
            
            return result; 
        }
        catch (FisException e)
        {
            result.Add(e.mErrmsg);
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }                       
    }

    [WebMethod]
    public void wfcancel(string prodid)
    {
        try
        {
            iOfflineLcdCtPrint.Cancel(prodid);
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


