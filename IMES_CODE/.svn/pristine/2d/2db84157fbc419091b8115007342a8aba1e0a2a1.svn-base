<%@ WebService Language="C#" Class="WebServiceCombineTmp" %>


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
public class WebServiceCombineTmp : System.Web.Services.WebService
{
    ICombineTmp iCombineTmp = ServiceAgent.getInstance().GetObjectByName<ICombineTmp>(WebConstant.CombineTmpObject);

    
    [WebMethod]
    public ArrayList inputMBSn(string prodId, string mbsn)
    {
        ArrayList retLst = new ArrayList();
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        try
        {
            //ret = iCombineTmp.InputMBSn(prodId, mbsn);
            ret = iCombineTmp.TryPartMatchCheck(prodId, mbsn);
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
    public ArrayList save(string prodId)
            
    {
        ArrayList retLst = new ArrayList();
       
        try
        {   string custsn="";
    
            iCombineTmp.Save(prodId, out  custsn);
            retLst.Add(WebConstant.SUCCESSRET);
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
    public ArrayList Cancel(string prodId, string station)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iCombineTmp.Cancel(prodId);

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


