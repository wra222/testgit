<%@ WebService Language="C#" Class="WebServiceFACombineKeyParts" %>


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
public class WebServiceFACombineKeyParts : System.Web.Services.WebService
{
    ICombineKeyParts iCombineKeyParts = ServiceAgent.getInstance().GetObjectByName<ICombineKeyParts>(WebConstant.CombineKeyPartsObject);

    [WebMethod]
    public ArrayList inputPPID(string prodId,
            string ppid)
    {


        ArrayList retLst = new ArrayList();
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        try
        {
            ret = iCombineKeyParts.TryPartMatchCheck(prodId, ppid);
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
        /*
        return ret;
        //IList<String> partnoLst = new List<String>();

        try
        {
            ret = iCombineKeyParts.InputPPID(prodId, ppid);
            //retLst.Add(WebConstant.SUCCESSRET);
            //retLst.Add(item);

        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }*/
        //retLst.Add(WebConstant.SUCCESSRET);

        //if (ppid == "10")
        //{
        //    partnoLst.Add("15");
        //    partnoLst.Add("12");
        //    partnoLst.Add("10");
        //}
        //else
        //{
        //    partnoLst.Add("11");
        //    partnoLst.Add("13");
        //    partnoLst.Add("90B4");


        //}

        //retLst.Add(partnoLst);
        //retLst.Add(ppid);


    }

    [WebMethod]
    public ArrayList save(string prodId, bool flag, bool flag_39, IList<PrintItem> printItemLst)
            
    {
        ArrayList retLst = new ArrayList();
       
        try
        {   string custsn="";

            IList<PrintItem> retLstPrint = iCombineKeyParts.Save(prodId, flag, flag_39, printItemLst, out  custsn);
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
            iCombineKeyParts.Cancel(prodId);

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

            ret = iCombineKeyParts.Reprint(prodid, reason, line, editor, station, customer, pCode, printItems);
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


