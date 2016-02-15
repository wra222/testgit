<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/PAK/Service/PackingListForPL
 * UI:CI-MES12-SPEC-PAK-UI Packing List.docx –2011/11/22 
 * UC:CI-MES12-SPEC-PAK-UC Packing List.docx –2011/11/22            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-22   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ WebService Language="C#" Class="WebServicePackingListForPL" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePackingListForPL : System.Web.Services.WebService
{
    IPackingList iPackingList = ServiceAgent.getInstance().GetObjectByName<IPackingList>(WebConstant.PackingListObject);    

    [WebMethod]
    public ArrayList CheckSN(string sn, string flag, int count, string doctype, string pdline, string station, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();

        try
        {
            ArrayList serviceRet = new ArrayList();
            serviceRet = iPackingList.CheckSN(sn, flag, count, doctype, pdline, station, editor, customer);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(serviceRet);
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
}


