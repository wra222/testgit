<%--
/*
 * INVENTEC corporation 2011 all rights reserved. 
 * Description:WebMethod for POD Label Check
 * UI:CI-MES12-SPEC-PAK-UI POD Label Check.docx –2011/11/01
 * UC:CI-MES12-SPEC-PAK-UC POD Label Check.docx –2011/11/01   
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11      Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：
 * UC 具体业务：
*/
--%>
<%@ WebService Language="C#" Class="WebServicePODLabelCheck" %>

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

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePODLabelCheck  : System.Web.Services.WebService 
{

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    IPodLabelCheck iPodLabelCheck = ServiceAgent.getInstance().GetObjectByName<IPodLabelCheck>(WebConstant.PodLabelCheckObject);
    [WebMethod]
    public ArrayList InputCustSnForCQ(string custsn, string pdLine, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList  retR = iPodLabelCheck.InputCustSnForCQ(custsn, pdLine, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retR[0]); // ProductID
            ret.Add(retR[1]); //Model
            ret.Add(retR[2]); //Label Type
            ret.Add(retR[3]); //Model Type (E or F....)
            ret.Add(retR[4]); //"" or NoNeed
            
            return ret;

        }
        catch (FisException ex)
        {
            //throw new Exception(ex.mErrmsg);
            if (ex.mErrcode == "CHK020")
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    [WebMethod]
    public ArrayList inputCustSNOnCooLabel(string custsn, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        string custpn = "";
        string BoxID = "";
        MRPLabelDef MRP = new MRPLabelDef();
        try
        {
            ProductModel proModel = iPodLabelCheck.InputCustSnOnCooLabel(custsn, pdLine, editor, stationId, customerId, out custpn, out MRP, out BoxID);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(proModel.ProductID);
            ret.Add(proModel.Model);
            ret.Add(proModel.CustSN);
            ret.Add(MRP.IndiaPrice.Trim());
            ret.Add(MRP.IndiaPriceID.Trim());
            ret.Add(BoxID.Trim());
            return ret;
        }
        catch (FisException ex)
        {
            //throw new Exception(ex.mErrmsg);
            if (ex.mErrcode == "CHK020")
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
      

    [WebMethod]
    public ArrayList inputCustPNOnPODLabel(string productIdValue, string custSnOnCooValue,string custPnOnPodValue, string pdLine, string editor, string stationId, string customerId)
    {
        ArrayList ret = new ArrayList();
        string temp = "";
        string Loc = "";
        try
        {
            temp = iPodLabelCheck.InputCustPnOnPodLabel(productIdValue, custSnOnCooValue, custPnOnPodValue, pdLine, editor, stationId, customerId, out Loc);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(temp);
            ret.Add(Loc);
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
 

    [WebMethod]
    public string Cancel(string sn)
    {
        try
        {
            iPodLabelCheck.Cancel(sn);
            return (WebConstant.SUCCESSRET);
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

