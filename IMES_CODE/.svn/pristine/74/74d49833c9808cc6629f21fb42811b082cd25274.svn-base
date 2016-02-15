// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 053CombinePOInCarton
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
    /// CombinePOInCarton
    /// </summary>
    public interface ICombinePOInCartonForFRU
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流053ScanSN.xoml，根据输入CustSN获取Model,
        /// 将SN放到Session.CustSN里
        /// 成功后将ProductModel.Model放到Session.FirstProductModel中
        /// 调用getDNList
        /// </summary>
        /// <param name="firstSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>返回ProductModel</returns>
        ProductModel ScanFirstSN(string firstSN, string line, string editor, string station, string customer);
      
        /// <summary>
        /// 除第一个SN外，每次刷SN都调用该方法，进行SFC,判断Model是否和第一个相同,刷SN返回ProductID
        /// 将uutSN放到Session.CustSN里
        /// </summary>
        /// <param name="firstSN"></param>
        /// <param name="uutSN"></param>
        /// <returns>返回ProductModel</returns>
        ProductModel ScanSN(string productID, string uutSN, string deliveryNo);

        /// <summary>
        /// 刷满了当前Carton，扫入并检查通过的Product数量与设定的PCS相等。调用该方法
        /// 创建Carton号码，更新所有机器状态，记录所有机器Log，返回打印重量标签的PrintItem，结束工作流。
        /// 将deliveryNo放到Session.DeliveryNo里
        /// 将NewScanedProductIDList放到Session.NewScanedProductIDList里
        /// 以deliveryNo为SessionKey创建工作流053CombinePOInCarton.xoml
        /// </summary>
        /// <param name="NewScanedProductModelList"></param>
        /// <param name="NewScanedModelList"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<PrintItem> Save(IList<string> NewScanedProductIDList, IList<string> NewScanedModelList, IList<string> custSNList, string line, string editor, string station, string customer, IList<PrintItem> printItems, out string cartonSN);



        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="firstSN"></param>
        void Cancel(string productID);


       

        /// <summary>
        /// 输入的可以是CartonNo，也可以是ProductID,也可以是CustSN
        /// 使用工作流053CombinePOInCartonRePrint.xoml
        /// </summary>
        /// <param name="ProductIDOrSNOrCartonNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param> 
        /// <param name="cartonNo"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintCartonLabel(string ProductIDOrSNOrCartonNo, string line, string editor, string station, string customer, string reason, IList<PrintItem> printItems, out string cartonNo);


        #endregion

      

    }
}
