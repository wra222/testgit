// INVENTEC corporation (c)2009 all rights reserved. 
// Description: InitialPVS站使用的BLL接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Initial PVS 接口,收集Pizza料
    /// </summary>
    public interface IInitialPVS
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 此站输入的是SN，需要先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 将获得的Product放到Session.Product
        /// 再用ProductID启动工作流
        /// 卡站，获取Model,SN,ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        ProductModel InputUUT(string custSN, string line, string editor, string station, string customer);

        /// <summary>
        /// 输入料或者要检查的CheckItem进行检查
        /// 如果没有抛出Match异常，从Session.SessionKeys.MatchedParts中把当前Match的料取出
        /// 如果没有match到Part，从Session.SessionKeys.MatchedCheckItem取出match的CheckItem
        /// 返回给前台
        /// 检料全部完成后调用Save
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        IList<MatchedPartOrCheckItem> CheckPartAndItem(string productID, string checkValue);


        /// <summary>
        /// 扫描完本站应该扫描的所有Parts后调用本方法存储扫入的所有Parts信息，结束工作流。
        /// </summary>
        /// <param name="productID"></param>
        void Save(string productID);

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
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<BomItemInfo> GetNeedCheckPartAndItem(string productID);
        #endregion

    }
}
