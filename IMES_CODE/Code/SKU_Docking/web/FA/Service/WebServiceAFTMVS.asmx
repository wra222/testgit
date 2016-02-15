<%@ WebService Language="C#" Class="WebServiceAFTMVS" %>


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
public class WebServiceAFTMVS : System.Web.Services.WebService
{
    //IBoardInput iBoardInput = ServiceAgent.getInstance().GetObjectByName<IBoardInput>(WebConstant.BoardInputObject);
    IAFTMVS iAFTMVS = ServiceAgent.getInstance().GetObjectByName<IAFTMVS>(WebConstant.AFTMVSObject);
    
    [WebMethod]
    public ArrayList ChickProductScrews(string prodId)
    {
        ArrayList retLst = new ArrayList();
        bool ret = false;
        try
        {
            ret = iAFTMVS.ChickProductScrews(prodId);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(ret);
        }
        catch (FisException ex)
        {
            retLst.Add(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retLst; 
    }
	
	[WebMethod]
    public MatchedPartOrCheckItem jianLiao(string sessionKey, string checkValue)
    {
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        
        try
        {
            ret = iAFTMVS.TryPartMatchCheck(sessionKey, checkValue); //.JianLiao(prodId, custSN);
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
	
	public struct S_InputProductIDorCustSNResult
    {
        public string Success;
		public string Id;
		public string Model;
		public IList<BomItemInfo> Bom;
		public string BomPartNoItems;
    }
	
	[WebMethod]
	public S_InputProductIDorCustSNResult InputProductIDorCustSN(string input, string line, string editor, string station, string customer)
	{
		string id = "";
        string model = "";
		try
        {
            IList<BomItemInfo> tmpList = iAFTMVS.InputProductIDorCustSN(input, line, editor, station, customer, out id, out model);
            S_InputProductIDorCustSNResult ret = new S_InputProductIDorCustSNResult
			{
				Success = WebConstant.SUCCESSRET,
				Id = id,
				Model = model,
				Bom = tmpList,
			};
            
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


