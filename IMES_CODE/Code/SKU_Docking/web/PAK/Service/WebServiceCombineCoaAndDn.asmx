<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceCombineCoaAndDn" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCombineCoaAndDn : System.Web.Services.WebService
{
    ICombineCOAandDNNew combineCoaObject = ServiceAgent.getInstance().GetObjectByName<ICombineCOAandDNNew>(WebConstant.CombineCOAandDNNewObject);

    
    [WebMethod]
    public ArrayList InputSN(string custSN, string pdLine, string editor, string station, string customer)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        ProductModel curProduct = new ProductModel();

        try
        {
            tmpList = combineCoaObject.InputSN(custSN, pdLine, editor, station, customer);
            
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]); //productMode
            retList.Add(tmpList[1]); //curProduct.IsBT
            retList.Add(tmpList[2]); //coaPn
            retList.Add(tmpList[3]); //DN
            retList.Add(tmpList[4]); //win8
            retList.Add(tmpList[5]); //cdsi
            retList.Add(tmpList[6]); //productCoa 已经收集的COA

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
    public ArrayList checkCOA(string prodId, string coa)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
            tmpList = combineCoaObject.checkCOA(prodId, coa);
            
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]);//nodestr
            retList.Add(tmpList[1]);//coaPN
            
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
    public ArrayList save(string productID, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList tmp = new ArrayList();

        try
        {
            tmp = combineCoaObject.save(productID, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmp[0]);//printList
            ret.Add(tmp[1]);//jpflag
            ret.Add(tmp[2]);//dn
            
           
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }


        return ret;

    }

    [WebMethod]
    public void cancel(string productId)
    {
        try
        {
            combineCoaObject.cancel(productId);
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
