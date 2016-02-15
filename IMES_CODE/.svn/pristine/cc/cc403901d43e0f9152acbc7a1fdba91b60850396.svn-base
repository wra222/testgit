<%@ WebService Language="C#" Class="WebServiceAoiOfflineKbCheck" %>


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
//using IMES.FisObject.Common.Model;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceAoiOfflineKbCheck : System.Web.Services.WebService
{
    private IAoiOfflineKbCheck iAoiOfflineKbCheck = ServiceAgent.getInstance().GetObjectByName<IAoiOfflineKbCheck>(WebConstant.IAoiOfflineKbCheck);
    private IDefect iDefect = null;
    private IProduct iProduct = null;
 
    public struct S_AoiOfflineKbCheck
    {
        public string ProductID;
        public string Model;
        public IList<DefectInfo> DefectList;
    }
    [WebMethod]
    public S_AoiOfflineKbCheck input(string pdLine, string prodId, string editor, string station, string customer)
    {
         try
        {
           ArrayList arr = iAoiOfflineKbCheck.InputCustSn(pdLine, prodId, editor, station, customer);
           S_AoiOfflineKbCheck s_AoiOfflineKbCheck = new S_AoiOfflineKbCheck
             { ProductID = arr[0].ToString(), Model = arr[1].ToString(), DefectList = (IList<DefectInfo>)arr[2] };
          return s_AoiOfflineKbCheck;
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
    public  void save(string prodId, IList<string> defectList,string reason)
    {
   
        try
        {
         
           iAoiOfflineKbCheck.Save(prodId, defectList,reason);
            
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
         
            iAoiOfflineKbCheck.Cancel(productId);
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