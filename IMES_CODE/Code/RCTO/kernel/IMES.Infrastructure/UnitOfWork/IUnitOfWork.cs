using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Util;

namespace IMES.Infrastructure.UnitOfWork
{
    ///<summary>
    /// Unit of work接口
    ///</summary>
    public interface IUnitOfWork
    {
        ///<summary>
        /// 将一个FisObject注册为Added
        ///</summary>
        ///<param name="entity">fisobject</param>
        ///<param name="repository">该FisObject类型的Repository对象</param>
        void RegisterAdded(IAggregateRoot entity, IUnitOfWorkRepository repository);

        ///<summary>
        /// 将一个FisObject注册为Changed
        ///</summary>
        ///<param name="entity">fisobject</param>
        ///<param name="repository">该FisObject类型的Repository对象</param>
        void RegisterChanged(IAggregateRoot entity, IUnitOfWorkRepository repository);

        ///<summary>
        /// 将一个FisObject注册为Removed
        ///</summary>
        ///<param name="entity">fisobject</param>
        ///<param name="repository">该FisObject类型的Repository对象</param>
        void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository repository);

        /// <summary>
        /// 注册延迟改库方法
        /// </summary>
        /// <param name="ivkBdy"></param>
        void RegisterDeferMethods(InvokeBody ivkBdy);

        /// <summary>
        /// 注册对象间属性关联赋值请求
        /// </summary>
        /// <param name="setter"></param>
        void RegisterSetterBetween(SetterBetween setter);

        ///<summary>
        /// 提交所有修改结果
        ///</summary>
        void Commit();
    }
}
