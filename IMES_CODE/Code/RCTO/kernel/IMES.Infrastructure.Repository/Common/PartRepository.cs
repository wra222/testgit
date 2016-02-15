﻿// 2010-01-24 Liu Dong(eB1-4)         Modify ITC-1103-0132
// 2010-03-04 Liu Dong(eB1-4)         Modify ITC-1122-0182 
// 2010-03-08 Liu Dong(eB1-4)         Modify ITC-1136-0071
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository;//._Schema;
//
using IMES.Infrastructure.Util;
using IMES.DataModel;
using IMES.Infrastructure.Utility;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using IMES.Infrastructure.Utility.Cache;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Configuration;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Line;
using System.Text.RegularExpressions;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Part相关
    /// </summary>
    public class PartRepository : BaseRepository<IPart>, IPartRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        //private static IDictionary<int, PartCheck> _cache_partCheck = new Dictionary<int, PartCheck>();
        //private static IDictionary<string, PartCheck> _cache_partCheck = new Dictionary<string, PartCheck>();
        //private static IDictionary<string, IMES.FisObject.Common.Part.PartType> _cache_partType = new Dictionary<string, IMES.FisObject.Common.Part.PartType>();
        private static CacheManager _cache_real = null;
        private static CacheManager _cache
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = CacheFactory.GetCacheManager("PartsCache");
                return _cache_real;
            }
        }
        private static IDictionary<string, IList<string>> _byWhateverIndex = new Dictionary<string, IList<string>>();
        //private static IDictionary<string, IList<string>> _byWhateverIndex_partType = new Dictionary<string, IList<string>>();
        //private static IDictionary<string, IList<int>> _byWhateverIndex_partCheck = new Dictionary<string, IList<int>>();
        //private static string preStr1 = _Schema.Func.MakeKeyForIdxPre(_Schema.Part.fn_PartType);
        ////private static string preStr2 = _Schema.Func.MakeKeyForIdxPre(_Schema.Part.fn_PartType, _Schema.Part.fn_Descr);
        //private static string preStr3 = _Schema.Func.MakeKeyForIdxPre(_Schema.PartType.fn_PartTypeGroup);
        //private static string preStr4 = _Schema.Func.MakeKeyForIdxPre(_Schema.PartCheck.fn_PartType,_Schema.PartCheck.fn_Customer);
        //private static string preStr5 = _Schema.Func.MakeKeyForIdxPre(_Schema.PartInfo.fn_InfoType, _Schema.PartInfo.fn_InfoValue);
        private static object _syncObj_cache = new object();
        //private static object _syncObj_cache_type = new object();
        //private static object _syncObj_cache_check = new object();
        /// <summary>
        /// 记录方法是否被调用过一次
        /// </summary>
        private static IList<int> _calledMethods = new List<int>();

        private static string ToString(IDictionary<string, IList<string>> indexes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, IList<string>> item in indexes)
            {
                sb.AppendFormat("Index[Key:{0};Value:{1}];", item.Key, string.Join("|", item.Value.ToArray()));
            }
            return sb.ToString();
        }     

        #endregion

        #region Link To Other
        //private static IModelRepository _mdlRepository = null;
        //private static IModelRepository MdlRepository
        //{
        //    get
        //    {
        //        if (_mdlRepository == null)
        //            _mdlRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        //        return _mdlRepository;
        //    }
        //}
        private static IAssemblyCodeRepository _assRepository = null;
        private static IAssemblyCodeRepository AssRepository
        {
            get
            {
                if (_assRepository == null)
                    _assRepository = RepositoryFactory.GetInstance().GetRepository<IAssemblyCodeRepository, fons::AssemblyCode>();
                return _assRepository;
            }
        }
        #endregion

        #region Overrides of BaseRepository<IPart>

        protected override void PersistNewItem(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            StateTracker tracker = (item as IMES.FisObject.Common.Part.Part).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    //PersistInsertPart(item);
                    PersistInsertOrRecoverPart(item);//Part表加Flag

                    this.CheckAndInsertSubs(item, tracker);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.Part));
                }
            }
            finally
            {
                tracker.Clear();
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        protected override void PersistUpdatedItem(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            StateTracker tracker = (item as IMES.FisObject.Common.Part.Part).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    PersistUpdatePart(item);

                    this.CheckAndInsertSubs(item, tracker);

                    this.CheckAndUpdateSubs(item, tracker);

                    this.CheckAndDeleteSubs(item, tracker);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.Part));
                }
            }
            finally
            {
                tracker.Clear();
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        protected override void PersistDeletedItem(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            StateTracker tracker = (item as IMES.FisObject.Common.Part.Part).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.CheckAndDeleteSubs(item, tracker);

                    PersistDeletePart(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.Part));
                }
            }
            finally
            {
                tracker.Clear();
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region Implementation of IRepository<IPart>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IPart Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                IPart ret = Find_Cache(key);
                if (ret == null)
                {
                    ret = Find_DB(key);

                    if (ret != null)
                    {
                        lock (_syncObj_cache)
                        {
                            //if (!_cache.Contains((string)ret.Key))
                            //    AddToCache((string)ret.Key, ret);
                            UnregistIndexesForOnePart(ret);
                            AddAndRegistOnePart(ret);
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
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public override IList<IPart> FindAll()
        {
            try
            {
                IList<IPart> ret = new List<IPart>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        //Part表加Flag
                         _Metas.Part_NEW cond = new _Metas.Part_NEW();
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Part_NEW>(tk, null, null, new ConditionCollection<_Metas.Part_NEW>(new EqualCondition<_Metas.Part_NEW>(cond)));
                        sqlCtx.Param(_Metas.Part_NEW.fn_flag).Value = cond.flag;//Part表加Flag
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IPart item = new IMES.FisObject.Common.Part.Part(
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_bomNodeType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_custPartNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_descr)),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_FruNo]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Vendor]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_IECVersion]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_remark)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_autoDL)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_editor)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_cdt)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_udt)),
                                                null);
                            ((IMES.FisObject.Common.Part.Part)item).Tracker.Clear();
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
        public override void Add(IPart item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(IPart item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="work"></param>
        public override void Update(IPart item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IPartRepository

        public IPart FillPartInfos(IPart part)
        {
            try
            {
                this.FillPartInfos_DB(part);
                return part;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons::AssemblyCode> FindAssemblyCode(string assCode)
        {
            try
            {
                return AssRepository.FindAssemblyCode(assCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IList<VGAInfo> GetVGAList()
        //{
        //    
        //}

        //public IList<FANInfo> GetFANList()
        //{
        //    
        //}

        public IList<PartTypeInfo> GetPartTypeList()
        {
            try
            {
                //if (!IsCached())
                    return Converter(GetPartTypeList_DB());

                //IList<IMES.FisObject.Common.Part.PartType> ret = GetPartTypeList_Cache();
                //if (ret == null || ret.Count < 1)
                //{
                //    ret = GetPartTypeList_DB();
                //}
                //return Converter(ret);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得PPID类型信息列表
        /// </summary>
        /// <returns>PPID类型信息列表</returns>
        public IList<PPIDTypeInfo> GetPPIDTypeList()
        {
            throw new NotImplementedException("先不实现");//????先不实现
        }

        /// <summary>
        /// 取得PPID描述信息列表
        /// </summary>
        /// <returns>PPID描述信息列表</returns>
        public IList<PPIDDescriptionInfo> GetPPIDDescriptionList(string PPIDTypeId)
        {
            throw new NotImplementedException("先不实现");//????先不实现
        }

        /// <summary>
        /// 取得PartNo信息列表
        /// </summary>
        /// <param name="PPIDTypeId">PPID类型标识</param>
        /// <param name="PPIDDescrptionId">PPID描述标识</param>
        /// <returns>PartNo信息列表</returns>
        public IList<PartNoInfo> GetPartNoList(string PPIDTypeId, string PPIDDescrptionId)
        {
            throw new NotImplementedException("先不实现");//????先不实现
        }

        /// <summary>
        /// 取得KP类型列表
        /// </summary>
        /// <returns>KP类型列表</returns>
        public IList<KPTypeInfo> GetKPTypeList()
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                string grp = "KP";

                //if (!IsCached())
                    return Converter(Converter(GetTypeList_DB(grp)));

                //IList<IMES.FisObject.Common.Part.PartType> ret = GetTypeList_Cache(grp);
                //if (ret == null || ret.Count < 1)
                //{
                //    ret = GetTypeList_DB(grp);
                //}
                //return Converter(Converter(ret));
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
        /// 获取指定PartType的Match,Check,Save设定
        /// </summary>
        /// <param name="partType">part类型</param>
        /// <returns>Match,Check,Save设定</returns>
        /// <remark>需要提供缓存，以PartType为Key</remark>
        public IList<PartCheck> GetPartCheck(string partType, string customer)
        {
            try
            {
                //if (!IsCached())
                    return GetPartCheck_DB(partType, customer);

                //IList<PartCheck> ret = GetPartCheck_Cache(partType, customer);
                //if (ret == null || ret.Count < 1)
                //{
                //    ret = GetPartCheck_DB(partType, customer);
                //}
                //return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PartCheck FillMatchRule(PartCheck partCheck)
        {
            try
            {
                this.FillMatchRule_DB(partCheck);
                return partCheck;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IList<IPart> GetPartsByType(string type)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetPartsByType->Type:{0}", type);

        //        // 2010-03-04 Liu Dong(eB1-4)         Modify ITC-1122-0182 
        //        if (!IsCached())
        //            return GetPartsByType_DB(type);

        //        bool isAccessDB = true;

        //        IList<IPart> ret = null;

        //        if (IsFirstCalled(MethodBase.GetCurrentMethod().MetadataToken))
        //        {
        //            LoggingInfoFormat("GetPartsByType->HERE #1");
        //            isAccessDB = false;
        //            try
        //            {
        //                ret = GetPartsByType_Cache(type);
        //                if (ret == null || ret.Count < 1)
        //                {
        //                    LoggingInfoFormat("GetPartsByType->HERE #2");
        //                    isAccessDB = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LoggingInfoFormat("GetPartsByType->HERE #3");
        //                if (ex.Message == ItemDirty)
        //                    isAccessDB = true;
        //                else
        //                    throw ex;
        //            }
        //        }

        //        if (isAccessDB)
        //        {
        //            LoggingInfoFormat("GetPartsByType->HERE #4");

        //            ret = GetPartsByType_DB(type);

        //            if (IsCached() && ret != null && ret.Count > 0)
        //            {
        //                LoggingInfoFormat("GetPartsByType->HERE #5");

        //                lock (_syncObj_cache)
        //                {
        //                    foreach (IPart entry in ret)
        //                    {
        //                        UnregistIndexesForOnePart(entry);
        //                        AddAndRegistOnePart(entry);
        //                    }
        //                }
        //            }
        //        }
        //        // 2010-03-04 Liu Dong(eB1-4)         Modify ITC-1122-0182 
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //public IList<IPart> GetPartsByTypeAndDescr(string type, string descr)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetPartsByTypeAndDescr->Type:{0}, Descr:{1}", type, descr);

        //        // 2010-03-04 Liu Dong(eB1-4)         Modify ITC-1122-0182 
        //        if (!IsCached())
        //            return GetPartsByTypeAndDescr_DB(type, descr);

        //        bool isAccessDB = true;

        //        IList<IPart> ret = null;

        //        if (IsFirstCalled(MethodBase.GetCurrentMethod().MetadataToken))
        //        {
        //            LoggingInfoFormat("GetPartsByType->HERE #1");
        //            isAccessDB = false;
        //            try
        //            {
        //                ret = GetPartsByTypeAndDescr_Cache(type, descr);
        //                if (ret == null || ret.Count < 1)
        //                {
        //                    LoggingInfoFormat("GetPartsByType->HERE #2");
        //                    isAccessDB = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LoggingInfoFormat("GetPartsByType->HERE #3");
        //                if (ex.Message == ItemDirty)
        //                    isAccessDB = true;
        //                else
        //                    throw ex;
        //            }
        //        }

        //        if (isAccessDB)
        //        {
        //            LoggingInfoFormat("GetPartsByType->HERE #4");

        //            ret = GetPartsByTypeAndDescr_DB(type, descr);

        //            if (IsCached() && ret != null && ret.Count > 0)
        //            {
        //                LoggingInfoFormat("GetPartsByType->HERE #5");

        //                lock (_syncObj_cache)
        //                {
        //                    foreach (IPart entry in ret)
        //                    {
        //                        UnregistIndexesForOnePart(entry);
        //                        AddAndRegistOnePart(entry);
        //                    }
        //                }
        //            }
        //        }
        //        // 2010-03-04 Liu Dong(eB1-4)         Modify ITC-1122-0182 
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        public IMES.FisObject.Common.Part.PartType GetPartType(string type)
        {
            try
            {
                //if (!IsCached())
                    return GetPartType_DB(type);

                //IMES.FisObject.Common.Part.PartType ret = GetPartType_Cache(type);
                //if (ret == null)
                //{
                //    ret = GetPartType_DB(type);
                //}
                //return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartForbidden> GetPartForbidden(string family, string model, string descr, string pn, string code)
        {
            try
            {
                IList<PartForbidden> ret = new List<PartForbidden>();
                _Schema.SQLContextCollection sqlSetTotalTotal = new _Schema.SQLContextCollection();
                _Schema.SQLContextCollection sqlSetTotal = new _Schema.SQLContextCollection();
                int i = 0;

                //Priority	Family	Model	Description	Part No	Assembly Code
                //1	　	            ■	　	　	                ■
                //2	　	            ■	　	            ■	　  等于空
                //3	　	            ■	    ■	　	　  等于空  等于空
                //4	        ■	　	等于空	                    ■
                //5	        ■	　	等于空              ■	　  等于空
                //6	        ■	　	等于空  ■	　	　  等于空  等于空

                //1
                if (!string.IsNullOrEmpty(code))
                {
                    int j = 0;
                    _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Model(model));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Code(code));
                    sqlSetTotal.AddOne(i++, sqlSet.MergeToOneAndQuery());
                }
                //2
                if (true)
                {
                    int j = 0;
                    _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Model(model));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Pn(pn));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Code_NullOrEmpty());
                    sqlSetTotal.AddOne(i++, sqlSet.MergeToOneAndQuery());
                }

                //3
                if (true)
                {
                    int j = 0;
                    _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Model(model));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Descr(descr));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Pn_NullOrEmpty());
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Code_NullOrEmpty());
                    sqlSetTotal.AddOne(i++, sqlSet.MergeToOneAndQuery());
                }

                //4
                if (!string.IsNullOrEmpty(code))
                {
                    int j = 0;
                    _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Family(family));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Model_NullOrEmpty());
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Code(code));
                    sqlSetTotal.AddOne(i++, sqlSet.MergeToOneAndQuery());
                }

                //5
                if (true)
                {
                    int j = 0;
                    _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Family(family));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Model_NullOrEmpty());
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Pn(pn));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Code_NullOrEmpty());
                    sqlSetTotal.AddOne(i++, sqlSet.MergeToOneAndQuery());
                }

                //6
                if (true)
                {
                    int j = 0;
                    _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Family(family));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Model_NullOrEmpty());
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Descr(descr));
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Pn_NullOrEmpty());
                    sqlSet.AddOne(j++, ComposeForGetPartForbidden_Code_NullOrEmpty());
                    sqlSetTotal.AddOne(i++, sqlSet.MergeToOneAndQuery());
                }
                sqlSetTotalTotal.AddOne(0, sqlSetTotal.MergeToOneOrQuery());
                sqlSetTotalTotal.AddOne(1, ComposeForGetPartForbidden_Status(1));

                _Schema.SQLContext sqlCtx = sqlSetTotalTotal.MergeToOneAndQuery();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PartForbidden item = new PartForbidden(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_ID]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Family]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Model]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Descr]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_PartNo]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_AssemblyCode]),
                            //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_PartType]),
                            //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_DateCode]),
                                                                GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Status]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Editor]),
                                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Cdt]),
                                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Udt])
                                                                );
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

        private _Schema.SQLContext ComposeForGetPartForbidden_Family(string family)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden cond = new _Schema.PartForbidden();
                    cond.Family = family;
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;
                }
            }
            sqlCtx.Params[_Schema.PartForbidden.fn_Family].Value = family;
            return sqlCtx;
        }

        //private _Schema.SQLContext ComposeForGetPartForbidden_Family_NullOrEmpty()
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartForbidden noeCond = new _Schema.PartForbidden();
        //            noeCond.Family = string.Empty;
        //            //_Schema.PartForbidden cond = new _Schema.PartForbidden();
        //            //cond.Status = 1;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), null, null, null, null, null, null, null, null, null, null, noeCond);
        //            //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;

        //            sqlCtx = RenameParam(sqlCtx, _Schema.PartForbidden.fn_Family);
        //        }
        //    }
        //    sqlCtx.Params["NOE_" + _Schema.PartForbidden.fn_Family].Value = string.Empty;
        //    return sqlCtx;
        //}

        private _Schema.SQLContext ComposeForGetPartForbidden_Model(string model)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden cond = new _Schema.PartForbidden();
                    cond.Model = model;
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;
                }
            }
            sqlCtx.Params[_Schema.PartForbidden.fn_Model].Value = model;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetPartForbidden_Model_NullOrEmpty()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden noeCond = new _Schema.PartForbidden();
                    noeCond.Model = string.Empty;
                    //_Schema.PartForbidden cond = new _Schema.PartForbidden();
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), null, null, null, null, null, null, null, null, null, null, noeCond);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;

                    sqlCtx = RenameParam(sqlCtx, _Schema.PartForbidden.fn_Model);
                }
            }
            sqlCtx.Params["NOE_" + _Schema.PartForbidden.fn_Model].Value = string.Empty;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetPartForbidden_Descr(string descr)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden likeCond = new _Schema.PartForbidden();
                    likeCond.Descr = descr;
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExtRvs(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), null, null, null, null, null, null, null, null, null, null, null, likeCond);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;

                    sqlCtx.Sentence = sqlCtx.Sentence.Replace(string.Format("LIKE {0}", _Schema.PartForbidden.fn_Descr), string.Format("LIKE {0} + '%'", _Schema.PartForbidden.fn_Descr));
                }
            }
            sqlCtx.Params[_Schema.PartForbidden.fn_Descr].Value = descr;
            return sqlCtx;
        }

        //private _Schema.SQLContext ComposeForGetPartForbidden_Descr_NullOrEmpty()
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartForbidden noeCond = new _Schema.PartForbidden();
        //            noeCond.Descr = string.Empty;
        //            //_Schema.PartForbidden cond = new _Schema.PartForbidden();
        //            //cond.Status = 1;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), null, null, null, null, null, null, null, null, null, null, noeCond);
        //            //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;

        //            sqlCtx = RenameParam(sqlCtx, _Schema.PartForbidden.fn_Descr);
        //        }
        //    }
        //    sqlCtx.Params["NOE_" + _Schema.PartForbidden.fn_Descr].Value = string.Empty;
        //    return sqlCtx;
        //}

        private _Schema.SQLContext ComposeForGetPartForbidden_Pn(string pn)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden cond = new _Schema.PartForbidden();
                    cond.PartNo = pn;
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;
                }
            }
            sqlCtx.Params[_Schema.PartForbidden.fn_PartNo].Value = pn;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetPartForbidden_Pn_NullOrEmpty()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden noeCond = new _Schema.PartForbidden();
                    noeCond.PartNo = string.Empty;
                    //_Schema.PartForbidden cond = new _Schema.PartForbidden();
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), null, null, null, null, null, null, null, null, null, null, noeCond);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;

                    sqlCtx = RenameParam(sqlCtx, _Schema.PartForbidden.fn_PartNo);
                }
            }
            sqlCtx.Params["NOE_" + _Schema.PartForbidden.fn_PartNo].Value = string.Empty;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetPartForbidden_Code(string code)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden cond = new _Schema.PartForbidden();
                    cond.AssemblyCode = code;
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;
                }
            }
            sqlCtx.Params[_Schema.PartForbidden.fn_AssemblyCode].Value = code;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetPartForbidden_Code_NullOrEmpty()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden noeCond = new _Schema.PartForbidden();
                    noeCond.AssemblyCode = string.Empty;
                    //_Schema.PartForbidden cond = new _Schema.PartForbidden();
                    //cond.Status = 1;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), null, null, null, null, null, null, null, null, null, null, noeCond);
                    //sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status;

                    sqlCtx = RenameParam(sqlCtx, _Schema.PartForbidden.fn_AssemblyCode);
                }
            }
            sqlCtx.Params["NOE_" + _Schema.PartForbidden.fn_AssemblyCode].Value = string.Empty;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetPartForbidden_Status(int status)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartForbidden cond = new _Schema.PartForbidden();
                    cond.Status = status;
                    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = status;
            return sqlCtx;
        }

        private _Schema.SQLContext RenameParam(_Schema.SQLContext sqlCtx, string fieldName)
        {
            sqlCtx.Sentence = sqlCtx.Sentence.Replace(sqlCtx.Params[fieldName].ParameterName, "@NOE_" + sqlCtx.Params[fieldName].ParameterName.Substring(1));
            sqlCtx.Params[fieldName].ParameterName = "@NOE_" + sqlCtx.Params[fieldName].ParameterName.Substring(1);
            sqlCtx.Params.Add("NOE_" + fieldName, sqlCtx.Params[fieldName]);
            sqlCtx.Params.Remove(fieldName);

            return sqlCtx;
        }

        //public IList<PartForbidden> GetPartForbidden(string partType, string pn)
        //{
        //    try
        //    {
        //        IList<PartForbidden> ret = new List<PartForbidden>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.PartForbidden cond = new _Schema.PartForbidden();
        //                cond.PartType = partType;
        //                cond.PartNo = pn;
        //                cond.Status = 1;
        //                sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);
        //                sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = cond.Status; 
        //            }
        //        }
        //        sqlCtx.Params[_Schema.PartForbidden.fn_PartType].Value = partType;
        //        sqlCtx.Params[_Schema.PartForbidden.fn_PartNo].Value = pn;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            while (sqlR != null && sqlR.Read())
        //            {
        //                PartForbidden item = new PartForbidden( GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_ID]),
        //                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Family]),
        //                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Model]),
        //                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Descr]),
        //                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_PartNo]),
        //                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_AssemblyCode]),
        //                                                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_PartType]),
        //                                                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_DateCode]),
        //                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Status]),
        //                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Editor]),
        //                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Cdt]),
        //                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Udt])
        //                                                        );
        //                item.Tracker.Clear();
        //                ret.Add(item);
        //            }
        //        }
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<IPart> GetPartByInfoTypeValue(string infoType, string infoValue)
        //{
        //    try
        //    {
        //        // 2010-03-04 Liu Dong(eB1-4)         Modify ITC-1122-0182 
        //        if (!IsCached())
        //            return GetPartByInfoTypeValue_DB(infoType, infoValue);

        //        bool isAccessDB = true;

        //        IList<IPart> ret = null;

        //        if (IsFirstCalled(MethodBase.GetCurrentMethod().MetadataToken))
        //        {
        //            isAccessDB = false;
        //            try
        //            {
        //                ret = GetPartByInfoTypeValue_Cache(infoType, infoValue);
        //                if (ret == null || ret.Count < 1)
        //                {
        //                    isAccessDB = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                if (ex.Message == ItemDirty)
        //                    isAccessDB = true;
        //                else
        //                    throw ex;
        //            }
        //        }

        //        if (isAccessDB)
        //        {
        //            ret = GetPartByInfoTypeValue_DB(infoType, infoValue);

        //            if (IsCached() && ret != null && ret.Count > 0)
        //            {
        //                lock (_syncObj_cache)
        //                {
        //                    foreach (IPart entry in ret)
        //                    {
        //                        UnregistIndexesForOnePart(entry);
        //                        AddAndRegistOnePart(entry);
        //                    }
        //                }
        //            }
        //        }
        //        // 2010-03-04 Liu Dong(eB1-4)         Modify ITC-1122-0182 
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<string> GetVendorListByCTNO(string ctno)
        //{
        //    //1.--get vendor
        //    //declare @CTNO varchar(20)
        //    //select Vendor from  IMES_GetData..Part where PartNo=(select IECPn from IMES_FA..PartSN where IECSN=@CTNO)
        //    
        //}

        public string GetAssemblyCodeInfo(string assemblyCode, string infoType)
        {
            try
            {
                return AssRepository.GetAssemblyCodeInfo(assemblyCode, infoType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetAssemblyCodesByModel(string model)
        {
            try
            {
                return AssRepository.GetAssemblyCodesByModel(model);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<string> GetAssemblyCodesByPartNo(string partNo)
        {
            try
            {
                return AssRepository.GetAssemblyCodesByPartNo(partNo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BSParts FindBSParts(string partNo)
        {
            try
            {
                BSParts ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.BSParts cond = new _Schema.BSParts();
                        cond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.BSParts), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.BSParts.fn_PartNo].Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new BSParts();
                        //ret.Code = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.BSParts.fn_Code]);
                        ret.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.BSParts.fn_Descr]);
                        ret.FURNO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.BSParts.fn_FURNO]);
                        ret.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.BSParts.fn_PartNo]);
                        ret.PartType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.BSParts.fn_PartType]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartTypeListByCustomerFromPartCheck(string customer)
        {
            //select PartType from PartCheck where Customer=@customer
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartCheck cond = new _Schema.PartCheck();
                        cond.Customer = customer;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck), "DISTINCT", new List<string>(){_Schema.PartCheck.fn_PartType}, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_PartType]);
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

        public IList<string> GetFamilyList()
        {
            //SELECT DISTINCT b.InfoValue as Family
            //FROM IMES_GetData..Part a, IMES_GetData..PartInfo b, IMES_GetData..PartInfo c
            //WHERE a.PartNo = b.PartNo
            //   AND a.PartNo = c.PartNo 
            //   AND a.PartType = 'SB'
            //   AND a.Descr = 'TPM'
            //   AND LEFT(a.PartNo, 3) = '111'
            //   AND b.InfoType = 'MN'
            //   AND c.InfoType = 'MDL'
            //   ORDER BY Family
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields tf3 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Part);
                        _Schema.Part cond1 = new _Schema.Part();
                        cond1.PartType = "SB";
                        cond1.Descr = "TPM";
                        tf1.equalcond = cond1;
                        _Schema.Part likeCond1 = new _Schema.Part();
                        likeCond1.PartNo = "111%";
                        tf1.likecond = likeCond1;
                        tf1.ToGetFieldNames = null;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo cond2 = new _Schema.PartInfo();
                        cond2.InfoType = "MN";
                        tf2.equalcond = cond2;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new _Schema.TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo cond3 = new _Schema.PartInfo();
                        cond3.InfoType = "MDL";
                        tf3.equalcond = cond3;
                        tf3.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf3.alias, _Schema.PartInfo.fn_InfoValue));

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartType)].Value = cond1.PartType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Descr)].Value = cond1.Descr;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartNo)].Value = likeCond1.PartNo;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoType)].Value = cond2.InfoType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = cond3.InfoType;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);
                            ret.Add(item);
                        }
                    }
                }
                return (from item in ret orderby item select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPCBListByFamily(string family)
        {
            //SELECT DISTINCT b.InfoValue as PCB
            //FROM IMES_GetData..Part a, IMES_GetData..PartInfo b, IMES_GetData..PartInfo c
            //WHERE a.PartNo = b.PartNo
            //AND a.PartNo = c.PartNo
            //AND a.PartType = 'SB'
            //AND a.Descr = 'TPM'
            //AND LEFT(a.PartNo, 3) = '111'
            //AND b.InfoType = 'MDL'
            //AND c.InfoType = 'MN'
            //AND c.InfoValue = @Family
            //ORDER BY PCB
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields tf3 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Part);
                        _Schema.Part cond1 = new _Schema.Part();
                        cond1.PartType = "SB";
                        cond1.Descr = "TPM";
                        tf1.equalcond = cond1;
                        _Schema.Part likeCond1 = new _Schema.Part();
                        likeCond1.PartNo = "111%";
                        tf1.likecond = likeCond1;
                        tf1.ToGetFieldNames = null;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo cond2 = new _Schema.PartInfo();
                        cond2.InfoType = "MDL";
                        tf2.equalcond = cond2;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new _Schema.TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo cond3 = new _Schema.PartInfo();
                        cond3.InfoType = "MN";
                        cond3.InfoValue = family;
                        tf3.equalcond = cond3;
                        tf3.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf3.alias, _Schema.PartInfo.fn_InfoValue));

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartType)].Value = cond1.PartType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Descr)].Value = cond1.Descr;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartNo)].Value = likeCond1.PartNo;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoType)].Value = cond2.InfoType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = cond3.InfoType;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)].Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);
                            ret.Add(item);
                        }
                    }
                }
                return (from item in ret orderby item select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> Get111ListByPCB(string pcb, string family)
        {
            //SELECT DISTINCT a.PartNo as [111]
            //FROM IMES_GetData..Part a, IMES_GetData..PartInfo b, IMES_GetData..PartInfo c
            //WHERE a.PartNo = b.PartNo
            //AND a.PartNo = c.PartNo
            //AND a.PartType = 'SB'
            //AND a.Descr = 'TPM'
            //AND LEFT(a.PartNo, 3) = '111'
            //AND b.InfoType = 'MDL'
            //AND b.InfoValue = @PCB
            //AND c.InfoType = 'MN'
            //AND c.InfoValue = @Family
            //ORDER BY [111]
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields tf3 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Part);
                        _Schema.Part cond1 = new _Schema.Part();
                        cond1.PartType = "SB";
                        cond1.Descr = "TPM";
                        tf1.equalcond = cond1;
                        _Schema.Part likeCond1 = new _Schema.Part();
                        likeCond1.PartNo = "111%";
                        tf1.likecond = likeCond1;
                        tf1.ToGetFieldNames.Add(_Schema.Part.fn_PartNo);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo cond2 = new _Schema.PartInfo();
                        cond2.InfoType = "MDL";
                        cond2.InfoValue = pcb;
                        tf2.equalcond = cond2;
                        tf2.ToGetFieldNames = null;

                        tf3 = new _Schema.TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo cond3 = new _Schema.PartInfo();
                        cond3.InfoType = "MN";
                        cond3.InfoValue = family;
                        tf3.equalcond = cond3;
                        tf3.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf3.alias, _Schema.PartInfo.fn_InfoValue));

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartType)].Value = cond1.PartType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Descr)].Value = cond1.Descr;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartNo)].Value = likeCond1.PartNo;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoType)].Value = cond2.InfoType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = cond3.InfoType;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)].Value = pcb;
                sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)].Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartNo)]);
                            ret.Add(item);
                        }
                    }
                }
                return (from item in ret orderby item select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetFruNoBy111(string pn111)
        {
            //SELECT ISNULL(InfoValue, '') as [FRUNO] FROM IMES_GetData..PartInfo
            //WHERE PartNo = @PartNo
            //AND InfoType = 'FRUNO'
            try
            {
                string ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartInfo cond = new _Schema.PartInfo();
                        cond.PartNo = pn111;
                        cond.InfoType = "FRUNO";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo), null, new List<string>() { _Schema.PartInfo.fn_InfoValue }, cond, null, null, null, null, null, null, null);
                        sqlCtx.Params[_Schema.PartInfo.fn_InfoType].Value = cond.InfoType;
                    }
                }
                sqlCtx.Params[_Schema.PartInfo.fn_PartNo].Value = pn111;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_InfoValue]);
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

        private IPart Find_DB(object key)
        {
            try
            {
                IPart ret = null;

                PartDef retObj = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.partNo = (string)key;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Part_NEW>(tk, "TOP 1", null, new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)));

                        sqlCtx.Param(mtns::Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_partNo).Value = (string)key;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    retObj = FuncNew.SetFieldFromColumn<mtns::Part_NEW, PartDef>(retObj, sqlR, sqlCtx);
                }

                if (retObj != null)
                {
                    ret = new IMES.FisObject.Common.Part.Part(
                        retObj.partNo,
                        retObj.bomNodeType,
                        retObj.partType,
                        retObj.custPartNo,
                        retObj.descr,
                        retObj.remark,
                        retObj.autoDL,
                        retObj.editor,
                        retObj.cdt,
                        retObj.udt,
                        null
                        );
                }

                #region OLD
                //_Schema.SQLContext sqlCtx = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                //    {
                //        _Schema.Part cond = new _Schema.Part();
                //        cond.PartNo = (string)key;
                //        cond.Flag = 1;//Part表加Flag
                //        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), cond, null, null);

                //        sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag
                //    }
                //}
                //sqlCtx.Params[_Schema.Part.fn_PartNo].Value = (string)key;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                //    if (sqlR != null && sqlR.Read())
                //    {
                //        ret = new IMES.FisObject.Common.Part.Part(
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_partNo]),
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_bomNodeType]),
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_partType]),
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_custPartNo]),
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_descr]),
                //                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_FruNo]),
                //                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Vendor]),
                //                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_IECVersion]),
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_remark]),
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_autoDL]),
                //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_editor]),
                //                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_cdt]),
                //                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Metas.Part_NEW.fn_udt]),
                //                                null);
                //        ((IMES.FisObject.Common.Part.Part)ret).Tracker.Clear();
                //    }
                //}
                #endregion

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IPart Find_Cache(object key)
        {
            try
            {
                IPart ret = null;
                lock (_syncObj_cache)
                {
                    if (_cache.Contains((string)key))
                        ret = (IPart)_cache[(string)key];
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IPart FillPartInfos_DB(IPart part)
        {
            try
            {
                IList<IMES.FisObject.Common.Part.PartInfo> newFieldVal = new List<IMES.FisObject.Common.Part.PartInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartInfo cond = new _Schema.PartInfo();
                        cond.PartNo = part.PN;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartInfo.fn_PartNo].Value = part.PN;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.Part.PartInfo partInfo = new IMES.FisObject.Common.Part.PartInfo();
                        partInfo.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_Udt]);
                        partInfo.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_Cdt]);
                        partInfo.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_Editor]);
                        partInfo.InfoType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_InfoType]);
                        partInfo.InfoValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_InfoValue]);
                        partInfo.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_ID]);
                        partInfo.PN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_PartNo]);
                        partInfo.Tracker.Clear();
                        partInfo.Tracker = ((IMES.FisObject.Common.Part.Part)part).Tracker;
                        newFieldVal.Add(partInfo);
                    }
                }
                part.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(part, newFieldVal);

                return part;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IMES.FisObject.Common.Part.PartType> GetPartTypeList_DB()
        {
            try
            {
                IList<IMES.FisObject.Common.Part.PartType> ret = new List<IMES.FisObject.Common.Part.PartType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeEx));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartTypeEx.fn_PartTypeGroup, _Schema.PartTypeEx.fn_partType }));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMES.FisObject.Common.Part.PartType pti = new IMES.FisObject.Common.Part.PartType(
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_ID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_partType]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_PartTypeGroup]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Cdt]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Udt]));
                            pti.Tracker.Clear();
                            ret.Add(pti);
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

        //private IList<IMES.FisObject.Common.Part.PartType> GetPartTypeList_Cache()
        //{
        //    lock (_syncObj_cache_type)
        //    {
        //        return _cache_partType.Values.ToList();
        //    }
        //}

        private void PersistInsertOrRecoverPart(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                if (PersistPeekDeletedPart(item))
                    PersistRecoverPart(item);
                else
                    PersistInsertPart(item);
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

        private bool PersistPeekDeletedPart(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            SqlDataReader sqlR = null;
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Part cond = new _Schema.Part();
                        cond.PartNo = item.PN;
                        cond.Flag = 1;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, null, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");

                        sqlCtx.Params[_Schema.Part.fn_Flag].Value = 0;
                    }
                }
                sqlCtx.Params[_Schema.Part.fn_PartNo].Value = item.PN;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = true;
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
                if (sqlR != null)
                {
                    sqlR.Close();
                }
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private void PersistInsertPart(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Part.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Part.fn_CustPartNo].Value = item.CustPn;
                sqlCtx.Params[_Schema.Part.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Part.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.Part.fn_FruNo].Value = item.FruNo;
                //sqlCtx.Params[_Schema.Part.fn_IECVersion].Value = item.IecVersion;
                sqlCtx.Params[_Schema.Part.fn_PartNo].Value = item.PN;
                sqlCtx.Params[_Schema.Part.fn_PartType].Value = item.Type;
                sqlCtx.Params[_Schema.Part.fn_Remark].Value = item.Remark;
                sqlCtx.Params[_Schema.Part.fn_Udt].Value = cmDt;
                //sqlCtx.Params[_Schema.Part.fn_Vendor].Value = item.Vendor;
                sqlCtx.Params[_Schema.Part.fn_AutoDL].Value = item.AutoDL;
                sqlCtx.Params[_Schema.Part.fn_Flag].Value = 1;
                sqlCtx.Params[_Schema.Part.fn_Descr2].Value = item.Descr2;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        //Part表加Flag
        private void PersistRecoverPart(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Part cond = new _Schema.Part();
                        cond.Flag = 1;
                        cond.PartNo = item.PN;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null,new List<string>(){_Schema.Part.fn_PartNo},null, null, cond, null, null, null, null, null, null, null);

                        sqlCtx.Params[_Schema.Part.fn_Flag].Value = 0;
                    }
                }
                sqlCtx.Params[_Schema.Part.fn_PartNo].Value = item.PN;
                
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_CustPartNo)].Value = item.CustPn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Descr)].Value = item.Descr;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_PartType)].Value = item.Type;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Remark)].Value = item.Remark;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_AutoDL)].Value = item.AutoDL;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Flag)].Value = 1;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Descr2)].Value = item.Descr2;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        private void PersistUpdatePart(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        //Part表加Flag
                        //sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part));
                        _Schema.Part cond = new _Schema.Part();
                        cond.Flag = 1;
                        cond.PartNo = item.PN;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo, _Schema.Part.fn_Flag }, null, null, cond, null, null, null, null, null, null, null);

                        sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag
                    }
                }
                sqlCtx.Params[_Schema.Part.fn_PartNo].Value = item.PN;
                //Part表加Flag
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_CustPartNo)].Value = item.CustPn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Descr)].Value = item.Descr;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Editor)].Value = item.Editor;
                //sqlCtx.Params[_Schema.Part.fn_FruNo].Value = item.FruNo;
                //sqlCtx.Params[_Schema.Part.fn_IECVersion].Value = item.IecVersion;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_PartType)].Value = item.Type;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Remark)].Value = item.Remark;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Udt)].Value = cmDt;
                //sqlCtx.Params[_Schema.Part.fn_Vendor].Value = item.Vendor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_AutoDL)].Value = item.AutoDL;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Descr2)].Value = item.Descr2;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        private void PersistDeletePart(IPart item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        //Part表加Flag
                        //sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part));
                        _Schema.Part cond = new _Schema.Part();
                        cond.PartNo = item.PN;

                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), new List<string>() { _Schema.Part.fn_Flag }, null, null, null, cond, null, null, null, null, null, null, null);
                        sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Flag)].Value = 0;//Part表加Flag
                    }
                }
                sqlCtx.Params[_Schema.Part.fn_PartNo].Value = item.PN;
                //Part表加Flag
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Part.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        //private IList<IPart> GetPartsByType_DB(string type)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetPartsByType_DB->Type:{0}", type);

        //        IList<IPart> ret = new List<IPart>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.Part cond = new _Schema.Part();
        //                cond.PartType = type;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, null, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Part.fn_PartType].Value = type;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    IPart item = new Part(
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_PartNo]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_PartType]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_CustPartNo]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Descr]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_FruNo]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Vendor]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_IECVersion]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Remark]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_AutoDL]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Editor]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Cdt]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Udt]));
        //                    ((Part)item).Tracker.Clear();
        //                    ret.Add(item);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private IList<IPart> GetPartsByType_Cache(string type)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetPartsByType_Cache->Type:{0}", type);

        //        IList<IPart> ret = new List<IPart>();
        //        string key = _Schema.Func.MakeKeyForIdx(preStr1, type);
        //        lock (_syncObj_cache)
        //        {
        //            LoggingInfoFormat("GetPartsByType_Cache->Indexes:{0}", ToString(_byWhateverIndex));
        //            if (_byWhateverIndex.ContainsKey(key))
        //            {
        //                foreach (string pk in _byWhateverIndex[key])
        //                {
        //                    if (_cache.Contains(pk))
        //                        ret.Add((IPart)_cache[pk]);
        //                    else
        //                        throw new Exception(ItemDirty);//缺失Item,应该是 ICacheItemRefreshAction 拿掉的, 通知外面来重新拿DB.
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private IList<IPart> GetPartsByTypeAndDescr_DB(string type, string descr)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetPartsByTypeAndDescr_DB->Type:{0}, Descr:{1}", type, descr);

        //        IList<IPart> ret = new List<IPart>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.Part cond = new _Schema.Part();
        //                cond.PartType = type;
        //                cond.Descr = descr;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, null, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Part.fn_PartType].Value = type;
        //        sqlCtx.Params[_Schema.Part.fn_Descr].Value = descr;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    IPart item = new Part(
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_PartNo]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_PartType]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_CustPartNo]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Descr]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_FruNo]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Vendor]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_IECVersion]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Remark]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_AutoDL]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Editor]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Cdt]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Udt]));
        //                    ((Part)item).Tracker.Clear();
        //                    ret.Add(item);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private IList<IPart> GetPartsByTypeAndDescr_Cache(string type, string descr)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetPartsByTypeAndDescr_Cache->Type:{0}, Descr:{1}", type, descr);

        //        IList<IPart> ret = new List<IPart>();
        //        string key = _Schema.Func.MakeKeyForIdx(preStr2, type, descr);
        //        lock (_syncObj_cache)
        //        {
        //            LoggingInfoFormat("GetPartsByTypeAndDescr_Cache->Indexes:{0}", ToString(_byWhateverIndex));
        //            if (_byWhateverIndex.ContainsKey(key))
        //            {
        //                foreach (string pk in _byWhateverIndex[key])
        //                {
        //                    if (_cache.Contains(pk))
        //                        ret.Add((IPart)_cache[pk]);
        //                    else
        //                        throw new Exception(ItemDirty);//缺失Item,应该是 ICacheItemRefreshAction 拿掉的, 通知外面来重新拿DB.
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        private IList<PartCheck> GetAllPartCheck_DB()
        {
            try
            {
                IList<PartCheck> ret = new List<PartCheck>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PartCheck item = new PartCheck( GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_ID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Customer]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_PartType]),
                                                        //GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Mode]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_ValueType]),
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_NeedSave]),
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_NeedCheck]),
                                                        null
                                                        );
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Udt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Editor]);
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

        private IList<PartCheck> GetPartCheck_DB(string partType, string customer)
        {
            try
            {
                IList<PartCheck> ret = new List<PartCheck>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartCheck cond = new _Schema.PartCheck();
                        cond.PartType = partType;
                        cond.Customer = customer;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartCheck.fn_PartType].Value = partType;
                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PartCheck item = new PartCheck( GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_ID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Customer]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_PartType]),
                                                        //GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Mode]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_ValueType]),
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_NeedSave]),
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_NeedCheck]),
                                                        null
                                                        );
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Udt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheck.fn_Editor]);
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

        //private IList<PartCheck> GetPartCheck_Cache(string partType, string customer)
        //{
        //    try
        //    {
        //        IList<PartCheck> ret = new List<PartCheck>();
        //        string key = _Schema.Func.MakeKeyForIdx(preStr4, partType, customer);
        //        lock (_syncObj_cache_check)
        //        {
        //            if (_byWhateverIndex_partCheck.ContainsKey(key))
        //            {
        //                foreach (int pk in _byWhateverIndex_partCheck[key])
        //                {
        //                    if (_cache_partCheck.ContainsKey(pk))
        //                        ret.Add(_cache_partCheck[pk]);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private PartCheck FillMatchRule_DB(PartCheck partCheck)
        {
            try
            {
                IMatchRule newFieldVal = null;// new List<PartInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartCheckMatchRule cond = new _Schema.PartCheckMatchRule();
                        //cond.Customer = partCheck.Customer;
                        //cond.PartType = partCheck.PartType;
                        cond.PartCheckID = partCheck.ID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), cond, null, null);
                    }
                }
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_Customer].Value = partCheck.Customer;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartType].Value = partCheck.PartType;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartCheckID].Value = partCheck.ID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        IList<PartMatchRuleElement> pmrs = new List<PartMatchRuleElement>();
                        newFieldVal = new CommonMatchRule(pmrs); //???? 只此一种?
                        while (sqlR.Read())
                        {
                            PartMatchRuleElement item = new PartMatchRuleElement();
                            item.PnExp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_PnExp]);
                            item.RegExp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_RegExp]);
                            item.ContainCheckBit = Convert.ToBoolean(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_ContainCheckBit]));
                            item.PartPropertyExp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_PartPropertyExp]);
                            pmrs.Add(item);
                        }
                    }
                }
                partCheck.GetType().GetField("_matchRule", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(partCheck, newFieldVal);

                return partCheck;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IMES.FisObject.Common.Part.PartType> GetTypeList_DB(string partTypeGroup)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                IList<IMES.FisObject.Common.Part.PartType> ret = new List<IMES.FisObject.Common.Part.PartType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeEx cond = new _Schema.PartTypeEx();
                        cond.PartTypeGroup = partTypeGroup;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeEx), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeEx.fn_PartTypeGroup].Value = partTypeGroup;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.Common.Part.PartType item = new IMES.FisObject.Common.Part.PartType(
                                                         GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_ID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_partType]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_PartTypeGroup]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Cdt]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Udt]));
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

        //private IList<IMES.FisObject.Common.Part.PartType> GetTypeList_Cache(string partTypeGroup)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        IList<IMES.FisObject.Common.Part.PartType> ret = new List<IMES.FisObject.Common.Part.PartType>();
        //        string key = _Schema.Func.MakeKeyForIdx(preStr3, partTypeGroup);
        //        lock (_syncObj_cache_type)
        //        {
        //            LoggingInfoFormat("GetTypeList_Cache: Keys[{0}]", string.Join(",", _byWhateverIndex_partType.Keys.ToArray()));

        //            if (_byWhateverIndex_partType.ContainsKey(key))
        //            {
        //                foreach (string pk in _byWhateverIndex_partType[key])
        //                {
        //                    if (_cache_partType.ContainsKey(pk))
        //                        ret.Add(_cache_partType[pk]);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        private IMES.FisObject.Common.Part.PartType GetPartType_DB(string type)
        {
            try
            {
                IMES.FisObject.Common.Part.PartType ret = null;

                try
                {
                    _Schema.SQLContext sqlCtx = null;
                    lock (MethodBase.GetCurrentMethod())
                    {
                        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                        {
                            _Schema.PartTypeEx cond = new _Schema.PartTypeEx();
                            cond.partType = type;
                            sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeEx), cond, null, null);
                        }
                    }
                    sqlCtx.Params[_Schema.PartTypeEx.fn_partType].Value = type;
                    using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                    {
                        if (sqlR != null && sqlR.Read())
                        {
                            ret = new IMES.FisObject.Common.Part.PartType(
                                                GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_ID]),
                                                 GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_partType]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_PartTypeGroup]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Cdt]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeEx.fn_Udt]));
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
            catch (Exception)
            {
                throw;
            }
        }

        //private IMES.FisObject.Common.Part.PartType GetPartType_Cache(string type)
        //{
        //    try
        //    {
        //        IMES.FisObject.Common.Part.PartType ret = null;
        //        lock (_syncObj_cache_type)
        //        {
        //            if (_cache_partType.ContainsKey(type))
        //                ret = _cache_partType[type];
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private IList<KPTypeInfo> Converter(IList<PartTypeInfo> innerList)
        {
            IList<KPTypeInfo> ret = new List<KPTypeInfo>();
            if (innerList != null && innerList.Count > 0)
            {
                foreach (PartTypeInfo pt in innerList)
                {
                    ret.Add(Converter(pt));
                }
            }
            return ret;
        }

        private KPTypeInfo Converter(PartTypeInfo pt)
        {
            KPTypeInfo item = new KPTypeInfo();
            item.friendlyName = pt.friendlyName;
            item.id = pt.id;
            return item;
        }

        private PartTypeInfo Converter(IMES.FisObject.Common.Part.PartType pt)
        {
            PartTypeInfo ret = new PartTypeInfo();
            ret.id = ret.friendlyName = pt.PartTypeName;
            return ret;
        }

        private IList<PartTypeInfo> Converter(IList<IMES.FisObject.Common.Part.PartType> pts)
        {
            if (pts != null)
                return (from item in pts select Converter(item)).ToList();
            else
                return new List<PartTypeInfo>();
        }

        //private IList<IPart> GetPartByInfoTypeValue_DB(string infoType, string infoValue)
        //{
        //    try
        //    {
        //        IList<IPart> ret = new List<IPart>();

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //            {
        //                tf1 = new _Schema.TableAndFields();
        //                tf1.Table = typeof(_Schema.Part);

        //                tf2 = new _Schema.TableAndFields();
        //                tf2.Table = typeof(_Schema.PartInfo);
        //                _Schema.PartInfo cond = new _Schema.PartInfo();
        //                cond.InfoType = infoType;
        //                cond.InfoValue = infoValue;
        //                tf2.equalcond = cond;
        //                tf2.ToGetFieldNames = null;

        //                List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
        //                tblCnntIs.Add(tc1);

        //                _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

        //                sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);
        //            }
        //        }
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];

        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoType)].Value = infoType;
        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)].Value = infoValue;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    IPart item = new Part(
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartNo)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartType)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_CustPartNo)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Descr)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_FruNo)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Vendor)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_IECVersion)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Remark)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_AutoDL)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Editor)]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Cdt)]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Udt)]));
        //                    ((Part)item).Tracker.Clear();
        //                    ret.Add(item);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private IList<IPart> GetPartByInfoTypeValue_Cache(string infoType, string infoValue)
        //{
        //    try
        //    {
        //        IList<IPart> ret = new List<IPart>();
        //        string key = _Schema.Func.MakeKeyForIdx(preStr5, infoType, infoValue);
        //        lock (_syncObj_cache)
        //        {
        //            if (_byWhateverIndex.ContainsKey(key))
        //            {
        //                foreach (string pk in _byWhateverIndex[key])
        //                {
        //                    if (_cache.Contains(pk))
        //                        ret.Add((IPart)_cache[pk]);
        //                    else
        //                        throw new Exception(ItemDirty);//缺失Item,应该是 ICacheItemRefreshAction 拿掉的, 通知外面来重新拿DB.
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void CheckAndInsertSubs(IPart item, StateTracker tracker)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                IList<IMES.FisObject.Common.Part.PartInfo> fieldVal = (IList<IMES.FisObject.Common.Part.PartInfo>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                if (fieldVal != null && fieldVal.Count > 0)//(item.Attributes.Count > 0)
                {
                    foreach (IMES.FisObject.Common.Part.PartInfo pi in fieldVal)//item.Attributes)
                    {
                        if (tracker.GetState(pi) == DataRowState.Added)
                        {
                            pi.PN = item.PN;
                            this.PersistInsertPartInfo(pi);
                        }
                    }
                }
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private void CheckAndUpdateSubs(IPart item, StateTracker tracker)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                IList<IMES.FisObject.Common.Part.PartInfo> fieldVal = (IList<IMES.FisObject.Common.Part.PartInfo>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                if (fieldVal != null && fieldVal.Count > 0)//(item.Attributes.Count > 0)
                {
                    foreach (IMES.FisObject.Common.Part.PartInfo pi in fieldVal)//item.Attributes)
                    {
                        if (tracker.GetState(pi) == DataRowState.Modified)
                        {
                            this.PersistUpdatePartInfo(pi);
                        }
                    }
                }
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private void CheckAndDeleteSubs(IPart item, StateTracker tracker)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                IList<IMES.FisObject.Common.Part.PartInfo> fieldVal = (IList<IMES.FisObject.Common.Part.PartInfo>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                if (fieldVal != null && fieldVal.Count > 0)//(item.Attributes.Count > 0)
                {
                    IList<IMES.FisObject.Common.Part.PartInfo> iLstToDel = new List<IMES.FisObject.Common.Part.PartInfo>();
                    foreach (IMES.FisObject.Common.Part.PartInfo pi in fieldVal)//item.Attributes)
                    {
                        if (tracker.GetState(pi) == DataRowState.Deleted)
                        {
                            this.PersistDeletePartInfo(pi);
                            iLstToDel.Add(pi);
                        }
                    }
                    foreach (IMES.FisObject.Common.Part.PartInfo pi in iLstToDel)
                    {
                        fieldVal.Remove(pi);
                    }
                }
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        private void PersistInsertPartInfo(IMES.FisObject.Common.Part.PartInfo item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PartInfo.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.PartInfo.fn_ID].Value;
                sqlCtx.Params[_Schema.PartInfo.fn_InfoType].Value = item.InfoType;
                sqlCtx.Params[_Schema.PartInfo.fn_InfoValue].Value = item.InfoValue;
                sqlCtx.Params[_Schema.PartInfo.fn_PartNo].Value = item.PN;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
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

        private void PersistUpdatePartInfo(IMES.FisObject.Common.Part.PartInfo item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PartInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartInfo.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PartInfo.fn_InfoType].Value = item.InfoType;
                sqlCtx.Params[_Schema.PartInfo.fn_InfoValue].Value = item.InfoValue;
                sqlCtx.Params[_Schema.PartInfo.fn_PartNo].Value = item.PN;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        private void PersistDeletePartInfo(IMES.FisObject.Common.Part.PartInfo item)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo));
                    }
                }
                sqlCtx.Params[_Schema.PartInfo.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        //private static bool _isCached = false;
        public bool IsCached()
        {
            lock (MethodBase.GetCurrentMethod())
            {
                bool configVal = DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Part);
                //if (
                //    (false == _isCached && true == configVal)
                //    ||
                //    (true == _isCached && false == configVal)
                //    )
                //{
                //    ClearCalledMethods();
                //}
                //_isCached = configVal;
                return configVal;
            }
        }

        public void ProcessItem(CacheUpdateInfo item)
        {
            if (item.Type == CacheType.Part)
                LoadOneCache(item.Item);
            //else if (item.Type == CacheType.PartType)
            //    LoadAllCachePartType();
            //else if (item.Type == CacheType.PartCheck)
            //    LoadAllCachePartCheck();
        }

        public void ClearCache()
        {
            lock (_syncObj_cache)
            {
                _cache.Flush();
            }
        }

        private void LoadOneCache(string pk)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {
                LoggingInfoFormat("Part::LoadOneCache->PK:{0}", pk);
                lock (_syncObj_cache)
                {
                    if (_cache.Contains(pk))
                    {
                        UnregistIndexesForOnePart((IPart)_cache[pk]);
                        _cache.Remove(pk);
                    }

                    #region For YWH
                    /*
                    IPart prt = this.Find_DB(pk);
                    if (prt != null)
                    {
                        AddAndRegistOnePart(prt);
                    }
                    */
                    #endregion
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

        private static void AddAndRegistOnePart(IPart part)
        {
            //LoggingBegin(typeof(PartRepository), MethodBase.GetCurrentMethod());
            try
            {
                if (!_cache.Contains((string)part.Key))
                    AddToCache((string)part.Key, part);

                //LoggingInfoFormat("AddAndRegistOnePart%1->Indexes:{0}", ToString(_byWhateverIndex));

                ////Regist
                //Regist((string)part.Key, _Schema.Func.MakeKeyForIdx(preStr1, part.Type));

                //LoggingInfoFormat("AddAndRegistOnePart%2->Indexes:{0}", ToString(_byWhateverIndex));

                ////Regist
                //Regist((string)part.Key, _Schema.Func.MakeKeyForIdx(preStr2, part.Type, part.Descr));

                //LoggingInfoFormat("AddAndRegistOnePart%3->Indexes:{0}", ToString(_byWhateverIndex));

                ////Regist
                //if (part.Attributes != null)
                //{
                //    foreach (PartInfo pi in part.Attributes)
                //    {
                //        if (pi.InfoType != null && pi.InfoValue != null)
                //            Regist((string)part.Key, _Schema.Func.MakeKeyForIdx(preStr5, pi.InfoType, pi.InfoValue));
                //    }
                //}

                //LoggingInfoFormat("AddAndRegistOnePart%4->Indexes:{0}", ToString(_byWhateverIndex));
            }
            catch (Exception)
            {
                //LoggingError(typeof(PartRepository), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                //LoggingEnd(typeof(PartRepository), MethodBase.GetCurrentMethod());
            }
        }

        private static void UnregistIndexesForOnePart(IPart part)
        {
            //LoggingBegin(typeof(PartRepository), MethodBase.GetCurrentMethod());
            try
            {
                //LoggingInfoFormat("UnregistIndexesForOnePart%1->Indexes:{0}", ToString(_byWhateverIndex));

                //string idxStr1 = _Schema.Func.MakeKeyForIdx(preStr1, part.Type);
                //if (_byWhateverIndex.ContainsKey(idxStr1))// 2010-01-24 Liu Dong(eB1-4)         Modify ITC-1103-0132
                //{
                //    IList<string> stem = _byWhateverIndex[idxStr1];
                //    if (stem != null)
                //    {
                //        if (stem.Contains((string)part.Key))
                //            stem.Remove((string)part.Key);
                //        if (stem.Count < 1)
                //            _byWhateverIndex.Remove(idxStr1);
                //    }
                //}

                //LoggingInfoFormat("UnregistIndexesForOnePart%2->Indexes:{0}", ToString(_byWhateverIndex));

                //string idxStr2 = _Schema.Func.MakeKeyForIdx(preStr2, part.Type, part.Descr);
                //if (_byWhateverIndex.ContainsKey(idxStr2))// 2010-01-24 Liu Dong(eB1-4)         Modify ITC-1103-0132
                //{
                //    IList<string> stem = _byWhateverIndex[idxStr2];
                //    if (stem != null)
                //    {
                //        if (stem.Contains((string)part.Key))
                //            stem.Remove((string)part.Key);
                //        if (stem.Count < 1)
                //            _byWhateverIndex.Remove(idxStr2);
                //    }
                //}

                //LoggingInfoFormat("UnregistIndexesForOnePart%3->Indexes:{0}", ToString(_byWhateverIndex));

                //if (part.Attributes != null)
                //{
                //    foreach (PartInfo pi in part.Attributes)
                //    {
                //        if (pi.InfoType != null && pi.InfoValue != null)
                //        {
                //            string idxStr5 = _Schema.Func.MakeKeyForIdx(preStr5, pi.InfoType, pi.InfoValue);
                //            if (_byWhateverIndex.ContainsKey(idxStr5))
                //            {
                //                IList<string> stem = _byWhateverIndex[idxStr5];
                //                if (stem != null)
                //                {
                //                    if (stem.Contains((string)part.Key))
                //                        stem.Remove((string)part.Key);
                //                    if (stem.Count < 1)
                //                        _byWhateverIndex.Remove(idxStr5);
                //                }
                //            }
                //        }
                //    }
                //}

                //LoggingInfoFormat("UnregistIndexesForOnePart%4->Indexes:{0}", ToString(_byWhateverIndex));
            }
            catch (Exception)
            {
                //LoggingError(typeof(PartRepository), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                //LoggingEnd(typeof(PartRepository), MethodBase.GetCurrentMethod());
            }
        }

        //private void LoadAllCachePartType()
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        IList<IMES.FisObject.Common.Part.PartType> prtTps = this.GetPartTypeList_DB();
        //        if (prtTps != null && prtTps.Count > 0)
        //        {
        //            lock (_syncObj_cache_type)
        //            {
        //                _cache_partType.Clear();
        //                _byWhateverIndex_partType.Clear();

        //                foreach (IMES.FisObject.Common.Part.PartType prtTp in prtTps)
        //                {
        //                    _cache_partType.Add(prtTp.PartTypeName, prtTp);

        //                    //Regist
        //                    Regist_PartType(prtTp.PartTypeName, _Schema.Func.MakeKeyForIdx(preStr3, prtTp.PartTypeGroup));//PartTypeGroup
        //                }

        //                LoggingInfoFormat("LoadAllCachePartType: Keys[{0}]", string.Join(",", _byWhateverIndex_partType.Keys.ToArray()));
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private void LoadAllCachePartCheck()
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        IList<PartCheck> prtCks = this.GetAllPartCheck_DB();
        //        if (prtCks != null && prtCks.Count > 0)
        //        {
        //            lock (_syncObj_cache_check)
        //            {
        //                _cache_partCheck.Clear();
        //                _byWhateverIndex_partCheck.Clear();

        //                foreach (PartCheck prtCk in prtCks)
        //                {
        //                    _cache_partCheck.Add((int)prtCk.Key, prtCk);

        //                    //Regist
        //                    Regist_PartCheck(prtCk.ID, _Schema.Func.MakeKeyForIdx(preStr4, prtCk.PartType, prtCk.Customer));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private static void Regist(string pk, string key)
        //{
        //    LoggingBegin(typeof(PartRepository), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("Part::Regist->PK:{0}, Key:{1}.", pk, key);

        //        IList<string> PKs = null;
        //        try
        //        {
        //            PKs = _byWhateverIndex[key];
        //        }
        //        catch (KeyNotFoundException)
        //        {
        //            PKs = new List<string>();
        //            _byWhateverIndex.Add(key, PKs);
        //        }
        //        if (!PKs.Contains(pk))
        //            PKs.Add(pk);
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(typeof(PartRepository), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(typeof(PartRepository), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private static void Regist_PartType(string pk, string key)
        //{
        //    IList<string> PKs = null;
        //    try
        //    {
        //        PKs = _byWhateverIndex_partType[key];
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        PKs = new List<string>();
        //        _byWhateverIndex_partType.Add(key, PKs);
        //    }
        //    if (!PKs.Contains(pk))
        //        PKs.Add(pk);
        //}

        //private static void Regist_PartCheck(int pk, string key)
        //{
        //    IList<int> PKs = null;
        //    try
        //    {
        //        PKs = _byWhateverIndex_partCheck[key];
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        PKs = new List<int>();
        //        _byWhateverIndex_partCheck.Add(key, PKs);
        //    }
        //    if (!PKs.Contains(pk))
        //        PKs.Add(pk);
        //}

        private CacheUpdateInfo GetACacheSignal(string pk, string type)
        {
            CacheUpdateInfo ret = new CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = type;
            ret.Item = pk;
            return ret;
        }

        private static void AddToCache(string key, object obj)
        {
            //Vincent 2015-11-15 modify none callback and static absoluteTime
            //_cache.Add(key, obj, CacheItemPriority.Normal, new PartRefreshAction((IPart)obj), new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_PartsCache"].ToString()))));
            //_cache.Add(key, obj, CacheItemPriority.Normal, null, _cacheAbsoluteTime);
            _cache.Add(key, obj, CacheItemPriority.Normal, null, new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_PartsCache"].ToString()))));
        }

        /// <summary>
        /// 方法是否已经被调用过了第一次
        /// </summary>
        /// <param name="methodId"></param>
        /// <returns></returns>
        //private static bool IsFirstCalled(int methodId)
        //{
        //    lock (_calledMethods)
        //    {
        //        if (_calledMethods.Contains(methodId))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            _calledMethods.Add(methodId);
        //            return false;
        //        }
        //    }
        //}

        //private static void ClearCalledMethods()
        //{
        //    LoggingBegin(typeof(PartRepository), MethodBase.GetCurrentMethod());
        //    lock (_calledMethods)
        //    {
        //        _calledMethods.Clear();
        //    }
        //    LoggingEnd(typeof(PartRepository), MethodBase.GetCurrentMethod());
        //}

        [Serializable]
        private class PartRefreshAction : ICacheItemRefreshAction
        {
            IPart _part = null;
            public PartRefreshAction(IPart part)
            {
                _part = part;
            }
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
                LoggingInfoFormat("PartRefreshAction::Refresh->Key:{0}, Reason:{1}", key, removalReason.ToString());
                //lock (_syncObj_cache)
                //{
                //    UnregistIndexesForOnePart(_part); //这里不取消注册索引,便于知道Cache自动踢元素,以重新Load DB.
                //}
            }
        }

        #endregion

        #region For Maintain

        public IList<IPart> GetPartListByPreStr(string partNoPreStr)
        {
            try
            {
                IList<IPart> ret = new List<IPart>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Part_NEW eqCond = new _Metas.Part_NEW();
                        eqCond.flag = 1;//Part表加Flag
                        _Metas.Part_NEW likeCond = new _Metas.Part_NEW();
                        likeCond.partNo = partNoPreStr + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Part_NEW>(tk, null, null, new ConditionCollection<_Metas.Part_NEW>(
                            new EqualCondition<_Metas.Part_NEW>(eqCond),
                            new LikeCondition<_Metas.Part_NEW>(likeCond)));

                        sqlCtx.Param(_Metas.Part_NEW.fn_flag).Value = eqCond.flag;//Part表加Flag
                    }
                }
                sqlCtx.Param(_Metas.Part_NEW.fn_partNo).Value = partNoPreStr + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IPart item = new IMES.FisObject.Common.Part.Part(
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_bomNodeType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_custPartNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_descr)),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_FruNo]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Vendor]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_IECVersion]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_remark)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_autoDL)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_editor)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_cdt)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_udt)),
                                                null);
                        ((IMES.FisObject.Common.Part.Part)item).Tracker.Clear();
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

        public IList<fons::AssemblyCode> GetAssemblyCodeList(string partNo)
        {
            try
            {
                return AssRepository.GetAssemblyCodeList(partNo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddAssemblyCode(fons::AssemblyCode item)
        {
            try
            {
                //IUnitOfWork uow = new UnitOfWork.UnitOfWork();
                //AssRepository.Add(item, uow);
                //uow.Commit();

                //#region OLD

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.AssemblyCode.fn_assemblyCode].Value = item.AssCode;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Family].Value = item.Family;
                //sqlCtx.Params[_Schema.AssemblyCode.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = item.Pn;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = item.Region;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                item.Tracker.Clear();

                //#endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAssemblyCode(fons::AssemblyCode item)
        {
            try
            {
                //IUnitOfWork uow = new UnitOfWork.UnitOfWork();
                //AssRepository.Update(item, uow);
                //uow.Commit();

                //#region OLD

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.AssemblyCode.fn_assemblyCode].Value = item.AssCode;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.AssemblyCode.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = item.Pn;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = item.Region;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                item.Tracker.Clear();

                //#endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<int> CheckExistedAssemblyCode(string partNo, string assemblyCode)
        {
            try
            {
                return AssRepository.CheckExistedAssemblyCode(partNo, assemblyCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAssemblyCode(int assemblyCodeId)
        {
            try
            {
                //AssemblyCode toDel = new AssemblyCode();
                //toDel.ID = assemblyCodeId;
                //toDel.Tracker.Clear();

                //IUnitOfWork uow = new UnitOfWork.UnitOfWork();
                //AssRepository.Remove(toDel, uow);
                //uow.Commit();

                //#region OLD

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode));
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_ID].Value = assemblyCodeId;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                //#endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IList<PartTypeAttributeAndPartInfoValue> GetPartTypeAttributeAndPartInfoValueList(string partNo, string partType)
        //{
        //    //select PartTypeAttribute.Code as Item, 
        //    //Case When A.InfoValue is null then '' else A.InfoValue End Case as Content, A.ID
        //    //From (select Part.PartType, PartInfo.InfoType, PartInfo.InfoValue, PartInfo.ID 
        //    //        from Part 
        //    //        inner join PartInfo 
        //    //        on Part.PartNo = PartInfo.PartNo where Part.PartNo = ?) as A 
        //    //Right Outer Join PartTypeAttribute 
        //    //On A.PartType = PartTypeAttribute.PartType and A.InfoType = PartTypeAttribute.Code
        //    //where PartTypeAttribute.PartType = 'LCM'
        //    try
        //    {
        //        IList<PartTypeAttributeAndPartInfoValue> ret = new List<PartTypeAttributeAndPartInfoValue>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();
        //                sqlCtx.Sentence = "SELECT {0}.{10} AS Item, ISNULL(A.{5},'') AS Content, A.{6} " +
        //                                  "FROM (SELECT {1}.{3}, {2}.{4}, {2}.{5}, {2}.{6} FROM {1} INNER JOIN {2} ON {1}.{7}={2}.{8} WHERE {1}.{7}=@{7} AND {1}.{11}=1 ) AS A " + //Part表加Flag
        //                                  "RIGHT OUTER JOIN {0} ON A.{3}={0}.{9} AND A.{4}={0}.{10} " +
        //                                  "WHERE {0}.{9}=@{9} ";

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.PartTypeAttribute).Name,
        //                                                                typeof(_Schema.Part).Name,
        //                                                                typeof(_Schema.PartInfo).Name,
        //                                                                _Schema.Part.fn_PartType,
        //                                                                _Schema.PartInfo.fn_InfoType,
        //                                                                _Schema.PartInfo.fn_InfoValue,
        //                                                                _Schema.PartInfo.fn_ID,
        //                                                                _Schema.Part.fn_PartNo,
        //                                                                _Schema.PartInfo.fn_PartNo,
        //                                                                _Schema.PartTypeAttribute.fn_PartType,
        //                                                                _Schema.PartTypeAttribute.fn_Code,
        //                                                                _Schema.Part.fn_Flag);//Part表加Flag

        //                sqlCtx.Params.Add(_Schema.Part.fn_PartNo, new SqlParameter("@" + _Schema.Part.fn_PartNo, SqlDbType.VarChar));
        //                sqlCtx.Params.Add(_Schema.PartTypeAttribute.fn_PartType, new SqlParameter("@" + _Schema.PartTypeAttribute.fn_PartType, SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Part.fn_PartNo].Value = partNo;
        //        sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = partType;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    PartTypeAttributeAndPartInfoValue item = new PartTypeAttributeAndPartInfoValue();
        //                    item.Item = GetValue_Str(sqlR, 0);
        //                    item.Content = GetValue_Str(sqlR, 1);
        //                    item.PartInfoId = GetValue_Int32(sqlR, 2);
        //                    ret.Add(item);
        //                }
        //            }
        //        }

        //        #region  OLD
        //        //_Schema.SQLContext sqlCtx = null;
        //        //_Schema.TableAndFields tf1 = null;
        //        //_Schema.TableAndFields tf2 = null;
        //        //_Schema.TableAndFields tf3 = null;
        //        //_Schema.TableAndFields[] tblAndFldsesArray = null;
        //        //lock (MethodBase.GetCurrentMethod())
        //        //{
        //        //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //        //    {
        //        //        tf1 = new _Schema.TableAndFields();
        //        //        tf1.Table = typeof(_Schema.Part);
        //        //        _Schema.Part eqCond = new _Schema.Part();
        //        //        eqCond.PartNo = partNo;
        //        //        tf1.equalcond = eqCond;
        //        //        tf1.ToGetFieldNames = null;

        //        //        tf2 = new _Schema.TableAndFields();
        //        //        tf2.Table = typeof(_Schema.PartInfo);
        //        //        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);
        //        //        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_ID);

        //        //        tf3 = new _Schema.TableAndFields();
        //        //        tf3.Table = typeof(_Schema.PartTypeAttribute);
        //        //        _Schema.PartTypeAttribute equalCond = new _Schema.PartTypeAttribute();
        //        //        equalCond.PartType = partType;
        //        //        tf3.equalcond = equalCond;
        //        //        tf3.ToGetFieldNames.Add(_Schema.PartTypeAttribute.fn_Code);

        //        //        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //        //        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
        //        //        tblCnntIs.Add(tc1);
        //        //        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.PartInfo.fn_InfoType, tf3, _Schema.PartTypeAttribute.fn_Code);
        //        //        tblCnntIs.Add(tc2);
        //        //        _Schema.TableConnectionItem tc3 = new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartType, tf3, _Schema.PartTypeAttribute.fn_PartType);
        //        //        tblCnntIs.Add(tc3);

        //        //        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //        //        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3 };

        //        //        _Schema.TableBiJoinedLogic tblBiJndLgc = new _Schema.TableBiJoinedLogic();
        //        //        tblBiJndLgc.Add(tf1);
        //        //        tblBiJndLgc.Add(_Schema.Func.JOIN);
        //        //        tblBiJndLgc.Add(tf2);
        //        //        tblBiJndLgc.Add(tc1);
        //        //        tblBiJndLgc.Add(_Schema.Func.RIGHTJOIN);
        //        //        tblBiJndLgc.Add(tf3);
        //        //        tblBiJndLgc.Add(tc2);
        //        //        tblBiJndLgc.Add(tc3);

        //        //        sqlCtx = _Schema.Func.GetConditionedComprehensiveJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts, tblBiJndLgc);

        //        //        string toReplace = string.Format("{0}=@{1}", _Schema.Func.DecAliasInner(tf1.alias, _Schema.Part.fn_PartNo), _Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartNo));
        //        //        sqlCtx.Sentence = sqlCtx.Sentence.Replace(toReplace, string.Format("({0} OR {1} IS NULL)", toReplace, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Part.fn_PartNo)));
        //        //        //(t1.PartNo='110001' or t1.PartNo IS NULL)
        //        //    }
        //        //}
        //        //tf1 = tblAndFldsesArray[0];
        //        //tf2 = tblAndFldsesArray[1];
        //        //tf3 = tblAndFldsesArray[2];

        //        //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_PartNo)].Value = partNo;
        //        //sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.PartTypeAttribute.fn_PartType)].Value = partType;

        //        //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        //{
        //        //    if (sqlR != null)
        //        //    {
        //        //        while (sqlR.Read())
        //        //        {
        //        //            PartTypeAttributeAndPartInfoValue item = new PartTypeAttributeAndPartInfoValue();
        //        //            item.Item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf3.alias, _Schema.PartTypeAttribute.fn_Code)]);
        //        //            item.PartInfoId = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_ID)]);
        //        //            item.Content = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);
        //        //            ret.Add(item);
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        return ret;

        //        #region . misunderstood .
        //        //IList<PartInfo> ret = new List<PartInfo>();

        //        //_Schema.SQLContext sqlCtx = null;
        //        //lock (MethodBase.GetCurrentMethod())
        //        //{
        //            //if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            //{
        //            //    _Schema.PartInfo cond = new _Schema.PartInfo();
        //            //    cond.PartNo = partNo;

        //            //    sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo), cond, null, null);
        //            //}
        //        //}
        //        //sqlCtx.Params[_Schema.PartInfo.fn_PartNo].Value = partNo;
        //        //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        //{
        //        //    if (sqlR != null)
        //        //    {
        //        //        while (sqlR.Read())
        //        //        {
        //        //            PartInfo item = new PartInfo();
        //        //            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_Cdt]);
        //        //            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_Editor]);
        //        //            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_ID]);
        //        //            item.InfoType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_InfoType]);
        //        //            item.InfoValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_InfoValue]);
        //        //            item.PN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_PartNo]);
        //        //            item.Tracker.Clear();
        //        //            ret.Add(item);
        //        //        }
        //        //    }
        //        //}
        //        //return ret;
        //        #endregion
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public IList<IMES.FisObject.Common.Part.PartType> GetPartTypeObjList()
        {
            try
            {
                return GetPartTypeList_DB();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartTypeDescription> GetPartTypeDescriptionList(string partType)
        {
            try
            {
                IList<PartTypeDescription> ret = new List<PartTypeDescription>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeDescription cond = new _Schema.PartTypeDescription();
                        cond.PartType = partType;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeDescription), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartTypeDescription.fn_Description);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeDescription.fn_PartType].Value = partType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartTypeDescription item = new PartTypeDescription( GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartTypeDescription.fn_ID]),
                                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeDescription.fn_PartType]),
                                                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeDescription.fn_Description]));
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

        public fons::AssemblyCode FindAssemblyCodeById(int assCodeId)
        {
            try
            {
                fons::AssemblyCode ret = null;

                ret = AssRepository.Find(assCodeId);

                #region OLD
                //_Schema.SQLContext sqlCtx = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                //    {
                //        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                //        cond.ID = assCodeId;
                //        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null);
                //    }
                //}
                //sqlCtx.Params[_Schema.AssemblyCode.fn_ID].Value = assCodeId;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                //    if (sqlR != null)
                //    {
                //        if (sqlR.Read())
                //        {
                //            ret = new AssemblyCode(                 GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                //                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                //                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                //            ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                //            ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                //            ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                //            ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                //            ret.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                //            ret.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
                //            ret.Tracker.Clear();
                //        }
                //    }
                //}
                #endregion

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons::AssemblyCode> GetAssemblyCodeList(string model, string partNo)
        {
            try
            {
                return AssRepository.GetAssemblyCodeList(model, partNo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPartCheckList()
        {
            //SELECT [Customer],[PartType],[ValueType],[NeedSave],[NeedCheck],Editor,Cdt,Udt,[ID]
            //  FROM [IMES_GetData].[dbo].[PartCheck] order by [Customer],[PartType],[ValueType]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartCheck.fn_Customer, _Schema.PartCheck.fn_PartType, _Schema.PartCheck.fn_ValueType }));
                    }
                }
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null);
                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.PartCheck.fn_Customer],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_PartType],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_ValueType],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_NeedSave],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_NeedCheck],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_Editor],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_Cdt],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_Udt],
                                                                sqlCtx.Indexes[_Schema.PartCheck.fn_ID]});
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPartTypes()
        {
            //SELECT [PartType]
            // FROM [IMES_GetData].[dbo].[PartType] ORDER BY [PartType]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeEx));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartTypeEx.fn_partType);
                    }
                }
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetValueTypeList(string partType)
        {
            //SELECT [Code]      
            //  FROM [IMES_GetData].[dbo].[PartTypeAttribute]
            //where PartType='partType' ORDER BY [Code]  
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeAttribute cond = new _Schema.PartTypeAttribute();
                        cond.PartType = partType;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeAttribute), cond, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartTypeAttribute.fn_Code);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = partType;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetExistPartCheck(string customer, string partType, string valueType, int id)
        {
            //SELECT [ID]
            //  FROM [IMES_GetData].[dbo].[PartCheck]
            //WHERE [Customer]='customer' AND [PartType]='partType' AND [ValueType]='valueType'
            //AND [ID]<>id
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartCheck eqcond = new _Schema.PartCheck();
                        eqcond.Customer = customer;
                        eqcond.PartType = partType;
                        eqcond.ValueType = valueType;
                        _Schema.PartCheck neqcond = new _Schema.PartCheck();
                        neqcond.ID = id;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck), null, new List<string>() { _Schema.PartCheck.fn_ID }, eqcond, null, null, null, null, null, null, null, neqcond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = customer;
                sqlCtx.Params[_Schema.PartCheck.fn_PartType].Value = partType;
                sqlCtx.Params[_Schema.PartCheck.fn_ValueType].Value = valueType;
                sqlCtx.Params[_Schema.PartCheck.fn_ID].Value = id;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetExistPartCheck(string customer, string partType, string valueType)
        {
            //SELECT [ID]
            //  FROM [IMES_GetData].[dbo].[PartCheck]
            //WHERE [Customer]='customer' AND [PartType]='partType' AND [ValueType]='valueType'
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartCheck eqcond = new _Schema.PartCheck();
                        eqcond.Customer = customer;
                        eqcond.PartType = partType;
                        eqcond.ValueType = valueType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck), null, new List<string>() { _Schema.PartCheck.fn_ID }, eqcond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = customer;
                sqlCtx.Params[_Schema.PartCheck.fn_PartType].Value = partType;
                sqlCtx.Params[_Schema.PartCheck.fn_ValueType].Value = valueType;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartCheck(PartCheck item)
        {
            try
            {
                SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheck));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck));
                    }
                }
                sqlCtx.Params[_Schema.PartCheck.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public void AddPartCheck(PartCheck item)
        {
            try
            {
                SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheck));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = item.Customer;
                //sqlCtx.Params[_Schema.PartCheck.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PartCheck.fn_NeedCheck].Value = item.NeedCheck;
                sqlCtx.Params[_Schema.PartCheck.fn_NeedSave].Value = item.NeedSave;
                sqlCtx.Params[_Schema.PartCheck.fn_PartType].Value = item.PartType;
                sqlCtx.Params[_Schema.PartCheck.fn_ValueType].Value = item.ValueType;
                sqlCtx.Params[_Schema.PartCheck.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartCheck.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartCheck.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                item.Tracker.Clear();

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

        public void SavePartCheck(PartCheck item)
        {
            try
            {
                SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheck));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = item.Customer;
                sqlCtx.Params[_Schema.PartCheck.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PartCheck.fn_NeedCheck].Value = item.NeedCheck;
                sqlCtx.Params[_Schema.PartCheck.fn_NeedSave].Value = item.NeedSave;
                sqlCtx.Params[_Schema.PartCheck.fn_PartType].Value = item.PartType;
                sqlCtx.Params[_Schema.PartCheck.fn_ValueType].Value = item.ValueType;
                sqlCtx.Params[_Schema.PartCheck.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartCheck.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public DataTable GetMatchRuleByPartCheckID(int partCheckID)
        {
            //SELECT [RegExp],[PnExp],[PartPropertyExp],[ContainCheckBit],Editor,Cdt,Udt,[ID],[PartCheckID]    
            //  FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
            //where [PartCheckID]=partCheckID
            //order by [RegExp],[PnExp],[PartPropertyExp]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartCheckMatchRule eqcond = new _Schema.PartCheckMatchRule();
                        eqcond.PartCheckID = partCheckID;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_RegExp, _Schema.PartCheckMatchRule.fn_PnExp, _Schema.PartCheckMatchRule.fn_PartPropertyExp, _Schema.PartCheckMatchRule.fn_ContainCheckBit, _Schema.PartCheckMatchRule.fn_ID, _Schema.PartCheckMatchRule.fn_PartCheckID, _Schema.PartCheckMatchRule.fn_Editor, _Schema.PartCheckMatchRule.fn_Cdt, _Schema.PartCheckMatchRule.fn_Udt }, eqcond, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",",new string[]{_Schema.PartCheckMatchRule.fn_RegExp,_Schema.PartCheckMatchRule.fn_PnExp, _Schema.PartCheckMatchRule.fn_PartPropertyExp}));
                    }
                }
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartCheckID].Value = partCheckID;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_RegExp],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_PnExp],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_PartPropertyExp],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_ContainCheckBit],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_Editor],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_Cdt],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_Udt],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_ID],
                                                                sqlCtx.Indexes[_Schema.PartCheckMatchRule.fn_PartCheckID]});
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartCheckMatchRule(PartCheckMatchRule item)
        {
            try
            {
                SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheck));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule));
                    }
                }
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public void AddPartCheckMatchRule(PartCheckMatchRule item)
        {
            try
            {
                SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheck));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_ContainCheckBit].Value = Convert.ToInt32(item.ContainCheckBit);
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartCheckID].Value = item.PartCheckID;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartPropertyExp].Value = item.PartPropertyExp;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PnExp].Value = item.PnExp;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_RegExp].Value = item.RegExp;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_Udt].Value = cmDt;

                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));

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

        public void SavePartCheckMatchRule(PartCheckMatchRule item)
        {
            try
            {
                SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheck));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_ContainCheckBit].Value = Convert.ToInt32(item.ContainCheckBit);
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartCheckID].Value = item.PartCheckID;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartPropertyExp].Value = item.PartPropertyExp;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PnExp].Value = item.PnExp;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_RegExp].Value = item.RegExp;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartCheckMatchRule.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

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

        public DataTable GetExistPartCheckMatchRule(string regExp, string partPropertyExp, string pnExp, int partCheckID, int id)
        {
            //SELECT [ID]
            //  FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
            //Where [RegExp]='regExp' AND [PartPropertyExp]='partPropertyExp' AND [PnExp]='pnExp' AND PartCheckID='PartCheckID' AND ID<>id 
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                int i = 0;

                if (string.IsNullOrEmpty(regExp))
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_RegExp());
                else
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_RegExp(regExp));
                if (string.IsNullOrEmpty(partPropertyExp))
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PartPropertyExp());
                else
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PartPropertyExp(partPropertyExp));
                if (string.IsNullOrEmpty(pnExp))
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PnExp());
                else
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PnExp(pnExp));

                sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PartCheckID(partCheckID));
                sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_ID(id));

                sqlCtx = sqlSet.MergeToOneAndQuery();

                #region OLD
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                //    {
                //        _Schema.PartCheckMatchRule eqcond = new _Schema.PartCheckMatchRule();
                //        eqcond.RegExp = regExp;
                //        eqcond.PartPropertyExp = partPropertyExp;
                //        eqcond.PnExp = pnExp;
                //        eqcond.PartCheckID = partCheckID;
                //        _Schema.PartCheckMatchRule neqcond = new _Schema.PartCheckMatchRule();
                //        neqcond.ID = id;
                //       sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, eqcond, null, null, null, null, null, null, null, neqcond, null, null);
                //    }
                //}
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_RegExp].Value = regExp;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartPropertyExp].Value = partPropertyExp;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PnExp].Value = pnExp;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartCheckID].Value = partCheckID;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_ID].Value = id;
                #endregion
                
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetExistPartCheckMatchRule(string regExp, string partPropertyExp, string pnExp, int partCheckID)
        {
            //SELECT [ID]
            //  FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
            //Where [RegExp]='regExp' AND [PartPropertyExp]='partPropertyExp' AND [PnExp]='pnExp' AND PartCheckID='PartCheckID'
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                int i = 0;

                if (string.IsNullOrEmpty(regExp))
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_RegExp());
                else
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_RegExp(regExp));
                if (string.IsNullOrEmpty(partPropertyExp))
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PartPropertyExp());
                else
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PartPropertyExp(partPropertyExp));
                if (string.IsNullOrEmpty(pnExp))
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PnExp());
                else
                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PnExp(pnExp));

                sqlSet.AddOne(i++, ComposeForGetExistPartCheckMatchRule_PartCheckID(partCheckID));

                sqlCtx = sqlSet.MergeToOneAndQuery();

                #region OLD
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                //    {
                //        _Schema.PartCheckMatchRule eqcond = new _Schema.PartCheckMatchRule();
                //        eqcond.RegExp = regExp;
                //        eqcond.PartPropertyExp = partPropertyExp;
                //        eqcond.PnExp = pnExp;
                //        eqcond.PartCheckID = partCheckID;
                //        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, eqcond, null, null, null, null, null, null, null);
                //    }
                //}
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_RegExp].Value = regExp;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartPropertyExp].Value = partPropertyExp;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PnExp].Value = pnExp;
                //sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartCheckID].Value = partCheckID;
                #endregion

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_RegExp(string regExp)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule eqcond = new _Schema.PartCheckMatchRule();
                    eqcond.RegExp = regExp;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, eqcond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartCheckMatchRule.fn_RegExp].Value = regExp;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_RegExp()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule noecond = new _Schema.PartCheckMatchRule();
                    noecond.RegExp = string.Empty;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, null, null, null, null, null, null, null, null, noecond);
                    sqlCtx.Params[_Schema.PartCheckMatchRule.fn_RegExp].Value = noecond.RegExp;
                }
            }
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_PartPropertyExp(string partPropertyExp)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule eqcond = new _Schema.PartCheckMatchRule();
                    eqcond.PartPropertyExp = partPropertyExp;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, eqcond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartPropertyExp].Value = partPropertyExp;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_PartPropertyExp()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule noecond = new _Schema.PartCheckMatchRule();
                    noecond.PartPropertyExp = string.Empty;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, null, null, null, null, null, null, null, null, noecond);
                    sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartPropertyExp].Value = noecond.PartPropertyExp;
                }
            }
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_PnExp(string pnExp)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule eqcond = new _Schema.PartCheckMatchRule();
                    eqcond.PnExp = pnExp;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, eqcond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PnExp].Value = pnExp;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_PnExp()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule noecond = new _Schema.PartCheckMatchRule();
                    noecond.PnExp = string.Empty;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, null, null, null, null, null, null, null, null, noecond);
                    sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PnExp].Value = noecond.PnExp;
                }
            }
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_PartCheckID(int partCheckID)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule eqcond = new _Schema.PartCheckMatchRule();
                    eqcond.PartCheckID = partCheckID;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, eqcond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartCheckMatchRule.fn_PartCheckID].Value = partCheckID;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetExistPartCheckMatchRule_ID(int id)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartCheckMatchRule ncond = new _Schema.PartCheckMatchRule();
                    ncond.ID = id;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckMatchRule), null, new List<string>() { _Schema.PartCheckMatchRule.fn_ID }, null, null, null, null, null, null, null, null, ncond, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartCheckMatchRule.fn_ID].Value = id;
            return sqlCtx;
        }

        public DataTable GetCustomerPartTypeList(string customer)
        {
            //SELECT distinct [PartType] FROM [IMES_GetData_Datamaintain].[dbo].[PartCheck] WHERE [Customer]='customer' order by [PartType]
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;

                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartCheck eqcond = new _Schema.PartCheck();
                        eqcond.Customer = customer;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck), "DISTINCT", new List<string>() { _Schema.PartCheck.fn_PartType }, eqcond, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartCheck.fn_PartType);
                    }
                }
                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = customer;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IPart> GetPartList()
        {
            //所有的Flag栏位为1的PartNo，来自于Part数据表，按照part no排序。
            try
            {
                IList<IPart> ret = new List<IPart>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        //Part表加Flag
                        _Metas.Part_NEW cond = new _Metas.Part_NEW();
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Part_NEW>(tk, null, null, new ConditionCollection<_Metas.Part_NEW>(new EqualCondition<_Metas.Part_NEW>(cond)), _Metas.Part_NEW.fn_partNo);
                        sqlCtx.Param(_Metas.Part_NEW.fn_flag).Value = cond.flag;//Part表加Flag
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IPart item = new IMES.FisObject.Common.Part.Part(
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_bomNodeType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_custPartNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_descr)),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_FruNo]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Vendor]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_IECVersion]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_remark)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_autoDL)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_editor)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_cdt)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_udt)),
                                                null);
                            ((IMES.FisObject.Common.Part.Part)item).Tracker.Clear();
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

        public IPart GetPartByAssemblyCode(string strAssemblyCode)
        {
            //根据assembly code，取得part，其中包含editor, cdt, udt信息等。
            //AssemblyCode表里, 虽然AssemblyCode不是唯一的,但一个AssemblyCode只能对应一个Part Number.
            try
            {
                IPart ret = null;
                IList<fons::AssemblyCode> assemblies = this.FindAssemblyCode(strAssemblyCode);
                if (assemblies != null && assemblies.Count > 0)
                {
                    ret = this.Find_DB(assemblies[0].Pn);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistedAssemblyCode(string partNo, string family, string model, string region, string assemblyCodeId)
        {
            try
            {
                return AssRepository.CheckExistedAssemblyCode(partNo, family, model, region, assemblyCodeId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartTypeAttributeAndPartInfoValue> GetPartTypeAttributeAndPartInfoValueListByPartNo(string partNo, string partType)
        {
            //select PartTypeAttribute.Code as Item, PartTypeAttribute.Description as Description, isNull(A.InfoValue, '') as Content,
            //        A.Editor  as Editor, A.Cdt as Cdt
            //From (select PartNo, InfoType, InfoValue, Editor, Cdt from PartInfo where PartNo = ?) as A " +
            //Right Outer Join PartTypeAttribute " +
            //On A.InfoType = PartTypeAttribute.Code " +
            //where PartTypeAttribute.PartType = ? ";
            try
            {
                IList<PartTypeAttributeAndPartInfoValue> ret = new List<PartTypeAttributeAndPartInfoValue>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT {0}.{2} AS Item, {0}.{4} AS {4}, ISNULL(A.{7},'') AS Content, " +
                                            "A.{8} AS {8}, A.{9} AS {9}, A.{10}, A.{5}, A.{11} " +
                                            "FROM (SELECT {5}, {6}, {7}, {8}, {9}, {10}, {11} FROM {1} WHERE {5}=@{5}) AS A " +
                                            "RIGHT OUTER JOIN {0} ON A.{6}={0}.{2} " +
                                            "WHERE {0}.{3}=@{3} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.PartTypeAttribute).Name,
                                                                         typeof(_Schema.PartInfo).Name,
                                                                         _Schema.PartTypeAttribute.fn_Code,
                                                                         _Schema.PartTypeAttribute.fn_PartType,
                                                                         _Schema.PartTypeAttribute.fn_Description,
                                                                         _Schema.PartInfo.fn_PartNo,
                                                                         _Schema.PartInfo.fn_InfoType,
                                                                         _Schema.PartInfo.fn_InfoValue,
                                                                         _Schema.PartInfo.fn_Editor,
                                                                         _Schema.PartInfo.fn_Cdt,
                                                                         _Schema.PartInfo.fn_ID,
                                                                         _Schema.PartInfo.fn_Udt);

                        sqlCtx.Params.Add(_Schema.PartTypeAttribute.fn_PartType, new SqlParameter("@" + _Schema.PartTypeAttribute.fn_PartType, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.PartInfo.fn_PartNo, new SqlParameter("@" + _Schema.PartInfo.fn_PartNo, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = partType;
                sqlCtx.Params[_Schema.PartInfo.fn_PartNo].Value = partNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartTypeAttributeAndPartInfoValue item = new PartTypeAttributeAndPartInfoValue();
                            item.Item = GetValue_Str(sqlR, 0);
                            item.Description = GetValue_Str(sqlR, 1);
                            item.Content = GetValue_Str(sqlR, 2);
                            item.Editor = GetValue_Str(sqlR, 3);
                            item.Cdt = GetValue_DateTime(sqlR, 4);
                            item.Id = GetValue_Int32(sqlR, 5);
                            item.MainTblKey = GetValue_Str(sqlR, 6);
                            item.Udt = GetValue_DateTime(sqlR, 7);
                            ret.Add(item);
                        }
                    }
                }
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IList<PartTypeAttributeAndPartInfoValue> GetPartTypeAttributeAndPartInfoValueListByAssemblyCode(string assemblyCode, string partType)
        {
            //select PartTypeAttribute.Code as Item, PartTypeAttribute.Description as Description, isNull(A.InfoValue, '') as Content, 
            //        A.Editor as Editor, A.Cdt as Cdt
            //From (select AssemblyCode, InfoType, InfoValue, Editor, Cdt from AssemblyCodeInfo where AssemblyCode = ?) as A 
            //Right Outer Join PartTypeAttribute 
            //On A.InfoType = PartTypeAttribute.Code
            //where PartTypeAttribute.PartType = ?
            try
            {
                IList<PartTypeAttributeAndPartInfoValue> ret = new List<PartTypeAttributeAndPartInfoValue>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT {0}.{2} AS Item, {0}.{4} AS {4}, ISNULL(A.{7}, '') AS Content, " +
                                            "A.{8} AS {8}, A.{9} AS {9}, A.{10}, A.{5}, A.{11} " +
                                            "FROM (SELECT {5}, {6}, {7}, {8}, {9}, {10}, {11} FROM {1} WHERE {5}=@{5}) AS A " +
                                            "RIGHT OUTER JOIN {0} ON A.{6}={0}.{2} " +
                                            "WHERE {0}.{3}=@{3} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence,typeof(_Schema.PartTypeAttribute).Name,
                                                                        typeof(_Schema.AssemblyCodeInfo).Name,
                                                                        _Schema.PartTypeAttribute.fn_Code,
                                                                        _Schema.PartTypeAttribute.fn_PartType,
                                                                        _Schema.PartTypeAttribute.fn_Description,
                                                                        _Schema.AssemblyCodeInfo.fn_AssemblyCode,
                                                                        _Schema.AssemblyCodeInfo.fn_InfoType,
                                                                        _Schema.AssemblyCodeInfo.fn_InfoValue,
                                                                        _Schema.AssemblyCodeInfo.fn_Editor,
                                                                        _Schema.AssemblyCodeInfo.fn_Cdt,
                                                                        _Schema.AssemblyCodeInfo.fn_ID,
                                                                        _Schema.AssemblyCodeInfo.fn_Udt);

                        sqlCtx.Params.Add(_Schema.PartTypeAttribute.fn_PartType, new SqlParameter("@" + _Schema.PartTypeAttribute.fn_PartType, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.AssemblyCodeInfo.fn_AssemblyCode, new SqlParameter("@" + _Schema.AssemblyCodeInfo.fn_AssemblyCode, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = partType;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_AssemblyCode].Value = assemblyCode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartTypeAttributeAndPartInfoValue item = new PartTypeAttributeAndPartInfoValue();
                            item.Item = GetValue_Str(sqlR, 0);
                            item.Description = GetValue_Str(sqlR, 1);
                            item.Content = GetValue_Str(sqlR, 2);
                            item.Editor = GetValue_Str(sqlR, 3);
                            item.Cdt = GetValue_DateTime(sqlR, 4);
                            item.Id = GetValue_Int32(sqlR, 5);
                            item.MainTblKey = GetValue_Str(sqlR, 6);
                            item.Udt = GetValue_DateTime(sqlR, 7);
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

        public void AddAssemblyCodeInfo(AssemblyCodeInfo item)
        {
            try
            {
                AssRepository.AddAssemblyCodeInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveAssemblyCodeInfo(AssemblyCodeInfo item)
        {
            try
            {
                AssRepository.SaveAssemblyCodeInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAssemblyCodeInfo(AssemblyCodeInfo item)
        {
            try
            {
                AssRepository.DeleteAssemblyCodeInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.Common.Part.Region> GetRegionList()
        {
            try
            {
                IList<IMES.FisObject.Common.Part.Region> ret = new List<IMES.FisObject.Common.Part.Region>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Region));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Region.fn_region);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMES.FisObject.Common.Part.Region item = new IMES.FisObject.Common.Part.Region();
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Region.fn_Cdt]);
                            item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Region.fn_Description]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Region.fn_Editor]);
                            item.region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Region.fn_region]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Region.fn_Udt]);
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

        public void AddRegion(IMES.FisObject.Common.Part.Region region)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Region));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Region.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Region.fn_Description].Value = region.Description;
                sqlCtx.Params[_Schema.Region.fn_Editor].Value = region.Editor;
                sqlCtx.Params[_Schema.Region.fn_region].Value = region.region;
                sqlCtx.Params[_Schema.Region.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateRegion(IMES.FisObject.Common.Part.Region region, string regionname)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Region cond = new _Schema.Region();
                        cond.region = regionname;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Region), null, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Region.fn_region].Value = regionname;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Region.fn_Description)].Value = region.Description;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Region.fn_Editor)].Value = region.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Region.fn_region)].Value = region.region;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Region.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteRegionByName(string region)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Region));
                    }
                }
                sqlCtx.Params[_Schema.Region.fn_region].Value = region;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IFRegionIsExists(string region)
        {
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Region cond = new _Schema.Region();
                        cond.region = region;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Region), "COUNT", new List<string>() { _Schema.Region.fn_region }, cond, null,null,null,null,null,null,null);
                    }
                }
                sqlCtx.Params[_Schema.Region.fn_region].Value = region;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
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

        public bool IFRegionIsInUse(string region)
        {
            //select * from Model where Region=?
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model cond = new _Schema.Model();
                        cond.Region = region;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), "COUNT", new List<string>() { _Schema.Model.fn_model }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Region.fn_region].Value = region;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
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

        public void AddPartType(IMES.FisObject.Common.Part.PartType item)
        {
            try
            {
                //SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.PartType));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeEx));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartTypeEx.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartTypeEx.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartTypeEx.fn_partType].Value = item.PartTypeName;
                sqlCtx.Params[_Schema.PartTypeEx.fn_PartTypeGroup].Value = item.PartTypeGroup;
                sqlCtx.Params[_Schema.PartTypeEx.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                //SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.Dispose();
                //SqlTransactionManager.End();
            }
        }

        public void SavePartType(IMES.FisObject.Common.Part.PartType item, string strOldPartType)
        {
            try
            {
                //SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal((string)item.Key, CacheType.PartType));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeEx cond = new _Schema.PartTypeEx();
                        cond.partType = strOldPartType;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeEx), new List<string>() { _Schema.PartTypeEx.fn_Editor, _Schema.PartTypeEx.fn_partType, _Schema.PartTypeEx.fn_PartTypeGroup }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeEx.fn_partType].Value = strOldPartType;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeEx.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeEx.fn_partType)].Value = item.PartTypeName;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeEx.fn_PartTypeGroup)].Value = item.PartTypeGroup;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeEx.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                //SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.Dispose();
                //SqlTransactionManager.End();
            }
        }

        public void DeletePartTypeByPartType(string partType)
        {
            try
            {
                //SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartType));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeEx cond = new _Schema.PartTypeEx();
                        cond.partType = partType;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeEx), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeEx.fn_partType].Value = partType;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                // SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.Dispose();
                //SqlTransactionManager.End();
            }
        }

        public void DeletePartTypeByPartType(int id)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @" delete from PartTypeEx                                                                                                     
                                                            where ID=@ID";

                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["ID"].Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

        }

        public void DeletePartTypeAttAndDescByPartType(string partType)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @" delete from PartTypeAttribute 
                                                            where PartType = @PartType

                                                            delete from PartTypeDescription 
                                                            where PartType = @PartType ";

                        sqlCtx.Params.Add("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["PartType"].Value = partType;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

        }

        public void SavePartType(IMES.FisObject.Common.Part.PartType item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"update PartTypeEx
                                                            set PartType =@PartType,
                                                                PartTypeGroup = @PartTypeGroup,
                                                                Editor = @Editor,
                                                                Udt =@Now
                                                            where ID=@ID";

                        sqlCtx.Params.Add("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("PartTypeGroup", new SqlParameter("@PartTypeGroup", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));
                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["PartType"].Value = item.PartTypeName;
                sqlCtx.Params["PartTypeGroup"].Value = item.PartTypeGroup;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["Now"].Value = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params["ID"].Value = item.ID;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());

            }
            catch (Exception)
            {

                throw;
            }

        }



        public IList<PartTypeAttribute> GetPartTypeAttributes(string strPartType)
        {
            //根据PartType取得PartTypeAttribute表对应的记录
            //栏位包括Code和Description、Editor、Cdt、Udt
            //按Code列的字符序排序
            try
            {
                IList<PartTypeAttribute> ret = new List<PartTypeAttribute>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeAttribute cond = new _Schema.PartTypeAttribute();
                        cond.PartType = strPartType;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeAttribute), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartTypeAttribute.fn_Code);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = strPartType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartTypeAttribute pta = new PartTypeAttribute();
                            pta.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeAttribute.fn_Cdt]);
                            pta.Code = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeAttribute.fn_Code]);
                            pta.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeAttribute.fn_Description]);
                            pta.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeAttribute.fn_Editor]);
                            pta.PartType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeAttribute.fn_PartType]);
                            pta.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeAttribute.fn_Udt]);
                            ret.Add(pta);
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

        public void AddPartTypeAttribute(PartTypeAttribute item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeAttribute));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_Code].Value = item.Code;
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = item.PartType;
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SavePartTypeAttribute(PartTypeAttribute item, string strOldPartType, string strOldCode)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeAttribute cond = new _Schema.PartTypeAttribute();
                        cond.PartType = strOldPartType;
                        cond.Code = strOldCode;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeAttribute), null, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = strOldPartType;
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_Code].Value = strOldCode;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeAttribute.fn_Code)].Value = item.Code;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeAttribute.fn_Description)].Value = item.Description;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeAttribute.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeAttribute.fn_PartType)].Value = item.PartType;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PartTypeAttribute.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartTypeAttribute(string strPartType, string strCode)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeAttribute cond = new _Schema.PartTypeAttribute();
                        cond.PartType = strPartType;
                        cond.Code = strCode;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeAttribute), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_PartType].Value = strPartType;
                sqlCtx.Params[_Schema.PartTypeAttribute.fn_Code].Value = strCode;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPartTypeDesc(PartTypeDescription item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeDescription));
                    }
                }
                sqlCtx.Params[_Schema.PartTypeDescription.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.PartTypeDescription.fn_PartType].Value = item.PartType;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SavePartTypeDesc(PartTypeDescription item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeDescription));
                    }
                }
                sqlCtx.Params[_Schema.PartTypeDescription.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PartTypeDescription.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.PartTypeDescription.fn_PartType].Value = item.PartType;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartTypeDesc(PartTypeDescription item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeDescription));
                    }
                }
                sqlCtx.Params[_Schema.PartTypeDescription.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistedDesc(string strPartType, string strDesc, string strDescID)
        {
            //如果strDescID == "", where 子句不包含该查询条件
            //否则where 中包含条件 PartTypeDescription.ID != strDescID
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx  = null;

                _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                int i = 0;
                if (!string.IsNullOrEmpty(strDescID))
                {
                    sqlSet.AddOne(i++, ComposeForCheckExistedDesc_strDescID(Convert.ToInt32(strDescID)));
                }
                sqlSet.AddOne(i++, ComposeForCheckExistedDesc_Other(strPartType, strDesc));

                sqlCtx = sqlSet.MergeToOneAndQuery();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        private _Schema.SQLContext ComposeForCheckExistedDesc_Other(string strPartType, string strDesc)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartTypeDescription cond = new _Schema.PartTypeDescription();
                    cond.PartType = strPartType;
                    cond.Description = strDesc;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeDescription), "COUNT", new List<string>() { _Schema.PartTypeDescription.fn_ID }, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartTypeDescription.fn_PartType].Value = strPartType;
            sqlCtx.Params[_Schema.PartTypeDescription.fn_Description].Value = strDesc;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedDesc_strDescID(int strDescID)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartTypeDescription neqcond = new _Schema.PartTypeDescription();
                    neqcond.ID = strDescID;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeDescription), "COUNT", new List<string>() { _Schema.PartTypeDescription.fn_ID }, null, null, null, null, null, null, null, null, neqcond, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartTypeDescription.fn_ID].Value = strDescID;
            return sqlCtx;
        }

        public IList<PartTypeMapping> GetPartTypeMappingList(string strPartType)
        {
            //取得PartTypeMapping表的PartTypeMapping.FISType = strPartType记录
            //按字符序排序
            try
            {
                IList<PartTypeMapping> ret = new List<PartTypeMapping>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartTypeMapping cond = new _Schema.PartTypeMapping();
                        cond.FISType = strPartType;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeMapping), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartTypeMapping.fn_SAPType);
                    }
                }
                sqlCtx.Params[_Schema.PartTypeMapping.fn_FISType].Value = strPartType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartTypeMapping ptm = new PartTypeMapping();
                            ptm.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeMapping.fn_Cdt]);
                            ptm.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeMapping.fn_Editor]);
                            ptm.FISType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeMapping.fn_FISType]);
                            ptm.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartTypeMapping.fn_ID]);
                            ptm.SAPType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartTypeMapping.fn_SAPType]);
                            ptm.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartTypeMapping.fn_Udt]);
                            ret.Add(ptm);
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

        public void AddPartTypeMapping(PartTypeMapping item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeMapping));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartTypeMapping.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartTypeMapping.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartTypeMapping.fn_FISType].Value = item.FISType;
                sqlCtx.Params[_Schema.PartTypeMapping.fn_SAPType].Value = item.SAPType;
                sqlCtx.Params[_Schema.PartTypeMapping.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SavePartTypeMapping(PartTypeMapping item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeMapping));
                    }
                }
                sqlCtx.Params[_Schema.PartTypeMapping.fn_ID].Value = item.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartTypeMapping.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartTypeMapping.fn_FISType].Value = item.FISType;
                sqlCtx.Params[_Schema.PartTypeMapping.fn_SAPType].Value = item.SAPType;
                sqlCtx.Params[_Schema.PartTypeMapping.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartTypeMapping(PartTypeMapping item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeMapping));
                    }
                }
                sqlCtx.Params[_Schema.PartTypeMapping.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistedSAPType(string strFISType, string strSAPType, string strID)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                int i = 0;
                if (!string.IsNullOrEmpty(strID))
                {
                    sqlSet.AddOne(i++, ComposeForCheckExistedSAPType_strID(Convert.ToInt32(strID)));
                }
                sqlSet.AddOne(i++, ComposeForCheckExistedSAPType_Other(strFISType, strSAPType));

                sqlCtx = sqlSet.MergeToOneAndQuery();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        private _Schema.SQLContext ComposeForCheckExistedSAPType_Other(string strFISType, string strSAPType)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartTypeMapping cond = new _Schema.PartTypeMapping();
                    cond.FISType = strFISType;
                    cond.SAPType = strSAPType;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeMapping), "COUNT", new List<string>() { _Schema.PartTypeMapping.fn_ID }, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartTypeMapping.fn_FISType].Value = strFISType;
            sqlCtx.Params[_Schema.PartTypeMapping.fn_SAPType].Value = strSAPType;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedSAPType_strID(int strID)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PartTypeMapping neqcond = new _Schema.PartTypeMapping();
                    neqcond.ID = strID;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartTypeMapping), "COUNT", new List<string>() { _Schema.PartTypeMapping.fn_ID }, null, null, null, null, null, null, null, null, neqcond, null, null);
                }
            }
            sqlCtx.Params[_Schema.PartTypeMapping.fn_ID].Value = strID;
            return sqlCtx;
        }

        public IList<string> GetFamilyListForIECVersion()
        {
            //SELECT DISTINCT Descr as Family
            //FROM IMES_GetData..Part 
            //WHERE LEFT(PartNo, 3) = '111'
            //AND PartType IN ('MB', 'VB', 'SB')
            //ORDER BY Family
            //
            //现在要变成
            //SA的Family仍然从Part表中取Description，但对应的PartNo改为以“131”开头，并且PartType只能是“MB”
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Part likeCond = new _Schema.Part();
                        likeCond.PartNo = "131%";
                        _Schema.Part insetCond = new _Schema.Part();
                        insetCond.PartType = "INSET";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), "DISTINCT", new List<string>() { _Schema.Part.fn_Descr }, null, likeCond, null, null, null, null, null, insetCond);

                        sqlCtx.Params[_Schema.Part.fn_PartNo].Value = "131%";

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.Part.fn_PartType), _Schema.Func.ConvertInSet(new List<string>(new string[]{"MB"}))); //MB.MBType.GetAllTypes())));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Part.fn_Descr);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Descr]);
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

        public IList<PartForbidden> GetPartForbiddenListByFamily(string family)
        {
            //Model、Description、Part No、Assembly Code
            try
            {
                IList<PartForbidden> ret = new List<PartForbidden>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartForbidden cond = new _Schema.PartForbidden();
                        cond.Family = family;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[]{_Schema.PartForbidden.fn_Model, _Schema.PartForbidden.fn_Descr, _Schema.PartForbidden.fn_PartNo, _Schema.PartForbidden.fn_AssemblyCode }));
                    }
                }
                sqlCtx.Params[_Schema.PartForbidden.fn_Family].Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartForbidden item = new PartForbidden(
                                GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_ID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Family]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Model]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Descr]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_PartNo]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_AssemblyCode]),
                                GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Status]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Editor]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Cdt]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Udt])
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

        public int CheckExistedPartForbidden(string strModel, string strDescription, string strPartNo, string strAssemblyCode, string family)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartForbidden cond = new _Schema.PartForbidden();
                        cond.Model = strModel;
                        cond.Descr = strDescription;
                        cond.PartNo = strPartNo;
                        cond.AssemblyCode = strAssemblyCode;
                        cond.Family = family;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), "COUNT", new List<string>() { _Schema.PartForbidden.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartForbidden.fn_Model].Value = strModel;
                sqlCtx.Params[_Schema.PartForbidden.fn_Descr].Value = strDescription;
                sqlCtx.Params[_Schema.PartForbidden.fn_PartNo].Value = strPartNo;
                sqlCtx.Params[_Schema.PartForbidden.fn_AssemblyCode].Value = strAssemblyCode;
                sqlCtx.Params[_Schema.PartForbidden.fn_Family].Value = family;
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

        public void AddPartForbidden(PartForbidden item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartForbidden.fn_AssemblyCode].Value = item.AssemblyCode;
                sqlCtx.Params[_Schema.PartForbidden.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartForbidden.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.PartForbidden.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartForbidden.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.PartForbidden.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.PartForbidden.fn_PartNo].Value = item.PartNo;
                sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.PartForbidden.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SavePartForbidden(PartForbidden item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden));
                    }
                }
                sqlCtx.Params[_Schema.PartForbidden.fn_ID].Value = item.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartForbidden.fn_AssemblyCode].Value = item.AssemblyCode;
                sqlCtx.Params[_Schema.PartForbidden.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.PartForbidden.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PartForbidden.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.PartForbidden.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.PartForbidden.fn_PartNo].Value = item.PartNo;
                sqlCtx.Params[_Schema.PartForbidden.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.PartForbidden.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartForbidden(PartForbidden item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden));
                    }
                }
                sqlCtx.Params[_Schema.PartForbidden.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PartForbidden GetPartForbidden(int id)
        {
            try
            {
                PartForbidden ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartForbidden cond = new _Schema.PartForbidden();
                        cond.ID = id;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartForbidden), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartForbidden.fn_ID].Value = id;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = new PartForbidden(
                                GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_ID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Family]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Model]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Descr]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_PartNo]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_AssemblyCode]),
                                GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Status]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Editor]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Cdt]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartForbidden.fn_Udt])
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

        public IPart GetPartByPartNo(string partNo)
        {
            try
            {
                IPart ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Part_NEW cond = new _Metas.Part_NEW();
                        cond.partNo = partNo;
                        _Metas.Part_NEW neqCond = new _Metas.Part_NEW();
                        neqCond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Part_NEW>(tk, null, null, new ConditionCollection<_Metas.Part_NEW>(
                            new EqualCondition<_Metas.Part_NEW>(cond),
                            new NotEqualCondition<_Metas.Part_NEW>(neqCond)
                            ));
                        sqlCtx.Param(_Metas.Part_NEW.fn_flag).Value = 0;
                    }
                }
                sqlCtx.Param(_Metas.Part_NEW.fn_partNo).Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new IMES.FisObject.Common.Part.Part(
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_bomNodeType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partType)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_custPartNo)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_descr)),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_FruNo]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_Vendor]),
                                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_IECVersion]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_remark)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_autoDL)),
                                                GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_editor)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_cdt)),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_udt)),
                                                null);
                        ((IMES.FisObject.Common.Part.Part)ret).Tracker.Clear();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartNoList()
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Part.fn_PartNo);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Part.fn_PartNo]);
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

        public IList<string> GetPartInfoList()
        {
            // select Distinct InfoType from PartInfo
            // 按InfoType列的字符序排序
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo), "DISTINCT", new List<string>() { _Schema.PartInfo.fn_InfoType }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartInfo.fn_InfoType);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_InfoType]);
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

        public void AddAssemblyCodeDefered(IUnitOfWork uow, fons::AssemblyCode item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateAssemblyCodeDefered(IUnitOfWork uow, fons::AssemblyCode item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteAssemblyCodeDefered(IUnitOfWork uow, int assemblyCodeId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), assemblyCodeId);
        }

        public void DeletePartCheckDefered(IUnitOfWork uow, PartCheck item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPartCheckDefered(IUnitOfWork uow, PartCheck item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePartCheckDefered(IUnitOfWork uow, PartCheck item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePartCheckMatchRuleDefered(IUnitOfWork uow, PartCheckMatchRule item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPartCheckMatchRuleDefered(IUnitOfWork uow, PartCheckMatchRule item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePartCheckMatchRuleDefered(IUnitOfWork uow, PartCheckMatchRule item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SaveAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddRegionDefered(IUnitOfWork uow, IMES.FisObject.Common.Part.Region region)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), region);
        }

        public void UpdateRegionDefered(IUnitOfWork uow, IMES.FisObject.Common.Part.Region region, string regionname)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), region, regionname);
        }

        public void DeleteRegionByNameDefered(IUnitOfWork uow, string region)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), region);
        }

        public void AddPartTypeDefered(IUnitOfWork uow, IMES.FisObject.Common.Part.PartType item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePartTypeDefered(IUnitOfWork uow, IMES.FisObject.Common.Part.PartType item, string strOldPartType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, strOldPartType);
        }

        public void DeletePartTypeByPartTypeDefered(IUnitOfWork uow, string partType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partType);
        }

        public void AddPartTypeAttributeDefered(IUnitOfWork uow, PartTypeAttribute item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePartTypeAttributeDefered(IUnitOfWork uow, PartTypeAttribute item, string strOldPartType, string strOldCode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, strOldPartType, strOldCode);
        }

        public void DeletePartTypeAttributeDefered(IUnitOfWork uow, string strPartType, string strCode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), strPartType, strCode);
        }

        public void AddPartTypeDescDefered(IUnitOfWork uow, PartTypeDescription item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePartTypeDescDefered(IUnitOfWork uow, PartTypeDescription item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePartTypeDescDefered(IUnitOfWork uow, PartTypeDescription item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPartTypeMappingDefered(IUnitOfWork uow, PartTypeMapping item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePartTypeMappingDefered(IUnitOfWork uow, PartTypeMapping item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePartTypeMappingDefered(IUnitOfWork uow, PartTypeMapping item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPartForbiddenDefered(IUnitOfWork uow, PartForbidden item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePartForbiddenDefered(IUnitOfWork uow, PartForbidden item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePartForbiddenDefered(IUnitOfWork uow, PartForbidden item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #endregion

        #region AstRule  table
        public DataTable GetAssetCheckRuleList()
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
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.AstRule>(tk, null, new string[][]{
                         new string[]{_Metas.AstRule.fn_code, string.Format("{0} AS [Ast Type]", _Metas.AstRule.fn_code)},
                         new string[]{_Metas.AstRule.fn_checkTp, string.Format("{0} AS [Check Type]", _Metas.AstRule.fn_checkTp)}, 
                         new string[]{_Metas.AstRule.fn_station, string.Format("CASE WHEN {0}='65' THEN '65 After MVS' WHEN {0}='8C' THEN '8C Combine Pizza' WHEN {0}='85' THEN '85 Unit Weight' END AS [Check Station]", _Metas.AssetRule.fn_station)},
                         new string[]{_Metas.AstRule.fn_custName, string.Format("{0} AS [Cust Name]", _Metas.AstRule.fn_custName)}, 
                         new string[]{_Metas.AstRule.fn_checkItem, string.Format("{0} AS [Check Item]", _Metas.AstRule.fn_checkItem)}, 
                         new string[]{_Metas.AstRule.fn_editor},
                         new string[]{_Metas.AstRule.fn_cdt},
                         new string[]{_Metas.AstRule.fn_udt},
                         new string[]{_Metas.AstRule.fn_id}//,
                         //new string[]{_Metas.AstRule.fn_station}
                         },
                        new ConditionCollection<_Metas.AstRule>(),
                        _Metas.AstRule.fn_code);
                    }
                }
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IList<DataModel.AstRuleInfo> GetAstRule()
        {
            try
            {
                IList<DataModel.AstRuleInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<_Metas.AstRule>(tk, _Metas.AstRule.fn_station, _Metas.AstRule.fn_code);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.AstRule, DataModel.AstRuleInfo, DataModel.AstRuleInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IList<DataModel.AstRuleInfo> GetAstRuleByCondition(DataModel.AstRuleInfo condition)
        {
            IList<DataModel.AstRuleInfo> ret = null;

           
            SQLContextNew sqlCtx = null;

            _Metas.AstRule cond = FuncNew.SetColumnFromField<_Metas.AstRule, DataModel.AstRuleInfo>(condition);
            sqlCtx = FuncNew.GetConditionedSelect<_Metas.AstRule>(null, null, new ConditionCollection<_Metas.AstRule>(new EqualCondition<_Metas.AstRule>(cond)), _Metas.AstRule.fn_station, _Metas.AstRule.fn_code);

            sqlCtx = FuncNew.SetColumnFromField<_Metas.AstRule, DataModel.AstRuleInfo>(sqlCtx, condition);

            using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
            {
                ret = FuncNew.SetFieldFromColumn<_Metas.AstRule, DataModel.AstRuleInfo, DataModel.AstRuleInfo>(ret, sqlR, sqlCtx);
            }
            return ret;           
        }

        /// <summary>
        /// insert AssetRule
        /// </summary>
        /// <param name="item"></param>
        public void AddAstRule(AstRuleInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<_Metas.AstRule>(tk);
                    }
                }

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<_Metas.AstRule, DataModel.AstRuleInfo>(sqlCtx, item);

                sqlCtx.Param(_Metas.AstRule.fn_cdt).Value = cmDt;
                sqlCtx.Param(_Metas.AstRule.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// update AstRue
        /// </summary>
        /// <param name="item"></param>
        public void UpdateAstRule(AstRuleInfo item)
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
                        _Metas.AstRule cond = new _Metas.AstRule();
                        cond.id = item.id;
                        _Metas.AstRule setv = FuncNew.SetColumnFromField<_Metas.AstRule, AstRuleInfo>(item, _Metas.AstRule.fn_id);
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.AstRule>(tk, 
                                                                                                                    new SetValueCollection<_Metas.AstRule>(new CommonSetValue<_Metas.AstRule>(setv)), 
                                                                                                                    new ConditionCollection<_Metas.AstRule>(new EqualCondition<_Metas.AstRule>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.AstRule.fn_id).Value = item.id;

                sqlCtx = FuncNew.SetColumnFromField<_Metas.AstRule, AstRuleInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.AstRule.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAstRule(int id)
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
                        _Metas.AstRule cond = new _Metas.AstRule();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.AstRule>(tk, new ConditionCollection<_Metas.AstRule>(new EqualCondition<_Metas.AstRule>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.AstRule.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public void DeleteAssetCheckRule(int id)
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
                        _Metas.AssetRule cond = new _Metas.AssetRule();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.AssetRule>(tk, new ConditionCollection<_Metas.AssetRule>(new EqualCondition<_Metas.AssetRule>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.AssetRule.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetAstTypes(string nodeType)
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
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = nodeType;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<Part_NEW>(tk, "DISTINCT", new string[] { Part_NEW.fn_descr }, new ConditionCollection<Part_NEW>(new EqualCondition<Part_NEW>(cond)), Part_NEW.fn_descr);

                        sqlCtx.Param(Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(Part_NEW.fn_bomNodeType).Value = nodeType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string result = g.GetValue_Str(sqlR, 0);
                            ret.Add(result);
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

        public void AddAssetRule(DataModel.AssetRule item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<_Metas.AssetRule>(tk);
                    }
                }

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<_Metas.AssetRule, DataModel.AssetRule>(sqlCtx, item);

                sqlCtx.Param(_Metas.AssetRule.fn_cdt).Value = cmDt;
                sqlCtx.Param(_Metas.AssetRule.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExistAssetRule(DataModel.AssetRule item)
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
                         _Metas.AssetRule cond = new _Metas.AssetRule();
                        cond.astType = item.astType;
                        cond.station = item.station;
                        cond.checkingType = item.checkingType;
                        cond.custName = item.custName;
                        cond.checkItem = item.checkItem;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.AssetRule>(tk, null, new string[] { _Metas.AssetRule.fn_id },
                        new ConditionCollection<_Metas.AssetRule>(new EqualCondition<_Metas.AssetRule>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.AssetRule.fn_astType).Value = item.astType;
                sqlCtx.Param(_Metas.AssetRule.fn_station).Value = item.station;
                sqlCtx.Param(_Metas.AssetRule.fn_checkingType).Value = item.checkingType;
                sqlCtx.Param(_Metas.AssetRule.fn_custName).Value = item.custName;
                sqlCtx.Param(_Metas.AssetRule.fn_checkItem).Value = item.checkItem;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public IList<OlymBatteryInfo> GetBatteryInfoList()
        {
            try
            {
                IList<OlymBatteryInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<OlymBattery>(tk, OlymBattery.fn_hppn);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<OlymBattery, OlymBatteryInfo, OlymBatteryInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<OlymBatteryInfo> GetBatteryInfoList(string batteryVC)
        {
            try
            {
                IList<OlymBatteryInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        OlymBattery cond = new OlymBattery();
                        cond.hppn = batteryVC + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<OlymBattery>(tk, null, null, new ConditionCollection<OlymBattery>(new LikeCondition<OlymBattery>(cond)), OlymBattery.fn_hppn);
                    }
                }

                sqlCtx.Param(OlymBattery.fn_hppn).Value = batteryVC + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<OlymBattery, OlymBatteryInfo, OlymBatteryInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<OlymBatteryInfo> GetExistBattery(string batteryVC)
        {
            try
            {
                IList<OlymBatteryInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        OlymBattery cond = new OlymBattery();
                        cond.hppn = batteryVC;
                        sqlCtx = FuncNew.GetConditionedSelect<OlymBattery>(tk, null, null, new ConditionCollection<OlymBattery>(new EqualCondition<OlymBattery>(cond)));
                    }
                }
                sqlCtx.Param(OlymBattery.fn_hppn).Value = batteryVC;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<OlymBattery, OlymBatteryInfo, OlymBatteryInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ChangeBattery(OlymBatteryInfo item, string oldBattery)
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
                        OlymBattery cond = new OlymBattery();
                        cond.hppn = oldBattery;
                        OlymBattery setv = FuncNew.SetColumnFromField<OlymBattery, OlymBatteryInfo>(item);
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<OlymBattery>(tk, new SetValueCollection<OlymBattery>(new CommonSetValue<OlymBattery>(setv)), new ConditionCollection<OlymBattery>(new EqualCondition<OlymBattery>(cond)));
                    }
                }
                sqlCtx.Param(OlymBattery.fn_hppn).Value = oldBattery;

                sqlCtx = FuncNew.SetColumnFromField<OlymBattery, OlymBatteryInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(OlymBattery.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddBattery(OlymBatteryInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<OlymBattery>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<OlymBattery, OlymBatteryInfo>(sqlCtx, item);

                sqlCtx.Param(OlymBattery.fn_cdt).Value = cmDt;
                sqlCtx.Param(OlymBattery.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveBattery(OlymBatteryInfo item)
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
                        OlymBattery cond = new OlymBattery();
                        cond.id = item.id;
                        sqlCtx = FuncNew.GetConditionedDelete<OlymBattery>(tk, new ConditionCollection<OlymBattery>(new EqualCondition<OlymBattery>(cond)));
                    }
                }
                sqlCtx.Param(OlymBattery.fn_id).Value = item.id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OlymBatteryInfo FindBattery(string batteryVC)
        {
            try
            {
                OlymBatteryInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        OlymBattery cond = new OlymBattery();
                        cond.hppn = batteryVC;
                        sqlCtx = FuncNew.GetConditionedSelect<OlymBattery>(tk, null, null, new ConditionCollection<OlymBattery>(new EqualCondition<OlymBattery>(cond)));
                    }
                }
                sqlCtx.Param(OlymBattery.fn_hppn).Value = batteryVC;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<OlymBattery, OlymBatteryInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void DeleteAssetCheckRuleDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddAssetRuleDefered(IUnitOfWork uow, DataModel.AssetRule item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }
 
        public void ChangeBatteryDefered(IUnitOfWork uow, OlymBatteryInfo item, string oldBattery)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldBattery);
        }

        public void AddBatteryDefered(IUnitOfWork uow, OlymBatteryInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void RemoveBatteryDefered(IUnitOfWork uow, OlymBatteryInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        public IList<InternalCOAInfo> FindAllInternalCOA()
        {
            try
            {
                IList<InternalCOAInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<InternalCOA>(tk, InternalCOA.fn_type, InternalCOA.fn_code);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<InternalCOA, InternalCOAInfo, InternalCOAInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<InternalCOAInfo> GetExistInternalCOA(string code)
        {
            try
            {
                IList<InternalCOAInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        InternalCOA cond = new InternalCOA();
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<InternalCOA>(tk, null, null, new ConditionCollection<InternalCOA>(new EqualCondition<InternalCOA>(cond)));
                    }
                }
                sqlCtx.Param(InternalCOA.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<InternalCOA, InternalCOAInfo, InternalCOAInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddInternalCOA(InternalCOAInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<InternalCOA>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<InternalCOA, InternalCOAInfo>(sqlCtx, item);

                sqlCtx.Param(InternalCOA.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveInternalCOA(InternalCOAInfo item)
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
                        InternalCOA cond = new InternalCOA();
                        cond.id = item.id;
                        sqlCtx = FuncNew.GetConditionedDelete<InternalCOA>(tk, new ConditionCollection<InternalCOA>(new EqualCondition<InternalCOA>(cond)));
                    }
                }
                sqlCtx.Param(InternalCOA.fn_id).Value = item.id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public InternalCOAInfo FindInternalCOA(int id)
        {
            try
            {
                InternalCOAInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        InternalCOA cond = new InternalCOA();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedSelect<InternalCOA>(tk, null, null, new ConditionCollection<InternalCOA>(new EqualCondition<InternalCOA>(cond)));
                    }
                }
                sqlCtx.Param(InternalCOA.fn_id).Value = id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<InternalCOA, InternalCOAInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PODLabelPartDef> GetPODLabelPartListByPartNo(string partNo)
        {
            try
            {
                IList<PODLabelPartDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Podlabelpart cond = new Podlabelpart();
                        cond.partNo = partNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Podlabelpart>(tk, null, null, new ConditionCollection<Podlabelpart>(new AnyCondition<Podlabelpart>(cond, "CHARINDEX(RTRIM({0}),{1})=1")), Podlabelpart.fn_family);
                    }
                }
                sqlCtx.Param(g.DecAny(Podlabelpart.fn_partNo)).Value = partNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Podlabelpart, PODLabelPartDef, PODLabelPartDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PODLabelPartDef> GetListByPartNoAndFamily(string partNo, string family)
        {
            try
            {
                IList<PODLabelPartDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Podlabelpart cond = new Podlabelpart();
                        cond.partNo = partNo;
                        Podlabelpart cond2 = new Podlabelpart();
                        cond2.family = family;
                        sqlCtx = FuncNew.GetConditionedSelect<Podlabelpart>(tk, null, null, new ConditionCollection<Podlabelpart>(new AnyCondition<Podlabelpart>(cond, "CHARINDEX(RTRIM({0}),{1})=1"), new EqualCondition<Podlabelpart>(cond2)));
                    }
                }
                sqlCtx.Param(g.DecAny(Podlabelpart.fn_partNo)).Value = partNo;
                sqlCtx.Param(Podlabelpart.fn_family).Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Podlabelpart, PODLabelPartDef, PODLabelPartDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PODLabelPartDef> GetPodLabelPartListByPartNoLike(string model)
        {
            try
            {
                IList<PODLabelPartDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Podlabelpart cond = new Podlabelpart();
                        cond.partNo = model;
                        sqlCtx = FuncNew.GetConditionedSelect<Podlabelpart>(tk, null, null, new ConditionCollection<Podlabelpart>(
                            new AnyCondition<Podlabelpart>(cond, "{1} LIKE (RTRIM({0})+'%')")));
                    }
                }
                sqlCtx.Param(g.DecAny(Podlabelpart.fn_partNo)).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Podlabelpart, PODLabelPartDef, PODLabelPartDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PODLabelPartDef> GetPODLabelPartList()
        {
            try
            {
                IList<PODLabelPartDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Podlabelpart>(tk, Podlabelpart.fn_family);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Podlabelpart, PODLabelPartDef, PODLabelPartDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePODLabelPart(PODLabelPartDef obj, string partNo)
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
                        Podlabelpart cond = new Podlabelpart();
                        cond.partNo = partNo;
                        Podlabelpart setv = FuncNew.SetColumnFromField<Podlabelpart, PODLabelPartDef>(obj);
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<Podlabelpart>(tk, new SetValueCollection<Podlabelpart>(new CommonSetValue<Podlabelpart>(setv)), new ConditionCollection<Podlabelpart>(new EqualCondition<Podlabelpart>(cond)));
                    }
                }
                sqlCtx.Param(Podlabelpart.fn_partNo).Value = partNo;

                sqlCtx = FuncNew.SetColumnFromField<Podlabelpart, PODLabelPartDef>(sqlCtx, obj, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Podlabelpart.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPODLabelPart(PODLabelPartDef obj)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Podlabelpart>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Podlabelpart, PODLabelPartDef>(sqlCtx, obj);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Podlabelpart.fn_cdt).Value = cmDt;

                obj.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePODLabelPart(string PartNo)
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
                        Podlabelpart cond = new Podlabelpart();
                        cond.partNo = PartNo;
                        sqlCtx = FuncNew.GetConditionedDelete<Podlabelpart>(tk, new ConditionCollection<Podlabelpart>(new EqualCondition<Podlabelpart>(cond)));
                    }
                }
                sqlCtx.Param(Podlabelpart.fn_partNo).Value = PartNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetAllTypeDescr()
        {
            try
            {
                IList<string> ret = new List<string>();

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond0 = new Part_NEW();
                        cond0.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond0));
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = "C2 or P1";
                        tf1.Conditions.Add(new InSetCondition<Part_NEW>(cond));
                        tf1.AddRangeToGetFieldNames(Part_NEW.fn_descr);

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = "FU";
                        cond2.infoValue = "Y";
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond0.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = cond2.infoType;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)).Value = cond2.infoValue;

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias,g.DecInSet(Part_NEW.fn_bomNodeType)), g.ConvertInSet(new List<string>(new string[] { "C2", "P1" })));
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, Part_NEW.fn_descr)));
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

        public IList<string> GetPartNoByTypeDescr(string typeDescr)
        {
            try
            {
                IList<string> ret = new List<string>();

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond0 = new Part_NEW();
                        cond0.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond0));
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = "C2 or P1";
                        tf1.Conditions.Add(new InSetCondition<Part_NEW>(cond));
                        Part_NEW cond1 = new Part_NEW();
                        cond1.descr = typeDescr;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond1));
                        tf1.AddRangeToGetFuncedFieldNames(new string[] { Part_NEW.fn_partNo, string.Format("CASE CHARINDEX('-',t1.{0}) WHEN 0 THEN t1.{0} ELSE LEFT(t1.{0},CHARINDEX('-',t1.{0})-1) END AS {0}", Part_NEW.fn_partNo) });

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = "FU";
                        cond2.infoValue = "Y";
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond0.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = cond2.infoType;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)).Value = cond2.infoValue;

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias, g.DecInSet(Part_NEW.fn_bomNodeType)), g.ConvertInSet(new List<string>(new string[] { "C2", "P1" })));
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_descr)).Value = typeDescr;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, Part_NEW.fn_partNo)));
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

        public IList<string> GetAllPAKikittingStationName(string stationtype)
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
                        mtns::Station cond = new mtns::Station();
                        cond.stationType = stationtype;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Station>(tk, "DISTINCT", new string[] { mtns::Station.fn_name }, new ConditionCollection<mtns::Station>(new EqualCondition<mtns::Station>(cond)), mtns::Station.fn_name);
                    }
                }
                sqlCtx.Param(mtns::Station.fn_stationType).Value = stationtype;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Station.fn_name));
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

        public IList<PartTypeDef> GetPartTypeDefList()
        {
            try
            {
                IList<PartTypeDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<mtns::PartType>(tk, mtns::PartType.fn_code);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::PartType, PartTypeDef, PartTypeDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PartTypeDef GetPartTypeInfo(int id)
        {
            try
            {
                PartTypeDef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PartType cond = new mtns::PartType();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartType>(tk, null, null, new ConditionCollection<mtns::PartType>(new EqualCondition<mtns::PartType>(cond)), mtns::PartType.fn_code);
                    }
                }
                sqlCtx.Param(mtns::PartType.fn_id).Value = id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::PartType, PartTypeDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetTPList()
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
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartType>(tk, "DISTINCT", new string[]{ mtns::PartType.fn_tp }, new ConditionCollection<mtns::PartType>(), mtns::PartType.fn_tp);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PartType.fn_tp));
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

        public IList<string> GetCodeListByTp(string tp)
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
                        mtns::PartType cond = new mtns::PartType();
                        cond.tp = tp;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartType>(tk, "DISTINCT", new string[] { mtns::PartType.fn_code }, new ConditionCollection<mtns::PartType>(new EqualCondition<mtns::PartType>(cond)), mtns::PartType.fn_code);
                    }
                }
                sqlCtx.Param(mtns::PartType.fn_tp).Value = tp;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PartType.fn_code));
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

        public void AddPartType(PartTypeDef partTypeDef)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::PartType>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::PartType, PartTypeDef>(sqlCtx, partTypeDef);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::PartType.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::PartType.fn_udt).Value = cmDt;

                partTypeDef.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePartType(PartTypeDef item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<mtns::PartType>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::PartType, PartTypeDef>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::PartType.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartType(int id)
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
                        sqlCtx = FuncNew.GetCommonDelete<mtns::PartType>(tk);
                    }
                }
                sqlCtx.Param(mtns::PartType.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据PartType删除PartType记录
        /// </summary>
        /// <param name="partType"></param>
        public void DeletePartType(string partType)
        {
            DeletePartTypeByPartType(partType);
        }

        public IList<PartTypeDef> GetPartTypeByCode(string code)
        {
            try
            {
                IList<PartTypeDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PartType cond = new mtns::PartType();
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartType>(tk, null, null, new ConditionCollection<mtns::PartType>(new EqualCondition<mtns::PartType>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PartType.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::PartType, PartTypeDef, PartTypeDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartTypeDef> GetPartTypeListByTp(string tp)
        {
            try
            {
                IList<PartTypeDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PartType cond = new mtns::PartType();
                        cond.tp = tp;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartType>(tk, null, null, new ConditionCollection<mtns::PartType>(new EqualCondition<mtns::PartType>(cond)), mtns::PartType.fn_code);
                    }
                }
                sqlCtx.Param(mtns::PartType.fn_tp).Value = tp;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::PartType, PartTypeDef, PartTypeDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<AssetRangeInfo> GetAllAssetRanges()
        {
            try
            {
                IList<AssetRangeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<mtns::AssetRange>(tk, mtns::AssetRange.fn_code);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::AssetRange, AssetRangeInfo, AssetRangeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddAssetRangeItem(AssetRangeInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::AssetRange>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::AssetRange, AssetRangeInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::AssetRange.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::AssetRange.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAssetRangeItem(AssetRangeInfo item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<mtns::AssetRange>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::AssetRange, AssetRangeInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(AssetRange.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAssetRangeItem(int id)
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
                        sqlCtx = FuncNew.GetCommonDelete<mtns::AssetRange>(tk);
                    }
                }
                sqlCtx.Param(mtns::AssetRange.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // add by yunfeng
        public IList<CeldataInfo> GetAllCeldatas()
        {
            try
            {
                IList<CeldataInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<mtns::CELDATA>(tk, mtns::CELDATA.fn_zmod);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::CELDATA, CeldataInfo, CeldataInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddCeldataItem(CeldataInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::CELDATA>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::CELDATA, CeldataInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::CELDATA.fn_cdt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                //item.ZMOD=_Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteCeldataItem(String zmod)
        {
            try
            {
                //SqlTransactionManager.Begin();

                //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartType));

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.CELDATA));
                    }
                }
                sqlCtx.Params[_Schema.CELDATA.fn_zmod].Value = zmod;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                //SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.Dispose();
                //SqlTransactionManager.End();
            }
        }
        //

        public IList<PartTypeDef> GetPartNodeType(string tp)
        {
            try
            {
                IList<PartTypeDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PartType cond = new mtns::PartType();
                        cond.tp = tp;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartType>(tk, null, null, new ConditionCollection<mtns::PartType>(new EqualCondition<mtns::PartType>(cond)), mtns::PartType.fn_indx);
                    }
                }
                sqlCtx.Param(mtns::PartType.fn_tp).Value = tp;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::PartType, PartTypeDef, PartTypeDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartDef> GetListByPartType(string partType)
        {
            try
            {
                IList<PartDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.partType = partType;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Part_NEW>(tk, null, null, new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)), mtns::Part_NEW.fn_partNo);

                        sqlCtx.Param(mtns::Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_partType).Value = partType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Part_NEW, PartDef, PartDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePartByPartNo(PartDef obj, string partNo)
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
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.partNo = partNo;
                        mtns::Part_NEW setv = FuncNew.SetColumnFromField<mtns::Part_NEW, PartDef>(obj);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<mtns::Part_NEW>(tk, new SetValueCollection<mtns::Part_NEW>(new CommonSetValue<mtns::Part_NEW>(setv)), new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_partNo).Value = partNo;

                sqlCtx = FuncNew.SetColumnFromField<mtns::Part_NEW, PartDef>(sqlCtx, obj, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Part_NEW.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(partNo, CacheType.Part));
                if (obj.partNo != partNo)
                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal(obj.partNo, CacheType.Part));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPart(PartDef obj)
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
                        sqlCtx = FuncNew.GetCommonInsert<mtns::Part_NEW>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Part_NEW, PartDef>(sqlCtx, obj);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Part_NEW.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Part_NEW.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(obj.partNo, CacheType.Part));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePart(string partNo)
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
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.partNo = partNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Part_NEW>(tk, new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_partNo).Value = partNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(partNo, CacheType.Part));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DescTypeInfo> GetDescriptionList(string tp)
        {
            try
            {
                IList<DescTypeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::DescType cond = new mtns::DescType();
                        cond.tp = tp;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::DescType>(tk, null, null, new ConditionCollection<mtns::DescType>(new EqualCondition<mtns::DescType>(cond)));
                    }
                }
                sqlCtx.Param(mtns::DescType.fn_tp).Value = tp;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::DescType, DescTypeInfo, DescTypeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPartInfo(IMES.FisObject.Common.Part.PartInfo obj)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::PartInfo>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::PartInfo, IMES.FisObject.Common.Part.PartInfo>(sqlCtx, obj);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::PartInfo.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::PartInfo.fn_udt).Value = cmDt;

                obj.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(obj.PN, CacheType.Part));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartInfo(string infoType)
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
                        mtns::PartInfo cond = new mtns::PartInfo();
                        cond.infoType = infoType;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PartInfo>(tk, new ConditionCollection<mtns::PartInfo>(new EqualCondition<mtns::PartInfo>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PartInfo.fn_infoType).Value = infoType;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePartInfo(IMES.FisObject.Common.Part.PartInfo obj, string partno, string infoType, string infoValue)
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
                        mtns::PartInfo cond = new mtns::PartInfo();
                        cond.partNo = partno;
                        cond.infoType = infoType;
                        cond.infoValue = infoValue;
                        mtns::PartInfo setv = FuncNew.SetColumnFromField<mtns::PartInfo, IMES.FisObject.Common.Part.PartInfo>(obj, mtns::PartInfo.fn_id);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<mtns::PartInfo>(tk, new SetValueCollection<mtns::PartInfo>(new CommonSetValue<mtns::PartInfo>(setv)), new ConditionCollection<mtns::PartInfo>(new EqualCondition<mtns::PartInfo>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PartInfo.fn_partNo).Value = partno;
                sqlCtx.Param(mtns::PartInfo.fn_infoType).Value = infoType;
                sqlCtx.Param(mtns::PartInfo.fn_infoValue).Value = infoValue;

                sqlCtx = FuncNew.SetColumnFromField<mtns::PartInfo, IMES.FisObject.Common.Part.PartInfo>(sqlCtx, obj, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::PartInfo.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(partno, CacheType.Part));
                if (obj.PN != partno)
                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal(obj.PN, CacheType.Part));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartTypeAndPartInfoValue> GetPartTypeAndPartInfoValueListByPartNo(string partNo, string partType)
        {
            // SELECT partType.Code AS Item, partType.Description AS Description, ISNULL(A.InfoValue,'') 
            // AS Content, A.Editor AS Editor, A.Cdt AS Cdt, A.ID, A.PartNo, A.Udt 
            // FROM (SELECT PartNo, InfoType, InfoValue, Editor, Cdt, ID, Udt 
            // FROM PartInfo WHERE PartNo=@PartNo) AS A 
            // RIGHT OUTER JOIN partType 
            // ON A.InfoType=partType.Code 
            // WHERE partType.Code=@Code
            try
            {
                IList<PartTypeAndPartInfoValue> ret = new List<PartTypeAndPartInfoValue>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT {0}.{2} AS Item, {0}.{3} AS Description, ISNULL(A.{6},'') " +
                                            "AS Content, A.{7} AS Editor, A.{8} AS Cdt, A.{9}, A.{11}, A.{10} " +
                                            "FROM (SELECT {4}, {5}, {6}, {7}, {8}, {9}, {10} " +
                                            "FROM {1} WHERE {11}=@{11}) AS A " +
                                            "RIGHT OUTER JOIN {0} " +
                                            "ON A.{5}={0}.{2} " +
                                            "WHERE {0}.{12}=@{12} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Metas.PartType).Name,
                                                                         typeof(_Metas.PartInfo).Name,
                                                                         _Metas.PartType.fn_code,
                                                                         _Metas.PartType.fn_description,
                                                                         _Metas.PartInfo.fn_partNo,
                                                                         _Metas.PartInfo.fn_infoType,
                                                                         _Metas.PartInfo.fn_infoValue,
                                                                         _Metas.PartInfo.fn_editor,
                                                                         _Metas.PartInfo.fn_cdt,
                                                                         _Metas.PartInfo.fn_id,
                                                                         _Metas.PartInfo.fn_udt,
                                                                         _Metas.PartInfo.fn_partNo,
                                                                         _Metas.PartType.fn_code
                                                                         );

                        sqlCtx.Params.Add(_Metas.PartType.fn_code, new SqlParameter("@" + _Metas.PartType.fn_code, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Metas.PartInfo.fn_partNo, new SqlParameter("@" + _Metas.PartInfo.fn_partNo, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Metas.PartType.fn_code].Value = partType;
                sqlCtx.Params[_Metas.PartInfo.fn_partNo].Value = partNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PartTypeAndPartInfoValue item = new PartTypeAndPartInfoValue();
                            item.Item = GetValue_Str(sqlR, 0);
                            item.Description = GetValue_Str(sqlR, 1);
                            item.Content = GetValue_Str(sqlR, 2);
                            item.Editor = GetValue_Str(sqlR, 3);
                            item.Cdt = GetValue_DateTime(sqlR, 4);
                            item.Id = GetValue_Int32(sqlR, 5);
                            item.MainTableKey = GetValue_Str(sqlR, 6);
                            item.Udt = GetValue_DateTime(sqlR, 7);
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

        public IList<PartDef> GetLstByBomNode(string bomNode)
        {
            try
            {
                IList<PartDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.bomNodeType = bomNode;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Part_NEW>(tk, null, null, new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)));

                        sqlCtx.Param(mtns::Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_bomNodeType).Value = bomNode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Part_NEW, PartDef, PartDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartDef> GetLstByPartNo(string partNo)
        {
            try
            {
                IList<PartDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.partNo = partNo;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Part_NEW>(tk, null, null, new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)));

                        sqlCtx.Param(mtns::Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_partNo).Value = partNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Part_NEW, PartDef, PartDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.Common.Part.PartInfo> GetLstPartInfo(string partno, string infoType, string infoValue)
        {
            try
            {
                IList<IMES.FisObject.Common.Part.PartInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PartInfo cond = new mtns::PartInfo();
                        cond.partNo = partno;
                        cond.infoType = infoType;
                        cond.infoValue = infoValue;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartInfo>(tk, null, null, new ConditionCollection<mtns::PartInfo>(new EqualCondition<mtns::PartInfo>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PartInfo.fn_partNo).Value = partno;
                sqlCtx.Param(mtns::PartInfo.fn_infoType).Value = infoType;
                sqlCtx.Param(mtns::PartInfo.fn_infoValue).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::PartInfo, IMES.FisObject.Common.Part.PartInfo, IMES.FisObject.Common.Part.PartInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartInfoByPN(string partNo)
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
                        mtns::PartInfo cond = new mtns::PartInfo();
                        cond.partNo = partNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PartInfo>(tk, new ConditionCollection<mtns::PartInfo>(new EqualCondition<mtns::PartInfo>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PartInfo.fn_partNo).Value = partNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(partNo, CacheType.Part));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartInfoValueByPartDescr(string descr, string infoType)
        {
            try
            {
                IList<string> ret = new List<string>();

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.descr = descr;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = infoType;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, _Metas.PartInfo.fn_infoValue);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_descr)).Value = descr;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)));
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

        public IList<AssetRangeInfo> GetAssetRangesByCode(string code)
        {
            try
            {
                IList<AssetRangeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::AssetRange cond = new mtns::AssetRange();
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::AssetRange>(tk, null, null, new ConditionCollection<mtns::AssetRange>(new EqualCondition<mtns::AssetRange>(cond)));
                    }
                }
                sqlCtx.Param(mtns::AssetRange.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::AssetRange, AssetRangeInfo, AssetRangeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartDef> GetPartByBomNodeTypeAndDescr(string productId, string bomNodeType, string[] descrs)
        {
            try
            {
                IList<PartDef> ret = null;

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        Part_NEW cond0 = new Part_NEW();
                        cond0.descr = "[INSET]";
                        tf1.Conditions.Add(new InSetCondition<Part_NEW>(cond0));

                        tf2 = new TableAndFields<_Metas.Product_Part>();
                        _Metas.Product_Part cond2 = new _Metas.Product_Part();
                        cond2.productID = productId;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Product_Part>(cond2));
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_FA;
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.Product_Part>(tf1, Part_NEW.fn_partNo, tf2, _Metas.Product_Part.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, Part_NEW.fn_partNo);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Product_Part.fn_productID)).Value = productId;

                string Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias, g.DecInSet(Part_NEW.fn_descr)), g.ConvertInSet(new List<string>(descrs)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Part_NEW, PartDef, PartDef>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartDef> GetPartByBomNodeType(string productId, string bomNodeType)
        {
            try
            {
                IList<PartDef> ret = null;

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));

                        tf2 = new TableAndFields<_Metas.Product_Part>();
                        _Metas.Product_Part cond2 = new _Metas.Product_Part();
                        cond2.productID = productId;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Product_Part>(cond2));
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_FA;
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.Product_Part>(tf1, Part_NEW.fn_partNo, tf2, _Metas.Product_Part.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, Part_NEW.fn_partNo);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Product_Part.fn_productID)).Value = productId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Part_NEW, PartDef, PartDef>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetInfoValue(string type)
        {
            try
            {
                IList<string> ret = new List<string>();

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = type;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = type;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, _Metas.PartInfo.fn_infoValue);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_bomNodeType)).Value = type;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)));
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

        public IList<string> GetPartSnListFromProductPart(string partSnPrefix, string bomNodeType)
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
                        Product_Part cond = new Product_Part();
                        cond.partSn = partSnPrefix + "%";
                        Product_Part cond2 = new Product_Part();
                        cond2.bomNodeType = bomNodeType;
                        sqlCtx = FuncNew.GetConditionedSelect<Product_Part>(tk, "DISTINCT", new string[] { Product_Part.fn_partSn }, new ConditionCollection<Product_Part>(new LikeCondition<Product_Part>(cond), new EqualCondition<Product_Part>(cond2)), Product_Part.fn_partSn);
                    }
                }
                sqlCtx.Param(_Metas.Product_Part.fn_partSn).Value = partSnPrefix + "%";
                sqlCtx.Param(_Metas.Product_Part.fn_bomNodeType).Value = bomNodeType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(Product_Part.fn_partSn));
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

        public IList<string> GetPartSnListFromProductPart(string partSnPrefix, string bomNodeType, string productID)
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
                        Product_Part cond = new Product_Part();
                        cond.partSn = partSnPrefix + "%";
                        Product_Part cond2 = new Product_Part();
                        cond2.bomNodeType = bomNodeType;
                        cond2.productID = productID;
                        sqlCtx = FuncNew.GetConditionedSelect<Product_Part>(tk, "DISTINCT", new string[] { Product_Part.fn_partSn }, new ConditionCollection<Product_Part>(new LikeCondition<Product_Part>(cond), new EqualCondition<Product_Part>(cond2)), Product_Part.fn_partSn);
                    }
                }
                sqlCtx.Param(_Metas.Product_Part.fn_partSn).Value = partSnPrefix + "%";
                sqlCtx.Param(_Metas.Product_Part.fn_bomNodeType).Value = bomNodeType;
                sqlCtx.Param(_Metas.Product_Part.fn_productID).Value = productID;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(Product_Part.fn_partSn));
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

        public bool CheckExistInternalCOA(string code, string type)
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
                        InternalCOA cond = new InternalCOA();
                        cond.code = code;
                        cond.type = type;
                        sqlCtx = FuncNew.GetConditionedSelect<InternalCOA>(tk, "COUNT", new string[] { InternalCOA.fn_id }, new ConditionCollection<InternalCOA>(new EqualCondition<InternalCOA>(cond)));
                    }
                }
                sqlCtx.Param(InternalCOA.fn_code).Value = code;
                sqlCtx.Param(InternalCOA.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        /// <summary>
        ///  Vincent add ConstValueType 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<ConstValueTypeInfo> GetConstValueTypeList(string type)
        {
            try
            {
                List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {                        
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  ID, [Type], Value, Description, Editor, Cdt, Udt
                                                            from ConstValueType
                                                            where [Type]=@type
                                                             order by ID";
                        sqlCtx.AddParam("Type", new SqlParameter("@type" , SqlDbType.VarChar));
                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Type").Value = type;
                

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, 
                                                                                                                            CommandType.Text, 
                                                                                                                            sqlCtx.Sentence, 
                                                                                                                            sqlCtx.Params))
                {
                   while (sqlR != null && sqlR.Read())
                    {
                        ConstValueTypeInfo item = new ConstValueTypeInfo();
                        item.id = sqlR.GetInt32(0);
                        item.type = sqlR.GetString(1);
                        item.value = sqlR.GetString(2);
                        item.description = sqlR.GetString(3);
                        item.editor = sqlR.GetString(4);
                        item.cdt = sqlR.GetDateTime(5);
                        item.udt = sqlR.GetDateTime(6);

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
        ///  Vincent add ConstValueType 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<ConstValueTypeInfo> GetConstValueTypeList(string type, string value)
        {
            try
            {
                List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  ID, [Type], Value, Description, Editor, Cdt, Udt
                                                            from ConstValueType
                                                            where [Type]=@type and Value=@Value
                                                             order by ID";
                        sqlCtx.AddParam("Type", new SqlParameter("@type", SqlDbType.VarChar));
                        sqlCtx.AddParam("Value", new SqlParameter("@Value", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Type").Value = type;
                sqlCtx.Param("Value").Value = value;



                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ConstValueTypeInfo item = new ConstValueTypeInfo();
                        item.id = sqlR.GetInt32(0);
                        item.type = sqlR.GetString(1);
                        item.value = sqlR.GetString(2);
                        item.description = sqlR.GetString(3);
                        item.editor = sqlR.GetString(4);
                        item.cdt = sqlR.GetDateTime(5);
                        item.udt = sqlR.GetDateTime(6);

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
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetConstValueTypeList()
        {
            try
            {
                List<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  distinct [Type] as [type]
                                                            from ConstValueType";                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }                

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                         ret.Add(sqlR.GetString(0));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateConstValueType(ConstValueTypeInfo info)
        {
            try
            {
                List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"Update ConstValueType
                                                            set [Type] =@type, 
                                                                   Value=@value, 
                                                                  Description=@descr, 
                                                                  Editor=@editor, 
                                                                   Udt=getdate()
                                                            from ConstValueType
                                                            where ID=@id";
                        sqlCtx.AddParam("ID", new SqlParameter("@id", SqlDbType.Int));
                        sqlCtx.AddParam("Type", new SqlParameter("@type", SqlDbType.VarChar));
                        sqlCtx.AddParam("Value", new SqlParameter("@value", SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@descr", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ID").Value = info.id;
                sqlCtx.Param("Type").Value = info.type;
                sqlCtx.Param("Value").Value = info.value;
                sqlCtx.Param("Descr").Value = info.description;
                sqlCtx.Param("Editor").Value = info.editor;

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

        public void InsertConstValueType(ConstValueTypeInfo info)
        {
            try
            {
                List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"Insert ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
                                                            values(@type,@value,@descr,@editor,getdate(),getdate())";
                        sqlCtx.AddParam("Type", new SqlParameter("@type", SqlDbType.VarChar));
                        sqlCtx.AddParam("Value", new SqlParameter("@value", SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@descr", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Type").Value = info.type;
                sqlCtx.Param("Value").Value = info.value;
                sqlCtx.Param("Descr").Value = info.description;
                sqlCtx.Param("Editor").Value = info.editor;

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

        public void InsertMultiConstValueType(string type, IList<string> values, string descr, string editor)
        {
            try
            {
                List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"Insert ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
                                                            select @type,data,@descr,@editor,getdate(),getdate()
                                                             from @valueList";
                        sqlCtx.AddParam("Type", new SqlParameter("@type", SqlDbType.VarChar));

                        SqlParameter para = new SqlParameter("@valueList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("Value", para);
                        sqlCtx.AddParam("Descr", new SqlParameter("@descr", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Type").Value = type;

                DataTable valueDt = CreateStringListTb();
                foreach (string data in values)
                {
                    valueDt.Rows.Add(data);
                }
                sqlCtx.Param("Value").Value = valueDt;
                sqlCtx.Param("Descr").Value = descr;
                sqlCtx.Param("Editor").Value = editor;

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

        public void RemoveConstValueType(int id)
        {
            try
            {
                List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"Delete ConstValueType
                                                            where ID=@id";
                        sqlCtx.AddParam("ID", new SqlParameter("@id", SqlDbType.Int));
                     

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ID").Value = id;
               
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

        public void RemoveMultiConstValueType(string type, string value)
        {
            try
            {
                List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence =@"Delete ConstValueType
                                                            where Type=@type and Value in (select value from dbo.fn_split(@value,','))";
                        sqlCtx.AddParam("type", new SqlParameter("@type", SqlDbType.VarChar,40));
                        sqlCtx.AddParam("value", new SqlParameter("@value", SqlDbType.VarChar, 40));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("type").Value = type;
                sqlCtx.Param("value").Value = value;

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


        public IList<ConstValueInfo> GetConstValueListByType(string type)
        {
            try
            {
                IList<ConstValueInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::ConstValue cond = new mtns::ConstValue();
                        cond.type = type;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::ConstValue>(tk, null, null, new ConditionCollection<mtns::ConstValue>(new EqualCondition<mtns::ConstValue>(cond)), mtns::ConstValue.fn_name);
                    }
                }
                sqlCtx.Param(mtns::ConstValue.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::ConstValue, ConstValueInfo, ConstValueInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ProductPart> GetProductPart(string partSnPrefix, string bomNodeType)
        {
            try
            {
                IList<ProductPart> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Product_Part cond = new Product_Part();
                        cond.partSn = partSnPrefix + "%";
                        Product_Part cond2 = new Product_Part();
                        cond2.bomNodeType = bomNodeType;
                        sqlCtx = FuncNew.GetConditionedSelect<Product_Part>(tk, null, null, new ConditionCollection<Product_Part>(new LikeCondition<Product_Part>(cond), new EqualCondition<Product_Part>(cond2)), Product_Part.fn_partSn);
                    }
                }
                sqlCtx.Param(_Metas.Product_Part.fn_partSn).Value = partSnPrefix + "%";
                sqlCtx.Param(_Metas.Product_Part.fn_bomNodeType).Value = bomNodeType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Product_Part, ProductPart, ProductPart>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ProductPart> GetProductPart(string partSnPrefix, string bomNodeType, string prodId)
        {
            try
            {
                IList<ProductPart> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Product_Part cond = new Product_Part();
                        cond.partSn = partSnPrefix + "%";
                        Product_Part cond2 = new Product_Part();
                        cond2.bomNodeType = bomNodeType;
                        cond2.productID = prodId;
                        sqlCtx = FuncNew.GetConditionedSelect<Product_Part>(tk, null, null, new ConditionCollection<Product_Part>(new LikeCondition<Product_Part>(cond), new EqualCondition<Product_Part>(cond2)), Product_Part.fn_partSn);
                    }
                }
                sqlCtx.Param(_Metas.Product_Part.fn_partSn).Value = partSnPrefix + "%";
                sqlCtx.Param(_Metas.Product_Part.fn_bomNodeType).Value = bomNodeType;
                sqlCtx.Param(_Metas.Product_Part.fn_productID).Value = prodId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Product_Part, ProductPart, ProductPart>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateForceNWCByProductID(string forceNwc, string preStation, string productId)
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
                        ForceNWC cond = new ForceNWC();
                        cond.productID = productId;
                        ForceNWC setv = new ForceNWC();
                        setv.forceNWC = forceNwc;
                        setv.preStation = preStation;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<ForceNWC>(tk, new SetValueCollection<ForceNWC>(new CommonSetValue<ForceNWC>(setv)), new ConditionCollection<ForceNWC>(new EqualCondition<ForceNWC>(cond)));
                    }
                }
                sqlCtx.Param(ForceNWC.fn_productID).Value = productId;
                sqlCtx.Param(g.DecSV(ForceNWC.fn_forceNWC)).Value = forceNwc;
                sqlCtx.Param(g.DecSV(ForceNWC.fn_preStation)).Value = preStation;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Podlabelpart.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetConstValueNameListByType(string type)
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
                        mtns::ConstValue cond = new mtns::ConstValue();
                        cond.type = type;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::ConstValue>(tk, null, new string[] { _Metas.ConstValue.fn_name }, new ConditionCollection<mtns::ConstValue>(new EqualCondition<mtns::ConstValue>(cond)), mtns::ConstValue.fn_name);
                    }
                }
                sqlCtx.Param(mtns::ConstValue.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::ConstValue.fn_name));
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

        public void UpdateForceNWC(ForceNWCInfo setValue, ForceNWCInfo condition)
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
                ForceNWC cond = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(condition);
                ForceNWC setv = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<ForceNWC>(new SetValueCollection<ForceNWC>(new CommonSetValue<ForceNWC>(setv)), new ConditionCollection<ForceNWC>(new EqualCondition<ForceNWC>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.ForceNWC.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertForceNWC(ForceNWCInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<ForceNWC>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(ForceNWC.fn_udt).Value = cmDt;
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetValueFromSysSettingByName(string name)
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
                        SysSetting cond = new SysSetting();
                        cond.name = name;
                        sqlCtx = FuncNew.GetConditionedSelect<SysSetting>(tk, null, new string[] { SysSetting.fn_value }, new ConditionCollection<SysSetting>(new EqualCondition<SysSetting>(cond)), SysSetting.fn_value);
                    }
                }
                sqlCtx.Param(_Metas.SysSetting.fn_name).Value = name;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(SysSetting.fn_value));
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

        public IList<ConstValueInfo> GetConstValueInfoList(ConstValueInfo condition)
        {
            try
            {
                IList<ConstValueInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                ConstValue cond = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<ConstValue>(null, null, new ConditionCollection<ConstValue>(new EqualCondition<ConstValue>(cond)), ConstValue.fn_type, ConstValue.fn_name);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ConstValue, ConstValueInfo, ConstValueInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetTypeListFromConstValue()
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
                        sqlCtx = FuncNew.GetConditionedSelect<ConstValue>(tk, "DISTINCT", new string[] { ConstValue.fn_type }, new ConditionCollection<ConstValue>(), ConstValue.fn_type);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(ConstValue.fn_type));
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

        public IList<ConstValueInfo> GetConstValueListByType(ConstValueInfo eqCondition, ConstValueInfo neqCondition)
        {
            try
            {
                IList<ConstValueInfo> ret = null;

                if (eqCondition == null)
                    eqCondition = new ConstValueInfo();
                if (neqCondition == null)
                    neqCondition = new ConstValueInfo();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                _Metas.ConstValue cond = FuncNew.SetColumnFromField<_Metas.ConstValue, ConstValueInfo>(eqCondition);
                _Metas.ConstValue cond2 = FuncNew.SetColumnFromField<_Metas.ConstValue, ConstValueInfo>(neqCondition);

                sqlCtx = FuncNew.GetConditionedSelect<_Metas.ConstValue>(null, null, new ConditionCollection<_Metas.ConstValue>(new EqualCondition<_Metas.ConstValue>(cond), new NotEqualCondition<_Metas.ConstValue>(cond2)), mtns::ConstValue.fn_name);
                var sqlCtx2 = FuncNew.GetConditionedSelect<_Metas.ConstValue>(null, new string[] { _Metas.ConstValue.fn_id }, new ConditionCollection<_Metas.ConstValue>(new NotEqualCondition<_Metas.ConstValue>(cond2)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<_Metas.ConstValue, ConstValueInfo>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.ConstValue, ConstValueInfo>(sqlCtx2, neqCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::ConstValue, ConstValueInfo, ConstValueInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddConstValue(ConstValueInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<ConstValue>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(ConstValue.fn_cdt).Value = cmDt;
                sqlCtx.Param(ConstValue.fn_udt).Value = cmDt;
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateConstValue(ConstValueInfo setValue, ConstValueInfo condition)
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
                ConstValue cond = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(condition);
                ConstValue setv = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<ConstValue>(new SetValueCollection<ConstValue>(new CommonSetValue<ConstValue>(setv)), new ConditionCollection<ConstValue>(new EqualCondition<ConstValue>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.ConstValue.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveConstValue(ConstValueInfo condition)
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
                ConstValue cond = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<ConstValue>(new ConditionCollection<ConstValue>(new EqualCondition<ConstValue>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ConstValue, ConstValueInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IPart> GetPartsByBomNodeTypeAndLikeDescr(string bomNodeType, string descrPrefix)
        {
            try
            {
                IList<IPart> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Part_NEW cond = new Part_NEW();
                        cond.descr = descrPrefix + "%";
                        Part_NEW cond2 = new Part_NEW();
                        cond2.bomNodeType = bomNodeType;
                        cond2.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<Part_NEW>(tk, null, null, new ConditionCollection<Part_NEW>(new LikeCondition<Part_NEW>(cond), new EqualCondition<Part_NEW>(cond2)), Part_NEW.fn_partNo);

                        sqlCtx.Param(_Metas.Part_NEW.fn_flag).Value = cond2.flag;
                    }
                }
                sqlCtx.Param(_Metas.Part_NEW.fn_descr).Value = descrPrefix + "%";
                sqlCtx.Param(_Metas.Part_NEW.fn_bomNodeType).Value = bomNodeType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<IPart>();
                        while (sqlR.Read())
                        {
                            IPart item = new IMES.FisObject.Common.Part.Part(
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partNo)),
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_bomNodeType)),
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_partType)),
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_custPartNo)),
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_descr)),
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_remark)),
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_autoDL)),
                                                g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_editor)),
                                                g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_cdt)),
                                                g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_udt)),
                                                null);
                            ((IMES.FisObject.Common.Part.Part)item).Tracker.Clear();
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

        public string GetPartInfoValue(string partno, string infotype)
        {
            try
            {
                string ret = null;

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.partNo = partno;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = infotype;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "TOP 1", tafa, tblCnnts, "t2." + _Metas.PartInfo.fn_cdt);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_partNo)).Value = partno;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = infotype;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistForceNWC(ForceNWCInfo condition)
        {
            try
            {
                bool ret = false;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                ForceNWC cond = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<ForceNWC>("COUNT", new string[] { ForceNWC.fn_id }, new ConditionCollection<ForceNWC>(new EqualCondition<ForceNWC>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public IList<ForceNWCInfo> GetForceNWCListByCondition(ForceNWCInfo condition)
        {
            try
            {
                IList<ForceNWCInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                ForceNWC cond = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<ForceNWC>(null, null, new ConditionCollection<ForceNWC>(new EqualCondition<ForceNWC>(cond)), ForceNWC.fn_id);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ForceNWC, ForceNWCInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ForceNWC, ForceNWCInfo, ForceNWCInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckBySP(string spName, string prodId, string checkItem)
        {
            try
            {
                int ret = -1;

                SqlParameter[] paramsArray = new SqlParameter[3];

                paramsArray[0] = new SqlParameter("@ProductID", SqlDbType.VarChar);
                paramsArray[0].Value = prodId;

                paramsArray[1] = new SqlParameter("@CheckItem", SqlDbType.VarChar);
                paramsArray[1].Value = checkItem;

                paramsArray[2] = new SqlParameter("@returnvalue", SqlDbType.Int);
                paramsArray[2].Direction = ParameterDirection.ReturnValue;

                _Schema.SqlHelper.ExecuteNonQueryConsiderOutParams(_Schema.SqlHelper.ConnectionString_BOM, CommandType.StoredProcedure, spName, paramsArray);
                object data = paramsArray[2].Value;
                if (data != null)
                    ret = Convert.ToInt32(data);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddSysSettingInfo(SysSettingInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<SysSetting>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<SysSetting, SysSettingInfo>(sqlCtx, item);
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSysSettingInfo(SysSettingInfo setValue, SysSettingInfo condition)
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
                SysSetting cond = FuncNew.SetColumnFromField<SysSetting, SysSettingInfo>(condition);
                SysSetting setv = FuncNew.SetColumnFromField<SysSetting, SysSettingInfo>(setValue);

                sqlCtx = FuncNew.GetConditionedUpdate<SysSetting>(new SetValueCollection<SysSetting>(new CommonSetValue<SysSetting>(setv)), new ConditionCollection<SysSetting>(new EqualCondition<SysSetting>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<SysSetting, SysSettingInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<SysSetting, SysSettingInfo>(sqlCtx, setValue, true);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartInfoValueByPartDescr(string descr, string infoType, string infoType2, string infoValue2)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields tf3 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.descr = descr;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = infoType;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = infoType2;
                        cond3.infoValue = infoValue2;
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        tf3.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, _Metas.PartInfo.fn_infoValue);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_descr)).Value = descr;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = infoType2;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)).Value = infoValue2;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)));
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

        public IList<string> GetDescrListFromPartByBomNodeType(string bomNodeType)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Part_NEW>(tk, "DISTINCT", new string[] { mtns::Part_NEW.fn_descr }, new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)), mtns::Part_NEW.fn_descr);

                        sqlCtx.Param(mtns::Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_bomNodeType).Value = bomNodeType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Part_NEW.fn_descr));
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

        public IList<string> GetCauseItemListByType(string type)
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
                        mtns::ConstValue cond = new mtns::ConstValue();
                        cond.type = type;

                        mtns::ConstValue cond2 = new mtns::ConstValue();
                        cond2.value = "[0-9]";

                        mtns::ConstValue cond3 = new mtns::ConstValue();
                        cond3.description = string.Empty;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::ConstValue>(tk, null, new string[] { _Metas.ConstValue.fn_value, _Metas.ConstValue.fn_description }, new ConditionCollection<mtns::ConstValue>(
                            new EqualCondition<mtns::ConstValue>(cond),
                            new LikeCondition<mtns::ConstValue>(cond2,"LEFT({0},1)"),
                            new NotEqualCondition<mtns::ConstValue>(cond3)), _Metas.ConstValue.fn_value, _Metas.ConstValue.fn_description);

                        sqlCtx.Param(mtns::ConstValue.fn_value).Value = cond2.value;
                        sqlCtx.Param(mtns::ConstValue.fn_description).Value = cond3.description;
                    }
                }
                sqlCtx.Param(mtns::ConstValue.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = string.Format("{0} {1}", g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::ConstValue.fn_value)), g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::ConstValue.fn_description)));
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

        public int GetCountOfKpRepairCount(KeyPartRepairInfo condition)
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
                mtns::KeyPartRepair cond = mtns::FuncNew.SetColumnFromField<mtns::KeyPartRepair, KeyPartRepairInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::KeyPartRepair>("COUNT", new string[] { mtns::KeyPartRepair.fn_id }, new mtns::ConditionCollection<mtns::KeyPartRepair>(new mtns::EqualCondition<mtns::KeyPartRepair>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::KeyPartRepair, KeyPartRepairInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if(sqlR.Read())
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

        public IList<RepairInfo> GetKPRepairDefectList(string ctno)
        {
            try
            {
                IList<RepairInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        KeyPartRepair_DefectInfo cond = new KeyPartRepair_DefectInfo();
                        cond.keyPartRepairID = 88;
                        sqlCtx = FuncNew.GetConditionedSelect<KeyPartRepair_DefectInfo>(null, null, new ConditionCollection<KeyPartRepair_DefectInfo>(
                            new AnyCondition<KeyPartRepair_DefectInfo>(cond, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4} AND {5}=0)", "{0}", KeyPartRepair.fn_id, ToolsNew.GetTableName(typeof(KeyPartRepair)), KeyPartRepair.fn_productID, "{1}", KeyPartRepair.fn_status))));
                        sqlCtx.Param(g.DecAny(mtns::KeyPartRepair_DefectInfo.fn_keyPartRepairID)).SqlDbType = ToolsNew.GetDBFieldType<KeyPartRepair>(KeyPartRepair.fn_productID);
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::KeyPartRepair_DefectInfo.fn_keyPartRepairID)).Value = ctno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<RepairInfo>();
                        while (sqlR.Read())
                        {
                            var item = new RepairInfo();
                            item._4M = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn__4M_)) ;
                            item.action = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_action)) ;
                            item.cause = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_cause)) ;
                            //item.causeDesc = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn)) ;
                            item.cdt = g.GetValue_DateTime(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_cdt)) ;
                            item.component = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_component)) ;
                            item.cover = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_cover)) ;
                            //item.defectCodeDesc = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn)) ;
                            item.defectCodeID = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_defectCode)) ;
                            item.distribution = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_distribution)) ;
                            item.editor = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_editor)) ;
                            item.Identity = g.GetValue_Int32(sqlR, sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_id));
                            item.id = item.Identity.ToString();
                            item.isManual = g.GetValue_Int32(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_isManual)).ToString() ; 
                            item.location = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_location)) ;
                            item.majorPart = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_majorPart)) ;
                            item.manufacture = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_manufacture)) ;
                            item.mark = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_mark)) ;
                            item.mtaID = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_mtaid)) ;
                            item.newPart = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_newPart)) ; 
                            //item.newPartDateCode = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn)) ;
                            item.newPartSno = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_newPartSno)) ;
                            item.obligation = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_obligation)) ;
                            item.oldPart = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_oldPart)) ;
                            item.oldPartSno = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_oldPartSno)) ;
                            item.partType = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_partType)) ;
                            //item.pdLine = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn)) ;
                            item.piaStation = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_piastation)) ;
                            item.remark = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_remark)) ;
                            item.repairID = g.GetValue_Int32(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_keyPartRepairID)).ToString() ;
                            item.responsibility = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_responsibility)) ;
                            item.returnSign = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_returnSign)) ;
                            item.returnStation = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_returnStn)) ;
                            //item.side = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn)) ;
                            item.site = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_site)) ;
                            item.subDefect = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_subDefect)) ;
                            //item.testStation = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn)) ;
                            //item.testStationDesc = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn)) ;
                            item.trackingStatus = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_trackingStatus)) ;
                            item.type = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_type)) ;
                            item.udt = g.GetValue_DateTime(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_udt)) ;
                            item.uncover = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_uncover)) ;
                            item.vendorCT = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_vendorCT)) ;
                            item.versionA = g.GetValue_Str(sqlR,sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_versionA)) ;
                            item.versionB = g.GetValue_Str(sqlR, sqlCtx.Indexes(KeyPartRepair_DefectInfo.fn_versionB));
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

        public void AddKPRepair(KeyPartRepairInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<KeyPartRepair>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<KeyPartRepair, KeyPartRepairInfo>(sqlCtx, item);

                sqlCtx.Param(KeyPartRepair.fn_cdt).Value = cmDt;
                sqlCtx.Param(KeyPartRepair.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddKPRepairDefect(IMES.FisObject.Common.Repair.RepairDefect item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<KeyPartRepair_DefectInfo>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<KeyPartRepair_DefectInfo, IMES.FisObject.Common.Repair.RepairDefect>(sqlCtx, item, KeyPartRepair_DefectInfo.fn_isManual);

                sqlCtx.Param(KeyPartRepair_DefectInfo.fn_isManual).Value = Convert.ToInt32(item.IsManual);

                sqlCtx.Param(KeyPartRepair_DefectInfo.fn_cdt).Value = cmDt;
                sqlCtx.Param(KeyPartRepair_DefectInfo.fn_udt).Value = cmDt;

                item.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateKPRepairDefect(IMES.FisObject.Common.Repair.RepairDefect setValue, IMES.FisObject.Common.Repair.RepairDefect condition)
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
                KeyPartRepair_DefectInfo cond = FuncNew.SetColumnFromField<KeyPartRepair_DefectInfo, IMES.FisObject.Common.Repair.RepairDefect>(condition, KeyPartRepair_DefectInfo.fn_isManual);
                KeyPartRepair_DefectInfo setv = FuncNew.SetColumnFromField<KeyPartRepair_DefectInfo, IMES.FisObject.Common.Repair.RepairDefect>(setValue, KeyPartRepair_DefectInfo.fn_isManual, KeyPartRepair_DefectInfo.fn_id);
                setv.udt = DateTime.Now;
                setv.isManual = 1;

                sqlCtx = FuncNew.GetConditionedUpdate<KeyPartRepair_DefectInfo>(new SetValueCollection<KeyPartRepair_DefectInfo>(new CommonSetValue<KeyPartRepair_DefectInfo>(setv)), new ConditionCollection<KeyPartRepair_DefectInfo>(new EqualCondition<KeyPartRepair_DefectInfo>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<KeyPartRepair_DefectInfo, IMES.FisObject.Common.Repair.RepairDefect>(sqlCtx, condition, KeyPartRepair_DefectInfo.fn_isManual);
                sqlCtx = FuncNew.SetColumnFromField<KeyPartRepair_DefectInfo, IMES.FisObject.Common.Repair.RepairDefect>(sqlCtx, setValue, true, KeyPartRepair_DefectInfo.fn_isManual, KeyPartRepair_DefectInfo.fn_id);
                sqlCtx.Param(g.DecSV(_Metas.KeyPartRepair_DefectInfo.fn_isManual)).Value = Convert.ToInt32(setValue.IsManual);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.KeyPartRepair_DefectInfo.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateKPRepair(KeyPartRepairInfo setValue, KeyPartRepairInfo condition)
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
                KeyPartRepair cond = FuncNew.SetColumnFromField<KeyPartRepair, KeyPartRepairInfo>(condition);
                KeyPartRepair setv = FuncNew.SetColumnFromField<KeyPartRepair, KeyPartRepairInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<KeyPartRepair>(new SetValueCollection<KeyPartRepair>(new CommonSetValue<KeyPartRepair>(setv)), new ConditionCollection<KeyPartRepair>(new EqualCondition<KeyPartRepair>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<KeyPartRepair, KeyPartRepairInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<KeyPartRepair, KeyPartRepairInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.KeyPartRepair.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIfKPRepairFinished(string ctno)
        {
            try
            {
                bool ret = false;

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
                        tf1 = new TableAndFields<KeyPartRepair>();
                        KeyPartRepair cond = new KeyPartRepair();
                        cond.productID = ctno;
                        tf1.Conditions.Add(new EqualCondition<KeyPartRepair>(cond));
                        tf1.AddRangeToGetFieldNames(KeyPartRepair.fn_id);

                        tf2 = new TableAndFields<KeyPartRepair_DefectInfo>();
                        KeyPartRepair_DefectInfo cond2 = new KeyPartRepair_DefectInfo();
                        cond2.cause = string.Empty;
                        tf2.Conditions.Add(new EqualCondition<KeyPartRepair_DefectInfo>(cond2, "ISNULL({0},'')"));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<KeyPartRepair, KeyPartRepair_DefectInfo>(tf1, KeyPartRepair.fn_id, tf2, KeyPartRepair_DefectInfo.fn_keyPartRepairID));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, KeyPartRepair_DefectInfo.fn_cause)).Value = cond2.cause;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, KeyPartRepair.fn_productID)).Value = ctno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public IList<KeyPartRepairInfo> GetKPRepairList(KeyPartRepairInfo condition)
        {
            try
            {
                IList<KeyPartRepairInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                KeyPartRepair cond = FuncNew.SetColumnFromField<KeyPartRepair, KeyPartRepairInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<KeyPartRepair>(null, null, new ConditionCollection<KeyPartRepair>(new EqualCondition<KeyPartRepair>(cond)), KeyPartRepair.fn_udt + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<KeyPartRepair, KeyPartRepairInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<KeyPartRepair, KeyPartRepairInfo, KeyPartRepairInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetInfoValueList(string[] pnlist, string infoType)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PartInfo cond = new mtns::PartInfo();
                        cond.infoType = infoType;
                        mtns::PartInfo cond2 = new mtns::PartInfo();
                        cond2.partNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PartInfo>(tk, "DISTINCT", new string[] { mtns::PartInfo.fn_infoValue }, new ConditionCollection<mtns::PartInfo>(
                            new EqualCondition<mtns::PartInfo>(cond),
                            new InSetCondition<mtns::PartInfo>(cond2)), mtns::PartInfo.fn_infoValue);
                    }
                }
                sqlCtx.Param(mtns::PartInfo.fn_infoType).Value = infoType;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::PartInfo.fn_partNo), g.ConvertInSet(new List<string>(pnlist)));
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.PartInfo.fn_infoValue));
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

        public IList<string> GetPartInfoValueByInfoTypesAndInfoValuePrefix(string[] infoTypes, string ctno)
        {
            try
            {
                IList<string> ret = new List<string>();

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
                        tf1 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond = new _Metas.PartInfo();
                        cond.infoType = "[INSET]";
                        tf1.Conditions.Add(new InSetCondition<_Metas.PartInfo>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoValue = ctno;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2, null, "LEFT({0},5)"));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.PartInfo, _Metas.PartInfo>(tf1, _Metas.PartInfo.fn_partNo, tf2, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t1." + _Metas.PartInfo.fn_infoValue);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)).Value = ctno;
                string Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias, g.DecInSet(mtns::PartInfo.fn_infoType)), g.ConvertInSet(new List<string>(infoTypes)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.PartInfo.fn_infoValue)));
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

        public IList<IMES.FisObject.Common.Part.PartInfo> GetPartInfoList(IMES.FisObject.Common.Part.PartInfo condition)
        {
            try
            {
                IList<IMES.FisObject.Common.Part.PartInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.PartInfo cond = FuncNew.SetColumnFromField<mtns.PartInfo, IMES.FisObject.Common.Part.PartInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<mtns.PartInfo>(null, null, new ConditionCollection<mtns.PartInfo>(new EqualCondition<mtns.PartInfo>(cond)), mtns.PartInfo.fn_partNo);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.PartInfo, IMES.FisObject.Common.Part.PartInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.PartInfo, IMES.FisObject.Common.Part.PartInfo, IMES.FisObject.Common.Part.PartInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<AssetRangeInfo> GetAssetRangeInfoByRangeAndId(string begin, string end, int id)
        {
            try
            {
                IList<AssetRangeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::AssetRange cond = new mtns::AssetRange();
                        cond._Begin_ = begin;
                        mtns::AssetRange cond2 = new mtns::AssetRange();
                        cond2._End_ = end;
                        mtns::AssetRange cond3 = new mtns::AssetRange();
                        cond3.id = id;
                        mtns::AssetRange cond4 = new mtns::AssetRange();
                        cond4._Begin_ = begin;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::AssetRange>(tk, null, null, new ConditionCollection<mtns::AssetRange>(
                            new NotEqualCondition<mtns::AssetRange>(cond3),
                            new GreaterOrEqualCondition<mtns::AssetRange>(cond2),
                            new SmallerOrEqualCondition<mtns::AssetRange>(cond),
                            new EqualCondition<mtns::AssetRange>(cond4, "LEN({0})", "LEN({0})")
                            ));
                    }
                }
                sqlCtx.Param(g.DecSE(mtns::AssetRange.fn__Begin_)).Value = begin;
                sqlCtx.Param(g.DecGE(mtns::AssetRange.fn__End_)).Value = end;
                sqlCtx.Param(mtns::AssetRange.fn_id).Value = id;
                sqlCtx.Param(mtns.AssetRange.fn__Begin_).Value = begin;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::AssetRange, AssetRangeInfo, AssetRangeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<AssetRangeInfo> GetAssetRangeInfoByRangeAndIdReversely(string begin, string end, int id)
        {
            try
            {
                IList<AssetRangeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::AssetRange cond = new mtns::AssetRange();
                        cond._Begin_ = begin;
                        mtns::AssetRange cond2 = new mtns::AssetRange();
                        cond2._End_ = end;
                        mtns::AssetRange cond3 = new mtns::AssetRange();
                        cond3.id = id;
                        mtns::AssetRange cond4 = new mtns::AssetRange();
                        cond4._Begin_ = begin;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::AssetRange>(tk, null, null, new ConditionCollection<mtns::AssetRange>(
                            new NotEqualCondition<mtns::AssetRange>(cond3),
                            new SmallerOrEqualCondition<mtns::AssetRange>(cond2),
                            new GreaterOrEqualCondition<mtns::AssetRange>(cond),
                            new EqualCondition<mtns::AssetRange>(cond4, "LEN({0})", "LEN({0})")
                            ));
                    }
                }
                sqlCtx.Param(g.DecGE(mtns::AssetRange.fn__Begin_)).Value = begin;
                sqlCtx.Param(g.DecSE(mtns::AssetRange.fn__End_)).Value = end;
                sqlCtx.Param(mtns::AssetRange.fn_id).Value = id;
                sqlCtx.Param(mtns.AssetRange.fn__Begin_).Value = begin;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::AssetRange, AssetRangeInfo, AssetRangeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SysSettingInfo> GetSysSettingInfoes(SysSettingInfo condition)
        {
            try
            {
                IList<SysSettingInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                SysSetting cond = FuncNew.SetColumnFromField<SysSetting, SysSettingInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<SysSetting>(null, null, new ConditionCollection<SysSetting>(new EqualCondition<SysSetting>(cond)), SysSetting.fn_id + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<SysSetting, SysSettingInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<SysSetting, SysSettingInfo, SysSettingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void AddInternalCOADefered(IUnitOfWork uow, InternalCOAInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void RemoveInternalCOADefered(IUnitOfWork uow, InternalCOAInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdatePODLabelPartDefered(IUnitOfWork uow, PODLabelPartDef obj, string partNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj, partNo);
        }

        public void AddPODLabelPartDefered(IUnitOfWork uow, PODLabelPartDef obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void DeletePODLabelPartDefered(IUnitOfWork uow, string partNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partNo);
        }

        public void AddPartTypeDefered(IUnitOfWork uow, PartTypeDef partTypeDef)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partTypeDef);
        }

        public void UpdatePartTypeDefered(IUnitOfWork uow, PartTypeDef item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePartTypeDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void DeletePartTypeDefered(IUnitOfWork uow, string partType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partType);
        }

        public void AddAssetRangeItemDefered(IUnitOfWork uow, AssetRangeInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateAssetRangeItemDefered(IUnitOfWork uow, AssetRangeInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteAssetRangeItemDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void UpdatePartByPartNoDefered(IUnitOfWork uow, PartDef obj, string partNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj, partNo);
        }

        public void AddPartDefered(IUnitOfWork uow, PartDef obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void DeletePartDefered(IUnitOfWork uow, string partNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partNo);
        }

        public void AddPartInfoDefered(IUnitOfWork uow, IMES.FisObject.Common.Part.PartInfo obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void DeletePartInfoDefered(IUnitOfWork uow, string infoType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), infoType);
        }

        public void UpdatePartInfoDefered(IUnitOfWork uow, IMES.FisObject.Common.Part.PartInfo obj, string partno, string infoType, string infoValue)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj, partno, infoType, infoValue);
        }

        public void DeletePartInfoByPNDefered(IUnitOfWork uow, string partNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partNo);
        }

        public void UpdateForceNWCByProductIDDefered(IUnitOfWork uow, string forceNwc, string preStation, string productId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), forceNwc, preStation, productId);
        }

        public void UpdateForceNWCDefered(IUnitOfWork uow, ForceNWCInfo setValue, ForceNWCInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void InsertForceNWCDefered(IUnitOfWork uow, ForceNWCInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddSysSettingInfoDefered(IUnitOfWork uow, SysSettingInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateSysSettingInfoDefered(IUnitOfWork uow, SysSettingInfo setValue, SysSettingInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddKPRepairDefered(IUnitOfWork uow, KeyPartRepairInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddKPRepairDefectDefered(IUnitOfWork uow, IMES.FisObject.Common.Repair.RepairDefect item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateKPRepairDefectDefered(IUnitOfWork uow, IMES.FisObject.Common.Repair.RepairDefect setValue, IMES.FisObject.Common.Repair.RepairDefect condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateKPRepairDefered(IUnitOfWork uow, KeyPartRepairInfo setValue, KeyPartRepairInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddConstValueDefered(IUnitOfWork uow, ConstValueInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateConstValueDefered(IUnitOfWork uow, ConstValueInfo setValue, ConstValueInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void RemoveConstValueDefered(IUnitOfWork uow, ConstValueInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }
        
        #endregion

         #region private function
        private  System.Data.DataTable CreateStringListTb()
        {
            System.Data.DataTable list = new System.Data.DataTable("TbStringList");
            list.Columns.Add("data", typeof(string));

            return list;
        }
         #endregion

        #region for FailKPCollection
        public void AddFailKPCollection(FailKPCollectionInfo item)
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
                        sqlCtx.Sentence = @"insert FailKPCollection([Date], PdLine, Family, PartName, PartNo, 
                                                                                                 Vendor, Module, FailReason, Qty, Remark, 
                                                                                                 Editor, Cdt, Udt)
                                                                        values(@Date, @PdLine, @Family, @PartName, @PartNo, 
		                                                                         @Vendor, @Module, @FailReason, @Qty, @Remark, 
		                                                                         @Editor, getdate(), getdate()) ";
                        sqlCtx.AddParam("Date", new SqlParameter("@Date", SqlDbType.DateTime));
                        sqlCtx.AddParam("PdLine", new SqlParameter("@PdLine", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartName", new SqlParameter("@PartName", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));

                        sqlCtx.AddParam("Vendor", new SqlParameter("@Vendor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Module", new SqlParameter("@Module", SqlDbType.VarChar));
                        sqlCtx.AddParam("FailReason", new SqlParameter("@FailReason", SqlDbType.VarChar));
                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Date").Value = item.Date;
                sqlCtx.Param("PdLine").Value = item.PdLine;
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("PartName").Value = item.PartName;
                sqlCtx.Param("PartNo").Value = item.PartNo;

                sqlCtx.Param("Vendor").Value = item.Vendor;
                sqlCtx.Param("Module").Value = item.Module;
                sqlCtx.Param("FailReason").Value = item.FailReason;
                sqlCtx.Param("Qty").Value = item.Qty;
                sqlCtx.Param("Remark").Value = item.Remark;

                sqlCtx.Param("Editor").Value = item.Editor;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateFailKPCollection(FailKPCollectionInfo item)
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
                        sqlCtx.Sentence = @"update 	FailKPCollection
                                                                set   [Date]=@Date,
	                                                                  PdLine=@PdLine, 
	                                                                  Family=@Family, 
	                                                                  PartName=@PartName, 
	                                                                  PartNo=@PartNo, 
                                                                      Vendor=@Vendor, 
                                                                      Module=@Module, 
                                                                      FailReason=@FailReason, 
                                                                      Qty=@Qty, 
                                                                      Remark=@Remark, 
                                                                      Editor =@Editor, 
                                                                      Udt=GETDATE()
                                                                 where ID=@ID ";
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));
                        sqlCtx.AddParam("Date", new SqlParameter("@Date", SqlDbType.DateTime));
                        sqlCtx.AddParam("PdLine", new SqlParameter("@PdLine", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartName", new SqlParameter("@PartName", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));

                        sqlCtx.AddParam("Vendor", new SqlParameter("@Vendor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Module", new SqlParameter("@Module", SqlDbType.VarChar));
                        sqlCtx.AddParam("FailReason", new SqlParameter("@FailReason", SqlDbType.VarChar));
                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ID").Value = item.ID;
                sqlCtx.Param("Date").Value = item.Date;
                sqlCtx.Param("PdLine").Value = item.PdLine;
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("PartName").Value = item.PartName;
                sqlCtx.Param("PartNo").Value = item.PartNo;

                sqlCtx.Param("Vendor").Value = item.Vendor;
                sqlCtx.Param("Module").Value = item.Module;
                sqlCtx.Param("FailReason").Value = item.FailReason;
                sqlCtx.Param("Qty").Value = item.Qty;
                sqlCtx.Param("Remark").Value = item.Remark;

                sqlCtx.Param("Editor").Value = item.Editor;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteFailKPCollection(int ID)
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
                        sqlCtx.Sentence = @"delete  	FailKPCollection                                                             
                                                                 where ID=@ID ";
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));
                        //sqlCtx.AddParam("Date", new SqlParameter("@Date", SqlDbType.DateTime));
                        //sqlCtx.AddParam("PdLine", new SqlParameter("@PdLine", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        //sqlCtx.AddParam("PartName", new SqlParameter("@PartName", SqlDbType.VarChar));
                        //sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));

                        //sqlCtx.AddParam("Vendor", new SqlParameter("@Vendor", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Module", new SqlParameter("@Module", SqlDbType.VarChar));
                        //sqlCtx.AddParam("FailReason", new SqlParameter("@FailReason", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        //sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));

                        //sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ID").Value = ID;
                //sqlCtx.Param("Date").Value = item.Date;
                //sqlCtx.Param("PdLine").Value = item.PdLine;
                //sqlCtx.Param("Family").Value = item.Family;
                //sqlCtx.Param("PartName").Value = item.PartName;
                //sqlCtx.Param("PartNo").Value = item.PartNo;

                //sqlCtx.Param("Vendor").Value = item.Vendor;
                //sqlCtx.Param("Module").Value = item.Module;
                //sqlCtx.Param("FailReason").Value = item.FailReason;
                //sqlCtx.Param("Qty").Value = item.Qty;
                //sqlCtx.Param("Remark").Value = item.Remark;

                //sqlCtx.Param("Editor").Value = item.Editor;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<FailKPCollectionInfo> GetFailKPCollection(DateTime date, string pdLine)
        {
            try
            {
                IList<FailKPCollectionInfo> ret = new List<FailKPCollectionInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, [Date], PdLine, Family, PartName, 
                                                                        PartNo, Vendor, Module, FailReason, Qty, 
                                                                        Remark, Editor, Cdt, Udt 
                                                                from 	FailKPCollection
                                                                where [Date]=@Date and
                                                                      PdLine=@PdLine
                                                                 order by Cdt";
                        
                        sqlCtx.AddParam("Date", new SqlParameter("@Date", SqlDbType.DateTime));
                        sqlCtx.AddParam("PdLine", new SqlParameter("@PdLine", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        //sqlCtx.AddParam("PartName", new SqlParameter("@PartName", SqlDbType.VarChar));
                        //sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));

                        //sqlCtx.AddParam("Vendor", new SqlParameter("@Vendor", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Module", new SqlParameter("@Module", SqlDbType.VarChar));
                        //sqlCtx.AddParam("FailReason", new SqlParameter("@FailReason", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        //sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));

                        //sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Date").Value = date;
                sqlCtx.Param("PdLine").Value = pdLine;
                //sqlCtx.Param("Family").Value = item.Family;
                //sqlCtx.Param("PartName").Value = item.PartName;
                //sqlCtx.Param("PartNo").Value = item.PartNo;

                //sqlCtx.Param("Vendor").Value = item.Vendor;
                //sqlCtx.Param("Module").Value = item.Module;
                //sqlCtx.Param("FailReason").Value = item.FailReason;
                //sqlCtx.Param("Qty").Value = item.Qty;
                //sqlCtx.Param("Remark").Value = item.Remark;

                //sqlCtx.Param("Editor").Value = item.Editor;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<FailKPCollectionInfo>(sqlR));

                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<int> ExistInFailKPCollection(FailKPCollectionInfo item)
        {
            try
            {
                IList<int> ret = new List<int>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  ID
                                                                from 	FailKPCollection
                                                                where [Date]=@Date and
                                                                           PdLine=@PdLine and
                                                                           Family=@Family  and
                                                                           PartName=@PartName and
                                                                           PartNo=@PartNo and
                                                                           Vendor=@Vendor and
                                                                           Module=@Module and
                                                                           FailReason=@FailReason";

                        sqlCtx.AddParam("Date", new SqlParameter("@Date", SqlDbType.DateTime));
                        sqlCtx.AddParam("PdLine", new SqlParameter("@PdLine", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartName", new SqlParameter("@PartName", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));

                        sqlCtx.AddParam("Vendor", new SqlParameter("@Vendor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Module", new SqlParameter("@Module", SqlDbType.VarChar));
                        sqlCtx.AddParam("FailReason", new SqlParameter("@FailReason", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        //sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));

                        //sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Date").Value = item.Date;
                sqlCtx.Param("PdLine").Value = item.PdLine;
                sqlCtx.Param("Family").Value = item.Family;
                sqlCtx.Param("PartName").Value = item.PartName;
                sqlCtx.Param("PartNo").Value = item.PartNo;

                sqlCtx.Param("Vendor").Value = item.Vendor;
                sqlCtx.Param("Module").Value = item.Module;
                sqlCtx.Param("FailReason").Value = item.FailReason;
                //sqlCtx.Param("Qty").Value = item.Qty;
                //sqlCtx.Param("Remark").Value = item.Remark;

                //sqlCtx.Param("Editor").Value = item.Editor;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {

                        ret.Add(sqlR.GetInt32(0));
                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }


        #region AssetRange
        public IList<AssetRangeCodeInfo> GetDuplicateAssetRange(string code, string begin, string end)
        {
            try
            {
                IList<AssetRangeCodeInfo> ret = new List<AssetRangeCodeInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"SELECT ID,[Begin],[End],Status
                                                            FROM AssetRange 
                                                            WHERE [Begin]<=@Begin AND      
                                                                  [End]>=@Begin AND 
                                                                  [Code]=@Code
                                                            union
                                                            SELECT ID,[Begin],[End],Status
                                                            FROM AssetRange 
                                                            WHERE [Begin]<=@End AND      
                                                                  [End]>=@End AND 
                                                                  [Code]=@Code       
                                                            union      
                                                            SELECT ID,[Begin],[End],Status
                                                            FROM AssetRange 
                                                            WHERE [Begin]>=@Begin AND       
                                                                  [End]<=@End AND 
                                                                  [Code]=@Code";

                        sqlCtx.AddParam("Code", new SqlParameter("@Code", SqlDbType.VarChar));
                        sqlCtx.AddParam("Begin", new SqlParameter("@Begin", SqlDbType.VarChar));
                        sqlCtx.AddParam("End", new SqlParameter("@End", SqlDbType.VarChar));                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Code").Value = code;
                sqlCtx.Param("Begin").Value = begin;
                sqlCtx.Param("End").Value = end;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        AssetRangeCodeInfo item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<AssetRangeCodeInfo>(sqlR);
                        //IMES.Infrastructure.Repository._Schema.SQLData.TrimStringProperties(item);
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
        public int GetAssetRangeLength(string code)
        {
            try
            {
               int ret = -1;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select top 1 len([Begin]) as length
                                                            from AssetRange 
                                                            where [Code]=@Code";

                        sqlCtx.AddParam("Code", new SqlParameter("@Code", SqlDbType.VarChar));
                      
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Code").Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetInt32(0);                        
                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<AssetRangeInfo> GetAssetRangeByCode(string code)
        {
            try
            {
                IList<AssetRangeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::AssetRange cond = new mtns::AssetRange();
                        cond.code = code;
                        
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::AssetRange>(tk, null, null, 
                            new ConditionCollection<mtns::AssetRange>(new EqualCondition<mtns::AssetRange>(cond)),
                            mtns::AssetRange.fn__Begin_,mtns::AssetRange.fn_status);
                    }
                }

                sqlCtx.Param(mtns::AssetRange.fn_code).Value = code;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, 
                                                                                                                            CommandType.Text, 
                                                                                                                            sqlCtx.Sentence, 
                                                                                                                            sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::AssetRange, AssetRangeInfo, AssetRangeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<string> GetCodeListInAssetRange()
        {
            try
            {
                IList<string> ret = new List<string>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct [Code]
                                                            from AssetRange";                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add( sqlR.GetString(0).Trim());
                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public AssetRangeCodeInfo GetAssetRangeByStatus(string code, string[] status)
        {
            try
            {
                AssetRangeCodeInfo ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"SELECT Top 1 ID, [Begin],[End],Status
                                                            FROM AssetRange WITH (INDEX=IDX_Asset_Code_Status,ROWLOCK,UPDLOCK)
                                                            WHERE [Code]=@Code and
                                                                           Status in ({0})
                                                            ORDER BY Status, [Begin]";

                        sqlCtx.AddParam("Code", new SqlParameter("@Code", SqlDbType.VarChar));
                       
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Code").Value = code;
                string statusStr="'"+string.Join("','",status) +"'";
                string sqlStr = string.Format(sqlCtx.Sentence, statusStr);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                                             CommandType.Text,
                                                                                                                                             sqlStr,
                                                                                                                                             sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<AssetRangeCodeInfo>(sqlR);
                        //IMES.Infrastructure.Repository._Schema.SQLData.TrimStringProperties(ret);                    
                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SetAssetRangeStatus(int id, string status)
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
                        sqlCtx.Sentence = @"Update AssetRange
                                                             Set Status=@Status
                                                             where  ID=@ID";

                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Status").Value = status;
                sqlCtx.Param("ID").Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);

                

            }
            catch (Exception)
            {
                throw;
            }

        }
        public void SetAssetRangeStatusDefered(IUnitOfWork uow, int id, string status)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id,status);
        }
        #endregion

        public void DeleteSysSettingInfo(int id)
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
                        SysSetting  cond = new SysSetting();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.SysSetting>(tk, new ConditionCollection<_Metas.SysSetting>(new EqualCondition<_Metas.SysSetting>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.SysSetting.fn_id).Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, 
                                                                               CommandType.Text, 
                                                                               sqlCtx.Sentence, 
                                                                               sqlCtx.Params);   
              
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public IList<CheckItemTypeInfo> GetCheckItemType()
        {
            try
            {
                IList<CheckItemTypeInfo> ret = new List<CheckItemTypeInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                       sqlCtx = FuncNew.GetCommonSelect<_Metas.CheckItemType>(tk, _Metas.CheckItemType.fn_name);
                        
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, 
                                                                                                                             CommandType.Text, 
                                                                                                                             sqlCtx.Sentence, 
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::CheckItemType, CheckItemTypeInfo, CheckItemTypeInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public IList<CheckItemTypeInfo> GetCheckItemType(CheckItemTypeInfo condition)
        {
            try
            {
                IList<CheckItemTypeInfo> ret = new List<CheckItemTypeInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                   // if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   // {
                        CheckItemType cond = FuncNew.SetColumnFromField<CheckItemType, CheckItemTypeInfo>(condition);

                        sqlCtx = FuncNew.GetConditionedSelect<CheckItemType>(null, null,
                                                                                                                                new ConditionCollection<CheckItemType>(new EqualCondition<CheckItemType>(cond)), 
                                                                                                                               _Metas.CheckItemType.fn_name);

                   // }
                }
                sqlCtx = FuncNew.SetColumnFromField<CheckItemType, CheckItemTypeInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::CheckItemType, CheckItemTypeInfo, CheckItemTypeInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public void UpdateCheckItemType(CheckItemTypeInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    //if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    //{
                        CheckItemType cond = new CheckItemType();
                        cond.name = item.name;
                        CheckItemType setv = FuncNew.SetColumnFromField<CheckItemType, CheckItemTypeInfo>(item, CheckItemType.fn_name);
                        setv.udt = DateTime.Now;
                    
                        sqlCtx = FuncNew.GetConditionedUpdate<CheckItemType>( new SetValueCollection<CheckItemType>(new CommonSetValue<CheckItemType>(setv)), 
                                                                                                                       new ConditionCollection<CheckItemType>(new EqualCondition<CheckItemType>(cond)));
                  //}
              }
               
                 sqlCtx.Param(CheckItemType.fn_name).Value = item.name;

                 sqlCtx = FuncNew.SetColumnFromField<CheckItemType, CheckItemTypeInfo>(sqlCtx,item,true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(CheckItemType.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, 
                                                                                 CommandType.Text, 
                                                                                 sqlCtx.Sentence, 
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertCheckItemType(CheckItemTypeInfo item)
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
                        sqlCtx = FuncNew.GetCommonInsert<CheckItemType>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<CheckItemType, CheckItemTypeInfo>(sqlCtx, item);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                  sqlCtx.Sentence,
                                                                                  sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteCheckItemType(string name)
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
                        CheckItemType cond = new CheckItemType();
                        cond.name= name;

                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.CheckItemType>(tk, new ConditionCollection<_Metas.CheckItemType>(new EqualCondition<_Metas.CheckItemType>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.CheckItemType.fn_name).Value = name;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                               CommandType.Text,
                                                                               sqlCtx.Sentence,
                                                                               sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }           
        }
        #endregion

        #region for AssemblyVC
        public IList<AssemblyVCInfo> GetAssemblyVC(AssemblyVCInfo condition)
        {
            try
            {
                IList<AssemblyVCInfo> ret = new List<AssemblyVCInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
              
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    AssemblyVC cond = FuncNew.SetColumnFromField<AssemblyVC, AssemblyVCInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<AssemblyVC>(null, null,
                                                                                                                            new ConditionCollection<AssemblyVC>(new EqualCondition<AssemblyVC>(cond)),
                                                                                                                           _Metas.AssemblyVC.fn_vc, _Metas.AssemblyVC.fn_combineVC);

                  
                }
                sqlCtx = FuncNew.SetColumnFromField<AssemblyVC, AssemblyVCInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::AssemblyVC, AssemblyVCInfo, AssemblyVCInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public void UpdateAssemblyVC(AssemblyVCInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                   
                    AssemblyVC cond = new AssemblyVC();
                    cond.id = item.id;
                    AssemblyVC setv = FuncNew.SetColumnFromField<AssemblyVC, AssemblyVCInfo>(item, AssemblyVC.fn_id);
                    setv.udt = DateTime.Now;

                    sqlCtx = FuncNew.GetConditionedUpdate<AssemblyVC>( new SetValueCollection<AssemblyVC>(new CommonSetValue<AssemblyVC>(setv)),
                                                                                                           new ConditionCollection<AssemblyVC>(new EqualCondition<AssemblyVC>(cond)));
                   
                }

                sqlCtx.Param(AssemblyVC.fn_id).Value = item.id;

                sqlCtx = FuncNew.SetColumnFromField<AssemblyVC, AssemblyVCInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(AssemblyVC.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertAssemblyVC(AssemblyVCInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<AssemblyVC>(tk);
                       
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<AssemblyVC, AssemblyVCInfo>(sqlCtx, item);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                  sqlCtx.Sentence,
                                                                                  sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAssemblyVC(long id)
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
                        AssemblyVC cond = new AssemblyVC();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.AssemblyVC>(tk, new ConditionCollection<_Metas.AssemblyVC>(new EqualCondition<_Metas.AssemblyVC>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.AssemblyVC.fn_id).Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                               CommandType.Text,
                                                                               sqlCtx.Sentence,
                                                                               sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }           
        }
        #endregion

		#region PartForbid
        public IList<PartForbidRuleInfo> GetPartForbid(PartForbidRuleInfo condition)
        {
            try
            {
                IList<PartForbidRuleInfo> ret = new List<PartForbidRuleInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    PartForbidRule cond = FuncNew.SetColumnFromField<PartForbidRule, PartForbidRuleInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<PartForbidRule>(null, null,
                                                                                                     new ConditionCollection<PartForbidRule>(new EqualCondition<PartForbidRule>(cond)),
                                                                                                    _Metas.PartForbidRule.fn_customer, _Metas.PartForbidRule.fn_line, _Metas.PartForbidRule.fn_family);


                }
                sqlCtx = FuncNew.SetColumnFromField<PartForbidRule, PartForbidRuleInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::PartForbidRule, PartForbidRuleInfo, PartForbidRuleInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddPartForbid(PartForbidRuleInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<PartForbidRule>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<PartForbidRule, PartForbidRuleInfo>(sqlCtx, item);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                  sqlCtx.Sentence,
                                                                                  sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdatePartForbid(PartForbidRuleInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    PartForbidRule cond = new PartForbidRule();
                    cond.id = item.ID;
                    PartForbidRule setv = FuncNew.SetColumnFromField<PartForbidRule, PartForbidRuleInfo>(item, PartForbidRule.fn_id);
                    setv.udt = DateTime.Now;

                    sqlCtx = FuncNew.GetConditionedUpdate<PartForbidRule>(new SetValueCollection<PartForbidRule>(new CommonSetValue<PartForbidRule>(setv)),
                                                                                                              new ConditionCollection<PartForbidRule>(new EqualCondition<PartForbidRule>(cond)));

                }

                sqlCtx.Param(PartForbidRule.fn_id).Value = item.ID;

                sqlCtx = FuncNew.SetColumnFromField<PartForbidRule, PartForbidRuleInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(PartForbidRule.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeletePartForbid(long id)
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
                        PartForbidRule cond = new PartForbidRule();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.PartForbidRule>(tk, new ConditionCollection<_Metas.PartForbidRule>(new EqualCondition<_Metas.PartForbidRule>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.PartForbidRule.fn_id).Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                               CommandType.Text,
                                                                               sqlCtx.Sentence,
                                                                               sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public IList<PartForbidPriorityInfo> GetPartForbidPriority(string customer, string line, string family, string model, string productId, string status)
        {
            try
            {
                IList<PartForbidPriorityInfo> ret = new List<PartForbidPriorityInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID,(case when a.Line=@Line and a.Family=@ProductId then 1
                                                                        when a.Line=@Line and a.Family=@Model then 2 
                                                                        when a.Line=@Line and a.Family=@Family then 3
                                                                        when a.Line='' and a.Family=@ProductId then 4
                                                                        when a.Line='' and a.Family=@Model then 5 
                                                                        when a.Line='' and a.Family=@Family then 6
                                                                        when a.Line=@Line and a.Family='' then 7
                                                                        else 8                  
                                                                   end) as Priority,
                                                               a.Line,a.Family,a.Category,  
                                                               a.ExceptModel, a.BomNodeType, a.VendorCode, a.PartNo, 
                                                               a.NoticeMsg
                                                        from PartForbidRule a
                                                        where a.Customer=@Customer and
                                                              (a.Line ='' or Line=@Line) and
                                                               (a.Family='' or a.Family=@Family or a.Family=@Model or a.Family=@ProductId) and
                                                               a.Status=@Status
                                                        order by Priority";

                        sqlCtx.AddParam("Customer", new SqlParameter("@Customer", SqlDbType.VarChar));
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("ProductId", new SqlParameter("@ProductId", SqlDbType.VarChar));

                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Customer").Value = customer;
                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Family").Value = family;
                sqlCtx.Param("Model").Value = model;
                sqlCtx.Param("ProductId").Value = productId;

                sqlCtx.Param("Status").Value = status;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<PartForbidPriorityInfo>(sqlR));

                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PartForbidPriorityInfo> GetPartForbidWithFirstPriority(string customer, string pdLine, string family, string model, string productId)
        {
            IList<PartForbidPriorityInfo> ret = new List<PartForbidPriorityInfo>();
            ILineRepository lineRep = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
            //IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
           
            string aliasLine = "<NULL>";
            if (!string.IsNullOrEmpty(pdLine))
            {
                IMES.FisObject.Common.Line.Line line = lineRep.Find(pdLine);
                if (line!=null && 
                    line.LineEx != null && 
                    !string.IsNullOrEmpty(line.LineEx.AliasLine))
                {
                    //throw new FisException("IDL001", new List<string>());
                    aliasLine = line.LineEx.AliasLine.Trim();
                }
            }

            family = string.IsNullOrEmpty(family) ? "<NULL>" : family;
            model = string.IsNullOrEmpty(model) ? "<NULL>" : model;
            productId = string.IsNullOrEmpty(productId) ? "<NULL>" : productId;

            IList<PartForbidPriorityInfo> priorityInfo = GetPartForbidPriority(customer, aliasLine, family, model, productId, "Enable");
            if (priorityInfo.Count > 0)
            {
                int priority = priorityInfo.Min(x => x.Priority);
                ret = priorityInfo.Where(x => x.Priority == priority).ToList();
            }
            return ret.Count == 0 ? ret : ret.Where(x =>
            {
                if (string.IsNullOrEmpty(x.ExceptModel))
                {
                    return true;
                }
                if (x.Category == "Allow")  //允許，檢查例外禁用機型
                {
                    if (model != "<NULL>" && Regex.IsMatch(model, x.ExceptModel))
                    {
                        x.Category = "Forbid";
                    }
                    return true;
                }
                else   //禁用,檢查例外允許機型
                {
                    return model != "<NULL>" && !Regex.IsMatch(model, x.ExceptModel);
                }
            }).ToList();
        }


        public bool CheckPartForbid(IList<PartForbidPriorityInfo> partForbidList,
                                                  string bomNodeType,
                                                  string vendorCode,
                                                  string partNo,
                                                  out string noticeMsg)
        {
            bool ret = false;
            noticeMsg = "";
            if (partForbidList != null && partForbidList.Count > 0)
            {
                //1.禁用類型
                var forbidden = partForbidList.Where(y => y.Category != "Allow")
                                                            .FirstOrDefault(x => checkVendorCodeOrPartNo(bomNodeType, x.BomNodeType,
                                                                                                                                     vendorCode, x.VendorCode,
                                                                                                                                     partNo, x.PartNo));

                if (forbidden != null)
                {
                    noticeMsg = forbidden.NoticeMsg;
                    ret = true;
                }
                else  //允許類別
                {
                    var allow = partForbidList.Where(y => y.Category == "Allow");
                    if (allow.Count() > 0)
                    {
                        //任何符合條件都是允許結合
                        bool hasAllow = allow.Any(x => checkVendorCodeOrPartNo(bomNodeType, x.BomNodeType,
                                                                                                                                         vendorCode, x.VendorCode,
                                                                                                                                         partNo, x.PartNo));
                        if (!hasAllow)
                        {
                            //任何BomNodeType 相同狀況裡，不符合條件都是不允許結合
                            var forbiddenAllow = allow.FirstOrDefault(x => checkNotMatchVendorCodeOrPartNo(bomNodeType, x.BomNodeType,
                                                                                                                                                           vendorCode, x.VendorCode,
                                                                                                                                                           partNo, x.PartNo));
                            if (forbiddenAllow != null)
                            {
                                noticeMsg = forbiddenAllow.NoticeMsg;
                                ret = true;
                            }
                        }
                    }
                }


            }
            return ret;
        }


        private bool checkVendorCodeOrPartNo(string bomNodeType, string bomNodeTypeRule,
                                                                    string vendorCode, string vendorCodeRule,
                                                                    string partNo, string partNoRule)
        {
            // 檢查符合條件:BomNodeType 、VendorCode、PartNo  三條件都成立
            if (bomNodeType == bomNodeTypeRule)
            {
                if (!string.IsNullOrEmpty(vendorCodeRule) && !string.IsNullOrEmpty(partNoRule))
                {
                    return Regex.IsMatch(vendorCode, vendorCodeRule) && Regex.IsMatch(partNo, partNoRule);
                }
                else if (!string.IsNullOrEmpty(vendorCodeRule))
                {
                    return Regex.IsMatch(vendorCode, vendorCodeRule);
                }
                else if (!string.IsNullOrEmpty(partNoRule))
                {
                    return Regex.IsMatch(partNo, partNoRule);
                }
                else
                {
                    return true;
                }
            }
            return false;

        }

        private bool checkNotMatchVendorCodeOrPartNo(string bomNodeType, string bomNodeTypeRule,
                                                                     string vendorCode, string vendorCodeRule,
                                                                     string partNo, string partNoRule)
        {
            // 檢查符合條件:
            //在BomNodeType符合條件下,檢查VendorCode及PartNo  二條件不都成立
            if (bomNodeType == bomNodeTypeRule)
            {
                if (!string.IsNullOrEmpty(vendorCodeRule) && !string.IsNullOrEmpty(partNoRule))
                {
                    return !(Regex.IsMatch(vendorCode, vendorCodeRule) && Regex.IsMatch(partNo, partNoRule));
                }
                else if (!string.IsNullOrEmpty(vendorCodeRule))
                {
                    return !Regex.IsMatch(vendorCode, vendorCodeRule);
                }
                else if (!string.IsNullOrEmpty(partNoRule))
                {
                    return !Regex.IsMatch(partNo, partNoRule);
                }
                else
                {
                    return false;
                }
            }
            return false;

        }

        #endregion

        #region get multi-part
        public IList<IPart> FindPart(IList<string> partNoList)
        {
            
            try
            {
                if (!IsCached())
                    return GetPart_DB(partNoList);

                IList<string> noneCachePartNoList = null;
                IList<IPart> cachePartList = Find_Cache(partNoList, out noneCachePartNoList);
                if (noneCachePartNoList != null && 
                    noneCachePartNoList.Count>0)
                {
                    IList<IPart> dbPartList = GetPart_DB(noneCachePartNoList);

                    if (dbPartList != null && dbPartList.Count>0)
                    {
                        lock (_syncObj_cache)
                        {
                            //if (!_cache.Contains((string)ret.Key))
                            //    AddToCache((string)ret.Key, ret);
                            //UnregistIndexesForOnePart(ret);
                            foreach (IPart part in dbPartList)
                            {
                                AddAndRegistOnePart(part);
                            }
                        }
                        cachePartList = cachePartList.Concat(dbPartList).ToList();
                    }
                }
                return cachePartList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<IPart> Find_Cache(IList<string> partNoList, out IList<string> noneCachePartNoList)
        {
            try
            {
                IList<IPart> ret = new List<IPart>();
                noneCachePartNoList = new List<string>();
                lock (_syncObj_cache)
                {
                    foreach (string key in partNoList)
                    {
                        IPart part = null;
                        if (_cache.Contains(key))
                        {
                           part= (IPart)_cache[key];
                            if (part != null)
                            {
                                ret.Add(part);
                            }
                            else
                            {
                                _cache.Remove(key);
                                noneCachePartNoList.Add(key);
                            }
                        }
                        else
                        {
                            noneCachePartNoList.Add(key);
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
        private IList<IPart> GetPart_DB(IList<string> partNoList)
        {
            try
            {
                IList<IPart> ret = new List<IPart>();

                //PartDef retObj = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Part_NEW inCond = new mtns::Part_NEW();
                        inCond.partNo = "INSET";

                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Part_NEW>(tk, null, null,
                                                                                                                    new ConditionCollection<mtns::Part_NEW>(new InSetCondition<mtns::Part_NEW>(inCond),
                                                                                                                     new EqualCondition<mtns::Part_NEW>(cond)));

                        sqlCtx.Param(mtns::Part_NEW.fn_flag).Value = cond.flag;
                    }
                }

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::Part_NEW.fn_partNo), g.ConvertInSet(partNoList));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                    CommandType.Text,
                                                                                                                    Sentence,
                                                                                                                    sqlCtx.Params))
                {
                     IList<PartDef> retObjList = null;
                    retObjList = FuncNew.SetFieldFromColumn<mtns::Part_NEW, PartDef, PartDef>(retObjList, sqlR, sqlCtx);
                    if (retObjList != null && retObjList.Count > 0)
                    {
                        foreach (PartDef retObj in retObjList)
                        {
                            ret.Add(new IMES.FisObject.Common.Part.Part(
                                    retObj.partNo,
                                    retObj.bomNodeType,
                                    retObj.partType,
                                    retObj.custPartNo,
                                    retObj.descr,
                                    retObj.remark,
                                    retObj.autoDL,
                                    retObj.editor,
                                    retObj.cdt,
                                    retObj.udt,
                                    null)
                                    );
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

        #region remove Part Cache
        /// <summary>
        /// for Remove Cache item 
        /// </summary>
        /// <param name="partNoList"></param>
        public void RemoveCacheByKeyList(IList<string> partNoList)
        {
            try
            {
                lock (_syncObj_cache)
                {
                    foreach (string pk in partNoList)
                    {
                        if (_cache.Contains(pk))
                        {
                            _cache.Remove(pk);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
        }
        #endregion


        #region for maintain Part
        public void CopyPart(string scrPartNo, string destPartNo, int flag, string editor)
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
                        sqlCtx.Sentence = @"insert Part(PartNo, Descr, BomNodeType, PartType, CustPartNo, 
			                                                             AutoDL, Remark, Flag, Editor, Cdt, Udt)
                                                            select @DestPartNo, Descr, BomNodeType, PartType, CustPartNo, 
		                                                            AutoDL, Remark, @Flag, @Editor, GETDATE(), GETDATE() 
                                                            from Part
                                                            where PartNo =@PartNo

                                                            insert PartInfo(PartNo, InfoType, InfoValue, Editor, Cdt, Udt)
                                                            select @DestPartNo, InfoType, InfoValue, @Editor, GETDATE(), GETDATE() 
                                                             from  PartInfo
                                                             where PartNo =@PartNo ";
                        sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("DestPartNo", new SqlParameter("@DestPartNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Flag", new SqlParameter("@Flag", SqlDbType.Int));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PartNo").Value = scrPartNo;
                sqlCtx.Param("DestPartNo").Value = destPartNo;
                sqlCtx.Param("Flag").Value = flag;
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
        public void CopyPartlDefered(IUnitOfWork uow, string scrPartNo, string destPartNo, int flag, string editor)
        {
            Action deferAction = () => { CopyPart(scrPartNo, destPartNo, flag, editor); };
            AddOneInvokeBody(uow, deferAction);
        }
        #endregion
    }
}
