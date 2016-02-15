<%--
/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Web Service for Generate Customer SN For Docking Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN For Docking.docx –2012/5/18 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN For Docking.docx –2012/5/18           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-18   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/
 --%>
 
<%@ WebService Language="C#" Class="GenerateCustomerSNForDockingService" %>



using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Docking.Interface.DockingIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.DataModel;


//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class GenerateCustomerSNForDockingService  : System.Web.Services.WebService 
{

    IGenerateCustomerSNForDocking iGenerateCustomerSNForDocking = ServiceAgent.getInstance().GetObjectByName<IGenerateCustomerSNForDocking>(WebConstant.GenerateCustomerSNForDockingObject);
    IProduct iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(WebConstant.CommonObject);
    [WebMethod]
    public string ChangeModelCheck(string ProdID,string ModelID)
    {      
            string Info=string.Empty;
            string isChangeM = string.Empty;
            try
            {
                ProductInfo pidInfo;
                pidInfo = iProduct.GetProductInfo(ProdID);
               
                Info = pidInfo.modelId;
             
                if ((Info=="")||(Info==null))
                {
                    return "null";
                }
                if (Info == ModelID)
                {
                    isChangeM = "N";
                    return (isChangeM); 
                }
                else
                {
                    isChangeM = "Y";
                    return (isChangeM);
                }
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

    public ArrayList inputProdId(string pdLine, string prodId, string stationId, string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        IMES.DataModel.ProductModel  curProduct = new IMES.DataModel.ProductModel();
        
        try
        {
            if (prodId.StartsWith("5CG") && prodId.Length == 10)
            {
                ProductInfo pidInfo;
                pidInfo = iProduct.GetProductInfoByCustomSn(prodId);
                prodId = pidInfo.id;
            }
            iGenerateCustomerSNForDocking.InputProdId(pdLine, prodId, editor, stationId, customer,out curProduct);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(curProduct.ProductID);
            retLst.Add(curProduct.Model);
            retLst.Add(curProduct.CustSN);
                      
        }

        catch (FisException ex)
        {
            //retLst.Add(ex.mErrmsg);
            throw new Exception(ex.mErrmsg);
        }
            
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
    }
       
    
    [WebMethod]

    public ArrayList Print(string prodId, IList<PrintItem> printItems)
   {

       ArrayList ret = new ArrayList();
       IList<PrintItem> printLst = new List<PrintItem>();
       string customerSN = string.Empty;
       try
       {
           
           printLst = iGenerateCustomerSNForDocking.Print(prodId, printItems);
           
           ret.Add(WebConstant.SUCCESSRET);
           ret.Add(printLst);
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
    public ArrayList Reprint(string prodId, string editor, string stationid, string customer, IList<PrintItem> printItems, string reason)
    {
       
        ArrayList ret = new ArrayList();
        ArrayList returnLst = new ArrayList();
        string customerSN = string.Empty;
        try
        {

           returnLst = iGenerateCustomerSNForDocking.Reprint(prodId, editor, stationid, customer, printItems, reason, out customerSN);
           ret.Add(WebConstant.SUCCESSRET);
           ret.Add(returnLst[0]);//IList<PrintItem> 
           ret.Add(returnLst[1]);//ProductModel 
           return ret;
          
          //记录重印log
            
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
    public string Cancel(string prodID, string stationId)
    {
        try
        {
            iGenerateCustomerSNForDocking.Cancel(prodID);
            return (WebConstant.SUCCESSRET);
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
	public string GetModelFamily(string model)
	{
		try
        {
            return iGenerateCustomerSNForDocking.GetModelFamily(model);
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
    public ArrayList inputProdIdAndChangeModel(string pdLine, string prodId, string stationId, string editor, string customer, string newmodel)
    {
        ArrayList retLst = new ArrayList();
        IMES.DataModel.ProductModel  curProduct = new IMES.DataModel.ProductModel();
        
        try
        {
            if (prodId.StartsWith("5CG") && prodId.Length == 10)
            {
                ProductInfo pidInfo;
                pidInfo = iProduct.GetProductInfoByCustomSn(prodId);
                prodId = pidInfo.id;
            }

            iGenerateCustomerSNForDocking.InputProdIdAndChangeModel(pdLine, prodId, editor, stationId, customer,out curProduct, newmodel);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(curProduct.ProductID);
            retLst.Add(curProduct.Model);
            retLst.Add(curProduct.CustSN);
                      
        }

        catch (FisException ex)
        {
            //retLst.Add(ex.mErrmsg);
            throw new Exception(ex.mErrmsg);
        }
            
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
    }

}

