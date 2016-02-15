
<%@ WebService Language="C#" Class="WebServiceCreateProductandCombineLCM" %>


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
public class WebServiceCreateProductandCombineLCM : System.Web.Services.WebService
{
    ICreateProductandCombineLCM iCreateProductandCombineLCM = ServiceAgent.getInstance().GetObjectByName<ICreateProductandCombineLCM>(WebConstant.CreateProductandCombineLCM);

    [WebMethod]
    public ArrayList CreateProductID(string pdLine, string model, string mo, string editor, string station, string customer, string family, string moPrefix)
    {
        ArrayList retLst = new ArrayList();
        
        IList<String> snLst = null;
        try
        {
            snLst = iCreateProductandCombineLCM.CreateProductID(pdLine, model, mo, editor, station, customer, family, moPrefix);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(model);
            retLst.Add(snLst);
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
    public ArrayList inputPPID(string prodId,string ppid)
    {
        ArrayList retLst = new ArrayList();
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        try
        {
            ret = iCreateProductandCombineLCM.TryPartMatchCheck(prodId, ppid);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(ret.PNOrItemName);
            retLst.Add(ret.CollectionData);
        }
        catch (FisException ex)
        {
            retLst.Add(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }

    [WebMethod]
    public ArrayList save(string prodId, bool flag, bool flag_39, IList<PrintItem> printItemLst)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            string custsn = "";
            IList<PrintItem> retLstPrint = iCreateProductandCombineLCM.Save(prodId, flag, flag_39, printItemLst, out  custsn);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retLstPrint);
            retLst.Add(prodId);
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
    public string Cancel(string key, string station)
    {
        try
        {
            iCreateProductandCombineLCM.Cancel(key);
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

    [WebMethod]
    public ArrayList ClearPart(string prodId)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iCreateProductandCombineLCM.ClearPart(prodId);
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

    [WebMethod]
    public ArrayList RePrint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {

            ret = iCreateProductandCombineLCM.Reprint(prodid, reason, line, editor, station, customer, pCode, printItems);
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
}


