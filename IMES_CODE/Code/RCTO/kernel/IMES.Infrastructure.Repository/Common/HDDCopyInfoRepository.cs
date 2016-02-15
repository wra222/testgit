using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.HDDCopyInfo;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: HDDCopyInfo相关
    /// </summary>
    public class HDDCopyInfoRepository : BaseRepository<HDDCopyInfo>, IHDDCopyInfoRepository
    {
        #region Overrides of BaseRepository<HDDCopyInfo>

        protected override void PersistNewItem(HDDCopyInfo item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertHDDCopyInfo(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(HDDCopyInfo item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistModifyHDDCopyInfo(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(HDDCopyInfo item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteHDDCopyInfo(item);
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
        public override HDDCopyInfo Find(object key)
        {
            try
            {
                HDDCopyInfo ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Schema.HDDCopyInfo cond = new _Schema.HDDCopyInfo();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(tk, typeof(_Schema.HDDCopyInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new HDDCopyInfo(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_ID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_CopyMachineID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_ConnectorID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_SourceHDD]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_HDDNo]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_Editor]));

                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_Udt]);
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
        public override IList<HDDCopyInfo> FindAll()
        {
            try
            {
                IList<HDDCopyInfo> ret = new List<HDDCopyInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(tk, typeof(_Schema.Line));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        HDDCopyInfo item = new HDDCopyInfo(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_ID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_CopyMachineID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_ConnectorID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_SourceHDD]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_HDDNo]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_Editor]));

                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.HDDCopyInfo.fn_Udt]);
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
        public override void Add(HDDCopyInfo item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(HDDCopyInfo item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(HDDCopyInfo item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region IHDDCopyInfoRepository Members

        public int GetCountByConnectorNo(string connectorNo)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.HDDCopyInfo cond = new _Schema.HDDCopyInfo();
                        cond.ConnectorID = connectorNo;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.HDDCopyInfo), "COUNT", new List<string>() { _Schema.HDDCopyInfo.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_ConnectorID].Value = connectorNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        #endregion

        #region . Inners .

        private void PersistInsertHDDCopyInfo(HDDCopyInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.HDDCopyInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_ConnectorID].Value = item.ConnectorId;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_CopyMachineID].Value = item.CopyMachineId;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_HDDNo].Value = item.HddNo;
                //sqlCtx.Params[_Schema.HDDCopyInfo.fn_ID].Value = item.Id;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_SourceHDD].Value = item.SourceHdd;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_Udt].Value = cmDt;
                item.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistModifyHDDCopyInfo(HDDCopyInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.HDDCopyInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_ConnectorID].Value = item.ConnectorId;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_CopyMachineID].Value = item.CopyMachineId;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_HDDNo].Value = item.HddNo;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_ID].Value = item.Id;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_SourceHDD].Value = item.SourceHdd;
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteHDDCopyInfo(HDDCopyInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.HDDCopyInfo));
                    }
                }
                sqlCtx.Params[_Schema.HDDCopyInfo.fn_ID].Value = item.Id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
