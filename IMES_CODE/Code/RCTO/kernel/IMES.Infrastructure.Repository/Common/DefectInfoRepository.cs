using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Defect;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
//using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.Utility.Cache;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.DataModel;
using fons = IMES.FisObject.Common.Defect;
//

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: DefectInfo相关
    /// </summary>
    public class DefectInfoRepository : BaseRepository<fons.DefectInfo>, IDefectInfoRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        private static IDictionary<int, fons.DefectInfo> _cache = new Dictionary<int, fons.DefectInfo>();
        private static IDictionary<string, IList<int>> _byWhateverIndex = new Dictionary<string, IList<int>>();
        private static string preStr1 = _Schema.Func.MakeKeyForIdxPre(_Schema.DefectInfo.fn_Type);
        private static string preStr2 = _Schema.Func.MakeKeyForIdxPre(_Schema.DefectInfo.fn_Type, _Schema.DefectInfo.fn_Code);
        private static string preStr3 = _Schema.Func.MakeKeyForIdxPre(_Schema.DefectInfo.fn_Type, _Schema.DefectInfo.fn_CustomerID);
        private static object _syncObj_cache = new object();
        #endregion

        #region Overrides of BaseRepository<DefectInfo>

        protected override void PersistNewItem(fons.DefectInfo item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertDefectInfo(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(fons.DefectInfo item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateDefectInfo(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(fons.DefectInfo item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteDefectInfo(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<DefectInfo>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.DefectInfo Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                fons.DefectInfo ret = Find_Cache(key);
                if (ret == null)
                {
                    ret = Find_DB(key);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public override IList<fons.DefectInfo> FindAll()
        {
            try
            {
                if (!IsCached())
                    return FindAll_DB();

                IList<fons.DefectInfo> ret = FindAll_Cache();
                if (ret == null || ret.Count < 1)
                {
                    ret = FindAll_DB();
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        public override void Add(fons.DefectInfo item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(fons.DefectInfo item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons.DefectInfo item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IDefectInfoRepository

        public IList<IMES.DataModel.CauseInfo> GetCauseList(string customer, string stage)
        {
            try
            {
                var ret = this.FindByType<IMES.DataModel.CauseInfo>(stage == "SA" ? fons.DefectInfo.DefectInfoType.SaCause : fons.DefectInfo.DefectInfoType.FaCause, customer);
                return (from item in ret orderby item.id select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.MajorPartInfo> GetMajorPartList(string customer)
        {
            try
            {
                var ret = this.FindByType<IMES.DataModel.MajorPartInfo>(fons.DefectInfo.DefectInfoType.MajorPart, customer);
                return (from item in ret orderby item.id select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.ComponentInfo> GetComponentList(string customer)
        {
            try
            {
                var ret = this.FindByType<IMES.DataModel.ComponentInfo>(fons.DefectInfo.DefectInfoType.Component, customer);
                return (from item in ret orderby item.id select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.ObligationInfo> GetObligationList(string customer)
        {
            try
            {
                var ret = this.FindByType<IMES.DataModel.ObligationInfo>(fons.DefectInfo.DefectInfoType.Obligation, customer);
                return (from item in ret orderby item.id select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.ResponsibilityInfo> GetResponsibilityList(string customer)
        {
            try
            {
                return this.FindByType<IMES.DataModel.ResponsibilityInfo>(fons.DefectInfo.DefectInfoType.Responsibility, customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel._4MInfo> Get4MList(string customer)
        {
            try
            {
                return this.FindByType<IMES.DataModel._4MInfo>(fons.DefectInfo.DefectInfoType.M4, customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.CoverInfo> GetCoverList(string customer)
        {
            try
            {
                return this.FindByType<IMES.DataModel.CoverInfo>(fons.DefectInfo.DefectInfoType.Cover, customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.UncoverInfo> GetUncoverList(string customer)
        {
            try
            {
                return this.FindByType<IMES.DataModel.UncoverInfo>(fons.DefectInfo.DefectInfoType.Uncover, customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.TrackingStatusInfo> GetTrackingStatusList(string customer)
        {
            try
            {
                return this.FindByType<IMES.DataModel.TrackingStatusInfo>(fons.DefectInfo.DefectInfoType.TrackingStatus, customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.DistributionInfo> GetDistributionList(string customer)
        {
            try
            {
                return this.FindByType<IMES.DataModel.DistributionInfo>(fons.DefectInfo.DefectInfoType.Distribution, customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.DataModel.MarkInfo> GetMarkList(string customer)
        {
            try
            {
                return this.FindByType<IMES.DataModel.MarkInfo>(fons.DefectInfo.DefectInfoType.Mark, customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType(string type)
        {
            try
            {
                if (!IsCached())
                    return FindDefectInfoesByType_DB(type);

                IList<IMES.DataModel.DefectInfo> ret = FindDefectInfoesByType_Cache(type);
                if (ret == null || ret.Count < 1)
                {
                    ret = FindDefectInfoesByType_DB(type);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType(string type, string customer)
        {
            try
            {
                if (!IsCached())
                    return FindDefectInfoesByType_DB(type, customer);

                IList<IMES.DataModel.DefectInfo> ret = FindDefectInfoesByType_Cache(type, customer);
                if (ret == null || ret.Count < 1)
                {
                    ret = FindDefectInfoesByType_DB(type, customer);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string TransToDesc(string type, string code)
        {
            try
            {
                if (!IsCached())
                    return TransToDesc_DB(type, code);

                string ret = TransToDesc_Cache(type, code);
                if (ret == null || ret.Trim() == string.Empty)
                {
                    ret = TransToDesc_DB(type, code);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
       

        #endregion

        #region . Inners .

        private fons.DefectInfo Find_DB(object key)
        {
            try
            {
                fons.DefectInfo ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectInfo cond = new _Schema.DefectInfo();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectInfo.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.DefectInfo();
                        ret.Code = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Code]);
                        ret.CustomerId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_CustomerID]);
                        ret.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]);
                        ret.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Type]);
                        ret.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_ID]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Udt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Editor]);
                        ret.EngDescription = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_engDescr]);
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

        private fons.DefectInfo Find_Cache(object key)
        {
            try
            {
                fons.DefectInfo ret = null;
                lock (_syncObj_cache)
                {
                    if (_cache.ContainsKey((int)key))
                        ret = _cache[(int)key];
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons.DefectInfo> FindAll_DB()
        {
            try
            {
                IList<fons.DefectInfo> ret = new List<fons.DefectInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.DefectInfo item = new fons.DefectInfo();
                        item.Code = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Code]);
                        item.CustomerId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_CustomerID]);
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Type]);
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_ID]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Udt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Editor]);
                        item.EngDescription = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_engDescr]);
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

        private IList<fons.DefectInfo> FindAll_Cache()
        {
            try
            {
                lock (_syncObj_cache)
                {
                    return _cache.Values.ToList<fons.DefectInfo>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertDefectInfo(fons.DefectInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.DefectInfo.fn_Code].Value = item.Code;
                sqlCtx.Params[_Schema.DefectInfo.fn_CustomerID].Value = item.CustomerId;
                sqlCtx.Params[_Schema.DefectInfo.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.DefectInfo.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.DefectInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.DefectInfo.fn_engDescr].Value = item.EngDescription;
                item.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateDefectInfo(fons.DefectInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo));
                    }
                }
                sqlCtx.Params[_Schema.DefectInfo.fn_ID].Value = item.Id;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.DefectInfo.fn_Code].Value = item.Code;
                sqlCtx.Params[_Schema.DefectInfo.fn_CustomerID].Value = item.CustomerId;
                sqlCtx.Params[_Schema.DefectInfo.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.DefectInfo.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.DefectInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.DefectInfo.fn_engDescr].Value = item.EngDescription;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteDefectInfo(fons.DefectInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo));
                    }
                }
                sqlCtx.Params[_Schema.DefectInfo.fn_ID].Value = item.Id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<T> FindByType<T>(string type, string customer)
        {
            try
            {
                if (!IsCached())
                    return FindByType_DB<T>(type, customer);

                List<T> ret = FindByType_Cache<T>(type, customer);
                if (ret == null || ret.Count < 1)
                {
                    ret = FindByType_DB<T>(type, customer);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<T> FindByType_DB<T>(string type, string customer)
        {
            try
            {
                List<T> retu = new List<T>();
                Type tp = typeof(T);

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectInfo cond = new _Schema.DefectInfo();
                        cond.Type = type;
                        cond.CustomerID = customer;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectInfo.fn_Type].Value = type;
                sqlCtx.Params[_Schema.DefectInfo.fn_CustomerID].Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        T obj = (T)Activator.CreateInstance(tp, new object[] { GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Code]), GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]) });

                        //tp.GetField("id", BindingFlags.Instance | BindingFlags.Public).SetValue(obj,GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Code]));
                        //tp.GetField("friendlyName", BindingFlags.Instance | BindingFlags.Public).SetValue(obj, GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]));

                        retu.Add(obj);
                    }
                }
                return retu;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<T> FindByType_Cache<T>(string type, string customer)
        {
            try
            {
                List<T> retu = new List<T>();
                Type tp = typeof(T);
                string key = _Schema.Func.MakeKeyForIdx(preStr3, type, customer);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (int pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                            {
                                fons.DefectInfo di = _cache[pk];
                                T obj = (T)Activator.CreateInstance(tp, new object[] { di.Code, di.Description });
                                retu.Add(obj);
                            }
                        }
                    }
                }
                return retu;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType_DB(string type)
        {
            try
            {
                IList<IMES.DataModel.DefectInfo> ret = new List<IMES.DataModel.DefectInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectInfo cond = new _Schema.DefectInfo();
                        cond.Type = type;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectInfo.fn_Type].Value = type;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.DefectInfo obj = new IMES.DataModel.DefectInfo();
                        obj.description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]);
                        obj.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]);
                        obj.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Code]);
                        ret.Add(obj);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType_Cache(string type)
        {
            try
            {
                List<IMES.DataModel.DefectInfo> ret = new List<IMES.DataModel.DefectInfo>();
                string key = _Schema.Func.MakeKeyForIdx(preStr1, type);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (int pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                            {
                                fons.DefectInfo di = _cache[pk];
                                ret.Add(Converter(di));
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

        private IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType_DB(string type, string customer)
        {
            try
            {
                IList<IMES.DataModel.DefectInfo> ret = new List<IMES.DataModel.DefectInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectInfo cond = new _Schema.DefectInfo();
                        cond.Type = type;
                        cond.CustomerID = customer;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectInfo.fn_Type].Value = type;
                sqlCtx.Params[_Schema.DefectInfo.fn_CustomerID].Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.DefectInfo obj = new IMES.DataModel.DefectInfo();
                        obj.description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]);
                        obj.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]);
                        obj.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Code]);
                        ret.Add(obj);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType_Cache(string type, string customer)
        {
            try
            {
                List<IMES.DataModel.DefectInfo> ret = new List<IMES.DataModel.DefectInfo>();
                string key = _Schema.Func.MakeKeyForIdx(preStr3, type, customer);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (int pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                            {
                                fons.DefectInfo di = _cache[pk];
                                ret.Add(Converter(di));
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

        private string TransToDesc_DB(string type, string code)
        {
            try
            {
                string ret = string.Empty;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectInfo cond = new _Schema.DefectInfo();
                        cond.Type = type;
                        cond.Code = code;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectInfo), null, new List<string>() { _Schema.DefectInfo.fn_Description }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectInfo.fn_Type].Value = type;
                sqlCtx.Params[_Schema.DefectInfo.fn_Code].Value = code;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectInfo.fn_Description]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string TransToDesc_Cache(string type, string code)
        {
            try
            {
                string ret = null;
                string key = _Schema.Func.MakeKeyForIdx(preStr2, type, code);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (int pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                            {
                                ret = _cache[pk].Description;
                                break;
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

        private IMES.DataModel.DefectInfo Converter(fons.DefectInfo di)
        {
            IMES.DataModel.DefectInfo dim = new IMES.DataModel.DefectInfo();
            if (di != null)
            {
                dim.description = di.Description;
                dim.friendlyName = di.Description;
                dim.id = di.Code;
            }
            return dim;
        }

        #endregion

        #region ICache Members

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.DefectInfo);
        }

        public void ProcessItem(IMES.DataModel.CacheUpdateInfo item)
        {
            LoadAllCache();
        }

        private void LoadAllCache()
        {
            IList<fons.DefectInfo> dfctis = this.FindAll_DB();
            if (dfctis != null && dfctis.Count > 0)
            {
                lock (_syncObj_cache)
                {
                    _cache.Clear();
                    _byWhateverIndex.Clear();

                    foreach (fons.DefectInfo dfcti in dfctis)
                    {
                        _cache.Add(dfcti.Id, dfcti);

                        //Regist index
                        Regist(dfcti.Id, _Schema.Func.MakeKeyForIdx(preStr1, dfcti.Type));

                        //Regist index
                        Regist(dfcti.Id, _Schema.Func.MakeKeyForIdx(preStr2, dfcti.Type, dfcti.Code));

                        //Regist index
                        Regist(dfcti.Id, _Schema.Func.MakeKeyForIdx(preStr3, dfcti.Type, dfcti.CustomerId));
                    }
                }
            }
        }

        private void Regist(int pk, string key)
        {
            IList<int> PKs = null;
            try
            {
                PKs = _byWhateverIndex[key];
            }
            catch (KeyNotFoundException)
            {
                PKs = new List<int>();
                _byWhateverIndex.Add(key, PKs);
            }
            if (!PKs.Contains(pk))
                PKs.Add(pk);
        }

        private IMES.DataModel.CacheUpdateInfo GetACacheSignal()
        {
            IMES.DataModel.CacheUpdateInfo ret = new IMES.DataModel.CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = IMES.DataModel.CacheType.DefectInfo;
            return ret;
        }

        #endregion

        #region IDefectInfoRepository Members

        public IList<IMES.DataModel.DefectInfoDef> GetRepairInfoByCondition(string type)
        {
            try
            {
                IList<IMES.DataModel.DefectInfoDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DefectInfo cond = new _Metas.DefectInfo();
                        cond.type = type;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.DefectInfo>(tk, null, null, new ConditionCollection<_Metas.DefectInfo>(new EqualCondition<_Metas.DefectInfo>(cond)), _Metas.DefectInfo.fn_code);
                    }
                }
                sqlCtx.Param(_Metas.DefectInfo.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.DefectInfo, IMES.DataModel.DefectInfoDef, IMES.DataModel.DefectInfoDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddRepairInfoItem(IMES.DataModel.DefectInfoDef item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::DefectInfo>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::DefectInfo, DefectInfoDef>(sqlCtx, item);

                sqlCtx.Param(mtns::DefectInfo.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::DefectInfo.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistDefectInfo(string type, string code)
        {
            try
            {
                bool ret = false;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DefectInfo cond = new _Metas.DefectInfo();
                        cond.type = type;
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.DefectInfo>(tk, "COUNT", new string[] { _Metas.DefectInfo.fn_id }, new ConditionCollection<_Metas.DefectInfo>(new EqualCondition<_Metas.DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DefectInfo.fn_type).Value = type;
                sqlCtx.Param(_Metas.DefectInfo.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateRepairInfoItem(IMES.DataModel.DefectInfoDef item, string code, string type)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DefectInfo cond = new _Metas.DefectInfo();
                        cond.code = code;
                        cond.type = type;

                        _Metas.DefectInfo setv = FuncNew.SetColumnFromField<_Metas.DefectInfo, IMES.DataModel.DefectInfoDef>(item, _Metas.DefectInfo.fn_id, _Metas.DefectInfo.fn_cdt);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.DefectInfo>(tk, new SetValueCollection<_Metas.DefectInfo>(new CommonSetValue<_Metas.DefectInfo>(setv)), new ConditionCollection<_Metas.DefectInfo>(new EqualCondition<_Metas.DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DefectInfo.fn_code).Value = code;
                sqlCtx.Param(_Metas.DefectInfo.fn_type).Value = type;

                sqlCtx = FuncNew.SetColumnFromField<_Metas.DefectInfo, IMES.DataModel.DefectInfoDef>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.DefectInfo.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveRepairInfoItem(string code, string type)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::DefectInfo cond = new mtns::DefectInfo();
                        cond.code = code;
                        cond.type = type;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::DefectInfo>(tk, new ConditionCollection<mtns::DefectInfo>(new EqualCondition<mtns::DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(mtns::DefectInfo.fn_code).Value = code;
                sqlCtx.Param(mtns::DefectInfo.fn_type).Value = type;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfRepairInfoByTypeAndCode(string type, string code)
        {
            try
            {
                int ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DefectInfo cond = new _Metas.DefectInfo();
                        cond.type = type;
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.DefectInfo>(tk, "COUNT", new string[] { _Metas.DefectInfo.fn_id }, new ConditionCollection<_Metas.DefectInfo>(new EqualCondition<_Metas.DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DefectInfo.fn_type).Value = type;
                sqlCtx.Param(_Metas.DefectInfo.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectInfoDef> GetDefectInfoByTypeAndCode(string type, string code)
        {
            try
            {
                IList<IMES.DataModel.DefectInfoDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DefectInfo cond = new _Metas.DefectInfo();
                        cond.type = type;
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.DefectInfo>(tk, null, null, new ConditionCollection<_Metas.DefectInfo>(new EqualCondition<_Metas.DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DefectInfo.fn_type).Value = type;
                sqlCtx.Param(_Metas.DefectInfo.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.DefectInfo, IMES.DataModel.DefectInfoDef, IMES.DataModel.DefectInfoDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDefectInfo()
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.DefectInfo>(tk, "DISTINCT", new string[] { mtns.DefectInfo.fn_code }, new ConditionCollection<mtns.DefectInfo>(), mtns.DefectInfo.fn_code);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.DefectInfo.fn_code));
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

        #region . Defered .

        public void AddRepairInfoItemDefered(IUnitOfWork uow, IMES.DataModel.DefectInfoDef item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateRepairInfoItemDefered(IUnitOfWork uow, IMES.DataModel.DefectInfoDef item, string code, string type)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, code, type);
        }

        public void RemoveRepairInfoItemDefered(IUnitOfWork uow, string code, string type)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), code, type);
        }

        /// <summary>
        ///  update DefectInfo where Id=@Id
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        public void UpdateRepairInfoItemById(DefectInfoDef item, int id)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DefectInfo cond = new _Metas.DefectInfo();
                        cond.id = id;                       

                        _Metas.DefectInfo setv = FuncNew.SetColumnFromField<_Metas.DefectInfo, IMES.DataModel.DefectInfoDef>(item, _Metas.DefectInfo.fn_id, _Metas.DefectInfo.fn_cdt);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.DefectInfo>(tk, new SetValueCollection<_Metas.DefectInfo>(new CommonSetValue<_Metas.DefectInfo>(setv)), new ConditionCollection<_Metas.DefectInfo>(new EqualCondition<_Metas.DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DefectInfo.fn_id).Value = id;              

                sqlCtx = FuncNew.SetColumnFromField<_Metas.DefectInfo, IMES.DataModel.DefectInfoDef>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.DefectInfo.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete DefectInfo where Id=@Id
        /// </summary>
        /// <param name="id"></param>
        public void RemoveRepairInfoItemById(int id)
        {
             try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::DefectInfo cond = new mtns::DefectInfo();
                        cond.id = id;
                       
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::DefectInfo>(tk, new ConditionCollection<mtns::DefectInfo>(new EqualCondition<mtns::DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(mtns::DefectInfo.fn_id).Value = id;
               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistDefectInfo(string type, string code, string customerId)
        {
            try
            {
                bool ret = false;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DefectInfo cond = new _Metas.DefectInfo();
                        cond.type = type;
                        cond.code = code;
                        cond.customerID = customerId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.DefectInfo>(tk, "COUNT", new string[] { _Metas.DefectInfo.fn_id }, new ConditionCollection<_Metas.DefectInfo>(new EqualCondition<_Metas.DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DefectInfo.fn_type).Value = type;
                sqlCtx.Param(_Metas.DefectInfo.fn_code).Value = code;
                sqlCtx.Param(_Metas.DefectInfo.fn_customerID).Value = customerId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion
    }
}
