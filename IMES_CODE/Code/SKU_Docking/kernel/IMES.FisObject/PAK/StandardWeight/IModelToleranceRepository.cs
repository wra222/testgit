// INVENTEC corporation (c)2009 all rights reserved. 
// Description: ModelTolerance对象Repository接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PAK.StandardWeight
{
    /// <summary>
    /// ModelTolerance对象Repository接口
    /// </summary>
    public interface IModelToleranceRepository : IRepository<ModelTolerance>
    {
        #region For Maintain

        /// <summary>
        /// 根据customer获取Model误差范围
        /// 参考sql
        /// select * from IMES_PAK..ModelWeightTolerance where Model=? 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        ModelTolerance FindModelWeightToleranceByCustomer(string customer);

        /// <summary>
        /// 参考sql
        /// select * 
        /// from IMES_PAK..ModelWeightTolerance a 
        ///      join IMES_GetData..Model b 
        ///           on a.Model=b.Model 
        /// where b.Family=?
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<ModelTolerance> FindModelWeightToleranceListByFamily(string family);

        /// <summary>
        /// 删除Model误差范围
        /// </summary>
        /// <param name="model"></param>
        void DeleteModelWeightToleranceByModel(string model);

        /// <summary>
        /// 增加Model误差范围
        /// </summary>
        /// <param name="modelWeightTolerance"></param>
        void AddModelWeightTolerance(ModelTolerance modelWeightTolerance);


        /// <summary>
        /// 修改Model误差范围
        /// </summary>
        /// <param name="modelWeightTolerance"></param>
        /// <param name="model"></param>
        void UpdateModelWeightTolerance(ModelTolerance modelWeightTolerance, string model);

        #region Defered

        void DeleteModelWeightToleranceByModelDefered(IUnitOfWork uow, string model);

        void AddModelWeightToleranceDefered(IUnitOfWork uow, ModelTolerance modelWeightTolerance);

        void UpdateModelWeightToleranceDefered(IUnitOfWork uow, ModelTolerance modelWeightTolerance, string model);

        #endregion

        #endregion
    }
}
