// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-09   Lucy Liu                     create
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
    /// FGShippingLabelTRO 接口定义
    /// </summary>
    public interface IFGShippingLabelTRO
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 启动工作流，根据输入productID获取Model,成功后调用CheckSN
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>返回ProductModel</returns>
        ProductModel InputUUT(string productID, string line, string editor, string station, string customer);


        /// <summary>
        /// 检查CustSn,成功后调用save
        /// 将Session.IsComplete设为false
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="sn">sn</param>
        void CheckSN(string productID, string sn);


        /// <summary>
        /// 记录过站Log，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将Session.IsComplete设为True
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="printItems">打印列表</param> 
        /// <returns>打印列表</returns>
        IList<PrintItem> Save(string productID, IList<PrintItem> printItems, out string custSn);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uut sn</param>
        void Cancel(string uutSn);

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">打印列表</param> 
        /// <returns>打印列表</returns>
        IList<PrintItem> ReprintLabel(string productID, string line, string editor, string station, string customer, IList<PrintItem> printItems, out string custSn);

        /// <summary>
        /// 解除DN的绑定关系
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="line">product line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns></returns>
        void Unpack(string productID, string line, string editor, string station, string customer);


        #endregion




    }
}
