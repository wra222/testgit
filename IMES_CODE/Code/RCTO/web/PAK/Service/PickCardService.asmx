<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Pick Card Service
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-03-21  Tong.Zhi-Yong         Create                 
 * Known issues:
 */
 --%>
<%@ WebService Language="C#" Class="PickCardService" %>


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
public class PickCardService : System.Web.Services.WebService
{
    private IPickCard iPickCard;
    private string PickCardBllName = WebConstant.PickCardImplObject;

    [WebMethod]
    public IList Init(IList param)
    {
        try
        {
            iPickCard = ServiceAgent.getInstance().GetObjectByName<IPickCard>(PickCardBllName);

            return iPickCard.Init(param);
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
    public IList inputTruckID(string pdLine, string truckID, string dateStr, string editor, string station, string customer, IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        
        try
        {
            iPickCard = ServiceAgent.getInstance().GetObjectByName<IPickCard>(PickCardBllName);

            IList lst = iPickCard.InputTruckID(string.Empty, truckID, dateStr, editor, station, customer, items);

            ret = lst;
            return lst;
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
    public void cancel(string truckID)
    {
        try
        {
            iPickCard = ServiceAgent.getInstance().GetObjectByName<IPickCard>(PickCardBllName);

            iPickCard.Cancel(truckID);
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
