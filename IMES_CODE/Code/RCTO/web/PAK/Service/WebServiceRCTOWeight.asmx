<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:WebService for RCTOWeight Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI RCTO Weight
 * UC:CI-MES12-SPEC-PAK-UC RCTO Weight
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-09-08  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
--%>
<%@ WebService Language="C#" Class="WebServiceRCTOWeight" %>


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
public class WebServiceRCTOWeight : System.Web.Services.WebService 
{
    IRCTOWeight iRCTOWeight = ServiceAgent.getInstance().GetObjectByName<IRCTOWeight>(WebConstant.RCTOWeightObject);

    [WebMethod]
    public decimal getModelWeight(string model)
    {
        try
        {
            return iRCTOWeight.GetModelWeight(model);
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
    public void setModelWeight(string model, decimal weight, string editor)
    {
        try
        {
            iRCTOWeight.SetModelWeight(model, weight, editor);
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

