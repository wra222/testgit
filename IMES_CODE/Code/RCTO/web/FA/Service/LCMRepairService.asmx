<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WebService for LCM Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RCTO LCM Repair.docx
 * UC:CI-MES12-SPEC-FA-UC RCTO LCM Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-16  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
--%>
<%@ WebService Language="C#" Class="LCMRepairService" %>

using System;
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
public class LCMRepairService : System.Web.Services.WebService
{
    private ILCMRepair iLCMRepair = ServiceAgent.getInstance().GetObjectByName<ILCMRepair>(WebConstant.LCMRepairObject);

    [WebMethod]
    public string update(string id, RepairInfo defect)
    {   
        try
        {
            int ret = iLCMRepair.Edit(id, defect);

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
    public string add(string id, RepairInfo defect)
    {
        try
        {
            int ret = iLCMRepair.Add(id, defect);

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
    public void save(string id)
    {
        try
        {
            iLCMRepair.Save(id);
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
    public void cancel(string id)
    {
        try
        {
            iLCMRepair.Cancel(id);
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

