<%@ WebService Language="C#" Class="WebServicePackingList" %>


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
public class WebServicePackingList : System.Web.Services.WebService
{
    IPackingList iPackingList = ServiceAgent.getInstance().GetObjectByName<IPackingList>(WebConstant.PackingListObject);

    [WebMethod]
    public ArrayList WSQuery(string pdline, string station, string editor, string customer,
                            string region, string carrier, string btime, string etime)
    {
        ArrayList retLst = new ArrayList();
        IList<string> dnList = new List<string>();

        try
        {
            dnList = iPackingList.PackingListForOBQuery(pdline, station, editor, customer,
                            region, carrier, btime, etime);

            retLst.Add(WebConstant.SUCCESSRET);
            //zzhh For Test
            //dnList.Add(region);
            //dnList.Add(carrier);
            retLst.Add(dnList);
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
    public ArrayList WSClear(string none)        
    {
        ArrayList retLst = new ArrayList();
        retLst.Add(WebConstant.SUCCESSRET);
        return retLst;
    }

    [WebMethod]
    public ArrayList WSClientPrint(string none)
    {
        ArrayList retLst = new ArrayList();
        retLst.Add(WebConstant.SUCCESSRET);
        return retLst;
    }

    [WebMethod]
    public ArrayList InsertPrintList(string namelist, int count)
    {
        ArrayList retLst = new ArrayList();
        iPackingList.insertPrintListTable(namelist, count);
        retLst.Add(WebConstant.SUCCESSRET);
        return retLst;
    }

    [WebMethod]
    public ArrayList WSCheck(string pdline, string station, string editor, string customer,
                            string dn, string doctype, string region, string carrier, string key)
    {
        ArrayList retLst = new ArrayList();
        ArrayList serLst = new ArrayList();        

        try
        {
            serLst = iPackingList.PackingListForOBCheck(pdline, station, editor, customer,
                               dn, doctype, region, carrier, key);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(serLst);

            return retLst;

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
}
