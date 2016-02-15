
<%@ WebService Language="C#" Class="RCTOLabelReprint" %>

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
public class RCTOLabelReprint : System.Web.Services.WebService
{
    IRCTOLabelPrint iRCTOLabelReprint = ServiceAgent.getInstance().GetObjectByName<IRCTOLabelPrint>(WebConstant.RCTOLabelPrintObject);
    
    [WebMethod]
    public ArrayList Reprint(string prodid, string reason, string editor,  string station, string customer, string pCode, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {
            ret = iRCTOLabelReprint.Reprint(prodid, reason, editor, station, customer, pCode, printItems);
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


