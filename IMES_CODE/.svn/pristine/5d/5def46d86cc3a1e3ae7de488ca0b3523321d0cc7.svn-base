<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombinePalletWithoutCartonForFRU
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceCombinePalletWithoutCartonForFRU" %>


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
public class WebServiceCombinePalletWithoutCartonForFRU : System.Web.Services.WebService
{
	ICombinePalletWithoutCartonForFRU iCombinePalletWithoutCartonForFRU = ServiceAgent.getInstance().GetObjectByName<ICombinePalletWithoutCartonForFRU>(WebConstant.CombinePalletWithoutCartonForFRU);

    public struct S_Result_GetDnList
	{
		public string Success;
        public IList<string> DeliveryNoList;
		public IList<string> DeliveryModelList;
		public IList<string> DeliveryShipDateList;
		public IList<string> DeliveryQtyList;
	}
	
	[WebMethod]
    public S_Result_GetDnList GetDnList(string dn, string shipDate, string model, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombinePalletWithoutCartonForFRU.GetDnList(dn, shipDate, model, pdLine, editor, station, customer);
            S_Result_GetDnList result = new S_Result_GetDnList
			{
				Success = WebConstant.SUCCESSRET,
                DeliveryNoList = tmpList[0] as IList<string>,
				DeliveryModelList = tmpList[1] as IList<string>,
				DeliveryShipDateList = tmpList[2] as IList<string>,
				DeliveryQtyList = tmpList[3] as IList<string>
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
	
	public struct S_Result_GetPalletList
	{
		public string Success;
		public string Model;
		public string ShipDate;
		public string DnQty;
		public string DnStatus;
		public string DnEditor;
        public IList<string> PalletNoList;
	}
	
	[WebMethod]
    public S_Result_GetPalletList GetPalletList(string dn, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombinePalletWithoutCartonForFRU.GetPalletList(dn, pdLine, editor, station, customer);
            S_Result_GetPalletList result = new S_Result_GetPalletList
			{
				Success = WebConstant.SUCCESSRET,
                Model = tmpList[0] as string,
				ShipDate = tmpList[1] as string,
				DnQty = tmpList[2] as string,
                PalletNoList = tmpList[3] as IList<string>,
				DnStatus = tmpList[4] as string,
				DnEditor = tmpList[5] as string
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
	
	public struct S_Result_GetCntCartonOfPallet
	{
		public string Success;
		public string TotalCartonQty;
	}
	
	[WebMethod]
    public S_Result_GetCntCartonOfPallet GetCntCartonOfPallet(string palletNo, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombinePalletWithoutCartonForFRU.GetCntCartonOfPallet(palletNo, pdLine, editor, station, customer);
            S_Result_GetCntCartonOfPallet result = new S_Result_GetCntCartonOfPallet
			{
				Success = WebConstant.SUCCESSRET,
				TotalCartonQty = tmpList[0] as string
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
	
	public struct S_Result_Save
	{
		public string Success;
		public IList<PrintItem> PrintItem;
	}
	
	[WebMethod]
    public S_Result_Save Save(string dnsn, string palletNo, IList<PrintItem> printItems, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombinePalletWithoutCartonForFRU.Save(dnsn, palletNo, printItems, pdLine, editor, station, customer);
			S_Result_Save result = new S_Result_Save
			{
				Success = WebConstant.SUCCESSRET,
                PrintItem = tmpList[0] as IList<PrintItem>
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
	}
	
	[WebMethod]
    public S_Result_RePrint RePrint(string dnsn, string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        try
        {
            ArrayList tmpList = iCombinePalletWithoutCartonForFRU.Reprint(dnsn, palletNo, reason, line, editor, station, customer, printItems);
            S_Result_RePrint result = new S_Result_RePrint
			{
				Success = WebConstant.SUCCESSRET,
                PrintItem = tmpList[0] as IList<PrintItem>
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
            iCombinePalletWithoutCartonForFRU.cancel(productId);
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
