<%@ WebService Language="C#" Class="WebServicePCAShippingLabelPrint" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Linq;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
//using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections;
using IMES.DataModel;
//using IMES.FisObject.PCA.MB;
//using IMES.FisObject.Common.Model;
//using IMES.FisObject.Common.Warranty;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePCAShippingLabelPrint : System.Web.Services.WebService
{
    private IPCAShippingLabelPrint iPCAShippingLabelPrint = ServiceAgent.getInstance().GetObjectByName<IPCAShippingLabelPrint>(WebConstant.PCAShippingLabelPrintObject);
    //private IMB MB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);
    private string type = "PRD";
    
    [WebMethod]
    public IList InputMB(string PdLine, string MBSNo, string checkPCMBRCTOMB, string editor, string stationId, string customer)
    {
        IList ret = new ArrayList();

        try
        {
            ret = iPCAShippingLabelPrint.InputMBSNo(PdLine, MBSNo, checkPCMBRCTOMB, stationId, editor, customer);
            //MBInfo mbInfo = MB.GetMBInfo(MBSNo);

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
    public IList SetDataCodeValue(string model, string customer)
    {
        string ret = "";
        IList retLst = new ArrayList();
        try
        {
            ret = iPCAShippingLabelPrint.SetDataCodeValue(model, customer);

            retLst.Add(model);
            retLst.Add(ret);
            return retLst;
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public IList save(string MBno, string model, string dcode, string region, IList<PrintItem> printItems)
    {
        IList ret = new ArrayList();
        try
        {
            ret = iPCAShippingLabelPrint.save(MBno, model, dcode, region, printItems);
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
    public void cancel(string MBSNo)
    {
        try
        {
            iPCAShippingLabelPrint.Cancel(MBSNo);
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
    public ArrayList Reprint(string mbSno, string strReason, string line, string editor, string station, string customer, List<PrintItem> printItems)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            var retList = iPCAShippingLabelPrint.ReprintLabel(mbSno, strReason, line, editor, station, customer, printItems);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(retList);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            retLst.Add(ex.Message);
        }
        return retLst;
    }

}