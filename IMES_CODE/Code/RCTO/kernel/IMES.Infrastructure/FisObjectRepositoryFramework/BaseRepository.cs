﻿// 2010-04-09 Liu Dong(eB1-4)         Modify ITC-1122-0241
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using IMES.Infrastructure.Util;
using log4net;

namespace IMES.Infrastructure.FisObjectRepositoryFramework
{
    ///<summary>
    ///Repository基类
    ///</summary>
    ///<typeparam name="T">具体的Repository访问的IAggregateRoot对象类型</typeparam>
    public abstract class BaseRepository<T> : IRepository<T>, IUnitOfWorkRepository where T : IAggregateRoot
    {
        protected static ILog logger = LogManager.GetLogger("BaseRepository");
        
        protected const string ItemDirty = "Item Dirty";

        protected const string TypeName = "BaseRepository";

        private static readonly string MsgBegin = "BEGIN: {0}::{1}()";
        private static readonly string MsgEnd = "END: {0}::{1}()";
        private static readonly string MsgError = "ERROR: {0}::{1}()";

        private static readonly string MethodAdd = "Add";
        private static readonly string MethodRemove = "Remove";
        private static readonly string MethodUpdate = "Update";
        private static readonly string MethodPersistNewItem = "PersistNewItem";
        private static readonly string MethodPersistUpdatedItem = "PersistUpdatedItem";
        private static readonly string MethodPersistDeletedItem = "PersistDeletedItem";
        private static readonly string MethodAddOneInvokeBody = "AddOneInvokeBody";
        private static readonly string MethodAddOneInvokeGenericBody = "AddOneInvokeGenericBody";
        private static readonly string MethodAddOneInvokeAction = "AddOneInvokeAction";
        
        /// <summary>
        /// 批量SQL操作个数
        /// </summary>
        protected int batchSQLCnt = 88;

        //new logging method
        protected static void LoggingBegin(string typeName, string name)
        {
            logger.DebugFormat(MsgBegin, typeName, name);
        }
        protected static void LoggingEnd(string typeName, string name)
        {
            logger.DebugFormat(MsgEnd, typeName, name);
        }
        protected static void LoggingError(string typeName, string name)
        {
            logger.DebugFormat(MsgError, typeName, name);
        }

        //Old method
        protected static void LoggingBegin(Type tp, MethodBase method)
        {
            logger.DebugFormat(MsgBegin, tp.Name, method.Name);
        }
        protected static void LoggingEnd(Type tp, MethodBase method)
        {
            logger.DebugFormat(MsgEnd, tp.Name, method.Name);
        }
        protected static void LoggingError(Type tp, MethodBase method)
        {
            logger.DebugFormat(MsgError, tp.Name, method.Name);
        }
        
        protected static void LoggingInfoFormat(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
        }

        #region Methods

        protected abstract void PersistNewItem(T item);
        protected abstract void PersistUpdatedItem(T item);
        protected abstract void PersistDeletedItem(T item);

        #endregion

        #region Implementation of IRepository<T>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public virtual T Find(object key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public virtual IList<T> FindAll()
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 使用[]操作符,获取/设置指定key的对象
        ///// </summary>
        ///// <param name="key">对象key</param>
        ///// <returns>指定key的对象</returns>
        //public virtual T this[object key]
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        /// <param name="uow"></param>
        public virtual void Add(T item, IUnitOfWork uow)
        {
            //string methodName = "Add";
            LoggingBegin(TypeName, MethodAdd);
            try
            {
                if (uow != null)
                {
                    uow.RegisterAdded(item, this);
                }
            }
            finally
            {
                LoggingEnd(TypeName, MethodAdd);
            }
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public virtual void Remove(T item, IUnitOfWork uow)
        {
            //string methodName = "Remove";
            LoggingBegin(TypeName, MethodRemove);
            try
            {
                if (uow != null)
                {
                    if (item is FisObjectBase.FisObjectBase)
                    {
                        object obj = item;
                        FisObjectBase.FisObjectBase fobjb = (FisObjectBase.FisObjectBase)obj;//(FisObjectBase.FisObjectBase)Convert.ChangeType(item,typeof(FisObjectBase.FisObjectBase));
                        fobjb.Tracker.MarkAsDeleted(fobjb);
                    }
                    uow.RegisterRemoved(item, this);
                }
            }
            finally
            {
                LoggingEnd(TypeName, MethodRemove);
            }
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public virtual void Update(T item, IUnitOfWork uow)
        {
            //string methodName = "Update";
            LoggingBegin(TypeName, MethodUpdate);
            try
            {
                if (uow != null)
                {
                    uow.RegisterChanged(item, this);
                }
            }
            finally
            {
                LoggingEnd(TypeName, MethodUpdate);
            }
        }

        #endregion

        #region Implementation of IUnitOfWorkRepository

        /// <summary>
        /// 执行新增对象的持久化
        /// </summary>
        /// <param name="item">目标对象</param>
        public virtual void PersistNewItem(IAggregateRoot item)
        {
            //string methodName = "PersistNewItem";
            LoggingBegin(TypeName, MethodPersistNewItem);
            try
            {
                this.PersistNewItem((T)item);
            }
            finally
            {
                LoggingEnd(TypeName, MethodPersistNewItem);
            }
        }

        /// <summary>
        /// 执行更新对象的持久化
        /// </summary>
        /// <param name="item">目标对象</param>
        public virtual void PersistUpdatedItem(IAggregateRoot item)
        {
            //string methodName = "PersistUpdatedItem";
            LoggingBegin(TypeName, MethodPersistUpdatedItem);
            try
            {
                this.PersistUpdatedItem((T)item);
            }
            finally
            {
                LoggingEnd(TypeName, MethodPersistUpdatedItem);
            }
        }

        /// <summary>
        /// 执行删除对象的持久化
        /// </summary>
        /// <param name="item">目标对象</param>
        public virtual void PersistDeletedItem(IAggregateRoot item)
        {
            //string methodName = "PersistDeletedItem";
            LoggingBegin(TypeName, MethodPersistDeletedItem);
            try
            {
                this.PersistDeletedItem((T)item);
            }
            finally
            {
                LoggingEnd(TypeName, MethodPersistDeletedItem);
            }
        }

        #endregion

        #region . GetValues .

        protected bool IsNull(SqlDataReader sqlDataReader, int iCol)
        {
            return sqlDataReader.IsDBNull(iCol);
        }

        protected bool GetValue_Bit(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetBoolean(iCol);
            else
                return false;
        }

        /// <summary>
        /// 判断空值并赋值(string)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected string GetValue_Str(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetString(iCol).TrimEnd();
            else
                return string.Empty;
        }

        protected string GetValue_StrForNullable(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetString(iCol).TrimEnd();
            else
                return null;
        }

        /// <summary>
        /// 判断空值并赋值(Int32)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected int GetValue_Int32(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol) )
                return sqlDataReader.GetInt32(iCol);
            else
                return 0;
        }

