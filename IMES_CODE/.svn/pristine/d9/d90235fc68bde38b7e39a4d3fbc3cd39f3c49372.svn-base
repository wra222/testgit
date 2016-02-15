<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/11/11 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/11/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-01   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
 
<%@ WebService Language="C#" Class="GenerateCustomerSNService" %>



using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.DataModel;


//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class GenerateCustomerSNService  : System.Web.Services.WebService 
{

    IGenerateCustomerSN iGenerateCustomerSN = ServiceAgent.getInstance().GetObjectByName<IGenerateCustomerSN>(WebConstant.GenerateCustomerSNObject);
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
        ArrayList tmpLst = new ArrayList();      
        
        try
        {
            tmpLst=iGenerateCustomerSN.InputProdId(pdLine, prodId, editor, stationId, customer);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpLst[0]);//ProductModel
            retLst.Add(tmpLst[1]);//LabelType
            retLst.Add(tmpLst[2]);//WhetherNoCheckDDRCT
            retLst.Add(tmpLst[3]);
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
    public ArrayList InputDDRCT(string prodId, string ddrct)
	{
        ArrayList retLst = new ArrayList();
        ArrayList tmpLst = new ArrayList();      
        
		try
        {
            tmpLst = iGenerateCustomerSN.InputDDRCT(prodId, ddrct);
            retLst.Add(tmpLst[0]);
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
    public ArrayList InputMBSno(string prodId, string mbsno)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpLst = new ArrayList();

        try
        {
            tmpLst = iGenerateCustomerSN.InputMBSno(prodId, mbsno);
            retLst.Add(tmpLst[0]);
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
    public ArrayList save(string prodId)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpLst = new ArrayList();

        try
        {
            tmpLst = iGenerateCustomerSN.save(prodId);
            retLst.Add(tmpLst[0]);
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
           
           printLst = iGenerateCustomerSN.Print(prodId, printItems);
           
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

           returnLst = iGenerateCustomerSN.Reprint(prodId, editor, stationid, customer, printItems, reason, out customerSN);
           ret.Add(WebConstant.SUCCESSRET);
           ret.Add(returnLst[0]);//IList<PrintItem> 
           ret.Add(returnLst[1]);//ProductModel 
           ret.Add(returnLst[2]);//labeltype 
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
            iGenerateCustomerSN.Cancel(prodID);
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
    
    

}

