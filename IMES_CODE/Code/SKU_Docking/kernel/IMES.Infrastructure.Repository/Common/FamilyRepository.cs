// 2010-01-26 Liu Dong(eB1-4)         Modify ITC-1122-0104
// 2010-03-11 Liu Dong(eB1-4)         Modify 删除Family的时候不再删除ModelProcessRule

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Utility;
using IMES.DataModel;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using IMES.Infrastructure.Utility.Cache;
using System.Configuration;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using IMES.Infrastructure.Util;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository._Metas;
using mdlns = IMES.FisObject.Common.Model;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Model;
using System.Threading;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Family相关
    /// </summary>
    public class FamilyRepository : BaseRepository<fons.Family>, IFamilyRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        private static CacheManager _cache_real = null;
        private static CacheManager _cache_fml
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = CacheFactory.GetCacheManager("FamilyCache");
                return _cache_real;
            }
        }
        private static IDictionary<string, QCRatio> _cache_ratio = new Dictionary<string, QCRatio>();
        private static object _syncObj_cache_ratio = new object();
      
        private readonly static FamilyRefreshAction cacheRefreshAction = new FamilyRefreshAction();
               
        #endregion

        #region Overrides of BaseRepository<Model>

        protected override void PersistNewItem(fons.Family item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertFamily(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.Family));
                }
            }
            finally
            {
                tracker.Clear();
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        protected override void PersistUpdatedItem(fons.Family item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateFamily(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.Family));
                }
            }
            finally
            {
                tracker.Clear();
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        protected override void PersistDeletedItem(fons.Family item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    // 2010-03-11 Liu Dong(eB1-4)         Modify 删除Family的时候不再删除ModelProcessRule
                     //在ModelProcessRule表中删除该Family对应的通用规则
                    //this.PersistDeleteModelProcessRule(item.FamilyName);

                    this.PersistDeleteFamily(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.Family));
                }
            }
            finally
            {
                tracker.Clear();
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region IRepository<Family> Members

        public fons.Family Find(object key)
        {
            return FindFamily((string)key);
        }

        public IList<fons.Family> FindAll()
        {
            try
            {
                IList<fons.Family> ret = new List<fons.Family>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Family.fn_family);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Family item = new fons.Family(
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_family]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Descr]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_CustomerID])
                                                );
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Editor]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Udt]);
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

        public void Add(fons.Family item, IMES.Infrastructure.UnitOfWork.IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        public void Remove(fons.Family item, IMES.Infrastructure.UnitOfWork.IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        public void Update(fons.Family item, IMES.Infrastructure.UnitOfWork.IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region IFamilyRepository Members

        /// <summary>
        /// 根据Family获取OQCRatio
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public QCRatio GetQCRatio(string family)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                if (!IsCached())
                    return GetQCRatio_DB(family);

                QCRatio ret = GetQCRatio_Cache(family);
                if (ret == null)
                {
                    ret = GetQCRatio_DB(family);
                }
                return ret;
            }
            catch (Exception)
            {
                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        public fons.Family FindFamily(string family)
        {
            try
            {
                if (!IsCached())
                    return FindFamily_DB(family);

                fons.Family ret = FindFamily_Cache(family);
                if (ret == null)
                {
                    ret = FindFamily_DB(family);

                    if (IsCached() && ret != null)
                    {
                        #region old
                        //Monitor.Enter(ModelRepository._syncObj_cache);
                        ////lock (ModelRepository._syncObj_cache)
                        //try
                        //{
                        //    if (!_cache_fml.Contains(ret.FamilyName))
                        //        AddToCache_Family(ret.FamilyName, ret);
                        //}
                        //finally
                        //{
                        //    Monitor.Exit(ModelRepository._syncObj_cache);
                        //}
                        #endregion

                        ParameterizedThreadStart ts = new ParameterizedThreadStart(new ModelRepository.AsynCallFor(AsynCallFor_FindFamily));
                        Thread thrd = new Thread(ts);
                        thrd.Start(new object[] { ret });
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AsynCallFor_FindFamily(object parameters)
        {
            fons.Family ret = (fons.Family)((object[])parameters)[0];

            Monitor.Enter(ModelRepository._syncObj_cache);
            //lock (ModelRepository._syncObj_cache)
            try
            {
                if (!_cache_fml.Contains(ret.FamilyName))
                    AddToCache_Family(ret.FamilyName, ret);
            }
            finally
            {
                Monitor.Exit(ModelRepository._syncObj_cache);
            }
        }

        public IList<IMES.DataModel.FamilyInfo> GetFamilyList()
        {
            try
            {
                IList<IMES.DataModel.FamilyInfo> ret = new List<IMES.DataModel.FamilyInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.FamilyInfo fi = new IMES.DataModel.FamilyInfo();
                        fi.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Descr]);
                        fi.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_family]);
                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_family]),
                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Descr]),
                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_CustomerID])
                        ret.Add(fi);
                    }
                }
                return (from item in ret orderby item.friendlyName select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public fons.Model FillFamilyObj(fons.Model item)
        {
            try
            {
                fons.Family newFieldVal = FindFamily(item.FamilyName);
                item.GetType().GetField("_fmlObj", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, newFieldVal);
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.FamilyInfo> FindFamiliesByCustomer(string customer)
        {
            try
            {
                IList<IMES.DataModel.FamilyInfo> ret = new List<IMES.DataModel.FamilyInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Family cond = new _Schema.Family();
                        cond.CustomerID = customer;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Family.fn_family);
                    }
                }
                sqlCtx.Params[_Schema.Family.fn_CustomerID].Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.FamilyInfo item = new IMES.DataModel.FamilyInfo();
                        item.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Descr]);
                        item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_family]);
                        ret.Add(item);
                    }
                }
                return (from item in ret orderby item.friendlyName select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.FamilyInfo> FindFamiliesByCustomerOrderByFamily(string customer)
        {
            try
            {
                IList<IMES.DataModel.FamilyInfo> ret = new List<IMES.DataModel.FamilyInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Family cond = new _Schema.Family();
                        cond.CustomerID = customer;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Family.fn_family);
                    }
                }
                sqlCtx.Params[_Schema.Family.fn_CustomerID].Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.FamilyInfo item = new IMES.DataModel.FamilyInfo();
                        item.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Descr]);
                        item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_family]);
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

        public IList<string> GetFamilysByCustomer(string[] customerIds)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Family cond = new mtns::Family();
                        cond.customerID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Family>(tk, "DISTINCT", new string[] { mtns::Family.fn_family }, new ConditionCollection<mtns::Family>(new InSetCondition<mtns::Family>(cond)), mtns::Family.fn_family);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Family.fn_customerID), g.ConvertInSet(customerIds));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Family.fn_family));
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

        public int GetCountOfQCStatusInCurrentMonth(string line, string model, string tp)
        {
            try
            {
                int ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Qcstatus cond = new _Metas.Qcstatus();
                        cond.line = line;
                        cond.model = model;
                        cond.tp = tp;

                        _Metas.Qcstatus cond2 = new _Metas.Qcstatus();
                        cond2.cdt = DateTime.Now;

                        _Metas.Qcstatus cond3 = new _Metas.Qcstatus();
                        cond3.cdt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Qcstatus>(tk, "COUNT", new string[][] { new string[] { _Metas.Qcstatus.fn_productID, string.Format("DISTINCT {0}", _Metas.Qcstatus.fn_productID) } },
                            new ConditionCollection<_Metas.Qcstatus>(new EqualCondition<_Metas.Qcstatus>(cond),
                                new GreaterOrEqualCondition<_Metas.Qcstatus>(cond2),
                                new SmallerCondition<_Metas.Qcstatus>(cond3)));
                    }
                }
                sqlCtx.Param(mtns.Qcstatus.fn_line).Value = line;
                sqlCtx.Param(mtns.Qcstatus.fn_model).Value = model;
                sqlCtx.Param(mtns.Qcstatus.fn_tp).Value = tp;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecGE(mtns.Qcstatus.fn_cdt)).Value = new DateTime(cmDt.Year, cmDt.Month, 1, 0, 0, 0, 0);
                DateTime cmDt2 = cmDt.AddMonths(1);
                sqlCtx.Param(g.DecS(mtns.Qcstatus.fn_cdt)).Value = new DateTime(cmDt2.Year, cmDt2.Month, 1, 0, 0, 0, 0);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<int> GetEOQCRatioList(QCRatioInfo condition)
        {
            try
            {
                IList<int> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Qcratio cond = mtns::FuncNew.SetColumnFromField<mtns::Qcratio, QCRatioInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Qcratio>(null, new string[] { mtns.Qcratio.fn_eoqcratio }, new mtns::ConditionCollection<mtns::Qcratio>(new mtns::EqualCondition<mtns::Qcratio>(cond)), mtns::Qcratio.fn_eoqcratio);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Qcratio, QCRatioInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<int>();
                        while (sqlR.Read())
                        {
                            int item = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns.Qcratio.fn_eoqcratio));
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

        public IList<QCRatioInfo> GetQCRatioInfoList(QCRatioInfo condition)
        {
            try
            {
                IList<QCRatioInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Qcratio cond = mtns::FuncNew.SetColumnFromField<mtns::Qcratio, QCRatioInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Qcratio>(null, null, new mtns::ConditionCollection<mtns::Qcratio>(new mtns::EqualCondition<mtns::Qcratio>(cond)), mtns::Qcratio.fn_eoqcratio);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Qcratio, QCRatioInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Qcratio, QCRatioInfo, QCRatioInfo>(ret, sqlR, sqlCtx);
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

        private IList<QCRatio> GetAllQCRatio_DB()
        {
            try
            {
                IList<QCRatio> ret = new List<QCRatio>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCRatio));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        QCRatio item = new QCRatio(//GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_ID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Family]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_qcRatio]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_EOQCRatio]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_PAQCRatio]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Editor]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Cdt]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Udt]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_RPAQCRatio])
                            );

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

        private QCRatio GetQCRatio_DB(string family)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                //QCRatio Family 一對一
                QCRatio ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.QCRatio cond = new _Schema.QCRatio();
                        cond.Family = family;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCRatio), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.QCRatio.fn_Family].Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new QCRatio(//GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_ID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Family]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_qcRatio]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_EOQCRatio]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_PAQCRatio]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Editor]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Cdt]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_Udt]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.QCRatio.fn_RPAQCRatio])
                            );
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private QCRatio GetQCRatio_Cache(string family)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                QCRatio ret = null;
                lock (_syncObj_cache_ratio)
                {
                    //LoggingInfoFormat("GetQCRatio_Cache: Keys[{0}]",string.Join(",", _cache_ratio.Keys.ToArray()));

                    if (_cache_ratio.ContainsKey(family))
                        ret = _cache_ratio[family];
                }
                return ret;
            }
            catch (Exception)
            {
                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private fons.Family FindFamily_DB(string family)
        {
            try
            {
                fons.Family ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Family cond = new _Schema.Family();
                        cond.family = family;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Family.fn_family].Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.Family(
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_family]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Descr]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_CustomerID])
                            );
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Editor]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Udt]);
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

        private fons.Family FindFamily_Cache(string family)
        {
            try
            {
                fons.Family ret = null;

                if (Monitor.TryEnter(ModelRepository._syncObj_cache, ModelRepository._lockWaitSeconds))
                {
                    //lock (ModelRepository._syncObj_cache)
                    try
                    {
                        if (_cache_fml.Contains(family))
                            ret = (fons.Family)_cache_fml[family];
                    }
                    finally
                    {
                        Monitor.Exit(ModelRepository._syncObj_cache);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertFamily(fons.Family item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Family.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Family.fn_CustomerID].Value = item.Customer;
                sqlCtx.Params[_Schema.Family.fn_Descr].Value = item.Description;
                sqlCtx.Params[_Schema.Family.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Family.fn_family].Value = item.FamilyName;
                sqlCtx.Params[_Schema.Family.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateFamily(fons.Family item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Family.fn_CustomerID].Value = item.Customer;
                sqlCtx.Params[_Schema.Family.fn_Descr].Value = item.Description;
                sqlCtx.Params[_Schema.Family.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Family.fn_family].Value = item.FamilyName;
                sqlCtx.Params[_Schema.Family.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteFamily(fons.Family item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family));
                    }
                }
                sqlCtx.Params[_Schema.Family.fn_family].Value = item.FamilyName;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        //// 2010-03-11 Liu Dong(eB1-4)         Modify 删除Family的时候不再删除ModelProcessRule
        //private void PersistDeleteModelProcessRule(string family)
        //{
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelProcessRule cond = new _Schema.ModelProcessRule();
        //                cond.Family = family;
        //                sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelProcessRule), cond, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelProcessRule.fn_Family].Value = family;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        #region ICache Members

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Family);
        }

        public void ProcessItem(IMES.DataModel.CacheUpdateInfo item)
        {
            if (item.Type == IMES.DataModel.CacheType.Family)
                LoadOneCacheFamily(item.Item);
            else if (item.Type == IMES.DataModel.CacheType.QCRatio)
                LoadAllCacheQCRatio();
        }

        public void ClearCache()
        {
            Monitor.Enter(ModelRepository._syncObj_cache);
            //lock (ModelRepository._syncObj_cache)
            try
            {
                _cache_fml.Flush();
            }
            finally
            {
                Monitor.Exit(ModelRepository._syncObj_cache);
            }
        }

        private void LoadOneCacheFamily(string pk)
        {
            Monitor.Enter(ModelRepository._syncObj_cache);
            //lock (ModelRepository._syncObj_cache)
            try
            {
                if (_cache_fml.Contains(pk))
                {
                    _cache_fml.Remove(pk);
                }

                #region For YWH
                /*
                fons.Family family = FindFamily_DB(pk);
                if (family != null)
                    AddToCache_Family(pk, family);
                */
                #endregion

                if (ModelRepository._fml2mdlMapping.ContainsKey(pk))
                    ModelRepository._fml2mdlMapping.Remove(pk);
            }
            finally
            {
                Monitor.Exit(ModelRepository._syncObj_cache);
            }
        }

        private void LoadAllCacheQCRatio()
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                IList<QCRatio> qcrts = this.GetAllQCRatio_DB();
                if (qcrts != null && qcrts.Count > 0)
                {
                    lock (_syncObj_cache_ratio)
                    {
                        _cache_ratio.Clear();

                        foreach (QCRatio qcrt in qcrts)
                        {
                            if (!_cache_ratio.ContainsKey(qcrt.Family))//Family应是表QCRatio的主键
                                _cache_ratio.Add(qcrt.Family, qcrt);
                        }

                        //LoggingInfoFormat("LoadAllCacheQCRatio: Keys[{0}]", string.Join(",", _cache_ratio.Keys.ToArray()));
                    }
                }
            }
            catch (Exception)
            {
                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private IMES.DataModel.CacheUpdateInfo GetACacheSignal(string pk, string type)
        {
            IMES.DataModel.CacheUpdateInfo ret = new IMES.DataModel.CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = type;
            ret.Item = pk;
            return ret;
        }

        private void AddToCache_Family(string key, object obj)
        {
            //Vincent 2015-11-15 modify static AbsoluteTime
            //_cache_fml.Add(key, obj, CacheItemPriority.Normal, new FamilyRefreshAction(), new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_FamilyCache"].ToString()))));
            //_cache_fml.Add(key, obj, CacheItemPriority.Normal, cacheRefreshAction, _cacheAbsoluteTime);
            _cache_fml.Add(key, obj, CacheItemPriority.Normal, cacheRefreshAction, new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_FamilyCache"].ToString()))));
        }

        [Serializable]
        private class FamilyRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
                Monitor.Enter(ModelRepository._syncObj_cache);
                //lock (ModelRepository._syncObj_cache)
                try
                {
                    if (ModelRepository._fml2mdlMapping.ContainsKey(key))
                        ModelRepository._fml2mdlMapping.Remove(key);
                }
                finally
                {
                    Monitor.Exit(ModelRepository._syncObj_cache);
                }
            }
        }

        #endregion

        #region For Maintain

        public IList<fons.Family> GetFamilyList(string customerId)
        {
            try
            {
                IList<fons.Family> ret = new List<fons.Family>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Family cond = new _Schema.Family();
                        cond.CustomerID = customerId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Family.fn_family);
                    }
                }
                sqlCtx.Params[_Schema.Family.fn_CustomerID].Value = customerId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Family item = new fons.Family(
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_family]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Descr]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_CustomerID])
                            );
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Editor]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Family.fn_Udt]);
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

        public IList<fons.Family> GetFamilyObjList()
        {
            return this.FindAll();
        }

        public void ChangeFamily(fons.Family item, string oldFamilyName)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(oldFamilyName, CacheType.Family));
                // 2010-01-26 Liu Dong(eB1-4)         Modify ITC-1122-0104
                if (oldFamilyName != item.FamilyName)
                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.FamilyName, CacheType.Family));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Family cond = new _Schema.Family();
                        cond.family = oldFamilyName;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Family), null, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Family.fn_family].Value = oldFamilyName;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Family.fn_CustomerID)].Value = item.Customer;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Family.fn_Descr)].Value = item.Description;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Family.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Family.fn_family)].Value = item.FamilyName;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Family.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public DataTable GetQCRatioList(string customer)
        {
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT C.NewFamily,A.{9},A.{10},A.{11},A.{12},C.Model,A.{13},A.{14}, C.FamilyKey ,C.Flag, A.{16} FROM {2} AS A INNER JOIN " +
                                          "( " +
                                          " SELECT {4} AS FamilyKey,0 AS Flag, {4} AS NewFamily, '' AS Model FROM {1} WHERE {4}=@{4} " +
                                          " UNION " +
                                          " SELECT {7} AS FamilyKey,2 AS Flag, {8} AS NewFamily,{7} AS Model FROM {3} WHERE {8} IN (SELECT {6} FROM {0} WHERE {5}=@{4}) " +
                                          " UNION " +
                                          " SELECT {6} AS FamilyKey,1 AS Flag, {6} AS NewFamily, '' AS Model FROM {0} WHERE {5}=@{4} " +
                                          ") AS C ON A.{15}=C.FamilyKey ORDER BY NewFamily, Model ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Family).Name,
                                                                         string.Format("{0}..{1}", _Schema.SqlHelper.DB_GetData, typeof(_Schema.Customer).Name),
                                                                         string.Format("{0}..{1}", _Schema.SqlHelper.DB_FA, typeof(_Schema.QCRatio).Name),
                                                                         typeof(_Schema.Model).Name,
                                                                         _Schema.Customer.fn_customer,
                                                                         _Schema.Family.fn_CustomerID,
                                                                         _Schema.Family.fn_family,
                                                                         _Schema.Model.fn_model,
                                                                         _Schema.Model.fn_Family,
                                                                         _Schema.QCRatio.fn_qcRatio,
                                                                         _Schema.QCRatio.fn_EOQCRatio,
                                                                         _Schema.QCRatio.fn_PAQCRatio,
                                                                         _Schema.QCRatio.fn_Editor,
                                                                         _Schema.QCRatio.fn_Cdt,
                                                                         _Schema.QCRatio.fn_Udt,
                                                                         _Schema.QCRatio.fn_Family,
                                                                         _Schema.QCRatio.fn_RPAQCRatio);

                        sqlCtx.Params.Add(_Schema.Customer.fn_customer, new SqlParameter("@" + _Schema.Customer.fn_customer, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params[_Schema.Customer.fn_customer].Value = customer;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                #region OLD
                ////SELECT [IMES_FA].[dbo].[QCRatio].[Family]
                ////      ,[IMES_FA].[dbo].[QCRatio].[QCRatio]
                ////      ,[IMES_FA].[dbo].[QCRatio].[EOQCRatio]
                ////  FROM [IMES_FA].[dbo].[QCRatio] inner join 
                ////[AT_IMES_GetData].[dbo].[Family] on [IMES_FA].[dbo].[QCRatio].[Family]=[AT_IMES_GetData].[dbo].[Family].[Family]
                ////WHERE [AT_IMES_GetData].[dbo].[Family].[CustomerID]='customer' order by [Family]
                //_Schema.SQLContext sqlCtx = null;
                //_Schema.TableAndFields tf1 = null;
                //_Schema.TableAndFields tf2 = null;
                //_Schema.TableAndFields[] tblAndFldsesArray = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                //    {
                //        tf1 = new _Schema.TableAndFields();
                //        tf1.Table = typeof(_Schema.Family);
                //        _Schema.Family cond = new _Schema.Family();
                //        cond.CustomerID = customer;
                //        tf1.equalcond = cond;
                //        tf1.ToGetFieldNames = null;

                //        tf2 = new _Schema.TableAndFields();
                //        tf2.Table = typeof(_Schema.QCRatio);
                //        tf2.subDBCalalog = _Schema.SqlHelper.DB_FA;
                //        tf2.ToGetFieldNames.Add(_Schema.QCRatio.fn_Family);
                //        tf2.ToGetFieldNames.Add(_Schema.QCRatio.fn_qcRatio);
                //        tf2.ToGetFieldNames.Add(_Schema.QCRatio.fn_EOQCRatio);

                //        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                //        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Family.fn_family, tf2, _Schema.QCRatio.fn_Family);
                //        tblCnntIs.Add(tc1);

                //        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                //        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

                //        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                //        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Family.fn_family));
                //    }
                //}
                //tf1 = tblAndFldsesArray[0];
                //tf2 = tblAndFldsesArray[1];

                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Family.fn_CustomerID)].Value = customer;

                //ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                //ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.QCRatio.fn_Family)],
                //                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.QCRatio.fn_qcRatio)],
                //                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.QCRatio.fn_EOQCRatio)]});
                #endregion

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetQCRatioList2(string customer)
        {
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT C.NewFamily,A.{9},A.{10},A.{11},A.{12},C.Model,A.{13},A.{14}, C.FamilyKey ,C.Flag, A.{20} FROM {2} AS A INNER JOIN " +
                                          "( " +
                                          " SELECT {4} AS FamilyKey,0 AS Flag, {4} AS NewFamily, '' AS Model FROM {1} WHERE {4}=@{4} " +
                                          " UNION " +
                                          " SELECT {7} AS FamilyKey,3 AS Flag, {8} AS NewFamily,{7} AS Model FROM {3} WHERE {8} IN (SELECT {6} FROM {0} WHERE {5}=@{4}) " +
                                          " UNION " +
                                          " SELECT {6} AS FamilyKey,2 AS Flag, {6} AS NewFamily, '' AS Model FROM {0} WHERE {5}=@{4} " +
                                          " UNION " +
                                          " SELECT DISTINCT LEFT({17},1) AS FamilyKey,1 AS Flag, LEFT({17},1) AS NewFamily, '' AS Model FROM {16} WHERE {18}=@{4} AND ({19}='FA' OR {19}='PAK') " +
                                          ") AS C ON A.{15}=C.FamilyKey ORDER BY NewFamily, Model ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Family).Name,
                                                                         string.Format("{0}..{1}", _Schema.SqlHelper.DB_GetData, typeof(_Schema.Customer).Name),
                                                                         string.Format("{0}..{1}", _Schema.SqlHelper.DB_FA, typeof(_Schema.QCRatio).Name),
                                                                         typeof(_Schema.Model).Name,
                                                                         _Schema.Customer.fn_customer,
                                                                         _Schema.Family.fn_CustomerID,
                                                                         _Schema.Family.fn_family,
                                                                         _Schema.Model.fn_model,
                                                                         _Schema.Model.fn_Family,
                                                                         _Schema.QCRatio.fn_qcRatio,
                                                                         _Schema.QCRatio.fn_EOQCRatio,
                                                                         _Schema.QCRatio.fn_PAQCRatio,
                                                                         _Schema.QCRatio.fn_Editor,
                                                                         _Schema.QCRatio.fn_Cdt,
                                                                         _Schema.QCRatio.fn_Udt,
                                                                         _Schema.QCRatio.fn_Family,
                                                                         string.Format("{0}..{1}", _Schema.SqlHelper.DB_GetData, typeof(_Schema.Line).Name),
                                                                         _Schema.Line.fn_line,
                                                                         _Schema.Line.fn_CustomerID,
                                                                         _Schema.Line.fn_Stage,
                                                                         _Schema.QCRatio.fn_RPAQCRatio
                                                                         );

                        sqlCtx.Params.Add(_Schema.Customer.fn_customer, new SqlParameter("@" + _Schema.Customer.fn_customer, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params[_Schema.Customer.fn_customer].Value = customer;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetQCRatioList3(string customer)
        {
            //SELECT A.[Family],A.[QCRatio],A.[EOQCRatio],A.Editor,A.Cdt,A.Udt,A.RPAQCRatio FROM [QCRatio] AS A INNER JOIN
            //(
            //SELECT Family FROM [Family] WHERE [CustomerID]='customer' 
            //UNION 
            //SELECT Customer AS [Family] FROM Customer WHERE Customer='customer'
            //) AS C ON A.[Family]=C.[Family] ORDER BY [Family]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence =
                            "SELECT A.{6},A.{7},A.{8},A.{9},A.{10},A.{11},A.{12} FROM {2} AS A INNER JOIN " +
                            "( " +
                            "SELECT {5} FROM {0} WHERE {4}=@{3} " +
                            "UNION  " +
                            "SELECT {3} AS {5} FROM {1} WHERE {3}=@{3} " +
                            ") AS C ON A.{6}=C.{5} ORDER BY {6} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Family).Name,
                                                                         string.Format("{0}..{1}", _Schema.SqlHelper.DB_GetData, typeof(_Schema.Customer).Name),
                                                                         string.Format("{0}..{1}", _Schema.SqlHelper.DB_FA, typeof(_Schema.QCRatio).Name),
                                                                         _Schema.Customer.fn_customer,
                                                                         _Schema.Family.fn_CustomerID,
                                                                         _Schema.Family.fn_family,
                                                                         _Schema.QCRatio.fn_Family,
                                                                         _Schema.QCRatio.fn_qcRatio,
                                                                         _Schema.QCRatio.fn_EOQCRatio,
                                                                         _Schema.QCRatio.fn_Editor,
                                                                         _Schema.QCRatio.fn_Cdt,
                                                                         _Schema.QCRatio.fn_Udt,
                                                                         _Schema.QCRatio.fn_RPAQCRatio
                                                                         );

                        sqlCtx.Params.Add(_Schema.Customer.fn_customer, new SqlParameter("@" + _Schema.Customer.fn_customer, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.Customer.fn_customer].Value = customer;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddQCRatio(QCRatio item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.QCRatio));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCRatio));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.QCRatio.fn_PAQCRatio].Value = item.PAQCRatio;
                sqlCtx.Params[_Schema.QCRatio.fn_EOQCRatio].Value = item.EOQCRatio;
                sqlCtx.Params[_Schema.QCRatio.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.QCRatio.fn_qcRatio].Value = item.OQCRatio;
                sqlCtx.Params[_Schema.QCRatio.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.QCRatio.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.QCRatio.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.QCRatio.fn_RPAQCRatio].Value = item.RPAQCRatio;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                
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

        public void SaveQCRatio(QCRatio item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.QCRatio));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCRatio));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.QCRatio.fn_PAQCRatio].Value = item.PAQCRatio;
                sqlCtx.Params[_Schema.QCRatio.fn_EOQCRatio].Value = item.EOQCRatio;
                sqlCtx.Params[_Schema.QCRatio.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.QCRatio.fn_qcRatio].Value = item.OQCRatio;
                sqlCtx.Params[_Schema.QCRatio.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.QCRatio.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.QCRatio.fn_RPAQCRatio].Value = item.RPAQCRatio;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public void DeleteQCRatio(QCRatio item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.QCRatio));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCRatio));
                    }
                }
                sqlCtx.Params[_Schema.QCRatio.fn_Family].Value = item.Family;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public void UpdateQCRatio(QCRatio item, string oldId)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.QCRatio));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.QCRatio cond = new _Schema.QCRatio();
                        cond.Family = oldId;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCRatio), new List<string>() { _Schema.QCRatio.fn_Family, _Schema.QCRatio.fn_qcRatio, _Schema.QCRatio.fn_EOQCRatio, _Schema.QCRatio.fn_PAQCRatio, _Schema.QCRatio.fn_Editor, _Schema.QCRatio.fn_RPAQCRatio }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.QCRatio.fn_Family].Value = oldId;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.QCRatio.fn_PAQCRatio)].Value = item.PAQCRatio;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.QCRatio.fn_EOQCRatio)].Value = item.EOQCRatio;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.QCRatio.fn_Family)].Value = item.Family;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.QCRatio.fn_qcRatio)].Value = item.OQCRatio;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.QCRatio.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.QCRatio.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.QCRatio.fn_RPAQCRatio)].Value = item.RPAQCRatio;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public DataTable GetExistQCRatio(string customer, string QCRatioId)
        {
            //SELECT t.[Family]
            //FROM [IMES_FA]..[QCRatio] t INNER JOIN(
            //SELECT Family 
            //FROM [Family]
            //WHERE [CustomerID]='customer' 
            //UNION 
            //SELECT Customer AS [Family] 
            //FROM Customer
            //WHERE Customer='customer'
            //) AS C
            //ON t.[Family]=C.[Family]
            //WHERE t.[Family]='QCRatioId'
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence =   "SELECT t.{6} FROM {7}..{2} t INNER JOIN( " +
                                            "SELECT {4} FROM {0} WHERE {3}=@{3} " +
                                            "UNION " +
                                            "SELECT {5} AS {4} FROM {1} WHERE {5}=@{5} " +
                                            ") AS C ON t.{6}=C.{4} WHERE t.{6}=@{6} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Family).Name,
                                                                         typeof(_Schema.Customer).Name,
                                                                         typeof(_Schema.QCRatio).Name,
                                                                         _Schema.Family.fn_CustomerID,
                                                                         _Schema.Family.fn_family,
                                                                         _Schema.Customer.fn_customer,
                                                                         _Schema.QCRatio.fn_Family,
                                                                         _Schema.SqlHelper.DB_FA);

                        sqlCtx.Params.Add(_Schema.Family.fn_CustomerID, new SqlParameter("@" + _Schema.Family.fn_CustomerID, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.Customer.fn_customer, new SqlParameter("@" + _Schema.Customer.fn_customer, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.QCRatio.fn_Family, new SqlParameter("@" + _Schema.QCRatio.fn_Family, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params[_Schema.Family.fn_CustomerID].Value = customer;
                sqlCtx.Params[_Schema.Customer.fn_customer].Value = customer;
                sqlCtx.Params[_Schema.QCRatio.fn_Family].Value = QCRatioId;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                #region OLD
                ////SELECT [IMES_FA].[dbo].[QCRatio].[Family]
                ////  FROM [IMES_FA].[dbo].[QCRatio] inner join 
                ////[IMES_GetData].[dbo].[Family] on [IMES_FA].[dbo].[QCRatio].[Family]=[IMES_GetData].[dbo].[Family].[Family]
                ////WHERE [IMES_GetData].[dbo].[Family].[CustomerID]='customer' 
                ////AND [IMES_FA].[dbo].[QCRatio].[Family]='QCRatioId'
                //_Schema.SQLContext sqlCtx = null;
                //_Schema.TableAndFields tf1 = null;
                //_Schema.TableAndFields tf2 = null;
                //_Schema.TableAndFields[] tblAndFldsesArray = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                //    {
                //        tf1 = new _Schema.TableAndFields();
                //        tf1.Table = typeof(_Schema.Family);
                //        _Schema.Family cond = new _Schema.Family();
                //        cond.CustomerID = customer;
                //        cond.family = QCRatioId;
                //        tf1.equalcond = cond;
                //        tf1.ToGetFieldNames = null;

                //        tf2 = new _Schema.TableAndFields();
                //        tf2.Table = typeof(_Schema.QCRatio);
                //        tf2.subDBCalalog = _Schema.SqlHelper.DB_FA;
                //        tf2.ToGetFieldNames.Add(_Schema.QCRatio.fn_Family);

                //        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                //        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Family.fn_family, tf2, _Schema.QCRatio.fn_Family);
                //        tblCnntIs.Add(tc1);

                //        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                //        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

                //        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);
                //    }
                //}
                //tf1 = tblAndFldsesArray[0];
                //tf2 = tblAndFldsesArray[1];

                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Family.fn_CustomerID)].Value = customer;
                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Family.fn_family)].Value = QCRatioId;

                //ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                #endregion

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetMatchedCustomer(string param)
        {
            //SELECT [CustomerID]
            //  FROM dbo.Family
            //WHERE [Family]='param'
            //UNION 
            //SELECT Customer 
            //  FROM dbo.Customer
            //WHERE Customer='param'
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT {3} FROM {0} WHERE {4}=@{4} " +
                                          "UNION " +
                                          "SELECT {5} FROM {2}..{1} WHERE {5}=@{5} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence,typeof(_Schema.Family).Name,
                                                                        typeof(_Schema.Customer).Name,
                                                                        _Schema.SqlHelper.DB_GetData,
                                                                        _Schema.Family.fn_CustomerID,
                                                                        _Schema.Family.fn_family,
                                                                        _Schema.Customer.fn_customer);

                        sqlCtx.Params.Add(_Schema.Family.fn_family, new SqlParameter("@" + _Schema.Family.fn_family, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.Customer.fn_customer, new SqlParameter("@" + _Schema.Customer.fn_customer, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.Family.fn_family].Value = param;
                sqlCtx.Params[_Schema.Customer.fn_customer].Value = param;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string str = GetValue_Str(sqlR, 0);
                        ret.Add(str);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public Family GetFamilyByModel(string model)
        //{
        //    
        //}

        /// <summary>
        /// 取得存在的QCRatio
        /// SELECT [Family]
        /// FROM [QCRatio]
        /// WHERE [QCRatio].[Family]=@qcRatioFamily
        /// </summary>
        /// <param name="QCRatioFamily"></param>
        /// <returns></returns>
        public DataTable GetExistQCRatio(string qcRatioFamily)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.QCRatio cond = new _Schema.QCRatio();
                        cond.Family = qcRatioFamily;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCRatio), null, new List<string>() { _Schema.QCRatio.fn_Family }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.QCRatio.fn_Family].Value = qcRatioFamily;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                return ret;
            }
            catch (Exception)
            {
                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// 取得family下的Model
        /// SELECT [Model] FROM [Model] WHERE [Family]=@family ORDER BY [Model]
        /// </summary>
        /// <param name="Family"></param>
        /// <returns></returns>
        public DataTable GetModelListByFamily(string family)
        {
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.Family = family;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), null, new List<string>(){_Schema.Model.fn_model}, cond, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Model.fn_model);
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_Family].Value = family;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllFamily()
        {
            try
            {
                DataTable ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Family>(tk, null, new string[] { _Metas.Family.fn_family }, new ConditionCollection<_Metas.Family>(), _Metas.Family.fn_family);
                    }
                }
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<FamilyInfoDef> GetExistFamilyInfo(FamilyInfoDef condition)
        {
            try
            {
                IList<FamilyInfoDef> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::FamilyInfo cond = mtns::FuncNew.SetColumnFromField<mtns::FamilyInfo, FamilyInfoDef>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::FamilyInfo>(null, null, new mtns::ConditionCollection<mtns::FamilyInfo>(new mtns::EqualCondition<mtns::FamilyInfo>(cond)), mtns::FamilyInfo.fn_name);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::FamilyInfo, FamilyInfoDef>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::FamilyInfo, FamilyInfoDef, FamilyInfoDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddFamilyInfo(FamilyInfoDef item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::FamilyInfo>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::FamilyInfo, FamilyInfoDef>(sqlCtx, item);

                sqlCtx.Param(mtns::FamilyInfo.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::FamilyInfo.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveFamilyInfo(FamilyInfoDef condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.FamilyInfo cond = FuncNew.SetColumnFromField<mtns.FamilyInfo, FamilyInfoDef>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<mtns.FamilyInfo>(new ConditionCollection<mtns.FamilyInfo>(new EqualCondition<mtns.FamilyInfo>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.FamilyInfo, FamilyInfoDef>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateFamilyInfo(FamilyInfoDef setValue, FamilyInfoDef condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.FamilyInfo cond = FuncNew.SetColumnFromField<mtns.FamilyInfo, FamilyInfoDef>(condition);
                mtns.FamilyInfo setv = FuncNew.SetColumnFromField<mtns.FamilyInfo, FamilyInfoDef>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<mtns.FamilyInfo>(new SetValueCollection<mtns.FamilyInfo>(new CommonSetValue<mtns.FamilyInfo>(setv)), new ConditionCollection<mtns.FamilyInfo>(new EqualCondition<mtns.FamilyInfo>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.FamilyInfo, FamilyInfoDef>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<mtns.FamilyInfo, FamilyInfoDef>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns.FamilyInfo.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void ChangeFamilyDefered(IUnitOfWork uow, fons.Family Object, string oldFamilyName)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), Object, oldFamilyName);
        }

        public void AddQCRatioDefered(IUnitOfWork uow, QCRatio item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SaveQCRatioDefered(IUnitOfWork uow, QCRatio item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteQCRatioDefered(IUnitOfWork uow, QCRatio item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateQCRatioDefered(IUnitOfWork uow, QCRatio item, string oldId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldId);
        }

        public void AddFamilyInfoDefered(IUnitOfWork uow, FamilyInfoDef item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void RemoveFamilyInfoDefered(IUnitOfWork uow, FamilyInfoDef condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void UpdateFamilyInfoDefered(IUnitOfWork uow, FamilyInfoDef setValue, FamilyInfoDef condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        #endregion

        #endregion

        #region remove Cache
        /// <summary>
        /// for Remove Cache item 
        /// </summary>
        /// <param name="nameList"></param>
        public void RemoveCacheByKeyList(IList<string> nameList)
        {
            Monitor.Enter(ModelRepository._syncObj_cache);
            //lock (ModelRepository._syncObj_cache)
            try
            {
                foreach (string pk in nameList)
                {
                    if (_cache_fml.Contains(pk))
                    {
                        _cache_fml.Remove(pk);
                    }

                    if (ModelRepository._fml2mdlMapping.ContainsKey(pk))
                        ModelRepository._fml2mdlMapping.Remove(pk);
                }
            }
            finally
            {
                Monitor.Exit(ModelRepository._syncObj_cache);
            }
        }
        #endregion
    }
}
