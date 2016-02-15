using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.Infrastructure.Util;
using System.Data.SqlClient;
using System.Reflection;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.DataModel;
using fons = IMES.FisObject.PAK.Pizza;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: Pizza相关
    /// </summary>
    public class PizzaRepository : BaseRepository<Pizza>, IPizzaRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<PizzaID>

        protected override void PersistNewItem(Pizza item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertPizza(item);

                    fons.PizzaStatus pzStt = (fons.PizzaStatus)item.GetType().GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (pzStt != null)
                        this.PersistInsertPizzaStatus(item.PizzaID, pzStt);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(Pizza item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdatePizza(item);

                    fons.PizzaStatus pzStt = (fons.PizzaStatus)item.GetType().GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (pzStt != null)
                    {
                        if (tracker.GetState(pzStt) == DataRowState.Modified || tracker.GetState(pzStt) == DataRowState.Added)
                        {
                            this.PersistUpdatePizzaStatus(item.PizzaID, pzStt);
                        }
                    }

                    this.CheckAndInsertSubs(item, tracker);

                    this.CheckAndUpdateOrRemoveSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(Pizza item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {

                    fons.PizzaStatus pzStt = (fons.PizzaStatus)item.GetType().GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (pzStt != null)
                    {
                    }
                    else
                    {
                        pzStt = new fons.PizzaStatus();
                        pzStt.PizzaID = item.PizzaID;
                    }

                    this.PersistDeletePizzaStatus(pzStt);

                    this.CheckAndUpdateOrRemoveSubs(item, tracker);

                    this.PersistDeletePizza(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<PizzaID>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override Pizza Find(object key)
        {
            try
            {
                Pizza ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza cond = new _Schema.Pizza();
                        cond.PizzaID = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pizza.fn_PizzaID].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new Pizza();
                        ret.MMIID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_MMIID]);
                        ret.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_PizzaID]);
                        ret.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Model]);
                        ret.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_CartonSN]);
                        ret.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Remark]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Cdt]);
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
        public override IList<Pizza> FindAll()
        {
            try
            {
                IList<Pizza> ret = new List<Pizza>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        Pizza item = new Pizza();
                        item.MMIID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_MMIID]);
                        item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_PizzaID]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Model]);
                        item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_CartonSN]);
                        item.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Remark]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Cdt]);
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
        public override void Add(Pizza item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(Pizza item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(Pizza item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region IPizzaRepository Members

        public Pizza FillPizzaParts(Pizza currentPizza)
        {
            try
            {
                IList<IProductPart> newFieldVal = new List<IProductPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza_Part cond = new _Schema.Pizza_Part();
                        cond.pizzaID = currentPizza.PizzaID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pizza_Part.fn_pizzaID].Value = currentPizza.PizzaID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPart item = new ProductPart();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_id]);
                        item.PartID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partNo]);
                        item.ProductID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_pizzaID]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_station]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_udt]);
                        item.PartSn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partSn]);
                        item.Iecpn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_iecpn]);
                        item.CustomerPn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_custmerPn]);
                        item.PartType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partType]);
                        item.BomNodeType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_bomNodeType]);
                        item.CheckItemType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_checkItemType]);

                        item.Tracker.Clear();
                        item.Tracker = currentPizza.Tracker;
                        newFieldVal.Add(item);
                    }
                }
                currentPizza.GetType().GetField("_parts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPizza, newFieldVal);
                return currentPizza;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pizza FillPizzaStatus(Pizza currentPizza)
        {
            try
            {
                fons.PizzaStatus newFieldVal = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PizzaStatus cond = new _Schema.PizzaStatus();
                        cond.PizzaID = currentPizza.PizzaID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PizzaStatus), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PizzaStatus.fn_PizzaID].Value = currentPizza.PizzaID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        newFieldVal = new fons.PizzaStatus();
                        newFieldVal.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PizzaStatus.fn_Cdt]);
                        newFieldVal.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaStatus.fn_Editor]);
                        newFieldVal.LineID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaStatus.fn_Line]);
                        newFieldVal.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaStatus.fn_PizzaID]);
                        newFieldVal.StationID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaStatus.fn_Station]);
                        newFieldVal.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PizzaStatus.fn_Udt]);
                        newFieldVal.Tracker.Clear();
                        newFieldVal.Tracker = currentPizza.Tracker;
                    }
                }
                currentPizza.GetType().GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPizza, newFieldVal);
                return currentPizza;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IProductPart> GetPizzaPartsByPartNoAndValue(string partNo, string val)
        {
            try
            {
                IList<IProductPart> ret = new List<IProductPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza_Part cond = new _Schema.Pizza_Part();
                        cond.partNo = partNo;
                        cond.partSn = val;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pizza_Part.fn_partNo].Value = partNo;
                sqlCtx.Params[_Schema.Pizza_Part.fn_partSn].Value = val;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPart item = new ProductPart();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_cdt]); ;
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_editor]); ;
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_id]);
                        item.PartID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partNo]);
                        item.ProductID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_pizzaID]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_station]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_udt]);
                        item.PartSn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partSn]);
                        item.Iecpn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_iecpn]);
                        item.CustomerPn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_custmerPn]);
                        item.PartType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partType]);
                        item.BomNodeType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_bomNodeType]);
                        item.CheckItemType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_checkItemType]);

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

        public IList<IProductPart> GetPizzaPartsByPartSn(string partSn)
        {
            try
            {
                IList<IProductPart> ret = new List<IProductPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza_Part cond = new _Schema.Pizza_Part();
                        cond.partSn = partSn;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pizza_Part.fn_partSn].Value = partSn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ProductPart item = new ProductPart();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_cdt]); ;
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_editor]); ;
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_id]);
                        item.PartID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partNo]);
                        item.ProductID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_pizzaID]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_station]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_udt]);
                        item.PartSn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partSn]);
                        item.Iecpn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_iecpn]);
                        item.CustomerPn = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_custmerPn]);
                        item.PartType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_partType]);
                        item.BomNodeType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_bomNodeType]);
                        item.CheckItemType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza_Part.fn_checkItemType]);

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

        public int GetPizzaPartsCout(string pizzaID)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza_Part cond = new _Schema.Pizza_Part();
                        cond.pizzaID = pizzaID;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza_Part), "COUNT", new List<string>() { _Schema.Pizza_Part.fn_id }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pizza_Part.fn_pizzaID].Value = pizzaID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        public IList<PizzaPart> GetPizzaPartsByValue(string value)
        {
            try
            {
                IList<PizzaPart> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pizza_Part cond = new mtns::Pizza_Part();
                        cond.partSn = value;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pizza_Part>(tk, null, null, new ConditionCollection<mtns::Pizza_Part>(new EqualCondition<mtns::Pizza_Part>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pizza_Part.fn_partSn).Value = value;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pizza_Part, PizzaPart, PizzaPart>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDocCatsFromPakPakRT(string docCat)
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
                        mtns::PakDotpakrt cond = new mtns::PakDotpakrt();
                        cond.doc_cat = docCat + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PakDotpakrt>(tk, "DISTINCT", new string[] { mtns::PakDotpakrt.fn_doc_cat }, new ConditionCollection<mtns::PakDotpakrt>(new LikeCondition<mtns::PakDotpakrt>(cond)), mtns::PakDotpakrt.fn_doc_cat);
                    }
                }
                sqlCtx.Param(mtns::PakDotpakrt.fn_doc_cat).Value = docCat + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PakDotpakrt.fn_doc_cat));
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

        public IList<string> GetRegionsFromVPakComn()
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
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_PAKComn>(tk, "DISTINCT", new string[] { mtns::V_PAKComn.fn_region }, new ConditionCollection<mtns::V_PAKComn>(), mtns::V_PAKComn.fn_region);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_PAKComn.fn_region));
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

        public IList<string> GetIntlCarrierListFromVPakComn()
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
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_PAKComn>(tk, "DISTINCT", new string[] { mtns::V_PAKComn.fn_intl_carrier }, new ConditionCollection<mtns::V_PAKComn>(), mtns::V_PAKComn.fn_intl_carrier);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_PAKComn.fn_intl_carrier));
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

        public bool CheckExistPakDashPakComnByInternalID(string internalId)
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
                        _Metas.Pak_Pakcomn cond = new _Metas.Pak_Pakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Pakcomn>(tk, "COUNT", new string[] { _Metas.Pak_Pakcomn.fn_id }, new ConditionCollection<_Metas.Pak_Pakcomn>(new EqualCondition<_Metas.Pak_Pakcomn>(cond, "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(_Metas.Pak_Pakcomn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistVShipmentPakComnByConsolInvoiceOrShipment(string dn)
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
                        mtns::V_Shipment_PAKComn cond = new mtns::V_Shipment_PAKComn();
                        cond.consol_invoice = dn;
                        cond.shipment = dn;
                        var condSet = new ConditionCollection<mtns::V_Shipment_PAKComn>(false);
                        condSet.Add(new EqualCondition<mtns::V_Shipment_PAKComn>(cond));
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_Shipment_PAKComn>(tk, "COUNT", new string[] { V_Shipment_PAKComn.fn_id }, condSet);
                    }
                }
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_consol_invoice).Value = dn;
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_shipment).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistVShipmentPakComnByWaybillNo(string waybillNo)
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
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.waybill_number = waybillNo;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, "COUNT", new string[] { _Metas.V_Shipment_PAKComn.fn_id }, new ConditionCollection<_Metas.V_Shipment_PAKComn>(new EqualCondition<_Metas.V_Shipment_PAKComn>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_waybill_number).Value = waybillNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<VShipmentPakComnInfo> GetVShipmentPakComnListByLikeInternalID(string internalId)
        {
            try
            {
                IList<VShipmentPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::V_Shipment_PAKComn cond = new mtns::V_Shipment_PAKComn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_Shipment_PAKComn>(tk, null, null, new ConditionCollection<mtns::V_Shipment_PAKComn>(new EqualCondition<mtns::V_Shipment_PAKComn>(cond, "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<V_Shipment_PAKComn, VShipmentPakComnInfo, VShipmentPakComnInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPAKDotPAKEdi850RawByPoNum(string poNum)
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
                        _Metas.PakDotpakedi850raw cond = new _Metas.PakDotpakedi850raw();
                        cond.po_num = poNum;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PakDotpakedi850raw>(tk, "COUNT", new string[] { _Metas.PakDotpakedi850raw.fn_id }, new ConditionCollection<_Metas.PakDotpakedi850raw>(new EqualCondition<_Metas.PakDotpakedi850raw>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PakDotpakedi850raw.fn_po_num).Value = poNum;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<VShipmentPakComnInfo> GetVShipmentPakComnByConsolInvoiceOrShipment(string dn)
        {
            try
            {
                IList<VShipmentPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::V_Shipment_PAKComn cond = new mtns::V_Shipment_PAKComn();
                        cond.consol_invoice = dn;
                        cond.shipment = dn;
                        var condSet = new ConditionCollection<mtns::V_Shipment_PAKComn>(false);
                        condSet.Add(new EqualCondition<mtns::V_Shipment_PAKComn>(cond));
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_Shipment_PAKComn>(tk, null, new string[] { V_Shipment_PAKComn.fn_internalID, V_Shipment_PAKComn.fn_model, V_Shipment_PAKComn.fn_po_num, V_Shipment_PAKComn.fn_actual_shipdate }, condSet);
                    }
                }
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_consol_invoice).Value = dn;
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_shipment).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<VShipmentPakComnInfo>();
                        while (sqlR.Read())
                        {
                            VShipmentPakComnInfo item = new VShipmentPakComnInfo();
                            item.internalID = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_internalID));
                            item.model = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_model));
                            item.po_num = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_po_num));
                            item.actual_shipdate = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_actual_shipdate));
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

        public bool CheckExistPoNumNotInPAKDotPAKEdi850Raw(IList<string> infos)
        {
            try
            {
                bool ret = true;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.PakDotpakedi850raw cond = new _Metas.PakDotpakedi850raw();
                        cond.po_num = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakedi850raw>(tk, "COUNT", new string[][] { new string[] { _Metas.PakDotpakedi850raw.fn_po_num, string.Format("DISTINCT({0})", _Metas.PakDotpakedi850raw.fn_po_num) } }, new ConditionCollection<_Metas.PakDotpakedi850raw>(new InSetCondition<_Metas.PakDotpakedi850raw>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PakDotpakedi850raw.fn_po_num), g.ConvertInSet(infos));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        cnt = infos.Count - cnt;
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

        public IList<VShipmentPakComnInfo> GetVShipmentPakComnListByWaybillNo(string waybillNo)
        {
            try
            {
                IList<VShipmentPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        V_Shipment_PAKComn cond = new V_Shipment_PAKComn();
                        cond.waybill_number = waybillNo;
                        sqlCtx = FuncNew.GetConditionedSelect<V_Shipment_PAKComn>(tk, "DISTINCT", new string[] { V_Shipment_PAKComn.fn_internalID, V_Shipment_PAKComn.fn_model, V_Shipment_PAKComn.fn_po_num, V_Shipment_PAKComn.fn_actual_shipdate }, new ConditionCollection<V_Shipment_PAKComn>(new EqualCondition<V_Shipment_PAKComn>(cond)));
                    }
                }
                sqlCtx.Param(V_Shipment_PAKComn.fn_waybill_number).Value = waybillNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<VShipmentPakComnInfo>();
                        while(sqlR.Read())
                        {
                            VShipmentPakComnInfo item = new VShipmentPakComnInfo();
                            item.internalID = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_internalID));
                            item.model = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_model));
                            item.po_num = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_po_num));
                            item.actual_shipdate = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_actual_shipdate));
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

        public IList<PakDashPakComnInfo> GetPakDashPakComnListByInternalID(string internalId)
        {
            try
            {
                IList<PakDashPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Pakcomn cond = new Pak_Pakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Pakcomn>(tk, null, null, new ConditionCollection<Pak_Pakcomn>(new EqualCondition<Pak_Pakcomn>(cond, "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(Pak_Pakcomn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Pakcomn, PakDashPakComnInfo, PakDashPakComnInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakEdiInstr(string poNum)
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
                        _Metas.Pakedi_Instr cond = new _Metas.Pakedi_Instr();
                        cond.po_num = poNum;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pakedi_Instr>(tk, "COUNT", new string[] { _Metas.Pakedi_Instr.fn_id }, new ConditionCollection<_Metas.Pakedi_Instr>(new EqualCondition<_Metas.Pakedi_Instr>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pakedi_Instr.fn_po_num).Value = poNum;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<string> GetPoNumListFromPakDashPakComn(IList<string> internalIds, string instrFlag)
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
                        mtns::Pak_Pakcomn cond = new mtns::Pak_Pakcomn();
                        cond.instr_flag = instrFlag;
                        mtns::Pak_Pakcomn cond2 = new mtns::Pak_Pakcomn();
                        cond2.internalID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Pakcomn>(tk, "DISTINCT", new string[] { mtns::Pak_Pakcomn.fn_po_num }, new ConditionCollection<mtns::Pak_Pakcomn>(new EqualCondition<mtns::Pak_Pakcomn>(cond, "UPPER({0})"), new InSetCondition<mtns::Pak_Pakcomn>(cond2)), mtns::Pak_Pakcomn.fn_po_num);
                    }
                }
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_instr_flag).Value = instrFlag;
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Pakcomn.fn_internalID), g.ConvertInSet(internalIds));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Pakcomn.fn_po_num));
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

        public bool CheckExistPoNumNotInPakEdiInstr(IList<string> instrs)
        {
            try
            {
                bool ret = true;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pakedi_Instr cond = new _Metas.Pakedi_Instr();
                        cond.po_num = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Pakedi_Instr>(tk, "COUNT", new string[][] { new string[]{_Metas.Pakedi_Instr.fn_po_num, string.Format("DISTINCT({0})",_Metas.Pakedi_Instr.fn_po_num)} }, new ConditionCollection<_Metas.Pakedi_Instr>(new InSetCondition<_Metas.Pakedi_Instr>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pakedi_Instr.fn_po_num), g.ConvertInSet(instrs));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        cnt = instrs.Count - cnt;
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

        public IList<string> GetInternalIDsFromVShipmentPakComnListByLikeInternalID(string internalId)
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
                        mtns::V_Shipment_PAKComn cond = new mtns::V_Shipment_PAKComn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_Shipment_PAKComn>(tk, null, new string[] { mtns::V_Shipment_PAKComn.fn_internalID }, new ConditionCollection<mtns::V_Shipment_PAKComn>(new EqualCondition<mtns::V_Shipment_PAKComn>(cond, "LEFT({0},10)")), mtns::V_Shipment_PAKComn.fn_internalID);
                    }
                }
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_internalID).Value = internalId;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_internalID));
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

        public IList<string> GetInternalIDsFromVShipmentPakComnListByConsolInvoiceOrShipment(string dn)
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
                        mtns::V_Shipment_PAKComn cond = new mtns::V_Shipment_PAKComn();
                        cond.consol_invoice = dn;
                        cond.shipment = dn;
                        var condSet = new ConditionCollection<mtns::V_Shipment_PAKComn>(false);
                        condSet.Add(new EqualCondition<mtns::V_Shipment_PAKComn>(cond));
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_Shipment_PAKComn>(tk, null, new string[] { mtns::V_Shipment_PAKComn.fn_internalID }, condSet, mtns::V_Shipment_PAKComn.fn_internalID);
                    }
                }
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_consol_invoice).Value = dn;
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_shipment).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_internalID));
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

        public IList<string> GetInternalIDsFromVShipmentPakComnListByConsolInvoiceOrShipmentAndRegion(string dn, string region)
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
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.region = region;
                        mtns::V_Shipment_PAKComn cond2 = new mtns::V_Shipment_PAKComn();
                        cond2.consol_invoice = dn;
                        //cond.shipment = dn;
                        var condSet = new ConditionCollection<mtns::V_Shipment_PAKComn>(true);
                        condSet.Add(new EqualCondition<mtns::V_Shipment_PAKComn>(cond));
                        condSet.Add(new AnyCondition<mtns::V_Shipment_PAKComn>(cond2, string.Format("({0}={1} OR {2}={1})", "{0}", "{1}", _Metas.V_Shipment_PAKComn.fn_shipment)));
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_Shipment_PAKComn>(tk, null, new string[] { mtns::V_Shipment_PAKComn.fn_internalID }, condSet, mtns::V_Shipment_PAKComn.fn_internalID);
                    }
                }
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_region).Value = region;
                sqlCtx.Param(g.DecAny(_Metas.V_Shipment_PAKComn.fn_consol_invoice)).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_internalID));
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

        public IList<string> GetInternalIDsFromVShipmentPakComnListByWaybillNo(string waybillNo)
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
                        mtns::V_Shipment_PAKComn cond = new mtns::V_Shipment_PAKComn();
                        cond.waybill_number = waybillNo;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_Shipment_PAKComn>(tk, null, new string[] { mtns::V_Shipment_PAKComn.fn_internalID }, new ConditionCollection<mtns::V_Shipment_PAKComn>(new EqualCondition<mtns::V_Shipment_PAKComn>(cond)), mtns::V_Shipment_PAKComn.fn_internalID);
                    }
                }
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_waybill_number).Value = waybillNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Shipment_PAKComn.fn_internalID));
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

        public IList<string> GetDocSetNumListFromPakDashPakComnByLikeInternalID(string internalId)
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
                        mtns::PakDotpakcomn cond = new mtns::PakDotpakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PakDotpakcomn>(tk, null, new string[] { mtns::PakDotpakcomn.fn_doc_set_number }, new ConditionCollection<mtns::PakDotpakcomn>(new LikeCondition<mtns::PakDotpakcomn>(cond)), mtns::PakDotpakcomn.fn_doc_set_number);
                    }
                }
                //Vincent Change used like SQL for performance issue
                sqlCtx.Param(mtns::PakDotpakcomn.fn_internalID).Value = internalId.Trim()+"%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PakDotpakcomn.fn_doc_set_number));
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

        public IList<string> GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer(string docCat, string docSetNumer)
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
                        mtns::PakDotpakrt cond = new mtns::PakDotpakrt();
                        cond.doc_cat = docCat;
                        cond.doc_set_number = docSetNumer;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PakDotpakrt>(tk, null, new string[] { mtns::PakDotpakrt.fn_xsl_template_name }, new ConditionCollection<mtns::PakDotpakrt>(new EqualCondition<mtns::PakDotpakrt>(cond)), mtns::PakDotpakrt.fn_xsl_template_name);
                    }
                }
                sqlCtx.Param(mtns::PakDotpakrt.fn_doc_cat).Value = docCat;
                sqlCtx.Param(mtns::PakDotpakrt.fn_doc_set_number).Value = docSetNumer;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PakDotpakrt.fn_xsl_template_name));
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

        public IList<VPakComnInfo> GetVPakComnList(string internalId)
        {
            try
            {
                IList<VPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        V_PAKComn cond = new V_PAKComn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<V_PAKComn>(tk, null, new string[] { V_PAKComn.fn_internalID, V_PAKComn.fn_shipment, V_PAKComn.fn_waybill_number, V_PAKComn.fn_pack_id }, new ConditionCollection<V_PAKComn>(new EqualCondition<V_PAKComn>(cond)));
                    }
                }
                sqlCtx.Param(V_PAKComn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<VPakComnInfo>();
                        while (sqlR.Read())
                        {
                            VPakComnInfo item = new VPakComnInfo();
                            item.internalID = g.GetValue_Str(sqlR, sqlCtx.Indexes(V_PAKComn.fn_internalID));
                            item.shipment = g.GetValue_Str(sqlR, sqlCtx.Indexes(V_PAKComn.fn_shipment));
                            item.waybill_number = g.GetValue_Str(sqlR, sqlCtx.Indexes(V_PAKComn.fn_waybill_number));
                            item.PackId = g.GetValue_Str(sqlR, sqlCtx.Indexes(V_PAKComn.fn_pack_id));
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

        public bool CheckExistPakPackkingDataByDnList(IList<string> dnList)
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
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.internalID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Packkingdata>(tk, "COUNT", new string[] { _Metas.Pak_Packkingdata.fn_id }, new ConditionCollection<_Metas.Pak_Packkingdata>(new InSetCondition<_Metas.Pak_Packkingdata>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Packkingdata.fn_internalID), g.ConvertInSet(dnList));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
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

        public bool CheckExistPakDashPakComnByLikeInternalIDAndModel(string internalId, string model)
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
                        _Metas.Pak_Pakcomn cond = new _Metas.Pak_Pakcomn();
                        cond.internalID = internalId;
                        _Metas.Pak_Pakcomn cond2 = new _Metas.Pak_Pakcomn();
                        cond2.model = model + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Pakcomn>(tk, "COUNT", new string[] { _Metas.Pak_Pakcomn.fn_id }, new ConditionCollection<_Metas.Pak_Pakcomn>(new EqualCondition<_Metas.Pak_Pakcomn>(cond, "LEFT({0},10)"), new LikeCondition<_Metas.Pak_Pakcomn>(cond2)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_internalID).Value = internalId;
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_model).Value = model + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<PakPackkingDataInfo> GetPakPackkingDataListByLikeInternalID(string internalId)
        {
            try
            {
                IList<PakPackkingDataInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Packkingdata cond = new Pak_Packkingdata();
                        cond.internalID = internalId + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Packkingdata>(tk, null, null, new ConditionCollection<Pak_Packkingdata>(
                            new LikeCondition<Pak_Packkingdata>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Packkingdata.fn_internalID).Value = internalId + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Packkingdata, PakPackkingDataInfo, PakPackkingDataInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertForBackupPakPackkingData(IList<string> internalIds)
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
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.internalID = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Pak_Packkingdata, _Metas.Pak_Packkingdata_Del>(tk,
                            new string[][]{
                                new string[]{_Metas.Pak_Packkingdata.fn_actual_shipdate, _Metas.Pak_Packkingdata_Del.fn_actual_shipdate},
                                new string[]{_Metas.Pak_Packkingdata.fn_box_id, _Metas.Pak_Packkingdata_Del.fn_box_id},
                                new string[]{_Metas.Pak_Packkingdata.fn_cdt, _Metas.Pak_Packkingdata_Del.fn_cdt},
                                //new string[]{_Metas.Pak_Packkingdata.fn_id, _Metas.Pak_Packkingdata_Del.fn_id},
                                new string[]{_Metas.Pak_Packkingdata.fn_internalID, _Metas.Pak_Packkingdata_Del.fn_internalID},
                                new string[]{_Metas.Pak_Packkingdata.fn_pallet_id, _Metas.Pak_Packkingdata_Del.fn_pallet_id},
                                new string[]{_Metas.Pak_Packkingdata.fn_prod_type, _Metas.Pak_Packkingdata_Del.fn_prod_type},
                                new string[]{_Metas.Pak_Packkingdata.fn_serial_num, _Metas.Pak_Packkingdata_Del.fn_serial_num},
                                new string[]{_Metas.Pak_Packkingdata.fn_track_no_parcel, _Metas.Pak_Packkingdata_Del.fn_track_no_parcel},
                                new string[]{"GETDATE()", _Metas.Pak_Packkingdata_Del.fn_delDt},
                            },
                            new ConditionCollection<_Metas.Pak_Packkingdata>(new InSetCondition<_Metas.Pak_Packkingdata>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Packkingdata.fn_internalID), g.ConvertInSet(internalIds));
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakPackkingData(IList<string> internalIds)
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
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.internalID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.Pak_Packkingdata>(tk, new ConditionCollection<_Metas.Pak_Packkingdata>(new InSetCondition<_Metas.Pak_Packkingdata>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Packkingdata.fn_internalID), g.ConvertInSet(new List<string>(internalIds)));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPakPackkingData(IList<PakPackkingDataInfo> items)
        {
            SqlTransactionManager.Begin();
            try
            {
                if (items != null && items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        PakPackkingDataInfo item = items[i];
                        SQLContextNew sqlCtx = ComposeForInsertPakPackkingData(item);
                        item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                    }
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

        private SQLContextNew ComposeForInsertPakPackkingData(PakPackkingDataInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<_Metas.Pak_Packkingdata>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.Pak_Packkingdata, PakPackkingDataInfo>(sqlCtx, item);
                return sqlCtx;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetCountOfPakDashPakComnByLikeInternalID(string internalId)
        {
            try
            {
                decimal ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pak_Pakcomn cond = new _Metas.Pak_Pakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Pak_Pakcomn>(tk, "SUM", new string[][] { new string[] { _Metas.Pak_Pakcomn.fn_pack_id_unit_qty, string.Format("CONVERT(NUMERIC(18,0),{0})", _Metas.Pak_Pakcomn.fn_pack_id_unit_qty) } },
                            new ConditionCollection<_Metas.Pak_Pakcomn>(new EqualCondition<_Metas.Pak_Pakcomn>(cond, "LEFT({0},10)", "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_internalID).Value = internalId;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Decimal(sqlR, sqlCtx.Indexes("SUM"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPakPackkingDataByLikeInternalID(string internalId)
        {
            try
            {
                int ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Packkingdata>(tk, "COUNT", new string[] { _Metas.Pak_Packkingdata.fn_serial_num},
                            new ConditionCollection<_Metas.Pak_Packkingdata>(new EqualCondition<_Metas.Pak_Packkingdata>(cond, "LEFT({0},10)", "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(mtns::Pak_Packkingdata.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakPackkingData(string internalId)
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
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.internalID = internalId;
                        _Metas.Pak_Packkingdata cond2 = new _Metas.Pak_Packkingdata();
                        cond2.track_no_parcel = string.Empty;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Packkingdata>(tk, "COUNT", new string[] { _Metas.Pak_Packkingdata.fn_id },
                            new ConditionCollection<_Metas.Pak_Packkingdata>(
                                new EqualCondition<_Metas.Pak_Packkingdata>(cond),
                                new NotEqualCondition<_Metas.Pak_Packkingdata>(cond2))
                            );

                        sqlCtx.Param(mtns::Pak_Packkingdata.fn_track_no_parcel).Value = cond2.track_no_parcel;
                    }
                }
                sqlCtx.Param(mtns::Pak_Packkingdata.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public IList<string> GetInternalIdsFromVPakComn(string region, DateTime cdt, DateTime edt)
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
                        tf1 = new TableAndFields<V_PAKComn>();
                        V_PAKComn cond = new V_PAKComn();
                        cond.region = region;
                        V_PAKComn cond2 = new V_PAKComn();
                        cond2.actual_shipdate = "[BETWEEN]";
                        tf1.Conditions.Add(new EqualCondition<V_PAKComn>(cond));
                        tf1.Conditions.Add(new BetweenCondition<V_PAKComn>(cond2, "CONVERT(NVARCHAR(10),CONVERT(DATETIME,{0}),120)"));
                        tf1.AddRangeToGetFuncedFieldNames(new string[] { V_PAKComn.fn_internalID, "LEFT({0},10)" });

                        tf2 = new TableAndFields<PakDotpakedi850raw>();
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<V_PAKComn, PakDotpakedi850raw>(tf1, V_PAKComn.fn_po_num, tf2, PakDotpakedi850raw.fn_po_num));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, V_PAKComn.fn_region)).Value = region;

                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecBeg(V_PAKComn.fn_actual_shipdate))).Value = string.Format("{0}-{1}-{2}", cdt.Year.ToString(), cdt.Month.ToString().PadLeft(2, '0'), cdt.Day.ToString().PadLeft(2, '0')); //cdt.ToString();
                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecEnd(V_PAKComn.fn_actual_shipdate))).Value = string.Format("{0}-{1}-{2}", edt.Year.ToString(), edt.Month.ToString().PadLeft(2, '0'), edt.Day.ToString().PadLeft(2, '0')); //edt.ToString();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, V_PAKComn.fn_internalID)));
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

        public IList<string> GetInternalIdsFromVPakComn(string region, DateTime cdt, DateTime edt, string intlCarrier)
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
                        tf1 = new TableAndFields<V_PAKComn>();
                        V_PAKComn cond = new V_PAKComn();
                        cond.intl_carrier = intlCarrier;
                        cond.region = region;
                        V_PAKComn cond2 = new V_PAKComn();
                        cond2.actual_shipdate = "[BETWEEN]";
                        tf1.Conditions.Add(new EqualCondition<V_PAKComn>(cond));
                        tf1.Conditions.Add(new BetweenCondition<V_PAKComn>(cond2, "CONVERT(NVARCHAR(10),CONVERT(DATETIME,{0}),120)"));
                        tf1.AddRangeToGetFuncedFieldNames(new string[] { V_PAKComn.fn_internalID, "LEFT({0},10)" });

                        tf2 = new TableAndFields<PakDotpakedi850raw>();
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<V_PAKComn, PakDotpakedi850raw>(tf1, V_PAKComn.fn_po_num, tf2, PakDotpakedi850raw.fn_po_num));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, V_PAKComn.fn_intl_carrier)).Value = intlCarrier;
                sqlCtx.Param(g.DecAlias(tf1.Alias, V_PAKComn.fn_region)).Value = region;

                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecBeg(V_PAKComn.fn_actual_shipdate))).Value = string.Format("{0}-{1}-{2}",cdt.Year.ToString(), cdt.Month.ToString().PadLeft(2, '0'), cdt.Day.ToString().PadLeft(2, '0')); //cdt.ToString();
                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecEnd(V_PAKComn.fn_actual_shipdate))).Value = string.Format("{0}-{1}-{2}", edt.Year.ToString(), edt.Month.ToString().PadLeft(2, '0'), edt.Day.ToString().PadLeft(2, '0')); //edt.ToString();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, V_PAKComn.fn_internalID)));
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

        public IList<string> GetDocCatFromPakDotParRt(string docCatLike, string docCatNQ)
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
                        mtns::PakDotpakrt cond = new mtns::PakDotpakrt();
                        cond.doc_cat = docCatLike + "%";
                        mtns::PakDotpakrt cond2 = new mtns::PakDotpakrt();
                        cond2.doc_cat = docCatNQ;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PakDotpakrt>(tk, "DISTINCT", new string[] { mtns::PakDotpakrt.fn_doc_cat }, new ConditionCollection<mtns::PakDotpakrt>(
                            new LikeCondition<mtns::PakDotpakrt>(cond),
                            new NotEqualCondition<mtns::PakDotpakrt>(cond2)),
                            mtns::PakDotpakrt.fn_doc_cat + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::PakDotpakrt.fn_doc_cat).Value = docCatLike + "%";
                sqlCtx.Param(mtns::PakDotpakrt.fn_doc_cat + "$1").Value = docCatNQ;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PakDotpakrt.fn_doc_cat));
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

        public IList<string> GetRegionListFromVPakComn()
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
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_PAKComn>(tk, "DISTINCT", new string[] { mtns::V_PAKComn.fn_region }, new ConditionCollection<mtns::V_PAKComn>(), mtns::V_PAKComn.fn_region);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_PAKComn.fn_region));
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

        public IList<VShipmentPakComnInfo> GetVShipmentPakComnListByInternalID(string internalId)
        {
            try
            {
                IList<VShipmentPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        V_Shipment_PAKComn cond = new V_Shipment_PAKComn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<V_Shipment_PAKComn>(tk, null, null, new ConditionCollection<V_Shipment_PAKComn>(new EqualCondition<V_Shipment_PAKComn>(cond)));
                    }
                }
                sqlCtx.Param(V_Shipment_PAKComn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<V_Shipment_PAKComn, VShipmentPakComnInfo, VShipmentPakComnInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakDotPakEdi850RawByPoNum(string poNum)
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
                        _Metas.PakDotpakedi850raw cond = new _Metas.PakDotpakedi850raw();
                        cond.po_num = poNum;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PakDotpakedi850raw>(tk, "COUNT", new string[] { _Metas.PakDotpakedi850raw.fn_id }, new ConditionCollection<_Metas.PakDotpakedi850raw>(new EqualCondition<_Metas.PakDotpakedi850raw>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PakDotpakedi850raw.fn_po_num).Value = poNum;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<string> GetInstrFlagListFromPakDotPakComn(string internalId)
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
                        mtns::Pak_Pakcomn cond = new mtns::Pak_Pakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Pakcomn>(tk, null, new string[] { mtns::Pak_Pakcomn.fn_instr_flag }, new ConditionCollection<mtns::Pak_Pakcomn>(new EqualCondition<mtns::Pak_Pakcomn>(cond)), mtns::Pak_Pakcomn.fn_instr_flag);
                    }
                }
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Pakcomn.fn_instr_flag));
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

        public void InsertPakSkuMasterWeightFis(PakSkuMasterWeightFisInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Pak_Skumasterweight_Fis>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::Pak_Skumasterweight_Fis, PakSkuMasterWeightFisInfo>(sqlCtx, item);

                sqlCtx.Param(mtns::Pak_Skumasterweight_Fis.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetAverageWeightFromPakSkuMasterWeightFis(string model)
        {
            try
            {
                decimal ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pak_Skumasterweight_Fis cond = new _Metas.Pak_Skumasterweight_Fis();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Pak_Skumasterweight_Fis>(tk, null, new string[][] { new string[] { _Metas.Pak_Skumasterweight_Fis.fn_weight, string.Format("CONVERT(DECIMAL(10,3),SUM({0})/COUNT(*))", _Metas.Pak_Skumasterweight_Fis.fn_weight) } },
                            new ConditionCollection<_Metas.Pak_Skumasterweight_Fis>(new EqualCondition<_Metas.Pak_Skumasterweight_Fis>(cond, "LEFT({0},4)", "LEFT({0},4)")));
                    }
                }
                sqlCtx.Param(mtns::Pak_Skumasterweight_Fis.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_DecimalWithMinusForNull(sqlR, sqlCtx.Indexes(_Metas.Pak_Skumasterweight_Fis.fn_weight));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<decimal> GetWeightFromPakSkuMasterWeightFis(string model)
        {
            try
            {
                IList<decimal> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pak_Skumasterweight_Fis cond = new mtns::Pak_Skumasterweight_Fis();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Skumasterweight_Fis>(tk, null, new string[] { mtns::Pak_Skumasterweight_Fis.fn_weight }, new ConditionCollection<mtns::Pak_Skumasterweight_Fis>(new EqualCondition<mtns::Pak_Skumasterweight_Fis>(cond)), mtns::Pak_Skumasterweight_Fis.fn_weight);
                    }
                }
                sqlCtx.Param(mtns::Pak_Skumasterweight_Fis.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<decimal>();
                        while(sqlR.Read())
                        {
                            decimal item = g.GetValue_Decimal(sqlR, sqlCtx.Indexes(mtns::Pak_Skumasterweight_Fis.fn_weight));
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

        public IList<decimal> GetPackIdLineItemBoxQtyFromPakShipmentWeightFis(string internalId)
        {
            try
            {
                IList<decimal> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, null, new string[] { _Metas.V_Shipment_PAKComn.fn_pack_id_line_item_box_qty }, new ConditionCollection<_Metas.V_Shipment_PAKComn>(new EqualCondition<_Metas.V_Shipment_PAKComn>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<decimal>();

                        while (sqlR.Read())
                        {
                            decimal item = g.GetValue_Decimal(sqlR, sqlCtx.Indexes(_Metas.V_Shipment_PAKComn.fn_pack_id_line_item_box_qty));
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

        public void InsertPakShipmentWeightFis(PakShipmentWeightFisInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Pak_Shipmentweight_Fis>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::Pak_Shipmentweight_Fis, PakShipmentWeightFisInfo>(sqlCtx, item);

                sqlCtx.Param(mtns::Pak_Shipmentweight_Fis.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakShipmentWeightFis(string shipment)
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
                        _Metas.Pak_Shipmentweight_Fis cond = new _Metas.Pak_Shipmentweight_Fis();
                        cond.shipment = shipment;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Shipmentweight_Fis>(tk, "COUNT", new string[] { _Metas.Pak_Shipmentweight_Fis.fn_id }, new ConditionCollection<_Metas.Pak_Shipmentweight_Fis>(new EqualCondition<_Metas.Pak_Shipmentweight_Fis>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pak_Shipmentweight_Fis.fn_shipment).Value = shipment;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public decimal GetSumOfPackIdLineItemBoxQtyFromVShipmentPakComn(string consolInvoice)
        {
            try
            {
                decimal ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.consol_invoice = consolInvoice;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, "SUM", new string[] { _Metas.V_Shipment_PAKComn.fn_pack_id_line_item_box_qty },
                            new ConditionCollection<_Metas.V_Shipment_PAKComn>(
                                new EqualCondition<_Metas.V_Shipment_PAKComn>(cond))
                            );
                    }
                }
                sqlCtx.Param(mtns::V_Shipment_PAKComn.fn_consol_invoice).Value = consolInvoice;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_DecimalWithMinusForNull(sqlR, sqlCtx.Indexes("SUM"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDnPrintList(DnPrintListInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Dn_Printlist>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::Dn_Printlist, DnPrintListInfo>(sqlCtx, item);

                sqlCtx.Param(mtns::Dn_Printlist.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMpBtOrder(MpBtOrderInfo item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<mtns::Mp_Btorder>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Mp_Btorder, MpBtOrderInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Mp_Btorder.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertFaSnobtdet(FaSnobtdetInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Fa_Snobtdet>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::Fa_Snobtdet, FaSnobtdetInfo>(sqlCtx, item);

                sqlCtx.Param(mtns::Fa_Snobtdet.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Fa_Snobtdet.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistVShipmentPakComnByConsolInvoiceOrShipmentAndRegion(string dn, string region)
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
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.region = region;
                        _Metas.V_Shipment_PAKComn cond2 = new _Metas.V_Shipment_PAKComn();
                        cond2.consol_invoice = dn;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, "COUNT", new string[] { _Metas.V_Shipment_PAKComn.fn_id }, new ConditionCollection<_Metas.V_Shipment_PAKComn>(
                            new EqualCondition<_Metas.V_Shipment_PAKComn>(cond),
                            new AnyCondition<_Metas.V_Shipment_PAKComn>(cond2, string.Format("({0}={1} OR {2}={1})", "{0}", "{1}", _Metas.V_Shipment_PAKComn.fn_shipment))));
                    }
                }
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_region).Value = region;
                sqlCtx.Param(g.DecAny(_Metas.V_Shipment_PAKComn.fn_consol_invoice)).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistVShipmentPakComnByInternalIDAndRegion(string internalId, string region)
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
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.region = region;
                        _Metas.V_Shipment_PAKComn cond2 = new _Metas.V_Shipment_PAKComn();
                        cond2.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, "COUNT", new string[] { _Metas.V_Shipment_PAKComn.fn_id }, new ConditionCollection<_Metas.V_Shipment_PAKComn>(
                            new EqualCondition<_Metas.V_Shipment_PAKComn>(cond),
                            new EqualCondition<_Metas.V_Shipment_PAKComn>(cond2, "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_region).Value = region;
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<string> GetConsolInvoiceFromVShipmentPakComn(string internalId)
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
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, null, new string[] { _Metas.V_Shipment_PAKComn.fn_consol_invoice }, new ConditionCollection<_Metas.V_Shipment_PAKComn>(new EqualCondition<_Metas.V_Shipment_PAKComn>(cond, "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.V_Shipment_PAKComn.fn_consol_invoice));
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

        public IList<string> GetInternalIdsFromVShipmentPakComnByConsolInvoice(string consolInvoice)
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
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.consol_invoice = consolInvoice;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, null, new string[] { _Metas.V_Shipment_PAKComn.fn_internalID }, new ConditionCollection<_Metas.V_Shipment_PAKComn>(new EqualCondition<_Metas.V_Shipment_PAKComn>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.V_Shipment_PAKComn.fn_consol_invoice).Value = consolInvoice;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.V_Shipment_PAKComn.fn_internalID));
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

        public decimal GetSumOfPackIdLineItemBoxQtyFromVShipmentPakComn(IList<string> internalIds)
        {
            try
            {
                decimal ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.V_Shipment_PAKComn cond = new _Metas.V_Shipment_PAKComn();
                        cond.internalID = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.V_Shipment_PAKComn>(tk, "SUM", new string[] { _Metas.V_Shipment_PAKComn.fn_pack_id_line_item_box_qty },
                            new ConditionCollection<_Metas.V_Shipment_PAKComn>(
                                new InSetCondition<_Metas.V_Shipment_PAKComn>(cond))
                            );
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.V_Shipment_PAKComn.fn_internalID), g.ConvertInSet(new List<string>(internalIds)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_DecimalWithMinusForNull(sqlR, sqlCtx.Indexes("SUM"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPakPackkingData(IList<string> internalIds)
        {
            try
            {
                int ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.internalID = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Packkingdata>(tk, "COUNT", new string[] { _Metas.Pak_Packkingdata.fn_id },
                            new ConditionCollection<_Metas.Pak_Packkingdata>(
                                new InSetCondition<_Metas.Pak_Packkingdata>(cond))
                            );
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Packkingdata.fn_internalID), g.ConvertInSet(new List<string>(internalIds)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPakPackkingDataWithDistinctSerialNum(IList<string> internalIds)
        {
            try
            {
                int ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.internalID = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Pak_Packkingdata>(tk, "COUNT", new string[][] { new string[]{_Metas.Pak_Packkingdata.fn_serial_num, string.Format("DISTINCT {0}",_Metas.Pak_Packkingdata.fn_serial_num) } },
                            new ConditionCollection<_Metas.Pak_Packkingdata>(
                                new InSetCondition<_Metas.Pak_Packkingdata>(cond))
                            );
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Packkingdata.fn_internalID), g.ConvertInSet(new List<string>(internalIds)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistFisToSpaWeight(string dnDivShipment)
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
                        _Metas.Fistosap_Weight cond = new _Metas.Fistosap_Weight();
                        cond._DNDIVShipment_ = dnDivShipment;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Fistosap_Weight>(tk, "COUNT", new string[] { _Metas.Fistosap_Weight.fn_id }, new ConditionCollection<_Metas.Fistosap_Weight>(new EqualCondition<_Metas.Fistosap_Weight>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Fistosap_Weight.fn__DNDIVShipment_).Value = dnDivShipment;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void CopyFisToSapWeightToPakShipmentWeightFis(string dnDivShipment)
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
                        Fistosap_Weight cond = new Fistosap_Weight();
                        cond._DNDIVShipment_ = dnDivShipment;

                        sqlCtx = FuncNew.GetConditionedForBackupInsert<Fistosap_Weight, Pak_Shipmentweight_Fis>(tk, 
                            new string[][]{
                                new string[]{Fistosap_Weight.fn__DNDIVShipment_, Pak_Shipmentweight_Fis.fn_shipment},
                                new string[]{Fistosap_Weight.fn_status, Pak_Shipmentweight_Fis.fn_type},
                                new string[]{string.Format("CONVERT(DECIMAL(10,1),{0})", Fistosap_Weight.fn_kg), Pak_Shipmentweight_Fis.fn_weight},
                                new string[]{Fistosap_Weight.fn_cdt, Pak_Shipmentweight_Fis.fn_cdt}
                            },
                            new ConditionCollection<Fistosap_Weight>(new EqualCondition<Fistosap_Weight>(cond)), _Schema.SqlHelper.DB_PAK);
                    }
                }
                sqlCtx.Param(Fistosap_Weight.fn__DNDIVShipment_).Value = dnDivShipment;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetInternalIdsFromPakDashPakComnByLikeInternalID(string internalIdLike)
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
                        _Metas.Pak_Pakcomn cond = new _Metas.Pak_Pakcomn();
                        cond.internalID = internalIdLike;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Pakcomn>(tk, null, new string[] { _Metas.Pak_Pakcomn.fn_internalID }, new ConditionCollection<_Metas.Pak_Pakcomn>(new EqualCondition<_Metas.Pak_Pakcomn>(cond, "LEFT({0},10)")));
                    }
                }
                sqlCtx.Param(_Metas.Pak_Pakcomn.fn_internalID).Value = internalIdLike;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Pak_Pakcomn.fn_internalID));
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

        public IList<PakSkuMasterWeightFisInfo> GetPakSkuMasterWeightFisListByLikeModel(string model)
        {
            try
            {
                IList<PakSkuMasterWeightFisInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Skumasterweight_Fis cond = new Pak_Skumasterweight_Fis();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Skumasterweight_Fis>(tk, null, null, new ConditionCollection<Pak_Skumasterweight_Fis>(new EqualCondition<Pak_Skumasterweight_Fis>(cond, "LEFT({0},4)", "LEFT({0},4)")));
                    }
                }
                sqlCtx.Param(Pak_Skumasterweight_Fis.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Skumasterweight_Fis, PakSkuMasterWeightFisInfo, PakSkuMasterWeightFisInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPackinglistRePrint(PackinglistRePrintInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Packinglist_RePrint>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Packinglist_RePrint, PackinglistRePrintInfo>(sqlCtx, item);
                
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetAverageWeightFromPakSkuMasterWeightFisByModel(string model)
        {
            try
            {
                decimal ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pak_Skumasterweight_Fis cond = new _Metas.Pak_Skumasterweight_Fis();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Pak_Skumasterweight_Fis>(tk, null, new string[][] { new string[] { _Metas.Pak_Skumasterweight_Fis.fn_weight, string.Format("CONVERT(DECIMAL(10,3),SUM({0})/COUNT(*))", _Metas.Pak_Skumasterweight_Fis.fn_weight) } },
                            new ConditionCollection<_Metas.Pak_Skumasterweight_Fis>(new EqualCondition<_Metas.Pak_Skumasterweight_Fis>(cond, "LEFT({0},4)", "LEFT({0},4)"),
                                new AnySoloCondition<_Metas.Pak_Skumasterweight_Fis>(cond, "SUBSTRING({0},10,2)<>'25'")));
                    }
                }
                sqlCtx.Param(mtns::Pak_Skumasterweight_Fis.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_DecimalWithMinusForNull(sqlR, sqlCtx.Indexes(_Metas.Pak_Skumasterweight_Fis.fn_weight));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistDnPrintList(string dn)
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
                        _Metas.Dn_Printlist cond = new _Metas.Dn_Printlist();
                        cond.dn = dn;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Dn_Printlist>(tk, "COUNT", new string[] { _Metas.Dn_Printlist.fn_id }, new ConditionCollection<_Metas.Dn_Printlist>(new EqualCondition<_Metas.Dn_Printlist>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Dn_Printlist.fn_dn).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistDnPrintList(string dn, string docCat)
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
                        _Metas.Dn_Printlist cond = new _Metas.Dn_Printlist();
                        cond.dn = dn;
                        cond.doc_cat = docCat;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Dn_Printlist>(tk, "COUNT", new string[] { _Metas.Dn_Printlist.fn_id }, new ConditionCollection<_Metas.Dn_Printlist>(new EqualCondition<_Metas.Dn_Printlist>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Dn_Printlist.fn_dn).Value = dn;
                sqlCtx.Param(_Metas.Dn_Printlist.fn_doc_cat).Value = docCat;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistModelInPakSkuMasterWeightFis(string model)
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
                        _Metas.Pak_Skumasterweight_Fis cond = new _Metas.Pak_Skumasterweight_Fis();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Skumasterweight_Fis>(tk, "COUNT", new string[] { _Metas.Pak_Skumasterweight_Fis.fn_id }, new ConditionCollection<_Metas.Pak_Skumasterweight_Fis>(new EqualCondition<_Metas.Pak_Skumasterweight_Fis>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pak_Skumasterweight_Fis.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<PizzaPart> GetPizzaPartsByValueLike(int valueLength, string valuePrefix)
        {
            try
            {
                IList<PizzaPart> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pizza_Part cond = new mtns::Pizza_Part();
                        cond.partSn = valuePrefix + "%";

                        mtns::Pizza_Part cond2 = new mtns::Pizza_Part();
                        cond2.partSn = "[LEN]";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pizza_Part>(tk, null, null, new ConditionCollection<mtns::Pizza_Part>(
                            new LikeCondition<mtns::Pizza_Part>(cond),
                            new EqualCondition<mtns::Pizza_Part>(cond2,"LEN({0})","CONVERT(INT,{0})")));
                    }
                }
                sqlCtx.Param(mtns::Pizza_Part.fn_partSn).Value = valuePrefix + "%";
                sqlCtx.Param(mtns::Pizza_Part.fn_partSn + "$1").Value = valueLength.ToString();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pizza_Part, PizzaPart, PizzaPart>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<OlymBatteryInfo> GetOlymBatteryListByHppn(string hppn)
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
                        mtns::OlymBattery cond = new mtns::OlymBattery();
                        cond.hppn = hppn;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::OlymBattery>(tk, null, null, new ConditionCollection<mtns::OlymBattery>(
                            new EqualCondition<mtns::OlymBattery>(cond)));
                    }
                }
                sqlCtx.Param(mtns::OlymBattery.fn_hppn).Value = hppn;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::OlymBattery, OlymBatteryInfo, OlymBatteryInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ACAdapMaintainInfo> GetACAdapMaintainListByAssemb(string assemb)
        {
            try
            {
                IList<ACAdapMaintainInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Acadapmaintain cond = new mtns::Acadapmaintain();
                        cond.assemb = assemb;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Acadapmaintain>(tk, null, null, new ConditionCollection<mtns::Acadapmaintain>(new EqualCondition<mtns::Acadapmaintain>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Acadapmaintain.fn_assemb).Value = assemb;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Acadapmaintain, ACAdapMaintainInfo, ACAdapMaintainInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<VPakComnInfo> GetVPakComnListByInternalIdOrConsolInvoiceOrWaybillNumber(string internalId, out int totalLength)
        {
            try
            {
                totalLength = 0;

                IList<VPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::V_PAKComn cond = new mtns::V_PAKComn();
                        cond.internalID = internalId;
                        mtns::V_PAKComn cond2 = new mtns::V_PAKComn();
                        cond2.consol_invoice = internalId;
                        cond2.waybill_number = internalId;

                        ConditionCollection<mtns::V_PAKComn> condSet = new ConditionCollection<V_PAKComn>(false);
                        condSet.AddRange(
                            new EqualCondition<mtns::V_PAKComn>(cond, "LEFT({0},10)"),
                            new EqualCondition<mtns::V_PAKComn>(cond2));
 
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::V_PAKComn>(tk, null, new string[]{mtns::V_PAKComn.fn_internalID, mtns::V_PAKComn.fn_consol_invoice, mtns::V_PAKComn.fn_waybill_number, mtns::V_PAKComn.fn_intl_carrier}, condSet);
                    }
                }
                sqlCtx.Param(mtns::V_PAKComn.fn_internalID).Value = internalId;
                sqlCtx.Param(mtns::V_PAKComn.fn_consol_invoice).Value = internalId;
                sqlCtx.Param(mtns::V_PAKComn.fn_waybill_number).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<VPakComnInfo>();

                        while (sqlR.Read())
                        {
                            if (totalLength < 1000)
                            {
                                VPakComnInfo item = new VPakComnInfo();
                                item.internalID = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_PAKComn.fn_internalID));
                                item.consol_invoice = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_PAKComn.fn_consol_invoice));
                                item.intl_carrier = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_PAKComn.fn_intl_carrier));
                                item.waybill_number = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_PAKComn.fn_waybill_number));
                                ret.Add(item);
                            }
                            totalLength++;
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

        public void DeletePakDashPakComn(string internalId)
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
                        mtns::Pak_Pakcomn cond = new mtns::Pak_Pakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pak_Pakcomn>(tk, new ConditionCollection<mtns::Pak_Pakcomn>(new EqualCondition<mtns::Pak_Pakcomn>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_internalID).Value = internalId;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakDotPakComn(string internalId)
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
                        mtns::PakDotpakcomn cond = new mtns::PakDotpakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PakDotpakcomn>(tk, new ConditionCollection<mtns::PakDotpakcomn>(new EqualCondition<mtns::PakDotpakcomn>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PakDotpakcomn.fn_internalID).Value = internalId;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakDotPakPaltno(string internalId)
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
                        mtns::PakDotpakpaltno cond = new mtns::PakDotpakpaltno();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PakDotpakpaltno>(tk, new ConditionCollection<mtns::PakDotpakpaltno>(new EqualCondition<mtns::PakDotpakpaltno>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PakDotpakpaltno.fn_internalID).Value = internalId;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletetPakSkuMasterWeightFisByModel(string model)
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
                        mtns::Pak_Skumasterweight_Fis cond = new mtns::Pak_Skumasterweight_Fis();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pak_Skumasterweight_Fis>(tk, new ConditionCollection<mtns::Pak_Skumasterweight_Fis>(new EqualCondition<mtns::Pak_Skumasterweight_Fis>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Skumasterweight_Fis.fn_model).Value = model;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPakPqcLog(PakPqclogInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Pak_Pqclog>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Pak_Pqclog, PakPqclogInfo>(sqlCtx, item);
                
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Pak_Pqclog.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistMpBtOrder(string pno, int minDiff)
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
                        mtns::Mp_Btorder cond = new mtns::Mp_Btorder();
                        cond.pno = pno;

                        mtns::Mp_Btorder cond2 = new mtns::Mp_Btorder();
                        cond2.qty = minDiff;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mp_Btorder>(tk, "COUNT", new string[] { mtns::Mp_Btorder.fn_id }, new ConditionCollection<mtns::Mp_Btorder>(new EqualCondition<mtns::Mp_Btorder>(cond),
                            new GreaterCondition<mtns::Mp_Btorder>(cond2, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty))));
                    }
                }
                sqlCtx.Param(mtns::Mp_Btorder.fn_pno).Value = pno;
                sqlCtx.Param(g.DecG(mtns::Mp_Btorder.fn_qty)).Value = minDiff;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public IList<MpBtOrderInfo> GetMpBtOrderInfoList(string pno, int minDiff)
        {
            try
            {
                IList<MpBtOrderInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mp_Btorder cond = new mtns::Mp_Btorder();
                        cond.pno = pno;

                        mtns::Mp_Btorder cond2 = new mtns::Mp_Btorder();
                        cond2.qty = minDiff;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mp_Btorder>(tk, null, null, new ConditionCollection<mtns::Mp_Btorder>(new EqualCondition<mtns::Mp_Btorder>(cond),
                            new GreaterCondition<mtns::Mp_Btorder>(cond2, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty))), mtns::Mp_Btorder.fn_ref_date + FuncNew.DescendOrder, mtns::Mp_Btorder.fn_shipDate + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::Mp_Btorder.fn_pno).Value = pno;
                sqlCtx.Param(g.DecG(mtns::Mp_Btorder.fn_qty)).Value = minDiff;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mp_Btorder, MpBtOrderInfo, MpBtOrderInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertMpBtOrder(MpBtOrderInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Mp_Btorder>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Mp_Btorder, MpBtOrderInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Mp_Btorder.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Mp_Btorder.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateForIncreasePrtQty(string bt, string pno, int count)
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
                        Mp_Btorder cond = new Mp_Btorder();
                        cond.bt = bt;
                        cond.pno = pno;

                        Mp_Btorder setv = new Mp_Btorder();
                        setv.prtQty = count;

                        Mp_Btorder setv2 = new Mp_Btorder();
                        setv2.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Mp_Btorder>(tk, new SetValueCollection<Mp_Btorder>(new ForIncSetValue<Mp_Btorder>(setv), new CommonSetValue<Mp_Btorder>(setv2)), new ConditionCollection<Mp_Btorder>(new EqualCondition<Mp_Btorder>(cond)));
                    }
                }
                sqlCtx.Param(Mp_Btorder.fn_bt).Value = bt;
                sqlCtx.Param(Mp_Btorder.fn_pno).Value = pno;

                sqlCtx.Param(g.DecInc(Mp_Btorder.fn_prtQty)).Value = count;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Mp_Btorder.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistMpBtOrder(string pno, int minDiff, int maxdiff)
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
                        mtns::Mp_Btorder cond = new mtns::Mp_Btorder();
                        cond.pno = pno;

                        mtns::Mp_Btorder cond2 = new mtns::Mp_Btorder();
                        cond2.qty = minDiff;

                        mtns::Mp_Btorder cond3 = new mtns::Mp_Btorder();
                        cond3.qty = maxdiff;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mp_Btorder>(tk, "COUNT", new string[] { mtns::Mp_Btorder.fn_id }, new ConditionCollection<mtns::Mp_Btorder>(new EqualCondition<mtns::Mp_Btorder>(cond),
                            new GreaterCondition<mtns::Mp_Btorder>(cond2, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty)),
                            new SmallerCondition<mtns::Mp_Btorder>(cond3, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty))));
                    }
                }
                sqlCtx.Param(mtns::Mp_Btorder.fn_pno).Value = pno;
                sqlCtx.Param(g.DecG(mtns::Mp_Btorder.fn_qty)).Value = minDiff;
                sqlCtx.Param(g.DecS(mtns::Mp_Btorder.fn_qty)).Value = maxdiff;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public int GetCountofMpBtOrder(string pno, int minDiff, int maxdiff)
        {
            try
            {
                int ret = 0;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mp_Btorder cond = new mtns::Mp_Btorder();
                        cond.pno = pno;

                        mtns::Mp_Btorder cond2 = new mtns::Mp_Btorder();
                        cond2.qty = minDiff;

                        mtns::Mp_Btorder cond3 = new mtns::Mp_Btorder();
                        cond3.qty = maxdiff;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mp_Btorder>(tk, "COUNT", new string[] { mtns::Mp_Btorder.fn_id }, new ConditionCollection<mtns::Mp_Btorder>(new EqualCondition<mtns::Mp_Btorder>(cond),
                            new GreaterCondition<mtns::Mp_Btorder>(cond2, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty)),
                            new SmallerCondition<mtns::Mp_Btorder>(cond3, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty))));
                    }
                }
                sqlCtx.Param(mtns::Mp_Btorder.fn_pno).Value = pno;
                sqlCtx.Param(g.DecG(mtns::Mp_Btorder.fn_qty)).Value = minDiff;
                sqlCtx.Param(g.DecS(mtns::Mp_Btorder.fn_qty)).Value = maxdiff;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MpBtOrderInfo> GetMpBtOrderInfoList(string pno, int minDiff, int maxdiff)
        {
            try
            {
                IList<MpBtOrderInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mp_Btorder cond = new mtns::Mp_Btorder();
                        cond.pno = pno;

                        mtns::Mp_Btorder cond2 = new mtns::Mp_Btorder();
                        cond2.qty = minDiff;

                        mtns::Mp_Btorder cond3 = new mtns::Mp_Btorder();
                        cond3.qty = maxdiff;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mp_Btorder>(tk, null, null, new ConditionCollection<mtns::Mp_Btorder>(new EqualCondition<mtns::Mp_Btorder>(cond),
                            new GreaterCondition<mtns::Mp_Btorder>(cond2, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty)),
                            new SmallerCondition<mtns::Mp_Btorder>(cond3, string.Format("{0} - {1}", "{0}", mtns::Mp_Btorder.fn_prtQty))), mtns::Mp_Btorder.fn_ref_date + FuncNew.DescendOrder, mtns::Mp_Btorder.fn_shipDate + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::Mp_Btorder.fn_pno).Value = pno;
                sqlCtx.Param(g.DecG(mtns::Mp_Btorder.fn_qty)).Value = minDiff;
                sqlCtx.Param(g.DecS(mtns::Mp_Btorder.fn_qty)).Value = maxdiff;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mp_Btorder, MpBtOrderInfo, MpBtOrderInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPakDotPakComn(PakDotPakComnInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::PakDotpakcomn>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::PakDotpakcomn, PakDotPakComnInfo>(sqlCtx, item);

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPakDashPakComn(PakDashPakComnInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Pak_Pakcomn>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Pak_Pakcomn, PakDashPakComnInfo>(sqlCtx, item);
                
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakDotPakComn(string internalId)
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
                        _Metas.PakDotpakcomn cond = new _Metas.PakDotpakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PakDotpakcomn>(tk, "COUNT", new string[] { _Metas.PakDotpakcomn.fn_id }, new ConditionCollection<_Metas.PakDotpakcomn>(new EqualCondition<_Metas.PakDotpakcomn>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PakDotpakcomn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistPakDashPakComn(string internalId)
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
                        _Metas.Pak_Pakcomn cond = new _Metas.Pak_Pakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Pakcomn>(tk, "COUNT", new string[] { _Metas.Pak_Pakcomn.fn_id }, new ConditionCollection<_Metas.Pak_Pakcomn>(new EqualCondition<_Metas.Pak_Pakcomn>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pak_Pakcomn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistPakDotPakPaltno(string internalId)
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
                        _Metas.PakDotpakpaltno cond = new _Metas.PakDotpakpaltno();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PakDotpakpaltno>(tk, "COUNT", new string[] { _Metas.PakDotpakpaltno.fn_id }, new ConditionCollection<_Metas.PakDotpakpaltno>(new EqualCondition<_Metas.PakDotpakpaltno>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PakDotpakpaltno.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void AddPakDotPakPaltnoInfo(PakDotPakPaltnoInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::PakDotpakpaltno>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::PakDotpakpaltno, PakDotPakPaltnoInfo>(sqlCtx, item);
                
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public fons.PizzaStatus GetPizzaStatus(string pizza_id)
        {
            try
            {
                fons.PizzaStatus ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PizzaStatus cond = new mtns::PizzaStatus();
                        cond.pizzaID = pizza_id;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PizzaStatus>(tk, "TOP 1", null, new ConditionCollection<mtns::PizzaStatus>(
                            new EqualCondition<mtns::PizzaStatus>(cond)), mtns::PizzaStatus.fn_udt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::PizzaStatus.fn_pizzaID).Value = pizza_id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::PizzaStatus, fons.PizzaStatus>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartSnPrefixesFromPizzaPart(string[] partSnPrefixes, int lenOfPartSn)
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
                        mtns::Pizza_Part cond = new mtns::Pizza_Part();
                        cond.partSn = lenOfPartSn.ToString();
                        mtns::Pizza_Part cond2 = new mtns::Pizza_Part();
                        cond2.partSn = "[INSET LIKE]";

                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns::Pizza_Part>(tk, "DISTINCT", new string[][] { new string[] { mtns::Pizza_Part.fn_partSn, string.Format("LEFT({0},5)", mtns::Pizza_Part.fn_partSn) } },
                            new ConditionCollection<mtns::Pizza_Part>(
                                new EqualCondition<mtns::Pizza_Part>(cond, "LEN(RTRIM(PartSn))", "CONVERT(INT,{0})"),
                                new AnyCondition<mtns::Pizza_Part>(cond2)));
                    }
                }
                sqlCtx.Param(mtns::Pizza_Part.fn_partSn).Value = lenOfPartSn.ToString();
                string Sentence = sqlCtx.Sentence.Replace(g.GetCertainConditionSegment(sqlCtx.Sentence, g.DecAny(mtns::Pizza_Part.fn_partSn)), g.GetIteratedString(partSnPrefixes, string.Format("{0} LIKE '[LIKE]%'", mtns::Pizza_Part.fn_partSn), "[LIKE]", " OR "));
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pizza_Part.fn_partSn));
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

        public IList<ACAdapMaintainInfo> GetACAdapMaintainList(string[] assemb)
        {
            try
            {
                IList<ACAdapMaintainInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Acadapmaintain cond = new mtns::Acadapmaintain();
                        cond.assemb = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Acadapmaintain>(tk, null, null, new ConditionCollection<mtns::Acadapmaintain>(
                            new InSetCondition<mtns::Acadapmaintain>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Acadapmaintain.fn_assemb), g.ConvertInSet(assemb));
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Acadapmaintain, ACAdapMaintainInfo, ACAdapMaintainInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int[] GetMinAndMaxIdOfPakEdiInstr(string poNum, string fields, string poItem)
        {
            try
            {
                int[] ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pakedi_Instr cond = new _Metas.Pakedi_Instr();
                        cond.po_num = poNum;
                        cond.fields = fields;
                        cond.po_item = poItem;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Pakedi_Instr>(tk, null, new string[][] { 
                            new string[] { _Metas.Pakedi_Instr.fn_id, string.Format("MAX({0})", _Metas.Pakedi_Instr.fn_id) },
                            new string[] { _Metas.Pakedi_Instr.fn_id, string.Format("MIN({0})", _Metas.Pakedi_Instr.fn_id) }},
                            new ConditionCollection<_Metas.Pakedi_Instr>(new EqualCondition<_Metas.Pakedi_Instr>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pakedi_Instr.fn_po_num).Value = poNum;
                sqlCtx.Param(mtns::Pakedi_Instr.fn_fields).Value = fields;
                sqlCtx.Param(mtns::Pakedi_Instr.fn_po_item).Value = poItem;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new int[]
                        {
                            g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pakedi_Instr.fn_id)),
                            g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pakedi_Instr.fn_id + "_2")),
                        };
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakediInstrInfo> GetPakediInstrInfoListByPoNumAndId(string poNum, int id)
        {
            try
            {
                IList<PakediInstrInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pakedi_Instr cond = new mtns::Pakedi_Instr();
                        cond.po_num = poNum;
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pakedi_Instr>(tk, null, null, new ConditionCollection<mtns::Pakedi_Instr>(
                            new EqualCondition<mtns::Pakedi_Instr>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pakedi_Instr.fn_po_num).Value = poNum;
                sqlCtx.Param(mtns::Pakedi_Instr.fn_id).Value = id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pakedi_Instr, PakediInstrInfo, PakediInstrInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakediInstrByPoNumAndIdAndValue(string poNum, int id)
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
                        _Metas.Pakedi_Instr cond = new _Metas.Pakedi_Instr();
                        cond.po_num = poNum;
                        cond.id = id;
                        _Metas.Pakedi_Instr cond2 = new _Metas.Pakedi_Instr();
                        cond2.value = "1 N";
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pakedi_Instr>(tk, "COUNT", new string[] { _Metas.Pakedi_Instr.fn_id }, new ConditionCollection<_Metas.Pakedi_Instr>(
                            new EqualCondition<_Metas.Pakedi_Instr>(cond),
                            new AnyCondition<_Metas.Pakedi_Instr>(cond2, "CHARINDEX({1},{0})>0")));

                        sqlCtx.Param(g.DecAny(_Metas.Pakedi_Instr.fn_value)).Value = cond2.value;
                    }
                }
                sqlCtx.Param(_Metas.Pakedi_Instr.fn_po_num).Value = poNum;
                sqlCtx.Param(_Metas.Pakedi_Instr.fn_id).Value = id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<PakDashPakComnInfo> GetPakDashPakComnListByInternalIDAndOrderTypeAndRegions(string internalId, string orderType, string[] regions)
        {
            try
            {
                IList<PakDashPakComnInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pak_Pakcomn cond = new mtns::Pak_Pakcomn();
                        cond.region = "[INSET]";
                        mtns::Pak_Pakcomn cond2 = new mtns::Pak_Pakcomn();
                        cond2.internalID = internalId;
                        cond2.order_type = orderType;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Pakcomn>(tk, null, null, new ConditionCollection<mtns::Pak_Pakcomn>(
                            new InSetCondition<mtns::Pak_Pakcomn>(cond),
                            new EqualCondition<mtns::Pak_Pakcomn>(cond2)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_internalID).Value = internalId;
                sqlCtx.Param(mtns::Pak_Pakcomn.fn_order_type).Value = orderType;
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Pakcomn.fn_region), g.ConvertInSet(regions));
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pak_Pakcomn, PakDashPakComnInfo, PakDashPakComnInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetProdDescBaseFromPakDotPakEdi850raw(string poNum)
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
                        mtns::PakDotpakedi850raw cond = new mtns::PakDotpakedi850raw();
                        cond.po_num = poNum;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PakDotpakedi850raw>(tk, "DISTINCT", new string[] { mtns::PakDotpakedi850raw.fn_prod_desc_base }, new ConditionCollection<mtns::PakDotpakedi850raw>(new EqualCondition<mtns::PakDotpakedi850raw>(cond)), mtns::PakDotpakedi850raw.fn_prod_desc_base);
                    }
                }
                sqlCtx.Param(mtns::PakDotpakedi850raw.fn_po_num).Value = poNum;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PakDotpakedi850raw.fn_prod_desc_base));
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

        public IList<PizzaPart> GetPizzaPartsByValueLike(int valueLength, string valuePrefix, string pizzaId)
        {
            try
            {
                IList<PizzaPart> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pizza_Part cond = new mtns::Pizza_Part();
                        cond.partSn = valuePrefix + "%";

                        mtns::Pizza_Part cond2 = new mtns::Pizza_Part();
                        cond2.partSn = "[LEN]";

                        mtns::Pizza_Part cond3 = new mtns::Pizza_Part();
                        cond3.pizzaID = pizzaId;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pizza_Part>(tk, null, null, new ConditionCollection<mtns::Pizza_Part>(
                            new LikeCondition<mtns::Pizza_Part>(cond),
                            new EqualCondition<mtns::Pizza_Part>(cond2,"LEN({0})","CONVERT(INT,{0})"),
                            new EqualCondition<mtns::Pizza_Part>(cond3)));
                    }
                }
                sqlCtx.Param(mtns::Pizza_Part.fn_partSn).Value = valuePrefix + "%";
                sqlCtx.Param(mtns::Pizza_Part.fn_partSn + "$1").Value = valueLength.ToString();
                sqlCtx.Param(mtns::Pizza_Part.fn_pizzaID).Value = pizzaId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pizza_Part, PizzaPart, PizzaPart>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDocSetNumListFromPakDashPakComnByInternalID(string internalId)
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
                        mtns::PakDotpakcomn cond = new mtns::PakDotpakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PakDotpakcomn>(tk, "DISTINCT", new string[] { mtns::PakDotpakcomn.fn_doc_set_number }, new ConditionCollection<mtns::PakDotpakcomn>(new EqualCondition<mtns::PakDotpakcomn>(cond)), mtns::PakDotpakcomn.fn_doc_set_number);
                    }
                }
                sqlCtx.Param(mtns::PakDotpakcomn.fn_internalID).Value = internalId;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PakDotpakcomn.fn_doc_set_number));
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

        public bool CheckExistMeidLogByPltAndStringIdValue(string palletId, string stringIdValue)
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
                        _Metas.Meid_Log cond = new _Metas.Meid_Log();
                        cond.pallet_id = palletId;
                        cond.stringIDValue = stringIdValue;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Meid_Log>(tk, "COUNT", new string[] { _Metas.Meid_Log.fn_id }, new ConditionCollection<_Metas.Meid_Log>(
                            new EqualCondition<_Metas.Meid_Log>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Meid_Log.fn_pallet_id).Value = palletId;
                sqlCtx.Param(_Metas.Meid_Log.fn_stringIDValue).Value = stringIdValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistMeidLogByPltAndIsPass(string palletId, short isPass)
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
                        _Metas.Meid_Log cond = new _Metas.Meid_Log();
                        cond.pallet_id = palletId;
                        cond.isPass = isPass;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Meid_Log>(tk, "COUNT", new string[] { _Metas.Meid_Log.fn_id }, new ConditionCollection<_Metas.Meid_Log>(
                            new EqualCondition<_Metas.Meid_Log>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Meid_Log.fn_pallet_id).Value = palletId;
                sqlCtx.Param(_Metas.Meid_Log.fn_isPass).Value = isPass;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void UpdateMeidLogIsPassByPalletIdAndStringIDValue(short isPass, string palletId, string stringIdValue)
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
                        Meid_Log cond = new Meid_Log();
                        cond.pallet_id = palletId;
                        cond.stringIDValue = stringIdValue;
                        Meid_Log setv = new Meid_Log();
                        setv.isPass = isPass;
                        sqlCtx = FuncNew.GetConditionedUpdate<Meid_Log>(tk, new SetValueCollection<Meid_Log>(new CommonSetValue<Meid_Log>(setv)), new ConditionCollection<Meid_Log>(new EqualCondition<Meid_Log>(cond)));
                    }
                }
                sqlCtx.Param(Meid_Log.fn_pallet_id).Value = palletId;
                sqlCtx.Param(Meid_Log.fn_stringIDValue).Value = stringIdValue;
                sqlCtx.Param(g.DecSV(Meid_Log.fn_isPass)).Value = isPass;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakPackkingDataBySerialNum(string serialNum)
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
                        _Metas.Pak_Packkingdata cond = new _Metas.Pak_Packkingdata();
                        cond.serial_num = serialNum;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Packkingdata>(tk, "COUNT", new string[] { _Metas.Pak_Packkingdata.fn_id }, new ConditionCollection<_Metas.Pak_Packkingdata>(
                            new EqualCondition<_Metas.Pak_Packkingdata>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pak_Packkingdata.fn_serial_num).Value = serialNum;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void InsertSapWeight(SapWeightInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Sap_Weight>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Sap_Weight, SapWeightInfo>(sqlCtx, item);
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDescrsFromLocalMaintainByTpAndCode(string tp, string code)
        {
            throw new NotImplementedException();
        }

        public void UpdateSapWeight(decimal addingAmount, SapWeightInfo condition)
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
                Sap_Weight cond = FuncNew.SetColumnFromField<Sap_Weight, SapWeightInfo>(condition);
                Sap_Weight setv = new Sap_Weight();
                setv.kg = addingAmount;

                sqlCtx = FuncNew.GetConditionedUpdate<Sap_Weight>(new SetValueCollection<Sap_Weight>(new ForIncSetValue<Sap_Weight>(setv)), new ConditionCollection<Sap_Weight>(new EqualCondition<Sap_Weight>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Sap_Weight, SapWeightInfo>(sqlCtx, condition);
                sqlCtx.Param(g.DecInc(Sap_Weight.fn_kg)).Value = addingAmount;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakShipmentWeightFisWithShipmentInSapWeight()
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
                        mtns::Pak_Shipmentweight_Fis cond = new mtns::Pak_Shipmentweight_Fis();
                        cond.shipment = "shipment";
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pak_Shipmentweight_Fis>(tk, new ConditionCollection<mtns::Pak_Shipmentweight_Fis>(new AnySoloCondition<mtns::Pak_Shipmentweight_Fis>(cond, string.Format("{0} IN (SELECT {1} FROM {2}..{3})", "{0}", mtns.Sap_Weight.fn__DNDIVShipment_, _Schema.SqlHelper.DB_PAK, ToolsNew.GetTableName(typeof(mtns.Sap_Weight))))));
                    }
                }
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMpBtOrder(MpBtOrderInfo setValue, MpBtOrderInfo condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Mp_Btorder cond = mtns::FuncNew.SetColumnFromField<mtns::Mp_Btorder, MpBtOrderInfo>(condition);
                mtns::Mp_Btorder setv = mtns::FuncNew.SetColumnFromField<mtns::Mp_Btorder, MpBtOrderInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Mp_Btorder>(new mtns::SetValueCollection<mtns::Mp_Btorder>(new mtns::CommonSetValue<mtns::Mp_Btorder>(setv)), new mtns::ConditionCollection<mtns::Mp_Btorder>(new mtns::EqualCondition<mtns::Mp_Btorder>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Mp_Btorder, MpBtOrderInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Mp_Btorder, MpBtOrderInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Mp_Btorder.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistMpBtOrder(string pno)
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
                        _Metas.Mp_Btorder cond = new _Metas.Mp_Btorder();
                        cond.pno = pno;
                        _Metas.Mp_Btorder cond2 = new _Metas.Mp_Btorder();
                        cond2.qty = 0;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Mp_Btorder>(tk, "COUNT", new string[] { _Metas.Mp_Btorder.fn_id }, new ConditionCollection<_Metas.Mp_Btorder>(
                            new EqualCondition<_Metas.Mp_Btorder>(cond),
                            new AnySoloCondition<_Metas.Mp_Btorder>(cond2, string.Format("{0}>{1}", "{0}", _Metas.Mp_Btorder.fn_prtQty))));
                    }
                }
                sqlCtx.Param(_Metas.Mp_Btorder.fn_pno).Value = pno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void UpdateForIncreasePrtQty(MpBtOrderInfo condition, int count)
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
                        Mp_Btorder cond = FuncNew.SetColumnFromField<Mp_Btorder, MpBtOrderInfo>(condition);

                        Mp_Btorder setv = new Mp_Btorder();
                        setv.prtQty = count;

                        Mp_Btorder setv2 = new Mp_Btorder();
                        setv2.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Mp_Btorder>(new SetValueCollection<Mp_Btorder>(new ForIncSetValue<Mp_Btorder>(setv), new CommonSetValue<Mp_Btorder>(setv2)), new ConditionCollection<Mp_Btorder>(new EqualCondition<Mp_Btorder>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Mp_Btorder, MpBtOrderInfo>(sqlCtx, condition);

                sqlCtx.Param(g.DecInc(Mp_Btorder.fn_prtQty)).Value = count;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Mp_Btorder.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MpBtOrderInfo> GetMpBtOrderInfoList(string pno)
        {
            try
            {
                IList<MpBtOrderInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mp_Btorder cond = new mtns::Mp_Btorder();
                        cond.pno = pno;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mp_Btorder>(tk, null, null, new ConditionCollection<mtns::Mp_Btorder>(new EqualCondition<mtns::Mp_Btorder>(cond)), mtns::Mp_Btorder.fn_ref_date + FuncNew.DescendOrder, mtns::Mp_Btorder.fn_shipDate + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::Mp_Btorder.fn_pno).Value = pno;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mp_Btorder, MpBtOrderInfo, MpBtOrderInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BackUpPizzaStatus(string pizzaId, string uEditor)
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
                        _Metas.PizzaStatus cond = new _Metas.PizzaStatus();
                        cond.pizzaID = pizzaId;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.PizzaStatus, _Metas.UnpackPizzaStatus>(tk,
                            new string[][]{
                                new string[]{_Metas.PizzaStatus.fn_cdt,_Metas.UnpackPizzaStatus.fn_cdt},
                                new string[]{_Metas.PizzaStatus.fn_editor,_Metas.UnpackPizzaStatus.fn_editor},
                                new string[]{_Metas.PizzaStatus.fn_line,_Metas.UnpackPizzaStatus.fn_line},
                                new string[]{_Metas.PizzaStatus.fn_pizzaID,_Metas.UnpackPizzaStatus.fn_pizzaID},
                                new string[]{_Metas.PizzaStatus.fn_station,_Metas.UnpackPizzaStatus.fn_station},
                                new string[]{_Metas.PizzaStatus.fn_udt,_Metas.UnpackPizzaStatus.fn_udt},
                                new string[]{string.Format("@{0}",_Metas.UnpackPizzaStatus.fn_ueditor), _Metas.UnpackPizzaStatus.fn_ueditor},
                                new string[]{"GETDATE()",_Metas.UnpackPizzaStatus.fn_updt}
                            },
                            new ConditionCollection<_Metas.PizzaStatus>(new EqualCondition<_Metas.PizzaStatus>(cond)));
                        sqlCtx.AddParam(_Metas.UnpackPizzaStatus.fn_ueditor, new SqlParameter(string.Format("@{0}", _Metas.UnpackPizzaStatus.fn_ueditor), ToolsNew.GetDBFieldType<_Metas.UnpackPizzaStatus>(_Metas.UnpackPizzaStatus.fn_ueditor)));
                    }
                }
                sqlCtx.Param(_Metas.PizzaStatus.fn_pizzaID).Value = pizzaId;
                sqlCtx.Param(_Metas.UnpackPizzaStatus.fn_ueditor).Value = uEditor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable CallTemplateCheckLaNew(string dn, string docType)
        {
            try
            {
                DataTable ret = null;
                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@DN", SqlDbType.NVarChar);
                paramsArray[0].Value = dn;
                paramsArray[1] = new SqlParameter("@doctpye", SqlDbType.NVarChar);
                paramsArray[1].Value = docType;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.StoredProcedure, "op_TemplateCheck_LANEW", paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartNoListFromPizzaPart(string[] pizzaIds, string bomNodeType, string infoType, string infoValue)
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
                        tf1 = new TableAndFields<Pizza_Part>();
                        Pizza_Part cond = new Pizza_Part();
                        cond.pizzaID = "[INSET]";
                        tf1.Conditions.Add(new InSetCondition<Pizza_Part>(cond));
                        tf1.AddRangeToGetFieldNames( Pizza_Part.fn_partSn );

                        tf2 = new TableAndFields<Part_NEW>();
                        Part_NEW cond2 = new Part_NEW();
                        cond2.bomNodeType = bomNodeType;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<Part_NEW>(cond2));
                        tf2.ClearToGetFieldNames();
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_BOM;

                        tf3 = new TableAndFields<mtns.PartInfo>();
                        mtns.PartInfo cond3 = new mtns.PartInfo();
                        cond3.infoType = infoType;
                        cond3.infoValue = infoValue;
                        tf3.Conditions.Add(new EqualCondition<mtns.PartInfo>(cond3));
                        tf3.ClearToGetFieldNames();
                        tf3.SubDBCalalog = _Schema.SqlHelper.DB_BOM;

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Pizza_Part, Part_NEW>(tf1, Pizza_Part.fn_partNo, tf2, Part_NEW.fn_partNo),
                            new TableConnectionItem<Pizza_Part, mtns.PartInfo>(tf1, Pizza_Part.fn_partNo, tf3, mtns.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t1." + Pizza_Part.fn_partSn);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, mtns.Part_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf2.Alias, mtns.Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, mtns.PartInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, mtns.PartInfo.fn_infoValue)).Value = infoValue;

                string Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias, g.DecInSet(Pizza_Part.fn_pizzaID)), g.ConvertInSet(pizzaIds));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, Pizza_Part.fn_partSn)));
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

        public void DeletePakDashPakComn(IList<string> prefixList)
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
                        mtns::Pak_Pakcomn cond = new mtns::Pak_Pakcomn();
                        cond.internalID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pak_Pakcomn>(tk, new ConditionCollection<mtns::Pak_Pakcomn>(new InSetCondition<mtns::Pak_Pakcomn>(cond, "LEFT({0}, 10)")));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pak_Pakcomn.fn_internalID), g.ConvertInSet(prefixList));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakDotPakComn(IList<string> prefixList)
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
                        mtns::PakDotpakcomn cond = new mtns::PakDotpakcomn();
                        cond.internalID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PakDotpakcomn>(tk, new ConditionCollection<mtns::PakDotpakcomn>(new InSetCondition<mtns::PakDotpakcomn>(cond, "LEFT({0}, 10)")));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PakDotpakcomn.fn_internalID), g.ConvertInSet(prefixList));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakDotPakPaltno(IList<string> prefixList)
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
                        mtns::PakDotpakpaltno cond = new mtns::PakDotpakpaltno();
                        cond.internalID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PakDotpakpaltno>(tk, new ConditionCollection<mtns::PakDotpakpaltno>(new InSetCondition<mtns::PakDotpakpaltno>(cond, "LEFT({0}, 10)")));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PakDotpakpaltno.fn_internalID), g.ConvertInSet(prefixList));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePizzaPart(PizzaPart condition)
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
                Pizza_Part cond = FuncNew.SetColumnFromField<Pizza_Part, PizzaPart>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Pizza_Part>(new ConditionCollection<Pizza_Part>(new EqualCondition<Pizza_Part>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pizza_Part, PizzaPart>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void InsertForBackupPakPackkingDataDefered(IUnitOfWork uow, IList<string> internalIds)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), internalIds);
        }

        public void DeletePakPackkingDataDefered(IUnitOfWork uow, IList<string> internalIds)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), internalIds);
        }

        public void InsertPakPackkingDataDefered(IUnitOfWork uow, IList<PakPackkingDataInfo> items)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), items);
        }

        public void InsertPakSkuMasterWeightFisDefered(IUnitOfWork uow, PakSkuMasterWeightFisInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertPakShipmentWeightFisDefered(IUnitOfWork uow, PakShipmentWeightFisInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertDnPrintListDefered(IUnitOfWork uow, DnPrintListInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateMpBtOrderDefered(IUnitOfWork uow, MpBtOrderInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertFaSnobtdetDefered(IUnitOfWork uow, FaSnobtdetInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void CopyFisToSapWeightToPakShipmentWeightFisDefered(IUnitOfWork uow, string dnDivShipment)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dnDivShipment);
        }

        public void InsertPackinglistRePrintDefered(IUnitOfWork uow, PackinglistRePrintInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePakDashPakComnDefered(IUnitOfWork uow, string internalId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), internalId);
        }

        public void DeletePakDotPakComnDefered(IUnitOfWork uow, string internalId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), internalId);
        }

        public void DeletePakDotPakPaltnoDefered(IUnitOfWork uow, string internalId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), internalId);
        }

        public void DeletetPakSkuMasterWeightFisByModelDefered(IUnitOfWork uow, string model)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model);
        }

        public void InsertPakPqcLogDefered(IUnitOfWork uow, PakPqclogInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertMpBtOrderDefered(IUnitOfWork uow, MpBtOrderInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateForIncreasePrtQtyDefered(IUnitOfWork uow, string bt, string pno, int count)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), bt, pno, count);
        }

        public void AddPakDotPakComnDefered(IUnitOfWork uow, PakDotPakComnInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPakDashPakComnDefered(IUnitOfWork uow, PakDashPakComnInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPakDotPakPaltnoInfoDefered(IUnitOfWork uow, PakDotPakPaltnoInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateMeidLogIsPassByPalletIdAndStringIDValueDefered(IUnitOfWork uow, short isPass, string palletId, string stringIdValue)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), isPass, palletId, stringIdValue);
        }

        public void InsertSapWeightDefered(IUnitOfWork uow, SapWeightInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateSapWeightDefered(IUnitOfWork uow, decimal addingAmount, SapWeightInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), addingAmount, condition);
        }

        public void DeletePakShipmentWeightFisWithShipmentInSapWeightDefered(IUnitOfWork uow)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod());
        }

        public void UpdateMpBtOrderDefered(IUnitOfWork uow, MpBtOrderInfo setValue, MpBtOrderInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateForIncreasePrtQtyDefered(IUnitOfWork uow, int id, int count)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id, count);
        }
        
        public void BackUpPizzaStatusDefered(IUnitOfWork uow, string pizzaId, string uEditor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pizzaId, uEditor);
        }

        public void DeletePakDashPakComnDefered(IUnitOfWork uow, IList<string> prefixList)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), prefixList);
        }

        public void DeletePakDotPakComnDefered(IUnitOfWork uow, IList<string> prefixList)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), prefixList);
        }

        public void DeletePakDotPakPaltnoDefered(IUnitOfWork uow, IList<string> prefixList)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), prefixList);
        }

        public void DeletePizzaPartDefered(IUnitOfWork uow, PizzaPart condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        #endregion

        #endregion

        #region . Inners .

        private void PersistInsertPizza(Pizza item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza));
                    }
                }
                sqlCtx.Params[_Schema.Pizza.fn_MMIID].Value = item.MMIID;
                sqlCtx.Params[_Schema.Pizza.fn_PizzaID].Value = item.PizzaID;
                sqlCtx.Params[_Schema.Pizza.fn_Model].Value = string.IsNullOrEmpty(item.Model) ? "" : item.Model;
                sqlCtx.Params[_Schema.Pizza.fn_CartonSN].Value = string.IsNullOrEmpty(item.CartonSN) ? "" : item.CartonSN;
                sqlCtx.Params[_Schema.Pizza.fn_Remark].Value = string.IsNullOrEmpty(item.Remark) ? "" : item.Remark;
                sqlCtx.Params[_Schema.Pizza.fn_Cdt].Value = DateTime.Now;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePizza(Pizza item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza));
                    }
                }
                sqlCtx.Params[_Schema.Pizza.fn_MMIID].Value = item.MMIID;
                sqlCtx.Params[_Schema.Pizza.fn_PizzaID].Value = item.PizzaID;
                sqlCtx.Params[_Schema.Pizza.fn_Model].Value = string.IsNullOrEmpty(item.Model) ? "" : item.Model;
                sqlCtx.Params[_Schema.Pizza.fn_CartonSN].Value = string.IsNullOrEmpty(item.CartonSN) ? "" : item.CartonSN;
                sqlCtx.Params[_Schema.Pizza.fn_Remark].Value = string.IsNullOrEmpty(item.Remark) ? "" : item.Remark;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePizza(Pizza item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza));
                    }
                }
                sqlCtx.Params[_Schema.Pizza.fn_PizzaID].Value = item.PizzaID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertPizzaStatus(string pizzaId, fons.PizzaStatus status)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PizzaStatus));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PizzaStatus.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PizzaStatus.fn_Editor].Value = status.Editor;
                sqlCtx.Params[_Schema.PizzaStatus.fn_Line].Value = status.LineID;
                sqlCtx.Params[_Schema.PizzaStatus.fn_PizzaID].Value = status.PizzaID = pizzaId;
                sqlCtx.Params[_Schema.PizzaStatus.fn_Station].Value = status.StationID;
                sqlCtx.Params[_Schema.PizzaStatus.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePizzaStatus(string pizzaId, fons.PizzaStatus status)
        {
            try
            {
                string line = status.LineID;
                if (line != null)
                    line = line.Trim();

                if (string.IsNullOrEmpty(line))
                    PersistUpdatePizzaStatusWithoutLine(pizzaId, status);
                else
                    PersistUpdatePizzaStatusWithLine(pizzaId, status);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePizzaStatusWithLine(string pizzaId, fons.PizzaStatus status)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PizzaStatus));
                    }
                }
                sqlCtx.Params[_Schema.PizzaStatus.fn_PizzaID].Value = status.PizzaID = pizzaId;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PizzaStatus.fn_Editor].Value = status.Editor;
                sqlCtx.Params[_Schema.PizzaStatus.fn_Line].Value = status.LineID;
                sqlCtx.Params[_Schema.PizzaStatus.fn_Station].Value = status.StationID;
                sqlCtx.Params[_Schema.PizzaStatus.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePizzaStatusWithoutLine(string pizzaId, fons.PizzaStatus status)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PizzaStatus cond = new _Schema.PizzaStatus();
                        cond.PizzaID = pizzaId;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PizzaStatus), null, new List<string>() { _Schema.PizzaStatus.fn_Line }, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PizzaStatus.fn_PizzaID].Value = status.PizzaID = pizzaId;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PizzaStatus.fn_Editor)].Value = status.Editor;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.PizzaStatus.fn_Line)].Value = status.LineID;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PizzaStatus.fn_PizzaID)].Value = status.PizzaID = pizzaId;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PizzaStatus.fn_Station)].Value = status.StationID;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PizzaStatus.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePizzaStatus(fons.PizzaStatus status)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PizzaStatus));
                    }
                }
                sqlCtx.Params[_Schema.PizzaStatus.fn_PizzaID].Value = status.PizzaID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckAndInsertSubs(Pizza item, StateTracker tracker)
        {
            //persist PizzaPart
            IList<IProductPart> lstPzPart = (IList<IProductPart>)item.GetType().GetField("_parts", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPzPart != null && lstPzPart.Count > 0)
            {
                foreach (ProductPart pzprt in lstPzPart)
                {
                    if (tracker.GetState(pzprt) == DataRowState.Added)
                    {
                        pzprt.ProductID = item.PizzaID;
                        this.PersistInsertPizzaPart(pzprt);
                    }
                }
            }

            IList<PizzaLog> lstLog = (IList<PizzaLog>)item.GetType().GetField("_logs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstLog != null && lstLog.Count > 0)
            {
                foreach (PizzaLog log in lstLog)
                {
                    if (tracker.GetState(log) == DataRowState.Added)
                    {

                        this.PersistInsertPizzaLog(log);
                    }
                }
            }


        }

        private void CheckAndUpdateOrRemoveSubs(Pizza item, StateTracker tracker)
        {
            //persist PizzaPart
            IList<IProductPart> lstPzPart = (IList<IProductPart>)item.GetType().GetField("_parts", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPzPart != null && lstPzPart.Count > 0)
            {
                IList<IProductPart> iLstToDel = new List<IProductPart>();
                foreach (ProductPart pzprt in lstPzPart)
                {
                    if (tracker.GetState(pzprt) == DataRowState.Modified)
                    {
                        this.PersistUpdatePizzaPart(pzprt);
                    }
                    else if (tracker.GetState(pzprt) == DataRowState.Deleted)
                    {
                        this.PersistDeletePizzaPart(pzprt);
                        iLstToDel.Add(pzprt);
                    }
                }
                foreach (IProductPart prdprt in iLstToDel)
                {
                    lstPzPart.Remove(prdprt);
                }
            }
        }

        private void PersistInsertPizzaPart(ProductPart pzprt)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza_Part));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Pizza_Part.fn_cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Pizza_Part.fn_editor].Value = pzprt.Editor;
                //sqlCtx.Params[_Schema.Pizza_Part.fn_ID].Value = pzprt.ID;
                sqlCtx.Params[_Schema.Pizza_Part.fn_partNo].Value = pzprt.PartID;
                sqlCtx.Params[_Schema.Pizza_Part.fn_pizzaID].Value = pzprt.ProductID;
                sqlCtx.Params[_Schema.Pizza_Part.fn_udt].Value = cmDt;
                sqlCtx.Params[_Schema.Pizza_Part.fn_partSn].Value = pzprt.PartSn;
                sqlCtx.Params[_Schema.Pizza_Part.fn_iecpn].Value = pzprt.Iecpn;
                sqlCtx.Params[_Schema.Pizza_Part.fn_custmerPn].Value = pzprt.CustomerPn;
                sqlCtx.Params[_Schema.Pizza_Part.fn_partType].Value = pzprt.PartType;
                sqlCtx.Params[_Schema.Pizza_Part.fn_station].Value = pzprt.Station;
                sqlCtx.Params[_Schema.Pizza_Part.fn_bomNodeType].Value = pzprt.BomNodeType;
                sqlCtx.Params[_Schema.Pizza_Part.fn_checkItemType].Value = pzprt.CheckItemType;

                pzprt.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePizzaPart(ProductPart pzprt)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza_Part));
                    }
                }
                sqlCtx.Params[_Schema.Pizza_Part.fn_id].Value = pzprt.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Pizza_Part.fn_editor].Value = pzprt.Editor;
                sqlCtx.Params[_Schema.Pizza_Part.fn_partNo].Value = pzprt.PartID;
                sqlCtx.Params[_Schema.Pizza_Part.fn_pizzaID].Value = pzprt.ProductID;
                sqlCtx.Params[_Schema.Pizza_Part.fn_udt].Value = cmDt;
                sqlCtx.Params[_Schema.Pizza_Part.fn_partSn].Value = pzprt.PartSn;
                sqlCtx.Params[_Schema.Pizza_Part.fn_iecpn].Value = pzprt.Iecpn;
                sqlCtx.Params[_Schema.Pizza_Part.fn_custmerPn].Value = pzprt.CustomerPn;
                sqlCtx.Params[_Schema.Pizza_Part.fn_partType].Value = pzprt.PartType;
                sqlCtx.Params[_Schema.Pizza_Part.fn_station].Value = pzprt.Station;
                sqlCtx.Params[_Schema.Pizza_Part.fn_bomNodeType].Value = pzprt.BomNodeType;
                sqlCtx.Params[_Schema.Pizza_Part.fn_checkItemType].Value = pzprt.CheckItemType;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePizzaPart(ProductPart pzprt)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza_Part));
                    }
                }
                sqlCtx.Params[_Schema.Pizza_Part.fn_id].Value = pzprt.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region for PizzaLog
        public Pizza FillPizzaLogs(Pizza currentPizza)
        {
            try
            {
                IList<PizzaLog> newFieldVal = new List<PizzaLog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PizzaLog cond = new _Schema.PizzaLog();
                        cond.PizzaID = currentPizza.PizzaID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PizzaLog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PizzaLog.fn_PizzaID].Value = currentPizza.PizzaID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PizzaLog item = new PizzaLog();
                      
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_id]);
                        item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_PizzaID]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_Model]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_Station]);
                        item.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_Line]);
                        item.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_Descr]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PizzaLog.fn_Editor]);
                        item.Tracker.Clear();
                        item.Tracker = currentPizza.Tracker;
                        newFieldVal.Add(item);
                    }
                }
                currentPizza.GetType().GetField("_logs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPizza, newFieldVal);
                return currentPizza;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void PersistInsertPizzaLog(PizzaLog log)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PizzaLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PizzaLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PizzaLog.fn_Editor].Value = log.Editor;

                sqlCtx.Params[_Schema.PizzaLog.fn_PizzaID].Value = log.PizzaID;
                sqlCtx.Params[_Schema.PizzaLog.fn_Model].Value = log.Model;
                sqlCtx.Params[_Schema.PizzaLog.fn_Station].Value = log.Station;
                sqlCtx.Params[_Schema.PizzaLog.fn_Line].Value = log.Line;
                sqlCtx.Params[_Schema.PizzaLog.fn_Descr].Value = log.Descr;

                log.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// select distinct PartNo from Pizza_Part where PizzaID
        /// </summary>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        public IList<string> GetCombinedPartNo(string pizzaId)
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza_Part cond = new _Schema.Pizza_Part();
                        cond.pizzaID =pizzaId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(tk, typeof(_Schema.Pizza_Part),
                                                                                                               "DISTINCT ", new List<string> { _Schema.Pizza_Part.fn_partNo},
                                                                                                                cond, null, null, null,
                                                                                                                null,null,null, null);
                    }
                }
                sqlCtx.Params[_Schema.PizzaLog.fn_PizzaID].Value = pizzaId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, 
                                                                                                                             CommandType.Text, 
                                                                                                                             sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {

                        ret.Add( GetValue_Str(sqlR,0));                       
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
        /// select count(1)  from Pizza_Part where PizzaID=@PizzaID
        /// </summary>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        public int GetCombinedPartQty(string pizzaId)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza_Part cond = new _Schema.Pizza_Part();
                        cond.pizzaID = pizzaId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(tk, typeof(_Schema.Pizza_Part),
                                                                                                               "COUNT ", new List<string> { _Schema.Pizza_Part.fn_pizzaID },
                                                                                                                cond, null, null, null,
                                                                                                                null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PizzaLog.fn_PizzaID].Value = pizzaId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {

                        ret = GetValue_Int32(sqlR, 0);
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
        /// select PartNo, count(1) as qty from Pizza_Part where PizzaID group by PartNo
        /// </summary>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        public IList<PizzaPartNoQtyInfo> GetCombinePartNoQty(string pizzaId)
        {
            
            try {

                IList<PizzaPartNoQtyInfo> ret = new  List<PizzaPartNoQtyInfo>();
                SQLContextNew sqlCtx = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                      sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @"Select PartNo, count(1) as Qty 
                                                            from Pizza_Part 
                                                            where PizzaID=@PizzaID
                                                            group by PartNo  ";
                        sqlCtx.AddParam("PizzaID", new SqlParameter("@PizzaID" , SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
             sqlCtx.Param("PizzaID").Value = pizzaId;
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {


                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(_Schema.SQLData.ToObject<PizzaPartNoQtyInfo>(sqlR));
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
        /// select * from Pizza where CartonSN=@CartonSN
        /// </summary>
        /// <param name="cartonSn"></param>
        /// <returns></returns>
        public IList<Pizza> GetCombinePizzaByCartonSN(string cartonSn)
        {
            try
            {
                IList<Pizza> ret = new List<Pizza>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza cond = new _Schema.Pizza();
                        cond.CartonSN = cartonSn;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pizza.fn_CartonSN].Value = cartonSn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, 
                                                                                                                             CommandType.Text, 
                                                                                                                             sqlCtx.Sentence, 
                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        Pizza item = new Pizza();
                        item.MMIID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_MMIID]);
                        item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_PizzaID]);
                        item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Model]);
                        item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_CartonSN]);
                        item.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Remark]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pizza.fn_Cdt]);
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
        ///  select count(CartonSN)  from Pizza where CartonSN=@CartonSN
        /// </summary>
        /// <param name="cartonSn"></param>
        /// <returns></returns>
        public int GetCombinePizzaQtyByCartonSN(string cartonSn)
        {
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pizza cond = new _Schema.Pizza();
                        cond.CartonSN = cartonSn;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pizza),
                                                                                                                "COUNT",new List<string>{_Schema.Pizza.fn_CartonSN },  cond, 
                                                                                                                null, null,null,null,null ,null ,null );
                    }
                }
                sqlCtx.Params[_Schema.Pizza.fn_CartonSN].Value = cartonSn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, 0);
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

        #region backup Pizza_Part
        public void BackUpPizzaPart(string pizzaId, string editor)
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
                        _Metas.Pizza_Part cond = new _Metas.Pizza_Part();
                        cond.pizzaID = pizzaId;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Pizza_Part, _Metas.UnpackPizza_Part>(tk,
                            new string[][]{
                                new string[]{_Metas.Pizza_Part.fn_bomNodeType,_Metas.UnpackPizza_Part.fn_bomNodeType},
                                new string[]{_Metas.Pizza_Part.fn_cdt,_Metas.UnpackPizza_Part.fn_cdt},
                                new string[]{_Metas.Pizza_Part.fn_checkItemType,_Metas.UnpackPizza_Part.fn_checkItemType},
                                new string[]{_Metas.Pizza_Part.fn_custmerPn,_Metas.UnpackPizza_Part.fn_custmerPn},
                                new string[]{_Metas.Pizza_Part.fn_editor,_Metas.UnpackPizza_Part.fn_editor},
                                new string[]{_Metas.Pizza_Part.fn_id,_Metas.UnpackPizza_Part.fn_pizza_PartID},
                                new string[]{_Metas.Pizza_Part.fn_iecpn,_Metas.UnpackPizza_Part.fn_iecpn},
                                new string[]{_Metas.Pizza_Part.fn_partNo,_Metas.UnpackPizza_Part.fn_partNo},
                                new string[]{_Metas.Pizza_Part.fn_partSn,_Metas.UnpackPizza_Part.fn_partSn},
                                new string[]{_Metas.Pizza_Part.fn_partType,_Metas.UnpackPizza_Part.fn_partType},
                                new string[]{_Metas.Pizza_Part.fn_pizzaID,_Metas.UnpackPizza_Part.fn_pizzaID},
                                new string[]{_Metas.Pizza_Part.fn_station,_Metas.UnpackPizza_Part.fn_station},
                                new string[]{_Metas.Pizza_Part.fn_udt,_Metas.UnpackPizza_Part.fn_udt},
                                new string[]{string.Format("@{0}",_Metas.UnpackPizza_Part.fn_ueditor), _Metas.UnpackPizza_Part.fn_ueditor},
                                new string[]{"GETDATE()",_Metas.UnpackPizza_Part.fn_updt}
                            },
                            new ConditionCollection<_Metas.Pizza_Part>(
                                new EqualCondition<_Metas.Pizza_Part>(cond)));
                        sqlCtx.AddParam(_Metas.UnpackPizza_Part.fn_ueditor, new SqlParameter(string.Format("@{0}", _Metas.UnpackProduct_Part.fn_ueditor), ToolsNew.GetDBFieldType<_Metas.UnpackPizza_Part>(_Metas.UnpackPizza_Part.fn_ueditor)));
                    }
                }
                sqlCtx.Param(_Metas.Pizza_Part.fn_pizzaID).Value = pizzaId;
                sqlCtx.Param(_Metas.UnpackPizza_Part.fn_ueditor).Value = editor;
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

        public void BackUpPizzaPartByPartType(string pizzaId, string partType, string editor)
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
                        _Metas.Pizza_Part cond = new _Metas.Pizza_Part();
                        cond.pizzaID = pizzaId;
                        cond.partType = partType;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Pizza_Part, _Metas.UnpackPizza_Part>(tk,
                            new string[][]{
                                new string[]{_Metas.Pizza_Part.fn_bomNodeType,_Metas.UnpackPizza_Part.fn_bomNodeType},
                                new string[]{_Metas.Pizza_Part.fn_cdt,_Metas.UnpackPizza_Part.fn_cdt},
                                new string[]{_Metas.Pizza_Part.fn_checkItemType,_Metas.UnpackPizza_Part.fn_checkItemType},
                                new string[]{_Metas.Pizza_Part.fn_custmerPn,_Metas.UnpackPizza_Part.fn_custmerPn},
                                new string[]{_Metas.Pizza_Part.fn_editor,_Metas.UnpackPizza_Part.fn_editor},
                                new string[]{_Metas.Pizza_Part.fn_id,_Metas.UnpackPizza_Part.fn_pizza_PartID},
                                new string[]{_Metas.Pizza_Part.fn_iecpn,_Metas.UnpackPizza_Part.fn_iecpn},
                                new string[]{_Metas.Pizza_Part.fn_partNo,_Metas.UnpackPizza_Part.fn_partNo},
                                new string[]{_Metas.Pizza_Part.fn_partSn,_Metas.UnpackPizza_Part.fn_partSn},
                                new string[]{_Metas.Pizza_Part.fn_partType,_Metas.UnpackPizza_Part.fn_partType},
                                new string[]{_Metas.Pizza_Part.fn_pizzaID,_Metas.UnpackPizza_Part.fn_pizzaID},
                                new string[]{_Metas.Pizza_Part.fn_station,_Metas.UnpackPizza_Part.fn_station},
                                new string[]{_Metas.Pizza_Part.fn_udt,_Metas.UnpackPizza_Part.fn_udt},
                                new string[]{string.Format("@{0}",_Metas.UnpackPizza_Part.fn_ueditor), _Metas.UnpackPizza_Part.fn_ueditor},
                                new string[]{"GETDATE()",_Metas.UnpackPizza_Part.fn_updt}
                            },
                            new ConditionCollection<_Metas.Pizza_Part>(
                                new EqualCondition<_Metas.Pizza_Part>(cond)));
                        sqlCtx.AddParam(_Metas.UnpackPizza_Part.fn_ueditor, new SqlParameter(string.Format("@{0}", _Metas.UnpackProduct_Part.fn_ueditor), ToolsNew.GetDBFieldType<_Metas.UnpackPizza_Part>(_Metas.UnpackPizza_Part.fn_ueditor)));
                    }
                }
                sqlCtx.Param(_Metas.Pizza_Part.fn_pizzaID).Value = pizzaId;
                sqlCtx.Param(_Metas.Pizza_Part.fn_partType).Value = partType;
                sqlCtx.Param(_Metas.UnpackPizza_Part.fn_ueditor).Value = editor;
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
        public void BackUpPizzaPartByPartSn(string pizzaId, string partSn, string editor)
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
                        _Metas.Pizza_Part cond = new _Metas.Pizza_Part();
                        cond.pizzaID = pizzaId;
                        cond.partSn = partSn;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Pizza_Part, _Metas.UnpackPizza_Part>(tk,
                            new string[][]{
                                new string[]{_Metas.Pizza_Part.fn_bomNodeType,_Metas.UnpackPizza_Part.fn_bomNodeType},
                                new string[]{_Metas.Pizza_Part.fn_cdt,_Metas.UnpackPizza_Part.fn_cdt},
                                new string[]{_Metas.Pizza_Part.fn_checkItemType,_Metas.UnpackPizza_Part.fn_checkItemType},
                                new string[]{_Metas.Pizza_Part.fn_custmerPn,_Metas.UnpackPizza_Part.fn_custmerPn},
                                new string[]{_Metas.Pizza_Part.fn_editor,_Metas.UnpackPizza_Part.fn_editor},
                                new string[]{_Metas.Pizza_Part.fn_id,_Metas.UnpackPizza_Part.fn_pizza_PartID},
                                new string[]{_Metas.Pizza_Part.fn_iecpn,_Metas.UnpackPizza_Part.fn_iecpn},
                                new string[]{_Metas.Pizza_Part.fn_partNo,_Metas.UnpackPizza_Part.fn_partNo},
                                new string[]{_Metas.Pizza_Part.fn_partSn,_Metas.UnpackPizza_Part.fn_partSn},
                                new string[]{_Metas.Pizza_Part.fn_partType,_Metas.UnpackPizza_Part.fn_partType},
                                new string[]{_Metas.Pizza_Part.fn_pizzaID,_Metas.UnpackPizza_Part.fn_pizzaID},
                                new string[]{_Metas.Pizza_Part.fn_station,_Metas.UnpackPizza_Part.fn_station},
                                new string[]{_Metas.Pizza_Part.fn_udt,_Metas.UnpackPizza_Part.fn_udt},
                                new string[]{string.Format("@{0}",_Metas.UnpackPizza_Part.fn_ueditor), _Metas.UnpackPizza_Part.fn_ueditor},
                                new string[]{"GETDATE()",_Metas.UnpackPizza_Part.fn_updt}
                            },
                            new ConditionCollection<_Metas.Pizza_Part>(
                                new EqualCondition<_Metas.Pizza_Part>(cond)));
                        sqlCtx.AddParam(_Metas.UnpackPizza_Part.fn_ueditor, new SqlParameter(string.Format("@{0}", _Metas.UnpackProduct_Part.fn_ueditor), ToolsNew.GetDBFieldType<_Metas.UnpackPizza_Part>(_Metas.UnpackPizza_Part.fn_ueditor)));
                    }
                }
                sqlCtx.Param(_Metas.Pizza_Part.fn_pizzaID).Value = pizzaId;
                sqlCtx.Param(_Metas.Pizza_Part.fn_partSn).Value = partSn;
                sqlCtx.Param(_Metas.UnpackPizza_Part.fn_ueditor).Value = editor;
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
        public void BackUpPizzaPartDefered(IUnitOfWork uow, string pizzaId, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pizzaId, editor);
        }

        public void BackUpPizzaPartByPartTypeDefered(IUnitOfWork uow, string pizzaId, string partType, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pizzaId, partType,editor);
        }

        public void BackUpPizzaPartByPartSnDefered(IUnitOfWork uow, string pizzaId, string partSn, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pizzaId,partSn, editor);
        }

        #endregion

        #region others function

        public IList<PizzaPart> GetPizzaPart(PizzaPart condition)
        {
            try
            {
                IList<PizzaPart> ret = new List<PizzaPart>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.Pizza_Part cond = FuncNew.SetColumnFromField<_Metas.Pizza_Part, PizzaPart>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pizza_Part>(null, null,
                                                                                                   new ConditionCollection<_Metas.Pizza_Part>(new EqualCondition<_Metas.Pizza_Part>(cond)),
                                                                                                   _Metas.Pizza_Part.fn_pizzaID, _Metas.Pizza_Part.fn_id);


                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.Pizza_Part, PizzaPart>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::Pizza_Part, PizzaPart, PizzaPart>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdatePizzaPart(PizzaPart item)
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
                        _Metas.Pizza_Part cond = new _Metas.Pizza_Part();
                        cond.id = item.ID;
                        _Metas.Pizza_Part setv = FuncNew.SetColumnFromField<_Metas.Pizza_Part, PizzaPart>(item, _Metas.Pizza_Part.fn_id);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.Pizza_Part>(tk,
                                                                                                         new SetValueCollection<_Metas.Pizza_Part>(new CommonSetValue<_Metas.Pizza_Part>(setv)),
                                                                                                         new ConditionCollection<_Metas.Pizza_Part>(new EqualCondition<_Metas.Pizza_Part>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.Pizza_Part.fn_id).Value = item.ID;

                sqlCtx = FuncNew.SetColumnFromField<_Metas.Pizza_Part, PizzaPart>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Pizza_Part.fn_udt)).Value = cmDt;

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
        public void BackUpPizzaPartById(int id, string editor)
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
                        _Metas.Pizza_Part cond = new _Metas.Pizza_Part();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Pizza_Part, _Metas.UnpackPizza_Part>(tk,
                            new string[][]{
                                new string[]{_Metas.Pizza_Part.fn_bomNodeType,_Metas.UnpackPizza_Part.fn_bomNodeType},
                                new string[]{_Metas.Pizza_Part.fn_cdt,_Metas.UnpackPizza_Part.fn_cdt},
                                new string[]{_Metas.Pizza_Part.fn_checkItemType,_Metas.UnpackPizza_Part.fn_checkItemType},
                                new string[]{_Metas.Pizza_Part.fn_custmerPn,_Metas.UnpackPizza_Part.fn_custmerPn},
                                new string[]{_Metas.Pizza_Part.fn_editor,_Metas.UnpackPizza_Part.fn_editor},
                                new string[]{_Metas.Pizza_Part.fn_id,_Metas.UnpackPizza_Part.fn_pizza_PartID},
                                new string[]{_Metas.Pizza_Part.fn_iecpn,_Metas.UnpackPizza_Part.fn_iecpn},
                                new string[]{_Metas.Pizza_Part.fn_partNo,_Metas.UnpackPizza_Part.fn_partNo},
                                new string[]{_Metas.Pizza_Part.fn_partSn,_Metas.UnpackPizza_Part.fn_partSn},
                                new string[]{_Metas.Pizza_Part.fn_partType,_Metas.UnpackPizza_Part.fn_partType},
                                new string[]{_Metas.Pizza_Part.fn_pizzaID,_Metas.UnpackPizza_Part.fn_pizzaID},
                                new string[]{_Metas.Pizza_Part.fn_station,_Metas.UnpackPizza_Part.fn_station},
                                new string[]{_Metas.Pizza_Part.fn_udt,_Metas.UnpackPizza_Part.fn_udt},
                                new string[]{string.Format("@{0}",_Metas.UnpackPizza_Part.fn_ueditor), _Metas.UnpackPizza_Part.fn_ueditor},
                                new string[]{"GETDATE()",_Metas.UnpackPizza_Part.fn_updt}
                            },
                            new ConditionCollection<_Metas.Pizza_Part>(
                                new EqualCondition<_Metas.Pizza_Part>(cond)));
                        sqlCtx.AddParam(_Metas.UnpackPizza_Part.fn_ueditor, new SqlParameter(string.Format("@{0}", _Metas.UnpackProduct_Part.fn_ueditor), ToolsNew.GetDBFieldType<_Metas.UnpackPizza_Part>(_Metas.UnpackPizza_Part.fn_ueditor)));
                    }
                }
                sqlCtx.Param(_Metas.Pizza_Part.fn_id).Value = id;
                sqlCtx.Param(_Metas.UnpackPizza_Part.fn_ueditor).Value = editor;
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
        public void UpdatePizzaPartDefered(IUnitOfWork uow, PizzaPart item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }
        public void BackUpPizzaPartByIdDefered(IUnitOfWork uow, int id, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id, editor);
        }

        #endregion
    }
}
