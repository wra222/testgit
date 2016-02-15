<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Service Agent Code for Final Scan page
 * UI:CI-MES12-SPEC-PAK-UI Final Scan.docx --2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Final Scan.docx --2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-10  Zhang.Kai-sheng       (Reference Ebook SourceCode) 
 * Known issues:
 */
 --%>
 
<%@ WebService Language="C#" Class="FinalScanService" %>


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

public class FinalScanService : System.Web.Services.WebService
{
    private IFinalScan iFinalScan;
    private string FinalScanBllName = WebConstant.FinalScanObject;
    private string commBllName = WebConstant.CommonObject;
    
    [WebMethod]
    ///---------------------------------------------------
    ///| Name		    :	inputPickID
    ///| Description	:	Get Forwarder, Driver, Truck ID, (Via input PickID
    ///| Input para.	:	
    ///| Ret value      :
    ///---------------------------------------------------
    public IList inputPickID(string pdLine, string pickID, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        
        try
        {
            iFinalScan = ServiceAgent.getInstance().GetObjectByName<IFinalScan>(FinalScanBllName);

            ret = iFinalScan.InputPickID(string.Empty, pickID, editor, station, customer);

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
    ///---------------------------------------------------
    ///| Name		    :	InputUCCID
    ///| Description	:	Handle Input UCC ID
    ///| Input para.	:	
    ///| Ret value      :
    ///---------------------------------------------------
    public IList InputUCCID(string pdLine, string pickID, string UCCID, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        try
        {
            iFinalScan = ServiceAgent.getInstance().GetObjectByName<IFinalScan>(FinalScanBllName);

            ret = iFinalScan.InputUCCID(pdLine, pickID, UCCID, editor, station, customer);
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
    ///---------------------------------------------------
    ///| Name		    :	InputPalletNo
    ///| Description	:	Handle Input pallet No
    ///| Input para.	:	
    ///| Ret value      :
    ///---------------------------------------------------
    public IList InputPalletNo(string pdLine, string pickID, string pltNo, string editor, string station, string customer)
    {
        try
        {
            iFinalScan = ServiceAgent.getInstance().GetObjectByName<IFinalScan>(FinalScanBllName);

            return iFinalScan.InputPalletNo(pdLine, pickID, pltNo, editor, station, customer);
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
    ///---------------------------------------------------
    ///| Name		    :	InputPalletNo
    ///| Description	:	Handle Input pallet No
    ///| Input para.	:	
    ///| Ret value      :InputChepPalletNo(emptyString, gKeyPickID, gKeyPalletNo, data, editor, stationId, customer
    ///---------------------------------------------------
    public IList InputChepPalletNo(string pdLine, string pickID, string pltNo, string ChepPltNo, string editor, string station, string customer)
    {
        try
        {
            iFinalScan = ServiceAgent.getInstance().GetObjectByName<IFinalScan>(FinalScanBllName);
           
            return iFinalScan.InputChepPalletNo(pdLine, pickID, pltNo,ChepPltNo, editor, station, customer);
            
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
    ///---------------------------------------------------
    ///| Name		    :	cancel
    ///| Description	:	Handle ancel operation
    ///| Input para.	:	
    ///| Ret value      :
    ///---------------------------------------------------
    public void cancel(string pickID)
    {
        try
        {
            iFinalScan = ServiceAgent.getInstance().GetObjectByName<IFinalScan>(FinalScanBllName);

            iFinalScan.Cancel(pickID);
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