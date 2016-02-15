//2010-03-06  itc207031     Modify ITC-1122-0206

using System;
using System.Collections.Generic;
using System.Transactions;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Util;
using log4net;
using System.Collections;
using System.Reflection;

namespace IMES.Infrastructure.UnitOfWork
{
    public class SetterBetween
    {
        public SetterBetween(object origin, string originProperty, object destination, string destinationProperty)
        {
            _origin = origin;
            _originProperty = originProperty;
            _destination = destination;
            _destinationProperty = destinationProperty;
        }
        object _origin = null;
        string _originProperty = string.Empty;
        object _destination = null;
        string _destinationProperty = string.Empty;

        public object Origin
        {
            get { return _origin; }
        }
        public string OriginProperty
        {
            get { return _originProperty; }
        }
        public object Destination
        {
            get { return _destination; }
        }
        public string DestinationProperty
        {
            get { return _destinationProperty; }
        }
    }

    ///<summary>
    /// Unit of work
    ///</summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IList _execSeq = new ArrayList();
        
        private readonly Dictionary<IAggregateRoot, IUnitOfWorkRepository> _added;
        private readonly Dictionary<IAggregateRoot, IUnitOfWorkRepository> _changed;
        private readonly Dictionary<IAggregateRoot, IUnitOfWorkRepository> _deleted;
        private readonly Dictionary<IAggregateRoot, IUnitOfWorkRepository> _command;

        private readonly List<InvokeBody> _invokeBodies;

        private static readonly string MsgRegisterAdded = "UnitOfWork.RegisterAdded: {0}, {1}";
        private static readonly string MsgRegisterChanged = "UnitOfWork.RegisterChanged: {0}, {1}";
        private static readonly string MsgRegisterRemoved = "UnitOfWork.RegisterRemoved: {0}, {1}";
        private static readonly string MsgRegisterDeferMethods ="UnitOfWork.RegisterDeferMethods: {0}";
        private static readonly string MsgCommitAdd = "UnitOfWork.Commit: added {0}, {1}";
        private static readonly string MsgCommitChanged = "UnitOfWork.Commit: changed {0}, {1}";
        private static readonly string MsgCommitDelete = "UnitOfWork.Commit: delete {0}, {1}";
        private static readonly string MsgCommitDefered = "UnitOfWork.Commit: defered method {0}";

        /// <summary>
        /// Constructor
        /// </summary>
        public UnitOfWork()
        {
            _added = new Dictionary<IAggregateRoot,
                                     IUnitOfWorkRepository>();
            _changed = new Dictionary<IAggregateRoot,
                                       IUnitOfWorkRepository>();
            _deleted = new Dictionary<IAggregateRoot,
                                       IUnitOfWorkRepository>();
            _invokeBodies = new List<InvokeBody>();
        }

        #region Implementation of IUnitOfWork

        ///<summary>
        /// 将一个FisObject注册为Added
        ///</summary>
        ///<param name="entity">fisobject</param>
        ///<param name="repository">该FisObject类型的Repository对象</param>
        public void RegisterAdded(IAggregateRoot entity, IUnitOfWorkRepository repository)
        {
            if (!this._added.ContainsKey(entity))
            {
                logger.DebugFormat(MsgRegisterAdded, entity.GetType(), entity.Key);
                this._added.Add(entity, repository);
                _execSeq.Add(entity);
            }
        }

        ///<summary>
        /// 将一个FisObject注册为Changed
        ///</summary>
        ///<param name="entity">fisobject</param>
        ///<param name="repository">该FisObject类型的Repository对象</param>
        public void RegisterChanged(IAggregateRoot entity, IUnitOfWorkRepository repository)
        {
            if (!this._changed.ContainsKey(entity))
            {
                logger.DebugFormat(MsgRegisterChanged, entity.GetType(), entity.Key);
                this._changed.Add(entity, repository);
                _execSeq.Add(entity);
            }
        }

