<%--
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/Docking/Service/ProdIdPrint
 * UI:CI-MES12-SPEC-FA-UI ProdId Print For Docking.docx –2012/5/22 
 * UC:CI-MES12-SPEC-FA-UC ProdId Print For Docking.docx –2012/5/22            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-22   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
--%>
<%@ WebService Language="C#" Class="WebServiceProdIdPrint" %>


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
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceProdIdPrint : System.Web.Services.WebService
{
    IProdIdPrintForDocking iProdIdPrint = ServiceAgent.getInstance().GetObjectByName<IProdIdPrintForDocking>(WebConstant.ProdIdPrintForDocking);

    [WebMethod]
    public ArrayList PrintProdIdForDocking(string pdLine, string model, string mo, int qty, bool IsNextMonth, 
                                string editor, string station, string customer, string ecr,
                                IList<PrintItem> printItemLst)
    {
        ArrayList retLst = new ArrayList();
        
        IList<String> snLst = null;
        IList<PrintItem> retPrintItemLst = null;
        ArrayList retPrintItemLstArray = new ArrayList();
        string battery=null;
        string lcm=null;
        
        try
        {
            retPrintItemLst = iProdIdPrint.ProductIdPrint(pdLine, model, mo, qty, IsNextMonth, editor, station, customer, ecr, out snLst, printItemLst);
            
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
}


