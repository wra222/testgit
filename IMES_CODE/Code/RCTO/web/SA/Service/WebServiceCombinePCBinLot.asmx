<%@ WebService Language="C#" Class="WebServiceCombinePCBinLot" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
//using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCombinePCBinLot : System.Web.Services.WebService
{
    ICombinePcbInLot iCombinePcbInLot = ServiceAgent.getInstance().GetObjectByName<ICombinePcbInLot>(WebConstant.CombinePcbinLotObject);
    IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    IMB iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);

    
    [WebMethod]
    public string HelloWorld() {
        //throw new FieldAccessException("errormess");
        return "Hello World3";
    }

    [WebMethod]
    public ArrayList inputMBCode(string mbSn,string Station, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList infoList = new ArrayList();
        try
        {
            infoList = iCombinePcbInLot.inputMBSno(mbSn, editor, Station, customer);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(infoList[0]);//mbsn
            retLst.Add(infoList[1]);//line+descr
            retLst.Add(infoList[2]);//PassQty
            retLst.Add(infoList[3]);//LotList
            retLst.Add(infoList[4]);//status list
            //retorderbylot[0].lotNo
            //retorderbylot[0].type
            //retorderbylot[0].qty
            //retorderbylot[0].status
            //retorderbylot[0].cdt
            
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
    public ArrayList save(String mbSn, string LotNo)
    {
        ArrayList retLst = new ArrayList();
        ArrayList infoList = new ArrayList();
        try
        {
            infoList = iCombinePcbInLot.save(mbSn, LotNo);
            retLst.Add(WebConstant.SUCCESSRET);  //0
            if ((String)infoList[0] == "OK")
            {
                retLst.Add("OK");                //1
                return retLst;
            }
            else if ((String)infoList[0] == "ReloadLotList")
            {
                retLst.Add("ReloadLotList");    //1
                retLst.Add(infoList[1]);//error message //2
                retLst.Add(infoList[2]);//Lot List      //3
                retLst.Add(infoList[3]);//Status list   //4
                return retLst;
            }
            
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
    public ArrayList Cancel(string MB)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iCombinePcbInLot.Cancel(MB);

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