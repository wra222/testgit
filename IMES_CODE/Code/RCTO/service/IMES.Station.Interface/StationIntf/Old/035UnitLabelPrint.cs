// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于UnitLabelPrint站
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-27   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// UnitLabelPrint Interface，打印UnitLabel
    /// </summary>
    public interface IUnitLabelPrint
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 刷productID，启动工作流，检查输入的productID，卡站，获取Model,ProductID,
        /// 调用成功后调用GetNeedCheckPartAndItem方法获取需要检料的列表
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        ProductModel InputProductID(string productID, string line, string editor, string station, string customer);


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
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Save(string productID, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        void Cancel(string productID);
        #endregion

        #region "methods do not interact with the running workflow"


        /// <summary>
        /// 获取需要刷料的列表,从SessionBom中获取要刷的料,从CheckItem中获取要刷的CheckItem
        /// 本站为SN和COA，用Product获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<BomItemInfo> GetNeedCheckPartAndItem(string productID);


        /// <summary>
        /// 重印标签,调用Product的CanReprintUnitLabel方法判断是否能重印
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> ReprintLabel(string productID, string line, string editor, string station, string customer, string reason, IList<PrintItem> printItems, out string backProID, out string model);
       
        #endregion



    }
}
