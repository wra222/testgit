// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
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
using System.Data;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PAK.StandardWeight
{
    /// <summary>
    /// ModelWeight对象Repository接口
    /// </summary>
    public interface IModelWeightRepository : IRepository<ModelWeight>
    {
        /// <summary>
        /// SELECT [Model],[UnitWeight],FROM [ModelWeight] where [Model]=@model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DataTable GetModelWeightItem(string model);

        /// <summary>
        /// UPDATE IMES_GetData..ModelWeight SET UnitWeight = @weight, 
        /// Editor = @editor, Udt = GETDATE() WHERE Model = @model
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateModelWeight(ModelWeightInfo setValue, ModelWeightInfo condition);

        #region . Defered .

        void UpdateModelWeightDefered(IUnitOfWork uow, ModelWeightInfo setValue, ModelWeightInfo condition);

        #endregion
    }
}
