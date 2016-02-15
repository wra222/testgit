<%--
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
 --%>
<%@ WebService Language="C#" Class="WebServicePizzaKitting" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePizzaKitting : System.Web.Services.WebService
{
    IPizzaKitting pizza = ServiceAgent.getInstance().GetObjectByName<IPizzaKitting>(WebConstant.IPizzaKitting);
    private IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    private string type = "PRD";
    
    [WebMethod]
    public IList inputSN(string pdLine, string curStation, string custSN, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        
            
        try 
        {
            ArrayList tmpList = pizza.InputSN(custSN, pdLine, curStation, editor, station, customer);
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tmpList[0]);//DataModel.ProductInfo
            //Get Part List 中获取的'DIB'开头的Part No，是取消'DIB' 呈现给用户的，
            //而WipBuffer 对于这种Part 还是按照有'DIB' 的Part No 来进行维护的
            IList<BomItemInfo> bomItemList = (IList<BomItemInfo>)tmpList[1];
            for (int i = bomItemList.Count - 1; i >= 0; i--)
            {
                BomItemInfo bomNode = bomItemList[i];
                bomNode.PartNoItem = bomItemList[i].PartNoItem.Replace(",DIB",",");
                bomItemList[i] = bomNode;
                if (bomNode.PartNoItem.StartsWith("DIB"))
                {
                    bomNode.PartNoItem = bomItemList[i].PartNoItem.Substring(3);
                    bomItemList[i] = bomNode;
                }
            }
            ret.Add(bomItemList);//IList<BomItemInfo>
            /*IList<BomItemInfo> bomItemList = new List<BomItemInfo>();
            for (int i = 0; i < 8; i++)
            {
                BomItemInfo bomnode = new BomItemInfo();
                bomnode.PartNoItem = "Part12345678901234567890";
                bomnode.tp = "tp123";
                bomnode.description = "desc12345678901234567890";
                bomnode.qty = 1;
                bomnode.scannedQty = 0;
                bomnode.collectionData = new List<pUnit>();
                bomItemList.Add(bomnode);
            }
            ProductInfo info = new ProductInfo();
            info.customSN="cust";
            info.id ="id";
            info.pizzaId="pizzaid";
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(info);
            ret.Add(bomItemList);*/
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
    public IList InputPizzaID(string productID, string pizzaID, string line, string curStation, string model,
                                string editor, string stationId, string customer)
    {
        try
        {
            IList ret = new ArrayList();
            
            pizza.InputPizzaID(productID, pizzaID, line, curStation, model, editor, stationId, customer);      
            ret.Add(WebConstant.SUCCESSRET);
            
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
    public IList checkPart(string productID, string part)
    {
        try
        {
            IList ret = new ArrayList();

            MatchedPartOrCheckItem item = pizza.TryPartMatchCheck(productID, part);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(item);
            
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
    public IList Save(string productID)
    {
        try
        {
            IList ret = new ArrayList();

            pizza.Save(productID);
            ret.Add(WebConstant.SUCCESSRET);
           
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
    public ArrayList Print(string productID, string line,string code, string floor, 
                                string editor, string station, string customer,IList<PrintItem> printItems)
    {
        ArrayList retLst = new ArrayList();

        try
        {
            ArrayList tempLst = pizza.Print(productID, line, code, floor, editor, station, customer, printItems);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tempLst[0]);//printList
            retLst.Add(tempLst[1]);//label Template
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }


        return retLst;

    }

    [WebMethod]
    public void cancel(string productId)
    {
        try
        {
            pizza.Cancel(productId);
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
    public ArrayList ReprintPizzaKitting(string customerSN, string reason, IList<PrintItem> printItems,
                    string line, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList tmpList = new ArrayList();

        try
        {
            tmpList = pizza.ReprintPizzaKitting(customerSN, reason, printItems, line, editor, station, customer);

            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(tmpList[0]);//printList
            retLst.Add(tmpList[1]);//label Template

            /*retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add("productID");
            retLst.Add("customerPN");
            retLst.Add("warranty");
            retLst.Add("configuration");
            retLst.Add("assetCheck");
            */

        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        /*catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }*/
        

        return retLst;


    }
             
}
