<%@ WebService Language="C#" Class="WebServiceChangeToEPIAPIA" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceChangeToEPIAPIA : System.Web.Services.WebService
{
    private ChangeToEPIAPIA iChangeToEPIAPIA;
    private Object ChangeToEPIAPIAServiceObj;
    private string commBllName = WebConstant.CommonObject;
    private string ChangeToEPIAPIABllName = WebConstant.ChangeToEPIAPIAObject;
    private IProduct iProduct;
    private IDefect iDefect;
    
    [WebMethod]
    public IList input(string pdLine, string prodId, string editor, string station, string customer,bool isChangetoEpia)
    {
        IList ret = new ArrayList();
        ProductInfo info;
        ProductStatusInfo statusInfo;
        string outQCStatus = string.Empty;
        string preStation = string.Empty;

        try
        {
            ChangeToEPIAPIAServiceObj = ServiceAgent.getInstance().GetObjectByName<ChangeToEPIAPIA>(ChangeToEPIAPIABllName);
            iChangeToEPIAPIA = (ChangeToEPIAPIA)ChangeToEPIAPIAServiceObj;
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(commBllName);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(commBllName);

            List<string> para = iChangeToEPIAPIA.InputProdId(pdLine, prodId, editor, station, customer, isChangetoEpia,out outQCStatus);
            prodId = para[0];                    
            preStation = para[1];

            info = iProduct.GetProductInfo(prodId);
            statusInfo = iProduct.GetProductStatusInfo(prodId);
            outQCStatus = Convert.ToString(statusInfo.status);
            
            ret.Add(info);
            ret.Add(statusInfo);
            //ret.Add(preStation);
            ret.Add(outQCStatus);

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


