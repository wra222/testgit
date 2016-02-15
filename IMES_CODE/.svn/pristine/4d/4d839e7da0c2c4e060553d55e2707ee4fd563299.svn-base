<%@ WebService Language="C#" Class="SAInputXRayWebService" %>
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
public class SAInputXRayWebService : System.Web.Services.WebService
{

    [WebMethod]
    public ArrayList Save(string input, string pdline, string model, string location, string obligation, string remark,string state, string customer, string editor)
    {
        ArrayList result = new ArrayList();
        try
        {
            ISAInputXRay inputxrayService =ServiceAgent.getInstance().GetObjectByName<ISAInputXRay>(WebConstant.SAInputXRayObject);

            inputxrayService.Save(input, pdline, model,location,obligation,remark,state,customer,editor);
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

