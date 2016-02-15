using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.ReprintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Repository._Metas;
using omtns = IMES.Infrastructure.Repository._Schema;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: ReprintLog相关
    /// </summary>
    public class ReprintLogRepository : BaseRepository<ReprintLog>, IReprintLogRepository
    {
        #region Overrides of BaseRepository<ReprintLog>

        protected override void PersistNewItem(ReprintLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertReprintLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(ReprintLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateReprintLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(ReprintLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteReprintLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<PrintLog>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override ReprintLog Find(object key)
        {
            try
            {
                ReprintLog ret = null;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        omtns.RePrintLog cond = new omtns.RePrintLog();
                        cond.ID = (int)key;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(omtns.RePrintLog), cond, null, null);
                    }
                }
                sqlCtx.Params[omtns.RePrintLog.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new ReprintLog();
                        ret.BegNo = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_BegNo]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Cdt]);
                        ret.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Descr]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Editor]);
                        ret.EndNo = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_EndNo]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_ID]);
                        ret.LabelName = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_LabelName]);
                        ret.Reason = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Reason]);
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
        public override IList<ReprintLog> FindAll()
        {
            try
            {
                IList<ReprintLog> ret = new List<ReprintLog>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(omtns.RePrintLog));
                    }
                }
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ReprintLog item = new ReprintLog();
                        item.BegNo = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_BegNo]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Cdt]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Editor]);
                        item.EndNo = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_EndNo]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_ID]);
                        item.LabelName = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_LabelName]);
                        item.Reason = GetValue_Str(sqlR, sqlCtx.Indexes[omtns.RePrintLog.fn_Reason]);
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
        public override void Add(ReprintLog item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(ReprintLog item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(ReprintLog item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region . Inners .

        private void PersistInsertReprintLog(ReprintLog item)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(omtns.RePrintLog));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[omtns.RePrintLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[omtns.RePrintLog.fn_BegNo].Value = item.BegNo;
                sqlCtx.Params[omtns.RePrintLog.fn_Descr].Value = item.Descr;
                sqlCtx.Params[omtns.RePrintLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[omtns.RePrintLog.fn_EndNo].Value = item.EndNo;
                //sqlCtx.Params[omtns.RePrintLog.fn_ID].Value = item.ID;
                sqlCtx.Params[omtns.RePrintLog.fn_LabelName].Value = item.LabelName;
                sqlCtx.Params[omtns.RePrintLog.fn_Reason].Value = item.Reason;
                item.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateReprintLog(ReprintLog item)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(omtns.RePrintLog));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[omtns.RePrintLog.fn_BegNo].Value = item.BegNo;
                sqlCtx.Params[omtns.RePrintLog.fn_Descr].Value = item.Descr;
                sqlCtx.Params[omtns.RePrintLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[omtns.RePrintLog.fn_EndNo].Value = item.EndNo;
                sqlCtx.Params[omtns.RePrintLog.fn_ID].Value = item.ID;
                sqlCtx.Params[omtns.RePrintLog.fn_LabelName].Value = item.LabelName;
                sqlCtx.Params[omtns.RePrintLog.fn_Reason].Value = item.Reason;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteReprintLog(ReprintLog item)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(omtns.RePrintLog));
                    }
                }
                sqlCtx.Params[omtns.RePrintLog.fn_ID].Value = item.ID;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region IReprintLogRepository Members

        public bool CheckExistReprintLogByLabelNameAndDescr(string labelName, string descr)
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
                        _Metas.RePrintLog cond = new _Metas.RePrintLog();
                        cond.labelName = labelName;
                        cond.descr = descr;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.RePrintLog>(tk, "COUNT", new string[] { _Metas.RePrintLog.fn_id }, new ConditionCollection<_Metas.RePrintLog>(new EqualCondition<_Metas.RePrintLog>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.RePrintLog.fn_labelName).Value = labelName;
                sqlCtx.Param(_Metas.RePrintLog.fn_descr).Value = descr;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        #endregion
    }
}
