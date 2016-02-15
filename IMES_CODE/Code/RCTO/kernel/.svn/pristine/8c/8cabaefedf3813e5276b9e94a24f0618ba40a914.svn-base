using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Hold;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Util;
using System.Data;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using Metas=IMES.Infrastructure.Repository._Metas;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;

namespace IMES.Infrastructure.Repository.Common
{
    public class HoldRepository: BaseRepository<Hold>, IHoldRepository
    {
        private static Metas.GetValueClass g = new Metas.GetValueClass();
      
        #region Overrides of BaseRepository<Hold>

        protected override void PersistNewItem(Hold item)
        {
        }

        protected override void PersistUpdatedItem(Hold item)
        {
        }

        protected override void PersistDeletedItem(Hold item)
        {
        }

        #endregion

        #region Implementation of IRepository<Hold>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override Hold Find(object key)
        {
            return null;
        }

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public override IList<Hold> FindAll()
        {
            return new List<Hold>();

        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        /// <param name="uow"></param>
        public override void Add(Hold item, IUnitOfWork uow)
        {
            //base.Add(item, uow);
            throw new Exception("No implementation");
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(Hold item, IUnitOfWork uow)
        {
            //base.Remove(item, uow); 
            throw new Exception("No implementation");
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(Hold item, IUnitOfWork uow)
        {
            //base.Update(item, uow);
            throw new Exception("No implementation");
        }

        #endregion

        #region . Inners .       

        #endregion

        #region implement HoldRule interface
        public void InsertHoldRule(HoldRuleInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;                

                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = _Metas.FuncNew.GetAquireIdInsert<_Metas.HoldRule>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.HoldRule, HoldRuleInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.HoldRule.fn_udt).Value = cmDt;
                sqlCtx.Param(_Metas.HoldRule.fn_cdt).Value = cmDt;

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
        public void UpdateHoldRule(HoldRuleInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;               

                lock (mthObj)
                {

                    _Metas.HoldRule cond = new _Metas.HoldRule();
                    cond.id = item.ID;
                    _Metas.HoldRule setv = _Metas.FuncNew.SetColumnFromField<_Metas.HoldRule, HoldRuleInfo>(item, _Metas.HoldRule.fn_id);
                    setv.udt = DateTime.Now;

                    sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.HoldRule>(new _Metas.SetValueCollection<_Metas.HoldRule>(new _Metas.CommonSetValue<_Metas.HoldRule>(setv)),
                                                                                                                          new _Metas.ConditionCollection<_Metas.HoldRule>(new _Metas.EqualCondition<_Metas.HoldRule>(cond)));
                 
                }

                sqlCtx.Param(_Metas.HoldRule.fn_id).Value = item.ID;

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.HoldRule, HoldRuleInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.HoldRule.fn_udt)).Value = cmDt;

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
        public void DeleteHoldRule(int id)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = _Metas.FuncNew.GetCommonDelete<_Metas.HoldRule>(tk);
                    }
                }

