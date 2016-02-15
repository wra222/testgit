<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Service Agent Code for FA Test Station page
 * UI:CI-MES12-SPEC-FA-UI FA Test Station.docx --2011/10/20 
 * UC:CI-MES12-SPEC-FA-UC FA Test Station.docx --2011/10/20           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-24  Zhang.Kai-sheng       (Reference Ebook SourceCode) 
 * Known issues:
 */
 --%>
<%@ WebService Language="C#" Class="WebServiceFATestStation" %>

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
public class WebServiceFATestStation : System.Web.Services.WebService
{
    IFATestStation iFATestStation = ServiceAgent.getInstance().GetObjectByName<IFATestStation>(WebConstant.FATestStationObject);
    IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    IProduct iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(WebConstant.CommonObject);
    
    [WebMethod]
    public ArrayList inputCustsn(string pdLine, string testStation, string custsn, string editor, string customer)
    {
        ///return ArrayList
        ///No1:return WebConstant.SUCCESSRET
        ///No2:ProductID
        ///No3:Model
        ///No4:defect List
        ///No5:defect Station
        ///No6:isAllowPass -true
        ///No7:PdLine
        ///No8:Last Station
    
        ArrayList retLst = new ArrayList();
        IList<DefectInfo> defectList;
        string model = String.Empty;
        string prodId = String.Empty;
        string defectStation = "";
        string lastStation = "";
        bool  isAllowPass = true;
        try
        {
            iFATestStation.InputCustsn(out  pdLine, testStation, custsn, editor, customer, out defectStation,out isAllowPass,out lastStation);
            ProductInfo productInfo = iProduct.GetProductInfoByCustomSn(custsn);
            model = productInfo.modelId;
            prodId = productInfo.id;
            defectList = iDefect.GetDefectList("PRD");
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(prodId);
            retLst.Add(model);
            retLst.Add(defectList);
            retLst.Add(defectStation);
            retLst.Add(isAllowPass);
            retLst.Add(pdLine);
            retLst.Add(lastStation);
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
    public string getPreStationPdLin(string prodId)
    {
        ProductStatusInfo inf = iProduct.GetProductStatusInfo(prodId);
        string pdline = "";
        try
        {
           if (inf.pdLine!= null)
            {
                pdline = inf.pdLine;
            }
         }
        catch (Exception e)
        {
            throw e;
        }

        return pdline;
    }
    
    [WebMethod]
    public ArrayList inputProdId(string pdLine, string testStation, string prodId, string editor, string customer)
    {
        ///return ArrayList
        ///No1:return WebConstant.SUCCESSRET
        ///No2:ProductID
        ///No3:Model
        ///No4:defect List
        ArrayList retLst = new ArrayList();
        IList<DefectInfo> defectList;
        string model = String.Empty ;
        string strRuninrtn = string.Empty;
        bool JustKeyDefectCode = false;
        string RunInTimes = "";
        try
        {
            ArrayList result = iFATestStation.InputProdId(pdLine, testStation, prodId, editor, customer, out JustKeyDefectCode, out RunInTimes);
            //ProductInfo productInfo = iProduct.GetProductInfo(prodId);
            //model = productInfo.modelId;
            defectList = iDefect.GetDefectList("PRD");///Get default list
			
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(result[0].ToString()); //prodId
            retLst.Add(result[1].ToString()); //model
            retLst.Add(defectList);
            retLst.Add(result[2].ToString()); //strRuninrtn
            retLst.Add(JustKeyDefectCode);
            retLst.Add(RunInTimes);
            retLst.Add(result[3].ToString()); //customSN
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //model = "model";
        //retLst.Add(WebConstant.SUCCESSRET);
        //retLst.Add(prodId);
        //retLst.Add(model);
        //defectList = new List<DefectInfo>();
        //DefectInfo defect = new DefectInfo();
        //defect.id = "aaa";
        //defect.description = "111111111111111222222222222222333333333333333333344444444444444444455555555555555";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "bbb";
        //defect.description = "bbb";
        //defectList.Add(defect);
        //retLst.Add(defectList);
        return retLst;
    }

    [WebMethod]
    public ArrayList savePage(string prodId, List<string> defectList,bool isNeedPrint2D, IList<PrintItem> printItemLst)
    {
        ///return ArrayList
        ///No1:return WebConstant.SUCCESSRET
        ///No2:retLst = iFATestStation.InputDefectCodeList(prodId, iList,isNeedPrint2D,printItemLst);
        ArrayList ret = new ArrayList();   
        try
        {
            IList<string> iList = defectList;
	        string qcMethod = "";
            string setMsg = "";
            IList<PrintItem> retLst = iFATestStation.InputDefectCodeList(prodId, iList,isNeedPrint2D,printItemLst, out qcMethod,out setMsg);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(qcMethod);
            ret.Add(retLst);
            ret.Add(setMsg);
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
    public ArrayList Cancel(string MB, string station)
    {
        ///return ArrayList
        ///No1:return WebConstant.SUCCESSRET
        ///No2:MB  document.getElementById("<txtProdId.ClientID%>").innerText
        ArrayList ret = new ArrayList();
        try
        {
            iFATestStation.Cancel(MB);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(MB);
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
  
}