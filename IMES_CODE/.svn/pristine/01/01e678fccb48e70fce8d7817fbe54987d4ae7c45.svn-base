<%@ WebService Language="C#" Class="RegenerateCustSN" %>


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
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class RegenerateCustSN : System.Web.Services.WebService
{
    private RegenerateCustSNForDocking iRegenerateCustSN;
    private Object RegenerateCustSNServiceObj;
    private string commBllName = WebConstant.CommonObject;
    private string RegenerateCustSNObjectBllName = WebConstant.RegenerateCustSNForDockingObject;
    private IProduct iProduct;
   
    [WebMethod]
    public ArrayList input(string pdLine, string prodId, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();

        try
        {
            RegenerateCustSNServiceObj = ServiceAgent.getInstance().GetObjectByName<RegenerateCustSNForDocking>(RegenerateCustSNObjectBllName);
            iRegenerateCustSN = (RegenerateCustSNForDocking)RegenerateCustSNServiceObj;
            ArrayList retList = new ArrayList();

            retList = iRegenerateCustSN.InputProdId(pdLine, prodId, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retList);
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


