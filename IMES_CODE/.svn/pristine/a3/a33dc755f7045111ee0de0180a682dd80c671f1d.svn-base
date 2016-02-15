<%@ WebService Language="C#" Class="WebServiceCombineCarton_CR" %>


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
public class WebServiceCombineCarton_CR : System.Web.Services.WebService
{
    ICombineKeyParts iCombineKeyParts = ServiceAgent.getInstance().GetObjectByName<ICombineKeyParts>(WebConstant.CombineKeyPartsObject);
    ICombineCarton_CR iCombineCarton_CR = ServiceAgent.getInstance().GetObjectByName<ICombineCarton_CR>(WebConstant.CombineCarton_CR);
    
    [WebMethod]
    public ArrayList inputProductFirst(string pdLine, string input, string Station, string editor, string customerId)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            retLst = iCombineCarton_CR.inputProductFirst(pdLine, input, Station, editor, customerId);
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
    public ArrayList inputProductOther(string input, string FirstProductID)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            retLst = iCombineCarton_CR.inputProductOther(input, FirstProductID);
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
    public ArrayList save(string FirstProductID, IList<PrintItem> printItemLst)
            
    {
        ArrayList retLst = new ArrayList();
        try
        {
            string cartonsn = "";
            IList<PrintItem> retLstPrint = iCombineCarton_CR.Save(FirstProductID, printItemLst, out cartonsn);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retLstPrint);
            retLst.Add(cartonsn);
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
    public ArrayList ClearPart(string prodId)
    {

        ArrayList ret = new ArrayList();
        try
        {
            iCombineKeyParts.ClearPart(prodId);

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
    public ArrayList Cancel(string prodId, string station)
    {
        ArrayList ret = new ArrayList();
        try
        {
            //iCombineKeyParts.Cancel(prodId);
            iCombineCarton_CR.Cancel(prodId);
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
            ret = iCombineCarton_CR.Reprint(prodid, reason, line, editor, station, customer, pCode, printItems);
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


