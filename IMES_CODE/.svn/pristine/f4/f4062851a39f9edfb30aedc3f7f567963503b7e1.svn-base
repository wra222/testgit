using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PartSn;
using System.Data;
using IMES.Infrastructure.Util;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: PartSn相关
    /// </summary>
    public class PartSnRepository : BaseRepository<PartSn>, IPartSnRepository
    {
        #region Overrides of BaseRepository<PartSn>

        protected override void PersistNewItem(PartSn item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    PersistInsertPartSn(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(PartSn item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    PersistUpdatePartSn(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(PartSn item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    PersistDeletePartSn(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<IPart>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override PartSn Find(object key)
        {
            try
            {
                PartSn ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartSN cond = new _Schema.PartSN();
                        cond.IECSN = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartSN), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartSN.fn_IECSN].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new PartSn(
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_IECSN]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_IECPn]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_PartType]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VendorSN]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VendorDCode]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VCode]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn__151PartNo]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Editor]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_DateCode])
                            );

                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Udt]);
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
        public override IList<PartSn> FindAll()
        {
            try
            {
                IList<PartSn> ret = new List<PartSn>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartSN));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartSn item = new PartSn(
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_IECSN]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_IECPn]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_PartType]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VendorSN]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VendorDCode]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VCode]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn__151PartNo]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Editor]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_DateCode])
                                            );
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Cdt]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Udt]);
                            item.Tracker.Clear();
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

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        public override void Add(PartSn item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(PartSn item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="work"></param>
        public override void Update(PartSn item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IPartSnRepository
        /// <summary>
        /// 根据Vendor SN取PartSn
        /// </summary>
        /// <param name="vendorSn">vendorSn</param>
        /// <returns>PartSn</returns>
        public PartSn FindPartSnByVendorSn(string vendorSn)
        {
            try
            {
                PartSn ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartSN cond = new _Schema.PartSN();
                        cond.VendorSN = vendorSn;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartSN), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartSN.fn_VendorSN].Value = vendorSn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new PartSn(
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_IECSN]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_IECPn]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_PartType]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VendorSN]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VendorDCode]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_VCode]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn__151PartNo]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Editor]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_DateCode])
                            );

                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartSN.fn_Udt]);
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
        /// 获取不同时间段内KeyParts已结合的数量
        /// 夜班：前一天晚上20:30到早上7:30
        /// 白班：早上8:00到晚上20:30
        /// 线别最后一位D是白班(从早上8:00开始)，N是夜班(从晚上20:30开始)
        /// 注：
        /// A.若当天班次还没有到开班时间，则取前一天的数量
        /// B.从PartSN表里按照PartType获取时间段内已结合的数量
        /// </summary>
        /// <returns></returns>
        public int GetCombineCountByPartType(string partType, DateTime startTime, DateTime endTime)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartSN cond = new _Schema.PartSN();
                        cond.PartType = partType;
                        _Schema.PartSN notEqCond = new _Schema.PartSN();
                        notEqCond.VendorSN = string.Empty;
                        _Schema.PartSN gCond = new _Schema.PartSN();
                        gCond.Udt = startTime;
                        _Schema.PartSN sCond = new _Schema.PartSN();
                        sCond.Udt = endTime;

                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartSN), "COUNT", new List<string>() { _Schema.PartSN.fn_IECSN }, cond, null, null, gCond, sCond, null, null, null, notEqCond, null, null);

                        sqlCtx.Params[_Schema.PartSN.fn_VendorSN].Value = string.Empty;
                    }
                }
                sqlCtx.Params[_Schema.PartSN.fn_PartType].Value = partType;
                sqlCtx.Params[_Schema.Func.DecG(_Schema.PartSN.fn_Udt)].Value = startTime;
                sqlCtx.Params[_Schema.Func.DecS(_Schema.PartSN.fn_Udt)].Value = endTime; 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    }
                }
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Inners .
        private void PersistInsertPartSn(PartSn item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartSN));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartSN.fn__151PartNo].Value = item.Pn151;
                sqlCtx.Params[_Schema.PartSN.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartSN.fn_DateCode].Value = item.DateCode;
                sqlCtx.Params[_Schema.PartSN.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartSN.fn_IECPn].Value = item.IecPn;
                sqlCtx.Params[_Schema.PartSN.fn_IECSN].Value = item.IecSn;
                sqlCtx.Params[_Schema.PartSN.fn_PartType].Value = item.Type;
                sqlCtx.Params[_Schema.PartSN.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PartSN.fn_VCode].Value = item.VCode;
                sqlCtx.Params[_Schema.PartSN.fn_VendorDCode].Value = item.VendorDCode;
                sqlCtx.Params[_Schema.PartSN.fn_VendorSN].Value = item.VendorSn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePartSn(PartSn item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartSN));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartSN.fn__151PartNo].Value = item.Pn151;
                //sqlCtx.Params[_Schema.PartSN.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartSN.fn_DateCode].Value = item.DateCode;
                sqlCtx.Params[_Schema.PartSN.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartSN.fn_IECPn].Value = item.IecPn;
                sqlCtx.Params[_Schema.PartSN.fn_IECSN].Value = item.IecSn;
                sqlCtx.Params[_Schema.PartSN.fn_PartType].Value = item.Type;
                sqlCtx.Params[_Schema.PartSN.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PartSN.fn_VCode].Value = item.VCode;
                sqlCtx.Params[_Schema.PartSN.fn_VendorDCode].Value = item.VendorDCode;
                sqlCtx.Params[_Schema.PartSN.fn_VendorSN].Value = item.VendorSn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePartSn(PartSn item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartSN));
                    }
                }
                sqlCtx.Params[_Schema.PartSN.fn_IECSN].Value = item.IecSn;
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
