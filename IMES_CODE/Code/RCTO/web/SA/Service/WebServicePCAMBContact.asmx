<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for MB Combine CPU
 * UI:CI-MES12-SPEC-SA-UI MB Combine CPU.docx –2011/12/9 
 * UC:CI-MES12-SPEC-SA-UC MB Combine CPU.docx –2011/12/9            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-1-4             (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
 --%>
 
<%@ WebService Language="C#" Class="WebServicePCAMBContact" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]


public class WebServicePCAMBContact : System.Web.Services.WebService 
{
    PCAMBContact iPCAMBContact = ServiceAgent.getInstance().GetObjectByName<PCAMBContact>(WebConstant.PCAMBContactObject);


    [WebMethod]
    public string CheckMBandSave(string pdLine, string NewMB, string OldMB, string editor, string stationId, string customerId)
    {
       
        try
        {
            iPCAMBContact.CheckMBandSave(pdLine, NewMB, OldMB, editor, stationId, customerId);
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

