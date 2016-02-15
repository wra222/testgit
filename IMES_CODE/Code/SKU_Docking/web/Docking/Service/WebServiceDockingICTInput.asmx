<%@ WebService Language="C#" Class="WebServiceDockingICTInput" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceDockingICTInput : System.Web.Services.WebService
{
    private string type = "PRD";


    [WebMethod]
    public IList input(string MBSno, string ecr, string dCode, string editor, string line, string station, string customer)
    {
        IList ret = new ArrayList();

        try
        {
            IICTInput iPCAICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.ICTInputDocking);

            int warrantyID = Convert.ToInt32(dCode);

            iPCAICTInput.InputMBSno(MBSno, ecr, warrantyID, editor, line, station, customer);

            ret.Add(WebConstant.SUCCESSRET);

        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            ret.Add(ex.Message);
        }
        return ret;

    }

    [WebMethod]
    public IList save(string mbSno, string aoi, IList<string> defectList, IList<PrintItem> printItems)
    {
        IList ret = new ArrayList();

        try
        {
            IICTInput iPCAICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.ICTInputDocking);
            var retLst = iPCAICTInput.Save(mbSno, aoi, defectList, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst[0]);
            ret.Add(retLst[1]);
        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            ret.Add(ex.Message);
        }
        return ret;
    }

    [WebMethod]
    public IList GetDefectList()
    {
        IList ret = new ArrayList();
       
        try
        {
             IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);

             IList<DefectInfo> defectList = iDefect.GetDefectList(type);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(defectList);

        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            ret.Add(ex.Message);
        }
        return ret;

    }

    [WebMethod]
    public void cancel(string mbSno)
    {
        try
        {
            IICTInput iPCAICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.ICTInputDocking);
            iPCAICTInput.Cancel(mbSno);
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