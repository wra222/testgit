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
using IMES.Station.Interface.CommonIntf;
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

    IUnpackForRCTO iUnpack = ServiceAgent.getInstance().GetObjectByName<IUnpackForRCTO>(WebConstant.UnpackForRCTOObject); 
    
    //[WebMethod]
    //public string Cancel(string ProductIDOrSNOrCartonNo)
    //{
    //    ArrayList ret = new ArrayList();
    //    try
    //    {
    //        iUnpack.Cancel(ProductIDOrSNOrCartonNo);
    //        return WebConstant.SUCCESSRET;
    //    }
    //    catch (FisException ex)
    //    {
    //        throw new Exception(ex.mErrmsg);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    [WebMethod]
    public string UnpackDummyPalletNo(string DummyPalletNo, string editor, string customer)
    {
        string line = string.Empty;
        try
        {
            iUnpack.UnpackDummyPalletNo(DummyPalletNo, line, editor, customer);
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
    public string UnpackDNByDN(string deliveryNo, bool bSuperUI, string editor, string stationId, string customer)
    {
        string line = string.Empty;
        try
        {
            iUnpack.UnpackDNByDN(deliveryNo, bSuperUI, line, editor, stationId, customer);
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
    public string UnpackCarton(string ProductIDOrSNOrCartonNo, string editor, string stationId, string customer)
    {
        string line = string.Empty;
        try
        {
            iUnpack.UnpackCarton(ProductIDOrSNOrCartonNo, line, editor, stationId, customer);
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
    public string UnpackDN(string ProductIDOrSNOrCartonNo, string editor, string stationId, string customer)
    {
        string line = string.Empty;
        try
        {
            iUnpack.UnpackDN(ProductIDOrSNOrCartonNo, line, editor, stationId, customer);
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
    public string UnpackPallet(string ProductIDOrSNOrCartonNo, string editor, string stationId, string customer)
    {
        string line = string.Empty;
        try
        {
            iUnpack.UnpackPallet(ProductIDOrSNOrCartonNo, line, editor, stationId, customer);
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
    public string UnpackDNbySNCheck(string ProductIDOrSNOrCartonNo, string editor, string stationId, string customer)
    {
        string line = string.Empty;

        try
        {
           iUnpack.UnpackDNbySNCheck(ProductIDOrSNOrCartonNo, line, editor, stationId, customer);
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
    public IList UnpackDNbySNSave(string ProductIDOrSNOrCartonNo,string stationId)
    {
        IList ret = new ArrayList();
        //string line = string.Empty;
        bool isPallet=false;
        try
        {
            if (stationId != "91")
            {
                isPallet = true;
            }

            string code = iUnpack.UnpackDNbySNSave(ProductIDOrSNOrCartonNo, isPallet);
            ret.Add( WebConstant.SUCCESSRET);
            ret.Add(code);
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
    public string UnpackAllbySNCheck(string ProductIDOrSNOrCartonNo, string editor, string stationId, string customer)
    {
        string line = string.Empty;

        try
        {
            iUnpack.UnpackAllBySNCheck(ProductIDOrSNOrCartonNo, line, editor, stationId, customer);
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
    public IList UnpackAllbySNSave(string ProductIDOrSNOrCartonNo)
    {
        IList ret = new ArrayList();
        try
        {
            string code =iUnpack.UnpackAllbySNSave(ProductIDOrSNOrCartonNo);
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
    public string UnpackCartonForFRU(string cartonOrDN, string editor, string stationId, string customer)
   {
        string line = string.Empty;
        try
        {
            iUnpack.UnpackCartonForFRU(cartonOrDN, line, editor, stationId, customer);
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

