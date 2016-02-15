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
<%@ WebService Language="C#" Class="WebServiceROMEOBattery" %>

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
public class WebServiceROMEOBattery : System.Web.Services.WebService
{
    IROMEOBattery battery = ServiceAgent.getInstance().GetObjectByName<IROMEOBattery>(WebConstant.IROMEOBattery);
    private IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
    private string type = "PRD";
    
    [WebMethod]
    public IList inputSN(string pdLine, string custSN, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
            
        try 
        {
            ArrayList tmpList = battery.InputSN(custSN, pdLine, editor, station, customer);
            
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
    public IList checkPart(string productID, string part)
    {
        try
        {
            IList ret = new ArrayList();

            MatchedPartOrCheckItem item = battery.TryPartMatchCheck(productID, part);
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

            battery.Save(productID);
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
    public void cancel(string productId)
    {
        try
        {
            battery.Cancel(productId);
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
