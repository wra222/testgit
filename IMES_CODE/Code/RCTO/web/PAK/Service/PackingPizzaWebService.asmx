
<%@ WebService Language="C#" Class="PackingPizzaWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Collections.Generic;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using com.inventec.iMESWEB;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PackingPizzaWebService : System.Web.Services.WebService
{

    IPackingPizza currentPackingPizzaService = ServiceAgent.getInstance().GetObjectByName<IPackingPizza>(WebConstant.PackPizzaObject);

    /// <summary>
    /// 用model启动工作流
    /// </summary>
    /// <param name="model"></param>
    /// <param name="line"></param>
    /// <param name="editor"></param>
    /// <param name="station"></param>
    /// <param name="customer"></param>
    /// <returns></returns>
    [WebMethod]
    public IList<BomItemInfo> InputUUT(string model, string line, string editor, string station, string customer)
    {
        /*
        IList ret = new ArrayList();
        IList modelinfo = new ArrayList();
        IList partinfo = new ArrayList();
        string collectionData = string.Empty;
        var qtyCount = 0;
        */
        try
        {
            currentPackingPizzaService.InputUUT(model, line, editor, station, customer);
            return currentPackingPizzaService.GetNeedCheckPartAndItem(model);
            /*
            for (var i = 0; i < bominfo.Count; i++)
            {
                IList tmpinfo = new ArrayList();
                tmpinfo.Add(bominfo[i].parts[0].iecPartNo);
                tmpinfo.Add(bominfo[i].parts[0].partTypeId);
                tmpinfo.Add(bominfo[i].parts[0].description);
                tmpinfo.Add(bominfo[i].qty);
                tmpinfo.Add(bominfo[i].scannedQty);
                if (bominfo[i].collectionData != null)
                {
                    for (var j = 0; j < bominfo[i].collectionData.Count; j++)
                    {
                        collectionData += bominfo[i].collectionData[j].sn + ",";
                    }
                    collectionData = collectionData.Trim(new char[] { ',' });
                }
                if (collectionData.Length > 20)
                {
                    collectionData = collectionData.Substring(0, 20) + "...";
                }
                tmpinfo.Add(collectionData);
                tmpinfo.Add(false);
                partinfo.Add(tmpinfo);
                qtyCount += bominfo[i].qty;
            }
            modelinfo.Add(qtyCount.ToString());
            modelinfo.Add(bominfo.Count);
            ret.Add(modelinfo);
            ret.Add(partinfo);

            return ret;
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
    }

    /// <summary>
    /// 输入料或者要检查的CheckItem进行检查
    /// 如果没有抛出Match异常，从Session.SessionKeys.MatchedParts中把当前Match的料取出
    /// 如果没有match到Part，从Session.SessionKeys.MatchedCheckItem取出match的CheckItem
    /// 返回给前台
    /// 检料全部完成后调用Save
    /// </summary>
    /// <param name="model"></param>
    /// <param name="checkValue"></param>
    /// <returns></returns>
    [WebMethod]
    public MatchedPartOrCheckItem CheckPartAndItem(string model, string checkValue)
    {
        try
        {
           /*
           currentPackingPizzaService.CheckPartAndItem(model, checkValue);
           return checkValue;
           */
            return currentPackingPizzaService.TryPartMatchCheck(model, checkValue);
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

    /// <summary>
    /// 扫描完本站应该扫描的所有Parts后调用本方法存储扫入的所有Parts信息，结束工作流。
    /// </summary>
    /// <param name="model"></param>
    /// <param name="printItems"></param>
    /// <returns></returns>
    [WebMethod]
    public IList Save(string model, IList<PrintItem> printItems)
    {
        IList retLst = new ArrayList();
        try
        {
            retLst = currentPackingPizzaService.Save(model, printItems);
            return retLst;
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

    /// <summary>
    /// 取消工作流
    /// </summary>
    /// <param name="sessionKey"></param>
    [WebMethod]
    public void Cancel(string sessionKey)
    {
        currentPackingPizzaService.Cancel(sessionKey);
    }
    
    /// <summary>
    /// Reprint Label
    /// </summary>
    /// <param name="pizzaID"></param>
    /// <param name="editor"></param>
    /// <param name="customer"></param>
    /// <param name="station"></param>
    /// <param name="reason"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    [WebMethod]
    public IList reprintPizzaLabel(string pizzaID, string editor, string customer, string station, string reason, IList<PrintItem> items)
    {
        IList ret = new ArrayList();
        IList<PrintItem> backPrintItems = null;
        //string out_proID = string.Empty;
        //string out_model = string.Empty;

        try
        {
            backPrintItems = currentPackingPizzaService.ReprintLabel(pizzaID, string.Empty, editor, station, customer, reason, items);
            ret.Add(pizzaID);
            ret.Add(backPrintItems);

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

    /// <summary>
    /// Unpack Pizza
    /// </summary>
    /// <param name="kitID"></param>
    /// <param name="editor"></param>
    /// <param name="station"></param>
    /// <param name="customer"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    [WebMethod]
    public string UnpackPizza(string kitID, string editor, string station, string customer, string reason)
    {
        string line = string.Empty;
        try
        {
            currentPackingPizzaService.UnpackPizza(kitID, line, editor, station, customer, reason);
            return WebConstant.SUCCESSRET;
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