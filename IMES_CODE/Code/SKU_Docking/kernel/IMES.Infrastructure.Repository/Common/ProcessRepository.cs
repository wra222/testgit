// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对Process对象的操作
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   Yuan XiaoWei                 create
// 2010-02-01   206010                       Modify ITC-1103-0077
// 2010-02-01   206010                       Modify ITC-1122-0014
// 2010-02-01   206010                       Modify ITC-1122-0015
// Known issues:

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using IMES.FisObject.Common.Process;
using fons = IMES.FisObject.Common.Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using IMES.Infrastructure.Utility;
using IMES.DataModel;
using dmns = IMES.DataModel;
using IMES.Infrastructure.Utility.Cache;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Process相关
    /// </summary>
    public class ProcessRepository : BaseRepository<fons.Process>, IProcessRepository, ICache
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        private static IDictionary<string, fons.Process> _cache = new Dictionary<string, fons.Process>();
        private static IDictionary<string, IList<string>> _byWhateverIndex = new Dictionary<string, IList<string>>();
        private static string preStr1 = _Schema.Func.MakeKeyForIdxPre(_Schema.Model_Process.fn_Model);  
        private static object _syncObj_cache = new object();
        #endregion

        #region Overrides of BaseRepository<Process>

        protected override void PersistNewItem(fons.Process item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertProcess(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        protected override void PersistUpdatedItem(fons.Process item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistModifyProcess(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        protected override void PersistDeletedItem(fons.Process item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteProcess(item);

                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Process>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.Process Find(object key)
        {
            try
            {
                if (!IsCached())
                    return Find_DB(key);

                fons.Process ret = Find_Cache(key);
                if (ret == null)
                {
                    ret = Find_DB(key);
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
        public override IList<fons.Process> FindAll()
        {
            try
            {
                if (!IsCached())
                    return FindAll_DB();

                IList<fons.Process> ret = FindAll_Cache();
                if (ret == null || ret.Count < 1)
                {
                    ret = FindAll_DB();
                }
                if (ret != null && ret.Count > 0)
                {
                    ret = (from item in ret orderby item select item).ToList();
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
        public override void Add(fons.Process item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(fons.Process item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons.Process item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region Implementation of IProcessRepository

        public fons.Process FillProcessStations(fons.Process proc)
        {
            try
            {
                FillProcessStations_DB(proc);
                return proc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons.Process> GetProcessByModel(string model)
        {
            try
            {
                if (!IsCached())
                    return GetProcessByModel_DB(model);

                IList<fons.Process> ret = GetProcessByModel_Cache(model);
                if (ret == null || ret.Count < 1)
                {
                    ret = GetProcessByModel_DB(model);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SFC(string line, string customer, string currentStation, string key, string processType)
        {
            try
            {
                string currentDB = "";
                string currentSPName = "";
                switch (processType)
                {
                    case "MB":
                        currentDB = _Schema.SqlHelper.ConnectionString_PCA;
                        currentSPName = "IMES_SFC_MB";
                        break;
                    case "Product":
                        currentDB = _Schema.SqlHelper.ConnectionString_FA;
                        currentSPName = "IMES_SFC_Product";
                        break;
                    case "Pallet":
                        currentDB = _Schema.SqlHelper.ConnectionString_PAK;
                        currentSPName = "IMES_SFC_Pallet";
                        break;
                    default:
                        break;
                }

                SqlParameter[] paramsArray = new SqlParameter[4];

                paramsArray[0] = new SqlParameter("@Key", SqlDbType.VarChar);
                paramsArray[0].Value = key;
                paramsArray[1] = new SqlParameter("@Customer", SqlDbType.VarChar);
                paramsArray[1].Value = customer;
                paramsArray[2] = new SqlParameter("@Line", SqlDbType.VarChar);
                paramsArray[2].Value = line;
                paramsArray[3] = new SqlParameter("@CurrentStation", SqlDbType.VarChar);
                paramsArray[3].Value = currentStation;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(currentDB, CommandType.StoredProcedure, currentSPName, paramsArray))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        string result = GetValue_Str(sqlR, 0);
                        if (result != "SUCCESS")
                        {
                            List<string> errpara = new List<string>();
                            for (int i = 1; i < sqlR.FieldCount; i++)
                            {
                                errpara.Add(GetValue_Str(sqlR, i));
                            }
                            throw new FisException(result, errpara);

                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreateModelProcess(string model, string editor,string line)
        {
            try
            {
                string currentDB = _Schema.SqlHelper.ConnectionString_GetData;
                string currentSPName = "IMES_Create_ModelProcess";

                SqlParameter[] paramsArray = new SqlParameter[3];

                paramsArray[0] = new SqlParameter("@Model", SqlDbType.VarChar);
                paramsArray[0].Value = model;
                paramsArray[1] = new SqlParameter("@Editor", SqlDbType.VarChar);
                paramsArray[1].Value = editor;
                paramsArray[2] = new SqlParameter("@Line", SqlDbType.VarChar);
                paramsArray[2].Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(currentDB, CommandType.StoredProcedure, currentSPName, paramsArray))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        string result = GetValue_Str(sqlR, 0);
                        if (result != "SUCCESS")
                        {
                            List<string> errpara = new List<string>();
                            for (int i = 1; i < sqlR.FieldCount; i++)
                            {
                                errpara.Add(GetValue_Str(sqlR, i));
                            }
                            throw new FisException(result, errpara);

                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetReleaseType(string reworkCode)
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

                        sqlCtx.Sentence = "SELECT DISTINCT {2} FROM {0} WHERE {3}=(SELECT TOP 1 {4} FROM {1} WHERE {5}=@{5})";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Rework_ReleaseType).Name,
                                                                        typeof(_Schema.Rework_Process).Name,
                                                                        _Schema.Rework_ReleaseType.fn_ReleaseType,
                                                                        _Schema.Rework_ReleaseType.fn_Process,
                                                                        _Schema.Rework_Process.fn_Process,
                                                                        _Schema.Rework_Process.fn_ReworkCode);

                        sqlCtx.Params.Add(_Schema.Rework_Process.fn_ReworkCode, new SqlParameter("@" + _Schema.Rework_Process.fn_ReworkCode, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx); 
                    }
                }
                sqlCtx.Params[_Schema.Rework_Process.fn_ReworkCode].Value = reworkCode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, 0);
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

        public IList<ModelProcess> GetModelProcessByModelLine(string model,string line)
        {
            try
            {
                IList<ModelProcess> ret = new List<ModelProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model_Process cond = new _Schema.Model_Process();
                        cond.Model = model;
                        cond.Line = line;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model_Process), null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model_Process.fn_Model].Value = model;
                sqlCtx.Params[_Schema.Model_Process.fn_Line].Value = line;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ModelProcess item = new ModelProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_ID]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Model]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Udt]);
                        item.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Line]);
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

        public DataTable GetExistPartProcess(string mbFamily)
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
                        _Metas.PartProcess cond = new _Metas.PartProcess();
                        cond.mbfamily = mbFamily;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.PartProcess>(tk, "DISTINCT", new string[] { mtns.PartProcess.fn_mbfamily, mtns.PartProcess.fn_process }, new ConditionCollection<_Metas.PartProcess>(new EqualCondition<_Metas.PartProcess>(cond)), mtns.PartProcess.fn_mbfamily, mtns.PartProcess.fn_process);
                    }
                }
                sqlCtx.Param(_Metas.PartProcess.fn_mbfamily).Value = mbFamily;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Inners .

        private fons.Process Find_DB(object key)
        {
            try
            {
                fons.Process ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process cond = new _Schema.Process();
                        cond.process = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Process.fn_process].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.Process();
                        ret.process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_process]);
                        //ret.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Name]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Editor]);
                        //ret.LastStationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_LastStation]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Udt]);
                        ret.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Descr]);
                        ret.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_type]);
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

        private fons.Process Find_Cache(object key)
        {
            try
            {
                fons.Process ret = null;
                lock (_syncObj_cache)
                {
                    if (_cache.ContainsKey((string)key))
                        ret = _cache[(string)key];
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertProcess(fons.Process item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Process.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Process.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.Process.fn_Name].Value = item.Name;
                sqlCtx.Params[_Schema.Process.fn_process].Value = item.process;
                sqlCtx.Params[_Schema.Process.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Process.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Process.fn_type].Value = item.Type;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistModifyProcess(fons.Process item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Process.fn_Editor].Value = item.Editor;
                //sqlCtx.Params[_Schema.Process.fn_Name].Value = item.Name;
                sqlCtx.Params[_Schema.Process.fn_process].Value = item.process;
                sqlCtx.Params[_Schema.Process.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Process.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.Process.fn_type].Value = item.Type;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteProcess(fons.Process item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process));
                    }
                }
                sqlCtx.Params[_Schema.Process.fn_process].Value = item.process;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons.Process> FindAll_DB()
        {
            try
            {
                IList<fons.Process> ret = new List<fons.Process>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons.Process item = new fons.Process();
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Cdt]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Editor]);
                            //item.LastStationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_LastStation]);
                            //item.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Name]);
                            item.process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_process]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Udt]);
                            item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Descr]);
                            item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_type]);
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

        private IList<fons.Process> FindAll_Cache()
        {
            try
            {
                lock (_syncObj_cache)
                {
                    return _cache.Values.ToList<fons.Process>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<_Schema.Model_Process> FindAllProcessModelRelations_DB(string process)
        {
            try
            {
                IList<_Schema.Model_Process> ret = new List<_Schema.Model_Process>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model_Process cond = new _Schema.Model_Process();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model_Process), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model_Process.fn_Process].Value = process;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        _Schema.Model_Process item = new _Schema.Model_Process();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Cdt]);
                        //item.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Customer]);
                        //item.RuleType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_RuleType]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_ID]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Model]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Udt]);
                        item.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Line]);
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

        private fons.Process FillProcessStations_DB(fons.Process proc)
        {
            try
            {
                IList<ProcessStation> newFieldVal = new List<ProcessStation>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process_Station cond = new _Schema.Process_Station();
                        cond.Process = proc.process;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process_Station), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Process_Station.fn_Process].Value = proc.process;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProcessStation prs = new ProcessStation();
                        prs.PreStation = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_PreStation]);
                        prs.ProcessID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Process]);
                        prs.StationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Station]);
                        prs.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Status]);
                        prs.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_ID]);
                        prs.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Cdt]);
                        prs.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Udt]);
                        prs.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Editor]);

                        prs.Tracker.Clear();
                        prs.Tracker = proc.Tracker;
                        newFieldVal.Add(prs);
                    }
                }
                proc.GetType().GetField("_processStations", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(proc, newFieldVal);

                return proc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<fons.Process> GetProcessByModel_DB(string model)
        {
            try
            {
                IList<fons.Process> ret = new List<fons.Process>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Process);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Model_Process);
                        _Schema.Model_Process equalCond = new _Schema.Model_Process();
                        equalCond.Model = model;
                        tf2.equalcond = equalCond;
                        tf2.ToGetFieldNames = null;

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(new _Schema.TableConnectionItem[] { 
                                                                            new _Schema.TableConnectionItem(tf1, _Schema.Process.fn_process, tf2, _Schema.Model_Process.fn_Process) });
                        
                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Process.fn_process));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Model_Process.fn_Model)].Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Process item = new fons.Process();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_Cdt)]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.Process.fn_Editor)]);
                        //item.LastStationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.Process.fn_LastStation)]);
                        //item.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.Process.fn_Name)]);
                        item.process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.Process.fn_process)]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.Process.fn_Udt)]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_Descr)]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_type)]);
                        item.Tracker.Clear();
                        ret.Add(item);
                    }
                }
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        private IList<fons.Process> GetProcessByModel_Cache(string model)
        {
            try
            {
                IList<fons.Process> ret = new List<fons.Process>();
                string key = _Schema.Func.MakeKeyForIdx(preStr1, model);
                lock (_syncObj_cache)
                {
                    if (_byWhateverIndex.ContainsKey(key))
                    {
                        foreach (string pk in _byWhateverIndex[key])
                        {
                            if (_cache.ContainsKey(pk))
                                ret.Add(_cache[pk]);
                        }
                    }
                }
                if (ret != null)
                {
                    ret = (from item in ret orderby item.process select item).ToList();
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

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.Process);
        }

        public void ProcessItem(CacheUpdateInfo item)
        {
            LoadAllCache();
        }

        private void LoadAllCache()
        {
            IList<fons.Process> prcs = this.FindAll_DB();
            if (prcs != null && prcs.Count > 0)
            {
                lock (_syncObj_cache)
                {
                    _cache.Clear();
                    _byWhateverIndex.Clear();

                    foreach (fons.Process prc in prcs)
                    {
                        _cache.Add(prc.process, prc);

                        //Regist index with model
                        IList<_Schema.Model_Process> mps = FindAllProcessModelRelations_DB(prc.process);
                        if (mps != null && mps.Count > 0)
                        {
                            foreach (_Schema.Model_Process mp in mps)
                            {
                                Regist(prc.process, _Schema.Func.MakeKeyForIdx(preStr1, mp.Model));
                            }
                        }
                    }
                }
            }
        }

        private void Regist(string pk, string key)
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

        private CacheUpdateInfo GetACacheSignal()
        {
            CacheUpdateInfo ret = new CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = CacheType.Process;
            return ret;
        }

        #endregion

        #region For Maintain

        public IList<fons.Process> getProcessList(string process_like)
        {
            try
            {
                IList<fons.Process> ret = new List<fons.Process>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process cond = new _Schema.Process();
                        cond.process = "%" + process_like + "%";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process), null, null, null, cond, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Process.fn_process);
                    }
                }
                sqlCtx.Params[_Schema.Model_Process.fn_Process].Value = "%" + process_like + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Process item = new fons.Process();
                        item.process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_process]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Editor]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Udt]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_Descr]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process.fn_type]);
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

        public IList<ModelProcess> GetModelProcessListByModel(string model_like)
        {
            try
            {
                IList<ModelProcess> ret = new List<ModelProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model_Process cond = new _Schema.Model_Process();
                        cond.Model = "%" + model_like + "%";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model_Process), null, null, null, cond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model_Process.fn_Model].Value = "%" + model_like + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ModelProcess item = new ModelProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_ID]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Model]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Process]);
                        //item.RuleType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_RuleType]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Udt]);
                        item.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Model_Process.fn_Line]);
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

        public IList<PalletProcess> GetPalletProcessListByCustomer(string customer_like)
        {
            try
            {
                IList<PalletProcess> ret = new List<PalletProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PalletProcess cond = new _Schema.PalletProcess();
                        cond.Customer = "%" + customer_like + "%";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletProcess), null, null, null, cond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PalletProcess.fn_Customer].Value = "%" + customer_like + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletProcess item = new PalletProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Cdt]);
                        item.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Customer]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Editor]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Udt]);
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

        public IList<fons.PartProcess> GetPartProcessListByMBFamily(string mbFamily_like)
        {
            try
            {
                IList<fons.PartProcess> ret = new List<fons.PartProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartProcess cond = new _Schema.PartProcess();
                        cond.MBFamily = "%" + mbFamily_like + "%";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartProcess), null, null, null, cond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartProcess.fn_MBFamily].Value = "%" + mbFamily_like + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PartProcess item = new fons.PartProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Editor]);
                        item.MBFamily = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_MBFamily]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Udt]);
                        item.PilotRun = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_PilotRun]);
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

        public IList<ReworkProcess> GetReworkProcessListByReworkCode(string reworkCode_like)
        {
            try
            {
                IList<ReworkProcess> ret = new List<ReworkProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Rework_Process cond = new _Schema.Rework_Process();
                        cond.ReworkCode = "%" + reworkCode_like + "%";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_Process), null, null, null, cond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Rework_Process.fn_ReworkCode].Value = "%" + reworkCode_like + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ReworkProcess item = new ReworkProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Editor]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Process]);
                        item.ReworkCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_ReworkCode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Udt]);
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

        public IList<PalletProcess> GetPalletProcessListByProcess(string process)
        {
            try
            {
                IList<PalletProcess> ret = new List<PalletProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PalletProcess cond = new _Schema.PalletProcess();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletProcess), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PalletProcess.fn_Process].Value = process;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletProcess item = new PalletProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Cdt]);
                        item.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Customer]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Editor]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Udt]);
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

        public IList<fons.PartProcess> GetPartProcessListByProcess(string process)
        {
            try
            {
                IList<fons.PartProcess> ret = new List<fons.PartProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartProcess cond = new _Schema.PartProcess();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartProcess), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartProcess.fn_Process].Value = process;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PartProcess item = new fons.PartProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Editor]);
                        item.MBFamily = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_MBFamily]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Udt]);
                        item.PilotRun = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_PilotRun]);
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

        public IList<ReworkProcess> GetReworkProcessListByProcess(string process)
        {
            try
            {
                IList<ReworkProcess> ret = new List<ReworkProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Rework_Process cond = new _Schema.Rework_Process();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_Process), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Rework_Process.fn_Process].Value = process;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ReworkProcess item = new ReworkProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Editor]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Process]);
                        item.ReworkCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_ReworkCode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Udt]);
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

        public IList<PalletProcess> GetPalletProcessList()
        {
            try
            {
                IList<PalletProcess> ret = new List<PalletProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletProcess));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletProcess item = new PalletProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Cdt]);
                        item.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Customer]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Editor]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletProcess.fn_Udt]);
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

        public IList<fons.PartProcess> GetPartProcessList()
        {
            try
            {
                IList<fons.PartProcess> ret = new List<fons.PartProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartProcess));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PartProcess item = new fons.PartProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Editor]);
                        item.MBFamily = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_MBFamily]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Process]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_Udt]);
                        item.PilotRun = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartProcess.fn_PilotRun]);
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

        public IList<ReworkProcess> GetReworkProcessList()
        {
            try
            {
                IList<ReworkProcess> ret = new List<ReworkProcess>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_Process));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ReworkProcess item = new ReworkProcess();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Editor]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Process]);
                        item.ReworkCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_ReworkCode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Rework_Process.fn_Udt]);
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

        public void DeletePalletProcessByProcess(string process)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PalletProcess cond = new _Schema.PalletProcess();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletProcess), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PalletProcess.fn_Process].Value = process;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartProcessByProcess(string process)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartProcess cond = new _Schema.PartProcess();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartProcess), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PartProcess.fn_Process].Value = process;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteReworkProcessByProcess(string process)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Rework_Process cond = new _Schema.Rework_Process();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_Process), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Rework_Process.fn_Process].Value = process;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ProcessStation> GetProcessStationList(string process)
        {
            try
            {
                IList<ProcessStation> ret = new List<ProcessStation>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process_Station cond = new _Schema.Process_Station();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process_Station), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[]{_Schema.Process_Station.fn_PreStation, _Schema.Process_Station.fn_Status, _Schema.Process_Station.fn_Station}));
                    }
                }
                sqlCtx.Params[_Schema.Process_Station.fn_Process].Value = process;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProcessStation prs = new ProcessStation();
                        prs.PreStation = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_PreStation]);
                        prs.ProcessID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Process]);
                        prs.StationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Station]);
                        prs.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Status]);
                        prs.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_ID]);
                        prs.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Cdt]);
                        prs.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Udt]);
                        prs.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Editor]);

                        prs.Tracker.Clear();
                        ret.Add(prs);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProcessStation GetProcessStation(int id)
        {
            try
            {
                ProcessStation ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process_Station cond = new _Schema.Process_Station();
                        cond.ID = id;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process_Station), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Process_Station.fn_ID].Value = id;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new ProcessStation();
                        ret.PreStation = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_PreStation]);
                        ret.ProcessID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Process]);
                        ret.StationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Station]);
                        ret.Status = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Status]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_ID]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Udt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Process_Station.fn_Editor]);

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

        public void DeleteProcessStation(int id)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process_Station));
                    }
                }
                sqlCtx.Params[_Schema.Process_Station.fn_ID].Value = id;
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

        public int AddProcessStation(ProcessStation obj)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process_Station));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Process_Station.fn_PreStation].Value = obj.PreStation;
                sqlCtx.Params[_Schema.Process_Station.fn_Process].Value = obj.ProcessID;
                sqlCtx.Params[_Schema.Process_Station.fn_Station].Value = obj.StationID;
                sqlCtx.Params[_Schema.Process_Station.fn_Status].Value = obj.Status;
                sqlCtx.Params[_Schema.Process_Station.fn_Editor].Value = obj.Editor;
                sqlCtx.Params[_Schema.Process_Station.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Process_Station.fn_Udt].Value = cmDt;
                obj.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                obj.Tracker.Clear();

                SqlTransactionManager.Commit();

                return obj.ID;
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

        public void SaveProcessStation(ProcessStation obj)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process_Station));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Process_Station.fn_ID].Value = obj.ID;
                sqlCtx.Params[_Schema.Process_Station.fn_PreStation].Value = obj.PreStation;
                sqlCtx.Params[_Schema.Process_Station.fn_Process].Value = obj.ProcessID;
                sqlCtx.Params[_Schema.Process_Station.fn_Station].Value = obj.StationID;
                sqlCtx.Params[_Schema.Process_Station.fn_Status].Value = obj.Status;
                sqlCtx.Params[_Schema.Process_Station.fn_Editor].Value = obj.Editor;
                sqlCtx.Params[_Schema.Process_Station.fn_Udt].Value = cmDt;
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

        public void AddPartProcess(fons.PartProcess obj)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartProcess));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PartProcess.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PartProcess.fn_Editor].Value = obj.Editor;
                sqlCtx.Params[_Schema.PartProcess.fn_MBFamily].Value = obj.MBFamily;
                sqlCtx.Params[_Schema.PartProcess.fn_Process].Value = obj.Process;
                sqlCtx.Params[_Schema.PartProcess.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PartProcess.fn_PilotRun].Value = obj.PilotRun;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddReworkProcess(ReworkProcess obj)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_Process));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Rework_Process.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Rework_Process.fn_Editor].Value = obj.Editor;
                sqlCtx.Params[_Schema.Rework_Process.fn_Process].Value = obj.Process;
                sqlCtx.Params[_Schema.Rework_Process.fn_ReworkCode].Value = obj.ReworkCode;
                sqlCtx.Params[_Schema.Rework_Process.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPalletProcess(PalletProcess obj)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletProcess));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PalletProcess.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletProcess.fn_Customer].Value = obj.Customer;
                sqlCtx.Params[_Schema.PalletProcess.fn_Editor].Value = obj.Editor;
                sqlCtx.Params[_Schema.PalletProcess.fn_Process].Value = obj.Process;
                sqlCtx.Params[_Schema.PalletProcess.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ProcessRuleSet> GetAllProcessRuleset()
        {
            //select * from ProcessRuleset order by Priority
            try
            {
                IList<ProcessRuleSet> ret = new List<ProcessRuleSet>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderByDesc, _Schema.ProcessRuleset.fn_Priority);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ProcessRuleSet item = new ProcessRuleSet();
                            item.Condition1 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition1]);
                            item.Condition2 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition2]);
                            item.Condition3 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition3]);
                            item.Condition4 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition4]);
                            item.Condition5 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition5]);
                            item.Condition6 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition6]);
                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_ID]);
                            item.Priority = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Priority]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Editor]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Cdt]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Udt]);
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

        public void UpdateRuleSetPriority(ProcessRuleSet ruleSet)
        {
            //update ProcessRuleset 
            //  set Priority=ruleSet.Priority 
            //where ID=ruleSet.ID
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRuleset cond = new _Schema.ProcessRuleset();
                        cond.ID = ruleSet.ID;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset), new List<string>() { _Schema.ProcessRuleset.fn_Priority, _Schema.ProcessRuleset.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ProcessRuleset.fn_ID].Value = ruleSet.ID;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Priority)].Value = ruleSet.Priority;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Editor)].Value = ruleSet.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void DeleteProcessRulesetByID(int rule_set_id)
        {
            try
            {
                SqlTransactionManager.Begin();

                RemoveProcessRule(rule_set_id);
                RemoveProcessRuleSet(rule_set_id);

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

        private void RemoveProcessRuleSet(int rule_set_id)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset));
                    }
                }
                sqlCtx.Params[_Schema.ProcessRuleset.fn_ID].Value = rule_set_id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RemoveProcessRule(int rule_set_id)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRule cond = new _Schema.ProcessRule();
                        cond.RuleSetID = rule_set_id;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ProcessRule.fn_RuleSetID].Value = rule_set_id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddProcessRuleSet(ProcessRuleSet ruleset)
        {
            //insert ProcessRuleset(Priority,Condition1, Condition2, Condition3, Condition4, Condition5) 
            //select ISNULL(max(Priority),'0')+1,' ruleset.Condition1','ruleset.Condition2','ruleset.Condition3','ruleset.Condition4','ruleset.Condition5'
            //from ProcessRuleset
            try
            {
                SqlTransactionManager.Begin();

                ruleset.Priority = GetMaxPriority() + 1;
                InsertProcessRuleSet(ruleset);

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

        private void InsertProcessRuleSet(ProcessRuleSet ruleset)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition1].Value = ruleset.Condition1;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition2].Value = ruleset.Condition2;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition3].Value = ruleset.Condition3;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition4].Value = ruleset.Condition4;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition5].Value = ruleset.Condition5;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition6].Value = ruleset.Condition6;
                //sqlCtx.Params[_Schema.ProcessRuleset.fn_ID].Value = ruleset.Id;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Priority].Value = ruleset.Priority;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Editor].Value = ruleset.Editor;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Udt].Value = cmDt;
                ruleset.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int GetMaxPriority()
        {
            SqlDataReader sqlR = null;
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset), "MAX", new List<string>() { _Schema.ProcessRuleset.fn_Priority }, null, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = GetValue_Int32(sqlR, sqlCtx.Indexes["MAX"]);
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

        public void UpdateProcessRuleSet(ProcessRuleSet singleRuleSet)
        {
            //update ProcessRuleset 
            //set Condition1='singleRuleSet.Condition1' ,
            //Condition2='singleRuleSet.Condition2' ,
            //Condition3='singleRuleSet.Condition3' ,
            //Condition4='singleRuleSet.Condition4' ,
            //Condition5='singleRuleSet.Condition5' ,
            //Condition6='singleRuleSet.Condition6' ,
            //where ID='singleRuleSet.ID'
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRuleset cond = new _Schema.ProcessRuleset();
                        cond.ID = singleRuleSet.ID;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset), null, new List<string>() { _Schema.ProcessRuleset.fn_Priority, _Schema.ProcessRuleset.fn_ID }, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ProcessRuleset.fn_ID].Value = singleRuleSet.ID;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Condition1)].Value = singleRuleSet.Condition1;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Condition2)].Value = singleRuleSet.Condition2;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Condition3)].Value = singleRuleSet.Condition3;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Condition4)].Value = singleRuleSet.Condition4;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Condition5)].Value = singleRuleSet.Condition5;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Condition6)].Value = singleRuleSet.Condition6;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Editor)].Value = singleRuleSet.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ProcessRuleset.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<dmns.ProcessRule> GetAllRuleByRuleSetID(int rule_set_id)
        {
            //select * from ProcessRule where RuleSetID='rule_set_id' order by Process 
            try
            {
                IList<dmns.ProcessRule> ret = new List<dmns.ProcessRule>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRule cond = new _Schema.ProcessRule();
                        cond.RuleSetID = rule_set_id;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.ProcessRule.fn_Process);
                    }
                }
                sqlCtx.Params[_Schema.ProcessRule.fn_RuleSetID].Value = rule_set_id;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        dmns.ProcessRule item = new dmns.ProcessRule();
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_ID]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Process]);
                        item.Rule_set_id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_RuleSetID]);
                        item.Value1 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value1]);
                        item.Value2 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value2]);
                        item.Value3 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value3]);
                        item.Value4 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value4]);
                        item.Value5 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value5]);
                        item.Value6 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value6]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Editor]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Udt]);
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

        public void AddProcessRule(dmns.ProcessRule processrule)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                //sqlCtx.Params[_Schema.ProcessRule.fn_ID].Value = processrule.Id;
                sqlCtx.Params[_Schema.ProcessRule.fn_Process].Value = processrule.Process;
                sqlCtx.Params[_Schema.ProcessRule.fn_RuleSetID].Value = processrule.Rule_set_id;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value1].Value = processrule.Value1;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value2].Value = processrule.Value2;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value3].Value = processrule.Value3;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value4].Value = processrule.Value4;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value5].Value = processrule.Value5;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value6].Value = processrule.Value6;
                sqlCtx.Params[_Schema.ProcessRule.fn_Editor].Value = processrule.Editor;
                sqlCtx.Params[_Schema.ProcessRule.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.ProcessRule.fn_Udt].Value = cmDt;
                processrule.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProcessRule(dmns.ProcessRule processrule)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.ProcessRule.fn_ID].Value = processrule.Id;
                sqlCtx.Params[_Schema.ProcessRule.fn_Process].Value = processrule.Process;
                sqlCtx.Params[_Schema.ProcessRule.fn_RuleSetID].Value = processrule.Rule_set_id;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value1].Value = processrule.Value1;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value2].Value = processrule.Value2;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value3].Value = processrule.Value3;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value4].Value = processrule.Value4;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value5].Value = processrule.Value5;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value6].Value = processrule.Value6;
                sqlCtx.Params[_Schema.ProcessRule.fn_Editor].Value = processrule.Editor;
                sqlCtx.Params[_Schema.ProcessRule.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteProcessRuleByID(int id)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule));
                    }
                }
                sqlCtx.Params[_Schema.ProcessRule.fn_ID].Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistedProcess(string processName)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process cond = new _Schema.Process();
                        cond.process = processName;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process), "COUNT", new List<string>() { _Schema.Process.fn_process }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Process.fn_process].Value = processName;
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

        public void DeleteModelProcessByModel(string modelName)
        {
            //删除一条Model_Process表的记录,where Model=?
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model_Process cond = new _Schema.Model_Process();
                        cond.Model = modelName;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model_Process), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model_Process.fn_Model].Value = modelName;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void SaveProcess(string strOldProcessName, fons.Process obj)
        {
            try
            {
                SqlTransactionManager.Begin();

                DataChangeMediator.AddChangeDemand(this.GetACacheSignal());

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process cond = new _Schema.Process();
                        cond.process = strOldProcessName;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process), null, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Process.fn_process].Value = strOldProcessName;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Process.fn_Editor)].Value = obj.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Process.fn_process)].Value = obj.process;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Process.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Process.fn_Descr)].Value = obj.Descr;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Process.fn_type)].Value = obj.Type;
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

        public IList<int> GetProcessRuleIDByModel(string modelName)
        {
            //select b.ID from 
            //ProcessRuleSet a inner join ProcessRule b 
            //on a.ID = b.RuleSetID 
            //where a.Condition1='Model' 
            //and ISNULL(a.Condition2, '')='' 
            //and b.Value1 = 'Model' 
            //and ISNULL(b.Value2, '')=''
            try
            {
                IList<int> ret = new List<int>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.ProcessRuleset);
                        _Schema.ProcessRuleset cond = new _Schema.ProcessRuleset();
                        cond.Condition1 = modelName;
                        tf1.equalcond = cond;
                        _Schema.ProcessRuleset noecond = new _Schema.ProcessRuleset();
                        noecond.Condition2 = string.Empty;
                        tf1.nullOrEqual = noecond;
                        tf1.ToGetFieldNames = null;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.ProcessRule);
                        _Schema.ProcessRule cond2 = new _Schema.ProcessRule();
                        cond2.Value1 = modelName;
                        tf2.equalcond = cond2;
                        _Schema.ProcessRule noecond2 = new _Schema.ProcessRule();
                        noecond2.Value2 = string.Empty;
                        tf2.nullOrEqual = noecond2;
                        tf2.ToGetFieldNames.Add(_Schema.ProcessRule.fn_ID);

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.ProcessRuleset.fn_ID, tf2, _Schema.ProcessRule.fn_RuleSetID);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ProcessRuleset.fn_Condition2)].Value = noecond.Condition2;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.ProcessRule.fn_Value2)].Value = noecond2.Value2;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ProcessRuleset.fn_Condition1)].Value = modelName;
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.ProcessRule.fn_Value1)].Value = modelName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        int item = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.ProcessRule.fn_ID)]);
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

        public void DeleteRuleByRuleSetID(int ruleSetId)
        {
            try
            {
                RemoveProcessRule(ruleSetId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public dmns.ProcessRule GetProcessRuleById(int id)
        {
            try
            {
                dmns.ProcessRule ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRule cond = new _Schema.ProcessRule();
                        cond.ID = id;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ProcessRule.fn_ID].Value = id;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new dmns.ProcessRule();
                        ret.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_ID]);
                        ret.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Process]);
                        ret.Rule_set_id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_RuleSetID]);
                        ret.Value1 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value1]);
                        ret.Value2 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value2]);
                        ret.Value3 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value3]);
                        ret.Value4 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value4]);
                        ret.Value5 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value5]);
                        ret.Value6 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value6]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Editor]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Udt]);

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProcessRuleSet GetRuleSetById(int id)
        {
            try
            {
                ProcessRuleSet ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRuleset cond = new _Schema.ProcessRuleset();
                        cond.ID = id;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ProcessRuleset.fn_ID].Value = id;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new ProcessRuleSet();
                        ret.Condition1 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition1]);
                        ret.Condition2 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition2]);
                        ret.Condition3 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition3]);
                        ret.Condition4 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition4]);
                        ret.Condition5 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition5]);
                        ret.Condition6 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Condition6]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_ID]);
                        ret.Priority = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Priority]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Editor]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRuleset.fn_Udt]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void TruncateModelProcess()
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonTruncate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model_Process));
                    }
                }
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons.Process> GetAllProcessForRework()
        {
            try
            {
                IList<fons.Process> ret = new List<fons.Process>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Process);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Rework_ReleaseType);
                        tf2.ToGetFieldNames = null;

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(new _Schema.TableConnectionItem[] { 
                                                                            new _Schema.TableConnectionItem(tf1, _Schema.Process.fn_process, tf2, _Schema.Rework_ReleaseType.fn_Process) });

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Process.fn_process));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.Process item = new fons.Process();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_Cdt)]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_Editor)]);
                        item.process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_process)]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_Udt)]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_Descr)]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Process.fn_type)]);
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

        public void DeleteAllModelProcess()
        {
            //delete from Model_Process
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model_Process));
                        sqlCtx.Sentence = sqlCtx.Sentence.Remove(sqlCtx.Sentence.IndexOf("WHERE"));
                    }
                }
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IFRuleSetIsExists(ProcessRuleSet ruleset)
        {
            //Select * from ProcessRuleset where Condition1=? and Condition2=? and Condition3=? and Condition4=? and Condition5=? And ID <> ?
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRuleset cond = new _Schema.ProcessRuleset();
                        cond.Condition1 = ruleset.Condition1;
                        cond.Condition2 = ruleset.Condition2;
                        cond.Condition3 = ruleset.Condition3;
                        cond.Condition4 = ruleset.Condition4;
                        cond.Condition5 = ruleset.Condition5;
                        cond.Condition6 = ruleset.Condition6;
                        _Schema.ProcessRuleset neqCond = new _Schema.ProcessRuleset();
                        neqCond.ID = ruleset.ID;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset), "COUNT", new List<string>() { _Schema.ProcessRuleset.fn_ID }, cond, null, null, null, null, null, null, null, neqCond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition1].Value = ruleset.Condition1;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition2].Value = ruleset.Condition2;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition3].Value = ruleset.Condition3;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition4].Value = ruleset.Condition4;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition5].Value = ruleset.Condition5;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition6].Value = ruleset.Condition6;
                sqlCtx.Params[_Schema.ProcessRuleset.fn_ID].Value = ruleset.ID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public bool IFRuleIsExists(dmns.ProcessRule rule)
        {
            //Select * from ProcessRule where RuleSetId=? And Process=? And Value1=? And Value2=? And Value3=? And Value4=? And Value5=?  And ID <> ?
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRule cond = new _Schema.ProcessRule();
                        cond.RuleSetID = rule.Rule_set_id;
                        cond.Process = rule.Process;
                        cond.Value1 = rule.Value1;
                        cond.Value2 = rule.Value2;
                        cond.Value3 = rule.Value3;
                        cond.Value4 = rule.Value4;
                        cond.Value5 = rule.Value5;
                        cond.Value6 = rule.Value6;
                        _Schema.ProcessRule neqCond = new _Schema.ProcessRule();
                        neqCond.ID = rule.Id;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule), "COUNT", new List<string>() { _Schema.ProcessRule.fn_ID }, cond, null, null, null, null, null, null, null, neqCond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.ProcessRule.fn_RuleSetID].Value = rule.Rule_set_id;
                sqlCtx.Params[_Schema.ProcessRule.fn_Process].Value = rule.Process;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value1].Value = rule.Value1;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value2].Value = rule.Value2;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value3].Value = rule.Value3;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value4].Value = rule.Value4;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value5].Value = rule.Value5;
                sqlCtx.Params[_Schema.ProcessRule.fn_Value6].Value = rule.Value6;
                sqlCtx.Params[_Schema.ProcessRule.fn_ID].Value = rule.Id;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public bool IFConditionIsModel(int rulesetID)
        {
            //Select * from ProcessRuleset where ID=? And Condition1=’Model’ and isnull(Condition2,’’)==’’ and isnull(Condition3,’’)==’’ and isnull(Condition4,’’)==’’ and isnull(Condition5,’’)==’’
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRuleset cond = new _Schema.ProcessRuleset();
                        cond.ID = rulesetID;
                        cond.Condition1 = "Model";
                        _Schema.ProcessRuleset noeCond = new _Schema.ProcessRuleset();
                        noeCond.Condition2 = string.Empty;
                        noeCond.Condition3 = string.Empty;
                        noeCond.Condition4 = string.Empty;
                        noeCond.Condition5 = string.Empty;
                        noeCond.Condition6 = string.Empty;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRuleset), "COUNT", new List<string>() { _Schema.ProcessRuleset.fn_ID }, cond, null, null, null, null, null, null, null, noeCond);
                        sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition1].Value = cond.Condition1;
                        sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition2].Value = noeCond.Condition2;
                        sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition3].Value = noeCond.Condition3;
                        sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition4].Value = noeCond.Condition4;
                        sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition5].Value = noeCond.Condition5;
                        sqlCtx.Params[_Schema.ProcessRuleset.fn_Condition6].Value = noeCond.Condition6;
                    }
                }
                sqlCtx.Params[_Schema.ProcessRuleset.fn_ID].Value = rulesetID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public IList<ReworkReleaseType> GetReworkReleaseTypeListByProcess(string process)
        {
            try
            {
                IList<ReworkReleaseType> ret = new List<ReworkReleaseType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Rework_ReleaseType cond = new _Schema.Rework_ReleaseType();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_ReleaseType), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Rework_ReleaseType.fn_Process].Value = process;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ReworkReleaseType item = new ReworkReleaseType();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Rework_ReleaseType.fn_Cdt]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Rework_ReleaseType.fn_ID]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_ReleaseType.fn_Process]);
                        item.ReleaseType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Rework_ReleaseType.fn_ReleaseType]);
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

        public IList<string> GetReworkReleaseTypeList()
        {
            //(select distinct PartType as ReleaseType from PartCheck)
            //union
            //(select distinct ItemName as ReleaseType from CheckItem)
            try
            {
                IList<string> ret = new List<string>() {    "BWEIGHT",
                                                            "CN",
                                                            "COA",
                                                            "CPQSNO",
                                                            "Delivery",
                                                            "KIT ID",
                                                            "MB",
                                                            "MMI",
                                                            "PCMAC",
                                                            "PLT",
                                                            "WEIGHT"};

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence =   "(SELECT DISTINCT {2} FROM {0}) " +
                                            "UNION " +
                                            "(SELECT DISTINCT {3} FROM {1}) ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.PartCheck).Name,
                                                                        typeof(_Schema.CheckItem).Name,
                                                                        _Schema.PartCheck.fn_PartType,
                                                                        _Schema.CheckItem.fn_ItemName);

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, 0);
                        ret.Add(item);
                    }
                }
                return (from entry in ret orderby entry select entry).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteReworkReleaseTypeByProcess(string process)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Rework_ReleaseType cond = new _Schema.Rework_ReleaseType();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_ReleaseType), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Rework_ReleaseType.fn_Process].Value = process;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddReworkReleaseType(ReworkReleaseType item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Rework_ReleaseType));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Rework_ReleaseType.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Rework_ReleaseType.fn_Process].Value = item.Process;
                sqlCtx.Params[_Schema.Rework_ReleaseType.fn_ReleaseType].Value = item.ReleaseType;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistedProcessStation(string process, string station, string preStation)
        {
            //select count(*) from Process_Station where Process=? and Station = ? and PreStation = ?
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Process_Station cond = new _Schema.Process_Station();
                        cond.Process = process;
                        cond.Station = station;
                        cond.PreStation = preStation;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Process_Station), "COUNT", new List<string>() { _Schema.Process_Station.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Process_Station.fn_Process].Value = process;
                sqlCtx.Params[_Schema.Process_Station.fn_Station].Value = station;
                sqlCtx.Params[_Schema.Process_Station.fn_PreStation].Value = preStation;
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

        public IList<string> GetPCBFamilyList()
        {
            #region . old .
            /*
            //select distinct Descr as PCBFamily from Part A inner join IMES_PCA..PCB B on A.PartNo = B.PCBModelID order by PCBFamily
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Part);
                        tf1.subDBCalalog = _Schema.SqlHelper.DB_BOM;
                        tf1.ToGetFieldNames.Add(_Schema.Part.fn_Descr);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.PCB);
                        tf2.ToGetFieldNames = null;

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(new _Schema.TableConnectionItem[] { 
                                                                            new _Schema.TableConnectionItem(tf1, _Schema.Part.fn_PartNo, tf2, _Schema.PCB.fn_PCBModelID) });

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Part.fn_Descr));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Part.fn_Descr)]);
                        ret.Add(item);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }*/
            #endregion

            //select distinct Descr as Family from Part where BomNodeType='MB'
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
                        mtns.Part_NEW cond = new mtns.Part_NEW();
                        cond.bomNodeType = "MB";
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Part_NEW>(tk, "DISTINCT", new string[] { mtns.Part_NEW.fn_descr }, new ConditionCollection<mtns.Part_NEW>(new EqualCondition<mtns.Part_NEW>(cond)), mtns.Part_NEW.fn_descr);
                        sqlCtx.Param(mtns.Part_NEW.fn_bomNodeType).Value = cond.bomNodeType;
                        sqlCtx.Param(mtns.Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Part_NEW.fn_descr));
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

        public IList<string> GetPCBModelList()
        {
            //select distinct PCBModelID as PCBModel from IMES_PCA..PCB order by PCBModel
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCB), "DISTINCT", new List<string>() { _Schema.PCB.fn_PCBModelID }, null, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PCB.fn_PCBModelID);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCB.fn_PCBModelID]);
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

        public IList<dmns.ProcessRule> GetRuleListByRuleSetIdAndProcess(int ruleSetId, string processName)
        {
            //Select * from ProcessRule where RuleSetID=? And Process=? 
            try
            {
                IList<dmns.ProcessRule> ret = new List<dmns.ProcessRule>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.ProcessRule cond = new _Schema.ProcessRule();
                        cond.RuleSetID = ruleSetId;
                        cond.Process = processName;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ProcessRule), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.ProcessRule.fn_Process);
                    }
                }
                sqlCtx.Params[_Schema.ProcessRule.fn_RuleSetID].Value = ruleSetId;
                sqlCtx.Params[_Schema.ProcessRule.fn_Process].Value = processName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        dmns.ProcessRule item = new dmns.ProcessRule();
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_ID]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Process]);
                        item.Rule_set_id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_RuleSetID]);
                        item.Value1 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value1]);
                        item.Value2 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value2]);
                        item.Value3 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value3]);
                        item.Value4 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value4]);
                        item.Value5 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value5]);
                        item.Value6 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value6]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Editor]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Udt]);
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

        public IList<ProcessMaintainInfo> GetProcessList()
        {
            try
            {
                IList<ProcessMaintainInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<mtns.Process>(tk, mtns.Process.fn_type, mtns.Process.fn_process);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.Process, ProcessMaintainInfo, ProcessMaintainInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ExistProcessRule(string process)
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
                        mtns.ProcessRule cond = new mtns.ProcessRule();
                        cond.process = process;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.ProcessRule>(tk, "COUNT", new string[] { mtns.ProcessRule.fn_id }, new ConditionCollection<mtns.ProcessRule>(new EqualCondition<mtns.ProcessRule>(cond)));
                    }
                }
                sqlCtx.Param(mtns.ProcessRule.fn_process).Value = process;

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

        public void AddProcessStations(List<ProcessStation> stationList)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (stationList != null)
                {
                    foreach (ProcessStation station in stationList)
                    {
                        AddProcessStation_Inner(station);
                    }
                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal());
                }

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

        private void AddProcessStation_Inner(ProcessStation item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Process_Station>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Process_Station, ProcessStation>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Process_Station.fn_cdt).Value = cmDt;
                sqlCtx.Param(Process_Station.fn_udt).Value = cmDt;
                item.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void DeletePalletProcessByProcessDefered(IUnitOfWork uow, string process)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), process);
        }

        public void DeletePartProcessByProcessDefered(IUnitOfWork uow, string process)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), process);
        }

        public void DeleteReworkProcessByProcessDefered(IUnitOfWork uow, string process)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), process);
        }

        public void DeleteProcessStationDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddProcessStationDefered(IUnitOfWork uow, ProcessStation obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void SaveProcessStationDefered(IUnitOfWork uow, ProcessStation obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void AddPartProcessDefered(IUnitOfWork uow, fons.PartProcess obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void AddReworkProcessDefered(IUnitOfWork uow, ReworkProcess obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void AddPalletProcessDefered(IUnitOfWork uow, PalletProcess obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void UpdateRuleSetPriorityDefered(IUnitOfWork uow, ProcessRuleSet ruleSet)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), ruleSet);
        }

        public void DeleteProcessRulesetByIDDefered(IUnitOfWork uow, int rule_set_id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), rule_set_id);
        }

        public void AddProcessRuleSetDefered(IUnitOfWork uow, string condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void UpdateProcessRuleSetDefered(IUnitOfWork uow, ProcessRuleSet singleRuleSet)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), singleRuleSet);
        }

        public void AddProcessRuleDefered(IUnitOfWork uow, dmns.ProcessRule processrule)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), processrule);
        }

        public void UpdateProcessRuleDefered(IUnitOfWork uow, dmns.ProcessRule processrule)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), processrule);
        }

        public void DeleteProcessRuleByIDDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void DeleteModelProcessByModelDefered(IUnitOfWork uow, string modelName)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), modelName);
        }

        public void SaveProcessDefered(IUnitOfWork uow, string strOldProcessName, fons.Process obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), strOldProcessName, obj);
        }

        public void DeleteRuleByRuleSetIDDefered(IUnitOfWork uow, int ruleSetId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), ruleSetId);
        }

        //public void TruncateModelProcessDefered(IUnitOfWork uow)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod().Name);
        //}

        public void DeleteAllModelProcessDefered(IUnitOfWork uow)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod());
        }

        public void DeleteReworkReleaseTypeByProcessDefered(IUnitOfWork uow, string process)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), process);
        }

        public void AddReworkReleaseTypeDefered(IUnitOfWork uow, ReworkReleaseType item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddProcessStationsDefered(IUnitOfWork uow, List<ProcessStation> stationList)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), stationList);
        }

        #endregion

        #endregion

        #region for material Process

        public void AddMaterialProcess(string materialType, string process, string editor)
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
                        sqlCtx.Sentence = @"MERGE INTO Material_Process as Target 
                                                              Using (select @MaterialType)
                                                                     as Source (MaterialType)
                                                              ON Target.MaterialType = Source.MaterialType 
                                                            WHEN NOT MATCHED THEN
                                                                insert (MaterialType, Process, Editor, Cdt, Udt)		 		
                                                                values( @MaterialType, @Process, @Editor, getdate(), getdate())
                                                            WHEN MATCHED THEN
                                                                 update set Process= @Process,
                                                                             Editor =@Editor,
                                                                             Udt=getdate();";
                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        sqlCtx.AddParam("Process", new SqlParameter("@Process", SqlDbType.VarChar));                      
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialType").Value = materialType;
                sqlCtx.Param("Process").Value = process;
                sqlCtx.Param("Editor").Value = editor;

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
        public void RemoveMaterialProcess(string process)
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
                        sqlCtx.Sentence = @"delete from Material_Process 
                                                            where Process=@Process ";
                      
                        sqlCtx.AddParam("Process", new SqlParameter("@Process", SqlDbType.VarChar));
                       

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
              
                sqlCtx.Param("Process").Value = process;
               
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
        public void RemoveMaterialProcessByType(string materialType)
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
                        sqlCtx.Sentence = @"delete from Material_Process 
                                                            where MaterialType=@MaterialType ";
                       
                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("MaterialType").Value = materialType;

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

       public  void AddMaterialProcessDefered(IUnitOfWork uow, string materialType, string process, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), materialType, process, editor);
        }
        public void RemoveMaterialProcessDefered(IUnitOfWork uow, string process)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), process);
        }
       public  void RemoveMaterialProcessByTypeDefered(IUnitOfWork uow, string materialType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), materialType);
        }

        public IList<MaterialProcess> GetMaterialProcessByProcess(string process)
        {
            try
            {
                IList<MaterialProcess> ret = new List<MaterialProcess>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, MaterialType, Process, Editor, Cdt, Udt
                                                                from Material_Process
                                                                where Process= @Process
                                                                order by MaterialType";
                        sqlCtx.AddParam("Process", new SqlParameter("@Process", SqlDbType.VarChar));
                       
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Process").Value = process;
             

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<MaterialProcess>(sqlR));

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MaterialProcess GetMaterialProcessByType(string materialType)
        {
            try
            {
                MaterialProcess ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, MaterialType, Process, Editor, Cdt, Udt
                                                                from Material_Process
                                                                where MaterialType= @MaterialType
                                                                order by MaterialType";
                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialType").Value = materialType;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret=IMES.Infrastructure.Repository._Schema.SQLData.ToObject<MaterialProcess>(sqlR);

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<ProcessStation> GetMaterialProcessNextStatus(string materialType, string curStatus)
        {
            try
            {
                IList<ProcessStation> ret = new List<ProcessStation>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.ID, a.Process as ProcessID, a.PreStation, a.Station as StationID, a.Status, 
                                                                       a.Editor, a.Cdt, a.Udt
                                                                from Process_Station a,
                                                                     Material_Process b
                                                                where a.Process = b.Process and
                                                                      b.MaterialType =@MaterialType and
                                                                      a.PreStation = @Station";
                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialType").Value = materialType;
                sqlCtx.Param("Station").Value = curStatus;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<ProcessStation>(sqlR));

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool CheckMaterialProcessStatus(string materialType, string curStatus, string nextStatus)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.ID
                                                                from Process_Station a,
                                                                         Material_Process b
                                                                where a.Process = b.Process and
                                                                      b.MaterialType =@MaterialType and
                                                                      a.PreStation = @PreStation and
                                                                      a.Station = @Station";
                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        sqlCtx.AddParam("PreStation", new SqlParameter("@PreStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialType").Value = materialType;
                sqlCtx.Param("PreStation").Value = curStatus;
                sqlCtx.Param("Station").Value = nextStatus;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
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

        #endregion


        #region for ModelProcess
        public void DeleteModelProcessByProcess(string process)
        {
            //删除一条Model_Process表的记录,where Process=?
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Model_Process cond = new _Schema.Model_Process();
                        cond.Process = process;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model_Process), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Model_Process.fn_Process].Value = process;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteModelProcessByProcessDefered(IUnitOfWork uow, string process)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), process);
        }

        #endregion

        #region ProcessRule
        public IList<dmns.ProcessRule> GetAllProcessRule()
        {
            //select * from ProcessRule 
            try
            {
                IList<dmns.ProcessRule> ret = new List<dmns.ProcessRule>();
                MethodBase mbase = MethodBase.GetCurrentMethod();
                int mtk = mbase.MetadataToken;
                _Schema.SQLContext sqlCtx = null;
                lock (mbase)
                {
                    if (!_Schema.Func.PeerTheSQL(mtk, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(mtk, typeof(_Schema.ProcessRule));
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                    CommandType.Text,
                                                                                                                    sqlCtx.Sentence,
                                                                                                                    sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        dmns.ProcessRule item = new dmns.ProcessRule();
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_ID]);
                        item.Process = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Process]);
                        item.Rule_set_id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_RuleSetID]);
                        item.Value1 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value1]);
                        item.Value2 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value2]);
                        item.Value3 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value3]);
                        item.Value4 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value4]);
                        item.Value5 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value5]);
                        item.Value6 = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Value6]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Editor]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ProcessRule.fn_Udt]);
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

        public IList<int> GetAllRuleSetIDInProcessRule()
        {
            try
            {
                IList<int> ret = new List<int>();

                _Schema.SQLContext sqlCtx = null;
                MethodBase mbase = MethodBase.GetCurrentMethod();
                int mtk = mbase.MetadataToken;
                lock (mbase)
                {
                    if (!_Schema.Func.PeerTheSQL(mtk, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(mtk, typeof(_Schema.ProcessRule), "DISTINCT", new List<String> { _Schema.ProcessRule.fn_RuleSetID },
                                                                                                     null, null, null, null, null, null, null, null);
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                    CommandType.Text,
                                                                                                                    sqlCtx.Sentence,
                                                                                                                    sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(GetValue_Int32(sqlR, 0));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddModelProcess(string modelName, string processName, string firstLine, string editor)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                MethodBase mbase = MethodBase.GetCurrentMethod();
                int mtk = mbase.MetadataToken;
                lock (mbase)
                {
                    if (!_Schema.Func.PeerTheSQL(mtk, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(mtk, typeof(_Schema.Model_Process));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx.Params[_Schema.Model_Process.fn_Model].Value = modelName;
                sqlCtx.Params[_Schema.Model_Process.fn_Process].Value = processName;
                sqlCtx.Params[_Schema.Model_Process.fn_Line].Value = firstLine;
                sqlCtx.Params[_Schema.Model_Process.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Model_Process.fn_Editor].Value = editor;
                sqlCtx.Params[_Schema.Model_Process.fn_Cdt].Value = cmDt;
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
        #endregion
    }
}
