<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServiceCombinePizza" %>


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
public class WebServiceCombinePizza : System.Web.Services.WebService
{
	ICombinePizza iCombinePizza = ServiceAgent.getInstance().GetObjectByName<ICombinePizza>(WebConstant.CombinePizzaObject);
    
    [WebMethod]
    public ArrayList inputSN(string custSN, string pdLine, string curStation, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        
        try
        {
            //ret = iCombinePizza.InputSN_OnPizzaLabel(pdLine, custSN, editor, station, customer);
            ArrayList tmpList = iCombinePizza.InputSN(custSN, pdLine, curStation, editor, station, customer);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//DataModel.ProductInfo
            ret.Add(tmpList[1]);//IList<BomItemInfo>            
            ret.Add(tmpList[2]);//isOceanShipping -- bool
            ret.Add(tmpList[3]);//isOceanShipping --No ModelWeight Err Msg
            
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
    public IList inputSN_OnCooLabel(string prodId, string custSN)
    {
        IList ret = new ArrayList();
        
        try
        {
            //ret = iCombinePizza.InputSN_OnCooLabel(prodId, custSN);
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
    public MatchedPartOrCheckItem jianLiao(string sessionKey, string checkValue)
    {
        MatchedPartOrCheckItem ret = new MatchedPartOrCheckItem();
        
        try
        {
            ret = iCombinePizza.TryPartMatchCheck(sessionKey, checkValue); //.JianLiao(prodId, custSN);
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
            iCombinePizza.Save(prodId);
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
            iCombinePizza.Cancel(productId);
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
    public ArrayList CheckQCStatus(string productId)
    {
        try
        {

            ArrayList ret = new ArrayList();
            string qcstatus;
            qcstatus = iCombinePizza.CheckQCStatus(productId);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(qcstatus);
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
    public ArrayList CheckWarrantyCard(string productId)
    {
        try
        {

            ArrayList ret = new ArrayList();
            string strreturn;
            strreturn = iCombinePizza.CheckWarrantyCard(productId);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(strreturn);
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
