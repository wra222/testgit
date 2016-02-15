<%@ WebService Language="C#" Class="WebServiceChangeAST" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceChangeAST  : System.Web.Services.WebService
{
    IChangeAST iChangeAST = ServiceAgent.getInstance().GetObjectByName<IChangeAST>(WebConstant.ChangeASTObject);

    [WebMethod]
    public ArrayList Change(string pdline, string station, string editor, string customer, 
                            string prod1, string prod2, string model1, string model2,
                            string ASTType, string PartNo, string PartSn, IList<PrintItem> printItemLst)
    {
        ArrayList retService = new ArrayList();
        ArrayList ret = new ArrayList();
        
        try
        {
            retService = iChangeAST.Change(pdline, editor, station, customer,
                                            prod1, prod2, model1, model2,
                                            ASTType, PartNo, PartSn, printItemLst);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retService);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            return ret;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
