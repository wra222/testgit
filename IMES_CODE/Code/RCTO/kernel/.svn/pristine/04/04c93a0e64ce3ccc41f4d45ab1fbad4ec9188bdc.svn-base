using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure.Utility;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using IMES.Infrastructure.Utility.Cache;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Station;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Station相关
    /// </summary>
    public class StationRepository : BaseRepository<IStation>, IStationRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        //private static CacheManager _cache = CacheFactory.GetCacheManager("StationCache");
        private static IDictionary<string,IStation> _cache = new Dictionary<string,IStation>();
        private static IDictionary<StationType, IList<string>> _byTypeIndex = new Dictionary<StationType, IList<string>>(); //Type, StationIds
        private static object _syncObj_cache = new object();
        #endregion

        #region Overrides of BaseRepository<IStation>

        protected override void PersistNewItem(IStation item)
        {
            StateTracker tracker = (item as fons::Station).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertStation(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(IStation item)
        {
            StateTracker tracker = (item as fons::Station).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistModifyStation(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(IStation item)
        {
            StateTracker tracker = (item as fons::Station).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteStation(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Station>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IStation Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                IStation ret = Find_Cache(key);
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
        public override IList<IStation> FindAll()
        {
            try
            {
                if (!IsCached())
                    return FindAll_DB();

                IList<IStation> ret = FindAll_Cache();
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
        public override void Add(IStation item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(IStation item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(IStation item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region Implementation of IStationRepository

        public IList<IStation> GetStationList(StationType type)
        {
            try
            {
                if (!IsCached())
                    return GetStationList_DB(type);

                IList<IStation> ret = GetStationList_Cache(type);
                if (ret == null || ret.Count < 1)
                {
                    ret = GetStationList_DB(type);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得FA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        public IList<TestStationInfo> GetFATestStationList()
        {
            try
            {
                IList<TestStationInfo> ret = new List<TestStationInfo>();
                IList<IStation> val = GetStationList(StationType.FATest);
                if (val != null && val.Count > 0)
                {
                    foreach (IStation sttn in val)
                    {
                        TestStationInfo tsi = new TestStationInfo();
                        tsi.friendlyName = sttn.Descr;
                        tsi.id = sttn.StationId;
                        ret.Add(tsi);
                    }
                }
                return (from item in ret orderby item.friendlyName select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得SA测试站信息列表
        /// </summary>
        /// <returns>测试站信息列表</returns>
        public IList<TestStationInfo> GetSATestStationList()
        {
            try
            {
                IList<TestStationInfo> ret = new List<TestStationInfo>();
                IList<IStation> val = GetStationList(StationType.SATest);
                if (val != null && val.Count > 0)
                {
                    foreach (IStation sttn in val)
                    {
                        TestStationInfo tsi = new TestStationInfo();
                        tsi.friendlyName = sttn.Descr;
                        tsi.id = sttn.StationId;
                        ret.Add(tsi);
                    }
                }
                return (from item in ret orderby item.friendlyName select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TestStationInfo> GetTestStationListFor023()
        {
            return this.GetFATestStationList();
        }

        public string TransToDesc(string code)
        {
            try
            {
                IStation stt = this.Find(code);
                if (stt != null)
                    return stt.Descr;
                return null;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public DataTable GetLightNoStationList(string type)
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
                        mtns::Station cond = new mtns::Station();
                        cond.stationType = type;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Station>(tk, null, new string[] { mtns::Station.fn_station, mtns::Station.fn_name, mtns::Station.fn_descr }, new ConditionCollection<mtns::Station>(new EqualCondition<mtns::Station>(cond, "RTRIM({0})")), mtns::Station.fn_station);
                    }
                }
                sqlCtx.Param(mtns::Station.fn_stationType).Value = type;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IStation> GetStationListFromStation(string stationType)
        {
            try
            {
                IList<IStation> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Station cond = new mtns::Station();
                        cond.stationType = stationType;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Station>(tk, null, null, new ConditionCollection<mtns::Station>(new EqualCondition<mtns::Station>(cond)), mtns.Station.fn_descr);
                    }
                }
                sqlCtx.Param(mtns::Station.fn_stationType).Value = stationType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<IStation>();
                        while (sqlR.Read())
                        {
                            fons.Station item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<mtns::Station, fons.Station>(item, sqlR, sqlCtx);
                            item.StationType = (StationType)Enum.Parse(typeof(StationType), GetValue_Str(sqlR, sqlCtx.Indexes(_Schema.Station.fn_StationType)));
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

        public IList<DefectCodeStationInfo> GetDefectCodeStationList(DefectCodeStationInfo condition)
        {
            try
            {
                IList<DefectCodeStationInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::DefectCode_Station cond = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::DefectCode_Station>(null, null, new mtns::ConditionCollection<mtns::DefectCode_Station>(new mtns::EqualCondition<mtns::DefectCode_Station>(cond)), mtns::DefectCode_Station.fn_id);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::DefectCode_Station, DefectCodeStationInfo, DefectCodeStationInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectCodeStationInfo> GetDefectCodeStationList(DefectCodeStationInfo eqCondition, DefectCodeStationInfo isNullCondition)
        {
            try
            {
                IList<DefectCodeStationInfo> ret = null;

                if (eqCondition == null)
                    eqCondition = new DefectCodeStationInfo();
                if (isNullCondition == null)
                    isNullCondition = new DefectCodeStationInfo();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::DefectCode_Station cond = FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(eqCondition);
                mtns::DefectCode_Station cond2 = FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(isNullCondition);

                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::DefectCode_Station>(null, null, new mtns::ConditionCollection<mtns::DefectCode_Station>(new mtns::EqualCondition<mtns::DefectCode_Station>(cond), new EqualCondition<_Metas.DefectCode_Station>(cond2, "ISNULL({0},'')")), mtns::DefectCode_Station.fn_id);
                var sqlCtx2 = FuncNew.GetConditionedSelect<_Metas.DefectCode_Station>(null, new string[] { _Metas.DefectCode_Station.fn_id }, new ConditionCollection<_Metas.DefectCode_Station>(new EqualCondition<_Metas.DefectCode_Station>(cond2, "ISNULL({0},'')")));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.DefectCode_Station, DefectCodeStationInfo>(sqlCtx2, isNullCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::DefectCode_Station, DefectCodeStationInfo, DefectCodeStationInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectCodeStationInfo> GetDefectCodeStationList2(DefectCodeStationInfo condition)
        {
            try
            {
                IList<DefectCodeStationInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::DefectCode_Station cond = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::DefectCode_Station>(null, null, 
                    new mtns::ConditionCollection<mtns::DefectCode_Station>(new mtns::EqualCondition<mtns::DefectCode_Station>(cond)), 
                    mtns::DefectCode_Station.fn_pre_stn, mtns::DefectCode_Station.fn_crt_stn, 
                    mtns::DefectCode_Station.fn_majorPart, mtns::DefectCode_Station.fn_cause, mtns::DefectCode_Station.fn_defect);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, condition);

                string[] parts = sqlCtx.Sentence.Split(new string[] { string.Format(" FROM {0} ", ToolsNew.GetTableName(typeof(mtns::DefectCode_Station))) }, StringSplitOptions.RemoveEmptyEntries);
                string[] fields = parts[0]
                    .Split(new string[]{" ", ","}, StringSplitOptions.RemoveEmptyEntries);
                string selectPart = string.Format("SELECT {0}." + string.Join(",{0}.", fields, 1, fields.Length - 1), ToolsNew.GetTableName(typeof(mtns.DefectCode_Station)));

                sqlCtx.Sentence = selectPart + string.Format(" FROM {0} ", ToolsNew.GetTableName(typeof(mtns::DefectCode_Station))) + parts[1];

                string innerPart = string.Format(",pres.{2} AS preName,curs.{2} AS curName,nxts.{2} AS nxtName,isnull(dc.{3},'') AS dfDescr " +
                " FROM {5} " +
                " JOIN {0} AS pres ON pres.{4} = {5}.{6} " +
                " JOIN {0} AS curs ON curs.{4} = {5}.{7} " +
                " JOIN {0} AS nxts ON nxts.{4} = {5}.{8} " +
                " LEFT  JOIN {1} AS dc ON dc.Defect = {5}.{9} ", 
                ToolsNew.GetTableName(typeof(mtns.Station)), 
                ToolsNew.GetTableName(typeof(mtns.DefectCode)),
                mtns.Station.fn_name,
                mtns.DefectCode.fn_descr,
                mtns.Station.fn_station,
                ToolsNew.GetTableName(typeof(mtns.DefectCode_Station)),
                mtns.DefectCode_Station.fn_pre_stn, 
                mtns.DefectCode_Station.fn_crt_stn,
                mtns.DefectCode_Station.fn_nxt_stn, 
                mtns.DefectCode_Station.fn_defect,
                mtns.DefectCode.fn_defect
                );

                //" ORDER BY a.PRE_STN,a.CRT_STN,a.Defect ";
                string Sentence = sqlCtx.Sentence.Replace(string.Format(" FROM {0} ", ToolsNew.GetTableName(typeof(mtns::DefectCode_Station))), innerPart);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DefectCodeStationInfo>();
                        while (sqlR.Read())
                        {
                            DefectCodeStationInfo item = null;
                            item = mtns::FuncNew.SetFieldFromColumnWithoutReadReader<mtns::DefectCode_Station, DefectCodeStationInfo>(item, sqlR, sqlCtx);
                            item.preName = g.GetValue_Str(sqlR, 11);
                            item.curName = g.GetValue_Str(sqlR, 12);
                            item.nxtName = g.GetValue_Str(sqlR, 13);
                            item.dfDescr = g.GetValue_Str(sqlR, 14);
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

        public bool CheckStationExistByName(string name)
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
                        _Metas.Station cond = new _Metas.Station();
                        cond.name = name;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Station>(tk, "COUNT", new string[] { _Metas.Station.fn_station }, new ConditionCollection<_Metas.Station>(new EqualCondition<_Metas.Station>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Station.fn_name).Value = name;

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

        public IList<IMES.FisObject.Common.Station.Station> GetStationItemByName(string name)
        {
            try
            {
                IList<IMES.FisObject.Common.Station.Station> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Station cond = new _Metas.Station();
                        cond.name = name;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Station>(tk, null, null, new ConditionCollection<_Metas.Station>(new EqualCondition<_Metas.Station>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Station.fn_name).Value = name;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Station, fons.Station, fons.Station>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.Common.Station.Station> GetStationItemByStationID(string staion)
        {
            try
            {
                IList<IMES.FisObject.Common.Station.Station> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Station cond = new _Metas.Station();
                        cond.station = staion;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Station>(tk, null, null, new ConditionCollection<_Metas.Station>(new EqualCondition<_Metas.Station>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Station.fn_station).Value = staion;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Station, fons.Station, fons.Station>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StationInfo> GetStationInfoListByNotLikeStationType(string exceptStationTypePrefix)
        {
            try
            {
                IList<StationInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Station cond = new _Metas.Station();
                        cond.stationType = exceptStationTypePrefix + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Station>(tk, null, new string[] { _Metas.Station.fn_station, _Metas.Station.fn_name }, new ConditionCollection<_Metas.Station>(new NotLikeCondition<_Metas.Station>(cond)), _Metas.Station.fn_station);
                    }
                }
                sqlCtx.Param(_Metas.Station.fn_stationType).Value = exceptStationTypePrefix + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<StationInfo>();
                        while(sqlR.Read())
                        {
                            StationInfo item = new StationInfo();
                            item.StationId = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Station.fn_station));
                            item.Descr = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Station.fn_name));
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

        public IList<StationCheckInfo> GetStationCheckInfoList(StationCheckInfo condition)
        {
            try
            {
                IList<StationCheckInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::StationCheck cond = mtns::FuncNew.SetColumnFromField<mtns::StationCheck, StationCheckInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::StationCheck>(null, null, new mtns::ConditionCollection<mtns::StationCheck>(new mtns::EqualCondition<mtns::StationCheck>(cond)), mtns::StationCheck.fn_station, mtns::StationCheck.fn_line, mtns::StationCheck.fn_checkItemType);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::StationCheck, StationCheckInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::StationCheck, StationCheckInfo, StationCheckInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertStationCheckInfo(StationCheckInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<StationCheck>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<StationCheck, StationCheckInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::StationCheck.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::StationCheck.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateStationCheckInfo(StationCheckInfo setValue, StationCheckInfo condition)
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
                mtns::StationCheck cond = mtns::FuncNew.SetColumnFromField<mtns::StationCheck, StationCheckInfo>(condition);
                mtns::StationCheck setv = mtns::FuncNew.SetColumnFromField<mtns::StationCheck, StationCheckInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::StationCheck>(new mtns::SetValueCollection<mtns::StationCheck>(new mtns::CommonSetValue<mtns::StationCheck>(setv)), new mtns::ConditionCollection<mtns::StationCheck>(new mtns::EqualCondition<mtns::StationCheck>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::StationCheck, StationCheckInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::StationCheck, StationCheckInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::StationCheck.fn_udt)).Value = cmDt;
 
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteStationCheckInfo(StationCheckInfo condition)
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
                StationCheck cond = FuncNew.SetColumnFromField<StationCheck, StationCheckInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<StationCheck>(new ConditionCollection<StationCheck>(new EqualCondition<StationCheck>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<StationCheck, StationCheckInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void InsertStationCheckInfoDefered(IUnitOfWork uow, StationCheckInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateStationCheckInfoDefered(IUnitOfWork uow, StationCheckInfo setValue, StationCheckInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeleteStationCheckInfoDefered(IUnitOfWork uow, StationCheckInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        #endregion

        #endregion

        #region . Inners .

        private void PersistInsertStation(IStation item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Station.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Station.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Station.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Station.fn_OperationObject].Value = item.OperationObject;
                sqlCtx.Params[_Schema.Station.fn_station].Value = item.StationId;
                sqlCtx.Params[_Schema.Station.fn_StationType].Value = Enum.GetName(typeof(StationType), item.StationType);
                sqlCtx.Params[_Schema.Station.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Station.fn_Name].Value = item.Name;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistModifyStation(IStation item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Station.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Station.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Station.fn_OperationObject].Value = item.OperationObject;
                sqlCtx.Params[_Schema.Station.fn_station].Value = item.StationId;
                sqlCtx.Params[_Schema.Station.fn_StationType].Value = Enum.GetName(typeof(StationType), item.StationType);
                sqlCtx.Params[_Schema.Station.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Station.fn_Name].Value = item.Name;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteStation(IStation item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station));
                    }
                }
                sqlCtx.Params[_Schema.Station.fn_station].Value = item.StationId;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IStation> FindAll_DB()
        {
            try
            {
                IList<IStation> ret = new List<IStation>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons::Station item = new fons::Station();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Cdt]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Editor]);
                        item.OperationObject = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Station.fn_OperationObject]);
                        item.StationId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_station]);
                        item.StationType = (StationType)Enum.Parse(typeof(StationType), GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_StationType]));
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Udt]);
                        item.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Name]);
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

        private IList<IStation> FindAll_Cache()
        {
            try
            {
                //IList<IStation> ret = new List<IStation>();
                lock (_syncObj_cache)
                {

                    return _cache.Values.ToList<IStation>();

                //    IEnumerator<KeyValuePair<string,IStation>> eumrt = _cache.GetEnumerator();
                //    if (eumrt != null)
                //    {
                //        while (eumrt.MoveNext())
                //        {
                //            ret.Add(eumrt.Current.Value);
                //        }
                //    }
                }
                //return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IStation Find_DB(object key)
        {
            try
            {
                fons::Station ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Station cond = new _Schema.Station();
                        cond.station = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Station.fn_station].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons::Station();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Cdt]);
                        ret.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Descr]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Editor]);
                        ret.OperationObject = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Station.fn_OperationObject]);
                        ret.StationId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_station]);
                        ret.StationType = (StationType)Enum.Parse(typeof(StationType), GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_StationType]));
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Udt]);
                        ret.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Name]);
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

        private IStation Find_Cache(object key)
        {
            try
            {
                IStation ret = null;
                lock (_syncObj_cache)
                {
                    if (_cache.ContainsKey((string)key))
                        ret = _cache[(string)key];
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IStation> GetStationList_DB(StationType type)
        {
            try
            {
                IList<IStation> ret = new List<IStation>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Station cond = new _Schema.Station();
                        cond.StationType = Enum.GetName(typeof(StationType), type);
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Station.fn_station);
                    }
                }
                sqlCtx.Params[_Schema.Station.fn_StationType].Value = Enum.GetName(typeof(StationType), type);
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons::Station item = new fons::Station();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Cdt]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Editor]);
                        item.OperationObject = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Station.fn_OperationObject]);
                        item.StationId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_station]);
                        item.StationType = (StationType)Enum.Parse(typeof(StationType), GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_StationType]));
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Udt]);
                        item.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station.fn_Name]);
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

        private IList<IStation> GetStationList_Cache(StationType type)
        {
            try
            {
                IList<IStation> ret = new List<IStation>();
                lock (_syncObj_cache)
                {
                    if (_byTypeIndex.ContainsKey(type))
                    {
                        foreach(string pk in _byTypeIndex[type])
                        {
                            if (_cache.ContainsKey(pk))
                                ret.Add(_cache[pk]);
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

        public IList<IStation> GetStationOrderByStation()
        {
            var ret = FindAll();

            if (ret != null)
                return (from item in ret orderby item.StationId select item).ToList();
            else
                return null;
        }

        #endregion

        #region ICache Members

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Station);
        }

        public void ProcessItem(CacheUpdateInfo item)
        {
            LoadAllCache();
        }

        private void LoadAllCache()
        {
            IList<IStation> stts = this.FindAll_DB();
            if (stts != null && stts.Count > 0)
            {
                lock (_syncObj_cache)
                {
                    //_cache.Flush();
                    _cache.Clear();
                    _byTypeIndex.Clear();

                    foreach(IStation stt in stts)
                    {
                        //_cache.Add(stt.StationId, stt);
                        _cache.Add(stt.StationId, stt);

                        //Regist index by type
                        //if (stt.StationType != null) Enum doesn't need.
                        //{
                            IList<string> PKs = null;
                            try
                            {
                                PKs = _byTypeIndex[stt.StationType];
                            }
                            catch (KeyNotFoundException)
                            {
                                PKs = new List<string>();
                                _byTypeIndex.Add(stt.StationType, PKs);
                            }
                            PKs.Add(stt.StationId);
                        //}
                    }
                }
            }
        }

        private CacheUpdateInfo GetACacheSignal()
        {
            CacheUpdateInfo ret = new CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = CacheType.Station;
            return ret;
        }

        #endregion

        #region For Maintain

        public DataTable GetStationList()
        {
            //SELECT [Station]
            // FROM [IMES_GetData_Datamaintain].[dbo].[Station]
            //ORDER BY [Cdt]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station), null, new List<string>() { _Schema.Station.fn_station }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Station.fn_station);
                    }
                }
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetStationInfoList()
        {
            // SELECT [Station]
            //       ,[Name]
            //       ,[StationType]
            //       ,[OperationObject]
            //       ,[Descr]
            //       ,[Editor]
            //       ,[Cdt]
            //       ,[Udt]
            //   FROM [IMES_GetData_Datamaintain].[dbo].[Station]
            // order by [Station]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station), null, new List<string>() { _Schema.Station.fn_station, _Schema.Station.fn_Name, _Schema.Station.fn_StationType, _Schema.Station.fn_OperationObject, _Schema.Station.fn_Descr, _Schema.Station.fn_Editor, _Schema.Station.fn_Cdt, _Schema.Station.fn_Udt }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Station.fn_station);
                    }
                }
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null);
                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Station.fn_station],
                                                                sqlCtx.Indexes[_Schema.Station.fn_Name],
                                                                sqlCtx.Indexes[_Schema.Station.fn_StationType],
                                                                sqlCtx.Indexes[_Schema.Station.fn_OperationObject],
                                                                sqlCtx.Indexes[_Schema.Station.fn_Descr],
                                                                sqlCtx.Indexes[_Schema.Station.fn_Editor],
                                                                sqlCtx.Indexes[_Schema.Station.fn_Cdt],
                                                                sqlCtx.Indexes[_Schema.Station.fn_Udt]});
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteStation(string station)
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
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station));
                    }
                }
                sqlCtx.Params[_Schema.Station.fn_station].Value = station;
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

        public bool IsExistStation(string station)
        {
            // IF EXISTS(
            // SELECT [Station]     
            //   FROM [Station]
            // where Station ='station'
            // )
            // set @return='True'
            // ELSE
            // set @return='False'
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Station cond = new _Schema.Station();
                        cond.station = station;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Station.fn_station].Value = station;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddStation(fons::Station item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                this.PersistInsertStation(item);

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

        public void UpdateStation(fons::Station item, string oldStationId)
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
                        _Schema.Station cond = new _Schema.Station();
                        cond.station = oldStationId;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station), new List<string>() { _Schema.Station.fn_Descr, _Schema.Station.fn_Editor, _Schema.Station.fn_OperationObject, _Schema.Station.fn_station, _Schema.Station.fn_StationType, _Schema.Station.fn_Name }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Station.fn_station].Value = oldStationId;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Station.fn_Descr)].Value = item.Descr;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Station.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Station.fn_OperationObject)].Value = item.OperationObject;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Station.fn_station)].Value = item.StationId;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Station.fn_StationType)].Value = Enum.GetName(typeof(StationType), item.StationType);
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Station.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Station.fn_Name)].Value = item.Name;
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

        public void InsertDefectCodeStationInfo(DefectCodeStationInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<DefectCode_Station>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<DefectCode_Station, DefectCodeStationInfo>(sqlCtx, item);

                sqlCtx.Param(DefectCode_Station.fn_cdt).Value = cmDt;
                sqlCtx.Param(DefectCode_Station.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDefectCodeStationInfo(DefectCodeStationInfo setValue, DefectCodeStationInfo condition)
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
                mtns::DefectCode_Station cond = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(condition);
                mtns::DefectCode_Station setv = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::DefectCode_Station>(new mtns::SetValueCollection<mtns::DefectCode_Station>(new mtns::CommonSetValue<mtns::DefectCode_Station>(setv)), new mtns::ConditionCollection<mtns::DefectCode_Station>(new mtns::EqualCondition<mtns::DefectCode_Station>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::DefectCode_Station.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDefectCodeStationInfo(DefectCodeStationInfo condition)
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
                DefectCode_Station cond = FuncNew.SetColumnFromField<DefectCode_Station, DefectCodeStationInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<DefectCode_Station>(new ConditionCollection<DefectCode_Station>(new EqualCondition<DefectCode_Station>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<DefectCode_Station, DefectCodeStationInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void DeleteStationDefered(IUnitOfWork uow, string station)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), station);
        }

        public void AddStationDefered(IUnitOfWork uow, fons::Station item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateStationDefered(IUnitOfWork uow, fons::Station item, string oldStationId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldStationId);
        }

        public void InsertDefectCodeStationInfoDefered(IUnitOfWork uow, DefectCodeStationInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateDefectCodeStationInfoDefered(IUnitOfWork uow, DefectCodeStationInfo setValue, DefectCodeStationInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeleteDefectCodeStationInfoDefered(IUnitOfWork uow, DefectCodeStationInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        #endregion

        #endregion


        #region  for DefectCode_Station add New Major part
        public IList<DefectCodeStationInfo> GetDefectStationByCurStation(string curStation)
        {
             try
            {
                IList<DefectCodeStationInfo> ret = null;
                
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                DefectCodeStationInfo condition = new DefectCodeStationInfo();
                condition.crt_stn = curStation;    
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::DefectCode_Station cond = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(condition);
                       sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::DefectCode_Station>(null, null, 
                                                                                                                                             new mtns::ConditionCollection<mtns::DefectCode_Station>(new mtns::EqualCondition<mtns::DefectCode_Station>(cond)),
                                                                                                                                             mtns::DefectCode_Station.fn_pre_stn,
                                                                                                                                              mtns::DefectCode_Station.fn_majorPart,
                                                                                                                                               mtns::DefectCode_Station.fn_cause,
                                                                                                                                                mtns::DefectCode_Station.fn_defect);    
                    }
                }

                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, 
                                                                                                                            CommandType.Text, 
                                                                                                                            sqlCtx.Sentence, 
                                                                                                                            sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::DefectCode_Station, DefectCodeStationInfo, DefectCodeStationInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public IList<DefectCodeStationInfo> GetDefectStationByPreStation(string preStation)
        {
             try
            {
                IList<DefectCodeStationInfo> ret = null;
                
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                DefectCodeStationInfo condition = new DefectCodeStationInfo();
                condition.pre_stn = preStation;    
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::DefectCode_Station cond = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(condition);
                       sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::DefectCode_Station>(null, null, 
                                                                                                                                             new mtns::ConditionCollection<mtns::DefectCode_Station>(new mtns::EqualCondition<mtns::DefectCode_Station>(cond)),
                                                                                                                                             mtns::DefectCode_Station.fn_crt_stn,
                                                                                                                                              mtns::DefectCode_Station.fn_majorPart,
                                                                                                                                               mtns::DefectCode_Station.fn_cause,
                                                                                                                                                mtns::DefectCode_Station.fn_defect);    
                    }
                }

                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, 
                                                                                                                            CommandType.Text, 
                                                                                                                            sqlCtx.Sentence, 
                                                                                                                            sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::DefectCode_Station, DefectCodeStationInfo, DefectCodeStationInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DefectCodeStationInfo> GetDefectStation(string preStation, string curStation)
        {
              try
            {
                IList<DefectCodeStationInfo> ret = null;
                
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                DefectCodeStationInfo condition = new DefectCodeStationInfo();
                condition.pre_stn = preStation;
                condition.crt_stn = curStation;    
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::DefectCode_Station cond = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(condition);
                       sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::DefectCode_Station>(null, null, 
                                                                                                                                             new mtns::ConditionCollection<mtns::DefectCode_Station>(new mtns::EqualCondition<mtns::DefectCode_Station>(cond)),
                                                                                                                                              mtns::DefectCode_Station.fn_majorPart,
                                                                                                                                               mtns::DefectCode_Station.fn_cause,
                                                                                                                                                mtns::DefectCode_Station.fn_defect);    
                    }
                }

                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::DefectCode_Station, DefectCodeStationInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, 
                                                                                                                            CommandType.Text, 
                                                                                                                            sqlCtx.Sentence, 
                                                                                                                            sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::DefectCode_Station, DefectCodeStationInfo, DefectCodeStationInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool CheckDefectStationUnique(string preStation, string curStation, string majorPart, string cause, string defect)
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


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select top 1 ID
                                                                from DefectCode_Station
                                                                where   PRE_STN = @PRE_STN and
                                                                             CRT_STN = @CRT_STN and
                                                                             MajorPart = @MajorPart and
                                                                             Cause = @Cause and
                                                                             Defect = @Defect ";
                        sqlCtx.AddParam("PRE_STN", new SqlParameter("@PRE_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("CRT_STN", new SqlParameter("@CRT_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("MajorPart", new SqlParameter("@MajorPart", SqlDbType.VarChar));
                        sqlCtx.AddParam("Cause", new SqlParameter("@Cause", SqlDbType.VarChar));
                        sqlCtx.AddParam("Defect", new SqlParameter("@Defect", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("PRE_STN").Value = preStation;
                sqlCtx.Param("CRT_STN").Value = curStation;
                sqlCtx.Param("MajorPart").Value = majorPart;
                sqlCtx.Param("Cause").Value = cause;
                sqlCtx.Param("Defect").Value = defect;
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.HasRows;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public  IList<DefectCodeNextStationInfo> GetNextStationFromDefectStation(string preStation, string curStation, string majorPart, string cause, string defect)
        {
            try
            {
                IList<DefectCodeNextStationInfo> ret = new List<DefectCodeNextStationInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  ID ,NXT_STN,MajorPart,Cause,Defect,
                                                                       (case when MajorPart=@MajorPart and Cause=@Cause and Defect=@Defect then
                                                                               1
                                                                             when MajorPart=@MajorPart and Cause=@Cause and Defect='' then
                                                                               2
                                                                             when MajorPart=@MajorPart and Cause='' and Defect=@Defect then
                                                                               3
                                                                             when MajorPart='' and Cause=@Cause and Defect=@Defect then
                                                                               4
                                                                             when MajorPart=@MajorPart and Cause='' and Defect='' then
                                                                               5
                                                                             when MajorPart='' and Cause=@Cause and Defect='' then
                                                                               6
                                                                             when MajorPart='' and Cause='' and Defect=@Defect then
                                                                               7
                                                                             else
                                                                               8  
                                                                         end) as Priority,
                                                                          0 as FamilyPriority                    
                                                                from DefectCode_Station
                                                                where PRE_STN=@PRE_STN and
                                                                      CRT_STN=@CRT_STN and
                                                                      (MajorPart='' or MajorPart=@MajorPart  ) and
                                                                      (Cause='' or Cause=@Cause) and
                                                                      (Defect='' or Defect=@Defect)
                                                                order by Priority";
                        sqlCtx.AddParam("PRE_STN", new SqlParameter("@PRE_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("CRT_STN", new SqlParameter("@CRT_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("MajorPart", new SqlParameter("@MajorPart", SqlDbType.VarChar));
                        sqlCtx.AddParam("Cause", new SqlParameter("@Cause", SqlDbType.VarChar));
                        sqlCtx.AddParam("Defect", new SqlParameter("@Defect", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PRE_STN").Value = preStation;
                sqlCtx.Param("CRT_STN").Value = curStation;
                sqlCtx.Param("MajorPart").Value = majorPart;
                sqlCtx.Param("Cause").Value = cause;
                sqlCtx.Param("Defect").Value = defect;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DefectCodeNextStationInfo item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<DefectCodeNextStationInfo>(sqlR);
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
        public IList<string> GetCurStationFromDefectStation()
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct CRT_STN
                                                                from DefectCode_Station ";                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {

                        ret.Add(sqlR.GetString(0));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<string> GetPreStationFromDefectStation()
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct PRE_STN
                                                                from DefectCode_Station ";

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {

                        ret.Add(sqlR.GetString(0));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDefectStationInfo(int id)
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


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = "Delete from DefectCode_Station  where ID= @ID";
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("ID").Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                              CommandType.StoredProcedure,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params);
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDefectStationUnique(string preStation, string curStation, string majorPart, string cause, string defect, string family)
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


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select top 1 ID
                                                                from DefectCode_Station
                                                                where   PRE_STN = @PRE_STN and
                                                                             CRT_STN = @CRT_STN and
                                                                             MajorPart = @MajorPart and
                                                                             Cause = @Cause and
                                                                             Defect = @Defect and
                                                                             Family = @Family";
                        sqlCtx.AddParam("PRE_STN", new SqlParameter("@PRE_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("CRT_STN", new SqlParameter("@CRT_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("MajorPart", new SqlParameter("@MajorPart", SqlDbType.VarChar));
                        sqlCtx.AddParam("Cause", new SqlParameter("@Cause", SqlDbType.VarChar));
                        sqlCtx.AddParam("Defect", new SqlParameter("@Defect", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("PRE_STN").Value = preStation;
                sqlCtx.Param("CRT_STN").Value = curStation;
                sqlCtx.Param("MajorPart").Value = majorPart;
                sqlCtx.Param("Cause").Value = cause;
                sqlCtx.Param("Defect").Value = defect;
                sqlCtx.Param("Family").Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.HasRows;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<DefectCodeNextStationInfo> GetNextStationFromDefectStation(string preStation, string curStation,
                                                                                                                        string majorPart, string cause,
                                                                                                                        string defect, string family,
                                                                                                                        string model)
        {
            try
            {
                IList<DefectCodeNextStationInfo> ret = new List<DefectCodeNextStationInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  ID ,NXT_STN,MajorPart,Cause,Defect,
                                                                       (case when MajorPart=@MajorPart and Cause=@Cause and Defect=@Defect then
                                                                               1
                                                                             when MajorPart=@MajorPart and Cause=@Cause and Defect='' then
                                                                               2
                                                                             when MajorPart=@MajorPart and Cause='' and Defect=@Defect then
                                                                               3
                                                                             when MajorPart='' and Cause=@Cause and Defect=@Defect then
                                                                               4
                                                                             when MajorPart=@MajorPart and Cause='' and Defect='' then
                                                                               5
                                                                             when MajorPart='' and Cause=@Cause and Defect='' then
                                                                               6
                                                                             when MajorPart='' and Cause='' and Defect=@Defect then
                                                                               7
                                                                             else
                                                                               8  
                                                                         end) as Priority,
                                                                           (case when Family=@Model then
                                                                               1
                                                                             when  Family=@Family then
                                                                               2                                                                            
                                                                             else
                                                                               3  
                                                                         end) as FamilyPriority                    
                                                                from DefectCode_Station
                                                                where PRE_STN=@PRE_STN and
                                                                      CRT_STN=@CRT_STN and
                                                                      (MajorPart='' or MajorPart=@MajorPart  ) and
                                                                      (Cause='' or Cause=@Cause) and
                                                                      (Defect='' or Defect=@Defect) and
                                                                      (Family='' or Family=@Model or 
                                                                        Family=@Family)
                                                                order by FamilyPriority, Priority";
                        sqlCtx.AddParam("PRE_STN", new SqlParameter("@PRE_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("CRT_STN", new SqlParameter("@CRT_STN", SqlDbType.VarChar));
                        sqlCtx.AddParam("MajorPart", new SqlParameter("@MajorPart", SqlDbType.VarChar));
                        sqlCtx.AddParam("Cause", new SqlParameter("@Cause", SqlDbType.VarChar));
                        sqlCtx.AddParam("Defect", new SqlParameter("@Defect", SqlDbType.VarChar));
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PRE_STN").Value = preStation;
                sqlCtx.Param("CRT_STN").Value = curStation;
                sqlCtx.Param("MajorPart").Value = majorPart;
                sqlCtx.Param("Cause").Value = cause;
                sqlCtx.Param("Defect").Value = defect;
                sqlCtx.Param("Model").Value = model;
                sqlCtx.Param("Family").Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DefectCodeNextStationInfo item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<DefectCodeNextStationInfo>(sqlR);
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
        #endregion


        #region for StationAttr table
        /// <summary>
        /// select  AttrName, Station, AttrValue, Descr, Editor, Cdt, Udt
        ///                                                    from StationAttr
        ///                                                    where Station =@Station
        //                                                    order by AttrName
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public IList<StationAttrDef> GetStationAttr(string station)
        {
            try
            {
                IList<StationAttrDef> ret = new List<StationAttrDef>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  AttrName, Station, AttrValue, Descr, Editor, Cdt, Udt
                                                            from StationAttr
                                                            where Station =@Station
                                                            order by AttrName ";
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                       
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Station").Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        StationAttrDef item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<StationAttrDef>(sqlR);
                        //IMES.Infrastructure.Repository._Schema.SQLData.TrimStringProperties(item);
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
        /// <summary>
        /// select  top 1 AttrValue
        ///                                                    from StationAttr
        ///                                                    where Station =@Station and
        ///                                                               AttrName =@AttrName
        //                                                    order by Udt desc
        /// </summary>
        /// <param name="station"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public String GetStationAttrValue(string station, string attrName)
        {
            try
            {
                string ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  top 1 AttrValue
                                                            from StationAttr
                                                            where Station =@Station and
                                                                       AttrName =@AttrName
                                                            order by Udt desc";
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("AttrName").Value = attrName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret=sqlR.GetString(0).Trim();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// select distinct Station from StationAttr where AttrName =@AttrName and AttrValue=@AttrValue order by Station
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        /// <returns></returns>
        public IList<string> GetStationByAttrNameValue(string attrName, string attrValue)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct Station
                                                            from StationAttr
                                                            where AttrName =@AttrName and
                                                                      AttrValue=@AttrValue
                                                            order by Station ";
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("AttrValue").Value = attrValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add( sqlR.GetString(0).Trim());
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// insert StationAttr( AttrName, Station, AttrValue, Descr, 
        ///                                                                                Editor, Cdt, Udt)
        ///                                                    values(@AttrName, @Station, @AttrValue, @Descr, 
        ///                                                                @Editor, GETDATE(), GETDATE())
        /// </summary>
        /// <param name="attr"></param>
        public void AddStationAttr(StationAttrDef attr)
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


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"insert StationAttr( AttrName, Station, AttrValue, Descr, 
                                                                                        Editor, Cdt, Udt)
                                                            values(@AttrName, @Station, @AttrValue, @Descr, 
                                                                        @Editor, GETDATE(), GETDATE())  ";
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Station").Value = attr.Station;
                sqlCtx.Param("AttrName").Value = attr.AttrName;

                sqlCtx.Param("AttrValue").Value = attr.AttrValue;
                sqlCtx.Param("Descr").Value = attr.Descr;
                sqlCtx.Param("Editor").Value = attr.Editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// update  StationAttr
       ///                                                     set    AttrValue=@AttrValue, 
       ///                                                            Descr=@Descr, 
       ///                                                            Editor=@Editor, 
       ///                                                            Udt=GETDATE()
       ///                                                     where Station = @Station and
       ///                                                            AttrName = @AttrName
        /// </summary>
        /// <param name="attr"></param>
        public void UpdateStationAttr(StationAttrDef attr)
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


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update  StationAttr
                                                            set    AttrValue=@AttrValue, 
                                                                   Descr=@Descr, 
                                                                   Editor=@Editor, 
                                                                   Udt=GETDATE()
                                                            where Station = @Station and
                                                                   AttrName = @AttrName ";
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Station").Value = attr.Station;
                sqlCtx.Param("AttrName").Value = attr.AttrName;

                sqlCtx.Param("AttrValue").Value = attr.AttrValue;
                sqlCtx.Param("Descr").Value = attr.Descr;
                sqlCtx.Param("Editor").Value = attr.Editor;            


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// delete from StationAttr
        ///                                                    where Station = @Station and
        ///                                                          AttrName = @AttrName
        /// </summary>
        /// <param name="station"></param>
        /// <param name="attrName"></param>
        public void DeleteStationAttr(string station, string attrName)
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


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"delete from StationAttr
                                                            where Station = @Station and
                                                                  AttrName = @AttrName";
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("AttrName").Value = attrName;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
