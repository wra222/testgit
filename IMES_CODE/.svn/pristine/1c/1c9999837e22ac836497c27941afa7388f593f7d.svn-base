<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServicePDPALabel01" %>


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
public class WebServicePDPALabel01 : System.Web.Services.WebService
{
    IPDPALabel01 pdpalabel = ServiceAgent.getInstance().GetObjectByName<IPDPALabel01>(WebConstant.IPDPALabel01);
    private IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    private string type = "PRD";
    
    [WebMethod]
    public IList input(bool queryflag, string pdLine, string code, string floor, string custSN, string editor, string station, string customer)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();  
        ProductModel curProduct;
        
        try
        {
            tmpList = pdpalabel.InputSN(queryflag, custSN, pdLine, code, floor, editor, station, customer);
            curProduct = (ProductModel)tmpList[0];
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]);//model
            retList.Add(tmpList[1]);//wipBufferList
            retList.Add(tmpList[2]);//wlabel
            retList.Add(tmpList[3]);//clabel
            retList.Add(tmpList[4]);//cmessage
            retList.Add(tmpList[5]);//llabel
            //add by Benson
            retList.Add(tmpList[6]);//win8label
            //add by Benson
            retList.Add(tmpList[7]);//postellabel
            
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
    public IList input_CQ(bool queryflag, string pdLine, string code, string floor, string custSN, string editor, string station, string customer)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        ProductModel curProduct;
        try
        {
            tmpList = pdpalabel.InputSN(queryflag, custSN, pdLine, code, floor, editor, station, customer);
            curProduct = (ProductModel)tmpList[0];
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]);//model
            retList.Add(tmpList[1]);//wipBufferList
            retList.Add(tmpList[2]);//wlabel
            retList.Add(tmpList[3]);//clabel
            retList.Add(tmpList[4]);//cmessage
            retList.Add(tmpList[5]);//llabel
            //add by Benson
            retList.Add(tmpList[6]);//win8label
            retList.Add(tmpList[7]);//energy label
            //add by Benson
            retList.Add(tmpList[8]);//other label
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
            IList<PrintItem> retLst = pdpalabel.Print(productID, line, code, floor, editor, station, customer, printItems);
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
    public ArrayList Reprint(string customerSN, string reason, IList<PrintItem> printItems,
                    string line, string editor, string station, string customer)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();

        try
        {
            tmpList = pdpalabel.ReprintLabel(customerSN, reason, printItems, line, editor, station, customer);

            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(tmpList[0]);//printList
            retList.Add(tmpList[1]);//wlabel
            retList.Add(tmpList[2]);//clabel
            retList.Add(tmpList[3]);//cmessage
            retList.Add(tmpList[4]);//llabel
            retList.Add(tmpList[5]);//win8label
            retList.Add(tmpList[6]);//postellabel
            
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
    public void cancel(string productId)
    {
        try
        {
            pdpalabel.cancel(productId);
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
    public ArrayList GetCommSetting(string hostname, string editor)
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
        IPDPALabel02 pdpalabe2 = ServiceAgent.getInstance().GetObjectByName<IPDPALabel02>(WebConstant.IPDPALabel02);
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
             
}
