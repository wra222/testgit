<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/11/04 
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx –2011/11/04            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-04   Du.Xuan               Create   
* Known issues:
* TODO：
*/
 --%>
<%@ WebService Language="C#" Class="PalletWeightWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Infrastructure;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class PalletWeightWebService  : System.Web.Services.WebService 
{
    IPalletWeight palletweightManager = (IPalletWeight)ServiceAgent.getInstance().GetObjectByName<IPalletWeight>(WebConstant.PalletWeight);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="palletNo"></param>
    /// <param name="actualWeight"></param>
    /// <param name="line"></param>
    /// <param name="editor"></param>
    /// <param name="station"></param>
    /// <param name="customer"></param>
    /// <returns></returns>
    [WebMethod]
    public ArrayList InputPallet(string inputID, string actualWeight,string type, string line, string editor, string station, string customer)
    {
        ArrayList retlist = new ArrayList();
        
        try
        {
           ArrayList tempList = palletweightManager.InputPallet(inputID, decimal.Parse(actualWeight),type, 
                                                    line, editor, station, customer);
            
            retlist.Add(WebConstant.SUCCESSRET);
            retlist.Add(tempList[0]); //chkFlag
            retlist.Add(tempList[1]); //PalletNo
            retlist.Add(tempList[2]); //palletType
            retlist.Add(tempList[3]); //command
            retlist.Add(tempList[4]); //modelList
            retlist.Add(tempList[5]); //standardWeight
            retlist.Add(tempList[6]); //tolerance
            retlist.Add(tempList[7]); //提示字符串
            retlist.Add(tempList[8]); //checkType是否检查pallettype
            retlist.Add(tempList[9]); //cartonflag是否刷cartsn
        }
        catch (FisException ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.mErrmsg);
            if (ex.mErrcode == "CHK133")
            {
                retlist.Add(true);
            }
        }
        catch (Exception ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.Message);
        }
        return retlist;
    }

    [WebMethod]
    public ArrayList InputCustSN(string custSN, string palletNo,string palletType,string modelType)
    {
        ArrayList retlist = null;
        try
        {
            ArrayList tmpList = palletweightManager.InputCustSN(custSN, palletNo, palletType, modelType);
            retlist = new ArrayList();
            retlist.Add(WebConstant.SUCCESSRET);
            retlist.Add(tmpList[0]);//custSN
            retlist.Add(tmpList[1]);//cartonSN
        }
        catch (FisException ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.mErrmsg);
            if (ex.mErrcode == "CHK133")
            {
                retlist.Add(true);
            }
        }
        catch (Exception ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.Message);
        }
        return retlist;
    }

    [WebMethod]
    public ArrayList inputIMEI(string custSN, string imei)
    {
        ArrayList retlist = null;
        try
        {
            String imeistr = palletweightManager.CheckIMEI(custSN, imei);
            retlist = new ArrayList();
            retlist.Add(WebConstant.SUCCESSRET);
            retlist.Add(imeistr);
        }
        catch (FisException ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.mErrmsg);
            if (ex.mErrcode == "CHK133")
            {
                retlist.Add(true);
            }
        }
        catch (Exception ex)
        {
            retlist = new ArrayList();
            retlist.Add(ex.Message);
        }
        return retlist;
    }
    /// <summary>
    /// 保存机器重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
    /// 将printItems放到Session.PrintItems中
    /// 将actualWeight放到Session.ActuralWeight中
    /// </summary>
    /// <param name="productID"></param>
    /// <param name="actualWeight"></param>
    /// <param name="printItems"></param>
    /// <returns></returns>
    [WebMethod]
    public ArrayList Save(string palletNo,string custSN,string actualWeight, IList<PrintItem> printItems)
    {
        ArrayList retlist = new ArrayList();
        try
        {
            StandardWeight sw = new StandardWeight();

            IList<PrintItem> retprintItems = palletweightManager.Save(palletNo, custSN, decimal.Parse(actualWeight), printItems);
           // retlist = new ArrayList();
            retlist.Add(WebConstant.SUCCESSRET);
            retlist.Add(retprintItems);
            
        }
        /*catch (FisException ex)
        {
            retlist = new ArrayList();
            
            //if (ex.mErrcode == "CHK016")
            //{
                retlist.Add("CHECK");
                retlist.Add(ex.mErrcode);
                retlist.Add(ex.mErrmsg);
                retlist.Add(palletNo);
            //}
            
        }
        catch (Exception ex)
        {
            retlist = new ArrayList();
            retlist.Add("ERROR");
            retlist.Add(palletNo);
            retlist.Add(ex.Message);
        }*/
        catch (FisException e)
        {
            retlist.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retlist;
    }

    /// <summary>
    /// 取消工作流
    /// </summary>
    /// <param name="productID"></param>
    [WebMethod]
    public string Cancel(string productID)
    {
        try
        {
            palletweightManager.Cancel(productID);
            return "";
        }
        catch (FisException ex)
        {
            return ex.mErrmsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }





    /// <summary>
    /// 重印Unit Weight Label
    /// </summary>
    /// <param name="custSN"></param>
    /// <param name="line"></param>
    /// <param name="editor"></param>
    /// <param name="station"></param>
    /// <param name="customer"></param>
    /// <returns></returns>
    [WebMethod]
    public ArrayList ReprintPalletWeightLabel(string custSN, string reason,string line, string editor, string station, 
                                            string customer, IList<PrintItem> printItemList)
    {
        ArrayList retlist = new ArrayList();
        try
        {
            //decimal palletweight = 0;
            ArrayList tmpList = palletweightManager.ReprintPalletWeightLabel(custSN, reason, line, editor, station, customer, printItemList);
            
            retlist.Add(WebConstant.SUCCESSRET);;
            retlist.Add(tmpList[0]);//printItem
            retlist.Add(tmpList[1]);//PalletNo
            retlist.Add(tmpList[2]);//Weight
            
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
    
    [WebMethod]
    public ArrayList GetWeightSetting(string hostname)
    {
        IPakUnitWeight iPakUnitWeight = ServiceAgent.getInstance().GetObjectByName<IPakUnitWeight>(WebConstant.PakUnitWeightObject);

        List<string> configparams = new List<string>();
        ArrayList ret = new ArrayList();
        IList<COMSettingDef> UnitWeighInfo = new List<COMSettingDef>();
        try
        {
            UnitWeighInfo = iPakUnitWeight.GetWeightSettingInfo(hostname);
            if (UnitWeighInfo == null || UnitWeighInfo.Count < 0)
            {
                return null;
            }
            else
            {
                ret.Add(WebConstant.SUCCESSRET);
                ret.Add(UnitWeighInfo[0].commport);
                ret.Add(UnitWeighInfo[0].baudRate);
                ret.Add(UnitWeighInfo[0].rthreshold);
                ret.Add(UnitWeighInfo[0].sthreshold);
                ret.Add(UnitWeighInfo[0].handshaking);
                return ret;
            }


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

