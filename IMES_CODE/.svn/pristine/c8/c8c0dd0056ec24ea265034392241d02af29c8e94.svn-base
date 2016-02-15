// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Interface of PalletDataCollectionTRO station
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-10   Yuan XiaoWei                 create
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
    /// 绑定Pallet和ProductID,打印Pallet标签
    /// </summary>
    public interface IPalletDataCollectionTRO
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取ProductModel,
        /// 将SN放到Session.CustSN里
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="firstSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ProductModel StartWorkFlow(string dn, string palletNo, string firstSN, string line, string editor, string station, string customer);


        /// <summary>
        /// 除首次外，每次刷SN都调用该方法，进行SFC,判断DN是否和但前选定DN相同,刷SN返回ProductModel
        /// 将SN放到Session.CustSN里
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="uutSN"></param>
        /// <returns>返回ProductModel</returns>
        ProductModel ScanSN(string dn, string palletNo, string uutSN);

        /// <summary>
        ///  更新所有机器状态，记录所有机器Log，绑定Pallet，返回打印重量标签的PrintItem，结束工作流。
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> Save(string dn, string palletNo, IList<PrintItem> printItems, out bool isPalletFull);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        void Cancel(string dn, string palletNo);

        #endregion

        #region methods do not interact with the running workflow

        /// <summary>
        /// 获取可以做PalletDataCollectionTRO的DN列表，成功后调用ScanSN
        /// 调用 IDeliveryRepository.getDeliveryNoListFor054()
        /// </summary>
        /// <returns></returns>
        IList<string> getDNList();

        /// <summary>
        /// 根据DN获取Pallet列表
        /// </summary>
        /// <returns></returns>
        IList<string> getPalletList(string dn);

        /// <summary>
        /// 输入的可以是PalletNo，也可以是ProductID
        /// </summary>
        /// <param name="ProductIDOrPalletNo"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintLabel(string ProductIDOrPalletNo, IList<PrintItem> printItems, string line, string editor, string station, string customer, string reason, out bool isPalletFull, out string out_palletNo);

        /// <summary>
        /// 获取属于该DN,Pallet的所有Product
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="pallet"></param>
        /// <returns></returns>
        IList<ProductModel> getProductByDnPallet(string dn, string pallet);

        /// <summary>
        /// 获取属于该Pallet的所有CustSN
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<string> getProductByPallet(string palletNo);

        /// <summary>
        /// 根据palletNo获取Delivery_Pallet中的DN和Qty
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        Dictionary<string, int> getDeliveryAndQtyByPalletNo(string palletNo);

        /// <summary>
        /// 获取Delivery_Pallet的qty
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        int getDNPalletQty(string deliveryNo, string palletNo);

        #endregion

    }
}
