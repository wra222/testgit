<%@ WebService Language="C#" Class="WebServicePCAOQCCosmetic" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
using log4net;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePCAOQCCosmetic : System.Web.Services.WebService
{
    IPCAOQCCosmeticForDocking iPCAOQCCosmetic = ServiceAgent.getInstance().GetObjectByName<IPCAOQCCosmeticForDocking>(WebConstant.PCAOQCCosmeticForDocking);
    

    [WebMethod]
    public ArrayList ProcessMBSN(string mbsn, string line, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serviceLst = new ArrayList();

        try
        {
            serviceLst = iPCAOQCCosmetic.CheckAndGetMBInfo(mbsn, line, editor, station, customer);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(mbsn);
            retLst.Add(serviceLst);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }       
        return retLst;      
    }

    [WebMethod]
    public ArrayList ProcessDefect(string code)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serviceLst = new ArrayList();
        
        try
        {
            serviceLst = iPCAOQCCosmetic.GetDefectInfo(code);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(code);
            retLst.Add(serviceLst);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }

    [WebMethod]
    public void wfcancel(string mbsn)
    {
        try
        {
            iPCAOQCCosmetic.WFCancel(mbsn);
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
    public ArrayList save(string key, string mbsn, string lotNo, string remark, string check, IList<string> list)
    {
        ArrayList retLst = new ArrayList();

        try
        {
            iPCAOQCCosmetic.Save(key, mbsn, lotNo, remark, check, list);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(check);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }


    [WebMethod]
    public ArrayList query(string none)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serviceLst = new ArrayList();

        try
        {
            serviceLst = iPCAOQCCosmetic.Query();
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(serviceLst);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }            
}
