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
<%@ WebService Language="C#" Class="WebServiceProductReinput" %>


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
public class WebServiceProductReinput : System.Web.Services.WebService
{
    private IProductReinput iProductReinput = ServiceAgent.getInstance().GetObjectByName<IProductReinput>(WebConstant.ProductReinputObject);
    
    [WebMethod]
    public IList checkProdList(IList<string> prodList, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ArrayList tmpList = new ArrayList();
       
        
        try
        {

            tmpList = iProductReinput.CheckProdList(prodList, editor, station, customer);
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);
            ret.Add(tmpList[1]);

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
    public IList checkProdPassList(IList<ProductInfo> passList, IList<ProductInfo> failList,
                                string reStation, string editor, string station, string customer)
    {
        IList ret = new ArrayList();
        ArrayList tmpList = new ArrayList();
        
        try
        {
            tmpList = iProductReinput.CheckProdPassList(passList,failList,reStation, editor, station, customer);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);
            ret.Add(tmpList[1]);

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
    public IList save(IList<ProductInfo> passList, string reStation, bool isPrint,
                      string editor, string station, string customer, string line, IList<PrintItem> printItems)
    {
        IList ret = new ArrayList();
        ArrayList tmpList = new ArrayList();

        try
        {
            tmpList = iProductReinput.Save(passList, reStation, isPrint, editor, station, customer,line,printItems);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);
            
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
