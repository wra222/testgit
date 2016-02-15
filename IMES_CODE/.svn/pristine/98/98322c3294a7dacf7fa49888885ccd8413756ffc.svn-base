<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:PACosmetic page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-09  Du Xuan               Create          
 * Known issues:
 * TODO:
 */
 --%>

<%@ WebService Language="C#" Class="WebServicePACosmeticDocking" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePACosmeticDocking : System.Web.Services.WebService
{
    private IDefect iDefect = null;
    private IProduct iProduct = null;
    private string type = "PRD";

    IPACosmeticDocking iPACosmetic = (IPACosmeticDocking)ServiceAgent.getInstance().GetObjectByName<IPACosmeticDocking>(WebConstant.PACosmeticObjectDocking);
    
    [WebMethod]
    public IList input(string pdLine, string prodId, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        IList<DefectInfo> defectList;
        //string swc = "69";

        try
        {
            
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
            
            ArrayList tmpList = iPACosmetic.InputCustSn(pdLine, prodId, editor, station, customer);
            
            defectList = iDefect.GetDefectList(type);
            
            //È±ÉÙwwancheckµÄÅÐ¶Ï
            ret.Add(tmpList[0]); //ProductInfo
            ret.Add(tmpList[1]); //ProductStatusInfo
            ret.Add(defectList);
            
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