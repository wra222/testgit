<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: master Label Print and RePrint Service Method
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-03-15  Chen Xu (eB1-4)      Create 
 
 Known issues:
 --%>
<%@ WebService Language="C#" Class="WebServicePNOPLabelPrint" %>

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
//using System.Web.Services.Protocols;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePNOPLabelPrint : System.Web.Services.WebService
{
    IPNOPLabelPrint iPNOPLabelPrint = ServiceAgent.getInstance().GetObjectByName<IPNOPLabelPrint>(WebConstant.PNOPLabelPrintObject);
    private const string SUCCESS = "SUCCESSRET";

    [WebMethod]
    public ArrayList InputModel(string Model)
    {
        ArrayList ret = new ArrayList();
        try
        {
            var para = iPNOPLabelPrint.InputModel(Model);
            ret.Add(SUCCESS);
            ret.Add(Model);
            ret.Add(para);
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
    public ArrayList display(string MBSN, string PdLine, string DCode, string Model, string InfoValue, IList<PrintItem> printItems, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        string preStation = string.Empty;
        try
        {

            var para = iPNOPLabelPrint.InputMBSN(MBSN, PdLine, DCode, Model, InfoValue, printItems, editor, station, customer);
            ret.Add(SUCCESS);
            ret.Add(para);
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
    public void Cancel(string productId)
    {
        try
        {
            iPNOPLabelPrint.Cancel(productId);
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

