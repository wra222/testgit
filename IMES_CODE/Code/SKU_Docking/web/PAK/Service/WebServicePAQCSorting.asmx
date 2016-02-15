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
<%@ WebService Language="C#" Class="WebServicePAQCSorting" %>


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
public class WebServicePAQCSorting : System.Web.Services.WebService
{
    private IPAQCSorting iPAQCSorting = ServiceAgent.getInstance().GetObjectByName<IPAQCSorting>(WebConstant.PAQCSortingObject);
    private string commBllName = WebConstant.CommonObject;
    private string type = "PRD";
    
    [WebMethod]
    public IList input(string pdLine, string custSN, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ArrayList tmpList = new ArrayList();
        
        try
        {
            tmpList = iPAQCSorting.InputSN(custSN, pdLine, editor, station, customer);

            ret.Add(tmpList[0]);

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
    public IList save(int sortingID, string custSN,string editor, string station, string customer)
    {
        try
        {
            IList ret = new ArrayList();
            ArrayList tmpList = new ArrayList();
            
            tmpList = iPAQCSorting.save(sortingID,custSN, editor, station,customer);

            ret.Add(tmpList[0]);

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
    public IList updateStation(int sortingID, string editor, string station, string customer)
    {
        try
        {
            IList ret = new ArrayList();
            ArrayList tmpList = new ArrayList();

            //tmpList = iPAQCSorting.updateStation(sortingID, editor, station, customer);

            //ret.Add(tmpList[0]);

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
    public IList addSorting(string customer, string line, string station, string editor, string remark)
    {
        try
        {
            IList ret = new ArrayList();
            ArrayList tmpList = new ArrayList();

            tmpList = iPAQCSorting.addSorting(customer, line,station,editor,remark);

            //ret.Add(tmpList[0]);
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
