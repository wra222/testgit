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
<%@ WebService Language="C#" Class="WebServiceOfflinePizzaKittingForRCTO" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceOfflinePizzaKittingForRCTO : System.Web.Services.WebService
{
    IOfflinePizzaKittingForRCTO offlinePizzaKitting = ServiceAgent.getInstance().GetObjectByName<IOfflinePizzaKittingForRCTO>(WebConstant.OfflinePizzaKittingForRCTO);
    private IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    private string type = "PRD";
    
    [WebMethod]
    public IList GetBOM(string model, string lastKey, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        string key = Guid.NewGuid().ToString();
            
        try 
        {
            ArrayList tmpList = offlinePizzaKitting.GetBOM(model, key, lastKey, line, editor, station, customer);
        
            ret.Add(WebConstant.SUCCESSRET); // ret idx 0
            ret.Add(key);
			ret.Add(tmpList[0]); // ret idx 2
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
    public IList checkPart(string key, string part,string partNo)
    {
        try
        {
            IList ret = new ArrayList();

            List<string> item = offlinePizzaKitting.TryPartMatchCheck(key, part, partNo);
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

    [WebMethod]
    public IList Save(IList<PrintItem> printItems,string key,string line)
    {
        try
        {
            ArrayList ret = new ArrayList();
            ArrayList tmpList = offlinePizzaKitting.Save(printItems, key,line);
            ret.Add(WebConstant.SUCCESSRET); // ret idx 0
            ret.Add(tmpList[0]); //Pizza ID
            ret.Add(tmpList[1]); // PrintItem
            ret.Add(tmpList[2]); // new key
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
    public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            ArrayList arr = offlinePizzaKitting.RePrint(sn, reason, line, editor, station, customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(arr[0]);
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
    public void Cancel(string key)
    {
        try
        {
            offlinePizzaKitting.Cancel(key);
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
