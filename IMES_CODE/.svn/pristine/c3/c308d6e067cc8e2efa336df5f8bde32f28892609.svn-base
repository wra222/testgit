<%@ WebService Language="C#" Class="WebServiceBoardInput" %>


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
public class WebServiceBoardInput : System.Web.Services.WebService
{
    IBoardInput iBoardInput = ServiceAgent.getInstance().GetObjectByName<IBoardInput>(WebConstant.BoardInputObject);

    
    [WebMethod]
    public ArrayList inputMBSn(string prodId, string mbsn)
    {
        ArrayList retLst = new ArrayList();
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        try
        {
            //ret = iBoardInput.InputMBSn(prodId, mbsn);
            ret = iBoardInput.TryPartMatchCheck(prodId, mbsn);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(ret.PNOrItemName);
            retLst.Add(ret.CollectionData);
            //return ret;
            
        }
        catch (FisException ex)
        {
            retLst.Add(ex.mErrmsg);
            // throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst; 

    }

    [WebMethod]
    public IList SetDataCodeValue(string model, string customer)
    {
        string ret = "";
        IList retLst = new ArrayList();
        try
        {
            ret = iBoardInput.SetDataCodeValue(model, customer);

            retLst.Add(model);
            retLst.Add(ret);
            return retLst;
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public IList saveForPCAShippingLabel(string prodId, string MBno, string model, string dcode, IList<PrintItem> printItems)
    {
        IList ret = new ArrayList();
        try
        {
            ret = iBoardInput.saveForPCAShippingLabel(prodId, MBno, model, dcode, printItems);
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
    public ArrayList save(string prodId, IList<PrintItem> printItemLst, bool printflag)
            
    {
        ArrayList retLst = new ArrayList();
       
        try
        {   string custsn="";
    
            IList<PrintItem> retLstPrint = iBoardInput.Save(prodId, printItemLst, printflag, out  custsn);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retLstPrint);
            retLst.Add(custsn);
    
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
            IList<PrintItem> retLst = iBoardInput.PrintCustsnLabel(printItemLst, custsn);
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
    public ArrayList GetQty(string line)
    {
        ArrayList ret = new ArrayList();
        int q = 0;
        q= iBoardInput.CountQty(line);
        ret.Add(WebConstant.SUCCESSRET);
        ret.Add(q);
        return ret;
    }
    
    [WebMethod]
    public ArrayList Cancel(string prodId, string station)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iBoardInput.Cancel(prodId);

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
            ret = iBoardInput.Reprint(prodid, reason, line, editor, station, customer, pCode, printItems);
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


