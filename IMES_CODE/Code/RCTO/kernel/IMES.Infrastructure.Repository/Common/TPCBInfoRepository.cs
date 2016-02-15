using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.TPCB;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using IMES.DataModel;
using IMES.Infrastructure.Util;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: TPCBInfo相关
    /// </summary>
    public class TPCBInfoRepository : BaseRepository<TPCB_Info>, ITPCBInfoRepository
    {
        #region Overrides of BaseRepository<TPCB_Info>

        protected override void PersistNewItem(TPCB_Info item)
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

        protected override void PersistUpdatedItem(TPCB_Info item)
        {
            throw new NotImplementedException("Normal");
        }

        protected override void PersistDeletedItem(TPCB_Info item)
        {
            throw new NotImplementedException("Normal");
        }

        #endregion

        #region Implementation of IRepository<TPCB_Info>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override TPCB_Info Find(object key)
        {
            try
            {
                TPCB_Info ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new TPCB_Info();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        ret.Dcode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        ret.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        ret.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        ret.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        ret.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        ret.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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
        public override IList<TPCB_Info> FindAll()
        {
            try
            {
                IList<TPCB_Info> ret = new List<TPCB_Info>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCB_Info item = new TPCB_Info();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        item.Dcode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        item.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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
        public override void Add(TPCB_Info item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(TPCB_Info item, IUnitOfWork uow)
        {
            throw new NotImplementedException("Normal");
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(TPCB_Info item, IUnitOfWork uow)
        {
            throw new NotImplementedException("Normal");
        }

        #endregion

        #region ITPCBInfoRepository Members

        public IList<IMES.DataModel.FamilyInfo> GetFamilyList()
        {
            //SELECT '' as Family UNION SELECT DISTINCT CASE (CHARINDEX(' ', Family) - 1) WHEN -1 THEN Family
            //ELSE SUBSTRING(Family, 1, (CHARINDEX(' ', Family) - 1)) END AS Family 
            //FROM IMES_GetData..Model	
            //WHERE LEFT(Model, 4) <> '1397'AND ISNULL(Family, '') <> ''	
            //ORDER BY Family
            try
            {
                IList<IMES.DataModel.FamilyInfo> ret = new List<IMES.DataModel.FamilyInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence =   "SELECT DISTINCT CASE (CHARINDEX(' ', {1}) - 1) WHEN -1 THEN {1} " + 
                                            "ELSE SUBSTRING({1}, 1, (CHARINDEX(' ', {1}) - 1)) END AS {1} " + 
                                            "FROM {0} " + 
                                            "WHERE LEFT({2}, 4) <> '1397' AND ISNULL({1}, '') <> '' " + 	
                                            "ORDER BY {1}";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Model).Name,
                                                                            _Schema.Model.fn_Family,
                                                                            _Schema.Model.fn_model);

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.DataModel.FamilyInfo item = new IMES.DataModel.FamilyInfo();
                        item.friendlyName = item.id = GetValue_Str(sqlR, 0);
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

        public IList<string> GetTypeList(string family)
        {
            //SELECT '' as [Type] UNION
            //SELECT DISTINCT RTRIM(Type) as [Type]
            //FROM TPCB WHERE Family = @Family AND PdLine = 'TPCB' ORDER BY [Type]
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.Family = family;
                        eqCond.PdLine = "TPCB";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "DISTINCT", new List<string>() { _Schema.TPCB.fn_Type }, eqCond, null, null, null, null, null, null, null);
                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.TPCB.fn_Type);
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        ret.Add(item);
                    }
                }
                return (from item in ret select item).Distinct().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartNoList(string family, string type)
        {
            //SELECT '' as [Part No] UNION
            //SELECT DISTINCT RTRIM(PartNo) as [Part No]
            //FROM TPCB	WHERE Family = @Family AND Type = @Type AND PdLine = 'TPCB'	ORDER BY [Part No]
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.Family = family;
                        eqCond.Type = type;
                        eqCond.PdLine = "TPCB";
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "DISTINCT", new List<string>() { _Schema.TPCB.fn_PartNo }, eqCond, null, null, null, null, null, null, null);
                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.TPCB.fn_PartNo);
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_Type].Value = type;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        ret.Add(item);
                    }
                }
                return (from item in ret select item).Distinct().ToList();
             }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDCode(string family, string partno)
        {
            //SELECT RTRIM(Dcode) as [Date Code]
            //FROM TPCB WHERE Family = @Family AND PartNo = @PartNo AND PdLine = 'TPCB'
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.Family = family;
                        eqCond.PdLine = "TPCB";
                        eqCond.PartNo = partno;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "DISTINCT", new List<string>() { _Schema.TPCB.fn_Dcode }, eqCond, null, null, null, null, null, null, null);
                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine;
                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.TPCB.fn_Type);
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partno;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
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

        public IList<string> GetVendorSN(string family, string partno)
        {
            //SELECT RTRIM(Vendor) as [Vendor SN]
            //FROM TPCB WHERE Family = @Family AND PartNo = @PartNo AND PdLine = 'TPCB'
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.Family = family;
                        eqCond.PdLine = "TPCB";
                        eqCond.PartNo = partno;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "DISTINCT", new List<string>() { _Schema.TPCB.fn_Vendor }, eqCond, null, null, null, null, null, null, null);
                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine;
                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.TPCB.fn_Type);
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partno;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
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

        public IList<TPCBInfo> Query(string family, string pdline)
        {
            //SELECT RTRIM(Family) as Family, RTRIM(PdLine) as [PdLine], 
            //RTRIM(Type) as [Type],RTRIM(PartNo) as [Part No], 
            //RTRIM(Vendor) as Vendor, 
            //RTRIM(Dcode) as [Date Code],
            //RTRIM(Editor) as Editor, 
            //Cdt as [Create Date]	
            //FROM TPCB 
            //WHERE Family = @Family AND PdLine = @PdLine ORDER BY Type, PartNo
            try
            {
                IList<TPCBInfo> ret = new List<TPCBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.Family = family;
                        eqCond.PdLine = pdline;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), eqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",",new string[]{ _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo }));
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = pdline;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCBInfo item = new TPCBInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        item.DCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        item.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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

        public IList<TPCBInfo> Query(string family, string pdline, string partNo)
        {
            try
            {
                IList<TPCBInfo> ret = new List<TPCBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.Family = family;
                        eqCond.PdLine = pdline;
                        eqCond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), eqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo }));
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = pdline;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCBInfo item = new TPCBInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        item.DCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        item.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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

        public void SaveTPCB(TPCBInfo value)
        {
            //IIF EXISTS(SELECT * FROM TPCB WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo)
                //UPDATE TPCB SET Dcode = @Dcode, Editor = @Editor, Cdt = GETDATE()
                //WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo
            //ELSE
                //INSERT INTO [TPCB]([Family],[PdLine],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
                //VALUES (@Family, @PdLine, @Type, @PartNo, @Vendor, @Dcode, @Editor, GETDATE())
            try
            {
                SqlTransactionManager.Begin();

                if (PeekTPCB(value.Family, value.PdLine, value.PartNo))
                    UpdateTPCB(value);
                else
                    InsertTPCB(value);

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

        private bool PeekTPCB(string family, string pdline, string partNo)
        {
            SqlDataReader sqlR = null;
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.Family = family;
                        cond.PdLine = pdline;
                        cond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "COUNT", new List<string>() { _Schema.TPCB.fn_Family }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = pdline;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partNo;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    ret = cnt > 0 ? true : false;
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

        private void UpdateTPCB(TPCBInfo value)
        {
            //UPDATE TPCB SET Dcode = @Dcode, Editor = @Editor, Udt = GETDATE()
            //WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.Family = value.Family;
                        cond.PdLine = value.PdLine;
                        cond.PartNo = value.PartNo;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), new List<string>() { _Schema.TPCB.fn_Dcode, _Schema.TPCB.fn_Editor/*, _Schema.TPCB.fn_Cdt*/ }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = value.Family;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = value.PdLine;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = value.PartNo;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Dcode)].Value = value.DCode;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Editor)].Value = value.Editor;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Cdt)].Value = value.Cdt = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Udt)].Value = value.Udt = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertTPCB(TPCBInfo value)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.TPCB.fn_Cdt].Value = value.Cdt = cmDt;
                sqlCtx.Params[_Schema.TPCB.fn_Dcode].Value = value.DCode;
                sqlCtx.Params[_Schema.TPCB.fn_Editor].Value = value.Editor;
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = value.Family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = value.PartNo;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = value.PdLine;
                sqlCtx.Params[_Schema.TPCB.fn_Type].Value = value.Type;
                sqlCtx.Params[_Schema.TPCB.fn_Vendor].Value = value.Vendor;
                sqlCtx.Params[_Schema.TPCB.fn_Udt].Value = value.Udt = cmDt;
                value.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteTPCB(string family, string pdline, string partno)
        {
            // DELETE FROM TPCB WHERE Family = @Family AND PdLine = @PdLine AND PartNo = @PartNo
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.Family = family;
                        cond.PdLine = pdline;
                        cond.PartNo = partno;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = pdline;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partno;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveTPCBDet(string tpcbCode, string family, string pdline, string editor)
        {
            //INSERT INTO [TPCBDet]([Code],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
            //SELECT @TPCBCode, Type, PartNo, Vendor, Dcode, @Editor, GETDATE()
            //FROM TPCB 
            //WHERE Family = @Family AND PdLine = @PdLine
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = "INSERT INTO {0} ({1}) " +
                                            "SELECT {2} " +
                                            "FROM {3} " +
                                            "WHERE {4} = @{4} AND {5} = @{5} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.TPCBDet).Name,
                            string.Join(",", new string[] { _Schema.TPCBDet.fn_Code, _Schema.TPCBDet.fn_Type, _Schema.TPCBDet.fn_PartNo, _Schema.TPCBDet.fn_Vendor, _Schema.TPCBDet.fn_Dcode, _Schema.TPCBDet.fn_Editor, _Schema.TPCBDet.fn_Cdt }),
                            string.Join(",", new string[] { "@" + _Schema.TPCBDet.fn_Code, _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo, _Schema.TPCB.fn_Vendor, _Schema.TPCB.fn_Dcode, "@" + _Schema.TPCBDet.fn_Editor, "@" + _Schema.TPCBDet.fn_Cdt }),
                            typeof(_Schema.TPCB).Name,
                            _Schema.TPCB.fn_Family,
                            _Schema.TPCB.fn_PdLine);

                        sqlCtx.Params.Add(_Schema.TPCBDet.fn_Code, new SqlParameter("@" + _Schema.TPCBDet.fn_Code, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.TPCBDet.fn_Editor, new SqlParameter("@" + _Schema.TPCBDet.fn_Editor, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.TPCBDet.fn_Cdt, new SqlParameter("@" + _Schema.TPCBDet.fn_Cdt, SqlDbType.DateTime));
                        sqlCtx.Params.Add(_Schema.TPCB.fn_Family, new SqlParameter("@" + _Schema.TPCB.fn_Family, SqlDbType.VarChar));
                        sqlCtx.Params.Add(_Schema.TPCB.fn_PdLine, new SqlParameter("@" + _Schema.TPCB.fn_PdLine, SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx.Params[_Schema.TPCBDet.fn_Code].Value = tpcbCode;
                sqlCtx.Params[_Schema.TPCBDet.fn_Editor].Value = editor;
                sqlCtx.Params[_Schema.TPCBDet.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = pdline;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TPCBDet> CheckTPCBDet(string tpcbCode, string editor)
        {
            try
            {
                IList<TPCBDet> ret = new List<TPCBDet>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCBDet eqCond = new _Schema.TPCBDet();
                        eqCond.Code = tpcbCode;
                        eqCond.Editor = editor;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCBDet), eqCond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TPCBDet.fn_Code].Value = tpcbCode;
                sqlCtx.Params[_Schema.TPCBDet.fn_Editor].Value = editor;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCBDet item = new TPCBDet();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCBDet.fn_Cdt]);
                        item.Code = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCBDet.fn_Code]);
                        item.DCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCBDet.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCBDet.fn_Editor]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCBDet.fn_PartNo]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCBDet.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCBDet.fn_Vendor]);
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

        private void PersistInsertTPCB(TPCB_Info item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.TPCB.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.TPCB.fn_Dcode].Value = item.Dcode;
                sqlCtx.Params[_Schema.TPCB.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = item.PartNo;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = item.PdLine;
                sqlCtx.Params[_Schema.TPCB.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.TPCB.fn_Vendor].Value = item.Vendor;
                sqlCtx.Params[_Schema.TPCB.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Defered

        public void SaveTPCBDefered(IUnitOfWork uow, TPCBInfo value)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), value);
        }

        public void DeleteTPCBDefered(IUnitOfWork uow, string family, string pdline, string partno)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), family, pdline, partno);
        }

        public void SaveTPCBDetDefered(IUnitOfWork uow, string tpcbCode, string family, string pdline, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), tpcbCode, family, pdline, editor);
        }

        #endregion

        #region For Maintain

        public IList<TPCBInfo> GetTpcbList()
        {
            try
            {
                IList<TPCBInfo> ret = new List<TPCBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.PdLine = "TPCB";
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), eqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.TPCB.fn_Family, _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo, _Schema.TPCB.fn_Vendor }));

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine; 
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCBInfo item = new TPCBInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        item.DCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        item.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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

        public IList<TPCBInfo> GetTpcbList(string family)
        {
            try
            {
                IList<TPCBInfo> ret = new List<TPCBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.PdLine = "TPCB";
                        eqCond.Family = family;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), eqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.TPCB.fn_Family, _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo, _Schema.TPCB.fn_Vendor }));

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine;
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCBInfo item = new TPCBInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        item.DCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        item.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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

        public void SaveTPCBInfo(TPCBInfo item)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (PeekTPCB_Maintain(item))
                    UpdateTPCB_Maintain(item);
                else
                    InsertTPCB_Maintain(item);

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

        private bool PeekTPCB_Maintain(TPCBInfo value)
        {
            //SELECT * FROM [IMES_FA].[dbo].[TPCB]
            //          WHERE PdLine = 'TPCB'
            //                    AND Family = @Family
            //                    AND PartNo = @PartNo)
            SqlDataReader sqlR = null;
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.Family = value.Family;
                        cond.PdLine = "TPCB";
                        cond.PartNo = value.PartNo;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "COUNT", new List<string>() { _Schema.TPCB.fn_Family }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = cond.PdLine;
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = value.Family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = value.PartNo;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    ret = cnt > 0 ? true : false;
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

        private void UpdateTPCB_Maintain(TPCBInfo value)
        {
            //UPDATE [IMES_FA].[dbo].[TPCB] SET Vendor = @VendorSN, Editor = @Editor, Udt = GETDATE()
            //WHERE PdLine = 'TPCB' AND Family = @Family AND PartNo = @PartNo
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.Family = value.Family;
                        cond.PdLine = "TPCB";
                        cond.PartNo = value.PartNo;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), new List<string>() { _Schema.TPCB.fn_Vendor, _Schema.TPCB.fn_Editor /*, _Schema.TPCB.fn_Cdt*/ }, null, null, null, cond, null, null, null, null, null, null, null);

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = cond.PdLine; 
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = value.Family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = value.PartNo;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Vendor)].Value = value.Vendor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Editor)].Value = value.Editor;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Cdt)].Value = value.Cdt = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Udt)].Value = value.Udt = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertTPCB_Maintain(TPCBInfo value)
        {
            //INSERT INTO [IMES_FA].[dbo].[TPCB]([Family],[PdLine],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
            //VALUES (@Family, 'TPCB', @Type, @PartNo, @VendorSN, '', @Editor, GETDATE())
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.TPCB.fn_Cdt].Value = value.Cdt = cmDt;
                sqlCtx.Params[_Schema.TPCB.fn_Dcode].Value = string.Empty;
                sqlCtx.Params[_Schema.TPCB.fn_Editor].Value = value.Editor;
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = value.Family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = value.PartNo;
                sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = "TPCB";
                sqlCtx.Params[_Schema.TPCB.fn_Type].Value = value.Type;
                sqlCtx.Params[_Schema.TPCB.fn_Vendor].Value = value.Vendor;
                sqlCtx.Params[_Schema.TPCB.fn_Udt].Value = value.Udt = cmDt;
                value.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetID(string family, string partNo)
        {
            // SELECT ID
            // FROM [IMES_FA].[dbo].[TPCB]
            // WHERE PdLine = 'TPCB'
            //       AND Family = @Family
            //       AND PartNo = @PartNo
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.PdLine = "TPCB";
                        eqCond.Family = family;
                        eqCond.PartNo = partNo;

                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "TOP 1", new List<string>() { _Schema.TPCB.fn_ID }, eqCond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo }));

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine; 
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partNo;
;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteTPCBInfo(string family, string partNo)
        {
            //DELETE FROM [IMES_FA].[dbo].[TPCB]
            //WHERE PdLine = 'TPCB'
            //AND Family = @Family
            //AND PartNo = @PartNo
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.Family = family;
                        cond.PdLine = "TPCB";
                        cond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), cond, null, null);

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = cond.PdLine;
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TPCBInfo> CheckHasList(string family, string partNo)
        {
            // SELECT *  FROM [IMES_FA].[dbo].[TPCB]
            // WHERE PdLine = 'TPCB' AND Family = @Family AND PartNo = @PartNo
            try
            {
                IList<TPCBInfo> ret = new List<TPCBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.PdLine = "TPCB";
                        eqCond.Family = family;
                        eqCond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), eqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.TPCB.fn_Family, _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo, _Schema.TPCB.fn_Vendor }));

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine;
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCBInfo item = new TPCBInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        item.DCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        item.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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

        public IList<TPCBInfo> CheckSameList(TPCBInfo cond)
        {
            //SELECT * FROM [IMES_FA].[dbo].[TPCB]
            //WHERE PdLine = 'TPCB' AND Family = @Family AND Type = @Type AND PartNo = @PartNo
            try
            {
                IList<TPCBInfo> ret = new List<TPCBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB eqCond = new _Schema.TPCB();
                        eqCond.PdLine = "TPCB";
                        eqCond.Family = cond.Family;
                        eqCond.Type = cond.Type;
                        eqCond.PartNo = cond.PartNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), eqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.TPCB.fn_Family, _Schema.TPCB.fn_Type, _Schema.TPCB.fn_PartNo, _Schema.TPCB.fn_Vendor }));

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = eqCond.PdLine;
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = cond.Family;
                sqlCtx.Params[_Schema.TPCB.fn_Type].Value = cond.Type;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = cond.PartNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TPCBInfo item = new TPCBInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Cdt]);
                        item.DCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Dcode]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Family]);
                        item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PartNo]);
                        item.PdLine = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_PdLine]);
                        item.Type = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Type]);
                        item.Vendor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Vendor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_ID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.TPCB.fn_Udt]);
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

        public void UpdateTPCBInfo(TPCBInfo item)
        {
            // IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[TPCB]
            //          WHERE PdLine = 'TPCB'
            //                    AND Family = @Family
            //                    AND PartNo = @PartNo)
            //         UPDATE [IMES_FA].[dbo].[TPCB] 
            //                    SET Vendor = @VendorSN,
            //                         Type=@Type,
            //                         Editor = @Editor,
            //                         Udt = GETDATE()
            //                    WHERE PdLine = 'TPCB'
            //                         AND Family = @Family
            //                         AND PartNo = @PartNo
            try
            {
                SqlTransactionManager.Begin();

                if (PeekTPCB_Maintain(item))
                    UpdateTPCBInfo_Maintain(item);

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

        private void UpdateTPCBInfo_Maintain(TPCBInfo value)
        {
            //UPDATE [IMES_FA].[dbo].[TPCB] SET Vendor = @VendorSN, Type=@Type, Editor = @Editor, Udt = GETDATE()
            //WHERE PdLine = 'TPCB' AND Family = @Family AND PartNo = @PartNo
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.Family = value.Family;
                        cond.PdLine = "TPCB";
                        cond.PartNo = value.PartNo;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), new List<string>() { _Schema.TPCB.fn_Vendor, _Schema.TPCB.fn_Editor, _Schema.TPCB.fn_Type }, null, null, null, cond, null, null, null, null, null, null, null);

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = cond.PdLine;
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = value.Family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = value.PartNo;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Vendor)].Value = value.Vendor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Type)].Value = value.Type;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Editor)].Value = value.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TPCB.fn_Udt)].Value = value.Udt = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertTPCBInfo(TPCBInfo item)
        {
            // INSERT INTO [IMES_FA].[dbo].[TPCB]([Family],[PdLine],[Type],[PartNo],[Vendor],[Dcode],[Editor],[Cdt])
            // VALUES (@Family, 'TPCB', @Type, @PartNo, @VendorSN, '', @Editor, GETDATE())
            try
            {
                this.InsertTPCB_Maintain(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckExistsRecord(string family, string partNo)
        {
            // SELECT COUNT(*) FROM [IMES_FA].[dbo].[TPCB]
            // WHERE  PdLine = 'TPCB' AND [Family]=@Family AND [PartNo]=@PartNo
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TPCB cond = new _Schema.TPCB();
                        cond.PdLine = "TPCB";
                        cond.Family = family;
                        cond.PartNo = partNo;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TPCB), "COUNT", new List<string>() { _Schema.TPCB.fn_ID }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Params[_Schema.TPCB.fn_PdLine].Value = cond.PdLine;
                    }
                }
                sqlCtx.Params[_Schema.TPCB.fn_Family].Value = family;
                sqlCtx.Params[_Schema.TPCB.fn_PartNo].Value = partNo;
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

        #region Defered

        public void SaveTPCBInfoDefered(IUnitOfWork uow, TPCBInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteTPCBInfoDefered(IUnitOfWork uow, string family, string partNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), family, partNo);
        }

        public void UpdateTPCBInfoDefered(IUnitOfWork uow, TPCBInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertTPCBInfoDefered(IUnitOfWork uow, TPCBInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #endregion
    }
}
