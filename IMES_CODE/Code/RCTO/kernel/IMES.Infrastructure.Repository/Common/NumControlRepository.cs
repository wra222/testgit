// 2010-04-28 Liu Dong(eB1-4)         Modify 生成号: 不再将每个生成的号码都存NumControl表了.
// 2011-05-18 Liu Dong(eB1-4)         Modify ITC-1281-0025 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.NumControl;
using IMES.DataModel;
//using IMES.FisObject.Common.Part;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: NumControl相关
    /// </summary>
    public class NumControlRepository : BaseRepository<fons.NumControl>, INumControlRepository
    {
        private static GetValueClass g = new GetValueClass();

        private static MathSequenceWithCarryNumberRule _macr = new MathSequenceWithCarryNumberRule(12, "0123456789ABCDEF");

        #region Overrides of BaseRepository<NumControl>

        protected override void PersistNewItem(fons.NumControl item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertNumControl(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(fons.NumControl item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateNumControl(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(fons.NumControl item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteNumControl(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<NumControl>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.NumControl Find(object key)
        {
            try
            {
                fons.NumControl ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.NumControl(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_ID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_NoType]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_NoName]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_Value]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_CustomerID]));
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
        public override IList<fons.NumControl> FindAll()
        {
            try
            {
                IList<fons.NumControl> ret = new List<fons.NumControl>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            fons.NumControl item = new fons.NumControl(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_ID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_NoType]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_NoName]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_Value]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.NumControl.fn_CustomerID]));
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
        public override void Add(fons.NumControl item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(fons.NumControl item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons.NumControl item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region Implementation of INumControlRepository

        public string GetMaxNumber(string type, string preStr, string customer)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                string ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl likecond = new _Schema.NumControl();
                        likecond.Value = string.Format(preStr, "%");

                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = type;
                        cond.CustomerID = customer;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), "MAX", new List<string>() { _Schema.NumControl.fn_Value }, cond, likecond, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_Value].Value = string.Format(preStr, "%");
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = type;
                sqlCtx.Params[_Schema.NumControl.fn_CustomerID].Value = customer;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes["MAX"]);
                    }
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}

                    SqlTransactionManager.Commit();

                return ret;
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

        public fons.NumControl GetMaxNumberObj(string type, string preStr, string customer)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                fons.NumControl ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.NumControl likecond = new _Metas.NumControl();
                        likecond.value = string.Format(preStr, "%");

                        _Metas.NumControl cond = new _Metas.NumControl();
                        cond.noType = type;
                        cond.customer = customer;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.NumControl>(tk, "TOP 1", null, new ConditionCollection<_Metas.NumControl>(new EqualCondition<_Metas.NumControl>(cond), new LikeCondition<_Metas.NumControl>(likecond)), _Metas.NumControl.fn_value + FuncNew.DescendOrder);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.NumControl.fn_value).Value = string.Format(preStr, "%");
                sqlCtx.Param(_Metas.NumControl.fn_noType).Value = type;
                sqlCtx.Param(_Metas.NumControl.fn_customer).Value = customer;

                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                    ret = FuncNew.SetFieldFromColumn<_Metas.NumControl, IMES.FisObject.Common.NumControl.NumControl>(ret, sqlR, sqlCtx);
                    if (ret != null)
                        ret.Tracker.Clear();
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}

                SqlTransactionManager.Commit();

                return ret;
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

        public string GetMaxNumber(string type, string preStr)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                string ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl likecond = new _Schema.NumControl();
                        likecond.Value = string.Format(preStr, "%");

                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = type;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), "MAX", new List<string>() { _Schema.NumControl.fn_Value }, cond, likecond, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_Value].Value = string.Format(preStr, "%");
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = type;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes["MAX"]);
                    }
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}

                SqlTransactionManager.Commit();

                return ret;
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

        public fons.NumControl GetMaxNumberObj(string type, string preStr)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                fons.NumControl ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.NumControl likecond = new _Metas.NumControl();
                        likecond.value = string.Format(preStr, "%");

                        _Metas.NumControl cond = new _Metas.NumControl();
                        cond.noType = type;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.NumControl>(tk, "TOP 1", null, new ConditionCollection<_Metas.NumControl>(new EqualCondition<_Metas.NumControl>(cond), new LikeCondition<_Metas.NumControl>(likecond)), _Metas.NumControl.fn_value + FuncNew.DescendOrder);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.NumControl.fn_value).Value = string.Format(preStr, "%");
                sqlCtx.Param(_Metas.NumControl.fn_noType).Value = type;
 
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                    ret = FuncNew.SetFieldFromColumn<_Metas.NumControl, IMES.FisObject.Common.NumControl.NumControl>(ret, sqlR, sqlCtx);
                    if (ret != null)
                        ret.Tracker.Clear();
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}

                SqlTransactionManager.Commit();

                return ret;
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

        public string GetMaxUCCIDNumber(string type)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                string ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = type;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), "MAX", new List<string>() { _Schema.NumControl.fn_Value }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=IDX_NumControl_NoType,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = type;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes["MAX"]);
                    }
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}

                SqlTransactionManager.Commit();

                return ret;
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

        // 2010-04-28 Liu Dong(eB1-4)         Modify 生成号: 不再将每个生成的号码都存NumControl表了.
        public void SaveMaxNumber(fons.NumControl item, bool insertOrUpdate, string preStr)
        {
            try
            {
                if (insertOrUpdate)
                    this.PersistInsertNumControl(item);
                else
                    this.PersistUpdateNumControlByTypeAndPreStr(item, preStr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveMaxNumberWithOutByCustomer(fons.NumControl item, bool insertOrUpdate, string preStr)
        {
            try
            {
                if (insertOrUpdate)
                    this.PersistInsertNumControl(item);
                else
                    this.PersistUpdateNumControlByTypeAndPreStrWithOutByCustomer(item, preStr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveMaxNumber(fons.NumControl item, bool insertOrUpdate)
        {
            try
            {
                if (insertOrUpdate)
                    this.PersistInsertNumControl(item);
                else
                    this.PersistUpdateNumControlByID(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MACRange GetMaxMACRange(string _111PartCode, string rangeStatus, out string maxNumber)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                MACRange ret = null;
                maxNumber = null;
                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.NumControl);
                        _Schema.NumControl equalcond1 = new _Schema.NumControl();
                        equalcond1.NoName = _111PartCode;
                        equalcond1.NoType = "MAC";
                        tf1.equalcond = equalcond1;
                        tf1.ToGetFieldNames.Add(_Schema.NumControl.fn_Value);


                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.MACRange);
                        tf2.subDBCalalog = _Schema.SqlHelper.DB_PCA;
                        _Schema.MACRange equalcond2 = new _Schema.MACRange();
                        equalcond2.Code = _111PartCode;
                        equalcond2.Status = rangeStatus;
                        tf2.equalcond = equalcond2;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.NumControl.fn_Value, tf2, _Schema.MACRange.fn_BegNo, "{0}>={1}");
                        tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf1, _Schema.NumControl.fn_Value, tf2, _Schema.MACRange.fn_EndNo, "{0}<={1}");
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "TOP 1", ref tblAndFldsesArray, tblCnnts);
                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.NumControl.fn_NoType)].Value = equalcond1.NoType;
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderByDesc, _Schema.Func.DecAliasInner(tf1.alias, _Schema.NumControl.fn_Value));

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.NumControl.fn_NoName)].Value = _111PartCode;
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_Code)].Value = _111PartCode;
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_Status)].Value = rangeStatus;

                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = new MACRange(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_ID)]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_Code)]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_BegNo)]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_EndNo)]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_Status)]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_Editor)]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_Cdt)]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.MACRange.fn_Udt)]));
                            maxNumber = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.NumControl.fn_Value)]);
                            ret.Tracker.Clear();
                        }
                    }
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}
                    SqlTransactionManager.Commit();

                return ret;
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

        public MACRange GetAvailableRange(string _111PartCode, string rangeStatus)
        {
            //SELECT TOP 1 BegNo as [New MAC] FROM MACRange WHERE Status = 'R' AND Code = @Code ORDER BY Cdt
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                MACRange ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MACRange cond = new _Schema.MACRange();
                        cond.Code = _111PartCode;
                        cond.Status = rangeStatus;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), "TOP 1", null, cond, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MACRange.fn_Cdt);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=CodeStatusIndex,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.MACRange.fn_Code].Value = _111PartCode;
                sqlCtx.Params[_Schema.MACRange.fn_Status].Value = rangeStatus;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MACRange(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_ID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Code]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Status]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Editor]),
                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Cdt]),
                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Udt]));
                        ret.Tracker.Clear();
                    }
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}
                    SqlTransactionManager.Commit();

                return ret;
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

        public void SetMACRangeStatus(int macRangeId, string rangeStatus)
        {
            try
            {
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MACRange cond = new _Schema.MACRange();
                        cond.ID = macRangeId;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), new List<string>() { _Schema.MACRange.fn_Status, _Schema.MACRange.fn_Udt }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MACRange.fn_ID].Value = macRangeId;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MACRange.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MACRange.fn_Status)].Value = rangeStatus;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveMaxMAC(fons.NumControl item)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (PeekForSaveMaxMAC(item))
                    UpdateForSaveMaxMAC(item);
                else
                    PersistInsertNumControl(item);

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

        public IList<fons.NumControl> GetNumControlByNoTypeAndValue(string noType, string value)
        {
            try
            {
                IList<fons.NumControl> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::NumControl cond = new mtns::NumControl();
                        cond.noType = noType;
                        cond.value = value;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::NumControl>(tk, null, null, new ConditionCollection<mtns::NumControl>(new EqualCondition<mtns::NumControl>(cond)));
                    }
                }
                sqlCtx.Param(mtns::NumControl.fn_noType).Value = noType;
                sqlCtx.Param(mtns::NumControl.fn_value).Value = value;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::NumControl, fons.NumControl, fons.NumControl>(ret, sqlR, sqlCtx);
                }
                if (ret != null && ret.Count > 0)
                {
                    foreach(fons.NumControl nct in ret)
                    {
                        nct.Tracker.Clear();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertNumControl(fons.NumControl item)
        {
            PersistInsertNumControl(item);
        }

        public IList<AssetRangeInfo> GetAssetRangeInfo(string code)
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

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public string GetMaxAssetNumber(string noType, string noName, string customer)
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
                        tf1 = new TableAndFields<_Metas.NumControl>();
                        _Metas.NumControl equalcond1 = new _Metas.NumControl();
                        equalcond1.noType = noType;
                        equalcond1.noName = noName;
                        equalcond1.customer = customer;
                        tf1.Conditions.Add(new EqualCondition<_Metas.NumControl>(equalcond1));
                        tf1.AddRangeToGetFieldNames(_Metas.NumControl.fn_value);

                        tf2 = new TableAndFields<_Metas.AssetRange>();
                        _Metas.AssetRange equalcond2 = new _Metas.AssetRange();
                        equalcond2.code = noName;
                        tf2.Conditions.Add(new EqualCondition<_Metas.AssetRange>(equalcond2));
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_BOM;
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.NumControl, _Metas.AssetRange>(tf1, _Metas.NumControl.fn_value, tf2, _Metas.AssetRange.fn__Begin_, "{0}>={1}"),
                            new TableConnectionItem<_Metas.NumControl, _Metas.AssetRange>(tf1, _Metas.NumControl.fn_value, tf2, _Metas.AssetRange.fn__End_, "{0}<={1}"));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "TOP 1", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.NumControl.fn_noType)).Value = noType;
                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.NumControl.fn_noName)).Value = noName;
                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.NumControl.fn_customer)).Value = customer;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.AssetRange.fn_code)).Value = noName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns.NumControl.fn_value)));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveMaxAssetNumber(fons.NumControl item, bool insertOrUpdate)
        {
            try
            {
                if (insertOrUpdate)
                    this.PersistInsertNumControl(item);
                else
                    this.PersistUpdateNumControlByTypeAndNameAndCust(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MACRange GetMACRange(string code, string[] statuses)
        {
            SqlDataReader sqlR = null;
            try
            {
                MACRange ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Macrange cond = new _Metas.Macrange();
                        cond.status = "[INSET]";

                        _Metas.Macrange cond2 = new _Metas.Macrange();
                        cond2.code = code;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Macrange>(tk, "TOP 1", null, new ConditionCollection<_Metas.Macrange>(
                            new InSetCondition<_Metas.Macrange>(cond),
                            new EqualCondition<_Metas.Macrange>(cond2)), _Metas.Macrange.fn_cdt);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=CodeStatusIndex,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.Macrange.fn_code).Value = code;
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Macrange.fn_status), g.ConvertInSet(statuses));

                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params);
                if (sqlR != null && sqlR.Read())
                {
                    ret = FuncNew.SetFieldFromColumnWithoutReadReader<Macrange, MACRange>(ret, sqlR, sqlCtx);
                }
                if (ret != null)
                    ret.Tracker.Clear();
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

        public IMES.FisObject.Common.NumControl.NumControl GetMaxValue(string noType, string noName)
        {
            SqlDataReader sqlR = null;
            try
            {
                IMES.FisObject.Common.NumControl.NumControl ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.NumControl cond = new _Metas.NumControl();
                        cond.noType = noType;
                        cond.noName = noName;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.NumControl>(tk, "TOP 1", null, new ConditionCollection<_Metas.NumControl>(new EqualCondition<_Metas.NumControl>(cond)), _Metas.NumControl.fn_value + FuncNew.DescendOrder);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=NoNameNoTypeIndex,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.NumControl.fn_noType).Value = noType;
                sqlCtx.Param(_Metas.NumControl.fn_noName).Value = noName;

                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                if (sqlR != null && sqlR.Read()) 
                {
                    ret = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.NumControl, IMES.FisObject.Common.NumControl.NumControl>(ret, sqlR, sqlCtx);
                }
                if (ret != null)
                    ret.Tracker.Clear();
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

        public IMES.FisObject.Common.NumControl.NumControl GetMaxValue(string noType, string noName, string customer)
        {
            try
            {
                IMES.FisObject.Common.NumControl.NumControl ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.NumControl cond = new _Metas.NumControl();
                        cond.noType = noType;
                        cond.noName = noName;
                        cond.customer = customer;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.NumControl>(tk, "TOP 1", null, new ConditionCollection<_Metas.NumControl>(new EqualCondition<_Metas.NumControl>(cond)), _Metas.NumControl.fn_value + FuncNew.DescendOrder);
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=IDX_Type_Name_Custom,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.NumControl.fn_noType).Value = noType;
                sqlCtx.Param(_Metas.NumControl.fn_noName).Value = noName;
                sqlCtx.Param(_Metas.NumControl.fn_customer).Value = customer;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.NumControl, IMES.FisObject.Common.NumControl.NumControl>(ret, sqlR, sqlCtx);
                }
                if (ret != null)
                    ret.Tracker.Clear();
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Inners .

        private void PersistInsertNumControl(fons.NumControl item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl));
                    }
                }
                //DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.NumControl.fn_CustomerID].Value = item.Customer;
                //sqlCtx.Params[_Schema.NumControl.fn_ID].Value
                sqlCtx.Params[_Schema.NumControl.fn_NoName].Value = item.NOName;
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = item.NOType;
                sqlCtx.Params[_Schema.NumControl.fn_Value].Value = item.Value;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateNumControl(fons.NumControl item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl));
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_CustomerID].Value = item.Customer;
                sqlCtx.Params[_Schema.NumControl.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.NumControl.fn_NoName].Value = item.NOName;
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = item.NOType;
                sqlCtx.Params[_Schema.NumControl.fn_Value].Value = item.Value;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteNumControl(fons.NumControl item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl));
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool PeekForSaveMaxMAC(fons.NumControl item)
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
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = item.NOType;
                        cond.NoName = item.NOName;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), "COUNT", new List<string>() { _Schema.NumControl.fn_ID }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=NoNameNoTypeIndex,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = item.NOType;
                sqlCtx.Params[_Schema.NumControl.fn_NoName].Value = item.NOName;
                //using (SqlDataReader 
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                //)
                //{
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                        ret = cnt > 0 ? true : false;
                    }
                //}
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

        private void UpdateForSaveMaxMAC(fons.NumControl item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = item.NOType;
                        cond.NoName = item.NOName;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), new List<string>() { _Schema.NumControl.fn_Value, _Schema.NumControl.fn_CustomerID }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = item.NOType;
                sqlCtx.Params[_Schema.NumControl.fn_NoName].Value = item.NOName;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.NumControl.fn_Value)].Value = item.Value;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.NumControl.fn_CustomerID)].Value = item.Customer;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        // 2010-04-28 Liu Dong(eB1-4)         Modify 生成号: 不再将每个生成的号码都存NumControl表了.
        private void PersistUpdateNumControlByTypeAndPreStr(fons.NumControl item, string preStr)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl likecond = new _Schema.NumControl();
                        likecond.Value = string.Format(preStr, "%");
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = item.NOType;
                        cond.CustomerID = item.Customer;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), new List<string>() { _Schema.NumControl.fn_Value }, null, null, null, cond, likecond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_Value].Value = string.Format(preStr, "%");
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = item.NOType;
                sqlCtx.Params[_Schema.NumControl.fn_CustomerID].Value = item.Customer;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.NumControl.fn_Value)].Value = item.Value;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateNumControlByID(fons.NumControl item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.ID = item.ID;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), new List<string>() { _Schema.NumControl.fn_Value }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.NumControl.fn_Value)].Value = item.Value;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateNumControlByTypeAndNameAndCust(fons.NumControl item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = item.NOType;
                        cond.NoName = item.NOName;
                        cond.CustomerID = item.Customer;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), new List<string>() { _Schema.NumControl.fn_Value }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = item.NOType;
                sqlCtx.Params[_Schema.NumControl.fn_NoName].Value = item.NOName;
                sqlCtx.Params[_Schema.NumControl.fn_CustomerID].Value = item.Customer;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.NumControl.fn_Value)].Value = item.Value;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateNumControlByTypeAndPreStrWithOutByCustomer(fons.NumControl item, string preStr)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.NumControl likecond = new _Schema.NumControl();
                        likecond.Value = string.Format(preStr, "%");
                        _Schema.NumControl cond = new _Schema.NumControl();
                        cond.NoType = item.NOType;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.NumControl), new List<string>() { _Schema.NumControl.fn_Value }, null, null, null, cond, likecond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.NumControl.fn_Value].Value = string.Format(preStr, "%");
                sqlCtx.Params[_Schema.NumControl.fn_NoType].Value = item.NOType;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.NumControl.fn_Value)].Value = item.Value;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Defered  .

        public void SaveMaxNumberDefered(IUnitOfWork uow, fons.NumControl item, bool insertOrUpdate, string preStr)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, insertOrUpdate, preStr);
        }

        public void SaveMaxNumberWithOutByCustomerDefered(IUnitOfWork uow, fons.NumControl item, bool insertOrUpdate, string preStr)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, insertOrUpdate, preStr);
        }

        public void SetMACRangeStatusDefered(IUnitOfWork uow, int macRangeId, string rangeStatus)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), macRangeId, rangeStatus);
        }

        public void SaveMaxMACDefered(IUnitOfWork uow, fons.NumControl item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertNumControlDefered(IUnitOfWork uow, fons.NumControl item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #region For Maintain

        public IList<MACRange> GetMACRangeList()
        {
            try
            {
                IList<MACRange> ret = new List<MACRange>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",",new string[]{_Schema.MACRange.fn_Code, _Schema.MACRange.fn_Cdt}));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MACRange item = new MACRange(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_ID]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Code]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Status]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Editor]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Cdt]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Udt]));
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

        public MACRange GetMACRange(int macRangeId)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                MACRange ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MACRange cond = new _Schema.MACRange();
                        cond.ID = macRangeId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.MACRange.fn_ID].Value = macRangeId;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MACRange(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_ID]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Code]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Status]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Editor]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Cdt]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Udt]));
                        ret.Tracker.Clear();
                    }
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}
                    SqlTransactionManager.Commit();

                return ret;
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

        // 2011-05-18 Liu Dong(eB1-4)         Modify ITC-1281-0025 
        private MACRange PeekIfMACRangeOverlapped_1(string begNo, string endNo, int exceptId)
        {
            SqlDataReader sqlR = null;
            try
            {
                MACRange ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MACRange betCond = new _Schema.MACRange();
                        betCond.BegNo = "BETWEEN begin AND end";
                        _Schema.MACRange neqCond = new _Schema.MACRange();
                        neqCond.ID = exceptId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), null, null, null, null, betCond, null, null, null, null, null, neqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE"); 
                    }
                }
                sqlCtx.Params[_Schema.Func.DecBeg(_Schema.MACRange.fn_BegNo)].Value = begNo;
                sqlCtx.Params[_Schema.Func.DecEnd(_Schema.MACRange.fn_BegNo)].Value = endNo;
                sqlCtx.Params[_Schema.MACRange.fn_ID].Value = exceptId;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = new MACRange(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_ID]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Code]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Status]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Editor]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Cdt]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Udt]));
                    ret.Tracker.Clear();
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

        private MACRange PeekIfMACRangeOverlapped_2(string begNo, string endNo, int exceptId)
        {
            SqlDataReader sqlR = null;
            try
            {
                MACRange ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MACRange betCond = new _Schema.MACRange();
                        betCond.EndNo = "BETWEEN begin AND end";
                        _Schema.MACRange neqCond = new _Schema.MACRange();
                        neqCond.ID = exceptId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), null, null, null, null, betCond, null, null, null, null, null, neqCond, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE"); 
                    }
                }
                sqlCtx.Params[_Schema.Func.DecBeg(_Schema.MACRange.fn_EndNo)].Value = begNo;
                sqlCtx.Params[_Schema.Func.DecEnd(_Schema.MACRange.fn_EndNo)].Value = endNo;
                sqlCtx.Params[_Schema.MACRange.fn_ID].Value = exceptId;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = new MACRange(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_ID]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Code]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Status]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Editor]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Cdt]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Udt]));
                    ret.Tracker.Clear();
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

        private MACRange PeekIfMACRangeOverlapped_3(string begNo, string endNo, int exceptId)
        {
            SqlDataReader sqlR = null;
            try
            {
                MACRange ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MACRange smallerThanCond = new _Schema.MACRange();
                        smallerThanCond.BegNo = begNo;
                        _Schema.MACRange greaterThanCond = new _Schema.MACRange();
                        greaterThanCond.EndNo = endNo;
                        _Schema.MACRange neqCond = new _Schema.MACRange();
                        neqCond.ID = exceptId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), null, null, null, null, null, greaterThanCond, smallerThanCond, null, null, null, neqCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.Func.DecS(_Schema.MACRange.fn_BegNo)].Value = begNo;
                sqlCtx.Params[_Schema.Func.DecG(_Schema.MACRange.fn_EndNo)].Value = endNo;
                sqlCtx.Params[_Schema.MACRange.fn_ID].Value = exceptId;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = new MACRange(GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_ID]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Code]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Status]),
                                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Editor]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Cdt]),
                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_Udt]));
                    ret.Tracker.Clear();
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

        public void AddMACRange(MACRange item)
        {
            try
            {
                SqlTransactionManager.Begin();

                bool isThrow = false;
                MACRange temp = null;
                temp = PeekIfMACRangeOverlapped_1(item.BegNo, item.EndNo, -1);
                if (temp != null)
                {
                    isThrow = true;
                }
                else
                {
                    temp = PeekIfMACRangeOverlapped_2(item.BegNo, item.EndNo, -1);
                    if (temp != null)
                    {
                        isThrow = true;
                    }
                    else
                    {
                        temp = PeekIfMACRangeOverlapped_3(item.BegNo, item.EndNo, -1);
                        if (temp != null)
                        {
                            isThrow = true;
                        }
                    }
                }
                if (isThrow)
                {
                    throw new FisException("MAC001", new string[] { temp.ID.ToString(), temp.BegNo, temp.EndNo });
                }
                AddMACRange_Inner(item);

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

        public void UpdateMACRange(MACRange item)
        {
            try
            {
                SqlTransactionManager.Begin();

                bool isThrow = false;
                MACRange temp = null;
                temp = PeekIfMACRangeOverlapped_1(item.BegNo, item.EndNo, item.ID);
                if (temp != null)
                {
                    isThrow = true;
                }
                else
                {
                    temp = PeekIfMACRangeOverlapped_2(item.BegNo, item.EndNo, item.ID);
                    if (temp != null)
                    {
                        isThrow = true;
                    }
                    else
                    {
                        temp = PeekIfMACRangeOverlapped_3(item.BegNo, item.EndNo, item.ID);
                        if (temp != null)
                        {
                            isThrow = true;
                        }
                    }
                }
                if (isThrow)
                {
                    throw new FisException("MAC001", new string[] { temp.ID.ToString(), temp.BegNo, temp.EndNo });
                }
                UpdateMACRange_Inner(item);

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

        private void AddMACRange_Inner(MACRange item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.MACRange.fn_BegNo].Value = item.BegNo;
                sqlCtx.Params[_Schema.MACRange.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.MACRange.fn_Code].Value = item.Code;
                sqlCtx.Params[_Schema.MACRange.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.MACRange.fn_EndNo].Value = item.EndNo;
                //sqlCtx.Params[_Schema.MACRange.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.MACRange.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.MACRange.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateMACRange_Inner(MACRange item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.MACRange.fn_BegNo].Value = item.BegNo;
                sqlCtx.Params[_Schema.MACRange.fn_Code].Value = item.Code;
                sqlCtx.Params[_Schema.MACRange.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.MACRange.fn_EndNo].Value = item.EndNo;
                sqlCtx.Params[_Schema.MACRange.fn_ID].Value = item.ID;
                sqlCtx.Params[_Schema.MACRange.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.MACRange.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }
        // 2011-05-18 Liu Dong(eB1-4)         Modify ITC-1281-0025 

        public void DeleteMACRange(int macRangeId)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange));
                    }
                }
                sqlCtx.Params[_Schema.MACRange.fn_ID].Value = macRangeId;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long GetMACRangeTotalByCode(string code)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                long ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MACRange cond = new _Schema.MACRange();
                        cond.Code = code;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), null, new List<string>(){_Schema.MACRange.fn_BegNo, _Schema.MACRange.fn_EndNo}, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=CodeIndex,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.MACRange.fn_Code].Value = code;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                    while (sqlR != null && sqlR.Read())
                    {
                        string beg = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]);
                        string end = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]);
                        ret = ret + (Convert.ToInt64(_macr.CalculateDifference(beg, end)) + 1);
                    }
                }
                finally
                {
                    if (sqlR != null)
                    {
                        sqlR.Close();
                    }
                }
                //}
                    SqlTransactionManager.Commit();

                return ret;
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

        public int GetMACRangeTotalUsedByCode(string code)
        {
            try
            {
                SqlTransactionManager.Begin();

                int countClosed = GetMACRangeTotalByCodeAndStatus(code,"C");

                int countActiveUsed = 0;

                string currNum = string.Empty;
                MACRange actvRng = GetMaxMACRange(code, "A", out currNum);
                if (actvRng != null && !string.IsNullOrEmpty(currNum))
                {
                    countActiveUsed = Convert.ToInt32(_macr.CalculateDifference(actvRng.BegNo, currNum)) + 1;
                }

                SqlTransactionManager.Commit();

                return countClosed + countActiveUsed;
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

        private int GetMACRangeTotalByCodeAndStatus(string code, string status)
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
                        _Schema.MACRange cond = new _Schema.MACRange();
                        cond.Code = code;
                        cond.Status = status;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MACRange), null, new List<string>() { _Schema.MACRange.fn_BegNo, _Schema.MACRange.fn_EndNo }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=CodeStatusIndex,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.MACRange.fn_Code].Value = code;
                sqlCtx.Params[_Schema.MACRange.fn_Status].Value = status;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                    sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                    while (sqlR != null && sqlR.Read())
                    {
                        string beg = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_BegNo]);
                        string end = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MACRange.fn_EndNo]);
                        ret = ret + (Convert.ToInt32(_macr.CalculateDifference(beg, end)) + 1);
                    }
                //}
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

        public DataTable GetHDCPQuery()
        {
            //SELECT COUNT(*) FROM HDCPKey WHERE Status = 'R'
            try
            {
                try
                {
                    DataTable ret = null;

                    _Schema.SQLContext sqlCtx = null;
                    lock (MethodBase.GetCurrentMethod())
                    {
                        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                        {
                            _Schema.HDCPKey cond = new _Schema.HDCPKey();
                            cond.Status = "R";
                            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.HDCPKey), "COUNT", new List<string>() { _Schema.HDCPKey.fn_hdcpKey }, cond, null, null, null, null, null, null, null);
                            sqlCtx.Params[_Schema.HDCPKey.fn_Status].Value = cond.Status; 
                        }
                    }
                    ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        #region Defered

        public void AddMACRangeDefered(IUnitOfWork uow, MACRange item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateMACRangeDefered(IUnitOfWork uow, MACRange item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteMACRangeDefered(IUnitOfWork uow, int macRangeId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), macRangeId);
        }

        public void SaveMaxAssetNumberDefered(IUnitOfWork uow, IMES.FisObject.Common.NumControl.NumControl item, bool insertOrUpdate)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, insertOrUpdate);
        }

        #endregion

        #endregion
    }
}
