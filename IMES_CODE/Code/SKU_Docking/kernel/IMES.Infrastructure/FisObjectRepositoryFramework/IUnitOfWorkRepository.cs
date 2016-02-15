using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.Infrastructure.FisObjectRepositoryFramework
{
    ///<summary>
    /// 执行UnitOfWork数据操作的Repository接口
    ///</summary>
    public interface IUnitOfWorkRepository
    {
        /// <summary>
        /// 执行新增对象的持久化
        /// </summary>
        /// <param name="item">目标对象</param>
        void PersistNewItem(IAggregateRoot item);

        /// <summary>
        /// 执行更新对象的持久化
        /// </summary>
        /// <param name="item">目标对象</param>
        void PersistUpdatedItem(IAggregateRoot item);

        /// <summary>
        /// 执行删除对象的持久化
        /// </summary>
        /// <param name="item">目标对象</param>
        void PersistDeletedItem(IAggregateRoot item);
    }
}
