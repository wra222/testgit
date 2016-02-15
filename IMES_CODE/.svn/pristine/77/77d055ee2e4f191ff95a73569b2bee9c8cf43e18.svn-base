/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FRU Weight Shipment Interface
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-09   LuycLiu     Create 
 * 该实现文件不需要编写工作流，直接掉Repositroy就可以
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    public interface IFRUWeightShipment
    {
        /// <summary>
        /// Shipment称重
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="weight">重量</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        void WeightShipment(string shipment, decimal weight,
             string editor, string stationId, string customerId);

        /// <summary>
        /// 根据shipment获取重量
        /// </summary>
        /// <param name="shipment">shipment</param>
        /// <returns>重量</returns>
        decimal GetShipmentWeight(string shipment);
    }
}
