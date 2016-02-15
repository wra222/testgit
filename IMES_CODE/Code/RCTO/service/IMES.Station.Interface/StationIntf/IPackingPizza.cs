// INVENTEC corporation (c)2009 all rights reserved. 
// Description: PackingPizza
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-07   zhu lei                      create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Packing Pizza 接口,收集Pizza料
    /// </summary>
    public interface IPackingPizza
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 用model启动工作流
        /// </summary>
        /// <param name="model"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void InputUUT(string model, string line, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);


        /// <summary>
        /// 扫描完本站应该扫描的所有Parts后调用本方法存储扫入的所有Parts信息，结束工作流。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        ArrayList Save(string model, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
        #endregion

        #region "methods do not interact with the running workflow"

        /// <summary>
        /// 获取所有需要扫描的PizzaParts，在调用inputUUT成功开启工作流之后调用
        /// 用productID获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<BomItemInfo> GetNeedCheckPartAndItem(string model);

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="pizzaID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> ReprintLabel(string pizzaID, string line, string editor, string station, string customer, string reason, IList<PrintItem> printItems);

        /// <summary>
        /// Unpack Pizza
        /// </summary>
        /// <param name="kitID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="reason"></param>
        void UnpackPizza(string kitID, string line, string editor, string station, string customer, string reason);
        #endregion

    }
}
