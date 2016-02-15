using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.TPCB;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.DataModel;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: TPCB相关
    /// </summary>
    public class TPCBRepository : BaseRepository<TPCB>, ITPCBRepository
    {
        #region Overrides of BaseRepository<TPCB>

        protected override void PersistNewItem(TPCB item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertTPCB(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(TPCB item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateTPCB(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(TPCB item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteTPCB(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<TPCB>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override TPCB Find(object key)
        {
            try
            {
                TPCB ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB_Maintain cond = new _Schema.TPCB_Maintain();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new TPCB();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Editor]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_ID]);
                        ret.Tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Tp]);
                        ret.Tpcb = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_TPCB]);
                        ret.Vcode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Vcode]);
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
        public override IList<TPCB> FindAll()
        {
            try
            {
                IList<TPCB> ret = new List<TPCB>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCB item = new TPCB();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_ID]);
                        item.Tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Tp]);
                        item.Tpcb = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_TPCB]);
                        item.Vcode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Vcode]);
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
        public override void Add(TPCB item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(TPCB item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(TPCB item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region ITPCBRepository Members

        public IList<string> GetVCodeInfo(string tpcb, string tp)
        {
            //SELECT Vcode FROM GetData..TPCB_Maintain WHERE TPCB = @TPCB AND Tp = @Tp
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB_Maintain cond = new _Schema.TPCB_Maintain();
                        cond.TPCB = tpcb;
                        cond.Tp = tp;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain), null, new List<string>() { _Schema.TPCB_Maintain.fn_Vcode }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_TPCB].Value = tpcb;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Tp].Value = tp;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Vcode]);
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

        public IList<VCodeInfo> GetVCodeCombineInfo(string vcode)
        {
            //SELECT * FROM GetData..TPCB_Maintain WHERE Vcode = @Vcode
            try
            {
                IList<VCodeInfo> ret = new List<VCodeInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB_Maintain cond = new _Schema.TPCB_Maintain();
                        cond.Vcode = vcode;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Vcode].Value = vcode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        VCodeInfo item = new VCodeInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Editor]);
                        item.tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Tp]);
                        item.tpcb = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_TPCB]);
                        item.vcode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Vcode]);
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

        public void SaveVCodeInfo(VCodeInfo value)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (PeekVCodeInfo(value.tpcb, value.tp))
                    UpdateVCodeInfo(value);
                else
                    InsertVCodeInfo(value);

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

        private bool PeekVCodeInfo(string tpcb, string tp)
        {
            //EXISTS(SELECT * FROM GetData..TPCB_Maintain WHERE TPCB = @TPCB AND Tp = @Tp)
            SqlDataReader sqlR = null;
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB_Maintain cond = new _Schema.TPCB_Maintain();
                        cond.TPCB = tpcb;
                        cond.Tp = tp;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_TPCB].Value = tpcb;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Tp].Value = tp;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = true;
                }
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

        private void UpdateVCodeInfo(VCodeInfo value)
        {
            //UPDATE [TPCB_Maintain] SET [Vcode] = @Vcode, [Editor] = @Editor, [Cdt] = GETDATE() WHERE TPCB = @TPCB AND Tp = @Tp
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB_Maintain cond = new _Schema.TPCB_Maintain();
                        cond.TPCB = value.tpcb;
                        cond.Tp = value.tp;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain), new List<string>() { _Schema.TPCB_Maintain.fn_Vcode, _Schema.TPCB_Maintain.fn_Editor, _Schema.TPCB_Maintain.fn_Cdt }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Tp].Value = value.tp;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_TPCB].Value = value.tpcb;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB_Maintain.fn_Editor)].Value = value.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB_Maintain.fn_Vcode)].Value = value.vcode;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB_Maintain.fn_Cdt)].Value = value.Cdt = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertVCodeInfo(VCodeInfo value)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Cdt].Value = value.Cdt = cmDt;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Editor].Value = value.Editor;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Tp].Value = value.tp;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_TPCB].Value = value.tpcb;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Vcode].Value = value.vcode;
                int id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteVcodeInfo(string vcode)
        {
            //DELETE GetData..TPCB_Maintain WHERE Vcode = @Vcode
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB_Maintain cond = new _Schema.TPCB_Maintain();
                        cond.Vcode = vcode;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Vcode].Value = vcode;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckVCode(string tpcb, string tp, string vcode)
        {
            //SELECT COUNT(*) FROM GetData..TPCB_Maintain WHERE TPCB = @TPCB AND Tp = @Tp AND Vcode = @Vcode
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB_Maintain cond = new _Schema.TPCB_Maintain();
                        cond.TPCB = tpcb;
                        cond.Tp = tp;
                        cond.Vcode = vcode;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain), "COUNT", new List<string>() { _Schema.TPCB_Maintain.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_TPCB].Value = tpcb;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Tp].Value = tp;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Vcode].Value = vcode;
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

        public IList<VCodeInfo> QueryAll()
        {
            //SELECT * FROM GetData..TPCB_Maintain ORDER BY TPCB,Tp,Vcode
            try
            {
                IList<VCodeInfo> ret = new List<VCodeInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.TPCB_Maintain.fn_TPCB, _Schema.TPCB_Maintain.fn_Tp, _Schema.TPCB_Maintain.fn_Vcode }));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        VCodeInfo item = new VCodeInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Editor]);
                        item.tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Tp]);
                        item.tpcb = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_TPCB]);
                        item.vcode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB_Maintain.fn_Vcode]);
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

        #region . Inners .

        private void PersistInsertTPCB(TPCB item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Tp].Value = item.Tp;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_TPCB].Value = item.Tpcb;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Vcode].Value = item.Vcode;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateTPCB(TPCB item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain));
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Tp].Value = item.Tp;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_TPCB].Value = item.Tpcb;
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_Vcode].Value = item.Vcode;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteTPCB(TPCB item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB_Maintain));
                    }
                }
                sqlCtx.Params[_Schema.TPCB_Maintain.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Defered

        public void SaveVCodeInfoDefered(IUnitOfWork uow, string tpcb, string tp, string vcode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), tpcb, tp, vcode);
        }

        public void DeleteVcodeInfoDefered(IUnitOfWork uow, string vcode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), vcode);
        }

        public void SaveTPCBDetDefered(IUnitOfWork uow, string tpcbCode, string family, string pdline, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), tpcbCode, family, pdline, editor);
        }

        #endregion
    }
}
