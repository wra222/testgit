<%@ WebService Language="C#" Class="ExecSPWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ExecSPWebService : System.Web.Services.WebService
{

    [WebMethod]
    public ArrayList GetSPResult(string editor,string db, string sp, string[] parameterNameArray, string[] parameterValueArray)
    {

        ArrayList result = new ArrayList();
        try
        {
            ICommonFunction CurrentService = ServiceAgent.getInstance().GetObjectByName<ICommonFunction>(com.inventec.iMESWEB.WebConstant.MaintainCommonFunctionObject);
            System.Data.DataTable SPResult = CurrentService.GetSPResult(editor,db, sp, parameterNameArray, parameterValueArray);
            if (SPResult != null && SPResult.Rows.Count>0 && SPResult.Columns.Count >1) { 
                result.Add(SPResult.Rows[0][0]);
                result.Add(SPResult.Rows[0][1]);
            }
        }
        catch (FisException fe)
        {
            result.Add("ERROR");
            result.Add(fe.Message);
        }
        catch (Exception e)
        {
            result.Add("ERROR");
            result.Add(e.Message);
        }
        return result;
    }

}

