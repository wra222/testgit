<%@ WebService Language="C#" Class="MBBorrowWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.StationIntf;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MBBorrowWebService : System.Web.Services.WebService
{
    [WebMethod]
    public ArrayList InputKey(string key, string keyType, string editor, string line, string station, string customer)
    {
        ArrayList result = new ArrayList();
        try
        {
            IBorrowControl currentBorrowService = GetBorrowService(key, keyType);
            string model = currentBorrowService.InputKey(key, editor, line, station, customer);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(model);
            result.Add(key);
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
    public ArrayList Save(string key, string keyType, string borrowerOrReturner, string action)
    {
        ArrayList result = new ArrayList();
        try
        {
            IBorrowControl currentBorrowService = GetBorrowService(key, keyType);
            currentBorrowService.Save(key, borrowerOrReturner, action);
            result.Add(WebConstant.SUCCESSRET);
            result.Add(action);
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
    public void Cancel(string key, string keyType)
    {
        IBorrowControl currentBorrowService = GetBorrowService(key, keyType);
        currentBorrowService.Cancel(key);

    }

    private IBorrowControl GetBorrowService(string key, string keyType)
    {
        switch (keyType)
        {
            case "MB":
                return com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IBorrowControl>(com.inventec.iMESWEB.WebConstant.MBBorrowObject);

            case "Product":
                return com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IBorrowControl>(com.inventec.iMESWEB.WebConstant.ProductBorrowObject);

            case "CT":
                return com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IBorrowControl>(com.inventec.iMESWEB.WebConstant.CTBorrowObject);
            default:
                return null;
        }
    }
}

