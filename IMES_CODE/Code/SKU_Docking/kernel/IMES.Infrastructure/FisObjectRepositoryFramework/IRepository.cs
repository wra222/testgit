using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Infrastructure.FisObjectRepositoryFramework
{
    ///<summary>
    /// Repository基类
    ///</summary>
    ///<typeparam name="T">Repository访问的IAggregateRoot具体类型</typeparam>
    public interface IRepository<T> where T : IAggregateRoot
    {
        //void SetUnitOfWork(IUnitOfWork unitOfWork);
        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        T Find(object key);

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        IList<T> FindAll();

        ///// <summary>
        ///// 使用[]操作符,获取/设置指定key的对象
        ///// </summary>
        ///// <param name="key">对象key</param>
        ///// <returns>指定key的对象</returns>
        //T this[object key] { get; set; }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        /// <param name="uow"></param>
        void Add(T item, IUnitOfWork uow);

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        void Remove(T item, IUnitOfWork uow);

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        void Update(T item, IUnitOfWork uow);

    }
}
