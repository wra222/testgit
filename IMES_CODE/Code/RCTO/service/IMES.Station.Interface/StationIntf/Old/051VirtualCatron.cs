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
    /// 虚拟Carton站接口
    /// </summary>
    public interface IVirtualCarton
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取Model,成功后调用ScanSN
        /// 将custSN放到Session.CustSN中(string)
        /// 将返回的ProductModel中的Model放到Session.FirstProductModel(string)
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>返回ProductModel</returns>
        ProductModel StartWorkFlow(string firstCustSN, string line, string editor, string station, string customer);

        /// <summary>
        /// 每次刷SN都调用该方法，进行SFC,判断Model是否和第一个相同
        /// 将custSN放到Session.CustSN中(string)
        /// 返回ProductModel
        /// 
        /// </summary>
        /// <param name="custSN"></param>
        /// <returns>返回ProductModel</returns>
        ProductModel ScanSN(string firstCustSN, string custSN);

        /// <summary>
        /// 创建Virtual Carton号码，返回打印重量标签的PrintItem，结束工作流。
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param> 
        /// <param name="virtualCartonNo"></param>
        /// <returns></returns>
        IList<PrintItem> Save(string firstCustSN, IList<PrintItem> printItems, out string virtualCartonNo);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>
        void Cancel(string uutSn);

        #endregion

    }
}