        /// <summary>
        /// 判断空值并赋值(Byte)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected Byte GetValue_Byte(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol) )
                return sqlDataReader.GetByte(iCol);
            else
                return 0;
        }

        /// <summary>
        /// 判断空值并赋值(Int16)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected short GetValue_Int16(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol) )
                return sqlDataReader.GetInt16(iCol);
            else
                return 0;
        }

        /// <summary>
        /// 判断空值并赋值(DateTime)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected DateTime GetValue_DateTime(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol) )
                return sqlDataReader.GetDateTime(iCol);
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// 判断空值并赋值(float)
        /// 数据库里的 Float 类型，其实相当于double, DataReader.GetFloat()會報錯
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected double GetValue_Float(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetDouble(iCol);// 2010-04-09 Liu Dong(eB1-4)         Modify ITC-1122-0241
            else
                return 0.0D;
        }

        ///// <summary>
        ///// 判断空值并赋值(double) 
        ///// 数据库里的 Float 类型，其实相当于double, DataReader.GetFloat()會報錯
        ///// </summary>
        ///// <param name="sqlDataReader"></param>
        ///// <param name="iCol"></param>
        //protected double GetValue_Double(SqlDataReader sqlDataReader, int iCol)
        //{
        //    if (! sqlDataReader.IsDBNull(iCol) )
        //        return sqlDataReader.GetDouble(iCol);
        //    else
        //        return 0.0;
        //}

        /// <summary>
        /// 判断空值并赋值(decimal)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected decimal GetValue_Decimal(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol) )
                return sqlDataReader.GetDecimal(iCol);
            else
                return 0.0M;
        }

        /// <summary>
        /// 判断空值并赋值(char)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        protected char GetValue_Char(SqlDataReader sqlDataReader, int iCol)
        {
            if (! sqlDataReader.IsDBNull(iCol) )
                return Convert.ToChar(sqlDataReader.GetValue(iCol));
            else
                return Char.MinValue;
        }

        /// <summary>
        /// 判断空值并赋值(Int64)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected long GetValue_Int64(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetInt64(iCol);
            else
                return 0L;
        }

        protected string GetValue_Int32_ToStringWithNull(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetInt32(iCol).ToString();
            else
                return null;
        }

        /// <summary>
        /// 判断空值并赋值(char)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        protected bool GetValue_Boolean(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return Convert.ToBoolean(sqlDataReader.GetValue(iCol));
            else
                return false;
        }

        protected object ToInt32WithNull(string str)
        {
            if (str == null)
                return Convert.DBNull;
            return Convert.ToInt32(str);
        }

        #endregion

        #region . Invoking .

        protected InvokeBody AddOneInvokeBody(IUnitOfWork uow, MethodBase method, params object[] args)
        {
            //string methodName = "AddOneInvokeBody";
            LoggingBegin(TypeName, MethodAddOneInvokeBody);
            try
            {
                return InvokeBody.AddOneInvokeBody(this, uow, method, args);
            }
            finally
            {
                LoggingEnd(TypeName, MethodAddOneInvokeBody);
            }
        }

        protected InvokeBody AddOneInvokeBody(IUnitOfWork uow, int iBeginParam, MethodBase method, params object[] args)
        {
            //string methodName = "AddOneInvokeBody";
            LoggingBegin(TypeName, MethodAddOneInvokeBody);
            try
            {
                return InvokeBody.AddOneInvokeBody(this, uow, method, iBeginParam, args);
            }
            finally
            {
                LoggingEnd(TypeName, MethodAddOneInvokeBody);
            }
        }

        protected InvokeBody AddOneInvokeGenericBody(IUnitOfWork uow, MethodBase method, Type[] genericTypes, params object[] args)
        {
            //string methodName = "AddOneInvokeBody";
            LoggingBegin(TypeName, MethodAddOneInvokeGenericBody);
            try
            {
                return InvokeBody.AddOneInvokeGenericBody(this, uow, method, genericTypes, args);
            }
            finally
            {
                LoggingEnd(TypeName, MethodAddOneInvokeGenericBody);
            }
        }
        protected InvokeBody AddOneInvokeBody(IUnitOfWork uow, Action action)
        {
            //string methodName = "AddOneInvokeBody";
            LoggingBegin(TypeName, MethodAddOneInvokeAction);
            try
            {
                return InvokeBody.AddOneInvokeBody(this, uow, action);
            }
            finally
            {
                LoggingEnd(TypeName, MethodAddOneInvokeAction);
            }
        }
        #endregion
    }

}
