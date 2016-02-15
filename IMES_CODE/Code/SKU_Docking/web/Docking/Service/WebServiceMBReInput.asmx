<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:PCA ICT INPUT-04 MB ReInput
 * UI:CI-MES12-SPEC-SA-UI PCA ICT Input.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA ICT Input.docx         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-18   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  重流板子
 */
--%>

<%@ WebService Language="C#" Class="WebServiceMBReInput" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceMBReInput : System.Web.Services.WebService
{

    IICTInput iICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.ICTInputDocking);
    
    [WebMethod]
    public ArrayList InputMBSno(string pdLine, string mbsno, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        MBInfo mbinfo = new MBInfo();
        string mbct = string.Empty;
        try
        {
            mbinfo= iICTInput.MBReinputInputMBSno(mbsno, editor, pdLine, station,customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(mbinfo.dateCode);
            ret.Add(mbinfo.ecr);
            ret.Add(mbinfo.mac);
            bool a = mbinfo.properties.TryGetValue("MBCT", out mbct);
            if (a)
            {
                if (!string.IsNullOrEmpty(mbct))
                {
                    ret.Add(mbct);
                }
                else ret.Add("");
            }
            else
            {
                ret.Add("");
            }
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
            iICTInput.Cancel(uutSn);
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
    public ArrayList reInput(string pdLine, string mbsno, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iICTInput.MBReinputSave(mbsno, editor, pdLine, station, customer);
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
}

