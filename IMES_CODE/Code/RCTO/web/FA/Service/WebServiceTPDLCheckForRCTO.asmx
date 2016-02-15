<%@ WebService Language="C#" Class="WebServiceTPDLCheckForRCTO" %>


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
public class WebServiceTPDLCheckForRCTO : System.Web.Services.WebService
{
    ITPDLCheckForRCTO tpdlObject = ServiceAgent.getInstance().GetObjectByName<ITPDLCheckForRCTO>(WebConstant.TPDLCheckObjectForRCTO);
    
    IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    IMB iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);

    
    [WebMethod]
    public ArrayList inputProductID(string prodID, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpList = new ArrayList();

        string line = string.Empty;
        
        try
        {
            tmpList =  tpdlObject.InputProductID(prodID, line, editor, station, customer);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpList[0]);//curProduct.ProId
            retLst.Add(tmpList[1]);//LCM　CT
            retLst.Add(tmpList[2]);//TPDL　CT
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }    
        return retLst;
    }

    [WebMethod]
    public ArrayList Save(string prodID,string Tpdl)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpList = new ArrayList();

        try
        {
            tmpList = tpdlObject.Save(prodID, Tpdl);
            retLst.Add(WebConstant.SUCCESSRET);
           
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;

    }
    
    [WebMethod]
    public void cancel(string productId)
    {
        try
        {
            tpdlObject.Cancel(productId);
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
