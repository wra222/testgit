// 2010-06-04 Liu Dong(eB1-4)         Modify 取Model list时需要过滤掉Hold的Model

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Util;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fmns = IMES.FisObject.Common.Model;
using dmns = IMES.DataModel;
using fons = IMES.FisObject.Common.MO;

namespace IMES.Infrastructure.Repository.PCA
{
    public class MBMORepository : BaseRepository<IMBMO>, IMBMORepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<IMBModel>

        protected override void PersistNewItem(IMBMO item)
        {
            StateTracker tracker = (item as MBMO).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    PersistInsertMBMO(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(IMBMO item)
        {
            StateTracker tracker = (item as MBMO).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    PersistUpdateMBMO(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(IMBMO item)
        {
            StateTracker tracker = (item as MBMO).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    PersistDeleteMBMO(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<MBMO>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IMBMO Find(object key)
        {
            try
            {
                IMBMO ret = null;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        SMTMO cond = new SMTMO();
                        cond.SmtMo = (string)key;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO), cond, null, null);
                    }
                }
                sqlCtx.Params[SMTMO.fn_SmtMo].Value = (string)key;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MBMO(
                        GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_SmtMo]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_PCBFamily]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_IECPartNo]),
                        GetValue_Int32(sqlR, sqlCtx.Indexes[SMTMO.fn_Qty]),
                        GetValue_Int32(sqlR, sqlCtx.Indexes[SMTMO.fn_PrintQty]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Remark]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Status]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Editor]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[SMTMO.fn_Cdt]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[SMTMO.fn_Udt]));
                        ((MBMO)ret).Tracker.Clear();
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
        public override IList<IMBMO> FindAll()
        {
            try
            {
                IList<IMBMO> ret = new List<IMBMO>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO));
                    }
                }
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMBMO item = new MBMO(
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_SmtMo]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_PCBFamily]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_IECPartNo]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[SMTMO.fn_Qty]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[SMTMO.fn_PrintQty]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Remark]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Status]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Editor]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[SMTMO.fn_Cdt]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[SMTMO.fn_Udt]));
                            ((MBMO)item).Tracker.Clear();
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
        public override void Add(IMBMO item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(IMBMO item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="work"></param>
        public override void Update(IMBMO item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IMBMORepository

        /// <summary>
        /// 根据主板型号，取得未完成的制造订单列表。
        /// </summary>
        /// <param name="model">主板型号</param>
        /// <returns>未完成制造订单对象集合</returns>
        public IList<IMBMO> GetUnfinishedMOByModel(string model)
        {
            //是否看 PrintedQty < Qty ? , 还是MBSn尚未生成满?
            try
            {
                IList<IMBMO> ret = new List<IMBMO>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(SMTMO);
                        SMTMO cond = new SMTMO();
                        cond.IECPartNo = model;
                        tf1.equalcond = cond;

                        _Schema.TableConnectionCollection tcc = new _Schema.TableConnectionCollection(new TableConnectionItem[] { new TableConnectionItem(tf1, SMTMO.fn_PrintQty, tf1, SMTMO.fn_Qty, "{0}<{1}") });

                        tblAndFldsesArray = new TableAndFields[] { tf1 };

                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tcc);
                    }
                }
                tf1 = tblAndFldsesArray[0];

                sqlCtx.Params[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)].Value = model;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMBMO item = new MBMO(
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_SmtMo)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_PCBFamily)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_IECPartNo)]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_Qty)]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_PrintQty)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_Remark)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_Status)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_Editor)]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias,SMTMO.fn_Cdt)]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Udt)]));
                            ((MBMO)item).Tracker.Clear();
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
        /// 取得所有未完成的制造订单
        /// </summary>
        /// <returns>未完成制造订单对象集合</returns>
        public IList<IMBMO> GetUnfinishedMO()
        {
            //是否看 PrintedQty < Qty ? , 还是MBSn尚未生成满?
            try
            {
                IList<IMBMO> ret = new List<IMBMO>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(SMTMO);

                        _Schema.TableConnectionCollection tcc = new _Schema.TableConnectionCollection(new TableConnectionItem[] { new TableConnectionItem(tf1, SMTMO.fn_PrintQty, tf1, SMTMO.fn_Qty, "{0}<{1}") });

                        tblAndFldsesArray = new TableAndFields[] { tf1 };

                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tcc);
                    }
                }
                tf1 = tblAndFldsesArray[0];

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMBMO item = new MBMO(
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_SmtMo)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_PCBFamily)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Qty)]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_PrintQty)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Remark)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Status)]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Editor)]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Cdt)]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Udt)]));
                            ((MBMO)item).Tracker.Clear();
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
        /// 根据量产/试产获取最大MO
        /// </summary>
        /// <param name="isExperiment">是否为试产</param>
        /// <returns>最大MO</returns>
        //public string GetMaxMO(bool isExperiment)
        //{
        //    //try
        //    //{
        //    //    string ret = string.Empty;

        //    //    SMTMO likecond = new SMTMO();
        //    //    likecond.SmtMo = preStr + "%";
        //    //int i = System.Reflection.MethodBase.GetCurrentMethod().MetadataToken;
        //    //Console.WriteLine(i);
        //    //    SQLContext sqlCtx = Func.GetConditionedFuncSelect(typeof(SMTMO), "MAX", new List<string>() { SMTMO.fn_SmtMo }, null, likecond);

        //    //    using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //    //    {
        //    //        if (sqlR != null && sqlR.Read())
        //    //        {
        //    //            ret = GetValue_Str(sqlR, sqlCtx.Indexes["MAX"]);
        //    //        }
        //    //    }
        //    //    return ret;
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    throw e;
        //    //}
        //}

        /// <summary>
        /// 根据前缀获取最大MO
        /// </summary>
        /// <param name="preStr">前缀</param>
        /// <returns>最大MO<</returns>
        public string GetMaxMO(string preStr)
        {
            try
            {
                string ret = string.Empty;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        SMTMO likecond = new SMTMO();
                        likecond.SmtMo = preStr + "%";

                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO), "MAX", new List<string>() { SMTMO.fn_SmtMo }, null, likecond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[SMTMO.fn_SmtMo].Value = preStr + "%";
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes["MAX"]);
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
        /// 更新MO最大号(NumControl)
        /// </summary>
        /// <param name="isExperiment">是否为试产</param>
        /// <param name="maxMO">MO最大号</param>
        public void SetMaxMO(bool isExperiment, IMBMO maxMO)
        {
            try
            {
                PersistInsertMBMO(maxMO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当天产生的未生成所有MBSn的MBMO
        /// </summary>
        /// <returns></returns>
        public IList<IMBMO> GetUnfinishedMOToday()
        {
            //是否看 PrintedQty < Qty ? , 还是MBSn尚未生成满?
            try
            {
                IList<IMBMO> ret = new List<IMBMO>();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        SMTMO greaterCond = new SMTMO();
                        greaterCond.Qty = 100;

                        SMTMO greaterOrEqualCond = new SMTMO();
                        greaterOrEqualCond.Cdt = dt;

                        SMTMO smallerCond = new SMTMO();
                        smallerCond.Cdt = dt;

                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO), null, null, null, null, null, greaterCond, smallerCond, greaterOrEqualCond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("@" + Func.DecG(SMTMO.fn_Qty), "({0})");//为拼接子查询做准备.
                        sqlCtx.Params.Remove(Func.DecG(SMTMO.fn_Qty));

                        SQLContext sqlCtx_sub = ComposeSubSQLForGetUnfinishedMOToday();

                        sqlCtx_sub.Sentence = sqlCtx_sub.Sentence.Replace("@" + PCB.fn_SMTMOID, Func.DecAliasInner(typeof(SMTMO).Name, SMTMO.fn_SmtMo));
                        sqlCtx_sub.Params.Remove(PCB.fn_SMTMOID);

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, sqlCtx_sub.Sentence);
                    }
                }

                sqlCtx.Params[Func.DecGE(SMTMO.fn_Cdt)].Value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
                sqlCtx.Params[Func.DecS(SMTMO.fn_Cdt)].Value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0).AddDays(1);

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMBMO item = new MBMO(
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_SmtMo]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_PCBFamily]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_IECPartNo]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[SMTMO.fn_Qty]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[SMTMO.fn_PrintQty]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Remark]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Status]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_Editor]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[SMTMO.fn_Cdt]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[SMTMO.fn_Udt]));
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

        private SQLContext ComposeSubSQLForGetUnfinishedMOToday()
        {
            SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    PCB cond = new PCB();
                    cond.SMTMOID = "SMTMOID";
                    sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), "COUNT", new List<string>() { PCB.fn_PCBNo }, cond, null, null, null, null, null, null, null);
                }
            }
            return sqlCtx;
        }

        /// <summary>
        /// 更新指定mo的printedQty, 在其上加指定的值, 直接写db
        /// </summary>
        /// <param name="mo">mo</param>
        /// <param name="prtQtyInc">需要加的數量</param>
        public void UpdateSMTMOPrtQtyForInc(string mo, int prtQtyInc)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        SMTMO cond = new SMTMO();
                        cond.SmtMo = mo;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO), null, null, new List<string>() { SMTMO.fn_PrintQty }, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[Func.DecSV(SMTMO.fn_SmtMo)].Value = mo;
                sqlCtx.Params[Func.DecInc(SMTMO.fn_PrintQty)].Value = prtQtyInc;
                sqlCtx.Params[Func.DecSV(SMTMO.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SMTMOInfo> GetSMTMOList(string _111LevelId)
        {
            try
            {
                IList<SMTMOInfo> ret = new List<SMTMOInfo>();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(SMTMO);
                        SMTMO eqCond = new SMTMO();
                        eqCond.IECPartNo = _111LevelId;
                        tf1.equalcond = eqCond;
                        tf1.ToGetFieldNames.Add(SMTMO.fn_SmtMo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_IECPartNo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Qty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_PrintQty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Remark);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Cdt);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        tf2.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        tf3.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoType = "MDL";
                        tf3.equalcond = equalCond;
                        tf3.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, SMTMO.fn_PrintQty, tf1, SMTMO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);
                        TableConnectionItem tc3 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc3);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderByDesc, Func.DecAliasInner(tf1.alias, SMTMO.fn_Cdt));

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = equalCond.InfoType;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)].Value = _111LevelId;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            SMTMOInfo item = new SMTMOInfo();
                            item._111LevelId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)]);//A.IECPartNo as IECPartNo,
                            item.cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Cdt)]);//A.Cdt as Cdt
                            item.description = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)]);//D.InfoValue as Description, 
                            item.id = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_SmtMo)]);//A.SMTMO as MO, 
                            item.friendlyName = item.id;
                            item.MB_CODEId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);//B.InfoValue as MBCODE,
                            item.remark = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Remark)]);//A.Remark as Remark,
                            item.totalMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Qty)]);//A.Qty as Qty, 
                            item.printedMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_PrintQty)]);//A.PrintQty as PrintQty,  
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

        public SMTMOInfo GetSMTMOInfo(string SMTMOId)
        {
            try
            {
                SMTMOInfo ret = new SMTMOInfo();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(SMTMO);
                        SMTMO eqCond = new SMTMO();
                        eqCond.SmtMo = SMTMOId;
                        tf1.equalcond = eqCond;
                        tf1.ToGetFieldNames.Add(SMTMO.fn_SmtMo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_IECPartNo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Qty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_PrintQty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Remark);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Cdt);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        tf2.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        tf3.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoType = "MDL";
                        tf3.equalcond = equalCond;
                        tf3.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, SMTMO.fn_PrintQty, tf1, SMTMO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);
                        TableConnectionItem tc3 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc3);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderByDesc, Func.DecAliasInner(tf1.alias, SMTMO.fn_Cdt));

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = equalCond.InfoType;
                    }
                }

                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[Func.DecAlias(tf1.alias, SMTMO.fn_SmtMo)].Value = SMTMOId;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = new SMTMOInfo();
                            ret._111LevelId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)]);//A.IECPartNo as IECPartNo,
                            ret.cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Cdt)]);//A.Cdt as Cdt
                            ret.description = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)]);//D.InfoValue as Description, 
                            ret.id = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_SmtMo)]);//A.SMTMO as MO, 
                            ret.friendlyName = ret.id;
                            ret.MB_CODEId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);//B.InfoValue as MBCODE,
                            ret.remark = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Remark)]);//A.Remark as Remark,
                            ret.totalMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Qty)]);//A.Qty as Qty, 
                            ret.printedMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_PrintQty)]);//A.PrintQty as PrintQty,  
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

        public IList<SMTMOInfo> GetSMTMOInfos()
        {
            //select A.SMTMO as MO, B.InfoValue as MBCODE,D.InfoValue as Description, A.IECPartNo as IECPartNo, A.Qty as Qty, A.PrintQty as PrintQty, A.Remark as Remark,A.Cdt as Cdt
            //from SMTMO as A, PartInfo as B,PartInfo as D
            //where A.PrintQty < A.Qty 
            //and convert(varchar(10),A.Cdt,120) = convert(varchar(10),getdate(),120)  
            //and A.IECPartNo= B.PartNo 
            //And B.InfoType IN ('MB','VB','SB')
            //and A.IECPartNo= D.PartNo 
            //and D.InfoType='MDL'
            //ORDER BY A.Cdt DESC

            try
            {
                IList<SMTMOInfo> ret = new List<SMTMOInfo>();

                DateTime dt = SqlHelper.GetDateTime();


                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(SMTMO);
                        SMTMO greaterOrEqualCond = new SMTMO();
                        greaterOrEqualCond.Cdt = dt;
                        tf1.greaterOrEqualcond = greaterOrEqualCond;
                        SMTMO smallerCond = new SMTMO();
                        smallerCond.Cdt = dt;
                        tf1.smallercond = smallerCond;
                        tf1.ToGetFieldNames.Add(SMTMO.fn_SmtMo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_IECPartNo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Qty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_PrintQty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Remark);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Cdt);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        tf2.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        tf3.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoType = "MDL";
                        tf3.equalcond = equalCond;
                        tf3.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, SMTMO.fn_PrintQty, tf1, SMTMO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);
                        TableConnectionItem tc3 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc3);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderByDesc, Func.DecAliasInner(tf1.alias, SMTMO.fn_Cdt));

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = equalCond.InfoType;
                    }
                }

                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[Func.DecAlias(tf1.alias, Func.DecGE(SMTMO.fn_Cdt))].Value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
                sqlCtx.Params[Func.DecAlias(tf1.alias, Func.DecS(SMTMO.fn_Cdt))].Value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0).AddDays(1);

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            SMTMOInfo item = new SMTMOInfo();
                            item._111LevelId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)]);//A.IECPartNo as IECPartNo,
                            item.cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Cdt)]);//A.Cdt as Cdt
                            item.description = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)]);//D.InfoValue as Description, 
                            item.id = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_SmtMo)]);//A.SMTMO as MO, 
                            item.friendlyName = item.id;
                            item.MB_CODEId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);//B.InfoValue as MBCODE,
                            item.remark = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Remark)]);//A.Remark as Remark,
                            item.totalMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Qty)]);//A.Qty as Qty, 
                            item.printedMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_PrintQty)]);//A.PrintQty as PrintQty,  
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

        public IList<string> GetModelsByFamilyWithMO(string family)
        {
            try
            {
                //Select DISTINCT Model.Model from Model,MO 
                //Where Model.Family=@family and MO.Model=Model.Model 
                //and MO.Status='H' and MO.Qty>MO.Print_Qty and  MO.SAPStatus=''AND Model.Status=1

                IList<string> ret = new List<string>();

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
                        // 2010-06-04 Liu Dong(eB1-4)         Modify 取Model list时需要过滤掉Hold的Model
                        equalCond1.Status = 1;
                        tf1.equalcond = equalCond1;
                        tf1.ToGetFieldNames.Add(_Schema.Model.fn_model);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(MO);
                        MO equalCond2 = new MO();
                        equalCond2.Status = "H";
                        equalCond2.SAPStatus = "";
                        tf2.equalcond = equalCond2;
                        tf2.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf2, MO.fn_Print_Qty, tf2, MO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, _Schema.Model.fn_model, tf2, MO.fn_Model);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf1.alias, _Schema.Model.fn_model));

                        sqlCtx.Params[Func.DecAlias(tf2.alias, MO.fn_Status)].Value = equalCond2.Status;

                        sqlCtx.Params[Func.DecAlias(tf2.alias, MO.fn_SAPStatus)].Value = equalCond2.SAPStatus;

                        sqlCtx.Params[Func.DecAlias(tf1.alias, _Schema.Model.fn_Status)].Value = equalCond1.Status;
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
                            string res = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, _Schema.Model.fn_model)]);
                            ret.Add(res);
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

        public IList<string> GetModelsByFamilyWithMORecentOneMonth(string family)
        {
            try
            {
                //Select DISTINCT Model.Model from Model,MO 
                //Where Model.Family=@family and MO.Model=Model.Model 
                //and MO.Status='H' and MO.Qty>MO.Print_Qty and  MO.SAPStatus='' AND MO.Udt>dateadd(day,-30,getdate()) AND Model.Status=1

                IList<string> ret = new List<string>();

                DateTime dt = SqlHelper.GetDateTime();

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
                        // 2010-06-04 Liu Dong(eB1-4)         Modify 取Model list时需要过滤掉Hold的Model
                        equalCond1.Status = 1;
                        tf1.equalcond = equalCond1;
                        tf1.ToGetFieldNames.Add(_Schema.Model.fn_model);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(MO);
                        MO equalCond2 = new MO();
                        equalCond2.Status = "H";
                        equalCond2.SAPStatus = "";
                        MO greaterCond = new MO();
                        greaterCond.Udt = dt.AddDays(-30);
                        tf2.equalcond = equalCond2;
                        tf2.greatercond = greaterCond;
                        tf2.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf2, MO.fn_Print_Qty, tf2, MO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, _Schema.Model.fn_model, tf2, MO.fn_Model);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf1.alias, _Schema.Model.fn_model));

                        sqlCtx.Params[Func.DecAlias(tf2.alias, MO.fn_Status)].Value = equalCond2.Status;

                        sqlCtx.Params[Func.DecAlias(tf2.alias, MO.fn_SAPStatus)].Value = equalCond2.SAPStatus;

                        sqlCtx.Params[Func.DecAlias(tf1.alias, _Schema.Model.fn_Status)].Value = equalCond1.Status;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[Func.DecAlias(tf1.alias, _Schema.Model.fn_Family)].Value = family;
                sqlCtx.Params[Func.DecAlias(tf2.alias, Func.DecG(MO.fn_Udt))].Value = dt.AddDays(-30);

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string res = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, _Schema.Model.fn_model)]);
                            ret.Add(res);
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

        public IList<dmns.FamilyInfo> GetFamilysByCustomerWithMORecentOneMonth(string customer)
        {
            try
            {
                IList<dmns.FamilyInfo> ret = new List<dmns.FamilyInfo>();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(_Schema.Model);
                        //Model equalCond1 = new Model();
                        //equalCond1.Family = family;
                        //tf1.equalcond = equalCond1;
                        //tf1.ToGetFieldNames.Add(Model.fn_model);
                        tf1.ToGetFieldNames = null;

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(MO);
                        MO equalCond2 = new MO();
                        equalCond2.Status = "H";
                        equalCond2.SAPStatus = "";
                        MO greaterCond = new MO();
                        greaterCond.Udt = dt.AddDays(-30);
                        tf2.equalcond = equalCond2;
                        tf2.greatercond = greaterCond;
                        tf2.ToGetFieldNames = null;

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.Family);
                        _Schema.Family equalCond3 = new _Schema.Family();
                        equalCond3.CustomerID = customer;
                        tf3.equalcond = equalCond3;
                        tf3.ToGetFieldNames.Add(_Schema.Family.fn_family);
                        tf3.ToGetFieldNames.Add(_Schema.Family.fn_Descr);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf2, MO.fn_Print_Qty, tf2, MO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, _Schema.Model.fn_model, tf2, MO.fn_Model);
                        tblCnntIs.Add(tc2);
                        TableConnectionItem tc3 = new TableConnectionItem(tf3, _Schema.Family.fn_family, tf1, _Schema.Model.fn_Family);
                        tblCnntIs.Add(tc3);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf3.alias, _Schema.Family.fn_family));

                        sqlCtx.Params[Func.DecAlias(tf2.alias, MO.fn_Status)].Value = equalCond2.Status;

                        sqlCtx.Params[Func.DecAlias(tf2.alias, MO.fn_SAPStatus)].Value = equalCond2.SAPStatus;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                //sqlCtx.Params[Func.DecAlias(tf1.alias, Model.fn_Family)].Value = family;
                sqlCtx.Params[Func.DecAlias(tf2.alias, Func.DecG(MO.fn_Udt))].Value = dt.AddDays(-30);
                sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.Family.fn_CustomerID)].Value = customer;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            dmns.FamilyInfo res = new dmns.FamilyInfo();
                            res.id = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.Family.fn_family)]);
                            res.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.Family.fn_Descr)]);
                            ret.Add(res);
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

        public IList<string> GetMOsRecentOneMonthByModel(string model)
        {
            //Select MO from MO 
            //Where Model= @model 
            //and Qty-Print_Qty > 0 and  Status='H' and Udt>dateadd(day,-30,getdate()) 
            //and MO.SAPStatus='' order by MO
            try
            {
                IList<string> ret = new List<string>();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        MO equalCond = new MO();
                        equalCond.Model = model;
                        equalCond.Status = "H";
                        equalCond.SAPStatus = "";

                        MO greaterCond = new MO();
                        greaterCond.Udt = dt.AddDays(-30);

                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(MO), null, new List<string>() { MO.fn_Mo }, equalCond, null, null, greaterCond, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0}-{1}>0 ", MO.fn_Qty, MO.fn_Print_Qty) + string.Format(Func.OrderBy, MO.fn_Mo);

                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);

                        sqlCtx.Params[MO.fn_Status].Value = equalCond.Status;
                        sqlCtx.Params[MO.fn_SAPStatus].Value = equalCond.SAPStatus;
                    }
                }

                sqlCtx.Params[MO.fn_Model].Value = model;
                sqlCtx.Params[Func.DecG(MO.fn_Udt)].Value = dt.AddDays(-30);

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string res = GetValue_Str(sqlR, sqlCtx.Indexes[MO.fn_Mo]);
                            ret.Add(res);
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

        public IList<string> GetMOsRecentOneMonthByModelRegardlessUdt(string model)
        {
            try
            {
                IList<string> ret = new List<string>();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        MO equalCond = new MO();
                        equalCond.Model = model;
                        equalCond.Status = "H";
                        equalCond.SAPStatus = "";

                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(MO), null, new List<string>() { MO.fn_Mo }, equalCond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0}-{1}>0 ", MO.fn_Qty, MO.fn_Print_Qty) + string.Format(Func.OrderBy, MO.fn_Mo);

                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);

                        sqlCtx.Params[MO.fn_Status].Value = equalCond.Status;
                        sqlCtx.Params[MO.fn_SAPStatus].Value = equalCond.SAPStatus;
                    }
                }
                sqlCtx.Params[MO.fn_Model].Value = model;
 
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string res = GetValue_Str(sqlR, sqlCtx.Indexes[MO.fn_Mo]);
                            ret.Add(res);
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
        /// 根据MoList删除MO,返回其中不符合删除条件的MO列表
        /// 实现逻辑：
        ///         1.delete from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 and PrintQty = 0
        ///         2.select distinct SMTMO from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 
        /// </summary>
        /// <param name="moList"></param> 
        /// <returns></returns>
        public IList<string> DeleteSMTMO(IList<string> moList)
        {
            try
            {
               
                DeleteSMTMOByList(moList);
                //Vincent 2014-01-04 change logical update SMTMO status ='C'
                updateSMTMOClosedByList(moList);
               
                //return FindSMTMOByList(moList);
                return null;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SMTMOInfo> GetSMTMOListFor002(string _111LevelId)
        {
            //SELECT SMTMO FROM IMES_PCA..SMTMO WHERE IECPartNo = @111LevelPartNo
            //AND PrintQty < Qty
            //ORDER BY SMTMO
            try
            {
                IList<SMTMOInfo> ret = new List<SMTMOInfo>();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(SMTMO);
                        SMTMO eqCond = new SMTMO();
                        eqCond.IECPartNo = _111LevelId;
                        //Vincent for CQ add condition 
                        eqCond.Status = "H";
                        tf1.equalcond = eqCond;
                        tf1.ToGetFieldNames.Add(SMTMO.fn_SmtMo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_IECPartNo);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Qty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_PrintQty);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Remark);
                        tf1.ToGetFieldNames.Add(SMTMO.fn_Cdt);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        tf2.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        tf3.subDBCalalog = SqlHelper.DB_BOM;
                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoType = "MDL";
                        tf3.equalcond = equalCond;
                        tf3.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, SMTMO.fn_PrintQty, tf1, SMTMO.fn_Qty, "{0}<{1}");
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);
                        TableConnectionItem tc3 = new TableConnectionItem(tf1, SMTMO.fn_IECPartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc3);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf1.alias, SMTMO.fn_SmtMo));

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = equalCond.InfoType;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)].Value = _111LevelId;
                sqlCtx.Params[Func.DecAlias(tf1.alias, SMTMO.fn_Status)].Value = "H";

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            SMTMOInfo item = new SMTMOInfo();
                            item._111LevelId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_IECPartNo)]);//A.IECPartNo as IECPartNo,
                            item.cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Cdt)]);//A.Cdt as Cdt
                            item.description = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)]);//D.InfoValue as Description, 
                            item.id = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_SmtMo)]);//A.SMTMO as MO, 
                            item.friendlyName = item.id;
                            item.MB_CODEId = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);//B.InfoValue as MBCODE,
                            item.remark = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Remark)]);//A.Remark as Remark,
                            item.totalMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_Qty)]);//A.Qty as Qty, 
                            item.printedMBQty = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, SMTMO.fn_PrintQty)]);//A.PrintQty as PrintQty,  
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

        public IList<string> GetMOsRecentOneMonthByModelConsiderStartDate(string model)
        {
            //Select MO from MO 
            //Where Model= @model 
            //and Qty-Print_Qty > 0 and  Status='H' and Udt>dateadd(day,-30,getdate()) 
            //and MO.SAPStatus='' and StartDate>dateadd(day,-3,getdate()) order by MO
            try
            {
                IList<string> ret = new List<string>();

                DateTime dt = SqlHelper.GetDateTime();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        MO equalCond = new MO();
                        equalCond.Model = model;
                        equalCond.Status = "H";
                        equalCond.SAPStatus = "";

                        MO greaterCond = new MO();
                        greaterCond.Udt = dt.AddDays(-30);
                        greaterCond.StartDate = dt.AddDays(-3);

                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(MO), null, new List<string>() { MO.fn_Mo }, equalCond, null, null, greaterCond, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0}-{1}>0 ", MO.fn_Qty, MO.fn_Print_Qty) + string.Format(Func.OrderBy, MO.fn_Mo);

                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.MO.fn_Mo);

                        sqlCtx.Params[MO.fn_Status].Value = equalCond.Status;
                        sqlCtx.Params[MO.fn_SAPStatus].Value = equalCond.SAPStatus;
                    }
                }

                sqlCtx.Params[MO.fn_Model].Value = model;
                sqlCtx.Params[Func.DecG(MO.fn_Udt)].Value = dt.AddDays(-30);
                sqlCtx.Params[Func.DecG(MO.fn_StartDate)].Value = dt.AddDays(-3);

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string res = GetValue_Str(sqlR, sqlCtx.Indexes[MO.fn_Mo]);
                            ret.Add(res);
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

        public IList<string> GetSmtMoListByPno(string partNo)
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
                        Smtmo cond = new Smtmo();
                        cond.iecpartno = partNo;
                        //Vincent 2014-01-04 : add condition
                        cond.status = "H";
                        Smtmo cond2 = new Smtmo();
                        cond2.printQty = 0;
                        sqlCtx = FuncNew.GetConditionedSelect<Smtmo>(tk, null, new string[] { Smtmo.fn_smtmo }, new ConditionCollection<Smtmo>(
                            new EqualCondition<Smtmo>(cond),
                            new AnyCondition<Smtmo>(cond2, string.Format("{0}<{1}", "{0}", Smtmo.fn_qty))), Smtmo.fn_smtmo);
                    }
                }
                sqlCtx.Param(Smtmo.fn_iecpartno).Value = partNo;
                sqlCtx.Param(Smtmo.fn_status).Value = "H";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(Smtmo.fn_smtmo));
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

        public IList<SmtmoInfo> GetSmtmoInfoList(SmtmoInfo condition)
        {
            try
            {
                IList<SmtmoInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Smtmo cond = FuncNew.SetColumnFromField<Smtmo, SmtmoInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<Smtmo>(null, null, new ConditionCollection<Smtmo>(new EqualCondition<Smtmo>(cond)), Smtmo.fn_qty, Smtmo.fn_printQty);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Smtmo, SmtmoInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Smtmo, SmtmoInfo, SmtmoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SmtmoInfo> GetSmtmoInfoListToday()
        {
            try
            {
                IList<SmtmoInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Smtmo cond = new Smtmo();
                        cond.cdt = DateTime.Now;
                       
                        Smtmo cond2 = new Smtmo();
                        cond2.printQty = 0;

                        //Vincent 2014-01-04 for CQ add condition
                        Smtmo cond1 = new Smtmo();   
                        cond1.status = "H";
                        sqlCtx = FuncNew.GetConditionedSelect<Smtmo>(null, null, new ConditionCollection<Smtmo>(
                            new EqualCondition<Smtmo>(cond, "CONVERT(CHAR(10),{0},111)", "CONVERT(CHAR(10),{0},111)"),
                            new AnyCondition<Smtmo>(cond2, string.Format("{0}<{1}", "{0}", Smtmo.fn_qty)), 
                            new EqualCondition<Smtmo>(cond1) ), 
                            Smtmo.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Smtmo.fn_cdt).Value = cmDt;
                sqlCtx.Param(Smtmo.fn_status).Value = "H";
                


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Smtmo, SmtmoInfo, SmtmoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
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
                        cond.status = 1;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Model>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.Mo>();
                        _Metas.Mo cond2 = new _Metas.Mo();
                        cond2.sapstatus = string.Empty;
                        cond2.status = "H";
                        tf2.Conditions.Add(new EqualCondition<_Metas.Mo>(cond2));

                        _Metas.Mo cond3 = new _Metas.Mo();
                        cond3.udt = DateTime.Now;
                        tf2.Conditions.Add(new GreaterCondition<_Metas.Mo>(cond3, "CONVERT(VARCHAR(8),{0},112)", "CONVERT(VARCHAR(8),DATEADD(DAY,-10,{0}),112)"));

                        _Metas.Mo cond4 = new _Metas.Mo();
                        cond4.startDate = DateTime.Now;
                        tf2.Conditions.Add(new GreaterCondition<_Metas.Mo>(cond4, "CONVERT(VARCHAR(8),{0},112)", "CONVERT(VARCHAR(8),DATEADD(DAY,-3,{0}),112)"));

                        _Metas.Mo cond5 = new _Metas.Mo();
                        cond5.qty = 88;
                        tf2.Conditions.Add(new AnySoloCondition<_Metas.Mo>(cond5,string.Format("{0}>{1}","{0}",_Metas.Mo.fn_print_Qty)));

                        tf2.AddRangeToGetFieldNames(_Metas.Mo.fn_model);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        _Metas.TableConnectionCollection tblCnnts = new _Metas.TableConnectionCollection(new _Metas.TableConnectionItem<_Metas.Model, _Metas.Mo>(tf1, _Metas.Model.fn_model, tf2, _Metas.Mo.fn_model));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t2." + _Metas.Mo.fn_model);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Model.fn_status)).Value = cond.status;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Mo.fn_sapstatus)).Value = cond2.sapstatus;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Mo.fn_status)).Value = cond2.status;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Model.fn_family)).Value = family;
                sqlCtx.Param(g.DecAlias(tf2.Alias, g.DecG(_Metas.Mo.fn_udt))).Value = cmDt;
                sqlCtx.Param(g.DecAlias(tf2.Alias, g.DecG(_Metas.Mo.fn_startDate))).Value = cmDt;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.Model.fn_model)));
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

        public void AddRctombmaintainInfo(RctombmaintainInfo item)
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
                        sqlCtx = FuncNew.GetCommonInsert<mtns::Rctombmaintain>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Rctombmaintain, RctombmaintainInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Rctombmaintain.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Rctombmaintain.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteRctombmaintainInfo(RctombmaintainInfo condition)
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
                Rctombmaintain cond = FuncNew.SetColumnFromField<Rctombmaintain, RctombmaintainInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Rctombmaintain>(new ConditionCollection<Rctombmaintain>(new EqualCondition<Rctombmaintain>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Rctombmaintain, RctombmaintainInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateRctombmaintainInfo(RctombmaintainInfo setValue, RctombmaintainInfo condition)
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
                Rctombmaintain cond = FuncNew.SetColumnFromField<Rctombmaintain, RctombmaintainInfo>(condition);
                Rctombmaintain setv = FuncNew.SetColumnFromField<Rctombmaintain, RctombmaintainInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Rctombmaintain>(new SetValueCollection<Rctombmaintain>(new CommonSetValue<Rctombmaintain>(setv)), new ConditionCollection<Rctombmaintain>(new EqualCondition<Rctombmaintain>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Rctombmaintain, RctombmaintainInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Rctombmaintain, RctombmaintainInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Rctombmaintain.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons.MO> GetMoModelInfoListFromMoModel(string family, string model, string mo, DateTime startDate, DateTime endDate, string status)
        {
            try
            {
                IList<fons.MO> ret = null;

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
                        cond.family = "%" + family + "%";
                        tf1.Conditions.Add(new LikeCondition<_Metas.Model>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.Mo>();
                        _Metas.Mo cond2 = new _Metas.Mo();
                        cond2.model = "%" + model + "%";
                        cond2.mo = "%" + mo + "%";
                        tf2.Conditions.Add(new LikeCondition<_Metas.Mo>(cond2));

                        _Metas.Mo cond21 = new _Metas.Mo();
                        cond21.status = "H";
                        tf2.Conditions.Add(new EqualCondition<_Metas.Mo>(cond21));

                        _Metas.Mo cond3 = new _Metas.Mo();
                        cond3.createDate = DateTime.Now;
                        tf2.Conditions.Add(new GreaterOrEqualCondition<_Metas.Mo>(cond3, "CONVERT(VARCHAR,{0},111)", "CONVERT(VARCHAR,{0},111)"));

                        _Metas.Mo cond4 = new _Metas.Mo();
                        cond4.createDate = DateTime.Now;
                        tf2.Conditions.Add(new SmallerOrEqualCondition<_Metas.Mo>(cond4, "CONVERT(VARCHAR,{0},111)", "CONVERT(VARCHAR,{0},111)"));

                        _Metas.Mo cond5 = new _Metas.Mo();
                        cond5.qty = 88;
                        tf2.Conditions.Add(new AnySoloCondition<_Metas.Mo>(cond5, string.Format("{0}>{1}", "{0}", _Metas.Mo.fn_print_Qty)));

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        _Metas.TableConnectionCollection tblCnnts = new _Metas.TableConnectionCollection(new _Metas.TableConnectionItem<_Metas.Model, _Metas.Mo>(tf1, _Metas.Model.fn_model, tf2, _Metas.Mo.fn_model));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t2." + _Metas.Mo.fn_mo);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Mo.fn_status)).Value = cond21.status;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Model.fn_family)).Value = "%" + family + "%";
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Mo.fn_model)).Value = "%" + model + "%";
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Mo.fn_mo)).Value = "%" + mo + "%";
                sqlCtx.Param(g.DecAlias(tf2.Alias, g.DecGE(_Metas.Mo.fn_createDate))).Value = startDate;
                sqlCtx.Param(g.DecAlias(tf2.Alias, g.DecSE(_Metas.Mo.fn_createDate))).Value = endDate;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Mo, fons.MO, fons.MO>(ret, sqlR, sqlCtx, tf2.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<RctombmaintainInfo> GetRctombmaintainInfoList(RctombmaintainInfo condition)
        {
            try
            {
                IList<RctombmaintainInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Rctombmaintain cond = mtns::FuncNew.SetColumnFromField<mtns::Rctombmaintain, RctombmaintainInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Rctombmaintain>(null, null, new mtns::ConditionCollection<mtns::Rctombmaintain>(new mtns::EqualCondition<mtns::Rctombmaintain>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Rctombmaintain, RctombmaintainInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Rctombmaintain, RctombmaintainInfo, RctombmaintainInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public IList<string> GetModelListWithMoExcel(string status, string family, string line)
        {
            try
            {
                IList<string> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Model>();
                        _Metas.Model cond = new _Metas.Model();
                        cond.family = family;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Model>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.Model.fn_model);

                        tf2 = new TableAndFields<_Metas.Mo>();
                        _Metas.Mo cond2 = new _Metas.Mo();
                        cond2.status = status;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Mo>(cond2));
                        _Metas.Mo cond21 = new _Metas.Mo();
                        cond21.qty = 88;
                        tf2.Conditions.Add(new AnySoloCondition<_Metas.Mo>(cond21, string.Format("{0}>{1}", "{0}", _Metas.Mo.fn_print_Qty)));
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<_Metas.Mo_Excel>();
                        _Metas.Mo_Excel cond3 = new _Metas.Mo_Excel();
                        cond3.line = line;
                        tf3.Conditions.Add(new EqualCondition<_Metas.Mo_Excel>(cond3, "LEFT({0},1)", "LEFT({0},1)"));
                        _Metas.Mo_Excel cond31 = new _Metas.Mo_Excel();
                        cond31.qty = 88;
                        tf3.Conditions.Add(new AnySoloCondition<_Metas.Mo_Excel>(cond31, string.Format("{0}>{1}", "{0}", _Metas.Mo_Excel.fn_printQty)));
                        tf3.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        _Metas.TableConnectionCollection tblCnnts = new _Metas.TableConnectionCollection(
                            new _Metas.TableConnectionItem<_Metas.Model, _Metas.Mo>(tf1, _Metas.Model.fn_model, tf2, _Metas.Mo.fn_model),
                            new _Metas.TableConnectionItem<_Metas.Mo_Excel, _Metas.Mo>(tf3, _Metas.Mo_Excel.fn_model, tf2, _Metas.Mo.fn_model));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t1." + _Metas.Model.fn_model);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Model.fn_family)).Value = family;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Mo.fn_status)).Value = status;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.Mo_Excel.fn_line)).Value = line;

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

        public void IncreasePrintQtyForMoExcel(int qty, string line, string model)
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
                        Mo_Excel cond = new Mo_Excel();
                        cond.line = line;

                        Mo_Excel cond2 = new Mo_Excel();
                        cond2.model = model;

                        Mo_Excel setv = new Mo_Excel();
                        setv.printQty = qty;

                        Mo_Excel setv2 = new Mo_Excel();
                        setv2.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Mo_Excel>(tk, new SetValueCollection<Mo_Excel>(new ForIncSetValue<Mo_Excel>(setv), new CommonSetValue<Mo_Excel>(setv2)), new ConditionCollection<Mo_Excel>(
                            new EqualCondition<Mo_Excel>(cond, "LEFT({0},1)", "LEFT({0},1)"),
                            new EqualCondition<Mo_Excel>(cond2)
                            ));
                    }
                }
                sqlCtx.Param(Mo_Excel.fn_line).Value = line;
                sqlCtx.Param(Mo_Excel.fn_model).Value = model;

                sqlCtx.Param(g.DecInc(Mo_Excel.fn_printQty)).Value = qty;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Mo_Excel.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Inners .
        private void PersistInsertMBMO(IMBMO item)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[SMTMO.fn_Cdt].Value = cmDt;
                sqlCtx.Params[SMTMO.fn_Editor].Value = item.Editor;
                sqlCtx.Params[SMTMO.fn_PCBFamily].Value = item.Family;
                sqlCtx.Params[SMTMO.fn_IECPartNo].Value = item.Model;
                sqlCtx.Params[SMTMO.fn_SmtMo].Value = item.MONo;
                sqlCtx.Params[SMTMO.fn_PrintQty].Value = item.PrintedQty;
                sqlCtx.Params[SMTMO.fn_Qty].Value = item.Qty;
                sqlCtx.Params[SMTMO.fn_Remark].Value = item.Remark;
                sqlCtx.Params[SMTMO.fn_Status].Value = item.Status;
                sqlCtx.Params[SMTMO.fn_Udt].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMBMO(IMBMO item)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[SMTMO.fn_Editor].Value = item.Editor;
                sqlCtx.Params[SMTMO.fn_PCBFamily].Value = item.Family;
                sqlCtx.Params[SMTMO.fn_IECPartNo].Value = item.Model;
                sqlCtx.Params[SMTMO.fn_SmtMo].Value = item.MONo;
                sqlCtx.Params[SMTMO.fn_PrintQty].Value = item.PrintedQty;
                sqlCtx.Params[SMTMO.fn_Qty].Value = item.Qty;
                sqlCtx.Params[SMTMO.fn_Remark].Value = item.Remark;
                sqlCtx.Params[SMTMO.fn_Status].Value = item.Status;
                sqlCtx.Params[SMTMO.fn_Udt].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMBMO(IMBMO item)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO));
                    }
                }
                sqlCtx.Params[SMTMO.fn_SmtMo].Value = item.MONo;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DeleteSMTMOByList(IList<string> moList)
        {
            try
            {
                //delete from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 and PrintQty = 0
      
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        SMTMO insetCond = new SMTMO();
                        insetCond.SmtMo = "INSET";
                        SMTMO eqCond = new SMTMO();
                        eqCond.PrintQty = 0;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.SMTMO), eqCond, null, insetCond);
                        sqlCtx.Params[SMTMO.fn_PrintQty].Value = eqCond.PrintQty;
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(SMTMO.fn_SmtMo), Func.ConvertInSet(moList));
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<string> FindSMTMOByList(IList<string> moList)
        {
            try
            {
                //select distinct SMTMO from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 

                IList<string> ret = new List<string>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        SMTMO insetCond = new SMTMO();
                        insetCond.SmtMo = "INSET";
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(SMTMO), null, null, insetCond);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(SMTMO.fn_SmtMo), Func.ConvertInSet(moList));

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(GetValue_Str(sqlR, sqlCtx.Indexes[SMTMO.fn_SmtMo]));
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

        public int GetMBCountByMOAndStatusStation(string smtMo)
        {
            //select count(*) from PCB a inner join PCBStatus b on 
            //a.PCBNo = b.PCBNo where a.SMTMO=@smtmo and Status='1' and  
            //(b.Station='30' or b.Station='8G')
            try
            {
                int ret = 0;

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(PCB);
                        PCB cond = new PCB();
                        cond.SMTMOID = smtMo;
                        tf1.equalcond = cond;
                        tf1.ToGetFieldNames.Add(PCB.fn_PCBNo);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(PCBStatus);
                        PCBStatus eqCond = new PCBStatus();
                        eqCond.Status = 1;
                        tf2.equalcond = eqCond;
                        PCBStatus inSetCond = new PCBStatus();
                        inSetCond.StationID = "INSET";
                        tf2.inSetcond = inSetCond;
                        tf2.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, PCB.fn_PCBNo, tf2, PCBStatus.fn_PCBID);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };

                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "COUNT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[Func.DecAlias(tf2.alias, PCBStatus.fn_Status)].Value = eqCond.Status;
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(PCBStatus.fn_StationID)), Func.ConvertInSet(new List<string>() { "30", "8G" }));
                    }
                }

                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[Func.DecAlias(tf1.alias, PCB.fn_SMTMOID)].Value = smtMo;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public int GetMBCountByMO(string smtMo)
        {
            //select count(*) from PCB where a.SMTMO=@smtmo
            try
            {
                int ret = 0;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB cond = new PCB();
                        cond.SMTMOID = smtMo;
                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), "COUNT", new List<string>() { PCB.fn_PCBNo}, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[PCB.fn_SMTMOID].Value = smtMo;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        private void updateSMTMOClosedByList(IList<string> moList)
        {
            try
            {
                //Update Status='C'  from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        SMTMO insetCond = new SMTMO();
                        insetCond.SmtMo = "INSET";
                        
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.SMTMO),
                                                                                                new List<string>() { SMTMO.fn_Status }, null,
                                                                                                  null, null, null, null, null, null, null, null, null,
                                                                                                insetCond);                       
                    }
                }

                sqlCtx.Params[_Schema.Func.DecSV(SMTMO.fn_Status)].Value = "C";
                sqlCtx.Params[_Schema.Func.DecSV(SMTMO.fn_Udt)].Value = _Schema.SqlHelper.GetDateTime();

                string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(SMTMO.fn_SmtMo), Func.ConvertInSet(moList));
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Defered  .

        public void SetMaxMODefered(IUnitOfWork uow, bool isExperiment, IMBMO maxMO)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), isExperiment, maxMO);
        }

        public void UpdateSMTMOPrtQtyForIncDefered(IUnitOfWork uow, string mo, int prtQtyInc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo, prtQtyInc);
        }

        //IList<string> DeleteSMTMODefered(IUnitOfWork uow, IList<string> moList)
        //{

        //}

        public void AddRctombmaintainInfoDefered(IUnitOfWork uow, RctombmaintainInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteRctombmaintainInfoDefered(IUnitOfWork uow, RctombmaintainInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void UpdateRctombmaintainInfoDefered(IUnitOfWork uow, RctombmaintainInfo setValue, RctombmaintainInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void IncreasePrintQtyForMoExcelDefered(IUnitOfWork uow, int qty, string line, string model)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), qty, line, model);
        }

        #endregion

        #region other functions
        public IList<SmtmoInfo> GetSmtmoInfoListByDay(int diffDayswithToday, string status)
        {
            try
            {
                IList<SmtmoInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Smtmo cond = new Smtmo();
                        cond.cdt = DateTime.Now;

                        Smtmo cond2 = new Smtmo();
                        cond2.printQty = 0;

                        //Vincent 2014-01-04 for CQ add condition
                        Smtmo cond1 = new Smtmo();
                        cond1.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<Smtmo>(null, null, new ConditionCollection<Smtmo>(
                            new GreaterOrEqualCondition<Smtmo>(cond),
                            new AnyCondition<Smtmo>(cond2, string.Format("{0}<{1}", "{0}", Smtmo.fn_qty)),
                            new EqualCondition<Smtmo>(cond1)),
                            Smtmo.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecGE(Smtmo.fn_cdt)).Value = cmDt.AddDays(diffDayswithToday);
                sqlCtx.Param(Smtmo.fn_status).Value = status;



                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Smtmo, SmtmoInfo, SmtmoInfo>(ret, sqlR, sqlCtx);
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
