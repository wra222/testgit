using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBChangeLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.PCA
{
    ///<summary>
    /// Repository for MBChangeLog
    ///</summary>
    public class MBChangeLogRepository : BaseRepository<MBChangeLog>, IMBChangeLogRepository
    {
        #region Overrides of BaseRepository<MBChangeLog>

        protected override void PersistNewItem(MBChangeLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertMBChangeLog(item);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(MBChangeLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateMBChangeLog(item);

                    this.CheckAndInsertSubs(item, tracker);

                    this.CheckAndUpdateOrRemoveSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(MBChangeLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteMBChangeLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<MBChangeLog>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override MBChangeLog Find(object key)
        {
            try
            {
                MBChangeLog ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Change_PCB cond = new _Schema.Change_PCB();
                        cond.OldPCBNo = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Change_PCB), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Change_PCB.fn_OldPCBNo].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MBChangeLog();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_Editor]);
                        ret.NewSn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_NewPCBNo]);
                        ret.Sn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_OldPCBNo]);
                        ret.Reason = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_Reason]);
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
        public override IList<MBChangeLog> FindAll()
        {
            try
            {
                IList<MBChangeLog> ret = new List<MBChangeLog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Change_PCB));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MBChangeLog item = new MBChangeLog();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_Editor]);
                        item.NewSn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_NewPCBNo]);
                        item.Sn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_OldPCBNo]);
                        item.Reason = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Change_PCB.fn_Reason]);
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
        public override void Add(MBChangeLog item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public new void Remove(MBChangeLog item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public void Update(MBChangeLog item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        #endregion

        #region . Inners .

        private void PersistInsertMBChangeLog(MBChangeLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Change_PCB));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Change_PCB.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Change_PCB.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Change_PCB.fn_NewPCBNo].Value = item.NewSn;
                sqlCtx.Params[_Schema.Change_PCB.fn_OldPCBNo].Value = item.Sn;
                sqlCtx.Params[_Schema.Change_PCB.fn_Reason].Value = item.Reason;
                 _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMBChangeLog(MBChangeLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Change_PCB));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Change_PCB.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Change_PCB.fn_NewPCBNo].Value = item.NewSn;
                sqlCtx.Params[_Schema.Change_PCB.fn_OldPCBNo].Value = item.Sn;
                sqlCtx.Params[_Schema.Change_PCB.fn_Reason].Value = item.Reason;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMBChangeLog(MBChangeLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Change_PCB));
                    }
                }
                sqlCtx.Params[_Schema.Change_PCB.fn_OldPCBNo].Value = item.Sn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckAndInsertSubs(MBChangeLog item, StateTracker tracker)
        {

        }

        private void CheckAndUpdateOrRemoveSubs(MBChangeLog item, StateTracker tracker)
        {

        }

        #endregion

        #region IMBChangeLogRepository Members

        public IList<MBChangeLog> GetMBChangeLogs(MBChangeLog condition)
        {
            try
            {
                IList<MBChangeLog> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Change_PCB cond = FuncNew.SetColumnFromField<Change_PCB, MBChangeLog>(condition);
                var condSet = new ConditionCollection<Change_PCB>(false);
                condSet.Add(new EqualCondition<Change_PCB>(cond));
                sqlCtx = FuncNew.GetConditionedSelect<Change_PCB>(null, null, condSet, Change_PCB.fn_newPCBNo);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Change_PCB, MBChangeLog>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Change_PCB, MBChangeLog, MBChangeLog>(ret, sqlR, sqlCtx);
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
