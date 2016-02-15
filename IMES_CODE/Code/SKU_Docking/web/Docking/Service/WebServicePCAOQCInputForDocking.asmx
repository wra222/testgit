<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Output
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Input.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Input.docx           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-31  Kaisheng               Create
 * Known issues:
 * TODO：
 * UC 具体业务： 
 *                
 * UC Revision: 
 */
--%>

<%@ WebService Language="C#" Class="WebServicePCAOQCInputForDocking" %>

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

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePCAOQCInputForDocking : System.Web.Services.WebService
{

    IPCAOQCInput iPCAOQCInput = ServiceAgent.getInstance().GetObjectByName<IPCAOQCInput>(WebConstant.PCAOQCInputObjectDocking);
    
    IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    
    private const string SUCCESS = "SUCCESSRET";

    [WebMethod]
    public ArrayList InputMBorLot(string Inputstring, string strType, string editor,string station, string customer)
    {
        ArrayList ret = new ArrayList();
        string model = string.Empty;
        try
        {
            ArrayList tmpList = iPCAOQCInput.inputMBSnoORLotNo(Inputstring, strType, editor, station, customer);
            //--------Service 
            //lstRet.Add(ReturnLotInfo[0].lotNo);      //0
            //lstRet.Add(lotLineinfo);                 //1 
            //lstRet.Add(ReturnLotInfo[0].type);       //2
            //lstRet.Add(ReturnLotInfo[0].qty);        //3 
            //lstRet.Add(strStatus);                   //4
            //lstRet.Add(mbsnList);                    //5
            //lstRet.Add(ReturnLotInfo[0].line.Trim());//6
            //lstRet.Add(checkedList);                 //7
            //lstRet.Add(checkQtyforLine);             //8
            //lstRet.Add(iCheckqty);                   //9
            //lstRet.Add(havePromptstr);              //10
            ret.Add(WebConstant.SUCCESSRET);        //0
            ret.Add(tmpList[0]);//lotNo             //1
            ret.Add(tmpList[1]);//lotLineinfo       //2
            ret.Add(tmpList[2]);//type              //3
            ret.Add(tmpList[3]);//qty;              //4
            ret.Add(tmpList[4]);//strStatus         //5
            ret.Add(tmpList[5]);//mbsnList          //6
            ret.Add(tmpList[6]);//line              //7 
            ret.Add(tmpList[7]);//checkedlist       //8
            ret.Add(tmpList[8]);//checkQtyforLine   //9
            ret.Add(tmpList[9]);//LotCheckQty       //10
            ret.Add(tmpList[10]);                   //11
            
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
    public ArrayList InsertmbsntoPcbLotCheck(string lotNo, string mbSno, int icheckQty,string line, string editor,string customer)
    {
        ArrayList ret = new ArrayList();
        try
        {
            //ArrayList InsertPcbLotCheck(string lotNo, String mbSno, string editor, string line, string customer);
             //retLst.Add(mbsnList);
             //retLst.Add(checkedList);
             //retLst.Add(iCheckqty);
            ArrayList tmpList = iPCAOQCInput.InsertPcbLotCheck(lotNo,mbSno,editor,line,customer);
            ret.Add(WebConstant.SUCCESSRET);        //0
            ret.Add(tmpList[0]);//mbsnList           //1
            ret.Add(tmpList[1]);//checkedlist        //2
            ret.Add(tmpList[2]);//iCheckqty          //3
            if ((int)tmpList[2] >= icheckQty)
                ret.Add("GotoPass");                 //4 
            else//4
                ret.Add("None");
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
    public string save(string LotNo, string strCMD, string editor, string line, string customer)
    {
        try
        {
            iPCAOQCInput.save(LotNo, strCMD, editor, line, customer);
            return (WebConstant.SUCCESSRET);
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
    public string Cancel(string uutSn)
    {
        try
        {
            iPCAOQCInput.Cancel(uutSn);
            return (WebConstant.SUCCESSRET);
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }

        catch (Exception e)
        {
            throw e;
        }

    }
    


    
}

