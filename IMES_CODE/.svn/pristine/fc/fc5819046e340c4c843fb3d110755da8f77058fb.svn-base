using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;
using IMES.Infrastructure;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;

namespace IMES.Maintain.Implementation
{
    public class SpecialOrder : MarshalByRefObject, ISpecialOrder
    {
        IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
        IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        #region Implementation of ISpecialOrder

        /// <summary>
        /// 寫入一筆記錄
        /// </summary>
        /// <returns></returns>
        public void InsertSpecialOrder(SpecialOrderInfo sepcialOrder)
        {
            try
            {
                DeliveryRepository.InsertSpecialOrder(sepcialOrder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 寫入多筆記錄
        /// </summary>
        /// <returns></returns>
        public IList<string> InsertSpecialOrder(IList<SpecialOrderInfo> sepcialOrderList)
        {
            IList<string> ret = new List<string>();
            try
            {
                foreach (SpecialOrderInfo item in sepcialOrderList)
                {
                    if (ExistSpecialOrder(item.FactoryPO))
                    {
                        ret.Add(item.FactoryPO);
                    }
                    else
                    {
                        DeliveryRepository.InsertSpecialOrder(item);    
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        /// <summary>
        /// 更新一筆記錄
        /// </summary>
        /// <returns></returns>
        public void UpdateSpecialOrder(SpecialOrderInfo sepcialOrder)
        {
            try
            {
                DeliveryRepository.UpdateSpecialOrder(sepcialOrder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除一筆記錄
        /// </summary>
        /// <returns></returns>
        public void DeleteSpecialOrder(string factoryPO)
        {
            try
            {
                DeliveryRepository.DeleteSpecialOrder(factoryPO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查詢記錄
        /// </summary>
        /// <returns></returns>
        public IList<SpecialOrderInfo> GetSpecialOrder(string category, SpecialOrderStatus status, DateTime startTime, DateTime endTime)
        {
            try
            {
                IList<SpecialOrderInfo> list = DeliveryRepository.GetSpecialOrder(category, status, startTime, endTime);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 記錄是否存在
        /// </summary>
        /// <returns></returns>
        public bool ExistSpecialOrder(string factoryPO)
        {
            try
            {
                bool ret = DeliveryRepository.ExistSpecialOrder(factoryPO);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// CostValueType 下拉選項
        /// </summary>
        /// <returns></returns>
        public IList<ConstValueTypeInfo> GetConstValueTypeList(string Type)
        {
            try
            {
                IList<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();
                ret =  PartRepository.GetConstValueTypeList(Type);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查詢記錄by FactoryPO
        /// </summary>
        /// <returns></returns>
        public IList<SpecialOrderInfo> GetSpecialOrderByPO(string factoryPO)
        {
            try
            {
                SpecialOrderInfo item = DeliveryRepository.GetSpecialOrderByPO(factoryPO);
                IList<SpecialOrderInfo> ret = new List<SpecialOrderInfo>();
                if (item != null)
                {
                    ret.Add(item);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion




    }
}