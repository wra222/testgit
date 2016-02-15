﻿<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PCA Repair Page
 *             
 * UI:CI-MES12-SPEC-SA-UI PCA Repair For Docking.docx
 * UC:CI-MES12-SPEC-SA-UC PCA Repair For Docking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-12  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
--%>
<%@ WebService Language="C#" Class="PCARepairService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Docking.Interface.DockingIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PCARepairService : System.Web.Services.WebService
{
    private IPCARepair iPCARepair = ServiceAgent.getInstance().GetObjectByName<IPCARepair>(WebConstant.PCARepairForDockingObject);

    [WebMethod]
    public string update(string mbsno, RepairInfo defect)
    {   
        try
        {
            int ret = iPCARepair.Edit(mbsno, defect);

            return ret.ToString();
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
    public string add(string mbsno, RepairInfo defect)
    {
        try
        {
            int ret = iPCARepair.Add(mbsno, defect);

            return ret.ToString();
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
    public void save(string mbsno)
    {
        try
        {
            iPCARepair.Save(mbsno);
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
    public void cancel(string mbsno)
    {
        try
        {
            iPCARepair.Cancel(mbsno);
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

