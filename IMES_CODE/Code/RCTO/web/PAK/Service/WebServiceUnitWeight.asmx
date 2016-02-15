<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Unit Weight
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx –2011/11/25
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx –2011/11/25         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-25   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  本站站号：85
 *                1. Unit 称重；
 *                2. 列印Unit Weight Label；
 *                3. 上传数据至SAP
 */
--%>
<%@ WebService Language="C#" Class="WebServiceUnitWeight" %>


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


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceUnitWeight : System.Web.Services.WebService 
{

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    IPakUnitWeight iPakUnitWeight = ServiceAgent.getInstance().GetObjectByName<IPakUnitWeight>(WebConstant.PakUnitWeightObject);

    [WebMethod]
    public ArrayList GetWeightSetting(string hostname)
    {
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
    
    [WebMethod]
    public ArrayList inputCustSN(string custSN,string actWeight, string line, string editor, string station, string customer)
    {
        List<string> configparams = new List<string>();
        ArrayList ret = new ArrayList();
        ArrayList inputRet = new ArrayList();
        try
        {
           inputRet = iPakUnitWeight.InputUUT(custSN, Convert.ToDecimal(actWeight), line, editor, station, customer, out configparams);
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(inputRet);
            ret.Add(configparams);
            return ret;
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "CHK020")
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
      

    [WebMethod]
    public ArrayList checkRMN(string productID, string RMN)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iPakUnitWeight.CheckRMN(productID, RMN);
            ret.Add(WebConstant.SUCCESSRET);
            return ret;
        }
        catch (FisException ex)
        {
           // throw new Exception(ex.mErrmsg);
            if (ex.mErrcode == "PAK033" || ex.mErrcode == "PAK032" || ex.mErrcode == "PAK073")
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [WebMethod]
    public ArrayList checkBoxIDorUCC(string productID, string RMN)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iPakUnitWeight.CheckBoxIDorUCC(productID, RMN);
            ret.Add(WebConstant.SUCCESSRET);
            return ret;
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "PAK035" || ex.mErrcode == "PAK034")
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public ArrayList save(string productID, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();

        try
        {
            IList<PrintItem> retLst = iPakUnitWeight.Save(productID, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
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
    public string Cancel(string productID)
    {
        try
        {
            iPakUnitWeight.Cancel(productID);
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
    public ArrayList reprint(string custSN, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItemList)
    {
        ArrayList retlist = new ArrayList();
        try
        {
            string printlabeltype ="";
            string model = "";
            string printexepath = "";
            IList<PrintItem> retprintItems = iPakUnitWeight.ReprintLabel(custSN, reason, line, editor, station, customer, printItemList, out printlabeltype, out model, out printexepath);
            
            retlist.Add(WebConstant.SUCCESSRET);
            retlist.Add(retprintItems);
            retlist.Add(printlabeltype);
            retlist.Add(model);
            retlist.Add(printexepath);
            return retlist;
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

