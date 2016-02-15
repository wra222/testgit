using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.UPS;
using IMES.FisObject.Common.Misc;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Line相关
    /// </summary>
    public class UPSRepository : BaseRepository<fons.UPSCombinePO>, fons.IUPSRepository
    {
        private static mtns::GetValueClass g = new mtns::GetValueClass();

        #region Link To Other
        private static IMiscRepository _miscRep = null;
        private static IMiscRepository MiscRep
        {
            get
            {
                if (_miscRep == null)
                    _miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                return _miscRep;
            }
        } 
        #endregion

        #region Overrides of BaseRepository<UPSCombinePO>

        protected override void PersistNewItem(fons.UPSCombinePO item)
        {
            throw new Exception("Not support Insert");
        }

        protected override void PersistUpdatedItem(fons.UPSCombinePO item)
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

                        _Metas.UPSCombinePO cond = new _Metas.UPSCombinePO();
                        cond.id = item.ID;
                        _Metas.UPSCombinePO setv = FuncNew.SetColumnFromField<_Metas.UPSCombinePO, fons.UPSCombinePO>(item, _Metas.UPSCombinePO.fn_id, 
                                                                                                                                                                                                _Metas.UPSCombinePO.fn_hppo,
                                                                                                                                                                                                _Metas.UPSCombinePO.fn_iecpo,
                                                                                                                                                                                                _Metas.UPSCombinePO.fn_iecpoitem,
                                                                                                                                                                                                _Metas.UPSCombinePO.fn_model,
                                                                                                                                                                                                _Metas.UPSCombinePO.fn_receiveDate);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.UPSCombinePO>(tk, new SetValueCollection<_Metas.UPSCombinePO>(new CommonSetValue<_Metas.UPSCombinePO>(setv)),
                                                                                                                  new ConditionCollection<_Metas.UPSCombinePO>(new EqualCondition<_Metas.UPSCombinePO>(cond)));

                    }
                }

                sqlCtx.Param(_Metas.UPSCombinePO.fn_id).Value = item.ID;

                sqlCtx = FuncNew.SetColumnFromField<_Metas.UPSCombinePO, fons.UPSCombinePO>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.UPSCombinePO.fn_udt)).Value = cmDt;

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

        protected override void PersistDeletedItem(fons.UPSCombinePO item)
        {
            throw new Exception("Not Support Delete");
        }

        #endregion

        #region Implementation of IRepository<fons.UPSCombinePO>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.UPSCombinePO Find(object key)
        {
            try
            {
                fons.UPSCombinePO ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;              
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                     if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        _Metas.UPSCombinePO cond = new _Metas.UPSCombinePO();
                        cond.productID = (string)key;
                        cond.status = fons.EnumUPSCombinePOStatus.Used.ToString();
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSCombinePO>(tk, "TOP 1", null,
                                                                                      new ConditionCollection<_Metas.UPSCombinePO>(new EqualCondition<_Metas.UPSCombinePO>(cond)));

                     }
                }
                sqlCtx.Param(_Metas.UPSCombinePO.fn_productID).Value = (string)key;
                sqlCtx.Param(_Metas.UPSCombinePO.fn_status).Value = fons.EnumUPSCombinePOStatus.Used.ToString();
                

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.UPSCombinePO, fons.UPSCombinePO>(ret, sqlR, sqlCtx);
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
        public override IList<fons.UPSCombinePO> FindAll()
        {
            try
            {
                IList<fons.UPSCombinePO> ret = new List<fons.UPSCombinePO>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<_Metas.UPSCombinePO>(tk, _Metas.UPSCombinePO.fn_id);
                    }
                }
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.UPSCombinePO, fons.UPSCombinePO, fons.UPSCombinePO>(ret, sqlR, sqlCtx);
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
        public override void Add(fons.UPSCombinePO item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(fons.UPSCombinePO item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons.UPSCombinePO item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IUPSRepository
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public bool IsUPSProductID(string productID)
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

                        _Metas.UPSCombinePO cond = new _Metas.UPSCombinePO();
                        cond.productID = productID;
                        cond.status = fons.EnumUPSCombinePOStatus.Used.ToString();
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSCombinePO>(tk, "TOP 1", new string[]{ _Metas.UPSCombinePO.fn_id},
                                                                                      new ConditionCollection<_Metas.UPSCombinePO>(new EqualCondition<_Metas.UPSCombinePO>(cond)));

                    }
                }

                sqlCtx.Param(_Metas.UPSCombinePO.fn_productID).Value = productID;
                sqlCtx.Param(_Metas.UPSCombinePO.fn_status).Value = fons.EnumUPSCombinePOStatus.Used.ToString();
                
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

        public bool IsUPSModel(string model)
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

                        _Metas.UPSModel cond = new _Metas.UPSModel();
                        cond.model = model;
                        cond.status = fons.EnumUPSModelPOStatus.Enable.ToString();
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSModel>(tk, "TOP 1", new string[] { _Metas.UPSModel.fn_model },
                                                                                      new ConditionCollection<_Metas.UPSModel>(new EqualCondition<_Metas.UPSModel>(cond)));

                    }
                }

                sqlCtx.Param(_Metas.UPSModel.fn_model).Value = model;
                sqlCtx.Param(_Metas.UPSModel.fn_status).Value = fons.EnumUPSModelPOStatus.Enable.ToString(); 
                
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

        public bool IsUPSModel(string model, DateTime afterReciveDate)
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

                        _Metas.UPSModel cond = new _Metas.UPSModel();
                        cond.model = model;
                        cond.status = fons.EnumUPSModelPOStatus.Enable.ToString();
                        _Metas.UPSModel cond2 = new _Metas.UPSModel();
                        cond2.firstReceiveDate = afterReciveDate;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSModel>(tk, "TOP 1", new string[] { _Metas.UPSModel.fn_model },
                                                                                      new ConditionCollection<_Metas.UPSModel>(new EqualCondition<_Metas.UPSModel>(cond),
                                                                                                                                                                   new GreaterCondition<_Metas.UPSModel>(cond2)));

                    }
                }

                sqlCtx.Param(_Metas.UPSModel.fn_model).Value = model;
                sqlCtx.Param(_Metas.UPSModel.fn_status).Value = fons.EnumUPSModelPOStatus.Enable.ToString();
                sqlCtx.Param(g.DecG(_Metas.UPSModel.fn_firstReceiveDate)).Value = afterReciveDate;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public fons.UPSCombinePO GetAvailablePOWithTrans(string model, IList<string> statusList)
        {
            try
            {
                fons.UPSCombinePO ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        _Metas.UPSCombinePO cond = new _Metas.UPSCombinePO();
                        cond.model =model;
                        _Metas.UPSCombinePO inCond = new _Metas.UPSCombinePO();
                        inCond.status = "INSET";
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSCombinePO>(tk,"TOP 1",null,
                                                                                      new ConditionCollection<_Metas.UPSCombinePO>(new EqualCondition<_Metas.UPSCombinePO>(cond),
                                                                                                                                                                   new InSetCondition<_Metas.UPSCombinePO>(inCond)));

                       sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=IDX_UPSCombinePO_Model_Status,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.UPSCombinePO.fn_model).Value = model;
                string sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.UPSCombinePO.fn_status), g.ConvertInSet(statusList));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                                CommandType.Text,
                                                                                                                               sentence,
                                                                                                                              sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.UPSCombinePO, fons.UPSCombinePO>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons.UPSCombinePO> GetUPSCombinePO(fons.UPSCombinePO condition)
        {
            return MiscRep.GetData<_Metas.UPSCombinePO, fons.UPSCombinePO>(condition);
        }

        public IList<fons.UPSCombinePO> GetUPSCombinePOByStatus(fons.UPSCombinePO condition, IList<string> statusList)
        {
            return MiscRep.GetDataByList<_Metas.UPSCombinePO, fons.UPSCombinePO>(condition, _Metas.UPSCombinePO.fn_status, statusList);
        }

        public IList<UPSPOAVPartInfo> GetAVPartByHPPO(string hppo)
        {
            try
            {
                IList<UPSPOAVPartInfo> ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        _Metas.UPSPOAVPart cond = new _Metas.UPSPOAVPart();
                        cond.hppo = hppo;
                         sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSPOAVPart>(tk,null, null,
                                                                                      new ConditionCollection<_Metas.UPSPOAVPart>(new EqualCondition<_Metas.UPSPOAVPart>(cond)));

                    }
                }
                sqlCtx.Param(_Metas.UPSPOAVPart.fn_hppo).Value = hppo;
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                   ret = FuncNew.SetFieldFromColumn<_Metas.UPSPOAVPart,UPSPOAVPartInfo,UPSPOAVPartInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UPSHPPOInfo GetHPPO(string hppo)
        {
            try
            {
                UPSHPPOInfo ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        _Metas.UPSHPPO cond = new _Metas.UPSHPPO();
                        cond.hppo = hppo;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSHPPO>(tk,null, null,
                                                                                     new ConditionCollection<_Metas.UPSHPPO>(new EqualCondition<_Metas.UPSHPPO>(cond)));

                    }
                }
                sqlCtx.Param(_Metas.UPSHPPO.fn_hppo).Value = hppo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.UPSHPPO, UPSHPPOInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public UPSIECPOInfo GetIECPO(string iecPO, string model)
        {
            try
            {
                UPSIECPOInfo ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        _Metas.UPSIECPO cond = new _Metas.UPSIECPO();
                        cond.iecpo = iecPO;
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSIECPO>(tk,"TOP 1 ", null,
                                                                                     new ConditionCollection<_Metas.UPSIECPO>(new EqualCondition<_Metas.UPSIECPO>(cond)));

                    }
                }
                sqlCtx.Param(_Metas.UPSIECPO.fn_iecpo).Value = iecPO;
                sqlCtx.Param(_Metas.UPSIECPO.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.UPSIECPO, UPSIECPOInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<UPSIECPOInfo> GetIECPO(string hppo)
        {
            try
            {
                IList<UPSIECPOInfo> ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        _Metas.UPSIECPO cond = new _Metas.UPSIECPO();
                        cond.hppo = hppo;                      
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.UPSIECPO>(tk, null, null,
                                                                                     new ConditionCollection<_Metas.UPSIECPO>(new EqualCondition<_Metas.UPSIECPO>(cond)));

                    }
                }
                sqlCtx.Param(_Metas.UPSIECPO.fn_hppo).Value = hppo;
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.UPSIECPO, UPSIECPOInfo, UPSIECPOInfo>(ret, sqlR, sqlCtx);
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
