// 2010-01-26 Liu Dong(eB1-4)         Modify ITC-1122-0244
// 2010-05-19 Liu Dong(eB1-4)         Modify ITC-1155-0119

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using IMES.Infrastructure.Utility.Cache;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Util;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Part;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: AssemblyCode相关
    /// </summary>
    public class AssemblyCodeRepository : BaseRepository<fons::AssemblyCode>, IAssemblyCodeRepository//, ICache
    {
        #region Cache

        private static CacheManager _cache_real = null;
        private static CacheManager _cache
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = CacheFactory.GetCacheManager("AssemblyCodeCache");
                return _cache_real;
            }
        }

        private static IDictionary<string, IList<string>> _byWhateverIndex = new Dictionary<string, IList<string>>();
        private static string preStr1 = _Schema.Func.MakeKeyForIdxPre(_Schema.AssemblyCode.fn_assemblyCode);
        private static string preStr2 = _Schema.Func.MakeKeyForIdxPre(_Schema.AssemblyCode.fn_PartNo);
        private static object _syncObj_cache = new object();

        /// <summary>
        /// 记录方法是否被调用过一次
        /// </summary>
        private static IList<int> _calledMethods = new List<int>();

        #endregion
        
        #region Link To Other
        private static IModelRepository _mdlRepository = null;
        private static IModelRepository MdlRepository
        {
            get
            {
                if (_mdlRepository == null)
                    _mdlRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
                return _mdlRepository;
            }
        }
        #endregion

        #region Overrides of BaseRepository<IPart>

        protected override void PersistNewItem(fons::AssemblyCode item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    PersistInsertAssemblyCode(item);

                    //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.Key.ToString()));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(fons::AssemblyCode item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    PersistUpdateAssemblyCode(item);

                    //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.Key.ToString()));
                }
            }
            finally
            {
                tracker.Clear();
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }

        protected override void PersistDeletedItem(fons::AssemblyCode item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    PersistDeleteAssemblyCode(item);

                    //DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.Key.ToString()));
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<AssemblyCode>

        public override fons::AssemblyCode Find(object key)
        {
            try
            {
                //if (!IsCached())
                    return Find_DB(key);

                //fons::AssemblyCode ret = Find_Cache(key);
                //if (ret == null)
                //{
                //    ret = Find_DB(key);

                //    if (ret != null)
                //    {
                //        lock (_syncObj_cache)
                //        {
                //            UnregistIndexesForOneAssemblyCode(ret);
                //            AddAndRegistOneAssemblyCode(ret);
                //        }
                //    }
                //}
                //return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IList<fons::AssemblyCode> FindAll()
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons::AssemblyCode item = new fons::AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                                                                        GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                            item.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                            item.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
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

        public override void Add(fons::AssemblyCode item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        public override void Remove(fons::AssemblyCode item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        public override void Update(fons::AssemblyCode item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region IAssemblyCodeRepository Members

        public IList<fons::AssemblyCode> FindAssemblyCode(string assCode)
        {
            try
            {
                //if (!IsCached())
                    return FindAssemblyCode_DB(assCode);

                //bool isAccessDB = true;

                //IList<fons::AssemblyCode> ret = null;

                //if (IsFirstCalled(MethodBase.GetCurrentMethod().MetadataToken))
                //{
                //    isAccessDB = false;
                //    try
                //    {
                //        ret = FindAssemblyCode_Cache(assCode);
                //        if (ret == null || ret.Count < 1)
                //        {
                //            isAccessDB = true;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        if (ex.Message == ItemDirty)
                //            isAccessDB = true;
                //        else
                //            throw ex;
                //    }
                //}

                //if (isAccessDB)
                //{
                //    ret = FindAssemblyCode_DB(assCode);

                    //if (IsCached() && ret != null && ret.Count > 0)
                    //{
                    //    lock (_syncObj_cache)
                    //    {
                    //        foreach (fons::AssemblyCode entry in ret)
                    //        {
                    //            UnregistIndexesForOneAssemblyCode(entry);
                    //            AddAndRegistOneAssemblyCode(entry);
                    //        }
                    //    }
                    //}
                //}
                //return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAssemblyCodeInfo(string assemblyCode, string infoType)
        {
            try
            {
                string ret = string.Empty;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCodeInfo cond = new _Schema.AssemblyCodeInfo();
                        cond.AssemblyCode = assemblyCode;
                        cond.InfoType = infoType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCodeInfo), null, new List<string>() { _Schema.AssemblyCodeInfo.fn_InfoValue }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_AssemblyCode].Value = assemblyCode;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_InfoType].Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCodeInfo.fn_InfoValue]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetAssemblyCodesByModel(string model)
        {
            // 2010-05-19 Liu Dong(eB1-4)         Modify ITC-1155-0119
            //select distinct AssemblyCode from IMES_GetData..AssemblyCode where PartNo IN (Select Value from IMES_GetData..ModelInfo where Name='PN' and Model='')
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT DISTINCT {5} FROM {1} " +
                                            "WHERE {6} IN (SELECT {2} FROM {0} WHERE {3}=@{3} AND {4}=@t1_{4})";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelInfo).Name,
                                                                         typeof(_Schema.AssemblyCode).Name,
                                                                         _Schema.ModelInfo.fn_Value,
                                                                         _Schema.ModelInfo.fn_Name,//
                                                                         _Schema.ModelInfo.fn_Model, //
                                                                         _Schema.AssemblyCode.fn_assemblyCode,
                                                                         _Schema.AssemblyCode.fn_PartNo);//

                        sqlCtx.Params.Add(_Schema.ModelInfo.fn_Name, new SqlParameter("@" + _Schema.ModelInfo.fn_Name, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.Func.DecAlias("t1", _Schema.ModelInfo.fn_Model), new SqlParameter("@" + _Schema.Func.DecAlias("t1", _Schema.ModelInfo.fn_Model), SqlDbType.VarChar));
                        //sqlCtx.Params.Add(_Schema.Func.DecAlias("t2", _Schema.AssemblyCode.fn_Model), new SqlParameter("@" + _Schema.Func.DecAlias("t2", _Schema.AssemblyCode.fn_Model), SqlDbType.VarChar));

                        sqlCtx.Params[_Schema.ModelInfo.fn_Name].Value = "PN";

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.Func.DecAlias("t1", _Schema.ModelInfo.fn_Model)].Value = model;
                //sqlCtx.Params[_Schema.Func.DecAlias("t2", _Schema.AssemblyCode.fn_Model)].Value = model;

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

        public IList<string> GetAssemblyCodesByPartNo(string partNo)
        {
            //select distinct AssemblyCode from AssemblyCode where PartNo=''
            try
            {
                IList<string> ret = new List<string>();

                IList<fons::AssemblyCode> result = GetAssemblyCodesByPartNo_Inner(partNo);

                if (result != null && result.Count > 0)
                {
                    ret = (from ass in result orderby ass.AssCode select ass.AssCode).Distinct().ToList();
                }

                #region OLD
                //_Schema.SQLContext sqlCtx = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                //    {
                //        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                //        cond.PartNo = partNo;
                //        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "DISTINCT", new List<string>() { _Schema.AssemblyCode.fn_assemblyCode }, cond, null, null, null, null, null, null, null);
                //    }
                //}
                //sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                //    if (sqlR != null)
                //    {
                //        while (sqlR.Read())
                //        {
                //            string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]);
                //            ret.Add(item);
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

        public IList<fons::AssemblyCode> GetAssemblyCodeListByPartNo(string partNo)
        {
            try
            {
                return GetAssemblyCodesByPartNo_Inner(partNo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons::AssemblyCode> GetAssemblyCodesByPartNo_Inner(string partNo)
        {
            try
            {
                //if (!IsCached())
                    return GetAssemblyCodesByPartNo_DB(partNo);

                //bool isAccessDB = true;

                //IList<fons::AssemblyCode> ret = null;

                //if (IsFirstCalled(MethodBase.GetCurrentMethod().MetadataToken))
                //{
                //    isAccessDB = false;
                //    try
                //    {
                //        ret = GetAssemblyCodesByPartNo_Cache(partNo);
                //        if (ret == null || ret.Count < 1)
                //        {
                //            isAccessDB = true;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        if (ex.Message == ItemDirty)
                //            isAccessDB = true;
                //        else
                //            throw ex;
                //    }
                //}

                //if (isAccessDB)
                //{
                //    ret = GetAssemblyCodesByPartNo_DB(partNo);

                //    if (IsCached() && ret != null && ret.Count > 0)
                //    {
                //        lock (_syncObj_cache)
                //        {
                //            foreach (fons::AssemblyCode entry in ret)
                //            {
                //                UnregistIndexesForOneAssemblyCode(entry);
                //                AddAndRegistOneAssemblyCode(entry);
                //            }
                //        }
                //    }
                //}
                //return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Inners .

        private fons::AssemblyCode Find_DB(object key)
        {
            try
            {
                fons::AssemblyCode ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                        cond.ID = Convert.ToInt32(key);
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_ID].Value = Convert.ToInt32(key); ;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons::AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                        ret.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                        ret.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
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

        private fons::AssemblyCode Find_Cache(object key)
        {
            try
            {
                fons::AssemblyCode ret = null;
                lock (_syncObj_cache)
                {
                    if (_cache.Contains(key.ToString()))
                        ret = (fons::AssemblyCode)_cache[key.ToString()];
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertAssemblyCode(fons::AssemblyCode item)
        {
            try
            {
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
                sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = item.Pn;
                sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = item.Region;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateAssemblyCode(fons::AssemblyCode item)
        {
            try
            {
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
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteAssemblyCode(fons::AssemblyCode item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode));
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons::AssemblyCode> FindAssemblyCode_DB(string assCode)
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                        cond.assemblyCode = assCode;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "DISTINCT", null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_assemblyCode].Value = assCode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons::AssemblyCode item = new fons::AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                            item.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                            item.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
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

        public IList<fons::AssemblyCode> FindAssemblyCode_Cache(string assCode)
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();
                string key = _Schema.Func.MakeKeyForIdx(preStr1, assCode);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (string pk in _byWhateverIndex[key])
                        {
                            if (_cache.Contains(pk))
                                ret.Add((fons::AssemblyCode)_cache[pk]);
                            else
                                throw new Exception(ItemDirty);//缺失Item,应该是 ICacheItemRefreshAction 拿掉的, 通知外面来重新拿DB.
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

        public IList<fons::AssemblyCode> GetAssemblyCodesByPartNo_DB(string partNo)
        {
            //select distinct AssemblyCode from AssemblyCode where PartNo=''
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                        cond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "DISTINCT", null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons::AssemblyCode item = new fons::AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                            item.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                            item.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
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

        public IList<fons::AssemblyCode> GetAssemblyCodesByPartNo_Cache(string partNo)
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();
                string key = _Schema.Func.MakeKeyForIdx(preStr2, partNo);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (string pk in _byWhateverIndex[key])
                        {
                            if (_cache.Contains(pk))
                                ret.Add((fons::AssemblyCode)_cache[pk]);
                            else
                                throw new Exception(ItemDirty);//缺失Item,应该是 ICacheItemRefreshAction 拿掉的, 通知外面来重新拿DB.
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

        #region ICache Members
        /*
        private static bool _isCached = false;
        public bool IsCached()
        {
            lock (MethodBase.GetCurrentMethod())
            {
                bool configVal = DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.AssemblyCode);
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

        public void ProcessItem(CacheUpdateInfo item)
        {
            if (item.Type == CacheType.AssemblyCode)
                LoadOneCache(item.Item);
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
            try
            {
                lock (_syncObj_cache)
                {
                    if (_cache.Contains(pk))
                    {
                        UnregistIndexesForOneAssemblyCode((fons::AssemblyCode)_cache[pk]);
                        _cache.Remove(pk);
                    }
                    fons::AssemblyCode ass = this.Find_DB(pk);
                    if (ass != null)
                    {
                        AddAndRegistOneAssemblyCode(ass);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void AddAndRegistOneAssemblyCode(fons::AssemblyCode ass)
        {
            try
            {
                if (!_cache.Contains(ass.Key.ToString()))
                    AddToCache(ass.Key.ToString(), ass);

                //Regist
                Regist(ass.Key.ToString(), _Schema.Func.MakeKeyForIdx(preStr1, ass.AssCode));

                //Regist
                Regist(ass.Key.ToString(), _Schema.Func.MakeKeyForIdx(preStr2, ass.Pn));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void UnregistIndexesForOneAssemblyCode(fons::AssemblyCode ass)
        {
            try
            {
                string idxStr1 = _Schema.Func.MakeKeyForIdx(preStr1, ass.AssCode);
                if (_byWhateverIndex.ContainsKey(idxStr1))
                {
                    IList<string> stem = _byWhateverIndex[idxStr1];
                    if (stem != null)
                    {
                        if (stem.Contains(ass.Key.ToString()))
                            stem.Remove(ass.Key.ToString());
                        if (stem.Count < 1)
                            _byWhateverIndex.Remove(idxStr1);
                    }
                }

                string idxStr2 = _Schema.Func.MakeKeyForIdx(preStr2, ass.Pn);
                if (_byWhateverIndex.ContainsKey(idxStr2))
                {
                    IList<string> stem = _byWhateverIndex[idxStr2];
                    if (stem != null)
                    {
                        if (stem.Contains(ass.Key.ToString()))
                            stem.Remove(ass.Key.ToString());
                        if (stem.Count < 1)
                            _byWhateverIndex.Remove(idxStr2);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Regist(string pk, string key)
        {
            try
            {
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
                throw;
            }
        }

        private CacheUpdateInfo GetACacheSignal(string pk)
        {
            CacheUpdateInfo ret = new CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = CacheType.AssemblyCode;
            ret.Item = pk;
            return ret;
        }

        private static void AddToCache(string key, object obj)
        {
            _cache.Add(key, obj, CacheItemPriority.Normal, new AssemblyCodeRefreshAction(), new SlidingTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_AssemblyCodeCache"].ToString()))));
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
            lock (_calledMethods)
            {
                _calledMethods.Clear();
            }
        }

        [Serializable]
        private class AssemblyCodeRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {

            }
        }
        */
        #endregion

        #region For Maintain

        public IList<fons::AssemblyCode> GetAssemblyCodeList(string partNo)
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                        cond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null);

                        //按Family、Model、Region列的字符序排序
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.AssemblyCode.fn_Family, _Schema.AssemblyCode.fn_Model, _Schema.AssemblyCode.fn_Region }));
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons::AssemblyCode item = new fons::AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                            item.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                            item.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
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

        public IList<int> CheckExistedAssemblyCode(string partNo, string assemblyCode)
        {
            try
            {
                IList<int> ret = new List<int>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                        cond.PartNo = partNo;
                        cond.assemblyCode = assemblyCode;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), null, new List<string>() { _Schema.AssemblyCode.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
                sqlCtx.Params[_Schema.AssemblyCode.fn_assemblyCode].Value = assemblyCode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            int item = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
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

        public IList<fons::AssemblyCode> GetAssemblyCodeList(string model, string partNo)
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();

                // 规则:
                // 1、	取得该Model的Family和Region栏位的数据。
                // 2、	在AssemblyCode表中搜索此PartNo和Model直接对应的AssemblyCode。若记录集不为空，则返回此结果集，放弃后续步骤。
                // 3、	在AssemblyCode表中搜索此PartNo、Family、Region和空Model直接对应的AssemblyCode。若记录集不为空，则返回此结果集，放弃后续步骤。
                // 4、	在AssemblyCode表中搜索此PartNo、Family和空Region、Model直接对应的AssemblyCode。若记录集不为空，则返回此结果集，放弃后续步骤。
                // 5、	在AssemblyCode表中搜索此PartNo和空Family、Region、Model直接对应的AssemblyCode。返回此结果集。

                ret = GetAssemblyCodeList_Inner(model, partNo);

                if (ret != null && ret.Count > 0)
                    return ret;

                IMES.FisObject.Common.Model.Model mdl = MdlRepository.Find((string)model);

                if (mdl != null)
                {
                    ret = this.ExcecuteForGetAssemblyCodeList_VarCond(this.ComposeForGetAssemblyCodeList_VarCond_Inner1(string.Empty, partNo, mdl.FamilyName, mdl.Region));
                    if (ret != null && ret.Count > 0)
                        return ret;

                    ret = this.ExcecuteForGetAssemblyCodeList_VarCond(this.ComposeForGetAssemblyCodeList_VarCond_Inner2(string.Empty, partNo, mdl.FamilyName, string.Empty));
                    if (ret != null && ret.Count > 0)
                        return ret;

                    ret = this.ExcecuteForGetAssemblyCodeList_VarCond(this.ComposeForGetAssemblyCodeList_VarCond_Inner3(string.Empty, partNo, string.Empty, string.Empty));
                }

                #region OLD
                // 2010-03-08 Liu Dong(eB1-4)         Modify ITC-1136-0071
                //_Schema.SQLContext sqlCtx = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                //    {
                //        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                //        cond.Model = model;
                //        cond.PartNo = partNo;
                //        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null);

                //        //按AssemblyCode列的字符序排序
                //        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.AssemblyCode.fn_assemblyCode }));
                //    }
                //}
                //sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = model;
                //sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                //    if (sqlR != null)
                //    {
                //        while (sqlR.Read())
                //        {
                //            AssemblyCode item = new AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                //                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                //                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                //            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                //            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                //            item.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                //            item.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
                //            item.Tracker.Clear();
                //            ret.Add(item);
                //        }
                //    }
                //}
                // 2010-03-08 Liu Dong(eB1-4)         Modify ITC-1136-0071
                #endregion

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons::AssemblyCode> GetAssemblyCodeList_Inner(string model, string partNo)
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                        cond.Model = model;
                        cond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null);

                        //按AssemblyCode列的字符序排序
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.AssemblyCode.fn_assemblyCode }));
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = model;
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons::AssemblyCode item = new fons::AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                            item.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                            item.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
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

        //在AssemblyCode表中搜索此PartNo、Family、Region和空Model直接对应的AssemblyCode
        private _Schema.SQLContext ComposeForGetAssemblyCodeList_VarCond_Inner1(string model, string partNo, string family, string region)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                    cond.PartNo = partNo;
                    cond.Family = family;
                    cond.Region = region;
                    _Schema.AssemblyCode noecond = new _Schema.AssemblyCode();
                    noecond.Model = model;
                    sqlCtx = _Schema.Func.GetConditionedSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null, noecond);
                    //按AssemblyCode列的字符序排序
                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.AssemblyCode.fn_assemblyCode }));
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Family].Value = family;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = region;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = model;
            return sqlCtx;
        }

        //在AssemblyCode表中搜索此PartNo、Family和空Region、Model直接对应的AssemblyCode
        private _Schema.SQLContext ComposeForGetAssemblyCodeList_VarCond_Inner2(string model, string partNo, string family, string region)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                    cond.PartNo = partNo;
                    cond.Family = family;
                    _Schema.AssemblyCode noecond = new _Schema.AssemblyCode();
                    noecond.Region = region;
                    noecond.Model = model;
                    sqlCtx = _Schema.Func.GetConditionedSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null, noecond);
                    //按AssemblyCode列的字符序排序
                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.AssemblyCode.fn_assemblyCode }));
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Family].Value = family;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = region;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = model;
            return sqlCtx;
        }

        //在AssemblyCode表中搜索此PartNo和空Family、Region、Model直接对应的AssemblyCode
        private _Schema.SQLContext ComposeForGetAssemblyCodeList_VarCond_Inner3(string model, string partNo, string family, string region)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                    cond.PartNo = partNo;
                    _Schema.AssemblyCode noecond = new _Schema.AssemblyCode();
                    noecond.Family = family;
                    noecond.Region = region;
                    noecond.Model = model;
                    sqlCtx = _Schema.Func.GetConditionedSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), cond, null, null, noecond);
                    //按AssemblyCode列的字符序排序
                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.AssemblyCode.fn_assemblyCode }));
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Family].Value = family;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = region;
            sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = model;
            return sqlCtx;
        }

        private IList<fons::AssemblyCode> ExcecuteForGetAssemblyCodeList_VarCond(_Schema.SQLContext sqlCtx)
        {
            try
            {
                IList<fons::AssemblyCode> ret = new List<fons::AssemblyCode>();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons::AssemblyCode item = new fons::AssemblyCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_assemblyCode]),
                                                                    GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Family]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_PartNo]));

                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Editor]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Udt]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Cdt]);
                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_ID]);
                            item.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Model]);
                            item.Region = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.AssemblyCode.fn_Region]);
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

        public int CheckExistedAssemblyCode(string partNo, string family, string model, string region, string assemblyCodeId)
        {
            //如果model不为空串，where PartNo = ? And isNull(Model,'') = ?
            //如果model为空串，where PartNo = ? And isNull(Family,'') = ? And isNull(Region,'') = ? And isNull(Model,'') = ''
            try
            {
                int ret = 0;

                _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
                int i = 0;
                sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_partNo(partNo));
                if (!string.IsNullOrEmpty(assemblyCodeId))
                    sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_assemblyCode(Convert.ToInt32(assemblyCodeId)));
                if (string.IsNullOrEmpty(model))
                {
                    sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_model());
                    if (string.IsNullOrEmpty(family))
                        sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_family());
                    else
                        sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_family(family));
                    if (string.IsNullOrEmpty(region))
                        sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_region());
                    else
                        sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_region(region));
                }
                else
                {
                    sqlSet.AddOne(i++, ComposeForCheckExistedAssemblyCode_model(model));
                }
                _Schema.SQLContext sqlCtx = sqlSet.MergeToOneAndQuery();
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_partNo(string partNo)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                    cond.PartNo = partNo;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_assemblyCode(int assemblyCodeId)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode ncond = new _Schema.AssemblyCode();
                    ncond.ID = assemblyCodeId;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, null, null, null, null, null, null, null, null, ncond, null, null);
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_ID].Value = assemblyCodeId;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_family(string family)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                    cond.Family = family;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_Family].Value = family;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_family()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode noecond = new _Schema.AssemblyCode();
                    noecond.Family = string.Empty;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, null, null, null, null, null, null, null, null, noecond);
                    sqlCtx.Params[_Schema.AssemblyCode.fn_Family].Value = noecond.Family;
                }
            }
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_model(string model)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                    cond.Model = model;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = model;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_model()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode noecond = new _Schema.AssemblyCode();
                    noecond.Model = string.Empty;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, null, null, null, null, null, null, null, null, noecond);
                    sqlCtx.Params[_Schema.AssemblyCode.fn_Model].Value = noecond.Model;
                }
            }
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_region(string region)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                    cond.Region = region;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = region;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForCheckExistedAssemblyCode_region()
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.AssemblyCode noecond = new _Schema.AssemblyCode();
                    noecond.Region = string.Empty;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, null, null, null, null, null, null, null, null, noecond);
                    sqlCtx.Params[_Schema.AssemblyCode.fn_Region].Value = noecond.Region;
                }
            }
            return sqlCtx;
        }

        public void AddAssemblyCodeInfo(AssemblyCodeInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCodeInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_AssemblyCode].Value = item.AssemblyCode;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_InfoType].Value = item.InfoType;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_InfoValue].Value = item.InfoValue;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
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
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCodeInfo));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_AssemblyCode].Value = item.AssemblyCode;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_InfoType].Value = item.InfoType;
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_InfoValue].Value = item.InfoValue;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCodeInfo));
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckSameAssemblyCode(string partNo, string assemblyCode)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.AssemblyCode cond = new _Schema.AssemblyCode();
                        cond.assemblyCode = assemblyCode;
                        _Schema.AssemblyCode neqCond = new _Schema.AssemblyCode();
                        neqCond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.AssemblyCode), "COUNT", new List<string>() { _Schema.AssemblyCode.fn_ID }, cond, null, null, null, null, null, null, null, neqCond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;
                sqlCtx.Params[_Schema.AssemblyCode.fn_assemblyCode].Value = assemblyCode;
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

        public IList<string> GetPartTypeDescr(string infoType, string partNo)
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
                        sqlCtx.Sentence =   "SELECT DISTINCT {2} " +
                                            "FROM {0} " +
                                            "WHERE {3}=@{3} " +
                                            "AND ISNULL({2}, '') <> '' " +
                                            "AND {4} IN (SELECT {6} FROM {1} WHERE {5}=@{5}) " +
                                            "ORDER BY {2} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.AssemblyCodeInfo).Name,
                                                                         typeof(_Schema.AssemblyCode).Name,
                                                                         _Schema.AssemblyCodeInfo.fn_InfoValue,
                                                                         _Schema.AssemblyCodeInfo.fn_InfoType,
                                                                         _Schema.AssemblyCodeInfo.fn_AssemblyCode,
                                                                         _Schema.AssemblyCode.fn_PartNo,
                                                                         _Schema.AssemblyCode.fn_assemblyCode);

                        sqlCtx.Params.Add(_Schema.AssemblyCodeInfo.fn_InfoType, new SqlParameter("@" + _Schema.AssemblyCodeInfo.fn_InfoType, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.AssemblyCode.fn_PartNo, new SqlParameter("@" + _Schema.AssemblyCode.fn_PartNo, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.AssemblyCodeInfo.fn_InfoType].Value = infoType;
                sqlCtx.Params[_Schema.AssemblyCode.fn_PartNo].Value = partNo;

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

        public void DeleteAssemblyCodeInfoByPN(string partNo)
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
                        mtns::AssemblyCode cond = new mtns::AssemblyCode();
                        cond.partNo = partNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::AssemblyCode>(tk, new ConditionCollection<mtns::AssemblyCode>(new EqualCondition<mtns::AssemblyCode>(cond)));
                    }
                }
                sqlCtx.Param(mtns::AssemblyCode.fn_partNo).Value = partNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

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

        public void DeleteAssemblyCodeInfoByPNDefered(IUnitOfWork uow, string partNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partNo);
        }

        #endregion

        #endregion
    }
}
