// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-06   Luy Liu                      create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// FGShippingLabel 站接口定义
    /// </summary>
    public interface IFGShippingLabel
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 启动工作流，根据输入productID获取Model,成功后调用getExplicitCheckItem
        /// 如果有需要刷料检查的CheckItem，调用checkExplicitCheckItem，否则调用Save
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>返回ProductModel</returns>
        ProductModel InputUUT(string productID, string line, string editor, string station, string customer);


        /// <summary>
        /// 刷料检查的一些属性,全部检查成功后调用save
        /// 将Session.IsComplete设为False
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="checkValue">检查值</param>
        /// <returns>匹配项列表</returns>
        IList<MatchedPartOrCheckItem> CheckExplicitCheckItem(string productID, string checkValue);

        /// <summary>
        /// 记录过站Log，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将Session.IsComplete设为True
        /// </summary>
        /// <param name="productID">production id</param>
        /// <param name="printItems">打印列表</param> 
        /// <param name="cust sn">cust sn</param> 
        /// <returns>打印列表</returns>
        IList<PrintItem> Save(string productID, IList<PrintItem> printItems, out string custSn);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uut sn</param>
        void Cancel(string uutSn);

        #endregion


        #region methods do not interact with the running workflow

        /// 获取所有需要扫描的Parts和CheckItem，在调用inputUUT成功开启工作流之后调用
        /// 本站为CustSN，Model,PizzaID，MMIID属性
        /// 用productID获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        /// <param name="productID">product id</param>
        /// <returns>Parts和CheckItem</returns>
        IList<BomItemInfo> GetNeedCheckPartAndItem(string productID);


        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">站</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">打印列表</param> 
        /// <returns>打印列表</returns>
        IList<PrintItem> ReprintLabel(string productID, string line, string editor, string station, string customer, IList<PrintItem> printItems, out string custSn);


        #endregion


    }
}
