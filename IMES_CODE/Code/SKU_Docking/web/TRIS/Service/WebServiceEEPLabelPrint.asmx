
<%@ WebService Language="C#" Class="WebServiceEEPLabelPrint" %>



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


//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebServiceEEPLabelPrint : System.Web.Services.WebService 
{

    //IGenerateCustomerSN iGenerateCustomerSN = ServiceAgent.getInstance().GetObjectByName<IGenerateCustomerSN>(WebConstant.GenerateCustomerSNObject);
    IEEPLabelPrint iEEPLabelPrint = ServiceAgent.getInstance().GetObjectByName<IEEPLabelPrint>(WebConstant.EEPLabelPrintObject);
    IProduct iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(WebConstant.CommonObject);
    [WebMethod]
    public string ChangeModelCheck(string ProdID,string ModelID)
    {      
        string Info=string.Empty;
        string isChangeM = string.Empty;
        try
        {
            ProductInfo pidInfo;
            pidInfo = iProduct.GetProductInfo(ProdID);
            Info = pidInfo.modelId;

            if ((Info=="")||(Info==null))
            {
                return "null";
            }
            if (Info == ModelID)
            {
                isChangeM = "N";
                return (isChangeM); 
            }
            else
            {
                isChangeM = "Y";
                return (isChangeM);
            }
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
    public ArrayList inputProdId(string pdLine, string prodId, string stationId, string PrintLogName, string editor, string customer, IList<PrintItem> printItems)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpLst = new ArrayList();      
        try
        {
            tmpLst = iEEPLabelPrint.InputProdId(pdLine, prodId, editor, stationId, PrintLogName, customer, printItems);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpLst[0]);//Product
            retLst.Add(tmpLst[1]);//printItems
        }
        catch (FisException ex)
        {
            //retLst.Add(ex.mErrmsg);
            throw new Exception(ex.mErrmsg);
        }
            
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst;
    }
 
    [WebMethod]
    public ArrayList Reprint(string prodId, string reason, string editor, string stationid, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList returnLst = new ArrayList();
        string customerSN = string.Empty;
        try
        {
            returnLst = iEEPLabelPrint.Reprint(prodId, reason,"", editor, stationid, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(returnLst[0]);//ProductModel
            ret.Add(returnLst[1]);//IList<PrintItem>  
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
    public string Cancel(string prodID)
    {
        try
        {
            iEEPLabelPrint.Cancel(prodID);
            return (WebConstant.SUCCESSRET);
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

