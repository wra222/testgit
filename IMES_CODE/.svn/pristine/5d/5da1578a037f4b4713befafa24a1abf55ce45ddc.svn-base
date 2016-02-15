<%--
 INVENTEC corporation (c)2010 all rights reserved. 
 Description: Unpack Service
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-03-15  Lucy Liu            Create 
 2011-10-20  itc202017           Add UnpackDNByDN method
 2012-04-06  Chen Xu (itc208014) Add UnpackDummyPalletNo
 Known issues:
 --%>
 
<%@ WebService Language="C#" Class="UnpackService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Docking.Interface.DockingIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class UnpackService: System.Web.Services.WebService
{

    IUnpackForDocking iUnpack = ServiceAgent.getInstance().GetObjectByName<IUnpackForDocking>(WebConstant.UnpackForDockingObject); 
    
   
    [WebMethod]
    public string UnpackbySNCheck(string ProductIDOrSNOrCartonNo, string editor, string stationId, string customer)
    {
        string line = string.Empty;

        try
        {
            iUnpack.UnpackBySNCheck(ProductIDOrSNOrCartonNo, line, editor, stationId, customer);
            return WebConstant.SUCCESSRET;

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
    public IList UnpackbySNSave(string ProductIDOrSNOrCartonNo)
    {
        IList ret = new ArrayList();
        try
        {
            iUnpack.UnpackbySNSave(ProductIDOrSNOrCartonNo);
            ret.Add(WebConstant.SUCCESSRET);
            //ret.Add(code);
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
    public void cancel(string productId)
    {
        try
        {
            iUnpack.Cancel(productId);
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

