<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/FA/ITCNDCheck Page
 * UI:CI-MES12-SPEC-FA-UI ITCNDCheck.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCNDCheck.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-11   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* Check Item
* 
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ WebService Language="C#" Class="WebServiceITCNDCheckQuery" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using log4net;
using com.inventec.template;
using com.inventec.template.structure;
using com.inventec.imes.BLL;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebServiceITCNDCheckQuery : System.Web.Services.WebService 
{
    IITCNDCheck iITCNDCheck = ServiceAgent.getInstance().GetObjectByName<IITCNDCheck>(WebConstant.ITCNDCheckObject);

    [WebMethod]
    public ArrayList ITCNDQuery(string pdline, string station, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        IList<string> res = new List<string>();
        try
        {
            res = iITCNDCheck.GetPdLinePass(pdline, station, editor, customer);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(res);
            retLst.Add(res.Count);
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


