
<%@ WebService Language="C#" Class="CooLabelWebService" %>

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
using IMES.Docking.Interface.DockingIntf;
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CooLabelWebService : System.Web.Services.WebService
{

    ICooLabel iCooLabel = ServiceAgent.getInstance().GetObjectByName<ICooLabel>(WebConstant.CooLabel);
    ICombinePoInCarton CombinePoInCarton = ServiceAgent.getInstance().GetObjectByName<ICombinePoInCarton>(WebConstant.CombinePoInCartonObject);

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
            backPrintItems = iCooLabel.PrintCOOLabel(custSN, string.Empty, editor, station, customer, items);
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
    /// reprint coo
    /// </summary>
    /// <param name="reason"></param>
    /// <param name="custSN"></param>
    /// <param name="editor"></param>
    /// <param name="station"></param> 
    /// <param name="customer"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    [WebMethod]
    public IList ReprintCOO(string reason, string custSN, string editor, string station, string customer, string pcode,IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        IList<PrintItem> backPrintItems = null;

        try
        {
            string site = CombinePoInCarton.GetSysSetting("Site");
            backPrintItems = iCooLabel.RePrintCOO(reason, custSN, string.Empty, editor, station, customer,pcode,items);
            string check = "CN";
            if (site == "ICC")
            {
                check = "5CG";
            }

            if (custSN.IndexOf(check) == 0)
            {
                S_CooLabel product = iCooLabel.GetProductBySN(custSN, station);
                ret.Add(product.ProductID);
            }
            else
            {
                ret.Add(custSN);
            }
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
}