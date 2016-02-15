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
<%@ WebService Language="C#" Class="WebServiceCoaChange" %>


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
public class WebServiceCoaChange : System.Web.Services.WebService
{
    ICOAStatusChange iCOAStatusChange = ServiceAgent.getInstance().GetObjectByName<ICOAStatusChange>(WebConstant.COAStatusChangeObject);

    
    [WebMethod]
    public ArrayList GetCoaListForBuyer(List<string> coaNoList)
    {
        ArrayList retList = new ArrayList();
        IList<S_RowData_COAStatus> tmpList = new List<S_RowData_COAStatus>();
        List<string>  tempCoaNoList = new List<string>();
        foreach (string tmpStr in coaNoList)
        {
            if (tempCoaNoList.IndexOf(tmpStr) != -1)
            { }
            else
            {
                tempCoaNoList.Add(tmpStr);
            }
        }
        if (tempCoaNoList.Count == 0)
        {
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(null);
            return retList;
        }
        try
        {
            tmpList = iCOAStatusChange.GetListOneByOne(tempCoaNoList);
            ArrayList arrayConvert = new ArrayList();
            foreach (S_RowData_COAStatus tmp in tmpList)
            { 
                ArrayList temp = new ArrayList();
                temp.Add(tmp.CoaNO);
                temp.Add(tmp.Status);
                temp.Add(tmp.Pno);
                temp.Add(tmp.pdLine);
                arrayConvert.Add(temp);
            }
            retList.Add(WebConstant.SUCCESSRET);
            retList.Add(arrayConvert);
            return retList;
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
    public ArrayList UpdateCOAListLot(List<string> coaNoList, string editor, string status, string pdLine, string station)
    {
        ArrayList retList = new ArrayList();
        try
        {
            iCOAStatusChange.UpdateCOAList(coaNoList, editor,  status,  pdLine,   station);
            retList.Add(WebConstant.SUCCESSRET);
            return retList; 
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
