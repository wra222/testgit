using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using IMES.Infrastructure.Utility.Cache;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Configuration;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: ModelTolerance相关
    /// </summary>
    public class ModelToleranceRepository : BaseRepository<ModelTolerance>, IModelToleranceRepository, ICache
    {
        #region Cache
        private static CacheManager _cache_real = null;
        private static CacheManager _cache
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = CacheFactory.GetCacheManager("ModelToleranceCache");
                return _cache_real;
            }
        }
        private static object _syncObj_cache = new object();
        #endregion

        #region Overrides of BaseRepository<ModelTolerance>

        protected override void PersistNewItem(ModelTolerance item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertModelWeight(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(ModelTolerance item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateModelWeight(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(ModelTolerance item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteModelWeight(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region IRepository<ModelTolerance> Members

        public ModelTolerance Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                ModelTolerance ret = Find_Cache(key);
                if (ret == null)
                {
                    ret = Find_DB(key);

                    if (ret != null)
                    {
                        lock (_syncObj_cache)
                        {
                            if (!_cache.Contains((string)ret.Key))
                            {
                                AddToCache((string)ret.Key, ret);
                            }
                        }
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ModelTolerance> FindAll()
        {
            try
            {
                IList<ModelTolerance> ret = new List<ModelTolerance>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeightTolerance));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ModelTolerance item = new ModelTolerance();
                            item.CartonTolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_CartonTolerance]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Cdt]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Editor]);
                            item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Model]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Udt]);
                            item.UnitTolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_UnitTolerance]);
                            item.Tracker.Clear();
                            ret.Add(item);
                        }
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Add(ModelTolerance item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        public void Remove(ModelTolerance item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        public void Update(ModelTolerance item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region . Inners .

        private ModelTolerance Find_DB(object key)
        {
            try
            {
                ModelTolerance ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ModelWeightTolerance cond = new _Schema.ModelWeightTolerance();
                        cond.Model = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeightTolerance), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Model].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new ModelTolerance();
                        ret.CartonTolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_CartonTolerance]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Editor]);
                        ret.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Model]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_Udt]);
                        ret.UnitTolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeightTolerance.fn_UnitTolerance]);
                        ret.Tracker.Clear();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ModelTolerance Find_Cache(object key)
        {
            try
            {
                ModelTolerance ret = null;
                lock (_syncObj_cache)
                {
                    if (_cache.Contains((string)key))
                        ret = (ModelTolerance)_cache[(string)key];
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertModelWeight(ModelTolerance item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeightTolerance));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_CartonTolerance].Value = item.CartonTolerance;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_UnitTolerance].Value = item.UnitTolerance;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateModelWeight(ModelTolerance item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeightTolerance));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_CartonTolerance].Value = item.CartonTolerance;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_UnitTolerance].Value = item.UnitTolerance;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteModelWeight(ModelTolerance item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeightTolerance));
                    }
                }
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Model].Value = item.Model;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ICache Members

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.ModelTolerance);
        }

        public void ProcessItem(IMES.DataModel.CacheUpdateInfo item)
        {
            if (item.Type == IMES.DataModel.CacheType.ModelTolerance)
                LoadOneCache(item.Item);
        }

        public void ClearCache()
        {
            lock (_syncObj_cache)
            {
                _cache.Flush();
            }
        }

        private void LoadOneCache(string pk)
        {
            lock (_syncObj_cache)
            {
                if (_cache.Contains(pk))
                {
                    _cache.Remove(pk);
                }

                #region For YWH
                /*
                ModelTolerance mdltlr = this.Find_DB(pk);
                if (mdltlr != null)
                {
                    AddToCache(pk, mdltlr);
                }
                */
                #endregion
            }
        }

        private IMES.DataModel.CacheUpdateInfo GetACacheSignal(string pk)
        {
            IMES.DataModel.CacheUpdateInfo ret = new IMES.DataModel.CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = IMES.DataModel.CacheType.ModelTolerance;
            ret.Item = pk;
            return ret;
        }

        private void AddToCache(string key, object obj)
        {
            _cache.Add(key, obj, CacheItemPriority.Normal, new ModelToleranceRefreshAction(), new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_ModelToleranceCache"].ToString()))));
        }

        [Serializable]
        private class ModelToleranceRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
            }
        }

        #endregion

        #region For Maintain

        public ModelTolerance FindModelWeightToleranceByCustomer(string customer)
        {
            //select * from IMES_PAK..ModelWeightTolerance where Model=? 
            try
            {
                return Find_DB(customer);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<ModelTolerance> FindModelWeightToleranceListByFamily(string family)
        {
            //select * from IMES_PAK..ModelWeightTolerance a 
            //     join IMES_GetData..Model b 
            //          on a.Model=b.Model 
            //where b.Family=?
            try
            {
                IList<ModelTolerance> ret = new List<ModelTolerance>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.ModelWeightTolerance);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Model);
                        tf2.subDBCalalog = _Schema.SqlHelper.DB_BOM;
                        _Schema.Model cond = new _Schema.Model();
                        cond.Family = family;
                        tf2.equalcond = cond;
                        tf2.ToGetFieldNames = null;

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(new _Schema.TableConnectionItem[] { 
                                                                            new _Schema.TableConnectionItem(tf1, _Schema.ModelWeightTolerance.fn_Model, tf2, _Schema.Model.fn_model) });

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.ModelWeightTolerance.fn_Model));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Model.fn_Family)].Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ModelTolerance item = new ModelTolerance();
                        item.CartonTolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelWeightTolerance.fn_CartonTolerance)]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.ModelWeightTolerance.fn_Cdt)]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.ModelWeightTolerance.fn_Editor)]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.ModelWeightTolerance.fn_Model)]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.ModelWeightTolerance.fn_Udt)]);
                        item.UnitTolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.ModelWeightTolerance.fn_UnitTolerance)]);
                        item.Tracker.Clear();
                        ret.Add(item);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteModelWeightToleranceByModel(string model)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(model));

                ModelTolerance cond = new ModelTolerance();
                cond.Model = model;
                this.PersistDeleteModelWeight(cond);

                SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        public void AddModelWeightTolerance(ModelTolerance modelWeightTolerance)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(modelWeightTolerance.Model));

                this.PersistInsertModelWeight(modelWeightTolerance);

                SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        public void UpdateModelWeightTolerance(ModelTolerance modelWeightTolerance, string model)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(model));
                if (model != modelWeightTolerance.Model)
                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal(modelWeightTolerance.Model));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ModelWeightTolerance cond = new _Schema.ModelWeightTolerance();
                        cond.Model = model;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeightTolerance), null, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ModelWeightTolerance.fn_Model].Value = model;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelWeightTolerance.fn_Model)].Value = modelWeightTolerance.Model;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelWeightTolerance.fn_CartonTolerance)].Value = modelWeightTolerance.CartonTolerance;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelWeightTolerance.fn_Editor)].Value = modelWeightTolerance.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelWeightTolerance.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelWeightTolerance.fn_UnitTolerance)].Value = modelWeightTolerance.UnitTolerance;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        #region Defered

        public void DeleteModelWeightToleranceByModelDefered(IUnitOfWork uow, string model)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model);
        }

        public void AddModelWeightToleranceDefered(IUnitOfWork uow, ModelTolerance modelWeightTolerance)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), modelWeightTolerance);
        }

        public void UpdateModelWeightToleranceDefered(IUnitOfWork uow, ModelTolerance modelWeightTolerance, string model)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), modelWeightTolerance, model);
        }

        #endregion

        #endregion
    }
}
