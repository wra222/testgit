<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Output
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Output.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Output.docx           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	记录SAOQC 结果，若有不良，则记录不良信息
 *                
 * UC Revision: 3382
 */
--%>

<%@ WebService Language="C#" Class="WebServicePCAOQCOutput" %>

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
public class WebServicePCAOQCOutput : System.Web.Services.WebService
{

    IPCAOQCOutput iPCAOQCOutput = ServiceAgent.getInstance().GetObjectByName<IPCAOQCOutput>(WebConstant.PCAOQCOutputObject);
    
    IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    
    private const string SUCCESS = "SUCCESSRET";

    [WebMethod]
    public ArrayList InputMBSno(string mbsno, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        IList<DefectInfo> defectList;
        string model = string.Empty;
        string type = "PRD";
        MBInfo mbinfo = new MBInfo();
        try
        {
            model = iPCAOQCOutput.inputMBSno(mbsno, editor, station, customer, out mbinfo);
            defectList = iDefect.GetDefectList(type);
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(model);
            ret.Add(defectList);
            ret.Add(mbinfo);
            
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
    public string Cancel(string uutSn)
    {
        try
        {
            iPCAOQCOutput.Cancel(uutSn);
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
    public void save(string mbsno, IList<string> defectList)
    {
        try
        {
            iPCAOQCOutput.save(mbsno, defectList);
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

