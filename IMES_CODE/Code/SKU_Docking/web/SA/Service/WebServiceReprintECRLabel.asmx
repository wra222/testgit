<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:Reprint PCA ICT Input-02 ECR Label Reprint
 * UI:CI-MES12-SPEC-SA-UI PCA ICT Input.docx –2011/11/03
 * UC:CI-MES12-SPEC-SA-UC PCA ICT Input.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-17   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  标签损坏时，重印 ECR Label
 */
--%>

<%@ WebService Language="C#" Class="WebServiceReprintECRLabel" %>

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
public class WebServiceReprintECRLabel : System.Web.Services.WebService
{

    IICTInput iICTInput = ServiceAgent.getInstance().GetObjectByName<IICTInput>(WebConstant.ICTInputObject);
    
    private const string SUCCESS = "SUCCESSRET";

    [WebMethod]
    public ArrayList InputMBSno(string pdLine, string mbsno, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        string model = string.Empty;
        IList<MBInfo> mbinfo = new List<MBInfo>();
        try
        {
            mbinfo = iICTInput.EcrReprintInputMBSno(mbsno, editor, pdLine, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(mbinfo);
            //ret.Add(mbinfo.dateCode);
            //ret.Add(mbinfo.ecr);
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
            iICTInput.Cancel(uutSn);
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
    public ArrayList reprint(string mbsno, string reason, string ecr,string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<PrintItem> retLst = iICTInput.EcrReprint(mbsno, reason, ecr,editor, line, station,customer,printItems);
            
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
    public ArrayList reprints(string[] mbsnoList, string reason, string[] ecrList, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<PrintItem> retLst = iICTInput.EcrReprint(mbsnoList[0], reason, ecrList[0], editor, line, station, customer, printItems);
            string retMBSn = "";
            foreach (string item in mbsnoList)
            {
                retMBSn += item + ",";
            }
            retMBSn = retMBSn.Substring(0, retMBSn.Length - 1);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            ret.Add(mbsnoList);
            ret.Add(retMBSn);
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

