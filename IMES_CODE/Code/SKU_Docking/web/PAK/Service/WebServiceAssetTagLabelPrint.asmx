<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for Asset Tag Label Print Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：Product.DeliveryNo 在DeliveryInfo分别查找InfoType=‘CustPo’/‘IECSo’-数据接口已提供，需进一步调试（in Activity：GetProductByCustomerSN）
* UC 具体业务：在ProductLog不存在Station='81' and Status=1 and Line= 'ATSN Print'的记录-数据接口已提供，需进一步调试（in Activity：CheckAssetSNReprint）
* UC 具体业务：Product_Part.Value where ProductID=@prdid and PartNo in (bom中BomNodeType=’AT’对应的Pn)-数据接口已提供，需进一步调试（in Activity：GetProductByCustomerSN）
* UC 具体业务：保存product和Asset SN的绑定关系-- Insert Product_Part values(@prdid,@partpn,@astsn,’’,’AT’,@user,getdate(),getdate()) -数据接口已提供，需进一步调试（in Activity：GenerateAssetTagLabel））
* UC 具体业务：得到IE维护的Asset SN可用范围select @descr=rtrim(Descr) from Maintain (nolock) where Tp='AST' and Code=@cust ，其中@cust是Product.Model对应的ModelInfo中Name=’Cust’对应的值-数据接口尚未定义（in Activity：GenerateAssetTagLabel）
* UC 具体业务：查找CUST（其中@cust是Product.Model对应的ModelInfo中Name=’Cust’对应的值）已用过的最大号-数据接口存在协商问题（in Activity：GenerateAssetTagLabel））
*/
 --%>
 
<%@ WebService Language="C#" Class="WebServiceAssetTagLabelPrint" %>


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
public class WebServiceAssetTagLabelPrint : System.Web.Services.WebService
{
    IAssetTagLabelPrint iPrintAssetTagLabel = ServiceAgent.getInstance().GetObjectByName<IAssetTagLabelPrint>(WebConstant.AssetTagLabelPrintObject);
    
    [WebMethod]
    public ArrayList print(string custsn, string productid, string stationId, string editor, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> retLst = new List<PrintItem>();
        string astCodeList = "";
        try
        {
            retLst = iPrintAssetTagLabel.Print(custsn, productid, stationId, editor, customer, printItems, out astCodeList);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(astCodeList);
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

        //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
        string strImageUrl = "";
        string strErrorFlag = "";
        string astCodeList = "";
        try
        {
            /* 2012-7-16, Jessica Liu, 新需求：增加ESOP显示
            retLst = iPrintAssetTagLabel.RePrint(custsn, pdline, stationId, editor, customer, reason, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            */
            retLst = iPrintAssetTagLabel.RePrint(custsn, pdline, stationId, editor, customer, reason, printItems, out strErrorFlag, out strImageUrl, out astCodeList);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(strErrorFlag);
            ret.Add(strImageUrl);
            ret.Add(astCodeList);
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

        //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
        string strErrorFlag = "";
        string strImageUrl = "";
        
        try
        {
            retLst = iPrintAssetTagLabel.CheckCustomerSN(custsn, pdline, stationId, editor, customer);
            if (retLst != null)
            {
                CustomerSN = retLst[0];
                ProductID = retLst[1];

                //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
                strErrorFlag = retLst[2];
                strImageUrl = retLst[3];
            }
                 
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(CustomerSN);
            ret.Add(ProductID);

            //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
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
			iPrintAssetTagLabel.Cancel(key);
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
    
}
