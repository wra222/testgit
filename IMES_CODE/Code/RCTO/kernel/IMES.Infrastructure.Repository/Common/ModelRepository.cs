﻿// 2010-02-25 Liu Dong(eB1-4)         Modify ITC-1122-0138 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using dtmdl = IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
//using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure.Utility;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using IMES.Infrastructure.Utility.Cache;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Configuration;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Repository._Metas;
using mdlns = IMES.FisObject.Common.Model;
using mtns = IMES.Infrastructure.Repository._Metas;
using System.Threading;
using modelNS = IMES.DataModel;
using IMES.DataModel;
using prod = IMES.FisObject.FA.Product;
//using IMES.DataModel;
//

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Model相关
    /// </summary>
    public class ModelRepository : BaseRepository<mdlns::Model>, IModelRepository, IModelObjectRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();
        private static IList<string> needCheckInQtyFAIFAState = new List<string> { "Approval", "Pilot" };
        private static IList<string> needCheckInQtyFAIPAKState = new List<string> { "Hold" };

        #region Cache
        private static CacheManager _cache_real = null;
        private static CacheManager _cache
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = CacheFactory.GetCacheManager("ModelCache");
                return _cache_real;
            }
        }
        internal static IDictionary<string, IList<string>> _fml2mdlMapping = new Dictionary<string, IList<string>>();
        private static IDictionary<string, IList<string>> _byWhateverIndex = new Dictionary<string, IList<string>>();
        private static string preStr1 = _Schema.Func.MakeKeyForIdxPre(_Schema.Model.fn_Status);
        internal static object _syncObj_cache = new object();
        /// <summary>
        /// 记录方法是否被调用过一次
        /// </summary>
        private static IList<int> _calledMethods = new List<int>();
        private static string ToString(IDictionary<string, IList<string>> indexes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, IList<string>> item in indexes)
            {
                sb.AppendFormat("Index[Key:{0};Value:{1}];", item.Key, string.Join("|",item.Value.ToArray()));
            }
            return sb.ToString();
        }
        internal static int _lockWaitSeconds = 1;
        internal delegate void AsynCallFor(object parameters);       
        #endregion

        #region Link To Other
        private static IMBMORepository _mbmoRepository = null;
        private static IMBMORepository MbmoRepository
        {
            get
            {
                if (_mbmoRepository == null)
                    _mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                return _mbmoRepository;
            }
        }

        private static IPartRepository _partRepository = null;
        private static IPartRepository PartRepository
        {
            get
            {
                if (_partRepository == null)
                    _partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                return _partRepository;
            }
        }
        #endregion

        #region Overrides of BaseRepository<Model>

        protected override void PersistNewItem(mdlns::Model item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertModel(item);

                    this.CheckAndInsertSubs(item, tracker);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        protected override void PersistUpdatedItem(mdlns::Model item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateModel(item);

                    this.CheckAndInsertSubs(item, tracker);

                    this.CheckAndUpdateSubs(item, tracker);

                    this.CheckAndRemoveSubs(item, tracker);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        protected override void PersistDeletedItem(mdlns::Model item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.CheckAndRemoveSubs(item, tracker);

                    this.PersistDeleteModel(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        #endregion

        #region Implementation of BaseRepository<Model>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override mdlns::Model Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                mdlns::Model ret = Find_Cache(key);
                if (ret == null)
                {
                    ret = Find_DB(key);

                    if (IsCached() && ret != null)
                    {
                        #region old
                        //Monitor.Enter(_syncObj_cache);
                        ////lock (_syncObj_cache)
                        //try
                        //{
                        //    if (_fml2mdlMapping.ContainsKey(ret.FamilyName))
                        //        _fml2mdlMapping.Remove(ret.FamilyName);

                        //    //if (!_cache.Contains((string)ret.Key))
                        //    //{
                        //    //    AddToCache((string)ret.Key, ret);
                        //    //}
                        //    UnregistIndexesForOneModel(ret);
                        //    AddAndRegistOneModel(ret);
                        //}
                        //finally
                        //{
                        //    Monitor.Exit(_syncObj_cache);
                        //}
                        #endregion

                        ParameterizedThreadStart ts = new ParameterizedThreadStart(new AsynCallFor(AsynCallFor_Find));
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

        private void AsynCallFor_Find(object parameters)
        {
            mdlns::Model ret = (mdlns::Model)((object[])parameters)[0];

            Monitor.Enter(_syncObj_cache);
            //lock (_syncObj_cache)
            try
            {
                if (_fml2mdlMapping.ContainsKey(ret.FamilyName))
                    _fml2mdlMapping.Remove(ret.FamilyName);

                //if (!_cache.Contains((string)ret.Key))
                //{
                //    AddToCache((string)ret.Key, ret);
                //}
                UnregistIndexesForOneModel(ret);
                AddAndRegistOneModel(ret);
            }
            finally
            {
                Monitor.Exit(_syncObj_cache);
            }
        }

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public override IList<mdlns::Model> FindAll()
        {
            try
            {
                IList<mdlns::Model> ret = new List<mdlns::Model>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        mdlns::Model item = new mdlns::Model();
                        item.BOMApproveDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_BOMApproveDate]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Cdt]);
                        item.CustPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_CustPN]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Editor]);
                        item.FamilyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Family]);
                        item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
                        item.OSCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSCode]);
                        item.OSDesc = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSDesc]);
                        item.Price = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Price]);
                        item.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Region]);
                        item.ShipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_ShipType]);
                        item.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Status]).ToString();
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Udt]);
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
        public override void Add(mdlns::Model item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(mdlns::Model item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="work"></param>
        public override void Update(mdlns::Model item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IModelRepository

        public mdlns::Model FillModelAttributes(mdlns::Model item)
        {
            try
            {
                this.FillModelAttributes_DB(item);
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.ModelInfo> GetModelList(string familyId)
        {
            try
            {
                // 2010-02-25 Liu Dong(eB1-4)         Modify ITC-1122-0138 
                if (!IsCached())
                    return Converter(GetModelList_DB(familyId));

                bool isAccessDB = true;

                IList<mdlns::Model> ret = null;

                if (IsFirstCalled(MethodBase.GetCurrentMethod().MetadataToken))
                {
                    isAccessDB = false;
                    try
                    {
                        ret = GetModelList_Cache(familyId);
                        if (ret == null || ret.Count < 1)
                        {
                            isAccessDB = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == ItemDirty)
                            isAccessDB = true;
                        else
                            throw ex;
                    }
                }

                if (isAccessDB)
                {
                    ret = GetModelList_DB(familyId);

                    if (IsCached() && ret != null && ret.Count > 0)
                    {
                        #region old
                        //Monitor.Enter(_syncObj_cache);
                        ////lock (_syncObj_cache)
                        //try
                        //{
                        //    if (_fml2mdlMapping.ContainsKey(familyId))
                        //        _fml2mdlMapping.Remove(familyId);

                        //    foreach (mdlns::Model mdl in ret)
                        //    {
                        //        RegistFamilyMapping(mdl.ModelName, familyId);

                        //        //if (!_cache.Contains(mdl.ModelName))
                        //        //    AddToCache(mdl.ModelName, mdl);
                        //        UnregistIndexesForOneModel(mdl);
                        //        AddAndRegistOneModel(mdl);
                        //    }
                        //}
                        //finally
                        //{
                        //    Monitor.Exit(_syncObj_cache);
                        //}
                        #endregion

                        ParameterizedThreadStart ts = new ParameterizedThreadStart(new AsynCallFor(AsynCallFor_GetModelList));
                        Thread thrd = new Thread(ts);
                        thrd.Start(new object[] { ret, familyId });
                    }
                }
                // 2010-02-25 Liu Dong(eB1-4)         Modify ITC-1122-0138 
                return Converter(ret);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AsynCallFor_GetModelList(object parameters)
        {
            IList<mdlns::Model> ret = (IList<mdlns::Model>)((object[])parameters)[0];
            string familyId = (string)((object[])parameters)[1];

            Monitor.Enter(_syncObj_cache);
            //lock (_syncObj_cache)
            try
            {
                if (_fml2mdlMapping.ContainsKey(familyId))
                    _fml2mdlMapping.Remove(familyId);

                foreach (mdlns::Model mdl in ret)
                {
                    RegistFamilyMapping(mdl.ModelName, familyId);

                    //if (!_cache.Contains(mdl.ModelName))
                    //    AddToCache(mdl.ModelName, mdl);
                    UnregistIndexesForOneModel(mdl);
                    AddAndRegistOneModel(mdl);
                }
            }
            finally
            {
                Monitor.Exit(_syncObj_cache);
            }
        }

        //只列出可以产生ProdId的MO对应的model，即MO.Status='H' and MO.Qty>MO.Print_Qty and  MO.SAPStatus=''
        public IList<IMES.DataModel.ModelInfo> GetModelListFor014_RecentOneMonth(string familyId)
        {
            try
            {
                IList<string> res = MbmoRepository.GetModelsByFamilyWithMORecentOneMonth(familyId);
                if (res != null)
                {
                    IList<IMES.DataModel.ModelInfo> ret = new List<IMES.DataModel.ModelInfo>();
                    foreach(string item in res)
                    {
                        IMES.DataModel.ModelInfo mdli = new IMES.DataModel.ModelInfo();
                        mdli.id = mdli.friendlyName = item;
                        ret.Add(mdli);
                    }
                    return ret;
                }
                else
                    return new List<IMES.DataModel.ModelInfo>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //只列出可以产生ProdId的MO对应的model，即MO.Status='H' and MO.Qty>MO.Print_Qty and  MO.SAPStatus=''
        public IList<IMES.DataModel.ModelInfo> GetModelListFor014(string familyId)
        {
            try
            {
                IList<string> res = MbmoRepository.GetModelsByFamilyWithMO(familyId);
                if (res != null)
                {
                    IList<IMES.DataModel.ModelInfo> ret = new List<IMES.DataModel.ModelInfo>();
                    foreach (string item in res)
                    {
                        IMES.DataModel.ModelInfo mdli = new IMES.DataModel.ModelInfo();
                        mdli.id = mdli.friendlyName = item;
                        ret.Add(mdli);
                    }
                    return ret;
                }
                else
                    return new List<IMES.DataModel.ModelInfo>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //GetData..[Model] – Model 前4位为 ‘1397’， 按照Family 排序显示Family 
        public IList<IMES.DataModel.FamilyInfo> GetFamilyListFor008()
        {
            //LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                IList<IMES.DataModel.FamilyInfo> ret = new List<IMES.DataModel.FamilyInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model likecond = new _Schema.Model();
                        likecond.model = "1397%";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), "DISTINCT", new List<string>() { _Schema.Model.fn_Family }, null, likecond, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Model.fn_Family);
                        sqlCtx.Params[_Schema.Model.fn_model].Value = likecond.model;
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.FamilyInfo fmli = new IMES.DataModel.FamilyInfo();
                        fmli.friendlyName = fmli.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Family]);
                        ret.Add(fmli);
                    }
                }

                return ret;

                #region . OLD . Cached from PartRepository .
                //GetData..[Part] – PartType = ‘MA’， 按照Descr 排序显示Descr
                //IList<IMES.DataModel.FamilyInfo> ret = new List<IMES.DataModel.FamilyInfo>();

                //IList<IPart> parts = PartRepository.GetPartsByType("MA");
                //if (parts != null && parts.Count > 0)
                //{
                //    var result = (from prt in parts orderby prt.Descr select prt.Descr).Distinct().ToList();
                //    if (result != null && result.Count > 0)
                //        foreach (string str in result)
                //        {
                //            IMES.DataModel.FamilyInfo fi = new IMES.DataModel.FamilyInfo();
                //            fi.id = fi.friendlyName = str;
                //            ret.Add(fi);
                //        }
                //}
                //return ret;

                #endregion

                #region . OLD . Direct from DB .
                /*
                IList<IMES.DataModel.FamilyInfo> ret = new List<IMES.DataModel.FamilyInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                { 
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Part cond = new _Schema.Part();
                        cond.PartType = "MA";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), "DISTINCT", new List<string>() { _Schema.Part.fn_Descr }, cond, null, null,null,null,null,null,null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Part.fn_Descr);
                        sqlCtx.Params[_Schema.Part.fn_PartType].Value = cond.PartType;
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.FamilyInfo fmli = new IMES.DataModel.FamilyInfo();
                        fmli.friendlyName = fmli.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Descr]);
                        ret.Add(fmli);
                    }
                }
                return ret;
                  * */
                #endregion
            }
            catch (Exception)
            {
                //LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                //LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        //GetData..[Model] – Model 前4位为 ‘1397’， Family = @Family 为条件
        /// <summary>
        /// 取得1397阶信息列表
        /// </summary>
        /// <param name="familyId">Family标识</param>
        /// <returns>1397阶信息列表</returns>
        public IList<IMES.DataModel._1397LevelInfo> Get1397ListFor008(string familyId)
        {
            //LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                //LoggingInfoFormat("Get1397ListFor008->Family:{0}", familyId);

                IList<IMES.DataModel._1397LevelInfo> ret = new List<IMES.DataModel._1397LevelInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model likecond = new _Schema.Model();
                        likecond.model = "1397%";
                        _Schema.Model eqcond = new _Schema.Model();
                        eqcond.Family = familyId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), null, new List<string>() { _Schema.Model.fn_model }, eqcond, likecond, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Model.fn_model);
                        sqlCtx.Params[_Schema.Model.fn_model].Value = likecond.model;
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_Family].Value = familyId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel._1397LevelInfo fi = new IMES.DataModel._1397LevelInfo();
                        fi.friendlyName = fi.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
                        ret.Add(fi);
                    }
                }

                return ret;

                #region . OLD . Cached from PartRepository .
                //GetData..[Part] – PartType = ‘MA’， Descr = @Family 为条件 
                //IList<IMES.DataModel._1397LevelInfo> ret = new List<IMES.DataModel._1397LevelInfo>();

                //IList<IPart> parts = PartRepository.GetPartsByTypeAndDescr("MA", familyId);
                //if (parts != null && parts.Count > 0)
                //{
                //    var result = (from prt in parts select prt.PN).ToList();
                //    if (result != null && result.Count > 0)
                //        foreach (string str in result)
                //        {
                //            IMES.DataModel._1397LevelInfo fi = new IMES.DataModel._1397LevelInfo();
                //            fi.id = fi.friendlyName = str;
                //            ret.Add(fi);
                //        }
                //}
                //return ret;

                #endregion

                #region . OLD . Direct from DB .
                /*
                IList<IMES.DataModel._1397LevelInfo> ret = new List<IMES.DataModel._1397LevelInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                { 
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Part cond = new _Schema.Part();
                        cond.PartType = "MA";
                        cond.Descr = familyId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo }, cond, null, null, null, null, null, null, null);
                        sqlCtx.Params[_Schema.Part.fn_PartType].Value = cond.PartType;
                    }
                }
                sqlCtx.Params[_Schema.Part.fn_Descr].Value = familyId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel._1397LevelInfo _1397li = new IMES.DataModel._1397LevelInfo();
                        _1397li.friendlyName = _1397li.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_PartNo]);
                        ret.Add(_1397li);
                    }
                }
                return ret;
                 * */
                #endregion
            }
            catch (Exception)
            {
                //LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                //LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        public int GetSampleCount(string line, string model)
        {
            //select @cnt=count(*) from QCStatus 
            //where Tp='PIA' and PdLine=@pdline 
            //and Pno=model# and convert(char(10),Cdt,111)=convert(char(10),getdate(),111) 
            //@pdline：Product_Status.LineID 
            try
            {
                int ret = 0;

                DateTime now = _Schema.SqlHelper.GetDateTime();
                DateTime dt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.QCStatus);
                        tf1.ToGetFieldNames.Add(_Schema.QCStatus.fn_ID);
                        _Schema.QCStatus eqCond = new _Schema.QCStatus();
                        eqCond.Tp = "PIA";
                        eqCond.Model = model;
                        tf1.equalcond = eqCond;

                        _Schema.QCStatus geCond = new _Schema.QCStatus();
                        geCond.Cdt = dt;
                        tf1.greaterOrEqualcond = geCond;

                        _Schema.QCStatus sCond = new _Schema.QCStatus();
                        sCond.Cdt = dt.AddDays(1);
                        tf1.smallercond = sCond;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.ProductStatus);
                        _Schema.ProductStatus psEqCond = new _Schema.ProductStatus();
                        psEqCond.Line = line;
                        tf2.equalcond = psEqCond;
                        tf2.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.QCStatus.fn_ProductID, tf2, _Schema.ProductStatus.fn_ProductID);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "COUNT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.QCStatus.fn_Tp)].Value = eqCond.Tp;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.QCStatus.fn_Model)].Value = model;
                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.QCStatus.fn_Cdt))].Value = dt;
                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecS(_Schema.QCStatus.fn_Cdt))].Value = dt.AddDays(1);
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.ProductStatus.fn_Line)].Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
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

        public int GetSampleCount(string line, string model, string type)
        {
            //select @cnt=count(*) from QCStatus 
            //where Tp=@type and PdLine=@pdline 
            //and Pno=model# and convert(char(10),Cdt,111)=convert(char(10),getdate(),111) 
            //@pdline：Product_Status.LineID 
            try
            {
                int ret = 0;

                DateTime now = _Schema.SqlHelper.GetDateTime();
                DateTime dt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                _Schema.SQLContext sqlCtx = null;
                //_Schema.TableAndFields tf1 = null;
                //_Schema.TableAndFields tf2 = null;
                //_Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    //if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        //tf1 = new _Schema.TableAndFields();
                        //tf1.Table = typeof(_Schema.QCStatus);
                        //tf1.ToGetFieldNames.Add(_Schema.QCStatus.fn_ID);

                        _Schema.QCStatus eqCond = new _Schema.QCStatus();
                        eqCond.Tp = type;
                        eqCond.Model = model;
                        eqCond.Line = line;
                        //tf1.equalcond = eqCond;

                        _Schema.QCStatus geCond = new _Schema.QCStatus();
                        geCond.Cdt = dt;
                        //tf1.greaterOrEqualcond = geCond;

                        _Schema.QCStatus sCond = new _Schema.QCStatus();
                        sCond.Cdt = dt.AddDays(1);
                        //tf1.smallercond = sCond;

                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.QCStatus), "COUNT", new List<string>() { _Schema.QCStatus.fn_ID }, eqCond, null, null, null, sCond, geCond, null, null);

                        //tf2 = new _Schema.TableAndFields();
                        //tf2.Table = typeof(_Schema.ProductStatus);
                        //_Schema.ProductStatus psEqCond = new _Schema.ProductStatus();
                        //psEqCond.Line = line;
                        //tf2.equalcond = psEqCond;
                        //tf2.ToGetFieldNames = null;

                        //List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        //_Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.QCStatus.fn_ProductID, tf2, _Schema.ProductStatus.fn_ProductID);
                        //tblCnntIs.Add(tc1);

                        //_Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        //tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        //sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "COUNT", ref tblAndFldsesArray, tblCnnts);
                    }
                }
                //tf1 = tblAndFldsesArray[0];
                //tf2 = tblAndFldsesArray[1];

                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.QCStatus.fn_Tp)].Value = eqCond.Tp;
                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.QCStatus.fn_Model)].Value = model;
                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.QCStatus.fn_Cdt))].Value = dt;
                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecS(_Schema.QCStatus.fn_Cdt))].Value = dt.AddDays(1);
                //sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.ProductStatus.fn_Line)].Value = line;

                sqlCtx.Params[_Schema.QCStatus.fn_Tp].Value = type;
                sqlCtx.Params[_Schema.QCStatus.fn_Model].Value = model;
                sqlCtx.Params[_Schema.Func.DecGE(_Schema.QCStatus.fn_Cdt)].Value = dt;
                sqlCtx.Params[_Schema.Func.DecS(_Schema.QCStatus.fn_Cdt)].Value = dt.AddDays(1);
                sqlCtx.Params[_Schema.QCStatus.fn_Line].Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
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

        public IList<IMES.DataModel.ModelInfo> GetModelListFor102()
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            //SELECT Model FROM IMES_GetData..Model WHERE Status = '1' ORDER BY Model
            try
            {

                // 2010-02-25 Liu Dong(eB1-4)         Modify ITC-1122-0138 
                if (!IsCached())
                    return Converter(GetModelListByStatus_DB(1));

                bool isAccessDB = true;

                IList<mdlns::Model> ret = null;

                if (IsFirstCalled(MethodBase.GetCurrentMethod().MetadataToken))
                {
                    LoggingInfoFormat("GetModelListFor102->HERE #1");
                    isAccessDB = false;
                    try
                    {
                        ret = GetModelListByStatus_Cache(1);
                        if (ret == null || ret.Count < 1)
                        {
                            LoggingInfoFormat("GetModelListFor102->HERE #2");
                            isAccessDB = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggingInfoFormat("GetModelListFor102->HERE #3");
                        if (ex.Message == ItemDirty)
                            isAccessDB = true;
                        else
                            throw ex;
                    }
                }

                if (isAccessDB)
                {
                    LoggingInfoFormat("GetModelListFor102->HERE #4");

                    ret = GetModelListByStatus_DB(1);

                    if (IsCached() && ret != null && ret.Count > 0)
                    {
                        LoggingInfoFormat("GetModelListFor102->HERE #5");

                        #region old
                        //Monitor.Enter(_syncObj_cache);
                        ////lock (_syncObj_cache)
                        //try
                        //{
                        //    foreach (mdlns::Model mdl in ret)
                        //    {
                        //        RegistFamilyMapping(mdl.ModelName, mdl.FamilyName);

                        //        UnregistIndexesForOneModel(mdl);
                        //        AddAndRegistOneModel(mdl);
                        //    }
                        //}
                        //finally
                        //{
                        //    Monitor.Exit(_syncObj_cache);
                        //}
                        #endregion

                        ParameterizedThreadStart ts = new ParameterizedThreadStart(new AsynCallFor(AsynCallFor_GetModelListFor102));
                        Thread thrd = new Thread(ts);
                        thrd.Start(new object[] { ret });
                    }
                }
                // 2010-02-25 Liu Dong(eB1-4)         Modify ITC-1122-0138 
                return Converter(ret);
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

        private void AsynCallFor_GetModelListFor102(object parameters)
        {
            IList<mdlns::Model> ret = (IList<mdlns::Model>)((object[])parameters)[0];

            Monitor.Enter(_syncObj_cache);
            //lock (_syncObj_cache)
            try
            {
                foreach (mdlns::Model mdl in ret)
                {
                    RegistFamilyMapping(mdl.ModelName, mdl.FamilyName);

                    UnregistIndexesForOneModel(mdl);
                    AddAndRegistOneModel(mdl);
                }
            }
            finally
            {
                Monitor.Exit(_syncObj_cache);
            }
        }

        public IList<IMES.DataModel.ModelInfo> GetModelListFor094(string familyId)
        {
            //根据family得到model且Model对应的MO存在 SAPStatus<>’CLOSE’ and Status=’H’ and Qty<Print_Qty
            try
            {
                IList<IMES.DataModel.ModelInfo> ret = new List<IMES.DataModel.ModelInfo>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Model);
                        _Schema.Model eqCond1 = new _Schema.Model();
                        eqCond1.Family = familyId;
                        tf1.equalcond = eqCond1;
                        tf1.ToGetFieldNames.Add(_Schema.Model.fn_model);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.MO);
                        _Schema.MO eqCond2 = new _Schema.MO();
                        eqCond2.Status = "H";
                        tf2.equalcond = eqCond2;
                        _Schema.MO neqCond2 = new _Schema.MO();
                        neqCond2.SAPStatus = "CLOSE";
                        tf2.notEqualcond = neqCond2;
                        tf2.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Model.fn_model, tf2, _Schema.MO.fn_Model);
                        tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.MO.fn_Qty, tf2, _Schema.MO.fn_Print_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.MO.fn_Status)].Value = eqCond2.Status;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.MO.fn_SAPStatus)].Value = neqCond2.SAPStatus;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Model.fn_model));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Model.fn_Family)].Value = familyId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMES.DataModel.ModelInfo item = new IMES.DataModel.ModelInfo();
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Model.fn_model)]);
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

        //public IList<string> GetFamilyListByCTNO(string ctno)
        //{
        //    
        //}

        public string GetKittingCodeByModel(string model)
        {
            //SELECT RTRIM(a.Value) + '-' + CASE (CHARINDEX(' ', Family) - 1) WHEN -1 THEN Family
            //       ELSE SUBSTRING(Family, 1, (CHARINDEX(' ', Family) - 1)) END as [KittingCode]
            //       FROM IMES_GetData..ModelInfo a, IMES_GetData..Model b
            //       WHERE a.Model = b.Model
            //           AND a.Name = 'DM2'
            //           AND a.Model = @Model
            try
            {
                string ret = string.Empty;

                mdlns::Model mdl = this.Find(model);
                if (mdl != null)
                {
                    string val = mdl.GetAttribute("DM2");
                    string family = mdl.FamilyName;
                    if (val != null && family != null)
                    {
                        int idx = family.IndexOf(" ");
                        if (idx != -1)
                            ret = val.Trim() + "-" + family.Substring(0, idx);
                        else
                            ret = val.Trim() + "-" + family;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetMaxModelInfoValue(string modelName, string infoName)
        {
            try
            {
                string ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.ModelInfo cond = new _Metas.ModelInfo();
                        cond.model = modelName;
                        cond.name = infoName;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ModelInfo>(tk, "MAX", new string[] { _Metas.ModelInfo.fn_value }, new ConditionCollection<_Metas.ModelInfo>(new EqualCondition<_Metas.ModelInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.ModelInfo.fn_model).Value = modelName;
                sqlCtx.Param(_Metas.ModelInfo.fn_name).Value = infoName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes("MAX"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.Common.Model.ModelInfo> GetModelInfoByModelAndName(string model, string name)
        {
            try
            {
                IList<IMES.FisObject.Common.Model.ModelInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.ModelInfo cond = new mtns.ModelInfo();
                        cond.model = model;
                        cond.name = name;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.ModelInfo>(tk, null, null, new ConditionCollection<mtns.ModelInfo>(new EqualCondition<mtns.ModelInfo>(cond)), mtns.ModelInfo.fn_id);
                    }
                }
                sqlCtx.Param(mtns.ModelInfo.fn_model).Value = model;
                sqlCtx.Param(mtns.ModelInfo.fn_name).Value = name;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.ModelInfo, IMES.FisObject.Common.Model.ModelInfo, IMES.FisObject.Common.Model.ModelInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetCodeFromHPWeekCodeInRangeOfDescr()
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
                        Hpweekcode cond = new Hpweekcode();
                        cond.descr = "descr";
                        sqlCtx = FuncNew.GetConditionedSelect<Hpweekcode>(tk, "DISTINCT", new string[] { Hpweekcode.fn_code }, new ConditionCollection<Hpweekcode>(new AnySoloCondition<Hpweekcode>(cond, "CONVERT(CHAR(10),GETDATE(),111)>=SUBSTRING({0},1,10) AND CONVERT(CHAR(10),GETDATE(),111)<=SUBSTRING({0},12,10)")), Hpweekcode.fn_code);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(Hpweekcode.fn_code));
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

        public IList<IMES.DataModel.HpweekcodeInfo> GetHPWeekCodeInRangeOfDescr()
        {
            try
            {
                IList<IMES.DataModel.HpweekcodeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Hpweekcode cond = new Hpweekcode();
                        cond.descr = "descr";
                        sqlCtx = FuncNew.GetConditionedSelect<Hpweekcode>(tk, null, null, new ConditionCollection<Hpweekcode>(new AnySoloCondition<Hpweekcode>(cond, "CONVERT(CHAR(10),GETDATE(),111)>=SUBSTRING({0},1,10) AND CONVERT(CHAR(10),GETDATE(),111)<=SUBSTRING({0},12,10)")), Hpweekcode.fn_code);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Hpweekcode, dtmdl.HpweekcodeInfo, dtmdl.HpweekcodeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.ModelInfo> GetModelListByFamilyAndStatus(string family, int status)
        {
            try
            {
                IList<IMES.DataModel.ModelInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Model cond = new mtns.Model();
                        cond.family = family;
                        cond.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Model>(tk, null, new string[] { mtns.Model.fn_model }, new ConditionCollection<mtns.Model>(new EqualCondition<mtns.Model>(cond)), mtns.Model.fn_model);
                    }
                }
                sqlCtx.Param(mtns.Model.fn_family).Value = family;
                sqlCtx.Param(mtns.Model.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<IMES.DataModel.ModelInfo>();
                        while (sqlR.Read())
                        {
                            IMES.DataModel.ModelInfo item = new IMES.DataModel.ModelInfo();
                            item.id = item.friendlyName = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Model.fn_model));
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

        public bool CheckExistModelInfo(string name, string model, string valuePrefix)
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
                        _Metas.ModelInfo cond = new _Metas.ModelInfo();
                        cond.name = name;
                        cond.model = model;
                        _Metas.ModelInfo cond2 = new _Metas.ModelInfo();
                        cond2.value = valuePrefix;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ModelInfo>(tk, "COUNT", new string[] { _Metas.ModelInfo.fn_id }, new ConditionCollection<_Metas.ModelInfo>(
                            new EqualCondition<_Metas.ModelInfo>(cond),
                            new EqualCondition<_Metas.ModelInfo>(cond2, "RIGHT(RTRIM({0}),4)")));
                    }
                }
                sqlCtx.Param(_Metas.ModelInfo.fn_name).Value = name;
                sqlCtx.Param(_Metas.ModelInfo.fn_model).Value = model;
                sqlCtx.Param(_Metas.ModelInfo.fn_value).Value = valuePrefix;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetModelByNameAndValue(string name, string value)
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
                        _Metas.ModelInfo cond = new _Metas.ModelInfo();
                        cond.name = name;
                        cond.value = value;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ModelInfo>(tk, "DISTINCT", new string[] { _Metas.ModelInfo.fn_model }, new ConditionCollection<_Metas.ModelInfo>(
                            new EqualCondition<_Metas.ModelInfo>(cond)), _Metas.ModelInfo.fn_model);
                    }
                }
                sqlCtx.Param(_Metas.ModelInfo.fn_name).Value = name;
                sqlCtx.Param(_Metas.ModelInfo.fn_value).Value = value;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null) 
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.ModelInfo.fn_model));
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

        public DataTable GetCurrentStationList(string model)
        {
            DataTable ret = null;

            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    sqlCtx = new _Schema.SQLContext();
                    sqlCtx.Sentence = "SELECT RTRIM(c.{4}) + ' ' + RTRIM (c.{5}) AS Descr, c.{4}, COUNT(1) AS Qty " +
                                         "FROM {0} a, {1} b, {2} c " +
                                         "WHERE a.{6} = b.{9} " +
                                         "AND b.{11} = @{11} " +
                                         "AND b.{9} NOT IN ( " +
                                         "SELECT {12} FROM {3} " +
                                         "WHERE {13} = @{11} " +
                                         "AND {14} = '69' " +
                                         "AND {15} = 1) " +
                                         "AND b.{10} <>'' " +
                                         "AND a.{8} = 1 " +
                                         "AND a.{7} = c.{4} " +
                                         "GROUP BY c.{4}, c.{5} " +
                                         "ORDER BY c.{4} DESC";

                    sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ProductStatus).Name,
                                                                    typeof(_Schema.Product).Name,
                                                                    string.Format("{0}..{1}", _Schema.SqlHelper.DB_GetData, typeof(_Schema.Station).Name),
                                                                    typeof(_Schema.ProductLog).Name,
                                                                   _Schema.Station.fn_station,
                                                                   _Schema.Station.fn_Descr,
                                                                   _Schema.ProductStatus.fn_ProductID,
                                                                   _Schema.ProductStatus.fn_Station,
                                                                   _Schema.ProductStatus.fn_Status,
                                                                   _Schema.Product.fn_ProductID,
                                                                   _Schema.Product.fn_CUSTSN,
                                                                   _Schema.Product.fn_Model,
                                                                   _Schema.ProductLog.fn_ProductID,
                                                                   _Schema.ProductLog.fn_Model,
                                                                   _Schema.ProductLog.fn_Station,
                                                                   _Schema.ProductLog.fn_Status
                                                                    );

                    sqlCtx.Params.Add(_Schema.Product.fn_Model, new SqlParameter("@" + _Schema.Product.fn_Model, SqlDbType.VarChar));

                    _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                }
            }
            sqlCtx.Params[_Schema.Product.fn_Model].Value = model;

            ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

            return ret;
        }

        public IList<dtmdl.PaqcsortingInfo> GetPreviousFailTimeList(string line, string station)
        {
            try
            {
                IList<dtmdl.PaqcsortingInfo> ret = new List<dtmdl.PaqcsortingInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Paqcsorting cond = new mtns::Paqcsorting();
                        cond.line = line;
                        mtns::Paqcsorting cond2 = new mtns::Paqcsorting();
                        cond2.station = station;
                        cond2.status = "O";
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Paqcsorting>(tk, null, null, new mtns::ConditionCollection<mtns::Paqcsorting>(
                            new mtns::EqualCondition<mtns::Paqcsorting>(cond, "LEFT({0},1)", "LEFT({0},1)"),
                            new mtns::EqualCondition<mtns::Paqcsorting>(cond2)));
                        sqlCtx.Param(Paqcsorting.fn_status).Value = cond2.status;
                    }
                }
                sqlCtx.Param(Paqcsorting.fn_line).Value = line;
                sqlCtx.Param(Paqcsorting.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Paqcsorting, dtmdl.PaqcsortingInfo, dtmdl.PaqcsortingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<dtmdl.PaqcsortingInfo> GetPaqcsortingInfoList(string line, string station)
        {
            try
            {
                IList<dtmdl.PaqcsortingInfo> ret = new List<dtmdl.PaqcsortingInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Paqcsorting cond = new mtns::Paqcsorting();
                        cond.line = line;
                        mtns::Paqcsorting cond2 = new mtns::Paqcsorting();
                        cond2.station = station;
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Paqcsorting>(tk, null, null, new mtns::ConditionCollection<mtns::Paqcsorting>(
                            new mtns::EqualCondition<mtns::Paqcsorting>(cond, "LEFT({0},1)", "LEFT({0},1)"),
                            new mtns::EqualCondition<mtns::Paqcsorting>(cond2)));
                     }
                }
                sqlCtx.Param(Paqcsorting.fn_line).Value = line;
                sqlCtx.Param(Paqcsorting.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Paqcsorting, dtmdl.PaqcsortingInfo, dtmdl.PaqcsortingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<dtmdl.PaqcsortingInfo> GetPaqcsortingInfoList(dtmdl.PaqcsortingInfo condition)
        {
            try
            {
                IList<dtmdl.PaqcsortingInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Paqcsorting cond = FuncNew.SetColumnFromField<Paqcsorting, dtmdl.PaqcsortingInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<Paqcsorting>(null, null, new ConditionCollection<Paqcsorting>(new EqualCondition<Paqcsorting>(cond)), Paqcsorting.fn_id);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Paqcsorting, dtmdl.PaqcsortingInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Paqcsorting, dtmdl.PaqcsortingInfo, dtmdl.PaqcsortingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<dtmdl.PaqcsortingProductInfo> GetPaqcsortingProductInfoList(dtmdl.PaqcsortingProductInfo eqCondition, dtmdl.PaqcsortingProductInfo neqCondition)
        {
            try
            {
                IList<dtmdl.PaqcsortingProductInfo> ret = null;

                if (eqCondition == null)
                    eqCondition = new dtmdl.PaqcsortingProductInfo();
                if (neqCondition == null)
                    neqCondition = new dtmdl.PaqcsortingProductInfo();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Paqcsorting_Product cond = FuncNew.SetColumnFromField<Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(eqCondition);
                Paqcsorting_Product cond2 = FuncNew.SetColumnFromField<Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(neqCondition);

                sqlCtx = FuncNew.GetConditionedSelect<Paqcsorting_Product>(null, null, new ConditionCollection<Paqcsorting_Product>(new EqualCondition<Paqcsorting_Product>(cond), new NotEqualCondition<Paqcsorting_Product>(cond2, "ISNULL({0},'')")));
                var sqlCtx2 = FuncNew.GetConditionedSelect<Paqcsorting_Product>(null, new string[] { Paqcsorting_Product.fn_id }, new ConditionCollection<Paqcsorting_Product>(new NotEqualCondition<Paqcsorting_Product>(cond2)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(sqlCtx2, neqCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Paqcsorting_Product, dtmdl.PaqcsortingProductInfo, dtmdl.PaqcsortingProductInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPqacSortingProduct(IMES.DataModel.PaqcsortingProductInfo condition)
        {
            try
            {
                int ret = 0;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Paqcsorting_Product cond = mtns::FuncNew.SetColumnFromField<mtns::Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Paqcsorting_Product>("COUNT", new string[] { mtns::Paqcsorting_Product.fn_id }, new mtns::ConditionCollection<mtns::Paqcsorting_Product>(new mtns::EqualCondition<mtns::Paqcsorting_Product>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public int GetCountOfPqacSortingProduct(IMES.DataModel.PaqcsortingProductInfo condition, DateTime previousFailTime)
        {
            try
            {
                int ret = 0;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Paqcsorting_Product cond = mtns::FuncNew.SetColumnFromField<mtns::Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(condition);
                mtns::Paqcsorting_Product cond2 = new mtns::Paqcsorting_Product();
                cond2.cdt = previousFailTime;
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Paqcsorting_Product>("COUNT", new string[] { mtns::Paqcsorting_Product.fn_id }, new mtns::ConditionCollection<mtns::Paqcsorting_Product>(
                    new mtns::EqualCondition<mtns::Paqcsorting_Product>(cond),
                    new mtns::GreaterCondition<mtns::Paqcsorting_Product>(cond2)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(sqlCtx, condition);
                sqlCtx.Param(g.DecG(Paqcsorting_Product.fn_cdt)).Value = previousFailTime;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void InsertPqacSortingProductInfo(IMES.DataModel.PaqcsortingProductInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Paqcsorting_Product>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx = FuncNew.SetColumnFromField<Paqcsorting_Product, dtmdl.PaqcsortingProductInfo>(sqlCtx, item);
                sqlCtx.Param(Paqcsorting_Product.fn_cdt).Value = cmDt;
                //sqlCtx.Param(Paqcsorting_Product.fn_udt).Value = cmDt;
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePqacSortingInfo(IMES.DataModel.PaqcsortingInfo setValue, IMES.DataModel.PaqcsortingInfo condition)
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
                Paqcsorting cond = FuncNew.SetColumnFromField<Paqcsorting, dtmdl.PaqcsortingInfo>(condition);
                Paqcsorting setv = FuncNew.SetColumnFromField<Paqcsorting, dtmdl.PaqcsortingInfo>(setValue);
                setv.udt = DateTime.Now;
                sqlCtx = FuncNew.GetConditionedUpdate<Paqcsorting>(new SetValueCollection<Paqcsorting>(new CommonSetValue<Paqcsorting>(setv)), new ConditionCollection<Paqcsorting>(new EqualCondition<Paqcsorting>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Paqcsorting, dtmdl.PaqcsortingInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Paqcsorting, dtmdl.PaqcsortingInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Paqcsorting.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPqacSortingInfo(IMES.DataModel.PaqcsortingInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Paqcsorting>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx = FuncNew.SetColumnFromField<Paqcsorting, dtmdl.PaqcsortingInfo>(sqlCtx, item);
                sqlCtx.Param(Paqcsorting.fn_cdt).Value = cmDt;
                //sqlCtx.Param(Paqcsorting.fn_udt).Value = cmDt;
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DateTime GetMinCdtFromPaqcSortingProduct(int paqcSortingId)
        {
            try
            {
                DateTime ret = DateTime.MinValue;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Paqcsorting_Product cond = new _Metas.Paqcsorting_Product();
                        cond.paqcsortingid = paqcSortingId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Paqcsorting_Product>(tk, "MIN", new string[] { _Metas.Paqcsorting_Product.fn_cdt }, new ConditionCollection<_Metas.Paqcsorting_Product>(new EqualCondition<_Metas.Paqcsorting_Product>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Paqcsorting_Product.fn_paqcsortingid).Value = paqcSortingId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_DateTime(sqlR, sqlCtx.Indexes("MIN"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertIntoPaqCSortingProductFromProductAndProductLog(DateTime startTime, string line, string station, int paqcSortingId)
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
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "INSERT INTO {0} ({3}, {4}, {5}, {6}, {7}) " +
                                            "SELECT @{3}, a.{8}, b.{10}, b.{11}, b.{12} " +
                                            "FROM {1} a (NOLOCK), {2} b (NOLOCK) " +
                                            "WHERE a.{9} = b.{13} " +
                                            "AND b.{12} > @{12} " +
                                            "AND LEFT(b.{14}, 1) = LEFT(@{14}, 1) " +
                                            "AND b.{15} = @{15} " +
                                            "AND a.{8} NOT IN (SELECT {4} FROM {0} NOLOCK WHERE {3} = @{3}) ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Paqcsorting_Product)),
                                                                        ToolsNew.GetTableName(typeof(_Metas.Product)),
                                                                        ToolsNew.GetTableName(typeof(ProductLog)),
                                                                        Paqcsorting_Product.fn_paqcsortingid, 
                                                                        Paqcsorting_Product.fn_custsn,
                                                                        Paqcsorting_Product.fn_status, 
                                                                        Paqcsorting_Product.fn_editor,
                                                                        Paqcsorting_Product.fn_cdt,
                                                                        _Metas.Product.fn_custsn,
                                                                        _Metas.Product.fn_productID,
                                                                        ProductLog.fn_status,
                                                                        ProductLog.fn_editor,
                                                                        ProductLog.fn_cdt,
                                                                        ProductLog.fn_productID,
                                                                        ProductLog.fn_line,
                                                                        ProductLog.fn_station
                                                                        );
                        sqlCtx.AddParam(mtns.Paqcsorting_Product.fn_paqcsortingid, new SqlParameter("@" + mtns.Paqcsorting_Product.fn_paqcsortingid, ToolsNew.GetDBFieldType<mtns.Paqcsorting_Product>(mtns.Paqcsorting_Product.fn_paqcsortingid)));
                        sqlCtx.AddParam(mtns.ProductLog.fn_cdt, new SqlParameter("@" + mtns.ProductLog.fn_cdt, ToolsNew.GetDBFieldType<mtns.ProductLog>(mtns.ProductLog.fn_cdt)));
                        sqlCtx.AddParam(mtns.ProductLog.fn_line, new SqlParameter("@" + mtns.ProductLog.fn_line, ToolsNew.GetDBFieldType<mtns.ProductLog>(mtns.ProductLog.fn_line)));
                        sqlCtx.AddParam(mtns.ProductLog.fn_station, new SqlParameter("@" + mtns.ProductLog.fn_station, ToolsNew.GetDBFieldType<mtns.ProductLog>(mtns.ProductLog.fn_station)));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(_Metas.Paqcsorting_Product.fn_paqcsortingid).Value = paqcSortingId;
                sqlCtx.Param(_Metas.ProductLog.fn_cdt).Value = startTime;
                sqlCtx.Param(_Metas.ProductLog.fn_line).Value = line;
                sqlCtx.Param(_Metas.ProductLog.fn_station).Value = station;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertIntoPaqCSortingProductFromProductAndProductLog(DateTime startTime, string line, string station, int paqcSortingId, int n3)
        {
            try
            {
                string sN3 = "n3";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "INSERT INTO {0} ({3}, {4}, {5}, {6}, {7}) " +
                                            "SELECT TOP @{16} @{3}, a.{8}, b.{10}, b.{11}, b.{12} " +
                                            "FROM {1} a (NOLOCK), {2} b (NOLOCK) " +
                                            "WHERE a.{9} = b.{13} " +
                                            "AND b.{12} > @{12} " +
                                            "AND LEFT(b.{14}, 1) = LEFT(@{14}, 1) " +
                                            "AND b.{15} = @{15} " +
                                            "AND a.{8} NOT IN (SELECT {4} FROM {0} NOLOCK WHERE {3} = @{3}) ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Paqcsorting_Product)),
                                                                        ToolsNew.GetTableName(typeof(_Metas.Product)),
                                                                        ToolsNew.GetTableName(typeof(ProductLog)),
                                                                        Paqcsorting_Product.fn_paqcsortingid,
                                                                        Paqcsorting_Product.fn_custsn,
                                                                        Paqcsorting_Product.fn_status,
                                                                        Paqcsorting_Product.fn_editor,
                                                                        Paqcsorting_Product.fn_cdt,
                                                                        _Metas.Product.fn_custsn,
                                                                        _Metas.Product.fn_productID,
                                                                        ProductLog.fn_status,
                                                                        ProductLog.fn_editor,
                                                                        ProductLog.fn_cdt,
                                                                        ProductLog.fn_productID,
                                                                        ProductLog.fn_line,
                                                                        ProductLog.fn_station,
                                                                        sN3
                                                                        );
                        sqlCtx.AddParam(mtns.Paqcsorting_Product.fn_paqcsortingid, new SqlParameter("@" + mtns.Paqcsorting_Product.fn_paqcsortingid, ToolsNew.GetDBFieldType<mtns.Paqcsorting_Product>(mtns.Paqcsorting_Product.fn_paqcsortingid)));
                        sqlCtx.AddParam(mtns.ProductLog.fn_cdt, new SqlParameter("@" + mtns.ProductLog.fn_cdt, ToolsNew.GetDBFieldType<mtns.ProductLog>(mtns.ProductLog.fn_cdt)));
                        sqlCtx.AddParam(mtns.ProductLog.fn_line, new SqlParameter("@" + mtns.ProductLog.fn_line, ToolsNew.GetDBFieldType<mtns.ProductLog>(mtns.ProductLog.fn_line)));
                        sqlCtx.AddParam(mtns.ProductLog.fn_station, new SqlParameter("@" + mtns.ProductLog.fn_station, ToolsNew.GetDBFieldType<mtns.ProductLog>(mtns.ProductLog.fn_station)));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(_Metas.Paqcsorting_Product.fn_paqcsortingid).Value = paqcSortingId;
                sqlCtx.Param(_Metas.ProductLog.fn_cdt).Value = startTime;
                sqlCtx.Param(_Metas.ProductLog.fn_line).Value = line;
                sqlCtx.Param(_Metas.ProductLog.fn_station).Value = station;

                string Sentence = sqlCtx.Sentence.Replace(string.Format("TOP @{0}", sN3), "TOP " + n3.ToString());

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void InsertPqacSortingProductInfoDefered(IUnitOfWork uow, IMES.DataModel.PaqcsortingProductInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdatePqacSortingInfoDefered(IUnitOfWork uow, IMES.DataModel.PaqcsortingInfo setValue, IMES.DataModel.PaqcsortingInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void InsertPqacSortingInfoDefered(IUnitOfWork uow, IMES.DataModel.PaqcsortingInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertIntoPaqCSortingProductFromProductAndProductLogDefered(IUnitOfWork uow, DateTime startTime, string line, string station, int paqcSortingId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), startTime, line, station, paqcSortingId);
        }

        #endregion

        #endregion

        #region . Inners .

        private void RegistFamilyMapping(string pk, string key)
        {
            IList<string> PKs = null;
            try
            {
                PKs = _fml2mdlMapping[key];
            }
            catch (KeyNotFoundException)
            {
                PKs = new List<string>();
                _fml2mdlMapping.Add(key, PKs);
            }
            if (!PKs.Contains(pk))
                PKs.Add(pk);
        }

        private mdlns::Model Find_DB(object key)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                mdlns::Model ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.model = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_model].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    //LoggingInfoFormat("Model::Find_DB->SQL:{0}; Parameter:{1}", sqlCtx.Sentence, key);
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new mdlns::Model();
                        ret.BOMApproveDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_BOMApproveDate]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Cdt]);
                        ret.CustPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_CustPN]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Editor]);
                        ret.FamilyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Family]);
                        ret.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
                        ret.OSCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSCode]);
                        ret.OSDesc = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSDesc]);
                        ret.Price = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Price]);
                        ret.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Region]);
                        ret.ShipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_ShipType]);
                        ret.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Status]).ToString();
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Udt]);
                        ret.Tracker.Clear();

                        //LoggingInfoFormat("Model::Find_DB->Model Key:{0}; Model Hash:{1}", (string)ret.Key, ret.GetHashCode().ToString());
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

        private mdlns::Model Find_Cache(object key)
        {
            try
            {
                mdlns::Model ret = null;

                if (Monitor.TryEnter(_syncObj_cache, _lockWaitSeconds))
                {
                    //lock (_syncObj_cache)
                    try
                    {
                        if (_cache.Contains((string)key))
                            ret = (mdlns::Model)_cache[(string)key];
                    }
                    finally
                    {
                        Monitor.Exit(_syncObj_cache);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private mdlns::Model FillModelAttributes_DB(mdlns::Model item)
        {
            try
            {
                IList<IMES.FisObject.Common.Model.ModelInfo> newFieldVal = new List<IMES.FisObject.Common.Model.ModelInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ModelInfo cond = new _Schema.ModelInfo();
                        cond.Model = item.ModelName;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ModelInfo.fn_Model].Value = item.ModelName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.Model.ModelInfo mdlinfo = new IMES.FisObject.Common.Model.ModelInfo();
                        mdlinfo.ID = GetValue_Int64(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_ID]);
                        mdlinfo.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Model]);
                        mdlinfo.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Cdt]);
                        mdlinfo.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Descr]);
                        mdlinfo.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Editor]);
                        mdlinfo.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Name]);
                        mdlinfo.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Udt]);
                        mdlinfo.Value = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Value]);
                        mdlinfo.Tracker.Clear();
                        mdlinfo.Tracker = item.Tracker;
                        newFieldVal.Add(mdlinfo);
                    }
                }
                item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, newFieldVal);
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertModel(mdlns::Model item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Model.fn_BOMApproveDate].Value = (item.BOMApproveDate == DateTime.MinValue ? Convert.DBNull : item.BOMApproveDate);
                sqlCtx.Params[_Schema.Model.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Model.fn_CustPN].Value = item.CustPN;
                sqlCtx.Params[_Schema.Model.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Model.fn_Family].Value = item.FamilyName;
                sqlCtx.Params[_Schema.Model.fn_model].Value = item.ModelName;
                sqlCtx.Params[_Schema.Model.fn_OSCode].Value = item.OSCode;
                sqlCtx.Params[_Schema.Model.fn_OSDesc].Value = item.OSDesc;
                sqlCtx.Params[_Schema.Model.fn_Price].Value = item.Price;
                sqlCtx.Params[_Schema.Model.fn_Region].Value = item.Region;
                sqlCtx.Params[_Schema.Model.fn_ShipType].Value = item.ShipType;
                sqlCtx.Params[_Schema.Model.fn_Status].Value = int.Parse(item.Status);
                sqlCtx.Params[_Schema.Model.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateModel(mdlns::Model item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Model.fn_BOMApproveDate].Value = (item.BOMApproveDate == DateTime.MinValue ? Convert.DBNull : item.BOMApproveDate);
                sqlCtx.Params[_Schema.Model.fn_CustPN].Value = item.CustPN;
                sqlCtx.Params[_Schema.Model.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Model.fn_Family].Value = item.FamilyName;
                sqlCtx.Params[_Schema.Model.fn_model].Value = item.ModelName;
                sqlCtx.Params[_Schema.Model.fn_OSCode].Value = item.OSCode;
                sqlCtx.Params[_Schema.Model.fn_OSDesc].Value = item.OSDesc;
                sqlCtx.Params[_Schema.Model.fn_Price].Value = item.Price;
                sqlCtx.Params[_Schema.Model.fn_Region].Value = item.Region;
                sqlCtx.Params[_Schema.Model.fn_ShipType].Value = item.ShipType;
                sqlCtx.Params[_Schema.Model.fn_Status].Value = int.Parse(item.Status);
                sqlCtx.Params[_Schema.Model.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteModel(mdlns::Model item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model));
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_model].Value = item.ModelName;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<mdlns::Model> GetModelList_DB(string familyId)
        {
            try
            {
                IList<mdlns::Model> ret = new List<mdlns::Model>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.Family = familyId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_Family].Value = familyId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.Model.Model item = new IMES.FisObject.Common.Model.Model();
                        item.BOMApproveDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_BOMApproveDate]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Cdt]);
                        item.CustPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_CustPN]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Editor]);
                        item.FamilyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Family]);
                        item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
                        item.OSCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSCode]);
                        item.OSDesc = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSDesc]);
                        item.Price = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Price]);
                        item.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Region]);
                        item.ShipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_ShipType]);
                        item.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Status]).ToString();
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Udt]);
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

        private IList<mdlns::Model> GetModelList_Cache(string familyId)
        {
            IList<mdlns::Model> ret = null;

            if (Monitor.TryEnter(_syncObj_cache, _lockWaitSeconds))
            {
                //lock (_syncObj_cache)
                try
                {
                    if (_fml2mdlMapping.ContainsKey(familyId))
                    {
                        ret = new List<mdlns::Model>();
                        foreach (string mdlId in _fml2mdlMapping[familyId])
                        {
                            if (_cache.Contains(mdlId))
                                ret.Add((mdlns::Model)_cache[mdlId]);
                            else
                                throw new Exception(ItemDirty);//缺失Item,应该是 ICacheItemRefreshAction 拿掉的, 通知外面来重新拿DB.
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(_syncObj_cache);
                }
            }
            return ret;
        }

        private IList<IMES.DataModel.ModelInfo> Converter(IList<IMES.FisObject.Common.Model.Model> innerList)
        {
            IList<IMES.DataModel.ModelInfo> ret = new List<IMES.DataModel.ModelInfo>();
            if (innerList != null && innerList.Count > 0)
            {
                foreach (IMES.FisObject.Common.Model.Model ln in innerList)
                {
                    ret.Add(Converter(ln));
                }
            }
            return ret;
        }

        private IMES.DataModel.ModelInfo Converter(IMES.FisObject.Common.Model.Model ln)
        {
            IMES.DataModel.ModelInfo item = new IMES.DataModel.ModelInfo();
            item.friendlyName = ln.ModelName;
            item.id = ln.ModelName;
            return item;
        }

        private void CheckAndInsertSubs(IMES.FisObject.Common.Model.Model item, StateTracker tracker)
        {
            //persist ModelInfo
            IList<IMES.FisObject.Common.Model.ModelInfo> lstMdl = (IList<IMES.FisObject.Common.Model.ModelInfo>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMdl != null && lstMdl.Count > 0)//(item.Attributes.Count > 0)
            {
                foreach (IMES.FisObject.Common.Model.ModelInfo mi in lstMdl)//item.Attributes)
                {
                    if (tracker.GetState(mi) == DataRowState.Added)
                    {
                        mi.ModelName = item.ModelName;
                        this.PersistInsertModelInfo(mi);
                    }
                }
            }
        }

        private void CheckAndUpdateSubs(mdlns::Model item, StateTracker tracker)
        {
            IList<IMES.FisObject.Common.Model.ModelInfo> lstMdl = (IList<IMES.FisObject.Common.Model.ModelInfo>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMdl != null && lstMdl.Count > 0)//(item.Attributes.Count > 0)
            {
                foreach (IMES.FisObject.Common.Model.ModelInfo mi in lstMdl)//item.Attributes)
                {
                    if (tracker.GetState(mi) == DataRowState.Modified)
                    {
                        this.PersistUpdateModelInfo(mi);
                    }
                }
            }
        }

        private void CheckAndRemoveSubs(mdlns::Model item, StateTracker tracker)
        {
            IList<IMES.FisObject.Common.Model.ModelInfo> lstMdl = (IList<IMES.FisObject.Common.Model.ModelInfo>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMdl != null && lstMdl.Count > 0)//(item.Attributes.Count > 0)
            {
                IList<IMES.FisObject.Common.Model.ModelInfo> miToDel = new List<IMES.FisObject.Common.Model.ModelInfo>();
                foreach (IMES.FisObject.Common.Model.ModelInfo mi in item.Attributes)
                {
                    if (tracker.GetState(mi) == DataRowState.Deleted)
                    {
                        miToDel.Add(mi);
                    }
                }
                if (miToDel != null)
                {
                    foreach (IMES.FisObject.Common.Model.ModelInfo mitd in miToDel)
                    {
                        lstMdl.Remove(mitd);
                        this.PersistDeleteModelInfo(mitd);
                    }
                }
            }
        }

        private void PersistInsertModelInfo(IMES.FisObject.Common.Model.ModelInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelInfo.fn_Descr].Value = item.Description;
                sqlCtx.Params[_Schema.ModelInfo.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.ModelInfo.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.ModelInfo.fn_Model].Value = item.ModelName;
                sqlCtx.Params[_Schema.ModelInfo.fn_Name].Value = item.Name;
                sqlCtx.Params[_Schema.ModelInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelInfo.fn_Value].Value = item.Value;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateModelInfo(IMES.FisObject.Common.Model.ModelInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelInfo.fn_Descr].Value = item.Description;
                sqlCtx.Params[_Schema.ModelInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.ModelInfo.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.ModelInfo.fn_Model].Value = item.ModelName;
                sqlCtx.Params[_Schema.ModelInfo.fn_Name].Value = item.Name;
                sqlCtx.Params[_Schema.ModelInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelInfo.fn_Value].Value = item.Value;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteModelInfo(IMES.FisObject.Common.Model.ModelInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfo));
                    }
                }
                sqlCtx.Params[_Schema.ModelInfo.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<mdlns::Model> GetModelListByStatus_DB(int status)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                LoggingInfoFormat("GetModelListByStatus_DB->Status:{0}", status.ToString());

                IList<mdlns::Model> ret = new List<mdlns::Model>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.Status = status;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_Status].Value = status;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.Model.Model item = new IMES.FisObject.Common.Model.Model();
                        item.BOMApproveDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_BOMApproveDate]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Cdt]);
                        item.CustPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_CustPN]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Editor]);
                        item.FamilyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Family]);
                        item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
                        item.OSCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSCode]);
                        item.OSDesc = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSDesc]);
                        item.Price = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Price]);
                        item.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Region]);
                        item.ShipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_ShipType]);
                        item.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Status]).ToString();
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Udt]);
                        item.Tracker.Clear();
                        ret.Add(item);
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

        private IList<mdlns::Model> GetModelListByStatus_Cache(int status)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                LoggingInfoFormat("GetModelListByStatus_Cache->Status:{0}", status.ToString());

                IList<IMES.FisObject.Common.Model.Model> ret = null;
                string key = _Schema.Func.MakeKeyForIdx(preStr1, status.ToString());

                if (Monitor.TryEnter(_syncObj_cache, _lockWaitSeconds))
                {
                    //lock (_syncObj_cache)
                    try
                    {
                        if (_byWhateverIndex.ContainsKey(key))
                        {
                            ret = new List<IMES.FisObject.Common.Model.Model>();
                            foreach (string mdlId in _byWhateverIndex[key])
                            {
                                if (_cache.Contains(mdlId))
                                    ret.Add((mdlns::Model)_cache[mdlId]);
                                else
                                    throw new Exception(ItemDirty);//缺失Item,应该是 ICacheItemRefreshAction 拿掉的, 通知外面来重新拿DB.
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(_syncObj_cache);
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

        #endregion

        #region ICache Members

        private static bool _isCached = false;
        public bool IsCached()
        {
            lock (MethodBase.GetCurrentMethod())
            {
                bool configVal = DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Model);
                if (
                    (false == _isCached && true == configVal)
                    ||
                    (true == _isCached && false == configVal)
                    )
                {
                    ClearCalledMethods();
                }
                _isCached = configVal;
                return configVal;
            }
        }

        public void ProcessItem(IMES.DataModel.CacheUpdateInfo item)
        {
            if (item.Type == IMES.DataModel.CacheType.Model)
                LoadOneCache(item.Item);
        }

        public void ClearCache()
        {
            Monitor.Enter(_syncObj_cache);
            //lock (_syncObj_cache)
            try
            {
                _cache.Flush();
            }
            finally
            {
                Monitor.Exit(_syncObj_cache);
            }
        }

        private void LoadOneCache(string pk)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                LoggingInfoFormat("Model::LoadOneCache->PK:{0}", pk);

                Monitor.Enter(_syncObj_cache);
                //lock (_syncObj_cache)
                try
                {
                    if (_cache.Contains(pk))
                    {
                        IMES.FisObject.Common.Model.Model model = (IMES.FisObject.Common.Model.Model)_cache[pk];
                        if (model != null)
                        {
                            string family = model.FamilyName;
                            if (_fml2mdlMapping.ContainsKey(family))
                                _fml2mdlMapping.Remove(family);

                            UnregistIndexesForOneModel(model);
                        }
                        _cache.Remove(pk);
                    }


                    #region For YWH
                    /*
                    IMES.FisObject.Common.Model.Model mdl = this.Find_DB(pk);
                    if (mdl != null)
                    {
                        AddAndRegistOneModel(mdl);
                        //AddToCache(pk, mdl);

                        if (_fml2mdlMapping.ContainsKey(mdl.FamilyName))
                            _fml2mdlMapping.Remove(mdl.FamilyName);
                    }
                    */
                    #endregion
                }
                finally
                {
                    Monitor.Exit(_syncObj_cache);
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

        private IMES.DataModel.CacheUpdateInfo GetACacheSignal(string pk)
        {
            IMES.DataModel.CacheUpdateInfo ret = new IMES.DataModel.CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = IMES.DataModel.CacheType.Model;
            ret.Item = pk;
            return ret;
        }

        private static void AddToCache(string key, object obj)
        {
            //Vincent 2015-11-15 modify none callback and static absoluteTime
            //_cache.Add(key, obj, CacheItemPriority.Normal, new ModelRefreshAction(((mdlns::Model)obj).FamilyName), new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_ModelCache"].ToString()))));
            //_cache.Add(key, obj, CacheItemPriority.Normal, null, _cacheAbsoluteTime);
            _cache.Add(key, obj, CacheItemPriority.Normal, null, new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_ModelCache"].ToString()))));
        }

        private static void AddAndRegistOneModel(mdlns::Model mdl)
        {
            LoggingBegin(typeof(ModelRepository), MethodBase.GetCurrentMethod());
            try
            {
                LoggingInfoFormat("AddAndRegistOneMode->Key:{0}", (string)mdl.Key);

                if (!_cache.Contains((string)mdl.Key))
                    AddToCache((string)mdl.Key, mdl);

                LoggingInfoFormat("AddAndRegistOneModel%1->Indexes:{0}", ToString(_byWhateverIndex));

                //Regist
                Regist((string)mdl.Key, _Schema.Func.MakeKeyForIdx(preStr1, mdl.Status));

                LoggingInfoFormat("AddAndRegistOneModel%2->Indexes:{0}", ToString(_byWhateverIndex));
            }
            catch (Exception)
            {
                LoggingError(typeof(ModelRepository), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(typeof(ModelRepository), MethodBase.GetCurrentMethod());
            }
        }

        private static void UnregistIndexesForOneModel(mdlns::Model mdl)
        {
            LoggingBegin(typeof(ModelRepository), MethodBase.GetCurrentMethod());
            try
            {
                LoggingInfoFormat("UnregistIndexesForOneModel%1->Indexes:{0}", ToString(_byWhateverIndex));

                string idxStr1 = _Schema.Func.MakeKeyForIdx(preStr1, mdl.Status);
                if (_byWhateverIndex.ContainsKey(idxStr1))
                {
                    IList<string> stem = _byWhateverIndex[idxStr1];
                    if (stem != null)
                    {
                        if (stem.Contains((string)mdl.Key))
                            stem.Remove((string)mdl.Key);
                        if (stem.Count < 1)
                            _byWhateverIndex.Remove(idxStr1);
                    }
                }

                LoggingInfoFormat("UnregistIndexesForOneModel%2->Indexes:{0}", ToString(_byWhateverIndex));
            }
            catch (Exception)
            {
                LoggingError(typeof(ModelRepository), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(typeof(ModelRepository), MethodBase.GetCurrentMethod());
            }
        }

        private static void Regist(string pk, string key)
        {
            LoggingBegin(typeof(ModelRepository), MethodBase.GetCurrentMethod());
            try
            {
                LoggingInfoFormat("Model::Regist->PK:{0}, Key:{1}.", pk, key);

                IList<string> PKs = null;
                try
                {
                    PKs = _byWhateverIndex[key];
                }
                catch (KeyNotFoundException)
                {
                    PKs = new List<string>();
                    _byWhateverIndex.Add(key, PKs);
                }
                if (!PKs.Contains(pk))
                    PKs.Add(pk);
            }
            catch (Exception)
            {
                LoggingError(typeof(ModelRepository), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(typeof(ModelRepository), MethodBase.GetCurrentMethod());
            }
        }

        /// <summary>
        /// 方法是否已经被调用过了第一次
        /// </summary>
        /// <param name="methodId"></param>
        /// <returns></returns>
        private static bool IsFirstCalled(int methodId)
        {
            lock (_calledMethods)
            {
                if (_calledMethods.Contains(methodId))
                {
                    return true;
                }
                else
                {
                    _calledMethods.Add(methodId);
                    return false;
                }
            }
        }

        private static void ClearCalledMethods()
        {
            LoggingBegin(typeof(ModelRepository), MethodBase.GetCurrentMethod());
            lock (_calledMethods)
            {
                _calledMethods.Clear();
            }
            LoggingEnd(typeof(ModelRepository), MethodBase.GetCurrentMethod());
        }

        [Serializable]
        private class ModelRefreshAction : ICacheItemRefreshAction
        {
            string _family = null;
            public ModelRefreshAction(string family)
            {
                _family = family;
            }
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
                LoggingInfoFormat("ModelRefreshAction::Refresh->Key:{0}, Reason:{1}", key, removalReason.ToString());
                //lock (_syncObj_cache)
                //{
                //    if (_fml2mdlMapping.ContainsKey(_family)) //这里不取消注册索引,便于知道Cache自动踢元素,以重新Load DB.
                //        _fml2mdlMapping.Remove(_family);
                //}
            }
        }

        #endregion

        #region For Maintain

        public IList<mdlns::Model> GetModelObjList(string familyId)
        {
            try
            {
                IList<mdlns::Model> ret = new List<mdlns::Model>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.Family = familyId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_Family].Value = familyId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        mdlns::Model item = new mdlns::Model();
                        item.BOMApproveDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_BOMApproveDate]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Cdt]);
                        item.CustPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_CustPN]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Editor]);
                        item.FamilyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Family]);
                        item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
                        item.OSCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSCode]);
                        item.OSDesc = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSDesc]);
                        item.Price = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Price]);
                        item.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Region]);
                        item.ShipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_ShipType]);
                        item.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Status]).ToString();
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Udt]);
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

        public IMES.FisObject.Common.Model.ModelInfo GetModelInfoById(int modelInfoId)
        {
            try
            {
                IMES.FisObject.Common.Model.ModelInfo ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ModelInfo cond = new _Schema.ModelInfo();
                        cond.ID = modelInfoId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ModelInfo.fn_ID].Value = modelInfoId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new IMES.FisObject.Common.Model.ModelInfo();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Cdt]);
                        ret.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Descr]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Editor]);
                        ret.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Name]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Udt]);
                        ret.Value = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Value]);
                        ret.ID = GetValue_Int64(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_ID]);
                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Model]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistedModelInfo(string modelId, string attrName)
        {
            try
            {
                int ret = 0;
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ModelInfo cond = new _Schema.ModelInfo();
                        cond.Model = modelId;
                        cond.Name = attrName;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfo), "COUNT", new List<string>() { _Schema.ModelInfo.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ModelInfo.fn_Model].Value = modelId;
                sqlCtx.Params[_Schema.ModelInfo.fn_Name].Value = attrName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public void ChangeModel(mdlns::Model item, string oldModelName)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(oldModelName));
                if (oldModelName != item.ModelName)
                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.ModelName));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.model = oldModelName;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), null, new List<string>() { _Schema.Model.fn_BOMApproveDate }, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_model].Value = oldModelName;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_BOMApproveDate)].Value = (item.BOMApproveDate == DateTime.MinValue ? Convert.DBNull : item.BOMApproveDate);
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_CustPN)].Value = item.CustPN;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_Family)].Value = item.FamilyName;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_model)].Value = item.ModelName;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_OSCode)].Value = item.OSCode;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_OSDesc)].Value = item.OSDesc;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_Price)].Value = item.Price;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_Region)].Value = item.Region;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_ShipType)].Value = item.ShipType;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_Status)].Value = item.Status;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Model.fn_Udt)].Value = cmDt;
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

        public mdlns::Model FindFromDB(string ModelId)
        {
            return this.Find(ModelId);
        }

        public DataTable GetCustomerModelList(string customer)
        {
            //SELECT distinct Model.Model
            //FROM  Family INNER JOIN
            //Model ON Family.Family = Model.Family WHERE CustomerID ='customer' 
            //order by Model
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Model);
                        tf1.ToGetFieldNames.Add(_Schema.Model.fn_model);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Family);
                        _Schema.Family cond = new _Schema.Family();
                        cond.CustomerID = customer;
                        tf2.equalcond = cond;
                        tf2.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Model.fn_Family, tf2, _Schema.Family.fn_family);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Model.fn_model));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Family.fn_CustomerID)].Value = customer;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<string> GetAllModelInfoName()
        {
            //select distinct Name from ModelInfo order by Name
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfo), "DISTINCT", new List<string>() { _Schema.ModelInfo.fn_Name }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.ModelInfo.fn_Name); 
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfo.fn_Name]);
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

        public IList<mdlns::Model> GetModelListByModel(string family, string model)
        {
            try
            {
                IList<mdlns::Model> ret = new List<mdlns::Model>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.Family = family;
                        _Schema.Model likecond = new _Schema.Model();
                        likecond.model = model + "%";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), null, null, cond, likecond, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Model.fn_model); 
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_Family].Value = family;
                sqlCtx.Params[_Schema.Model.fn_model].Value = model + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        mdlns::Model item = new mdlns::Model();
                        item.BOMApproveDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_BOMApproveDate]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Cdt]);
                        item.CustPN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_CustPN]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Editor]);
                        item.FamilyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Family]);
                        item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
                        item.OSCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSCode]);
                        item.OSDesc = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_OSDesc]);
                        item.Price = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Price]);
                        item.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Region]);
                        item.ShipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_ShipType]);
                        item.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Status]).ToString();
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model.fn_Udt]);
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

        public IList<ShipType> GetShipTypeList()
        {
            try
            {
                IList<ShipType> ret = new List<ShipType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ShipType));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.ShipType.fn_shipType);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ShipType item = new ShipType();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Cdt]);
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Description]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Editor]);
                        item.shipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_shipType]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Udt]);
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

        public IList<ModelInfoName> GetModelInfoNameList()
        {
            try
            {
                IList<ModelInfoName> ret = new List<ModelInfoName>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfoName));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[]{_Schema.ModelInfoName.fn_Region, _Schema.ModelInfoName.fn_Name}));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ModelInfoName item = new ModelInfoName();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelInfoName.fn_Cdt]);
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfoName.fn_Description]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfoName.fn_Editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ModelInfoName.fn_ID]);
                        item.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfoName.fn_Name]);
                        item.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelInfoName.fn_Region]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelInfoName.fn_Udt]);
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

        public int CheckExistedModelInfoName(string region, string modelInfoName, string modelInfoNameID)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlCtxCllctn = new _Schema.SQLContextCollection();
                int i = 0;
                sqlCtxCllctn.AddOne(i++,ComposeForCheckExistedModelInfoName_Region_ModelInfoName(region, modelInfoName));
                if (!string.IsNullOrEmpty(modelInfoNameID))
                    sqlCtxCllctn.AddOne(i++, ComposeForCheckExistedModelInfoName_ID(Convert.ToInt32(modelInfoNameID)));

                sqlCtx = sqlCtxCllctn.MergeToOneAndQuery();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
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

        private _Schema.SQLContext ComposeForCheckExistedModelInfoName_Region_ModelInfoName(string region, string modelInfoName)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.ModelInfoName cond = new _Schema.ModelInfoName();
                    cond.Region = region;
                    cond.Name = modelInfoName;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfoName), "COUNT", new List<string>() { _Schema.ModelInfoName.fn_ID }, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.ModelInfoName.fn_Region].Value = region;
            sqlCtx.Params[_Schema.ModelInfoName.fn_Name].Value = modelInfoName;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedModelInfoName_ID(int ID)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.ModelInfoName ncond = new _Schema.ModelInfoName();
                    ncond.ID = ID;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfoName), "COUNT", new List<string>() { _Schema.ModelInfoName.fn_ID }, null, null, null, null, null, null, null, null, ncond, null, null);
                }
            }
            sqlCtx.Params[_Schema.ModelInfoName.fn_ID].Value = ID;
            return sqlCtx;
        }

        public void AddModelInfoName(ModelInfoName item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfoName));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelInfoName.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Name].Value = item.Name;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Region].Value = item.Region;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveModelInfoName(ModelInfoName item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfoName));
                    }
                }
                sqlCtx.Params[_Schema.ModelInfoName.fn_ID].Value = item.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ModelInfoName.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Name].Value = item.Name;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Region].Value = item.Region;
                sqlCtx.Params[_Schema.ModelInfoName.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteModelInfoName(ModelInfoName item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfoName));
                    }
                }
                sqlCtx.Params[_Schema.ModelInfoName.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.DataModel.ModelInfoNameAndModelInfoValue> GetModelInfoNameAndModelInfoValueListByModel(string Model)
        {
            //select B.Name as Name, B.Description as Description, isNull(A.Value, '') as Value,
            //        A.Editor as Editor, A.Cdt as Cdt, A.Udt as Udt, A.ID as ID
            //From (select Model, Name, Value, Editor, Cdt, Udt, ID from ModelInfo where Model = ?) as A 
            //Right Outer Join ModelInfoName as B
            //On A.Name = B.Name
            //按Region、Info Name列的字母序排序
            try
            {
                IList<IMES.DataModel.ModelInfoNameAndModelInfoValue> ret = new List<IMES.DataModel.ModelInfoNameAndModelInfoValue>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT B.{2} AS {2}, B.{3} AS {3}, ISNULL(A.{6},'') AS {6}," +
                                            "A.{7} AS {7}, A.{8} AS {8}, A.{9} AS {9}, A.{10} AS {10} " +
                                            "FROM (SELECT {4}, {5}, {6}, {7}, {8}, {9}, {10} FROM {1} WHERE {4}=@{4}) AS A " +
                                            "RIGHT OUTER JOIN {0} AS B " +
                                            "ON A.{5}=B.{2} ORDER BY {11},{12}";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelInfoName).Name,
                                                                         typeof(_Schema.ModelInfo).Name,
                                                                         _Schema.ModelInfoName.fn_Name,
                                                                         _Schema.ModelInfoName.fn_Description,
                                                                         _Schema.ModelInfo.fn_Model,
                                                                         _Schema.ModelInfo.fn_Name,
                                                                         _Schema.ModelInfo.fn_Value,
                                                                         _Schema.ModelInfo.fn_Editor,
                                                                         _Schema.ModelInfo.fn_Cdt,
                                                                         _Schema.ModelInfo.fn_Udt,
                                                                         _Schema.ModelInfo.fn_ID,
                                                                         _Schema.ModelInfoName.fn_Region,
                                                                         _Schema.ModelInfoName.fn_Name);

                        sqlCtx.Params.Add(_Schema.ModelInfo.fn_Model, new SqlParameter("@" + _Schema.ModelInfo.fn_Model, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.ModelInfo.fn_Model].Value = Model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMES.DataModel.ModelInfoNameAndModelInfoValue item = new IMES.DataModel.ModelInfoNameAndModelInfoValue();
                            item.Name = GetValue_Str(sqlR, 0);
                            item.Description = GetValue_Str(sqlR, 1);
                            item.Value = GetValue_Str(sqlR, 2);
                            item.Editor = GetValue_Str(sqlR, 3);
                            item.Cdt = GetValue_DateTime(sqlR, 4);
                            item.Udt = GetValue_DateTime(sqlR, 5);
                            item.ID = GetValue_Int64(sqlR, 6);
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

        public void AddModelInfo(IMES.FisObject.Common.Model.ModelInfo item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.ModelName));

                PersistInsertModelInfo(item);

                SqlTransactionManager.Commit();
            }
            catch(Exception)
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

        public void SaveModelInfo(IMES.FisObject.Common.Model.ModelInfo item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.ModelName));

                PersistUpdateModelInfo(item);

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

        public void DeleteModelInfo(IMES.FisObject.Common.Model.ModelInfo item)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.ModelName));

                PersistDeleteModelInfo(item);

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

        public void DeleteModelInfoNameByRegion(string region)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ModelInfoName cond = new _Schema.ModelInfoName();
                        cond.Region = region;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelInfoName), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ModelInfoName.fn_Region].Value = region;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistModel(string customer, string model)
        {
            //SELECT Model.Model FROM Family INNER JOIN Model ON Family.Family = Model.Family
            //WHERE Family.CustomerID = 'customer' AND  Model.Model='model'
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Model);
                        _Schema.Model cond1 = new _Schema.Model();
                        cond1.model = model;
                        tf1.equalcond = cond1;
                        tf1.ToGetFieldNames.Add(_Schema.Model.fn_model);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Family);
                        _Schema.Family cond2 = new _Schema.Family();
                        cond2.CustomerID = customer;
                        tf2.equalcond = cond2;
                        tf2.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Model.fn_Family, tf2, _Schema.Family.fn_family);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "COUNT", ref tblAndFldsesArray, tblCnnts);
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Model.fn_model)].Value = model;
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Family.fn_CustomerID)].Value = customer;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                            ret = cnt > 0 ? true : false;
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

        public IList<string> GetFamilyList()
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT DISTINCT CASE (CHARINDEX(' ', {1}) - 1) WHEN -1 THEN {1} ELSE SUBSTRING({1}, 1, (CHARINDEX(' ', {1}) - 1)) END AS {1} " + 
                                            "FROM {0} " +
                                            "WHERE LEFT({2}, 4) <> '1397' AND ISNULL({1}, '') <> '' " +
                                            "ORDER BY {1} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Model).Name,
                                                                        _Schema.Model.fn_Family,
                                                                        _Schema.Model.fn_model);

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = GetValue_Str(sqlR, 0);
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

        public IList<string> GetModelList()
        {
            // select Model from Model
            // 按Model 列的字符序排序
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), null, new List<string>() { _Schema.Model.fn_model }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Model.fn_model);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model.fn_model]);
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

        public IList<ShipType> GetAllShipType()
        {
            //select * from ShipType order by ShipType 
            try
            {
                IList<ShipType> ret = new List<ShipType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ShipType));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.ShipType.fn_shipType);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ShipType item = new ShipType();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Cdt]);
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Description]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Editor]);
                        item.shipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_shipType]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Udt]);
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

        public bool IfShipTypeIsEXists(string shipType)
        {
            // select * from ShipType where ShipType=?
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ShipType cond = new _Schema.ShipType();
                        cond.shipType = shipType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ShipType), "COUNT", new List<string>() { _Schema.ShipType.fn_shipType }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ShipType.fn_shipType].Value = shipType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
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

        public void UpdateShipType(ShipType shipType)
        {
            // Update ShipType Set Description=?,Editor=?,Udt=getdate() where ShipType=?
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ShipType));
                    }
                }
                sqlCtx.Params[_Schema.ShipType.fn_shipType].Value = shipType.shipType;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ShipType.fn_Description].Value = shipType.Description;
                sqlCtx.Params[_Schema.ShipType.fn_Editor].Value = shipType.Editor;
                sqlCtx.Params[_Schema.ShipType.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertShipType(ShipType shipType)
        {
            // insert ShipType(ShipType,Description,Editor,Cdt,Udt)values(?,?,?,getdate(),getdate())
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ShipType));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ShipType.fn_shipType].Value = shipType.shipType;
                sqlCtx.Params[_Schema.ShipType.fn_Description].Value = shipType.Description;
                sqlCtx.Params[_Schema.ShipType.fn_Editor].Value = shipType.Editor;
                sqlCtx.Params[_Schema.ShipType.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.ShipType.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IfShipTypeIsInUse(string shipType)
        {
            //select count(1) from Model where ShipType=?
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.ShipType = shipType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), "COUNT", new List<string>() { _Schema.Model.fn_model }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model.fn_ShipType].Value = shipType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
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

        public ShipType GetShipTypeByKey(string shipType)
        {
            try
            {
                ShipType ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ShipType cond = new _Schema.ShipType();
                        cond.shipType = shipType;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ShipType), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ShipType.fn_shipType].Value = shipType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new ShipType();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Cdt]);
                        ret.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Description]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Editor]);
                        ret.shipType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_shipType]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ShipType.fn_Udt]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteShipTypeByKey(string shipType)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ShipType));
                    }
                }
                sqlCtx.Params[_Schema.ShipType.fn_shipType].Value = shipType;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetModelListByFamly(string family, int status)
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
                        mtns.Model cond = new mtns.Model();
                        cond.family = family;
                        cond.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Model>(tk, null, new string[] { mtns.Model.fn_model }, new ConditionCollection<mtns.Model>(new EqualCondition<mtns.Model>(cond)), mtns.Model.fn_model);
                    }
                }
                sqlCtx.Param(mtns.Model.fn_family).Value = family;
                sqlCtx.Param(mtns.Model.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Model.fn_model));
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

        #region Defered

        public void ChangeModelDefered(IUnitOfWork uow, mdlns::Model item, string oldModelName)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldModelName);
        }

        public void AddModelInfoNameDefered(IUnitOfWork uow, ModelInfoName item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SaveModelInfoNameDefered(IUnitOfWork uow, ModelInfoName item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteModelInfoNameDefered(IUnitOfWork uow, ModelInfoName item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddModelInfoDefered(IUnitOfWork uow, IMES.FisObject.Common.Model.ModelInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SaveModelInfoDefered(IUnitOfWork uow, IMES.FisObject.Common.Model.ModelInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteModelInfoDefered(IUnitOfWork uow, IMES.FisObject.Common.Model.ModelInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteModelInfoNameByRegionDefered(IUnitOfWork uow, string region)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), region);
        }

        public void UpdateShipTypeDefered(IUnitOfWork uow, ShipType shipType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipType);
        }

        public void InsertShipTypeDefered(IUnitOfWork uow, ShipType shipType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipType);
        }

        public void DeleteShipTypeByKeyDefered(IUnitOfWork uow, string shipType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipType);
        }

        #endregion

        #endregion

        #region 未实现方法
        /// <summary>
        /// 检查model是否在Model表中。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModelExist(string model)
        {
            return false;
        }
        #endregion

        #region IModelRepository Members

        public int GetCountOfQcStatus(string tp, string pdline)
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
                        mtns.Qcstatus cond = new mtns.Qcstatus();
                        cond.tp = tp;

                        mtns::Qcstatus cond2 = new mtns::Qcstatus();
                        cond2.line = pdline;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Qcstatus>(tk, "COUNT", new string[] { _Metas.Qcstatus.fn_id }, new ConditionCollection<mtns::Qcstatus>(
                            new EqualCondition<mtns::Qcstatus>(cond),
                            //new EqualCondition<mtns::Qcstatus>(cond2, "LEFT({0},1)", "LEFT({0},1)")
                            new LikeCondition<mtns::Qcstatus>(cond2)
                            ));
                    }
                }
                sqlCtx.Param(mtns::Qcstatus.fn_tp).Value = tp;
                sqlCtx.Param(mtns::Qcstatus.fn_line).Value = pdline.Substring(0,1)+"%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public int GetCountOfQcStatus(string tp, string pdline, DateTime startTime, DateTime endTime)
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
                        mtns.Qcstatus cond = new mtns.Qcstatus();
                        cond.tp = tp;

                        mtns::Qcstatus cond2 = new mtns::Qcstatus();
                        cond2.line = pdline;

                        _Metas.Qcstatus cond3 = new _Metas.Qcstatus();
                        cond3.cdt = DateTime.Now;

                        _Metas.Qcstatus cond4 = new _Metas.Qcstatus();
                        cond4.cdt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Qcstatus>(tk, "COUNT", new string[] { _Metas.Qcstatus.fn_id }, new ConditionCollection<mtns::Qcstatus>(
                            new EqualCondition<mtns::Qcstatus>(cond),
                            //new EqualCondition<mtns::Qcstatus>(cond2, "LEFT({0},1)", "LEFT({0},1)"),
                            new LikeCondition<mtns::Qcstatus>(cond2),
                            new GreaterOrEqualCondition<mtns::Qcstatus>(cond3),
                            new SmallerCondition<mtns::Qcstatus>(cond4)));
                    }
                }
                sqlCtx.Param(mtns::Qcstatus.fn_tp).Value = tp;
                sqlCtx.Param(mtns::Qcstatus.fn_line).Value = pdline.Substring(0,1)+"%";
                sqlCtx.Param(g.DecGE(_Metas.Qcstatus.fn_cdt)).Value = startTime;
                sqlCtx.Param(g.DecS(_Metas.Qcstatus.fn_cdt)).Value = endTime;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        #region for ModelChangeQty table
        /// <summary>
        /// order by line, Cdt
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shipDate"></param>
        /// <returns></returns>
        public IList<modelNS.ModelChangeQtyDef> GetModelChangeQtyByModelShipDate(string model, DateTime shipDate)
        {
            try
            {
                IList<modelNS.ModelChangeQtyDef> ret = new List<modelNS.ModelChangeQtyDef>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select ID, Line, Model, Qty, ShipDate, AssignedQty, Status, Editor, Cdt, Udt
                                                            from ModelChangeQty
                                                            where Model=@Model and
                                                                       ShipDate = @ShipDate
                                                            order by Line, Cdt  ";

                        sqlCtx.Params.Add("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ShipDate", new SqlParameter("@ShipDate", SqlDbType.DateTime));

                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["Model"].Value = model;
                sqlCtx.Params["ShipDate"].Value = shipDate;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                           sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        modelNS.ModelChangeQtyDef item = _Schema.SQLData.ToObject<modelNS.ModelChangeQtyDef>(sqlR);
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
        ///  Query in transaction order by ShipDate, AssignedQty
        /// </summary>
        /// <param name="line"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public modelNS.ModelChangeQtyDef GetActiveModelChangeQty(string line, string model)
        {
            try
            {
                modelNS.ModelChangeQtyDef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select top 1 ID, Line, Model, Qty, ShipDate, AssignedQty, Status
                                                            from ModelChangeQty WITH (UPDLOCK,ROWLOCK)
                                                            where Line=@Line and
                                                                       Model = @Model and
                                                                        Status='A'
                                                            order by ShipDate,(Qty-AssignedQty) desc";

                        sqlCtx.Params.Add("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Model", new SqlParameter("@Model", SqlDbType.VarChar));


                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["Model"].Value = model;
                sqlCtx.Params["Line"].Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                              CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = _Schema.SQLData.ToObject<modelNS.ModelChangeQtyDef>(sqlR);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void AddModelChangeQty(modelNS.ModelChangeQtyDef modeChangeQty)
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
                        sqlCtx.Sentence = @"insert into ModelChangeQty(Line, Model, Qty, ShipDate, AssignedQty, 
                                                                                                          Status,Editor,Cdt,Udt)
                                                            values(@Line, @Model, @Qty, @ShipDate, @AssignedQty, 
                                                                            @Status,@Editor,@Now,@Now)";

                        sqlCtx.Params.Add("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        sqlCtx.Params.Add("ShipDate", new SqlParameter("@ShipDate", SqlDbType.DateTime));
                        sqlCtx.Params.Add("AssignedQty", new SqlParameter("@AssignedQty", SqlDbType.Int));
                        sqlCtx.Params.Add("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));




                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["Line"].Value = modeChangeQty.Line;
                sqlCtx.Params["Model"].Value = modeChangeQty.Model;
                sqlCtx.Params["Qty"].Value = modeChangeQty.Qty;
                sqlCtx.Params["ShipDate"].Value = modeChangeQty.ShipDate;
                sqlCtx.Params["AssignedQty"].Value = modeChangeQty.AssignedQty;
                sqlCtx.Params["Status"].Value = modeChangeQty.Status;
                sqlCtx.Params["Editor"].Value = modeChangeQty.Editor;
                sqlCtx.Params["Now"].Value = _Schema.SqlHelper.GetDateTime();


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

        public void DeleteModelChangeQty(int id)
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
                        sqlCtx.Sentence = @"delete from  ModelChangeQty
                                                            where ID=@ID";

                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["ID"].Value = id;

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

        public void AssignedModelChangeQty(int id)
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
                        sqlCtx.Sentence = @"update ModelChangeQty
                                                                set AssignedQty=AssignedQty+1,
                                                                     Status= case when (Qty-AssignedQty)>1 then  
                                                                                  'A'
                                                                                  else
                                                                                   'C'
                                                                                  end,
                                                                     Udt=getdate()     
                                                                 where ID=@ID ";

                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["ID"].Value = id;

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

        public void AssignedModelChangeQtyDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }
        public void RollbackAssignedModelChangeQty(int id)
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
                        sqlCtx.Sentence = @"update ModelChangeQty
                                                                set AssignedQty=AssignedQty-1,
                                                                    Status= case when (Qty-AssignedQty)>=0 then  
                                                                                  'A'
                                                                                  else
                                                                                   'C'
                                                                                  end,
                                                                    Udt=getdate()  
                                                                 where ID=@ID ";

                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["ID"].Value = id;

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

        #region FAIModel Info
        public IList<FAIModelInfo> GetFAIModel(FAIModelInfo condition)
        {
            try
            {
                IList<FAIModelInfo> ret = new List<FAIModelInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    FAIModel cond = FuncNew.SetColumnFromField<FAIModel, FAIModelInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<FAIModel>(null, null,
                                                                                                    new ConditionCollection<FAIModel>(new EqualCondition<FAIModel>(cond)),
                                                                                                    _Metas.FAIModel.fn_model);


                }
                sqlCtx = FuncNew.SetColumnFromField<FAIModel, FAIModelInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<FAIModel, FAIModelInfo, FAIModelInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateFAIModel(FAIModelInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    FAIModel cond = new FAIModel();
                    cond.model = item.Model;
                    FAIModel setv = FuncNew.SetColumnFromField<FAIModel, FAIModelInfo>(item, FAIModel.fn_model);
                    setv.udt = DateTime.Now;

                    sqlCtx = FuncNew.GetConditionedUpdate<FAIModel>(new SetValueCollection<FAIModel>(new CommonSetValue<FAIModel>(setv)),
                                                                                                                   new ConditionCollection<FAIModel>(new EqualCondition<FAIModel>(cond)));

                }

                sqlCtx.Param(FAIModel.fn_model).Value = item.Model;

                sqlCtx = FuncNew.SetColumnFromField<FAIModel, FAIModelInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(FAIModel.fn_udt)).Value = cmDt;

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
        public void InsertFAIModel(FAIModelInfo item)
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
                        sqlCtx = FuncNew.GetCommonInsert<FAIModel>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<FAIModel, FAIModelInfo>(sqlCtx, item);

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
        public void DeleteFAIModel(string model)
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
                        FAIModel cond = new FAIModel();
                        cond.model = model;

                        sqlCtx = FuncNew.GetConditionedDelete<FAIModel>(tk, new ConditionCollection<FAIModel>(new EqualCondition<_Metas.FAIModel>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.FAIModel.fn_model).Value = model;
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

        public void UpdateFAIModelDefered(IUnitOfWork uow, FAIModelInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }
        public void InsertFAIModelDefered(IUnitOfWork uow, FAIModelInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }
        public void DeleteFAIModelDefered(IUnitOfWork uow, string model)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model);
        }

        public bool IsFAIModel(string model)
        {
            try
            {
                int ret = 0;
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    FAIModel cond = new FAIModel();
                    cond.model = model;
                    sqlCtx = FuncNew.GetConditionedSelect<FAIModel>("COUNT", new string[] { FAIModel.fn_model },
                                                                                                    new ConditionCollection<FAIModel>(new EqualCondition<FAIModel>(cond)));


                }
                sqlCtx.Param(FAIModel.fn_model).Value = model;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, 0);
                    }
                }
                return ret == 0 ? false : true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FAIModelInfo GetFAIModelByModelWithTrans(string model)
        {
            try
            {
                FAIModelInfo ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    FAIModel cond = new FAIModel();
                    cond.model = model;
                    sqlCtx = FuncNew.GetConditionedSelect<FAIModel>(null, null,
                                                                                                    new ConditionCollection<FAIModel>(new EqualCondition<FAIModel>(cond)));

                    sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=FAIModel_PK,ROWLOCK,UPDLOCK) WHERE");
                }

                sqlCtx.Param(FAIModel.fn_model).Value = model;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                                 CommandType.Text,
                                                                                                                                 sqlCtx.Sentence,
                                                                                                                                 sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<FAIModel, FAIModelInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public FAIModelInfo GetFAIModelByModel(string model)
        {
            try
            {
                FAIModelInfo ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    FAIModel cond = new FAIModel();
                    cond.model = model;
                    sqlCtx = FuncNew.GetConditionedSelect<FAIModel>(null, null,
                                                                                                    new ConditionCollection<FAIModel>(new EqualCondition<FAIModel>(cond)));

                    //sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=FAIModel_PK,ROWLOCK,UPDLOCK) WHERE");
                }

                sqlCtx.Param(FAIModel.fn_model).Value = model;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                                 CommandType.Text,
                                                                                                                                 sqlCtx.Sentence,
                                                                                                                                 sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<FAIModel, FAIModelInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void CheckAndSetInFAQtyWithTrans(string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor)
        {

            FAIModelInfo faiModel = this.GetFAIModelByModelWithTrans(model);
            if (faiModel != null && needCheckInQtyFAIFAState.Contains(faiModel.FAState))
            {
                int remainingQty = faiModel.FAQty - faiModel.inFAQty;
                if ((remainingQty - inQty) < 0)
                {
                    throw new FisException("CQCHK50005", new List<string>{model, 
                                                                                                            "FA Travel Card Station", 
                                                                                                            remainingQty.ToString(), 
                                                                                                            inQty.ToString()});
                }

                faiModel.inFAQty = faiModel.inFAQty + inQty;
                if (faiModel.FAState == "Approval")
                {
                    faiModel.FAState = "Pilot";
                }
                faiModel.Editor = editor;
                this.UpdateFAIModel(faiModel);
                if (productList != null)
                {
                    if (!string.IsNullOrEmpty(InfoType) &&
                        InfoValue != null)
                    {
                        foreach (prod.IProduct item in productList)
                        {
                            //item.SetExtendedProperty("FAIinFA", "Y", editor);
                            item.SetExtendedProperty(InfoType, InfoValue, editor);
                        }
                    }
                }
            }
        }
        public void CheckAndSetInFAQtyWithTransDefered(IUnitOfWork uow, string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model, inQty, productList, InfoType, InfoValue, editor);
        }
        public void CheckAndSetInPAKQtyWithTrans(string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor)
        {
            FAIModelInfo faiModel = this.GetFAIModelByModelWithTrans(model);
            if (faiModel != null &&
                needCheckInQtyFAIPAKState.Contains(faiModel.PAKState))
            {
                if (faiModel.PAKStartDate > DateTime.Now)
                {
                    throw new FisException("CQCHK50006", new List<string>{model, 
                                                                                                            faiModel.PAKStartDate.ToString("yyyyMMdd HH:mm:ss")
                                                                                                            });
                }

                int remainingQty = faiModel.PAKQty - faiModel.inPAKQty;
                if ((remainingQty - inQty) < 0)
                {
                    throw new FisException("CQCHK50005", new List<string>{model, 
                                                                                                            "PAK In Station", 
                                                                                                            remainingQty.ToString(), 
                                                                                                            inQty.ToString()});
                }

                faiModel.inPAKQty = faiModel.inPAKQty + inQty;
                faiModel.Editor = editor;
                this.UpdateFAIModel(faiModel);
                if (productList != null)
                {
                    if (!string.IsNullOrEmpty(InfoType) &&
                         InfoValue != null)
                    {
                        foreach (prod.IProduct item in productList)
                        {
                            // item.SetExtendedProperty("FAIinPAK", "Y", editor);
                            item.SetExtendedProperty(InfoType, InfoValue, editor);
                        }
                    }
                }
            }
        }
        public void CheckAndSetInPAKQtyWithTransDefered(IUnitOfWork uow, string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model, inQty, productList, InfoType, InfoValue, editor);
        }

        public IList<FAIModelApprovalInfo> GetFAIModelApprovalInfo(string pakState, string model, string actionName)
        {
            try
            {
                IList<FAIModelApprovalInfo> ret = new List<FAIModelApprovalInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select d.Model,d.FAQty, d.InFAQty, d.PlanInputDate, d.FAState, 
                                                                d.PAKQty, d.InPAKQty, d.PAKState,d.PAKStartDate,d.Remark,
                                                                 d.Cdt as FAIModelCdt, d.Udt as FAIModelUdt,
                                                                a.ID as ApprovalID, a.Module, a.ActionName, a.Department, a.IsNeedApprove, 
                                                                a.IsNeedUploadFile, a.NoticeMsg, a.OwnerEmail,a.CCEmail,
                                                                b.ID as ApprovalStatusID, b.ModuleKeyValue, b.[Status] as ApprovalStatus,
                                                                b.Comment, c.ID as uploadFilesIID, c.UploadFileGUIDName, c.UploadFileName,
                                                                c.UploadServerName, c.Cdt as UploadDate         
                                                         from FAIModel d,
                                                              ApprovalItem a,
                                                              ApprovalStatus b,
                                                              UploadFiles c
                                                         where d.PAKState =@PAKState and
                                                                   d.Model =@Model and 
                                                                a.ID  = b.ApprovalItemID and
                                                               b.ID = c.ApprovalStatusID and
                                                               b.ModuleKeyValue =d.Model    and
                                                               a.ActionName =@ActionName";

                        sqlCtx.Params.Add("PAKState", new SqlParameter("@PAKState", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ActionName", new SqlParameter("@ActionName", SqlDbType.VarChar));


                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["PAKState"].Value = pakState;
                sqlCtx.Params["Model"].Value = model;
                sqlCtx.Params["ActionName"].Value = actionName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                              CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret.Add(_Schema.SQLData.ToObjectByField<FAIModelApprovalInfo>(sqlR));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IList<FAIModelApprovalInfo> GetFAIModelApprovalInfo(string pakState, string actionName)
        {
            try
            {
                IList<FAIModelApprovalInfo> ret = new List<FAIModelApprovalInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select d.Model,d.FAQty, d.InFAQty, d.PlanInputDate, d.FAState, 
                                                                d.PAKQty, d.InPAKQty, d.PAKState,d.PAKStartDate,d.Remark,
                                                                 d.Cdt as FAIModelCdt, d.Udt as FAIModelUdt,
                                                                a.ID as ApprovalID, a.Module, a.ActionName, a.Department, a.IsNeedApprove, 
                                                                a.IsNeedUploadFile, a.NoticeMsg, a.OwnerEmail,a.CCEmail,
                                                                b.ID as ApprovalStatusID, b.ModuleKeyValue, b.[Status] as ApprovalStatus,
                                                                b.Comment, c.ID as uploadFilesIID, c.UploadFileGUIDName, c.UploadFileName,
                                                                c.UploadServerName, c.Cdt as UploadDate         
                                                         from FAIModel d,
                                                              ApprovalItem a,
                                                              ApprovalStatus b,
                                                              UploadFiles c
                                                         where d.PAKState =@PAKState and                                                                   
                                                                a.ID  = b.ApprovalItemID and
                                                               b.ID = c.ApprovalStatusID and
                                                               b.ModuleKeyValue =d.Model    and
                                                               a.ActionName =@ActionName";

                        sqlCtx.Params.Add("PAKState", new SqlParameter("@PAKState", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ActionName", new SqlParameter("@ActionName", SqlDbType.VarChar));


                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["PAKState"].Value = pakState;
                // sqlCtx.Params["Model"].Value = model;
                sqlCtx.Params["ActionName"].Value = actionName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                              CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret.Add(_Schema.SQLData.ToObjectByField<FAIModelApprovalInfo>(sqlR));
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

        #region FAI Approval
        public IList<ApprovalItemInfo> GetApprovalItem(ApprovalItemInfo condition)
        {
            try
            {
                IList<ApprovalItemInfo> ret = new List<ApprovalItemInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    ApprovalItem cond = FuncNew.SetColumnFromField<ApprovalItem, ApprovalItemInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<ApprovalItem>(null, null,
                                                                                                    new ConditionCollection<ApprovalItem>(new EqualCondition<ApprovalItem>(cond)),
                                                                                                    _Metas.ApprovalItem.fn_id);


                }
                sqlCtx = FuncNew.SetColumnFromField<ApprovalItem, ApprovalItemInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<ApprovalItem, ApprovalItemInfo, ApprovalItemInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateApprovalItem(ApprovalItemInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    ApprovalItem cond = new ApprovalItem();
                    cond.id = item.ID;
                    ApprovalItem setv = FuncNew.SetColumnFromField<ApprovalItem, ApprovalItemInfo>(item, ApprovalItem.fn_id);
                    setv.udt = DateTime.Now;

                    sqlCtx = FuncNew.GetConditionedUpdate<ApprovalItem>(new SetValueCollection<ApprovalItem>(new CommonSetValue<ApprovalItem>(setv)),
                                                                                                                   new ConditionCollection<ApprovalItem>(new EqualCondition<ApprovalItem>(cond)));

                }

                sqlCtx.Param(ApprovalItem.fn_id).Value = item.ID;

                sqlCtx = FuncNew.SetColumnFromField<ApprovalItem, ApprovalItemInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(ApprovalItem.fn_udt)).Value = cmDt;

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
        public void InsertApprovalItem(ApprovalItemInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<ApprovalItem>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<ApprovalItem, ApprovalItemInfo>(sqlCtx, item);

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
        public void DeleteApprovalItem(long id)
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
                        ApprovalItem cond = new ApprovalItem();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<ApprovalItem>(tk, new ConditionCollection<ApprovalItem>(new EqualCondition<_Metas.ApprovalItem>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.ApprovalItem.fn_id).Value = id;
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

        public IList<ApprovalStatusInfo> GetApprovalStatus(ApprovalStatusInfo condition)
        {
            try
            {
                IList<ApprovalStatusInfo> ret = new List<ApprovalStatusInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    ApprovalStatus cond = FuncNew.SetColumnFromField<ApprovalStatus, ApprovalStatusInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<ApprovalStatus>(null, null,
                                                                                                    new ConditionCollection<ApprovalStatus>(new EqualCondition<ApprovalStatus>(cond)),
                                                                                                    _Metas.ApprovalStatus.fn_id);


                }
                sqlCtx = FuncNew.SetColumnFromField<ApprovalStatus, ApprovalStatusInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<ApprovalStatus, ApprovalStatusInfo, ApprovalStatusInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateApprovalStatus(ApprovalStatusInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    ApprovalStatus cond = new ApprovalStatus();
                    cond.id = item.ID;
                    ApprovalStatus setv = FuncNew.SetColumnFromField<ApprovalStatus, ApprovalStatusInfo>(item, ApprovalStatus.fn_id);
                    setv.udt = DateTime.Now;

                    sqlCtx = FuncNew.GetConditionedUpdate<ApprovalStatus>(new SetValueCollection<ApprovalStatus>(new CommonSetValue<ApprovalStatus>(setv)),
                                                                                                                   new ConditionCollection<ApprovalStatus>(new EqualCondition<ApprovalStatus>(cond)));

                }

                sqlCtx.Param(ApprovalStatus.fn_id).Value = item.ID;

                sqlCtx = FuncNew.SetColumnFromField<ApprovalStatus, ApprovalStatusInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(ApprovalStatus.fn_udt)).Value = cmDt;

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
        public void InsertApprovalStatus(ApprovalStatusInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<ApprovalStatus>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<ApprovalStatus, ApprovalStatusInfo>(sqlCtx, item);

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
        public void DeleteApprovalStatus(long id)
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
                        ApprovalStatus cond = new ApprovalStatus();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<ApprovalStatus>(tk, new ConditionCollection<ApprovalStatus>(new EqualCondition<_Metas.ApprovalStatus>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.ApprovalStatus.fn_id).Value = id;
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

        public IList<UploadFilesInfo> GetUploadFiles(UploadFilesInfo condition)
        {
            try
            {
                IList<UploadFilesInfo> ret = new List<UploadFilesInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    UploadFiles cond = FuncNew.SetColumnFromField<UploadFiles, UploadFilesInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<UploadFiles>(null, null,
                                                                                                    new ConditionCollection<UploadFiles>(new EqualCondition<UploadFiles>(cond)),
                                                                                                    _Metas.UploadFiles.fn_id);


                }
                sqlCtx = FuncNew.SetColumnFromField<UploadFiles, UploadFilesInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<UploadFiles, UploadFilesInfo, UploadFilesInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateUploadFiles(UploadFilesInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    UploadFiles cond = new UploadFiles();
                    cond.id = item.ID;
                    UploadFiles setv = FuncNew.SetColumnFromField<UploadFiles, UploadFilesInfo>(item, UploadFiles.fn_id);

                    sqlCtx = FuncNew.GetConditionedUpdate<UploadFiles>(new SetValueCollection<UploadFiles>(new CommonSetValue<UploadFiles>(setv)),
                                                                                                                   new ConditionCollection<UploadFiles>(new EqualCondition<UploadFiles>(cond)));

                }

                sqlCtx.Param(UploadFiles.fn_id).Value = item.ID;

                sqlCtx = FuncNew.SetColumnFromField<UploadFiles, UploadFilesInfo>(sqlCtx, item, true);

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
        public void InsertUploadFiles(UploadFilesInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<UploadFiles>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<UploadFiles, UploadFilesInfo>(sqlCtx, item);

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
        public void DeleteUploadFiles(long id)
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
                        UploadFiles cond = new UploadFiles();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<UploadFiles>(tk, new ConditionCollection<UploadFiles>(new EqualCondition<_Metas.UploadFiles>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.UploadFiles.fn_id).Value = id;
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

        public IList<ApprovalItemAttrInfo> GetApprovalItemAttr(ApprovalItemAttrInfo condition)
        {
            try
            {
                IList<ApprovalItemAttrInfo> ret = new List<ApprovalItemAttrInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    ApprovalItemAttr cond = FuncNew.SetColumnFromField<ApprovalItemAttr, ApprovalItemAttrInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<ApprovalItemAttr>(null, null,
                                                                                                    new ConditionCollection<ApprovalItemAttr>(new EqualCondition<ApprovalItemAttr>(cond)),
                                                                                                    _Metas.ApprovalItemAttr.fn_id);


                }
                sqlCtx = FuncNew.SetColumnFromField<ApprovalItemAttr, ApprovalItemAttrInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<ApprovalItemAttr, ApprovalItemAttrInfo, ApprovalItemAttrInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateApprovalItemAttr(ApprovalItemAttrInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    ApprovalItemAttr cond = new ApprovalItemAttr();
                    cond.id = item.ID;
                    ApprovalItemAttr setv = FuncNew.SetColumnFromField<ApprovalItemAttr, ApprovalItemAttrInfo>(item, ApprovalItemAttr.fn_id);

                    sqlCtx = FuncNew.GetConditionedUpdate<ApprovalItemAttr>(new SetValueCollection<ApprovalItemAttr>(new CommonSetValue<ApprovalItemAttr>(setv)),
                                                                                                                   new ConditionCollection<ApprovalItemAttr>(new EqualCondition<ApprovalItemAttr>(cond)));

                }

                sqlCtx.Param(ApprovalItemAttr.fn_id).Value = item.ID;

                sqlCtx = FuncNew.SetColumnFromField<ApprovalItemAttr, ApprovalItemAttrInfo>(sqlCtx, item, true);

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
        public void InsertApprovalItemAttr(ApprovalItemAttrInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<ApprovalItemAttr>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<ApprovalItemAttr, ApprovalItemAttrInfo>(sqlCtx, item);

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
        public void DeleteApprovalItemAttr(long id)
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
                        ApprovalItemAttr cond = new ApprovalItemAttr();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<ApprovalItemAttr>(tk, new ConditionCollection<ApprovalItemAttr>(new EqualCondition<_Metas.ApprovalItemAttr>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.ApprovalItemAttr.fn_id).Value = id;
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

        public void DeleteApprovalItemAttr(long approvalItemID, string attrName)
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
                        ApprovalItemAttr cond = new ApprovalItemAttr();
                        cond.approvalItemID = approvalItemID;
                        cond.attrName = attrName;

                        sqlCtx = FuncNew.GetConditionedDelete<ApprovalItemAttr>(tk, new ConditionCollection<ApprovalItemAttr>(new EqualCondition<_Metas.ApprovalItemAttr>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.ApprovalItemAttr.fn_approvalItemID).Value = approvalItemID;
                sqlCtx.Param(_Metas.ApprovalItemAttr.fn_attrName).Value = attrName;
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

        public IList<ApprovalContentInfo> GetApprovalContent(string moduleKeyValue, string actionName)
        {
            try
            {
                IList<ApprovalContentInfo> ret = new List<ApprovalContentInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select a.ID as ApprovalID, a.Module, a.ActionName, a.Department, a.IsNeedApprove, 
                                                                    a.IsNeedUploadFile, a.NoticeMsg, a.OwnerEmail,a.CCEmail,
                                                                    b.ID as ApprovalStatusID, b.ModuleKeyValue, b.[Status] as ApprovalStatus,
                                                                    b.Comment, c.ID as uploadFilesIID, c.UploadFileGUIDName, c.UploadFileName,
                                                                    c.UploadServerName, , c.Cdt as UploadDate         
                                                             from ApprovalItem a,
                                                                  ApprovalStatus b,
                                                                  UploadFiles c
                                                             where a.ID  = b.ApprovalItemID and
                                                                   b.ID = c.ApprovalStatusID and
                                                                   b.ModuleKeyValue =@ModuleKeyValue   and
                                                                   a.ActionName =@ActionName";

                        sqlCtx.Params.Add("ModuleKeyValue", new SqlParameter("@ModuleKeyValue", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ActionName", new SqlParameter("@ActionName", SqlDbType.VarChar));


                        _Schema.Func.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Params["ModuleKeyValue"].Value = moduleKeyValue;
                sqlCtx.Params["ActionName"].Value = actionName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                              CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret.Add(_Schema.SQLData.ToObjectByField<ApprovalContentInfo>(sqlR));
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

        #region Remove Model Cache
        /// <summary>
        /// for Remove Family Cache item 
        /// </summary>
        /// <param name="nameList"></param>
        public void RemoveCacheByKeyList(IList<string> nameList)
        {

            Monitor.Enter(_syncObj_cache);
            //lock (_syncObj_cache)
            try
            {
                foreach (string pk in nameList)
                {
                    if (_cache.Contains(pk))
                    {
                        IMES.FisObject.Common.Model.Model model = (IMES.FisObject.Common.Model.Model)_cache[pk];
                        if (model != null)
                        {
                            string family = model.FamilyName;
                            if (_fml2mdlMapping.ContainsKey(family))
                                _fml2mdlMapping.Remove(family);

                            UnregistIndexesForOneModel(model);
                        }
                        _cache.Remove(pk);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Monitor.Exit(_syncObj_cache);
            }

        }
        #endregion

        #region for maintain Model
        public void CopyModel(string scrModelName, string destModelName, int status, string editor)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"insert Model(Model, Family, CustPN, Region, ShipType, Status, 
                                                                             OSCode, OSDesc, Price, BOMApproveDate, Editor, 
                                                                             Cdt, Udt)
                                                                select @DestModel, Family, CustPN, Region, ShipType, @Status, 
                                                                       OSCode, OSDesc, Price, BOMApproveDate, @Editor, 
                                                                       GETDATE(), GETDATE() 
                                                                from Model
                                                                where Model =@Model

                                                                insert ModelInfo(Model, Name, Value, Descr, Editor, Cdt, Udt)
                                                                select @DestModel, Name, Value, Descr, @Editor, GETDATE(), GETDATE()
                                                                 from  ModelInfo
                                                                 where Model =@Model ";
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("DestModel", new SqlParameter("@DestModel", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.Int));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Model").Value = scrModelName;
                sqlCtx.Param("DestModel").Value = destModelName;
                sqlCtx.Param("Status").Value = status;
                sqlCtx.Param("Editor").Value = editor;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                            CommandType.Text,
                                                                            sqlCtx.Sentence,
                                                                            sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void CopyModelDefered(IUnitOfWork uow, string scrModelName, string destModelName, int status, string editor)
        {
            Action deferAction = () => { CopyModel(scrModelName, destModelName, status, editor); };
            AddOneInvokeBody(uow, deferAction);
        }
        #endregion
    }
}
