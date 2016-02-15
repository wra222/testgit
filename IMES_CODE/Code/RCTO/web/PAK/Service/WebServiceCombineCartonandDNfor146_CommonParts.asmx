
<%@ WebService Language="C#" Class="WebServiceCombineCartonandDNfor146_CommonParts" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCombineCartonandDNfor146_CommonParts : System.Web.Services.WebService
{

    //ICombineCartonInDNForRCTO CombineCartonInDN = ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDNForRCTO>(WebConstant.CombineCartonInDNObjectForRCTO);
    ICombineCartonandDNfor146_CommonParts iCombineCartonandDNfor146_CommonParts = ServiceAgent.getInstance().GetObjectByName<ICombineCartonandDNfor146_CommonParts>(WebConstant.CombineCartonandDNfor146_CommonPartsObject);
    [WebMethod]
    public ArrayList InputPCSinCarton(string pdLine, string input, string dnno, string model, string editor, string stationId, string customerId, IList<PrintItem> printItems)
    {
        ArrayList retList = new ArrayList();
        ArrayList tmpList = new ArrayList();
        ProductModel curProduct = new ProductModel();

        try
        {
            tmpList = iCombineCartonandDNfor146_CommonParts.InputPCSinCarton(pdLine,input,dnno,model,editor,stationId,customerId,printItems);
            
            retList.Add(tmpList[0]);
            retList.Add(tmpList[1]);
            retList.Add(tmpList[2]);
            retList.Add(tmpList[3]);
            retList.Add(tmpList[4]);
            
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
    public void cancel(string deliverNo)
    {
        try
        {
            iCombineCartonandDNfor146_CommonParts.Cancel(deliverNo);
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
