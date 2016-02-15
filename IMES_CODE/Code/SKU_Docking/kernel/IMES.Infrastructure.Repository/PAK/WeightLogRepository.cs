using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMES.FisObject.PAK.WeightLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.Infrastructure.Util;
using System.Reflection;
using System.Data.SqlClient;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: WeightLog相关
    /// </summary>
    public class WeightLogRepository : BaseRepository<WeightLog>, IWeightLogRepository
    {
        #region Overrides of BaseRepository<WeightLog>

        protected override void PersistNewItem(WeightLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertWeightLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(WeightLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateWeightLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(WeightLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteWeightLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<WeightLog>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override WeightLog Find(object key)
        {
            try
            {
                WeightLog ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.WeightLog cond = new _Schema.WeightLog();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.WeightLog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.WeightLog.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new WeightLog();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Editor]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_ID]);
                        ret.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Line]);
                        ret.SN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_SN]);
                        ret.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Station]);
                        ret.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Weight]);
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
        public override IList<WeightLog> FindAll()
        {
            try
            {
                IList<WeightLog> ret = new List<WeightLog>();
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.WeightLog));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        WeightLog item = new WeightLog();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_ID]);
                        item.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Line]);
                        item.SN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_SN]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Station]);
                        item.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.WeightLog.fn_Weight]);
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
        public override void Add(WeightLog item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(WeightLog item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(WeightLog item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region Implementation of IWeightLogRepository

        public int GetCountOfFRUWeightLog(string sn)
        {
            //select count(*) from IMES_PAK..FRUWeightLog where SN=''
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUWeightLog cond = new _Schema.FRUWeightLog();
                        cond.SN = sn;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUWeightLog), "COUNT", new List<string>() { _Schema.FRUWeightLog.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUWeightLog.fn_SN].Value = sn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public void AddOrModifyFRUWeightLog(IMES.DataModel.FRUWeightLog item)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (PeekFRUWeightLog(item))
                    UpdateFRUWeightLogBySn(item);
                else
                    InsertFRUWeightLog(item);

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

        private bool PeekFRUWeightLog(IMES.DataModel.FRUWeightLog item)
        {
            SqlDataReader sqlR = null;
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUWeightLog cond = new _Schema.FRUWeightLog();
                        cond.SN = item.SN;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUWeightLog), "COUNT", new List<string>() { _Schema.FRUWeightLog.fn_ID }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.FRUWeightLog.fn_SN].Value = item.SN;
                //using (SqlDataReader 
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                //)
                //{
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                        ret = cnt > 0 ? true : false;
                    }
                //}
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlR != null)
                {
                    sqlR.Close();
                }
            }
        }

        private void UpdateFRUWeightLogBySn(IMES.DataModel.FRUWeightLog item)
        {
            //update IMES_PAK..FRUWeightLog Set Weight='',Cdt=getdate() where SN=''
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUWeightLog cond = new _Schema.FRUWeightLog();
                        cond.SN = item.SN;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUWeightLog), new List<string>() { _Schema.FRUWeightLog.fn_Editor, _Schema.FRUWeightLog.fn_Line, _Schema.FRUWeightLog.fn_Station, _Schema.FRUWeightLog.fn_Weight, _Schema.FRUWeightLog.fn_Cdt }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUWeightLog.fn_SN].Value = item.SN;
                //sqlCtx.Params[_Schema.FRUWeightLog.fn_ID].Value = item.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUWeightLog.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUWeightLog.fn_Line)].Value = item.Line;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUWeightLog.fn_Station)].Value = item.Station;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUWeightLog.fn_Weight)].Value = item.Weight;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUWeightLog.fn_Cdt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertFRUWeightLog(IMES.DataModel.FRUWeightLog item)
        {
            //insert into IMES_PAK..FRUWeightLog (SN,Weight,Line,Station,Editor,Cdt) Values('','','','','',getdate())
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUWeightLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.FRUWeightLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.FRUWeightLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.FRUWeightLog.fn_Line].Value = item.Line;
                sqlCtx.Params[_Schema.FRUWeightLog.fn_SN].Value = item.SN;
                sqlCtx.Params[_Schema.FRUWeightLog.fn_Station].Value = item.Station;
                sqlCtx.Params[_Schema.FRUWeightLog.fn_Weight].Value = item.Weight;
                int id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetWeightOfFRUWeightLog(string sn)
        {
            //select Weight from IMES_PAK..FRUWeightLog where SN=''
            try
            {
                decimal ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUWeightLog cond = new _Schema.FRUWeightLog();
                        cond.SN = sn;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUWeightLog), null, new List<string>() { _Schema.FRUWeightLog.fn_Weight }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUWeightLog.fn_SN].Value = sn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.FRUWeightLog.fn_Weight]);
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

        private void PersistInsertWeightLog(WeightLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.WeightLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.WeightLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.WeightLog.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.WeightLog.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.WeightLog.fn_Line].Value = item.Line;
                sqlCtx.Params[_Schema.WeightLog.fn_SN].Value = item.SN;
                sqlCtx.Params[_Schema.WeightLog.fn_Station].Value = item.Station;
                sqlCtx.Params[_Schema.WeightLog.fn_Weight].Value = item.Weight;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateWeightLog(WeightLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.WeightLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.WeightLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.WeightLog.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.WeightLog.fn_Line].Value = item.Line;
                sqlCtx.Params[_Schema.WeightLog.fn_SN].Value = item.SN;
                sqlCtx.Params[_Schema.WeightLog.fn_Station].Value = item.Station;
                sqlCtx.Params[_Schema.WeightLog.fn_Weight].Value = item.Weight;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteWeightLog(WeightLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.WeightLog));
                    }
                }
                sqlCtx.Params[_Schema.WeightLog.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Defered

        public void AddOrModifyFRUWeightLogDefered(IUnitOfWork uow, IMES.DataModel.FRUWeightLog item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion
    }
}
