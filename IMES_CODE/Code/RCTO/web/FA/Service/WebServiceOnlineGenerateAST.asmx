<%--
 /*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for Online Generate AST Page
 * UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2011/11/21 
 * UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2011/11/21            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-1775, Jessica Liu, 2012-5-2
*/
 --%>
 
<%@ WebService Language="C#" Class="WebServiceOnlineGenerateAST" %>


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
public class WebServiceOnlineGenerateAST : System.Web.Services.WebService
{
    IOnlineGenerateAST iOnlineGenerateAST = ServiceAgent.getInstance().GetObjectByName<IOnlineGenerateAST>(WebConstant.OnlineGenerateASTObject);

    [WebMethod]
    public ArrayList print(string custsn, string pdline, string stationId, string editor, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> retLst = new List<PrintItem>();
        string ast = "";
        try
        {
            retLst = iOnlineGenerateAST.Print(custsn, pdline, stationId, editor, customer, printItems, out ast);
            
            //2012-7-14, Jessica Liu, for mantis bug
            if (string.IsNullOrEmpty(ast))
            {
                ast = " ";
            }
                
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(ast);
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
    public ArrayList reprint(string custsn, string pdline, string stationId, string editor, string customer, string reason, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> retLst = new List<PrintItem>();
        string prodid = "";// ast = "";
        //ITC-1360-1775, Jessica Liu, 2012-5-2
        string strImageUrl = "";
        string strErrorFlag = "";
        
        try
        {
            //ITC-1360-1775, Jessica Liu, 2012-5-2
            //retLst = iOnlineGenerateAST.RePrint(custsn, pdline, stationId, editor, customer, reason, printItems, out prodid);//ast);
            retLst = iOnlineGenerateAST.RePrint(custsn, pdline, stationId, editor, customer, reason, printItems, out prodid, out strErrorFlag, out strImageUrl);//ast);
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(prodid);
            //ret.Add(ast);
            //ITC-1360-1775, Jessica Liu, 2012-5-2
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
    public List<string> CheckCustomerSN(string custsn, string pdline, string stationId, string editor, string customer)
    {
        List<string> ret = new List<string>();
        List<string> retLst = new List<string>();
        string CustomerSN = "";
        string ProductID = "";
        string Model = "";
        //2012-4-9
        string strToCombineAST = "";
        //2012-4-10
        string strErrorFlag = "";
        string strImageUrl = "";
            
        try
        {
            retLst = iOnlineGenerateAST.CheckCustomerSN(custsn, pdline, stationId, editor, customer);
            if (retLst != null)
            {
                CustomerSN = retLst[0];
                ProductID = retLst[1];
                Model = retLst[2];
                //2012-4-9
                strToCombineAST = retLst[3];
                //2012-4-10
                strErrorFlag = retLst[4];
                strImageUrl = retLst[5];
            }
                 
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(CustomerSN);
            ret.Add(ProductID);
            ret.Add(Model);
            //2012-4-9
            ret.Add(strToCombineAST);
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
            iOnlineGenerateAST.Cancel(key);

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
