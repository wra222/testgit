// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-09   Yuan XiaoWei                 create
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
    /// 虚拟Pallet站接口定义
    /// </summary>
    public interface IVirtualPallet
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取Model,成功后调用ScanSN
        /// 将custSN放到Session.CustSN中(string)
        /// 从Session中获取Product对象，用来构造ProductModel，和CustPN返回
        /// CustPN是Model对应的PN，用于打印
        /// 将返回的ProductModel中的Model放到Session.FirstProductModel
        /// </summary>
        /// <param name="firstSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>返回ProductModel</returns>
        ProductModel StartWorkFlow(string firstSN, string line, string editor, string station, string customer,out string CustPN);

        /// <summary>
        /// 每次刷SN都调用该方法，进行SFC,判断Model是否和第一个相同
        /// 将custSN放到Session.CustSN中(string)
        /// 返回ProductModel
        /// 
        /// </summary>
        /// <param name="firstSN"></param>
        /// <param name="uutSN"></param>
        /// <returns>返回ProductModel</returns>
        ProductModel ScanSN(string firstSN, string custSN);

        /// <summary>
        /// 创建Virtual Pallet号码，返回打印重量标签的PrintItem，结束工作流。
        /// </summary>
        /// <param name="firstSN"></param>
        /// <param name="printItems"></param> 
        /// <param name="virtualPalletNo"></param> 
        /// <returns></returns>
        IList<PrintItem> Save(string firstSN, IList<PrintItem> printItems, out string virtualPalletNo);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>
        void Cancel(string uutSn);

        #endregion

    }
}
