<%@ WebService Language="C#" Class="WebServiceChangeSamplePO" %>

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
public class WebServiceChangeSamplePO : System.Web.Services.WebService
{
    [WebMethod]
    public ArrayList InputSN1(string sn1, string editor, string line, string station, string customer)
    {
        ArrayList result = new ArrayList();
        try
        {
            IChangeSamplePO CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeSamplePO>(com.inventec.iMESWEB.WebConstant.ChangeSamplePOObject);
            ArrayList ret = CurrentService.InputSN1(sn1, editor, line, station, customer);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(ret[0]);
        }
        catch (FisException fe)
        {
            throw new Exception(fe.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }

    [WebMethod]
    public ArrayList InputSN2(string sn1, string sn2)
    {
        ArrayList result = new ArrayList();
        try
        {
            IChangeSamplePO CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeSamplePO>(com.inventec.iMESWEB.WebConstant.ChangeSamplePOObject);
            ArrayList ret = CurrentService.InputSN2(sn1, sn2);
            result.Add(WebConstant.SUCCESSRET);
			result.Add(ret[0]);
        }
        catch (FisException fe)
        {
            throw new Exception(fe.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }

    [WebMethod]
    public ArrayList Change(string sn1, string sn2)
    {
        ArrayList result = new ArrayList();
        try
        {
            IChangeSamplePO CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeSamplePO>(com.inventec.iMESWEB.WebConstant.ChangeSamplePOObject);
            ArrayList ret = CurrentService.Change(sn1, sn2);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(ret[0]);
			result.Add(ret[1]);
        }
        catch (FisException fe)
        {
            throw new Exception(fe.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;

    }

    [WebMethod]
    public void Cancel(string key)
    {
        IChangeSamplePO CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IChangeSamplePO>(com.inventec.iMESWEB.WebConstant.ChangeSamplePOObject);
        CurrentService.Cancel(key);

    }
}

