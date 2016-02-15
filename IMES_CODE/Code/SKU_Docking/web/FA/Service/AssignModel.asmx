<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 Known issues:
 --%>

<%@ WebService Language="C#" Class="AssignModel" %>

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
public class AssignModel : System.Web.Services.WebService
{
    IAssignModel iAssignModel = ServiceAgent.getInstance().GetObjectByName<IAssignModel>(WebConstant.AssignModelObject);
    private const string SUCCESS = "SUCCESSRET";
    
    [WebMethod]
    public ArrayList ToAssignModel(string custsn, string line, string family, string model, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems)
    {
		ArrayList ret = new ArrayList();
		try
        {
			ArrayList arr = iAssignModel.ToAssignModel(custsn, line, family, model, pdLine, editor, stationId, customerId, printItems);
			ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]);
			ret.Add(arr[1]);
            return ret;
		}
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
	}
    
    [WebMethod]
    public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {

            ArrayList arr = iAssignModel.RePrint(sn, reason, line, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]);
            ret.Add(arr[1]);
            ret.Add(arr[2]);
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

