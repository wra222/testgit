
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FRU Weight Shipment Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-09   LuycLiu     Create 
 * 该实现文件不需要编写工作流，直接掉Repositroy就可以
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{

    public class FRUWeightShipment : MarshalByRefObject, IFRUWeightShipment
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region IFRUWeightShipment Members


        /// <summary>
        /// Shipment称重
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="weight">重量</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        public void WeightShipment(string shipment, decimal weight,
             string editor, string stationId, string customerId)
        {
            logger.Debug("(FRUWeightShipment)Weight Start, "
                          + " [shipment]:" + shipment
                          + " [weight]:" + weight
                          + " [editor]:" + editor
                          + " [stationId]:" + stationId
                          + " [customer]:" + customerId);
            try
            {
                //如果FRUWeight中有该Pno重量则更新,没有就插入
                FRUFISToSAPWeight sapWeight = new FRUFISToSAPWeight();
                sapWeight.Shipment = shipment;
                sapWeight.Weight = weight;
                sapWeight.Type = "S";
                sapWeight.Status = "0";

                IPalletWeightRepository weightRep = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, IMES.FisObject.PAK.Pallet.PalletWeight>();
                weightRep.AddOrModifyFRUFISToSAPWeight(sapWeight);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {

                logger.Debug("(FRUWeightShipment)Weight End, "
                            + " [shipment]:" + shipment
                            + " [weight]:" + weight
                            + " [editor]:" + editor
                            + " [stationId]:" + stationId
                            + " [customer]:" + customerId);
            }

        }


        /// <summary>
        /// 根据shipment获取重量
        /// </summary>
        /// <param name="shipment">shipment</param>
        /// <returns>重量</returns>
        public decimal GetShipmentWeight(string shipment)
        {
            logger.Debug("(FRUWeightShipment)GetShipmentWeight Start, "
                     + " [shipment]:" + shipment);
            try
            {
                IPalletWeightRepository weightRep = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, IMES.FisObject.PAK.Pallet.PalletWeight>();

                
                decimal result = weightRep.GetWeightOfFRUFISToSAPWeight(shipment);
                return result;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(FRUWeightShipment)GetShipmentWeight End, "
                     + " [shipment]:" + shipment);
            }
        }
        #endregion
    }
}