using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.FRU;
using IMES.Infrastructure.UnitOfWork;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure.Util;
using System.Data;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: Gift相关
    /// </summary>
    public class GiftRepository : BaseRepository<FRUGift>, IGiftRepository
    {
        #region Overrides of BaseRepository<FRUGift>

        protected override void PersistNewItem(FRUGift item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertFRUGift(item);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(FRUGift item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateFRUGift(item);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(FRUGift item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteFRUGift(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<FRUGift>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override FRUGift Find(object key)
        {
            try
            {
                FRUGift ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUGift cond = new _Schema.FRUGift();
                        cond.ID = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUGift.fn_ID].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new FRUGift(  GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift.fn_Model]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift.fn_ID]),
                                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FRUGift.fn_Qty]));
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
        public override IList<FRUGift> FindAll()
        {
            try
            {
                IList<FRUGift> ret = new List<FRUGift>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FRUGift item = new FRUGift( GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift.fn_Model]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift.fn_ID]),
                                                    GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FRUGift.fn_Qty]));
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
        public override void Add(FRUGift item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(FRUGift item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(FRUGift item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region IGiftRepository Members

        public FRUGift FillGiftParts(FRUGift gift)
        {
            try
            {
                IList<IFRUPart> newFieldVal = new List<IFRUPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUGift_Part cond = new _Schema.FRUGift_Part();
                        cond.GiftID = gift.GiftID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUGift_Part.fn_GiftID].Value = gift.GiftID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FRUPart fruPart = new FRUPart(  GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift_Part.fn_GiftID]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift_Part.fn_PartNo]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift_Part.fn_Value]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FRUGift_Part.fn_Editor]),
                                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.FRUGift_Part.fn_Cdt])
                                                     );

                        fruPart.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FRUGift_Part.fn_ID]);
                        fruPart.Tracker.Clear();
                        fruPart.Tracker = gift.Tracker;
                        newFieldVal.Add(fruPart);
                    }
                }
                gift.GetType().GetField("_parts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gift, newFieldVal);

                return gift;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfFRUGift(string id)
        {
            //select count(*) from IMES_PAK..FRUGift where ID=''
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUGift cond = new _Schema.FRUGift();
                        cond.ID = id;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift), "COUNT", new List<string>() { _Schema.FRUGift.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUGift.fn_ID].Value = id;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    }
                }
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion

        #region . Inners .

        private void PersistInsertFRUGift(FRUGift item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift));
                    }
                }
                sqlCtx.Params[_Schema.FRUGift.fn_ID].Value = item.GiftID;
                sqlCtx.Params[_Schema.FRUGift.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.FRUGift.fn_Qty].Value = item.Qty;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateFRUGift(FRUGift item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift));
                    }
                }
                sqlCtx.Params[_Schema.FRUGift.fn_ID].Value = item.GiftID;
                sqlCtx.Params[_Schema.FRUGift.fn_Model].Value = item.Model;
                sqlCtx.Params[_Schema.FRUGift.fn_Qty].Value = item.Qty;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteFRUGift(FRUGift item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift));
                    }
                }
                sqlCtx.Params[_Schema.FRUGift.fn_ID].Value = item.GiftID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckAndInsertSubs(FRUGift item, StateTracker tracker)
        {
            IList<IFRUPart> lstPart = (IList<IFRUPart>)item.GetType().GetField("_parts", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPart != null && lstPart.Count > 0)
            {
                foreach (IFRUPart part in lstPart)
                {
                    if (tracker.GetState(part) == DataRowState.Added)
                    {
                        part.FRUID = item.GiftID;
                        this.PersistInsertFRUPart(part);
                    }
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
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUGift_Part));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.FRUGift_Part.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.FRUGift_Part.fn_Editor].Value = part.Editor;
                sqlCtx.Params[_Schema.FRUGift_Part.fn_GiftID].Value = part.FRUID;
                sqlCtx.Params[_Schema.FRUGift_Part.fn_PartNo].Value = part.PartID;
                sqlCtx.Params[_Schema.FRUGift_Part.fn_Value].Value = part.Value;
                part.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
