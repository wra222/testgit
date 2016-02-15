<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:SeaReturn
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceSeaReturn" %>


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
public class WebServiceSeaReturn : System.Web.Services.WebService
{
	ISeaReturn iSeaReturn = ServiceAgent.getInstance().GetObjectByName<ISeaReturn>(WebConstant.SeaReturn);

	public struct S_Result_InputCT
	{
		public string Success;
		public IList<PrintItem> PrintItem;
		public string ProId;
	}
	
	[WebMethod]
    public S_Result_InputCT InputCT(string ct, string returnType, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems)
    {
        try
        {
            ArrayList tmpList = iSeaReturn.InputCT(ct, returnType, pdLine, editor, stationId, customerId, printItems);
			S_Result_InputCT result = new S_Result_InputCT
			{
				Success = WebConstant.SUCCESSRET,
                PrintItem = tmpList[0] as IList<PrintItem>,
				ProId = tmpList[1] as string
			};
            return result;
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
            iSeaReturn.Cancel(productId);
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
