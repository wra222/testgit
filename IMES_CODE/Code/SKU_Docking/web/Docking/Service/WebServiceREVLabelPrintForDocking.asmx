<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for REV Label Print For Docking Page
 * UI:CI-MES12-SPEC-FA-UI REV Label Print For Docking.docx –2012/5/28 
 * UC:CI-MES12-SPEC-FA-UC REV Label Print For Docking.docx –2012/5/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-30   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
 --%>
 
<%@ WebService Language="C#" Class="WebServiceREVLabelPrintForDocking" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceREVLabelPrintForDocking : System.Web.Services.WebService
{
    IREVLabelPrintForDocking iREVLabelPrintForDocking = ServiceAgent.getInstance().GetObjectByName<IREVLabelPrintForDocking>(WebConstant.REVLabelPrintForDockingObject);
    
    [WebMethod]
    public ArrayList print(string family, string dcode, int qty, string stationId, string editor, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList retLst = new ArrayList();

        try
        {
            retLst = iREVLabelPrintForDocking.Print(family, dcode, qty, stationId, editor, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
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
    public string Cancel(string key)
    {
        try
        {
            iREVLabelPrintForDocking.Cancel(key);

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
