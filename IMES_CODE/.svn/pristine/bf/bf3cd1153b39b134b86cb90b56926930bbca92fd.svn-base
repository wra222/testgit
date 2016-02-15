
<%@ WebService Language="C#" Class="FARepairService" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class FARepairService : System.Web.Services.WebService
{
    private IFARepair iFARepair = ServiceAgent.getInstance().GetObjectByName<IFARepair>(WebConstant.FARepairObject);
    
    [WebMethod]
    public void update(string productId, string macValue, RepairInfo defect, string editor, string station, string customer)
    {
        try
        {
            iFARepair.Edit(productId, macValue, defect, station);
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
    public void add(string productId, string macValue, RepairInfo defect, string editor, string station, string customer)
    {
        try
        {
            iFARepair.Add(productId, macValue, defect, station);
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
    public ArrayList save(string prodId, string returnStation, string type)
    {
        ArrayList ret = new ArrayList();
        ArrayList serviceRet = new ArrayList();
        try
        {
            string setMsg = "";
            serviceRet = iFARepair.Save(prodId, returnStation, type, out setMsg);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(serviceRet);
            ret.Add(setMsg);
            return ret;
        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
            return ret;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public void wfcancel(string prodId)
    {
        try
        {
            iFARepair.WFCancel(prodId);
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
    public ArrayList GetFaultyPartSN(string prodId)
    {
        ArrayList ret = new ArrayList();
        string serviceRet = "";
        try
        {
            serviceRet = iFARepair.GetFaultyPartSN(prodId);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(serviceRet);
            return ret;
        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
            return ret;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }                
    
	[WebMethod]
    public ArrayList Print(string objIdDefect, string prodId, RepairInfo defect, IList<PrintItem> printItems, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        ArrayList serviceRet = new ArrayList();
        try
        {
            serviceRet = iFARepair.Print(objIdDefect, prodId, defect, printItems, line, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(serviceRet[0]);
			ret.Add(serviceRet[1]);
            return ret;
        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
            return ret;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
	
	[WebMethod]
    public ArrayList SaveFaultPartSno(string objIdDefect, string prodId, RepairInfo defect, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        ArrayList serviceRet = new ArrayList();
        try
        {
            serviceRet = iFARepair.SaveFaultPartSno(objIdDefect, prodId, defect, line, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(serviceRet[0]);
            return ret;
        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
            return ret;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}

