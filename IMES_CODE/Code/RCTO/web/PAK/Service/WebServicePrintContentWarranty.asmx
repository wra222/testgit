<%@ WebService Language="C#" Class="WebServicePrintContentWarranty" %>


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
public class WebServicePrintContentWarranty : System.Web.Services.WebService
{
    IPrintContentWarranty printcw = ServiceAgent.getInstance().GetObjectByName<IPrintContentWarranty>(WebConstant.IPrintContentWarranty);
    
    IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    IMB iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);

    
    [WebMethod]
    public ArrayList inputCustomerSN(string customerSN, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpList = new ArrayList();
        
        string productID = String.Empty;
        string customerPN = String.Empty;
        string warranty = String.Empty;
        string configuration = String.Empty;
        string assetCheck = String.Empty;

        string line = string.Empty;
        
        try
        {
            //customerSN = "30012444Q";
            tmpList = printcw.InputCustomerSN(customerSN, line, editor, station, customer);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpList);

            /*retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add("productID");
            retLst.Add("customerPN");
            retLst.Add("warranty");
            retLst.Add("configuration");
            retLst.Add("assetCheck");
            retLst.Add("printflag");
             * 
            retList.Add(assetMessage)
            //Add kaisheng,2012/07/21
            retList.Add(WarrantyPrintFlag);
            */

        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
        return retLst;


    }

    [WebMethod]
    public ArrayList print(string productID, Boolean colprint, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> WprintItemLst = new List<PrintItem>();
        //IList<PrintItem> CprintItemLst = new List<PrintItem>();
        string assetCheck = string.Empty;
        try
        {

            WprintItemLst = printcw.WarrantyPrint(productID, colprint, printItems);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(WprintItemLst);

            /*if (colprint)
            {
                CprintItemLst = printcw.ConfigurationPrint(productID, printItems);
                ret.Add(CprintItemLst);
            }
            */
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
    public void cancel(string productId)
    {
        try
        {
            printcw.Cancel(productId);
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
    public ArrayList ReprintConfigurationLabel(string customerSN, string reason, IList<PrintItem> printItems,
                    string line, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpList = new ArrayList();

        try
        {
            tmpList = printcw.ReprintConfigurationLabel(customerSN, reason,printItems,line, editor, station, customer);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpList[0]);
            return retLst;
            
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
    public ArrayList ReprintWarrantyLabel(string customerSN, string reason, IList<PrintItem> printItems,
                    string line, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpList = new ArrayList();

        try
        {
            tmpList = printcw.ReprintWarrantyLabel(customerSN, reason, printItems, line, editor, station, customer);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpList[0]);

            /*retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add("productID");
            retLst.Add("customerPN");
            retLst.Add("warranty");
            retLst.Add("configuration");
            retLst.Add("assetCheck");
            */
            return retLst;

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
