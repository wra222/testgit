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
 
<%@ WebService Language="C#" Class="WebServiceRCTOCTLabelPrint" %>



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
public class WebServiceRCTOCTLabelPrint  : System.Web.Services.WebService 
{

    IRCTOCTLabelPrint iCTLabelPrint = ServiceAgent.getInstance().GetObjectByName<IRCTOCTLabelPrint>(WebConstant.CTLabelPrintObjectForRCTO);
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

    public ArrayList inputCTNO(string ctno,int qty,IList<PrintItem> printItems,string pdLine,  string editor, string stationId,string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpLst = new ArrayList();      
        
        try
        {
            tmpLst=iCTLabelPrint.inputCTNO(ctno,qty,printItems, pdLine, editor, stationId, customer);
            
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpLst[0]);//ctno
            retLst.Add(tmpLst[1]);//qty
            retLst.Add(tmpLst[2]);//printList
            retLst.Add(tmpLst[3]);//ctnoList

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

    public ArrayList checkCTNO(string ctno)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpLst = new ArrayList();

        try
        {
            tmpLst = iCTLabelPrint.checkCTNO(ctno);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpLst[0]);//partno
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

