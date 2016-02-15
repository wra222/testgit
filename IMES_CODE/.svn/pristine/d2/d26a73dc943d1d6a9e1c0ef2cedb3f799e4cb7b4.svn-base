
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FRU Weight Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-09   LuycLiu     Create 
 * 2010-05-11  Lucy Liu(EB1)       Modify:   ITC-1155-0060 
 * 该实现文件不需要编写工作流，直接掉Repositroy就可以
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.WeightLog;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{

    public class FRUWeight : MarshalByRefObject, IFRUWeight
    {
       
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region IFRUWeight Members


        /// <summary>
        /// 称重
        /// </summary>
        /// <param name="pno">PNO</param>
        /// <param name="weight">重量</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="line">product line</param>
        public void Weight(string pno, decimal weight, string line,
             string editor, string stationId, string customerId)
        {



            logger.Debug("(FRUWeight)Weight Start, "
                           + " [pno]:" + pno
                           + " [weight]:" + weight
                           + " [editor]:" + editor
                           + " [stationId]:" + stationId
                           + " [customer]:" + customerId);
            try
            {
                //如果FRUWeight中有该Pno重量则更新,没有就插入
                FRUWeightLog weightLog = new FRUWeightLog();
                weightLog.SN = pno;
                weightLog.Line = line;
                weightLog.Station = stationId;
                weightLog.Editor = editor;
                //这里的weight需要用weight/Qty,保留3位小数传给刘东
                //decimal oneWeight = weight / num;
                //weightLog.Weight = decimal.Round(oneWeight, 3);
                weightLog.Weight = weight;

                IWeightLogRepository weightRep = RepositoryFactory.GetInstance().GetRepository<IWeightLogRepository, WeightLog>();
                weightRep.AddOrModifyFRUWeightLog(weightLog);
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

                logger.Debug("(FRUWeight)Weight End, "
                               + " [pno]:" + pno
                               + " [weight]:" + weight
                               + " [editor]:" + editor
                               + " [stationId]:" + stationId
                               + " [customer]:" + customerId);
            }

        }


        /// <summary>
        /// 检查扫入Pno的合法性
        /// </summary>
        /// <param name="pno">Pno</param>
        /// <returns>bool</returns>
        public bool ValidatePNO(string pno)
        {
           
            logger.Debug("(FRUWeight)ValidatePNO Start, "
                           + " [pno]:" + pno);
            try
            {
                IProductRepository weightRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                bool result = weightRep.CheckFruNo(pno);


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
                logger.Debug("(FRUWeight)ValidatePNO End, "
                           + " [pno]:" + pno);
            }
           
        }

        /// <summary>
        /// 根据扫入的pno获取重量
        /// </summary>
        /// <param name="pno">pno</param>
        /// <returns>重量</returns>
        public decimal GetWeight(string pno)
        {

            logger.Debug("(FRUWeight)GetWeight Start, "
                       + " [pno]:" + pno);
            try
            {
                IWeightLogRepository weightRep = RepositoryFactory.GetInstance().GetRepository<IWeightLogRepository, WeightLog>();
                decimal result = weightRep.GetWeightOfFRUWeightLog(pno);
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
                logger.Debug("(FRUWeight)GetWeight End, "
                           + " [pno]:" + pno);
            }
        }
        #endregion

      
    }
}