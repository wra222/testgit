<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for ConbimeOfflinePizza Page
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceConbimeOfflinePizza" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceConbimeOfflinePizza : System.Web.Services.WebService
{
    IConbimeOfflinePizza iConbimeOfflinePizza = ServiceAgent.getInstance().GetObjectByName<IConbimeOfflinePizza>(WebConstant.ConbimeOfflinePizza);

    [WebMethod]
    public ArrayList getProduct(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList result = new ArrayList();
		ArrayList ret = iConbimeOfflinePizza.getProduct(custSN, line, editor, station, customer, printItems);
		result.Add(WebConstant.SUCCESSRET);
		result.Add(ret[0]); // model
		result.Add(ret[1]); // bom
		result.Add(ret[2]); // printItems
        return result;
    }

    private ArrayList GetPrintItem(ref ArrayList ret)
    {
        ArrayList result = new ArrayList();
        IList<PrintItem> retPrintItems = new List<PrintItem>();
        if (ret != null)
        {
            IList<PrintItem> backPrintItems = ret[0] as IList<PrintItem>;
            for (int i = 0; i < backPrintItems.Count; i++)
            {
                if (backPrintItems[i].LabelType == "Offline_COOLabel")
                {
                    retPrintItems.Add(backPrintItems[i]);
                    break;
                }
            }
            if (retPrintItems.Count > 0)
            {
                result.Add(WebConstant.SUCCESSRET);
                result.Add(retPrintItems);
				
				// 2 paqc
				if (ret.Count >= 2)
				{
					result.Add(ret[1]);
				}
				else
					result.Add("");
				
                return result;
            }
        }

        throw new Exception("No Print Item!");
    }
	
	[WebMethod]
    public MatchedPartOrCheckItem jianLiao(string sessionKey, string checkValue)
    {
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        
        try
        {
            ret = iConbimeOfflinePizza.TryPartMatchCheck(sessionKey, checkValue); //.JianLiao(prodId, custSN);
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
    public ArrayList Save(string custSN, string pizzaId, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
		ArrayList arr = iConbimeOfflinePizza.Save(custSN, pizzaId, line, editor, station, customer, printItems);
		ret.Add(WebConstant.SUCCESSRET);
		ret.Add(arr[0]); // paqc
        return ret;
    }
	
	[WebMethod]
    public ArrayList RePrint(string custSN, string reason, string line, string editor, string station,
                                            string customer, IList<PrintItem> printItemList)
    {
		ArrayList ret = new ArrayList();
        try
        {
            ArrayList arr = iConbimeOfflinePizza.Reprint(custSN, reason, line, editor, station, customer, printItemList);
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
    public void cancel(string productId)
    {
        try
        {
            iConbimeOfflinePizza.Cancel(productId);
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
