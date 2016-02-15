<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for Image Download Page
 * UI:CI-MES12-SPEC-FA-UI Image Download.docx –2011/10/28 
 * UC:CI-MES12-SPEC-FA-UC Image Download.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：[FA].[dbo].[rpt_ITCNDTS_SET_IMAGEDOWN_14]-数据接口尚未定义（in Activity：DoImageDownloadSave）
* UC 具体业务：检查Customer SN 是否存在（具体见接口需求表）-数据接口尚未定义（in Activity：CheckCPQSNO）
* UC 具体业务：select @grade=Grade from PAK.dbo.HP_Grade where Family=@descr-数据接口尚未定义（in Activity：DoImageDownloadSave）
* UC 具体业务：根据@flag 进行不同的处理（具体见接口需求表）-数据接口尚未定义（in Activity：DoImageDownloadSave）
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceImageDownload" %>


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
public class WebServiceImageDownload : System.Web.Services.WebService
{
    IImageDownload iImageDownload = ServiceAgent.getInstance().GetObjectByName<IImageDownload>(WebConstant.ImageDownloadObject);

    [WebMethod]
    public ArrayList checkCPQSNO(string cpqsno, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            IList<String> ret1 = iImageDownload.checkCPQSNO(cpqsno, pdLine, editor, stationId, customerId);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(ret1[0]);
            ret.Add(ret1[1]);
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
    public ArrayList doSave(string cpqsno, string bios, string image, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iImageDownload.DoSave(cpqsno, bios, image, pdLine, editor, stationId, customerId);
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
    public string Cancel(string key)
    {
        try
        {
            iImageDownload.Cancel(key);

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
