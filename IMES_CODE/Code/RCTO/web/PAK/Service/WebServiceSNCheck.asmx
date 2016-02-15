<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：判断是否需要检查Asset Tag SN Check：	Select b.Tp,Type,@message=Message,Sno1, @lwc=L_WC, @mwc=M_WC from Special_Det a,Special_Maintain b where a.SnoId=@Productid and b.Type=a.Tp and b.SWC=”68”得到的Tp=”K”的记录时，表示需要检查Asset Tag SN Check是否做过 (目前只考虑得到一条记录的情况??)-数据接口尚未定义（in Activity：CheckAssetTagSN）
* UC 具体业务：当BOM(存在PartType=ALC and BomNodeType=PL的part) 且model<>PC4941AAAAAY时，表示有ALC，这时没有真正的Pizza盒-数据接口尚未定义（in Activity：CheckSNIdentical）
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceSNCheck" %>


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
public class WebServiceSNCheck : System.Web.Services.WebService
{
    ISNCheck iSNCheck = ServiceAgent.getInstance().GetObjectByName<ISNCheck>(WebConstant.SNCheckObject);

    [WebMethod]
    public ArrayList inputCustSNOnProduct(string custsn, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            string prodID = iSNCheck.InputCustSNOnProduct(custsn, pdLine, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(prodID);
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
    public ArrayList inputCustSNOnPizza(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iSNCheck.InputCustSNOnPizza(custsnOnPizza, custsnOnProduct, pdLine, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
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
    public ArrayList inputCustSNOnPizzaReturn(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            string retInfo = iSNCheck.InputCustSNOnPizzaReturn(custsnOnPizza, custsnOnProduct, pdLine, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retInfo);
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
    public ArrayList checkTwoSNIdentical(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iSNCheck.CheckTwoSNIdentical(custsnOnPizza, custsnOnProduct, pdLine, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
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
            iSNCheck.Cancel(key);

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
