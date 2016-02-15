using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.Utility.Cache;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Line;
//using shema=IMES.Infrastructure.Repository._Schema;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Line相关
    /// </summary>
    public class LineRepository : BaseRepository<fons.Line>, ILineRepository, ICache
    {
        private static mtns::GetValueClass g = new mtns::GetValueClass();

        #region Cache
        private static IDictionary<string, fons.Line> _cache = new Dictionary<string, fons.Line>();
        private static IDictionary<string, IList<string>> _byWhateverIndex = new Dictionary<string, IList<string>>(); //customer, LineIds //customer + station, LineIds
        private static string preStr1 = _Schema.Func.MakeKeyForIdxPre(_Schema.Line.fn_CustomerID);
        private static string preStr2 = _Schema.Func.MakeKeyForIdxPre(_Schema.Line.fn_CustomerID, _Schema.Line_Station.fn_Station);        
        private static object _syncObj_cache = new object();
        #endregion

        #region Overrides of BaseRepository<Line>

        protected override void PersistNewItem(fons.Line item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertLine(item);

                    fons.LineEx lineEx = (fons.LineEx)item.GetType().GetField("_lineEx", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (lineEx != null )
                    {
                        switch (lineEx.Tracker.GetState(lineEx))
                            {
                                case DataRowState.Added:
                                    this.PersistInsertLineEx(lineEx);
                                    break;
                                case DataRowState.Modified:
                                    this.PersistUpdateLineEx(lineEx);
                                    break;
                                default:
                                    break;
                            }                        
                    }

                    IList<fons.LineSpeed> lineSpeeds = (IList<fons.LineSpeed>)item.GetType().GetField("_lineSpeed", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (lineSpeeds != null && lineSpeeds.Count > 0)
                    {
                        foreach (fons.LineSpeed info in lineSpeeds)
                        {
                            switch (info.Tracker.GetState(info))
                            {
                                case DataRowState.Added:
                                case DataRowState.Modified: 
                                    this.PersistInsertLineSpeed(info);
                                    break;
                                
                                    //this.PersistUpdateLineSpeed(info);
                                    //break;
                                default:
                                    break;
                            }
                        }
                    }

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(fons.Line item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified || item.GetRelationTableState()== DataRowState.Modified)
                {
                    if (tracker.GetState(item) == DataRowState.Modified)
                    {
                        this.PersistModifyLine(item);
                    }

                    fons.LineEx lineEx = (fons.LineEx)item.GetType().GetField("_lineEx", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (lineEx != null)
                    {
                        switch (lineEx.Tracker.GetState(lineEx))
                        {
                            case DataRowState.Added:
                                this.PersistInsertLineEx(lineEx);
                                break;
                            case DataRowState.Modified:
                                this.PersistUpdateLineEx(lineEx);
                                break;
                            default:
                                break;
                        }
                    }


                    IList<fons.LineSpeed> lineSpeeds = (IList<fons.LineSpeed>)item.GetType().GetField("_lineSpeed", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (lineSpeeds != null && lineSpeeds.Count > 0)
                    {
                        foreach (fons.LineSpeed info in lineSpeeds)
                        {
                            switch (info.Tracker.GetState(info))
                            {
                                case DataRowState.Added:
                                case DataRowState.Modified:
                                    this.PersistInsertLineSpeed( info);
                                    break;
                               
                                    //this.PersistUpdateLineSpeed(info);
                                    //break;
                                default:
                                    break;
                            }
                        }
                    }


                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(fons.Line item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    

                    if (item.LineEx != null)
                    {
                        this.PersistDeleteLineSpeed(item.LineEx);

                        item.GetType().GetField("_lineSpeed", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, null);
                        this.PersistDeleteLineEx(item);
                        item.GetType().GetField("_lineEx", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, null);
                    }
                    this.PersistDeleteLine(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Line>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.Line Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                fons.Line ret = Find_Cache(key);
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
        public override IList<fons.Line> FindAll()
        {
            try
            {
                if (!IsCached())
                    return FindAll_DB();

                IList<fons.Line> ret = FindAll_Cache();
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
        public override void Add(fons.Line item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(fons.Line item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons.Line item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of ILineRepository

        ///// <summary>
        ///// Get PdLine List from database
        ///// </summary>
        ///// <param name="stationId">Station Identifier</param>
        ///// <returns>PdLine Info List</returns>
        //public IList<PdLineInfo> GetPdLineList(string stationId)
        //{

        //}

        public IList<PdLineInfo> GetPdLineList(string customerId, string stationId)
        {
            try
            {
                if (!IsCached())
                    return GetPdLineList_DB(customerId, stationId);

                IList<PdLineInfo> ret = GetPdLineList_Cache(customerId, stationId);
                if (ret == null || ret.Count < 1)
                {
                    ret = GetPdLineList_DB(customerId, stationId);
                }
                return (from item in ret orderby item.friendlyName select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PdLineInfo> GetAllPdLineListByCust(string customerId)
        {
            try
            {
                if (!IsCached())
                    return GetAllPdLineListByCust_DB(customerId);

                IList<PdLineInfo> ret = GetAllPdLineListByCust_Cache(customerId);
                if (ret == null || ret.Count < 1)
                {
                    ret = GetAllPdLineListByCust_DB(customerId);
                }
                return (from item in ret orderby item.friendlyName select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<LineInfo> GetAllPdLineListByStage(string stage)
        {
            try
            {
                IList<LineInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!mtns.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Line cond = new _Metas.Line();
                        cond.stage = stage;
                        sqlCtx = mtns.FuncNew.GetConditionedSelect<_Metas.Line>(tk, null, null, new mtns.ConditionCollection<_Metas.Line>(new mtns.EqualCondition<_Metas.Line>(cond)), _Metas.Line.fn_line);
                    }
                }
                sqlCtx.Param(_Metas.Line.fn_stage).Value = stage;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns.FuncNew.SetFieldFromColumn<_Metas.Line, LineInfo, LineInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetLineByCustomerAndStage(string customer, string stage)
        {
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line cond = new _Schema.Line();
                        cond.CustomerID = customer;
                        cond.Stage = stage;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), null, new List<string>() { _Schema.Line.fn_line, _Schema.Line.fn_Descr }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Line.fn_line);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_CustomerID].Value = customer;
                sqlCtx.Params[_Schema.Line.fn_Stage].Value = stage;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DOAListInfo> GetDOAListListByDn(DOAListInfo condition)
        {
            try
            {
                IList<DOAListInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                        mtns::Doalist cond = mtns::FuncNew.SetColumnFromField<mtns::Doalist, DOAListInfo>(condition);
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Doalist>(null, null, new mtns::ConditionCollection<mtns::Doalist>(new mtns::EqualCondition<mtns::Doalist>(cond)), mtns::Doalist.fn_id);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Doalist, DOAListInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_DOA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Doalist, DOAListInfo, DOAListInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDOAList(PoDataInfo setValue, PoDataInfo condition)
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
                        mtns::PoData cond = mtns::FuncNew.SetColumnFromField<mtns::PoData, PoDataInfo>(condition);
                        mtns::PoData setv = mtns::FuncNew.SetColumnFromField<mtns::PoData, PoDataInfo>(setValue);
                        setv.udt = DateTime.Now;

                        sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::PoData>(new mtns::SetValueCollection<mtns::PoData>(new mtns::CommonSetValue<mtns::PoData>(setv)), new mtns::ConditionCollection<mtns::PoData>(new mtns::EqualCondition<mtns::PoData>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PoData, PoDataInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PoData, PoDataInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::PoData.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_DOA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetLinesByStage(string stage)
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line cond = new _Schema.Line();
                        cond.Stage = stage;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), null, new List<string>() { _Schema.Line.fn_line }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Line.fn_line);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_Stage].Value = stage;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_line]);
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

        public IList<LineInfo> GetPdLineListByStageAndCustomer(string stage, string customer)
        {
            try
            {
                IList<LineInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!mtns.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Line cond = new _Metas.Line();
                        cond.stage = stage;
                        cond.customerID = customer;
                        sqlCtx = mtns.FuncNew.GetConditionedSelect<_Metas.Line>(tk, null, null, new mtns.ConditionCollection<_Metas.Line>(new mtns.EqualCondition<_Metas.Line>(cond)), _Metas.Line.fn_line);
                    }
                }
                sqlCtx.Param(_Metas.Line.fn_stage).Value = stage;
                sqlCtx.Param(_Metas.Line.fn_customerID).Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns.FuncNew.SetFieldFromColumn<_Metas.Line, LineInfo, LineInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.Common.Station.IStation> GetStationListByLineAndStationType(string line, string stationType, string status)
        {
            try
            {
                IList<IMES.FisObject.Common.Station.IStation> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Station>();
                        _Metas.Station cond = new _Metas.Station();
                        cond.stationType = stationType;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Station>(cond, "RTRIM({0})"));
                        //tf1.AddRangeToGetFieldNames(_Metas.Station.fn_station, _Metas.Station.fn_descr);

                        tf2 = new TableAndFields<_Metas.Line_Station>();
                        _Metas.Line_Station cond2 = new _Metas.Line_Station();
                        cond2.status = status;
                        cond2.line = line;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Line_Station>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Station, _Metas.Line_Station>(tf1, _Metas.Station.fn_station, tf2, _Metas.Line_Station.fn_station));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t1." + _Metas.Station.fn_station, "t1." + _Metas.Station.fn_descr);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Station.fn_stationType)).Value = stationType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, Line_Station.fn_status)).Value = status;
                sqlCtx.Param(g.DecAlias(tf2.Alias, Line_Station.fn_line)).Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Station, IMES.FisObject.Common.Station.Station, IMES.FisObject.Common.Station.IStation>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.Common.Station.IStation> GetStationListByLineAndStationType(string line, string stationType)
        {
            try
            {
                IList<IMES.FisObject.Common.Station.IStation> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Station>();
                        _Metas.Station cond = new _Metas.Station();
                        cond.stationType = stationType;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Station>(cond, "RTRIM({0})"));
                        //tf1.AddRangeToGetFieldNames(_Metas.Station.fn_station, _Metas.Station.fn_descr);

                        tf2 = new TableAndFields<_Metas.Line_Station>();
                        _Metas.Line_Station cond2 = new _Metas.Line_Station();
                        cond2.line = line;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Line_Station>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Station, _Metas.Line_Station>(tf1, _Metas.Station.fn_station, tf2, _Metas.Line_Station.fn_station));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t1." + _Metas.Station.fn_station, "t1." + _Metas.Station.fn_descr);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Station.fn_stationType)).Value = stationType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, Line_Station.fn_line)).Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Station, IMES.FisObject.Common.Station.Station, IMES.FisObject.Common.Station.IStation>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<LineInfo> GetLineListByStationAndCustomer(string station, string customer)
        {
            try
            {
                IList<LineInfo> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Line>();
                        _Metas.Line cond = new _Metas.Line();
                        cond.customerID = customer;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Line>(cond));

                        tf2 = new TableAndFields<_Metas.Line_Station>();
                        _Metas.Line_Station cond2 = new _Metas.Line_Station();
                        cond2.station = station;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Line_Station>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Line, _Metas.Line_Station>(tf1, _Metas.Line.fn_line, tf2, _Metas.Line_Station.fn_line));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts, "t1." + _Metas.Line.fn_descr);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Line.fn_customerID)).Value = customer;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Line_Station.fn_station)).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Line, LineInfo, LineInfo>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetLinePrefixListByStage(string stage)
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
                        _Metas.Line cond = new _Metas.Line();
                        cond.stage = stage;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Line>(tk, "DISTINCT", new string[][] { new string[] { _Metas.Line.fn_line, string.Format("LEFT({0},1)", _Metas.Line.fn_line) } }, new ConditionCollection<_Metas.Line>(new EqualCondition<_Metas.Line>(cond)), string.Format("LEFT({0},1)", _Metas.Line.fn_line));
                    }
                }
                sqlCtx.Param(_Metas.Line.fn_stage).Value = stage;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Line.fn_line));
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

        public IList<string> GetLinePrefixListByStages(string[] stages)
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
                        _Metas.Line cond = new _Metas.Line();
                        cond.stage = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Line>(tk, "DISTINCT", new string[][] { new string[] { _Metas.Line.fn_line, string.Format("LEFT({0},1)", _Metas.Line.fn_line) } }, new ConditionCollection<_Metas.Line>(new InSetCondition<_Metas.Line>(cond)), string.Format("LEFT({0},1)", _Metas.Line.fn_line));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Line.fn_stage), g.ConvertInSet(new List<string>(stages)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Line.fn_line));
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

        public IList<string> GetLinePrefixListByStages(string[] stages, string customer)
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
                        _Metas.Line cond = new _Metas.Line();
                        cond.stage = "[INSET]";
                        _Metas.Line cond2 = new _Metas.Line();
                        cond2.customerID = customer;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Line>(tk, "DISTINCT", new string[][] { new string[] { _Metas.Line.fn_line, string.Format("LEFT({0},1)", _Metas.Line.fn_line) } }, new ConditionCollection<_Metas.Line>(
                            new InSetCondition<_Metas.Line>(cond),
                            new EqualCondition<_Metas.Line>(cond2)), string.Format("LEFT({0},1)", _Metas.Line.fn_line));
                    }
                }
                sqlCtx.Param(_Metas.Line.fn_customerID).Value = customer;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Line.fn_stage), g.ConvertInSet(new List<string>(stages)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Line.fn_line));
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

        public IList<IMES.FisObject.Common.Station.IStation> GetStationListByStage(string stage)
        {
            try
            {
                IList<IMES.FisObject.Common.Station.IStation> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Station>();

                        tf2 = new TableAndFields<_Metas.Line_Station>();
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<_Metas.Line>();
                        _Metas.Line cond = new _Metas.Line();
                        cond.stage = stage;
                        tf3.Conditions.Add(new EqualCondition<_Metas.Line>(cond));
                        tf3.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Station, _Metas.Line_Station>(tf1, _Metas.Station.fn_station, tf2, _Metas.Line_Station.fn_station),
                            new TableConnectionItem<_Metas.Line, _Metas.Line_Station>(tf1, _Metas.Line.fn_line, tf2, _Metas.Line_Station.fn_line));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t1." + _Metas.Station.fn_station);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.Line.fn_stage)).Value = stage;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Station, IMES.FisObject.Common.Station.Station, IMES.FisObject.Common.Station.IStation>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void UpdateDOAListDefered(IUnitOfWork uow, PoDataInfo setValue, PoDataInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        #endregion

        #endregion

        #region . Inners .

        private fons.Line Find_DB(object key)
        {
            try
            {
                fons.Line ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line cond = new _Schema.Line();
                        cond.line = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_line].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.Line();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Cdt]);
                        ret.CustomerId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_CustomerID]);
                        ret.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Descr]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Editor]);
                        ret.Id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_line]);
                        ret.StageId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Stage]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Udt]);
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

        private fons.Line Find_Cache(object key)
        {
            try
            {
                fons.Line ret = null;
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

        private IList<fons.Line> FindAll_DB()
        {
            try
            {
                IList<fons.Line> ret = new List<fons.Line>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Line item = new fons.Line();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Cdt]);
                        item.CustomerId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_CustomerID]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Editor]);
                        item.Id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_line]);
                        item.StageId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Stage]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Udt]);
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

        private IList<_Schema.Line_Station> FindAllLineStationRelations_DB(string lineId)
        {
            try
            {
                IList<_Schema.Line_Station> ret = new List<_Schema.Line_Station>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line_Station cond = new _Schema.Line_Station();
                        cond.Line = lineId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line_Station), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Line_Station.fn_Line].Value = lineId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        _Schema.Line_Station item = new _Schema.Line_Station();
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Line_Station.fn_ID]);
                        item.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line_Station.fn_Line]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line_Station.fn_Station]);
                        item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line_Station.fn_Status]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line_Station.fn_Editor]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line_Station.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line_Station.fn_Udt]);
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

        private IList<fons.Line> FindAll_Cache()
        {
            try
            {
                lock (_syncObj_cache)
                {
                    return _cache.Values.ToList<fons.Line>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<PdLineInfo> GetPdLineList_DB(string customerId, string stationId)
        {
            try
            {
                return Convert(FindByStation_DB(customerId, stationId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<PdLineInfo> GetPdLineList_Cache(string customerId, string stationId)
        {
            try
            {
                IList<PdLineInfo> ret = new List<PdLineInfo>();
                string key = _Schema.Func.MakeKeyForIdx(preStr2, customerId, stationId);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (string pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                                ret.Add(Convert(_cache[pk]));
                        }
                    }
                }
                if (ret != null)
                {
                    ret = (from item in ret orderby item.friendlyName select item).ToList();
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<PdLineInfo> GetAllPdLineListByCust_DB(string customerId)
        {
            try
            {
                return Convert(FindByCustomer_DB(customerId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<PdLineInfo> GetAllPdLineListByCust_Cache(string customerId)
        {
            try
            {
                IList<PdLineInfo> ret = new List<PdLineInfo>();
                string key = _Schema.Func.MakeKeyForIdx(preStr1, customerId);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (string pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                                ret.Add(Convert(_cache[pk]));
                        }
                    }
                }
                if (ret != null)
                {
                    ret = (from item in ret orderby item.friendlyName select item).ToList();
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons.Line> FindByStation_DB(string customerId, string stationId)
        {
            try
            {
                IList<fons.Line> ret = new List<fons.Line>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Line);
                        _Schema.Line eqCond1 = new _Schema.Line();
                        eqCond1.CustomerID = customerId;
                        tf1.equalcond = eqCond1;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Line_Station);
                        _Schema.Line_Station equalCond = new _Schema.Line_Station();
                        equalCond.Station = stationId;
                        tf2.equalcond = equalCond;
                        tf2.ToGetFieldNames = null;
                        //tf2.ToGetFieldNames.Add(_Schema.Line_Station.fn_Station);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(new _Schema.TableConnectionItem[] { 
                                                new _Schema.TableConnectionItem(tf1, _Schema.Line.fn_line, tf2, _Schema.Line_Station.fn_Line) });

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Line.fn_Descr));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_CustomerID)].Value = customerId;
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Line_Station.fn_Station)].Value = stationId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Line entry = new fons.Line();
                        entry.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_Cdt)]);
                        entry.CustomerId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_CustomerID)]);
                        entry.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_Descr)]);
                        entry.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_Editor)]);
                        entry.Id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_line)]);
                        entry.StageId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_Stage)]);
                        entry.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Line.fn_Udt)]);
                        entry.Tracker.Clear();
                        ret.Add(entry);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons.Line> FindByCustomer_DB(string customerId)
        {
            try
            {
                IList<fons.Line> ret = new List<fons.Line>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line cond = new _Schema.Line();
                        cond.CustomerID = customerId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Line.fn_Descr);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_CustomerID].Value = customerId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Line item = new fons.Line();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Cdt]);
                        item.CustomerId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_CustomerID]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Editor]);
                        item.Id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_line]);
                        item.StageId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Stage]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Line.fn_Udt]);
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

        private void PersistInsertLine(fons.Line item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Line.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Line.fn_CustomerID].Value = item.CustomerId;
                sqlCtx.Params[_Schema.Line.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Line.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Line.fn_line].Value = item.Id;
                sqlCtx.Params[_Schema.Line.fn_Stage].Value = item.StageId;
                sqlCtx.Params[_Schema.Line.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistModifyLine(fons.Line item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Line.fn_CustomerID].Value = item.CustomerId;
                sqlCtx.Params[_Schema.Line.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Line.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Line.fn_line].Value = item.Id;
                sqlCtx.Params[_Schema.Line.fn_Stage].Value = item.StageId;
                sqlCtx.Params[_Schema.Line.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteLine(fons.Line item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line));
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_line].Value = item.Id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertLineEx(fons.LineEx info)
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
                        sqlCtx.Sentence = @"insert LineEx(Line, AliasLine, AvgSpeed, AvgManPower, AvgStationQty, 
                                                                                  [Owner], IEOwner, Editor, Udt)
                                                                    values( @Line, @AliasLine, @AvgSpeed, @AvgManPower, @AvgStationQty, 
                                                                            @Owner, @IEOwner, @Editor, GETDATE())    ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("AliasLine", new SqlParameter("@AliasLine", SqlDbType.VarChar));
                        sqlCtx.AddParam("AvgSpeed", new SqlParameter("@AvgSpeed", SqlDbType.Int));
                        sqlCtx.AddParam("AvgManPower", new SqlParameter("@AvgManPower", SqlDbType.Int));
                        sqlCtx.AddParam("AvgStationQty", new SqlParameter("@AvgStationQty", SqlDbType.Int));
                        sqlCtx.AddParam("Owner", new SqlParameter("@Owner", SqlDbType.VarChar));
                        sqlCtx.AddParam("IEOwner", new SqlParameter("@IEOwner", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = info.Line;
                sqlCtx.Param("AliasLine").Value = info.AliasLine;
                sqlCtx.Param("AvgSpeed").Value = info.AvgSpeed;
                sqlCtx.Param("AvgManPower").Value = info.AvgManPower;
                sqlCtx.Param("AvgStationQty").Value = info.AvgStationQty;
                sqlCtx.Param("Owner").Value = info.Owner;
                sqlCtx.Param("IEOwner").Value = info.IEOwner;
                sqlCtx.Param("Editor").Value = info.Editor;              
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                info.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateLineEx(fons.LineEx info)
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
                        sqlCtx.Sentence = @"update  LineEx
                                                                  set AliasLine = @AliasLine,
                                                                        AvgSpeed=@AvgSpeed,
                                                                        AvgManPower=@AvgManPower,
                                                                        AvgStationQty = @AvgStationQty,
                                                                         [Owner] =@Owner,
                                                                         IEOwner=@IEOwner,
                                                                         Editor=@Editor,
                                                                         Udt =GETDATE()
                                                                 where Line= @Line";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("AliasLine", new SqlParameter("@AliasLine", SqlDbType.VarChar));
                        sqlCtx.AddParam("AvgSpeed", new SqlParameter("@AvgSpeed", SqlDbType.Int));
                        sqlCtx.AddParam("AvgManPower", new SqlParameter("@AvgManPower", SqlDbType.Int));
                        sqlCtx.AddParam("AvgStationQty", new SqlParameter("@AvgStationQty", SqlDbType.Int));
                        sqlCtx.AddParam("Owner", new SqlParameter("@Owner", SqlDbType.VarChar));
                        sqlCtx.AddParam("IEOwner", new SqlParameter("@IEOwner", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = info.Line;
                sqlCtx.Param("AliasLine").Value = info.AliasLine;
                sqlCtx.Param("AvgSpeed").Value = info.AvgSpeed;
                sqlCtx.Param("AvgManPower").Value = info.AvgManPower;
                sqlCtx.Param("AvgStationQty").Value = info.AvgStationQty;
                sqlCtx.Param("Owner").Value = info.Owner;
                sqlCtx.Param("IEOwner").Value = info.IEOwner;
                sqlCtx.Param("Editor").Value = info.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                info.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteLineEx(fons.Line info)
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
                        sqlCtx.Sentence = @"delete from   LineEx
                                                             where Line= @Line";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        //sqlCtx.AddParam("AliasLine", new SqlParameter("@AliasLine", SqlDbType.VarChar));
                        //sqlCtx.AddParam("AvgSpeed", new SqlParameter("@AvgSpeed", SqlDbType.Int));
                        //sqlCtx.AddParam("AvgManPower", new SqlParameter("@AvgManPower", SqlDbType.Int));
                        //sqlCtx.AddParam("AvgStationQty", new SqlParameter("@AvgStationQty", SqlDbType.Int));
                        //sqlCtx.AddParam("Owner", new SqlParameter("@Owner", SqlDbType.VarChar));
                        //sqlCtx.AddParam("IEOwner", new SqlParameter("@IEOwner", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = info.Id;
                //sqlCtx.Param("AliasLine").Value = info.AliasLine;
                //sqlCtx.Param("AvgSpeed").Value = info.AvgSpeed;
                //sqlCtx.Param("AvgManPower").Value = info.AvgManPower;
                //sqlCtx.Param("AvgStationQty").Value = info.AvgStationQty;
                //sqlCtx.Param("Owner").Value = info.Owner;
                //sqlCtx.Param("IEOwner").Value = info.IEOwner;
                //sqlCtx.Param("Editor").Value = info.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                info.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertLineSpeed(fons.LineSpeed info)
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
                        sqlCtx.Sentence = @"merge LineSpeed as T
                                                            using (select @Station, @AliasLine) 
                                                                       as S(Station,AliasLine)
                                                              on  T.Station= S.Station and
                                                                    T.AliasLine=S.AliasLine
                                                             When Matched then
                                                                Update Set LimitSpeed= @LimitSpeed,
                                                                        IsCheckPass = @IsCheckPass,
                                                                        LimitSpeedExpression= @LimitSpeedExpression,
                                                                        IsHoldStation =  @IsHoldStation,
                                                                        Editor = @Editor,
                                                                        Udt =GETDATE()
                                                             When Not Matched then
                                                                insert (Station, AliasLine, LimitSpeed, IsCheckPass, LimitSpeedExpression, 
					                                                              IsHoldStation, Editor, Udt)
	                                                             values(@Station, @AliasLine, @LimitSpeed, @IsCheckPass, @LimitSpeedExpression, 
					                                                              @IsHoldStation, @Editor, GETDATE()) ;";
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("AliasLine", new SqlParameter("@AliasLine", SqlDbType.VarChar));
                        sqlCtx.AddParam("LimitSpeed", new SqlParameter("@LimitSpeed", SqlDbType.Int));
                        sqlCtx.AddParam("IsCheckPass", new SqlParameter("@IsCheckPass", SqlDbType.VarChar));
                        sqlCtx.AddParam("LimitSpeedExpression", new SqlParameter("@LimitSpeedExpression", SqlDbType.VarChar));
                        sqlCtx.AddParam("IsHoldStation", new SqlParameter("@IsHoldStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                       

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Station").Value = info.Station;
                sqlCtx.Param("AliasLine").Value = info.AliasLine;
                sqlCtx.Param("LimitSpeed").Value = info.LimitSpeed;
                sqlCtx.Param("IsCheckPass").Value = info.IsCheckPass.ToString();
                sqlCtx.Param("LimitSpeedExpression").Value = new  CastSpeedExpression().ConvertTo( info.LimitSpeedExpression,typeof(string));
                sqlCtx.Param("IsHoldStation").Value = info.IsHoldStation.ToString();
                sqlCtx.Param("Editor").Value = info.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                info.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

//        private void PersistUpdateLineSpeed(fons.LineSpeed info)
//        {
//            try
//            {
//                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"update   LineSpeed
//                                                                 set    LimeSpeed= @LimeSpeed,
//                                                                          PassLimitSpeed = @PassLimitSpeed,
//                                                                          LimitSpeedExpression= @LimitSpeedExpression,
//                                                                          IsHoldStation =  @IsHoldStation,
//                                                                          Editor = @Editor,
//                                                                          Udt =GETDATE()
//                                                                 where   Station=@Station  and
//                                                                         AliasLine =@AliasLine";
//                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AliasLine", new SqlParameter("@AliasLine", SqlDbType.VarChar));
//                        sqlCtx.AddParam("LimeSpeed", new SqlParameter("@LimeSpeed", SqlDbType.Int));
//                        sqlCtx.AddParam("PassLimitSpeed", new SqlParameter("@PassLimitSpeed", SqlDbType.Int));
//                        sqlCtx.AddParam("LimitSpeedExpression", new SqlParameter("@LimitSpeedExpression", SqlDbType.VarChar));
//                        sqlCtx.AddParam("IsHoldStation", new SqlParameter("@IsHoldStation", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("Station").Value = info.Station;
//                sqlCtx.Param("AliasLine").Value = info.AliasLine;
//                sqlCtx.Param("LimeSpeed").Value = info.LimeSpeed;
//                sqlCtx.Param("PassLimitSpeed").Value = info.PassLimitSpeed;
//                sqlCtx.Param("LimitSpeedExpression").Value = info.LimitSpeedExpression;
//                sqlCtx.Param("IsHoldStation").Value = info.IsHoldStation;
//                sqlCtx.Param("Editor").Value = info.Editor;
//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                 CommandType.Text,
//                                                                                 sqlCtx.Sentence,
//                                                                                 sqlCtx.Params);
//                info.Tracker.Clear();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

        private void PersistDeleteLineSpeed(fons.LineEx info)
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
                        sqlCtx.Sentence = @" delete LineSpeed
                                                             where not exists(select a.Line 
                                                                                           from  Line a 
                                                                                           inner join LineEx b on a.Line=b.Line
                                                                                          where  a.Line != @Line and
                                                                                                 b.AliasLine = @AliasLine) and
                                                                    AliasLine=@AliasLine  ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("AliasLine", new SqlParameter("@AliasLine", SqlDbType.VarChar));
                        //sqlCtx.AddParam("LimeSpeed", new SqlParameter("@LimeSpeed", SqlDbType.Int));
                        //sqlCtx.AddParam("PassLimitSpeed", new SqlParameter("@PassLimitSpeed", SqlDbType.Int));
                        //sqlCtx.AddParam("LimitSpeedExpression", new SqlParameter("@LimitSpeedExpression", SqlDbType.VarChar));
                        //sqlCtx.AddParam("IsHoldStation", new SqlParameter("@IsHoldStation", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = info.Line;
                sqlCtx.Param("AliasLine").Value = info.AliasLine;
                //sqlCtx.Param("LimeSpeed").Value = info.LimeSpeed;
                //sqlCtx.Param("PassLimitSpeed").Value = info.PassLimitSpeed;
                //sqlCtx.Param("LimitSpeedExpression").Value = info.LimitSpeedExpression;
                //sqlCtx.Param("IsHoldStation").Value = info.IsHoldStation;
                //sqlCtx.Param("Editor").Value = info.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                info.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<PdLineInfo> Convert(IList<fons.Line> innerList)
        {
            IList<PdLineInfo> ret = new List<PdLineInfo>();
            if (innerList != null && innerList.Count > 0)
            {
                foreach (fons.Line ln in innerList)
                {
                    ret.Add(Convert(ln));
                }
            }
            return ret;
        }

        private PdLineInfo Convert(fons.Line ln)
        {
            PdLineInfo item = new PdLineInfo();
            item.friendlyName = ln.Descr;
            item.id = ln.Id;
            return item;
        }

        #endregion

        #region ICache Members

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Line);
        }

        public void ProcessItem(CacheUpdateInfo item)
        {
            LoadAllCache();
        }

        private void LoadAllCache()
        {
            IList<fons.Line> lns = this.FindAll_DB();
            if (lns != null && lns.Count > 0)
            {
                lock (_syncObj_cache)
                {
                    _cache.Clear();
                    _byWhateverIndex.Clear();

                    foreach (fons.Line ln in lns)
                    {
                        _cache.Add(ln.Id, ln);

                        //Regist index by customer
                        Regist(ln.Id, _Schema.Func.MakeKeyForIdx(preStr1, ln.CustomerId));

                        //Regist index by customer + station
                        IList<_Schema.Line_Station> rlts = FindAllLineStationRelations_DB(ln.Id);
                        if (rlts != null && rlts.Count > 0)
                        {
                            foreach(_Schema.Line_Station rlt in rlts)
                            {
                                Regist(ln.Id, _Schema.Func.MakeKeyForIdx(preStr2, ln.CustomerId, rlt.Station ));
                            }
                        }
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

        private CacheUpdateInfo GetACacheSignal()
        {
            CacheUpdateInfo ret = new CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = CacheType.Line;
            return ret;
        }

        #endregion

        #region For Maintain

        public DataTable GetPaLineStationListByLine(string line)
        {
            //select a.ID,a.Station,b.StationType,a.Status,b.Descr,a.Editor,a.Cdt,a.Udt  
            //from Line_Station a join Station b on a.Station=b.Station  
            //where a.Line=? 
            //Order by a.Cdt 
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Station);
                        tf1.ToGetFieldNames.Add(_Schema.Station.fn_StationType);
                        tf1.ToGetFieldNames.Add(_Schema.Station.fn_Descr);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Line_Station);
                        _Schema.Line_Station equalCond = new _Schema.Line_Station();
                        equalCond.Line = line;
                        tf2.equalcond = equalCond;

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(new _Schema.TableConnectionItem[] { 
                                                new _Schema.TableConnectionItem(tf1, _Schema.Station.fn_station, tf2, _Schema.Line_Station.fn_Station) });

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf2.alias, _Schema.Line_Station.fn_Cdt));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Line_Station.fn_Line)].Value = line;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Line_Station.fn_ID)],
                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Line_Station.fn_Station)],
                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.Station.fn_StationType)],
                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Line_Station.fn_Status)],
                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.Station.fn_Descr)],
                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Line_Station.fn_Editor)],
                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Line_Station.fn_Cdt)],
                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Line_Station.fn_Udt)]
                                                                });
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetAllStage()
        {
            //select Stage from Stage order by Stage 
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Stage), null, new List<string>(){ _Schema.Stage.fn_stage }, null, null, null, null, null, null, null, null);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Stage.fn_stage]);
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

        public IList<string> GetLineByCustAndStage(string Cust, string stage)
        {
            //select Line from Line where CustomerID=? and Stage=? order by Line 
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line cond = new _Schema.Line();
                        cond.CustomerID = Cust;
                        cond.Stage = stage;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), null, new List<string>() { _Schema.Line.fn_line }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Line.fn_line);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_CustomerID].Value = Cust;
                sqlCtx.Params[_Schema.Line.fn_Stage].Value = stage;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Line.fn_line]);
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

        public void DeleteLineStationByID(int id)
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
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line_Station));
                    }
                }
                sqlCtx.Params[_Schema.Line_Station.fn_ID].Value = id;
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

        public bool IFLineStationIsExists(string line, string station)
        {
            //select * from Line_Station where Line=? and Station=? 
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line_Station cond = new _Schema.Line_Station();
                        cond.Line = line;
                        cond.Station = station;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line_Station), "COUNT", new List<string>() { _Schema.Line_Station.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Line_Station.fn_Line].Value = line;
                sqlCtx.Params[_Schema.Line_Station.fn_Station].Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                        ret = cnt > 0 ? true : false;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddLineStation(LineStation lineStation)
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
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line_Station));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Line_Station.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Line_Station.fn_Editor].Value = lineStation.Editor;
                sqlCtx.Params[_Schema.Line_Station.fn_Line].Value = lineStation.Line;
                sqlCtx.Params[_Schema.Line_Station.fn_Station].Value = lineStation.Station;
                sqlCtx.Params[_Schema.Line_Station.fn_Status].Value = lineStation.Status;
                sqlCtx.Params[_Schema.Line_Station.fn_Udt].Value = cmDt;
                lineStation.ID = System.Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                
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

        public void UpdateLineStation(LineStation lineStation)
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
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line_Station));
                    }
                }
                sqlCtx.Params[_Schema.Line_Station.fn_ID].Value = lineStation.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Line_Station.fn_Editor].Value = lineStation.Editor;
                sqlCtx.Params[_Schema.Line_Station.fn_Line].Value = lineStation.Line;
                sqlCtx.Params[_Schema.Line_Station.fn_Station].Value = lineStation.Station;
                sqlCtx.Params[_Schema.Line_Station.fn_Status].Value = lineStation.Status;
                sqlCtx.Params[_Schema.Line_Station.fn_Udt].Value = cmDt;
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

        public DataTable GetLineInfoList(string customer, string stage)
        {
            // SELECT [Line]
            //       ,[Descr]
            //       ,[Editor]
            //       ,[Cdt]
            //       ,[Udt]
            //       ,[CustomerID]
            //       ,[Stage]
            //   FROM [IMES_GetData_Datamaintain].[dbo].[Line]
            // where [CustomerID]='CustomerID' AND [Stage]='stage'
            // order by [Line]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Line cond = new _Schema.Line();
                        cond.CustomerID = customer;
                        cond.Stage = stage;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), null, new List<string>() { _Schema.Line.fn_line, _Schema.Line.fn_Descr, _Schema.Line.fn_Editor, _Schema.Line.fn_Cdt, _Schema.Line.fn_Udt, _Schema.Line.fn_CustomerID, _Schema.Line.fn_Stage }, cond, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Line.fn_line);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_CustomerID].Value = customer;
                sqlCtx.Params[_Schema.Line.fn_Stage].Value = stage;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray());
                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Line.fn_line],
                                                                sqlCtx.Indexes[_Schema.Line.fn_Descr],
                                                                sqlCtx.Indexes[_Schema.Line.fn_Editor],
                                                                sqlCtx.Indexes[_Schema.Line.fn_Cdt],
                                                                sqlCtx.Indexes[_Schema.Line.fn_Udt],
                                                                sqlCtx.Indexes[_Schema.Line.fn_CustomerID],
                                                                sqlCtx.Indexes[_Schema.Line.fn_Stage]});
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetStageList()
        {
            // SELECT [Stage]
            //   FROM [IMES_GetData_Datamaintain].[dbo].[Stage]
            // order by [Stage]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Stage), null, new List<string>() { _Schema.Stage.fn_stage }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Stage.fn_stage);
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

        public void DeleteLine(string line)
        {
            fons.Line info = new fons.Line();
            info.Id =  line;
            PersistDeleteLineEx(info);
            inner_deleteLine(line);
        }

        private void inner_deleteLine(string line)
        {
            try
            {
                //SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line));
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_line].Value = line;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                //SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.Dispose();
                //SqlTransactionManager.End();
            }
        }

        public bool IsExistLine(string line)
        {
            // IF EXISTS(
            // SELECT [Line]
            //         FROM [IMES_GetData_Datamaintain].[dbo].[Line]
            // where [Line]='line'
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
                        _Schema.Line cond = new _Schema.Line();
                        cond.line = line;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_line].Value = line;
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

        public void AddLine(fons.Line item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                this.PersistInsertLine(item);

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

        public void UpdateLine(fons.Line item, string oldLineId)
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
                        _Schema.Line cond = new _Schema.Line();
                        cond.line = oldLineId;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Line), new List<string>() { _Schema.Line.fn_CustomerID,_Schema.Line.fn_Descr,_Schema.Line.fn_Editor,_Schema.Line.fn_line,_Schema.Line.fn_Stage }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Line.fn_line].Value = oldLineId;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Line.fn_CustomerID)].Value = item.CustomerId;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Line.fn_Descr)].Value = item.Descr;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Line.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Line.fn_line)].Value = item.Id;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Line.fn_Stage)].Value = item.StageId;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Line.fn_Udt)].Value = cmDt;
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

        public bool IsExistLineDescr(string descr)
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
                        _Metas.Line cond = new _Metas.Line();
                        cond.descr = descr;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Line>(tk, "COUNT", new string[] { _Metas.Line.fn_line },
                            new ConditionCollection<_Metas.Line>(
                                new EqualCondition<_Metas.Line>(cond))
                            );
                    }
                }
                sqlCtx.Param(mtns::Line.fn_descr).Value = descr;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0 ? true : false;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsExistLineDescrExceptLine(string descr, string line)
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
                        _Metas.Line cond = new _Metas.Line();
                        cond.descr = descr;
                        _Metas.Line cond2 = new _Metas.Line();
                        cond2.line = line;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Line>(tk, "COUNT", new string[] { _Metas.Line.fn_line },
                            new ConditionCollection<_Metas.Line>(
                                new EqualCondition<_Metas.Line>(cond),
                                new NotEqualCondition<_Metas.Line>(cond2))
                            );
                    }
                }
                sqlCtx.Param(mtns::Line.fn_descr).Value = descr;
                sqlCtx.Param(mtns::Line.fn_line).Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0 ? true : false;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void DeleteLineStationByIDDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddLineStationDefered(IUnitOfWork uow, LineStation lineStation)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), lineStation);
        }

        public void UpdateLineStationDefered(IUnitOfWork uow, LineStation lineStation)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), lineStation);
        }

        public void DeleteLineDefered(IUnitOfWork uow, string line)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), line);
        }

        public void AddLineDefered(IUnitOfWork uow, fons.Line item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateLineDefered(IUnitOfWork uow, fons.Line item, string oldLineId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldLineId);
        }

        #endregion

        #endregion

        #region for Fill Data
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public void FillLineSpeed(fons.Line line)
        {
            try
            {
                IList<fons.LineSpeed> ret = new List<fons.LineSpeed>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.Station, a.AliasLine, a.LimitSpeed, a.IsCheckPass, a.LimitSpeedExpression, 
	                                                                   a.IsHoldStation, a.Editor, a.Udt
                                                                  from LineSpeed a 
                                                                  inner join LineEx b on a.AliasLine = b.AliasLine 
                                                                 where b.Line=@Line 
                                                                 order by a.Station ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = line.Id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.Line.LineSpeed item = _Schema.SQLData.ToObjectWithAttribute<IMES.FisObject.Common.Line.LineSpeed>(sqlR);
                        item.Tracker.Clear();
                        ret.Add(item);

                    }

                }
                line.GetType().GetField("_lineSpeed", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(line, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillLineEx(fons.Line line)
        {
            try
            {
                fons.LineEx ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Line, AliasLine, AvgSpeed, AvgManPower, AvgStationQty, 
                                                                       [Owner], IEOwner, Editor, Udt
                                                                from LineEx
                                                                where Line =@Line
                                                                order by AliasLine  ";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = line.Id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = _Schema.SQLData.ToObject<fons.LineEx>(sqlR);
                        ret.Tracker.Clear();                      

                    }

                }
                line.GetType().GetField("_lineEx", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(line, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region for LineSpeed Insert/Delete/Update
        public DataTable GetLineByStage(string customer, string stage)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.Line, a.CustomerID,a.Stage, a.Descr, 
                                                                       isnull(b.AliasLine,'') as AliasLine,isnull(b.AvgManPower,0) as AvgManPower, 
                                                                       isnull(b.AvgSpeed,0) as AvgSpeed ,isnull(b.AvgStationQty,0) as AvgStationQty, 
                                                                        isnull(b.IEOwner,'') as IEOwner , isnull(b.Owner,'') as Owner, 
                                                                        a.Editor, a.Cdt, a.Udt      
                                                                 from Line a
                                                                 left join  LineEx b on a.Line = b.Line 
                                                                where  a.CustomerID = @Customer and
                                                                      a.Stage =@Stage  ";
                        sqlCtx.AddParam("Customer", new SqlParameter("@Customer", SqlDbType.VarChar));
                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Customer").Value = customer;
                sqlCtx.Param("Stage").Value = stage;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<fons.LineSpeed> GetLineSpeedByLineStation(string customer, string stage, string station)
        {
            try
            {
                IList<fons.LineSpeed> ret = new List<fons.LineSpeed>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct d.Station, d.AliasLine, d.LimitSpeedExpression, 
                                                                                d.LimitSpeed, d.IsHoldStation, 
                                                                                d.IsCheckPass, d.Editor, d.Udt
                                                                                from Line a, 
                                                                                        LineEx b, 
                                                                                         Line_Station c, 
                                                                                         LineSpeed d
                                                                                where a.Line = b.Line and
                                                                                      b.Line = c.Line and
                                                                                      b.AliasLine = d.AliasLine and
                                                                                      a.CustomerID = @Customer and 
                                                                                      a.Stage=@Stage and 
                                                                                      d.Station=@Station      
                                                                                order by d.AliasLine";
                        sqlCtx.AddParam("Customer", new SqlParameter("@Customer", SqlDbType.VarChar));
                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Customer").Value = customer;
                sqlCtx.Param("Stage").Value = stage;
                sqlCtx.Param("Station").Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.Line.LineSpeed item = _Schema.SQLData.ToObjectWithAttribute<IMES.FisObject.Common.Line.LineSpeed>(sqlR);
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
        public void AddLineSpeed(fons.LineSpeed lineSpeed)
        {
            PersistInsertLineSpeed(lineSpeed);
        }
        public void UpdateLineSpeed(fons.LineSpeed lineSpeed)
        {
            PersistInsertLineSpeed(lineSpeed);
        }
        public void RemoveLineSpeedByStation(string station)
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
                        sqlCtx.Sentence = @" delete LineSpeed
                                                             where   Station=@Station  ";

                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));                     

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Station").Value = station;              
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RemoveLineSpeed(string aliasLine, string station)
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
                        sqlCtx.Sentence = @" delete LineSpeed
                                                             where Station=@Station and  
                                                                        AliasLine=@AliasLine";

                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("AliasLine", new SqlParameter("@AliasLine", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("AliasLine").Value = aliasLine;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddLineSpeedDefered(IUnitOfWork uow, LineSpeed lineSpeed)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), lineSpeed);
        }
        public void UpdateLineSpeedDefered(IUnitOfWork uow, LineSpeed lineSpeed)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), lineSpeed);
        }
        public void RemoveLineSpeedDefered(IUnitOfWork uow, string aliasLine, string station)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), aliasLine,station);
        }

        public void RemoveLineSpeedByStationDefered(IUnitOfWork uow,  string station)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),  station);
        }
        public IList<string> GetAllAliasLine()
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
                        sqlCtx.Sentence = @"select distinct AliasLine
                                                              from  LineEx      
                                                            order by AliasLine";
                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                } 
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence))
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
        #endregion
    }
}
