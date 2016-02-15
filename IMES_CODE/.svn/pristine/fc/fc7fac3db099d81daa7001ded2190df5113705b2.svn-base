<%@ WebService Language="C#" Class="WebServiceASTCheck" %>


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
public class WebServiceASTCheck : System.Web.Services.WebService
{
    //IBoardInput iBoardInput = ServiceAgent.getInstance().GetObjectByName<IBoardInput>(WebConstant.BoardInputObject);
    ICheckAst iCheckAst = ServiceAgent.getInstance().GetObjectByName<ICheckAst>(WebConstant.CheckAST);
    
   
	
	[WebMethod]
    public MatchedPartOrCheckItem jianLiao(string sessionKey, string checkValue)
    {
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        
        try
        {
            ret = iCheckAst.TryPartMatchCheck(sessionKey, checkValue); //.JianLiao(prodId, custSN);
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
        public string image1src;
        public string image2src;
    }
	
	[WebMethod]
    public S_InputProductIDorCustSNResult InputProductIDorCustSN(string input, string line, string editor, string station, string customer, bool bquery, string image1Parameter, string image2Parameter)
	{
		string id = "";
        string model = "";
		try
        {
            ArrayList tmpList = iCheckAst.InputProductIDorCustSN(input, line, editor, station, customer, bquery, image1Parameter, image2Parameter, out id, out model);
            S_InputProductIDorCustSNResult ret = new S_InputProductIDorCustSNResult
			{
				Success = WebConstant.SUCCESSRET,
				Id = id,
				Model = model,
                Bom = (IList<BomItemInfo>)tmpList[2],
                image1src=(string)tmpList[0],
                image2src = (string)tmpList[1],
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
    
    [WebMethod]
    public void save(string prodId)
    {
        try
        {
            iCheckAst.Save(prodId);
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
            iCheckAst.Cancel(productId);
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


