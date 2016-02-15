
<%@ WebService Language="C#" Class="WebServiceFRULabelPrint" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceFRULabelPrint : System.Web.Services.WebService
{
    private IFRULabelPrint iFRULabelPrint;
    //private string PickCardBllName = WebConstant.PickCardImplObject;

    [WebMethod]
    public ArrayList CheckModel(string model)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iFRULabelPrint = ServiceAgent.getInstance().GetObjectByName<IFRULabelPrint>(WebConstant.FRULabelPrintObject);
            ArrayList check = iFRULabelPrint.CheckModel(model);
            ret.Add(model);
            ret.Add(check[0]);
            ret.Add(check[1]);
            ret.Add(check[2]);
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
    public ArrayList InputModel(string model, string editor, string station, string customer, IList<PrintItem> items)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            //ArrayList list = new ArrayList();
            iFRULabelPrint = ServiceAgent.getInstance().GetObjectByName<IFRULabelPrint>(WebConstant.FRULabelPrintObject);
            //list = iFRULabelPrint.InputModel(model, editor, station, customer, items);

            return iFRULabelPrint.InputModel(model, editor, station, customer, items);
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
    public void cancel(string truckID)
    {
        try
        {
            iFRULabelPrint = ServiceAgent.getInstance().GetObjectByName<IFRULabelPrint>(WebConstant.FRULabelPrintObject);
            iFRULabelPrint.Cancel(truckID);
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
