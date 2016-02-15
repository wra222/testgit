using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;

namespace IMES.FisObject.Common.Warranty
{
    ///<summary>
    ///</summary>
    public interface IWarrantyRepository : IRepository<Warranty>
    {
        /// <summary>
        /// 为MB获得DCodeRule列表 (区分是否FRU)
        /// </summary>
        /// <param name="isFRU"></param>
        /// <returns></returns>
        IList<Warranty> GetDCodeRuleListForMB(bool isFRU, string customer);

        /// <summary>
        /// 为MB获得DCodeRule列表
        ///  SELECT  [ID], [Descr]  FROM [IMES2012_GetData].[dbo].[Warranty] where Type='MBDateCode' and Customer=(select Value from [SysSetting] where Name = @Customer)
        /// </summary>
        /// <returns></returns>
        IList<Warranty> GetDCodeRuleListForMB(string customer);

        /// <summary>
        /// 为VB获得DCodeRule列表
        /// </summary>
        /// <returns></returns>
        IList<Warranty> GetDCodeRuleListForVB(string customer);

        /// <summary>
        /// 为KP获得DCodeRule列表
        /// </summary>
        /// <returns></returns>
        IList<Warranty> GetDCodeRuleListForKP(string customer);

        /// <summary>
        /// 为DK获得DCodeRule列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<Warranty> GetDCodeRuleListForDK(string customer);

        /// <summary>
        /// Almighty
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<Warranty> GetWarrantyListByCondition(Warranty condition);

        #region For Maintain

        /// <summary>
        /// 取得Customer Id下的Warranty数据的list(按Description栏位排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        IList<Warranty> GetWarrantyList(string customerId);
 
        /// <summary>
        /// 取得一条Warranty的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        Warranty GetWarranty(int warrantyId);

        /// <summary>
        /// SELECT [ID]      
        ///   FROM [IMES_GetData].[dbo].[Warranty]
        /// where [Customer]='Customer' AND [Descr]='Descr' AND ID<>id
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="descr"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetExistWarranty(string customer, string descr, int id);

        /// <summary>
        /// SELECT [ID]      
        ///  FROM [IMES_GetData].[dbo].[Warranty]
        /// where [Customer]='Customer' AND [Descr]='Descr'
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="descr"></param>
        /// <returns></returns>
        DataTable GetExistWarranty(string customer, string descr);

        #endregion
    }
}
