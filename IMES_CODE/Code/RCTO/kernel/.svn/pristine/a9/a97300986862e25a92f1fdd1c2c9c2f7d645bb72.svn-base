// 2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0159 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.Util;
using IMES.FisObject.Common.Warranty;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Warranty;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Warranty相关
    /// </summary>
    public class WarrantyRepository : BaseRepository<fons.Warranty>, IWarrantyRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<Warranty>

        protected override void PersistNewItem(fons.Warranty item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    PersistInsertWarranty(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(fons.Warranty item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    PersistUpdateWarranty(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(fons.Warranty item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    PersistDeleteWarranty(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Warranty>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.Warranty Find(object key)
        {
            try
            {
                fons.Warranty ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Warranty cond = new _Schema.Warranty();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Warranty.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.Warranty(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Customer]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Type]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_DateCodeType]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyFormat]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ShipTypeCode]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyCode]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Descr]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Editor]),
                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Cdt]),
                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Udt])
                                            );
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
        public override IList<fons.Warranty> FindAll()
        {
            try
            {
                IList<fons.Warranty> ret = new List<fons.Warranty>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons.Warranty item = new fons.Warranty(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Customer]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Type]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_DateCodeType]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyFormat]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ShipTypeCode]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyCode]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Descr]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Cdt]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Udt])
                                                        );
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
        public override void Add(fons.Warranty item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(fons.Warranty item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="work"></param>
        public override void Update(fons.Warranty item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IWarrantyRepository

        public IList<fons.Warranty> GetDCodeRuleListForMB(bool isFRU, string customer)
        {
            return GetDCodeRuleListByTypeAndShipType(fons.Warranty.WarrantyType.MBDateCode, 
                isFRU ?
                new string[] { fons.Warranty.ShippingTypeCode.H, fons.Warranty.ShippingTypeCode.C } 
                :
                new string[] { fons.Warranty.ShippingTypeCode.J, fons.Warranty.ShippingTypeCode.P }, customer, false);
        }

        public IList<fons.Warranty> GetDCodeRuleListForMB(string customer)
        {
            //return GetDCodeRuleListByTypeAndShipType(Warranty.WarrantyType.MBDateCode, new string[] { Warranty.ShippingTypeCode.H, Warranty.ShippingTypeCode.C, Warranty.ShippingTypeCode.J, Warranty.ShippingTypeCode.P }, customer);
            return GetDCodeRuleListByTypeAndShipType(fons.Warranty.WarrantyType.MBDateCode, null, customer, true);
        }

        public IList<fons.Warranty> GetDCodeRuleListForVB(string customer)
        {
            return GetDCodeRuleListByTypeAndShipType(fons.Warranty.WarrantyType.VBDateCode, null, customer, false);
        }

        public IList<fons.Warranty> GetDCodeRuleListForKP(string customer)
        {
            return GetDCodeRuleListByTypeAndShipType(fons.Warranty.WarrantyType.KPDateCode, null, customer, false);
        }

        public IList<fons.Warranty> GetDCodeRuleListForDK(string customer)
        {
            return GetDCodeRuleListByTypeAndShipType(fons.Warranty.WarrantyType.DKDateCode, null, customer, false);
        }

        public IList<fons.Warranty> GetWarrantyListByCondition(fons.Warranty condition)
        {
            try
            {
                IList<fons.Warranty> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Warranty cond = mtns::FuncNew.SetColumnFromField<mtns::Warranty, fons.Warranty>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Warranty>(null, null, new mtns::ConditionCollection<mtns::Warranty>(new mtns::EqualCondition<mtns::Warranty>(cond)), mtns::Warranty.fn_warrantyCode);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Warranty, fons.Warranty>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Warranty, fons.Warranty, fons.Warranty>(ret, sqlR, sqlCtx);
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
        private void PersistInsertWarranty(fons.Warranty item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Warranty.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Warranty.fn_Customer].Value = item.Customer;
                sqlCtx.Params[_Schema.Warranty.fn_DateCodeType].Value = item.DateCodeType;
                sqlCtx.Params[_Schema.Warranty.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Warranty.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.Warranty.fn_ID].Value = item.Id;
                sqlCtx.Params[_Schema.Warranty.fn_ShipTypeCode].Value = item.ShipTypeCode;
                sqlCtx.Params[_Schema.Warranty.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.Warranty.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Warranty.fn_WarrantyCode].Value = item.WarrantyCode;
                sqlCtx.Params[_Schema.Warranty.fn_WarrantyFormat].Value = item.WarrantyFormat;
                item.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateWarranty(fons.Warranty item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Warranty.fn_Customer].Value = item.Customer;
                sqlCtx.Params[_Schema.Warranty.fn_DateCodeType].Value = item.DateCodeType;
                sqlCtx.Params[_Schema.Warranty.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Warranty.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Warranty.fn_ID].Value = item.Id;
                sqlCtx.Params[_Schema.Warranty.fn_ShipTypeCode].Value = item.ShipTypeCode;
                sqlCtx.Params[_Schema.Warranty.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.Warranty.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Warranty.fn_WarrantyCode].Value = item.WarrantyCode;
                sqlCtx.Params[_Schema.Warranty.fn_WarrantyFormat].Value = item.WarrantyFormat;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteWarranty(fons.Warranty item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty));
                    }
                }
                sqlCtx.Params[_Schema.Warranty.fn_ID].Value = item.Id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons.Warranty> GetDCodeRuleListByTypeAndShipType(string warrantyType, string[] shipTypeCodes, string customer, bool isPeerSysSetting)
        {
            try
            {
                IList<fons.Warranty> ret = new List<fons.Warranty>();

                string Sentence = string.Empty;// 2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0159 
                _Schema.SQLContext sqlCtx = null;
                if (shipTypeCodes != null)
                {
                    sqlCtx = this.ComposeForGetDCodeRuleListByTypeAndShipType(warrantyType, shipTypeCodes, customer);
                    Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.Warranty.fn_ShipTypeCode), _Schema.Func.ConvertInSet(shipTypeCodes));
                }
                else
                {
                    if (isPeerSysSetting)
                    {
                        SQLContextNew sqlCtxNew = this.ComposeForGetDCodeRuleListByTypeAndShipTypePeerSysSetting(warrantyType, customer);
                        sqlCtx = _Schema.SQLContext.ToOld(sqlCtxNew);
                        Sentence = sqlCtx.Sentence;
                    }
                    else
                    {
                        sqlCtx = this.ComposeForGetDCodeRuleListByTypeAndShipType(warrantyType, customer);
                        Sentence = sqlCtx.Sentence;
                    }
                }
                sqlCtx.Params[_Schema.Warranty.fn_Type].Value = warrantyType;
                if (false == isPeerSysSetting)
                    sqlCtx.Params[_Schema.Warranty.fn_Customer].Value = customer;
                else
                    sqlCtx.Params[g.DecAny(_Schema.Warranty.fn_Customer)].Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null) 
                    {
                        while (sqlR.Read())
                        {
                            fons.Warranty item = new fons.Warranty(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ID]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Customer]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Type]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_DateCodeType]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyFormat]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ShipTypeCode]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyCode]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Descr]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Editor]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Cdt]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Udt])
                                                );
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

        private _Schema.SQLContext ComposeForGetDCodeRuleListByTypeAndShipType(string warrantyType, string customer)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Warranty cond = new _Schema.Warranty();
                    cond.Type = warrantyType;
                    cond.Customer = customer;

                    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty), cond, null, null);
                }
            }
            return sqlCtx;
        }

        private _Metas.SQLContextNew ComposeForGetDCodeRuleListByTypeAndShipTypePeerSysSetting(string warrantyType, string customer)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            _Metas.SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.Warranty cond = new _Metas.Warranty();
                    cond.type = warrantyType;

                    _Metas.Warranty cond2 = new _Metas.Warranty();
                    cond2.customer = customer;

                    sqlCtx = _Metas.FuncNew.GetConditionedSelect<_Metas.Warranty>(tk, null, null, new ConditionCollection<_Metas.Warranty>(
                        new EqualCondition<_Metas.Warranty>(cond),
                        new AnyCondition<_Metas.Warranty>(cond2, string.Format("{0}=(SELECT {1} FROM {2} where {3}={4})", "{0}", _Metas.SysSetting.fn_value, ToolsNew.GetTableName(typeof(_Metas.SysSetting)), _Metas.SysSetting.fn_name, "{1}"))));
                }
            }
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetDCodeRuleListByTypeAndShipType(string warrantyType, string[] shipTypeCodes, string customer)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Warranty cond = new _Schema.Warranty();
                    cond.Type = warrantyType;
                    cond.Customer = customer;

                    _Schema.Warranty inSetCond = null;
                    inSetCond = new _Schema.Warranty();
                    inSetCond.ShipTypeCode = "INSET";

                    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty), cond, null, inSetCond);
                }
            }
            return sqlCtx;
        }

        #endregion

        #region For Maintain

        public IList<fons.Warranty> GetWarrantyList(string customerId)
        {
            try
            {
                IList<fons.Warranty> ret = new List<fons.Warranty>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Warranty cond = new _Schema.Warranty();
                        cond.Customer = customerId;

                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Warranty.fn_Descr);
                    }
                }
                sqlCtx.Params[_Schema.Warranty.fn_Customer].Value = customerId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons.Warranty item = new fons.Warranty(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ID]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Customer]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Type]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_DateCodeType]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyFormat]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ShipTypeCode]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyCode]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Descr]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Editor]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Cdt]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Udt])
                                                );
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

        public fons.Warranty GetWarranty(int warrantyId)
        {
            try
            {
                fons.Warranty ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Warranty cond = new _Schema.Warranty();
                        cond.ID = warrantyId;

                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Warranty.fn_ID].Value = warrantyId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = new fons.Warranty(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ID]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Customer]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Type]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_DateCodeType]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyFormat]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_ShipTypeCode]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_WarrantyCode]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Descr]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Editor]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Cdt]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Warranty.fn_Udt])
                                                );
                            ret.Tracker.Clear();
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

        public DataTable GetExistWarranty(string customer, string descr, int id)
        {
            //SELECT [ID]      
            //  FROM [IMES_GetData].[dbo].[Warranty]
            //where [Customer]='Customer' AND [Descr]='Descr' AND ID<>id
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Warranty cond = new _Schema.Warranty();
                        cond.Customer = customer;
                        cond.Descr = descr;
                        _Schema.Warranty neqCond = new _Schema.Warranty();
                        neqCond.ID = id;
                        
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty), null, new List<string>() { _Schema.Warranty.fn_ID }, cond, null, null, null, null, null, null, null, neqCond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Warranty.fn_Customer].Value = customer;
                sqlCtx.Params[_Schema.Warranty.fn_Descr].Value = descr;
                sqlCtx.Params[_Schema.Warranty.fn_ID].Value = id;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public DataTable GetExistWarranty(string customer, string descr)
        {
            //SELECT [ID]      
            //  FROM [IMES_GetData].[dbo].[Warranty]
            //where [Customer]='Customer' AND [Descr]='Descr'
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Warranty cond = new _Schema.Warranty();
                        cond.Customer = customer;
                        cond.Descr = descr;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Warranty), null, new List<string>() { _Schema.Warranty.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Warranty.fn_Customer].Value = customer;
                sqlCtx.Params[_Schema.Warranty.fn_Descr].Value = descr;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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