                sqlCtx.Param(_Metas.HoldRule.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, 
                                                                          CommandType.Text, 
                                                                          sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertMultiCustSNHoldRule(IList<string> custSnList, HoldRuleInfo item)
        {
            try
            {
              
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Metas.SQLContextNew();
                        sqlCtx.Sentence = @"insert HoldRule(Line, Family, Model, CUSTSN, HoldStation, 
                                                                    CheckInStation, HoldCode, HoldDescr, Editor, Cdt, 
                                                                    Udt)
                                                         select '','','',a.data,@HoldStation,
                                                                  @CheckInStation, @HoldCode, @HoldDescr, @Editor, GETDATE(),
                                                                  GETDATE()                
                                                       from @ProductIDList a ";
                        SqlParameter para = new SqlParameter("@ProductIDList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        para.Direction = ParameterDirection.Input;
                        sqlCtx.AddParam("ProductIDList", para);

                        sqlCtx.AddParam("HoldStation", new SqlParameter("@HoldStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("CheckInStation", new SqlParameter("@CheckInStation", SqlDbType.VarChar));

                        sqlCtx.AddParam("HoldCode", new SqlParameter("@HoldCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("HoldDescr", new SqlParameter("@HoldDescr", SqlDbType.NVarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                      
                    }
                }

                DataTable dt1 = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable(custSnList);
                sqlCtx.Param("ProductIDList").Value = dt1;

                sqlCtx.Param("HoldStation").Value = item.HoldStaion;
                sqlCtx.Param("CheckInStation").Value = item.CheckInStaion;
                sqlCtx.Param("HoldCode").Value = item.HoldCode;
                sqlCtx.Param("HoldDescr").Value = item.HoldDescr;
                sqlCtx.Param("Editor").Value = item.Editor;

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

        public IList<HoldRuleInfo> GetHoldRule(HoldRuleInfo condition)
        {
            try
            {
                IList<HoldRuleInfo> ret = new List<HoldRuleInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.HoldRule cond = _Metas.FuncNew.SetColumnFromField<_Metas.HoldRule, HoldRuleInfo>(condition);
                    sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.HoldRule>(null, null,
                                                                                                       new Metas.ConditionCollection<_Metas.HoldRule>(new Metas.EqualCondition<_Metas.HoldRule>(cond)),
                                                                                                       Metas.HoldRule.fn_id);
                }

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.HoldRule, HoldRuleInfo>(sqlCtx, condition);


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.HoldRule, HoldRuleInfo, HoldRuleInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<HoldRulePriorityInfo> GetHoldRulePriority(string line, string family, string model, string custSN, string checkInStation)
        {
            try
            {
                IList<HoldRulePriorityInfo> ret = new List<HoldRulePriorityInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new Metas.SQLContextNew();
                        sqlCtx.Sentence = @"select ID,HoldStation, HoldCode, HoldDescr,
                                                           (case when isnull(CUSTSN,'')!='' and CUSTSN=@CUSTSN then
                                                                   1
                                                                when Line=@Line and isnull(Family,'')='' and Model=@Model  and isnull(CUSTSN,'')='' then
                                                                   2
                                                                when Line=@Line and isnull(Model,'')='' and Family=@Family and isnull(CUSTSN,'')='' then
                                                                   3   
                                                                when  isnull(Line,'')=''  and isnull(Family,'')='' and Model=@Model and isnull(CUSTSN,'')='' then
                                                                   4
                                                                when  isnull(Line,'')=''  and isnull(Model,'')='' and Family=@Family and isnull(CUSTSN,'')='' then
                                                                   5   
                                                                when  isnull(Family,'')=''  and isnull(Model,'')='' and Line=@Line and isnull(CUSTSN,'')='' then
                                                                   6
                                                                else
                                                                   -1
                                                            end) as Priority               
                                                                      
                                                    from HoldRule
                                                    where CheckInStation=@CheckInStation
                                                    order by Priority";
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("CUSTSN", new SqlParameter("@CUSTSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("CheckInStation", new SqlParameter("@CheckInStation", SqlDbType.VarChar));
                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Line").Value = line;
                sqlCtx.Param("Model").Value = model;

                sqlCtx.Param("Family").Value = family;
                sqlCtx.Param("CUSTSN").Value = custSN;
                sqlCtx.Param("CheckInStation").Value = checkInStation;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null )
                    {
                        while (sqlR.Read())
                        {
                        ret.Add( SQLData.ToObjectByField<HoldRulePriorityInfo>(sqlR));
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

        public bool ExistHoldRuleByCustSn(IList<string> custSnList)
        {
            try
            {
               bool ret = false;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new Metas.SQLContextNew();
                        sqlCtx.Sentence = @"select a.ID 
                                                    from HoldRule a,
                                                             @ProductIDList b
                                                    where a.CUSTSN=b.data";
                        SqlParameter para = new SqlParameter("@ProductIDList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        para.Direction = ParameterDirection.Input;
                        sqlCtx.AddParam("ProductIDList", para);
                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                DataTable dt1 = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable(custSnList);
                sqlCtx.Param("ProductIDList").Value = dt1;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null)
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


        #region implement DefectHoldRule interface
        public void InsertDefectHoldRule(DefectHoldRuleInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;

                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = _Metas.FuncNew.GetAquireIdInsert<_Metas.DefectHoldRule>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.DefectHoldRule, DefectHoldRuleInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.DefectHoldRule.fn_udt).Value = cmDt;
                sqlCtx.Param(_Metas.DefectHoldRule.fn_cdt).Value = cmDt;

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
        public void UpdateDefectHoldRule(DefectHoldRuleInfo item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;

                lock (mthObj)
                {

                    _Metas.DefectHoldRule cond = new _Metas.DefectHoldRule();
                    cond.id = item.ID;
                    _Metas.DefectHoldRule setv = _Metas.FuncNew.SetColumnFromField<_Metas.DefectHoldRule, DefectHoldRuleInfo>(item, _Metas.DefectHoldRule.fn_id);
                    setv.udt = DateTime.Now;

                    sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.DefectHoldRule>(new _Metas.SetValueCollection<_Metas.DefectHoldRule>(new _Metas.CommonSetValue<_Metas.DefectHoldRule>(setv)),
                                                                                                                          new _Metas.ConditionCollection<_Metas.DefectHoldRule>(new _Metas.EqualCondition<_Metas.DefectHoldRule>(cond)));

                }

                sqlCtx.Param(_Metas.DefectHoldRule.fn_id).Value = item.ID;

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.DefectHoldRule, DefectHoldRuleInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.DefectHoldRule.fn_udt)).Value = cmDt;

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
        public void DeleteDefectHoldRule(int id)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                _Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = _Metas.FuncNew.GetCommonDelete<_Metas.DefectHoldRule>(tk);
                    }
                }

                sqlCtx.Param(_Metas.DefectHoldRule.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                          CommandType.Text,
                                                                          sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<DefectHoldRuleInfo> GetDefectHoldRule(DefectHoldRuleInfo condition)
        {
            try
            {
                IList<DefectHoldRuleInfo> ret = new List<DefectHoldRuleInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.DefectHoldRule cond = _Metas.FuncNew.SetColumnFromField<_Metas.DefectHoldRule, DefectHoldRuleInfo>(condition);
                    sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.DefectHoldRule>(null, null,
                                                                                                       new Metas.ConditionCollection<_Metas.DefectHoldRule>(new Metas.EqualCondition<_Metas.DefectHoldRule>(cond)),
                                                                                                       Metas.DefectHoldRule.fn_checkInStation, Metas.DefectHoldRule.fn_id);
                }

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.DefectHoldRule, DefectHoldRuleInfo>(sqlCtx, condition);


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.DefectHoldRule, DefectHoldRuleInfo, DefectHoldRuleInfo>(ret, sqlR, sqlCtx);
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
