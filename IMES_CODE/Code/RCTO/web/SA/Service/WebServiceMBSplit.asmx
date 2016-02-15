<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:MB Split
 * UI:CI-MES12-SPEC-SA-UI MB Split.docx 
 * UC:CI-MES12-SPEC-SA-UC MB Split.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	连板切割入口，实现连板的切割
 *                2.	打印子板标签
 * UC Revision: 3924
 */
--%>

<%@ WebService Language="C#" Class="WebServiceMBSplit" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceMBSplit : System.Web.Services.WebService
{

    IMBSplit iMBSplit = ServiceAgent.getInstance().GetObjectByName<IMBSplit>(WebConstant.MBSplitObject);
    
    private const string SUCCESS = "SUCCESSRET";

    [WebMethod]
    public ArrayList InputMBSno(string pdLine, string mbsno, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        string model = string.Empty;
        IList<string> MBObjectList = new List<string>();
        try
        {
            IList<PrintItem> retLst=iMBSplit.inputMBSno(pdLine, mbsno, editor,station, customer, printItems, out model, out MBObjectList);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(model);
            ret.Add(MBObjectList);
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
    public string Cancel(string uutSn)
    {
        try
        {
            iMBSplit.Cancel(uutSn);
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


    [WebMethod]
    public ArrayList reprint(string mbsno, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<PrintItem> retLst = iMBSplit.rePrint(mbsno, reason, line, editor, station, customer, printItems);
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
}

