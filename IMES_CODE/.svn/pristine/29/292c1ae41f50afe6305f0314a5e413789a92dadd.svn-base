<%@ WebService Language="C#" Class="ChangeModelWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;
using System.Collections;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ChangeModelWebService : System.Web.Services.WebService
{
    [WebMethod]
    public ArrayList InputModel1(string model1, string editor, string line, string station, string customer)
    {
        ArrayList result = new ArrayList();
        try
        {
            IChangeModel CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeModel>(com.inventec.iMESWEB.WebConstant.ChangeModelObject);
            List<StationDescrQty> stationList = CurrentService.InputModel1(model1, editor, line, station, customer);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(stationList);
        }
        catch (FisException fe)
        {
            result.Add(fe.Message);
        }
        catch (Exception e)
        {
            result.Add(e.Message);
        }
        return result;
    }

    [WebMethod]
    public ArrayList InputModel2(string model1, string model2)
    {
        ArrayList result = new ArrayList();
        try
        {
            IChangeModel CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeModel>(com.inventec.iMESWEB.WebConstant.ChangeModelObject);
            CurrentService.InputModel2(model1, model2);
            result.Add(WebConstant.SUCCESSRET);
        }
        catch (FisException fe)
        {
            result.Add(fe.Message);
        }
        catch (Exception e)
        {
            result.Add(e.Message);
        }
        return result;
    }

    [WebMethod]
    public ArrayList Change(string model1, string selectStation, int changeQty)
    {
        ArrayList result = new ArrayList();
        try
        {
            IChangeModel CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeModel>(com.inventec.iMESWEB.WebConstant.ChangeModelObject);
            List<ProductModel> ProductIDCustSNTable = CurrentService.Change(model1, selectStation, changeQty);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(ProductIDCustSNTable);
        }
        catch (FisException fe)
        {
            result.Add(fe.Message);
        }
        catch (Exception e)
        {
            result.Add(e.Message);
        }
        return result;

    }

    [WebMethod]
    public void Cancel(string key)
    {
        IChangeModel CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeModel>(com.inventec.iMESWEB.WebConstant.ChangeModelObject);
        CurrentService.Cancel(key);

    }
}

