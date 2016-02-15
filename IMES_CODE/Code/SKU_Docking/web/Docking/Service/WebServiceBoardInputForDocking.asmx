<%@ WebService Language="C#" Class="WebServiceBoardInputForDocking" %>


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
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceBoardInputForDocking : System.Web.Services.WebService
{
    IBoardInputForDocking iBoardInputForDocking = ServiceAgent.getInstance().GetObjectByName<IBoardInputForDocking>(WebConstant.BoardInputForDockingObject);

    
    [WebMethod]
    public ArrayList inputMBSn(string prodId, string mbsn)
    {
        ArrayList retLst = new ArrayList();
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        try
        {
            //ret = iBoardInputForDocking.InputMBSn(prodId, mbsn);
            ret = iBoardInputForDocking.TryPartMatchCheck(prodId, mbsn);
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
    public ArrayList save(string prodId, IList<PrintItem> printItemLst, bool printflag)
            
    {
        ArrayList retLst = new ArrayList();
       
        try
        {   string custsn="";
    
            //iBoardInputForDocking.Save(prodId, out  custsn);
            //retLst.Add(WebConstant.SUCCESSRET);
            //retLst.Add(custsn);
            IList<PrintItem> retLstPrint = iBoardInputForDocking.Save(prodId, printItemLst, printflag, out  custsn);
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
            IList<PrintItem> retLst = iBoardInputForDocking.PrintCustsnLabel(printItemLst, custsn);
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
    public IList ShowQueryData(string line)
    {
        string ret = "";
        IList retLst = new ArrayList();
        try
        {
            ret = iBoardInputForDocking.GetModelPassedQty(line);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(ret);
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
    public ArrayList Cancel(string prodId, string station)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iBoardInputForDocking.Cancel(prodId);

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


