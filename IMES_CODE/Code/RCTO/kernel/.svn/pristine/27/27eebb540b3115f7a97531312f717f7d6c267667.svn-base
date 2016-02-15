using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.Infrastructure.Util;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Metas;
using IMES.DataModel;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: COA相关
    /// </summary>
    public class COARepository : BaseRepository<IMES.FisObject.PAK.COA.COAStatus>, ICOAStatusRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<COAStatus>

        protected override void PersistNewItem(IMES.FisObject.PAK.COA.COAStatus item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertCOAStatus(item);

                    this.CheckAndInsertSubs(item,tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(IMES.FisObject.PAK.COA.COAStatus item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateCOAStatus(item);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(IMES.FisObject.PAK.COA.COAStatus item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteCOAStatus(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<COAStatus>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IMES.FisObject.PAK.COA.COAStatus Find(object key)
        {
            try
            {
                IMES.FisObject.PAK.COA.COAStatus ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.COAStatus cond = new _Schema.COAStatus();
                        cond.COASN = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.COAStatus), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.COAStatus.fn_COASN].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new IMES.FisObject.PAK.COA.COAStatus();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Cdt]);
                        ret.COASN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_COASN]);
                        ret.IECPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_IECPN]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Editor]);
                        ret.LineID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Line]);
                        ret.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Status]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Udt]);
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

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public override IList<IMES.FisObject.PAK.COA.COAStatus> FindAll()
        {
            try
            {
                IList<IMES.FisObject.PAK.COA.COAStatus> ret = new List<IMES.FisObject.PAK.COA.COAStatus>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.COAStatus));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.PAK.COA.COAStatus item = new IMES.FisObject.PAK.COA.COAStatus();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Cdt]);
                        item.COASN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_COASN]);
                        item.IECPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_IECPN]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Editor]);
                        item.LineID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Line]);
                        item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Status]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.COAStatus.fn_Udt]);
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

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        public override void Add(IMES.FisObject.PAK.COA.COAStatus item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(IMES.FisObject.PAK.COA.COAStatus item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(IMES.FisObject.PAK.COA.COAStatus item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of ICOAStatusRepository

        public IMES.FisObject.PAK.COA.COAStatus FillCOALogs(IMES.FisObject.PAK.COA.COAStatus item)
        {
            try
            {
                IList<COALog> newFieldVal = new List<COALog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.COALog cond = new _Schema.COALog();
                        cond.COASN = item.COASN;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.COALog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.COALog.fn_COASN].Value = item.COASN;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        COALog coalog = new COALog();
                        coalog.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.COALog.fn_Cdt]);
                        coalog.COASN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COALog.fn_COASN]);
                        coalog.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COALog.fn_Editor]);
                        coalog.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.COALog.fn_ID]);
                        coalog.LineID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COALog.fn_Line]);
                        coalog.StationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COALog.fn_Station]);
                        coalog.Tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.COALog.fn_Tp]);
                        coalog.Tracker.Clear();
                        coalog.Tracker = item.Tracker;
                        newFieldVal.Add(coalog);
                    }
                }
                item.GetType().GetField("_coaLogs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, newFieldVal);

                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCOAMas(COAMasInfo item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<Coamas>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Coamas, COAMasInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Coamas.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertCSNLog(CSNLogInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Csnlog>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Csnlog, CSNLogInfo>(sqlCtx, item);

                sqlCtx.Param(Csnlog.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCSNMas(CSNMasInfo item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<Csnmas>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Csnmas, CSNMasInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Csnmas.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PAK.COA.COAStatus> GetCOAStatusRange(string beginNo, string endNo)
        {
            try
            {
                IList<IMES.FisObject.PAK.COA.COAStatus> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Coastatus cond = new _Metas.Coastatus();
                        cond.coasn = beginNo;

                        _Metas.Coastatus cond2 = new _Metas.Coastatus();
                        cond2.coasn = endNo;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Coastatus>(tk, null, null, new ConditionCollection<_Metas.Coastatus>(new GreaterOrEqualCondition<_Metas.Coastatus>(cond), new SmallerOrEqualCondition<_Metas.Coastatus>(cond2)), _Metas.Coastatus.fn_coasn);
                    }
                }
                sqlCtx.Param(g.DecGE(_Metas.Coastatus.fn_coasn)).Value = beginNo;
                sqlCtx.Param(g.DecSE(_Metas.Coastatus.fn_coasn)).Value = endNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Coastatus, IMES.FisObject.PAK.COA.COAStatus, IMES.FisObject.PAK.COA.COAStatus>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertCOALog(COALog newLog)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Coalog>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Coalog, COALog>(sqlCtx, newLog);

                sqlCtx.Param(Coalog.fn_cdt).Value = cmDt;

                newLog.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertCOATransLog(COATransLog newLog)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Coatrans_Log>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Coatrans_Log, COATransLog>(sqlCtx, newLog);

                sqlCtx.Param(Coatrans_Log.fn_cdt).Value = cmDt;

                newLog.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetStationOfNewestCOALog(string coano)
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
                        Coalog cond = new Coalog();
                        cond.coasn = coano;
                        sqlCtx = FuncNew.GetConditionedSelect<Coalog>(tk, "TOP 1", new string[] { Coalog.fn_station }, new ConditionCollection<Coalog>(new EqualCondition<Coalog>(cond)), Coalog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Coalog.fn_coasn).Value = coano;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(Coalog.fn_station));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCOAStatus(COAStatus item)
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
                        Coastatus cond = new Coastatus();
                        cond.coasn = item.COASN;

                        Coastatus setv = new Coastatus();
                        setv = FuncNew.SetColumnFromField<Coastatus, COAStatus>(item, Coastatus.fn_cdt, Coastatus.fn_coasn);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Coastatus>(tk, new SetValueCollection<Coastatus>(new CommonSetValue<Coastatus>(setv)), new ConditionCollection<Coastatus>(new EqualCondition<Coastatus>(cond)));
                    }
                }
                sqlCtx.Param(Coastatus.fn_coasn).Value = item.COASN;

                sqlCtx = FuncNew.SetColumnFromField<Coastatus, COAStatus>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Coastatus.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<CSNMasInfo> GetCSNMasRange(string beginNo, string endNo)
        {
            try
            {
                IList<CSNMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Csnmas cond = new Csnmas();
                        cond.csn2 = beginNo;

                        Csnmas cond2 = new Csnmas();
                        cond2.csn2 = endNo;

                        sqlCtx = FuncNew.GetConditionedSelect<Csnmas>(tk, null, null, new ConditionCollection<Csnmas>(new GreaterOrEqualCondition<Csnmas>(cond), new SmallerOrEqualCondition<Csnmas>(cond2)), Csnmas.fn_csn2);
                    }
                }
                sqlCtx.Param(g.DecGE(Csnmas.fn_csn2)).Value = beginNo;
                sqlCtx.Param(g.DecSE(Csnmas.fn_csn2)).Value = endNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Csnmas, CSNMasInfo, CSNMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveTXTIntoTmpTable(IList<TmpTableInfo> dataLst)
        {
            SqlTransactionManager.Begin();
            try
            {
                if (dataLst != null && dataLst.Count > 0)
                {
                    for (int i = 0; i < dataLst.Count; i++)
                    {
                        TmpTableInfo item = dataLst[i];
                        SQLContextNew sqlCtx = ComposeForInsertTmpTable(item);
                        item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                    }
                }
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

        private SQLContextNew ComposeForInsertTmpTable(TmpTableInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<_Metas.TmpTable>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.TmpTable, TmpTableInfo>(sqlCtx, item);
                return sqlCtx;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveTmpTableItem(string pc)
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
                        _Metas.TmpTable cond = new _Metas.TmpTable();
                        cond.pc = pc;
                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.TmpTable>(tk, new ConditionCollection<_Metas.TmpTable>(new EqualCondition<_Metas.TmpTable>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.TmpTable.fn_pc).Value = pc;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TmpTableInfo> GetTmpTableItem(string pc)
        {
            try
            {
                IList<TmpTableInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        TmpTable cond = new TmpTable();
                        cond.pc = pc;
                        sqlCtx = FuncNew.GetConditionedSelect<TmpTable>(tk, null, null, new ConditionCollection<TmpTable>(new EqualCondition<TmpTable>(cond)), TmpTable.fn_id);
                    }
                }
                sqlCtx.Param(TmpTable.fn_pc).Value = pc;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<TmpTable, TmpTableInfo, TmpTableInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveItemIntoCOAReceiveTable(IList<COAReceive> items)
        {
            SqlTransactionManager.Begin();
            try
            {
                if (items != null && items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        COAReceive item = items[i];
                        SQLContextNew sqlCtx = ComposeForInsertCOAReceive(item);
                        item.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                    }
                }
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

        private SQLContextNew ComposeForInsertCOAReceive(COAReceive item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<_Metas.Coareceive>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.Coareceive, COAReceive>(sqlCtx, item);
                return sqlCtx;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveItemIntoCOAStatusTable(IList<COAStatus> items)
        {
            SqlTransactionManager.Begin();
            try
            {
                if (items != null && items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        COAStatus item = items[i];
                        SQLContextNew sqlCtx = ComposeForInsertCOAStatus(item);
                        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                    }
                }
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

        private SQLContextNew ComposeForInsertCOAStatus(COAStatus item)
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
                        sqlCtx = FuncNew.GetCommonInsert<_Metas.Coastatus>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.Coastatus, COAStatus>(sqlCtx, item);
                return sqlCtx;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<COAReceive> GetAllCOAReceivingItems()
        {
            try
            {
                IList<COAReceive> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<_Metas.Coareceive>(tk, _Metas.Coareceive.fn_begSN, _Metas.Coareceive.fn_endSN);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Coareceive, COAReceive, COAReceive>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public COAStatus GetCoaStatus(string coa_sn)
        {
            try
            {
                COAStatus ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Coastatus cond = new Coastatus();
                        cond.coasn = coa_sn;
                        sqlCtx = FuncNew.GetConditionedSelect<Coastatus>(tk, "TOP 1", null, new ConditionCollection<Coastatus>(
                            new EqualCondition<Coastatus>(cond)), Coastatus.fn_udt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Coastatus.fn_coasn).Value = coa_sn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Coastatus, COAStatus>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CSNMasInfo GetCsnMas(string csn2)
        {
            try
            {
                CSNMasInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Csnmas cond = new Csnmas();
                        cond.csn2 = csn2;
                        sqlCtx = FuncNew.GetConditionedSelect<Csnmas>(tk, "TOP 1", null, new ConditionCollection<Csnmas>(
                            new EqualCondition<Csnmas>(cond)), Csnmas.fn_udt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Csnmas.fn_csn2).Value = csn2;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Csnmas, CSNMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<COAStatus> GetCOAStatusListByStatusAndIecPn(string[] statuses, string iecPn, DateTime cdtEnd, string coaSnEnd)
        {
            try
            {
                IList<COAStatus> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Coastatus cond = new _Metas.Coastatus();
                        cond.status = "[INSET]";

                        _Metas.Coastatus cond2 = new _Metas.Coastatus();
                        cond2.iecpn = iecPn;

                        _Metas.Coastatus cond3 = new _Metas.Coastatus();
                        cond3.cdt = DateTime.Now;
                        cond3.coasn = coaSnEnd;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Coastatus>(tk, null, null, new ConditionCollection<_Metas.Coastatus>(
                            new InSetCondition<_Metas.Coastatus>(cond), 
                            new EqualCondition<_Metas.Coastatus>(cond2),
                            new SmallerCondition<_Metas.Coastatus>(cond3)), _Metas.Coastatus.fn_coasn);
                    }
                }
                sqlCtx.Param(_Metas.Coastatus.fn_iecpn).Value = iecPn;
                sqlCtx.Param(g.DecS(_Metas.Coastatus.fn_cdt)).Value = cdtEnd;
                sqlCtx.Param(g.DecS(_Metas.Coastatus.fn_coasn)).Value = coaSnEnd;
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Coastatus.fn_status), g.ConvertInSet(statuses));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Coastatus, COAStatus, COAStatus>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertCSNMas(CSNMasInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Csnmas>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Csnmas, CSNMasInfo>(sqlCtx, item);

                sqlCtx.Param(Csnmas.fn_cdt).Value = cmDt;
                sqlCtx.Param(Csnmas.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetMaxCSN1FromCSNMas(CSNMasInfo condition)
        {
            try
            {
                string ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Csnmas cond = FuncNew.SetColumnFromField<Csnmas, CSNMasInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<Csnmas>("MAX", new string[] { Csnmas.fn_csn1 }, new ConditionCollection<Csnmas>(new EqualCondition<Csnmas>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Csnmas, CSNMasInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes("MAX"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCSNMas(CSNMasInfo setValue, CSNMasInfo condition)
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
                mtns::Csnmas cond = mtns::FuncNew.SetColumnFromField<mtns::Csnmas, CSNMasInfo>(condition);
                mtns::Csnmas setv = mtns::FuncNew.SetColumnFromField<mtns::Csnmas, CSNMasInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Csnmas>(new mtns::SetValueCollection<mtns::Csnmas>(new mtns::CommonSetValue<mtns::Csnmas>(setv)), new mtns::ConditionCollection<mtns::Csnmas>(new mtns::EqualCondition<mtns::Csnmas>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Csnmas, CSNMasInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Csnmas, CSNMasInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Csnmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCOAReturnInfo(COAReturnInfo setValue, COAReturnInfo condition)
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
                mtns::Coareturn cond = mtns::FuncNew.SetColumnFromField<mtns::Coareturn, COAReturnInfo>(condition);
                mtns::Coareturn setv = mtns::FuncNew.SetColumnFromField<mtns::Coareturn, COAReturnInfo>(setValue);
                setv.udt = DateTime.Now;
                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Coareturn>(new mtns::SetValueCollection<mtns::Coareturn>(new mtns::CommonSetValue<mtns::Coareturn>(setv)), new mtns::ConditionCollection<mtns::Coareturn>(new mtns::EqualCondition<mtns::Coareturn>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Coareturn, COAReturnInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Coareturn, COAReturnInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Coareturn.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<COAReturnInfo> GetCOAReturnInfoList(COAReturnInfo eqCondition, COAReturnInfo isNullCondition)
        {
            try
            {
                IList<COAReturnInfo> ret = null;

                if (eqCondition == null)
                    eqCondition = new COAReturnInfo();
                if (isNullCondition == null)
                    isNullCondition = new COAReturnInfo();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {

                _Metas.Coareturn cond = FuncNew.SetColumnFromField<_Metas.Coareturn, COAReturnInfo>(eqCondition);
                _Metas.Coareturn cond2 = FuncNew.SetColumnFromField<_Metas.Coareturn, COAReturnInfo>(isNullCondition);

                sqlCtx = FuncNew.GetConditionedSelect<_Metas.Coareturn>(null, null, new ConditionCollection<_Metas.Coareturn>(new EqualCondition<_Metas.Coareturn>(cond), new EqualCondition<_Metas.Coareturn>(cond2,"ISNULL({0},'')")), _Metas.Coareturn.fn_id);
                var sqlCtx2 = FuncNew.GetConditionedSelect<_Metas.Coareturn>(null, new string[] { _Metas.Coareturn.fn_id }, new ConditionCollection<_Metas.Coareturn>(new EqualCondition<_Metas.Coareturn>(cond2, "ISNULL({0},'')")));

                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<_Metas.Coareturn, COAReturnInfo>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.Coareturn, COAReturnInfo>(sqlCtx2, isNullCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Coareturn, COAReturnInfo, COAReturnInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertEcoareturn(EcoareturnInfo item)
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
                        sqlCtx = FuncNew.GetCommonInsert<Ecoareturn>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Ecoareturn, EcoareturnInfo>(sqlCtx, item);

                sqlCtx.Param(Ecoareturn.fn_cdt).Value = cmDt;
                sqlCtx.Param(Ecoareturn.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertCOAReturn(COAReturnInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Coareturn>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Coareturn, COAReturnInfo>(sqlCtx, item);

                sqlCtx.Param(Coareturn.fn_cdt).Value = cmDt;
                sqlCtx.Param(Coareturn.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered  .

        public void UpdateCOAMasDefered(IUnitOfWork uow, IMES.DataModel.COAMasInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertCSNLogDefered(IUnitOfWork uow, IMES.DataModel.CSNLogInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateCSNMasDefered(IUnitOfWork uow, IMES.DataModel.CSNMasInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertCOALogDefered(IUnitOfWork uow, COALog newLog)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), newLog);
        }

        public void InsertCOATransLogDefered(IUnitOfWork uow, COATransLog newLog)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), newLog);
        }

        public void UpdateCOAStatusDefered(IUnitOfWork uow, IMES.FisObject.PAK.COA.COAStatus item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SaveTXTIntoTmpTableDefered(IUnitOfWork uow, IList<TmpTableInfo> dataLst)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dataLst);
        }

        public void RemoveTmpTableItemDefered(IUnitOfWork uow, string pc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pc);
        }

        public void SaveItemIntoCOAReceiveTableDefered(IUnitOfWork uow, IList<COAReceive> items)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), items);
        }

        public void SaveItemIntoCOAStatusTableDefered(IUnitOfWork uow, IList<COAStatus> items)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), items);
        }

        public void InsertCSNMasDefered(IUnitOfWork uow, CSNMasInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateCSNMasDefered(IUnitOfWork uow, CSNMasInfo setValue, CSNMasInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateCOAReturnInfoDefered(IUnitOfWork uow, COAReturnInfo setValue, COAReturnInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void InsertEcoareturnDefered(IUnitOfWork uow, EcoareturnInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertCOAReturnDefered(IUnitOfWork uow, COAReturnInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #endregion

        #region . Inners .

        private void PersistInsertCOAStatus(IMES.FisObject.PAK.COA.COAStatus item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.COAStatus));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.COAStatus.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.COAStatus.fn_COASN].Value = item.COASN;
                sqlCtx.Params[_Schema.COAStatus.fn_IECPN].Value = item.IECPN;
                sqlCtx.Params[_Schema.COAStatus.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.COAStatus.fn_Line].Value = item.LineID;
                sqlCtx.Params[_Schema.COAStatus.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.COAStatus.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateCOAStatus(IMES.FisObject.PAK.COA.COAStatus item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.COAStatus));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.COAStatus.fn_COASN].Value = item.COASN;
                sqlCtx.Params[_Schema.COAStatus.fn_IECPN].Value = item.IECPN;
                sqlCtx.Params[_Schema.COAStatus.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.COAStatus.fn_Line].Value = item.LineID;
                sqlCtx.Params[_Schema.COAStatus.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.COAStatus.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteCOAStatus(IMES.FisObject.PAK.COA.COAStatus item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery));
                    }
                }
                sqlCtx.Params[_Schema.COAStatus.fn_COASN].Value = item.COASN;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckAndInsertSubs(IMES.FisObject.PAK.COA.COAStatus item, StateTracker tracker)
        {
            IList<COALog> lstCoaLg = (IList<COALog>)item.GetType().GetField("_coaLogs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstCoaLg != null && lstCoaLg.Count > 0)//(item.CoaLogs.Count > 0)
            {
                foreach (COALog coalg in lstCoaLg)//item.CoaLogs)
                {
                    if (tracker.GetState(coalg) == DataRowState.Added)
                    {
                        coalg.COASN = item.COASN;
                        this.PersistInsertCOALog(coalg);
                    }
                }
            }
        }

        private void PersistInsertCOALog(COALog newLog)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.COALog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.COALog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.COALog.fn_COASN].Value = newLog.COASN;
                sqlCtx.Params[_Schema.COALog.fn_Editor].Value = newLog.Editor;
                //sqlCtx.Params[_Schema.COALog.fn_ID].Value = newLog.ID;
                sqlCtx.Params[_Schema.COALog.fn_Line].Value = newLog.LineID;
                sqlCtx.Params[_Schema.COALog.fn_Station].Value = newLog.StationID;
                sqlCtx.Params[_Schema.COALog.fn_Tp].Value = newLog.Tp;
                newLog.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
