<%@ WebService Language="C#" Class="WebServiceITCNDCheck" %>


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
public class WebServiceITCNDCheck  : System.Web.Services.WebService
{
    IITCNDCheck iITCNDCheck = ServiceAgent.getInstance().GetObjectByName<IITCNDCheck>(WebConstant.ITCNDCheckObject);

    [WebMethod]
    public ArrayList CheckImageDL(IList<PrintItem> printItemLst, string pdline, string prodid, string station, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService = new ArrayList();
        IList<string> res = new List<string>();
        try
        {
            retService = iITCNDCheck.CheckImageDL(printItemLst, pdline, prodid, station, editor, customer);
                retLst.Add(WebConstant.SUCCESSRET);
                retLst.Add(retService);            
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
            if (e.mErrcode == "CHK832" || e.mErrcode == "CHK833")
            {
                retLst.Add("CheckFail");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
    }
    [WebMethod]
    public ArrayList RePrint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {

            ret = iITCNDCheck.Reprint(prodid, reason, line, editor, station, customer, pCode, printItems);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(ret);

            return result;
        }
        catch (FisException e)
        {
            result.Add(e.mErrmsg);
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}
