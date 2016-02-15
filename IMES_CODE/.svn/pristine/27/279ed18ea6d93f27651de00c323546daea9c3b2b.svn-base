// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 071PrintShiptoCartonLabel
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-22   Chen Xu (eB1-4)              create
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
    /// PrintShiptoCartonLabel
    /// </summary>
    public interface IPrintShiptoCartonLabel
    {

        #region methods interact with the running workflow

        /// <summary>
        /// 刷CartonSN，选择相应的DN后，调用该方法
        /// 更新Carton 中所有Product 的状态，记录Carton 中所有Product Log，记录Carton 中所有Product 与DN 的绑定， DN 满，则更新DN 的状态
        /// 返回打印重量标签的PrintItem，结束工作流。
        /// 将CartonSN,deliveryNo放到Session.DeliveryNo里
        /// 以CartonSN为SessionKey创建工作流071PrintShiptoCartonLabel.xoml
        /// </summary>
        /// <param name="PdLine"></param>
        /// <param name="CartonSN"></param>
        /// <param name="deliveryNo"></param>
        /// <param name="printItems"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<PrintItem> Save(string PdLine, string CartonSN, string deliveryNo, IList<PrintItem> printItems, string editor, string station, string customer);

        
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="cartonSN"></param>
        void Cancel(string cartonSN);

        /// <summary>
        /// 输入CartonNo,选择Reason
        /// 使用工作流071ReprintShiptoCartonLabelImpl.xoml
        /// </summary>
        IList<PrintItem> ReprintShiptoCartonLabel(string cartonNo, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer);

        #endregion

        #region methods do not interact with the running workflow

        /// <summary>
        /// 检查DN状态是否已经满了
        /// 总DN数量： Delivery.Qty
        /// 已经使用的DN数量： select (*) count  from IMES_FA..Product WHERE DeliveryNo = @DN
        /// </summary>
        /// <returns></returns>
        Boolean CheckDNisFull(string deliveryNo);

        /// <summary>
        /// 检查用户输入的[Carton No] 在IMES_FA..Product 表中是否存在
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <returns>model</returns>
        string CheckCartonSN(string cartonSN);

        /// <summary>
        /// 显示被选DN的total 数量，已结合数量和剩余数量
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        IList<string> GetDNQtyInfo(string deliveryNo);

        /// <summary>
        /// 获取PrintShiptoCartonLabel的DN列表
        /// 调用 IDeliveryRepository.getDeliveryNoListFor071()
        /// DN 按照Model（按照Carton No 查询IMES_FA..Product 得到）
        /// Status='00'
        /// Ship Date大于等于当天为条件进行查询
        /// 结果按照ShipDate, Delivery No 进行排序
        /// </summary>
        IList<string> getDNList(string Model);

        #endregion

      

    }
}
