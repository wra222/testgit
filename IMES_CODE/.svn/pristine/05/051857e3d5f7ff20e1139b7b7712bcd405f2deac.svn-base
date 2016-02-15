<%@ WebService Language="C#" Class="WebServiceCartonWeight" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.BSamIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
//using IMES.FisObject.Common.Model;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCartonWeight : System.Web.Services.WebService
{
    private ICartonWeight iCartonWeight = ServiceAgent.getInstance().GetObjectByName<ICartonWeight>(WebConstant.ICartonWeight);
    [WebMethod]
    public S_CartonWeightResult SaveForTablet(string custSN, IList<PrintItem> printItems)
    {
        try
        {
            ArrayList ret = new ArrayList();
            ret= iCartonWeight.SaveForTablet(custSN,printItems);
            S_CartonWeightResult cartonWeightResult = new S_CartonWeightResult
                                                                             {
                                                                                 success = WebConstant.SUCCESSRET,
                                                                                 isNeedPrintNONLabel = (bool)ret[0],
                                                                                 printItem = (IList<PrintItem>)ret[1]
                                                                             };

            return cartonWeightResult;
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
    // class:reference type	 struce:	value type
    public struct S_CartonWeightResult
    {
        public string success;
        public bool isNeedPrintNONLabel;
        public IList<PrintItem> printItem;
     
    
    }
    
   
    [WebMethod]
    public ArrayList Save(string custSN)
    {
        try
        {
            ArrayList ret = new ArrayList();
            iCartonWeight.Save(custSN);
            ret.Add(WebConstant.SUCCESSRET);
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
    public string Cancel(string custSN)
    {
        try
        {
            iCartonWeight = ServiceAgent.getInstance().GetObjectByName<ICartonWeight>(WebConstant.ICartonWeight);
            iCartonWeight.Cancel(custSN);
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