<%@ WebService Language="C#" Class="WebServiceHPPNLabelforRCTO" %>


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
public class WebServiceHPPNLabelforRCTO : System.Web.Services.WebService
{
    private IProduct iProduct;
    private string commBllName = WebConstant.CommonObject;
    IHPPNLabelforRCTO iHPPNLabelforRCTO = ServiceAgent.getInstance().GetObjectByName<IHPPNLabelforRCTO>(WebConstant.HPPNLabelforRCTOObject);
    private const string SUCCESS = "SUCCESSRET";
    [WebMethod]
    public ArrayList display(string productID, string model, string HPPN, IList<PrintItem> printItems, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        ProductInfo info;
        string preStation = string.Empty;
        try
        {
            var para = iHPPNLabelforRCTO.InputProdId(productID, model, HPPN, printItems, editor, station, customer);
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(commBllName);

            productID = (string)para[0];
            preStation = (string)para[1];

            info = iProduct.GetProductInfo(productID);
            
            ret.Add(para[1]);
            ret.Add(para[2]);
            ret.Add(para[3]);
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
    public ArrayList GetData(string productID, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        ProductInfo info;
        string preStation = string.Empty;
        try
        {
            var para = iHPPNLabelforRCTO.GetProductInfo(productID, editor, station, customer);
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(commBllName);

            productID = (string)para[0];
            preStation = (string)para[1];

            info = iProduct.GetProductInfo(productID);

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
            iHPPNLabelforRCTO.Cancel(productId);
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


