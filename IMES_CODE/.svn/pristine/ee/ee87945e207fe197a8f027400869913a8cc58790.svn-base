<%--
 /*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for Combine AST Page
 * UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2011/12/5 
 * UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2011/12/5            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0818, Jessica Liu, 2012-2-28
*/
 --%>
 
<%@ WebService Language="C#" Class="WebServiceCombineAST" %>


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
public class WebServiceCombineAST : System.Web.Services.WebService
{
    ICombineAST iCombineAST = ServiceAgent.getInstance().GetObjectByName<ICombineAST>(WebConstant.CombineASTObject);

    [WebMethod]
    public ArrayList dosave(string prodid, string ast, string stationId, string editor, string customer)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> retLst = new List<PrintItem>();
        try
        {
            iCombineAST.DoSave(prodid, ast, stationId, editor, customer);
            ret.Add(WebConstant.SUCCESSRET);
            //ret.Add(retLst);
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
    /*
    public List<string> doprint(string prodid, bool deleteflag, string stationId, string editor, string customer)
    {
        List<string> ret = new List<string>();
        List<string> retLst = new List<string>();
        string ProdId = "";
        string CustomerSN = "";
        string Model = "";
        string isCDSI = "";
        
        try
        {
            retLst = iCombineAST.DoPrint(prodid, deleteflag, stationId, editor, customer);
            if (retLst != null)
            {
                ProdId = retLst[0];
                CustomerSN = retLst[1];
                Model = retLst[2];
                isCDSI = retLst[3];
            }

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(ProdId);
            ret.Add(CustomerSN);
            ret.Add(Model);
            ret.Add(isCDSI);
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
    */
    public ArrayList doprint(string prodid, bool deleteflag, string stationId, string editor, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> retPntLst = new List<PrintItem>();
        string ProdId = "";
        string CustomerSN = "";
        string Model = "";
        string isCDSI = "";
        //2012-5-2
        string astInfo = "";

        try
        {
            //2012-5-2
            //retPntLst = iCombineAST.DoPrint(prodid, deleteflag, stationId, editor, customer, printItems, out ProdId, out CustomerSN, out Model, out isCDSI);
            retPntLst = iCombineAST.DoPrint(prodid, deleteflag, stationId, editor, customer, printItems, out ProdId, out CustomerSN, out Model, out isCDSI, out astInfo);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retPntLst);
            ret.Add(ProdId);
            ret.Add(CustomerSN);
            ret.Add(Model);
            ret.Add(isCDSI);
            //2012-5-2
            ret.Add(astInfo);
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
    public List<string> blockcheck(string prodid, int length, string pdline, string status, string stationId, string editor, string customer)
    {
        List<string> ret = new List<string>();
        string astreturn = "";
        //2012-4-10
        List<string> retLst = new List<string>();
        string strErrorFlag = "";
        string strImageUrl = "";
        
        try
        {
            //2012-4-10
            //astreturn = iCombineAST.BlockCheck(prodid, length, pdline, status, stationId, editor, customer);
            retLst = iCombineAST.BlockCheck(prodid, length, pdline, status, stationId, editor, customer);
            if (retLst != null)
            {
                astreturn = retLst[0];
                strErrorFlag = retLst[1];
                strImageUrl = retLst[2];
            }
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(astreturn);
            //2012-4-10
            ret.Add(strErrorFlag);
            ret.Add(strImageUrl);
            
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
    public string Cancel(string key, string station)
    {
        try
        {
            iCombineAST.Cancel(key);

            return "";

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
