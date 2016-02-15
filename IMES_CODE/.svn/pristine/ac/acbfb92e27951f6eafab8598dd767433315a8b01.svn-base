<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-03              Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1. 检查Pallet 上的所有SKU
 *                2. 列印Ship to Pallet Label；
 *                3. 内销要额外列印一张Pallet CPMO Label
 */
--%>
<%@ WebService Language="C#" Class="WebServicePalletVerifyForRCTO" %>


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
using System.Data;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePalletVerifyForRCTO : System.Web.Services.WebService 
{

    private IPalletVerifyForRCTO iPalletVerify;
    private Object PalletVerify;
    private string PalletVerifyObjectBllName = WebConstant.PalletVerifyForRCTOObject;
    
    [WebMethod]
    public ArrayList inputFirstCustSN(string firstCustSn, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName <IPalletVerifyForRCTO>(PalletVerifyObjectBllName);
            iPalletVerify=(IPalletVerifyForRCTO) PalletVerify;
            ArrayList retLst = new ArrayList();
            retLst = iPalletVerify.InputFirstCartonNo(firstCustSn, line, editor, station, customer);
            ret.Add(retLst);
            
            return ret;
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "CHK021") ///tmp
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
      

    [WebMethod]
    public ArrayList inputCustSN(string firstCustSn, string custSn, string firstPalletNo)
    {
        ArrayList ret = new ArrayList();

        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyForRCTO>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyForRCTO)PalletVerify;

            ArrayList retLst = new ArrayList();
            retLst = iPalletVerify.InputCartonNo(firstCustSn, custSn,firstPalletNo);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            return ret;
        }
        catch (FisException ex)
        {
          //  throw new Exception(ex.mErrmsg);

            if (ex.mErrcode == "PAK144" || ex.mErrcode == "CHK880" || ex.mErrcode == "CHK881" || ex.mErrcode == "SFC002")
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [WebMethod]
    public ArrayList save(string firstCustSn, IList<PrintItem> printItems, string PalletNo,string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
       // ArrayList printparam1 = new ArrayList();
       // ArrayList printparam2 = new ArrayList();
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyForRCTO>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyForRCTO)PalletVerify;

            ArrayList retLst = new ArrayList();
            retLst = iPalletVerify.Save(firstCustSn, printItems, PalletNo,line, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
           
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
    public string Cancel(string uutSn)
    {
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyForRCTO>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyForRCTO)PalletVerify;


            iPalletVerify.Cancel(uutSn);
            return (WebConstant.SUCCESSRET);
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
    public ArrayList callTemplateCheckLaNew(string dn, string docType)
    {
        ArrayList ret = new ArrayList();
        DataTable dt = new DataTable();
        try
        {
            
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
    public ArrayList reprint(string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
	  
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyForRCTO>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyForRCTO)PalletVerify;

            ArrayList retLst = new ArrayList();
            retLst = iPalletVerify.rePrint(palletNo, reason, line, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
	       
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
}

