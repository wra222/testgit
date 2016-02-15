<%--
/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Web Service for PAQC Output For RCTO Page
* UI:CI-MES12-SPEC-PAK-UI PAQC Output_RCTO.docx –2012/6/11 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output_RCTO.docx –2012/7/10            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-25   Jessica Liu           Create
* Known issues:
* TODO：
*/
 --%>
<%@ WebService Language="C#" Class="WebServicePAQCOutputForRCTO" %>


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
public class WebServicePAQCOutputForRCTO : System.Web.Services.WebService
{
    private IPAQCOutputForRCTO iPAQCOutput;
    private string PAQCOutputBllName = WebConstant.PAQCOutputForRCTOObject;
    private IDefect iDefect;
    private string commBllName = WebConstant.CommonObject;
    private string type = "PRD";
    
    [WebMethod]
    public IList input(string pdLine, string custSN, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ArrayList tmpList = new ArrayList();
        IList<DefectInfo> defectList;
        
        try
        {
            iPAQCOutput = ServiceAgent.getInstance().GetObjectByName<IPAQCOutputForRCTO>(PAQCOutputBllName);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(commBllName);

            tmpList = iPAQCOutput.InputSN(custSN, pdLine, editor, station, customer);
            defectList = iDefect.GetDefectList(type);

            ret.Add(tmpList[0]);
            ret.Add(defectList);
            ret.Add(tmpList[1]);
			ret.Add(tmpList[2]); // needCheckMB

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
    
    //2012-7-20
    [WebMethod]
    public void checkmb(string pdLine, string mbsno, string prodId, string editor, string station, string customer)
    {       
        try
        {
            iPAQCOutput = ServiceAgent.getInstance().GetObjectByName<IPAQCOutputForRCTO>(PAQCOutputBllName);

            iPAQCOutput.checkMBSno(mbsno, prodId, pdLine, editor, station, customer);
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
            iPAQCOutput = ServiceAgent.getInstance().GetObjectByName<IPAQCOutputForRCTO>(PAQCOutputBllName);

            iPAQCOutput.save(prodId, defectList);
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
            iPAQCOutput = ServiceAgent.getInstance().GetObjectByName<IPAQCOutputForRCTO>(PAQCOutputBllName);

            iPAQCOutput.cancel(productId);
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
