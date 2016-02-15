<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="PAQCOutputService" %>


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
public class PAQCOutputService : System.Web.Services.WebService
{
    private PAQCOutput iPAQCOutput;
    private string PAQCOutputBllName = WebConstant.PAQCOutputObject;
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
            iPAQCOutput = ServiceAgent.getInstance().GetObjectByName<PAQCOutput>(PAQCOutputBllName);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(commBllName);

            tmpList = iPAQCOutput.InputSN(custSN, pdLine, editor, station, customer);
            defectList = iDefect.GetDefectList(type);

            ret.Add(tmpList[0]);
            ret.Add(defectList);
            ret.Add(tmpList[1]);

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
    public void save(string prodId,string line, string editor, IList<string> defectList)
    {
        try
        {
            iPAQCOutput = ServiceAgent.getInstance().GetObjectByName<PAQCOutput>(PAQCOutputBllName);

            iPAQCOutput.save(prodId, line, editor,defectList);
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
            iPAQCOutput = ServiceAgent.getInstance().GetObjectByName<PAQCOutput>(PAQCOutputBllName);

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
