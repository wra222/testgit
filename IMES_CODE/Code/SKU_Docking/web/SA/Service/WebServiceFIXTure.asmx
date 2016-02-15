<%@ WebService Language="C#" CodeBehind="~/App_Code/WebServiceFIXTure.cs" Class="WebServiceFIXTure" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceFIXTure : System.Web.Services.WebService
{
    [WebMethod]
    public ArrayList Save(string input, string loc,string editor)
    {
        ArrayList result = new ArrayList();
        try
        {
            IFIXTure fixture = ServiceAgent.getInstance().GetObjectByName<IFIXTure>(WebConstant.FIXTure);
            fixture.Save(input, loc, editor);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(input);
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
    public ArrayList OutSave(string input, string pdline, string editor)
    {
        ArrayList result = new ArrayList();
        try
        {
            IFIXTure fixture = ServiceAgent.getInstance().GetObjectByName<IFIXTure>(WebConstant.FIXTure);
            fixture.OutSave(input, pdline, editor);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(input);
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
}

