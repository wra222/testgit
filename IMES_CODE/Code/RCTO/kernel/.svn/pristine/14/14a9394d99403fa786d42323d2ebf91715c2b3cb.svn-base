using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.LCM;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using IMES.DataModel;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.FA
{
    /// <summary>
    /// 数据访问与持久化类: LCM相关
    /// </summary>
    public class LCMRepository : BaseRepository<LCM>, ILCMRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<LCM>

        protected override void PersistNewItem(LCM item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    throw new NotImplementedException("Normal");
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(LCM item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    throw new NotImplementedException("Normal");
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(LCM item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    throw new NotImplementedException("Normal");
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region IRepository<LCM> Members

        public LCM Find(object key)
        {
            try
            {
                LCM ret = null;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.PartSN);
                        _Schema.PartSN equalCond = new _Schema.PartSN();
                        equalCond.IECSN = (string)key;
                        equalCond.PartType = "LCM";
                        tf1.equalcond = equalCond;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.LCMBind);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, _Schema.PartSN.fn_IECSN, tf2, _Schema.LCMBind.fn_LCMSno);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };

                        _Schema.TableBiJoinedLogic tblBiJndLgc = new _Schema.TableBiJoinedLogic();
                        tblBiJndLgc.Add(tf1);
                        tblBiJndLgc.Add(_Schema.Func.LEFTJOIN);
                        tblBiJndLgc.Add(tf2);
                        tblBiJndLgc.Add(tc1);

                        sqlCtx = _Schema.Func.GetConditionedComprehensiveJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts, tblBiJndLgc);

                        sqlCtx.Params[Func.DecAlias(tf1.alias, _Schema.PartSN.fn_PartType)].Value = equalCond.PartType;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[Func.DecAlias(tf1.alias, _Schema.PartSN.fn_IECSN)].Value = (string)key;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        IList<LCMME> content = new List<LCMME>();
                        while (sqlR.Read())
                        {
                            if (ret == null)
                                ret = new LCM(GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PartSN.fn_IECSN)]),
                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PartSN.fn_IECPn)]),
                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PartSN.fn_VendorSN)]),
                                    content
                                    );

                            if (!IsNull(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, LCMBind.fn_LCMSno)]))
                            {
                                LCMME item = new LCMME(GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, LCMBind.fn_ID)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, LCMBind.fn_LCMSno)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, LCMBind.fn_MESno)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, LCMBind.fn_METype)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, LCMBind.fn_Editor)]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, LCMBind.fn_Cdt)]));
                                item.Tracker.Clear();
                                content.Add(item);
                            }
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

        public IList<LCM> FindAll()
        {
            throw new NotImplementedException("Normal");
        }

        public void Add(LCM item, IUnitOfWork uow)
        {
            throw new NotImplementedException("Normal");
        }

        public void Remove(LCM item, IUnitOfWork uow)
        {
            throw new NotImplementedException("Normal");
        }

        public void Update(LCM item, IUnitOfWork uow)
        {
            throw new NotImplementedException("Normal");
        }

        #endregion

        #region ILCMRepository Members

        public int GetLCMBindCount(string lcmSno, string meType)
        {
            //select count(*) from IMES_FA..LCMBind where LCMSno=@CTNO and METype=@metype
            try
            {
                int ret = 0;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        LCMBind cond = new LCMBind();
                        cond.LCMSno = lcmSno;
                        cond.METype = meType;
                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(LCMBind), "COUNT", new List<string>() { LCMBind.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[LCMBind.fn_LCMSno].Value = lcmSno;
                sqlCtx.Params[LCMBind.fn_METype].Value = meType;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public int GetLCMBindCount(string meSno)
        {
            //select count(*) from IMES_FA..LCMBind where MESno=@btdlsn
            try
            {
                int ret = 0;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        LCMBind cond = new LCMBind();
                        cond.MESno = meSno;
                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(LCMBind), "COUNT", new List<string>() { LCMBind.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[LCMBind.fn_MESno].Value = meSno;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public void InsertLCMBind(LCMME item)
        {
            //Insert LCMBind: LCMSno=LCM CT#, MESno=BTDL SN#, METype='BTDL'
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(LCMBind));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[LCMBind.fn_Cdt].Value = cmDt;
                sqlCtx.Params[LCMBind.fn_Editor].Value = item.Editor;
                sqlCtx.Params[LCMBind.fn_LCMSno].Value = item.LCMSn;
                sqlCtx.Params[LCMBind.fn_MESno].Value = item.MESn;
                sqlCtx.Params[LCMBind.fn_METype].Value = item.METype;
                item.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ICASADef> GetICASAList()
        {
            try
            {
                IList<ICASADef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Icasa>(tk, Icasa.fn_vc);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Icasa, ICASADef, ICASADef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICASADef GetICASAInfoById(int id)
        {
            try
            {
                ICASADef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Icasa cond = new Icasa();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedSelect<Icasa>(tk, null, null, new ConditionCollection<Icasa>(new EqualCondition<Icasa>(cond)));
                    }
                }
                sqlCtx.Param(Icasa.fn_id).Value = id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Icasa, ICASADef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICASADef GetICASAInfoByVC(string vc)
        {
            try
            {
                ICASADef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Icasa cond = new Icasa();
                        cond.vc = vc;
                        sqlCtx = FuncNew.GetConditionedSelect<Icasa>(tk, null, null, new ConditionCollection<Icasa>(new EqualCondition<Icasa>(cond)));
                    }
                }
                sqlCtx.Param(Icasa.fn_vc).Value = vc;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Icasa, ICASADef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddICASAInfo(ICASADef item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Icasa>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Icasa, ICASADef>(sqlCtx, item);

                sqlCtx.Param(Icasa.fn_cdt).Value = cmDt;
                sqlCtx.Param(Icasa.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateICASAInfo(ICASADef item, int id)
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
                        Icasa cond = new Icasa();
                        cond.id = id;
                        Icasa setv = FuncNew.SetColumnFromField<Icasa, ICASADef>(item, Icasa.fn_id, Icasa.fn_cdt);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Icasa>(tk, new SetValueCollection<Icasa>(new CommonSetValue<Icasa>(setv)), new ConditionCollection<Icasa>(new EqualCondition<Icasa>(cond)));
                    }
                }
                sqlCtx.Param(Icasa.fn_id).Value = id;

                sqlCtx = FuncNew.SetColumnFromField<Icasa, ICASADef>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Icasa.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteICASAInfo(int id)
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
                        Icasa cond = new Icasa();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<Icasa>(tk, new ConditionCollection<Icasa>(new EqualCondition<Icasa>(cond)));
                    }
                }
                sqlCtx.Param(Icasa.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistICASAByVC(string vc)
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
                        Icasa cond = new Icasa();
                        cond.vc = vc;
                        Icasa cond2 = new Icasa();
                        cond2.antel1 = "antel1";

                        sqlCtx = FuncNew.GetConditionedSelect<Icasa>(tk, "COUNT", new string[] { Icasa.fn_id }, new ConditionCollection<Icasa>(new EqualCondition<Icasa>(cond),
                            new AnySoloCondition<Icasa>(cond2, string.Format("NOT (ISNULL({0},'')='' AND ISNULL({1},'')='')", "{0}", Icasa.fn_icasa))));
                    }
                }
                sqlCtx.Param(Icasa.fn_vc).Value = vc;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<ICASADef> GetICASAInfoByVCs(string[] vcs)
        {
            try
            {
                IList<ICASADef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Icasa cond = new Icasa();
                        cond.vc = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<Icasa>(tk, null, null, new ConditionCollection<Icasa>(new InSetCondition<Icasa>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(Icasa.fn_vc), g.ConvertInSet(vcs));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Icasa, ICASADef, ICASADef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void InsertLCMBindDefered(IUnitOfWork uow, LCMME item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddICASAInfoDefered(IUnitOfWork uow, IMES.DataModel.ICASADef item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateICASAInfoDefered(IUnitOfWork uow, IMES.DataModel.ICASADef item, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, id);
        }

        public void DeleteICASAInfoDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        #endregion

        #endregion
    }
}
