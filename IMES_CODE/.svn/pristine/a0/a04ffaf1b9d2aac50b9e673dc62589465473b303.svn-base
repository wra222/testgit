
<%@ WebService Language="C#" Class="CombineDNBTWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Collections.Generic;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using com.inventec.iMESWEB;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CombineDNBTWebService : System.Web.Services.WebService
{

    ICombineDNPalletforBT iCombineDNPalletforBT = ServiceAgent.getInstance().GetObjectByName<ICombineDNPalletforBT>(WebConstant.CombineDNPalletforBTObject);

    [WebMethod]
    public ArrayList GetSysSettingList(IList<string> name)
    {
        ArrayList ret = new ArrayList();
        ArrayList value = new ArrayList();
        try
        {
            value = iCombineDNPalletforBT.GetSysSettingList(name); 
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(value);
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