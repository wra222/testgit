<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI-Combine Carton in DN 
* UC:CI-MES12-SPEC-PAK-UC-Combine Carton in DN           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/09/08    ITC000052             Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceCombineCartonInDNForRCTO" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCombineCartonInDNForRCTO : System.Web.Services.WebService
{

    ICombineCartonInDNForRCTO CombineCartonInDN = ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDNForRCTO>(WebConstant.CombineCartonInDNObjectForRCTO);
    
    [WebMethod]
    public ArrayList InputSN(string inputSN, string line, string editor, string station, string customer)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        ProductModel curProduct = new ProductModel();

        try
        {
            tmpList = CombineCartonInDN.InputSN(inputSN, line, editor, station, customer);
            
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]);//ProductInfoMaintain
            retList.Add(tmpList[1]);//prodList.Count
            
            return retList;
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
    public ArrayList Save(string firstProID, string deliveryNo, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList tmpLst = CombineCartonInDN.Save(firstProID, deliveryNo, printItems);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpLst[0]); //printlist
            ret.Add(tmpLst[1]); //packedQty
            ret.Add(tmpLst[2]); //carton SN
            ret.Add(tmpLst[3]); //Delivery No
            ret.Add(tmpLst[4]); //customer SN  
            ret.Add(tmpLst[5]); //strEnd
            ret.Add(tmpLst[6]); //printflag         


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
    public void cancel(string deliverNo)
    {
        try
        {
            CombineCartonInDN.cancel(deliverNo);
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
