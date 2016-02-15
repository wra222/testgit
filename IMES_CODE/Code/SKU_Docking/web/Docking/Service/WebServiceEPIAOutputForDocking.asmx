<%--
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: WebServicePIAOutput
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2009-11-09  Tong.Zhi-Yong         Create   
               
 * Known issues:
 */
 --%>
<%@ WebService Language="C#" Class="WebServicePIAOutput" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePIAOutput : System.Web.Services.WebService
{
    private EPIAOutput iPIAOutput;
    private Object oqcOutputServiceObj;
    private string commBllName = WebConstant.CommonObject;
    private string EPiaOutputBllName = WebConstant.EPIAOutputObject;
    private IProduct iProduct;
    private IDefect iDefect;
    private string type = "PRD";
    
    [WebMethod]
    public IList input(string pdLine, string prodId, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ProductInfo info;
        ProductStatusInfo statusInfo;
        IList<DefectInfo> defectList;
        string outQCStatus = string.Empty;
        string preStation = string.Empty;
        string custsn = "";
        
        try
        {
            oqcOutputServiceObj = ServiceAgent.getInstance().GetObjectByName<EPIAOutput>(EPiaOutputBllName);
            custsn = prodId;
            iPIAOutput = (EPIAOutput)oqcOutputServiceObj;
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(commBllName);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(commBllName);

            List<string> para = iPIAOutput.InputProdId(pdLine, prodId, editor, station, customer, out outQCStatus);
            prodId = para[0];                  
            preStation=para[1];
                                                                        
            info = iProduct.GetProductInfo(prodId);
            statusInfo = iProduct.GetProductStatusInfo(prodId);
            defectList = iDefect.GetDefectList(type);
            //outQCStatus = Convert.ToString(statusInfo.status);
            outQCStatus = para[2];
                
            ret.Add(info);
            ret.Add(statusInfo);
            ret.Add(defectList);
            ret.Add(preStation);
            ret.Add(outQCStatus);
            ret.Add(custsn);
          
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
    public void save(string prodId, IList<string> defectList,bool isInputDefet,string qcstatus)
    {
        try
        {
            oqcOutputServiceObj = ServiceAgent.getInstance().GetObjectByName<EPIAOutput>(EPiaOutputBllName);

            iPIAOutput = (EPIAOutput)oqcOutputServiceObj;

            iPIAOutput.InputDefectCodeList(prodId, defectList, isInputDefet, qcstatus);
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
    public void cancel(string productId)
    {
        try
        {
            oqcOutputServiceObj = ServiceAgent.getInstance().GetObjectByName<EPIAOutput>(EPiaOutputBllName);
            iPIAOutput = (EPIAOutput)oqcOutputServiceObj;
            iPIAOutput.Cancel(productId);
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

