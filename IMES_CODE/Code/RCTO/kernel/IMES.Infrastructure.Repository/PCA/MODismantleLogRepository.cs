using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace IMES.Infrastructure.Repository.PCA
{
    public class MODismantleLogRepository : BaseRepository<MODismantleLog>, IMODismantleLogRepository
    {
        #region Overrides of BaseRepository<IStation>

        protected override void PersistNewItem(MODismantleLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertMODismantleLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(MODismantleLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistModifyMODismantleLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(MODismantleLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteMODismantleLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<MODismantleLog>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override MODismantleLog Find(object key)
        {
            try
            {
                MODismantleLog ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MODismantleLog cond = new _Schema.MODismantleLog();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MODismantleLog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MODismantleLog.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MODismantleLog(
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_PCBNo]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Tp]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_SMTMO]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Reason]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Cdt]),
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_ID]));
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
        public override IList<MODismantleLog> FindAll()
        {
            try
            {
                IList<MODismantleLog> ret = new List<MODismantleLog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MODismantleLog));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MODismantleLog item = new MODismantleLog(
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_PCBNo]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Tp]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_SMTMO]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Reason]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_Cdt]),
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MODismantleLog.fn_ID]));
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
        /// <param name="uow"></param>
        public override void Add(MODismantleLog item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(MODismantleLog item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(MODismantleLog item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region IMODismantleLogRepository Members

        public void AddBatch(IList<MODismantleLog> items)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    IList<MODismantleLog> batch = new List<MODismantleLog>();
                    int i = 0;
                    foreach (MODismantleLog entry in items)
                    {
                        batch.Add(entry);
                        if ((i + 1) % batchSQLCnt == 0 || i == items.Count - 1)
                        {
                            AddBatch_Inner(batch);
                            batch.Clear();
                        }
                        entry.Tracker.Clear();
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Inners .

        private void AddBatch_Inner(IList<MODismantleLog> items)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    _Schema.SQLContextCollection sqlCtxSet = new _Schema.SQLContextCollection();

                    int i = 0;
                    foreach(MODismantleLog entry in items)
                    {
                        _Schema.SQLContext sqlCtx = ComposeForPersistInsertMODismantleLog(entry);
                        sqlCtxSet.AddOne(i, sqlCtx);
                        i++;
                    }
                    _Schema.SQLContext sqlCtxBatch = sqlCtxSet.MergeToOneNonQuery();
                    _Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtxBatch.Sentence, sqlCtxBatch.Params.Values.ToArray<SqlParameter>());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertMODismantleLog(MODismantleLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MODismantleLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.MODismantleLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.MODismantleLog.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.MODismantleLog.fn_ID].Value = item.Id;
                sqlCtx.Params[_Schema.MODismantleLog.fn_PCBNo].Value = item.PcbNo;
                sqlCtx.Params[_Schema.MODismantleLog.fn_Reason].Value = item.Reason;
                sqlCtx.Params[_Schema.MODismantleLog.fn_SMTMO].Value = item.SmtMo;
                sqlCtx.Params[_Schema.MODismantleLog.fn_Tp].Value = item.Tp;
                item.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private _Schema.SQLContext ComposeForPersistInsertMODismantleLog(MODismantleLog item)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MODismantleLog));
                }
            }
            DateTime cmDt = _Schema.SqlHelper.GetDateTime();
            sqlCtx.Params[_Schema.MODismantleLog.fn_Cdt].Value = cmDt;
            sqlCtx.Params[_Schema.MODismantleLog.fn_Editor].Value = item.Editor;
            //sqlCtx.Params[_Schema.MODismantleLog.fn_ID].Value = item.Id;
            sqlCtx.Params[_Schema.MODismantleLog.fn_PCBNo].Value = item.PcbNo;
            sqlCtx.Params[_Schema.MODismantleLog.fn_Reason].Value = item.Reason;
            sqlCtx.Params[_Schema.MODismantleLog.fn_SMTMO].Value = item.SmtMo;
            sqlCtx.Params[_Schema.MODismantleLog.fn_Tp].Value = item.Tp;
            return sqlCtx;
        }

        private void PersistModifyMODismantleLog(MODismantleLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MODismantleLog));
                    }
                }
                sqlCtx.Params[_Schema.MODismantleLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.MODismantleLog.fn_PCBNo].Value = item.PcbNo;
                sqlCtx.Params[_Schema.MODismantleLog.fn_Reason].Value = item.Reason;
                sqlCtx.Params[_Schema.MODismantleLog.fn_SMTMO].Value = item.SmtMo;
                sqlCtx.Params[_Schema.MODismantleLog.fn_Tp].Value = item.Tp;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMODismantleLog(MODismantleLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MODismantleLog));
                    }
                }
                sqlCtx.Params[_Schema.MODismantleLog.fn_ID].Value = item.Id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Defered

        public void AddBatchDefered(IUnitOfWork uow, IList<MODismantleLog> items)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), items);
        }

        #endregion
    }
}
