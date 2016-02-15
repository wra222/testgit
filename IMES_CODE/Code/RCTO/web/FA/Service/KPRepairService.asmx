<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Webservice for KP Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI KeyParts Repair.docx
 * UC:CI-MES12-SPEC-FA-UC KeyParts Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-26  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
--%>
<%@ WebService Language="C#" Class="KPRepairService" %>

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
public class KPRepairService : System.Web.Services.WebService
{
    private IKPRepair iKPRepair = ServiceAgent.getInstance().GetObjectByName<IKPRepair>(WebConstant.KPRepairObject);

    [WebMethod]
    public string update(string ctno, RepairInfo defect)
    {   
        try
        {
            int ret = iKPRepair.Edit(ctno, defect);

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
    public string add(string ctno, RepairInfo defect)
    {
        try
        {
            int ret = iKPRepair.Add(ctno, defect);

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
    public void save(string ctno)
    {
        try
        {
            iKPRepair.Save(ctno);
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
    public void cancel(string ctno)
    {
        try
        {
            iKPRepair.Cancel(ctno);
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

