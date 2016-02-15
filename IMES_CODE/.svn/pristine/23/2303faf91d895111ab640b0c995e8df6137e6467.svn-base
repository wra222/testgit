using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using UPH.Entity.Infrastructure.Interface;
using log4net;
using System.Reflection;

namespace UPH.Entity.Infrastructure.Framework
{
    public class Repository<T> : IRepository<T>//, IUowRepository
    where T : class
    {
         //private object syncRoot = new object();

        private string _dbName = null;
        //protected IDataContext _dataContextFactory;
        //protected DataContext context = UnitOfWork.Context;

        public DataContext Context()
        {
            return UnitOfWork.Context(_dbName);
        }

        /// <summary>
        /// Return all instances of type T.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Query()
        {           
              return GetTable;          
        }

        /// <summary>
        /// Return all instances of type T that match the expression exp.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IList<T> Find(Func<T, bool> exp)
        {
            //string name = PrimaryKeyName;
           return GetTable.Where<T>(exp).ToList();
            
        }

        /// <summary>See _vertexRepository.</summary>
        /// <param name="exp"></param><returns></returns>
        public T Single(Func<T, bool> exp)
        {           
             return GetTable.SingleOrDefault(exp);
         }

        /// <summary>See _vertexRepository.</summary>
        /// <param name="exp"></param><returns></returns>
        public T First(Func<T, bool> exp)
        {
            return GetTable.FirstOrDefault(exp);           
        }

        /// <summary>Delete</summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity, bool bAttached)
        {
            if (bAttached)
            {
                GetTable.Attach(entity, false);
            }
           GetTable.DeleteOnSubmit(entity);            
        }

       

        /// <summary>
        /// Create a new instance of type T.
        /// </summary>
        /// <returns></returns>
        public virtual void Insert(T entity)
        {    
            GetTable.InsertOnSubmit(entity);            
        }

        /// <summary>commit save change</summary>
        public virtual void Update(T entity)
        {
            GetTable.Attach(entity,true);
        }

        //public void Attach(T newEntity, T oldEntity)
        //{
        //    GetTable.Attach(newEntity, oldEntity);
        //}

        //public void Attach(T newEntity, bool bModified)
        //{
        //    GetTable.Attach(newEntity, bModified);
        //}

        public Repository(string dbName)
        {
            _dbName = dbName;
        }

        
        #region Properties

        private IList<string> PrimaryKeyName
        {
           
            get{
                IList<string> ret= new List<string>();
                
                 foreach (MetaDataMember item in TableMetadata.RowType.IdentityMembers)
                 {
                     if (item.IsPrimaryKey == true)
                     {
                         ret.Add(item.Name);
                     }
                 }

                return ret;
             }  
        }

        private Table<T> GetTable
        {
            get {
                return UnitOfWork.Context(_dbName).GetTable<T>(); 
                    }
        }

        private MetaTable TableMetadata
        {
            get { return UnitOfWork.Context(_dbName).Mapping.GetTable(typeof(T)); }
        }

        private MetaType ClassMetadata
        {
            get { return UnitOfWork.Context(_dbName).Mapping.GetMetaType(typeof(T)); }
        }

        #endregion
    }
}
