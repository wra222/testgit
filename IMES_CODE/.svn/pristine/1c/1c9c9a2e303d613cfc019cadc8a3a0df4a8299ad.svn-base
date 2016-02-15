<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/FA/Service/TravelCardPrint
 * UI:CI-MES12-SPEC-FA-UI Travel Card Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC Travel Card Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-15   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* Exception控件
* UC/UI变更
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ WebService Language="C#" Class="WebServiceTravelCardExcel" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceTravelCardExcel : System.Web.Services.WebService
{
    ITravelCardPrintExcel iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrintExcel>(WebConstant.TravelCardPrintExcelObject);

    [WebMethod]
    public ArrayList PrintTCWithProductIDForBN(string pdLine, string model, string mo, int qty, bool IsNextMonth, 
                                string editor, string station, string customer, 
                                IList<PrintItem> printItemLst,string deliverydate,
                                string bomremark, string remark, string exception)
    {
        ArrayList retLst = new ArrayList();
        
        IList<String> snLst = null;
        IList<PrintItem> retPrintItemLst = null;
        ArrayList retPrintItemLstArray = new ArrayList();
        string battery=null;
        string lcm=null;
        deliverydate = deliverydate.Replace("-", "/");
        try
        {
            retPrintItemLst = iTravelCardPrint.PrintTCWithProductIDForBN(pdLine, model, mo, qty, IsNextMonth, editor, station, customer, out snLst,printItemLst,out battery,out lcm,deliverydate, bomremark, remark, exception);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retPrintItemLst);
            retLst.Add(model);
            retLst.Add(snLst);
            retLst.Add(battery);
            retLst.Add(lcm);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }


        return retLst;

    }
    
    /*
    public ArrayList PrintTCWithProductID(string pdLine, string model, string mo, int qty, string ecr, bool IsNextMonth, string editor, string station, string customer, IList<PrintItem> printItemLst)
    {
        ArrayList retLst = new ArrayList();

        IList<String> snLst = null;
        IList<PrintItem> retPrintItemLst = null;
        ArrayList retPrintItemLstArray = new ArrayList();

        try
        {
            retPrintItemLst = iTravelCardPrint.PrintTCWithProductID(pdLine, mo, qty, ecr, IsNextMonth, editor, station, customer, out snLst, printItemLst);
            retPrintItemLstArray.Add(retPrintItemLst);
            for (int i = 1; i < snLst.Count; i++)
            {
                IList<PrintItem> retPrintItemLst1 = new List<PrintItem>();
                for (int j = 0; j < retPrintItemLst.Count ; j++)
                {
                    PrintItem printItem = new PrintItem();
                    printItem.dpi = retPrintItemLst[j].dpi;
                    printItem.LabelType = retPrintItemLst[j].LabelType;
                    printItem.OffsetX = retPrintItemLst[j].OffsetX;
                    printItem.OffsetY = retPrintItemLst[j].OffsetY;
                    printItem.Piece = retPrintItemLst[j].Piece;
                    printItem.PrinterPort = retPrintItemLst[j].PrinterPort;
                    printItem.PrintMode = retPrintItemLst[j].PrintMode;
                    printItem.RuleMode = retPrintItemLst[j].RuleMode;
                    printItem.SpName = retPrintItemLst[j].SpName;
                    printItem.TemplateName = retPrintItemLst[j].TemplateName;
                    retPrintItemLst1.Add(printItem);
                    
                }
                retPrintItemLstArray.Add(retPrintItemLst1);
                
            }
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retPrintItemLstArray);
            retLst.Add(model);
            retLst.Add(snLst);

        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

       
        return retLst;

    }

    [WebMethod]
    public ArrayList PrintTCNoProductID(string pdLine, string model, int qty, string editor, string station, string customer, IList<PrintItem> printItemLst)
    {
        ArrayList retLst = new ArrayList();
        IList<PrintItem> retPrintItemLst = null;
        ArrayList retPrintItemLstArray = new ArrayList();
        try
        {

            retPrintItemLst = iTravelCardPrint.PrintTCNoProductID(pdLine, model, qty, editor, station, customer, printItemLst);
            retPrintItemLstArray.Add(retPrintItemLst);
            for (int i = 1; i < qty; i++)
            {
                IList<PrintItem> retPrintItemLst1 = new List<PrintItem>();
                for (int j = 0; j < retPrintItemLst.Count; j++)
                {
                    PrintItem printItem = new PrintItem();
                    printItem.dpi = retPrintItemLst[j].dpi;
                    printItem.LabelType = retPrintItemLst[j].LabelType;
                    printItem.OffsetX = retPrintItemLst[j].OffsetX;
                    printItem.OffsetY = retPrintItemLst[j].OffsetY;
                    printItem.Piece = retPrintItemLst[j].Piece;
                    printItem.PrinterPort = retPrintItemLst[j].PrinterPort;
                    printItem.PrintMode = retPrintItemLst[j].PrintMode;
                    printItem.RuleMode = retPrintItemLst[j].RuleMode;
                    printItem.SpName = retPrintItemLst[j].SpName;
                    printItem.TemplateName = retPrintItemLst[j].TemplateName;
                    retPrintItemLst1.Add(printItem);

                }
                retPrintItemLstArray.Add(retPrintItemLst1);

            }
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retPrintItemLstArray);
            retLst.Add(model);

        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

       
        return retLst;

    }
    */  
}


