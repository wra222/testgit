<%@ WebService Language="C#" Class="WebServiceCollectTabletFaPart" %>


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
public class WebServiceCollectTabletFaPart : System.Web.Services.WebService
{
    ICollectTabletFaPart iCollectTabletFaPart = ServiceAgent.getInstance().GetObjectByName<ICollectTabletFaPart>(WebConstant.ICollectTabletFaPart);
                                                                                     
    
    [WebMethod]
     public ArrayList inputVenderCT(string sessionKey, string checkValue)
    {
        ArrayList retLst = new ArrayList();
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();

        try
        {
            ret = iCollectTabletFaPart.TryPartMatchCheck(sessionKey, checkValue); 
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
    public IList inputSN(string custSN, string pdLine, string curStation, string editor, string station, string customer, string allownullbomitem)
      {
        ArrayList ret = new ArrayList();
        IList<BomItemInfo> BomListbyTPM = new List<BomItemInfo>();

        try
        {
            bool allnullcheckitem = false;
            if (allownullbomitem.Trim().ToUpper()=="Y")
             {
                 allnullcheckitem = true;
             }
            ArrayList tmpList = iCollectTabletFaPart.InputSN(custSN, pdLine, curStation, editor, station, customer, allnullcheckitem);
            // DEBUG  ArrayList tmpList = iCombineTPM.InputSN(custSN, pdLine, "3C", editor, "3C", customer);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//DataModel.ProductInfo
            BomListbyTPM = (List <BomItemInfo>)tmpList[1];
            ret.Add(BomListbyTPM);//IList<BomItemInfo>
            ret.Add(tmpList[2]);//need  print POD Label
            ret.Add(tmpList[3]);//POD Label color
      
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
    public ArrayList save(string prodId, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
           ArrayList arr= iCollectTabletFaPart.Save(prodId, printItems);
           ret.Add(WebConstant.SUCCESSRET);
           ret.Add(arr[0]);
           ret.Add(arr[1]);
           if (arr.Count > 2) // add check Label bool  
           {
               ret.Add(arr[2]); //¤Ú¦èE-waste label
               ret.Add(arr[3]); //BAHASA label
               ret.Add(arr[4]);//NOM Label
           }
           return ret;
        }
        catch (FisException e)
        {
          ret.Add(e.mErrmsg);
        //    throw new Exception(e.mErrmsg);
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

            ArrayList arr = iCollectTabletFaPart.RePrint(sn, reason, line, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]);
            ret.Add(arr[1]);
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
    public ArrayList RePrintPOD(string sn, string reason, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {

            ArrayList arr = iCollectTabletFaPart.RePrintPOD(sn, reason, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]); //Model
            ret.Add(arr[1]);  //Color
            return ret;
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
    
    }
    [WebMethod]
    public ArrayList Cancel(string prodId)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iCollectTabletFaPart.Cancel(prodId);
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


