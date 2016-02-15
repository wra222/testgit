<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for MB Combine CPU
 * UI:CI-MES12-SPEC-SA-UI MB Combine CPU.docx –2011/12/9 
 * UC:CI-MES12-SPEC-SA-UC MB Combine CPU.docx –2011/12/9            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-1-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
 --%>
 
<%@ WebService Language="C#" Class="WebServiceMBCombineCPU" %>

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


public class WebServiceMBCombineCPU : System.Web.Services.WebService 
{
    IMBCombineCPU iMBCombineCPU = ServiceAgent.getInstance().GetObjectByName<IMBCombineCPU>(WebConstant.MBCombineCPUObject);


    [WebMethod]
    public string mbsnoSFC(string pdLine, string MB_SNo, string editor, string stationId, string customerId)
    {
       
        try
        {
            iMBCombineCPU.MBCombineInputMBSN(pdLine, MB_SNo, editor, stationId, customerId);
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
    
    
    [WebMethod]
    public string MBCombineCPU(string pdLine, string MB_SNo, string CPUVendorSN, string editor, string stationId, string customerId, bool SessionReStartFlag)
    {
        
        try
        {
            if (SessionReStartFlag)
            {
                iMBCombineCPU.MBCombineInputMBSN(pdLine, MB_SNo, editor, stationId, customerId); 
            } 
            
           iMBCombineCPU.CombineCPU(MB_SNo, CPUVendorSN);
           return (WebConstant.SUCCESSRET);
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "CHK008")
            {
                return (ex.mErrmsg);
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }
    
    
    [WebMethod]
    public string Cancel(string MB_SNo, string stationId)
    {
        try
        {
            iMBCombineCPU.CancelMB(MB_SNo);
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