        ///<summary>
        /// 将一个FisObject注册为Removed
        ///</summary>
        ///<param name="entity">fisobject</param>
        ///<param name="repository">该FisObject类型的Repository对象</param>
        public void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository repository)
        {
            if (!this._deleted.ContainsKey(entity))
            {
                logger.DebugFormat(MsgRegisterRemoved, entity.GetType(), entity.Key);
                this._deleted.Add(entity, repository);
                _execSeq.Add(entity);
            }
        }

        /// <summary>
        /// 注册延迟改库方法
        /// </summary>
        /// <param name="ivkBdy"></param>
        public void RegisterDeferMethods(InvokeBody ivkBdy)
        {
            if (ivkBdy != null && !_invokeBodies.Contains(ivkBdy))
            {
                logger.DebugFormat(MsgRegisterDeferMethods, ivkBdy.ToString());
                _invokeBodies.Add(ivkBdy);
                _execSeq.Add(ivkBdy);
            }
        }

        /// <summary>
        /// 注册对象间属性关联赋值请求
        /// </summary>
        /// <param name="setter"></param>
        public void RegisterSetterBetween(SetterBetween setter)
        {
            try
            {
                _execSeq.Add(setter);
            }
            catch(Exception)
            {
                throw;
            }
        }

        ///<summary>
        /// 提交所有修改结果
        ///</summary>
        public void Commit()
        {
            //TransactionOptions tsos = new TransactionOptions();
            //tsos.IsolationLevel = IsolationLevel.ReadCommitted;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, tsos))
            //{
                try
                {
                    SqlTransactionManager.Begin();

                    foreach (object obj in _execSeq)
                    {
                        if (obj is IAggregateRoot)
                        {
                            IAggregateRoot iarObj = (IAggregateRoot)obj;
                            if (this._added.ContainsKey(iarObj))
                            {
                                logger.DebugFormat(MsgCommitAdd, iarObj.GetType(), iarObj.Key);
                                this._added[iarObj].PersistNewItem(iarObj);
                            }
                            else if (this._changed.ContainsKey(iarObj))
                            {
                                logger.DebugFormat(MsgCommitChanged, iarObj.GetType(), iarObj.Key);
                                this._changed[iarObj].PersistUpdatedItem(iarObj);
                            }
                            else if (this._deleted.ContainsKey(iarObj))//2012-03-17 LiuDong 对于同一个FisObject在一个事务里先更后删考虑不足
                            {
                                logger.DebugFormat(MsgCommitDelete, iarObj.GetType(), iarObj.Key);
                                this._deleted[iarObj].PersistDeletedItem(iarObj);
                            }
                        }
                        else if (obj is InvokeBody)
                        {
                            InvokeBody ibdy = (InvokeBody)obj;
                            //2010-03-06  itc207031     Modify ITC-1122-0206
                            if (this._invokeBodies.Contains(ibdy))
                            {
                                logger.DebugFormat(MsgCommitDefered, ibdy.ToString());
                                InvokeBody.ExecuteOne(ibdy);
                            }
                        }
                        else //SetterBetween
                        {
                            SetterBetween setter = (SetterBetween)obj;
                            PropertyInfo propi_orig = setter.Origin.GetType().GetProperty(setter.OriginProperty, BindingFlags.Instance | BindingFlags.Public);
                            PropertyInfo propi_dest = setter.Destination.GetType().GetProperty(setter.DestinationProperty, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty);
                            if (propi_orig != null && propi_dest != null)
                            {
                                MethodInfo mi_orig = propi_orig.GetGetMethod();
                                MethodInfo mi_dest = propi_dest.GetSetMethod(true);
                                if (mi_orig != null && mi_dest != null)
                                {
                                    mi_dest.Invoke(setter.Destination, new object[] { mi_orig.Invoke(setter.Origin, null) });
                                }
                            }
                        }
                    }
                    SqlTransactionManager.Commit();
                }
                catch (Exception e)
                {
                    SqlTransactionManager.Rollback();
                    throw e;
                }
                finally
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }
            //    scope.Complete();
            //}

            this._deleted.Clear();
            this._added.Clear();
            this._changed.Clear();
            this._invokeBodies.Clear();
            //2010-03-06  itc207031     Modify ITC-1122-0206
            this._execSeq.Clear();
            //2010-03-06  itc207031     Modify ITC-1122-0206
        }

        #endregion
    }
}