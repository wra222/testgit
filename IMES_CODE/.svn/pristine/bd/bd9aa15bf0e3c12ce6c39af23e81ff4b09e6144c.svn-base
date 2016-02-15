<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:RCTO LCM Defect Input
* UI:CI-MES12-SPEC-FA-UI RCTO LCM Defect Input.docx
* UC:CI-MES12-SPEC-FA-UC RCTO LCM Defect Input.docx             
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-24   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceLCMDefectInputRCTO" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceLCMDefectInputRCTO : System.Web.Services.WebService
{
    private IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    private ILCMDefectInputForRCTO iLCMObject = ServiceAgent.getInstance().GetObjectByName<ILCMDefectInputForRCTO>(WebConstant.LCMDefectInputObjectForRCTO);
    
    private string type = "PRD";
    
    [WebMethod]
    public IList inputCTNO(string ctno, string pdLine, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ArrayList tmpList = new ArrayList();
        IList<DefectInfo> defectList;
        
        try
        {
            tmpList = iLCMObject.InputCTNO(ctno, pdLine, editor, station, customer);
            defectList = iDefect.GetDefectList(type);
            
            ret.Add(defectList);
            ret.Add(tmpList[0]);//productid
            ret.Add(tmpList[1]);//Product_Part存在
           
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
    public void save(string prodId, IList<string> defectList)
    {
        try
        {
            iLCMObject.save(prodId, defectList);
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
    public void cancel(string productId)
    {
        try
        {
            iLCMObject.cancel(productId);
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
