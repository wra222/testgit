<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: Japanese Label Print and RePrint Service Method
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-03-15  Chen Xu (eB1-4)      Create 
 
 Known issues:
 --%>
<%@ WebService Language="C#" Class="PrintInatelICASAService" %>

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
public class PrintInatelICASAService : System.Web.Services.WebService
{
    private IProduct iProduct;
    private string commBllName = WebConstant.CommonObject;
    PrintInatelICASA iPrintInatelICASA = ServiceAgent.getInstance().GetObjectByName<PrintInatelICASA>(WebConstant.PrintInatelICASAObject);
    private const string SUCCESS = "SUCCESSRET";
    
    [WebMethod]
    public ArrayList display(string pdLine, string customSn, IList<PrintItem> printItems, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        ProductInfo info;
        string preStation = string.Empty;
        try
        {
            string productID;
            var para = iPrintInatelICASA.InputProdId(pdLine, customSn, printItems, editor, station, customer);
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(commBllName);

            productID = (string)para[0];//Dean 20110312 由CustSN來取得ProductID                        
            preStation = (string)para[1];

            info = iProduct.GetProductInfo(productID);

            ret.Add(info);
            ret.Add(para[2]);
            ret.Add(para[3]);
            ret.Add(productID);
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
            var para = iPrintInatelICASA.rePrint(pdLine, productID, reason, printItems, editor, station, customer);
            ret.Add(SUCCESS);
            ret.Add(para[0]);
            ret.Add(para[1]);
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
    public void Cancel(string productId)
    {
        try
        {
            iPrintInatelICASA.Cancel(productId);
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

