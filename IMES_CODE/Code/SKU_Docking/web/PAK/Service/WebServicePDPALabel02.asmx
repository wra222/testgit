<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServicePDPALabel02" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePDPALabel02 : System.Web.Services.WebService
{
    IPDPALabel02 pdpalabe2 = ServiceAgent.getInstance().GetObjectByName<IPDPALabel02>(WebConstant.IPDPALabel02);
    private string type = "PRD";

    [WebMethod]
    public ArrayList InputSNforCQ(bool queryflag, string pdLine, string code, string floor, string custSN, string editor, string station, string customer)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        ProductModel curProduct = new ProductModel();

        try
        {
            //  tmpList = pdpalabe2.InputSN(queryflag, custSN, pdLine, code, floor, editor, station, customer);
            tmpList = pdpalabe2.InputSNforCQ(queryflag, custSN, pdLine, code, floor, editor, station, customer);

            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]); //productMode
            retList.Add(tmpList[1]); //wwanCheck
            retList.Add(tmpList[2]); //hitachiCheck
            retList.Add(tmpList[3]); //Warranty Label Check
            retList.Add(tmpList[4]); //Part List
            return retList;
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
    public ArrayList InputSN(bool queryflag, string pdLine, string code, string floor, string custSN, string editor, string station, string customer)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        ProductModel curProduct = new ProductModel();

        try
        {
            tmpList = pdpalabe2.InputSN(queryflag, custSN, pdLine, code, floor, editor, station, customer);
            
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]); //productMode
            retList.Add(tmpList[1]); //wwanCheck
            retList.Add(tmpList[2]); //hitachiCheck
            retList.Add(tmpList[3]); //Warranty Label Check
            retList.Add(tmpList[4]); //AT4check
    
            return retList;
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
    public ArrayList InputWWANID(string prodId, string WWANID)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
            tmpList = pdpalabe2.InputWWANID(prodId,WWANID);
            
            retList.Add(WebConstant.SUCCESSRET);
            //retList.Add(tmpList[0]);//sn check falg
            //retList.Add(true);//sn check falg
            
            return retList;
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
    public ArrayList checkCOA(string prodId, string WWANSN)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
            tmpList = pdpalabe2.checkCOA(prodId,WWANSN);
            
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]);//wipBufferList
            retList.Add(tmpList[1]);//DeliveryNo
            retList.Add(tmpList[2]);//PalletNo
            retList.Add(tmpList[3]);//boxid
            retList.Add(tmpList[4]);//pdf
            retList.Add(tmpList[5]);//templatename
            retList.Add(tmpList[6]);//need print ast list
            
            return retList;
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
    public ArrayList checkCOAQuery(string prodId)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
            tmpList = pdpalabe2.checkCOAQuery(prodId);

            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]);//wipBufferList
            //retList.Add(tmpList[1]);//DeliveryNo
            //retList.Add(tmpList[2]);//PalletNo
            //retList.Add(tmpList[3]);//boxid
            //retList.Add(tmpList[4]);//pdf
            //retList.Add(tmpList[5]);//templatename

            return retList;
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
    public ArrayList Print(string productID, string line,string code, string floor, 
                                string editor, string station, string customer,IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();

        try
        {
            IList<PrintItem> retLst = pdpalabe2.Print(productID, line, code, floor, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
           
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

    [WebMethod]
    public void cancel(string productId)
    {
        try
        {
            pdpalabe2.cancel(productId);
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
    public ArrayList GetCommSetting(string hostname,string editor)
    {
        ILabelLightGuide iLabelLightGuide = ServiceAgent.getInstance().GetObjectByName<ILabelLightGuide>(WebConstant.LabelLightGuideObject);

        List<string> configparams = new List<string>();
        ArrayList ret = new ArrayList();
        
        try
        {
            IList<COMSettingInfo> wsiList = iLabelLightGuide.getCommSetting(hostname, editor);
            
                ret.Add(WebConstant.SUCCESSRET);
                ret.Add(wsiList[0].commPort.ToString());
                ret.Add(wsiList[0].baudRate);
                ret.Add(wsiList[0].rthreshold.ToString());
                ret.Add(wsiList[0].sthreshold.ToString());
                ret.Add(wsiList[0].handshaking.ToString());
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
    public ArrayList GetSysSetting(string name)
    {
        ArrayList ret = new ArrayList();
        string value = "";  
        try
        {
            value = pdpalabe2.GetSysSetting(name);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(value);
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
    public ArrayList GetSysSettingList(IList<string> name)
    {
        ArrayList ret = new ArrayList();
        ArrayList value = new ArrayList();  
        try
        {
            value = pdpalabe2.GetSysSettingList(name);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(value);
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
    
}
