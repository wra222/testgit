<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombineCartonDNfor146MB
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceCombineCartonDNfor146MB" %>


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
public class WebServiceCombineCartonDNfor146MB : System.Web.Services.WebService
{
	ICombineCartonDNfor146MB iCombineCartonDNfor146MB = ServiceAgent.getInstance().GetObjectByName<ICombineCartonDNfor146MB>(WebConstant.CombineCartonDNfor146MB);

    public struct S_Result_InputMBSN
	{
		public string Success;
		public string MBSN;
		public string Model;
        public IList<string> DeliveryNoList;
		public IList<string> ShipDatesOfDn;
		public IList<string> ModelsOfDn;
		public IList<string> QtysOfDn;
		public IList<string> MBSNs;
        public IList<string> DeliveryShipway;
	}
	
	[WebMethod]
    public S_Result_InputMBSN InputMBSN(string mbsn, bool isGetDn, string shipMode, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombineCartonDNfor146MB.InputMBSN(mbsn, isGetDn, shipMode, pdLine, editor, station, customer);
            S_Result_InputMBSN result = new S_Result_InputMBSN
			{
				Success = WebConstant.SUCCESSRET,
				MBSN = tmpList[0] as string,
                Model = tmpList[1] as string,
                DeliveryNoList = tmpList[2] as IList<string>,
				ShipDatesOfDn = tmpList[3] as IList<string>,
				ModelsOfDn = tmpList[4] as IList<string>,
				QtysOfDn = tmpList[5] as IList<string>,
				MBSNs = tmpList[6] as IList<string>,
                DeliveryShipway = tmpList[7] as IList<string>
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
	
	public struct S_Result_GetDnQty
	{
		public string Success;
		public string CnQty;
		public string DnQty;
		public string DnRemainQty;
		public string Win8sps;
	}
	
	[WebMethod]
    public S_Result_GetDnQty GetDnQty(string dn, string model, string shipMode, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombineCartonDNfor146MB.GetDnQty(dn, model, shipMode, pdLine, editor, station, customer);
            S_Result_GetDnQty result = new S_Result_GetDnQty
			{
				Success = WebConstant.SUCCESSRET,
				CnQty = tmpList[0] as string,
                DnQty = tmpList[1] as string,
				DnRemainQty = tmpList[2] as string,
				Win8sps = tmpList[3] as string
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
		public string CartonSn;
		public string PalletNo;
	}
	
	[WebMethod]
    public S_Result_Save Save(IList<string> mbsns, string dnsn, string model146, IList<PrintItem> printItems, string shipMode, string pdLine, string editor, string station, string customer)
    {
        try
        {
            ArrayList tmpList = iCombineCartonDNfor146MB.Save(mbsns, dnsn, model146, printItems, shipMode, pdLine, editor, station, customer);
			S_Result_Save result = new S_Result_Save
			{
				Success = WebConstant.SUCCESSRET,
                PrintItem = tmpList[0] as IList<PrintItem>,
                CartonSn = tmpList[1] as string,
				PalletNo = tmpList[2] as string
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
		public string CartonSn;
	}
	
	[WebMethod]
    public S_Result_RePrint RePrint(string mbsn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        try
        {
            ArrayList tmpList = iCombineCartonDNfor146MB.Reprint(mbsn, reason, line, editor, station, customer, printItems);
            S_Result_RePrint result = new S_Result_RePrint
			{
				Success = WebConstant.SUCCESSRET,
                PrintItem = tmpList[0] as IList<PrintItem>,
				CartonSn = tmpList[1] as string
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
    public string CheckRMAOA3(string mbsn,string selectmodel)
    {

        string line = string.Empty;
        try
        {
            iCombineCartonDNfor146MB.CheckFRUMBOA3(mbsn, selectmodel);
            return WebConstant.SUCCESSRET;
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
            iCombineCartonDNfor146MB.cancel(productId);
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
