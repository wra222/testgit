<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for BT Change Page
 * UI:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28 
 * UC:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-28   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：确认不是一个已存在的Model[数据表ProductBT：栏位：ProductID（char（10）），BT（varchar（50）），Editor，Cdt,Udt]-接口待测试（in Activity：CheckModelValid）
* UC 具体业务：查找符合条件的Product：1） ProductStatus.Line的Stage=“FA” 2） ProductStatus.Line的Stage=“PAK”并且ProductStatus.Station=“69”-接口待测试（in Activity：GetProductByProductStatus）
* UC 具体业务：BT 转 非BT：删除ProductBT表中符合条件的Product-接口待测试（in Activity：BTChangeToUnBT）
* UC 具体业务：非BT 转 BT：将符合条件的Product添加到ProductBT表中ProductBT.BT ='BT'+convert(nvarchar(20),getdate(),112)-接口待测试（in Activity：unBTChangeToBT）
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceBTChange" %>


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
public class WebServiceBTChange : System.Web.Services.WebService
{
    IBTChange iBTChange = ServiceAgent.getInstance().GetObjectByName<IBTChange>(WebConstant.BTChangeObject);

    [WebMethod]
    public ArrayList inputModel(string model, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            iBTChange.InputModel(model, pdLine, editor, stationId, customerId);
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
    public ArrayList DoBTChange(string model, bool BTToUnBT, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iBTChange.DoBTChange(model, BTToUnBT, pdLine, editor, stationId, customerId);
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
            iBTChange.Cancel(key);

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
