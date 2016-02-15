<%@ WebService Language="C#" Class="WebServiceScanningList" %>


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
public class WebServiceScanningList : System.Web.Services.WebService
{
    ScanningList iScanningList = ServiceAgent.getInstance().GetObjectByName<ScanningList>(WebConstant.ScanningListObject);


    [WebMethod]
    public ArrayList Check(string pdline, string station, string editor, string customer,
                             string doctype,string data)
    {
        ArrayList retLst = new ArrayList();
        IList<string> dnList = new List<string>();

        try
        {
            ArrayList  para = iScanningList.ScanningListForCheck(pdline, station, editor, customer,
                                      data, doctype);

            retLst.Add(WebConstant.SUCCESSRET);
            dnList.Add(data);
            retLst.Add(dnList);
            retLst.Add(para[4]);
            retLst.Add(para);

        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
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
            iScanningList.Cancel(productId);
            
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
    public void print(string uuId, List<string> printList)
    {
        try
        {
            iScanningList.Print(uuId, printList);
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
    public ArrayList FilePath()
    {
        ArrayList pathList = new ArrayList();
        try
        {
           pathList = iScanningList.ScanningCopyFile();
           return pathList;
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
