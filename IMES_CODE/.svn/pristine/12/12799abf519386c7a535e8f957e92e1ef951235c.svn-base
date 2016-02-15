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

    [WebMethod]
    public ArrayList SetMsgInfoTODialogtoSession(string SQLText)
    {

        ArrayList result = new ArrayList();
        try
        {
            
            //System.Data.DataTable dt = InitTable();
            //ICommonFunction CurrentService = ServiceAgent.getInstance().GetObjectByName<ICommonFunction>(com.inventec.iMESWEB.WebConstant.MaintainCommonFunctionObject);
            //System.Data.DataSet SqlResult = CurrentService.GetSQLResult(editor, db, SQLText, null, null);
            //if (SqlResult != null && SqlResult.Tables.Count > 0 && SqlResult.Tables[0].Rows.Count > 0)
            //{
            //    dt = SqlResult.Tables[0];
            //}
            //if (dt != null && dt.Rows.Count != 0)
            //{
            //    result.Add(dt.Rows[0][0].ToString().Trim());
            //    result.Add(dt.Rows[0][1].ToString().Trim());
            //    result.Add(dt.Rows[0][2].ToString().Trim());
            //    result.Add(dt.Rows[0][3].ToString().Trim());
            //    result.Add(dt.Rows[0][4].ToString().Trim());
            //    string a = dt.Rows[0][5].ToString();
            //    a = HttpUtility.UrlEncode(a);
            //    result.Add(a);
            //    //result.Add(dt.Rows[0][5].ToString().Replace('#','$').Trim());
            //}
            //else
            //{
            //    System.Data.DataRow newRow = null;
            //    newRow = dt.NewRow();
            //    dt.Rows.Add(newRow);
            //}
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


    //private System.Data.DataTable InitTable()
    //{
    //    System.Data.DataTable result = new System.Data.DataTable();
    //    result.Columns.Add("No Data", Type.GetType("System.String"));
    //    return result;
    //}

}

