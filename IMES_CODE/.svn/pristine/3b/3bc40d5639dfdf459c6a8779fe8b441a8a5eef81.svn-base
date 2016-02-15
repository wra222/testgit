<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Unit Weight

 */
--%>
<%@ WebService Language="C#" Class="WebServiceUnitWeightNew" %>


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
public class WebServiceUnitWeightNew : System.Web.Services.WebService 
{

    IPakUnitWeightNew iPakUnitWeight = ServiceAgent.getInstance().GetObjectByName<IPakUnitWeightNew>(WebConstant.PakUnitWeightNewObject);

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
          inputRet = iPakUnitWeight.InputCustsn(custSN, Convert.ToDecimal(actWeight), line, editor, station, customer, out configparams);
          ret.Add(WebConstant.SUCCESSRET);
          ret.Add(inputRet);
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
    public ArrayList Save(string productID, string model,IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<PrintItem> retLst = iPakUnitWeight.Save(productID, printItems);
            string color = iPakUnitWeight.GetCqPodLabelColor(model);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(color);
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
            string color = iPakUnitWeight.GetCqPodLabelColor(model);
            retlist.Add(color);
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
    [WebMethod]
    public string GetCqPodLabelColor(string model)
    {
        try
        {
            string color = iPakUnitWeight.GetCqPodLabelColor(model);
           return color;
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
    public ArrayList GetPODLabelPathAndSite()
    {
        try
        {
            ArrayList arr = iPakUnitWeight.GetPODLabelPathAndSite();
            return arr;
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
    public IList checkPart(string custsSN, string part)
    {
        try
        {
            IList ret = new ArrayList();

            MatchedPartOrCheckItem item = iPakUnitWeight.TryPartMatchCheck(custsSN, part);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(item);

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

