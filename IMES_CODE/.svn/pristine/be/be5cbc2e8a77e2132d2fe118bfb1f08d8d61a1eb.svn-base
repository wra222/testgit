<%--
 --%>
<%@ WebService Language="C#" Class="WebServiceCheckCartonCTForRCTO" %>


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
public class WebServiceCheckCartonCTForRCTO : System.Web.Services.WebService
{
    private ICheckCartonCTForRCTO iCheckCartonCT;
    private string CheckCartonCTBllName = WebConstant.CheckCartonCTForRCTOObject;
    private IDefect iDefect;
    private string commBllName = WebConstant.CommonObject;
    
    [WebMethod]
    public IList input(string CartonSN, string line, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ArrayList tmpList = new ArrayList();
        try
        {
            iCheckCartonCT = ServiceAgent.getInstance().GetObjectByName<ICheckCartonCTForRCTO>(CheckCartonCTBllName);
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(commBllName);

            tmpList = iCheckCartonCT.InputSN(CartonSN, line, editor, station, customer);
            ret.Add(CartonSN);
            ret.Add(tmpList.Count);
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
    public void checkmb(string pdLine, string mbsno, string prodId, string editor, string station, string customer)
    {       
        try
        {
            /*iCheckCartonCT = ServiceAgent.getInstance().GetObjectByName<ICheckCartonCTForRCTO>(CheckCartonCTBllName);
            iCheckCartonCT.checkMBSno(mbsno, prodId, pdLine, editor, station, customer);*/
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
    public void save(string CartonSN, string line, string editor, string station)
    {
        try
        {
            iCheckCartonCT = ServiceAgent.getInstance().GetObjectByName<ICheckCartonCTForRCTO>(CheckCartonCTBllName);
            iCheckCartonCT.save(CartonSN, line, editor, station);
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
    public void cancel(string productId)
    {
        try
        {
            /*iCheckCartonCT = ServiceAgent.getInstance().GetObjectByName<ICheckCartonCTForRCTO>(CheckCartonCTBllName);
            iCheckCartonCT.cancel(productId);*/
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
