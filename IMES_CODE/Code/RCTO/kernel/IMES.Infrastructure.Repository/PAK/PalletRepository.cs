﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using IMES.DataModel.Entity;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using System.Data;
using IMES.Infrastructure.Util;
using System.Data.SqlClient;
using System.Reflection;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
//using IMES.Infrastructure.Repository._Schema;
using log4net;
using fons = IMES.FisObject.PAK.Pallet;

namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: Pallet相关
    /// </summary>
    public class PalletRepository : BaseRepository<IMES.FisObject.PAK.Pallet.Pallet>, IPalletRepository
    {
        private static GetValueClass g = new GetValueClass();

        ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Overrides of BaseRepository<Pallet>

        protected override void PersistNewItem(IMES.FisObject.PAK.Pallet.Pallet item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertPallet(item);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(IMES.FisObject.PAK.Pallet.Pallet item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdatePallet(item);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(IMES.FisObject.PAK.Pallet.Pallet item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeletePallet(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Pallet>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IMES.FisObject.PAK.Pallet.Pallet Find(object key)
        {
            try
            {
                IMES.FisObject.PAK.Pallet.Pallet ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pallet cond = new _Schema.Pallet();
                        cond.PalletNo = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pallet.fn_PalletNo].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new IMES.FisObject.PAK.Pallet.Pallet();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Editor]);
                        ret.Height = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Height]);
                        ret.Length = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Length]);
                        ret.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletNo]);
                        ret.PalletModel = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletModel]);
                        ret.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Station]);
                        ret.UCC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_UCC]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Udt]);
                        ret.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Weight]);
                        ret.Width = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Width]);
                        ret.Weight_L = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_weight_L]);
                        ret.Floor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Floor]);
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
        public override IList<IMES.FisObject.PAK.Pallet.Pallet> FindAll()
        {
            try
            {
                IList<IMES.FisObject.PAK.Pallet.Pallet> ret = new List<IMES.FisObject.PAK.Pallet.Pallet>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.PAK.Pallet.Pallet item = new IMES.FisObject.PAK.Pallet.Pallet();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Editor]);
                        item.Height = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Height]);
                        item.Length = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Length]);
                        item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletNo]);
                        item.PalletModel = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletModel]);
                        item.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Station]);
                        item.UCC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_UCC]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Udt]);
                        item.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Weight]);
                        item.Width = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Width]);
                        item.Weight_L = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_weight_L]);
                        item.Floor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Floor]);
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
        public override void Add(IMES.FisObject.PAK.Pallet.Pallet item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(IMES.FisObject.PAK.Pallet.Pallet item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(IMES.FisObject.PAK.Pallet.Pallet item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IPalletRepository

        public IList<PalletInfo> GetPalletList(string DNId)
        {
            try
            {
                IList<PalletInfo> ret = new List<PalletInfo>();

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
                        cond1.DeliveryNo = DNId;
                        //cond1.Status = "0";//2012.01.10 高永勃 ITC) [13:49]: 暂时不要看了，我们并没有控制过它
                        tf1.equalcond = cond1;
                        tf1.ToGetFieldNames = null;

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.Pallet);
                        tf2.ToGetFieldNames.Add(_Schema.Pallet.fn_PalletNo);

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.Delivery_Pallet.fn_PalletNo, tf2, _Schema.Pallet.fn_PalletNo);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery_Pallet.fn_Status)].Value = cond1.Status;//2012.01.10 高永勃 ITC) [13:49]: 暂时不要看了，我们并没有控制过它

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf2.alias, _Schema.Pallet.fn_PalletNo));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.Delivery_Pallet.fn_DeliveryNo)].Value = DNId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletInfo item = new PalletInfo();
                        item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias, _Schema.Pallet.fn_PalletNo)]);
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

        public IMES.FisObject.PAK.Pallet.Pallet FillPalletLogs(IMES.FisObject.PAK.Pallet.Pallet currentPallet)
        {
            try
            {
                IList<fons.PalletLog> newFieldVal = new List<fons.PalletLog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PalletLog cond = new _Schema.PalletLog();
                        cond.PalletNo = currentPallet.PalletNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletLog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PalletLog.fn_PalletNo].Value = currentPallet.PalletNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PalletLog plltlog = new fons.PalletLog();
                        plltlog.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletLog.fn_Cdt]);
                        plltlog.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletLog.fn_Editor]);
                        plltlog.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PalletLog.fn_ID]);
                        plltlog.Line = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletLog.fn_Line]);
                        plltlog.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletLog.fn_PalletNo]);
                        plltlog.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletLog.fn_Station]);
                        plltlog.Tracker.Clear();
                        plltlog.Tracker = currentPallet.Tracker;
                        newFieldVal.Add(plltlog);
                    }
                }
                currentPallet.GetType().GetField("_palletLogs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPallet, newFieldVal);
                return currentPallet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryPallet> GetDeliveryPallet(string PalletNo)
        {
            try
            {
                IList<DeliveryPallet> ret = new List<DeliveryPallet>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Delivery_Pallet cond = new _Schema.Delivery_Pallet();
                        cond.PalletNo = PalletNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_PalletNo].Value = PalletNo;
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
                        item.PalletType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_palletType]);
                        item.DeviceQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_deviceQty]);
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

        public void UpdatePltWeightToSAP(string PalletNo)
        {
            try
            {
                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@pallet", SqlDbType.VarChar);
                paramsArray[0].Value = PalletNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "op_Plt_upload_to_SAP", paramsArray);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeliveryPallet> GetDeliveryPalletByDNAndPallet(string DnNo, string PalletNo)
        {
            try
            {
                IList<DeliveryPallet> ret = new List<DeliveryPallet>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Delivery_Pallet cond = new _Schema.Delivery_Pallet();
                        cond.DeliveryNo = DnNo;
                        cond.PalletNo = PalletNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Delivery_Pallet), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_DeliveryNo].Value = DnNo;
                sqlCtx.Params[_Schema.Delivery_Pallet.fn_PalletNo].Value = PalletNo;
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
                        item.PalletType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_palletType]);
                        item.DeviceQty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Delivery_Pallet.fn_deviceQty]);
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

        public IList<ForwarderInfo> GetForwarderInfoByTruckID(string truckID)
        {
            try
            {
                IList<ForwarderInfo> ret = new List<ForwarderInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Forwarder cond = new _Schema.Forwarder();
                        cond.TruckID = truckID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Forwarder), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Forwarder.fn_TruckID].Value = truckID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ForwarderInfo item = new ForwarderInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_Cdt]);
                        item.Date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_Date]);
                        item.Driver = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_Driver]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_Editor]);
                        item.Forwarder = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_forwarder]);
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_Id]);
                        item.MAWB = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_MAWB]);
                        item.TruckID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_TruckID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_Udt]);
                        item.ContainerId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Forwarder.fn_ContainerId]);
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

        public IList<PickIDCtrlInfo> GetPickIDByTruckIDAndDate(string truckID, DateTime date)
        {
            try
            {
                IList<PickIDCtrlInfo> ret = new List<PickIDCtrlInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PickIDCtrl cond = new _Schema.PickIDCtrl();
                        cond.TruckID = truckID;
                        cond.Dt = string.Format("{0}/{1}/{2}", date.Year.ToString(), date.Month.ToString().PadLeft(2,'0'), date.Day.ToString().PadLeft(2,'0'));
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PickIDCtrl), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PickIDCtrl.fn_TruckID].Value = truckID;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_Dt].Value = string.Format("{0}/{1}/{2}", date.Year.ToString(), date.Month.ToString().PadLeft(2, '0'), date.Day.ToString().PadLeft(2, '0'));
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PickIDCtrlInfo item = new PickIDCtrlInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Cdt]);
                        item.Driver = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Driver]);
                        item.Dt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Dt]);
                        item.Fwd = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Fwd]);
                        item.InDt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_InDt]);
                        item.OutDt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_OutDt]);
                        item.PickID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_PickID]);
                        item.TruckID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_TruckID]);
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Id]);
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

        public IList<MAWBInfo> GetMAWBInfoByMAWB(string MAWB)
        {
            try
            {
                IList<MAWBInfo> ret = new List<MAWBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MAWB cond = new _Schema.MAWB();
                        cond.mAWB = MAWB;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MAWB), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MAWB.fn_mAWB].Value = MAWB;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MAWBInfo item = new MAWBInfo();
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_Id]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_Cdt]);
                        item.Delivery = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_Delivery]);
                        item.MAWB = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_mAWB]);
                        item.DeclarationId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_DeclarationId]);
                        item.OceanContainer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_OceanContainer]);
                        item.HAWB = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_HAWB]);
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

        public IList<MAWBInfo> GetMAWBInfoByMAWBorHAWB(string MAWB)
        {
            try
            {
                IList<MAWBInfo> ret = new List<MAWBInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MAWB cond = new _Schema.MAWB();
                        cond.mAWB = MAWB;
                        cond.HAWB = MAWB;
                        sqlCtx = _Schema.Func.GetConditionedSelectExtOr(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MAWB), cond, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MAWB.fn_mAWB].Value = MAWB;
                sqlCtx.Params[_Schema.MAWB.fn_HAWB].Value = MAWB;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MAWBInfo item = new MAWBInfo();
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_Id]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_Cdt]);
                        item.Delivery = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_Delivery]);
                        item.MAWB = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_mAWB]);
                        item.DeclarationId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_DeclarationId]);
                        item.OceanContainer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_OceanContainer]);
                        item.HAWB = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MAWB.fn_HAWB]);
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

        public void InsertPickIDCtrl(PickIDCtrlInfo pickIDCtrlInfo)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PickIDCtrl));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PickIDCtrl.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_Driver].Value = pickIDCtrlInfo.Driver;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_Dt].Value = pickIDCtrlInfo.Dt;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_Fwd].Value = pickIDCtrlInfo.Fwd;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_InDt].Value = pickIDCtrlInfo.InDt;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_OutDt].Value = pickIDCtrlInfo.OutDt;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_PickID].Value = pickIDCtrlInfo.PickID;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_TruckID].Value = pickIDCtrlInfo.TruckID;
                pickIDCtrlInfo.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertFwdPlt(FwdPltInfo fwdPltInfo)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FwdPlt));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.FwdPlt.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.FwdPlt.fn_Date].Value = fwdPltInfo.Date;
                sqlCtx.Params[_Schema.FwdPlt.fn_Operator].Value = fwdPltInfo.Operator;
                sqlCtx.Params[_Schema.FwdPlt.fn_PickID].Value = fwdPltInfo.PickID;
                sqlCtx.Params[_Schema.FwdPlt.fn_Plt].Value = fwdPltInfo.Plt;
                sqlCtx.Params[_Schema.FwdPlt.fn_Qty].Value = fwdPltInfo.Qty;
                sqlCtx.Params[_Schema.FwdPlt.fn_Status].Value = fwdPltInfo.Status;
                sqlCtx.Params[_Schema.FwdPlt.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PickIDCtrlInfo> GetPickIDCtrlInfoByPickIDAndDate(string pickID, DateTime date)
        {
            try
            {
                IList<PickIDCtrlInfo> ret = new List<PickIDCtrlInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PickIDCtrl cond = new _Schema.PickIDCtrl();
                        cond.PickID = pickID;
                        cond.Dt = string.Format("{0}/{1}/{2}", date.Year.ToString(), date.Month.ToString().PadLeft(2, '0'), date.Day.ToString().PadLeft(2, '0'));
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PickIDCtrl), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PickIDCtrl.fn_PickID].Value = pickID;
                sqlCtx.Params[_Schema.PickIDCtrl.fn_Dt].Value = string.Format("{0}/{1}/{2}", date.Year.ToString(), date.Month.ToString().PadLeft(2, '0'), date.Day.ToString().PadLeft(2, '0'));
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PickIDCtrlInfo item = new PickIDCtrlInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Cdt]);
                        item.Driver = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Driver]);
                        item.Dt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Dt]);
                        item.Fwd = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Fwd]);
                        item.InDt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_InDt]);
                        item.OutDt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_OutDt]);
                        item.PickID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_PickID]);
                        item.TruckID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_TruckID]);
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Id]);
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

        public IList<FwdPltInfo> GetFwdPltInfosByPickIDAndStatusAndDate(string pickID, string status, DateTime date)
        {
            try
            {
                IList<FwdPltInfo> ret = new List<FwdPltInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FwdPlt cond = new _Schema.FwdPlt();
                        cond.PickID = pickID;
                        cond.Status = status;
                        cond.Date = string.Format("{0}/{1}/{2}", date.Year.ToString(), date.Month.ToString().PadLeft(2, '0'), date.Day.ToString().PadLeft(2, '0'));
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FwdPlt), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FwdPlt.fn_PickID].Value = pickID;
                sqlCtx.Params[_Schema.FwdPlt.fn_Status].Value = status;
                sqlCtx.Params[_Schema.FwdPlt.fn_Date].Value = string.Format("{0}/{1}/{2}", date.Year.ToString(), date.Month.ToString().PadLeft(2, '0'), date.Day.ToString().PadLeft(2, '0'));
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FwdPltInfo item = new FwdPltInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_Cdt]);
                        item.Date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_Date]);
                        item.Operator = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_Operator]);
                        item.PickID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_PickID]);
                        item.Plt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_Plt]);
                        item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_Qty]);
                        item.Status = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_Status]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.FwdPlt.fn_Udt]);
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

        public void UpdateFwdPltStatus(string pickID, string pltNo, string status, string editor)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FwdPlt cond = new _Schema.FwdPlt();
                        cond.PickID = pickID;
                        cond.Plt = pltNo;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FwdPlt), new List<string>() { _Schema.FwdPlt.fn_Status, _Schema.FwdPlt.fn_Operator }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx.Params[_Schema.FwdPlt.fn_PickID].Value = pickID;
                sqlCtx.Params[_Schema.FwdPlt.fn_Plt].Value = pltNo;

                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FwdPlt.fn_Status)].Value = status;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FwdPlt.fn_Operator)].Value = editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FwdPlt.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPalletId(PalletIdInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletId));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PalletId.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletId.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PalletId.fn_palletId].Value = item.PalletId;
                sqlCtx.Params[_Schema.PalletId.fn_PalletNo].Value = item.PalletNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int RemoveFwdPltByPickID(string pickID)
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
                        mtns::FwdPlt cond = new mtns::FwdPlt();
                        cond.pickID = pickID;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::FwdPlt>(tk, new ConditionCollection<mtns::FwdPlt>(new EqualCondition<mtns::FwdPlt>(cond)));
                    }
                }
                sqlCtx.Param(mtns::FwdPlt.fn_pickID).Value = pickID;

                return _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PickIDCtrlInfo> GetPickIDCtrlInfoByPickID(string pickID)
        {
            try
            {
                IList<PickIDCtrlInfo> ret = new List<PickIDCtrlInfo>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PickIDCtrl cond = new _Schema.PickIDCtrl();
                        cond.PickID = pickID;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PickIDCtrl), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PickIDCtrl.fn_PickID].Value = pickID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PickIDCtrlInfo item = new PickIDCtrlInfo();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Cdt]);
                        item.Driver = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Driver]);
                        item.Dt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Dt]);
                        item.Fwd = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Fwd]);
                        item.InDt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_InDt]);
                        item.OutDt = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_OutDt]);
                        item.PickID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_PickID]);
                        item.TruckID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_TruckID]);
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PickIDCtrl.fn_Id]);
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
        /// 根据fullQty取得表PalletStatndard的相应记录where FullQty= fullQty
        /// </summary>
        /// <param name="fullQty"></param>
        /// <returns></returns>
        //IList<PalletQtyInfo> GetPalletByFullQty(int fullQty);

        /// <summary>
        /// 使用Pallet No 查询IMES_PAK..Delivery_Pallet 表，得到Shipment，然后按照如下方法确定是否需要提示用户Scan 2D Barcode
        ///SELECT @setnum=DOC_SET_NUMBER FROM HP_EDI..[PAK.PAKComn] (NOLOCK) WHERE InternalID=@shipment 
        ///IF EXISTS (SELECT * FROM HP_EDI..[PAK.PAKRT] 
        /// WHERE DOC_SET_NUMBER=@setnum AND XSL_TEMPLATE_NAME like '%Verizon2D%' 
        /// AND DOC_CAT='Pallet Ship Label- Pack ID Single' )
        /// BEGIN
        ///     SELECT '1'	
        /// END
        ///ELSE 
        /// BEGIN 
        ///     SELECT '0'	
        /// END
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        public bool IsScan2DBarCode(string palletNo)
        {
            return false;
        }

        #endregion

        #region Defered

        public void UpdatePltWeightToSAPDefered(IUnitOfWork uow, string PalletNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), PalletNo);
        }

        public void InsertPickIDCtrlDefered(IUnitOfWork uow, PickIDCtrlInfo pickIDCtrlInfo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pickIDCtrlInfo);
        }

        public void InsertFwdPltDefered(IUnitOfWork uow, FwdPltInfo fwdPltInfo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), fwdPltInfo);
        }

        public void UpdateFwdPltStatusDefered(IUnitOfWork uow, string pickID, string pltNo, string status, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pickID, pltNo, status, editor);
        }

        public void InsertPalletIdDefered(IUnitOfWork uow, PalletIdInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #region . Inners .

        private void PersistInsertPallet(IMES.FisObject.PAK.Pallet.Pallet item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Pallet.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Pallet.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Pallet.fn_Height].Value = item.Height;
                sqlCtx.Params[_Schema.Pallet.fn_Length].Value = item.Length;
                sqlCtx.Params[_Schema.Pallet.fn_PalletModel].Value = item.PalletModel;
                sqlCtx.Params[_Schema.Pallet.fn_PalletNo].Value = item.PalletNo;
                sqlCtx.Params[_Schema.Pallet.fn_Station].Value = item.Station;
                sqlCtx.Params[_Schema.Pallet.fn_UCC].Value = item.UCC;
                sqlCtx.Params[_Schema.Pallet.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Pallet.fn_Weight].Value = item.Weight;
                sqlCtx.Params[_Schema.Pallet.fn_Width].Value = item.Width;
                sqlCtx.Params[_Schema.Pallet.fn_weight_L].Value = item.Weight_L;
                sqlCtx.Params[_Schema.Pallet.fn_Floor].Value = item.Floor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePallet(IMES.FisObject.PAK.Pallet.Pallet item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Pallet.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Pallet.fn_Height].Value = item.Height;
                sqlCtx.Params[_Schema.Pallet.fn_Length].Value = item.Length;
                sqlCtx.Params[_Schema.Pallet.fn_PalletModel].Value = item.PalletModel;
                sqlCtx.Params[_Schema.Pallet.fn_PalletNo].Value = item.PalletNo;
                sqlCtx.Params[_Schema.Pallet.fn_Station].Value = item.Station;
                sqlCtx.Params[_Schema.Pallet.fn_UCC].Value = item.UCC;
                sqlCtx.Params[_Schema.Pallet.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Pallet.fn_Weight].Value = item.Weight;
                sqlCtx.Params[_Schema.Pallet.fn_Width].Value = item.Width;
                sqlCtx.Params[_Schema.Pallet.fn_weight_L].Value = item.Weight_L;
                sqlCtx.Params[_Schema.Pallet.fn_Floor].Value = item.Floor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePallet(IMES.FisObject.PAK.Pallet.Pallet item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet));
                    }
                }
                sqlCtx.Params[_Schema.Pallet.fn_PalletNo].Value = item.PalletNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePallet(string palletNo)
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
                        mtns::Pallet cond = new mtns::Pallet();
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pallet>(tk, new ConditionCollection<mtns::Pallet>(new EqualCondition<mtns::Pallet>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pallet.fn_palletNo).Value = palletNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePalletAttr(string palletNo)
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
                        mtns::PalletAttr cond = new mtns::PalletAttr();
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::PalletAttr>(tk, new ConditionCollection<mtns::PalletAttr>(new EqualCondition<mtns::PalletAttr>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PalletAttr.fn_palletNo).Value = palletNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckAndInsertSubs(IMES.FisObject.PAK.Pallet.Pallet item, StateTracker tracker)
        {
            IList<fons.PalletLog> lstLog = (IList<fons.PalletLog>)item.GetType().GetField("_palletLogs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstLog != null && lstLog.Count > 0)
            {
                foreach (fons.PalletLog log in lstLog)
                {
                    if (tracker.GetState(log) == DataRowState.Added)
                    {
                        log.PalletNo = item.PalletNo;
                        this.PersistInsertPalletLog(log);
                    }
                }
            }
        }

        private void PersistInsertPalletLog(fons.PalletLog log)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PalletLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletLog.fn_Editor].Value = log.Editor;
                sqlCtx.Params[_Schema.PalletLog.fn_Line].Value = log.Line;
                sqlCtx.Params[_Schema.PalletLog.fn_PalletNo].Value = log.PalletNo;
                sqlCtx.Params[_Schema.PalletLog.fn_Station].Value = log.Station;
                log.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . For Maintain  .

        public IList<PalletWeight> GetAllPalletWeightByFamily(string family)
        {
            try
            {
                IList<PalletWeight> ret = new List<PalletWeight>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PalletWeight cond = new _Schema.PalletWeight();
                        cond.Family = family;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PalletWeight.fn_Region);
                    }
                }
                sqlCtx.Params[_Schema.PalletWeight.fn_Family].Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletWeight item = new PalletWeight();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Editor]);
                        item.FamilyID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Family]);
                        item.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_ID]);
                        item.Qty = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Qty]);
                        item.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Region]);
                        item.Tolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Tolerance]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Udt]);
                        item.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Weight]);
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

        public void DeletePalletWeightByID(int id)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight));
                    }
                }
                sqlCtx.Params[_Schema.PalletWeight.fn_ID].Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPalletWeight(PalletWeight palletWeight)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PalletWeight.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletWeight.fn_Editor].Value = palletWeight.Editor;
                sqlCtx.Params[_Schema.PalletWeight.fn_Family].Value = palletWeight.FamilyID;
                sqlCtx.Params[_Schema.PalletWeight.fn_Qty].Value = palletWeight.Qty;
                sqlCtx.Params[_Schema.PalletWeight.fn_Region].Value = palletWeight.Region;
                sqlCtx.Params[_Schema.PalletWeight.fn_Tolerance].Value = palletWeight.Tolerance;
                sqlCtx.Params[_Schema.PalletWeight.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletWeight.fn_Weight].Value = palletWeight.Weight;
                palletWeight.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                palletWeight.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePalletWeight(PalletWeight palletWeight)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight));
                    }
                }
                sqlCtx.Params[_Schema.PalletWeight.fn_ID].Value = palletWeight.Id;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PalletWeight.fn_Editor].Value = palletWeight.Editor;
                sqlCtx.Params[_Schema.PalletWeight.fn_Family].Value = palletWeight.FamilyID;
                sqlCtx.Params[_Schema.PalletWeight.fn_Qty].Value = palletWeight.Qty;
                sqlCtx.Params[_Schema.PalletWeight.fn_Region].Value = palletWeight.Region;
                sqlCtx.Params[_Schema.PalletWeight.fn_Tolerance].Value = palletWeight.Tolerance;
                sqlCtx.Params[_Schema.PalletWeight.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletWeight.fn_Weight].Value = palletWeight.Weight;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                palletWeight.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IFPalletWeightIsExists(string family, string region)
        {
            //select * from PalletWeight where Family=? and Region=?
            try
            {
                bool ret = false;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PalletWeight cond = new _Schema.PalletWeight();
                        cond.Family = family;
                        cond.Region = region;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight), "COUNT", new List<string>() { _Schema.PalletWeight.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PalletWeight.fn_Family].Value = family;
                sqlCtx.Params[_Schema.PalletWeight.fn_Region].Value = region;
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

        /// <summary>
        /// 添加一条Forwarder
        /// id被写回在item里面返回
        /// </summary>
        /// <param name="item"></param>
        public void AddForwarder(ForwarderInfo item)
        {
            try
            {
                InsertForwarder_Inner(item);
            }
            catch (Exception)
            {
                throw;
            }
        }
 
        /// <summary>
        /// 取得查询列表
        /// SELECT [Date], [Forwarder],[MAWB],[Driver],[TruckID],[Editor],CONVERT(char(10), [Cdt], 21) as [UploadDate],CONVERT(char(10), [Udt], 21) as Udt, Id  
        ///         FROM [Forwarder]
        ///         WHERE CONVERT(char(10), Cdt, 21) BETWEEN @StartDate AND @EndDate
        ///         ORDER BY [Cdt]
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable GetForwarderList(string startDate, string endDate)
        {
            DataTable ret = null;

            string paramName1 = "@StartDate";
            string paramName2 = "@EndDate";

            _Schema.SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    sqlCtx = new _Schema.SQLContext();
                    sqlCtx.Sentence =   "SELECT {1},{2},{3},{4},{5},{6},CONVERT(CHAR(10), {7}, 21) AS UploadDate, CONVERT(CHAR(10), {8}, 21) AS Udt, {9},{12} " +
                                        "FROM {0} " +
                                        "WHERE CONVERT(CHAR(10), {7}, 21) BETWEEN {10} AND {11} " +
                                        "ORDER BY {7}";

                    sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Forwarder).Name,
                                                                        _Schema.Forwarder.fn_Date,
                                                                        _Schema.Forwarder.fn_forwarder,
                                                                        _Schema.Forwarder.fn_MAWB,
                                                                        _Schema.Forwarder.fn_Driver,
                                                                        _Schema.Forwarder.fn_TruckID,
                                                                        _Schema.Forwarder.fn_Editor,
                                                                        _Schema.Forwarder.fn_Cdt,
                                                                        _Schema.Forwarder.fn_Udt,
                                                                        _Schema.Forwarder.fn_Id,
                                                                        paramName1,
                                                                        paramName2,
                                                                        _Schema.Forwarder.fn_ContainerId);

                    sqlCtx.Params.Add(paramName1, new SqlParameter(paramName1, SqlDbType.VarChar));
                    sqlCtx.Params.Add(paramName2, new SqlParameter(paramName2, SqlDbType.VarChar));

                    _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                }
            }

            sqlCtx.Params[paramName1].Value = startDate;
            sqlCtx.Params[paramName2].Value = endDate;

            ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

            return ret;
        }

        /// <summary>
        /// 更新Forwarder
        /// UPDATE [Forwarder]
        ///  SET [Driver] = @NewDriver,
        ///  [TruckID] = @NewTruckID,
        ///  [Editor] = @Editor,
        ///  [Udt] = GETDATE()
        ///  WHERE [Id] = @Id
        /// </summary>
        /// <param name="item"></param>
        public void UpdateForwarder(ForwarderInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Forwarder cond = new _Schema.Forwarder();
                        cond.Id = item.Id;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Forwarder), new List<string>() { _Schema.Forwarder.fn_Driver, _Schema.Forwarder.fn_TruckID, _Schema.Forwarder.fn_Editor, _Schema.Forwarder.fn_ContainerId}, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Forwarder.fn_Id].Value = item.Id;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Forwarder.fn_Driver)].Value = item.Driver;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Forwarder.fn_TruckID)].Value = item.TruckID;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Forwarder.fn_Editor)].Value = item.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Forwarder.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.Forwarder.fn_ContainerId)].Value = item.ContainerId;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除数据
        /// DELETE FROM [IMES_PAK].[dbo].[Forwarder] WHERE [Forwarder]=@Id
        /// </summary>
        /// <param name="item"></param>
        public void DeleteForwarder(ForwarderInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Forwarder));
                    }
                }
                sqlCtx.Params[_Schema.Forwarder.fn_Id].Value = item.Id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得存在的Forwarder相关数据
        /// SELECT Id FROM [Forwarder]
        /// WHERE [Forwarder]=@Forwarder AND [Date]=@Date AND [MAWB]=@MAWB AND [Driver]=@Driver AND [TruckID]=@TruckID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataTable GetExistForwarder(ForwarderInfo item)
        {
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Forwarder cond = new _Schema.Forwarder();
                        cond.forwarder = item.Forwarder;
                        cond.Date = item.Date;
                        cond.MAWB = item.MAWB;
                        cond.Driver = item.Driver;
                        cond.TruckID = item.TruckID;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Forwarder), null, new List<string>() { _Schema.Forwarder.fn_Id }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Forwarder.fn_forwarder].Value = item.Forwarder;
                sqlCtx.Params[_Schema.Forwarder.fn_Date].Value = item.Date;
                sqlCtx.Params[_Schema.Forwarder.fn_MAWB].Value = item.MAWB;
                sqlCtx.Params[_Schema.Forwarder.fn_Driver].Value = item.Driver;
                sqlCtx.Params[_Schema.Forwarder.fn_TruckID].Value = item.TruckID;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ImportForwarders(IList<ForwarderInfo> items)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (items != null)
                {
                    foreach (ForwarderInfo item in items)
                    {
                        if (!PeekForwarder(item))
                            InsertForwarder_Inner(item);
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

        private bool PeekForwarder(ForwarderInfo item)
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
                        _Schema.Forwarder cond = new _Schema.Forwarder();
                        cond.forwarder = item.Forwarder;
                        cond.Date = item.Date;
                        cond.MAWB = item.MAWB;
                        cond.Driver = item.Driver;
                        cond.TruckID = item.TruckID;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Forwarder), "COUNT", new List<string>() { _Schema.Forwarder.fn_Id }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.Forwarder.fn_forwarder].Value = item.Forwarder;
                sqlCtx.Params[_Schema.Forwarder.fn_Date].Value = item.Date;
                sqlCtx.Params[_Schema.Forwarder.fn_MAWB].Value = item.MAWB;
                sqlCtx.Params[_Schema.Forwarder.fn_Driver].Value = item.Driver;
                sqlCtx.Params[_Schema.Forwarder.fn_TruckID].Value = item.TruckID;

                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    ret = cnt > 0 ? true : false;
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

        private void InsertForwarder_Inner(ForwarderInfo item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Forwarder));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Forwarder.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.Forwarder.fn_Date].Value = item.Date;
                sqlCtx.Params[_Schema.Forwarder.fn_Driver].Value = item.Driver;
                sqlCtx.Params[_Schema.Forwarder.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.Forwarder.fn_forwarder].Value = item.Forwarder;
                sqlCtx.Params[_Schema.Forwarder.fn_MAWB].Value = item.MAWB;
                sqlCtx.Params[_Schema.Forwarder.fn_TruckID].Value = item.TruckID;
                sqlCtx.Params[_Schema.Forwarder.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.Forwarder.fn_ContainerId].Value = item.ContainerId;

                item.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePalletIDByPalletNo(string palletNo )
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletId));
                    }
                }
                sqlCtx.Params[_Schema.PalletId.fn_PalletNo].Value = palletNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Defered

        public void DeletePalletWeightByIDDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddPalletWeightDefered(IUnitOfWork uow, PalletWeight palletWeight)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletWeight);
        }

        public void UpdatePalletWeightDefered(IUnitOfWork uow, PalletWeight palletWeight)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletWeight);
        }

        public void AddForwarderDefered(IUnitOfWork uow, ForwarderInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateForwarderDefered(IUnitOfWork uow, ForwarderInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteForwarderDefered(IUnitOfWork uow, ForwarderInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void ImportForwardersDefered(IUnitOfWork uow, IList<ForwarderInfo> items)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), items);
        }

        #endregion

        #endregion

        public IList<ChepPalletInfo> GetChepPalletList()
        {
            try
            {
                IList<ChepPalletInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<ChepPallet>(tk, ChepPallet.fn_palletNo);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ChepPallet, ChepPalletInfo, ChepPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ChepPalletInfo GetChepPalletInfo(string chepPalletNo)
        {
            try
            {
                ChepPalletInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        ChepPallet cond = new ChepPallet();
                        cond.palletNo = chepPalletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<ChepPallet>(tk, null, null, new ConditionCollection<ChepPallet>(new EqualCondition<ChepPallet>(cond)));
                    }
                }
                sqlCtx.Param(ChepPallet.fn_palletNo).Value = chepPalletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ChepPallet, ChepPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddGetChepPalletInfo(ChepPalletInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<ChepPallet>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<ChepPallet, ChepPalletInfo>(sqlCtx, item);

                sqlCtx.Param(ChepPallet.fn_cdt).Value = cmDt;
                sqlCtx.Param(ChepPallet.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteChepPalletInfo(int id)
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
                        ChepPallet cond = new ChepPallet();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<ChepPallet>(tk, new ConditionCollection<ChepPallet>(new EqualCondition<ChepPallet>(cond)));
                    }
                }
                sqlCtx.Param(ChepPallet.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PalletQtyInfo> GetQtyInfoList()
        {
            try
            {
                IList<PalletQtyInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<PalletStatndard>(tk, PalletStatndard.fn_fullQty);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<PalletStatndard, PalletQtyInfo, PalletQtyInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PalletQtyInfo GetQtyInfo(int id)
        {
            try
            {
                PalletQtyInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        PalletStatndard cond = new PalletStatndard();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedSelect<PalletStatndard>(tk, null, null, new ConditionCollection<PalletStatndard>(new EqualCondition<PalletStatndard>(cond)));
                    }
                }
                sqlCtx.Param(PalletStatndard.fn_id).Value = id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<PalletStatndard, PalletQtyInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddQtyInfo(PalletQtyInfo pqInfo)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<PalletStatndard>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<PalletStatndard, PalletQtyInfo>(sqlCtx, pqInfo);

                sqlCtx.Param(PalletStatndard.fn_cdt).Value = cmDt;
                sqlCtx.Param(PalletStatndard.fn_udt).Value = cmDt;

                pqInfo.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteQtyInfo(int id)
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
                        PalletStatndard cond = new PalletStatndard();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<PalletStatndard>(tk, new ConditionCollection<PalletStatndard>(new EqualCondition<PalletStatndard>(cond)));
                    }
                }
                sqlCtx.Param(PalletStatndard.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateQtyInfo(PalletQtyInfo pqInfo, int id)
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
                        PalletStatndard cond = new PalletStatndard();
                        cond.id = id;
                        PalletStatndard setv = FuncNew.SetColumnFromField<PalletStatndard, PalletQtyInfo>(pqInfo, PalletStatndard.fn_id);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<PalletStatndard>(tk, new SetValueCollection<PalletStatndard>(new CommonSetValue<PalletStatndard>(setv)), new ConditionCollection<PalletStatndard>(new EqualCondition<PalletStatndard>(cond)));
                    }
                }
                sqlCtx.Param(PalletStatndard.fn_id).Value = id;

                sqlCtx = FuncNew.SetColumnFromField<PalletStatndard, PalletQtyInfo>(sqlCtx, pqInfo, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(PalletStatndard.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateStatusForPakBtLocMas(string status, string snoId)
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
                        Pak_Btlocmas cond = new Pak_Btlocmas();
                        cond.snoId = snoId;

                        Pak_Btlocmas setv = new Pak_Btlocmas();
                        setv.status = status;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Btlocmas>(tk, new SetValueCollection<Pak_Btlocmas>(new CommonSetValue<Pak_Btlocmas>(setv)), new ConditionCollection<Pak_Btlocmas>(new EqualCondition<Pak_Btlocmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Btlocmas.fn_snoId).Value = snoId;
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_status)).Value = status;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateStatusForPakBtLocMas(string status, string snoId, string editor)
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
                        Pak_Btlocmas cond = new Pak_Btlocmas();
                        cond.snoId = snoId;

                        Pak_Btlocmas setv = new Pak_Btlocmas();
                        setv.status = status;
                        setv.editor = editor;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Btlocmas>(tk, new SetValueCollection<Pak_Btlocmas>(new CommonSetValue<Pak_Btlocmas>(setv)), new ConditionCollection<Pak_Btlocmas>(new EqualCondition<Pak_Btlocmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Btlocmas.fn_snoId).Value = snoId;
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_status)).Value = status;
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_editor)).Value = editor;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakBtLocMasInfo> GetPakBtLocMasInfos(string model, string status)
        {
            try
            {
                IList<PakBtLocMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Btlocmas cond = new Pak_Btlocmas();
                        cond.model = model;
                        cond.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Btlocmas>(tk, null, null, new ConditionCollection<Pak_Btlocmas>(new EqualCondition<Pak_Btlocmas>(cond)), Pak_Btlocmas.fn_snoId);
                    }
                }
                sqlCtx.Param(Pak_Btlocmas.fn_model).Value = model;
                sqlCtx.Param(Pak_Btlocmas.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Btlocmas, PakBtLocMasInfo, PakBtLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertSnoDetBtLocInfo(SnoDetBtLocInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<SnoDet_BTLoc>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<SnoDet_BTLoc, SnoDetBtLocInfo>(sqlCtx, item);

                sqlCtx.Param(SnoDet_BTLoc.fn_cdt).Value = cmDt;
                sqlCtx.Param(SnoDet_BTLoc.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SnoDetBtLocInfo> GetSnoDetBtLocInfosByCondition(SnoDetBtLocInfo condition)
        {
            try
            {
                IList<SnoDetBtLocInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                        SnoDet_BTLoc cond = FuncNew.SetColumnFromField<SnoDet_BTLoc, SnoDetBtLocInfo>(condition);
                        sqlCtx = FuncNew.GetConditionedSelect<SnoDet_BTLoc>(null, null, new ConditionCollection<SnoDet_BTLoc>(new EqualCondition<SnoDet_BTLoc>(cond)), SnoDet_BTLoc.fn_id);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<SnoDet_BTLoc, SnoDetBtLocInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<SnoDet_BTLoc, SnoDetBtLocInfo, SnoDetBtLocInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateForIncPakBtLocMas(string snoId, string editor)
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
                        Pak_Btlocmas cond = new Pak_Btlocmas();
                        cond.snoId = snoId;
                        Pak_Btlocmas setv = new Pak_Btlocmas();
                        setv.cmbQty = 1;
                        Pak_Btlocmas setv2 = new Pak_Btlocmas();
                        setv2.udt = DateTime.Now;
                        setv2.editor = editor;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Btlocmas>(tk, new SetValueCollection<Pak_Btlocmas>(new ForIncSetValueSelf<Pak_Btlocmas>(setv), new CommonSetValue<Pak_Btlocmas>(setv2)), new ConditionCollection<Pak_Btlocmas>(new EqualCondition<Pak_Btlocmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Btlocmas.fn_snoId).Value = snoId;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_editor)).Value = editor;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateForIncPakBtLocMas(string snoId)
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
                        Pak_Btlocmas cond = new Pak_Btlocmas();
                        cond.snoId = snoId;
                        Pak_Btlocmas setv = new Pak_Btlocmas();
                        setv.cmbQty = 1;
                        Pak_Btlocmas setv2 = new Pak_Btlocmas();
                        setv2.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Btlocmas>(tk, new SetValueCollection<Pak_Btlocmas>(new ForIncSetValueSelf<Pak_Btlocmas>(setv), new CommonSetValue<Pak_Btlocmas>(setv2)), new ConditionCollection<Pak_Btlocmas>(new EqualCondition<Pak_Btlocmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Btlocmas.fn_snoId).Value = snoId;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Btlocmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetPalletNoByUcc(string ucc)
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
                        _Metas.Pallet cond = new _Metas.Pallet();
                        cond.ucc = ucc;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pallet>(tk, null, new string[] { _Metas.Pallet.fn_palletNo }, new ConditionCollection<_Metas.Pallet>(new EqualCondition<_Metas.Pallet>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pallet.fn_ucc).Value = ucc;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Pallet.fn_palletNo));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTierQtyFromPalletQtyInfo(string fullQty)
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
                        PalletStatndard cond = new PalletStatndard();
                        cond.fullQty = fullQty;
                        sqlCtx = FuncNew.GetConditionedSelect<PalletStatndard>(tk, null, new string[] { PalletStatndard.fn_tierQty }, new ConditionCollection<PalletStatndard>(new EqualCondition<PalletStatndard>(cond)));
                    }
                }
                sqlCtx.Param(PalletStatndard.fn_fullQty).Value = fullQty;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes(PalletStatndard.fn_tierQty));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string[]> GetInfoTypeInfoValuePairsFromDeliveryInfo(string palletNo)
        {
            try
            {
                IList<string[]> ret = null;

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
                        _Metas.Delivery_Pallet cond = new Delivery_Pallet();
                        cond.palletNo = palletNo;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Delivery_Pallet>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.DeliveryInfo>();
                        tf2.AddRangeToGetFieldNames(_Metas.DeliveryInfo.fn_infoType, _Metas.DeliveryInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Delivery_Pallet, _Metas.DeliveryInfo>(tf1, _Metas.Delivery_Pallet.fn_deliveryNo, tf2, _Metas.DeliveryInfo.fn_deliveryNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);
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
                        ret = new List<string[]>();
                        while (sqlR.Read())
                        {
                            string[] item = new string[]
                            { 
                                g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns.DeliveryInfo.fn_infoType))),
                                g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, mtns.DeliveryInfo.fn_infoValue)))
                            };
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

        public PakBtLocMasInfo GetPakBtLocMasInfo(string model, string status)
        {
            try
            {
                PakBtLocMasInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Btlocmas cond = new Pak_Btlocmas();
                        cond.model = model;
                        cond.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Btlocmas>(tk, "TOP 1", null, new ConditionCollection<Pak_Btlocmas>(new EqualCondition<Pak_Btlocmas>(cond)), Pak_Btlocmas.fn_snoId);
                    }
                }
                sqlCtx.Param(Pak_Btlocmas.fn_model).Value = model;
                sqlCtx.Param(Pak_Btlocmas.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Btlocmas, PakBtLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetQtyOfPackedPalletToday()
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
                        _Metas.Pallet cond = new _Metas.Pallet();
                        cond.udt = DateTime.Now;

                        _Metas.Pallet cond2 = new _Metas.Pallet();
                        cond2.weight = 0;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pallet>(tk, "COUNT", new string[] { _Metas.Pallet.fn_palletNo }, new ConditionCollection<_Metas.Pallet>(new GreaterOrEqualCondition<_Metas.Pallet>(cond, null, "CONVERT(CHAR(10),{0},111)"), new NotNullCondition<_Metas.Pallet>(cond2)));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecGE(_Metas.Pallet.fn_udt)).Value = new DateTime(cmDt.Year, cmDt.Month, cmDt.Day);

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

        public IList<PalletQtyInfo> GetPalletByFullQty(string fullQty)
        {
            try
            {
                IList<PalletQtyInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        PalletStatndard cond = new PalletStatndard();
                        cond.fullQty = fullQty;
                        sqlCtx = FuncNew.GetConditionedSelect<PalletStatndard>(tk, null, null, new ConditionCollection<PalletStatndard>(new EqualCondition<PalletStatndard>(cond)), PalletStatndard.fn_fullQty);
                    }
                }
                sqlCtx.Param(PalletStatndard.fn_fullQty).Value = fullQty;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<PalletStatndard, PalletQtyInfo, PalletQtyInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UnPackPakOdmSessionByDeliveryNo(string dn)
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

                        //sqlCtx.Sentence = "DELETE {0} FROM {0} O " +
                        //                    "INNER JOIN {1}..{2} AS P ON O.{3}=P.{4} " +
                        //                    "WHERE P.{5}=@{5} AND (P.{6} LIKE 'PC%' OR P.{6} LIKE 'QC%') ";


                        sqlCtx.Sentence = "DELETE {0} FROM {0} O " +
                                      "INNER JOIN {1}..{2} AS P ON O.{3}=P.{4} " +
                                      "WHERE P.{5}=@{5} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(_Metas.Pakodmsession)),
                                                                         _Schema.SqlHelper.DB_FA,
                                                                         ToolsNew.GetTableName(typeof(_Metas.Product)),
                                                                         _Metas.Pakodmsession.fn_serial_num,
                                                                         _Metas.Product.fn_custsn,
                                                                         _Metas.Product.fn_deliveryNo,
                                                                         _Metas.Product.fn_model);

                        sqlCtx.AddParam(_Metas.Product.fn_deliveryNo, new SqlParameter("@" + _Metas.Product.fn_deliveryNo, ToolsNew.GetDBFieldType<_Metas.Product>(_Metas.Product.fn_deliveryNo)));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(_Metas.Product.fn_deliveryNo).Value = dn;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
             }
            catch (Exception)
            {
                throw;
            }
        }

        public void UnPackPackingDataByDeliveryNo(string dn)
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
                        mtns::Pak_Packkingdata cond = new mtns::Pak_Packkingdata();
                        cond.internalID = dn;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pak_Packkingdata>(tk, new ConditionCollection<mtns::Pak_Packkingdata>(new EqualCondition<mtns::Pak_Packkingdata>(cond)));
                    }
                }
                sqlCtx.Param(mtns::PakDotpakpaltno.fn_internalID).Value = dn;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfDummyShipDetByPlt(string plt)
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
                        Dummy_ShipDet cond = new Dummy_ShipDet();
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<Dummy_ShipDet>(tk, "COUNT", new string[] { Dummy_ShipDet.fn_snoId }, new ConditionCollection<Dummy_ShipDet>(new EqualCondition<Dummy_ShipDet>(cond)));
                    }
                }
                sqlCtx.Param(Dummy_ShipDet.fn_plt).Value = plt;

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

        public void InsertWhPltLog(WhPltLogInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Wh_Pltlog>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Wh_Pltlog, WhPltLogInfo>(sqlCtx, item);

                sqlCtx.Param(Wh_Pltlog.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateWhPltMas(WhPltMasInfo item, string plt)
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
                        Wh_Pltmas cond = new Wh_Pltmas();
                        cond.plt = plt;
                        Wh_Pltmas setv = FuncNew.SetColumnFromField<Wh_Pltmas, WhPltMasInfo>(item, Wh_Pltmas.fn_id, Wh_Pltmas.fn_cdt, Wh_Pltmas.fn_plt);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Wh_Pltmas>(tk, new SetValueCollection<Wh_Pltmas>(new CommonSetValue<Wh_Pltmas>(setv)), new ConditionCollection<Wh_Pltmas>(new EqualCondition<Wh_Pltmas>(cond)));
                    }
                }
                sqlCtx.Param(Wh_Pltmas.fn_plt).Value = plt;

                sqlCtx = FuncNew.SetColumnFromField<Wh_Pltmas, WhPltMasInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Wh_Pltmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public WhPltMasInfo GetWHPltMas(string palletNo)
        {
            try
            {
                WhPltMasInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Wh_Pltmas cond = new Wh_Pltmas();
                        cond.plt = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Wh_Pltmas>(tk, "TOP 1", null, new ConditionCollection<Wh_Pltmas>(new EqualCondition<Wh_Pltmas>(cond)), Wh_Pltmas.fn_udt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Wh_Pltmas.fn_plt).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Wh_Pltmas, WhPltMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertWhPltMas(WhPltMasInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Wh_Pltmas>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Wh_Pltmas, WhPltMasInfo>(sqlCtx, item);

                sqlCtx.Param(Wh_Pltmas.fn_cdt).Value = cmDt;
                sqlCtx.Param(Wh_Pltmas.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DummyShipDetInfo> GetDummyShipDetListByPlt(string plt)
        {
            try
            {
                IList<DummyShipDetInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Dummy_ShipDet cond = new Dummy_ShipDet();
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<Dummy_ShipDet>(tk, null, null, new ConditionCollection<Dummy_ShipDet>(new EqualCondition<Dummy_ShipDet>(cond)), Dummy_ShipDet.fn_snoId);
                    }
                }
                sqlCtx.Param(Dummy_ShipDet.fn_plt).Value = plt;
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

        public IList<PakWhPltTypeInfo> GetPakWhPltTypeListByPlt(string plt)
        {
            try
            {
                IList<PakWhPltTypeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Whplt_Type cond = new Pak_Whplt_Type();
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Whplt_Type>(tk, null, null, new ConditionCollection<Pak_Whplt_Type>(new EqualCondition<Pak_Whplt_Type>(cond)), Pak_Whplt_Type.fn_id);
                    }
                }
                sqlCtx.Param(Pak_Whplt_Type.fn_plt).Value = plt;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Whplt_Type, PakWhPltTypeInfo, PakWhPltTypeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakWhPltTypeByPlt(string plt)
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
                        mtns::Pak_Whplt_Type cond = new mtns::Pak_Whplt_Type();
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pak_Whplt_Type>(tk, new ConditionCollection<mtns::Pak_Whplt_Type>(new EqualCondition<mtns::Pak_Whplt_Type>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Whplt_Type.fn_plt).Value = plt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPakWhPltTypeInfo(PakWhPltTypeInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pak_Whplt_Type>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Pak_Whplt_Type, PakWhPltTypeInfo>(sqlCtx, item);

                sqlCtx.Param(Pak_Whplt_Type.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakWhLocMasInfo> GetPakWhLocMasListByBolAndPlt1(string bol, string plt1)
        {
            try
            {
                IList<PakWhLocMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.bol = bol;
                        cond.plt1 = plt1;
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Wh_Locmas>(tk, null, null, new ConditionCollection<Pak_Wh_Locmas>(new EqualCondition<Pak_Wh_Locmas>(cond)), Pak_Wh_Locmas.fn_loc + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Pak_Wh_Locmas.fn_bol).Value = bol;
                sqlCtx.Param(Pak_Wh_Locmas.fn_plt1).Value = plt1;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Wh_Locmas, PakWhLocMasInfo, PakWhLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPakWhPltType(string bol, string tp)
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
                        Pak_Whplt_Type cond = new Pak_Whplt_Type();
                        cond.bol = bol;
                        cond.tp = tp;
                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Whplt_Type>(tk, "COUNT", new string[] { Pak_Whplt_Type.fn_id }, new ConditionCollection<Pak_Whplt_Type>(new EqualCondition<Pak_Whplt_Type>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Whplt_Type.fn_bol).Value = bol;
                sqlCtx.Param(Pak_Whplt_Type.fn_tp).Value = tp;

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

        public IList<PakWhLocMasInfo> GetPakWhLocMasListByBolAndPlt1AndCarrier(string bol, string plt1, string carrier)
        {
            try
            {
                IList<PakWhLocMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.bol = bol;
                        cond.plt1 = plt1;

                        Pak_Wh_Locmas cond2 = new Pak_Wh_Locmas();
                        cond2.carrier = carrier;

                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Wh_Locmas>(tk, null, null, new ConditionCollection<Pak_Wh_Locmas>(new EqualCondition<Pak_Wh_Locmas>(cond), new AnyCondition<Pak_Wh_Locmas>(cond2, "CHARINDEX({1},{0})>0")), Pak_Wh_Locmas.fn_loc + FuncNew.DescendOrder, Pak_Wh_Locmas.fn_col);
                    }
                }
                sqlCtx.Param(Pak_Wh_Locmas.fn_bol).Value = bol;
                sqlCtx.Param(Pak_Wh_Locmas.fn_plt1).Value = plt1;
                sqlCtx.Param(g.DecAny(Pak_Wh_Locmas.fn_carrier)).Value = carrier;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Wh_Locmas, PakWhLocMasInfo, PakWhLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakWhLocBolByColAndLoc(string bol, string col, int loc)
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
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.col = col;
                        cond.loc = loc;

                        Pak_Wh_Locmas setv = new Pak_Wh_Locmas();
                        setv.bol = bol;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Wh_Locmas>(tk, new SetValueCollection<Pak_Wh_Locmas>(new CommonSetValue<Pak_Wh_Locmas>(setv)), new ConditionCollection<Pak_Wh_Locmas>(new EqualCondition<Pak_Wh_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Wh_Locmas.fn_col).Value = col;
                sqlCtx.Param(Pak_Wh_Locmas.fn_loc).Value = loc;
                sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_bol)).Value = bol;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertWhPltLocLogInfo(WhPltLocLogInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Wh_Pltloclog>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Wh_Pltloclog, WhPltLocLogInfo>(sqlCtx, item);

                sqlCtx.Param(Wh_Pltloclog.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakWhLocByColAndLoc(PakWhLocMasInfo item, string col, int loc)
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
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.col = col;
                        cond.loc = loc;

                        Pak_Wh_Locmas setv = FuncNew.SetColumnFromField<Pak_Wh_Locmas, PakWhLocMasInfo>(item, Pak_Wh_Locmas.fn_id, Pak_Wh_Locmas.fn_cdt);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Wh_Locmas>(tk, new SetValueCollection<Pak_Wh_Locmas>(new CommonSetValue<Pak_Wh_Locmas>(setv)), new ConditionCollection<Pak_Wh_Locmas>(new EqualCondition<Pak_Wh_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Wh_Locmas.fn_col).Value = col;
                sqlCtx.Param(Pak_Wh_Locmas.fn_loc).Value = loc;

                sqlCtx = FuncNew.SetColumnFromField<Pak_Wh_Locmas, PakWhLocMasInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakWhLocByPltForClearPlt1AndPlt2(string plt)
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
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.plt1 = plt;
 
                        Pak_Wh_Locmas setv = new Pak_Wh_Locmas();
                        setv.plt1 = string.Empty;
                        setv.plt2 = string.Empty;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Wh_Locmas>(tk, new SetValueCollection<Pak_Wh_Locmas>(new CommonSetValue<Pak_Wh_Locmas>(setv)), new ConditionCollection<Pak_Wh_Locmas>(new AnyCondition<Pak_Wh_Locmas>(cond, string.Format("({0}={1} OR {2}={1}) AND {1}<>''", "{0}", "{1}", Pak_Wh_Locmas.fn_plt2))));

                        sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_plt1)).Value = setv.plt1;
                        sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_plt2)).Value = setv.plt2;
                    }
                }
                sqlCtx.Param(g.DecAny(Pak_Wh_Locmas.fn_plt1)).Value = plt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PoPltInfo> GetPoPltByPlt(string plt)
        {
            try
            {
                IList<PoPltInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        PoPlt cond = new PoPlt();
                        cond.plt = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<PoPlt>(tk, null, null, new ConditionCollection<PoPlt>(new EqualCondition<PoPlt>(cond)), PoPlt.fn_plt);
                    }
                }
                sqlCtx.Param(PoPlt.fn_plt).Value = plt;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<PoPlt, PoPltInfo, PoPltInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPoPlt(PoPltInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<PoPlt>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<PoPlt, PoPltInfo>(sqlCtx, item);

                sqlCtx.Param(PoPlt.fn_cdt).Value = cmDt;
                sqlCtx.Param(PoPlt.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPoData(PoDataInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<PoData>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<PoData, PoDataInfo>(sqlCtx, item);

                sqlCtx.Param(PoData.fn_cdt).Value = cmDt;
                sqlCtx.Param(PoData.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PalletCapacityInfo> GetPalletCapacityInfoList(string shipmentNo)
        {
            try
            {
                IList<PalletCapacityInfo> ret = null;
            
                var firstSet = GetTotalQtyForGetPalletCapacityInfoList(shipmentNo);
                if (firstSet != null && firstSet.Count > 0)
                {
                    var secondSet = GetOKQtyForGetPalletCapacityInfoList(firstSet);
                    ret = firstSet.Values.ToArray();
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IDictionary<string, PalletCapacityInfo> GetTotalQtyForGetPalletCapacityInfoList(string shipmentNo)
        {
            try
            {
                IDictionary<string, PalletCapacityInfo> ret = new Dictionary<string, PalletCapacityInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Delivery_Pallet cond = new Delivery_Pallet();
                        cond.shipmentNo = shipmentNo;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<Delivery_Pallet>(tk, null, new string[][] { 
                            new string[] { Delivery_Pallet.fn_deliveryQty, string.Format("SUM({0})", Delivery_Pallet.fn_deliveryQty) },
                            new string[] { Delivery_Pallet.fn_palletNo, Delivery_Pallet.fn_palletNo }
                        },new ConditionCollection<Delivery_Pallet>(new EqualCondition<Delivery_Pallet>(cond)));
                    }
                }
                sqlCtx.Param(Delivery_Pallet.fn_shipmentNo).Value = shipmentNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while(sqlR.Read())
                        {
                            PalletCapacityInfo item = new PalletCapacityInfo();
                            item.TotalQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Delivery_Pallet.fn_deliveryQty));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(Delivery_Pallet.fn_palletNo));
                            ret.Add(item.PalletNo, item);
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

        private IDictionary<string, PalletCapacityInfo> GetOKQtyForGetPalletCapacityInfoList(IDictionary<string, PalletCapacityInfo> firstSet)
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
                        Product cond = new Product();
                        cond.palletNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<Product>(tk, null, new string[][] { 
                            new string[] { Product.fn_productID, string.Format("COUNT({0})", Product.fn_productID) },
                            new string[] { Product.fn_palletNo, Product.fn_palletNo }
                        }, new ConditionCollection<Product>(new InSetCondition<Product>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Product.fn_palletNo), g.ConvertInSet(firstSet.Keys.ToArray()));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string pltNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(Product.fn_palletNo));
                            if (!string.IsNullOrEmpty(pltNo) && firstSet.ContainsKey(pltNo))
                            {
                                PalletCapacityInfo item = firstSet[pltNo];
                                item.OKQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Product.fn_productID));
                            }
                        }
                    }
                }
                return firstSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfBoundProduct(string plt)
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
                        _Metas.Product cond = new _Metas.Product();
                        cond.palletNo = plt;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Product>(tk, "COUNT", new string[] { _Metas.Product.fn_productID }, new ConditionCollection<_Metas.Product>(new EqualCondition<_Metas.Product>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Product.fn_palletNo).Value = plt;

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

        public void DeletePalletsByDn(string dn)
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
                        mtns::Pallet cond = new mtns::Pallet();
                        cond.palletNo = dn;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pallet>(tk, new ConditionCollection<mtns::Pallet>(
                            new AnyCondition<mtns::Pallet>(cond, string.Format("{0} IN (SELECT DISTINCT {1} FROM {2} WHERE {3}={4})", "{0}", mtns::Delivery_Pallet.fn_palletNo, ToolsNew.GetTableName(typeof(mtns::Delivery_Pallet)), mtns::Delivery_Pallet.fn_deliveryNo, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::Pallet.fn_palletNo)).Value = dn;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePalletsByShipmentNo(string shipmentNo)
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
                        mtns::Pallet cond = new mtns::Pallet();
                        cond.palletNo = shipmentNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pallet>(tk, new ConditionCollection<mtns::Pallet>(
                            new AnyCondition<mtns::Pallet>(cond, string.Format("{0} IN (SELECT DISTINCT {1} FROM {2} WHERE {3}={4})", "{0}", mtns::Delivery_Pallet.fn_palletNo, ToolsNew.GetTableName(typeof(mtns::Delivery_Pallet)), mtns::Delivery_Pallet.fn_shipmentNo, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::Pallet.fn_palletNo)).Value = shipmentNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ChepPalletInfo> GetChepPalletListByPalletNo(string palletNo)
        {
            try
            {
                IList<ChepPalletInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        ChepPallet cond = new ChepPallet();
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<ChepPallet>(tk, null, null, new ConditionCollection<ChepPallet>(new EqualCondition<ChepPallet>(cond)), ChepPallet.fn_id);
                    }
                }
                sqlCtx.Param(ChepPallet.fn_palletNo).Value = palletNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ChepPallet, ChepPalletInfo, ChepPalletInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetBolFromPakWhLocMasByPlt1AndPlt2(string palletNo)
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
                        mtns::Pak_Wh_Locmas cond = new mtns::Pak_Wh_Locmas();
                        cond.plt1 = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Wh_Locmas>(tk, null, new string[] { mtns::Pak_Wh_Locmas.fn_bol }, new ConditionCollection<mtns::Pak_Wh_Locmas>(new AnyCondition<Pak_Wh_Locmas>(cond, string.Format("{0}=RTRIM({1}) OR {2}=RTRIM({1})", "{0}", "{1}", Pak_Wh_Locmas.fn_plt2))));
                    }
                }
                sqlCtx.Param(g.DecAny(mtns::Pak_Wh_Locmas.fn_plt1)).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Wh_Locmas.fn_bol));
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

        public void UpdatePakWhLocByPltForClearBol(string bol, string editor)
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
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.plt1 = string.Empty;

                        Pak_Wh_Locmas cond2 = new Pak_Wh_Locmas();
                        cond2.bol = bol;

                        Pak_Wh_Locmas setv = new Pak_Wh_Locmas();
                        setv.bol = string.Empty;
                        setv.editor = editor;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Wh_Locmas>(tk, new SetValueCollection<Pak_Wh_Locmas>(new CommonSetValue<Pak_Wh_Locmas>(setv)), new ConditionCollection<Pak_Wh_Locmas>(new EqualCondition<Pak_Wh_Locmas>(cond), new AnyCondition<Pak_Wh_Locmas>(cond2, "{0}=RTRIM({1}) AND {1}<>''")));

                        sqlCtx.Param(Pak_Wh_Locmas.fn_plt1).Value = cond.plt1;
                        sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_bol)).Value = setv.bol;
                    }
                }
                sqlCtx.Param(g.DecAny(Pak_Wh_Locmas.fn_bol)).Value = bol;
                sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_editor)).Value = editor;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakWhLocByPltForClearBolAndPlt1AndPlt2(string palletNo)
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
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.plt1 = palletNo;

                        Pak_Wh_Locmas setv = new Pak_Wh_Locmas();
                        setv.bol = string.Empty;
                        setv.plt1 = string.Empty;
                        setv.plt2 = string.Empty;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Wh_Locmas>(tk, new SetValueCollection<Pak_Wh_Locmas>(new CommonSetValue<Pak_Wh_Locmas>(setv)), new ConditionCollection<Pak_Wh_Locmas>(new AnyCondition<Pak_Wh_Locmas>(cond, string.Format("{0}=RTRIM({1}) OR {2}=RTRIM({1})", "{0}", "{1}", Pak_Wh_Locmas.fn_plt2))));

                        sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_bol)).Value = setv.bol;
                        sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_plt1)).Value = setv.plt1;
                        sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_plt2)).Value = setv.plt2;
                    }
                }
                sqlCtx.Param(g.DecAny(Pak_Wh_Locmas.fn_plt1)).Value = palletNo;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Wh_Locmas.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakWhLocTmpForClearPltAndTpAndBol(string palletNo)
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
                        Pak_Whloc_Tmp cond = new Pak_Whloc_Tmp();
                        cond.plt = palletNo;

                        Pak_Whloc_Tmp setv = new Pak_Whloc_Tmp();
                        setv.plt = string.Empty;
                        setv.tp = string.Empty;
                        setv.bol = string.Empty;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Whloc_Tmp>(tk, new SetValueCollection<Pak_Whloc_Tmp>(new CommonSetValue<Pak_Whloc_Tmp>(setv)), new ConditionCollection<Pak_Whloc_Tmp>(new EqualCondition<Pak_Whloc_Tmp>(cond, null, "RTRIM({0})")));

                        sqlCtx.Param(g.DecSV(Pak_Whloc_Tmp.fn_plt)).Value = setv.plt;
                        sqlCtx.Param(g.DecSV(Pak_Whloc_Tmp.fn_tp)).Value = setv.tp;
                        sqlCtx.Param(g.DecSV(Pak_Whloc_Tmp.fn_bol)).Value = setv.bol;
                    }
                }
                sqlCtx.Param(Pak_Whloc_Tmp.fn_plt).Value = palletNo;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Whloc_Tmp.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakWhLocTmpForClearBol(string bol)
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
                        Pak_Whloc_Tmp cond = new Pak_Whloc_Tmp();
                        cond.plt = string.Empty;

                        Pak_Whloc_Tmp cond2 = new Pak_Whloc_Tmp();
                        cond2.bol = bol;

                        Pak_Whloc_Tmp setv = new Pak_Whloc_Tmp();
                        setv.bol = string.Empty;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Whloc_Tmp>(tk, new SetValueCollection<Pak_Whloc_Tmp>(new CommonSetValue<Pak_Whloc_Tmp>(setv)), new ConditionCollection<Pak_Whloc_Tmp>(new EqualCondition<Pak_Whloc_Tmp>(cond), new AnyCondition<Pak_Whloc_Tmp>(cond2, "{0}=RTRIM({1}) AND {1}<>''")));

                        sqlCtx.Param(Pak_Whloc_Tmp.fn_plt).Value = cond.plt; 
                        sqlCtx.Param(g.DecSV(Pak_Whloc_Tmp.fn_bol)).Value = setv.bol;
                    }
                }
                sqlCtx.Param(g.DecAny(Pak_Whloc_Tmp.fn_bol)).Value = bol;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Whloc_Tmp.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteChepPalletInfo(string palletNo)
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
                        mtns::ChepPallet cond = new mtns::ChepPallet();
                        cond.palletNo = palletNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::ChepPallet>(tk, new ConditionCollection<mtns::ChepPallet>(new EqualCondition<mtns::ChepPallet>(cond)));
                    }
                }
                sqlCtx.Param(mtns::ChepPallet.fn_palletNo).Value = palletNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePalletRfid(string plt, string rfidCode)
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
                        mtns::Pallet_RFID cond = new mtns::Pallet_RFID();
                        cond.plt = plt;
                        cond.rfidcode = rfidCode;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pallet_RFID>(tk, new ConditionCollection<mtns::Pallet_RFID>(new EqualCondition<mtns::Pallet_RFID>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pallet_RFID.fn_plt).Value = plt;
                sqlCtx.Param(mtns::Pallet_RFID.fn_rfidcode).Value = rfidCode;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPalletRfid(PalletRfidInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pallet_RFID>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Pallet_RFID, PalletRfidInfo>(sqlCtx, item);

                sqlCtx.Param(Pallet_RFID.fn_cdt).Value = cmDt;
                sqlCtx.Param(Pallet_RFID.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DummyShipDetInfo GetDummyShipDet(string snoId)
        {
            try
            {
                DummyShipDetInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Dummy_ShipDet cond = new Dummy_ShipDet();
                        cond.snoId = snoId;
                        sqlCtx = FuncNew.GetConditionedSelect<Dummy_ShipDet>(tk, "TOP 1", null, new ConditionCollection<Dummy_ShipDet>(new EqualCondition<Dummy_ShipDet>(cond)), Dummy_ShipDet.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Dummy_ShipDet.fn_snoId).Value = snoId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Dummy_ShipDet, DummyShipDetInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DummyShipDetInfo> GetDummyShipDetListByBol(string bol)
        {
            try
            {
                IList<DummyShipDetInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Dummy_ShipDet cond = new Dummy_ShipDet();
                        cond.bol = bol;
                        sqlCtx = FuncNew.GetConditionedSelect<Dummy_ShipDet>(tk, null, null, new ConditionCollection<Dummy_ShipDet>(new EqualCondition<Dummy_ShipDet>(cond)), Dummy_ShipDet.fn_snoId);
                    }
                }
                sqlCtx.Param(Dummy_ShipDet.fn_bol).Value = bol;
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

        public IList<DeliveryPallet> GetDeliveryPalletByShipmentNoList(IList<string> shipmentNoList)
        {
            try
            {
                IList<DeliveryPallet> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Delivery_Pallet cond = new mtns::Delivery_Pallet();
                        cond.shipmentNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Delivery_Pallet>(tk, null, null, new ConditionCollection<mtns::Delivery_Pallet>(new InSetCondition<mtns::Delivery_Pallet>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Delivery_Pallet.fn_shipmentNo), g.ConvertInSet(shipmentNoList));
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DeliveryPallet>();
                        while (sqlR.Read())
                        {
                            DeliveryPallet item = new DeliveryPallet();
                            item.ID = g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_ID));
                            item.Cdt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_Cdt));
                            item.DeliveryID = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_DeliveryNo));
                            item.DeliveryQty = g.GetValue_Int16(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_DeliveryQty));
                            item.Editor = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_Editor));
                            item.PalletID = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_PalletNo));
                            item.ShipmentID = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_ShipmentNo));
                            item.Status = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_Status));
                            item.Udt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_Udt));
                            item.PalletType = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_palletType));
                            item.DeviceQty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Schema.Delivery_Pallet.fn_deviceQty));
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

        public string GetSnoIdFromPakLocMasByPno(string pno)
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
                        mtns::Pak_Locmas cond = new mtns::Pak_Locmas();
                        cond.pno = pno;

                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Locmas>(tk, null, new string[] { mtns::Pak_Locmas.fn_snoId }, new ConditionCollection<mtns::Pak_Locmas>(new EqualCondition<mtns::Pak_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Locmas.fn_pno).Value = pno;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret =  g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Locmas.fn_snoId));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetModelByTypeAndPartNo(string type, string partno)
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
                        mtns::Pak_Chn_Tw_Light cond = new mtns::Pak_Chn_Tw_Light();
                        cond.type = type;
                        cond.partNo = partno;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Chn_Tw_Light>(tk, null, new string[]{ mtns::Pak_Chn_Tw_Light.fn_model }, new ConditionCollection<mtns::Pak_Chn_Tw_Light>(new EqualCondition<mtns::Pak_Chn_Tw_Light>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_type).Value = type;
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_partNo).Value = partno;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Chn_Tw_Light.fn_model));
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

        public IList<PakChnTwLightInfo> GetModelbyTypeAndPartNoAndFamily(string type, string partno, string model)
        {
            try
            {
                IList<PakChnTwLightInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pak_Chn_Tw_Light cond = new mtns::Pak_Chn_Tw_Light();
                        cond.type = type;
                        cond.partNo = partno;
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Chn_Tw_Light>(tk, null, null, new ConditionCollection<mtns::Pak_Chn_Tw_Light>(new EqualCondition<mtns::Pak_Chn_Tw_Light>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_type).Value = type;
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_partNo).Value = partno;
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_model).Value = model;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo, PakChnTwLightInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ModelStatistics> GetByModelStatisticsForSinglePallet(string pltNo)
        {
            try
            {
                IList<ModelStatistics> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Product cond = new _Metas.Product();
                        cond.palletNo = pltNo;
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Product>(tk, null, new string[][] { new string[] { _Metas.Product.fn_model, _Metas.Product.fn_model }, new string[] { _Metas.Product.fn_productID, string.Format("COUNT({0})", _Metas.Product.fn_productID) } }, new ConditionCollection<_Metas.Product>(new EqualCondition<_Metas.Product>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Product.fn_palletNo).Value = pltNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<ModelStatistics>();
                        while (sqlR.Read())
                        {
                            ModelStatistics item = new ModelStatistics();
                            item.Model = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Product.fn_model));
                            item.Qty = g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Product.fn_productID));
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

        public IList<PakChnTwLightInfo> GetPakChnTwLightInfoListByLikeModel(string modelPrefix)
        {
            try
            {
                IList<PakChnTwLightInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pak_Chn_Tw_Light cond = new mtns::Pak_Chn_Tw_Light();
                        cond.model = modelPrefix + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Chn_Tw_Light>(tk, null, null, new ConditionCollection<mtns::Pak_Chn_Tw_Light>(new LikeCondition<mtns::Pak_Chn_Tw_Light>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_model).Value = modelPrefix + "%";
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo, PakChnTwLightInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakChnTwLightInfo> GetPakChnTwLightInfoListByModelAndPno(string model, string pno)
        {
            try
            {
                IList<PakChnTwLightInfo> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Pak_Chn_Tw_Light>();
                        _Metas.Pak_Chn_Tw_Light cond0 = new _Metas.Pak_Chn_Tw_Light();
                        cond0.model = model;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Pak_Chn_Tw_Light>(cond0));

                        tf2 = new TableAndFields<_Metas.ModelBOM_NEW>();
                        _Metas.ModelBOM_NEW cond = new _Metas.ModelBOM_NEW();
                        cond.material = pno;
                        cond.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<_Metas.ModelBOM_NEW>(cond));
                        tf2.ClearToGetFieldNames();
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_GetData;

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Pak_Chn_Tw_Light, _Metas.ModelBOM_NEW>(tf1, _Metas.Pak_Chn_Tw_Light.fn_partNo, tf2, _Metas.ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.ModelBOM_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Pak_Chn_Tw_Light.fn_model)).Value = model;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.ModelBOM_NEW.fn_material)).Value = pno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo, PakChnTwLightInfo>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakChnTwLightInfo> GetPakChnTwLightInfoListByModelAndPno2(string model, string pno)
        {
            try
            {
                IList<PakChnTwLightInfo> ret = null;

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
                        tf1 = new TableAndFields<_Metas.Pak_Chn_Tw_Light>();
                        _Metas.Pak_Chn_Tw_Light cond0 = new _Metas.Pak_Chn_Tw_Light();
                        cond0.model = model;
                        tf1.Conditions.Add(new EqualCondition<_Metas.Pak_Chn_Tw_Light>(cond0, null, "CASE CHARINDEX(' ',{0}) WHEN 0 THEN {0} ELSE LEFT({0},CHARINDEX(' ',{0})-1) END"));

                        tf2 = new TableAndFields<_Metas.ModelBOM_NEW>();
                        _Metas.ModelBOM_NEW cond = new _Metas.ModelBOM_NEW();
                        cond.material = pno;
                        cond.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<_Metas.ModelBOM_NEW>(cond));
                        tf2.ClearToGetFieldNames();
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_GetData;

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Pak_Chn_Tw_Light, _Metas.ModelBOM_NEW>(tf1, _Metas.Pak_Chn_Tw_Light.fn_partNo, tf2, _Metas.ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.ModelBOM_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Pak_Chn_Tw_Light.fn_model)).Value = model;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.ModelBOM_NEW.fn_material)).Value = pno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo, PakChnTwLightInfo>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetTypeListFromPakChnTwLightByModel(string model)
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
                        mtns::Pak_Chn_Tw_Light cond = new mtns::Pak_Chn_Tw_Light();
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Chn_Tw_Light>(tk, "DISTINCT", new string[]{ mtns::Pak_Chn_Tw_Light.fn_type }, new ConditionCollection<mtns::Pak_Chn_Tw_Light>(new EqualCondition<mtns::Pak_Chn_Tw_Light>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_model).Value = model;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Chn_Tw_Light.fn_model));
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

        public bool CheckExistPakLocMas(string pno, string tp, string fl)
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
                        _Metas.Pak_Locmas cond = new _Metas.Pak_Locmas();
                        cond.pno = pno;
                        cond.tp = tp;
                        cond.fl = fl;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Locmas>(tk, "COUNT", new string[] { _Metas.Pak_Locmas.fn_id }, new ConditionCollection<_Metas.Pak_Locmas>(new EqualCondition<_Metas.Pak_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pak_Locmas.fn_pno).Value = pno;
                sqlCtx.Param(_Metas.Pak_Locmas.fn_tp).Value = tp;
                sqlCtx.Param(_Metas.Pak_Locmas.fn_fl).Value = fl;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void UpdatePakLocMasForPdLine(string pdLine, string pno, string tp)
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
                        Pak_Locmas cond = new Pak_Locmas();
                        cond.pno = pno;
                        cond.tp = tp;
                        Pak_Locmas setv = new Pak_Locmas();
                        setv.pdLine = pdLine;
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Locmas>(tk, new SetValueCollection<Pak_Locmas>(new CommonSetValue<Pak_Locmas>(setv)), new ConditionCollection<Pak_Locmas>(new EqualCondition<Pak_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Locmas.fn_pno).Value = pno;
                sqlCtx.Param(Pak_Locmas.fn_tp).Value = tp;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_pdLine)).Value = pdLine;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakLocMasInfo> GetPakLocMasList(string pno, string tp, string fl)
        {
            try
            {
                IList<PakLocMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pak_Locmas cond = new mtns::Pak_Locmas();
                        cond.pno = pno;
                        cond.tp = tp;
                        cond.fl = fl;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Locmas>(tk, null, null, new ConditionCollection<mtns::Pak_Locmas>(new EqualCondition<mtns::Pak_Locmas>(cond)), string.Format("CONVERT(INT,{0})",mtns::Pak_Locmas.fn_snoId) + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::Pak_Locmas.fn_pno).Value = pno;
                sqlCtx.Param(mtns::Pak_Locmas.fn_tp).Value = tp;
                sqlCtx.Param(mtns::Pak_Locmas.fn_fl).Value = fl;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pak_Locmas, PakLocMasInfo, PakLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakLocMasForPdLineAndPno(string pdLine, string newPno, string oldPno, string tp, string snoId)
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
                        Pak_Locmas cond = new Pak_Locmas();
                        cond.pno = oldPno;
                        cond.tp = tp;
                        cond.snoId = snoId;
                        Pak_Locmas setv = new Pak_Locmas();
                        setv.pdLine = pdLine;
                        setv.pno = newPno;
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Locmas>(tk, new SetValueCollection<Pak_Locmas>(new CommonSetValue<Pak_Locmas>(setv)), new ConditionCollection<Pak_Locmas>(new EqualCondition<Pak_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Locmas.fn_pno).Value = oldPno;
                sqlCtx.Param(Pak_Locmas.fn_tp).Value = tp;
                sqlCtx.Param(Pak_Locmas.fn_snoId).Value = snoId;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_pdLine)).Value = pdLine;
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_pno)).Value = newPno;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakLocMasInfo> GetPakLocMasList(string pno, string tp)
        {
            try
            {
                IList<PakLocMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Pak_Locmas cond = new mtns::Pak_Locmas();
                        cond.pno = pno;
                        cond.tp = tp;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Locmas>(tk, null, null, new ConditionCollection<mtns::Pak_Locmas>(new EqualCondition<mtns::Pak_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pak_Locmas.fn_pno).Value = pno;
                sqlCtx.Param(mtns::Pak_Locmas.fn_tp).Value = tp;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Pak_Locmas, PakLocMasInfo, PakLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakLocMasInfo> GetPakLocMasList(PakLocMasInfo condition)
        {
            try
            {
                IList<PakLocMasInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pak_Locmas cond = mtns::FuncNew.SetColumnFromField<mtns::Pak_Locmas, PakLocMasInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pak_Locmas>(null, null, new mtns::ConditionCollection<mtns::Pak_Locmas>(new mtns::EqualCondition<mtns::Pak_Locmas>(cond)), string.Format("CONVERT(INT,{0})", mtns::Pak_Locmas.fn_snoId) + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pak_Locmas, PakLocMasInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pak_Locmas, PakLocMasInfo, PakLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakLocMasInfo> GetPakLocMasList(PakLocMasInfo eqCondition, PakLocMasInfo neqCondition)
        {
            try
            {
                IList<PakLocMasInfo> ret = null;

                if (eqCondition == null)
                    eqCondition = new PakLocMasInfo();
                if (neqCondition == null)
                    neqCondition = new PakLocMasInfo();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {

                _Metas.Pak_Locmas cond = FuncNew.SetColumnFromField<_Metas.Pak_Locmas, PakLocMasInfo>(eqCondition);
                _Metas.Pak_Locmas cond2 = FuncNew.SetColumnFromField<_Metas.Pak_Locmas, PakLocMasInfo>(neqCondition);

                sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pak_Locmas>(null, null, new ConditionCollection<_Metas.Pak_Locmas>(new EqualCondition<_Metas.Pak_Locmas>(cond), new NotEqualCondition<_Metas.Pak_Locmas>(cond2)), string.Format("CONVERT(INT,{0})", mtns::Pak_Locmas.fn_snoId) + FuncNew.DescendOrder);
                var sqlCtx2 = FuncNew.GetConditionedSelect<_Metas.Pak_Locmas>(null, new string[] { _Metas.Pak_Locmas.fn_id }, new ConditionCollection<_Metas.Pak_Locmas>(new NotEqualCondition<_Metas.Pak_Locmas>(cond2)));

                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<_Metas.Pak_Locmas, PakLocMasInfo>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.Pak_Locmas, PakLocMasInfo>(sqlCtx2, neqCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pak_Locmas, PakLocMasInfo, PakLocMasInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakLocMasForPno(string newPno, string oldPno, string tp)
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
                        Pak_Locmas cond = new Pak_Locmas();
                        cond.pno = oldPno;
                        cond.tp = tp;
                        Pak_Locmas setv = new Pak_Locmas();
                        setv.pno = newPno;
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Locmas>(tk, new SetValueCollection<Pak_Locmas>(new CommonSetValue<Pak_Locmas>(setv)), new ConditionCollection<Pak_Locmas>(new EqualCondition<Pak_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Locmas.fn_pno).Value = oldPno;
                sqlCtx.Param(Pak_Locmas.fn_tp).Value = tp;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_pno)).Value = newPno;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPakOdmSession(PakOdmSessionInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Pakodmsession>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Pakodmsession, PakOdmSessionInfo>(sqlCtx, item);
                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_HP_EDI, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistFromPakDotPakRtByDocCatAndDocSetNumer(string docCat, string docSetNumer, string xslTemplateNameInnerStr)
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
                        _Metas.PakDotpakrt cond = new _Metas.PakDotpakrt();
                        cond.doc_cat = docCat;
                        cond.doc_set_number = docSetNumer;

                        _Metas.PakDotpakrt cond2 = new _Metas.PakDotpakrt();
                        cond2.xsl_template_name = "%" + xslTemplateNameInnerStr + "%";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.PakDotpakrt>(tk, "COUNT", new string[] { _Metas.PakDotpakrt.fn_id }, new ConditionCollection<_Metas.PakDotpakrt>(
                            new EqualCondition<_Metas.PakDotpakrt>(cond),
                            new LikeCondition<_Metas.PakDotpakrt>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.PakDotpakrt.fn_doc_cat).Value = docCat;
                sqlCtx.Param(_Metas.PakDotpakrt.fn_doc_set_number).Value = docSetNumer;
                sqlCtx.Param(_Metas.PakDotpakrt.fn_xsl_template_name).Value = "%" + xslTemplateNameInnerStr + "%";

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

        public IList<WhPltWeightInfo> GetWhPltWeightList(WhPltWeightInfo condition)
        {
            try
            {
                IList<WhPltWeightInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Wh_Pltweight cond = FuncNew.SetColumnFromField<Wh_Pltweight, WhPltWeightInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<Wh_Pltweight>(null, null, new ConditionCollection<Wh_Pltweight>(new EqualCondition<Wh_Pltweight>(cond)), Wh_Pltweight.fn_id);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Wh_Pltweight, WhPltWeightInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Wh_Pltweight, WhPltWeightInfo, WhPltWeightInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateWhPltWeight(WhPltWeightInfo setValue, WhPltWeightInfo condition)
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
                Wh_Pltweight cond = FuncNew.SetColumnFromField<Wh_Pltweight, WhPltWeightInfo>(condition);
                Wh_Pltweight setv = FuncNew.SetColumnFromField<Wh_Pltweight, WhPltWeightInfo>(setValue);

                sqlCtx = FuncNew.GetConditionedUpdate<Wh_Pltweight>(new SetValueCollection<Wh_Pltweight>(new CommonSetValue<Wh_Pltweight>(setv)), new ConditionCollection<Wh_Pltweight>(new EqualCondition<Wh_Pltweight>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Wh_Pltweight, WhPltWeightInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Wh_Pltweight, WhPltWeightInfo>(sqlCtx, setValue, true);
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PalletNoAndWeight> GetAllDistinctPalletNoAndWeights()
        {
            try
            {
                IList<PalletNoAndWeight> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Pallet>(tk, "DISTINCT", new string[][] { new string[] { _Metas.Pallet.fn_palletNo, _Metas.Pallet.fn_palletNo }, new string[] { _Metas.Pallet.fn_weight, string.Format("CONVERT(DECIMAL(9,1), ISNULL(RTRIM({0}),'0'))", _Metas.Pallet.fn_weight) } }, new ConditionCollection<_Metas.Pallet>(), _Metas.Pallet.fn_palletNo);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<PalletNoAndWeight>();
                        while (sqlR.Read())
                        {
                            PalletNoAndWeight item = new PalletNoAndWeight();
                            item.SnoId = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Pallet.fn_palletNo));
                            item.KG = g.GetValue_Decimal(sqlR, sqlCtx.Indexes(_Metas.Pallet.fn_weight));
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

        public IList<PalletNoAndWeight> GetPalletNoAndWeightConsiderProduct(IList<PalletNoAndWeight> plts)
        {
            try
            {
                IList<PalletNoAndWeight> ret = null;

                IDictionary<string, PalletNoAndWeight> dic = ConvertToDic(plts);

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Product cond = new _Metas.Product();
                        cond.palletNo = "[INSET]";
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<_Metas.Product>(tk, null, new string[][] { new string[] { _Metas.Product.fn_palletNo, _Metas.Product.fn_palletNo }, new string[] { _Metas.Product.fn_unitWeight, string.Format("SUM(CONVERT(DECIMAL(9,1), ISNULL({0}, '0')))", _Metas.Product.fn_unitWeight) } }, new ConditionCollection<_Metas.Product>(new InSetCondition<_Metas.Product>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Product.fn_palletNo), g.ConvertInSet(dic.Keys.ToArray()));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<PalletNoAndWeight>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Product.fn_palletNo));
                            if (dic.ContainsKey(item))//此处应该没有例外,但为保险起见.
                            {
                                dic[item].KG = g.GetValue_Decimal(sqlR, sqlCtx.Indexes(_Metas.Product.fn_unitWeight));
                                ret.Add(dic[item]);
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

        private IDictionary<string, PalletNoAndWeight> ConvertToDic(IList<PalletNoAndWeight> plts)
        {
            try
            {
                IDictionary<string, PalletNoAndWeight> ret = null;
                if (plts != null)
                {
                    ret = new Dictionary<string, PalletNoAndWeight>();
                    foreach (PalletNoAndWeight item in plts)
                    {
                        if (!ret.ContainsKey(item.SnoId))
                            ret.Add(item.SnoId, item);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ChinaLabelInfo> GetChinaLabel(ChinaLabelInfo condition)
        {
            try
            {
                IList<ChinaLabelInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                ChinaLabel cond = FuncNew.SetColumnFromField<ChinaLabel, ChinaLabelInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<ChinaLabel>(null, null, new ConditionCollection<ChinaLabel>(new EqualCondition<ChinaLabel>(cond)), ChinaLabel.fn_id);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<ChinaLabel, ChinaLabelInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ChinaLabel, ChinaLabelInfo, ChinaLabelInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ChinaLabelInfo> GetChinaLabelByLikeFamily(string familyPrefix)
        {
            try
            {
                IList<ChinaLabelInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::ChinaLabel cond = new mtns::ChinaLabel();
                        cond.family = familyPrefix + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::ChinaLabel>(tk, null, null, new ConditionCollection<mtns::ChinaLabel>(new LikeCondition<mtns::ChinaLabel>(cond)));
                    }
                }
                sqlCtx.Param(mtns::ChinaLabel.fn_family).Value = familyPrefix + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ChinaLabel, ChinaLabelInfo, ChinaLabelInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<KittingLocPLMappingStInfo> GetKitLocPLMapST(string line, string station, short lightno)
        {
            try
            {
                IList<KittingLocPLMappingStInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Kitting_Loc_PLMapping_St cond = new Kitting_Loc_PLMapping_St();
                        cond.pdLine = line;
                        cond.station = station;
                        cond.lightNo = lightno;
                        sqlCtx = FuncNew.GetConditionedSelect<Kitting_Loc_PLMapping_St>(tk, null, null, new ConditionCollection<Kitting_Loc_PLMapping_St>(new EqualCondition<Kitting_Loc_PLMapping_St>(cond)), Kitting_Loc_PLMapping_St.fn_tagID);
                    }
                }
                sqlCtx.Param(Kitting_Loc_PLMapping_St.fn_pdLine).Value = line;
                sqlCtx.Param(Kitting_Loc_PLMapping_St.fn_station).Value = station;
                sqlCtx.Param(Kitting_Loc_PLMapping_St.fn_lightNo).Value = lightno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_KIT, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Kitting_Loc_PLMapping_St, KittingLocPLMappingStInfo, KittingLocPLMappingStInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateKitLocationFVOn(string tagid, bool configstatus, bool runningStatus, string ledvalue)
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
                        _Metas.Kitting_Location_FV cond = new _Metas.Kitting_Location_FV();
                        cond.tagID = tagid;
                        _Metas.Kitting_Location_FV setv = new _Metas.Kitting_Location_FV();
                        setv.configedLEDStatus = true;
                        setv.runningLEDStatus = true;
                        setv.ledvalues = ledvalue;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.Kitting_Location_FV>(tk, new SetValueCollection<_Metas.Kitting_Location_FV>(new CommonSetValue<_Metas.Kitting_Location_FV>(setv)), new ConditionCollection<_Metas.Kitting_Location_FV>(new EqualCondition<_Metas.Kitting_Location_FV>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Kitting_Location_FV.fn_tagID).Value = tagid;
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_configedLEDStatus)).Value = configstatus;
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_runningLEDStatus)).Value = runningStatus;
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_ledvalues)).Value = ledvalue;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_KIT, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateKitLocationFVOn(string[] tagids, bool configstatus, bool runningStatus, string ledvalue)
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
                        _Metas.Kitting_Location_FV cond = new _Metas.Kitting_Location_FV();
                        cond.tagID = "[INSET]";
                        _Metas.Kitting_Location_FV setv = new _Metas.Kitting_Location_FV();
                        setv.configedLEDStatus = true;
                        setv.runningLEDStatus = true;
                        setv.ledvalues = ledvalue;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.Kitting_Location_FV>(tk, new SetValueCollection<_Metas.Kitting_Location_FV>(new CommonSetValue<_Metas.Kitting_Location_FV>(setv)), new ConditionCollection<_Metas.Kitting_Location_FV>(new InSetCondition<_Metas.Kitting_Location_FV>(cond)));
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Kitting_Location_FV.fn_tagID), g.ConvertInSet(tagids));

                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_configedLEDStatus)).Value = configstatus;
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_runningLEDStatus)).Value = runningStatus;
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_ledvalues)).Value = ledvalue;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_KIT, CommandType.Text, Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateKitLocationFVOff(string tagid, bool configstatus, bool runningStatus)
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
                        _Metas.Kitting_Location_FV cond = new _Metas.Kitting_Location_FV();
                        cond.tagID = tagid;
                        _Metas.Kitting_Location_FV setv = new _Metas.Kitting_Location_FV();
                        setv.configedLEDStatus = true;
                        setv.runningLEDStatus = true;
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.Kitting_Location_FV>(tk, new SetValueCollection<_Metas.Kitting_Location_FV>(new CommonSetValue<_Metas.Kitting_Location_FV>(setv)), new ConditionCollection<_Metas.Kitting_Location_FV>(new EqualCondition<_Metas.Kitting_Location_FV>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Kitting_Location_FV.fn_tagID).Value = tagid;
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_configedLEDStatus)).Value = configstatus;
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_runningLEDStatus)).Value = runningStatus;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Kitting_Location_FV.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_KIT, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateKittingLocationFaXInfo(KittingLocationFaXInfo setValue, int configedLEDStatus, int runningLEDStatus, int comm, KittingLocationFaXInfo condition, int[] proritySet)
        {
            logger.Error("------------------ begin UpdateKittingLocationFaXInfo ------------------------");
            try
            {
                logger.Error(string.Format("------KittingLocationFaXInfo setValue: ----ID {0}------------------",setValue.id.ToString()));
                logger.Error(string.Format("------configedLEDStatus: {0}------------------",configedLEDStatus.ToString()));
                logger.Error(string.Format("------comm: {0}------------------", comm.ToString()));
                logger.Error(string.Format("------KittingLocationFaXInfo condition: ----ID {0}------------------", condition.id.ToString()));
                if (proritySet != null && proritySet.Length > 0)
                    logger.Error(string.Format("------proritySet: {0}------------------", g.ConvertInSet(proritySet)));


                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Kitting_Location_FA_A cond = mtns::FuncNew.SetColumnFromField<mtns::Kitting_Location_FA_A, KittingLocationFaXInfo>(condition);
                mtns::Kitting_Location_FA_A cond2 = null;
                if (proritySet != null && proritySet.Length > 0)
                {
                    cond2 = new Kitting_Location_FA_A();
                    cond2.priority = 1;
                }

                mtns::Kitting_Location_FA_A setv = mtns::FuncNew.SetColumnFromField<mtns::Kitting_Location_FA_A, KittingLocationFaXInfo>(setValue);
                setv.udt = DateTime.Now;
                if (configedLEDStatus >= 0)
                    setv.configedLEDStatus = true;
                if (runningLEDStatus >= 0)
                    setv.runningLEDStatus = true;
                if (comm >= 0)
                    setv.comm = true;

                if (cond2 != null)
                    sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Kitting_Location_FA_A>(new mtns::SetValueCollection<mtns::Kitting_Location_FA_A>(new mtns::CommonSetValue<mtns::Kitting_Location_FA_A>(setv)), new mtns::ConditionCollection<mtns::Kitting_Location_FA_A>(
                        new mtns::EqualCondition<mtns::Kitting_Location_FA_A>(cond),
                        new mtns::InSetCondition<mtns::Kitting_Location_FA_A>(cond2)));
                else
                    sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Kitting_Location_FA_A>(new mtns::SetValueCollection<mtns::Kitting_Location_FA_A>(new mtns::CommonSetValue<mtns::Kitting_Location_FA_A>(setv)), new mtns::ConditionCollection<mtns::Kitting_Location_FA_A>(
                        new mtns::EqualCondition<mtns::Kitting_Location_FA_A>(cond)));

                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Kitting_Location_FA_A, KittingLocationFaXInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Kitting_Location_FA_A, KittingLocationFaXInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Kitting_Location_FA_A.fn_udt)).Value = cmDt;

                if (configedLEDStatus >= 0)
                    sqlCtx.Param(g.DecSV(mtns::Kitting_Location_FA_A.fn_configedLEDStatus)).Value = configedLEDStatus > 0 ? true : false;
                if (runningLEDStatus >= 0)
                    sqlCtx.Param(g.DecSV(mtns::Kitting_Location_FA_A.fn_runningLEDStatus)).Value = runningLEDStatus > 0 ? true : false;
                if (comm >= 0)
                    sqlCtx.Param(g.DecSV(mtns::Kitting_Location_FA_A.fn_comm)).Value = comm > 0 ? true : false;

                if (proritySet != null && proritySet.Length > 0)
                    sqlCtx.Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns.Kitting_Location_FA_A.fn_priority), g.ConvertInSet(proritySet));

                string tableName = ToolsNew.GetTableName(typeof(mtns::Kitting_Location_FA_A));
                string newTableName = tableName.TrimEnd('A') + setValue.tableNameEpilogue;
                sqlCtx.Sentence = sqlCtx.Sentence.Replace(tableName, newTableName);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_KIT, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);

                logger.Error(string.Format("----SQL: {0} ------------------------",sqlCtx.Sentence));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                logger.Error("------------------ end UpdateKittingLocationFaXInfo ------------------------");
            }
        }

        public IList<KittingLocPLMappingInfo> GetKittingLocPLMappingInfoList(KittingLocPLMappingInfo condition)
        {
            try
            {
                IList<KittingLocPLMappingInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Kitting_Loc_PLMapping cond = FuncNew.SetColumnFromField<Kitting_Loc_PLMapping, KittingLocPLMappingInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<Kitting_Loc_PLMapping>(null, null, new ConditionCollection<Kitting_Loc_PLMapping>(new EqualCondition<Kitting_Loc_PLMapping>(cond)), Kitting_Loc_PLMapping.fn_id);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Kitting_Loc_PLMapping, KittingLocPLMappingInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Kitting_Loc_PLMapping, KittingLocPLMappingInfo, KittingLocPLMappingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfWhPltMas(string wc)
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
                        _Metas.Wh_Pltmas cond = new _Metas.Wh_Pltmas();
                        cond.wc = wc;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Wh_Pltmas>(tk, "COUNT", new string[] { _Metas.Wh_Pltmas.fn_id }, new ConditionCollection<_Metas.Wh_Pltmas>(new EqualCondition<_Metas.Wh_Pltmas>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Wh_Pltmas.fn_wc).Value = wc;

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

        public IList<WhPltMasInfo> GetWhPltMasList(string wc, int days)
        {
            try
            {
                IList<WhPltMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Wh_Pltmas cond = new Wh_Pltmas();
                        cond.wc = wc;
                        Wh_Pltmas cond2 = new Wh_Pltmas();
                        cond2.cdt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedSelect<Wh_Pltmas>(tk, null, null, new ConditionCollection<Wh_Pltmas>(
                            new EqualCondition<Wh_Pltmas>(cond),
                            new SmallerCondition<Wh_Pltmas>(cond2)), Wh_Pltmas.fn_cdt);
                    }
                }
                sqlCtx.Param(Wh_Pltmas.fn_wc).Value = wc;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecS(Wh_Pltmas.fn_cdt)).Value = cmDt.AddDays( - days);
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Wh_Pltmas, WhPltMasInfo, WhPltMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CallOp_PackingData(string snoid, string model, string dn, string plt, string loc, int pltqty, string boxid, string ucc)
        {
            try
            {
                SqlParameter[] paramsArray = new SqlParameter[8];

                paramsArray[0] = new SqlParameter("@snoID", SqlDbType.Char);
                paramsArray[0].Value = snoid;
                paramsArray[1] = new SqlParameter("@Model", SqlDbType.Char);
                paramsArray[1].Value = model;
                paramsArray[2] = new SqlParameter("@Delivery", SqlDbType.Char);
                paramsArray[2].Value = dn;
                paramsArray[3] = new SqlParameter("@Pallet", SqlDbType.Char);
                paramsArray[3].Value = plt;
                paramsArray[4] = new SqlParameter("@Location", SqlDbType.VarChar);
                paramsArray[4].Value = loc;
                paramsArray[5] = new SqlParameter("@pltqty", SqlDbType.Int);
                paramsArray[5].Value = pltqty;
                paramsArray[6] = new SqlParameter("@BoxId", SqlDbType.VarChar);
                paramsArray[6].Value = boxid;
                paramsArray[7] = new SqlParameter("@UCC", SqlDbType.VarChar);
                paramsArray[7].Value = ucc;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "op_PackingData", paramsArray);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CallOp_PackingData(string boxid, string dn, string plt, string cn, DateTime cdt)
        {
            try
            {
                SqlParameter[] paramsArray = new SqlParameter[5];
                //@BoxId nvarchar(20),@dn char(16),@plt nvarchar(14),@cn  char(10),@cdt datetime
                paramsArray[0] = new SqlParameter("@BoxId", SqlDbType.NVarChar);
                paramsArray[0].Value = boxid;
                paramsArray[1] = new SqlParameter("@dn", SqlDbType.Char);
                paramsArray[1].Value = dn;
                paramsArray[2] = new SqlParameter("@plt", SqlDbType.NVarChar);
                paramsArray[2].Value = plt;
                paramsArray[3] = new SqlParameter("@cn", SqlDbType.Char);
                paramsArray[3].Value = cn;
                paramsArray[4] = new SqlParameter("@cdt", SqlDbType.DateTime);
                paramsArray[4].Value = cdt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "op_PackingData", paramsArray);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<COMSettingInfo> GetAllCOMSetting()
        {
            try
            {
                IList<COMSettingInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Comsetting>(tk, Comsetting.fn_name);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Comsetting, COMSettingInfo, COMSettingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveCOMSettingItem(int id)
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
                        mtns::Comsetting cond = new mtns::Comsetting();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Comsetting>(tk, new ConditionCollection<mtns::Comsetting>(new EqualCondition<mtns::Comsetting>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Comsetting.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddCOMSettingItem(COMSettingInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Comsetting>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<mtns::Comsetting, COMSettingInfo>(sqlCtx, item);

                sqlCtx.Param(Comsetting.fn_cdt).Value = cmDt;
                sqlCtx.Param(Comsetting.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCOMSettingItem(COMSettingInfo item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<mtns::Comsetting>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Comsetting, COMSettingInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Comsetting.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<COMSettingInfo> FindCOMSettingByName(string name)
        {
            try
            {
                IList<COMSettingInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Comsetting cond = new Comsetting();
                        cond.name = name;
                        sqlCtx = FuncNew.GetConditionedSelect<Comsetting>(tk, null, null, new ConditionCollection<Comsetting>(new EqualCondition<Comsetting>(cond)), Comsetting.fn_name);
                    }
                }
                sqlCtx.Param(Comsetting.fn_name).Value = name;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Comsetting, COMSettingInfo, COMSettingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePallets(IList<string> pltNos)
        {
            try
            {
                //SqlTransactionManager.Begin();

                if (pltNos != null && pltNos.Count > 0)
                {
                    foreach (string pltNo in pltNos)
                    {
                        PersistDeletePallet(pltNo);
                    }
                }
                //SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.End();
                //SqlTransactionManager.Dispose();
            }
        }

        public void DeletePalletAttrs(IList<string> pltNos)
        {
            try
            {
                //SqlTransactionManager.Begin();

                if (pltNos != null && pltNos.Count > 0)
                {
                    foreach (string pltNo in pltNos)
                    {
                        PersistDeletePalletAttr(pltNo);
                    }
                }
                //SqlTransactionManager.Commit();
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.End();
                //SqlTransactionManager.Dispose();
            }
        }

        public void UpdatePakLocMasInfo(PakLocMasInfo setValue, PakLocMasInfo condition)
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
                Pak_Locmas cond = FuncNew.SetColumnFromField<Pak_Locmas, PakLocMasInfo>(condition);
                Pak_Locmas setv = FuncNew.SetColumnFromField<Pak_Locmas, PakLocMasInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Pak_Locmas>(new SetValueCollection<Pak_Locmas>(new CommonSetValue<Pak_Locmas>(setv)), new ConditionCollection<Pak_Locmas>(new EqualCondition<Pak_Locmas>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pak_Locmas, PakLocMasInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pak_Locmas, PakLocMasInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DNPalletWeight> DismantlePalletWeight(string palletOrDn, string editor)
        {
            try
            {
                IList<DNPalletWeight> ret = null;

                SqlParameter[] paramsArray = new SqlParameter[2];

                paramsArray[0] = new SqlParameter("@PalletOrDn", SqlDbType.VarChar);
                paramsArray[0].Value = palletOrDn;
                paramsArray[1] = new SqlParameter("@Editor", SqlDbType.VarChar);
                paramsArray[1].Value = editor;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "IMES_DismantlePalletWeight", paramsArray))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletWeight>();
                        while (sqlR.Read())
                        {
                            DNPalletWeight item = new DNPalletWeight();
                            item.PalletNo = GetValue_Str(sqlR, 0);
                            item.Weight = GetValue_Decimal(sqlR, 1);
                            item.WeightL = GetValue_Decimal(sqlR, 2);
                            item.DeliveryNo = GetValue_Str(sqlR, 3);
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

        public IList<DNPalletWeight> QueryPalletWeight(string palletOrDn)
        {
            try
            {
                IList<DNPalletWeight> ret = null;

                SqlParameter[] paramsArray = new SqlParameter[1];

                paramsArray[0] = new SqlParameter("@PalletOrDn", SqlDbType.VarChar);
                paramsArray[0].Value = palletOrDn;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "IMES_QueryPalletWeight", paramsArray))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DNPalletWeight>();
                        while (sqlR.Read())
                        {
                            DNPalletWeight item = new DNPalletWeight();
                            item.PalletNo = GetValue_Str(sqlR, 0); ;
                            item.Weight = GetValue_Decimal(sqlR, 1); ;
                            item.WeightL = GetValue_Decimal(sqlR, 2); ;
                            item.DeliveryNo = GetValue_Str(sqlR, 3); ;
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

        public IList<DTPallet> GetDTPalletByUdt(string from, string to, string station)
        {
            try
            {
                IList<DTPallet> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pallet cond = new _Metas.Pallet();
                        cond.udt = DateTime.Now;

                        _Metas.Pallet cond2 = new _Metas.Pallet();
                        cond2.udt = DateTime.Now;

                        _Metas.Pallet cond3 = new _Metas.Pallet();
                        cond3.station = station;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pallet>(tk, null, new string[] { _Metas.Pallet.fn_cdt, _Metas.Pallet.fn_editor, _Metas.Pallet.fn_palletNo, _Metas.Pallet.fn_udt }, 
                            new ConditionCollection<_Metas.Pallet>(
                                new GreaterOrEqualCondition<_Metas.Pallet>(cond),
                                new SmallerOrEqualCondition<_Metas.Pallet>(cond2),
                                new EqualCondition<_Metas.Pallet>(cond3)), _Metas.Pallet.fn_udt, _Metas.Pallet.fn_palletNo);
                    }
                }

                string[] fromDate = from.Split('/');
                string[] toDate = to.Split('/');
                sqlCtx.Param(g.DecGE(_Metas.Pallet.fn_udt)).Value = new DateTime(int.Parse(fromDate[0]), int.Parse(fromDate[1]), int.Parse(fromDate[2]));
                sqlCtx.Param(g.DecSE(_Metas.Pallet.fn_udt)).Value = new DateTime(int.Parse(toDate[0]), int.Parse(toDate[1]), int.Parse(toDate[2]));
                sqlCtx.Param(_Metas.Pallet.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<DTPallet>();
                        while (sqlR.Read())
                        {
                            DTPallet item = new DTPallet();
                            item.Cdt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Pallet.fn_cdt));
                            item.Editor = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Pallet.fn_editor));
                            item.PalletNo = g.GetValue_Str(sqlR, sqlCtx.Indexes(_Metas.Pallet.fn_palletNo));
                            item.Udt = g.GetValue_DateTime(sqlR, sqlCtx.Indexes(_Metas.Pallet.fn_udt));
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

        public DataTable ExecSpRptPAKKittingLocUp(string pdline)
        {
            try
            {
                DataTable ret = null;
                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@pdline", SqlDbType.Char);
                paramsArray[0].Value = pdline;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "rpt_PAKKittingLoc_up", paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecSpRptKittingLocUp(string pdline)
        {
            try
            {
                DataTable ret = null;
                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@pdline", SqlDbType.VarChar);
                paramsArray[0].Value = pdline;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "rpt_KittingLoc_up", paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecSpRptKittingLoc(string code, string kittingType, string isLine)
        {
            try
            {
                DataTable ret = null;
                SqlParameter[] paramsArray = new SqlParameter[3];
                paramsArray[0] = new SqlParameter("@code", SqlDbType.VarChar);
                paramsArray[0].Value = code;
                paramsArray[1] = new SqlParameter("@kittingType ", SqlDbType.VarChar);
                paramsArray[1].Value = kittingType;
                paramsArray[2] = new SqlParameter("@isLine ", SqlDbType.Char);
                paramsArray[2].Value = isLine;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "rpt_KittingLoc", paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakWhLocMasInfo> GetPakWhLocMasListByPlt1OrPlt2(string plt1Orplt2)
        {
            try
            {
                IList<PakWhLocMasInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pak_Wh_Locmas cond = new Pak_Wh_Locmas();
                        cond.plt1 = plt1Orplt2;
                        cond.plt2 = plt1Orplt2;

                        var condSet = new ConditionCollection<Pak_Wh_Locmas>(false);
                        condSet.Add(new EqualCondition<Pak_Wh_Locmas>(cond));

                        sqlCtx = FuncNew.GetConditionedSelect<Pak_Wh_Locmas>(tk, null, null, condSet, Pak_Wh_Locmas.fn_loc + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Pak_Wh_Locmas.fn_plt2).Value = plt1Orplt2;
                sqlCtx.Param(Pak_Wh_Locmas.fn_plt1).Value = plt1Orplt2;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pak_Wh_Locmas, PakWhLocMasInfo, PakWhLocMasInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecSpRptKittingLocUp(string line, string family)
        {
            try
            {
                DataTable ret = null;
                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@pdline", SqlDbType.Char);
                paramsArray[0].Value = line;
                paramsArray[1] = new SqlParameter("@family", SqlDbType.VarChar);
                paramsArray[1].Value = family;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "rpt_KittingLoc_up", paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAutoAssignPallet(string deliveryNo)
        {

                string ret = null;
                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@DeliveryNo", SqlDbType.Char);
                paramsArray[0].Value = deliveryNo;
                ret = _Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "IMES_GetAutoAssignPallet", paramsArray) as string;
                return ret;

        }

        public IList<WhPltLogInfo> GetWhPltLogInfoList(string[] wcs, DateTime from, DateTime to)
        {
            try
            {
                IList<WhPltLogInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Wh_Pltlog cond = new Wh_Pltlog();
                        cond.wc = "[INSET]";
                        Wh_Pltlog cond2 = new Wh_Pltlog();
                        cond2.cdt = from;
                        Wh_Pltlog cond3 = new Wh_Pltlog();
                        cond3.cdt = to;
                        sqlCtx = FuncNew.GetConditionedSelect<Wh_Pltlog>(tk, null, null, new ConditionCollection<Wh_Pltlog>(
                            new InSetCondition<Wh_Pltlog>(cond),
                            new GreaterOrEqualCondition<Wh_Pltlog>(cond2),
                            new SmallerCondition<Wh_Pltlog>(cond3)), Wh_Pltlog.fn_cdt, Wh_Pltlog.fn_plt);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Wh_Pltlog.fn_wc), g.ConvertInSet(wcs));

                sqlCtx.Param(g.DecGE(Wh_Pltlog.fn_cdt)).Value = new DateTime(from.Year, from.Month, from.Day);
                sqlCtx.Param(g.DecS(Wh_Pltlog.fn_cdt)).Value = new DateTime(to.Year, to.Month, to.Day).AddDays(1);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Wh_Pltlog, WhPltLogInfo, WhPltLogInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<WhPltLogInfo> GetWhPltLogInfoList(string[] wcs, string palletNo)
        {
            try
            {
                IList<WhPltLogInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Wh_Pltlog cond = new Wh_Pltlog();
                        cond.wc = "[INSET]";
                        Wh_Pltlog cond2 = new Wh_Pltlog();
                        cond2.plt = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Wh_Pltlog>(tk, null, null, new ConditionCollection<Wh_Pltlog>(
                            new InSetCondition<Wh_Pltlog>(cond),
                            new EqualCondition<Wh_Pltlog>(cond2)), Wh_Pltlog.fn_cdt, Wh_Pltlog.fn_plt);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Wh_Pltlog.fn_wc), g.ConvertInSet(wcs));
                sqlCtx.Param(Wh_Pltlog.fn_plt).Value = palletNo;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Wh_Pltlog, WhPltLogInfo, WhPltLogInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public WhPltLogInfo GetWhPltLogInfoNewestly(string palletNo)
        {
            try
            {
                WhPltLogInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Wh_Pltlog cond = new Wh_Pltlog();
                        cond.plt = palletNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Wh_Pltlog>(tk, "TOP 1", null, new ConditionCollection<Wh_Pltlog>(
                            new EqualCondition<Wh_Pltlog>(cond)), Wh_Pltlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(Wh_Pltlog.fn_plt).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Wh_Pltlog, WhPltLogInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDummyDetInfoByPlt(string palletNo)
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
                        mtns::Dummy_ShipDet cond = new mtns::Dummy_ShipDet();
                        cond.plt = palletNo;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Dummy_ShipDet>(tk, new ConditionCollection<mtns::Dummy_ShipDet>(new EqualCondition<mtns::Dummy_ShipDet>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Dummy_ShipDet.fn_plt).Value = palletNo;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetSnoListFromSnoDetBtLoc(SnoDetBtLocInfo condition)
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
                        mtns::SnoDet_BTLoc cond = mtns::FuncNew.SetColumnFromField<mtns::SnoDet_BTLoc, SnoDetBtLocInfo>(condition);
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::SnoDet_BTLoc>(null, new string[]{mtns::SnoDet_BTLoc.fn_sno}, new mtns::ConditionCollection<mtns::SnoDet_BTLoc>(new mtns::EqualCondition<mtns::SnoDet_BTLoc>(cond)), mtns::SnoDet_BTLoc.fn_sno);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::SnoDet_BTLoc, SnoDetBtLocInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while(sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::SnoDet_BTLoc.fn_sno));
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

        public int GetCountOfSnoIdFromSnoDetBtLoc(SnoDetBtLocInfo condition)
        {
            try
            {
                int ret = 0;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                        mtns::SnoDet_BTLoc cond = mtns::FuncNew.SetColumnFromField<mtns::SnoDet_BTLoc, SnoDetBtLocInfo>(condition);
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::SnoDet_BTLoc>("COUNT", new string[]{mtns::SnoDet_BTLoc.fn_snoId}, new mtns::ConditionCollection<mtns::SnoDet_BTLoc>(new mtns::EqualCondition<mtns::SnoDet_BTLoc>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::SnoDet_BTLoc, SnoDetBtLocInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        if(sqlR.Read())
                        {
                            ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
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

        public void UpdateSnoDetBtLoc(SnoDetBtLocInfo setValue, SnoDetBtLocInfo condition)
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
                SnoDet_BTLoc cond = FuncNew.SetColumnFromField<SnoDet_BTLoc, SnoDetBtLocInfo>(condition);
                SnoDet_BTLoc setv = FuncNew.SetColumnFromField<SnoDet_BTLoc, SnoDetBtLocInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<SnoDet_BTLoc>(new SetValueCollection<SnoDet_BTLoc>(new CommonSetValue<SnoDet_BTLoc>(setv)), new ConditionCollection<SnoDet_BTLoc>(new EqualCondition<SnoDet_BTLoc>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<SnoDet_BTLoc, SnoDetBtLocInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<SnoDet_BTLoc, SnoDetBtLocInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.SnoDet_BTLoc.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakBtLocMas(PakBtLocMasInfo setValue, PakBtLocMasInfo condition)
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
                Pak_Btlocmas cond = FuncNew.SetColumnFromField<Pak_Btlocmas, PakBtLocMasInfo>(condition);
                Pak_Btlocmas setv = FuncNew.SetColumnFromField<Pak_Btlocmas, PakBtLocMasInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Pak_Btlocmas>(new SetValueCollection<Pak_Btlocmas>(new CommonSetValue<Pak_Btlocmas>(setv)), new ConditionCollection<Pak_Btlocmas>(new EqualCondition<Pak_Btlocmas>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pak_Btlocmas, PakBtLocMasInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pak_Btlocmas, PakBtLocMasInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Pak_Btlocmas.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PakChnTwLightInfo> GetPakChnTwLightInfoList(PakChnTwLightInfo condition)
        {
            try
            {
                IList<PakChnTwLightInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pak_Chn_Tw_Light cond = mtns::FuncNew.SetColumnFromField<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pak_Chn_Tw_Light>(null, null, new mtns::ConditionCollection<mtns::Pak_Chn_Tw_Light>(new mtns::EqualCondition<mtns::Pak_Chn_Tw_Light>(cond)), mtns::Pak_Chn_Tw_Light.fn_id);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo, PakChnTwLightInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPakChnTwLightInfo(PakChnTwLightInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pak_Chn_Tw_Light>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pak_Chn_Tw_Light, PakChnTwLightInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Pak_Chn_Tw_Light.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakChnTwLightInfo(PakChnTwLightInfo setValue, PakChnTwLightInfo condition)
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
                mtns::Pak_Chn_Tw_Light cond = mtns::FuncNew.SetColumnFromField<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo>(condition);
                mtns::Pak_Chn_Tw_Light setv = mtns::FuncNew.SetColumnFromField<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Pak_Chn_Tw_Light>(new mtns::SetValueCollection<mtns::Pak_Chn_Tw_Light>(new mtns::CommonSetValue<mtns::Pak_Chn_Tw_Light>(setv)), new mtns::ConditionCollection<mtns::Pak_Chn_Tw_Light>(new mtns::EqualCondition<mtns::Pak_Chn_Tw_Light>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pak_Chn_Tw_Light, PakChnTwLightInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Pak_Chn_Tw_Light.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePakChnTwLightInfo(PakChnTwLightInfo condition)
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
                Pak_Chn_Tw_Light cond = FuncNew.SetColumnFromField<Pak_Chn_Tw_Light, PakChnTwLightInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Pak_Chn_Tw_Light>(new ConditionCollection<Pak_Chn_Tw_Light>(new EqualCondition<Pak_Chn_Tw_Light>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pak_Chn_Tw_Light, PakChnTwLightInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetMinSnoIdByTpAndPno(string tp, string pno)
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
                        mtns::Pak_Locmas cond = new mtns::Pak_Locmas();
                        cond.tp = tp;
                        cond.pno = pno;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Pak_Locmas>(tk, "TOP 1", new string[] { mtns::Pak_Locmas.fn_snoId }, new ConditionCollection<mtns::Pak_Locmas>(new EqualCondition<mtns::Pak_Locmas>(cond)), string.Format("CONVERT(INT,{0})", mtns::Pak_Locmas.fn_snoId));
                    }
                }
                sqlCtx.Param(mtns::Pak_Locmas.fn_tp).Value = tp;
                sqlCtx.Param(mtns::Pak_Locmas.fn_pno).Value = pno;
 
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret =  g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Locmas.fn_snoId));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAResultForConsolidatedByPalletNo(string palletNo)
        {
            try
            {
                int ret = -1;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                        sqlCtx.Sentence = "DECLARE @reg char(5), @jcid char(20), @shiptp char(5), @country varchar(30) " + 
                                            "SET @reg = '' " +
                                            "SET @jcid = '' " +
                                            "SET @shiptp = '' " +
                                            "SET @country = '' " +
                                            //"DROP TABLE #dn4 " +
                                            "CREATE TABLE #dn4 (DeliveryNo varchar(20) COLLATE Chinese_Taiwan_Stroke_BIN,Consolidate varchar(50) COLLATE Chinese_Taiwan_Stroke_BIN) " +
                                            "INSERT #dn4 " + 
                                                "SELECT a.DeliveryNo, ISNULL(b.InfoValue, '') FROM Delivery_Pallet a (NOLOCk) LEFT JOIN DeliveryInfo b (NOLOCK) " + 
                                                   "ON a.DeliveryNo = b.DeliveryNo AND b.InfoType = 'Consolidated' " + 
                                                   "WHERE a.PalletNo = @PalletNo " + 

                                            "IF EXISTS (SELECT DeliveryNo FROM #dn4 WHERE Consolidate<>'') " + 
                                            "BEGIN " + 
                                                "SELECT @reg = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'RegId' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate <> '' " + 
                                                "SELECT @jcid = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'ConfigID' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate <> '' " + 
                                                "SELECT @shiptp = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'ShipTp' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate <> '' " + 
                                                "SELECT @country = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'Country' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate <> '' " + 

                                                //"SELECT @reg, @jcid, @shiptp, @country " + 
                                                "IF RTRIM(@reg)='SNA' " + 
                                                   "or (RTRIM(@reg)='SAF' AND RTRIM(@shiptp)='BTO') " + 
                                                   "or (RTRIM(@reg)='SAF' AND RTRIM(@shiptp)='CTO' AND RTRIM(@country)<>'Japan') " + 
                                                   "or RTRIM(@reg)='SCN' " + 
                                                   "or (RTRIM(@reg) in ('SNU','SUC') AND RTRIM(@shiptp)='CTO') " + 
                                                   "or RTRIM(@reg)='SNL' " + 
                                                "BEGIN " + 
                                                   "SELECT 1 " + 
                                                   "RETURN " + 
                                                "END " + 
                                                "ELSE " + 
                                                "BEGIN " + 
                                                   "SELECT 0 " + 
                                                   "RETURN " + 
                                                "END " + 
                                            "END " + 
                                            "ELSE " + 
                                            "BEGIN " + 
                                                "SELECT @reg = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'RegId' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate = '' " + 
                                                "SELECT @jcid = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'ConfigID' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate = '' " + 
                                                "SELECT @shiptp = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'ShipTp' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate = '' " + 
                                                "SELECT @country = InfoValue FROM DeliveryInfo a (Nolock), #dn4 b (nolock) " + 
                                                   "WHERE InfoType = 'Country' AND a.DeliveryNo = b.DeliveryNo AND b.Consolidate = '' " + 
                                                "IF LEFT(RTRIM(@PalletNo),2)='NA' " + 
                                                "BEGIN " + 
                                                   "SELECT 1 " + 
                                                   "RETURN " + 
                                                "END " + 
                                                "ELSE " + 
                                                "BEGIN " + 
                                                   "IF (RTRIM(@reg) in ('SNU','SUC') AND RTRIM(@shiptp)='BTO') " + 
                                                       "or RTRIM(@reg)='SNE' " + 
                                                       "or (RTRIM(@reg)='SAF' AND @country='Japan' AND @shiptp='CTO') " + 
                                                   "BEGIN " + 
                                                       "SELECT 0 " + 
                                                       "RETURN " + 
                                                   "END " + 
                                                   "ELSE " + 
                                                   "BEGIN " + 
                                                       "SELECT 1 " + 
                                                       "RETURN " + 
                                                   "END " +
                                                "END " + 
                                            "END";

                        sqlCtx.AddParam(_Metas.Pallet.fn_palletNo, new SqlParameter("@" + _Metas.Pallet.fn_palletNo, ToolsNew.GetDBFieldType<_Metas.Pallet>(_Metas.Pallet.fn_palletNo)));
                    }
                }
                sqlCtx.Param(mtns::Pallet.fn_palletNo).Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Int32(sqlR, 0);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetChepPalletQty(string pickId, string status)
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
                        tf1 = new TableAndFields<_Metas.FwdPlt>();
                        _Metas.FwdPlt cond = new _Metas.FwdPlt();
                        cond.pickID = pickId;
                        cond.status = status;
                        tf1.Conditions.Add(new EqualCondition<_Metas.FwdPlt>(cond));
                        _Metas.FwdPlt cond2 = new _Metas.FwdPlt();
                        cond2.udt = DateTime.Now;
                        tf1.Conditions.Add(new AnySoloCondition<_Metas.FwdPlt>(cond2, "CONVERT(CHAR(10),{0},111)=CONVERT(CHAR(10),GETDATE(),111)"));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.Pallet_RFID>();
                        tf2.AddRangeToGetFuncedFieldNames(new string[] { _Metas.Pallet_RFID.fn_plt, string.Format("DISTINCT {0}", _Metas.Pallet_RFID.fn_plt) });

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.FwdPlt, _Metas.Pallet_RFID>(tf1, _Metas.FwdPlt.fn_plt, tf2, _Metas.Pallet_RFID.fn_plt));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("FROM FwdPlt t1,Pallet_RFID t2 WHERE t1.Plt=t2.PLT AND ", "FROM FwdPlt t1 LEFT JOIN Pallet_RFID t2 ON t1.Plt=t2.PLT WHERE ");
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.FwdPlt.fn_pickID)).Value = pickId;
                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.FwdPlt.fn_status)).Value = status;

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

        public bool PickCardCheck(string truckID, string strDate)
        {
            try
            {
                bool ret = false;
                DataTable res = null;
                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@TruckID", SqlDbType.VarChar);
                paramsArray[0].Value = truckID;
                paramsArray[1] = new SqlParameter("@Date", SqlDbType.VarChar);
                paramsArray[1].Value = strDate;
                res = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "IMES_PickCardCheck", paramsArray);
                if (res != null && res.Rows != null && res.Rows.Count > 0)
                {
                    string flag = Convert.ToString(res.Rows[0][0]);
                    ret = flag == "0" ? false : true;
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PltstandardInfo> GetPltstandardInfoList(PltstandardInfo condition)
        {
            try
            {
                IList<PltstandardInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pltstandard cond = mtns::FuncNew.SetColumnFromField<mtns::Pltstandard, PltstandardInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pltstandard>(null, null, new mtns::ConditionCollection<mtns::Pltstandard>(new mtns::EqualCondition<mtns::Pltstandard>(cond)), mtns::Pltstandard.fn_pltno);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pltstandard, PltstandardInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pltstandard, PltstandardInfo, PltstandardInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PltspecificationInfo> GetPltspecificationInfoList(PltspecificationInfo condition)
        {
            try
            {
                IList<PltspecificationInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pltspecification cond = mtns::FuncNew.SetColumnFromField<mtns::Pltspecification, PltspecificationInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pltspecification>(null, null, new mtns::ConditionCollection<mtns::Pltspecification>(new mtns::EqualCondition<mtns::Pltspecification>(cond)), mtns::Pltspecification.fn_id);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pltspecification, PltspecificationInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pltspecification, PltspecificationInfo, PltspecificationInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePltstandardInfo(PltstandardInfo setValue, PltstandardInfo condition)
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
                mtns::Pltstandard cond = mtns::FuncNew.SetColumnFromField<mtns::Pltstandard, PltstandardInfo>(condition);
                mtns::Pltstandard setv = mtns::FuncNew.SetColumnFromField<mtns::Pltstandard, PltstandardInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Pltstandard>(new mtns::SetValueCollection<mtns::Pltstandard>(new mtns::CommonSetValue<mtns::Pltstandard>(setv)), new mtns::ConditionCollection<mtns::Pltstandard>(new mtns::EqualCondition<mtns::Pltstandard>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pltstandard, PltstandardInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pltstandard, PltstandardInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Pltstandard.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPltstandardInfo(PltstandardInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pltstandard>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pltstandard, PltstandardInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Pltstandard.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Pltstandard.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePltstandardInfo(PltstandardInfo condition)
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
                Pltstandard cond = FuncNew.SetColumnFromField<Pltstandard, PltstandardInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Pltstandard>(new ConditionCollection<Pltstandard>(new EqualCondition<Pltstandard>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pltstandard, PltstandardInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PalletLogInfo> GetPalletLogInfoList(PalletLogInfo condition)
        {
            try
            {
                IList<PalletLogInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::PalletLog cond = mtns::FuncNew.SetColumnFromField<mtns::PalletLog, PalletLogInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::PalletLog>(null, null, new mtns::ConditionCollection<mtns::PalletLog>(new mtns::EqualCondition<mtns::PalletLog>(cond)), mtns::PalletLog.fn_cdt + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PalletLog, PalletLogInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::PalletLog, PalletLogInfo, PalletLogInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePalletLog(PalletLogInfo condition)
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
                mtns.PalletLog cond = FuncNew.SetColumnFromField<mtns.PalletLog, PalletLogInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<mtns.PalletLog>(new ConditionCollection<mtns.PalletLog>(new EqualCondition<mtns.PalletLog>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns.PalletLog, PalletLogInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetFlListFromPakLacMas(PakLocMasInfo condition)
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
                mtns::Pak_Locmas cond = mtns::FuncNew.SetColumnFromField<mtns::Pak_Locmas, PakLocMasInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pak_Locmas>("DISTINCT", new string[] { mtns::Pak_Locmas.fn_fl }, new mtns::ConditionCollection<mtns::Pak_Locmas>(new mtns::EqualCondition<mtns::Pak_Locmas>(cond)), mtns::Pak_Locmas.fn_fl);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pak_Locmas, PakLocMasInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Pak_Locmas.fn_fl));
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

        public bool CheckAllPalletWeightOnDeliverysByPallet(string palletno)
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
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                        sqlCtx.Sentence = " select COUNT(PalletNo) from {0}..{1} where "
                                           + " PalletNo in ( select PalletNo from {0}..{2}"
                                           + " where DeliveryNo in (select DeliveryNo from {0}..{2}"
                                           + " where {3} =@{3})) and Weight<>0";
      
                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, 
                                                _Schema.SqlHelper.DB_PAK,
                                                ToolsNew.GetTableName(typeof(_Metas.Pallet)),
                                                ToolsNew.GetTableName(typeof(_Metas.Delivery_Pallet)),
                                                 _Metas.Delivery_Pallet.fn_palletNo
                                                );

                        sqlCtx.AddParam(_Metas.Delivery_Pallet.fn_palletNo, new SqlParameter("@" + _Metas.Delivery_Pallet.fn_palletNo, ToolsNew.GetDBFieldType<_Metas.Delivery_Pallet>(_Metas.Delivery_Pallet.fn_palletNo)));
                    }
                }
                sqlCtx.Param(_Metas.Delivery_Pallet.fn_palletNo).Value = palletno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, 0);
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

        

        #region . OnTrans .

        public IMES.FisObject.PAK.Pallet.Pallet Find_OnTrans(object key)
        {
            SqlDataReader sqlR = null;
            try
            {
                IMES.FisObject.PAK.Pallet.Pallet ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pallet cond = new _Schema.Pallet();
                        cond.PalletNo = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (INDEX=Pallet_PK,ROWLOCK,UPDLOCK) WHERE"); 
                    }
                }
                sqlCtx.Params[_Schema.Pallet.fn_PalletNo].Value = (string)key;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = new IMES.FisObject.PAK.Pallet.Pallet();
                    ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Cdt]);
                    ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Editor]);
                    ret.Height = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Height]);
                    ret.Length = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Length]);
                    ret.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletNo]);
                    ret.PalletModel = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletModel]);
                    ret.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Station]);
                    ret.UCC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_UCC]);
                    ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Udt]);
                    ret.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Weight]);
                    ret.Width = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Width]);
                    ret.Weight_L = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_weight_L]);
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

        #endregion

        #region . Defered .

        public void AddGetChepPalletInfoDefered(IUnitOfWork uow, ChepPalletInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteChepPalletInfoDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddQtyInfoDefered(IUnitOfWork uow, PalletQtyInfo pqInfo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pqInfo);
        }

        public void DeleteQtyInfoDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void UpdateQtyInfoDefered(IUnitOfWork uow, PalletQtyInfo pqInfo, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pqInfo, id);
        }

        public void UpdateStatusForPakBtLocMasDefered(IUnitOfWork uow, string status, string snoId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), status, snoId);
        }

        public void UpdateStatusForPakBtLocMasDefered(IUnitOfWork uow, string status, string snoId, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), status, snoId, editor);
        }

        public void InsertSnoDetBtLocInfoDefered(IUnitOfWork uow, SnoDetBtLocInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateForIncPakBtLocMasDefered(IUnitOfWork uow, string snoId, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), snoId, editor);
        }

        public void UpdateForIncPakBtLocMasDefered(IUnitOfWork uow, string snoId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), snoId);
        }

        public void UnPackPakOdmSessionByDeliveryNoDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void UnPackPackingDataByDeliveryNoDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void InsertWhPltLogDefered(IUnitOfWork uow, WhPltLogInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateWhPltMasDefered(IUnitOfWork uow, WhPltMasInfo item, string plt)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, plt);
        }

        public void InsertWhPltMasDefered(IUnitOfWork uow, WhPltMasInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePakWhPltTypeByPltDefered(IUnitOfWork uow, string plt)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), plt);
        }

        public void InsertPakWhPltTypeInfoDefered(IUnitOfWork uow, PakWhPltTypeInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdatePakWhLocBolByColAndLocDefered(IUnitOfWork uow, string bol, string col, int loc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), bol, col, loc);
        }

        public void InsertWhPltLocLogInfoDefered(IUnitOfWork uow, WhPltLocLogInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdatePakWhLocByColAndLocDefered(IUnitOfWork uow, PakWhLocMasInfo item, string col, int loc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, col, loc);
        }

        public void UpdatePakWhLocByPltForClearPlt1AndPlt2Defered(IUnitOfWork uow, string plt)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), plt);
        }

        public void InsertPoPltDefered(IUnitOfWork uow, PoPltInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertPoDataDefered(IUnitOfWork uow, PoDataInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePalletsByDnDefered(IUnitOfWork uow, string dn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn);
        }

        public void DeletePalletsByShipmentNoDefered(IUnitOfWork uow, string shipmentNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), shipmentNo);
        }

        public void UpdatePakWhLocByPltForClearBolDefered(IUnitOfWork uow, string bol, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), bol, editor);
        }

        public void UpdatePakWhLocByPltForClearBolAndPlt1AndPlt2Defered(IUnitOfWork uow, string palletNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletNo);
        }

        public void UpdatePakWhLocTmpForClearPltAndTpAndBolDefered(IUnitOfWork uow, string palletNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletNo);
        }

        public void UpdatePakWhLocTmpForClearBolDefered(IUnitOfWork uow, string bol)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), bol);
        }

        public void DeleteChepPalletInfoDefered(IUnitOfWork uow, string palletNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletNo);
        }

        public void DeletePalletRfidDefered(IUnitOfWork uow, string plt, string rfidCode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), plt, rfidCode);
        }

        public void InsertPalletRfidDefered(IUnitOfWork uow, PalletRfidInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdatePakLocMasForPdLineDefered(IUnitOfWork uow, string pdLine, string pno, string tp)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pdLine, pno, tp);
        }

        public void UpdatePakLocMasForPdLineAndPnoDefered(IUnitOfWork uow, string pdLine, string newPno, string oldPno, string tp, string snoId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pdLine, newPno, oldPno, tp, snoId);
        }

        public void UpdatePakLocMasForPnoDefered(IUnitOfWork uow, string newPno, string oldPno, string tp)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), newPno, oldPno, tp);
        }

        public void InsertPakOdmSessionDefered(IUnitOfWork uow, PakOdmSessionInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateWhPltWeightDefered(IUnitOfWork uow, WhPltWeightInfo setValue, WhPltWeightInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateKitLocationFVOnDefered(IUnitOfWork uow, string tagid, bool configstatus, bool runningStatus, string ledvalue)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), tagid, configstatus, runningStatus, ledvalue);
        }

        public void UpdateKitLocationFVOnDefered(IUnitOfWork uow, string[] tagids, bool configstatus, bool runningStatus, string ledvalue)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), tagids, configstatus, runningStatus, ledvalue);
        }

        public void UpdateKitLocationFVOffDefered(IUnitOfWork uow, string tagid, bool configstatus, bool runningStatus)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), tagid, configstatus, runningStatus);
        }

        public void UpdateKittingLocationFaXInfoDefered(IUnitOfWork uow, KittingLocationFaXInfo setValue, int configedLEDStatus, int runningLEDStatus, int comm, KittingLocationFaXInfo condition, int[] proritySet)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, configedLEDStatus, runningLEDStatus, comm, condition, proritySet);
        }

        public void CallOp_PackingDataDefered(IUnitOfWork uow, string snoid, string model, string dn, string plt, string loc, int pltqty, string boxid, string ucc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), snoid, model, dn, plt, loc, pltqty, boxid, ucc);
        }

        public void CallOp_PackingDataDefered(IUnitOfWork uow, string boxid, string dn, string plt, string cn, DateTime cdtEdi)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), boxid, dn, plt, cn, cdtEdi);
        }

        public void RemoveCOMSettingItemDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddCOMSettingItemDefered(IUnitOfWork uow, COMSettingInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateCOMSettingItemDefered(IUnitOfWork uow, COMSettingInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePalletsDefered(IUnitOfWork uow, IList<string> pltNos)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pltNos);
        }

        public void UpdatePakLocMasInfoDefered(IUnitOfWork uow, PakLocMasInfo setValue, PakLocMasInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeleteDummyDetInfoByPltDefered(IUnitOfWork uow, string palletNo)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletNo);
        }

        public void UpdateSnoDetBtLocDefered(IUnitOfWork uow, SnoDetBtLocInfo setValue, SnoDetBtLocInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdatePakBtLocMasDefered(IUnitOfWork uow, PakBtLocMasInfo setValue, PakBtLocMasInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void InsertPakChnTwLightInfoDefered(IUnitOfWork uow, PakChnTwLightInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdatePakChnTwLightInfoDefered(IUnitOfWork uow, PakChnTwLightInfo setValue, PakChnTwLightInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeletePakChnTwLightInfoDefered(IUnitOfWork uow, PakChnTwLightInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void RemoveFwdPltByPickIDDefered(IUnitOfWork uow, string pickID)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pickID);
        }

        public void DeletePalletAttrsDefered(IUnitOfWork uow, IList<string> pltNos)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pltNos);
        }

        public void UpdatePltstandardInfoDefered(IUnitOfWork uow, PltstandardInfo setValue, PltstandardInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddPltstandardInfoDefered(IUnitOfWork uow, PltstandardInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePltstandardInfoDefered(IUnitOfWork uow, PltstandardInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void DeletePalletLogDefered(IUnitOfWork uow, PalletLogInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        #endregion

        #region MIC for another update function
        public IList<IMES.FisObject.PAK.Pallet.PalletAttr> GetPalletAttr(string palletNo)
        {
            try
            {
                IList<IMES.FisObject.PAK.Pallet.PalletAttr> ret = new List<IMES.FisObject.PAK.Pallet.PalletAttr>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select AttrName, PalletNo, AttrValue, Editor, Cdt, Udt
                                                               from PalletAttr
                                                              where  PalletNo=@PalletNo
                                                              order by AttrName";
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<IMES.FisObject.PAK.Pallet.PalletAttr>(sqlR));                       

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IMES.FisObject.PAK.Pallet.PalletAttr GetPalletAttr(string palletNo, string attrName)
        {
            try
            {
                IMES.FisObject.PAK.Pallet.PalletAttr ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select AttrName, PalletNo, AttrValue, Editor, Cdt, Udt
                                                               from PalletAttr
                                                              where  PalletNo = @PalletNo and
                                                                          AttrName = @AttrName";
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;
                sqlCtx.Param("AttrName").Value = attrName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret= IMES.Infrastructure.Repository._Schema.SQLData.ToObject<IMES.FisObject.PAK.Pallet.PalletAttr>(sqlR);

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.FisObject.PAK.Pallet.PalletAttrLog> GetPalletAttrLog(string palletNo)
        {
            try
            {
                IList<IMES.FisObject.PAK.Pallet.PalletAttrLog> ret = new List<IMES.FisObject.PAK.Pallet.PalletAttrLog>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select PalletNo, PalletModel, AttrName, AttrOldValue, AttrNewValue, 
                                                                        Descr, Editor, Cdt
                                                              from PalletAttrLog
                                                              where  PalletNo=@PalletNo
                                                              order by Cdt";
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<IMES.FisObject.PAK.Pallet.PalletAttrLog>(sqlR));

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<IMES.FisObject.PAK.Pallet.PalletAttrLog> GetPalletAttrLog(string palletNo, string attrName)
        {
            try
            {
                IList<IMES.FisObject.PAK.Pallet.PalletAttrLog> ret = new List<IMES.FisObject.PAK.Pallet.PalletAttrLog>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select PalletNo, PalletModel, AttrName, AttrOldValue, AttrNewValue, 
                                                                        Descr, Editor, Cdt
                                                              from PalletAttrLog
                                                              where  PalletNo=@PalletNo and
                                                                          AttrName =@AttrName
                                                              order by Cdt";
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;
                sqlCtx.Param("AttrName").Value = attrName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<IMES.FisObject.PAK.Pallet.PalletAttrLog>(sqlR));

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public void UpdateAttr(string palletNo, string attrName, string attrValue, string descr, string editor)
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
                        sqlCtx.Sentence = @"insert PalletAttrLog(PalletNo, PalletModel, AttrName, AttrOldValue, AttrNewValue, Descr, Editor, Cdt)
                                                            select a.*
                                                            from (MERGE INTO PalletAttr as Target 
                                                                  Using (select @AttrName, @PalletNo)
                                                                         as Source (AttrName, PalletNo)
	                                                              ON Target.PalletNo = Source.PalletNo and
		                                                              Target.AttrName = Source.AttrName
	                                                            WHEN NOT MATCHED THEN
		                                                            insert (AttrName, PalletNo, AttrValue, Editor, Cdt, Udt)		 		
		                                                            values( @AttrName, @PalletNo, @AttrValue, @Editor, getdate(), getdate())
	                                                            WHEN MATCHED THEN
		                                                             update set AttrValue= @AttrValue,
				                                                                 Editor =@Editor,
				                                                                 Udt=getdate()
	                                                             OUTPUT INSERTED.PalletNo,'' as Model, INSERTED.AttrName, isnull(deleted.AttrValue,'') as oldAttrValue , 
	                                                                    INSERTED.AttrValue, @Descr as Descr,@Editor as Editor,INSERTED.Cdt as Cdt) a;";
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));                        


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;
                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("AttrValue").Value = attrValue;
                sqlCtx.Param("Descr").Value = descr;
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
        public void UpdateAttrDefered(IUnitOfWork uow, string palletNo, string attrName, string attrValue, string descr, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), palletNo, attrName, attrValue,descr, editor);
        }

        public IList<FwdPltInfo> GetFwdPltInfosByPickIDAndStatusAndDate(string pickID, string status, DateTime date, string notAllowStatus)
        {
            try
            {

                IList<FwdPltInfo> ret = new List<FwdPltInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" select a.PickID, a.Plt, a.Qty, a.Status, a.Date, a.Operator, a.Cdt, a.Udt, a.ID
                                                          from  FwdPlt a
                                                          where  a.PickID =@PickID    and
                                                                 a.Status  =@Status   and
                                                                 a.Date =@Date        and
                                                                 not exists(select 1 from  FwdPlt b where b.Plt =a.Plt and b.Status=@NotAllowStatus) 
                                                        order by a.Plt";
                        sqlCtx.AddParam("PickID", new SqlParameter("@PickID", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("Date", new SqlParameter("@Date", SqlDbType.VarChar));
                        sqlCtx.AddParam("NotAllowStatus", new SqlParameter("@NotAllowStatus", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PickID").Value = pickID;
                sqlCtx.Param("Status").Value = status;
                sqlCtx.Param("Date").Value = date.ToString("yyyy/MM/dd");
                sqlCtx.Param("NotAllowStatus").Value = notAllowStatus;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObject<FwdPltInfo>(sqlR));

                    }
                    return ret;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePakLocMasById(int id, string palletNo, string pdLine, string editor)
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
                        Pak_Locmas cond = new Pak_Locmas();
                        cond.id = id;                       
                        Pak_Locmas setv = new Pak_Locmas();
                        setv.pdLine = pdLine;
                        setv.pno = palletNo;
                        setv.editor = editor;
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<Pak_Locmas>(tk, new SetValueCollection<Pak_Locmas>(new CommonSetValue<Pak_Locmas>(setv)), new ConditionCollection<Pak_Locmas>(new EqualCondition<Pak_Locmas>(cond)));
                    }
                }
                sqlCtx.Param(Pak_Locmas.fn_id).Value = id;
                
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_pdLine)).Value = pdLine;
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_pno)).Value = palletNo;
                sqlCtx.Param(g.DecSV(Pak_Locmas.fn_editor)).Value = editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdatePakLocMasByIdDefered(IUnitOfWork uow, int id, string palletNo, string pdLine, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),  id , palletNo, pdLine, editor);
        }

        #endregion

        #region get Docking Pallet
        public IMES.FisObject.PAK.Pallet.Pallet FindWithDocking(string PalletNo)
        {
            try
            {
                IMES.FisObject.PAK.Pallet.Pallet ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.Pallet cond = new _Schema.Pallet();
                        cond.PalletNo = PalletNo;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Pallet), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.Pallet.fn_PalletNo].Value = PalletNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_Docking, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new IMES.FisObject.PAK.Pallet.Pallet();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Editor]);
                        ret.Height = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Height]);
                        ret.Length = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Length]);
                        ret.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletNo]);
                        ret.PalletModel = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_PalletModel]);
                        ret.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Station]);
                        ret.UCC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_UCC]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Udt]);
                        ret.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Weight]);
                        ret.Width = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Width]);
                        ret.Weight_L = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_weight_L]);
                        ret.Floor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Pallet.fn_Floor]);
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
        #endregion

    }
}
