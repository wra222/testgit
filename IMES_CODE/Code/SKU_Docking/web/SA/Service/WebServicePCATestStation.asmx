<%@ WebService Language="C#" Class="WebServicePCATestStation" %>


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
public class WebServicePCATestStation : System.Web.Services.WebService
{
    IPCATestStation iPCATestStation = ServiceAgent.getInstance().GetObjectByName<IPCATestStation>(WebConstant.PCATestStationObject);
    IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    IMB iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);

    
    
    [WebMethod]
    public string HelloWorld() {
        //throw new FieldAccessException("errormess");
        return "Hello World3";
    }

    [WebMethod]
    public ArrayList inputMBCode(string pdLine, string testStation, string mbSn, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        IList<DefectInfo> defectList;
        string _111 = String.Empty;
        IList<string> infoList = new List<string>();
        string AllowPass = String.Empty;
        string DefectStation = String.Empty;
        try
        {
            infoList = iPCATestStation.InputMBSNo(pdLine, testStation, mbSn, editor, customer, out AllowPass, out DefectStation);
            //MBInfo mbInfo = iMB.GetMBInfo(mbSn);
            //_111 = mbInfo._111LevelId;

            //获取所有Defect
            defectList = iDefect.GetDefectList("PRD");
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(mbSn);
            retLst.Add(infoList[0]);
            retLst.Add(defectList);
            retLst.Add(AllowPass);
            retLst.Add(DefectStation);
            retLst.Add(infoList[1]);
        }

        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        //_111 = "111";
        //retLst.Add(WebConstant.SUCCESSRET);
        //retLst.Add(mbSn);
        //retLst.Add(_111);
        //defectList = new List<DefectInfo>();
        //DefectInfo defect = new DefectInfo();
        //defect.id = "aaa";
        //defect.description = "aaa";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "bbb";
        //defect.description = "bbb";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "ccc";
        //defect.description = "ccc";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "ddd";
        //defect.description = "ddd";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "eee";
        //defect.description = "eee";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "fff";
        //defect.description = "fff";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "1";
        //defect.description = "1";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "2";
        //defect.description = "2";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "3";
        //defect.description = "3";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "4";
        //defect.description = "4";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "5";
        //defect.description = "5";
        //defectList.Add(defect);
        //retLst.Add(defectList);
        return retLst;
    }

    [WebMethod]
    public ArrayList inputMBCodeTrans(string pdLine, string testStation, string mbSn, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        IList<DefectInfo> defectList;
        string _111 = String.Empty;
        IList<string> infoList = new List<string>();
        string AllowPass = String.Empty;
        string DefectStation = String.Empty;
        try
        {
            infoList = iPCATestStation.InputMBSNoTrans(pdLine, testStation, mbSn, editor, customer, out AllowPass, out DefectStation);
            //MBInfo mbInfo = iMB.GetMBInfo(mbSn);
            //_111 = mbInfo._111LevelId;

            //获取所有Defect
            defectList = iDefect.GetDefectList("PRD");
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(mbSn);
            retLst.Add(infoList[0]);
            retLst.Add(defectList);
            retLst.Add(AllowPass);
            retLst.Add(DefectStation);
            retLst.Add(infoList[1]);
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
    public ArrayList inputMBCodeForLot(string pdLine, string testStation, string mbSn, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        IList<DefectInfo> defectList;
        string _111 = String.Empty;
        IList<string> infoList = new List<string>();
        string AllowPass = String.Empty;
        string DefectStation = String.Empty;
        try
        {
            infoList = iPCATestStation.InputMBSNoForLot(pdLine, testStation, mbSn, editor, customer, out AllowPass, out DefectStation);
            //MBInfo mbInfo = iMB.GetMBInfo(mbSn);
            //_111 = mbInfo._111LevelId;

            //获取所有Defect
            defectList = iDefect.GetDefectList("PRD");
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(mbSn);
            retLst.Add(infoList[0]);
            retLst.Add(defectList);
            retLst.Add(AllowPass);
            retLst.Add(DefectStation);
            retLst.Add(infoList[1]);
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
    public ArrayList savePage(String mbSn, IList<string> defectList)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            iPCATestStation.InputDefectCodeList(mbSn, defectList);
            retLst.Add(WebConstant.SUCCESSRET);
        }

        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //retLst.Add(WebConstant.SUCCESSRET);
        return retLst;
    }
    
    [WebMethod]
    public ArrayList savePageTrans(String mbSn, IList<string> defectList)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            iPCATestStation.InputDefectCodeListTrans(mbSn, defectList);
            retLst.Add(WebConstant.SUCCESSRET);
        }

        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //retLst.Add(WebConstant.SUCCESSRET);
        return retLst;
    }
    [WebMethod]
    public ArrayList savePageForLot(String mbSn, IList<string> defectList,bool FruCheck)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            iPCATestStation.InputDefectCodeListForLot(mbSn, defectList, FruCheck);
            retLst.Add(WebConstant.SUCCESSRET);
        }

        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //retLst.Add(WebConstant.SUCCESSRET);
        return retLst;
    }
    
    [WebMethod]
    public ArrayList Cancel(string MB, string station)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iPCATestStation.Cancel(MB);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(MB);
           
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }

    
   
}