using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace UPH.Entity.Infrastructure.Interface
{
       public interface IRepository<T> where T : class
    {  
   

        /// <summary>
        /// Get DataContext
        /// </summary>
        /// <returns></returns>
       DataContext Context();
        /// <summary>
        /// Return all instances of type T.
        /// </summary>
        /// <returns></returns>
       IQueryable<T> Query();

        /// <summary>
        /// Return all instances of type T that match the expression exp.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        IList<T> Find(Func<T, bool> exp);

        /// <summary>Returns the single entity matching the expression. Throws an exception if there is not exactly one such entity.</summary>
        /// <param name="exp"></param><returns></returns>
        T Single(Func<T, bool> exp);

        /// <summary>Returns the first element satisfying the condition.</summary>
        /// <param name="exp"></param><returns></returns>
        T First(Func<T, bool> exp);

        /// <summary>
        /// Mark an entity to be deleted when the context is saved.
        /// </summary>
        /// <param name="entity"></param>
        //void DeleteMark(T entity);

        /// <summary>
        /// Mark an entity to be deleted when the context is saved.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity, bool bAttached);

     

        /// <summary>
        /// Mark insert Data
        /// </summary>
        /// <returns></returns>
        void Insert(T entity);

        void Update(T entity);

        /// <summary>Persist the data context.</summary>
        //void Attach(T newEntity, T oldEntity);

        //void Attach(T newEntity, bool bModified);
    } 
}
