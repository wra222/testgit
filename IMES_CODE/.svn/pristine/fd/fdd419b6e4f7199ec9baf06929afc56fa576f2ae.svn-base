<%@ WebService Language="C#" Class="WebServicePACosmetic" %>


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
public class WebServicePACosmetic : System.Web.Services.WebService
{
    private IPACosmetic iPACosmetic = null;
    private IDefect iDefect = null;
    private IProduct iProduct = null;
    private string type = "PRD";
    
    [WebMethod]
    public IList input(string pdLine, string prodId, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ProductInfo info;
        ProductStatusInfo statusInfo;
        IList<DefectInfo> defectList;
        string tips = string.Empty;
        string swc = "69";

        try
        {
            iPACosmetic = ServiceAgent.getInstance().GetObjectByName<IPACosmetic>(WebConstant.PACosmeticObject);
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(WebConstant.CommonObject);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
            string checkItem = string.Empty;
            bool pcidflag = false;
            bool wwanflag = false;

            string ss = iPACosmetic.InputCustSn(pdLine, prodId, editor, station, customer);
            string[] sss = ss.Split(new char[]{'~'});
            checkItem = sss[0];
            string message = null;
            if (sss.Length > 1)
                message = sss[1];
            
            info = iProduct.GetProductInfoByCustomSn(prodId);
            tips = iPACosmetic.getOtherTips(info.id.ToString(), swc);
            statusInfo = iProduct.GetProductStatusInfo(prodId);
            defectList = iDefect.GetDefectList(type);
            if (checkItem == "PCID")
            {
                pcidflag = true;
            }
            else if (checkItem == "WWAN")
            {
                wwanflag = true;
            }
            //È±ÉÙwwancheckµÄÅÐ¶Ï
            ret.Add(info);
            ret.Add(statusInfo);
            ret.Add(defectList);
            ret.Add(pcidflag);
            ret.Add(wwanflag);
            ret.Add(tips);
            if (message != null)
                ret.Add(message);
            
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
    public ArrayList checkItem(string custsn, string data, bool checkPicdFlag, bool checkWwanFlag)
    {
        ArrayList ret = new ArrayList();
        bool errorFlgPicd = false;
        bool errorFlgWwan = false; 
        try
        {
            iPACosmetic = ServiceAgent.getInstance().GetObjectByName<IPACosmetic>(WebConstant.PACosmeticObject);
            if (checkPicdFlag)
            {
                errorFlgPicd = iPACosmetic.checkpcid(custsn, data);
            }
            else if (checkWwanFlag)
            {
                errorFlgWwan = iPACosmetic.checkwwan(custsn, data);
            }
            ret.Add(data);
            ret.Add(errorFlgPicd);
            ret.Add(errorFlgWwan);

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
    public void save(string prodId, IList<string> defectList)
    {
        try
        {
            iPACosmetic = ServiceAgent.getInstance().GetObjectByName<IPACosmetic>(WebConstant.PACosmeticObject);
            iPACosmetic.InputDefectCodeList(prodId, defectList);
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
            iPACosmetic = ServiceAgent.getInstance().GetObjectByName<IPACosmetic>(WebConstant.PACosmeticObject);
            iPACosmetic.Cancel(productId);
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