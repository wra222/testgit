using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Metas;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: MO相关
    /// </summary>
    public class MORepository : BaseRepository<IMES.FisObject.Common.MO.MO>, IMORepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Link To Other
        private static IMES.FisObject.PCA.MBMO.IMBMORepository _mbmoRepository = null;
        private static IMES.FisObject.PCA.MBMO.IMBMORepository MbmoRepository
        {
            get
            {
                if (_mbmoRepository == null)
                    _mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.PCA.MBMO.IMBMORepository, IMES.FisObject.PCA.MBMO.IMBMO>();
                return _mbmoRepository;
            }
        }
        #endregion

        #region Overrides of BaseRepository<MO>

        protected override void PersistNewItem(IMES.FisObject.Common.MO.MO item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    PersistInsertMO(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(IMES.FisObject.Common.MO.MO item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    PersistUpdateMO(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(IMES.FisObject.Common.MO.MO item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    PersistDeleteMO(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<MO>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IMES.FisObject.Common.MO.MO Find(object key)
        {
            try
            {
                IMES.FisObject.Common.MO.MO ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO cond = new _Schema.MO();
                        cond.Mo = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Mo].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new IMES.FisObject.Common.MO.MO();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Cdt]);
                        ret.CreateDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                        ret.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                        ret.MONO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                        ret.Plant = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Plant]);
                        ret.PrtQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                        ret.TransferQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Transfer_Qty]);
                        ret.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                        ret.SAPQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPQty]);
                        ret.SAPStatus = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPStatus]);
                        ret.StartDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                        ret.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Status]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Udt]);
                        ret.CustomerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                        ret.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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
        public override IList<IMES.FisObject.Common.MO.MO> FindAll()
        {
            try
            {
                IList<IMES.FisObject.Common.MO.MO> ret = new List<IMES.FisObject.Common.MO.MO>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.MO.MO item = new IMES.FisObject.Common.MO.MO();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Cdt]);
                        item.CreateDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                        item.MONO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                        item.Plant = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Plant]);
                        item.PrtQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                        item.TransferQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Transfer_Qty]);
                        item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                        item.SAPQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPQty]);
                        item.SAPStatus = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPStatus]);
                        item.StartDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                        item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Status]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Udt]);
                        item.CustomerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                        item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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
        public override void Add(IMES.FisObject.Common.MO.MO item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(IMES.FisObject.Common.MO.MO item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(IMES.FisObject.Common.MO.MO item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IMORepository

        public IList<MOInfo> GetMOListFor014(string model)
        {
            //select ID from MO where Qty-Print_Qty > 0 and  Status='H' and Udt>dateadd(day,-30,getdate()) and Model=model# and MO.SAPStatus='' order by ID
            try
            {
                IList<MOInfo> ret = new List<MOInfo>();

                IList<string> res = MbmoRepository.GetMOsRecentOneMonthByModel(model);

                if (res != null && res.Count > 0)
                {
                    ret = (from item in res select new MOInfo { id = item, friendlyName = item }).ToArray();
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据机器Model号，取得MO信息列表
        /// </summary>
        /// <param name="modelId">机器Model号码</param>
        /// <returns>MO信息列表</returns>
        public IList<MOInfo> GetMOList(string modelId)
        {
            try
            {
                IList<MOInfo> ret = new List<MOInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO eqCond = new _Schema.MO();
                        eqCond.Model = modelId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, new List<string>() { _Schema.MO.fn_Mo, _Schema.MO.fn_CreateDate, _Schema.MO.fn_Model, _Schema.MO.fn_Print_Qty, _Schema.MO.fn_Qty, _Schema.MO.fn_StartDate, _Schema.MO.fn_customerSN_Qty, _Schema.MO.fn_PoNo }, eqCond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);
                    }
                }

                sqlCtx.Params[_Schema.MO.fn_Model].Value = modelId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MOInfo item = new MOInfo();
                            item.createDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                            item.model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                            item.pqty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                            item.qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                            item.startDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                            item.customerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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

        //MO.Prt_Qty>0 and MO.Status='H' and MO.SAPStatus=''
        public IList<MOInfo> GetMOListFor013()
        {
            try
            {
                IList<MOInfo> ret = new List<MOInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO eqCond = new _Schema.MO();
                        eqCond.Status = "H";
                        eqCond.SAPStatus = string.Empty;

                        _Schema.MO gCond = new _Schema.MO();
                        gCond.Print_Qty = 0;

                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, new List<string>() { _Schema.MO.fn_Mo, _Schema.MO.fn_CreateDate, _Schema.MO.fn_Model, _Schema.MO.fn_Print_Qty, _Schema.MO.fn_Qty, _Schema.MO.fn_StartDate, _Schema.MO.fn_customerSN_Qty, _Schema.MO.fn_PoNo }, eqCond, null, null, gCond, null, null, null, null);

                        sqlCtx.Params[_Schema.MO.fn_Status].Value = eqCond.Status;
                        sqlCtx.Params[_Schema.MO.fn_SAPStatus].Value = eqCond.SAPStatus;
                        sqlCtx.Params[_Schema.Func.DecG(_Schema.MO.fn_Print_Qty)].Value = gCond.Print_Qty;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MOInfo item = new MOInfo();
                            item.createDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                            item.model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                            item.pqty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                            item.qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                            item.startDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                            item.customerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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
        /// 得到最小可用的Mo
        /// select @newmo=Min(ID)from MO where ModelID=@model and Qty-Print_Qty>0 and Print_Qty>=0 and Status='H'
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IMES.FisObject.Common.MO.MO GetMinUsableMO(string model)
        {
            try
            {
                IMES.FisObject.Common.MO.MO ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO eqCond = new _Schema.MO();
                        eqCond.Status = "H";
                        eqCond.Model = model;

                        _Schema.MO geCond = new _Schema.MO();
                        geCond.Print_Qty = 0;

                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), "MIN", new List<string>() { _Schema.MO.fn_Mo }, eqCond, null, null, null, null, geCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0}-{1}>0 ", _Schema.MO.fn_Qty, _Schema.MO.fn_Print_Qty);

                        sqlCtx.Params[_Schema.MO.fn_Status].Value = eqCond.Status;
                        sqlCtx.Params[_Schema.Func.DecGE(_Schema.MO.fn_Print_Qty)].Value = geCond.Print_Qty;
                    }
                }

                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            string mo = GetValue_Str(sqlR, sqlCtx.Indexes["MIN"]);
                            ret = this.Find(mo);
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
        /// 使Print_Qty增加指定数量
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="count"></param>
        public void IncreaseMOPrintedQty(IMES.FisObject.Common.MO.MO mo, short count)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO cond = new _Schema.MO();
                        cond.Mo = mo.MONO;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, null, new List<string>() { _Schema.MO.fn_Print_Qty }, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Mo].Value = mo.MONO;
                sqlCtx.Params[_Schema.Func.DecInc(_Schema.MO.fn_Print_Qty)].Value = count;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MO.fn_Udt)].Value = _Schema.SqlHelper.GetDateTime();

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 使Print_Qty减少指定数量
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="count"></param>
        public void DecreaseMOPrintedQty(IMES.FisObject.Common.MO.MO mo, short count)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO cond = new _Schema.MO();
                        cond.Mo = mo.MONO;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, null, null, new List<string>() { _Schema.MO.fn_Print_Qty }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Mo].Value = mo.MONO;
                sqlCtx.Params[_Schema.Func.DecDec(_Schema.MO.fn_Print_Qty)].Value = count;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MO.fn_Udt)].Value = _Schema.SqlHelper.GetDateTime();

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.Common.MO.MO> GetVirtualMObyModel(string model)
        {
            //SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM IMES_GetData..MO
            //WHERE Model = @Model
            //    AND LEFT(MO, 1) = 'V'
            //    AND Cdt BETWEEN CONVERT(varchar, GETDATE(),111) AND CONVERT(varchar, DATEADD(day, 1, GETDATE()), 111)
            //ORDER BY MO
            try
            {
                IList<IMES.FisObject.Common.MO.MO> ret = new List<IMES.FisObject.Common.MO.MO>();

                DateTime now = _Schema.SqlHelper.GetDateTime();
                DateTime cmDt = new DateTime(now.Year, now.Month, now.Day,0,0,0);

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO eqCond = new _Schema.MO();
                        eqCond.Model = model;

                        _Schema.MO likeCond = new _Schema.MO();
                        likeCond.Mo = "V%";

                        _Schema.MO geCond = new _Schema.MO();
                        geCond.Cdt = cmDt;

                        _Schema.MO sCond = new _Schema.MO();
                        sCond.Cdt = cmDt.AddDays(1);

                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, null, eqCond, likeCond, null, null, sCond, geCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);

                        sqlCtx.Params[_Schema.MO.fn_Mo].Value = likeCond.Mo;
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;
                sqlCtx.Params[_Schema.Func.DecGE(_Schema.MO.fn_Cdt)].Value = new DateTime(cmDt.Year, cmDt.Month, cmDt.Day);
                sqlCtx.Params[_Schema.Func.DecS(_Schema.MO.fn_Cdt)].Value = new DateTime(cmDt.Year, cmDt.Month, cmDt.Day).AddDays(1);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMES.FisObject.Common.MO.MO item = new IMES.FisObject.Common.MO.MO();
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Cdt]);
                            item.CreateDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                            item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                            item.MONO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                            item.Plant = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Plant]);
                            item.PrtQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                            item.TransferQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Transfer_Qty]);
                            item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                            item.SAPQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPQty]);
                            item.SAPStatus = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPStatus]);
                            item.StartDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                            item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Status]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Udt]);
                            item.CustomerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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

        public IList<MOInfo> GetOldMOListFor094(string model)
        {
            //From MO where SAPStatus<>’CLOSE’ and Status=’H’ and Qty<Print_Qty by model
            //显示以下信息：
            //MO / Qty / StartDate，按照StartDate降序
            try
            {
                IList<MOInfo> ret = new List<MOInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO eqCond = new _Schema.MO();
                        eqCond.Model = model;
                        eqCond.Status = "H";
                        _Schema.MO neqCond = new _Schema.MO();
                        neqCond.SAPStatus = "CLOSE";

                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, new List<string>() { _Schema.MO.fn_Mo, _Schema.MO.fn_CreateDate, _Schema.MO.fn_Model, _Schema.MO.fn_Print_Qty, _Schema.MO.fn_Qty, _Schema.MO.fn_StartDate, _Schema.MO.fn_customerSN_Qty,_Schema.MO.fn_PoNo }, eqCond, null, null, null, null, null, null, null, neqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0}<{1} ", _Schema.MO.fn_Qty, _Schema.MO.fn_Print_Qty);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderByDesc, _Schema.MO.fn_StartDate);

                        sqlCtx.Params[_Schema.MO.fn_Status].Value = eqCond.Status;
                        sqlCtx.Params[_Schema.MO.fn_SAPStatus].Value = neqCond.SAPStatus;
                    }
                }

                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MOInfo item = new MOInfo();
                            item.createDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                            item.model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                            item.pqty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                            item.qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                            item.startDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                            item.customerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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

        public IList<MOInfo> GetNewMOListFor094(string model)
        {
            //按照Model得到SAPStatus<>’CLOSE’ and Status=’H’ and Qty>Print_Qty by model 按照mo排序
            try
            {
                IList<MOInfo> ret = new List<MOInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO eqCond = new _Schema.MO();
                        eqCond.Model = model;
                        eqCond.Status = "H";
                        _Schema.MO neqCond = new _Schema.MO();
                        neqCond.SAPStatus = "CLOSE";

                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, new List<string>() { _Schema.MO.fn_Mo, _Schema.MO.fn_CreateDate, _Schema.MO.fn_Model, _Schema.MO.fn_Print_Qty, _Schema.MO.fn_Qty, _Schema.MO.fn_StartDate, _Schema.MO.fn_customerSN_Qty,_Schema.MO.fn_PoNo }, eqCond, null, null, null, null, null, null, null, neqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0}>{1} ", _Schema.MO.fn_Qty, _Schema.MO.fn_Print_Qty);
                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderByDesc, _Schema.MO.fn_StartDate);

                        sqlCtx.Params[_Schema.MO.fn_Status].Value = eqCond.Status;
                        sqlCtx.Params[_Schema.MO.fn_SAPStatus].Value = neqCond.SAPStatus;
                    }
                }

                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MOInfo item = new MOInfo();
                            item.createDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                            item.model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                            item.pqty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                            item.qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                            item.startDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                            item.customerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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

        public IMES.FisObject.Common.MO.MO GetUsableMOOrderByStartDate(string model)
        {
            //SELECT TOP 1 MO FROM IMES_GetData..MO WHERE Model = @NewModel AND Qty > Print_Qty AND Status = 'H' ORDER BY StartDate
            try
            {
                IMES.FisObject.Common.MO.MO ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO eqCond = new _Schema.MO();
                        eqCond.Status = "H";
                        eqCond.Model = model;

                        _Schema.MO geCond = new _Schema.MO();
                        geCond.Print_Qty = 0;

                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), "TOP 1", new List<string>() { _Schema.MO.fn_Mo }, eqCond, null, null, null, null, geCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0}-{1}>0 ", _Schema.MO.fn_Qty, _Schema.MO.fn_Print_Qty);

                        sqlCtx.Params[_Schema.MO.fn_Status].Value = eqCond.Status;
                        sqlCtx.Params[_Schema.Func.DecGE(_Schema.MO.fn_Print_Qty)].Value = geCond.Print_Qty;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_StartDate);
                    }
                }

                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            string mo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                            ret = this.Find(mo);
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

        public IList<MOInfo> GetMOListFor014ConsiderStartDate(string model)
        {
            //select ID from MO where Qty-Print_Qty > 0 and  Status='H' and Udt>dateadd(day,-10,getdate()) and Model=model# and MO.SAPStatus='' and StartDate>dateadd(day,-3,getdate()) order by ID 
            try
            {
                IList<MOInfo> ret = new List<MOInfo>();

                IList<string> res = MbmoRepository.GetMOsRecentOneMonthByModelConsiderStartDate(model);

                if (res != null && res.Count > 0)
                {
                    ret = (from item in res select new MOInfo { id = item, friendlyName = item }).ToArray();
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int[] GetQtyAndRemainedOfMo(string mo)
        {
            try
            {
                int[] ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Mo cond = new _Metas.Mo();
                        cond.mo = mo;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Mo>(tk, null, new string[] { _Metas.Mo.fn_qty, _Metas.Mo.fn_print_Qty }, new ConditionCollection<_Metas.Mo>(new EqualCondition<_Metas.Mo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Mo.fn_mo).Value = mo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Mo.fn_qty));
                        int prtqty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Mo.fn_print_Qty));
                        ret = new int[] { qty, qty - prtqty };
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DecreaseMOTransferQty(IMES.FisObject.Common.MO.MO mo, short count)
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
                        Mo cond = new Mo();
                        cond.mo = mo.MONO;
                        Mo setv = new Mo();
                        setv.transfer_Qty = count;
                        Mo setv2 = new Mo();
                        setv2.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<Mo>(tk, new SetValueCollection<Mo>(new ForDecSetValue<Mo>(setv), new CommonSetValue<Mo>(setv2)), new ConditionCollection<Mo>(new EqualCondition<Mo>(cond)));
                    }
                }
                sqlCtx.Param(Mo.fn_mo).Value = mo.MONO;

                sqlCtx.Param(g.DecDec(Mo.fn_transfer_Qty)).Value = count;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Mo.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MOInfo> GetMOListFor014ConsiderStartDateOrderByMo(string model)
        {
            try
            {
                IList<MOInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Mo cond = new Mo();
                        cond.sapstatus = string.Empty;
                        cond.status = "H";
                        cond.model = model;

                        Mo cond2 = new Mo();
                        cond2.udt = DateTime.Now;
                        cond2.startDate = DateTime.Now;

                        Mo cond3 = new Mo();
                        cond3.qty = 0;

                        sqlCtx = FuncNew.GetConditionedSelect<Mo>(tk, null, new string[]{Mo.fn_mo, Mo.fn_model, Mo.fn_createDate, Mo.fn_startDate, Mo.fn_qty, Mo.fn_print_Qty, Mo.fn_customerSN_Qty,Mo.fn_poNo }, new ConditionCollection<Mo>(
                            new EqualCondition<Mo>(cond),
                            new GreaterCondition<Mo>(cond2, "CONVERT(VARCHAR(8),{0},112)", "CONVERT(VARCHAR(8),{0},112)"),
                            new AnySoloCondition<Mo>(cond3, string.Format("{0}>{1}","{0}",Mo.fn_print_Qty ))
                            ), Mo.fn_mo);

                        sqlCtx.Param(Mo.fn_sapstatus).Value = cond.sapstatus;
                        sqlCtx.Param(Mo.fn_status).Value = cond.status;
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Mo.fn_model).Value = model;
                sqlCtx.Param(g.DecG(Mo.fn_udt)).Value = cmDt.AddDays(-10);
                sqlCtx.Param(g.DecG(Mo.fn_startDate)).Value = cmDt.AddDays(-3);
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<MOInfo>();
                        while (sqlR.Read())
                        {
                            MOInfo item = new MOInfo();
                            item.createDate = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(Mo.fn_createDate));
                            item.friendlyName = item.id = g.GetValue_Str(sqlR, sqlCtx.Indexes(Mo.fn_mo));
                            item.model = g.GetValue_Str(sqlR, sqlCtx.Indexes(Mo.fn_model));
                            item.pqty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Mo.fn_print_Qty));
                            item.qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Mo.fn_qty));
                            item.startDate = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(Mo.fn_startDate));
                            item.customerSN_Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Mo.fn_customerSN_Qty));
                            item.PoNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(Mo.fn_poNo));
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

        public IList<IMES.FisObject.Common.MO.MO> GetVirtualMOAndRealMObyModel(string model)
        {
            try
            {
                IList<IMES.FisObject.Common.MO.MO> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Mo cond = new Mo();
                        cond.model = model;

                        Mo cond2 = new Mo();
                        cond2.cdt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelect<Mo>(tk, null, null, new ConditionCollection<Mo>(
                            new EqualCondition<Mo>(cond),
                            new BetweenCondition<Mo>(cond2,"CONVERT(VARCHAR,{0},111)")), Mo.fn_mo);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Mo.fn_model).Value = model;
                sqlCtx.Param(g.DecBeg(Mo.fn_cdt)).Value = new DateTime(cmDt.Year, cmDt.Month, cmDt.Day);
                sqlCtx.Param(g.DecEnd(Mo.fn_cdt)).Value = new DateTime(cmDt.Year, cmDt.Month, cmDt.Day).AddDays(1);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Mo, IMES.FisObject.Common.MO.MO, IMES.FisObject.Common.MO.MO>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MOInfo> GetMOListByFamily(string family)
        {
            try
            {
                // SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM MO a，Model b
                // WHERE a.Model=b.Model and b.Family= @Family and  Qty> Print_Qty order by MO
        
                IList<MOInfo> ret = new List<MOInfo>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(_Schema.Model);
                        _Schema.Model equalCond1 = new _Schema.Model();
                        equalCond1.Family = family;
                        tf1.equalcond = equalCond1;
                        tf1.ToGetFieldNames = null;

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.MO);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_Mo);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_Model);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_CreateDate);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_StartDate);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_Qty);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_Print_Qty);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_customerSN_Qty);
                        tf2.ToGetFieldNames.Add(_Schema.MO.fn_PoNo);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf2, _Schema.MO.fn_Print_Qty, tf2, _Schema.MO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, _Schema.Model.fn_model, tf2, _Schema.MO.fn_Model);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf2.alias, _Schema.MO.fn_Mo));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[Func.DecAlias(tf1.alias, _Schema.Model.fn_Family)].Value = family;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MOInfo item = new MOInfo();
                            item.createDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_CreateDate)]);
                            item.id = item.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_Mo)]);
                            item.model = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_Model)]);
                            item.pqty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_Print_Qty)]);
                            item.qty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_Qty)]);
                            item.startDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_StartDate)]);
                            item.customerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_customerSN_Qty)]);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.MO.fn_PoNo)]);
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

        public void ExecuteMoUploadJOB(string jobName)
        {
            try
            {
                SqlParameter[] paramsArray = new SqlParameter[1];

                paramsArray[0] = new SqlParameter("@jobName", SqlDbType.VarChar);
                paramsArray[0].Value = jobName;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.StoredProcedure, "op_ExecuteMoUploadJOB", paramsArray);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMoForIncreaseCustomerSnQty(string productId, short count)
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
                        Mo setValue = new Mo();
                        setValue.customerSN_Qty = 1;
                        Mo cond = new Mo();
                        cond.mo = productId;
                        sqlCtx = FuncNew.GetConditionedUpdate(tk, new SetValueCollection<Mo>(
                                new ForIncSetValue<Mo>(setValue, "{0}=ISNULL({0},0)+{1}")), new ConditionCollection<Mo>(
                                new AnyCondition<Mo>(cond, string.Format("{0} IN (SELECT {1} FROM {2}..{3} WHERE {4}={5})", "{0}", _Metas.Product.fn_mo, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_productID, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(Mo.fn_mo)).Value = productId;
                sqlCtx.Param(g.DecInc(Mo.fn_customerSN_Qty)).Value = count;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetModelListFromMo(string family)
        {
            try
            {
                IList<string> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Model>();
                        _Metas.Model cond = new _Metas.Model();
                        cond.family = family;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Model>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.Model.fn_model);

                        tf2 = new TableAndFields<_Metas.Mo>();
                        _Metas.Mo cond2 = new _Metas.Mo();
                        cond2.print_Qty = 1;
                        tf2.Conditions.Add(new AnySoloCondition<_Metas.Mo>(cond2, string.Format("{0}<{1}", "{0}", _Metas.Mo.fn_qty)));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        _Metas.TableConnectionCollection tblCnnts = new _Metas.TableConnectionCollection(
                            new TableConnectionItem<_Metas.Model, _Metas.Mo>(tf1, _Metas.Model.fn_model, tf2, _Metas.Mo.fn_model));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, g.DecAliasInner("t1", _Metas.Model.fn_model));
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Model.fn_family)).Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Model.fn_model)));
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

        public IList<MOInfo> GetMOByModel(string model)
        {
            try
            {
                IList<MOInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Mo cond = new _Metas.Mo();
                        cond.model = model;
                        _Metas.Mo cond2 = new _Metas.Mo();
                        cond2.print_Qty = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Mo>(tk, null, null, new ConditionCollection<_Metas.Mo>(new EqualCondition<_Metas.Mo>(cond), new AnySoloCondition<_Metas.Mo>(cond2, string.Format("{0}<{1}", "{0}", _Metas.Mo.fn_qty))), _Metas.Mo.fn_udt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Mo.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<MOInfo>();
                        while (sqlR.Read())
                        {
                            MOInfo item = new MOInfo();
                            item.createDate = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(Mo.fn_createDate));
                            item.friendlyName = item.id = g.GetValue_Str(sqlR, sqlCtx.Indexes(Mo.fn_mo));
                            item.model = g.GetValue_Str(sqlR, sqlCtx.Indexes(Mo.fn_model));
                            item.pqty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Mo.fn_print_Qty));
                            item.qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Mo.fn_qty));
                            item.startDate = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(Mo.fn_startDate));
                            item.customerSN_Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Mo.fn_customerSN_Qty));
                            item.PoNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(Mo.fn_poNo));
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


        #region Product Plan
        /// <summary>
        /// select ID, PdLine, ShipDate, Family, Model, PlanQty, AddPrintQty, PrePrintQty, Editor, Cdt, Udt 
        ///from ProductPlan
        ///where PdLine = @line and
        ///ShipDate =@shipdate
        /// </summary>
        /// <param name="line">Phyical Line</param>
        /// <param name="shipDate">ShipDate</param>
        /// <returns></returns>

        public IList<ProductPlanInfo> GetProductPlanByLineAndShipDate(string line, DateTime shipDate)
        {
            try
            {
                List<ProductPlanInfo> ret = new List<ProductPlanInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                         
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, PdLine, ShipDate, Family, Model, PlanQty, AddPrintQty, PrePrintQty, Editor, Cdt, Udt, PoNo 
                                                            from ProductPlan
                                                            where PdLine = @line and
                                                                       ShipDate =@shipDate";
                        sqlCtx.AddParam("PdLine", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipDate", new SqlParameter("@shipDate", SqlDbType.DateTime));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PdLine").Value = line;
                sqlCtx.Param("ShipDate").Value = shipDate;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPlanInfo item = SQLData.ToObject<ProductPlanInfo>(sqlR);
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

        public IList<ProductPlanLog> GetProductPlanMOByLineAndShipDate(string line, DateTime shipDate, string station)
        {
            try
            {
                List<ProductPlanLog> ret = new List<ProductPlanLog>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.ID, 'Summary' as [Action], a.PdLine, a.ShipDate, a.Family, a.Model, a.PlanQty, a.AddPrintQty, 
                                               (b.Qty - b.Print_Qty) as RemainQty, 
                                               isnull(dbo.GetMOStationQty(@station, b.MO), 0) as NonInputQty, 
                                               (b.Print_Qty - isnull(dbo.GetMOStationQty(@station, b.MO), 0)) as InputQty,
                                               a.Editor, a.Cdt, a.Udt, a.PoNo 
                                            from ProductPlan a
                                            inner join MO b on b.MO = dbo.GetPlanMONo(a.ID, a.PdLine)
                                            where a.PdLine = @line and
                                                  a.ShipDate =@shipDate";
                        sqlCtx.AddParam("PdLine", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipDate", new SqlParameter("@shipDate", SqlDbType.DateTime));
                        sqlCtx.AddParam("Station", new SqlParameter("@station", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PdLine").Value = line;
                sqlCtx.Param("ShipDate").Value = shipDate;
                sqlCtx.Param("Station").Value = station;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPlanLog item = SQLData.ToObject<ProductPlanLog>(sqlR);
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

        public IList<ProductPlanLog> GetProductPlanLogByLineAndShipDateAndAction(string line, DateTime shipDate, string action)
        {
            try
            {
                //action: New, Revise

                List<ProductPlanLog> ret = new List<ProductPlanLog>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, [Action], PdLine, ShipDate, Family, Model, PlanQty, AddPrintQty, 
                                                   0 as RemainQty, 0 as NonInputQty, 0 as InputQty, 
                                                   Editor, Cdt, Cdt as Udt, PoNo
                                            from ProductPlanLog
                                            where [Action] =@action and ShipDate=@shipDate and PdLine=@line
                                            order by ID";
                        sqlCtx.AddParam("PdLine", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipDate", new SqlParameter("@shipDate", SqlDbType.DateTime));
                        sqlCtx.AddParam("Action", new SqlParameter("@action", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PdLine").Value = line;
                sqlCtx.Param("ShipDate").Value = shipDate;
                sqlCtx.Param("Action").Value = action;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPlanLog item = SQLData.ToObject<ProductPlanLog>(sqlR);
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
        /// declare @ProductPlanUpload TbProductPlan 
        ///  exec IMES_ProductPlanCheckInsert @ProductPlanUpload 
        /// </summary>
        /// <param name="ProdPlanList"> TVP parameter structure</param>
        public IList<ProductPlanLog> UploadProductPlan(IList<TbProductPlan> ProdPlanList,string combinePO)
        {
            try
            {
                List<ProductPlanLog> ret = new List<ProductPlanLog>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = "IMES_ProductPlanCheckInsert";
                        SqlParameter para = new SqlParameter("@ProductPlanUpload", SqlDbType.Structured);
                        para.TypeName = "TbProductPlan";
                        sqlCtx.AddParam("ProductPlan", para);
                        sqlCtx.AddParam("CombinedPO", new SqlParameter("@CombinedPO", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("ProductPlan").Value = SQLData.ToDataTable <TbProductPlan>(ProdPlanList);
                sqlCtx.Param("CombinedPO").Value = combinePO;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.StoredProcedure,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPlanLog item = SQLData.ToObject<ProductPlanLog>(sqlR);
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
        /// declare @ProductPlanUpload TbProductPlan 
        ///  exec IMES_ProductPlanCheckInsert @ProductPlanUpload 
        /// </summary>
        /// <param name="ProdPlanList"> TVP parameter structure</param>
        public IList<ProductPlanLog> UploadProductPlan_Revise(IList<TbProductPlan> ProdPlanList, string combinePO)
        {
            try
            {
                List<ProductPlanLog> ret = new List<ProductPlanLog>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = "IMES_ProductPlanCheckInsert_Revise";
                        SqlParameter para = new SqlParameter("@ProductPlanUpload", SqlDbType.Structured);
                        para.TypeName = "TbProductPlan";
                        sqlCtx.AddParam("ProductPlan", para);
                        sqlCtx.AddParam("CombinedPO", new SqlParameter("@CombinedPO", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("ProductPlan").Value = SQLData.ToDataTable<TbProductPlan>(ProdPlanList);
                sqlCtx.Param("CombinedPO").Value = combinePO;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.StoredProcedure,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPlanLog item = SQLData.ToObject<ProductPlanLog>(sqlR);
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
        /// SELECT distinct a.[Family]
        /// ,a.[Descr]
        ///,a.[CustomerID]
        ///FROM [Family] a, ProductPlan b, MO c
        ///Where a.Family = b.Family 
        /// and b.PdLine = left(@PdLine,1) 
        /// and b.ShipDate=@ShipDate
        /// and GetPlanMONo(b.ID, b.PdLine) = c.MO 
        ///and c.Qty > c.Print_Qty 
        /// and c.[Status] = 'H'
        /// </summary>
        /// <param name="line"></param>
        /// <param name="shipdate"></param>
        public IList<ProductPlanFamily> GetProductPlanFamily(string line, DateTime shipdate)
        {
            try
            {
                List<ProductPlanFamily> ret = new List<ProductPlanFamily>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" SELECT distinct a.[Family], a.[Descr],a.[CustomerID],a.Editor, a.Cdt,a.Udt
                                                            FROM [Family] a, ProductPlan b, MO c
                                                            Where a.Family = b.Family 
                                                             and b.PdLine = left(@line,1) 
                                                             and b.ShipDate=@shipDate
                                                             and dbo.GetPlanMONo(b.ID, b.PdLine) = c.MO 
                                                            and c.Qty > c.Print_Qty 
                                                             and c.[Status] = 'H'";
                        sqlCtx.AddParam("PdLine", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipDate", new SqlParameter("@shipDate", SqlDbType.DateTime));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PdLine").Value = line;
                sqlCtx.Param("ShipDate").Value = shipdate;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPlanFamily item = SQLData.ToObject<ProductPlanFamily>(sqlR);
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
        /// SELECT a.[Model], b.[ID]
        ///FROM [Model] a, ProductPlan b, MO c
        ///Where a.Family = @Family 
        ///      and a.Model = b.Model
        ///      and b.PdLine = left(@PdLine,1) 
        ///      and b.ShipDate=@ShipDate
        ///      and GetPlanMONo(b.ID, b.PdLine) = c.MO 
        ///      and c.Qty > c.Print_Qty 
        ///      and c.[Status] = 'H' 
        ///Order by a.[Model]
        /// </summary>
        /// <param name="line"></param>
        /// <param name="shipdate"></param>
        /// <param name="family"></param>
        /// <returns></returns>

        public IList<ProductPlanInfo> GetProductPlanModel(string line, DateTime shipdate, string family)
        {
            try
            {
                List<ProductPlanInfo> ret = new List<ProductPlanInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" SELECT b.ID, b.PdLine, b.ShipDate, b.Family, b.Model, 
                                                                            b.PlanQty, b.AddPrintQty, b.PrePrintQty, b.Editor, b.Cdt, 
                                                                            b.Udt, b.PoNo 
                                                            FROM [Model] a, ProductPlan b, MO c
                                                            Where a.Family = @family 
                                                                  and a.Model = b.Model
                                                                  and b.PdLine = left(@line,1) 
                                                                  and b.ShipDate=@shipDate
                                                                  and dbo.GetPlanMONo(b.ID, b.PdLine) = c.MO 
                                                                  and c.Qty > c.Print_Qty 
                                                                  and c.[Status] = 'H' 
                                                            Order by a.[Model]";
                        sqlCtx.AddParam("PdLine", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipDate", new SqlParameter("@shipDate", SqlDbType.DateTime));
                        sqlCtx.AddParam("Family", new SqlParameter("@family", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PdLine").Value = line;
                sqlCtx.Param("ShipDate").Value = shipdate;
                sqlCtx.Param("Family").Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPlanInfo item = SQLData.ToObject<ProductPlanInfo>(sqlR);
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
        /// SELECT a.*, b.PlanQty 
        ///FROM  MO a, ProductPlan b
        ///Where b.[ID] = @ID and 
        ///      a.MO= dbo. GetPlanMONo(b.ID, b.PdLine)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MOPlanInfo GetPlanMO(int id)
        {
            try
            {
                MOPlanInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" SELECT a.MO, a.Plant, a.Model, a.CreateDate, a.StartDate, 
                                                                            a.Qty, a.SAPStatus, a.SAPQty, a.Print_Qty, a.Transfer_Qty, 
                                                                            a.Status, a.Cdt, a.Udt, a.CustomerSN_Qty, b.PlanQty, b.PoNo 
                                                            FROM  MO a, ProductPlan b
                                                            Where b.[ID] = @ID and 
                                                                  a.MO= dbo.GetPlanMONo(b.ID, b.PdLine)";
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));
                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ID").Value = id;
                

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObject<MOPlanInfo>(sqlR);                      
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


        #region . Defered .

        public void IncreaseMOPrintedQtyDefered(IUnitOfWork uow, IMES.FisObject.Common.MO.MO mo, short count)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo, count);
        }

        public void DecreaseMOPrintedQtyDefered(IUnitOfWork uow, IMES.FisObject.Common.MO.MO mo, short count)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo, count);
        }

        public void DecreaseMOTransferQtyDefered(IUnitOfWork uow, IMES.FisObject.Common.MO.MO mo, short count)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo, count);
        }

        public void ExecuteMoUploadJOBDefered(IUnitOfWork uow, string jobName)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), jobName);
        }

        public void UpdateMoForIncreaseCustomerSnQtyDefered(IUnitOfWork uow, string productId, short count)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), productId, count);
        }

        #endregion

        #endregion

        #region . Inners .

        private void PersistInsertMO(IMES.FisObject.Common.MO.MO item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.MO.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.MO.fn_CreateDate].Value = cmDt;// item.CreateDate;
                sqlCtx.Params[_Schema.MO.fn_Mo].Value = item.MONO;
                sqlCtx.Params[_Schema.MO.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.MO.fn_Plant].Value = item.Plant;
                sqlCtx.Params[_Schema.MO.fn_Print_Qty].Value = item.PrtQty;
                sqlCtx.Params[_Schema.MO.fn_Transfer_Qty].Value = item.TransferQty;
                sqlCtx.Params[_Schema.MO.fn_Qty].Value = item.Qty;
                sqlCtx.Params[_Schema.MO.fn_SAPQty].Value = item.SAPQty;
                sqlCtx.Params[_Schema.MO.fn_SAPStatus].Value = item.SAPStatus;
                sqlCtx.Params[_Schema.MO.fn_StartDate].Value = (DateTime.MinValue == item.StartDate) ? cmDt : item.StartDate;// item.StartDate;
                sqlCtx.Params[_Schema.MO.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.MO.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.MO.fn_customerSN_Qty].Value = item.CustomerSN_Qty;
                sqlCtx.Params[_Schema.MO.fn_PoNo].Value = item.PoNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMO(IMES.FisObject.Common.MO.MO item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.MO.fn_CreateDate].Value = item.CreateDate;
                sqlCtx.Params[_Schema.MO.fn_Mo].Value = item.MONO;
                sqlCtx.Params[_Schema.MO.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.MO.fn_Plant].Value = item.Plant;
                sqlCtx.Params[_Schema.MO.fn_Print_Qty].Value = item.PrtQty;
                sqlCtx.Params[_Schema.MO.fn_Transfer_Qty].Value = item.TransferQty;
                sqlCtx.Params[_Schema.MO.fn_Qty].Value = item.Qty;
                sqlCtx.Params[_Schema.MO.fn_SAPQty].Value = item.SAPQty;
                sqlCtx.Params[_Schema.MO.fn_SAPStatus].Value = item.SAPStatus;
                sqlCtx.Params[_Schema.MO.fn_StartDate].Value = item.StartDate;
                sqlCtx.Params[_Schema.MO.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.MO.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.MO.fn_customerSN_Qty].Value = item.CustomerSN_Qty;
                sqlCtx.Params[_Schema.MO.fn_PoNo].Value = item.PoNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMO(IMES.FisObject.Common.MO.MO item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO));
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Mo].Value = item.MONO;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region For Maintain

        public IList<IMES.FisObject.Common.MO.MO> GetNonCMOList()
        {
            //选项包括Status栏位不是"C"(已出货)的所有MO
            try
            {
                IList<IMES.FisObject.Common.MO.MO> ret = new List<IMES.FisObject.Common.MO.MO>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO neqCond = new _Schema.MO();
                        neqCond.Status = "C";

                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), null, null, null, null, null, null, null, null, null, null, neqCond, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);
                        sqlCtx.Params[_Schema.MO.fn_Status].Value = neqCond.Status;
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMES.FisObject.Common.MO.MO item = new IMES.FisObject.Common.MO.MO();
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Cdt]);
                            item.CreateDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_CreateDate]);
                            item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Model]);
                            item.MONO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Mo]);
                            item.Plant = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Plant]);
                            item.PrtQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Print_Qty]);
                            item.TransferQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Transfer_Qty]);
                            item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Qty]);
                            item.SAPQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPQty]);
                            item.SAPStatus = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_SAPStatus]);
                            item.StartDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_StartDate]);
                            item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Status]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MO.fn_Udt]);
                            item.CustomerSN_Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MO.fn_customerSN_Qty]);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MO.fn_PoNo]);
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

        public DataTable GetMoByModel(string model)
        {
            //SELECT distinct MO, Qty, Print_Qty as StartQty, Udt FROM MO where Model='" + model + "' AND Status <>'C'
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MO cond = new _Schema.MO();
                        cond.Model = model;
                        _Schema.MO necond = new _Schema.MO();
                        necond.Status = "C";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), "DISTINCT", new List<string>() { _Schema.MO.fn_Mo, _Schema.MO.fn_Qty, _Schema.MO.fn_Print_Qty, _Schema.MO.fn_Udt }, cond, null, null, null, null, null, null, null, necond, null, null);

                        sqlCtx.Params[_Schema.MO.fn_Status].Value = necond.Status;
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.MO.fn_Mo],
                                                                sqlCtx.Indexes[_Schema.MO.fn_Qty],
                                                                sqlCtx.Indexes[_Schema.MO.fn_Print_Qty],
                                                                sqlCtx.Indexes[_Schema.MO.fn_Udt]
                                                                });
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region  for auto assigne MO
        /// <summary>
        /// order by CreateDate, (Qty-PrintQty) desc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetActiveMO(string model)
        {
            try
            {
                string ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select top 1 MO
                                                             from MO WITH (UPDLOCK,ROWLOCK)
                                                             where  Model =@Model   and
                                                                          Status='H'
                                                             order by CreateDate, (Qty-Print_Qty) desc ";

                        sqlCtx.Params.Add("Model", new SqlParameter("@Model", SqlDbType.VarChar));


                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["Model"].Value = model;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                              CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, 0);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string GetActiveMOWithNoneLock(string model)
        {
            try
            {
                string ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select top 1 MO
                                                             from MO WITH (UPDLOCK,ROWLOCK,READPAST)
                                                             where  Model =@Model   and
                                                                          Status='H'
                                                             order by CreateDate, (Qty-Print_Qty) desc ";

                        sqlCtx.Params.Add("Model", new SqlParameter("@Model", SqlDbType.VarChar));


                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["Model"].Value = model;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                              CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, 0);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void AssignedMO(string mo)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"update MO
                                                                set Print_Qty=Print_Qty+1,
                                                                    Status= case when (Qty-Print_Qty)>1 then  
                                                                                  'H'
                                                                                  else
                                                                                   'C'
                                                                                  end,
                                                                    Udt=getdate()     
                                                                 where MO=@MO ";

                        sqlCtx.Params.Add("MO", new SqlParameter("@MO", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["MO"].Value = mo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());

            }
            catch (Exception)
            {
                throw;
            }

        }
        public void AssignedMODefered(IUnitOfWork uow, string mo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo);
        }

        public void RollbackAssignedMO(string mo, string productId)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"update MO
                                                                set Print_Qty=Print_Qty-1,
                                                                    Status= case when (Qty-Print_Qty)>=0 then  
                                                                                                      'H'
                                                                                            else
                                                                                                    'C'
                                                                                   end,
                                                                    Udt=getdate()  
                                                                 where MO=@MO;
 
                                                               update Product
                                                                set MO=''
                                                                where ProductID=@ProductID;";

                        sqlCtx.Params.Add("MO", new SqlParameter("@MO", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ProductID", new SqlParameter("@ProductID", SqlDbType.VarChar));
                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["MO"].Value = mo;
                sqlCtx.Params["ProductID"].Value = productId;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region PilotMo table
        public void InsertPilotMo(PilotMoInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;

                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = _Metas.FuncNew.GetCommonInsert<_Metas.PilotMo>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(sqlCtx, item);
                //sqlCtx.Param(_Metas.PilotMo.fn_state).Value = item.state.ToString();
                //sqlCtx.Param(_Metas.PilotMo.fn_combinedState).Value = item.combinedState.ToString();
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.PilotMo.fn_udt).Value = cmDt;
                sqlCtx.Param(_Metas.PilotMo.fn_cdt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
                                                                                  CommandType.Text,
                                                                                  sqlCtx.Sentence,
                                                                                  sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPilotMoDefered(IUnitOfWork uow, PilotMoInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }
        public void UpdatePilotMo(PilotMoInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;


                lock (mthObj)
                {
                    _Metas.PilotMo cond = new _Metas.PilotMo();
                    cond.mo = item.mo;
                    _Metas.PilotMo setv = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(item, _Metas.PilotMo.fn_mo);
                    setv.udt = DateTime.Now;

                    sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.PilotMo>(new _Metas.SetValueCollection<_Metas.PilotMo>(new _Metas.CommonSetValue<_Metas.PilotMo>(setv)),
                                                                                                                          new _Metas.ConditionCollection<_Metas.PilotMo>(new _Metas.EqualCondition<_Metas.PilotMo>(cond)));

                }

                sqlCtx.Param(_Metas.PilotMo.fn_mo).Value = item.mo;

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.PilotMo.fn_udt)).Value = cmDt;
                //sqlCtx.Param(g.DecSV(_Metas.PilotMo.fn_state)).Value = item.state.ToString();
                //sqlCtx.Param(g.DecSV(_Metas.PilotMo.fn_combinedState)).Value = item.combinedState.ToString();

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdatePilotMoDefered(IUnitOfWork uow, PilotMoInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePilotMo(string mo)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.PilotMo cond = new _Metas.PilotMo();
                        cond.mo = mo;
                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.PilotMo>(tk, new ConditionCollection<_Metas.PilotMo>(new EqualCondition<_Metas.PilotMo>(cond)));

                    }
                }
                sqlCtx.Param(_Metas.PilotMo.fn_mo).Value = mo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePilotMoDefered(IUnitOfWork uow, string mo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo);
        }

        public IList<PilotMoInfo> SearchPilotMo(PilotMoInfo condition)
        {
            try
            {
                IList<PilotMoInfo> ret = new List<PilotMoInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.PilotMo cond = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(condition);
                    sqlCtx = _Metas.FuncNew.GetConditionedSelect<_Metas.PilotMo>(null, null,
                                                                                                       new _Metas.ConditionCollection<_Metas.PilotMo>(new _Metas.EqualCondition<_Metas.PilotMo>(cond)),
                                                                                                       _Metas.PilotMo.fn_mo, _Metas.PilotMo.fn_stage);

                }

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(sqlCtx, condition);


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = _Metas.FuncNew.SetFieldFromColumn<_Metas.PilotMo, PilotMoInfo, PilotMoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PilotMoInfo> SearchPilotMo(PilotMoInfo condition, IList<string> combinedState)
        {
            try
            {
                IList<PilotMoInfo> ret = new List<PilotMoInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.PilotMo cond = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(condition);
                    _Metas.PilotMo conditionIn = new _Metas.PilotMo();
                    conditionIn.combinedState = "[INSET]";
                    sqlCtx = _Metas.FuncNew.GetConditionedSelect<_Metas.PilotMo>(null, null,
                                                                                                       new _Metas.ConditionCollection<_Metas.PilotMo>(
                                                                                                           new _Metas.EqualCondition<_Metas.PilotMo>(cond),
                                                                                                           new _Metas.InSetCondition<_Metas.PilotMo>(conditionIn)),
                                                                                                       _Metas.PilotMo.fn_mo, _Metas.PilotMo.fn_stage);

                }

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(sqlCtx, condition);

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PilotMo.fn_combinedState), g.ConvertInSet(combinedState));
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                     CommandType.Text,
                                                                                                                     Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = _Metas.FuncNew.SetFieldFromColumn<_Metas.PilotMo, PilotMoInfo, PilotMoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IList<PilotMoInfo> SearchPilotMo(PilotMoInfo condition, DateTime beginCdt, DateTime endCdt)
        {
            try
            {
                IList<PilotMoInfo> ret = new List<PilotMoInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.PilotMo cond = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(condition);
                    _Metas.PilotMo betweenCond = new _Metas.PilotMo();
                    betweenCond.cdt = DateTime.Now;
                    sqlCtx = _Metas.FuncNew.GetConditionedSelect<_Metas.PilotMo>(null, null,
                                                                                                       new _Metas.ConditionCollection<_Metas.PilotMo>(
                                                                                                           new _Metas.EqualCondition<_Metas.PilotMo>(cond),
                                                                                                           new _Metas.BetweenCondition<_Metas.PilotMo>(betweenCond)),
                                                                                                       _Metas.PilotMo.fn_mo, _Metas.PilotMo.fn_stage);

                }

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.PilotMo, PilotMoInfo>(sqlCtx, condition);
                sqlCtx.Param(g.DecBeg(_Metas.PilotMo.fn_cdt)).Value = beginCdt;
                sqlCtx.Param(g.DecEnd(_Metas.PilotMo.fn_cdt)).Value = endCdt;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = _Metas.FuncNew.SetFieldFromColumn<_Metas.PilotMo, PilotMoInfo, PilotMoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PilotMoInfo GetPilotMo(string mo)
        {
            try
            {
                PilotMoInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.PilotMo cond = new _Metas.PilotMo();
                        cond.mo = mo;
                        sqlCtx = _Metas.FuncNew.GetConditionedSelect<_Metas.PilotMo>(null, null,
                                                                                                           new _Metas.ConditionCollection<_Metas.PilotMo>(
                                                                                                               new _Metas.EqualCondition<_Metas.PilotMo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PilotMo.fn_mo).Value = mo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                    CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = _Metas.FuncNew.SetFieldFromColumn<_Metas.PilotMo, PilotMoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PilotMoInfo GetAndLockPilotMo(string mo)
        {
            try
            {
                PilotMoInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.PilotMo cond = new _Metas.PilotMo();
                        cond.mo = mo;
                        sqlCtx = _Metas.FuncNew.GetConditionedSelect<_Metas.PilotMo>(null, null,
                                                                                                           new _Metas.ConditionCollection<_Metas.PilotMo>(
                                                                                                               new _Metas.EqualCondition<_Metas.PilotMo>(cond)));
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=PilotMo_PK,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.PilotMo.fn_mo).Value = mo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = _Metas.FuncNew.SetFieldFromColumn<_Metas.PilotMo, PilotMoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CheckAndCombinedPilotMo(string mo, int qty, string editor)
        {
            PilotMoInfo pilotMo = this.GetAndLockPilotMo(mo);
            if (pilotMo == null)
            {
                throw new FisException("CHK1094", new string[] { mo });
            }
            int remaingQty = pilotMo.qty - pilotMo.combinedQty;
            if (remaingQty < qty)
            {
                throw new FisException("CHK1094", new string[] { mo, pilotMo.qty.ToString(), pilotMo.combinedQty.ToString(), qty.ToString() });
            }
            remaingQty = remaingQty - qty;
            if (remaingQty > 0)
            {
                pilotMo.combinedState = PilotMoCombinedStateEnum.Partial.ToString();
            }
            else
            {
                pilotMo.combinedState = PilotMoCombinedStateEnum.Full.ToString();
            }
            pilotMo.editor = editor;
            pilotMo.combinedQty = pilotMo.combinedQty + qty;
            this.UpdatePilotMo(pilotMo);
        }
        public void CheckAndCombinedPilotMoDefered(IUnitOfWork uow, string mo, int qty, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo, qty, editor);
        }
        #endregion

        #region for assign PoNo
        public bool CheckModelBindPoNo(string model)
        {
            try
            {
                bool ret = false;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Schema.MO cond = new _Schema.MO();
                        cond.Model = model;

                        _Schema.MO notCond = new _Schema.MO();
                        notCond.PoNo = string.Empty;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(tk, typeof(_Schema.MO),"TOP 1", new List<string>{ _Schema.MO.fn_Mo},  
                                                                                                                            cond,null,null,null,null,null,null,null,
                                                                                                                            notCond,null,null);
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;
                sqlCtx.Params[_Schema.MO.fn_PoNo].Value = string.Empty;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                                                  CommandType.Text, 
                                                                                                                  sqlCtx.Sentence, 
                                                                                                                  sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null )
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

        public IList<string> GetBindPoNoByModel(string model)
        {
            try
            {
                IList<string> ret = new List<string>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Schema.MO cond = new _Schema.MO();
                        cond.Model = model;

                        _Schema.MO notCond = new _Schema.MO();
                        notCond.PoNo = string.Empty;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(tk, typeof(_Schema.MO), "DISTINCT ", new List<string> { _Schema.MO.fn_PoNo },
                                                                                                                            cond, null, null, null, null, null, null, null,
                                                                                                                            notCond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MO.fn_Model].Value = model;
                sqlCtx.Params[_Schema.MO.fn_PoNo].Value = string.Empty;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                  CommandType.Text,
                                                                                                                  sqlCtx.Sentence,
                                                                                                                  sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(sqlR.GetString(0).TrimEnd());
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
        #endregion
    }
}
