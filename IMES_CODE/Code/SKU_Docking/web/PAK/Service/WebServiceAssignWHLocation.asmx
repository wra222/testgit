<%@ WebService Language="C#" Class="WebServiceAssignWHLocation" %>


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

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]

public class WebServiceAssignWHLocation : System.Web.Services.WebService
{
    IAssignWHLocation iAssignWHLocation = ServiceAgent.getInstance().GetObjectByName<IAssignWHLocation>(WebConstant.AssignWHLocationObject);
    
    [WebMethod]
    public string HelloWorld() {
        //throw new FieldAccessException("errormess");
        return "Hello World3";
    }


    [WebMethod]
    public ArrayList inputProdId(string floor, string pdLine, string customerSn, string editor, string stationId, string customerId, string pre_model)
    {
        ArrayList retLst = new ArrayList();
        
        try
        {
            retLst = iAssignWHLocation.InputProdId(floor, pdLine, customerSn, editor, stationId, customerId, pre_model);
            //retLst.Add(WebConstant.SUCCESSRET);
            //retLst.Add(prodId);
        }
        catch (FisException e)
        {
            //retLst.Clear();
            //retLst.Add(e.mErrmsg);
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;

    }

    [WebMethod]
    public ArrayList closeLocation(string floor, string pdLine, string customerSn, string editor, string station, string customerId, string location)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            iAssignWHLocation.closeLocation(floor, pdLine, customerSn, editor, station, customerId, location);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(customerSn);
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
            //retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }


        return retLst;

    }

    [WebMethod]
    public ArrayList Cancel(string MB)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iAssignWHLocation.Cancel(MB);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(MB);

        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
  
}

