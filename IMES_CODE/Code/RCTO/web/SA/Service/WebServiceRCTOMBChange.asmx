<%--
 /*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:WebMethod for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-1    Jessica Liu           Create
* Known issues:
* TODO：
*/
 --%>
 
<%@ WebService Language="C#" Class="WebServiceRCTOMBChange" %>


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

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceRCTOMBChange : System.Web.Services.WebService
{
    IRCTOMBChange iRCTOMBChange = ServiceAgent.getInstance().GetObjectByName<IRCTOMBChange>(WebConstant.RCTOMBChangeObject);

    [WebMethod]
    public List<string> checkMBSNo(string mbSno, string pdLine, string stationId, string editor, string customerId)
    {
        List<string> ret = new List<string>();
        List<string> retLst = new List<string>();
        string mbsno = "";
        string model = "";
        
        try
        {
            retLst = iRCTOMBChange.CheckMBSNo(mbSno, pdLine, stationId, editor, customerId);
            if (retLst != null)
            {
                mbsno = retLst[0];
                model = retLst[1];
            }

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(mbsno);
            ret.Add(model);
            
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
    public ArrayList saveAndPrint(string mbSno, string pdLine, string stationId, string editor, string customerId, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> retLst = new List<PrintItem>();
        //2012-9-7, Jessica Liu
        string newMBSno = "";
            
        try
        {
            //2012-9-7, Jessica Liu
            //retLst = iRCTOMBChange.SaveAndPrint(mbSno, pdLine, stationId, editor, customerId, printItems);
            retLst = iRCTOMBChange.SaveAndPrint(mbSno, pdLine, stationId, editor, customerId, printItems, out newMBSno);

            //2012-9-7, Jessica Liu
            if (string.IsNullOrEmpty(newMBSno))
            {
                newMBSno = " ";
            }
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(newMBSno);

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
    public ArrayList rePrint(string mbSno, string reason, string pdLine, string customerId, string editor, string stationId, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        IList<PrintItem> retLst = new List<PrintItem>();
        
        try
        {
            retLst = iRCTOMBChange.RePrint(mbSno, reason, pdLine, customerId, editor, stationId, printItems);
            
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
    public string Cancel(string key, string station)
    {
        try
        {
            iRCTOMBChange.Cancel(key);

            return "";

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
