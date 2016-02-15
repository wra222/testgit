using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;
using IMES.Infrastructure.Util;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.Utility.Cache;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.PAK.StandardWeight;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: ModelWeight相关
    /// </summary>
    public class ModelWeightRepository : BaseRepository<fons.ModelWeight>, IModelWeightRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        private static CacheManager _cache_real = null;
        private static CacheManager _cache
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = CacheFactory.GetCacheManager("ModelWeightCache");
                return _cache_real;
            }
        }
        private static object _syncObj_cache = new object();
        #endregion

        #region Overrides of BaseRepository<ModelWeight>

        protected override void PersistNewItem(fons.ModelWeight item)
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

        protected override void PersistUpdatedItem(fons.ModelWeight item)
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

        protected override void PersistDeletedItem(fons.ModelWeight item)
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

        #region IRepository<ModelWeight> Members

        public fons.ModelWeight Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                fons.ModelWeight ret = Find_Cache(key);
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

        public IList<fons.ModelWeight> FindAll()
        {
            try
            {
                IList<fons.ModelWeight> ret = new List<fons.ModelWeight>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeight));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons.ModelWeight item = new fons.ModelWeight();
                            item.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_CartonWeight]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Cdt]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Editor]);
                            item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Model]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Udt]);
                            item.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_UnitWeight]);
                            item.SendStatus = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_SendStatus]);
                            item.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Remark]);
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

        public void Add(fons.ModelWeight item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        public void Remove(fons.ModelWeight item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        public void Update(fons.ModelWeight item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region . Inners .

        private fons.ModelWeight Find_DB(object key)
        {
            try
            {
                fons.ModelWeight ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ModelWeight cond = new _Schema.ModelWeight();
                        cond.Model = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeight), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ModelWeight.fn_Model].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.ModelWeight();
                        ret.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_CartonWeight]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Editor]);
                        ret.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Model]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Udt]);
                        ret.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_UnitWeight]);
                        ret.SendStatus = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_SendStatus]);
                        ret.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelWeight.fn_Remark]);
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

        private fons.ModelWeight Find_Cache(object key)
        {
            try
            {
                fons.ModelWeight ret = null;
                lock (_syncObj_cache)
                {
                    if (_cache.Contains((string)key))
                        ret = (fons.ModelWeight)_cache[(string)key];
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertModelWeight(fons.ModelWeight item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeight));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelWeight.fn_CartonWeight].Value = item.CartonWeight;
                sqlCtx.Params[_Schema.ModelWeight.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelWeight.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.ModelWeight.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.ModelWeight.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelWeight.fn_UnitWeight].Value = item.UnitWeight;
                sqlCtx.Params[_Schema.ModelWeight.fn_SendStatus].Value = item.SendStatus;
                sqlCtx.Params[_Schema.ModelWeight.fn_Remark].Value = item.Remark;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateModelWeight(fons.ModelWeight item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeight));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelWeight.fn_CartonWeight].Value = item.CartonWeight;
                sqlCtx.Params[_Schema.ModelWeight.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.ModelWeight.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.ModelWeight.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelWeight.fn_UnitWeight].Value = item.UnitWeight;
                sqlCtx.Params[_Schema.ModelWeight.fn_SendStatus].Value = item.SendStatus;
                sqlCtx.Params[_Schema.ModelWeight.fn_Remark].Value = item.Remark;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteModelWeight(fons.ModelWeight item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelWeight));
                    }
                }
                sqlCtx.Params[_Schema.ModelWeight.fn_Model].Value = item.Model;
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
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.ModelWeight);
        }

        public void ProcessItem(IMES.DataModel.CacheUpdateInfo item)
        {
            if (item.Type == IMES.DataModel.CacheType.ModelWeight)
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
                fons.ModelWeight mdlwgt = this.Find_DB(pk);
                if (mdlwgt != null)
                {
                    AddToCache(pk, mdlwgt);
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
            ret.Type = IMES.DataModel.CacheType.ModelWeight;
            ret.Item = pk;
            return ret;
        }

        private void AddToCache(string key, object obj)
        {
            _cache.Add(key, obj, CacheItemPriority.Normal, new ModelWeightRefreshAction(), new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_ModelWeightCache"].ToString()))));
        }

        [Serializable]
        private class ModelWeightRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
            }
        }

        #endregion

        #region IModelWeightRepository Members

        public DataTable GetModelWeightItem(string model)
        {
            try
            {
                DataTable ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::ModelWeight cond = new mtns::ModelWeight();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::ModelWeight>(tk, null, new string[] { mtns::ModelWeight.fn_model, mtns::ModelWeight.fn_unitWeight }, new ConditionCollection<mtns::ModelWeight>(new EqualCondition<mtns::ModelWeight>(cond)));
                    }
                }
                sqlCtx.Param(mtns::ModelWeight.fn_model).Value = model;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateModelWeight(IMES.DataModel.ModelWeightInfo setValue, IMES.DataModel.ModelWeightInfo condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::ModelWeight cond = mtns::FuncNew.SetColumnFromField<mtns::ModelWeight, IMES.DataModel.ModelWeightInfo>(condition);
                mtns::ModelWeight setv = mtns::FuncNew.SetColumnFromField<mtns::ModelWeight, IMES.DataModel.ModelWeightInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::ModelWeight>(new mtns::SetValueCollection<mtns::ModelWeight>(new mtns::CommonSetValue<mtns::ModelWeight>(setv)), new mtns::ConditionCollection<mtns::ModelWeight>(new mtns::EqualCondition<mtns::ModelWeight>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::ModelWeight, IMES.DataModel.ModelWeightInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::ModelWeight, IMES.DataModel.ModelWeightInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::ModelWeight.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void UpdateModelWeightDefered(IUnitOfWork uow, IMES.DataModel.ModelWeightInfo setValue, IMES.DataModel.ModelWeightInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        #endregion

        #endregion
    }
}
