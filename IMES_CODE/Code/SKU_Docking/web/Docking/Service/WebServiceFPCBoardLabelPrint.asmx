<%@ WebService Language="C#" Class="WebServiceFPCBoardLabelPrint" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
//using IMES.Station.Interface.StationIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;
using log4net;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceFPCBoardLabelPrint : System.Web.Services.WebService
{
    IFPCBoardLabelPrint iMBLabelPrint = ServiceAgent.getInstance().GetObjectByName<IFPCBoardLabelPrint>(WebConstant.FPCBoardLabelPrintDocking);

    [WebMethod]
    public ArrayList print(string pdLine, string month, string mbCode, string mo, string qty, string datecode, string editor, string station, string customer, string _111, string factor, IList<PrintItem> printItems, string labelType)
    {
        IList<String> snLst = new List<String>();
        IList<PrintItem> retPrintItemLst = null;
        ArrayList retLst = new ArrayList();
        int MultiQty = 0;
        try
        {
            retPrintItemLst = iMBLabelPrint.Print(pdLine, Convert.ToBoolean(month), mbCode, mo, int.Parse(qty), datecode, editor, station, customer, out snLst, out MultiQty, _111, factor, printItems);

            removePrintItem(labelType, retPrintItemLst);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retPrintItemLst);
            retLst.Add(snLst[0]);
            retLst.Add(snLst[1]);
            retLst.Add(MultiQty);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            retLst.Add(ex.Message);
        }
        return retLst;
    }
    

    [WebMethod]
    public ArrayList RePrint(string mbSno, string customerId, string reason, string editor, string stationId, IList<PrintItem> printItems, string labelType)
    {
        IList<PrintItem> retPrintItemLst = null;
        ArrayList retLst = new ArrayList();
        try
        {
            retPrintItemLst = iMBLabelPrint.RePrint(mbSno, customerId, reason, editor, stationId, printItems);

            removePrintItem(labelType, retPrintItemLst);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(mbSno);
            retLst.Add(retPrintItemLst);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            retLst.Add(ex.Message);
        }

        return retLst;        
    }

    [WebMethod]
    public void cancel(string mo)
    {
        try
        {
            iMBLabelPrint.Cancel(mo);
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

    private IList<PrintItem> removePrintItem(string labelType, IList<PrintItem> printItems)
    {
        PrintItem printItem = new PrintItem();

        if (printItems != null)
        {
            for (int i = 0; i < printItems.Count; i++)
            {
                if (printItems[i].LabelType == labelType)
                {
                    printItem = printItems[i];
                    break;
                }
            }

            printItems.Clear();

            if (printItem.LabelType != null)
            {
                printItems.Add(printItem);
            }
        }
        return printItems;
    }
}
