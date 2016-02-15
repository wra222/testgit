<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for QC Repair Page
 * UI:CI-MES12-SPEC-FA-UI QC Repair.docx –2011/10/19 
 * UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2011/10/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
 --%>
<%@ WebService Language="C#" Class="OQCRepairService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using IMES.DataModel;
using System.Collections.Generic;
using System.Collections;


//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class OQCRepairService : System.Web.Services.WebService
{
    private IOQCRepair iOQCRepair = ServiceAgent.getInstance().GetObjectByName<IOQCRepair>(WebConstant.OQCRepairObject);
    
    [WebMethod]
    public void update(string productId, RepairInfo defect, string editor, string station, string customer)
    {
        try
        {
            iOQCRepair.Edit(productId, defect);
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
    public void add(string productId, RepairInfo defect, string editor, string station, string customer)
    {
        try
        {
            iOQCRepair.Add(productId, defect);
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
    /* 2012-7-5，新需求
    public void save(string prodId)
    {
        try
        {
            iOQCRepair.Save(prodId);
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
    */
    /* 2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
    public ArrayList save(string prodId)
    {
        ArrayList ret = new ArrayList();
        IList<string> retLst = new List<string>();
        
        try
        {
            retLst = iOQCRepair.Save(prodId);
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst[0]);
            ret.Add(retLst[1]);
            ret.Add(retLst[2]);
            ret.Add(retLst[3]);
            ret.Add(retLst[4]);
            ret.Add(retLst[5]);
            ret.Add(retLst[6]);
            ret.Add(retLst[7]);
            //2012-7-18
            ret.Add(retLst[8]);
            ret.Add(retLst[9]);
            ret.Add(retLst[10]);
            //2012-7-19
            ret.Add(retLst[11]);

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
    */
    public ArrayList save(string prodId, string returnStation, string returnStationText)
    {
        ArrayList ret = new ArrayList();
        IList<string> retLst = new List<string>();

        try
        {
            retLst = iOQCRepair.Save(prodId, returnStation, returnStationText);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst[0]);
            ret.Add(retLst[1]);
            ret.Add(retLst[2]);
            ret.Add(retLst[3]);
            ret.Add(retLst[4]);
            ret.Add(retLst[5]);
            ret.Add(retLst[6]);
            ret.Add(retLst[7]);
            //2012-7-18
            ret.Add(retLst[8]);
            ret.Add(retLst[9]);
            ret.Add(retLst[10]);
            //2012-7-19
            ret.Add(retLst[11]);

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
    public void cancel(string prodId)
    {
        try
        {
            iOQCRepair.Cancel(prodId);
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

