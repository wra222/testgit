<%@ WebService Language="C#" Class="WebServiceMaterialReturn" %>


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

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceMaterialReturn : System.Web.Services.WebService
{
    //ICombineTPM iCombineTPM = ServiceAgent.getInstance().GetObjectByName<ICombineTPM>(WebConstant.CombineTPMObject);
    IMaterialReturn iMaterialReturn = ServiceAgent.getInstance().GetObjectByName<IMaterialReturn>(WebConstant.MaterialReturnObject);

    [WebMethod]
    public IList InputMaterialCTFirst(string materialCT, string materialType, string line, string editor, string station, string customer)
      {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList tmpList = iMaterialReturn.InputMaterialCTFirst(materialCT,"", line, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//Material CT
            ret.Add(tmpList[1]);//sessionkey
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
    public IList InputMaterialCT(string materialCT, string sessionkey)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList tmpList = iMaterialReturn.InputMaterialCT(materialCT, sessionkey);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//Material CT
            ret.Add(tmpList[1]);//sessionkey
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
    public ArrayList save(string prodId)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iMaterialReturn.Save(prodId);
           ret.Add(WebConstant.SUCCESSRET);
           //return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            //throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
   
   
    
    [WebMethod]
    public ArrayList Cancel(string prodId)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iMaterialReturn.Cancel(prodId);
            ret.Add(WebConstant.SUCCESSRET);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            //throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex; ;
        }
        return ret;
    }
  
}


