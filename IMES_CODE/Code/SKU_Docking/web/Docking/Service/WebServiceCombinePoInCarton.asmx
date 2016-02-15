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
<%@ WebService Language="C#" Class="WebServiceCombinePoInCarton" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCombinePoInCarton : System.Web.Services.WebService
{

    ICombinePoInCarton CombinePoInCarton = ServiceAgent.getInstance().GetObjectByName<ICombinePoInCarton>(WebConstant.CombinePoInCartonObject);
    private string type = "PRD";
    
	[WebMethod]
    public ArrayList InputSN(string inputSN, string deliveryNo, string model,string firstProID,string line, string editor, string station, string customer)
	{
		return InputSN(inputSN, deliveryNo, model, firstProID, line, editor, station, customer, "");
	}
	
    [WebMethod]
    public ArrayList InputSN(string inputSN, string deliveryNo, string model,string firstProID,string line, string editor, string station, string customer, string isBT)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        ProductModel curProduct = new ProductModel();

        try
        {
            tmpList = CombinePoInCarton.InputSN(inputSN, deliveryNo, model, firstProID,line, editor, station, customer, isBT);
            // 
            if ((string)tmpList[0] == "Error")
            {
                retList.Add(tmpList[0]);
                retList.Add(tmpList[1]);
            }
            else
            {
                retList.Add(WebConstant.SUCCESSRET);
                retList.Add(tmpList[1]);//curModel;
                retList.Add(tmpList[2]);//curProduct.DeliveryNo);
                retList.Add(tmpList[3]);//Total Qty
                retList.Add(tmpList[4]);//PCs in Carton
                retList.Add(tmpList[5]);//Packed Qty
                retList.Add(tmpList[6]);//Delivery Item
				retList.Add(tmpList[7]);// whether BT or not ; whether need choose dn <= Y,N,NeedDN
            }
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
    public ArrayList Save(string firstProID, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList tmpLst = CombinePoInCarton.Save(firstProID, printItems);
            
            ret.Add(WebConstant.SUCCESSRET);            
            ret.Add(tmpLst[0]); //printlist
            ret.Add(tmpLst[1]); //dev flag
            ret.Add(tmpLst[2]); //packedQty
            ret.Add(tmpLst[3]); //carton SN
            ret.Add(tmpLst[4]); //Delivery No
            ret.Add(tmpLst[5]); //customer SN
            ret.Add(tmpLst[6]); //pdf temp
            ret.Add(tmpLst[7]); //strLoc
            ret.Add(tmpLst[8]); //strEnd
            ret.Add(tmpLst[9]); //printflag

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
    public void cancel(string deliverNo)
    {
        try
        {
            CombinePoInCarton.cancel(deliverNo);
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
            value = CombinePoInCarton.GetSysSetting(name);
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
            value = CombinePoInCarton.GetSysSettingList(name);
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
    public ArrayList GetSysSettingSafe(string name, string defValue, string hostname, string editor)
    {
        ArrayList ret = new ArrayList();
        try
        {
            string value = CombinePoInCarton.GetSysSettingSafe(name, defValue, hostname, editor);
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
    public void SetSysSetting(string name, string value, string hostname, string editor)
    {
        try
        {
            CombinePoInCarton.SetSysSetting(name, value, hostname, editor);
            return;

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
    public ArrayList ReprintCartonLabel(string inputID, string reason, string line, string editor, string station,
                                            string customer, IList<PrintItem> printItemList)
    {
        ArrayList retlist = new ArrayList();
        try
        {
            //decimal palletweight = 0;
            ArrayList tmpList = CombinePoInCarton.ReprintCartonLabel(inputID, reason, line, editor, station, customer, printItemList);

            retlist.Add(WebConstant.SUCCESSRET);
            retlist.Add(tmpList[0]);//printList);
            retlist.Add(tmpList[1]);//flagstr);
            retlist.Add(tmpList[2]);//0);
            retlist.Add(tmpList[3]);//curProduct.CartonSN);
            retlist.Add(tmpList[4]);//curProduct.DeliveryNo);
            retlist.Add(tmpList[5]);//curProduct.CUSTSN);
            retlist.Add(tmpList[6]);//templatename);
            retlist.Add(tmpList[7]);//cqty
            retlist.Add(tmpList[8]);//printflag
            
        }
        catch (FisException ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.Message);
        }
        return retlist;
    }
}
