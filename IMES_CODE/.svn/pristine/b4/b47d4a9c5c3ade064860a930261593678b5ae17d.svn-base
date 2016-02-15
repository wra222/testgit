// INVENTEC corporation (c)2009 all rights reserved. 
// Description: JapaneseLabel Print interface
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
    /// Japanese Label Print
    /// </summary>
    public interface IJapaneseLabelPrint
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 启动工作流，根据输入productID获取ProductModel,成功后调用CheckSN
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>返回Model</returns>
        ProductModel InputUUT(string productID, string line, string editor, string station, string customer);

        /// <summary>
        /// 将custSn放到Session.CustSN中
        /// 将Session.IsComplete设为False
        /// 检查CustSn,成功后调用save
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="sn"></param>
        void CheckSN(string productID, string sn);

        /// <summary>
        /// 记录过站Log，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将Session.IsComplete设为True
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param> 
        /// <returns></returns>
        IList<PrintItem> Save(string productID, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>
        void Cancel(string uutSn);

        #endregion


        #region methods do not interact with the running workflow

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="productIDorSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param> 
        /// <param name="productId"></param> //ITC-1155-0029
        /// <returns></returns>
        IList<PrintItem> ReprintLabel(string productIDorSn, string line, string editor, string station, string customer, string reason, IList<PrintItem> printItems,out string productId);

        #endregion


    }
}
