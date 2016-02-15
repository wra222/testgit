<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: WebServiceOQCOutput
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-21  itc202017             Create   
               
 * Known issues:
 */
 --%>
<%@ WebService Language="C#" Class="OQCOutputService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class OQCOutputService : System.Web.Services.WebService
{
    private IOQCOutput iOQCOutput = ServiceAgent.getInstance().GetObjectByName<IOQCOutput>(WebConstant.OQCOutputObject);

    [WebMethod]
    public ArrayList input(string input, bool isID, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            IList<string> ret = iOQCOutput.InputProduct(input, isID, editor, station, customer);
            retLst.Add(ret);
            return retLst;
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
            iOQCOutput.InputDefectCodeList(prodId, defectList);
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
    public void cancel(string prodId)
    {
        try
        {
            iOQCOutput.Cancel(prodId);
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

