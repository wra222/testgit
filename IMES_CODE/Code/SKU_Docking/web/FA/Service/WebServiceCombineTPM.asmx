<%@ WebService Language="C#" Class="WebServiceCombineTPM" %>


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
public class WebServiceCombineTPM : System.Web.Services.WebService
{
    ICombineTPM iCombineTPM = ServiceAgent.getInstance().GetObjectByName<ICombineTPM>(WebConstant.CombineTPMObject);
                                                                                     
    
    [WebMethod]
     public ArrayList inputVenderCT(string sessionKey, string checkValue)
    {
        ArrayList retLst = new ArrayList();
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();

        try
        {
            ret = iCombineTPM.TryPartMatchCheck(sessionKey, checkValue); 
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(ret.PNOrItemName);
            retLst.Add(ret.CollectionData);
            //return retLst;
        }
        catch (FisException ex)
        {
            retLst.Add(ex.mErrmsg);

            //throw new Exception(ex.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
        return retLst;

    }
    [WebMethod]
    public IList inputSN(string custSN,string pdLine, string curStation,  string editor, string station, string customer)
      {
        ArrayList ret = new ArrayList();
        IList<BomItemInfo> BomListbyTPM = new List<BomItemInfo>();

        try
        {
            ArrayList tmpList = iCombineTPM.InputSN(custSN, pdLine, curStation, editor, station, customer);
            // DEBUG  ArrayList tmpList = iCombineTPM.InputSN(custSN, pdLine, "3C", editor, "3C", customer);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//DataModel.ProductInfo
            BomListbyTPM = (List <BomItemInfo>)tmpList[1];

            ret.Add(BomListbyTPM);//IList<BomItemInfo>
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
    public ArrayList save(string prodId)
    {
        ArrayList ret = new ArrayList();
        try
        {   
           iCombineTPM.Save(prodId);
           ret.Add(WebConstant.SUCCESSRET);
           //return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            //throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
   
   
    
    [WebMethod]
    public ArrayList Cancel(string prodId)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iCombineTPM.Cancel(prodId);
            ret.Add(WebConstant.SUCCESSRET);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            //throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex; ;
        }
        return ret;
    }
  
}


