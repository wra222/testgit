<%@ WebService Language="C#" Class="WebServiceReleaseProductIDHold" %>


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
public class WebServiceReleaseProductIDHold : System.Web.Services.WebService
{
    private IReleaseProductIDHold iReleaseProductIDHold = ServiceAgent.getInstance().GetObjectByName<IReleaseProductIDHold>(WebConstant.ReleaseProductIDHoldObject);

    [WebMethod]
    public ArrayList GetReleaseDefect()
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<string> list = iReleaseProductIDHold.GetReleaseDefectList("RELEASE");
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(list);
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
    public ArrayList save(string key, string releaseCode, string goToStation)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList list = iReleaseProductIDHold.Save(key, releaseCode, goToStation);
            //ret.Add(WebConstant.SUCCESSRET);
            //ret.Add(list);
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
    public ArrayList OfflineHoldSave(string key, string station,string editor,string customer,string releaseCode, string goToStation)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList list = iReleaseProductIDHold.OfflineHoldSave(key, station, editor, customer, releaseCode, goToStation);
            //ret.Add(WebConstant.SUCCESSRET);
            //ret.Add(list);
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
    public void cancel(string guid)
    {
        try
        {
            iReleaseProductIDHold.Cancel(guid);
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