// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Japanese PVS
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-05   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Japanese PVS
    /// </summary>
    public interface IJPNPVS
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 启动工作流，根据输入productID获取ProductModel,成功后调用getExplicitCheckItem
        /// 如果有需要刷料检查的CheckItem，调用checkExplicitCheckItem，否则调用Save
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>返回Model</returns>
        ProductModel InputUUT(string custSN, string line, string editor, string station, string customer);


        /// <summary>
        /// 刷料检查的一些属性,全部检查成功后调用save
        /// 将Session.IsComplete设为False
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="checkValue"></param>
        IList<MatchedPartOrCheckItem> CheckExplicitCheckItem(string productID, string checkValue);

        /// <summary>
        /// 记录过站Log，更新机器状态，结束工作流。
        /// 将Session.IsComplete设为True
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        void Save(string productID);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>
        void Cancel(string uutSn);

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 获取所有需要扫描的Parts和CheckItem，在调用inputUUT成功开启工作流之后调用
        /// 本站原定义要刷的内容为Model的TSBPN,UPCCODE
        /// 用productID获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<BomItemInfo> GetNeedCheckPartAndItem(string productID);
        #endregion


    }
}
