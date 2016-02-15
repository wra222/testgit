<%--
/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:WebMethod implement for UnitWeight (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight for Docking
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight for Docking            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-29  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
--%>
<%@ WebService Language="C#" Class="WebServiceUnitWeight" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Docking.Interface.DockingIntf;
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
    IPakUnitWeight iPakUnitWeight = ServiceAgent.getInstance().GetObjectByName<IPakUnitWeight>(WebConstant.PakUnitWeightForDockingObject);

    [WebMethod]
    public ArrayList inputCustSN(string custSN,string actWeight, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        ArrayList inputRet = new ArrayList();
        try
        {
           inputRet = iPakUnitWeight.InputUUT(custSN, Convert.ToDecimal(actWeight), line, editor, station, customer);
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(inputRet);
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
    public ArrayList save(string productID)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iPakUnitWeight.Save(productID);
            ret.Add(WebConstant.SUCCESSRET);
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

}

