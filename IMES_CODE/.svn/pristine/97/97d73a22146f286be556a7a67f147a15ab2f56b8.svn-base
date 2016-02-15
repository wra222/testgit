// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Pallet data collection
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-04-01   Tong.Zhi-Yong                create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;
namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 绑定Pallet和ProductID, 绑定DN和ProductID,打印Pallet标签
    /// </summary>
    public interface IPalletDataCollection
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 刷第一个CartonNo的时候，先调用本方法开启工作流
        /// 将palletNo放到Session.PalletNo中
        /// 将deliveryNo放到Session.DeliveryNo中
        /// </summary>
        /// <param name="cartonNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>IList</returns>
        IList startWorkflow(string cartonNo, string palletNo, string line, string editor, string station, string customer, Boolean isCombined);

        /// <summary>
        /// 刷SN，将custSN放到Session.CustSN中
        /// 卡站成功后返回ProductID
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="cartonNo"></param>
        /// <returns>返回cartonNo</returns>
        string inputCartonNo(string dn, string palletNo, string cartonNo);

        /// <summary>
        /// 这个Pallet上所有的SN都被刷入时，调用本方法结束工作流
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList save(string dn, string palletNo, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="palletNo"></param>
        void cancel(string dn, string palletNo);


        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="pltNoOrCartonNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintLabel(string pltNoOrCartonNo, string line, string editor, string station, string customer, IList<PrintItem> printItems, string reason);

        #endregion


        #region methods do not interact with the running workflow

        /// <summary>
        /// 获取属于该Carton的所有Pallet列表
        /// </summary>
        /// <param name="bolNo"></param>
        /// <returns></returns>
        IList<string> getPalletListByCarton(string cartonNo);

        /// <summary>
        /// get dn palletinfo by palletno
        /// </summary>
        /// <param name="pltno"></param>
        /// <returns></returns>
        IList<DNForUI> getInfoByPallet(string pltno);
        #endregion


    }
}
