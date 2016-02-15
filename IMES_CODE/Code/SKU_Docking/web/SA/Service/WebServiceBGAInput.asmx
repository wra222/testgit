<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Input
* UI:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   liuqingbiao           Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceBGAInput" %>


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
public class WebServiceBGAInput : System.Web.Services.WebService
{
    //private BGAInput input = ServiceAgent.getInstance().GetObjectByName<BGAInput>(WebConstant.IBGAInput);
    IBGAInput iBGAInput = ServiceAgent.getInstance().GetObjectByName<IBGAInput>(WebConstant.BGAInputObject);
    
    [WebMethod]
    public ArrayList inputSno(string sno, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
                
        try
        {
            string line = string.Empty;
            ArrayList tmpList = iBGAInput.InputSno(sno, line, editor, station, customer);
            

            ret.Add(WebConstant.SUCCESSRET);
            
            //ret.Add(tmpList[0]);//sno
            //ret.Add(tmpList[1]);//repairlist
            ret.Add(tmpList);
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
    public void save(string snoScaned)//string sno, IList<string> reworkList)
    {
        try
        {
            iBGAInput.save(snoScaned);//sno, reworkList);
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
            iBGAInput.cancel(sno);
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
