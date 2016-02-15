<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: master Label Print and RePrint Service Method
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-03-15  Chen Xu (eB1-4)      Create 
 
 Known issues:
 --%>
<%@ WebService Language="C#" Class="MasterLabelPrintService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;
//using System.Web.Services.Protocols;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MasterLabelPrintService : System.Web.Services.WebService
{
    private IProduct iProduct;
    private string commBllName = WebConstant.CommonObject;
    MasterLabelPrint iMasterLabelPrint = ServiceAgent.getInstance().GetObjectByName<MasterLabelPrint>(WebConstant.MasterLabelPrintObject);
    private const string SUCCESS = "SUCCESSRET";
    [WebMethod]
    public ArrayList display(string pdLine,string productID,IList<PrintItem> printItems, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        ProductInfo info;
        string preStation = string.Empty;
        try
        {
            var para = iMasterLabelPrint.InputProdId(pdLine, productID, printItems, editor, station, customer);
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(commBllName);

            productID =(string) para[0];
            preStation = (string)para[1];

            info = iProduct.GetProductInfo(productID);
            ret.Add(info);
            ret.Add(para[2]);
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
    public ArrayList rePrint(string pdLine, string productID, string reason, IList<PrintItem> printItems, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        string preStation = string.Empty;
        try
        {
            var para = iMasterLabelPrint.rePrint(pdLine, productID, reason,printItems, editor, station, customer);
            ret.Add(SUCCESS);
            ret.Add(para[0]);
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
    public void Cancel(string productId)
    {
        try
        {
            iMasterLabelPrint.Cancel(productId);
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

