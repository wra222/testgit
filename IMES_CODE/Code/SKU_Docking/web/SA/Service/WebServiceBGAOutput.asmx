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
<%@ WebService Language="C#" Class="WebServiceBGAOutput" %>


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
public class WebServiceBGAOutput : System.Web.Services.WebService
{
    private BGAOutput output = ServiceAgent.getInstance().GetObjectByName<BGAOutput>(WebConstant.IBGAOutput);
    
    [WebMethod]
    public IList inputSno(string sno, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
                
        try
        {
            string line = string.Empty;
            ArrayList tmpList = output.InputSno(sno, line, editor, station, customer);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//sno
            ret.Add(tmpList[1]);//repairlist

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
    public ArrayList addNew(string sno, string reworkStation)
    {
        try
        {
            ArrayList ret = new ArrayList();
            Rework rework = output.addNew(sno, reworkStation);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(rework);
            
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
    public void save(string sno)
    {
        try
        {
            output.save(sno);
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
    public void cancel(string sno)
    {
        try
        {
            output.cancel(sno);
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
