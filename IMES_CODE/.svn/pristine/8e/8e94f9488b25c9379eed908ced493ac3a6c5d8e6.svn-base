<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombineCartonPalletFor146
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceCombineCartonPalletFor146" %>


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
public class WebServiceCombineCartonPalletFor146 : System.Web.Services.WebService
{
	ICombineCartonPalletFor146 iCombineCartonPalletFor146 = ServiceAgent.getInstance().GetObjectByName<ICombineCartonPalletFor146>(WebConstant.CombineCartonPalletFor146);

	public struct S_Result_Save
	{
		public string Success;
		public IList<PrintItem> PrintItem;
		public string PalletNo;
		public string DN;
		public string Category;
		public string FullCartonsInPallet;
	}
	
	[WebMethod]
    public S_Result_Save Save(string cartonSn, IList<PrintItem> printItems, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombineCartonPalletFor146.Save(cartonSn, printItems, pdLine, editor, station, customer);
			S_Result_Save result = new S_Result_Save
			{
				Success = WebConstant.SUCCESSRET,
                PrintItem = tmpList[0] as IList<PrintItem>,
                PalletNo = tmpList[1] as string,
				DN = tmpList[2] as string,
				Category = tmpList[3] as string,
				FullCartonsInPallet = tmpList[4] as string
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
    
	public struct S_Result_RePrint
	{
		public string Success;
        public IList<PrintItem> PrintItem;
		public string PalletNo;
		public string Category;
	}
	
	[WebMethod]
    public S_Result_RePrint RePrint(string cartonSn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        try
        {
            ArrayList tmpList = iCombineCartonPalletFor146.Reprint(cartonSn, reason, line, editor, station, customer, printItems);
            S_Result_RePrint result = new S_Result_RePrint
			{
				Success = WebConstant.SUCCESSRET,
                PrintItem = tmpList[0] as IList<PrintItem>,
				PalletNo = tmpList[1] as string,
				Category = tmpList[2] as string
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
            iCombineCartonPalletFor146.cancel(productId);
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
