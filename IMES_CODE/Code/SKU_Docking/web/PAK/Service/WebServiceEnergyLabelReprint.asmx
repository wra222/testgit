<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:WebMethod for EnergyLabelReprint Page
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceEnergyLabelReprint" %>


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
public class WebServiceEnergyLabelReprint : System.Web.Services.WebService
{
    ISNCheck iSNCheck = ServiceAgent.getInstance().GetObjectByName<ISNCheck>(WebConstant.SNCheckObject);

    [WebMethod]
    public ArrayList Reprint(string custSN, string reason, string line, string editor, string station,
                                            string customer, IList<PrintItem> printItemList)
    {
        ArrayList ret = new ArrayList();
        PrintItem piEnergyLabel = new PrintItem();
        try
        {
            IList<PrintItem> backPrintItems = iSNCheck.ReprintEnergyLabel(custSN, reason, line, editor, station, customer, printItemList);
            if (backPrintItems != null)
            {
                for (int i = 0; i < backPrintItems.Count; i++)
                {
                    if (backPrintItems[i].LabelType == "EnergyLabel")
                    {
                        piEnergyLabel = backPrintItems[i];
                        break;
                    }
                }
                backPrintItems.Clear();
                if (piEnergyLabel.LabelType != null)
                {
                    backPrintItems.Add(piEnergyLabel);
                    ret.Add(WebConstant.SUCCESSRET);
                    ret.Add(backPrintItems);
                    return ret;
                }
            }

            ret.Add("");
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
