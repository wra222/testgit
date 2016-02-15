<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Virtual MO
 * UI:CI-MES12-SPEC-FA-UI Virtual MO.docx 
 * UC:CI-MES12-SPEC-FA-UC Virtual MO.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-30   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	Add Qty of Virtual MO
 *                2.	Add New Virtual MO
 */
--%>
<%@ WebService Language="C#" Class="WebServiceVirtualMO" %>

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
public class WebServiceVirtualMO  : System.Web.Services.WebService {

    IVirtualMoForDocking iVirtualMO = ServiceAgent.getInstance().GetObjectByName<IVirtualMoForDocking>(WebConstant.VirtualMoForDocking);
    private const string SUCCESS = "SUCCESSRET";

    [WebMethod]
    public ArrayList create(string model, int qty, string pCode,string startDate, string editor, string stationId, string customer)
    {
        ArrayList ret = new ArrayList();
        string pdLine = string.Empty;
        startDate = startDate.Replace("-", "/");
        try
        {
            iVirtualMO.CreateNewVirtualMo(model, qty, pCode,startDate, editor, pdLine, stationId, customer);
            ret.Add(WebConstant.SUCCESSRET);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            return ret;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    [WebMethod]
    public string checkModel(string model)
    {
        try
        {
            iVirtualMO.checkModel(model);
            return WebConstant.SUCCESSRET;
        }
        catch (FisException e)
        {
            return e.mErrmsg;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    [WebMethod]
    public string checkModelinFamily(string family, string model)
    {
        try
        {
            iVirtualMO.checkModelinFamily(family, model);
            return WebConstant.SUCCESSRET;
        }
        catch (FisException e)
        {
            return e.mErrmsg;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    
    [WebMethod]
    public string autoDownloadMO(string jobname)
    {
        try
        {
            iVirtualMO.AutoDownloadMO_SP(jobname);
            return WebConstant.SUCCESSRET;
        }
        catch (FisException e)
        {
            return e.mErrmsg;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    [WebMethod]
    public ArrayList DeleteMO(string mo, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iVirtualMO.DeleteMo(mo, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(mo);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            return ret;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}

