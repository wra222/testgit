using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.PrintLog;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: PrintLog相关
    /// </summary>
    public class PrintLogRepository : BaseRepository<fons.PrintLog>, IPrintLogRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<PrintLog>

        protected override void PersistNewItem(fons.PrintLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertPrintLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        protected override void PersistUpdatedItem(fons.PrintLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdatePrintLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        protected override void PersistDeletedItem(fons.PrintLog item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeletePrintLog(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<PrintLog>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.PrintLog Find(object key)
        {
            try
            {
                fons.PrintLog ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintLog cond = new _Schema.PrintLog();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintLog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PrintLog.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.PrintLog();
                        ret.BeginNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_BegNo]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Cdt]);
                        ret.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Descr]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Editor]);
                        ret.EndNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_EndNo]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_ID]);
                        ret.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Name]);
                        ret.LabelTemplate = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_LabelTemplate]);
                        ret.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Station]);
                   
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
        public override IList<fons.PrintLog> FindAll()
        {
            try
            {
                IList<fons.PrintLog> ret = new List<fons.PrintLog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintLog));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PrintLog item = new fons.PrintLog();
                        item.BeginNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_BegNo]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Cdt]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Editor]);
                        item.EndNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_EndNo]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_ID]);
                        item.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Name]);
                        item.LabelTemplate = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_LabelTemplate]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Station]);
                   
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
        public override void Add(fons.PrintLog item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(fons.PrintLog item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons.PrintLog item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region Implementation of IPrintLogRepository

        public IList<fons.PrintLog> GetPrintLogListByDescr(string descr)
        {
            try
            {
                IList<fons.PrintLog> ret = new List<fons.PrintLog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintLog cond = new _Schema.PrintLog();
                        cond.Descr = descr;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintLog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PrintLog.fn_Descr].Value = descr;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PrintLog item = new fons.PrintLog();
                        item.BeginNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_BegNo]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Cdt]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Descr]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Editor]);
                        item.EndNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_EndNo]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_ID]);
                        item.Name = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Name]);
                        item.LabelTemplate = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_LabelTemplate]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintLog.fn_Station]);
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

        public IList<fons.PrintLog> GetPrintLogListByRange(string prodId)
        {
            try
            {
                IList<fons.PrintLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.PrintLog cond = new _Metas.PrintLog();
                        cond.begNo = prodId;
                        _Metas.PrintLog cond2 = new _Metas.PrintLog();
                        cond2.endNo = prodId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PrintLog>(tk, null, null, new ConditionCollection<_Metas.PrintLog>(
                            new SmallerOrEqualCondition<_Metas.PrintLog>(cond),
                            new GreaterOrEqualCondition<_Metas.PrintLog>(cond2)));
                    }
                }
                sqlCtx.Param(g.DecSE(_Metas.PrintLog.fn_begNo)).Value = prodId;
                sqlCtx.Param(g.DecGE(_Metas.PrintLog.fn_endNo)).Value = prodId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::PrintLog, fons.PrintLog, fons.PrintLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons.PrintLog> GetPrintLogListByCondition(fons.PrintLog condition)
        {
            try
            {
                IList<fons.PrintLog> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::PrintLog cond = mtns::FuncNew.SetColumnFromField<mtns::PrintLog, fons.PrintLog>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::PrintLog>(null, null, new mtns::ConditionCollection<mtns::PrintLog>(new mtns::EqualCondition<mtns::PrintLog>(cond)), mtns::PrintLog.fn_id);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PrintLog, fons.PrintLog>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::PrintLog, fons.PrintLog, fons.PrintLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPrintLogByLabelNameAndDescr(string name, string descr)
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
                        _Metas.PrintLog cond = new _Metas.PrintLog();
                        cond.name = name;
                        cond.descr = descr;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PrintLog>(tk, "COUNT", new string[] { _Metas.PrintLog.fn_id }, new ConditionCollection<_Metas.PrintLog>(new EqualCondition<_Metas.PrintLog>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PrintLog.fn_name).Value = name;
                sqlCtx.Param(_Metas.PrintLog.fn_descr).Value = descr;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public IList<fons.PrintLog> GetPrintLogListByRange(string mbSno, string name)
        {
            try
            {
                IList<fons.PrintLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.PrintLog cond = new _Metas.PrintLog();
                        cond.begNo = mbSno;
                        _Metas.PrintLog cond2 = new _Metas.PrintLog();
                        cond2.endNo = mbSno;
                        _Metas.PrintLog cond3 = new _Metas.PrintLog();
                        cond3.name = name;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PrintLog>(tk, null, null, new ConditionCollection<_Metas.PrintLog>(
                            new SmallerOrEqualCondition<_Metas.PrintLog>(cond),
                            new GreaterOrEqualCondition<_Metas.PrintLog>(cond2),
                            new EqualCondition<_Metas.PrintLog>(cond3)));
                    }
                }
                sqlCtx.Param(g.DecSE(_Metas.PrintLog.fn_begNo)).Value = mbSno;
                sqlCtx.Param(g.DecGE(_Metas.PrintLog.fn_endNo)).Value = mbSno;
                sqlCtx.Param(_Metas.PrintLog.fn_name).Value = name;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::PrintLog, fons.PrintLog, fons.PrintLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPrintListInfo(IMES.DataModel.PrintListInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns.PrintList>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns.PrintList, IMES.DataModel.PrintListInfo>(sqlCtx, item);

                sqlCtx.Param(mtns.PrintList.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region IPrintLogRepository Members

        public void InsertPrintListInfoDefered(IUnitOfWork uow, IMES.DataModel.PrintListInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #endregion

        #region . Inners .

        private void PersistInsertPrintLog(fons.PrintLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PrintLog.fn_BegNo].Value = item.BeginNo;
                sqlCtx.Params[_Schema.PrintLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PrintLog.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.PrintLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PrintLog.fn_EndNo].Value = item.EndNo;
                sqlCtx.Params[_Schema.PrintLog.fn_LabelTemplate].Value = item.LabelTemplate;
                sqlCtx.Params[_Schema.PrintLog.fn_Station].Value = item.Station;
                //sqlCtx.Params[_Schema.PrintLog.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PrintLog.fn_Name].Value = item.Name;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePrintLog(fons.PrintLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintLog));
                    }
                }
                sqlCtx.Params[_Schema.PrintLog.fn_BegNo].Value = item.BeginNo;
                sqlCtx.Params[_Schema.PrintLog.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.PrintLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PrintLog.fn_EndNo].Value = item.EndNo;
                sqlCtx.Params[_Schema.PrintLog.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.PrintLog.fn_Name].Value = item.Name;
                sqlCtx.Params[_Schema.PrintLog.fn_LabelTemplate].Value = item.LabelTemplate;
                sqlCtx.Params[_Schema.PrintLog.fn_Station].Value = item.Station;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePrintLog(fons.PrintLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintLog));
                    }
                }
                sqlCtx.Params[_Schema.PrintLog.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region check print log method by Vincent
        public bool CheckPrintLogListByRange(string mbSno, string name)
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
                        sqlCtx.Sentence = @"SELECT top 1 ID  
                                                            FROM PrintLog 
                                                            WHERE BegNo<=@mbSno AND 
                                                                           EndNo>=@mbSno AND 
                                                                           Name=@Name";

                        sqlCtx.AddParam("mbSno", new SqlParameter("@mbSno", SqlDbType.VarChar));
                        sqlCtx.AddParam("Name", new SqlParameter("@Name", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }



                sqlCtx.Param("mbSno").Value = mbSno;
                sqlCtx.Param("Name").Value = name;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
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

        public bool CheckPrintLogListByRange(string mbSno, string name, string labelTemplate)
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
                        sqlCtx.Sentence = @"SELECT top 1 ID  
                                                            FROM PrintLog 
                                                            WHERE BegNo<=@mbSno AND 
                                                                           EndNo>=@mbSno AND 
                                                                           Name=@Name and
                                                                          LabelTemplate=@LabelTemplate";

                        sqlCtx.AddParam("mbSno", new SqlParameter("@mbSno", SqlDbType.VarChar));
                        sqlCtx.AddParam("Name", new SqlParameter("@Name", SqlDbType.VarChar));
                        sqlCtx.AddParam("LabelTemplate", new SqlParameter("@LabelTemplate", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("mbSno").Value = mbSno;
                sqlCtx.Param("Name").Value = name;
                sqlCtx.Param("LabelTemplate").Value = labelTemplate;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
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

       public bool CheckPrintLogListByRange(string begSno,string endSno, string name, string labelTemplate)
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
                        sqlCtx.Sentence = @"SELECT top 1 ID  
                                                            FROM PrintLog 
                                                            WHERE BegNo=@BegSno AND 
                                                                          EndNo=@EndSno AND 
                                                                          Name=@Name and
                                                                          LabelTemplate=@LabelTemplate";

                        sqlCtx.AddParam("BegSno", new SqlParameter("@BegSno", SqlDbType.VarChar));
                        sqlCtx.AddParam("EndSno", new SqlParameter("@EndSno", SqlDbType.VarChar));
                        sqlCtx.AddParam("Name", new SqlParameter("@Name", SqlDbType.VarChar));
                        sqlCtx.AddParam("LabelTemplate", new SqlParameter("@LabelTemplate", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }



                sqlCtx.Param("BegSno").Value = begSno;
                sqlCtx.Param("EndSno").Value = endSno;
                sqlCtx.Param("Name").Value = name;
                sqlCtx.Param("LabelTemplate").Value = labelTemplate;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
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
    }
}
