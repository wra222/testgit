
<%@ WebService Language="C#" Class="CombineCOAandDNWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Collections.Generic;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using com.inventec.iMESWEB;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CombineCOAandDNWebService : System.Web.Services.WebService
{

    ICombineCOAandDN iCombineCOAandDN = ServiceAgent.getInstance().GetObjectByName<ICombineCOAandDN>(WebConstant.CombineCOAandDNObject);

    /// <summary>
    /// print pizza Label
    /// </summary>
    /// <param name="custSN"></param>
    /// <param name="editor"></param>
    /// <param name="customer"></param>
    /// <param name="station"></param>
    /// <param name="reason"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    [WebMethod]
    public IList PrintPizzaLabel(string custSN, string editor,  string station,string customer , IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        IList<PrintItem> backPrintItems = null;

        //string out_proID = string.Empty;
        //string out_model = string.Empty;

        try
        {
            backPrintItems = iCombineCOAandDN.PrintPizzaLabel(custSN, "", editor, station, customer, items);
            ret.Add(custSN);
            ret.Add(backPrintItems);

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
    /// <summary>
    /// print pizza Label
    /// </summary>
    /// <param name="custSN"></param>
    /// <param name="editor"></param>
    /// <param name="customer"></param>
    /// <param name="station"></param>
    /// <param name="reason"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    [WebMethod]
    public IList PrintPizzaLabelFinal(string custSN, string editor, string station, string customer, IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        IList<PrintItem> backPrintItems = null;

        //string out_proID = string.Empty;
        //string out_model = string.Empty;

        try
        {
            backPrintItems = iCombineCOAandDN.PrintPizzaLabelFinal(custSN, "", editor, station, customer, items);
            if (custSN.IndexOf("#@#") != -1)
            {
                custSN = custSN.Substring(0, custSN.IndexOf("#@#"));
            }
            if (null == backPrintItems)
            {
                ret.Add("failedSession");
            }
            else
            {
                ret.Add(custSN);
                ret.Add(backPrintItems);
            }

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
    /// <summary>
    /// print COO Label
    /// </summary>
    /// <param name="custSN"></param>
    /// <param name="editor"></param>
    /// <param name="customer"></param>
    /// <param name="station"></param>
    /// <param name="reason"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    [WebMethod]
    public IList PrintCOOLabel(string custSN, string editor, string station, string customer, IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        IList<PrintItem> backPrintItems = null;
       
        try
        {
            backPrintItems = iCombineCOAandDN.PrintCOOLabel(custSN, string.Empty, editor, station, customer, items);
            ret.Add(custSN);
            ret.Add(backPrintItems);

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
    /// <summary>
    /// reprint 
    /// </summary>
    /// <param name="reason"></param>
    /// <param name="custSN"></param>
    /// <param name="editor"></param>
    /// <param name="customer"></param>
    /// <param name="station"></param>
    /// <param name="reason"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    [WebMethod]
    public IList Reprint(string reason, string custSN, string editor, string station, string customer, IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        IList<PrintItem> backPrintItems = null;
        S_RowData_Product temp = new S_RowData_Product();
        try
        {
            backPrintItems = iCombineCOAandDN.RePrint(reason,custSN, string.Empty, editor, station, customer, items);
            temp = iCombineCOAandDN.GetProductOnly(custSN);
            ret.Add(custSN);
            ret.Add(backPrintItems);
            ret.Add(temp.Model);
            ret.Add(temp.isBT);
            ret.Add(temp.isJapan);
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
    /// <summary>
    /// reprint coo
    /// </summary>
    /// <param name="reason"></param>
    /// <param name="custSN"></param>
    /// <param name="editor"></param>
    /// <param name="customer"></param>
    /// <param name="station"></param>
    /// <param name="reason"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    [WebMethod]
    public IList ReprintCOO(string reason, string custSN, string editor, string station, string customer, IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        IList<PrintItem> backPrintItems = null;

        try
        {
            backPrintItems = iCombineCOAandDN.RePrintCOO(reason,custSN, string.Empty, editor, station, customer, items);
            ret.Add(custSN);
            ret.Add(backPrintItems);

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
    public ArrayList GetDNInfo(string DN)
    {
        ArrayList ret = new ArrayList();
        try
        {
            S_RowData_COAandDN info = new S_RowData_COAandDN();
            info = iCombineCOAandDN.GetADN(DN);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(info.CustomerPN);
            ret.Add(info.Date);
            ret.Add(info.DeliveryNO);
            ret.Add(info.Model);
            ret.Add(info.PackedQty);
            ret.Add(info.PoNo);
            ret.Add(info.Qty);
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
    public ArrayList Start(ArrayList getResult)
    {
        ArrayList ret = new ArrayList();
        try
        {
            string temp = iCombineCOAandDN.Start();
            if (temp == "")
            { 
                
            }
            return getResult;
        }
        catch (FisException ex)
        {
            
            ret.Add(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
    /// <summary>
    /// UpdateAndPrint
    /// </summary>
    /// <param name="line"></param>
    /// <param name="userId"></param>
    /// <param name="station"></param>
    /// <param name="customer"></param>
    /// <param name="dn"></param>
    /// <param name="sn"></param>
    /// <param name="coa"></param>
    /// <param name="printItems"></param> 
    /// <returns></returns>
    [WebMethod]
    public ArrayList UpdateAndPrint(string line, string userId, string station, string customer, string dn, string sn, string coa, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList tmp = new ArrayList();
        try
        {
            tmp = iCombineCOAandDN.UpdateDeliveryStatusAndPrint(line, userId, station, customer, dn, sn, coa, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmp[0]);//printList
            ret.Add(tmp[1]);//jpflag
            ret.Add(tmp[2]);
			ret.Add(tmp[3]);//bsamLocId
            ret.Add(tmp[4]); //QC result;
            
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
    
}