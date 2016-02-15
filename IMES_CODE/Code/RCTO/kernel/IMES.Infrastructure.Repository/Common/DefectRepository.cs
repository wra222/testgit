using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Defect;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure.Utility;
using IMES.DataModel;
using IMES.Infrastructure.Utility.Cache;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
//

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Defect相关
    /// </summary>
    public class DefectRepository : BaseRepository<Defect>, IDefectRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        private static IDictionary<string, Defect> _cache = new Dictionary<string, Defect>();
        private static IDictionary<string, IList<string>> _byWhateverIndex = new Dictionary<string, IList<string>>();
        private static string preStr1 = _Schema.Func.MakeKeyForIdxPre(_Schema.DefectCode.fn_Type);
        private static object _syncObj_cache = new object();
        #endregion

        //private static IDefectInfoRepository _dfctiRepository = null;
        //private static IDefectInfoRepository DefectInfoRepository
        //{
        //    get
        //    {
        //        if (_dfctiRepository == null)
        //            _dfctiRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
        //        return _dfctiRepository;
        //    }
        //}

        #region Overrides of BaseRepository<Defect>

        protected override void PersistNewItem(Defect item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertDefect(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(Defect item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateDefect(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(Defect item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteDefect(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Defect>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override Defect Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                Defect ret = Find_Cache(key);
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
        public override IList<Defect> FindAll()
        {
            try
            {
                if (!IsCached())
                    return FindAll_DB();

                IList<Defect> ret = FindAll_Cache();
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
        public override void Add(Defect item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(Defect item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(Defect item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IDefectRepository

        public IList<IMES.DataModel.DefectInfo> GetDefectList(string type)
        {
            try
            {
                IList<IMES.DataModel.DefectInfo> ret = new List<IMES.DataModel.DefectInfo>();
                IList<Defect> val = this.FindByType(type);
                if (val != null && val.Count > 0)
                {
                    foreach (Defect entry in val)
                    {
                        IMES.DataModel.DefectInfo newItem = new IMES.DataModel.DefectInfo();
                        newItem.description = entry.Descr;
                        newItem.friendlyName = entry.Descr;
                        newItem.id = entry.DefectCode;
                        ret.Add(newItem);
                    }
                }
                return (from item in ret orderby item.id select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMES.DataModel.DefectInfo GetDefectInfo(string defectId)
        {
            try
            {
                IMES.DataModel.DefectInfo ret = new IMES.DataModel.DefectInfo();
                Defect val = this.Find(defectId);
                if (val != null)
                {
                    ret.description = val.Descr;
                    ret.friendlyName = val.Descr;
                    ret.id = val.DefectCode;
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.SubDefectInfo> GetSubDefectList()
        {
            try
            {
                return GetSubDefectList("QCSUB");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.SubDefectInfo> GetSubDefectList(string type)
        {
            try
            {
                IList<IMES.DataModel.SubDefectInfo> ret = new List<IMES.DataModel.SubDefectInfo>();
                IList<Defect> val = this.FindByType(type);
                if (val != null && val.Count > 0)
                {
                    foreach (Defect entry in val)
                    {
                        IMES.DataModel.SubDefectInfo newItem = new IMES.DataModel.SubDefectInfo();
                        newItem.friendlyName = entry.Descr;
                        newItem.id = entry.DefectCode;
                        ret.Add(newItem);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string TransToDesc(string code)
        {
            try
            {
                string ret = string.Empty;
                Defect dfct = this.Find(code);
                if (dfct != null && dfct.Descr != null)
                {
                    ret = dfct.Descr.Trim();
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIfIqcCauseExist(IqcCause1Info condition)
        {
            try
            {
                bool ret = false;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                IqcCause1 cond = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<IqcCause1>("COUNT", new string[] { IqcCause1.fn_id }, new ConditionCollection<IqcCause1>(new EqualCondition<IqcCause1>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void UpdateUDTofIqcCause(IqcCause1Info setValue, IqcCause1Info condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                IqcCause1 cond = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(condition);
                IqcCause1 setv = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<IqcCause1>(new SetValueCollection<IqcCause1>(new CommonSetValue<IqcCause1>(setv)), new ConditionCollection<IqcCause1>(new EqualCondition<IqcCause1>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.IqcCause1.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddIqcCause(IqcCause1Info item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<IqcCause1>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(sqlCtx, item);

                //sqlCtx.Param(IqcCause1.fn_cdt).Value = cmDt;
                sqlCtx.Param(IqcCause1.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectCodeInfo> GetDefectCodeInfoList(DefectCodeInfo condition)
        {
            try
            {
                IList<DefectCodeInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::DefectCode cond = mtns::FuncNew.SetColumnFromField<mtns::DefectCode, DefectCodeInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::DefectCode>(null, null, new mtns::ConditionCollection<mtns::DefectCode>(new mtns::EqualCondition<mtns::DefectCode>(cond)), mtns::DefectCode.fn_defect);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode, DefectCodeInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::DefectCode, DefectCodeInfo, DefectCodeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IqcCause1Info> GetIqcCause1InfoList(IqcCause1Info condition)
        {
            try
            {
                IList<IqcCause1Info> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::IqcCause1 cond = mtns::FuncNew.SetColumnFromField<mtns::IqcCause1, IqcCause1Info>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::IqcCause1>(null, null, new mtns::ConditionCollection<mtns::IqcCause1>(new mtns::EqualCondition<mtns::IqcCause1>(cond)), mtns::IqcCause1.fn_udt + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::IqcCause1, IqcCause1Info>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::IqcCause1, IqcCause1Info, IqcCause1Info>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IqcCause1Info> GetIqcCause1InfoList(IqcCause1Info eqCondition, IqcCause1Info neqCondition)
        {
            try
            {
                IList<IqcCause1Info> ret = null;

                if (eqCondition == null)
                    eqCondition = new IqcCause1Info();
                if (neqCondition == null)
                    neqCondition = new IqcCause1Info();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                _Metas.IqcCause1 cond = FuncNew.SetColumnFromField<_Metas.IqcCause1, IqcCause1Info>(eqCondition);
                _Metas.IqcCause1 cond2 = FuncNew.SetColumnFromField<_Metas.IqcCause1, IqcCause1Info>(neqCondition);

                sqlCtx = FuncNew.GetConditionedSelect<_Metas.IqcCause1>(null, null, new ConditionCollection<_Metas.IqcCause1>(new EqualCondition<_Metas.IqcCause1>(cond), new NotEqualCondition<_Metas.IqcCause1>(cond2, "ISNULL({0},'')")), mtns::IqcCause1.fn_udt + FuncNew.DescendOrder);
                var sqlCtx2 = FuncNew.GetConditionedSelect<_Metas.IqcCause1>(null, new string[] { _Metas.IqcCause1.fn_id }, new ConditionCollection<_Metas.IqcCause1>(new NotEqualCondition<_Metas.IqcCause1>(cond2)));

                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<_Metas.IqcCause1, IqcCause1Info>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.IqcCause1, IqcCause1Info>(sqlCtx2, neqCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::IqcCause1, IqcCause1Info, IqcCause1Info>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool CheckIfIqcCauseExist(IqcCause1Info eqCondition, IqcCause1Info neqCondition)
        {
            try
            {
                bool ret = false;

                if (eqCondition == null)
                    eqCondition = new IqcCause1Info();
                if (neqCondition == null)
                    neqCondition = new IqcCause1Info();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                IqcCause1 cond = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(eqCondition);
                _Metas.IqcCause1 cond2 = FuncNew.SetColumnFromField<_Metas.IqcCause1, IqcCause1Info>(neqCondition);

                sqlCtx = FuncNew.GetConditionedSelect<IqcCause1>("COUNT", new string[] { IqcCause1.fn_id }, new ConditionCollection<IqcCause1>(new EqualCondition<IqcCause1>(cond), new NotEqualCondition<_Metas.IqcCause1>(cond2, "ISNULL({0},'')")));
                var sqlCtx2 = FuncNew.GetConditionedSelect<_Metas.IqcCause1>(null, new string[] { _Metas.IqcCause1.fn_id }, new ConditionCollection<_Metas.IqcCause1>(new NotEqualCondition<_Metas.IqcCause1>(cond2)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<IqcCause1, IqcCause1Info>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.IqcCause1, IqcCause1Info>(sqlCtx2, neqCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        #region . Inners .

        private Defect Find_DB(object key)
        {
            try
            {
                Defect ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectCode cond = new _Schema.DefectCode();
                        cond.Defect = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new Defect();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Cdt]);
                        ret.DefectCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Defect]);
                        ret.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Descr]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Editor]);
                        ret.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Type]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Udt]);
                        ret.EngDescr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_engDescr]);
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

        private Defect Find_Cache(object key)
        {
            return null;
        }

        private IList<Defect> FindAll_DB()
        {
            try
            {
                IList<Defect> ret = new List<Defect>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        Defect item = new Defect();
                        item = new Defect();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Cdt]);
                        item.DefectCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Defect]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Editor]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Type]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Udt]);
                        item.EngDescr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_engDescr]);
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

        private IList<Defect> FindAll_Cache()
        {
            return null;
        }

        private void PersistInsertDefect(Defect item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.DefectCode.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = item.DefectCode;
                sqlCtx.Params[_Schema.DefectCode.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.DefectCode.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.DefectCode.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.DefectCode.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateDefect(Defect item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                //sqlCtx.Params[_Schema.DefectCode.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = item.DefectCode;
                sqlCtx.Params[_Schema.DefectCode.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.DefectCode.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.DefectCode.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.DefectCode.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectCode.fn_engDescr].Value = item.EngDescr;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteDefect(Defect item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode));
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = item.DefectCode;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<Defect> FindByType(string type)
        {
            try
            {
                if (!IsCached())
                    return FindByType_DB(type);

                IList<Defect> ret = FindByType_Cache(type);
                if (ret == null || ret.Count < 1)
                {
                    ret = FindByType_DB(type);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<Defect> FindByType_DB(string type)
        {
            try
            {
                IList<Defect> ret = new List<Defect>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectCode cond = new _Schema.DefectCode();
                        cond.Type = type;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Type].Value = type;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        Defect item = new Defect();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Cdt]);
                        item.DefectCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Defect]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Editor]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Type]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Udt]);
                        item.EngDescr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_engDescr]);
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

        private IList<Defect> FindByType_Cache(string type)
        {
            try
            {
                IList<Defect> ret = new List<Defect>();
                string key = _Schema.Func.MakeKeyForIdx(preStr1, type);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (string pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                            {
                                ret.Add(_cache[pk]);
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

        #endregion

        #region ICache Members

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Defect);
        }

        public void ProcessItem(CacheUpdateInfo item)
        {
            LoadAllCache();
        }

        private void LoadAllCache()
        {
            IList<Defect> dfcts = this.FindAll_DB();
            if (dfcts != null && dfcts.Count > 0)
            {
                lock (_syncObj_cache)
                {
                    _cache.Clear();
                    _byWhateverIndex.Clear();

                    foreach (Defect dfct in dfcts)
                    {
                        _cache.Add(dfct.DefectCode, dfct);

                        //Regist index
                        Regist(dfct.DefectCode, _Schema.Func.MakeKeyForIdx(preStr1, dfct.Type));
                    }
                }
            }
        }

        private void Regist(string pk, string key)
        {
            IList<string> PKs = null;
            try
            {
                PKs = _byWhateverIndex[key];
            }
            catch (KeyNotFoundException)
            {
                PKs = new List<string>();
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
            ret.Type = IMES.DataModel.CacheType.DefectCode;
            return ret;
        }

        #endregion

        #region For Maintain

        public IList<DefectCodeInfo> GetDefectCodeList(string type)
        {
            try
            {
                IList<DefectCodeInfo> ret = new List<DefectCodeInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectCode cond = new _Schema.DefectCode();
                        cond.Type = type;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode),  cond, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.DefectCode.fn_Type, _Schema.DefectCode.fn_Defect }));
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Type].Value = type;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DefectCodeInfo item = new DefectCodeInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Cdt]);
                        item.Defect = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Defect]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Editor]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Type]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Udt]);
                        item.engDescr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_engDescr]);
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

        public string GetDefect(string type, string defect)
        {
            // SELECT Defect FROM IMES_GetData..DefectCode WHERE Type = @Type AND Defect = @Defect
            try
            {
                string ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectCode cond = new _Schema.DefectCode();
                        cond.Type = type;
                        cond.Defect = defect;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode), "TOP 1", new List<string>() { _Schema.DefectCode.fn_Defect }, cond, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.DefectCode.fn_Type, _Schema.DefectCode.fn_Defect }));
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Type].Value = type;
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = defect;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Defect]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDefectCode(string type, string defect)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectCode cond = new _Schema.DefectCode();
                        cond.Type = type;
                        cond.Defect = defect;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Type].Value = type;
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = defect;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public void UpdateDefectCode(DefectCodeInfo dfc)
        {
            // IF EXISTS(SELECT * FROM IMES_GetData..DefectCode WHERE Defect = @Defect)
            //          UPDATE IMES_GetData..DefectCode SET Descr = @Descr, EngDescr=@EngDescr, Editor = @Editor, Udt = GETDATE()
            //                    WHERE Defect = @Defect
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectCode cond = new _Schema.DefectCode();
                        cond.Defect = dfc.Defect;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode), new List<string>() { _Schema.DefectCode.fn_Descr, _Schema.DefectCode.fn_Editor, _Schema.DefectCode.fn_engDescr }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = dfc.Defect;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.DefectCode.fn_Descr)].Value = dfc.Descr;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.DefectCode.fn_Editor)].Value = dfc.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.DefectCode.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.DefectCode.fn_engDescr)].Value = dfc.engDescr;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public void InsertDefectCode(DefectCodeInfo dfc)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.DefectCode.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = dfc.Defect;
                sqlCtx.Params[_Schema.DefectCode.fn_Descr].Value = dfc.Descr;
                sqlCtx.Params[_Schema.DefectCode.fn_Editor].Value = dfc.Editor;
                sqlCtx.Params[_Schema.DefectCode.fn_Type].Value = dfc.Type;
                sqlCtx.Params[_Schema.DefectCode.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.DefectCode.fn_engDescr].Value = dfc.engDescr;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public int CheckExistsRecord(string Defect)
        {
            // SELECT COUNT(*) FROM [IMES_GetData_Datamaintain].[dbo].[DefectCode]
            //   WHERE [Defect]=@Defect
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DefectCode cond = new _Schema.DefectCode();
                        cond.Defect = Defect;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode), "COUNT", new List<string>() { _Schema.DefectCode.fn_Defect }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DefectCode.fn_Defect].Value = Defect;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectCodeInfo> GetDefectCodeList()
        {
            // SELECT * FROM IMES_GetData..DefectCode ORDER BY Type, Defect
            try
            {
                IList<DefectCodeInfo> ret = new List<DefectCodeInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DefectCode));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.DefectCode.fn_Type, _Schema.DefectCode.fn_Defect }));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DefectCodeInfo item = new DefectCodeInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Cdt]);
                        item.Defect = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Defect]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Editor]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Type]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_Udt]);
                        item.engDescr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DefectCode.fn_engDescr]);
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

        public IList<SQEDefectCTNoInfo> GetSQEDefectCTNoInfo(string sn)
        {
            try
            {
                IList<SQEDefectCTNoInfo> ret = null;

                SQLContextNew sqlCtx = null;

                SQLContextCollectionNew sqlSet = new SQLContextCollectionNew();
                int i = 0;

                sqlSet.AddOne(i++, ComposeForGetSQEDefectCTNoInfo_1(sn));
                sqlSet.AddOne(i++, ComposeForGetSQEDefectCTNoInfo_2(sn));

                sqlCtx = sqlSet.MergeToOneUnionQuery();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<SQEDefectCTNoInfo>();
                        while (sqlR.Read())
                        {
                            SQEDefectCTNoInfo item = new SQEDefectCTNoInfo();
                            item.Cause = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(sqlCtx.TableFields[1].Alias, _Metas.ProductRepair_DefectInfo.fn_cause)));
                            item.Cdt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(g.DecAlias(sqlCtx.TableFields[1].Alias, _Metas.ProductRepair_DefectInfo.fn_cdt)));
                            item.Defect = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(sqlCtx.TableFields[1].Alias, _Metas.ProductRepair_DefectInfo.fn_defectCode)));
                            item.Line = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(sqlCtx.TableFields[0].Alias, _Metas.ProductRepair.fn_line)));
                            item.Udt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(g.DecAlias(sqlCtx.TableFields[1].Alias, _Metas.ProductRepair_DefectInfo.fn_udt)));
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

        private SQLContextNew ComposeForGetSQEDefectCTNoInfo_1(string sn)
        {
            ITableAndFields tf1 = null;
            ITableAndFields tf2 = null;
            ITableAndFields[] tafa = null;

            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    tf1 = new TableAndFields<_Metas.KeyPartRepair>();
                    tf1.AddRangeToGetFieldNames(_Metas.KeyPartRepair.fn_line);

                    tf2 = new TableAndFields<_Metas.KeyPartRepair_DefectInfo>();
                    _Metas.KeyPartRepair_DefectInfo cond2 = new _Metas.KeyPartRepair_DefectInfo();
                    cond2.vendorCT = sn;
                    tf2.Conditions.Add(new EqualCondition<_Metas.KeyPartRepair_DefectInfo>(cond2, null, "LEFT({0},14)"));
                    tf2.AddRangeToGetFieldNames(_Metas.KeyPartRepair_DefectInfo.fn_defectCode, _Metas.KeyPartRepair_DefectInfo.fn_cause, _Metas.KeyPartRepair_DefectInfo.fn_cdt, _Metas.KeyPartRepair_DefectInfo.fn_udt);

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.KeyPartRepair, _Metas.KeyPartRepair_DefectInfo>(tf1, _Metas.KeyPartRepair.fn_id, tf2, _Metas.KeyPartRepair_DefectInfo.fn_keyPartRepairID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelectWithCertainResultSetSequence(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf2.Alias, KeyPartRepair_DefectInfo.fn_vendorCT)).Value = sn;

            return sqlCtx;
        }

        private SQLContextNew ComposeForGetSQEDefectCTNoInfo_2(string sn)
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
                        IqcCause1 cond = new IqcCause1();
                        cond.ctLabel = sn;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<IqcCause1>(tk, null, new string[][]{
                        new string[]{IqcCause1.fn_id,"''"},
                        new string[]{IqcCause1.fn_mpDefect,IqcCause1.fn_mpDefect},
                        new string[]{IqcCause1.fn_id,"''"},
                        new string[]{IqcCause1.fn_udt,IqcCause1.fn_udt},
                        new string[]{IqcCause1.fn_udt,IqcCause1.fn_udt}
                        }, new ConditionCollection<IqcCause1>(new EqualCondition<IqcCause1>(cond)));
                    }
                }
                sqlCtx.Param(IqcCause1.fn_ctLabel).Value = sn;
                return sqlCtx;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartTypeSQEDefectReport(string sn)
        {
            try
            {
                IList<string> ret = null;

                SQLContextNew sqlCtx = null;

                SQLContextCollectionNew sqlSet = new SQLContextCollectionNew();
                int i = 0;

                sqlSet.AddOne(i++, ComposeForGetPartTypeSQEDefectReport_1(sn));
                sqlSet.AddOne(i++, ComposeForGetPartTypeSQEDefectReport_2(sn));

                sqlCtx = sqlSet.MergeToOneUnionQuery();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(sqlCtx.TableFields[1].Alias, _Metas.Part_NEW.fn_descr)));
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

        private SQLContextNew ComposeForGetPartTypeSQEDefectReport_1(string sn)
        {
            ITableAndFields tf1 = null;
            ITableAndFields tf2 = null;
            ITableAndFields tf3 = null;
            ITableAndFields[] tafa = null;

            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    tf1 = new TableAndFields<_Metas.KeyPartRepair_DefectInfo>();
                    _Metas.KeyPartRepair_DefectInfo cond = new _Metas.KeyPartRepair_DefectInfo();
                    cond.vendorCT = sn;
                    tf1.Conditions.Add(new EqualCondition<_Metas.KeyPartRepair_DefectInfo>(cond, null, "LEFT({0},14)"));
                    tf1.ClearToGetFieldNames();
                    //tf1.SubDBCalalog = _Schema.SqlHelper.DB_FA;

                    tf2 = new TableAndFields<_Metas.Part_NEW>();
                    _Metas.Part_NEW cond2 = new Part_NEW();
                    cond2.flag = 1;
                    tf2.Conditions.Add(new EqualCondition<_Metas.Part_NEW>(cond2));
                    tf2.AddRangeToGetFuncedFieldNames(new string[] { _Metas.Part_NEW.fn_descr, string.Format("LEFT(RTRIM(t2.{0}),3)", _Metas.Part_NEW.fn_descr) });

                    tf3 = new TableAndFields<_Metas.ModelBOM_NEW>();
                    _Metas.ModelBOM_NEW cond3 = new ModelBOM_NEW();
                    cond3.flag = 1;
                    tf3.Conditions.Add(new EqualCondition<_Metas.ModelBOM_NEW>(cond3));
                    tf3.ClearToGetFieldNames();

                    tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.KeyPartRepair_DefectInfo, _Metas.ModelBOM_NEW>(tf1, _Metas.KeyPartRepair_DefectInfo.fn_vendorCT, tf3, _Metas.ModelBOM_NEW.fn_component, "LEFT({0},5)={1}"),
                        new TableConnectionItem<_Metas.Part_NEW, _Metas.ModelBOM_NEW>(tf2, _Metas.Part_NEW.fn_partNo, tf3, _Metas.ModelBOM_NEW.fn_material));

                    sqlCtx = FuncNew.GetConditionedJoinedSelectWithCertainResultSetSequence(tk, null, tafa, tblCnnts);

                    sqlCtx.Param(g.DecAlias(tf2.Alias, Part_NEW.fn_flag)).Value = cond2.flag;
                    sqlCtx.Param(g.DecAlias(tf3.Alias, ModelBOM_NEW.fn_flag)).Value = cond3.flag;
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];
            tf3 = tafa[2];

            sqlCtx.Param(g.DecAlias(tf1.Alias, KeyPartRepair_DefectInfo.fn_vendorCT)).Value = sn;

            return sqlCtx;
        }

        private SQLContextNew ComposeForGetPartTypeSQEDefectReport_2(string sn)
        {
            ITableAndFields tf1 = null;
            ITableAndFields tf2 = null;
            ITableAndFields tf3 = null;
            ITableAndFields[] tafa = null;

            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    tf1 = new TableAndFields<_Metas.IqcCause1>();
                    _Metas.IqcCause1 cond = new _Metas.IqcCause1();
                    cond.ctLabel = sn;
                    tf1.Conditions.Add(new EqualCondition<_Metas.IqcCause1>(cond, null, "LEFT({0},14)"));
                    tf1.ClearToGetFieldNames();
                    tf1.SubDBCalalog = _Schema.SqlHelper.DB_FA;

                    tf2 = new TableAndFields<_Metas.Part_NEW>();
                    _Metas.Part_NEW cond2 = new Part_NEW();
                    cond2.flag = 1;
                    tf2.Conditions.Add(new EqualCondition<_Metas.Part_NEW>(cond2));
                    tf2.AddRangeToGetFuncedFieldNames(new string[] { _Metas.Part_NEW.fn_descr, string.Format("LEFT(RTRIM(t2.{0}),3)", _Metas.Part_NEW.fn_descr) });

                    tf3 = new TableAndFields<_Metas.ModelBOM_NEW>();
                    _Metas.ModelBOM_NEW cond3 = new ModelBOM_NEW();
                    cond3.flag = 1;
                    tf3.Conditions.Add(new EqualCondition<_Metas.ModelBOM_NEW>(cond3));
                    tf3.ClearToGetFieldNames();

                    tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.IqcCause1, _Metas.ModelBOM_NEW>(tf1, _Metas.IqcCause1.fn_ctLabel, tf3, _Metas.ModelBOM_NEW.fn_component, "LEFT({0},5)={1}"),
                        new TableConnectionItem<_Metas.Part_NEW, _Metas.ModelBOM_NEW>(tf2, _Metas.Part_NEW.fn_partNo, tf3, _Metas.ModelBOM_NEW.fn_material));

                    sqlCtx = FuncNew.GetConditionedJoinedSelectWithCertainResultSetSequence(tk, null, tafa, tblCnnts);

                    sqlCtx.Param(g.DecAlias(tf2.Alias, Part_NEW.fn_flag)).Value = cond2.flag;
                    sqlCtx.Param(g.DecAlias(tf3.Alias, ModelBOM_NEW.fn_flag)).Value = cond3.flag;
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];
            tf3 = tafa[2];

            sqlCtx.Param(g.DecAlias(tf1.Alias, IqcCause1.fn_ctLabel)).Value = sn;

            return sqlCtx;
        }

        public IList<SQEDefectProductRepairReportInfo> GetSQEDefectProductRepairInfo(string sn)
        {
            try
            {
                IList<SQEDefectProductRepairReportInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.ProductRepair_DefectInfo cond = new _Metas.ProductRepair_DefectInfo();
                        cond.vendorCT = sn;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ProductRepair_DefectInfo>(tk, null, null, new ConditionCollection<_Metas.ProductRepair_DefectInfo>(
                            new EqualCondition<_Metas.ProductRepair_DefectInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.ProductRepair_DefectInfo.fn_vendorCT).Value = sn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<SQEDefectProductRepairReportInfo>();
                        while (sqlR.Read())
                        {
                            SQEDefectProductRepairReportInfo item = new SQEDefectProductRepairReportInfo();
                            item.Cause = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_cause));
                            item.Cdt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_cdt));
                            item.Component = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_component));
                            item.DefectCode = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_defectCode));
                            item.Editor = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_editor));
                            item.MajorPart = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_majorPart));
                            item.Obligation = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_obligation));
                            item.Remark = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_remark));
                            item.Udt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_udt));
                            item.VendorCT = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ProductRepair_DefectInfo.fn_vendorCT));
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

        public void AddIqcKp(IqcKpDef iqcKp)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<IqcKp>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<IqcKp, IqcKpDef>(sqlCtx, iqcKp);

                sqlCtx.Param(IqcKp.fn_cdt).Value = cmDt;
                sqlCtx.Param(IqcKp.fn_udt).Value = cmDt;

                iqcKp.Id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string ctLabel, string defect)
        {
            try
            {
                IList<IqcKpDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        IqcKp cond = new IqcKp();
                        cond.tp = tp;
                        cond.ctLabel = ctLabel;
                        cond.defect = defect;
                        sqlCtx = FuncNew.GetConditionedSelect<IqcKp>(tk, null, null, new ConditionCollection<IqcKp>(new EqualCondition<IqcKp>(cond)));
                    }
                }
                sqlCtx.Param(IqcKp.fn_tp).Value = tp;
                sqlCtx.Param(IqcKp.fn_ctLabel).Value = ctLabel;
                sqlCtx.Param(IqcKp.fn_defect).Value = defect;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<IqcKp, IqcKpDef, IqcKpDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string ctLabel, string defect, string cause)
        {
            try
            {
                IList<IqcKpDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        IqcKp cond = new IqcKp();
                        cond.tp = tp;
                        cond.ctLabel = ctLabel;
                        cond.defect = defect;
                        cond.cause = cause;
                        sqlCtx = FuncNew.GetConditionedSelect<IqcKp>(tk, null, null, new ConditionCollection<IqcKp>(new EqualCondition<IqcKp>(cond)));
                    }
                }
                sqlCtx.Param(IqcKp.fn_tp).Value = tp;
                sqlCtx.Param(IqcKp.fn_ctLabel).Value = ctLabel;
                sqlCtx.Param(IqcKp.fn_defect).Value = defect;
                sqlCtx.Param(IqcKp.fn_cause).Value = cause;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<IqcKp, IqcKpDef, IqcKpDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateIqcKp(IqcKpDef setValue, IqcKpDef condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                IqcKp cond = FuncNew.SetColumnFromField<IqcKp, IqcKpDef>(condition);
                IqcKp setv = FuncNew.SetColumnFromField<IqcKp, IqcKpDef>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<IqcKp>(new SetValueCollection<IqcKp>(new CommonSetValue<IqcKp>(setv)), new ConditionCollection<IqcKp>(new EqualCondition<IqcKp>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<IqcKp, IqcKpDef>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<IqcKp, IqcKpDef>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.IqcKp.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectCodeInfo> GetDefecSQEDefectInfo(string type)
        {
            try
            {
                IList<DefectCodeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        DefectCode cond = new DefectCode();
                        cond.type = type;
                        sqlCtx = FuncNew.GetConditionedSelect<DefectCode>(tk, null, null, new ConditionCollection<DefectCode>(new EqualCondition<DefectCode>(cond)));
                    }
                }
                sqlCtx.Param(DefectCode.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<DefectCode, DefectCodeInfo, DefectCodeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDefectCode()
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
                        sqlCtx = FuncNew.GetConditionedSelect<DefectCode>(tk, "DISTINCT", new string[] { DefectCode.fn_defect }, new ConditionCollection<DefectCode>(), DefectCode.fn_defect);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(DefectCode.fn_defect));
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

        public IList<DefectCodeInfo> GetDefectCodeLst()
        {
            try
            {
                IList<DefectCodeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetConditionedSelect<DefectCode>(tk, null, null, new ConditionCollection<DefectCode>(), DefectCode.fn_defect);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<DefectCode, DefectCodeInfo, DefectCodeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDefectCode(string defect)
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
                        DefectCode cond = new DefectCode();
                        cond.defect = defect;
                        sqlCtx = FuncNew.GetConditionedDelete<DefectCode>(tk, new ConditionCollection<DefectCode>(new EqualCondition<DefectCode>(cond)));
                    }
                }
                sqlCtx.Param(DefectCode.fn_defect).Value = defect;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void DeleteDefectCodeDefered(IUnitOfWork uow, string type, string defect)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), type, defect);
        }

        public void UpdateDefectCodeDefered(IUnitOfWork uow, DefectCodeInfo dfc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dfc);
        }

        public void InsertDefectCodeDefered(IUnitOfWork uow, DefectCodeInfo dfc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dfc);
        }

        public void AddIqcKpDefered(IUnitOfWork uow, IqcKpDef iqcKp)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), iqcKp);
        }

        public void UpdateIqcKpDefered(IUnitOfWork uow, IqcKpDef setValue, IqcKpDef condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeleteDefectCodeDefered(IUnitOfWork uow, string defect)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), defect);
        }

        public void UpdateUDTofIqcCauseDefered(IUnitOfWork uow, IqcCause1Info setValue, IqcCause1Info condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddIqcCauseDefered(IUnitOfWork uow, IqcCause1Info item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #endregion
    }
}
