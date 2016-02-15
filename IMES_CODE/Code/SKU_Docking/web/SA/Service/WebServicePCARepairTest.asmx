<%@ WebService Language="C#" Class="WebServicePCARepairTest" %>


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
public class WebServicePCARepairTest : System.Web.Services.WebService
{
    private IPCARepairTest iPCARepairTest = null;
    private IDefect iDefect = null;
    private IMB iMB = null;
    private string type = "PRD";
    
    [WebMethod]
    public IList input(string pdLine, string mbSno, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        MBInfo info;
        IList<DefectInfo> defectList;

        try
        {
            iPCARepairTest = ServiceAgent.getInstance().GetObjectByName<IPCARepairTest>(WebConstant.PCARepairTestObject);
            iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);

            iPCARepairTest.InputMBSno(pdLine, mbSno, editor, station, customer);

            info = iMB.GetMBInfo(mbSno);
            defectList = iDefect.GetDefectList(type);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(info);
            ret.Add(defectList);

            
        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
            //throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            ret.Add(ex.Message);
            //throw ex;
        }
        return ret;
    }

    [WebMethod]
    public IList save(string mbSno, IList<string> defectList, string scanCode)
    {
        IList ret = new ArrayList();
        try
        {
            iPCARepairTest = ServiceAgent.getInstance().GetObjectByName<IPCARepairTest>(WebConstant.PCARepairTestObject);
            string code = iPCARepairTest.InputDefectCodeList(mbSno, defectList, scanCode);
            if (code == "SUCCESS")
            {
                ret.Add(WebConstant.SUCCESSRET);
            }
            else if (code == "ERROR")
            {
                ret.Add("ERROR");
            }
        }
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg);
            //throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            ret.Add(ex.Message);
            //throw ex;
        }
        return ret;
    }

    [WebMethod]
    public void cancel(string mbSno)
    {
        try
        {
            iPCARepairTest = ServiceAgent.getInstance().GetObjectByName<IPCARepairTest>(WebConstant.PCARepairTestObject);
            iPCARepairTest.Cancel(mbSno);
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