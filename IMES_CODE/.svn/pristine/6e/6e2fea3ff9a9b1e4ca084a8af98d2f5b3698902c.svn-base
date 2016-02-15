<%@ WebService Language="C#" Class="WebServicePCAICTInput" %>


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
public class WebServicePCAICTInput : System.Web.Services.WebService
{
    private IICTInput iPCAICTInput = null;
    private IDefect iDefect = null;
    private IMB iMB = null;
    private string type = "PRD";
    private IQty iQty = ServiceAgent.getInstance().GetObjectByName<IQty>(WebConstant.CommonObject);
    
    [WebMethod]
    public IList input(string MBSno, string ecr, string dCode, string editor, string line, string station, string customer)
    {
        IList ret = new ArrayList();
        //MBInfo info;
        IList<DefectInfo> defectList;

        try
        {
            iPCAICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.PCAICTInputObject);
            iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
            int warrantyID = Convert.ToInt32(dCode);

            iPCAICTInput.InputMBSno(MBSno, ecr, warrantyID, editor, line, station, customer);

            //info = iMB.GetMBInfo(MBSno);
            defectList = iDefect.GetDefectList(type);
            
            //ret.Add(info);
            ret.Add(WebConstant.SUCCESSRET);
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
    public IList save(bool IsRCTO,string mbSno, string aoi, IList<string> defectList, IList<PrintItem> printItems)
    {
        IList ret = new ArrayList();
        //IList<PrintItem> printLst = new List<PrintItem>();
        try
        {
            iPCAICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.PCAICTInputObject);
            var retLst = iPCAICTInput.Save(IsRCTO,mbSno, aoi, defectList, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst[0]);
            ret.Add(retLst[1]);
            ret.Add(retLst[2]);
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
    public IList cleatQty(string line, string qty)
    {
        IList ret = new ArrayList();
        try
        {
            iQty.ClearQty(line, qty);
            ret.Add(WebConstant.SUCCESSRET);
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
            iPCAICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.PCAICTInputObject);
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