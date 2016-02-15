//2010-05-31 Liu Dong(eB1-4)         Modify ITC-1155-0155
//2010-06-04 Liu Dong(eB1-4)         Modify ITC-1155-0178 
//2011-04-08 Liu Dong(eB1-4)         Modify ITC-1268-0016
//2011-04-13 Lucy Liu                Modify ITC-1268-0077
//2011-09-13 Liu Dong(eB2-2)         Modify FisObject更新库的并发问题。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.PAK.DN;
using IMES.FisObject.FA.Product;
using Delivery = IMES.FisObject.PAK.DN.Delivery;
using System.Text.RegularExpressions;
using Shema=IMES.Infrastructure.Repository._Schema;

//

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: Delivery相关
    /// </summary>
    public class DeliveryRepository : BaseRepository<fons::Delivery>, IDeliveryRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Link To Other
        private static IMES.FisObject.Common.Part.IPartRepository _prtRepository = null;
        private static IMES.FisObject.Common.Part.IPartRepository PrtRepository
        {
            get
            {
                if (_prtRepository == null)
                    _prtRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.IPartRepository, IMES.FisObject.Common.Part.IPart>();
                return _prtRepository;
            }
        }
        #endregion

        #region Overrides of BaseRepository<Delivery>

        protected override void PersistNewItem(fons::Delivery item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertDelivery(item);
                }

                this.CheckAndInsertSubs(item, tracker);
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(fons::Delivery item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                //2011-09-13 Liu Dong(eB2-2)         Modify FisObject更新库的并发问题。
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateDelivery(item);
                }

                this.CheckAndInsertOrUpdateOrRemoveSubs(item, tracker);
                //2011-09-13 Liu Dong(eB2-2)         Modify FisObject更新库的并发问题。
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(fons::Delivery item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteDelivery(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Delivery>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons::Delivery Find(object key)
        {
            try
            {
                fons::Delivery ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Delivery cond = new _Schema.Delivery();
                        cond.DeliveryNo = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons::Delivery();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Editor]);
                        ret.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_DeliveryNo]);
                        ret.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Model]);
                        ret.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_PoNo]);
                        ret.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Qty]);
                        ret.ShipDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_ShipDate]);
                        ret.ShipmentNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_ShipmentNo]);
                        ret.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Status]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Udt]);
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
        public override IList<fons::Delivery> FindAll()
        {
            try
            {
                IList<fons::Delivery> ret = new List<fons::Delivery>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons::Delivery item = new fons::Delivery();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Editor]);
                        item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_DeliveryNo]);
                        item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Model]);
                        item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_PoNo]);
                        item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Qty]);
                        item.ShipDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_ShipDate]);
                        item.ShipmentNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_ShipmentNo]);
                        item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Status]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_Udt]);
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
        public override void Add(fons::Delivery item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(fons::Delivery item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons::Delivery item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IDeliveryRepository

        public string GetPAKConnectionString()
        {
            return _Schema.SqlHelper.ConnectionString_PAK;
        }

        public string GetDataConnectionString()
        {
            return _Schema.SqlHelper.ConnectionString_GetData;
        }

        /// <summary>
        /// 获取符合出货条件的BolNo列表
        /// </summary>
        /// <returns>符合出货条件的BOL码集合</returns>
        public IList<BOLNoInfo> GetBolNo()
        {
            //Delivery表中Shipdate大于等于当前日期减五天
            try
            {
                IList<BOLNoInfo> ret = new List<BOLNoInfo>();

                DateTime now = DateTime.Now;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Delivery);
                        _Schema.Delivery geCond = new _Schema.Delivery();
                        geCond.ShipDate = now.AddDays(-5);
                        _Schema.Delivery cond1 = new _Schema.Delivery();
                        cond1.Status = "00";
                        tf1.equalcond = cond1;
                        tf1.greaterOrEqualcond = geCond;
                        tf1.ToGetFieldNames = null;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.DeliveryInfo);
                        _Schema.DeliveryInfo cond2 = new _Schema.DeliveryInfo();
                        cond2.InfoType = "BOL";
                        tf2.equalcond = cond2;
                        tf2.ToGetFieldNames.Add(_Schema.DeliveryInfo.fn_InfoValue);

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_DeliveryNo, tf2, _Schema.DeliveryInfo.fn_DeliveryNo);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_Status)].Value = cond1.Status;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoType)].Value = cond2.InfoType;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.Delivery.fn_ShipDate))].Value = now.AddDays(-5);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        BOLNoInfo item = new BOLNoInfo();
                        item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue)]);
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
        /// 根据BolNo获取DN
        /// </summary>
        /// <param name="bolNo">BOL码</param>
        /// <returns>DN集合</returns>
        public IList<string> GetDNByBolNo(string bolNo)
        {
            try
            {
                IList<string> ret = new List<string>();

                DateTime now = DateTime.Now;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields tf3 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Delivery);
                        _Schema.Delivery geCond = new _Schema.Delivery();
                        geCond.ShipDate = now.AddDays(-5);
                        _Schema.Delivery cond1 = new _Schema.Delivery();
                        cond1.Status = "00";
                        tf1.equalcond = cond1;
                        tf1.greaterOrEqualcond = geCond;
                        tf1.ToGetFieldNames.Add(_Schema.Delivery.fn_DeliveryNo);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.DeliveryInfo);
                        _Schema.DeliveryInfo cond2 = new _Schema.DeliveryInfo();
                        cond2.InfoType = "BOL";
                        cond2.InfoValue = bolNo;
                        tf2.equalcond = cond2;
                        tf2.ToGetFieldNames = null;

                        tf3 = new _Schema.TableAndFields();
                        tf3.Table = typeof(_Schema.Model);
                        tf3.subDBCalalog = _Schema.SqlHelper.DB_BOM;
                        _Schema.Model cond3 = new _Schema.Model();
                        cond3.ShipType = "PC";
                        tf3.equalcond = cond3;
                        tf3.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_DeliveryNo, tf2, _Schema.DeliveryInfo.fn_DeliveryNo);
                        tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_Model, tf3, _Schema.Model.fn_model);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_Status)].Value = cond1.Status;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoType)].Value = cond2.InfoType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.Model.fn_ShipType)].Value = cond3.ShipType;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Delivery.fn_DeliveryNo));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];


                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.Delivery.fn_ShipDate))].Value = now.AddDays(-5);
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue)].Value = bolNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_DeliveryNo)]);
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

        public IList<DNInfo> GetDNByModel(string modelId)
        {
            try
            {
                IList<DNInfo> ret = new List<DNInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Delivery cond = new _Schema.Delivery();
                        cond.Model = modelId;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery.fn_Model].Value = modelId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DNInfo item = new DNInfo();
                        item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_DeliveryNo]);
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

        public IList<string> GetDeliveryNoListFor054()
        {
            try
            {
                IList<string> ret = new List<string>();

                DateTime now = DateTime.Now;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields tf3 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Delivery);
                        _Schema.Delivery geCond = new _Schema.Delivery();
                        geCond.ShipDate = now.AddDays(-5);
                        _Schema.Delivery insetCond1 = new _Schema.Delivery();
                        insetCond1.Status = "INSET";
                        tf1.inSetcond = insetCond1;
                        tf1.greaterOrEqualcond = geCond;
                        tf1.ToGetFieldNames.Add(_Schema.Delivery.fn_DeliveryNo);

                        //tf2 = new _Schema.TableAndFields();
                        //tf2.Table = typeof(_Schema.DeliveryInfo);
                        //_Schema.DeliveryInfo cond2 = new _Schema.DeliveryInfo();
                        //cond2.InfoType = "BOL";
                        //cond2.InfoValue = bolNo;
                        //tf2.equalcond = cond2;
                        //tf2.ToGetFieldNames = null;

                        tf3 = new _Schema.TableAndFields();
                        tf3.Table = typeof(_Schema.Model);
                        tf3.subDBCalalog = _Schema.SqlHelper.DB_BOM;
                        _Schema.Model cond3 = new _Schema.Model();
                        cond3.ShipType = "PC";
                        cond3.Region = "TRO";
                        tf3.equalcond = cond3;
                        tf3.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        //_Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_DeliveryNo, tf2, _Schema.DeliveryInfo.fn_DeliveryNo);
                        //tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_Model, tf3, _Schema.Model.fn_model);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, /*tf2,*/ tf3 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecInSet(_Schema.Delivery.fn_Status)), _Schema.Func.ConvertInSet(new List<string>() { "00", "82" }));

                        //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_Status)].Value = cond1.Status;
                        //sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoType)].Value = cond2.InfoType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.Model.fn_ShipType)].Value = cond3.ShipType;
                        sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.Model.fn_Region)].Value = cond3.Region;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Delivery.fn_DeliveryNo));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                //tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[1];


                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.Delivery.fn_ShipDate))].Value = now.AddDays(-5);
                //sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue)].Value = bolNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_DeliveryNo)]);
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

        public IList<string> GetDeliveryNoListFor053(string Model)
        {
            try
            {
                IList<string> ret = new List<string>();

                DateTime now = DateTime.Now;

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields tf3 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Delivery);
                        _Schema.Delivery geCond = new _Schema.Delivery();
                        geCond.ShipDate = now.AddDays(-5);
                        _Schema.Delivery cond1 = new _Schema.Delivery();
                        cond1.Status = "00";
                        tf1.equalcond = cond1;
                        tf1.greaterOrEqualcond = geCond;
                        tf1.ToGetFieldNames.Add(_Schema.Delivery.fn_DeliveryNo);

                        //tf2 = new _Schema.TableAndFields();
                        //tf2.Table = typeof(_Schema.DeliveryInfo);
                        //_Schema.DeliveryInfo cond2 = new _Schema.DeliveryInfo();
                        //cond2.InfoType = "BOL";
                        //cond2.InfoValue = bolNo;
                        //tf2.equalcond = cond2;
                        //tf2.ToGetFieldNames = null;

                        tf3 = new _Schema.TableAndFields();
                        tf3.Table = typeof(_Schema.Model);
                        tf3.subDBCalalog = _Schema.SqlHelper.DB_BOM;
                        _Schema.Model cond3 = new _Schema.Model();
                        cond3.model = Model;
                        tf3.equalcond = cond3;
                        tf3.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        //_Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_DeliveryNo, tf2, _Schema.DeliveryInfo.fn_DeliveryNo);
                        //tblCnntIs.Add(tc1);
                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_Model, tf3, _Schema.Model.fn_model);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, /*tf2,*/ tf3 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_Status)].Value = cond1.Status;
                        //sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoType)].Value = cond2.InfoType;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Delivery.fn_DeliveryNo));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                //tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[1];


                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.Delivery.fn_ShipDate))].Value = now.AddDays(-5);
                //sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue)].Value = bolNo;
                sqlCtx.Params[_Schema.Func.DecAlias(tf3.alias, _Schema.Model.fn_model)].Value = Model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_DeliveryNo)]);
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

        public fons::Delivery FillDnPalletList(fons::Delivery delivery)
        {
            try
            {
                IList<DeliveryPallet> newFieldVal = new List<DeliveryPallet>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Delivery_Pallet cond = new _Schema.Delivery_Pallet();
                        cond.DeliveryNo = delivery.DeliveryNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryNo].Value = delivery.DeliveryNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DeliveryPallet item = new DeliveryPallet();
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_ID]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_Cdt]);
                        item.DeliveryID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_DeliveryNo]);
                        item.DeliveryQty = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_DeliveryQty]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_Editor]);
                        item.PalletID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_PalletNo]);
                        item.ShipmentID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_ShipmentNo]);
                        item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_Status]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_Udt]);

                        item.DeviceQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_deviceQty]);
                        item.PalletType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_palletType]);
                        //item.Tracker.Clear();
                        //item.Tracker = delivery.Tracker;
                        newFieldVal.Add(item);
                    }
                }
                delivery.GetType().GetField("_dnplts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(delivery, newFieldVal);
                return delivery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DNForUI> GetDNListByCondition(DNQueryCondition MyCondition, out int totalLength)
        {
            long sum = 0;
            return GetDNListByCondition_Inner(MyCondition, out totalLength, out sum);
        }

        public IList<DNForUI> GetDNListByCondition(DNQueryCondition MyCondition, out int totalLength, out long sum)
        {
            return GetDNListByCondition_Inner(MyCondition, out totalLength, out sum);
        }

        private IList<DNForUI> GetDNListByCondition_Inner(DNQueryCondition MyCondition, out int totalLength, out long sum)
        {
            try
            {
                sum = 0;
                totalLength = 0;
                IList<DNForUI> ret = new List<DNForUI>();

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlCtxCllctn = new _Schema.SQLContextCollection();
                bool isJoin = false;
                int i = 0;
                if (!string.IsNullOrEmpty(MyCondition.DNInfoValue))
                {
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_DNInfoValue(MyCondition.DNInfoValue));
                    isJoin = true;
                }

                if (!string.IsNullOrEmpty(MyCondition.DeliveryNo))
                {
                    if (MyCondition.DeliveryNo.Length == 16)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_DeliveryNo2(MyCondition.DeliveryNo, isJoin));
                    else if (MyCondition.DeliveryNo.Length == 10)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_DeliveryNo(MyCondition.DeliveryNo, isJoin));
                }

                if (!string.IsNullOrEmpty(MyCondition.Model))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_Model(MyCondition.Model));

                if (!string.IsNullOrEmpty(MyCondition.PONo))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_PONo(MyCondition.PONo));

                if (!string.IsNullOrEmpty(MyCondition.ShipmentNo))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_ShipmentNo(MyCondition.ShipmentNo));

                if (DateTime.MinValue != MyCondition.ShipDateFrom)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_ShipDateFrom(MyCondition.ShipDateFrom));

                if (DateTime.MinValue != MyCondition.ShipDateTo)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_ShipDateTo(MyCondition.ShipDateTo.AddDays(1)));

                if (i > 0)
                {
                    sqlCtx = sqlCtxCllctn.MergeToOneAndQuery();

                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, (isJoin ? "t1." : "") + _Schema.Delivery.fn_DeliveryNo);

                    using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                    {
                        if (sqlR != null)
                        {
                            while (sqlR.Read())
                            {
                                if (totalLength < 1000)
                                {
                                    DNForUI item = new DNForUI();
                                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Cdt]);
                                    item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_DeliveryNo]);
                                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Editor]);
                                    item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Model]);
                                    item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_PoNo]);
                                    item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Qty]);
                                    item.ShipDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_ShipDate]);
                                    item.ShipmentID = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_ShipmentNo]);
                                    item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Status]);
                                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Udt]);
                                    ret.Add(item);
                                }
                                totalLength++;
                                sum = sum + GetValue_Int32(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Qty]);
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

        public IList<DNForUI> GetDNListByCondition(DNQueryCondition MyCondition)
        {
            try
            {
                IList<DNForUI> ret = new List<DNForUI>();

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlCtxCllctn = new _Schema.SQLContextCollection();
                bool isJoin = false;
                int i = 0;
                if (!string.IsNullOrEmpty(MyCondition.DNInfoValue))
                {
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_DNInfoValue(MyCondition.DNInfoValue));
                    isJoin = true;
                }

                if (!string.IsNullOrEmpty(MyCondition.DeliveryNo))
                {
                    if (MyCondition.DeliveryNo.Length == 16)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_DeliveryNo2(MyCondition.DeliveryNo, isJoin));
                    else if (MyCondition.DeliveryNo.Length == 10)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_DeliveryNo(MyCondition.DeliveryNo, isJoin));
                }

                if (!string.IsNullOrEmpty(MyCondition.Model))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_Model(MyCondition.Model));

                if (!string.IsNullOrEmpty(MyCondition.PONo))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_PONo(MyCondition.PONo));

                if (!string.IsNullOrEmpty(MyCondition.ShipmentNo))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_ShipmentNo(MyCondition.ShipmentNo));

                if (DateTime.MinValue != MyCondition.ShipDateFrom)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_ShipDateFrom(MyCondition.ShipDateFrom));

                if (DateTime.MinValue != MyCondition.ShipDateTo)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByCondition_ShipDateTo(MyCondition.ShipDateTo.AddDays(1)));

                if (i > 0)
                {
                    sqlCtx = sqlCtxCllctn.MergeToOneAndQuery();

                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, (isJoin ? "t1." : "") + _Schema.Delivery.fn_DeliveryNo);

                    using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                    {
                        if (sqlR != null)
                        {
                            while (sqlR.Read())
                            {
                                DNForUI item = new DNForUI();
                                item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Cdt]);
                                item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_DeliveryNo]);
                                item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Editor]);
                                item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Model]);
                                item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_PoNo]);
                                item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Qty]);
                                item.ShipDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_ShipDate]);
                                item.ShipmentID = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_ShipmentNo]);
                                item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Status]);
                                item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[(isJoin ? "t1_" : "") + _Schema.Delivery.fn_Udt]);
                                ret.Add(item);
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

        public IList<DNForUI> GetDNListByConditionWithSorting(DNQueryCondition MyCondition)
        {
            var res = GetDNListByCondition(MyCondition);
            return (from item in res orderby item.ShipDate, item.Qty, item.DeliveryNo select item).ToList();
        }

        public IList<Srd4CoaAndDn> GetDNListByConditionForPerformance(DNQueryCondition condition)
        {
            if (!string.IsNullOrEmpty(condition.DNInfoValue))
            {
                return GetDNListByConditionForPerformance_Join(condition);
            }
            else
            {
                return GetDNListByConditionForPerformance_NonJoin(condition);
            }
        }

        public IList<Srd4CoaAndDn> GetDNListByConditionForPerformanceWithSorting(DNQueryCondition MyCondition)
        {
            var res = GetDNListByConditionForPerformance(MyCondition);
            return (from item in res orderby item.ShipDate, item.Qty, item.DeliveryNO select item).ToList();
        }

        private IList<Srd4CoaAndDn> GetDNListByConditionForPerformance_NonJoin(DNQueryCondition condition)
        {
            try
            {
                IList<Srd4CoaAndDn> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {

                var condCol = new ConditionCollection<_Metas.Delivery>();

                var cond0 = new _Metas.Delivery();
                if (!string.IsNullOrEmpty(condition.DeliveryNo))
                {
                    if (condition.DeliveryNo.Length == 16)
                    {
                        cond0.deliveryNo = condition.DeliveryNo;
                        condCol.Add(new EqualCondition<_Metas.Delivery>(cond0));
                    }
                    else if (condition.DeliveryNo.Length == 10)
                    {
                        cond0.deliveryNo = condition.DeliveryNo + "%";
                        condCol.Add(new LikeCondition<_Metas.Delivery>(cond0));
                    }
                }

                var cond = new _Metas.Delivery();
                if (!string.IsNullOrEmpty(condition.Model))
                    cond.model = condition.Model;
                if (!string.IsNullOrEmpty(condition.PONo))
                    cond.poNo = condition.PONo;
                if (!string.IsNullOrEmpty(condition.ShipmentNo))
                    cond.shipmentNo = condition.ShipmentNo;

                var cond11 = new _Metas.Delivery();
                if (DateTime.MinValue != condition.ShipDateFrom)
                    cond11.shipDate = condition.ShipDateFrom;

                var cond12 = new _Metas.Delivery();
                if (DateTime.MinValue != condition.ShipDateTo)
                    cond12.shipDate = condition.ShipDateTo.AddDays(1);

                var cond13 = new _Metas.Delivery();
                cond13.status = "00";

                var cond14 = new _Metas.Delivery();
                cond14.model = "PC";

                var cond15 = new _Metas.Delivery();
                cond15.qty = 88;

                condCol.AddRange(
                            new EqualCondition<_Metas.Delivery>(cond),
                            new GreaterOrEqualCondition<_Metas.Delivery>(cond11),
                            new SmallerCondition<_Metas.Delivery>(cond12),
                            new EqualCondition<_Metas.Delivery>(cond13),
                            new AnyCondition<_Metas.Delivery>(cond14, "(LEN({0})=12 AND SUBSTRING({0},1,2)={1})"),
                            new AnySoloCondition<_Metas.Delivery>(cond15, string.Format("{0}>=(SELECT COUNT(1) FROM {1}..{2} WHERE {2}.{3}={4}.{5})", "{0}", _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_deliveryNo, ToolsNew.GetTableName(typeof(_Metas.Delivery)), _Metas.Delivery.fn_deliveryNo))
                            );

                sqlCtx = FuncNew.GetConditionedSelectForFuncedField("DISTINCT", new string[][] { 
                            new string[] { _Metas.Delivery.fn_cdt,_Metas.Delivery.fn_cdt },
                            new string[] { _Metas.Delivery.fn_deliveryNo, _Metas.Delivery.fn_deliveryNo},
                            new string[] { _Metas.Delivery.fn_editor, _Metas.Delivery.fn_editor},
                            new string[] { _Metas.Delivery.fn_model, _Metas.Delivery.fn_model},
                            new string[] { _Metas.Delivery.fn_poNo, _Metas.Delivery.fn_poNo},
                            new string[] { _Metas.Delivery.fn_qty, _Metas.Delivery.fn_qty},
                            new string[] { _Metas.Delivery.fn_shipDate, _Metas.Delivery.fn_shipDate},
                            new string[] { _Metas.Delivery.fn_shipmentNo,_Metas.Delivery.fn_shipmentNo },
                            new string[] { _Metas.Delivery.fn_status, _Metas.Delivery.fn_status},
                            new string[] { _Metas.Delivery.fn_udt,_Metas.Delivery.fn_udt},
                            }, condCol, _Metas.Delivery.fn_deliveryNo);
                sqlCtx.Sentence = sqlCtx.Sentence.Substring(0, sqlCtx.Sentence.IndexOf(" FROM")) + "," +
                            string.Format("(SELECT TOP 1 {0} FROM {1} WHERE {1}.{2}={3}.{4} AND {5}='PartNo')", _Metas.DeliveryInfo.fn_infoValue, ToolsNew.GetTableName(typeof(_Metas.DeliveryInfo)), _Metas.DeliveryInfo.fn_deliveryNo, ToolsNew.GetTableName(typeof(_Metas.Delivery)), _Metas.Delivery.fn_deliveryNo, _Metas.DeliveryInfo.fn_infoType)
                            + "," +
                            string.Format("{0}(SELECT COUNT(1) FROM {1}..{2} WHERE {2}.{3}={4}.{5})", "", _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_deliveryNo, ToolsNew.GetTableName(typeof(_Metas.Delivery)), _Metas.Delivery.fn_deliveryNo)
                            + sqlCtx.Sentence.Substring(sqlCtx.Sentence.IndexOf(" FROM"));

                sqlCtx.Param(_Metas.Delivery.fn_status).Value = cond13.status;
                sqlCtx.Param(g.DecAny(_Metas.Delivery.fn_model)).Value = cond14.model;
                //    }
                //}

                if (!string.IsNullOrEmpty(condition.DeliveryNo))
                {
                    if (condition.DeliveryNo.Length == 16)
                        sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = condition.DeliveryNo;
                    else if (condition.DeliveryNo.Length == 10)
                        sqlCtx.Param( _Metas.Delivery.fn_deliveryNo).Value = condition.DeliveryNo + "%";
                }

                if (!string.IsNullOrEmpty(condition.Model))
                    sqlCtx.Param(_Metas.Delivery.fn_model).Value = condition.Model;
                if (!string.IsNullOrEmpty(condition.PONo))
                    sqlCtx.Param(_Metas.Delivery.fn_poNo).Value = condition.PONo;
                if (!string.IsNullOrEmpty(condition.ShipmentNo))
                    sqlCtx.Param(_Metas.Delivery.fn_shipmentNo).Value = condition.ShipmentNo;

                if (DateTime.MinValue != condition.ShipDateFrom)
                    sqlCtx.Param(g.DecGE(_Metas.Delivery.fn_shipDate)).Value = condition.ShipDateFrom;

                if (DateTime.MinValue != condition.ShipDateTo)
                    sqlCtx.Param(g.DecS(_Metas.Delivery.fn_shipDate)).Value = condition.ShipDateTo.AddDays(1);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<Srd4CoaAndDn>();
                        while (sqlR.Read())
                        {
                            var item = new Srd4CoaAndDn();
                            item.CustomerPN = GetValue_Str(sqlR, sqlCtx.IndexCount);
                            item.ShipDate = GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_shipDate));
                            item.DeliveryNO = GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_deliveryNo));
                            item.Model = GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_model));
                            item.PackedQty = GetValue_Int32(sqlR, sqlCtx.IndexCount + 1);
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_poNo));
                            item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_qty));

                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_cdt));
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_editor));
                            item.ShipmentID = GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_shipmentNo));
                            item.Status = GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_status));
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Delivery.fn_udt));

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

        private IList<Srd4CoaAndDn> GetDNListByConditionForPerformance_Join(DNQueryCondition condition)
        {
            try
            {
                IList<Srd4CoaAndDn> ret = null;

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                        #region Delivery

                        tf1 = new TableAndFields<_Metas.Delivery>();

                        var cond0 = new _Metas.Delivery();
                        if (!string.IsNullOrEmpty(condition.DeliveryNo))
                        {
                            if (condition.DeliveryNo.Length == 16)
                            {
                                cond0.deliveryNo = condition.DeliveryNo;
                                tf1.Conditions.Add(new EqualCondition<_Metas.Delivery>(cond0));
                            }
                            else if (condition.DeliveryNo.Length == 10)
                            {
                                cond0.deliveryNo = condition.DeliveryNo + "%";
                                tf1.Conditions.Add(new LikeCondition<_Metas.Delivery>(cond0));
                            }
                        }

                        var cond = new _Metas.Delivery();
                        if (!string.IsNullOrEmpty(condition.Model))
                            cond.model = condition.Model;
                        if (!string.IsNullOrEmpty(condition.PONo))
                            cond.poNo = condition.PONo;
                        if (!string.IsNullOrEmpty(condition.ShipmentNo))
                            cond.shipmentNo = condition.ShipmentNo;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Delivery>(cond));

                        var cond11 = new _Metas.Delivery();
                        if (DateTime.MinValue != condition.ShipDateFrom)
                            cond11.shipDate = condition.ShipDateFrom;
                        tf1.Conditions.Add(new GreaterOrEqualCondition<_Metas.Delivery>(cond11));

                        var cond12 = new _Metas.Delivery();
                        if (DateTime.MinValue != condition.ShipDateTo)
                            cond12.shipDate = condition.ShipDateTo.AddDays(1);
                        tf1.Conditions.Add(new SmallerCondition<_Metas.Delivery>(cond12));

                        var cond13 = new _Metas.Delivery();
                        cond13.status = "00";
                        tf1.Conditions.Add(new EqualCondition<_Metas.Delivery>(cond13));

                        var cond14 = new _Metas.Delivery();
                        cond14.model = "PC";
                        tf1.Conditions.Add(new AnyCondition<_Metas.Delivery>(cond14, "(LEN({0})=12 AND SUBSTRING({0},1,2)={1})"));

                        var cond15 = new _Metas.Delivery();
                        cond15.qty = 88;
                        tf1.Conditions.Add(new AnySoloCondition<_Metas.Delivery>(cond15, string.Format("{0}>=(SELECT COUNT(1) FROM {1}..{2} WHERE {2}.{3}={4}.{5})", "{0}", _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_deliveryNo, "t1", _Metas.Delivery.fn_deliveryNo)));


                        #endregion

                        #region DeliveryInfo

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        var cond3 = new _Metas.DeliveryInfo();
                        cond3.infoValue = condition.DNInfoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond3));

                        tf2.AddRangeToGetFuncedFieldNames(
                            new string[] { _Metas.DeliveryInfo.fn_infoValue, string.Format("(SELECT TOP 1 {0} FROM {1} WHERE {1}.{2}={3}.{4} AND {5}='PartNo')", _Metas.DeliveryInfo.fn_infoValue, ToolsNew.GetTableName(typeof(_Metas.DeliveryInfo)), _Metas.DeliveryInfo.fn_deliveryNo, "t2", _Metas.DeliveryInfo.fn_deliveryNo, _Metas.DeliveryInfo.fn_infoType) },
                            new string[] { _Metas.DeliveryInfo.fn_id, string.Format("{0}(SELECT COUNT(1) FROM {1}..{2} WHERE {2}.{3}={4}.{5})", "", _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_deliveryNo, "t1", _Metas.Delivery.fn_deliveryNo) }
                            );

                        #endregion

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect("DISTINCT", tafa, tblCnnts, "t1." + _Metas.Delivery.fn_deliveryNo);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_status)).Value = cond13.status;
                        sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecAny(_Metas.Delivery.fn_model))).Value = cond14.model;

                //    }
                //}
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                if (!string.IsNullOrEmpty(condition.DeliveryNo))
                {
                    if (condition.DeliveryNo.Length == 16)
                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_deliveryNo)).Value = condition.DeliveryNo;
                    else if (condition.DeliveryNo.Length == 10)
                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_deliveryNo)).Value = condition.DeliveryNo + "%";
                }

                if (!string.IsNullOrEmpty(condition.Model))
                    sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_model)).Value = condition.Model;
                if (!string.IsNullOrEmpty(condition.PONo))
                    sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_poNo)).Value = condition.PONo;
                if (!string.IsNullOrEmpty(condition.ShipmentNo))
                    sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_shipmentNo)).Value = condition.ShipmentNo;

                if (DateTime.MinValue != condition.ShipDateFrom)
                    sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecGE(_Metas.Delivery.fn_shipDate))).Value = condition.ShipDateFrom;

                if (DateTime.MinValue != condition.ShipDateTo)
                    sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecS(_Metas.Delivery.fn_shipDate))).Value = condition.ShipDateTo.AddDays(1);

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = condition.DNInfoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<Srd4CoaAndDn>();
                        while (sqlR.Read())
                        {
                            var item = new Srd4CoaAndDn();
                            item.CustomerPN = GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)));
                            item.ShipDate = GetValue_DateTime(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_shipDate)));
                            item.DeliveryNO = GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_deliveryNo)));
                            item.Model = GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_model)));
                            item.PackedQty = GetValue_Int32(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_id)));
                            item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_poNo)));
                            item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_qty)));

                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_cdt)));
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_editor)));
                            item.ShipmentID = GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_shipmentNo)));
                            item.Status = GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_status)));
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_udt)));

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

        private _Schema.SQLContext ComposeForGetDNListByCondition_DeliveryNo(string deliveryNo, bool isJoin)
        {
            _Schema.SQLContext ret = null;
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery likecond = new _Schema.Delivery();
                    likecond.DeliveryNo = deliveryNo + "%";
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, null, null, likecond, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = deliveryNo + "%";
            
            ret = new _Schema.SQLContext(sqlCtx);
            if (isJoin)
                ret.Sentence = sqlCtx.Sentence.Replace(_Schema.Delivery.fn_DeliveryNo + " LIKE", "t1." + _Schema.Delivery.fn_DeliveryNo + " LIKE");
            return ret;
        }

        private _Schema.SQLContext ComposeForGetDNListByCondition_DeliveryNo2(string deliveryNo, bool isJoin)
        {
            _Schema.SQLContext ret = null;
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery equalCond = new _Schema.Delivery();
                    equalCond.DeliveryNo = deliveryNo;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, null, equalCond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = deliveryNo;

            ret = new _Schema.SQLContext(sqlCtx);
            if (isJoin)
                ret.Sentence = sqlCtx.Sentence.Replace(_Schema.Delivery.fn_DeliveryNo + "=", "t1." + _Schema.Delivery.fn_DeliveryNo + "=");
            return ret;
        }

        private _Schema.SQLContext ComposeForGetDNListByCondition_PONo(string pono)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery equalCond = new _Schema.Delivery();
                    equalCond.PoNo = pono;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, null, equalCond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_PoNo].Value = pono;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetDNListByCondition_Model(string model)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery equalCond = new _Schema.Delivery();
                    equalCond.Model = model;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, null, equalCond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_Model].Value = model;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetDNListByCondition_ShipmentNo(string shipmentNo)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery equalCond = new _Schema.Delivery();
                    equalCond.ShipmentNo = shipmentNo;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, null, equalCond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_ShipmentNo].Value = shipmentNo;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetDNListByCondition_DNInfoValue(string dnInfoValue)
        {
            _Schema.SQLContext sqlCtx = null;

            _Schema.TableAndFields tf1 = null;
            _Schema.TableAndFields tf2 = null;
            _Schema.TableAndFields[] tblAndFldsesArray = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                {
                    tf1 = new _Schema.TableAndFields();
                    tf1.Table = typeof(_Schema.Delivery);

                    tf2 = new _Schema.TableAndFields();
                    tf2.Table = typeof(_Schema.DeliveryInfo);
                    _Schema.DeliveryInfo cond2 = new _Schema.DeliveryInfo();
                    cond2.InfoValue = dnInfoValue;
                    tf2.equalcond = cond2;
                    tf2.ToGetFieldNames = null;

                    List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                    _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_DeliveryNo, tf2, _Schema.DeliveryInfo.fn_DeliveryNo);
                    tblCnntIs.Add(tc1);

                    _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                    tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                    sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);
                }
            }
            tf1 = tblAndFldsesArray[0];
            tf2 = tblAndFldsesArray[1];

            sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue)].Value = dnInfoValue;

            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetDNListByCondition_ShipDateFrom(DateTime shipDateFrom)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery geCond = new _Schema.Delivery();
                    geCond.ShipDate = shipDateFrom;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, null, null, null, null, null, null, geCond, null, null);
                }
            }
            sqlCtx.Params[_Schema.Func.DecGE(_Schema.Delivery.fn_ShipDate)].Value = shipDateFrom;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForGetDNListByCondition_ShipDateTo(DateTime shipDateTo)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery sCond = new _Schema.Delivery();
                    sCond.ShipDate = shipDateTo;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, null, null, null, null, null, sCond, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Func.DecS(_Schema.Delivery.fn_ShipDate)].Value = shipDateTo;
            return sqlCtx;
        }

        public IList<DNInfoForUI> GetDNInfoList(string dn)
        {
            try
            {
                IList<DNInfoForUI> ret = new List<DNInfoForUI>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DeliveryInfo cond = new _Schema.DeliveryInfo();
                        cond.DeliveryNo = dn;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DeliveryInfo), null, new List<string>() { _Schema.DeliveryInfo.fn_ID, _Schema.DeliveryInfo.fn_InfoType, _Schema.DeliveryInfo.fn_InfoValue }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.DeliveryInfo.fn_InfoType);
                    }
                }
                sqlCtx.Params[_Schema.DeliveryInfo.fn_DeliveryNo].Value = dn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DNInfoForUI item = new DNInfoForUI();
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.DeliveryInfo.fn_ID]);
                        item.InfoType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DeliveryInfo.fn_InfoType]);
                        item.InfoValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DeliveryInfo.fn_InfoValue]);
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

        public IList<DNPalletQty> GetPalletList(string dn)
        {
            try
            {
                IList<DNPalletQty> ret = new List<DNPalletQty>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Delivery_Pallet);
                        _Schema.Delivery_Pallet cond1 = new _Schema.Delivery_Pallet();
                        cond1.DeliveryNo = dn;
                        tf1.equalcond = cond1;
                        tf1.ToGetFieldNames.Add(_Schema.Delivery_Pallet.fn_DeliveryQty);
                        tf1.ToGetFieldNames.Add(_Schema.Delivery_Pallet.fn_ID);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Pallet);
                        tf2.ToGetFieldNames.Add(_Schema.Pallet.fn_PalletNo);
                        tf2.ToGetFieldNames.Add(_Schema.Pallet.fn_UCC);

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery_Pallet.fn_PalletNo, tf2, _Schema.Pallet.fn_PalletNo);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf2.alias, _Schema.Pallet.fn_PalletNo));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery_Pallet.fn_DeliveryNo)].Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        DNPalletQty item = new DNPalletQty();
                        item.DeliveryQty = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery_Pallet.fn_DeliveryQty)]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery_Pallet.fn_ID)]);
                        item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.Pallet.fn_PalletNo)]);
                        item.UCC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.Pallet.fn_UCC)]);
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

        private string GetValueFromConstValue(string type, string name)
        {
            string ret = string.Empty;
            ConstValueInfo condition = new ConstValueInfo();
            condition.type = type;
            condition.name = name;
            var res = PrtRepository.GetConstValueInfoList(condition);
            if (res != null && res.Count > 0)
            {
                ret = res[0].value;
            }
            return ret;
        }

        private bool JudgeIs(Delivery dnObj, string pattern)
        {
            if (dnObj != null && !string.IsNullOrEmpty(dnObj.ModelName) && !string.IsNullOrEmpty(pattern))
                return Regex.IsMatch(dnObj.ModelName, pattern);
            else
                return false;
        }

        public IList<DNPalletQty> GetPalletList2(string[] dnList)
        {
            try
            {
                // 将dnList分为整机dn列表和docking dn列表
                IList<string> skuDnlist = new List<string>();
                IList<string> dockingDnlist = new List<string>();
                // 使用Delivery的Model，用正则表达式匹配，确定属于哪个集合

                //访问ConstValue 表，取得ConstValue.Type = 'ProcReg' 的记录，
                //这些记录的Name 字段为SKU / Docking 等名称，Value 为正则表达式；
                //使用Model 依次配置这些记录的正则表达式，匹配上的记录的Name 字段即可确定SKU / Docking...
                #region Vincent mark this code
                //string skuPattern = GetValueFromConstValue("ProcReg", "SKU");
                //string dockingPattern = GetValueFromConstValue("ProcReg", "Docking");
                //string ThinClientPattern = GetValueFromConstValue("ProcReg", "ThinClient");
                //string TabletPattern = GetValueFromConstValue("ProcReg", "Tablet");


                //foreach(string dn in dnList)
                //{
                //    var dnObj = this.Find(dn);
                //    if (dnObj != null)
                //    {
                //        if (JudgeIs(dnObj, skuPattern))
                //            skuDnlist.Add(dn);
                //        else if (JudgeIs(dnObj, dockingPattern))
                //            dockingDnlist.Add(dn);
                //        else if (JudgeIs(dnObj, ThinClientPattern))
                //            skuDnlist.Add(dn);
                //        else if (JudgeIs(dnObj, TabletPattern))
                //            skuDnlist.Add(dn);
                //    }
                //}
                #endregion

                ConstValueInfo condition = new ConstValueInfo();
                condition.type = "ProcReg";
                IList<ConstValueInfo> procTypeList = PrtRepository.GetConstValueInfoList(condition);

                foreach (string dn in dnList)
                {
                    var dnObj = this.Find(dn);
                    if (dnObj != null)
                    {
                        if (procTypeList.Any(x => x.description.TrimEnd() == "Y" && JudgeIs(dnObj, x.value)))
                        {
                            skuDnlist.Add(dn);
                        }
                        else
                        {
                            dockingDnlist.Add(dn);
                        }  
                    }
                }                
                IList<DNPalletQty> ret = new List<DNPalletQty>();

                var ret1 = GetPalletList2_NonBulk(skuDnlist.ToArray());
                //var ret2 = GetPalletList2_Bulk(skuDnlist.ToArray(), new string[] { "00", "01" });
                //var ret3 = GetPalletList2_Bulk(dockingDnlist.ToArray(), new string[] { "00", "01", "NA" });
                var ret2 = GetPalletList3_Bulk(skuDnlist.ToArray(), "[0-9][0-9]");
                var ret3 = GetPalletList3_Bulk(dockingDnlist.ToArray(), "");

                if (ret1 != null && ret1.Count > 0)
                {
                    foreach (var item in ret1)
                        ret.Add(item);
                }
                if (ret2 != null && ret2.Count > 0)
                {
                    foreach (var item in ret2)
                        ret.Add(item);
                }
                if (ret3 != null && ret3.Count > 0)
                {
                    foreach (var item in ret3)
                        ret.Add(item);
                }
                return (from c in ret orderby c.PalletNo select c).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<DNPalletQty> GetPalletList2FromView(string[] dnList)
        {
            try
            {
                // 将dnList分为整机dn列表和docking dn列表
                //skuDnList 是整機DN包含正常及虛擬棧板
                //dockingDnList是DN包含正常及散裝棧板

                IList<string> skuDnlist = new List<string>();
                IList<string> dockingDnlist = new List<string>();
                // 使用Delivery的Model，用正则表达式匹配，确定属于哪个集合

                //访问ConstValue 表，取得ConstValue.Type = 'ProcReg' 的记录，
                //这些记录的Name 字段为SKU / Docking 等名称，Value 为正则表达式；
                //使用Model 依次配置这些记录的正则表达式，匹配上的记录的Name 字段即可确定SKU / Docking...
                #region Vincent mark this code
                //string skuPattern = GetValueFromConstValue("ProcReg", "SKU");
                //string dockingPattern = GetValueFromConstValue("ProcReg", "Docking");
                //string ThinClientPattern = GetValueFromConstValue("ProcReg", "ThinClient");
                //string TabletPattern = GetValueFromConstValue("ProcReg", "Tablet");

                //foreach (string dn in dnList)
                //{
                //    var dnObj = this.FindFromView(dn);
                //    if (dnObj != null)
                //    {
                //        if (JudgeIs(dnObj, skuPattern))
                //            skuDnlist.Add(dn);
                //        else if (JudgeIs(dnObj, dockingPattern))
                //            dockingDnlist.Add(dn);
                //        else if (JudgeIs(dnObj, ThinClientPattern))
                //            skuDnlist.Add(dn);
                //        else if (JudgeIs(dnObj, TabletPattern))
                //            skuDnlist.Add(dn);
                //    }
                //}

                #endregion
                ConstValueInfo condition = new ConstValueInfo();
                condition.type = "ProcReg";               
                IList<ConstValueInfo> procTypeList = PrtRepository.GetConstValueInfoList(condition);

                foreach (string dn in dnList)
                {
                    var dnObj = this.Find(dn);
                    if (dnObj != null)
                    {
                        
                        if (procTypeList.Any(x => x.description.TrimEnd() == "Y" && JudgeIs(dnObj, x.value)))
                        {
                            skuDnlist.Add(dn);
                        }
                        else
                        {
                            dockingDnlist.Add(dn);
                        }                      
                    }
                }                

                IList<DNPalletQty> ret = new List<DNPalletQty>();

                var ret1 = GetPalletList2FromView_NonBulk(skuDnlist.ToArray());
                //var ret2 = GetPalletList2FromView_Bulk(skuDnlist.ToArray(), new string[] { "00", "01" });
                //var ret3 = GetPalletList2FromView_Bulk(dockingDnlist.ToArray(), new string[] { "00", "01", "NA" });
                var ret2 = GetPalletList3FromView_Bulk(skuDnlist.ToArray(), "[0-9][0-9]");
                var ret3 = GetPalletList3FromView_Bulk(dockingDnlist.ToArray(), "" );
                
                if (ret1 != null && ret1.Count > 0)
                {
                    foreach (var item in ret1)
                        ret.Add(item);
                }
                if (ret2 != null && ret2.Count > 0)
                {
                    foreach (var item in ret2)
                        ret.Add(item);
                }
                if (ret3 != null && ret3.Count > 0)
                {
                    foreach (var item in ret3)
                        ret.Add(item);
                }
                return (from c in ret orderby c.PalletNo select c).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public fons::Delivery FindFromView(object key)
        {
            try
            {
                fons::Delivery ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.V_Delivery cond = new _Schema.V_Delivery();
                        cond.DeliveryNo = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.V_Delivery), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.V_Delivery.fn_DeliveryNo].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons::Delivery();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_Editor]);
                        ret.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_DeliveryNo]);
                        ret.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_Model]);
                        ret.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_PoNo]);
                        ret.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_Qty]);
                        ret.ShipDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_ShipDate]);
                        ret.ShipmentNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_ShipmentNo]);
                        ret.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_Status]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.V_Delivery.fn_Udt]);
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
        private IList<DNPalletQty> GetPalletList2FromView_NonBulk(string[] dnList)
        {
            try
            {
                IList<DNPalletQty> ret = null;

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
                        tf1 = new TableAndFields<_Metas.V_DeliveryInfo>();
                        _Metas.V_DeliveryInfo cond = new _Metas.V_DeliveryInfo();
                        cond.infoType = "BOL";
                        tf1.Conditions.Add(new EqualCondition<_Metas.V_DeliveryInfo>(cond));
                        _Metas.V_DeliveryInfo cond2 = new _Metas.V_DeliveryInfo();
                        cond2.deliveryNo = "[INSET]";
                        tf1.Conditions.Add(new InSetCondition<_Metas.V_DeliveryInfo>(cond2));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<V_Dummy_ShipDet>();
                        tf2.AddRangeToGetFuncedFieldNames(new string[][]{
                            new string[]{_Metas.V_Dummy_ShipDet.fn_plt, _Metas.V_Dummy_ShipDet.fn_plt},
                            new string[]{_Metas.V_Dummy_ShipDet.fn_snoId, string.Format("COUNT(DISTINCT {0})", _Metas.V_Dummy_ShipDet.fn_snoId)}
                            }
                            );

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.V_DeliveryInfo, V_Dummy_ShipDet>(tf1, _Metas.V_DeliveryInfo.fn_infoValue, tf2, V_Dummy_ShipDet.fn_bol));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.V_DeliveryInfo.fn_infoType)).Value = cond.infoType;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                string Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias, g.DecInSet(mtns::DeliveryInfo.fn_deliveryNo)), g.ConvertInSet(new List<string>(dnList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while (sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.DeliveryQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns::V_Dummy_ShipDet.fn_snoId)));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns::V_Dummy_ShipDet.fn_plt)));
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

        private IList<DNPalletQty> GetPalletList2FromView_Bulk(string[] dnList, string[] pltNoPrefixes)
        {
            try
            {
                IList<DNPalletQty> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::V_Delivery_Pallet cond = new mtns::V_Delivery_Pallet();
                        cond.deliveryNo = "[INSET]";
                        mtns::V_Delivery_Pallet cond2 = new mtns::V_Delivery_Pallet();
                        cond2.palletNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns::V_Delivery_Pallet>(tk, null,
                            new string[][]
                            {
                                new string[]{mtns::V_Delivery_Pallet.fn_palletNo, mtns::V_Delivery_Pallet.fn_palletNo},
                                new string[]{mtns::V_Delivery_Pallet.fn_deliveryQty, string.Format("SUM({0})",mtns::V_Delivery_Pallet.fn_deliveryQty)}
                            }, new ConditionCollection<mtns::V_Delivery_Pallet>(
                                new InSetCondition<mtns::V_Delivery_Pallet>(cond),
                                new InSetCondition<mtns::V_Delivery_Pallet>(cond2, "LEFT({0},2)")), mtns::V_Delivery_Pallet.fn_palletNo);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::V_Delivery_Pallet.fn_deliveryNo), g.ConvertInSet(new List<string>(dnList)));
                Sentence = Sentence.Replace(g.DecInSet(mtns::V_Delivery_Pallet.fn_palletNo), g.ConvertInSet(new List<string>(pltNoPrefixes)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while (sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.DeliveryQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns::V_Delivery_Pallet.fn_deliveryQty));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Delivery_Pallet.fn_palletNo));
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

        private IList<DNPalletQty> GetPalletList3FromView_Bulk(string[] dnList, string pltNoPrefixes)
        {
            try
            {
                IList<DNPalletQty> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::V_Delivery_Pallet cond = new mtns::V_Delivery_Pallet();
                        cond.deliveryNo = "[INSET]";
                        mtns::V_Delivery_Pallet cond2 = new mtns::V_Delivery_Pallet();
                        cond2.palletNo = pltNoPrefixes;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns::V_Delivery_Pallet>(tk, null,
                            new string[][]
                            {
                                new string[]{mtns::V_Delivery_Pallet.fn_palletNo, mtns::V_Delivery_Pallet.fn_palletNo},
                                new string[]{mtns::V_Delivery_Pallet.fn_deliveryQty, string.Format("SUM({0})",mtns::V_Delivery_Pallet.fn_deliveryQty)}
                            }, new ConditionCollection<mtns::V_Delivery_Pallet>(
                                new InSetCondition<mtns::V_Delivery_Pallet>(cond),
                                new LikeCondition<mtns::V_Delivery_Pallet>(cond2,mtns::V_Delivery_Pallet.fn_palletNo)),mtns::V_Delivery_Pallet.fn_palletNo);
                                //new InSetCondition<mtns::V_Delivery_Pallet>(cond2, "LEFT({0},2)")), mtns::V_Delivery_Pallet.fn_palletNo);
                    }
                }
                sqlCtx.Param(mtns::V_Delivery_Pallet.fn_palletNo).Value = pltNoPrefixes+"%";
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::V_Delivery_Pallet.fn_deliveryNo), g.ConvertInSet(new List<string>(dnList)));
                //Sentence = Sentence.Replace(g.DecInSet(mtns::V_Delivery_Pallet.fn_palletNo), g.ConvertInSet(new List<string>(pltNoPrefixes)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while (sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.DeliveryQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns::V_Delivery_Pallet.fn_deliveryQty));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::V_Delivery_Pallet.fn_palletNo));
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

        private IList<DNPalletQty> GetPalletList2_NonBulk(string[] dnList)
        {
            try
            {
                IList<DNPalletQty> ret = null;

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
                        tf1 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoType = "BOL";
                        tf1.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond));
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.deliveryNo = "[INSET]";
                        tf1.Conditions.Add(new InSetCondition<_Metas.DeliveryInfo>(cond2));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<Dummy_ShipDet>();
                        tf2.AddRangeToGetFuncedFieldNames(new string[][]{
                            new string[]{_Metas.Dummy_ShipDet.fn_plt, _Metas.Dummy_ShipDet.fn_plt},
                            new string[]{_Metas.Dummy_ShipDet.fn_snoId, string.Format("COUNT(DISTINCT {0})", _Metas.Dummy_ShipDet.fn_snoId)}
                            }
                            );

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.DeliveryInfo, Dummy_ShipDet>(tf1, _Metas.DeliveryInfo.fn_infoValue, tf2, Dummy_ShipDet.fn_bol));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = cond.infoType;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                string Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias, g.DecInSet(mtns::DeliveryInfo.fn_deliveryNo)), g.ConvertInSet(new List<string>(dnList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while (sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.DeliveryQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns::Dummy_ShipDet.fn_snoId)));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns::Dummy_ShipDet.fn_plt)));
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

        private IList<DNPalletQty> GetPalletList2_Bulk(string[] dnList, string[] pltNoPrefixes)
        {
            try
            {
                IList<DNPalletQty> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Delivery_Pallet cond = new mtns::Delivery_Pallet();
                        cond.deliveryNo = "[INSET]";
                        mtns::Delivery_Pallet cond2 = new mtns::Delivery_Pallet();
                        cond2.palletNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns::Delivery_Pallet>(tk, null, 
                            new string[][]
                            {
                                new string[]{mtns::Delivery_Pallet.fn_palletNo, mtns::Delivery_Pallet.fn_palletNo},
                                new string[]{mtns::Delivery_Pallet.fn_deliveryQty, string.Format("SUM({0})",mtns::Delivery_Pallet.fn_deliveryQty)}
                            }, new ConditionCollection<mtns::Delivery_Pallet>(
                                new InSetCondition<mtns::Delivery_Pallet>(cond),
                                new InSetCondition<mtns::Delivery_Pallet>(cond2, "LEFT({0},2)")), mtns::Delivery_Pallet.fn_palletNo);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::Delivery_Pallet.fn_deliveryNo), g.ConvertInSet(new List<string>(dnList)));
                Sentence = Sentence.Replace(g.DecInSet(mtns::Delivery_Pallet.fn_palletNo), g.ConvertInSet(new List<string>(pltNoPrefixes)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while(sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.DeliveryQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns::Delivery_Pallet.fn_deliveryQty));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery_Pallet.fn_palletNo));
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


        private IList<DNPalletQty> GetPalletList3_Bulk(string[] dnList, string pltNoPrefixes)
        {
            try
            {
                IList<DNPalletQty> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Delivery_Pallet cond = new mtns::Delivery_Pallet();
                        cond.deliveryNo = "[INSET]";
                        mtns::Delivery_Pallet cond2 = new mtns::Delivery_Pallet();
                        cond2.palletNo = pltNoPrefixes;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns::Delivery_Pallet>(tk, null,
                            new string[][]
                            {
                                new string[]{mtns::Delivery_Pallet.fn_palletNo, mtns::Delivery_Pallet.fn_palletNo},
                                new string[]{mtns::Delivery_Pallet.fn_deliveryQty, string.Format("SUM({0})",mtns::Delivery_Pallet.fn_deliveryQty)}
                            }, new ConditionCollection<mtns::Delivery_Pallet>(
                                new InSetCondition<mtns::Delivery_Pallet>(cond),
                                new LikeCondition<mtns::Delivery_Pallet>(cond2, mtns::Delivery_Pallet.fn_palletNo)), mtns::Delivery_Pallet.fn_palletNo);
                    }
                }
                sqlCtx.Param(mtns::Delivery_Pallet.fn_palletNo).Value = pltNoPrefixes + "%";
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::Delivery_Pallet.fn_deliveryNo), g.ConvertInSet(new List<string>(dnList)));
                

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while (sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.DeliveryQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns::Delivery_Pallet.fn_deliveryQty));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery_Pallet.fn_palletNo));
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

        public void UpdateDNByCondition(DNUpdateCondition myCondition, string editor)
        {
            try
            {
                SqlTransactionManager.Begin();

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlCtxCllctn = new _Schema.SQLContextCollection();

                int i = 0;
                if (!string.IsNullOrEmpty(myCondition.Model))
                    sqlCtxCllctn.AddOne(i++, ComposeForUpdateDNByCondition_Model(myCondition.DeliveryNo, myCondition.Model, editor));
                if (!string.IsNullOrEmpty(myCondition.PoNo))
                    sqlCtxCllctn.AddOne(i++, ComposeForUpdateDNByCondition_PoNo(myCondition.DeliveryNo, myCondition.PoNo, editor));
                if (!string.IsNullOrEmpty(myCondition.ShipmentNo))
                    sqlCtxCllctn.AddOne(i++, ComposeForUpdateDNByCondition_ShipmentNo(myCondition.DeliveryNo, myCondition.ShipmentNo, editor));
                if (DateTime.MinValue != myCondition.ShipDate)
                    sqlCtxCllctn.AddOne(i++, ComposeForUpdateDNByCondition_ShipDate(myCondition.DeliveryNo, myCondition.ShipDate, editor));

                if (i > 0)
                {
                    sqlCtx = sqlCtxCllctn.MergeToOneNonQuery();
                    _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
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

        private _Schema.SQLContext ComposeForUpdateDNByCondition_ShipmentNo(string dn, string shipmentNo, string editor)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery cond = new _Schema.Delivery();
                    cond.DeliveryNo = dn;
                    sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), new List<string>() { _Schema.Delivery.fn_ShipmentNo, _Schema.Delivery.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = dn;
            DateTime cmDt = _Schema.SqlHelper.GetDateTime();
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_ShipmentNo)].Value = shipmentNo;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Editor)].Value = editor;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Udt)].Value = cmDt;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForUpdateDNByCondition_PoNo(string dn, string poNo, string editor)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery cond = new _Schema.Delivery();
                    cond.DeliveryNo = dn;
                    sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), new List<string>() { _Schema.Delivery.fn_PoNo, _Schema.Delivery.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = dn;
            DateTime cmDt = _Schema.SqlHelper.GetDateTime();
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_PoNo)].Value = poNo;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Editor)].Value = editor;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Udt)].Value = cmDt;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForUpdateDNByCondition_Model(string dn, string model, string editor)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery cond = new _Schema.Delivery();
                    cond.DeliveryNo = dn;
                    sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), new List<string>() { _Schema.Delivery.fn_Model, _Schema.Delivery.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = dn;
            DateTime cmDt = _Schema.SqlHelper.GetDateTime();
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Model)].Value = model;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Editor)].Value = editor;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Udt)].Value = cmDt;
            return sqlCtx;
        }

        private _Schema.SQLContext ComposeForUpdateDNByCondition_ShipDate(string dn, DateTime shipDate, string editor)
        {
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.Delivery cond = new _Schema.Delivery();
                    cond.DeliveryNo = dn;
                    sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), new List<string>() { _Schema.Delivery.fn_ShipDate, _Schema.Delivery.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = dn;
            DateTime cmDt = _Schema.SqlHelper.GetDateTime();
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_ShipDate)].Value = shipDate;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Editor)].Value = editor;
            sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Udt)].Value = cmDt;
            return sqlCtx;
        }

        public void UpdateDeliverInfoByID(int deliverInfoID, string infoValue, string editor)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.DeliveryInfo cond = new _Schema.DeliveryInfo();
                        cond.ID = deliverInfoID;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DeliveryInfo), new List<string>() { _Schema.DeliveryInfo.fn_InfoValue, _Schema.DeliveryInfo.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DeliveryInfo.fn_ID].Value = deliverInfoID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.DeliveryInfo.fn_InfoValue)].Value = infoValue;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.DeliveryInfo.fn_Editor)].Value = editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.DeliveryInfo.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDeliverQty(int deliveryPalletID, short deliveryQty, string editor)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Delivery_Pallet cond = new _Schema.Delivery_Pallet();
                        cond.ID = deliveryPalletID;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet), new List<string>() { _Schema.Delivery_Pallet.fn_DeliveryQty, _Schema.Delivery_Pallet.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_ID].Value = deliveryPalletID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery_Pallet.fn_DeliveryQty)].Value = deliveryQty;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery_Pallet.fn_Editor)].Value = editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery_Pallet.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Delivery FillDeliveryLogs(Delivery delivery)
        {
            try
            {
                IList<fons.DeliveryLog> newFieldVal = new List<fons.DeliveryLog>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.DeliveryLog cond = new mtns.DeliveryLog();
                        cond.deliveryNo = delivery.DeliveryNo;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.DeliveryLog>(tk, null, null, new ConditionCollection<mtns.DeliveryLog>(new EqualCondition<mtns.DeliveryLog>(cond)));
                    }
                }
                sqlCtx.Param(mtns.DeliveryLog.fn_deliveryNo).Value = delivery.DeliveryNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.DeliveryLog item = null;
                        item = FuncNew.SetFieldFromColumnWithoutReadReader<mtns.DeliveryLog, fons.DeliveryLog>(item, sqlR, sqlCtx);

                        item.Tracker.Clear();
                        item.Tracker = delivery.Tracker;
                        newFieldVal.Add(item);
                    }
                }
                delivery.GetType().GetField("_logs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(delivery, newFieldVal);
                return delivery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ShipmentInfo> GetDeliveryInfoValueByTypeAndModelPrefix(string infoType, string modelPrefix, DateTime shipDate)
        {
            //select distinct b.InfoValue from IMES_PAK..Delivery a,IMES_PAK..DeliveryInfo b where a.DeliveryNo = b.DeliveryNo 
            //and b.InfoType='RedShipment' and left(a.Model,2)='60' and a.ShipDate=’’order by b.InfoValue
            try
            {
                //2010-06-04 Liu Dong(eB1-4)         Modify ITC-1155-0178 
                DateTime shipDateBeg = new DateTime(shipDate.Year, shipDate.Month, shipDate.Day, 0, 0, 0, 0);
                DateTime shipDateEnd = new DateTime(shipDate.Year, shipDate.Month, shipDate.Day, 23, 59, 59, 999);

                IList<ShipmentInfo> ret = new List<ShipmentInfo>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Delivery);
                        _Schema.Delivery eqcond1 = new _Schema.Delivery();
                        eqcond1.ShipDate = shipDate;
                        //tf1.equalcond = eqcond1;//2010-06-04 Liu Dong(eB1-4)         Modify ITC-1155-0178 
                        tf1.greaterOrEqualcond = eqcond1;
                        tf1.smallercond = eqcond1;
                        _Schema.Delivery likecond = new _Schema.Delivery();
                        likecond.Model = modelPrefix + "%";
                        tf1.likecond = likecond;
                        tf1.ToGetFieldNames = null;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.DeliveryInfo);
                        _Schema.DeliveryInfo eqcond2 = new _Schema.DeliveryInfo();
                        eqcond2.InfoType = infoType;
                        tf2.equalcond = eqcond2;
                        tf2.ToGetFieldNames.Add(_Schema.DeliveryInfo.fn_InfoValue);

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_DeliveryNo, tf2, _Schema.DeliveryInfo.fn_DeliveryNo);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                //2010-06-04 Liu Dong(eB1-4)         Modify ITC-1155-0178 
                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.Delivery.fn_ShipDate))].Value = shipDateBeg;
                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecS(_Schema.Delivery.fn_ShipDate))].Value = shipDateEnd;
                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_Model)].Value = modelPrefix + "%"; ;
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoType)].Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ShipmentInfo item = new ShipmentInfo();
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.DeliveryInfo.fn_InfoValue)]);
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

        public IList<DNForUI> UploadPOData(string uploadID, string editor)
        {
            try
            {
                IList<DNForUI> ret = new List<DNForUI>();
                string currentDB = _Schema.SqlHelper.ConnectionString_PAK;
                string currentSPName = "IMES_Upload_POData";

                SqlParameter[] paramsArray = new SqlParameter[2];

                paramsArray[0] = new SqlParameter("@UploadId", SqlDbType.VarChar);
                paramsArray[0].Value = uploadID;
                paramsArray[1] = new SqlParameter("@Editor", SqlDbType.VarChar);
                paramsArray[1].Value = editor;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(currentDB, CommandType.StoredProcedure, currentSPName, paramsArray))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            int resultFieldCount = sqlR.FieldCount;
                            if (resultFieldCount != 7)
                            {
                                List<string> errpara = new List<string>();
                                for (int i = 1; i < sqlR.FieldCount; i++)
                                {
                                    errpara.Add(GetValue_Str(sqlR, i));
                                }
                                throw new FisException(GetValue_Str(sqlR, 0), errpara);

                            }
                            else
                            {
                                DNForUI item = new DNForUI();

                                item.DeliveryNo = GetValue_Str(sqlR, 0);
                                item.ModelName = GetValue_Str(sqlR, 3);
                                item.PoNo = GetValue_Str(sqlR, 2);
                                item.Qty = GetValue_Int16(sqlR, 5);
                                item.ShipDate = GetValue_DateTime(sqlR, 4);
                                item.ShipmentID = GetValue_Str(sqlR, 1);
                                item.Status = GetValue_Str(sqlR, 6);
                                ret.Add(item);
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

        public void UpdateDNStatusWhenDNPalletFull(string deliveryNo, string status, string editor)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (PeekPalletWhetherFull(deliveryNo))
                    UpdateDNStatus(deliveryNo, status, editor);

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

        private bool PeekPalletWhetherFull(string deliveryNo)
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
                        _Schema.Delivery_Pallet cond = new _Schema.Delivery_Pallet();
                        cond.Status = "0";
                        cond.DeliveryNo = deliveryNo;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet), "COUNT", new List<string>() { _Schema.Delivery_Pallet.fn_ID }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Params[_Schema.Delivery_Pallet.fn_Status].Value = cond.Status;

                        //2010-05-31 Liu Dong(eB1-4)         Modify ITC-1155-0155
                        //sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryNo].Value = deliveryNo;

                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    ret = cnt > 0 ? false : true;
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

        private void UpdateDNStatus(string deliveryNo, string status, string editor)
        {
            //Update Delivey set Status=@Status,Editor=@Editor,Udt=@Udt Where DeliceryNo=@DeliveryNo
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Delivery cond = new _Schema.Delivery();
                        cond.DeliveryNo = deliveryNo;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), new List<string>() { _Schema.Delivery.fn_Status, _Schema.Delivery.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = deliveryNo;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Editor)].Value = editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Status)].Value = status;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Delivery.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAllDNStatusWhenPalletFull(string deliveryNo, string palletno, string status, string editor)
        {
            try
            {
                SqlParameter[] paramsArray = new SqlParameter[4];

                paramsArray[0] = new SqlParameter("@DeliveryNo", SqlDbType.Char);
                paramsArray[0].Value = deliveryNo;
                paramsArray[1] = new SqlParameter("@PalletNo", SqlDbType.Char);
                paramsArray[1].Value = palletno;
                paramsArray[2] = new SqlParameter("@Editor", SqlDbType.VarChar);
                paramsArray[2].Value = editor;
                paramsArray[3] = new SqlParameter("@Status", SqlDbType.Char);
                paramsArray[3].Value = status;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "IMES_UpdateDN_PalletFull", paramsArray);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DNForUI> UpdatePOData(string uploadID, string editor)
        {
            try
            {
                IList<DNForUI> ret = new List<DNForUI>();
                string currentDB = _Schema.SqlHelper.ConnectionString_PAK;
                string currentSPName = "IMES_Update_POData";

                SqlParameter[] paramsArray = new SqlParameter[2];

                paramsArray[0] = new SqlParameter("@UploadId", SqlDbType.VarChar);
                paramsArray[0].Value = uploadID;
                paramsArray[1] = new SqlParameter("@Editor", SqlDbType.VarChar);
                paramsArray[1].Value = editor;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(currentDB, CommandType.StoredProcedure, currentSPName, paramsArray))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            int resultFieldCount = sqlR.FieldCount;
                            if (resultFieldCount != 7)
                            {
                                List<string> errpara = new List<string>();
                                for (int i = 1; i < sqlR.FieldCount; i++)
                                {
                                    errpara.Add(GetValue_Str(sqlR, i));
                                }
                                throw new FisException(GetValue_Str(sqlR, 0), errpara);

                            }
                            else
                            {
                                DNForUI item = new DNForUI();

                                item.DeliveryNo = GetValue_Str(sqlR, 0);
                                item.ModelName = GetValue_Str(sqlR, 3);
                                item.PoNo = GetValue_Str(sqlR, 2);
                                item.Qty = GetValue_Int16(sqlR, 5);
                                item.ShipDate = GetValue_DateTime(sqlR, 4);
                                item.ShipmentID = GetValue_Str(sqlR, 1);
                                item.Status = GetValue_Str(sqlR, 6);
                                ret.Add(item);
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

        public IList<string> GetDeliveryNoListFor071(string Model)
        {
            try
            {
                IList<string> ret = new List<string>();

                //2011-04-08 Liu Dong(eB1-4)         Modify ITC-1268-0016
                DateTime now = DateTime.Now;//.AddDays(1);

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.Delivery);
                        _Schema.Delivery geCond = new _Schema.Delivery();
                        geCond.ShipDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0) ;
                        _Schema.Delivery cond1 = new _Schema.Delivery();
                        cond1.Status = "00";
                        tf1.equalcond = cond1;
                        tf1.greaterOrEqualcond = geCond;
                        tf1.ToGetFieldNames.Add(_Schema.Delivery.fn_DeliveryNo);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Product);
                        tf2.subDBCalalog = _Schema.SqlHelper.DB_FA;
                        _Schema.Product cond2 = new _Schema.Product();
                        cond2.Model = Model;
                        tf2.equalcond = cond2;
                        tf2.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery.fn_Model, tf2, _Schema.Product.fn_Model);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_Status)].Value = cond1.Status;

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.Delivery.fn_DeliveryNo));
                        //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Format("{0},{1}", _Schema.Func.DecAliasInner(tf1.alias, _Schema.Delivery.fn_ShipDate), _Schema.Func.DecAliasInner(tf1.alias, _Schema.Delivery.fn_DeliveryNo)));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecGE(_Schema.Delivery.fn_ShipDate))].Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Product.fn_Model)].Value = Model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery.fn_DeliveryNo)]);
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

        public IList<string> GetPalletNoListByDn(string dn)
        {
            try
            {
                IList<string> ret = new List<string> ();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT B.{0} FROM {1} A " +
                                           "INNER JOIN {1} B " +
                                           "ON A.{0}=B.{0} AND A.{2}=@{2} AND A.{3}='0' " +
                                           "GROUP BY B.{0} HAVING COUNT(B.{2})>1 ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.Delivery_Pallet.fn_PalletNo,
                                                                        typeof(_Schema.Delivery_Pallet).Name,
                                                                        _Schema.Delivery_Pallet.fn_DeliveryNo,
                                                                        _Schema.Delivery_Pallet.fn_Status);

                        sqlCtx.Params.Add(_Schema.Delivery_Pallet.fn_DeliveryNo, new SqlParameter("@" + _Schema.Delivery_Pallet.fn_DeliveryNo, SqlDbType.Char));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryNo].Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string pltno = GetValue_Str(sqlR, 0);
                            ret.Add(pltno);
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

        public IList<string> GetPalletNoListByDnAndWithSoloDn(string dn)
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT B.{0} FROM {1} A " +
                                           "INNER JOIN {1} B " +
                                           "ON A.{0}=B.{0} AND A.{2}=@{2} AND A.{3}='0' AND B.{3}='0' " +
                                           "GROUP BY B.{0} HAVING COUNT(DISTINCT(B.{2}))=1 ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.Delivery_Pallet.fn_PalletNo,
                                                                        typeof(_Schema.Delivery_Pallet).Name,
                                                                        _Schema.Delivery_Pallet.fn_DeliveryNo,
                                                                        _Schema.Delivery_Pallet.fn_Status);

                        sqlCtx.Params.Add(_Schema.Delivery_Pallet.fn_DeliveryNo, new SqlParameter("@" + _Schema.Delivery_Pallet.fn_DeliveryNo, SqlDbType.Char));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryNo].Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string pltno = GetValue_Str(sqlR, 0);
                            ret.Add(pltno);
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

        public IList<string> GetPalletNoListByShipmentAndWithSoloShipment(string shipment)
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = "SELECT B.{0} FROM {1} A " +
                                           "INNER JOIN {1} B " +
                                           "ON A.{0}=B.{0} AND A.{2}=@{2} AND A.{3}='0' AND B.{3}='0' " +
                                           "GROUP BY B.{0} HAVING COUNT(DISTINCT(B.{2}))=1 ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.Delivery_Pallet.fn_PalletNo,
                                                                        typeof(_Schema.Delivery_Pallet).Name,
                                                                        _Schema.Delivery_Pallet.fn_ShipmentNo,
                                                                        _Schema.Delivery_Pallet.fn_Status);

                        sqlCtx.Params.Add(_Schema.Delivery_Pallet.fn_ShipmentNo, new SqlParameter("@" + _Schema.Delivery_Pallet.fn_ShipmentNo, SqlDbType.Char));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_ShipmentNo].Value = shipment;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string pltno = GetValue_Str(sqlR, 0);
                            ret.Add(pltno);
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

        public string GetDeliveryInfoValue(string dn, string infoType)
        {
            try
            {
                string ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.deliveryNo = dn;
                        cond.infoType = infoType;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.DeliveryInfo>(tk, null, new string[] { _Metas.DeliveryInfo.fn_infoValue }, new ConditionCollection<_Metas.DeliveryInfo>(new EqualCondition<_Metas.DeliveryInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DeliveryInfo.fn_deliveryNo).Value = dn;
                sqlCtx.Param(_Metas.DeliveryInfo.fn_infoType).Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.DeliveryInfo.fn_infoValue));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSnoidForShipBoxDet(string snoId, string dn)
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
                        ShipBoxDet cond = new ShipBoxDet();
                        cond.deliveryNo = dn;
                        ShipBoxDet setv = new ShipBoxDet();
                        setv.snoId = snoId;
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<ShipBoxDet>(tk, new SetValueCollection<ShipBoxDet>(new CommonSetValue<ShipBoxDet>(setv)), new ConditionCollection<ShipBoxDet>(new EqualCondition<ShipBoxDet>(cond)));
                    }
                }
                sqlCtx.Param(ShipBoxDet.fn_deliveryNo).Value = dn;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(ShipBoxDet.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecSV(ShipBoxDet.fn_snoId)).Value = snoId;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemovePAK_PackkingData_EDIDataByProdIds(string[] prodIds)
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
                        Pak_Packkingdata cond = new Pak_Packkingdata();
                        cond.serial_num = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<Pak_Packkingdata>(tk, new ConditionCollection<Pak_Packkingdata>(new InSetCondition<Pak_Packkingdata>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(Pak_Packkingdata.fn_serial_num), g.ConvertInSet(new List<string>(prodIds)));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemovePAKOdmSession_EDIDataByProdIds(string[] prodIds)
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
                        Pakodmsession cond = new Pakodmsession();
                        cond.serial_num = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<Pakodmsession>(tk, new ConditionCollection<Pakodmsession>(new InSetCondition<Pakodmsession>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(Pakodmsession.fn_serial_num), g.ConvertInSet(new List<string>(prodIds)));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCertainInfoValueForDeliveryByPalletNoOnDeliveryNo(string palletNo, string infoType)
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
                        tf1 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoType = infoType;
                        tf1.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.DeliveryInfo.fn_infoValue);

                        tf2 = new TableAndFields<Delivery_Pallet>();
                        Delivery_Pallet cond2 = new Delivery_Pallet();
                        cond2.palletNo = palletNo;
                        tf2.Conditions.Add(new EqualCondition<Delivery_Pallet>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.DeliveryInfo, Delivery_Pallet>(tf1, _Metas.DeliveryInfo.fn_deliveryNo, tf2, Delivery_Pallet.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "TOP 1", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, Delivery_Pallet.fn_palletNo)).Value = palletNo;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, Delivery_Pallet.fn_palletNo)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.DeliveryInfo.fn_infoValue)));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCertainInfoValueForDeliveryByPalletNoOnShipmentNo(string palletNo, string infoType)
        {
            try
            {
                string ret = null;

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
                        tf1 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoType = infoType;
                        tf1.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.DeliveryInfo.fn_infoValue);

                        tf2 = new TableAndFields<Delivery_Pallet>();
                        Delivery_Pallet cond2 = new Delivery_Pallet();
                        cond2.palletNo = palletNo;
                        tf2.Conditions.Add(new EqualCondition<Delivery_Pallet>(cond2));
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<mtns::Delivery>();
                        tf3.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.DeliveryInfo, Delivery_Pallet>(tf1, _Metas.DeliveryInfo.fn_deliveryNo, tf2, Delivery_Pallet.fn_deliveryNo),
                             new TableConnectionItem<_Metas.Delivery, Delivery_Pallet>(tf3, _Metas.Delivery.fn_shipmentNo, tf2, Delivery_Pallet.fn_shipmentNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "TOP 1", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, Delivery_Pallet.fn_palletNo)).Value = palletNo;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, Delivery_Pallet.fn_palletNo)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.DeliveryInfo.fn_infoValue)));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetSumofDeliveryQtyFromDeliveryPallet(string palletNo)
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
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, "SUM", new string[] { Delivery_Pallet.fn_deliveryQty }, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_palletNo).Value = palletNo;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("SUM"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons::Delivery> GetDeliveriesByShipDateRange(DateTime begin, DateTime end)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.shipDate = DateTime.Now;

                        mtns.Delivery cond2 = new mtns.Delivery();
                        cond2.shipDate = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery>(tk, null, null, new ConditionCollection<mtns.Delivery>(
                            new GreaterOrEqualCondition<mtns.Delivery>(cond),
                            new SmallerCondition<mtns.Delivery>(cond2)), mtns.Delivery.fn_deliveryNo);
                    }
                }
                //ITC-1360-0872
                sqlCtx.Param(g.DecGE(mtns.Delivery.fn_shipDate)).Value = begin;
                sqlCtx.Param(g.DecS(mtns.Delivery.fn_shipDate)).Value = end.AddDays(1);
                //ITC-1360-0872

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///// <summary>
        ///// a.	取得Customer S/N 绑定的Delivery No (IMES_FA..Product.DeliveryNo)
        ///// b.	取得该Delivery No 的Consolidated 属性（IMES_PAK.DeliveryInfo）
        ///// </summary>
        ///// <param name="CustomerSN"></param>
        ///// <returns>Consolidated</returns>
        //public string GetDeliveryInfoConsolidated(string CustomerSN)
        //{
        //    
        //}

        /// <summary>
        /// 使用ProductID = @ProductId and Tp = 'PAQC' 查询IMES_FA..QCStatus 表取Udt 最新的记录，
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public ProductQCStatus GetQCStatus(string productID, string tp)
        {
            try
            {
                ProductQCStatus ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Qcstatus cond = new mtns.Qcstatus();
                        cond.productID = productID;
                        cond.tp = tp;
 
                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Qcstatus>(tk, "TOP 1", null, new ConditionCollection<mtns.Qcstatus>(
                            new EqualCondition<mtns.Qcstatus>(cond)), mtns.Qcstatus.fn_udt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns.Qcstatus.fn_productID).Value = productID;
                sqlCtx.Param(mtns.Qcstatus.fn_tp).Value = tp;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.Qcstatus, ProductQCStatus>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPakPackkingData(IList<string> internalIds)
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
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Packkingdata>(tk, "COUNT", new string[] { _Metas.Pak_Packkingdata.fn_id }, new ConditionCollection<Pak_Packkingdata>(new InSetCondition<Pak_Packkingdata>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::Pak_Packkingdata.fn_internalID), g.ConvertInSet(internalIds));

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

        public IList<fons::Delivery> GetDNListForCombineCOAandDN()
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.shipDate = DateTime.Now;

                        mtns.Delivery cond2 = new mtns.Delivery();
                        cond2.model = "PC%";

                        mtns.Delivery cond3 = new mtns.Delivery();
                        cond3.status = "00";

                        mtns.Delivery cond4 = new mtns.Delivery();
                        cond4.model = "[LEN]";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery>(tk, null, null, new ConditionCollection<mtns.Delivery>(
                            new GreaterCondition<mtns.Delivery>(cond),
                            new LikeCondition<mtns.Delivery>(cond2),
                            new EqualCondition<mtns.Delivery>(cond3),
                            new EqualCondition<mtns.Delivery>(cond4, "LEN({0})", "CONVERT(INT,{0})")
                            ), mtns.Delivery.fn_deliveryNo);

                        sqlCtx.Param(mtns.Delivery.fn_status).Value = cond3.status;
                        sqlCtx.Param(mtns.Delivery.fn_model).Value = cond2.model;
                        sqlCtx.Param(mtns.Delivery.fn_model + "$1").Value = "12";
                    }
                }
                sqlCtx.Param(g.DecG(mtns.Delivery.fn_shipDate)).Value = _Schema.SqlHelper.GetDateTime().AddDays(-3);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetDistinctDeliveryNo(string consolidateNo)
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
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoValue = consolidateNo + "%";
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoType = "Consolidated";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.DeliveryInfo>(tk, "COUNT", new string[][] { new string[] { _Metas.DeliveryInfo.fn_deliveryNo, string.Format("DISTINCT LEFT({0},10)", _Metas.DeliveryInfo.fn_deliveryNo) } },
                            new ConditionCollection<_Metas.DeliveryInfo>(new LikeCondition<_Metas.DeliveryInfo>(cond),
                                                                         new EqualCondition<_Metas.DeliveryInfo>(cond2)));
                        sqlCtx.Param(_Metas.DeliveryInfo.fn_infoType).Value = cond2.infoType;
                    }
                }
                sqlCtx.Param(_Metas.DeliveryInfo.fn_infoValue).Value = consolidateNo + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public int GetSumDeliveryQtyOfACertainDN(string deliveryNo)
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
                        _Metas.Delivery_Pallet cond = new _Metas.Delivery_Pallet();
                        cond.deliveryNo = deliveryNo;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery_Pallet>(tk, "SUM", new string[] { _Metas.Delivery_Pallet.fn_deliveryQty }, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(mtns.Delivery_Pallet.fn_deliveryNo).Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("SUM"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDeliveryInfo(fons::DeliveryInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::DeliveryInfo>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::DeliveryInfo, fons::DeliveryInfo>(sqlCtx, item);

                sqlCtx.Param(mtns::DeliveryInfo.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::DeliveryInfo.fn_udt).Value = cmDt;

                item.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDNUdt(string dn)
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
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.deliveryNo = dn;
                        mtns.Delivery setv = new mtns.Delivery();
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<mtns.Delivery>(tk, new SetValueCollection<mtns.Delivery>(new CommonSetValue<mtns.Delivery>(setv)), new ConditionCollection<mtns.Delivery>(new EqualCondition<mtns.Delivery>(cond)));
                    }
                }
                sqlCtx.Param(mtns.Delivery.fn_deliveryNo).Value = dn;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns.Delivery.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SnoCtrlBoxIdInfo GetSnoCtrlBoxIdInfoByLikeCustAndValid(string custLike, string valid)
        {
            try
            {
                SnoCtrlBoxIdInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.SnoCtrl_BoxId cond = new mtns.SnoCtrl_BoxId();
                        cond.valid = valid;
                        mtns.SnoCtrl_BoxId cond2 = new mtns.SnoCtrl_BoxId();
                        cond2.cust = custLike;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.SnoCtrl_BoxId>(tk, "TOP 1", null, new ConditionCollection<mtns.SnoCtrl_BoxId>(
                            new EqualCondition<mtns.SnoCtrl_BoxId>(cond),
                            new LikeCondition<mtns.SnoCtrl_BoxId>(cond2)));
                    }
                }
                sqlCtx.Param(mtns.SnoCtrl_BoxId.fn_cust).Value = custLike;
                sqlCtx.Param(mtns.SnoCtrl_BoxId.fn_valid).Value = valid;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSnoCtrlBoxIdInfo(int id)
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
                        _Metas.SnoCtrl_BoxId cond = new _Metas.SnoCtrl_BoxId();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.SnoCtrl_BoxId>(tk, new ConditionCollection<_Metas.SnoCtrl_BoxId>(new EqualCondition<_Metas.SnoCtrl_BoxId>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.SnoCtrl_BoxId.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryListByLikeConsolidated(string consolidated)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Delivery>();

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoValue = consolidated + "%";
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoType = "Consolidated";
                        tf2.Conditions.Add(new LikeCondition<_Metas.DeliveryInfo>(cond));
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = cond2.infoType;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = consolidated + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryListByInfoTypeAndValue(string infoType, string infoValue)
        {
            try
            {
                return GetDeliveryByValueAndType(infoValue, infoType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertShipBoxDet(ShipBoxDetInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<ShipBoxDet>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<ShipBoxDet, ShipBoxDetInfo>(sqlCtx, item);

                sqlCtx.Param(ShipBoxDet.fn_cdt).Value = cmDt;
                sqlCtx.Param(ShipBoxDet.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSnoCtrlBoxIdSQInfo(SnoCtrlBoxIdSQInfo item)
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
                        SnoCtrl_BoxId_SQ cond = new SnoCtrl_BoxId_SQ();
                        cond.boxId = item.boxId;
                        cond.cust = item.cust;
                        sqlCtx = FuncNew.GetConditionedDelete<SnoCtrl_BoxId_SQ>(tk, new ConditionCollection<SnoCtrl_BoxId_SQ>(new EqualCondition<SnoCtrl_BoxId_SQ>(cond)));
                    }
                }
                sqlCtx.Param(SnoCtrl_BoxId_SQ.fn_boxId).Value = item.boxId;
                sqlCtx.Param(SnoCtrl_BoxId_SQ.fn_cust).Value = item.cust;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetColListFromPakWhLocMas()
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
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Wh_Locmas>(tk, "DISTINCT", new string[] { mtns::Pak_Wh_Locmas.fn_col }, new ConditionCollection<mtns::Pak_Wh_Locmas>(), mtns::Pak_Wh_Locmas.fn_col);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Wh_Locmas.fn_col));
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

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryListByInfoType(string infoType)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Delivery>();

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoType = infoType;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDeliveryPallet(DeliveryPalletInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Delivery_Pallet>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Delivery_Pallet, DeliveryPalletInfo>(sqlCtx, item);

                sqlCtx.Param(Delivery_Pallet.fn_cdt).Value = cmDt;
                sqlCtx.Param(Delivery_Pallet.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryListByStatusesAndModel(DateTime begin, string[] statuses, string modelPrefix, int modelLength)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.shipDate = DateTime.Now;

                        mtns.Delivery cond2 = new mtns.Delivery();
                        cond2.status = "[INSET]";

                        mtns.Delivery cond3 = new mtns.Delivery();
                        cond3.model = modelPrefix + "%";

                        mtns.Delivery cond4 = new mtns.Delivery();
                        cond4.model = "[LEN]";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery>(tk, null, null, new ConditionCollection<mtns.Delivery>(
                            new GreaterOrEqualCondition<mtns.Delivery>(cond),
                            new InSetCondition<mtns.Delivery>(cond2),
                            new LikeCondition<mtns.Delivery>(cond3),
                            new EqualCondition<mtns.Delivery>(cond4, "LEN({0})", "CONVERT(INT,{0})")), mtns.Delivery.fn_deliveryNo);
                    }
                }
                sqlCtx.Param(g.DecGE(mtns.Delivery.fn_shipDate)).Value = new DateTime(begin.Year, begin.Month, begin.Day);
                sqlCtx.Param(mtns.Delivery.fn_model).Value = modelPrefix + "%";
                sqlCtx.Param(mtns.Delivery.fn_model + "$1").Value = modelLength.ToString();

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::Delivery.fn_status), g.ConvertInSet(new List<string>(statuses)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryPalletInfo> GetDeliveryPalletListByDN(string deliveryNo)
        {
            try
            {
                IList<DeliveryPalletInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.deliveryNo = deliveryNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, null, null, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_deliveryNo).Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Delivery_Pallet, DeliveryPalletInfo, DeliveryPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryPalletInfo> GetDeliveryPalletListByDN(string deliveryNo, string palletNo)
        {
            try
            {
                IList<DeliveryPalletInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.deliveryNo = deliveryNo;
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, null, null, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_deliveryNo).Value = deliveryNo;
                sqlCtx.Param(Delivery_Pallet.fn_palletNo).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Delivery_Pallet, DeliveryPalletInfo, DeliveryPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryPalletInfo> GetDeliveryPalletListOrderByUnitQty(string deliveryNo)
        {
            try
            {
                IList<DeliveryPalletInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.deliveryNo = deliveryNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, null, null, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)),
                            string.Format("(SELECT COUNT(1) FROM {0} WHERE {0}.{1}={2}.{3})", ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_palletNo, ToolsNew.GetTableName(typeof(Delivery_Pallet)), Delivery_Pallet.fn_palletNo) + FuncNew.DescendOrder);//Delivery_Pallet.fn_deliveryQty + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_deliveryNo).Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Delivery_Pallet, DeliveryPalletInfo, DeliveryPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IList<DeliveryPalletInfo> GetDeliveryPalletListOrderByUnitQty(string deliveryNo)
        //{
        //    try
        //    {
        //        IList<DeliveryPalletInfo> ret = null;

        //        MethodBase mthObj = MethodBase.GetCurrentMethod();
        //        int tk = mthObj.MetadataToken;
        //        SQLContextNew sqlCtx = null;
        //        lock (mthObj)
        //        {
        //            if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
        //            {
        //                Delivery_Pallet cond = new Delivery_Pallet();
        //                cond.deliveryNo = deliveryNo;
        //                sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, null, null, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)), Delivery_Pallet.fn_deliveryQty + FuncNew.DescendOrder);
        //            }
        //        }
        //        sqlCtx.Param(Delivery_Pallet.fn_deliveryNo).Value = deliveryNo;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
        //        {
        //            ret = FuncNew.SetFieldFromColumn<Delivery_Pallet, DeliveryPalletInfo, DeliveryPalletInfo>(ret, sqlR, sqlCtx);
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public int GetSumDeliveryQtyOfACertainPallet(string plt)
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
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.palletNo = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, "SUM", new string[] { Delivery_Pallet.fn_deliveryQty }, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_palletNo).Value = plt;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("SUM"));
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryPalletInfo> GetDeliveryPalletListByPlt(string palletNo)
        {
            try
            {
                IList<DeliveryPalletInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, null, null, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_palletNo).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Delivery_Pallet, DeliveryPalletInfo, DeliveryPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BackupToDelivery(string dn)
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
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Delivery, _Metas.DeletedDelivery>(tk,
                            new string[][]{
                                new string[]{_Metas.Delivery.fn_cdt, DeletedDelivery.fn_cdt},
                                new string[]{_Metas.Delivery.fn_deliveryNo, DeletedDelivery.fn_deliveryNo},
                                new string[]{_Metas.Delivery.fn_editor, DeletedDelivery.fn_editor},
                                new string[]{_Metas.Delivery.fn_model, DeletedDelivery.fn_model},
                                new string[]{_Metas.Delivery.fn_poNo, DeletedDelivery.fn_poNo},
                                new string[]{_Metas.Delivery.fn_qty, DeletedDelivery.fn_qty},
                                new string[]{_Metas.Delivery.fn_shipDate, DeletedDelivery.fn_shipDate},
                                new string[]{_Metas.Delivery.fn_shipmentNo, DeletedDelivery.fn_shipmentNo},
                                new string[]{_Metas.Delivery.fn_status, DeletedDelivery.fn_status},
                                new string[]{_Metas.Delivery.fn_udt, DeletedDelivery.fn_udt}
                            },
                            new ConditionCollection<_Metas.Delivery>(new EqualCondition<_Metas.Delivery>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BackupToDeliveryPallet(string dn)
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
                        _Metas.Delivery_Pallet cond = new _Metas.Delivery_Pallet();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Delivery_Pallet, _Metas.DeletedDeliveryPallet>(tk,
                            new string[][]{
                                new string[]{Delivery_Pallet.fn_cdt, DeletedDeliveryPallet.fn_cdt},
                                new string[]{Delivery_Pallet.fn_deliveryNo, DeletedDeliveryPallet.fn_deliveryNo},
                                new string[]{Delivery_Pallet.fn_deliveryQty, DeletedDeliveryPallet.fn_deliveryQty},
                                new string[]{Delivery_Pallet.fn_editor, DeletedDeliveryPallet.fn_editor},
                                new string[]{Delivery_Pallet.fn_id, DeletedDeliveryPallet.fn_id},
                                new string[]{Delivery_Pallet.fn_palletNo, DeletedDeliveryPallet.fn_palletNo},
                                new string[]{Delivery_Pallet.fn_shipmentNo, DeletedDeliveryPallet.fn_shipmentNo},
                                new string[]{Delivery_Pallet.fn_status, DeletedDeliveryPallet.fn_status},
                                new string[]{Delivery_Pallet.fn_udt, DeletedDeliveryPallet.fn_udt},
                                new string[]{Delivery_Pallet.fn_deviceQty, DeletedDeliveryPallet.fn_deviceQty},
                                new string[]{Delivery_Pallet.fn_palletType, DeletedDeliveryPallet.fn_palletType}
                            },
                            new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_deliveryNo).Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DNForUI> GetDNListByConditionFromPoDataEdi(DNQueryCondition MyCondition, out int totalLength)
        {
            try
            {
                totalLength = 0;
                IList<DNForUI> ret = new List<DNForUI>();

                _Schema.SQLContext sqlCtx = null;

                _Schema.SQLContextCollection sqlCtxCllctn = new _Schema.SQLContextCollection();

                int i = 0;
                if (!string.IsNullOrEmpty(MyCondition.DNInfoValue))
                {
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPoDataEdi_DNInfoValue(MyCondition.DNInfoValue));
                }
                if (!string.IsNullOrEmpty(MyCondition.DeliveryNo))
                {
                    if (MyCondition.DeliveryNo.Length == 16)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPoDataEdi_DeliveryNo2(MyCondition.DeliveryNo));
                    else if (MyCondition.DeliveryNo.Length == 10)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPoDataEdi_DeliveryNo(MyCondition.DeliveryNo));
                }

                if (!string.IsNullOrEmpty(MyCondition.Model))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPoDataEdi_Model(MyCondition.Model));

                if (!string.IsNullOrEmpty(MyCondition.PONo))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPoDataEdi_PONo(MyCondition.PONo));

                if (DateTime.MinValue != MyCondition.ShipDateFrom)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPoDataEdi_ShipDateFrom(MyCondition.ShipDateFrom));

                if (DateTime.MinValue != MyCondition.ShipDateTo)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPoDataEdi_ShipDateTo(MyCondition.ShipDateTo.AddDays(1)));

                if (i > 0)
                {
                    sqlCtx = sqlCtxCllctn.MergeToOneAndQuery();

                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PoData_EDI.fn_deliveryNo);

                    using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                    {
                        if (sqlR != null)
                        {
                            while (sqlR.Read())
                            {
                                if (totalLength < 1000)
                                {
                                    DNForUI item = new DNForUI();

                                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_cdt]);
                                    item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_deliveryNo]);
                                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_editor]);
                                    item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_model]);
                                    item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_poNo]);
                                    item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_qty]);

                                    //DateTime.TryParse(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_shipDate]), out item.ShipDate);
                                    item.ShipDate = From10BitDateTime(GetValue_Str(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_shipDate]));//ITC-1360-1392

                                    item.ShipmentID = GetValue_Str(sqlR, sqlCtx.Indexes["t2_" + _Metas.PakDotpakcomn.fn_shipment]);
                                    item.Status = GetValue_Str(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_status]);
                                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes["t1_" + _Schema.PoData_EDI.fn_udt]);

                                    /*
                                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_cdt]);
                                    item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_deliveryNo]);
                                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_editor]);
                                    item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_model]);
                                    item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_poNo]);
                                    item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_qty]);

                                    //DateTime.TryParse(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_shipDate]), out item.ShipDate);
                                    item.ShipDate = From10BitDateTime(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_shipDate]));//ITC-1360-1392

                                    item.ShipmentID = null;
                                    item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_status]);
                                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PoData_EDI.fn_udt]);*/

                                    ret.Add(item);
                                }
                                totalLength++;
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

        private _Schema.SQLContext ComposeForGetDNListByConditionFromPoDataEdi_DeliveryNo(string deliveryNo)
        {
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
                    tf1 = new TableAndFields<_Metas.PoData_EDI>();
                    _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                    cond.deliveryNo = deliveryNo + "%";
                    tf1.Conditions.Add(new LikeCondition<_Metas.PoData_EDI>(cond));

                    tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                    tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                    tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.PoData_EDI.fn_deliveryNo)).Value = deliveryNo + "%";
            return _Schema.SQLContext.ToOld(sqlCtx);
            /*
           _Schema.SQLContext sqlCtx = null;
           lock (MethodBase.GetCurrentMethod())
           {
               if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
               {
                   _Schema.PoData_EDI likeCond = new _Schema.PoData_EDI();
                   likeCond.deliveryNo = deliveryNo + "%";
                   sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PoData_EDI), null, null, null, likeCond, null, null, null, null, null, null);
               }
           }
           sqlCtx.Params[_Schema.PoData_EDI.fn_deliveryNo].Value = deliveryNo + "%";
            return sqlCtx;*/
        }

        private _Schema.SQLContext ComposeForGetDNListByConditionFromPoDataEdi_DeliveryNo2(string deliveryNo)
        {
           
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
                    tf1 = new TableAndFields<_Metas.PoData_EDI>();
                    _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                    cond.deliveryNo = deliveryNo;
                    tf1.Conditions.Add(new EqualCondition<_Metas.PoData_EDI>(cond));

                    tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                    tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                    tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.PoData_EDI.fn_deliveryNo)).Value = deliveryNo;
            return _Schema.SQLContext.ToOld(sqlCtx);
            /*
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PoData_EDI equalCond = new _Schema.PoData_EDI();
                    equalCond.deliveryNo = deliveryNo;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PoData_EDI), null, null, equalCond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PoData_EDI.fn_deliveryNo].Value = deliveryNo;
            return sqlCtx;*/
        }

        private _Schema.SQLContext ComposeForGetDNListByConditionFromPoDataEdi_PONo(string pono)
        {
         
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
                    tf1 = new TableAndFields<_Metas.PoData_EDI>();
                    _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                    cond.poNo = pono;
                    tf1.Conditions.Add(new EqualCondition<_Metas.PoData_EDI>(cond));

                    tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                    tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                    tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.PoData_EDI.fn_poNo)).Value = pono;
            return _Schema.SQLContext.ToOld(sqlCtx);
            /*
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PoData_EDI equalCond = new _Schema.PoData_EDI();
                    equalCond.poNo = pono;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PoData_EDI), null, null, equalCond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PoData_EDI.fn_poNo].Value = pono;
            return sqlCtx;*/
        }

        private _Schema.SQLContext ComposeForGetDNListByConditionFromPoDataEdi_Model(string model)
        {
         
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
                    tf1 = new TableAndFields<_Metas.PoData_EDI>();
                    _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                    cond.model = model;
                    tf1.Conditions.Add(new EqualCondition<_Metas.PoData_EDI>(cond));

                    tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                    tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                    tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.PoData_EDI.fn_model)).Value = model;
            return _Schema.SQLContext.ToOld(sqlCtx);
            /*
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PoData_EDI equalCond = new _Schema.PoData_EDI();
                    equalCond.model = model;
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PoData_EDI), null, null, equalCond, null, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PoData_EDI.fn_model].Value = model;
            return sqlCtx;*/
        }

        private _Schema.SQLContext ComposeForGetDNListByConditionFromPoDataEdi_ShipDateFrom(DateTime shipDateFrom)
        {

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
                    tf1 = new TableAndFields<_Metas.PoData_EDI>();
                    _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                    cond.shipDate = To10BitDateTime(shipDateFrom);
                    tf1.Conditions.Add(new GreaterOrEqualCondition<_Metas.PoData_EDI>(cond));

                    tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                    tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                    tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecGE(_Metas.PoData_EDI.fn_shipDate))).Value = To10BitDateTime(shipDateFrom);
            return _Schema.SQLContext.ToOld(sqlCtx);
            /*
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PoData_EDI geCond = new _Schema.PoData_EDI();
                    geCond.shipDate = To10BitDateTime(shipDateFrom);
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PoData_EDI), null, null, null, null, null, null, null, geCond, null, null);
                }
            }
            sqlCtx.Params[_Schema.Func.DecGE(_Schema.PoData_EDI.fn_shipDate)].Value = To10BitDateTime(shipDateFrom);
            return sqlCtx;*/
        }

        private _Schema.SQLContext ComposeForGetDNListByConditionFromPoDataEdi_ShipDateTo(DateTime shipDateTo)
        {
         
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
                    tf1 = new TableAndFields<_Metas.PoData_EDI>();
                    _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                    cond.shipDate = To10BitDateTime(shipDateTo);
                    tf1.Conditions.Add(new SmallerCondition<_Metas.PoData_EDI>(cond));

                    tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                    tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                    tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecS(_Metas.PoData_EDI.fn_shipDate))).Value = To10BitDateTime(shipDateTo);
            return _Schema.SQLContext.ToOld(sqlCtx);
            /*
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PoData_EDI sCond = new _Schema.PoData_EDI();
                    sCond.shipDate = To10BitDateTime(shipDateTo);
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PoData_EDI), null, null, null, null, null, null, sCond, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.Func.DecS(_Schema.PoData_EDI.fn_shipDate)].Value = To10BitDateTime(shipDateTo);
            return sqlCtx;*/
        }

        private _Schema.SQLContext ComposeForGetDNListByConditionFromPoDataEdi_DNInfoValue(string dnInfoValue)
        {
         
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
                    tf1 = new TableAndFields<_Metas.PoData_EDI>();
                    _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                    cond.descr = string.Format("%={0}~%", dnInfoValue);
                    tf1.Conditions.Add(new LikeCondition<_Metas.PoData_EDI>(cond));

                    tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                    tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                    tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                    tafa = new ITableAndFields[] { tf1, tf2 };

                    TableConnectionCollection tblCnnts = new TableConnectionCollection(
                        new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                    sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                }
            }
            tafa = sqlCtx.TableFields;
            tf1 = tafa[0];
            tf2 = tafa[1];

            sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.PoData_EDI.fn_descr)).Value = string.Format("%={0}~%", dnInfoValue);
            return _Schema.SQLContext.ToOld(sqlCtx);
            /*
            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    _Schema.PoData_EDI likeCond = new _Schema.PoData_EDI();
                    likeCond.descr = string.Format("%={0}~%", dnInfoValue);
                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PoData_EDI), null, null, null, likeCond, null, null, null, null, null, null);
                }
            }
            sqlCtx.Params[_Schema.PoData_EDI.fn_descr].Value = string.Format("%={0}~%", dnInfoValue);
            return sqlCtx;*/
        }

        private string To10BitDateTime(DateTime dt)
        {
            return string.Format("{0}/{1}/{2}", dt.Year.ToString(), dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));// +dt.Hour.ToString().PadLeft(2, '0');
        }

        private DateTime From10BitDateTime(string dt)
        {
            DateTime ret = default(DateTime);
            if (!string.IsNullOrEmpty(dt))
            {
                string[] dts = dt.Split('/');
                if (dts != null && dts.Length > 2)
                {
                    ret = new DateTime(int.Parse(dts[0]), int.Parse(dts[1]), int.Parse(dts[2]));
                }
            }
            return ret;
        }

        private string To8BitDateTime(DateTime dt)
        {
            return string.Format("{0}{1}{2}", dt.Year.ToString(), dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));// +dt.Hour.ToString().PadLeft(2, '0');
        }

        private DateTime From8BitDateTime(string dt)
        {
            DateTime ret = default(DateTime);
            if (!string.IsNullOrEmpty(dt))
            {
                if (dt.Length >= 8)
                {
                    ret = new DateTime(int.Parse(dt.Substring(0, 4)), int.Parse(dt.Substring(4, 2)), int.Parse(dt.Substring(6, 2)));
                }
            }
            return ret;
        }

        public PoDataEdiInfo GetPoDataEdiInfo(string deliveryNo)
        {
            try
            {
                PoDataEdiInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                        cond.deliveryNo = deliveryNo;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PoData_EDI>(tk, "TOP 1", null, new ConditionCollection<_Metas.PoData_EDI>(new EqualCondition<_Metas.PoData_EDI>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PoData_EDI.fn_deliveryNo).Value = deliveryNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<PoData_EDI, PoDataEdiInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DNPalletQty> GetPalletListFromPoPltEdi(string dn)
        {
            try
            {
                IList<DNPalletQty> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PoPlt_EDI cond = new mtns::PoPlt_EDI();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PoPlt_EDI>(tk, null, new string[] { mtns::PoPlt_EDI.fn_qty, mtns::PoPlt_EDI.fn_id, mtns::PoPlt_EDI.fn_plt, mtns::PoPlt_EDI.fn_ucc }, new ConditionCollection<mtns::PoPlt_EDI>(new EqualCondition<mtns::PoPlt_EDI>(cond)), mtns::PoPlt_EDI.fn_plt);
                    }
                }
                sqlCtx.Param(mtns::PoPlt_EDI.fn_deliveryNo).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while (sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.DeliveryQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns::PoPlt_EDI.fn_qty));
                            item.ID = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns::PoPlt_EDI.fn_id));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PoPlt_EDI.fn_plt));
                            item.UCC = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PoPlt_EDI.fn_ucc));
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

        public IList<DNForUI> GetDNListByDNList(IList<string> dnList)
        {
            long sum = 0;
            return GetDNListByDNList_Inner(dnList, out sum);
        }

        public IList<DNForUI> GetDNListByDNList(IList<string> dnList, out long sum)
        {
            return GetDNListByDNList_Inner(dnList, out sum);
        }

        private IList<DNForUI> GetDNListByDNList_Inner(IList<string> dnList, out long sum)
        {
            try
            {
                sum = 0;
                IList<DNForUI> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Delivery cond = new mtns::Delivery();
                        cond.deliveryNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Delivery>(tk, null, null, new ConditionCollection<mtns::Delivery>(new InSetCondition<mtns::Delivery>(cond)), mtns::Delivery.fn_deliveryNo);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::Delivery.fn_deliveryNo), g.ConvertInSet(new List<string>(dnList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNForUI>();
                        while (sqlR.Read())
                        {
                            DNForUI item = new DNForUI();
                            item.Cdt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_cdt));
                            item.DeliveryNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_deliveryNo));
                            item.Editor = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_editor));
                            item.ModelName = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_model));
                            item.PoNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_poNo));
                            item.Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_qty));
                            item.ShipDate = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_shipDate));
                            item.ShipmentID = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_shipmentNo));
                            item.Status = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_status));
                            item.Udt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(mtns::Delivery.fn_udt));
                            ret.Add(item);

                            sum = sum + item.Qty;
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

        public IList<DNForUI> GetDNListByDNListFromPoDataEDI(IList<string> dnList)
        {
            try
            {
                IList<DNForUI> ret = null;

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
                        tf1 = new TableAndFields<_Metas.PoData_EDI>();
                        _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                        cond.deliveryNo = "[INSET]";
                        tf1.Conditions.Add(new InSetCondition<_Metas.PoData_EDI>(cond));

                        tf2 = new TableAndFields<_Metas.PakDotpakcomn>();
                        tf2.AddRangeToGetFieldNames(_Metas.PakDotpakcomn.fn_shipment);
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_HP_EDI;

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.PoData_EDI, _Metas.PakDotpakcomn>(tf1, _Metas.PoData_EDI.fn_deliveryNo, tf2, _Metas.PakDotpakcomn.fn_internalID));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                string Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf1.Alias, g.DecInSet(PoData_EDI.fn_deliveryNo)), g.ConvertInSet(new List<string>(dnList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNForUI>();
                        while(sqlR.Read())
                        {
                            DNForUI item = new DNForUI();
                            item.Cdt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_cdt)));
                            item.DeliveryNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_deliveryNo)));
                            item.Editor = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_editor)));
                            item.ModelName = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_model)));
                            item.PoNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_poNo)));
                            item.Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_qty)));

                            //DateTime.TryParse(g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::PoData_EDI.fn_shipDate)), out item.ShipDate);
                            item.ShipDate = From10BitDateTime(g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_shipDate))));

                            item.ShipmentID = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns::PakDotpakcomn.fn_shipment)));//ITC-1360-1405
                            item.Status = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_status)));
                            item.Udt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns::PoData_EDI.fn_udt)));
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

        public void DeletePoDataEdi(string dn)
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
                        mtns::PoData_EDI cond = new mtns::PoData_EDI();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PoData_EDI>(tk, new ConditionCollection<mtns::PoData_EDI>(new EqualCondition<mtns::PoData_EDI>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PoData_EDI.fn_deliveryNo).Value = dn;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePoPltEdi(string dn)
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
                        mtns::PoPlt_EDI cond = new mtns::PoPlt_EDI();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PoPlt_EDI>(tk, new ConditionCollection<mtns::PoPlt_EDI>(new EqualCondition<mtns::PoPlt_EDI>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PoPlt_EDI.fn_deliveryNo).Value = dn;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteShipBoxDet(string shipment)
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
                        mtns::ShipBoxDet cond = new mtns::ShipBoxDet();
                        cond.shipment = shipment;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::ShipBoxDet>(tk, new ConditionCollection<mtns::ShipBoxDet>(new EqualCondition<mtns::ShipBoxDet>(cond)));
                    }
                }
                sqlCtx.Param(mtns::ShipBoxDet.fn_shipment).Value = shipment;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDeliveryInfoForDecreaseConsolidated(string shipmentNo)
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
                        mtns.DeliveryInfo cond = new mtns.DeliveryInfo();
                        cond.infoType = "Consolidated";
                        mtns.DeliveryInfo cond2 = new mtns.DeliveryInfo();
                        cond2.deliveryNo = shipmentNo;
                        mtns.DeliveryInfo cond3 = new mtns.DeliveryInfo();
                        cond3.infoValue = "%/%";
                        mtns.DeliveryInfo setv = new mtns.DeliveryInfo();
                        setv.infoValue = "[Decrease]";
                        mtns.DeliveryInfo setv2 = new mtns.DeliveryInfo();
                        setv2.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<mtns.DeliveryInfo>(tk, new SetValueCollection<mtns.DeliveryInfo>(
                            new ForIncSetValueSelf<mtns.DeliveryInfo>(setv, "{0}=SUBSTRING({0},1,CHARINDEX('/',{0},1))+CONVERT(VARCHAR(199),CONVERT(INT,SUBSTRING({0},CHARINDEX('/',{0},1)+1,LEN({0})-CHARINDEX('/',{0},1)))-1)"),
                            new CommonSetValue<mtns.DeliveryInfo>(setv2)), new ConditionCollection<mtns.DeliveryInfo>(
                                new EqualCondition<mtns.DeliveryInfo>(cond),
                                new AnyCondition<mtns.DeliveryInfo>(cond2, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4})", "{0}", mtns.Delivery.fn_deliveryNo, ToolsNew.GetTableName(typeof(mtns.Delivery)), mtns.Delivery.fn_shipmentNo, "{1}")),
                                new LikeCondition<mtns.DeliveryInfo>(cond3)));
                        sqlCtx.Param(mtns.DeliveryInfo.fn_infoType).Value = cond.infoType;
                        sqlCtx.Param(mtns.DeliveryInfo.fn_infoValue).Value = cond3.infoValue; 
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.DeliveryInfo.fn_deliveryNo)).Value = shipmentNo;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns.DeliveryInfo.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteShipBoxDetByDn(string dn)
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
                        mtns::ShipBoxDet cond = new mtns::ShipBoxDet();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::ShipBoxDet>(tk, new ConditionCollection<mtns::ShipBoxDet>(new EqualCondition<mtns::ShipBoxDet>(cond)));
                    }
                }
                sqlCtx.Param(mtns::ShipBoxDet.fn_deliveryNo).Value = dn;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryPalletByShipmentNo(string shipmentNo)
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
                        mtns::Delivery_Pallet cond = new mtns::Delivery_Pallet();
                        cond.shipmentNo = shipmentNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Delivery_Pallet>(tk, new ConditionCollection<mtns::Delivery_Pallet>(new EqualCondition<mtns::Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Delivery_Pallet.fn_shipmentNo).Value = shipmentNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryPalletByPalletNo(string palletNo)
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
                        mtns::Delivery_Pallet cond = new mtns::Delivery_Pallet();
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Delivery_Pallet>(tk, new ConditionCollection<mtns::Delivery_Pallet>(new EqualCondition<mtns::Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Delivery_Pallet.fn_palletNo).Value = palletNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BackupToDeliveryByShipmentNo(string shipmentNo)
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
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.shipmentNo = shipmentNo;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Delivery, _Metas.DeletedDelivery>(tk,
                            new string[][]{
                                new string[]{_Metas.Delivery.fn_cdt, DeletedDelivery.fn_cdt},
                                new string[]{_Metas.Delivery.fn_deliveryNo, DeletedDelivery.fn_deliveryNo},
                                new string[]{_Metas.Delivery.fn_editor, DeletedDelivery.fn_editor},
                                new string[]{_Metas.Delivery.fn_model, DeletedDelivery.fn_model},
                                new string[]{_Metas.Delivery.fn_poNo, DeletedDelivery.fn_poNo},
                                new string[]{_Metas.Delivery.fn_qty, DeletedDelivery.fn_qty},
                                new string[]{_Metas.Delivery.fn_shipDate, DeletedDelivery.fn_shipDate},
                                new string[]{_Metas.Delivery.fn_shipmentNo, DeletedDelivery.fn_shipmentNo},
                                new string[]{_Metas.Delivery.fn_status, DeletedDelivery.fn_status},
                                new string[]{_Metas.Delivery.fn_udt, DeletedDelivery.fn_udt}
                            },
                            new ConditionCollection<_Metas.Delivery>(new EqualCondition<_Metas.Delivery>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_shipmentNo).Value = shipmentNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BackupToDeliveryPalletByShipmentNo(string shipmentNo)
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
                        _Metas.Delivery_Pallet cond = new _Metas.Delivery_Pallet();
                        cond.shipmentNo = shipmentNo;
                        sqlCtx = FuncNew.GetConditionedForBackupInsert<_Metas.Delivery_Pallet, _Metas.DeletedDeliveryPallet>(tk,
                            new string[][]{
                                new string[]{Delivery_Pallet.fn_cdt, DeletedDeliveryPallet.fn_cdt},
                                new string[]{Delivery_Pallet.fn_deliveryNo, DeletedDeliveryPallet.fn_deliveryNo},
                                new string[]{Delivery_Pallet.fn_deliveryQty, DeletedDeliveryPallet.fn_deliveryQty},
                                new string[]{Delivery_Pallet.fn_editor, DeletedDeliveryPallet.fn_editor},
                                new string[]{Delivery_Pallet.fn_id, DeletedDeliveryPallet.fn_id},
                                new string[]{Delivery_Pallet.fn_palletNo, DeletedDeliveryPallet.fn_palletNo},
                                new string[]{Delivery_Pallet.fn_shipmentNo, DeletedDeliveryPallet.fn_shipmentNo},
                                new string[]{Delivery_Pallet.fn_status, DeletedDeliveryPallet.fn_status},
                                new string[]{Delivery_Pallet.fn_udt, DeletedDeliveryPallet.fn_udt},
                                new string[]{Delivery_Pallet.fn_deviceQty, DeletedDeliveryPallet.fn_deviceQty},
                                new string[]{Delivery_Pallet.fn_palletType, DeletedDeliveryPallet.fn_palletType}
                            },
                            new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_shipmentNo).Value = shipmentNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryByShipmentNo(string shipmentNo)
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
                        mtns::Delivery cond = new mtns::Delivery();
                        cond.shipmentNo = shipmentNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Delivery>(tk, new ConditionCollection<mtns::Delivery>(new EqualCondition<mtns::Delivery>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Delivery.fn_shipmentNo).Value = shipmentNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryInfoByShipmentNo(string shipmentNo)
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
                        mtns::DeliveryInfo cond = new mtns::DeliveryInfo();
                        cond.deliveryNo = shipmentNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::DeliveryInfo>(tk, new ConditionCollection<mtns::DeliveryInfo>(new AnyCondition<mtns::DeliveryInfo>(cond, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4})", "{0}", mtns::Delivery.fn_deliveryNo, ToolsNew.GetTableName(typeof(mtns::Delivery)), mtns::Delivery.fn_shipmentNo, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::DeliveryInfo.fn_deliveryNo)).Value = shipmentNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetInfoValuesByPalletNoOnDeliveryNo(string palletNo)
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
                        tf1 = new TableAndFields<_Metas.Delivery_Pallet>();
                        _Metas.Delivery_Pallet cond = new _Metas.Delivery_Pallet();
                        cond.palletNo = palletNo;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Delivery_Pallet>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        tf2.AddRangeToGetFieldNames(_Metas.DeliveryInfo.fn_infoValue); 

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery_Pallet, _Metas.DeliveryInfo>(tf1, _Metas.Delivery_Pallet.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery_Pallet.fn_palletNo)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)));
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

        public IList<string> GetInfoValuesByPalletNoOnDeliveryNo(string palletNo, string infoType)
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
                        tf1 = new TableAndFields<_Metas.Delivery_Pallet>();
                        _Metas.Delivery_Pallet cond = new _Metas.Delivery_Pallet();
                        cond.palletNo = palletNo;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Delivery_Pallet>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoType = infoType;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.DeliveryInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery_Pallet, _Metas.DeliveryInfo>(tf1, _Metas.Delivery_Pallet.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery_Pallet.fn_palletNo)).Value = palletNo;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)));
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

        public IList<string> GetDeliveryNoByShipDateAndValueAndType(DateTime shipdate, string infoValue, string infoType)
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
                        tf1 = new TableAndFields<_Metas.Delivery>();
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.shipDate = shipdate;
                        tf1.Conditions.Add(new GreaterOrEqualCondition<_Metas.Delivery>(cond));
                        tf1.Conditions.Add(new SmallerCondition<_Metas.Delivery>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.Delivery.fn_deliveryNo);

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoType = infoType;
                        cond2.infoValue = infoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                DateTime thisDay = new DateTime(shipdate.Year, shipdate.Month, shipdate.Day, 0, 0, 0);
                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecGE(_Metas.Delivery.fn_shipDate))).Value = thisDay;
                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecS(_Metas.Delivery.fn_shipDate))).Value = thisDay.AddDays(1);
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();//ITC-1360-1484
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_deliveryNo)));
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

        public IList<string> GetShipmentNoByShipDateAndValueAndType(DateTime shipdate, string infoValue, string infoType)
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
                        tf1 = new TableAndFields<_Metas.Delivery>();
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.shipDate = shipdate;
                        tf1.Conditions.Add(new GreaterOrEqualCondition<_Metas.Delivery>(cond));
                        tf1.Conditions.Add(new SmallerCondition<_Metas.Delivery>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.Delivery.fn_shipmentNo);

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoType = infoType;
                        cond2.infoValue = infoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                DateTime thisDay = new DateTime(shipdate.Year, shipdate.Month, shipdate.Day, 0, 0, 0);
                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecGE(_Metas.Delivery.fn_shipDate))).Value = thisDay;
                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecS(_Metas.Delivery.fn_shipDate))).Value = thisDay.AddDays(1);
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Delivery.fn_shipmentNo)));
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

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryByValueAndType(string infoValue, string infoType)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Delivery>();

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoType = infoType;
                        cond2.infoValue = infoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2,_Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryByModelInfoName(string status, string vendorCT, string modelInfoName, string _146Models)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Delivery>();
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.status = status;
                        tf1.Conditions.Add(new SmallerCondition<_Metas.Delivery>(cond));
                        _Metas.Delivery cond1 = new _Metas.Delivery();
                        cond1.model = vendorCT;
                        tf1.Conditions.Add(new AnyCondition<_Metas.Delivery>(cond1, string.Format("{0} IN (SELECT {1} FROM {2}..{3} NOLOCK WHERE {4} IN (SELECT {5} FROM {2}..{6} NOLOCK WHERE {7}={8}))", "{0}", _Metas.Product.fn_model, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_productID, _Metas.Product_Part.fn_productID, ToolsNew.GetTableName(typeof(_Metas.Product_Part)), _Metas.Product_Part.fn_partSn, "{1}")));
                        

                        tf2 = new TableAndFields<_Metas.ModelInfo>();
                        _Metas.ModelInfo cond2 = new _Metas.ModelInfo();
                        cond2.name = modelInfoName;
                        tf2.Conditions.Add(new EqualCondition<_Metas.ModelInfo>(cond2));
                        _Metas.ModelInfo cond21 = new _Metas.ModelInfo();
                        cond21.value = _146Models;
                        tf2.Conditions.Add(new AnyCondition<_Metas.ModelInfo>(cond21, "CHARINDEX({0},{1})>0"));
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_BOM;
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.ModelInfo>(tf1, _Metas.Delivery.fn_model, tf2, _Metas.ModelInfo.fn_model));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts, "t1." + _Metas.Delivery.fn_deliveryNo, "t1." + _Metas.Delivery.fn_shipDate, "t1." + _Metas.Delivery.fn_qty);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecS(_Metas.Delivery.fn_status))).Value = status;
                sqlCtx.Param(g.DecAlias(tf1.Alias, g.DecAny(_Metas.Delivery.fn_model))).Value = vendorCT;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.ModelInfo.fn_name)).Value = modelInfoName;
                sqlCtx.Param(g.DecAlias(tf2.Alias, g.DecAny(_Metas.ModelInfo.fn_value))).Value = _146Models;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryByVendorCT(string status, string vendorCT, string _146Models)
        {
            try
            {
                IList<Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.status = status;

                        _Metas.Delivery cond1 = new _Metas.Delivery();
                        cond1.model = vendorCT;

                        _Metas.Delivery cond21 = new _Metas.Delivery();
                        cond21.model = _146Models;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null, new ConditionCollection<_Metas.Delivery>(
                            new SmallerCondition<_Metas.Delivery>(cond),
                            new AnyCondition<_Metas.Delivery>(cond1, string.Format("{0} IN (SELECT {1} FROM {2}..{3} NOLOCK WHERE {4} IN (SELECT {5} FROM {2}..{6} NOLOCK WHERE {7}={8}))", "{0}", _Metas.Product.fn_model, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(_Metas.Product)), _Metas.Product.fn_productID, _Metas.Product_Part.fn_productID, ToolsNew.GetTableName(typeof(_Metas.Product_Part)), _Metas.Product_Part.fn_partSn, "{1}")),
                            new AnyCondition<_Metas.Delivery>(cond21, "CHARINDEX({0},{1})>0"))
                            ,_Metas.Delivery.fn_deliveryNo, _Metas.Delivery.fn_shipDate, _Metas.Delivery.fn_qty);
                    }
                }
                sqlCtx.Param(g.DecS(_Metas.Delivery.fn_status)).Value = status;
                sqlCtx.Param(g.DecAny(_Metas.Delivery.fn_model)).Value = vendorCT;
                sqlCtx.Param(g.DecAny(_Metas.Delivery.fn_model) + "$1").Value = _146Models;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistShipBoxDet(string dn, string plt)
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
                        _Metas.ShipBoxDet cond = new _Metas.ShipBoxDet();
                        cond.deliveryNo = dn;
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ShipBoxDet>(tk, "COUNT", new string[] { _Metas.ShipBoxDet.fn_id }, new ConditionCollection<_Metas.ShipBoxDet>(new EqualCondition<_Metas.ShipBoxDet>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.ShipBoxDet.fn_deliveryNo).Value = dn;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_plt).Value = plt;

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

        public void AddPoDataEdiInfo(PoDataEdiInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::PoData_EDI>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::PoData_EDI, PoDataEdiInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::PoData_EDI.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::PoData_EDI.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPoPltEdiInfo(PoPltEdiInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::PoPlt_EDI>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::PoPlt_EDI, PoPltEdiInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::PoPlt_EDI.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::PoPlt_EDI.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateUdtForPoDataEdi(string dn)
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
                        PoData_EDI cond = new PoData_EDI();
                        cond.deliveryNo = dn;
                        PoData_EDI setv = new PoData_EDI();
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<PoData_EDI>(tk, new SetValueCollection<PoData_EDI>(new CommonSetValue<PoData_EDI>(setv)), new ConditionCollection<PoData_EDI>(new EqualCondition<PoData_EDI>(cond)));
                    }
                }
                sqlCtx.Param(PoData_EDI.fn_deliveryNo).Value = dn;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(PoData_EDI.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPoDataEdi(string dn)
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
                        _Metas.PoData_EDI cond = new _Metas.PoData_EDI();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PoData_EDI>(tk, "COUNT", new string[] { _Metas.PoData_EDI.fn_id }, new ConditionCollection<_Metas.PoData_EDI>(new EqualCondition<_Metas.PoData_EDI>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.PoData_EDI.fn_deliveryNo).Value = dn;

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

        public bool CheckExistShipBoxDetExceptPlt(string dn, string pltExcept, string snoId)
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
                        _Metas.ShipBoxDet cond = new _Metas.ShipBoxDet();
                        cond.deliveryNo = dn;
                        cond.snoId = snoId;
                        _Metas.ShipBoxDet cond2 = new _Metas.ShipBoxDet();
                        cond2.plt = pltExcept;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ShipBoxDet>(tk, "COUNT", new string[] { _Metas.ShipBoxDet.fn_id }, new ConditionCollection<_Metas.ShipBoxDet>(
                            new EqualCondition<_Metas.ShipBoxDet>(cond),
                            new NotEqualCondition<_Metas.ShipBoxDet>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.ShipBoxDet.fn_deliveryNo).Value = dn;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_snoId).Value = snoId;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_plt).Value = pltExcept;

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

        public void UpdateShipBoxDetForClearSnoid(string snoId, string dn)
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
                        ShipBoxDet cond = new ShipBoxDet();
                        cond.deliveryNo = dn;
                        cond.snoId = snoId;
                        ShipBoxDet setv = new ShipBoxDet();
                        setv.snoId = string.Empty;
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<ShipBoxDet>(tk, new SetValueCollection<ShipBoxDet>(new CommonSetValue<ShipBoxDet>(setv)), new ConditionCollection<ShipBoxDet>(new EqualCondition<ShipBoxDet>(cond)));
                        sqlCtx.Param(g.DecSV(ShipBoxDet.fn_snoId)).Value = setv.snoId;
                    }
                }
                sqlCtx.Param(ShipBoxDet.fn_deliveryNo).Value = dn;
                sqlCtx.Param(ShipBoxDet.fn_snoId).Value = snoId;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(ShipBoxDet.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistShipBoxDet(string dn, string plt, string snoId)
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
                        _Metas.ShipBoxDet cond = new _Metas.ShipBoxDet();
                        cond.deliveryNo = dn;
                        cond.snoId = snoId;
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ShipBoxDet>(tk, "COUNT", new string[] { _Metas.ShipBoxDet.fn_id }, new ConditionCollection<_Metas.ShipBoxDet>(
                            new EqualCondition<_Metas.ShipBoxDet>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.ShipBoxDet.fn_deliveryNo).Value = dn;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_plt).Value = plt;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_snoId).Value = snoId;

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

        public IList<ShipBoxDetInfo> GetShipBoxDetList(string dn, string plt, string snoId)
        {
            try
            {
                IList<ShipBoxDetInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.ShipBoxDet cond = new _Metas.ShipBoxDet();
                        cond.deliveryNo = dn;
                        cond.snoId = snoId;
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ShipBoxDet>(tk, null, null, new ConditionCollection<_Metas.ShipBoxDet>(
                            new EqualCondition<_Metas.ShipBoxDet>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.ShipBoxDet.fn_deliveryNo).Value = dn;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_plt).Value = plt;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_snoId).Value = snoId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.ShipBoxDet, ShipBoxDetInfo, ShipBoxDetInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetAndUpdateShipBoxDet(string dn, string plt, string snoId)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update top (1) ShipBoxDet with (rowlock, readpast)
                                                            set SnoId=@SnoId,
                                                                  Editor= @SnoId+Editor,
                                                                Udt =GETDATE() 
                                                            output DELETED.BoxId 
                                                            where DeliveryNo= @DN and
                                                                  PLT=@Plt  and
                                                                  SnoId='' ";


                        sqlCtx.AddParam("SnoId", new SqlParameter("@SnoId", SqlDbType.VarChar));
                        sqlCtx.AddParam("DN", new SqlParameter("@DN", SqlDbType.VarChar));
                        sqlCtx.AddParam("Plt", new SqlParameter("@Plt", SqlDbType.VarChar));
                      
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DN").Value = dn;
                sqlCtx.Param("Plt").Value = plt;
                sqlCtx.Param("SnoId").Value = snoId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, 
                                                                                                                             CommandType.Text, 
                                                                                                                              sqlCtx.Sentence, 
                                                                                                                               sqlCtx.Params))
                {
                    //ret = FuncNew.SetFieldFromColumn<_Metas.ShipBoxDet, ShipBoxDetInfo, ShipBoxDetInfo>(ret, sqlR, sqlCtx);
                    if (sqlR != null && sqlR.Read())
                    {
                        //ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret.Add( g.GetValue_Str(sqlR, 0));
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
        ///   Rollback before assign boxid, update 
        ///   update ShipBoxDet set SnoId='', Editor=replace(Editor,@snoid,''),Udt=getdate()
        ///   where Editor like @snoid+%
        /// </summary>
        /// <param name="snoId"></param>
        public void RollBackAssignBoxId(string snoId)
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
                       sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @"update ShipBoxDet
                                                            set SnoId='',
                                                                  Editor= replace(Editor,@SnoId,''),
                                                                Udt =GETDATE() 
                                                             where Editor like @Editor ";


                       sqlCtx.AddParam("SnoId", new SqlParameter("@SnoId", SqlDbType.VarChar));
                       sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }
           
               sqlCtx.Param("SnoId").Value = snoId;
               sqlCtx.Param("Editor").Value = snoId+"%";


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


        /// <summary>
        ///  succeed assign Boxid ,then change editor to original 
        ///  Update ShipBoxDet set Editor=replace(Editor,@snoid,''),Udt=getdate()
        ///  where DeliveryNo=@dn and SnoId=@snoid
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="snoId"></param>
        public void UpdateAssignBoxIdEditor(string dn, string plt, string snoId)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update ShipBoxDet
                                                            set Editor= replace(Editor,@SnoId,''),
                                                                  Udt =GETDATE() 
                                                            where DeliveryNo= @DN and
                                                                       PLT=@Plt  and
                                                                       SnoId=@SnoId";


                        sqlCtx.AddParam("SnoId", new SqlParameter("@SnoId", SqlDbType.VarChar));
                        sqlCtx.AddParam("DN", new SqlParameter("@DN", SqlDbType.VarChar));
                        sqlCtx.AddParam("Plt", new SqlParameter("@Plt", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DN").Value = dn;
                sqlCtx.Param("Plt").Value = plt;
                sqlCtx.Param("SnoId").Value = snoId;


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

        public void UpdateAssignBoxIdEditorDefered(IUnitOfWork uow, string dn, string plt, string snoId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn, plt,snoId);
        }

        public void UpdateShipBoxDetForSetSnoId(string snoid, string dn, string plt, string boxId)
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
                        ShipBoxDet cond = new ShipBoxDet();
                        cond.deliveryNo = dn;
                        cond.plt = plt;
                        cond.boxId = boxId;
                        ShipBoxDet setv = new ShipBoxDet();
                        setv.snoId = snoid;
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<ShipBoxDet>(tk, new SetValueCollection<ShipBoxDet>(new CommonSetValue<ShipBoxDet>(setv)), new ConditionCollection<ShipBoxDet>(new EqualCondition<ShipBoxDet>(cond)));
                    }
                }
                sqlCtx.Param(ShipBoxDet.fn_deliveryNo).Value = dn;
                sqlCtx.Param(ShipBoxDet.fn_plt).Value = plt;
                sqlCtx.Param(ShipBoxDet.fn_boxId).Value = boxId;
                sqlCtx.Param(g.DecSV(ShipBoxDet.fn_snoId)).Value = snoid;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(ShipBoxDet.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ShipBoxDetInfo> GetShipBoxDetList(string dn, string snoId)
        {
            try
            {
                IList<ShipBoxDetInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.ShipBoxDet cond = new _Metas.ShipBoxDet();
                        cond.deliveryNo = dn;
                        cond.snoId = snoId;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.ShipBoxDet>(tk, null, null, new ConditionCollection<_Metas.ShipBoxDet>(
                            new EqualCondition<_Metas.ShipBoxDet>(cond)), _Metas.ShipBoxDet.fn_plt, _Metas.ShipBoxDet.fn_boxId);
                    }
                }
                sqlCtx.Param(_Metas.ShipBoxDet.fn_deliveryNo).Value = dn;
                sqlCtx.Param(_Metas.ShipBoxDet.fn_snoId).Value = snoId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.ShipBoxDet, ShipBoxDetInfo, ShipBoxDetInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryListByCdtAboveAndStatusesAndModel(DateTime beginCdt, string[] statuses, string model)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.model = model;
                        mtns.Delivery cond2 = new mtns.Delivery();
                        cond2.status = "[INSET]";
                        mtns.Delivery cond3 = new mtns.Delivery();
                        cond3.shipDate = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery>(tk, null, null, new ConditionCollection<mtns.Delivery>(
                            new EqualCondition<mtns.Delivery>(cond),
                            new InSetCondition<mtns.Delivery>(cond2),
                            new GreaterCondition<mtns.Delivery>(cond3)), mtns.Delivery.fn_shipDate);
                    }
                }
                sqlCtx.Param(mtns.Delivery.fn_model).Value = model;
                sqlCtx.Param(g.DecG(mtns.Delivery.fn_shipDate)).Value = beginCdt;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Delivery.fn_status), g.ConvertInSet(statuses));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfBoundDeliveryByCdtAboveAndStatusesAndModel(DateTime beginCdt, string[] statuses, string model)
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
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.model = model;
                        mtns.Delivery cond2 = new mtns.Delivery();
                        cond2.status = "[INSET]";
                        mtns.Delivery cond3 = new mtns.Delivery();
                        cond3.cdt = DateTime.Now;
                        mtns.Delivery cond4 = new mtns.Delivery();
                        cond4.deliveryNo = "deliveryNo";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery>(tk, "COUNT", new string[] { mtns.Delivery.fn_deliveryNo }, new ConditionCollection<mtns.Delivery>(
                            new EqualCondition<mtns.Delivery>(cond),
                            new InSetCondition<mtns.Delivery>(cond2),
                            new GreaterCondition<mtns.Delivery>(cond3),
                            new AnySoloCondition<mtns.Delivery>(cond4, string.Format("{0} IN (SELECT DISTINCT {1} FROM {2}..{3})", "{0}", mtns.Product.fn_deliveryNo, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(mtns.Product))))));
                    }
                }
                sqlCtx.Param(mtns.Delivery.fn_model).Value = model;
                sqlCtx.Param(g.DecG(mtns.Delivery.fn_cdt)).Value = beginCdt;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Delivery.fn_status), g.ConvertInSet(statuses));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
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

        public IList<string> GetDeliveryListByShipmentAndShipDateInNearlyDays(string shipmentNo, int day)
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
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.shipmentNo = shipmentNo;
                        mtns.Delivery cond2 = new mtns.Delivery();
                        cond2.shipDate = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery>(tk, null, new string[] { mtns.Delivery.fn_deliveryNo }, new ConditionCollection<mtns.Delivery>(
                            new AnyCondition<mtns.Delivery>(cond, string.Format("({0}={1} OR {2}={1})", mtns.Delivery.fn_deliveryNo,"{1}","{0}")),
                            new GreaterCondition<mtns.Delivery>(cond2, null, "CONVERT(CHAR(10),{0},111)")), mtns.Delivery.fn_deliveryNo);
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.Delivery.fn_shipmentNo)).Value = shipmentNo;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime().AddDays( - day);
                sqlCtx.Param(g.DecG(mtns.Delivery.fn_shipDate)).Value = new DateTime(cmDt.Year, cmDt.Month, cmDt.Day);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery.fn_deliveryNo));
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

        public bool IsCombinedPallet(string dn)
        {
            try
            {
                bool ret = false;

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
                        tf1 = new TableAndFields<_Metas.Delivery_Pallet>();
                        _Metas.Delivery_Pallet cond = new _Metas.Delivery_Pallet();
                        cond.deliveryNo = dn;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Delivery_Pallet>(cond));
                        tf1.AddRangeToGetFieldNames(_Metas.Delivery_Pallet.fn_id);

                        tf2 = new TableAndFields<_Metas.Delivery_Pallet>();
                        _Metas.Delivery_Pallet cond2 = new _Metas.Delivery_Pallet();
                        cond2.deliveryNo = dn;
                        tf2.Conditions.Add(new NotEqualCondition<_Metas.Delivery_Pallet>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery_Pallet, _Metas.Delivery_Pallet>(tf1, _Metas.Delivery_Pallet.fn_palletNo, tf2, _Metas.Delivery_Pallet.fn_palletNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Delivery_Pallet.fn_deliveryNo)).Value = dn;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Delivery_Pallet.fn_deliveryNo)).Value = dn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0 ? false : true;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPalletWithTheSameShipmentNoPrefix(string palletNo)
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
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.palletNo = palletNo;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "COUNT", new string[] { mtns.Delivery_Pallet.fn_id }, new ConditionCollection<mtns.Delivery_Pallet>(
                            new AnyCondition<mtns.Delivery_Pallet>(cond, string.Format("LEFT({0}, 10) IN (SELECT DISTINCT LEFT({0}, 10) FROM {1} WHERE {2}={3})",mtns.Delivery_Pallet.fn_shipmentNo, ToolsNew.GetTableName(typeof(mtns.Delivery_Pallet)),"{0}","{1}" ))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.Delivery_Pallet.fn_palletNo)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public int GetCountOfDeliveryNoWithTheSameDeliveryNoPrefix(string deliveryNo)
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
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.deliveryNo = deliveryNo;

                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns.Delivery>(tk, "COUNT", new string[][] { new string[] { mtns.Delivery.fn_deliveryNo, string.Format("DISTINCT LEFT({0}, 10)", mtns.Delivery.fn_deliveryNo) } }, new ConditionCollection<mtns.Delivery>(
                            new AnyCondition<mtns.Delivery>(cond, string.Format("{0} LIKE LEFT({1}, 10) + '%'", "{0}", "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.Delivery.fn_deliveryNo)).Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public int GetCountOfPalletOfDeliveryWithInfoTypeAndInfoValue(string infoType, string infoValue)
        {
            try
            {
                int ret = 0;

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
                        tf1 = new TableAndFields<_Metas.Delivery_Pallet>();
                        tf1.AddRangeToGetFieldNames(_Metas.Delivery_Pallet.fn_palletNo);

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoType = infoType;
                        cond.infoValue = infoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery_Pallet, _Metas.DeliveryInfo>(tf1, _Metas.Delivery_Pallet.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT(DISTINCT {0})", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT(DISTINCT {0})"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetShipmentNoListWithSimilarShipmentNo(string shipmentNo)
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
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.shipmentNo = shipmentNo;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_shipmentNo }, new ConditionCollection<mtns.Delivery_Pallet>(
                            new AnyCondition<mtns.Delivery_Pallet>(cond, string.Format("({0} LIKE LEFT({1}, 10) + '%' OR {0}={1})", "{0}", "{1}"))), mtns.Delivery_Pallet.fn_shipmentNo);
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.Delivery_Pallet.fn_shipmentNo)).Value = shipmentNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_shipmentNo));
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

        public IList<string> GetPalletNoListWithSimilarShipmentNo(string shipmentNo)
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
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.shipmentNo = shipmentNo;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_palletNo }, new ConditionCollection<mtns.Delivery_Pallet>(
                            new AnyCondition<mtns.Delivery_Pallet>(cond, string.Format("({0} LIKE LEFT({1}, 10) + '%' OR {0}={1})", "{0}", "{1}"))), mtns.Delivery_Pallet.fn_palletNo);
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.Delivery_Pallet.fn_shipmentNo)).Value = shipmentNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_palletNo));
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

        public IList<PalletNoAndDeliveryQtyEntity> GetDistinctPalletNoAndDeliveryQtyWithSimilarShipmentNo(string shipmentNo)
        {
            try
            {
                IList<PalletNoAndDeliveryQtyEntity> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.shipmentNo = shipmentNo;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_palletNo, mtns.Delivery_Pallet.fn_deliveryQty }, new ConditionCollection<mtns.Delivery_Pallet>(
                            new AnyCondition<mtns.Delivery_Pallet>(cond, string.Format("({0} LIKE LEFT({1}, 10) + '%' OR {0}={1})", "{0}", "{1}"))), mtns.Delivery_Pallet.fn_palletNo);
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.Delivery_Pallet.fn_shipmentNo)).Value = shipmentNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<PalletNoAndDeliveryQtyEntity>();
                        while (sqlR.Read())
                        {
                            PalletNoAndDeliveryQtyEntity item = new PalletNoAndDeliveryQtyEntity();
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_palletNo));
                            item.DeliveryQty = g.GetValue_Int16(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_deliveryQty));
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

        public IList<string> GetShipmentNoListOfDeliveryWithInfoTypeAndInfoValue(string infoType, string infoValue)
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
                        tf1 = new TableAndFields<_Metas.Delivery_Pallet>();
                        tf1.AddRangeToGetFieldNames(_Metas.Delivery_Pallet.fn_shipmentNo);

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoType = infoType;
                        cond.infoValue = infoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery_Pallet, _Metas.DeliveryInfo>(tf1, _Metas.Delivery_Pallet.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns.Delivery_Pallet.fn_shipmentNo)));
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

        public IList<string> GetShipmentNoListByPalletNoAndShipmentNo(string pltNo, string[] shipmentNos)
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
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.palletNo = pltNo;

                        mtns.Delivery_Pallet cond2 = new mtns.Delivery_Pallet();
                        cond2.palletNo = "[NOT INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_shipmentNo }, new ConditionCollection<mtns.Delivery_Pallet>(
                            new EqualCondition<mtns.Delivery_Pallet>(cond),
                            new NotInSetCondition<mtns.Delivery_Pallet>(cond2)
                            ), mtns.Delivery_Pallet.fn_shipmentNo);
                    }
                }
                sqlCtx.Param(mtns.Delivery_Pallet.fn_palletNo).Value = pltNo;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Delivery_Pallet.fn_palletNo), g.ConvertInSet(shipmentNos));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_shipmentNo));
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

        public IList<PalletNoAndDeliveryQtyEntity> GetDistinctPalletNoAndDeliveryQtyByDeliveryNos(string[] deliveryNos)
        {
            try
            {
                IList<PalletNoAndDeliveryQtyEntity> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.deliveryNo = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_palletNo, mtns.Delivery_Pallet.fn_deliveryQty }, new ConditionCollection<mtns.Delivery_Pallet>(
                            new InSetCondition<mtns.Delivery_Pallet>(cond, "LEFT({0}, 10)")), mtns.Delivery_Pallet.fn_palletNo);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Delivery_Pallet.fn_deliveryNo), g.ConvertInSet(deliveryNos));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<PalletNoAndDeliveryQtyEntity>();
                        while (sqlR.Read())
                        {
                            PalletNoAndDeliveryQtyEntity item = new PalletNoAndDeliveryQtyEntity();
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_palletNo));
                            item.DeliveryQty = g.GetValue_Int16(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_deliveryQty));
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

        public IList<PalletNoAndDeliveryQtyEntity> GetDistinctPalletNoAndDeliveryQtyByShipmentNos(string[] shipmentNos)
        {
            try
            {
                IList<PalletNoAndDeliveryQtyEntity> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.shipmentNo = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_palletNo, mtns.Delivery_Pallet.fn_deliveryQty }, new ConditionCollection<mtns.Delivery_Pallet>(
                            new InSetCondition<mtns.Delivery_Pallet>(cond)), mtns.Delivery_Pallet.fn_palletNo);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Delivery_Pallet.fn_shipmentNo), g.ConvertInSet(shipmentNos));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<PalletNoAndDeliveryQtyEntity>();
                        while (sqlR.Read())
                        {
                            PalletNoAndDeliveryQtyEntity item = new PalletNoAndDeliveryQtyEntity();
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_palletNo));
                            item.DeliveryQty = g.GetValue_Int16(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_deliveryQty));
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

        public IList<string> GetExistedPalletNoList(string[] pltNos)
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
                        mtns.Pallet cond = new mtns.Pallet();
                        cond.palletNo = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Pallet>(tk, "DISTINCT", new string[] { mtns.Pallet.fn_palletNo }, new ConditionCollection<mtns.Pallet>(
                            new InSetCondition<mtns.Pallet>(cond)), mtns.Pallet.fn_palletNo);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Pallet.fn_palletNo), g.ConvertInSet(pltNos));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Pallet.fn_palletNo));
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

        public int GetCountOfProductOfDeliveryWithInfoTypeAndInfoValue(string infoType, string infoValue)
        {
            try
            {
                int ret = 0;

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
                        tf1 = new TableAndFields<_Metas.Product>();
                        tf1.AddRangeToGetFieldNames(_Metas.Product.fn_productID);
                        tf1.SubDBCalalog = _Schema.SqlHelper.DB_FA;

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoType = infoType;
                        cond.infoValue = infoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Product, _Metas.DeliveryInfo>(tf1, _Metas.Product.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public int GetCountOfProductWithTheSameShipmentNo(string palletNo)
        {
            try
            {
                int ret = 0;

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
                        tf1 = new TableAndFields<_Metas.Product>();
                        tf1.AddRangeToGetFieldNames(_Metas.Product.fn_productID);
                        tf1.SubDBCalalog = _Schema.SqlHelper.DB_FA;

                        tf2 = new TableAndFields<_Metas.Delivery_Pallet>();
                        _Metas.Delivery_Pallet cond = new _Metas.Delivery_Pallet();
                        cond.shipmentNo = palletNo;
                        tf2.Conditions.Add(new AnyCondition<_Metas.Delivery_Pallet>(cond, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4})", "{0}", _Metas.Delivery_Pallet.fn_shipmentNo, ToolsNew.GetTableName(typeof(_Metas.Delivery_Pallet)), _Metas.Delivery_Pallet.fn_palletNo, "{1}")));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Product, _Metas.Delivery_Pallet>(tf1, _Metas.Product.fn_palletNo, tf2, _Metas.Delivery_Pallet.fn_palletNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, g.DecAny(_Metas.Delivery_Pallet.fn_shipmentNo))).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public bool CheckExistDeliveryByShipmentPrefixAndModelPrefix(string shipment, string modelPrefix)
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
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.model = modelPrefix + "%";

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.deliveryNo = shipment;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, "COUNT", new string[] { _Metas.Delivery.fn_deliveryNo }, new ConditionCollection<_Metas.Delivery>(
                            new LikeCondition<_Metas.Delivery>(cond),
                            new LikeCondition<_Metas.Delivery>(cond2, null, "LEFT({0}, 10) + '%'")
                            ));
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = shipment;
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = modelPrefix + "%";

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

        public IList<string> GetModelsByShipmentPrefixAndModelPrefix(string shipment, string modelPrefix)
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
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.model = modelPrefix + "%";

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.deliveryNo = shipment;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, "DISTINCT", new string[] { _Metas.Delivery.fn_model }, new ConditionCollection<_Metas.Delivery>(
                            new LikeCondition<_Metas.Delivery>(cond),
                            new LikeCondition<_Metas.Delivery>(cond2, null, "LEFT({0}, 10) + '%'")
                            ), _Metas.Delivery.fn_model);
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = shipment;
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = modelPrefix + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery.fn_model));
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

        public int GetSumQtyOfDeliveryByShipmentPrefixAndModelPrefix(string shipment, string modelPrefix)
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
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.model = modelPrefix + "%";

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.deliveryNo = shipment;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, "SUM", new string[] { _Metas.Delivery.fn_qty }, new ConditionCollection<_Metas.Delivery>(
                            new LikeCondition<_Metas.Delivery>(cond),
                            new LikeCondition<_Metas.Delivery>(cond2, null, "LEFT({0}, 10) + '%'")));
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = shipment;
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = modelPrefix + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("SUM"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetBOLQty(string bol)
        {
            try
            {
                int ret = 0;

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
                        tf1 = new TableAndFields<_Metas.Delivery>();
                        tf1.AddRangeToGetFieldNames(_Metas.Delivery.fn_qty);

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoValue = "%" + bol + "%";
                        tf2.Conditions.Add(new LikeCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "SUM", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = "%" + bol + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("SUM"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryNoAndQtyEntity> GetDNAndQtyByDummyPalletNo(string dummyPalletNo)
        {
            try
            {
                IList<DeliveryNoAndQtyEntity> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Dummy_ShipDet>();
                        _Metas.Dummy_ShipDet cond = new _Metas.Dummy_ShipDet();
                        cond.plt = dummyPalletNo;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Dummy_ShipDet>(cond));
                        tf1.AddRangeToGetFuncedFieldNames(new string[]{_Metas.Dummy_ShipDet.fn_snoId, "COUNT({0})"});

                        tf2 = new TableAndFields<_Metas.Product>();
                        tf2.AddRangeToGetFuncedFieldNames(new string[]{_Metas.Product.fn_deliveryNo,"{0}"});
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_FA;

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Dummy_ShipDet, _Metas.Product>(tf1, _Metas.Dummy_ShipDet.fn_snoId, tf2, _Metas.Product.fn_productID));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Dummy_ShipDet.fn_plt)).Value = dummyPalletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DeliveryNoAndQtyEntity>(); 
                        while( sqlR.Read())
                        {
                            DeliveryNoAndQtyEntity item = new DeliveryNoAndQtyEntity();
                            item.DeliveryNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns.Product.fn_deliveryNo)));
                            item.Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, mtns.Dummy_ShipDet.fn_snoId)));
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

        public IList<string> GetCustSnListByDummyPalletNo(string dummyPalletNo)
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
                        tf1 = new TableAndFields<_Metas.Dummy_ShipDet>();
                        _Metas.Dummy_ShipDet cond = new _Metas.Dummy_ShipDet();
                        cond.plt = dummyPalletNo;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Dummy_ShipDet>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.Product>();
                        tf2.AddRangeToGetFieldNames(_Metas.Product.fn_custsn);
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_FA;

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Dummy_ShipDet, _Metas.Product>(tf1, _Metas.Dummy_ShipDet.fn_snoId, tf2, _Metas.Product.fn_productID));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts, g.DecAliasInner("t2", _Metas.Product.fn_custsn));
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Dummy_ShipDet.fn_plt)).Value = dummyPalletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns.Product.fn_custsn)));
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

        public void InsertDummyShipDetInfo(DummyShipDetInfo item)
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
                        sqlCtx = FuncNew.GetCommonInsert<Dummy_ShipDet>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Dummy_ShipDet, DummyShipDetInfo>(sqlCtx, item);

                sqlCtx.Param(Dummy_ShipDet.fn_cdt).Value = cmDt;
                sqlCtx.Param(Dummy_ShipDet.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Delivery GetDelivery(string deliverty_no)
        {
            return Find(deliverty_no);
        }

        public IList<DummyShipDetInfo> GetDummyShipDetInfoList(DummyShipDetInfo condition)
        {
            try
            {
                IList<DummyShipDetInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Dummy_ShipDet cond = FuncNew.SetColumnFromField<Dummy_ShipDet, DummyShipDetInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<Dummy_ShipDet>(null, null, new ConditionCollection<Dummy_ShipDet>(new EqualCondition<Dummy_ShipDet>(cond)), Dummy_ShipDet.fn_snoId);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Dummy_ShipDet, DummyShipDetInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Dummy_ShipDet, DummyShipDetInfo, DummyShipDetInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDummyShipDetInfo(DummyShipDetInfo setValue, DummyShipDetInfo condition)
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
                Dummy_ShipDet cond = FuncNew.SetColumnFromField<Dummy_ShipDet, DummyShipDetInfo>(condition);
                Dummy_ShipDet setv = FuncNew.SetColumnFromField<Dummy_ShipDet, DummyShipDetInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Dummy_ShipDet>(new SetValueCollection<Dummy_ShipDet>(new CommonSetValue<Dummy_ShipDet>(setv)), new ConditionCollection<Dummy_ShipDet>(new EqualCondition<Dummy_ShipDet>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Dummy_ShipDet, DummyShipDetInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Dummy_ShipDet, DummyShipDetInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Dummy_ShipDet.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPalletNoListByShipmentNo(string shipmentNo)
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
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.shipmentNo = shipmentNo;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_palletNo }, new ConditionCollection<mtns.Delivery_Pallet>(new EqualCondition<mtns.Delivery_Pallet>(cond)), mtns.Delivery_Pallet.fn_palletNo);
                    }
                }
                sqlCtx.Param(mtns.Delivery_Pallet.fn_shipmentNo).Value = shipmentNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_palletNo));
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

        public IList<string> GetPalletNoListByDeliveryNo(string deliveryNo)
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
                        mtns.Delivery_Pallet cond = new mtns.Delivery_Pallet();
                        cond.deliveryNo = deliveryNo;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns.Delivery_Pallet>(tk, "DISTINCT", new string[] { mtns.Delivery_Pallet.fn_palletNo }, new ConditionCollection<mtns.Delivery_Pallet>(new EqualCondition<mtns.Delivery_Pallet>(cond)), mtns.Delivery_Pallet.fn_palletNo);
                    }
                }
                sqlCtx.Param(mtns.Delivery_Pallet.fn_deliveryNo).Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery_Pallet.fn_palletNo));
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

        public IList<Delivery> GetDeliveryListByInfoValue(string infoValue)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Delivery>();

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        _Metas.DeliveryInfo cond2 = new _Metas.DeliveryInfo();
                        cond2.infoValue = infoValue;
                        tf2.Conditions.Add(new EqualCondition<_Metas.DeliveryInfo>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery, _Metas.DeliveryInfo>(tf1, _Metas.Delivery.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.DeliveryInfo.fn_infoValue)).Value = infoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, IMES.FisObject.PAK.DN.Delivery, IMES.FisObject.PAK.DN.Delivery>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteShipBoxDetByShipmentNo(string shipment)
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
                        mtns::ShipBoxDet cond = new mtns::ShipBoxDet();
                        cond.deliveryNo = shipment;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::ShipBoxDet>(tk, new ConditionCollection<mtns::ShipBoxDet>(
                            new AnyCondition<mtns::ShipBoxDet>(cond, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4})", "{0}", mtns.Delivery.fn_deliveryNo, ToolsNew.GetTableName(typeof(mtns.Delivery)), mtns.Delivery.fn_shipmentNo, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::ShipBoxDet.fn_deliveryNo)).Value = shipment;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateShipBoxDetInfo(ShipBoxDetInfo setValue, ShipBoxDetInfo condition)
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
                ShipBoxDet cond = FuncNew.SetColumnFromField<ShipBoxDet, ShipBoxDetInfo>(condition);
                ShipBoxDet setv = FuncNew.SetColumnFromField<ShipBoxDet, ShipBoxDetInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<ShipBoxDet>(new SetValueCollection<ShipBoxDet>(new CommonSetValue<ShipBoxDet>(setv)), new ConditionCollection<ShipBoxDet>(new EqualCondition<ShipBoxDet>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ShipBoxDet, ShipBoxDetInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<ShipBoxDet, ShipBoxDetInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.ShipBoxDet.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDeliveryForStatusChange(string[] dns, string newTitleChar)
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
                        mtns.Delivery setValue = new mtns.Delivery();
                        setValue.status = newTitleChar;
                        mtns.Delivery cond = new mtns.Delivery();
                        cond.deliveryNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedUpdate(tk, new SetValueCollection<mtns.Delivery>(
                                new ForIncSetValue<mtns.Delivery>(setValue, "{0}={1}+SUBSTRING({0},2,1)")), new ConditionCollection<mtns.Delivery>(
                                new InSetCondition<mtns.Delivery>(cond)));
                    }
                }
                sqlCtx.Param(g.DecInc(mtns.Delivery.fn_status)).Value = newTitleChar;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Delivery.fn_deliveryNo), g.ConvertInSet(new List<string>(dns)));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DNInfoForUI> GetPakDotPakComnInKeyValue(string internalId)
        {
            try
            {
                IList<DNInfoForUI> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PakDotpakcomn cond = new mtns::PakDotpakcomn();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::PakDotpakcomn>(tk, "TOP 1", null, new ConditionCollection<mtns::PakDotpakcomn>(new EqualCondition<mtns::PakDotpakcomn>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PakDotpakcomn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if(sqlR.Read())
                        {
                            ret = new List<DNInfoForUI>();
                            var res = FuncNew.SetFieldFromColumnInKeyPair<_Metas.PakDotpakcomn>(sqlR, sqlCtx);
                            if (res != null && res.Count > 0)
                            {
                                int i = 1;
                                foreach (string[] re in res)
                                {
                                    DNInfoForUI entry = new DNInfoForUI();
                                    entry.InfoType = re[0];
                                    entry.InfoValue = re[1];
                                    entry.ID = i;
                                    ret.Add(entry);
                                    i++;
                                }
                            }
                            ret = (from re in ret orderby re.InfoType select re).ToList();
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

        public IList<DNPalletQty> GetPakDotPakPaltnoInfoList(string internalId)
        {
            try
            {
                IList<DNPalletQty> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        PakDotpakpaltno cond = new PakDotpakpaltno();
                        cond.internalID = internalId;
                        sqlCtx = FuncNew.GetConditionedSelect<PakDotpakpaltno>(tk, null, new string[] { PakDotpakpaltno.fn_pallet_id, PakDotpakpaltno.fn_pallet_unit_qty, PakDotpakpaltno.fn_id },
                            new ConditionCollection<PakDotpakpaltno>(new EqualCondition<PakDotpakpaltno>(cond)), PakDotpakpaltno.fn_pallet_id);
                    }
                }
                sqlCtx.Param(mtns::PakDotpakcomn.fn_internalID).Value = internalId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletQty>();
                        while (sqlR.Read())
                        {
                            DNPalletQty item = new DNPalletQty();
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakpaltno.fn_pallet_id));
                            item.DeliveryQty = int.Parse(g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakpaltno.fn_pallet_unit_qty)));
                            item.UCC = string.Empty;
                            item.ID = g.GetValue_Int32(sqlR, sqlCtx.Indexes(PakDotpakpaltno.fn_id));
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

        public IList<DNForUI> GetDNListByDNListFromPakDotPakComn(IList<string> dnList)
        {
            try
            {
                IList<DNForUI> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::PakDotpakcomn cond = new mtns::PakDotpakcomn();
                        cond.internalID = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns::PakDotpakcomn>(tk, null, 
                            new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            },
                            new ConditionCollection<mtns::PakDotpakcomn>(new InSetCondition<mtns::PakDotpakcomn>(cond)), mtns::PakDotpakcomn.fn_internalID);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PakDotpakcomn.fn_internalID), g.ConvertInSet(dnList));
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNForUI>();
                        while (sqlR.Read())
                        {
                            DNForUI item = new DNForUI();
                            item.DeliveryNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_internalID));
                            item.ShipmentID = g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_consol_invoice));
                            item.PoNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_po_num));
                            item.ModelName = g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_model));
                            item.ShipDate_Str = g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_actual_shipdate));
                            item.ShipDate = From8BitDateTime(GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_actual_shipdate)));
                            item.Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_box_unit_qty));
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

        public IList<DNForUI> GetDNListByConditionFromPakDotPakComn(DNQueryCondition MyCondition, out int totalLength)
        {
            try
            {
                totalLength = 0;
                IList<DNForUI> ret = new List<DNForUI>();

                SQLContextNew sqlCtx = null;

                SQLContextCollectionNew sqlCtxCllctn = new SQLContextCollectionNew();

                int i = 0;
                if (!string.IsNullOrEmpty(MyCondition.DNInfoValue))
                {
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPakDotPakComn_DNInfoValue(MyCondition.DNInfoValue));
                }
                if (!string.IsNullOrEmpty(MyCondition.DeliveryNo))
                {
                    if (MyCondition.DeliveryNo.Length == 16)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPakDotPakComn_DeliveryNo2(MyCondition.DeliveryNo));
                    else if (MyCondition.DeliveryNo.Length == 10)
                        sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPakDotPakComn_DeliveryNo(MyCondition.DeliveryNo));
                }

                if (!string.IsNullOrEmpty(MyCondition.Model))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPakDotPakComn_Model(MyCondition.Model));

                if (!string.IsNullOrEmpty(MyCondition.PONo))
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPakDotPakComn_PONo(MyCondition.PONo));

                if (DateTime.MinValue != MyCondition.ShipDateFrom)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPakDotPakComn_ShipDateFrom(MyCondition.ShipDateFrom));

                if (DateTime.MinValue != MyCondition.ShipDateTo)
                    sqlCtxCllctn.AddOne(i++, ComposeForGetDNListByConditionFromPakDotPakComn_ShipDateTo(MyCondition.ShipDateTo.AddDays(1)));

                if (i > 0)
                {
                    sqlCtx = sqlCtxCllctn.MergeToOneAndQuery();

                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Metas.PakDotpakcomn.fn_internalID);

                    using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                    {
                        if (sqlR != null)
                        {
                            while (sqlR.Read())
                            {
                                if (totalLength < 1000)
                                {
                                    DNForUI item = new DNForUI();
                                    item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_internalID));
                                    item.ShipmentID = GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.PakDotpakcomn.fn_consol_invoice));
                                    item.PoNo = GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_po_num));
                                    item.ModelName = GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_model));
                                    item.ShipDate_Str = g.GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_actual_shipdate));
                                    item.ShipDate = From8BitDateTime(GetValue_Str(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_actual_shipdate)));
                                    item.Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(PakDotpakcomn.fn_box_unit_qty));
                                    ret.Add(item);
                                }
                                totalLength++;
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

        private SQLContextNew ComposeForGetDNListByConditionFromPakDotPakComn_DeliveryNo(string deliveryNo)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.PakDotpakcomn cond = new _Metas.PakDotpakcomn();
                    cond.internalID = deliveryNo + "%";
                    sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakcomn>(tk, null, new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            }, 
                            new ConditionCollection<_Metas.PakDotpakcomn>(new LikeCondition<_Metas.PakDotpakcomn>(cond)));
                }
            }
            sqlCtx.Param(_Metas.PakDotpakcomn.fn_internalID).Value = deliveryNo + "%";
            return sqlCtx;
        }

        private SQLContextNew ComposeForGetDNListByConditionFromPakDotPakComn_DeliveryNo2(string deliveryNo)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.PakDotpakcomn cond = new _Metas.PakDotpakcomn();
                    cond.internalID = deliveryNo;
                    sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakcomn>(tk, null, new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            }, 
                            new ConditionCollection<_Metas.PakDotpakcomn>(new EqualCondition<_Metas.PakDotpakcomn>(cond)));
                }
            }
            sqlCtx.Param(_Metas.PakDotpakcomn.fn_internalID).Value = deliveryNo;
            return sqlCtx;
        }

        private SQLContextNew ComposeForGetDNListByConditionFromPakDotPakComn_PONo(string pono)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.PakDotpakcomn cond = new _Metas.PakDotpakcomn();
                    cond.po_num = pono;
                    sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakcomn>(tk, null, new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            }, 
                            new ConditionCollection<_Metas.PakDotpakcomn>(new EqualCondition<_Metas.PakDotpakcomn>(cond)));
                }
            }
            sqlCtx.Param(_Metas.PakDotpakcomn.fn_po_num).Value = pono;
            return sqlCtx;
        }

        private SQLContextNew ComposeForGetDNListByConditionFromPakDotPakComn_Model(string model)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.PakDotpakcomn cond = new _Metas.PakDotpakcomn();
                    cond.model = model;
                    sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakcomn>(tk, null, new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            }, 
                            new ConditionCollection<_Metas.PakDotpakcomn>(new EqualCondition<_Metas.PakDotpakcomn>(cond)));
                }
            }
            sqlCtx.Param(_Metas.PakDotpakcomn.fn_model).Value = model;
            return sqlCtx;
        }

        private SQLContextNew ComposeForGetDNListByConditionFromPakDotPakComn_ShipDateFrom(DateTime shipDateFrom)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.PakDotpakcomn cond = new _Metas.PakDotpakcomn();
                    cond.actual_shipdate = To8BitDateTime(shipDateFrom);
                    sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakcomn>(tk, null, new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            }, 
                            new ConditionCollection<_Metas.PakDotpakcomn>(new GreaterOrEqualCondition<_Metas.PakDotpakcomn>(cond)));
                }
            }
            sqlCtx.Param(g.DecGE(_Metas.PakDotpakcomn.fn_actual_shipdate)).Value = To8BitDateTime(shipDateFrom);
            return sqlCtx;
        }

        private SQLContextNew ComposeForGetDNListByConditionFromPakDotPakComn_ShipDateTo(DateTime shipDateTo)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.PakDotpakcomn cond = new _Metas.PakDotpakcomn();
                    cond.actual_shipdate = To8BitDateTime(shipDateTo);
                    sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakcomn>(tk, null, new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            }, 
                            new ConditionCollection<_Metas.PakDotpakcomn>(new SmallerCondition<_Metas.PakDotpakcomn>(cond)));
                }
            }
            sqlCtx.Param(g.DecS(_Metas.PakDotpakcomn.fn_actual_shipdate)).Value = To8BitDateTime(shipDateTo);
            return sqlCtx;
        }

        private SQLContextNew ComposeForGetDNListByConditionFromPakDotPakComn_DNInfoValue(string dnInfoValue)
        {
            MethodBase mthObj = MethodBase.GetCurrentMethod();
            int tk = mthObj.MetadataToken;
            SQLContextNew sqlCtx = null;
            lock (mthObj)
            {
                if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                {
                    _Metas.PakDotpakcomn cond = FuncNew.GetAAllNonDefaultObject<_Metas.PakDotpakcomn>(new string[] { _Metas.PakDotpakcomn.fn_model, _Metas.PakDotpakcomn.fn_po_num, _Metas.PakDotpakcomn.fn_internalID }, SqlDbType.VarChar, SqlDbType.Char, SqlDbType.NVarChar, SqlDbType.NChar);
                    _Metas.PakDotpakcomn cond2 = FuncNew.GetAAllNonDefaultObject<_Metas.PakDotpakcomn>(new string[] { _Metas.PakDotpakcomn.fn_model, _Metas.PakDotpakcomn.fn_po_num, _Metas.PakDotpakcomn.fn_internalID }, SqlDbType.Decimal, SqlDbType.Decimal, SqlDbType.DateTime, SqlDbType.Int);
                    _Metas.PakDotpakcomn cond3 = new _Metas.PakDotpakcomn();
                    cond3.model = dnInfoValue;
                    cond3.po_num = dnInfoValue;
                    cond3.internalID = dnInfoValue;

                    var condSet = new ConditionCollection<_Metas.PakDotpakcomn>(false);
                    condSet.Add(new EqualCondition<_Metas.PakDotpakcomn>(cond));
                    condSet.Add(new EqualCondition<_Metas.PakDotpakcomn>(cond2, "CONVERT(NVARCHAR,{0})"));
                    condSet.Add(new AnyCondition<_Metas.PakDotpakcomn>(cond3));

                    sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.PakDotpakcomn>(tk, null, new string[][] {
                                new string[]{mtns::PakDotpakcomn.fn_internalID, mtns::PakDotpakcomn.fn_internalID},
                                new string[]{mtns::PakDotpakcomn.fn_consol_invoice, mtns::PakDotpakcomn.fn_consol_invoice},
                                new string[]{mtns::PakDotpakcomn.fn_po_num, mtns::PakDotpakcomn.fn_po_num },
                                new string[]{mtns::PakDotpakcomn.fn_model, mtns::PakDotpakcomn.fn_model },
                                new string[]{mtns::PakDotpakcomn.fn_actual_shipdate, mtns::PakDotpakcomn.fn_actual_shipdate },
                                new string[]{mtns::PakDotpakcomn.fn_box_unit_qty, string.Format("(SELECT ISNULL(SUM(CONVERT(INT,ISNULL({0},'0'))),0) FROM {1} WHERE {1}.{2}={3}.{4})", mtns.PakDotpakpaltno.fn_pallet_unit_qty, ToolsNew.GetTableName(typeof(mtns.PakDotpakpaltno)),mtns.PakDotpakpaltno.fn_internalID, ToolsNew.GetTableName(typeof(mtns.PakDotpakcomn)),mtns.PakDotpakcomn.fn_internalID)}
                            },
                            condSet);
                }
            }
            sqlCtx = FuncNew.SetColumnFromField<PakDotpakcomn>(sqlCtx, null, dnInfoValue, SqlDbType.NVarChar);
            sqlCtx = FuncNew.SetColumnFromField<PakDotpakcomn>(sqlCtx, null, dnInfoValue, g.DecAny("{0}"), SqlDbType.NVarChar);
            return sqlCtx;
        }

        public void UpdateSnoCtrlBoxIdInfo(SnoCtrlBoxIdInfo setValue, SnoCtrlBoxIdInfo condition)
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
                mtns::SnoCtrl_BoxId cond = mtns::FuncNew.SetColumnFromField<mtns::SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(condition);
                mtns::SnoCtrl_BoxId setv = mtns::FuncNew.SetColumnFromField<mtns::SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(setValue);

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::SnoCtrl_BoxId>(new mtns::SetValueCollection<mtns::SnoCtrl_BoxId>(new mtns::CommonSetValue<mtns::SnoCtrl_BoxId>(setv)), new mtns::ConditionCollection<mtns::SnoCtrl_BoxId>(new mtns::EqualCondition<mtns::SnoCtrl_BoxId>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(sqlCtx, setValue, true);
 
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetBoxIdListFromSnoCtrlBoxId(SnoCtrlBoxIdInfo condition)
        {
            try
            {
                IList<string> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::SnoCtrl_BoxId cond = mtns::FuncNew.SetColumnFromField<mtns::SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::SnoCtrl_BoxId>(null, new string[] { mtns.SnoCtrl_BoxId.fn_boxId }, new mtns::ConditionCollection<mtns::SnoCtrl_BoxId>(new mtns::EqualCondition<mtns::SnoCtrl_BoxId>(cond)), mtns::SnoCtrl_BoxId.fn_boxId);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.SnoCtrl_BoxId.fn_boxId));
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

        public void DeleteSnoCtrlBoxIdInfo(SnoCtrlBoxIdInfo condition)
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
                SnoCtrl_BoxId cond = FuncNew.SetColumnFromField<SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<SnoCtrl_BoxId>(new ConditionCollection<SnoCtrl_BoxId>(new EqualCondition<SnoCtrl_BoxId>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<SnoCtrl_BoxId, SnoCtrlBoxIdInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Delivery> GetDeliveryListByModel(string model, string status)
        {
            try
            {
                IList<Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.model = model;
                        cond.status = status;

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.shipDate = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null, new ConditionCollection<_Metas.Delivery>(
                            new EqualCondition<_Metas.Delivery>(cond),
                            new AnySoloCondition<_Metas.Delivery>(cond2, "{0}>=CONVERT(CHAR(10),GETDATE()-5,111)")), _Metas.Delivery.fn_shipDate);
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = model;
                sqlCtx.Param(_Metas.Delivery.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Delivery> GetDeliveryListByModelAndStatus(string model, string status)
        {
            try
            {
                IList<Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.model = model;

                        _Metas.Delivery cond1 = new _Metas.Delivery();
                        cond1.status = status;

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.shipDate = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null, new ConditionCollection<_Metas.Delivery>(
                            new EqualCondition<_Metas.Delivery>(cond),
                            new SmallerCondition<_Metas.Delivery>(cond1),
                            new AnySoloCondition<_Metas.Delivery>(cond2, "{0}>=CONVERT(CHAR(10),GETDATE()-5,111)")), _Metas.Delivery.fn_shipDate);
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = model;
                sqlCtx.Param(g.DecS(_Metas.Delivery.fn_status)).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDNListBy10BitPrefix(string input)
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
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.deliveryNo = input;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, new string[] { _Metas.Delivery.fn_deliveryNo }, new ConditionCollection<_Metas.Delivery>(
                            new EqualCondition<_Metas.Delivery>(cond, "SUBSTRING({0}, 1, 10)")
                            ), _Metas.Delivery.fn_deliveryNo);
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = input;
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Delivery.fn_deliveryNo));
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

        public int GetDnCountBySP(string value)
        {
            try
            {
                int ret = 0;

                SqlParameter[] paramsArray = new SqlParameter[1];

                paramsArray[0] = new SqlParameter("@Value", SqlDbType.VarChar);
                paramsArray[0].Value = value;
                object data = _Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "IMES_GetDnCount", paramsArray);
                if (data != null)
                    ret = Convert.ToInt32(data);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Delivery> GetDeliveryListByModelPrefix(string factoryPo, string modelPrefix, int modelLength, string status)
        {
            try
            {
                IList<Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond0 = new _Metas.Delivery();
                        cond0.model = modelPrefix;

                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.status = status;
                        cond.poNo = factoryPo;

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.shipDate = DateTime.Now;

                        mtns.Delivery cond3 = new mtns.Delivery();
                        cond3.model = "[LEN]";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null, new ConditionCollection<_Metas.Delivery>(
                            new EqualCondition<_Metas.Delivery>(cond0, "LEFT({0},2)"),
                            new EqualCondition<_Metas.Delivery>(cond),
                            new AnySoloCondition<_Metas.Delivery>(cond2, "CONVERT(CHAR(10),{0}, 111)>=CONVERT(CHAR(10),GETDATE()-3,111)"),
                            new EqualCondition<mtns.Delivery>(cond3, "LEN({0})", "CONVERT(INT,{0})")), _Metas.Delivery.fn_shipDate, _Metas.Delivery.fn_qty, _Metas.Delivery.fn_deliveryNo);
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = modelPrefix;
                sqlCtx.Param(_Metas.Delivery.fn_status).Value = status;
                sqlCtx.Param(_Metas.Delivery.fn_poNo).Value = factoryPo;
                sqlCtx.Param(_Metas.Delivery.fn_model + "$1").Value = modelLength.ToString();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Delivery> GetDeliveryListByModel(string model, string modelPrefix, int modelLength, string status)
        {
            try
            {
                IList<Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond0 = new _Metas.Delivery();
                        cond0.model = modelPrefix;

                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.status = status;
                        cond.model = model;

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.shipDate = DateTime.Now;

                        mtns.Delivery cond3 = new mtns.Delivery();
                        cond3.model = "[LEN]";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null, new ConditionCollection<_Metas.Delivery>(
                            new EqualCondition<_Metas.Delivery>(cond0, "LEFT({0},2)"),
                            new EqualCondition<_Metas.Delivery>(cond),
                            new AnySoloCondition<_Metas.Delivery>(cond2, "CONVERT(CHAR(10),{0}, 111)>=CONVERT(CHAR(10),GETDATE()-3,111)"),
                            new EqualCondition<mtns.Delivery>(cond3, "LEN({0})", "CONVERT(INT,{0})")), _Metas.Delivery.fn_shipDate, _Metas.Delivery.fn_deliveryNo);
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = modelPrefix;
                sqlCtx.Param(_Metas.Delivery.fn_status).Value = status;
                sqlCtx.Param(_Metas.Delivery.fn_model + "$1").Value = model;
                sqlCtx.Param(_Metas.Delivery.fn_model + "$2").Value = modelLength.ToString();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDeliveryNoPrefixByValueAndType(string infoValue, string infoType)
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
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.infoValue = infoValue;
                        cond.infoType = infoType;

                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.DeliveryInfo>(tk, "DISTINCT", new string[][] { new string[] { _Metas.DeliveryInfo.fn_deliveryNo, string.Format("LEFT({0}, 10)", _Metas.DeliveryInfo.fn_deliveryNo) } }, new ConditionCollection<_Metas.DeliveryInfo>(
                            new EqualCondition<_Metas.DeliveryInfo>(cond)
                            ));
                    }
                }
                sqlCtx.Param(_Metas.DeliveryInfo.fn_infoValue).Value = infoValue;
                sqlCtx.Param(_Metas.DeliveryInfo.fn_infoType).Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.DeliveryInfo.fn_deliveryNo));
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

        public IList<IMES.FisObject.PAK.DN.DeliveryInfo> GetDeliveryInfoList(IMES.FisObject.PAK.DN.DeliveryInfo condition)
        {
            try
            {
                IList<fons.DeliveryInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.DeliveryInfo cond = FuncNew.SetColumnFromField<mtns.DeliveryInfo, fons.DeliveryInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<mtns.DeliveryInfo>(null, null, new ConditionCollection<mtns.DeliveryInfo>(new EqualCondition<mtns.DeliveryInfo>(cond)), mtns.DeliveryInfo.fn_deliveryNo);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.DeliveryInfo, fons.DeliveryInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.DeliveryInfo, fons.DeliveryInfo, fons.DeliveryInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePoDataEdi(IList<string> prefixList)
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
                        mtns::PoData_EDI cond = new mtns::PoData_EDI();
                        cond.deliveryNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PoData_EDI>(tk, new ConditionCollection<mtns::PoData_EDI>(new InSetCondition<mtns::PoData_EDI>(cond, "LEFT({0}, 10)")));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PoData_EDI.fn_deliveryNo), g.ConvertInSet(prefixList));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePoPltEdi(IList<string> prefixList)
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
                        mtns::PoPlt_EDI cond = new mtns::PoPlt_EDI();
                        cond.deliveryNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PoPlt_EDI>(tk, new ConditionCollection<mtns::PoPlt_EDI>(new InSetCondition<mtns::PoPlt_EDI>(cond, "LEFT({0}, 10)")));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PoPlt_EDI.fn_deliveryNo), g.ConvertInSet(prefixList));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Delivery> GetDeliveryListByShipDateAndModelAndStatus(string model)
        {
            try
            {
                IList<Delivery> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.model = model;

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.shipDate = DateTime.Now;

                        _Metas.Delivery cond3 = new _Metas.Delivery();
                        cond3.status = "82";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null, new ConditionCollection<_Metas.Delivery>(
                            new EqualCondition<_Metas.Delivery>(cond),
                            new AnySoloCondition<_Metas.Delivery>(cond2, "{0}>=CONVERT(CHAR(10),GETDATE()-5,111)"),
                            new SmallerCondition<_Metas.Delivery>(cond3)), _Metas.Delivery.fn_shipDate);

                        sqlCtx.Param(g.DecS(_Metas.Delivery.fn_status)).Value = cond3.status;
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDeliveryAttrLogInfo(DeliveryAttrLogInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::DeliveryAttrLog>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::DeliveryAttrLog, DeliveryAttrLogInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::DeliveryAttrLog.fn_cdt).Value = cmDt;
                //sqlCtx.Param(mtns::DeliveryAttrLog.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDeliveryInfoValueByInfoTypeAndInfoValuePrefix(string newConsolidated, string infoType, string consolidate, string editor)
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
                        mtns::DeliveryInfo cond = new mtns::DeliveryInfo();
                        cond.infoType = infoType;
                        mtns::DeliveryInfo cond2 = new mtns::DeliveryInfo();
                        cond2.infoValue = consolidate;

                        _Metas.DeliveryInfo setv = new _Metas.DeliveryInfo();
                        setv.infoValue = newConsolidated;
                        setv.editor = editor;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<mtns::DeliveryInfo>(tk, new SetValueCollection<_Metas.DeliveryInfo>(new CommonSetValue<_Metas.DeliveryInfo>(setv)), new ConditionCollection<_Metas.DeliveryInfo>(
                            new EqualCondition<mtns::DeliveryInfo>(cond),
                            new EqualCondition<mtns::DeliveryInfo>(cond2, "LEFT({0},10)")
                            ));
                    }
                }
                sqlCtx.Param(mtns::DeliveryInfo.fn_infoType).Value = infoType;
                sqlCtx.Param(mtns::DeliveryInfo.fn_infoValue).Value = consolidate;
                sqlCtx.Param(g.DecSV(mtns::DeliveryInfo.fn_infoValue)).Value = newConsolidated;
                sqlCtx.Param(g.DecSV(mtns::DeliveryInfo.fn_editor)).Value = editor;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::DeliveryInfo.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDeliveryAttrLog(string newConsolidated, string editor, string consolidate)
        {
            try
            {
                string newCslParam = "NewConsolidated";
                string editorParam = "Editor";
                string cslParam = "Consolidate";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                        sqlCtx.Sentence =   "INSERT INTO {0}({3},{4},{5},{6},{7},{8},{9},{10}) " +
                                            "SELECT a.{11}, a.{12}, 'Consolidated', b.{13}, @{17}, 'CQ', @{18}, GETDATE() " +
                                            "FROM {1} a (NOLOCK), {2} b (NOLOCK) " +
                                            "WHERE a.{11} = b.{14} " +
                                            "AND b.{15} = 'Consolidated' " +
                                            "AND LEFT({13},10) = @{16}";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(mtns.DeliveryAttrLog)),
                                                                         ToolsNew.GetTableName(typeof(mtns.Delivery)),
                                                                         ToolsNew.GetTableName(typeof(mtns.DeliveryInfo)),
                                                                         mtns.DeliveryAttrLog.fn_deliveryNo,
                                                                         mtns.DeliveryAttrLog.fn_shipmentNo,
                                                                         mtns.DeliveryAttrLog.fn_attrName,
                                                                         mtns.DeliveryAttrLog.fn_attrOldValue,
                                                                         mtns.DeliveryAttrLog.fn_attrNewValue,
                                                                         mtns.DeliveryAttrLog.fn_descr,
                                                                         mtns.DeliveryAttrLog.fn_editor,
                                                                         mtns.DeliveryAttrLog.fn_cdt,
                                                                         mtns.Delivery.fn_deliveryNo,
                                                                         mtns.Delivery.fn_shipmentNo,
                                                                         mtns.DeliveryInfo.fn_infoValue,
                                                                         mtns.DeliveryInfo.fn_deliveryNo,
                                                                         mtns.DeliveryInfo.fn_infoType,
                                                                         cslParam,
                                                                         newCslParam,
                                                                         editorParam
                                                                         );

                        sqlCtx.AddParam(newCslParam, new SqlParameter("@" + newCslParam, ToolsNew.GetDBFieldType<mtns.DeliveryAttrLog>(mtns.DeliveryAttrLog.fn_attrNewValue)));
                        sqlCtx.AddParam(editorParam, new SqlParameter("@" + editorParam, ToolsNew.GetDBFieldType<mtns.DeliveryAttrLog>(mtns.DeliveryAttrLog.fn_editor)));
                        sqlCtx.AddParam(cslParam, new SqlParameter("@" + cslParam, ToolsNew.GetDBFieldType<mtns.DeliveryInfo>(mtns.DeliveryInfo.fn_infoValue)));
                    }
                }
                sqlCtx.Param(newCslParam).Value = newConsolidated;
                sqlCtx.Param(editorParam).Value = editor;
                sqlCtx.Param(cslParam).Value = consolidate;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfDeliveryNoPrefixForDoubleDeliveryInfoPairs(string infoType1, string infoValue1, string infoType2, string infoValue2)
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
                        mtns.DeliveryInfo cond = new mtns.DeliveryInfo();
                        cond.infoType = infoType1;

                        mtns.DeliveryInfo cond2 = new mtns.DeliveryInfo();
                        cond2.infoType = infoType2;


                        var condSet = new ConditionCollection<mtns.DeliveryInfo>(false);
                        condSet.AddRange(
                            new AnyCondition<mtns.DeliveryInfo>(cond, string.Format("({0}={1} AND {2}=RTRIM(@{2}))", "{0}", "{1}", mtns.DeliveryInfo.fn_infoValue)),
                            new AnyCondition<mtns.DeliveryInfo>(cond2, string.Format("({0}={1}$1 AND {2}=RTRIM(@{2}$1))", "{0}", "{1}", mtns.DeliveryInfo.fn_infoValue)));

                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns.DeliveryInfo>(tk, "COUNT", new string[][] { new string[] { mtns.DeliveryInfo.fn_deliveryNo, string.Format("DISTINCT LEFT({0}, 10)", mtns.DeliveryInfo.fn_deliveryNo) } }, condSet);

                        sqlCtx.AddParam(mtns.DeliveryInfo.fn_infoValue, new SqlParameter("@" + mtns.DeliveryInfo.fn_infoValue, ToolsNew.GetDBFieldType<mtns.DeliveryInfo>(mtns.DeliveryInfo.fn_infoValue)));
                        sqlCtx.AddParam(mtns.DeliveryInfo.fn_infoValue + "$1", new SqlParameter("@" + mtns.DeliveryInfo.fn_infoValue + "$1", ToolsNew.GetDBFieldType<mtns.DeliveryInfo>(mtns.DeliveryInfo.fn_infoValue)));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.DeliveryInfo.fn_infoType)).Value = infoType1;
                sqlCtx.Param(g.DecAny(mtns.DeliveryInfo.fn_infoType) + "$1").Value = infoType2;
                sqlCtx.Param(mtns.DeliveryInfo.fn_infoValue).Value = infoValue1;
                sqlCtx.Param(mtns.DeliveryInfo.fn_infoValue + "$1").Value = infoValue2;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void UpdateDeliveryForStatusChange(string editor, string cartonNo)
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
                        mtns.Delivery setv = new mtns.Delivery();
                        setv.status = "00";
                        setv.editor = editor;
                        setv.udt = DateTime.Now;

                        mtns.Delivery cond = new mtns.Delivery();
                        cond.deliveryNo = cartonNo;

                        sqlCtx = FuncNew.GetConditionedUpdate(tk, new SetValueCollection<mtns.Delivery>(
                                new CommonSetValue<mtns.Delivery>(setv)), new ConditionCollection<mtns.Delivery>(
                                new AnyCondition<mtns.Delivery>(cond, string.Format("{0} IN (SELECT DISTINCT {1} FROM {2}..{3} WHERE {4}={5})", "{0}", mtns.Product.fn_deliveryNo, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(mtns.Product)), mtns.Product.fn_cartonSN, "{1}"))));

                        sqlCtx.Param(g.DecSV(mtns.Delivery.fn_status)).Value = setv.status;
                    }
                }
                sqlCtx.Param(g.DecAny(mtns.Delivery.fn_deliveryNo)).Value = cartonNo;

                sqlCtx.Param(g.DecSV(mtns.Delivery.fn_editor)).Value = editor;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Delivery.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// update Delivery set [Status]=@status where DeliveryNo in (输入列表)
        /// </summary>
        /// <param name="dns"></param>
        /// <param name="status"></param>
        public void UpdateMultiDeliveryForStatusChange(string[] dns, string status)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update Delivery
                                                            set Status=@status,
                                                                  Udt = getdate()
                                                            where DeliveryNo in ({0}) ";


                        sqlCtx.AddParam("Status", new SqlParameter("@status", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                string dnStr = "'" + string.Join("','", dns) + "'";
                sqlCtx.Param("Status").Value = status;



                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 string.Format(sqlCtx.Sentence, dnStr),
                                                                                 sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateMultiDeliveryForStatusChangeDefered(IUnitOfWork uow, string[] dns, string status)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dns, status);
        }
       
        public void InsertDeliveryLog(fons::DeliveryLog item)
        {
            PersistInsertDeliveryLog(item.DeliveryNo, item);
        }

        public IList<ShipBoxDetInfo> GetShipBoxDetInfoListByCondition(ShipBoxDetInfo condition)
        {
            try
            {
                IList<ShipBoxDetInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.ShipBoxDet cond = FuncNew.SetColumnFromField<mtns.ShipBoxDet, ShipBoxDetInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<mtns.ShipBoxDet>(null, null, new ConditionCollection<mtns.ShipBoxDet>(new EqualCondition<mtns.ShipBoxDet>(cond)), mtns.ShipBoxDet.fn_cdt + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.ShipBoxDet, ShipBoxDetInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.ShipBoxDet, ShipBoxDetInfo, ShipBoxDetInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDeliveryPalletInfo(DeliveryPalletInfo setValue, DeliveryPalletInfo condition)
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
                Delivery_Pallet cond = FuncNew.SetColumnFromField<Delivery_Pallet, DeliveryPalletInfo>(condition);
                Delivery_Pallet setv = FuncNew.SetColumnFromField<Delivery_Pallet, DeliveryPalletInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Delivery_Pallet>(new SetValueCollection<Delivery_Pallet>(new CommonSetValue<Delivery_Pallet>(setv)), new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Delivery_Pallet, DeliveryPalletInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Delivery_Pallet, DeliveryPalletInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Delivery_Pallet.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Merge from RCTO        
        public int GetCountOfPonoFromDelivery(Delivery condition)
        {
            try
            {
                int ret = 0;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.Delivery cond = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(condition);
                sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns.Delivery>("COUNT", new string[][] { new string[]{ mtns.Delivery.fn_poNo, string.Format( "DISTINCT {0}", mtns.Delivery.fn_poNo) } }, new ConditionCollection<mtns.Delivery>(new EqualCondition<mtns.Delivery>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public int GetCountOfModelFromDelivery(Delivery condition)
        {
            try
            {
                int ret = 0;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.Delivery cond = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(condition);
                sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns.Delivery>("COUNT", new string[][] { new string[] { mtns.Delivery.fn_model, string.Format("DISTINCT {0}", mtns.Delivery.fn_model) } }, new ConditionCollection<mtns.Delivery>(new EqualCondition<mtns.Delivery>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<fons.DeliveryInfo> GetDeliveryInfoFromDeliveryByPalletNo(string palletNo, fons.DeliveryInfo condition)
        {
            try
            {
                IList<fons.DeliveryInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.DeliveryInfo cond = FuncNew.SetColumnFromField<mtns.DeliveryInfo, fons.DeliveryInfo>(condition);

                mtns.DeliveryInfo cond2 = new mtns.DeliveryInfo();
                cond2.deliveryNo = palletNo;

                sqlCtx = FuncNew.GetConditionedSelect<mtns.DeliveryInfo>(null, null, new ConditionCollection<mtns.DeliveryInfo>(
                    new EqualCondition<mtns.DeliveryInfo>(cond),
                    new AnyCondition<mtns.DeliveryInfo>(cond2, string.Format("{0} IN (SELECT {1} FROM {2}..{3} WHERE {4}={5})", "{0}", mtns.Product.fn_deliveryNo, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(mtns.Product)), mtns.Product.fn_palletNo, "{1}"))));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.DeliveryInfo, fons.DeliveryInfo>(sqlCtx, condition);
                sqlCtx.Param(g.DecAny(mtns.DeliveryInfo.fn_deliveryNo)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns.DeliveryInfo, fons.DeliveryInfo, fons.DeliveryInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPonoFromDeliveryByPalletNo(string palletNo, Delivery condition)
        {
            try
            {
                int ret = 0;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.Delivery cond = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(condition);

                mtns.Delivery cond2 = new mtns.Delivery();
                cond2.deliveryNo = palletNo;

                sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns.Delivery>("COUNT", new string[][] { new string[] { mtns.Delivery.fn_poNo, string.Format("DISTINCT {0}", mtns.Delivery.fn_poNo) } }, new ConditionCollection<mtns.Delivery>(
                    new EqualCondition<mtns.Delivery>(cond),
                    new AnyCondition<mtns.Delivery>(cond2, string.Format("{0} IN (SELECT {1} FROM {2}..{3} WHERE {4}={5})", "{0}", mtns.Product.fn_deliveryNo, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(mtns.Product)), mtns.Product.fn_palletNo, "{1}"))));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(sqlCtx, condition);
                sqlCtx.Param(g.DecAny(mtns.Delivery.fn_deliveryNo)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public int GetCountOfModelFromDeliveryByPalletNo(string palletNo, Delivery condition)
        {
            try
            {
                int ret = 0;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns.Delivery cond = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(condition);

                mtns.Delivery cond2 = new mtns.Delivery();
                cond2.deliveryNo = palletNo;

                sqlCtx = FuncNew.GetConditionedSelectForFuncedField<mtns.Delivery>("COUNT", new string[][] { new string[] { mtns.Delivery.fn_model, string.Format("DISTINCT {0}", mtns.Delivery.fn_model) } }, new ConditionCollection<mtns.Delivery>(
                    new EqualCondition<mtns.Delivery>(cond),
                    new AnyCondition<mtns.Delivery>(cond2, string.Format("{0} IN (SELECT {1} FROM {2}..{3} WHERE {4}={5})", "{0}", mtns.Product.fn_deliveryNo, _Schema.SqlHelper.DB_FA, ToolsNew.GetTableName(typeof(mtns.Product)), mtns.Product.fn_palletNo, "{1}"))));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.Delivery, Delivery>(sqlCtx, condition);
                sqlCtx.Param(g.DecAny(mtns.Delivery.fn_deliveryNo)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        
        #region Defered

        public void UpdateDNByConditionDefered(IUnitOfWork uow, DNUpdateCondition myCondition, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), myCondition, editor);
        }

        public void UpdateDeliverInfoByIDDefered(IUnitOfWork uow, int deliverInfoID, string infoValue, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), deliverInfoID, infoValue, editor);
        }

        public void UpdateDeliverQtyDefered(IUnitOfWork uow, int deliveryPalletID, short deliveryQty, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), deliveryPalletID, deliveryQty, editor);
        }

        public void UpdateDNStatusWhenDNPalletFullDefered(IUnitOfWork uow, string deliveryNo, string status, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), deliveryNo, status, editor);
        }

        public void UpdateAllDNStatusWhenPalletFullDefered(IUnitOfWork uow, string deliveryNo, string palletno, string status, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), deliveryNo, palletno, status, editor);
        }

        public void UpdateSnoidForShipBoxDetDefered(IUnitOfWork uow, string snoId, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), snoId, dn);
        }

        public void RemovePAK_PackkingData_EDIDataByProdIdsDefered(IUnitOfWork uow, string[] prodIds)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), prodIds);
        }

        public void RemovePAKOdmSession_EDIDataByProdIdsDefered(IUnitOfWork uow, string[] prodIds)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), prodIds);
        }

        public void InsertDeliveryInfoDefered(IUnitOfWork uow, IMES.FisObject.PAK.DN.DeliveryInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateDNUdtDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeleteSnoCtrlBoxIdInfoDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void InsertShipBoxDetDefered(IUnitOfWork uow, ShipBoxDetInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteSnoCtrlBoxIdSQInfoDefered(IUnitOfWork uow, SnoCtrlBoxIdSQInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertDeliveryPalletDefered(IUnitOfWork uow, DeliveryPalletInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteDeliveryPalletByDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void UpdateDeliveryInfoForDecreaseConsolidatedDefered(IUnitOfWork uow, string deliveryNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), deliveryNo);
        }

        public void BackupToDeliveryDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void BackupToDeliveryPalletDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeleteShipBoxDetByDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeleteDeliveryPalletByShipmentNoDefered(IUnitOfWork uow, string shipmentNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipmentNo);
        }

        public void DeleteDeliveryPalletByPalletNoDefered(IUnitOfWork uow, string palletNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletNo);
        }

        public void BackupToDeliveryByShipmentNoDefered(IUnitOfWork uow, string shipmentNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipmentNo);
        }

        public void BackupToDeliveryPalletByShipmentNoDefered(IUnitOfWork uow, string shipmentNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipmentNo);
        }

        public void DeleteDeliveryByShipmentNoDefered(IUnitOfWork uow, string shipmentNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipmentNo);
        }

        public void DeleteDeliveryInfoByShipmentNoDefered(IUnitOfWork uow, string shipmentNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipmentNo);
        }

        public void AddPoDataEdiInfoDefered(IUnitOfWork uow, PoDataEdiInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPoPltEdiInfoDefered(IUnitOfWork uow, PoPltEdiInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateUdtForPoDataEdiDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeletePoDataEdiDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeletePoPltEdiDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeleteShipBoxDetDefered(IUnitOfWork uow, string shipment)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipment);
        }

        public void UpdateShipBoxDetForClearSnoidDefered(IUnitOfWork uow, string snoId, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), snoId, dn);
        }

        public void UpdateShipBoxDetForSetSnoIdDefered(IUnitOfWork uow, string snoid, string dn, string plt, string boxId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), snoid, dn, plt, boxId);
        }

        public void InsertDummyShipDetInfoDefered(IUnitOfWork uow, DummyShipDetInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateDummyShipDetInfoDefered(IUnitOfWork uow, DummyShipDetInfo setValue, DummyShipDetInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeleteShipBoxDetByShipmentNoDefered(IUnitOfWork uow, string shipment)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipment);
        }

        public void UpdateShipBoxDetInfoDefered(IUnitOfWork uow, ShipBoxDetInfo setValue, ShipBoxDetInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateDeliveryForStatusChangeDefered(IUnitOfWork uow, string[] dns, string newTitleChar)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dns, newTitleChar);
        }

        public void PersistUpdatedItemDefered(IUnitOfWork uow, InvokeBody preCond, Delivery item)
        {
            var hdl = AddOneInvokeBody(uow, 2, MethodBase.GetCurrentMethod(), item);
            hdl.DependencyIvkbdy = preCond;
        }

        public void UpdateSnoCtrlBoxIdInfoDefered(IUnitOfWork uow, SnoCtrlBoxIdInfo setValue, SnoCtrlBoxIdInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeleteSnoCtrlBoxIdInfoDefered(IUnitOfWork uow, SnoCtrlBoxIdInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void DeletePoDataEdiDefered(IUnitOfWork uow, IList<string> prefixList)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), prefixList);
        }

        public void DeletePoPltEdiDefered(IUnitOfWork uow, IList<string> prefixList)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), prefixList);
        }

        public void DeleteDeliveryByDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeleteDeliveryInfoByDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeleteDeliveryAttrsByDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeleteDeliveryAttrLogByDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeletePalletAttrLogDefered(IUnitOfWork uow, IList<string> pltNos)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pltNos);
        }

        public void DeleteDeliveryAttrsByShipmentNoDefered(IUnitOfWork uow, string shipment)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipment);
        }

        public void DeleteDeliveryAttrLogByShipmentNoDefered(IUnitOfWork uow, string shipment)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipment);
        }

        public void InsertDeliveryAttrLogInfoDefered(IUnitOfWork uow, DeliveryAttrLogInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateDeliveryInfoValueByInfoTypeAndInfoValuePrefixDefered(IUnitOfWork uow, string newConsolidated, string infoType, string consolidate, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), newConsolidated, infoType, consolidate, editor);
        }

        public void InsertDeliveryAttrLogDefered(IUnitOfWork uow, string newConsolidated, string editor, string consolidate)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), newConsolidated, editor, consolidate);
        }

        public void UpdateDeliveryForStatusChangeDefered(IUnitOfWork uow, string editor, string cartonNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), editor, cartonNo);
        }

        public void InsertDeliveryLogDefered(IUnitOfWork uow, fons::DeliveryLog item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateDeliveryPalletInfoDefered(IUnitOfWork uow, DeliveryPalletInfo setValue, DeliveryPalletInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        #endregion

        #endregion

        #region . Inners .

        private void PersistInsertDelivery(fons::Delivery item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Delivery.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = item.DeliveryNo;
                sqlCtx.Params[_Schema.Delivery.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Delivery.fn_Model].Value = item.ModelName;
                sqlCtx.Params[_Schema.Delivery.fn_PoNo].Value = item.PoNo;
                sqlCtx.Params[_Schema.Delivery.fn_Qty].Value = item.Qty;
                sqlCtx.Params[_Schema.Delivery.fn_ShipDate].Value = item.ShipDate;
                sqlCtx.Params[_Schema.Delivery.fn_ShipmentNo].Value = item.ShipmentNo;
                sqlCtx.Params[_Schema.Delivery.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.Delivery.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateDelivery(fons::Delivery item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = item.DeliveryNo;
                sqlCtx.Params[_Schema.Delivery.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Delivery.fn_Model].Value = item.ModelName;
                sqlCtx.Params[_Schema.Delivery.fn_PoNo].Value = item.PoNo;
                sqlCtx.Params[_Schema.Delivery.fn_Qty].Value = item.Qty;
                sqlCtx.Params[_Schema.Delivery.fn_ShipDate].Value = item.ShipDate;
                sqlCtx.Params[_Schema.Delivery.fn_ShipmentNo].Value = item.ShipmentNo;
                sqlCtx.Params[_Schema.Delivery.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.Delivery.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteDelivery(fons::Delivery item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery));
                    }
                }
                sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = item.DeliveryNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckAndInsertSubs(fons::Delivery item, StateTracker tracker)
        {
            IList<fons.DeliveryInfo> lstInfo = (IList<fons.DeliveryInfo>)item.GetType().GetField("_deliveryInfoes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstInfo != null && lstInfo.Count > 0)
            {
                foreach (fons.DeliveryInfo info in lstInfo)
                {
                    if (tracker.GetState(info) == DataRowState.Added)
                    {
                        this.PersistInsertDeliveryInfo(info);
                    }
                }
            }

            fons.DeliveryEx deliveryExInfo = (fons.DeliveryEx)item.GetType().GetField("_deliveryExInfo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (deliveryExInfo != null )
            {
                switch (deliveryExInfo.Tracker.GetState(deliveryExInfo))
                {
                    case DataRowState.Added:
                             PersistInsertDeliveryEx(item.DeliveryNo, deliveryExInfo);
                             break;
                    case DataRowState.Modified:
                             PersistUpdateDeliveryEx(item.DeliveryNo, deliveryExInfo);
                             break;
                    default:
                             break;
                 }
            }

        }

        private void CheckAndInsertOrUpdateOrRemoveSubs(fons::Delivery item, StateTracker tracker)
        {
            IList<DeliveryPallet> lstDp = (IList<DeliveryPallet>)item.GetType().GetField("_dnplts", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstDp != null && lstDp.Count > 0)
            {
                IList<DeliveryPallet> iLstToDel = new List<DeliveryPallet>();
                foreach (DeliveryPallet dp in lstDp)
                {
                    if (tracker.GetState(dp) == DataRowState.Added)
                    {

                    }
                    if (tracker.GetState(dp) == DataRowState.Modified)
                    {
                        this.PersistUpdateDeliveryPallet(dp);
                    }
                    else if (tracker.GetState(dp) == DataRowState.Deleted)
                    {

                    }
                }
                foreach (DeliveryPallet toDel in iLstToDel)
                {

                }
            }

            IList<IMES.FisObject.PAK.DN.DeliveryLog> lstDnLg = (IList<IMES.FisObject.PAK.DN.DeliveryLog>)item.GetType().GetField("_logs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstDnLg != null && lstDnLg.Count > 0)
            {
                foreach (IMES.FisObject.PAK.DN.DeliveryLog dnLg in lstDnLg)
                {
                    if (tracker.GetState(dnLg) == DataRowState.Added)
                    {
                        this.PersistInsertDeliveryLog(item.DeliveryNo, dnLg);
                    }
                }
            }

            IList<fons.DeliveryInfo> lstInfo = (IList<fons.DeliveryInfo>)item.GetType().GetField("_deliveryInfoes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstInfo != null && lstInfo.Count > 0)
            {
                foreach (fons.DeliveryInfo info in lstInfo)
                {
                    if (tracker.GetState(info) == DataRowState.Added)
                    {
                        this.PersistInsertDeliveryInfo(info);
                    }
                    else if (tracker.GetState(info) == DataRowState.Modified)
                    {
                        this.PersistUpdateDeliveryInfo(info);
                    }
                }
            }

            fons.DeliveryEx deliveryExInfo = (fons.DeliveryEx)item.GetType().GetField("_deliveryExInfo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (deliveryExInfo != null)
            {
                switch (deliveryExInfo.Tracker.GetState(deliveryExInfo))
                {
                    case DataRowState.Added:
                        PersistInsertDeliveryEx(item.DeliveryNo, deliveryExInfo);
                        break;
                    case DataRowState.Modified:
                        PersistUpdateDeliveryEx(item.DeliveryNo, deliveryExInfo);
                        break;
                    default:
                        break;
                }
            }
        }

        private void PersistInsertDeliveryEx(string deliveryNo, fons.DeliveryEx item)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"insert DeliveryEx(DeliveryNo, ShipmentNo, ShipType, PalletType, ConsolidateQty, 
				                                                                           CartonQty, QtyPerCarton, MessageCode, ShipToParty, Priority, 
				                                                                           GroupId, OrderType, BOL, HAWB, Carrier, 
				                                                                           ShipWay, PackID, Editor, Udt,
                                                                                            StdPltFullQty, StdPltStackType)
                                                                        values (@DeliveryNo, @ShipmentNo, @ShipType, @PalletType, @ConsolidateQty, 
	                                                                           @CartonQty, @QtyPerCarton, @MessageCode, @ShipToParty, @Priority, 
	                                                                           @GroupId, @OrderType, @BOL, @HAWB, @Carrier, 
	                                                                           @ShipWay, @PackID, @Editor, GETDATE(),
                                                                                @StdPltFullQty, @StdPltStackType)";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipmentNo", new SqlParameter("@ShipmentNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipType", new SqlParameter("@ShipType", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletType", new SqlParameter("@PalletType", SqlDbType.VarChar));
                        sqlCtx.AddParam("ConsolidateQty", new SqlParameter("@ConsolidateQty", SqlDbType.Int));

                        sqlCtx.AddParam("CartonQty", new SqlParameter("@CartonQty", SqlDbType.Int));
                        sqlCtx.AddParam("QtyPerCarton", new SqlParameter("@QtyPerCarton", SqlDbType.Int));
                        sqlCtx.AddParam("MessageCode", new SqlParameter("@MessageCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipToParty", new SqlParameter("@ShipToParty", SqlDbType.VarChar));
                        sqlCtx.AddParam("Priority", new SqlParameter("@Priority", SqlDbType.VarChar));

                        sqlCtx.AddParam("GroupId", new SqlParameter("@GroupId", SqlDbType.VarChar));
                        sqlCtx.AddParam("OrderType", new SqlParameter("@OrderType", SqlDbType.VarChar));
                        sqlCtx.AddParam("BOL", new SqlParameter("@BOL", SqlDbType.VarChar));
                        sqlCtx.AddParam("HAWB", new SqlParameter("@HAWB", SqlDbType.VarChar));
                        sqlCtx.AddParam("Carrier", new SqlParameter("@Carrier", SqlDbType.VarChar));

                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("PackID", new SqlParameter("@PackID", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltStackType", new SqlParameter("@StdPltStackType", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DeliveryNo").Value = deliveryNo;
                sqlCtx.Param("ShipmentNo").Value = item.ShipmentNo;
                sqlCtx.Param("ShipType").Value = item.ShipType;
                sqlCtx.Param("PalletType").Value = item.PalletType;
                sqlCtx.Param("ConsolidateQty").Value = item.ConsolidateQty;

                sqlCtx.Param("CartonQty").Value = item.CartonQty;
                sqlCtx.Param("QtyPerCarton").Value = item.QtyPerCarton;
                sqlCtx.Param("MessageCode").Value = item.MessageCode;
                sqlCtx.Param("ShipToParty").Value = item.ShipToParty;
                sqlCtx.Param("Priority").Value = item.Priority;

                sqlCtx.Param("GroupId").Value = item.GroupId;
                sqlCtx.Param("OrderType").Value = item.OrderType;
                sqlCtx.Param("BOL").Value = item.BOL;
                sqlCtx.Param("HAWB").Value = item.HAWB;
                sqlCtx.Param("Carrier").Value = item.Carrier;

                sqlCtx.Param("ShipWay").Value = item.ShipWay;
                sqlCtx.Param("PackID").Value = item.PackID;
                sqlCtx.Param("Editor").Value = item.Editor;
                sqlCtx.Param("StdPltFullQty").Value = item.StdPltFullQty;
                sqlCtx.Param("StdPltStackType").Value = item.StdPltStackType;

                item.DeliveryNo = deliveryNo;

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

        private void PersistUpdateDeliveryEx(string deliveryNo, fons.DeliveryEx item)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update DeliveryEx
                                                                set  ShipmentNo=@ShipmentNo, ShipType=@ShipType, PalletType=@PalletType, ConsolidateQty=@ConsolidateQty, 
				                                                                           CartonQty=@CartonQty, QtyPerCarton=@QtyPerCarton, MessageCode=@MessageCode, ShipToParty=@ShipToParty, Priority=@Priority, 
				                                                                           GroupId=@GroupId, OrderType=@OrderType, BOL=@BOL, HAWB=@HAWB, Carrier=@Carrier, 
				                                                                           ShipWay=@ShipWay, PackID=@PackID, Editor=@Editor, Udt=getdate(),
                                                                                           StdPltFullQty=@StdPltFullQty, StdPltStackType=@StdPltStackType                                                                       
                                                             where DeliveryNo=@DeliveryNo";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipmentNo", new SqlParameter("@ShipmentNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipType", new SqlParameter("@ShipType", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletType", new SqlParameter("@PalletType", SqlDbType.VarChar));
                        sqlCtx.AddParam("ConsolidateQty", new SqlParameter("@ConsolidateQty", SqlDbType.Int));

                        sqlCtx.AddParam("CartonQty", new SqlParameter("@CartonQty", SqlDbType.Int));
                        sqlCtx.AddParam("QtyPerCarton", new SqlParameter("@QtyPerCarton", SqlDbType.Int));
                        sqlCtx.AddParam("MessageCode", new SqlParameter("@MessageCode", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipToParty", new SqlParameter("@ShipToParty", SqlDbType.VarChar));
                        sqlCtx.AddParam("Priority", new SqlParameter("@Priority", SqlDbType.VarChar));

                        sqlCtx.AddParam("GroupId", new SqlParameter("@GroupId", SqlDbType.VarChar));
                        sqlCtx.AddParam("OrderType", new SqlParameter("@OrderType", SqlDbType.VarChar));
                        sqlCtx.AddParam("BOL", new SqlParameter("@BOL", SqlDbType.VarChar));
                        sqlCtx.AddParam("HAWB", new SqlParameter("@HAWB", SqlDbType.VarChar));
                        sqlCtx.AddParam("Carrier", new SqlParameter("@Carrier", SqlDbType.VarChar));

                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("PackID", new SqlParameter("@PackID", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltStackType", new SqlParameter("@StdPltStackType", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DeliveryNo").Value = deliveryNo;
                sqlCtx.Param("ShipmentNo").Value = item.ShipmentNo;
                sqlCtx.Param("ShipType").Value = item.ShipType;
                sqlCtx.Param("PalletType").Value = item.PalletType;
                sqlCtx.Param("ConsolidateQty").Value = item.ConsolidateQty;

                sqlCtx.Param("CartonQty").Value = item.CartonQty;
                sqlCtx.Param("QtyPerCarton").Value = item.QtyPerCarton;
                sqlCtx.Param("MessageCode").Value = item.MessageCode;
                sqlCtx.Param("ShipToParty").Value = item.ShipToParty;
                sqlCtx.Param("Priority").Value = item.Priority;

                sqlCtx.Param("GroupId").Value = item.GroupId;
                sqlCtx.Param("OrderType").Value = item.OrderType;
                sqlCtx.Param("BOL").Value = item.BOL;
                sqlCtx.Param("HAWB").Value = item.HAWB;
                sqlCtx.Param("Carrier").Value = item.Carrier;

                sqlCtx.Param("ShipWay").Value = item.ShipWay;
                sqlCtx.Param("PackID").Value = item.PackID;
                sqlCtx.Param("Editor").Value = item.Editor;

                sqlCtx.Param("StdPltFullQty").Value = item.StdPltFullQty;
                sqlCtx.Param("StdPltStackType").Value = item.StdPltStackType;
                item.DeliveryNo = deliveryNo;

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

        private void PersistUpdateDeliveryPallet(DeliveryPallet dp)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet));
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_ID].Value = dp.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryNo].Value = dp.DeliveryID;
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryQty].Value = dp.DeliveryQty;
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_Editor].Value = dp.Editor;
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_PalletNo].Value = dp.PalletID;
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_ShipmentNo].Value = dp.ShipmentID;
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_Status].Value = dp.Status;
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertDeliveryLog(string dn, fons::DeliveryLog item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::DeliveryLog>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx = FuncNew.SetColumnFromField<mtns::DeliveryLog, fons::DeliveryLog>(sqlCtx, item);
                sqlCtx.Param(mtns::DeliveryLog.fn_cdt).Value = cmDt;
                item.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertDeliveryInfo(fons.DeliveryInfo di)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::DeliveryInfo>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx = FuncNew.SetColumnFromField<mtns::DeliveryInfo, fons::DeliveryInfo>(sqlCtx, di);
                sqlCtx.Param(mtns::DeliveryInfo.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::DeliveryInfo.fn_udt).Value = cmDt;
                di.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateDeliveryInfo(fons.DeliveryInfo di)
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
                        sqlCtx = FuncNew.GetCommonUpdate<mtns::DeliveryInfo>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx = FuncNew.SetColumnFromField<mtns::DeliveryInfo, fons::DeliveryInfo>(sqlCtx, di);
                sqlCtx.Param(mtns::DeliveryInfo.fn_udt).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region For Maintain

        public IList<string> GetDeliveryList()
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery), null, new List<string>() { _Schema.Delivery.fn_DeliveryNo }, null, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Delivery.fn_DeliveryNo);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery.fn_DeliveryNo]);
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

        public IList<string> GetDeliveryInfoList()
        {
            // select Distinct InfoType from DeliveryInfo
            // 按InfoType列的字符序排序
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DeliveryInfo), "DISTINCT", new List<string>() { _Schema.DeliveryInfo.fn_InfoType }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.DeliveryInfo.fn_InfoType);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.DeliveryInfo.fn_InfoType]);
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

        public IList<KittingLocationInfo> GetKittingLocationList()
        {
            try
            {
                IList<KittingLocationInfo> ret = new List<KittingLocationInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Kitting_Location));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[]{_Schema.Kitting_Location.fn_TagID, _Schema.Kitting_Location.fn_TagDescr}));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            KittingLocationInfo item = new KittingLocationInfo();
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Cdt]);
                            item.Comm = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Comm]);
                            item.ConfigedDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_ConfigedDate]);
                            item.ConfigedLEDBlock = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_ConfigedLEDBlock]);
                            item.ConfigedLEDStatus = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_ConfigedLEDStatus]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Editor]);
                            item.GateWayIP = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_GateWayIP]);
                            item.GateWayPort = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_GateWayPort]);
                            item.LEDValues = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_LEDValues]);
                            item.RackID = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RackID]);
                            item.RunningDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RunningDate]);
                            item.RunningLEDBlock = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RunningLEDBlock]);
                            item.RunningLEDStatus = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RunningLEDStatus]);
                            item.TagDescr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_TagDescr]);
                            item.TagID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_TagID]);
                            item.TagTP = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_TagTp]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Udt]);
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

        public IList<KittingLocationInfo> GetKittingLocationList(string tagID)
        {
            try
            {
                IList<KittingLocationInfo> ret = new List<KittingLocationInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Kitting_Location cond = new _Schema.Kitting_Location();
                        cond.TagID = tagID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Kitting_Location), cond, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.Kitting_Location.fn_TagID, _Schema.Kitting_Location.fn_TagDescr }));
                    }
                }
                sqlCtx.Params[_Schema.Kitting_Location.fn_TagID].Value = tagID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            KittingLocationInfo item = new KittingLocationInfo();
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Cdt]);
                            item.Comm = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Comm]);
                            item.ConfigedDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_ConfigedDate]);
                            item.ConfigedLEDBlock = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_ConfigedLEDBlock]);
                            item.ConfigedLEDStatus = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_ConfigedLEDStatus]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Editor]);
                            item.GateWayIP = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_GateWayIP]);
                            item.GateWayPort = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_GateWayPort]);
                            item.LEDValues = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_LEDValues]);
                            item.RackID = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RackID]);
                            item.RunningDate = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RunningDate]);
                            item.RunningLEDBlock = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RunningLEDBlock]);
                            item.RunningLEDStatus = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_RunningLEDStatus]);
                            item.TagDescr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_TagDescr]);
                            item.TagID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_TagID]);
                            item.TagTP = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_TagTp]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Kitting_Location.fn_Udt]);
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

        public void UpdateKittingLocation(KittingLocationInfo item)
        {
            // UPDATE [IMES_PAK].[dbo].[Kitting_Location]
            //    SET [TagDescr] = @TagDescr
            //       ,[Editor] = @Editor
            //       ,[Udt] = GETDATE()
            //  WHERE [GateWayIP]=@GateWayIP AND [RackID]=@RackID
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Kitting_Location cond = new _Schema.Kitting_Location();
                        cond.GateWayIP = item.GateWayIP;
                        cond.RackID = item.RackID;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Kitting_Location), new List<string>() { _Schema.Kitting_Location.fn_TagDescr,_Schema.Kitting_Location.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Kitting_Location.fn_GateWayIP].Value = item.GateWayIP;
                sqlCtx.Params[_Schema.Kitting_Location.fn_RackID].Value = item.RackID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Kitting_Location.fn_TagDescr)].Value = item.TagDescr;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Kitting_Location.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FA_Station.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void UpdateKittingLocationDefered(IUnitOfWork uow, KittingLocationInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #endregion

        #region Lucy Liu Added

        public void DeleteDeliveryByDn(string dn)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery));
                    }
                }
                sqlCtx.Params[_Schema.Delivery.fn_DeliveryNo].Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryInfoByDn(string dn)
        {
            try
            {
                
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        //sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DeliveryInfo));
                        _Schema.DeliveryInfo cond = new _Schema.DeliveryInfo();
                        cond.DeliveryNo = dn;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.DeliveryInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.DeliveryInfo.fn_DeliveryNo].Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryAttrsByDn(string dn)
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
                        mtns::DeliveryAttr cond = new mtns::DeliveryAttr();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::DeliveryAttr>(tk, new ConditionCollection<mtns::DeliveryAttr>(new EqualCondition<mtns::DeliveryAttr>(cond)));
                    }
                }
                sqlCtx.Param(mtns::DeliveryAttr.fn_deliveryNo).Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryAttrLogByDn(string dn)
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
                        mtns::DeliveryAttrLog cond = new mtns::DeliveryAttrLog();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::DeliveryAttrLog>(tk, new ConditionCollection<mtns::DeliveryAttrLog>(new EqualCondition<mtns::DeliveryAttrLog>(cond)));
                    }
                }
                sqlCtx.Param(mtns::DeliveryAttrLog.fn_deliveryNo).Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePalletAttrLog(IList<string> pltNos)
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
                        mtns::PalletAttrLog cond = new mtns::PalletAttrLog();
                        cond.palletNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PalletAttrLog>(tk, new ConditionCollection<mtns::PalletAttrLog>(new InSetCondition<mtns::PalletAttrLog>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.PalletAttrLog.fn_palletNo), g.ConvertInSet(pltNos));

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryAttrsByShipmentNo(string shipment)
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
                        mtns::DeliveryAttr cond = new mtns::DeliveryAttr();
                        cond.deliveryNo = shipment;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::DeliveryAttr>(tk, new ConditionCollection<mtns::DeliveryAttr>(new AnyCondition<mtns::DeliveryAttr>(cond, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4})", "{0}", mtns::Delivery.fn_deliveryNo, ToolsNew.GetTableName(typeof(mtns::Delivery)), mtns::Delivery.fn_shipmentNo, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::DeliveryAttr.fn_deliveryNo)).Value = shipment;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryAttrLogByShipmentNo(string shipment)
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
                        mtns::DeliveryAttrLog cond = new mtns::DeliveryAttrLog();
                        cond.deliveryNo = shipment;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::DeliveryAttrLog>(tk, new ConditionCollection<mtns::DeliveryAttrLog>(new AnyCondition<mtns::DeliveryAttrLog>(cond, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4})", "{0}", mtns::Delivery.fn_deliveryNo, ToolsNew.GetTableName(typeof(mtns::Delivery)), mtns::Delivery.fn_shipmentNo, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::DeliveryAttrLog.fn_deliveryNo)).Value = shipment;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeliveryPalletByDn(string dn)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        //sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet));
                        _Schema.Delivery_Pallet cond = new _Schema.Delivery_Pallet();
                        cond.DeliveryNo = dn;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryNo].Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// "删除Pallet表中一条记录数据
        /// </summary>
        /// <param name="palletNo"></param>
        public void DeletePalletByPalletNo(string palletNo)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        //sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet));
                        _Schema.Pallet cond = new _Schema.Pallet();
                        cond.PalletNo = palletNo;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pallet.fn_PalletNo].Value = palletNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillDeliveryAttributes(fons::Delivery delivery)
        {
            try
            {
                IList<fons::DeliveryInfo> newFieldVal = new List<fons::DeliveryInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.DeliveryInfo cond = new _Metas.DeliveryInfo();
                        cond.deliveryNo = delivery.DeliveryNo;
                        sqlCtx = FuncNew.GetConditionedSelect(tk, null, null, new ConditionCollection<_Metas.DeliveryInfo>(new EqualCondition<_Metas.DeliveryInfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.DeliveryInfo.fn_deliveryNo).Value = delivery.DeliveryNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    newFieldVal = FuncNew.SetFieldFromColumn<_Metas.DeliveryInfo, fons::DeliveryInfo, fons::DeliveryInfo>(newFieldVal, sqlR, sqlCtx);
                }
                if (newFieldVal != null && newFieldVal.Count > 0)
                {
                    foreach (fons::DeliveryInfo entry in newFieldVal)
                    {
                        entry.Tracker.Clear();
                        entry.Tracker = delivery.Tracker;
                    }
                }
                delivery.GetType().GetField("_deliveryInfoes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(delivery, newFieldVal);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDn(string dn)
        {
            try
            {
                IList<DNPalletQty> lstPallet = GetPalletList(dn);

                IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, IMES.FisObject.PAK.Pallet.Pallet>();
                bool conflictFlag = false;
                List<bool> conflictArray = new List<bool>();
                foreach (DNPalletQty dnplt in lstPallet)
                {
                    conflictFlag = false;

                    //判断这个pallet在Delivery_Palet表中是否还对应其它的dn，如果存在，那么这条PalletNo则不能删除，否则会引起冲突
                    IList<DeliveryPallet> deliveryPalletLst = palletRepository.GetDeliveryPallet(dnplt.PalletNo);
                    foreach (DeliveryPallet deliveryPallet in deliveryPalletLst)
                    {
                        if (deliveryPallet.DeliveryID != dn)
                        {
                            conflictFlag = true;
                            break;
                        }
                    }
                    conflictArray.Add(conflictFlag);                   

                }
                
                SqlTransactionManager.Begin();

                DeleteDeliveryInfoByDn(dn);
                DeleteDeliveryPalletByDn(dn);
                DeleteDeliveryByDn(dn);
                
               
                //ITC-1268-0077
                //删除Pallet表,PalletID表
                int i = 0;
                foreach (DNPalletQty dnplt in lstPallet)
                {
                   
                    if (!conflictArray[i])
                    {
                        DeletePalletByPalletNo(dnplt.PalletNo);
                        //delete Pallet ID
                        palletRepository.DeletePalletIDByPalletNo(dnplt.PalletNo);
                    }
                    i++;

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
      
        #endregion

        public void DeleteDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        #region . OnTrans .

        public fons.Delivery QueryAsLockDnForCombineCOAandDN_OnTrans(string dn)
        {
            SqlDataReader sqlR = null;
            try
            {
                fons.Delivery ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.deliveryNo = dn;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null, new ConditionCollection<_Metas.Delivery>(new EqualCondition<_Metas.Delivery>(cond)));
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=Delivery_PK,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = dn;

                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                ret = FuncNew.SetFieldFromColumn<mtns.Delivery, fons.Delivery>(ret, sqlR, sqlCtx);

                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sqlR != null)
                {
                    sqlR.Close();
                }
            }
        }

        public IList<SnoCtrlBoxIdSQInfo> GetSnoCtrlBoxIdSQListByCust(string cust, int rowCount, int offset)
        {
            SqlDataReader sqlR = null;
            try
            {
                if (offset < 0)
                    offset = 0;

                IList<SnoCtrlBoxIdSQInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        SnoCtrl_BoxId_SQ cond = new SnoCtrl_BoxId_SQ();
                        cond.cust = cust;
                        sqlCtx = FuncNew.GetConditionedSelect<SnoCtrl_BoxId_SQ>(tk, "TOP [TopCount]", null, new ConditionCollection<SnoCtrl_BoxId_SQ>(new EqualCondition<SnoCtrl_BoxId_SQ>(cond)));

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=IDX_SnoCtrlBoxIdSq_Cust,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Param(SnoCtrl_BoxId_SQ.fn_cust).Value = cust;
                string Sentence = sqlCtx.Sentence.Replace("TOP [TopCount]", "TOP " + (rowCount + offset).ToString());
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                //{
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params);
                if (sqlR != null)
                {
                    ret = new List<SnoCtrlBoxIdSQInfo>();

                    int i = 0;
                    while (sqlR.Read())
                    {
                        if (i < offset)
                        {
                            i++;//Ship
                        }
                        else
                        {
                            SnoCtrlBoxIdSQInfo item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<SnoCtrl_BoxId_SQ, SnoCtrlBoxIdSQInfo>(item, sqlR, sqlCtx);
                            ret.Add(item);
                        }
                    }
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

        #endregion


        #region Implement DeliveryEx table
        public void FillDeliveryEx(Delivery delivery)
        {
            try
            {
                DeliveryEx ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select DeliveryNo, ShipmentNo, ShipType, PalletType, ConsolidateQty, 
	                                                                   CartonQty, QtyPerCarton, MessageCode, ShipToParty, Priority, 
                                                                       GroupId, OrderType, BOL, HAWB, Carrier, 
                                                                       ShipWay, PackID, Editor, Udt, StdPltFullQty, StdPltStackType
                                                                from  DeliveryEx
                                                                where DeliveryNo = @deliveryNo";
                        sqlCtx.AddParam("deliveryNo", new SqlParameter("@deliveryNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("deliveryNo").Value = delivery.DeliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = Shema.SQLData.ToObject<DeliveryEx>(sqlR);
                        ret.Tracker.Clear();
                    }
                }

                delivery.GetType().GetField("_deliveryExInfo", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(delivery, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RemoveDeliveryEx(string deliveryNo)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"delete  DeliveryEx
                                                             where DeliveryNo=@DeliveryNo";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));                      


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DeliveryNo").Value = deliveryNo;            
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
        
        public void RemoveDeliveryExDefered(IUnitOfWork uow, string deliveryNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), deliveryNo);
        }

        public int GetUploadDNQtyByShipment(string shipmentNo)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select count(distinct SUBSTRING(DeliveryNo,1,10)) as UploadQty 
                                                            from DeliveryEx
                                                            where ShipmentNo =@ShipmentNo";
                        sqlCtx.AddParam("ShipmentNo", new SqlParameter("@ShipmentNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipmentNo").Value = shipmentNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetInt32(0);
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateConsolidateQtyInDeliveryEx(string consolidateId, int qty, string editor)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update  DeliveryEx
                                                            set ConsolidateQty= @ConsolidateQty,
                                                                   Editor =@Editor,
                                                                   Udt = getdate()  
                                                             where ShipmentNo= @ShipmentNo";
                        sqlCtx.AddParam("ShipmentNo", new SqlParameter("@ShipmentNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("ConsolidateQty", new SqlParameter("@ConsolidateQty", SqlDbType.Int));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipmentNo").Value = consolidateId;
                sqlCtx.Param("ConsolidateQty").Value = qty;
                sqlCtx.Param("Editor").Value = editor;
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
        public void UpdateConsolidateQtyInDeliveryExDefered(IUnitOfWork uow, string consolidateId, int qty, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), consolidateId, qty, editor);
        }

        public IList<ShipmentInfoDef> GetShipmentByDnList(string DBconnectionStr,IList<string> dnList)
        {
            try
            {
                IList<ShipmentInfoDef> ret = new List<ShipmentInfoDef>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select a.data as DeliveryNo,
                                                                    isnull(b.InfoValue,a.data) as Shipment,
                                                                    case when b.InfoValue is null then 'D'
                                                                         else 'S'
                                                                         end as ShipType 
                                                             from @data a
                                                             left join DeliveryInfo b on a.data = b.DeliveryNo and b.InfoType='Consolidated'";
                        SqlParameter para1 = new SqlParameter("@data", SqlDbType.Structured);
                        para1.TypeName = "TbStringList";
                        sqlCtx.AddParam("DNList", para1);
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DNList").Value = Shema.SQLData.ToDataTable(dnList);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(DBconnectionStr,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null )
                    {
                        while (sqlR.Read())
                        {
                            ShipmentInfoDef item = Shema.SQLData.ToObject<ShipmentInfoDef>(sqlR);
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


        public int GetSumCartonQtyByPalletNo(string palletNo)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select COUNT(distinct CartonSN) as CartonQty
                                                            from Product
                                                            where PalletNo =@PalletNo and
                                                                        CartonSN!=''  ";
                       
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo",SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                       ret= sqlR.GetInt32(0);
                    }
                }

                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetFullPalletCartonQtyByDeliveryPallet(string deliveryNo, string palletNo)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select [dbo].[fn_GetFullPLTQty](@DeliveryNo, @PalletNo) as CartonQty ";

                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;
                sqlCtx.Param("DeliveryNo").Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetInt32(0);
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

        #region Get EDI Data
        public MRPLabelDef GetMRPLabel(string deliveryNo)
        {
            try
            {
                MRPLabelDef ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select top 1 INDIA_PRICE as IndiaPrice, 
                                                                                INDIA_PRICE_ID as IndiaPriceID, 
                                                                                INDIA_GENERIC_DESC as IndiaPriceDescr
                                                              from [PAK.PAKComn] 
                                                            where InternalID=@deliveryNo";
                        sqlCtx.AddParam("deliveryNo", new SqlParameter("@deliveryNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("deliveryNo").Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = Shema.SQLData.ToObject<MRPLabelDef>(sqlR);
                       
                    }
                }
                return ret; 

            }
            catch (Exception)
            {
                throw;
            }

        }

        public int GetPackingDataSNCount(IList<string> dnList)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Qty=COUNT(distinct SERIAL_NUM)
                                                             from PAK_PackkingData
                                                             where InternalID in (select data from @DNList)  ";
                        SqlParameter par = new SqlParameter("@DNList", SqlDbType.Structured);
                        par.TypeName = "TbStringList";                        
                        sqlCtx.AddParam("DNList", par);
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DNList").Value = Shema.SQLData.ToDataTable(dnList);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetInt32(0);
                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<string> GetEdiDnListByWayBill(string wayBill)
        {
            try
            {
                IList<string> ret =  new List<string>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct InternalID
                                                            from [PAK.PAKComn] 
                                                            where WAYBILL_NUMBER=@WayBill  ";
                        
                        sqlCtx.AddParam("WayBill", new SqlParameter("@WayBill", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("WayBill").Value = wayBill;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null )
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(sqlR.GetString(0).Trim());
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
        public IList<string> GetEdiDnListByShipment(string shipmentNo)
        {
            try
            {
                IList<string> ret = new List<string>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct InternalID
                                                            from [PAK.PAKComn] 
                                                            where InternalID like @DeliveryShipNo or
                                                                       SHIPMENT = @ShipmentNo ";

                        sqlCtx.AddParam("ShipmentNo", new SqlParameter("@ShipmentNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("DeliveryShipNo", new SqlParameter("@DeliveryShipNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("ShipmentNo").Value = shipmentNo;
                sqlCtx.Param("DeliveryShipNo").Value = shipmentNo.Trim() + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(sqlR.GetString(0).Trim());
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
        public int GetPackComnSNCount(IList<string> dnList)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select Qty=isnull(sum(PACK_ID_LINE_ITEM_UNIT_QTY),0)
                                                              from [PAK.PAKComn] 
                                                            where InternalID in (select data from @DNList)    ";
                        SqlParameter par = new SqlParameter("@DNList", SqlDbType.Structured);
                        par.TypeName = "TbStringList";
                        sqlCtx.AddParam("DNList", par);
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DNList").Value = Shema.SQLData.ToDataTable(dnList);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = (int) sqlR.GetSqlDecimal(0);

                    }
                }
                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckEDI850ByHPPoNum(string hpPoNum)
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
                        sqlCtx.Sentence = @"select ID 
                                                        from dbo.[PAK.PAKEdi850raw]
                                                        where PO_NUM=@PO_Num";
                        sqlCtx.AddParam("PO_NUM", new SqlParameter("@PO_Num", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PO_NUM").Value = hpPoNum;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null )
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
        public void updateEDIPAKComnShipDate(string internalID, string shipDate)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update dbo.[PAK_PAKComn]
                                                           set  ACTUAL_SHIPDATE=@ShipDate
                                                         where  InternalID=@InternalID
                                                         
                                                         
                                                         update dbo.[PAK.PAKComn]
                                                           set  ACTUAL_SHIPDATE=@ShipDate
                                                         where  InternalID=@InternalID";
                        sqlCtx.AddParam("ShipDate", new SqlParameter("@ShipDate", SqlDbType.NVarChar));
                        sqlCtx.AddParam("InternalID", new SqlParameter("@InternalID", SqlDbType.NVarChar));
                      
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipDate").Value = shipDate;
                sqlCtx.Param("InternalID").Value = internalID;               

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void updateEDIPAKComnShipDateDefered(IUnitOfWork uow, string internalID, string shipDate)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), internalID, shipDate);
        }
        #endregion


        #region SpecialOrder table for maintain UI

        public void InsertSpecialOrder(SpecialOrderInfo sepcialOrder)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"insert into SpecialOrder(FactoryPO, Category, AssetTag, Qty, Status, 
                                                                                     Remark, Editor, Cdt, Udt)
                                                            values(@FactoryPO, @Category, @AssetTag, @Qty, @Status, 
                                                                   @Remark, @Editor, getdate(), getdate())";
                        sqlCtx.AddParam("FactoryPO", new SqlParameter("@FactoryPO", SqlDbType.VarChar));
                        sqlCtx.AddParam("Category", new SqlParameter("@Category", SqlDbType.VarChar));
                        sqlCtx.AddParam("AssetTag", new SqlParameter("@AssetTag", SqlDbType.VarChar));
                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));

                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));                        
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("FactoryPO").Value = sepcialOrder.FactoryPO;
                sqlCtx.Param("Category").Value = sepcialOrder.Category;
                sqlCtx.Param("AssetTag").Value = sepcialOrder.AssetTag;
                sqlCtx.Param("Qty").Value = sepcialOrder.Qty;
                sqlCtx.Param("Status").Value = sepcialOrder.Status.ToString();

                sqlCtx.Param("Remark").Value = sepcialOrder.Remark;
                sqlCtx.Param("Editor").Value = sepcialOrder.Editor;

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
        public void UpdateSpecialOrder(SpecialOrderInfo sepcialOrder)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update SpecialOrder
                                                                 set Category=@Category,
                                                                     AssetTag=@AssetTag,
                                                                     Qty=@Qty,
                                                                     Status=@Status,
                                                                     Remark=@Remark,
                                                                     Editor=@Editor,
                                                                     Udt=getdate()
                                                                where FactoryPO=@FactoryPO";
                        sqlCtx.AddParam("FactoryPO", new SqlParameter("@FactoryPO", SqlDbType.VarChar));
                        sqlCtx.AddParam("Category", new SqlParameter("@Category", SqlDbType.VarChar));
                        sqlCtx.AddParam("AssetTag", new SqlParameter("@AssetTag", SqlDbType.VarChar));
                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));

                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("FactoryPO").Value = sepcialOrder.FactoryPO;
                sqlCtx.Param("Category").Value = sepcialOrder.Category;
                sqlCtx.Param("AssetTag").Value = sepcialOrder.AssetTag;
                sqlCtx.Param("Qty").Value = sepcialOrder.Qty;
                sqlCtx.Param("Status").Value = sepcialOrder.Status.ToString();

                sqlCtx.Param("Remark").Value = sepcialOrder.Remark;
                sqlCtx.Param("Editor").Value = sepcialOrder.Editor;

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
        public void DeleteSpecialOrder(string factoryPO)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"delete from SpecialOrder
                                                            where FactoryPO=@FactoryPO";
                        sqlCtx.AddParam("FactoryPO", new SqlParameter("@FactoryPO", SqlDbType.VarChar));
                       

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("FactoryPO").Value = factoryPO;
               

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
        public IList<SpecialOrderInfo> GetSpecialOrder(string category, SpecialOrderStatus status, DateTime startTime, DateTime endTime)
        {
            try
            {
                IList<SpecialOrderInfo> ret = new List<SpecialOrderInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select FactoryPO, Category, AssetTag, Qty, Status, 
                                                                   Remark, Editor, Cdt, Udt
                                                            from SpecialOrder  
                                                            where Category=@Category and
                                                                       Status =@Status and
                                                                        Cdt between @StartTime and @EndTime";
                        sqlCtx.AddParam("Category", new SqlParameter("@Category", SqlDbType.VarChar));                       
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("StartTime", new SqlParameter("@StartTime", SqlDbType.DateTime));
                        sqlCtx.AddParam("EndTime", new SqlParameter("@EndTime", SqlDbType.DateTime));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Category").Value = category;
                sqlCtx.Param("Status").Value = status.ToString();
                sqlCtx.Param("StartTime").Value = startTime;
                sqlCtx.Param("EndTime").Value = endTime;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            SpecialOrderInfo item = Shema.SQLData.ToObjectWithAttribute<SpecialOrderInfo>(sqlR);
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
        public bool ExistSpecialOrder(string factoryPO)
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
                        sqlCtx.Sentence = @"select FactoryPO
                                                            from SpecialOrder  
                                                            where FactoryPO=@FactoryPO";
                        sqlCtx.AddParam("FactoryPO", new SqlParameter("@FactoryPO", SqlDbType.VarChar));                     
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("FactoryPO").Value = factoryPO;               

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.HasRows)
                    {
                        ret = true;
                    }
                }

                return ret;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public SpecialOrderInfo GetSpecialOrderByPO(string factoryPO)
        {
            try
            {
                SpecialOrderInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select FactoryPO, Category, AssetTag, Qty, Status, 
                                                                   Remark, Editor, Cdt, Udt
                                                            from SpecialOrder  
                                                            where FactoryPO=@FactoryPO ";
                        sqlCtx.AddParam("FactoryPO", new SqlParameter("@FactoryPO", SqlDbType.VarChar));                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("FactoryPO").Value = factoryPO;
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {                      
                         ret = Shema.SQLData.ToObjectWithAttribute<SpecialOrderInfo>(sqlR);
                     
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

        #region  DeliveryAttr insert/update/Get
        public void UpdateAndInsertDeliveryAttr(string deliveryNo, string attrName, string attrValue, string descr, string editor)
        {
            try
            {
                DeliveryAttrLogInfo log = null; 
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"merge  DeliveryAttr as T
                                                            using (select @DeliveryNo as DeliveryNo,
                                                                          @AttrName as AttrName,
                                                                          @AttrValue as AttrValue,
                                                                          @Editor as Editor) as S
                                                            on (T.DeliveryNo = S.DeliveryNo and T.AttrName=S.AttrName)
                                                            When MATCHED  Then
                                                              Update 
                                                              Set AttrValue= S.AttrValue,
                                                                  Udt = getdate(),
                                                                  Editor = S.Editor
                                                            When NOT MATCHED  Then
                                                              Insert (AttrName, DeliveryNo, AttrValue, Editor, Cdt, Udt)
                                                              values(S.AttrName, S.DeliveryNo, S.AttrValue, S.Editor, getdate(), getdate())
                                                            OUTPUT  inserted.DeliveryNo,inserted.AttrName,  '' as ShipmentNo,
                                                                    isnull(deleted.AttrValue,'') as AttrOldValue,inserted.AttrValue as AttrNewValue,
                                                                    inserted.Editor, inserted.Udt as Cdt, @Descr as Descr ; ";

                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = deliveryNo;
                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("AttrValue").Value = attrValue;
                sqlCtx.Param("Descr").Value = descr;
                sqlCtx.Param("Editor").Value = editor;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                  CommandType.Text,
                                                                                                                                  sqlCtx.Sentence,
                                                                                                                                  sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        log = Shema.SQLData.ToObjectByField<DeliveryAttrLogInfo>(sqlR);
                     }
                }
                log.descr = descr;
                InsertDeliveryAttrLogInfo(log);               

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertDeliveryAttrDefered(IUnitOfWork uow, string deliveryNo, string attrName, string attrValue, string descr,  string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), deliveryNo, attrName, attrValue, descr, editor);
        }

        public IList<DeliveryAttrInfo> GetDeliveryAttr(DeliveryAttrInfo condition)
        {
            try
            {
                IList<DeliveryAttrInfo> ret = new List<DeliveryAttrInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.DeliveryAttr cond = FuncNew.SetColumnFromField<_Metas.DeliveryAttr, DeliveryAttrInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<_Metas.DeliveryAttr>(null, null,
                                                                                                   new ConditionCollection<_Metas.DeliveryAttr>(new EqualCondition<_Metas.DeliveryAttr>(cond)),
                                                                                                   _Metas.DeliveryAttr.fn_attrName);


                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.DeliveryAttr, DeliveryAttrInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::DeliveryAttr, DeliveryAttrInfo, DeliveryAttrInfo>(ret, sqlR, sqlCtx);
                }

                
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryAttrLogInfo> GetDeliveryAttrLog(DeliveryAttrLogInfo condition)
        {
            try
            {
                IList<DeliveryAttrLogInfo> ret = new List<DeliveryAttrLogInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.DeliveryAttrLog cond = FuncNew.SetColumnFromField<_Metas.DeliveryAttrLog, DeliveryAttrLogInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<_Metas.DeliveryAttrLog>(null, null,
                                                                                                   new ConditionCollection<_Metas.DeliveryAttrLog>(new EqualCondition<_Metas.DeliveryAttrLog>(cond)),
                                                                                                   _Metas.DeliveryAttrLog.fn_cdt);


                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.DeliveryAttrLog, DeliveryAttrLogInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::DeliveryAttrLog, DeliveryAttrLogInfo, DeliveryAttrLogInfo>(ret, sqlR, sqlCtx);
                }


                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryForRCTO146> GetDeliveryForRCTO146(string model, string status, DateTime beginShipDate, DateTime endShipDate)
        {
            try
            {
                IList<DeliveryForRCTO146> ret = new List<DeliveryForRCTO146>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.DeliveryNo, a.Model, a.Qty, a.ShipDate, a.Status,
                                                                   b.CartonQty, b.MessageCode, b.OrderType, b.QtyPerCarton, b.CartonQty,
                                                                   b.ShipWay,b.ShipmentNo  
                                                            from Delivery a, 
                                                                 DeliveryEx b
                                                            where a.DeliveryNo = b.DeliveryNo and
                                                                  a.Model = @Model and
                                                                  a.Status = @Status and
                                                                  a.ShipDate between @BeginShipDate and @EndShipDate
                                                            order by a.ShipDate, a.Qty";

                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("BeginShipDate", new SqlParameter("@BeginShipDate", SqlDbType.DateTime));
                        sqlCtx.AddParam("EndShipDate", new SqlParameter("@EndShipDate", SqlDbType.DateTime));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Model").Value = model;
                sqlCtx.Param("Status").Value = status;
                sqlCtx.Param("BeginShipDate").Value = beginShipDate;
                sqlCtx.Param("EndShipDate").Value = endShipDate;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {

                            ret.Add(_Schema.SQLData.ToObject<DeliveryForRCTO146>(sqlR));
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
        public IList<DeliveryForRCTO146> GetDeliveryForRCTO146(string modelPrefix, string status, DateTime beginShipDate, DateTime endShipDate, 
                                                                                                    string modelInfoName, string modelInfoValue)
        {
            try
            {
                IList<DeliveryForRCTO146> ret = new List<DeliveryForRCTO146>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.DeliveryNo, a.Model, a.Qty, a.ShipDate, a.Status,
                                                                   b.CartonQty, b.MessageCode, b.OrderType, b.QtyPerCarton, b.CartonQty,
                                                                   b.ShipWay, b.ShipmentNo  
                                                            from Delivery a, 
                                                                 DeliveryEx b,
                                                                 ModelInfo c
                                                            where a.DeliveryNo = b.DeliveryNo and     
                                                                  a.Model =c.Model and
                                                                  a.Model like @Model and
                                                                  a.Status = @Status and
                                                                  a.ShipDate between @BeginShipDate and @EndShipDate and
                                                                  c.Name=@Name and
                                                                  c.Value=@Value      
                                                            order by a.ShipDate, a.Qty";

                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("BeginShipDate", new SqlParameter("@BeginShipDate", SqlDbType.DateTime));
                        sqlCtx.AddParam("EndShipDate", new SqlParameter("@EndShipDate", SqlDbType.DateTime));
                        sqlCtx.AddParam("Name", new SqlParameter("@Name", SqlDbType.VarChar));
                        sqlCtx.AddParam("Value", new SqlParameter("@Value", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Model").Value = modelPrefix+"%";
                sqlCtx.Param("Status").Value = status;
                sqlCtx.Param("BeginShipDate").Value = beginShipDate;
                sqlCtx.Param("EndShipDate").Value = endShipDate;
                sqlCtx.Param("Name").Value = modelInfoName;
                sqlCtx.Param("Value").Value = modelInfoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {

                            ret.Add(_Schema.SQLData.ToObject<DeliveryForRCTO146>(sqlR));
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
        public int GetDeliveryQtyOnTrans(string deliveryNo, string status)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  a.Qty
                                                         from Delivery a WITH (INDEX=Delivery_PK,ROWLOCK,UPDLOCK)
                                                          where a.DeliveryNo =@DeliveryNo and 
                                                                    a.Status=@Status ";

                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = deliveryNo;
                sqlCtx.Param("Status").Value = status;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetInt32(0);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetDeliveryQtyOnTrans(string deliveryNo)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select  a.Qty
                                                         from Delivery a WITH (INDEX=Delivery_PK,ROWLOCK,UPDLOCK)
                                                          where a.DeliveryNo =@DeliveryNo";

                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = deliveryNo;
              
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetInt32(0);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<DeliveryPalletInfo> GetDeliveryPalletListByDNOnTrans(string deliveryNo)
        {
            try
            {
                IList<DeliveryPalletInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.deliveryNo = deliveryNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Delivery_Pallet>(tk, null, null, new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_deliveryNo).Value = deliveryNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Delivery_Pallet, DeliveryPalletInfo, DeliveryPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetModelByPalletNo(string palletNo)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select distinct a.Model 
                                                          from Delivery a,
                                                               Delivery_Pallet b
                                                        where a.DeliveryNo = b.DeliveryNo and
                                                              b.PalletNo=@PalletNo";

                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("PalletNo").Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null )
                    {
                        while (sqlR.Read())
                        {
                            ret .Add(sqlR.GetString(0).Trim());
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
        public void UpdateDeliveryStatus(string deliveryNo, string status)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"update  Delivery
                                                            set  Status=@Status,
                                                                   Udt = GETDATE()
                                                                where  DeliveryNo=@DeliveryNo";

                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = deliveryNo;

                sqlCtx.Param("Status").Value = status;

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
        public void UpdateDeliveryStatusDefered(IUnitOfWork uow, string deliveryNo, string status)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     deliveryNo,
                                                    status);
        }


        public void InsertEDIUploadPOLog(EDIUploadPOLogInfo log)
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
                        sqlCtx = _Metas.FuncNew.GetAquireIdInsert<_Metas.UploadPOLog>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.UploadPOLog, EDIUploadPOLogInfo>(sqlCtx, log);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
               
                sqlCtx.Param(_Metas.UploadPOLog.fn_cdt).Value = cmDt;


                log.id = Convert.ToInt64(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_HP_EDI, 
                                                                                                           CommandType.Text, 
                                                                                                           sqlCtx.Sentence, 
                                                                                                           sqlCtx.Params));
             
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Upload SN to SAP
        public IList<Delivery> GetDeliveryListWithTrans(IList<string> deliveryNoList)
        {
            try
            {
                IList<Delivery> ret = new List<Delivery>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Delivery cond = new mtns::Delivery();
                        cond.deliveryNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Delivery>(tk, null, null, new ConditionCollection<mtns::Delivery>(new InSetCondition<mtns::Delivery>(cond)), mtns::Delivery.fn_deliveryNo);
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=Delivery_PK,ROWLOCK,UPDLOCK) WHERE");
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::Delivery.fn_deliveryNo), g.ConvertInSet(new List<string>(deliveryNoList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                            Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Delivery> GetDeliveryWithSamePrefixDeliveryNo(string prefixDeliveyNo)
        {
            try
            {
                IList<Delivery> ret = new List<Delivery>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond = new IMES.Infrastructure.Repository._Metas.Delivery();
                        cond.deliveryNo = prefixDeliveyNo + "%";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk, null, null,
                                                                                                         new ConditionCollection<_Metas.Delivery>(new LikeCondition<_Metas.Delivery>(cond)),
                                                                                                        _Metas.Delivery.fn_deliveryNo);
                    }


                }
                sqlCtx.Param(_Metas.Delivery.fn_deliveryNo).Value = prefixDeliveyNo + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery, Delivery, Delivery>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryPalletInfo> GetDeliveryPalletWithSamePrefixDeliveryNo(string prefixDeliveyNo)
        {
            try
            {
                IList<DeliveryPalletInfo> ret = new List<DeliveryPalletInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery_Pallet cond = new IMES.Infrastructure.Repository._Metas.Delivery_Pallet();
                        cond.deliveryNo = prefixDeliveyNo + "%";


                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery_Pallet>(tk, null, null,
                                                                                                         new ConditionCollection<_Metas.Delivery_Pallet>(new LikeCondition<_Metas.Delivery_Pallet>(cond)),
                                                                                                        _Metas.Delivery_Pallet.fn_palletNo, _Metas.Delivery_Pallet.fn_deliveryNo);
                    }

                }

                sqlCtx.Param(_Metas.Delivery_Pallet.fn_deliveryNo).Value = prefixDeliveyNo + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.Delivery_Pallet, DeliveryPalletInfo, DeliveryPalletInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region UPS
        public bool ExistsDeliveryByPoNo(string model, string poNo, string status, DateTime afterDate )
        {
            try
            {
               bool ret =false;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Delivery cond = new _Metas.Delivery();
                        cond.model = model;
                        cond.poNo = poNo;
                        cond.status = status;

                        _Metas.Delivery cond2 = new _Metas.Delivery();
                        cond2.shipDate = afterDate;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Delivery>(tk,"TOP 1", new string[] {_Metas.Delivery.fn_deliveryNo}, 
                         new ConditionCollection<_Metas.Delivery>(
                            new EqualCondition<_Metas.Delivery>(cond),
                            new GreaterCondition<_Metas.Delivery>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.Delivery.fn_model).Value = model;
                sqlCtx.Param(_Metas.Delivery.fn_status).Value = status;
                 sqlCtx.Param(_Metas.Delivery.fn_poNo).Value = poNo;
                 sqlCtx.Param(g.DecG(_Metas.Delivery.fn_shipDate)).Value = afterDate;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, 
                                                                                                                    CommandType.Text, 
                                                                                                                    sqlCtx.Sentence, 
                                                                                                                    sqlCtx.Params))
                {
                   if (sqlR!=null)
                   {
                       ret= sqlR.HasRows;
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
