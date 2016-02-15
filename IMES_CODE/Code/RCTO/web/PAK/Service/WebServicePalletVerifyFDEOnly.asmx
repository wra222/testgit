<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-03   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1. 检查Pallet 上的所有SKU
 *                2. 列印Ship to Pallet Label；
 *                3. 内销要额外列印一张Pallet CPMO Label
 */
--%>
<%@ WebService Language="C#" Class="WebServicePalletVerifyFDEOnly" %>


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


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePalletVerifyFDEOnly : System.Web.Services.WebService 
{

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    IPalletVerifyFDEOnly iPalletVerifyFEDOnly = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyFDEOnly>(WebConstant.PalletVerifyFDEOnlyObject);
    
    [WebMethod]
    public ArrayList inputFirstCustSN(string firstCustSn, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        int index =0;
        
        try
        {
            ArrayList retLst = new ArrayList();
            retLst = iPalletVerifyFEDOnly.InputFirstCustSn(firstCustSn, line, editor, station, customer, out index);
            ret.Add(retLst);
            ret.Add(index);
            return ret;
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "CHK020")
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
    public ArrayList inputCustSN(string firstCustSn, string custSn)
    {
        ArrayList ret = new ArrayList();

        try
        {
            int indexNo = 0;
            indexNo = iPalletVerifyFEDOnly.InputCustSn(firstCustSn, custSn);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(indexNo);
            return ret;
        }
        catch (FisException ex)
        {
           // throw new Exception(ex.mErrmsg);

            if (ex.mErrcode == "CHK079" || ex.mErrcode == "CHK152" || ex.mErrcode == "PAK022")
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
    public ArrayList save(string firstCustSn, IList<PrintItem> printItems, IList<string> ScanProductNoList)
    {
        ArrayList ret = new ArrayList();
        string DummyPalletNo = string.Empty;
        ArrayList printParams = new ArrayList();
        try
        {
            IList<PrintItem> PrintItemLst = iPalletVerifyFEDOnly.Save(firstCustSn, printItems, ScanProductNoList, out DummyPalletNo, out printParams);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(PrintItemLst);
            ret.Add(DummyPalletNo);
            ret.Add(printParams);
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
            iPalletVerifyFEDOnly.Cancel(uutSn);
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
    public ArrayList reprint(string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<PrintItem> retLst = iPalletVerifyFEDOnly.rePrint(palletNo, reason, line, editor, station, customer, printItems);
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

