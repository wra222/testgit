using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.FRU;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: FRUCarton相关
    /// </summary>
    public class FRUCartonRepository : BaseRepository<FRUCarton>, IFRUCartonRepository
    {
        #region Overrides of BaseRepository<FRUCarton>

        protected override void PersistNewItem(FRUCarton item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertFRUCarton(item);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(FRUCarton item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateFRUCarton(item);

                    this.CheckAndInsertSubs(item, tracker);

                    this.CheckAndUpdateOrRemoveSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(FRUCarton item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.CheckAndUpdateOrRemoveSubs(item, tracker);

                    this.PersistDeleteFRUCarton(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<FRUCarton>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override FRUCarton Find(object key)
        {
            try
            {
                FRUCarton ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUCarton cond = new _Schema.FRUCarton();
                        cond.ID = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton.fn_ID].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new FRUCarton(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton.fn_ID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton.fn_Model]),
                                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FRUCarton.fn_Qty]));
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
        public override IList<FRUCarton> FindAll()
        {
            try
            {
                IList<FRUCarton> ret = new List<FRUCarton>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FRUCarton item = new FRUCarton( GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton.fn_ID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton.fn_Model]),
                                                        GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FRUCarton.fn_Qty]));
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
        public override void Add(FRUCarton item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(FRUCarton item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(FRUCarton item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region IFRUCartonRepository Members

        public FRUCarton FillCartonParts(FRUCarton carton)
        {
            try
            {
                IList<IFRUPart> newFieldVal = new List<IFRUPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUCarton_Part cond = new _Schema.FRUCarton_Part();
                        cond.CartonID = carton.CartonID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton_Part.fn_CartonID].Value = carton.CartonID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FRUPart fruPart = new FRUPart(  GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_Part.fn_CartonID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_Part.fn_PartNo]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_Part.fn_Value]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_Part.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_Part.fn_Cdt])
                                                     );

                        fruPart.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_Part.fn_ID]);
                        fruPart.Tracker.Clear();
                        fruPart.Tracker = carton.Tracker;
                        newFieldVal.Add(fruPart);
                    }
                }
                carton.GetType().GetField("_parts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, newFieldVal);
                return carton;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FRUCarton FillCartonGifts(FRUCarton carton)
        {
            try
            {
                IList<IFRUPart> newFieldVal = new List<IFRUPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUCarton_FRUGift cond = new _Schema.FRUCarton_FRUGift();
                        cond.CartonID = carton.CartonID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton_FRUGift), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton_FRUGift.fn_CartonID].Value = carton.CartonID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FRUPart giftRelation = new FRUPart( GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_FRUGift.fn_CartonID]),
                                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_FRUGift.fn_GiftID]),
                                                            null,
                                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_FRUGift.fn_Editor]),
                                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_FRUGift.fn_Cdt])
                                                          );

                        giftRelation.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FRUCarton_FRUGift.fn_ID]);
                        giftRelation.Tracker.Clear();
                        giftRelation.Tracker = carton.Tracker;
                        newFieldVal.Add(giftRelation);
                    }
                }
                carton.GetType().GetField("_gifts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, newFieldVal);
                return carton;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistForFRUCarton_FRUGift(string giftId)
        {
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUCarton_FRUGift cond = new _Schema.FRUCarton_FRUGift();
                        cond.GiftID = giftId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton_FRUGift), "COUNT", new List<string>() { _Schema.FRUCarton_FRUGift.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton_FRUGift.fn_GiftID].Value = giftId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
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

        #endregion

        #region . Inners .

        private void PersistInsertFRUCarton(FRUCarton item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton));
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton.fn_ID].Value = item.CartonID;
                sqlCtx.Params[_Schema.FRUCarton.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.FRUCarton.fn_Qty].Value = item.Qty;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateFRUCarton(FRUCarton item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton));
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton.fn_ID].Value = item.CartonID;
                sqlCtx.Params[_Schema.FRUCarton.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.FRUCarton.fn_Qty].Value = item.Qty;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteFRUCarton(FRUCarton item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton));
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton.fn_ID].Value = item.CartonID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckAndInsertSubs(FRUCarton item, StateTracker tracker)
        {
            IList<IFRUPart> lstPart = (IList<IFRUPart>)item.GetType().GetField("_parts", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPart != null && lstPart.Count > 0)
            {
                foreach (IFRUPart part in lstPart)
                {
                    if (tracker.GetState(part) == DataRowState.Added)
                    {
                        part.FRUID = item.CartonID;
                        this.PersistInsertFRUPart(part);
                    }
                }
            }

            IList<IFRUPart> lstGiftRelation = (IList<IFRUPart>)item.GetType().GetField("_gifts", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstGiftRelation != null && lstGiftRelation.Count > 0)
            {
                foreach (IFRUPart giftRelation in lstGiftRelation)
                {
                    if (tracker.GetState(giftRelation) == DataRowState.Added)
                    {
                        giftRelation.FRUID = item.CartonID;
                        this.PersistInsertFRUGiftRelation(giftRelation);
                    }
                }
            }
        }

        private void CheckAndUpdateOrRemoveSubs(FRUCarton item, StateTracker tracker)
        {
            IList<IFRUPart> lstGiftRelation = (IList<IFRUPart>)item.GetType().GetField("_gifts", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstGiftRelation != null && lstGiftRelation.Count > 0)
            {
                IList<IFRUPart> iLstToDel = new List<IFRUPart>();
                foreach (IFRUPart giftRelation in lstGiftRelation)
                {
                    if (tracker.GetState(giftRelation) == DataRowState.Modified)
                    {
                        //this.PersistUpdateFRUGiftRelation(giftRelation);
                    }
                    else if (tracker.GetState(giftRelation) == DataRowState.Deleted)
                    {
                        this.PersistDeleteFRUGiftRelation(giftRelation);
                        iLstToDel.Add(giftRelation);
                    }
                }
                foreach (IFRUPart frugiftrelation in iLstToDel)
                {
                    lstGiftRelation.Remove(frugiftrelation);
                }
            }
        }

        private void PersistInsertFRUPart(IFRUPart part)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton_Part));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.FRUCarton_Part.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.FRUCarton_Part.fn_Editor].Value = part.Editor;
                sqlCtx.Params[_Schema.FRUCarton_Part.fn_CartonID].Value = part.FRUID;
                sqlCtx.Params[_Schema.FRUCarton_Part.fn_PartNo].Value = part.PartID;
                sqlCtx.Params[_Schema.FRUCarton_Part.fn_Value].Value = part.Value;
                part.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertFRUGiftRelation(IFRUPart giftRelation)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton_FRUGift));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.FRUCarton_FRUGift.fn_CartonID].Value = giftRelation.FRUID;
                sqlCtx.Params[_Schema.FRUCarton_FRUGift.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.FRUCarton_FRUGift.fn_Editor].Value = giftRelation.Editor;
                sqlCtx.Params[_Schema.FRUCarton_FRUGift.fn_GiftID].Value = giftRelation.PartID;
                //giftRelation.Value;
                giftRelation.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteFRUGiftRelation(IFRUPart giftRelation)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUCarton_FRUGift));
                    }
                }
                sqlCtx.Params[_Schema.FRUCarton_FRUGift.fn_ID].Value = giftRelation.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
