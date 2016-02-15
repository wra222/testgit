<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: Japanese Label Print and RePrint Service Method
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-03-15  Chen Xu (eB1-4)      Create 
 
 Known issues:
 --%>
<%@ WebService Language="C#" Class="ReturnUsedKeysService" %>

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
public class ReturnUsedKeysService : System.Web.Services.WebService
{
    private IProduct iProduct;
    private string commBllName = WebConstant.CommonObject;
    ReturnUsedKeys iReturnUsedKeys = ServiceAgent.getInstance().GetObjectByName<ReturnUsedKeys>(WebConstant.ReturnUsedKeysObject);
    private const string SUCCESS = "SUCCESSRET";
    
    [WebMethod]
    public ArrayList Check( string customSn, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {
            var para = iReturnUsedKeys.Check(customSn, editor, station, customer);
            ret.Add(customSn);
            ret.Add(para[0]);
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
    public ArrayList Save(List<string> SNList,List<string>PNList,List<string>ErrList, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        string preStation = string.Empty;
        try
        {
            var para = iReturnUsedKeys.Save(SNList, PNList,ErrList, editor, station, customer);
            ret.Add(para[0]);

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
            ;// iPrintInatelICASA.Cancel(productId);
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

