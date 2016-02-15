<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:ESOPandAoiKbTest
* 
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceESOPandAoiKbTest" %>


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
public class WebServiceESOPandAoiKbTest : System.Web.Services.WebService
{
	IESOPandAoiKbTest iESOPandAoiKbTest = ServiceAgent.getInstance().GetObjectByName<IESOPandAoiKbTest>(WebConstant.IESOPandAoiKbTest);
	
	[WebMethod]
    public MatchedPartOrCheckItem jianLiao(string sessionKey, string checkValue)
    {
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        
        try
        {
            ret = iESOPandAoiKbTest.TryPartMatchCheck(sessionKey, checkValue); //.JianLiao(prodId, custSN);
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
	
	public struct S_ESOPandAoiKbTestResult
    {
        public string Success;
		public ProductInfo PrdInfo;
        public string IsNeedAOI;
        public string AoiAddr;
        public string KbPn;
        public string LabelPn;
		public IList<BomItemInfo> Bom;
		public string BomPartNoItems;
    }
	
    [WebMethod]
    public S_ESOPandAoiKbTestResult GetProductInfo(string prodId, string pdLine, string editor, string station, string customer, bool bQuery)
    {
        try
        {
            ArrayList tmpList = iESOPandAoiKbTest.GetProductInfo(prodId, pdLine, editor, station, customer, bQuery);
            S_ESOPandAoiKbTestResult ret = new S_ESOPandAoiKbTestResult
			{
				Success = WebConstant.SUCCESSRET,
				PrdInfo = (ProductInfo) tmpList[0],
				IsNeedAOI = (string)tmpList[1],
				AoiAddr = (string)tmpList[2],
				KbPn = (string)tmpList[3],
				LabelPn = (string)tmpList[4],
				Bom = (IList<BomItemInfo>) tmpList[5],
			};
			List<string> lstItems = new List<string>();
			foreach (BomItemInfo bi in ret.Bom)
			{
				lstItems.Add(bi.PartNoItem);
			}
            ret.BomPartNoItems = string.Join("●", lstItems.ToArray());
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

/*
    [WebMethod]
    public void save(string prodId)
    {
        try
        {
            iESOPandAoiKbTest.Save(prodId);
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
            iESOPandAoiKbTest.Cancel(productId);
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
*/
                      
}
