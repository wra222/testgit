﻿//2010-01-06  itc200008     Modify ITC-1103-0012
//2010-03-08  itc207031     Modify ITC-1122-0207 
//2012-02-02  itc207031     Modify ITC-1360-0213

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using fomb = IMES.FisObject.PCA.MB;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository._Schema;
//
using IMES.Infrastructure.Util;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.PCA
{
    public class MBRepository : BaseRepository<IMB>, IMBRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Link To Other
        private static IDefectInfoRepository _dfciRepository = null;
        private static IDefectInfoRepository DfciRepository
        {
            get
            {
                if (_dfciRepository == null)
                    _dfciRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository, IMES.FisObject.Common.Defect.DefectInfo>();
                return _dfciRepository;
            }
        }

        private static IDefectRepository _dfctRepository = null;
        private static IDefectRepository DfctRepository
        {
            get
            {
                if (_dfctRepository == null)
                    _dfctRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
                return _dfctRepository;
            }
        }

        private static IPartRepository _ptRepository = null;
        private static IPartRepository PtRepository
        {
            get
            {
                if (_ptRepository == null)
                    _ptRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                return _ptRepository;
            }
        }

        private static IStationRepository _sttRepository = null;
        private static IStationRepository SttRepository
        {
            get
            {
                if (_sttRepository == null)
                    _sttRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository, IStation>();
                return _sttRepository;
            }
        }
        #endregion

        #region get private FieldInfo in PCB

        static readonly FieldInfo _info = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_info", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _logs = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_logs", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _part = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_part", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _rptRepair = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_rptRepair", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _modelObj = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_modelObj", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _attributeLogs = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_attributeLogs", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _attributes = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _repairDefects = (typeof(IMES.FisObject.Common.Repair.Repair)).GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _repair = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_repair", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _status = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _testLogDefects = (typeof(IMES.FisObject.Common.TestLog.TestLog)).GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo _testLogs = (typeof(IMES.FisObject.PCA.MB.MB)).GetField("_testLogs", BindingFlags.NonPublic | BindingFlags.Instance);
        #endregion

        #region Link To Delivery
        private static IMES.FisObject.PAK.DN.IDeliveryRepository _dnRep = null;
        private static IMES.FisObject.PAK.DN.IDeliveryRepository DeliveryRep
        {
            get
            {
                if (_dnRep == null)
                    _dnRep = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.PAK.DN.IDeliveryRepository>();
                return _dnRep;
            }
        }
        #endregion

        #region Overrides of BaseRepository<IMB>

        protected override void PersistNewItem(IMB item)
        {
            StateTracker tracker = (item as MB).Tracker;
            try
            {
                //persist MB object
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertMB(item);

                    //2010-03-08  itc207031     Modify ITC-1122-0207 
                    MBStatus mbStt = (MBStatus)item.GetType().GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (mbStt != null)
                        this.PersistInsertMBStatus(item.Sn, mbStt);

                    this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(IMB item)
        {
            StateTracker tracker = (item as MB).Tracker;
            try
            {
                //persist MB object
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateMB(item);

                    //persist MBStatus
                    //2010-03-08  itc207031     Modify ITC-1122-0207 
                    MBStatus mbStt = (MBStatus)item.GetType().GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (mbStt != null)
                    {
                        if (tracker.GetState(mbStt) == DataRowState.Modified || tracker.GetState(mbStt) == DataRowState.Added)
                        {
                            this.PersistUpdateMBStatus(item.Sn, mbStt);
                        }
                    }

                    this.CheckAndInsertSubs(item, tracker);

                    this.CheckAndUpdateSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(IMB item)
        {
            StateTracker tracker = (item as MB).Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteMBLogByPCBID(item.Sn);
                    this.PersistDeleteMBStatus(item.MBStatus);
                    this.PersistDeleteMB(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<IMB>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IMB Find(object key)
        {
            try
            {
                IMB ret = null;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB cond = new PCB();
                        cond.PCBNo = (string)key;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), cond, null, null);
                    }
                }
                sqlCtx.Params[PCB.fn_PCBNo].Value = (string)key;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MB(
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));
                        //ret.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                        //ret.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                        //ret.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                        //ret.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                        //ret.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                        //ret.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                        //ret.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                        //ret.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                        //ret.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                        addMBRemaingPropertyValue(ret, sqlR, sqlCtx);
                        ((MB)ret).Tracker.Clear();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IMB Find_OnTrans(object key)
        {
            SqlDataReader sqlR = null;
            try
            {
                IMB ret = null;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB cond = new PCB();
                        cond.PCBNo = (string)key;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), cond, null, null);
                    }
                }
                sqlCtx.Params[PCB.fn_PCBNo].Value = (string)key;
                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                if (sqlR != null && sqlR.Read())
                {
                    ret = new MB(
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));
                    //ret.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                    //ret.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                    //ret.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                    //ret.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                    //ret.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                    //ret.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                    //ret.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                    //ret.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                    //ret.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                    addMBRemaingPropertyValue(ret, sqlR, sqlCtx);
                    ((MB)ret).Tracker.Clear();
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

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public override IList<IMB> FindAll()
        {
            try
            {
                IList<IMB> ret = new List<IMB>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB));
                    }
                }
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMB item = new MB(
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));
                                //item.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                                //item.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                                //item.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                                //item.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                                //item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                                //item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                                //item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                                //item.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                                //item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                                addMBRemaingPropertyValue(item, sqlR, sqlCtx);
                            ((MB)item).Tracker.Clear();
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
        /// <param name="uow"></param>
        public override void Add(IMB item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(IMB item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(IMB item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region Implementation of IMBRepository

        #region . Filling Method .

        public IMB FillMBLogs(IMB mb)
        {
            try
            {
                IList<MBLog> newFieldVal = new List<MBLog>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBLog cond = new PCBLog();
                        cond.PCBID = mb.Sn;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBLog), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBLog.fn_PCBID].Value = mb.Sn;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MBLog mblog = new MBLog(
                        GetValue_Int32(sqlR, sqlCtx.Indexes[PCBLog.fn_ID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCBLog.fn_PCBID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCBLog.fn_PCBModelID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCBLog.fn_StationID]),
                        GetValue_Int32(sqlR, sqlCtx.Indexes[PCBLog.fn_Status]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCBLog.fn_LineID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCBLog.fn_Editor]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBLog.fn_Cdt]));

                        mblog.Tracker.Clear();
                        mblog.Tracker = ((MB)mb).Tracker;
                        newFieldVal.Add(mblog);
                    }
                }
                //mb.GetType().GetField("_logs",BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _logs.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMB FillMBInfos(IMB mb)
        {
            try
            {
                IList<IMES.FisObject.PCA.MB.MBInfo> newFieldVal = new List<IMES.FisObject.PCA.MB.MBInfo>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBInfo cond = new PCBInfo();
                        cond.PCBID = mb.Sn;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBInfo.fn_PCBID].Value = mb.Sn;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.PCA.MB.MBInfo mbinfo = new IMES.FisObject.PCA.MB.MBInfo(GetValue_Int32(sqlR, sqlCtx.Indexes[PCBInfo.fn_ID]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_PCBID]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_InfoType]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_InfoValue]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_Editor]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBInfo.fn_Cdt]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBInfo.fn_Udt]));

                        mbinfo.Tracker.Clear();
                        mbinfo.Tracker = ((MB)mb).Tracker;
                        newFieldVal.Add(mbinfo);
                    }
                }

                //mb.GetType().GetField("_info", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _info.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMB FillMBParts(IMB mb)
        {
            try
            {
                IList<IProductPart> newFieldVal = new List<IProductPart>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB_Part cond = new PCB_Part();
                        cond.pcbno = mb.Sn;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[PCB_Part.fn_pcbno].Value = mb.Sn;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IProductPart mbpart = new ProductPart(GetValue_Int32(sqlR, sqlCtx.Indexes[PCB_Part.fn_id]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partNo]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_pcbno]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_station]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partType]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_custmerPn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_iecpn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partSn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_editor]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB_Part.fn_cdt]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB_Part.fn_udt]));
                        ((ProductPart)mbpart).BomNodeType = GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_bomNodeType]);
                        ((ProductPart)mbpart).CheckItemType = GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_checkItemType]);
                        ((ProductPart)mbpart).Tracker.Clear();
                        ((ProductPart)mbpart).Tracker = ((MB)mb).Tracker;
                        newFieldVal.Add(mbpart);
                    }
                }

                //mb.GetType().GetField("_part", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _part.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 晚加载RptRepairs
        /// select * from rpt_PCARep where SnoId=@MBSNo order by Cdt
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public IMB FillMBRptRepairs(IMB mb)
        {
            try
            {
                IList<MBRptRepair> newFieldVal = new List<MBRptRepair>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Rpt_PCARep cond = new Rpt_PCARep();
                        cond.snoId = mb.Sn;
                        sqlCtx = FuncNew.GetConditionedSelect<Rpt_PCARep>(tk, null, null, new ConditionCollection<Rpt_PCARep>(new EqualCondition<Rpt_PCARep>(cond)));
                    }
                }
                sqlCtx.Param(Rpt_PCARep.fn_snoId).Value = mb.Sn;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MBRptRepair repi = new MBRptRepair
                            (
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_snoId)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_tp)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_remark)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_status)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_mark)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_username)),
                            g.GetValue_DateTime(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_cdt)),
                            g.GetValue_DateTime(sqlR, sqlCtx.Indexes(Rpt_PCARep.fn_udt))
                            );

                        repi.Tracker.Clear();
                        repi.Tracker = ((MB)mb).Tracker;
                        newFieldVal.Add(repi);
                    }
                }
                //mb.GetType().GetField("_rptRepair", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _rptRepair.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMB FillRepairs(IMB mb)
        {
            try
            {
                IList<Repair> newFieldVal = new List<Repair>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBRepair cond = new PCBRepair();
                        cond.PCBID = mb.Sn;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBRepair), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBRepair.fn_PCBID].Value = mb.Sn;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        Repair repi = new Repair(
                            GetValue_Int32(sqlR, sqlCtx.Indexes[PCBRepair.fn_ID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair.fn_PCBID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair.fn_PCBModelID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair.fn_Type]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair.fn_LineID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair.fn_StationID]),
                            (Repair.RepairStatus)Enum.Parse(typeof(Repair.RepairStatus), GetValue_Int32(sqlR, sqlCtx.Indexes[PCBRepair.fn_Status]).ToString()),
                            //GetValue_Int32(sqlR, sqlCtx.Indexes[PCBRepair.fn_ReturnID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair.fn_Editor]),
                            GetValue_Int32_ToStringWithNull(sqlR, sqlCtx.Indexes[PCBRepair.fn_TestLogID]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ProductRepair.fn_logID]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBRepair.fn_Cdt]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBRepair.fn_Udt]));

                        repi.Tracker.Clear();
                        repi.Tracker = ((MB)mb).Tracker;
                        repi.FillingRepairDefects += new FillRepair(MB.FillingRepairDefects);
                        newFieldVal.Add(repi);
                    }
                }

                //mb.GetType().GetField("_repair", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _repair.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMB FillTestLogs(IMB mb)
        {
            try
            {
                IList<TestLog> newFieldVal = new List<TestLog>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBTestLog cond = new PCBTestLog();
                        cond.PCBID = mb.Sn;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBTestLog), cond, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderByDesc, PCBTestLog.fn_Cdt);
                    }
                }
                sqlCtx.Params[PCBTestLog.fn_PCBID].Value = mb.Sn;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TestLog tstLg = new TestLog(
                                            GetValue_Int32(sqlR, sqlCtx.Indexes[PCBTestLog.fn_ID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_PCBID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_LineID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_FixtureID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_StationID]),
                                            (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), GetValue_Int32(sqlR, sqlCtx.Indexes[PCBTestLog.fn_Status]).ToString()),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_JoinID]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_ActionName]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_ErrorCode]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_Descr]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_Editor]),
                                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_Type]),
                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBTestLog.fn_Cdt]));
                        tstLg.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog.fn_remark]);

                        tstLg.Tracker.Clear();
                        tstLg.Tracker = ((MB)mb).Tracker;
                        tstLg.FillingTestLogDefects += new FillTestLog(MB.FillingTestLogDefects);
                        newFieldVal.Add(tstLg);
                    }
                }

                //mb.GetType().GetField("_testLogs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _testLogs.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMB FillStatus(IMB mb)
        {
            try
            {
                MBStatus newFieldVal = null;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBStatus cond = new PCBStatus();
                        cond.PCBID = mb.Sn;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBStatus.fn_PCBID].Value = mb.Sn;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        newFieldVal = new MBStatus(
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBStatus.fn_PCBID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBStatus.fn_StationID]),
                            (MBStatusEnum)Enum.Parse(typeof(MBStatusEnum), GetValue_Int32(sqlR, sqlCtx.Indexes[PCBStatus.fn_Status]).ToString()),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[PCBStatus.fn_TestFailCount]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBStatus.fn_Editor]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBStatus.fn_LineID]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBStatus.fn_Udt]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBStatus.fn_Cdt]));

                        newFieldVal.Tracker.Clear();
                        newFieldVal.Tracker = ((MB)mb).Tracker;
                    }
                }
                //mb.GetType().GetField("_status", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _status.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMB FillModelObj(IMB mb)
        {
            try
            {
                MBModel newFieldVal = null;
                if (!string.IsNullOrEmpty(mb.Model))
                {

                    IPart part = PtRepository.Find(mb.Model);

                    if (part != null)
                    {
                        string mbCode = string.Empty;
                        string mbType = TryToGetMBType(part, out mbCode);

                        newFieldVal = new MBModel(
                            part.PN,
                            mbCode,
                            part.GetAttribute("MDL"),
                            mbType,
                            part.Descr //2010-01-06  itc200008     Modify ITC-1103-0012
                            );
                        newFieldVal.Tracker.Clear();
                        newFieldVal.Tracker = ((MB)mb).Tracker;
                    }
                }
                //mb.GetType().GetField("_modelObj", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mb, newFieldVal);
                _modelObj.SetValue(mb, newFieldVal);
                return mb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// MB,VB,SB 都要试一遍,才能知道是什么Type
        /// </summary>
        /// <param name="part"></param>
        /// <param name="Mb_Code"></param>
        /// <returns></returns>
        public string TryToGetMBType(IPart part, out string Mb_Code)
        {
            Mb_Code = part.GetAttribute("MB");
            if (part.BOMNodeType.CompareTo("MB") == 0)
            {
                string vga = part.GetAttribute("VGA");
                if (!string.IsNullOrEmpty(vga) && vga.CompareTo("SV") == 0)
                {
                    return MB.MBType.VB;
                }
                string sb = part.GetAttribute("SB");
                if (!string.IsNullOrEmpty(sb) && sb.CompareTo("Y/N") == 0)
                {
                    return MB.MBType.SB;
                }
                return MB.MBType.MB;
            }
            throw new FisException("MDL001", new string[] { part.PN });//("Cannot figure out the type of the Board! [PCB Model: %1]"); //part.PN;

            //foreach (string typeName in MB.MBType.GetAllTypes())
            //{
            //    Mb_Code = part.GetAttribute(typeName);
            //    if (Mb_Code != null && Mb_Code.Trim() != string.Empty)
            //    {
            //        return typeName;
            //    }
            //}
            //throw new FisException("MDL001", new string[] { part.PN });//("Cannot figure out the type of the Board! [PCB Model: %1]"); //part.PN;
        }

        public Repair FillRepairDefectInfo(Repair rep)
        {
            try
            {
                IList<RepairDefect> newFieldVal = new List<RepairDefect>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBRepair_DefectInfo cond = new PCBRepair_DefectInfo();
                        cond.PCARepairID = rep.ID;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBRepair_DefectInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBRepair_DefectInfo.fn_PCARepairID].Value = rep.ID;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        RepairDefect repDfct = new RepairDefect(GetValue_Int32(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_ID]),
                                                                GetValue_Int32(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_PCARepairID]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Type]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_DefectCodeID]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Cause]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Obligation]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Component]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Site]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Location]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_MajorPart]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Remark]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_VendorCT]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_PartType]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_OldPart]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_OldPartSno]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_NewPart]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_NewPartSno]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_NewPartDateCode]),
                                                                Convert.ToBoolean(GetValue_Int32(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_IsManual])),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Manufacture]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_VersionA]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_VersionB]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_ReturnSign]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Side]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Mark]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_SubDefect]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_PIAStation]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Distribution]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn__4M_]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Responsibility]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Action]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Cover]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Uncover]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_TrackingStatus]),
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_MTAID]),
                                                                null,
                                                                GetValue_Str(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Editor]),
                                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Cdt]),
                                                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBRepair_DefectInfo.fn_Udt]));

                        repDfct.Tracker.Clear();
                        repDfct.Tracker = rep.Tracker;
                        newFieldVal.Add(repDfct);
                    }
                }

                //rep.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(rep, newFieldVal);
                _repairDefects.SetValue(rep, newFieldVal);
                return rep;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TestLog FillTestLogDefectInfo(TestLog testLog)
        {
            try
            {
                IList<TestLogDefect> newFieldVal = new List<TestLogDefect>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBTestLog_DefectInfo cond = new PCBTestLog_DefectInfo();
                        cond.PCBTestLogID = testLog.ID;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBTestLog_DefectInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_PCBTestLogID].Value = testLog.ID;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        TestLogDefect tld = new TestLogDefect(
                            GetValue_Int32(sqlR, sqlCtx.Indexes[PCBTestLog_DefectInfo.fn_ID]),
                            GetValue_Int32(sqlR, sqlCtx.Indexes[PCBTestLog_DefectInfo.fn_PCBTestLogID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog_DefectInfo.fn_DefectCodeID]),
                            GetValue_Str(sqlR, sqlCtx.Indexes[PCBTestLog_DefectInfo.fn_Editor]),
                            GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBTestLog_DefectInfo.fn_Cdt]));

                        tld.Tracker.Clear();
                        tld.Tracker = testLog.Tracker;
                        newFieldVal.Add(tld);
                    }
                }

               // testLog.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(testLog, newFieldVal);
                _testLogDefects.SetValue(testLog, newFieldVal);
                return testLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Lazy load of product attributes
        /// </summary>
        /// <param name="product"></param>
        public void FillPCBAttributes(IMB item)
        {
            try
            {
                IList<IMES.FisObject.FA.Product.ProductAttribute> newFieldVal = new List<IMES.FisObject.FA.Product.ProductAttribute>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBAttr cond = new _Schema.PCBAttr();
                        cond.PCBNo = item.Sn;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttr), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PCBAttr.fn_PCBNo].Value = item.Sn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.FA.Product.ProductAttribute pcba = new IMES.FisObject.FA.Product.ProductAttribute();

                        pcba.AttributeName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttr.fn_AttrName]);
                        pcba.AttributeValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttr.fn_AttrValue]);
                        pcba.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PCBAttr.fn_Cdt]);
                        pcba.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttr.fn_Editor]);
                        pcba.ProductId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttr.fn_PCBNo]);
                        pcba.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PCBAttr.fn_Udt]);

                        pcba.Tracker.Clear();
                        pcba.Tracker = ((MB)item).Tracker;
                        newFieldVal.Add(pcba);
                    }
                }

                //item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, newFieldVal);
                _attributes.SetValue(item, newFieldVal);
                //return product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Lazy load of product attribute logs
        /// </summary>
        /// <param name="product"></param>
        public void FillPCBAttributeLogs(IMB item)
        {
            try
            {
                IList<IMES.FisObject.FA.Product.ProductAttributeLog> newFieldVal = new List<IMES.FisObject.FA.Product.ProductAttributeLog>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBAttrLog cond = new _Schema.PCBAttrLog();
                        cond.PCBNo = item.Sn;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttrLog), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PCBAttrLog.fn_PCBNo].Value = item.Sn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.FA.Product.ProductAttributeLog pcbal = new IMES.FisObject.FA.Product.ProductAttributeLog();

                        pcbal.AttributeName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_AttrName]);
                        pcbal.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_Cdt]);
                        pcbal.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_AttrName]);
                        pcbal.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_ID]);
                        pcbal.NewValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_AttrNewValue]);
                        pcbal.OldValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_AttrOldValue]);
                        pcbal.ProductId = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_PCBNo]);
                        pcbal.Descr = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_Descr]);
                        pcbal.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_PCBModelID]);
                        pcbal.Station = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCBAttrLog.fn_Station]);

                        pcbal.Tracker.Clear();
                        pcbal.Tracker = ((MB)item).Tracker;
                        newFieldVal.Add(pcbal);
                    }
                }

                //item.GetType().GetField("_attributeLogs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, newFieldVal);
                _attributeLogs.SetValue(item, newFieldVal);
                //return product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// 根据前缀获取最大MBNO
        /// </summary>
        /// <param name="preStr">前缀</param>
        /// <returns>最大MBNO<</returns>
        public string GetMaxMBNO(string preStr)
        {
            try
            {
                string ret = string.Empty;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB likecond = new PCB();
                        likecond.PCBNo = preStr + "%";

                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), "MAX", new List<string>() { PCB.fn_PCBNo }, null, likecond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[PCB.fn_PCBNo].Value = preStr + "%";
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
        /// 更新MBNO最大号(NumControl)
        /// </summary>
        /// <param name="mbCode">MbCode</param>
        /// <param name="mbType">MbType</param>
        /// <param name="maxMO">MO最大号</param>
        public void SetMaxMBNO(string mbCode, string mbType, IMB maxMO)
        {
            try
            {
                PersistInsertMB(maxMO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除指定区间内的MB, MBStatus, 直接写db
        /// </summary>
        /// <param name="startSn">startSn</param>
        /// <param name="endSn">endSn</param>
        public void DeleteMBSection(string startSn, string endSn)
        {
            try
            {
                this.PersistDeleteMBStatus(startSn, endSn);
                this.PersistDeleteMB(startSn, endSn);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取指定区间中的MB列表
        /// </summary>
        /// <param name="startSn">startSn</param>
        /// <param name="endSn">endSn</param>
        /// <returns></returns>
        public IList<IMB> GetMBBySection(string startSn, string endSn)
        {
            try
            {
                IList<IMB> ret = new List<IMB>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB betweenCond = new PCB();
                        betweenCond.PCBNo = string.Format("{0} AND {1}", startSn, endSn);
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), null, betweenCond, null);
                    }
                }
                sqlCtx.Params[Func.DecBeg(PCB.fn_PCBNo)].Value = startSn;
                sqlCtx.Params[Func.DecEnd(PCB.fn_PCBNo)].Value = endSn;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMB item = new MB(
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));

                            //item.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                            //item.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                            //item.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                            //item.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                            //item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                            //item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                            //item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                            //item.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                            //item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                                addMBRemaingPropertyValue(item, sqlR, sqlCtx);

                            ((MB)item).Tracker.Clear();
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

        public IList<IMB> GetMBBySectionAndMO(string startSn, string endSn, string mo)
        {
            try
            {
                IList<IMB> ret = new List<IMB>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB eqCond = new PCB();
                        eqCond.SMTMOID = mo;

                        PCB betweenCond = new PCB();
                        betweenCond.PCBNo = string.Format("{0} AND {1}", startSn, endSn);
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), eqCond, betweenCond, null);
                    }
                }
                sqlCtx.Params[PCB.fn_SMTMOID].Value = mo;
                sqlCtx.Params[Func.DecBeg(PCB.fn_PCBNo)].Value = startSn;
                sqlCtx.Params[Func.DecEnd(PCB.fn_PCBNo)].Value = endSn;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMB item = new MB(
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                                GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                                GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));
                                //item.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                                //item.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                                //item.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                                //item.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                                //item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                                //item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                                //item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                                //item.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                                //item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                                addMBRemaingPropertyValue(item, sqlR, sqlCtx);
                            ((MB)item).Tracker.Clear();
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
        /// 获取指定mb的指定site,component的维修完成的RepairDefect列表
        /// select * from PCBRepair_DefectInfo inner join PCBRepair on PCBRepair_DefectInfo.PCARepairID = PCBRepair.ID where PCBNo = '' and Component = '' and Site = '' and Cause is not null 
        /// </summary>
        /// <param name="mbsn">mbsn</param>
        /// <param name="site">site</param>
        /// <param name="component">component</param>
        /// <returns>RepairDefect列表</returns>
        public IList<RepairDefect> GetRepairDefectBySiteComponent(string mbsn, string site, string component)
        {
            try
            {
                //select * from PCBRepair_DefectInfo inner join PCBRepair on PCBRepair_DefectInfo.PCARepairID = PCBRepair.ID 
                //where PCBNo = '' and Component = '' and Site = '' and Cause is not null 

                IList<RepairDefect> ret = new List<RepairDefect>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(PCBRepair_DefectInfo);
                        PCBRepair_DefectInfo equalCond1 = new PCBRepair_DefectInfo();
                        equalCond1.Component = component;
                        equalCond1.Site = site;
                        tf1.equalcond = equalCond1;
                        PCBRepair_DefectInfo notnullCond = new PCBRepair_DefectInfo();
                        notnullCond.Cause = "NOT NULL";
                        tf1.notNullcond = notnullCond;

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(PCBRepair);
                        PCBRepair equalCond2 = new PCBRepair();
                        equalCond2.PCBID = mbsn;
                        tf2.equalcond = equalCond2;
                        tf2.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, PCBRepair_DefectInfo.fn_PCARepairID, tf2, PCBRepair.fn_ID);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Component)].Value = equalCond1.Component;
                        sqlCtx.Params[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Site)].Value = equalCond1.Site;
                        sqlCtx.Params[Func.DecAlias(tf2.alias, PCBRepair.fn_PCBID)].Value = equalCond2.PCBID;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Component)].Value = component;
                sqlCtx.Params[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Site)].Value = site;
                sqlCtx.Params[Func.DecAlias(tf2.alias, PCBRepair.fn_PCBID)].Value = mbsn;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            RepairDefect rdfct = new RepairDefect(GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_ID)]),
                                                                    GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_PCARepairID)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Type)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_DefectCodeID)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Cause)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Obligation)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Component)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Site)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Location)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_MajorPart)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Remark)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_VendorCT)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_PartType)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_OldPart)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_OldPartSno)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_NewPart)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_NewPartSno)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_NewPartDateCode)]),
                                                                    Convert.ToBoolean(GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_IsManual)])),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Manufacture)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_VersionA)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_VersionB)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_ReturnSign)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Side)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Mark)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_SubDefect)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_PIAStation)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Distribution)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn__4M_)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Responsibility)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Action)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Cover)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Uncover)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_TrackingStatus)]),
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_MTAID)]),
                                                                    null,
                                                                    GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Editor)]),
                                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Cdt)]),
                                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair_DefectInfo.fn_Udt)]));

                            rdfct.Tracker.Clear();
                            ret.Add(rdfct);
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
        /// Replace Old MB Data with New MB Sno
        /// </summary>
        /// <param name="oldSn">oldSn</param>
        /// <param name="newSn">newSn</param>
        /// <remarks>
        /// 将下列各表中的Old MB Sno 对应记录的PCBNo 栏位Update 为New MB Sno
        /// IMES_PCA..MODismantleLog
        /// IMES_PCA..PCB
        /// IMES_PCA..PCBInfo
        /// IMES_PCA..PCBLog
        /// IMES_PCA..PCBRepair
        /// IMES_PCA..PCBStatus
        /// IMES_PCA..PCBTestLog
        /// IMES_PCA..PCB_Part
        /// IMES_PCA..SnoLog3D
        /// IMES_PCA..TransferToFISList
        /// </remarks>
        public void ReplaceMBSn(string oldSn, string newSn)
        {
            try
            {
                SqlTransactionManager.Begin();

                IMB board = this.Find_OnTrans(oldSn);
                if (board != null)
                {
                    board.SetSn(newSn);
                    this.PersistInsertMB_ForReplace(board);

                    ReplaceMBSn_MODismantleLog(oldSn, newSn);
                    //ReplaceMBSn_PCB(oldSn, newSn);
                    ReplaceMBSn_PCBInfo(oldSn, newSn);
                    ReplaceMBSn_PCBLog(oldSn, newSn);
                    ReplaceMBSn_PCBRepair(oldSn, newSn);
                    ReplaceMBSn_PCBStatus(oldSn, newSn);
                    ReplaceMBSn_PCBTestLog(oldSn, newSn);
                    ReplaceMBSn_PCB_Part(oldSn, newSn);
                    ReplaceMBSn_SnoLog3D(oldSn, newSn);
                    ReplaceMBSn_TestBoxDataLog(oldSn, newSn);
                    ReplaceMBSn_Attr(oldSn, newSn);
                    ReplaceMBSn_AttrLog(oldSn, newSn);
                    
                    board.SetSn(oldSn);
                    this.PersistDeleteMB(board);
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

        public IList<RepairInfo> GetMBRepairLogList(string MBId)
        {
            try
            {
                IList<RepairInfo> ret = new List<RepairInfo>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(PCBRepair);
                        PCBRepair equalCond = new PCBRepair();
                        equalCond.PCBID = MBId;
                        equalCond.Status = 0;
                        tf1.equalcond = equalCond;

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(PCBRepair_DefectInfo);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(new TableConnectionItem[] { new TableConnectionItem(tf1, PCBRepair.fn_ID, tf2, PCBRepair_DefectInfo.fn_PCARepairID) });

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Params[Func.DecAlias(tf1.alias, PCBRepair.fn_Status)].Value = equalCond.Status;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[Func.DecAlias(tf1.alias, PCBRepair.fn_PCBID)].Value = MBId;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        RepairInfo repi = new RepairInfo();
                        repi._4M = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn__4M_)]);
                        repi.action = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Action)]);
                        repi.cause = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Cause)]);
                        repi.cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Cdt)]);
                        repi.component = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Component)]);
                        repi.cover = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Cover)]);
                        repi.defectCodeID = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_DefectCodeID)]);
                        repi.distribution = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Distribution)]);
                        repi.editor = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Editor)]);
                        repi.id = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_ID)]).ToString();
                        repi.Identity = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_ID)]);
                        repi.isManual = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_IsManual)]).ToString();
                        repi.majorPart = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_MajorPart)]);
                        repi.manufacture = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Manufacture)]);
                        repi.side = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Side)]);
                        repi.mark = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Mark)]);
                        repi.newPart = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_NewPart)]);
                        repi.newPartDateCode = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_NewPartDateCode)]);
                        repi.newPartSno = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_NewPartSno)]);
                        repi.obligation = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Obligation)]);
                        repi.oldPart = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_OldPart)]);
                        repi.oldPartSno = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_OldPartSno)]);
                        repi.partType = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_PartType)]);
                        repi.pdLine = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair.fn_LineID)]);
                        repi.piaStation = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_PIAStation)]);
                        repi.remark = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Remark)]);
                        repi.repairID = GetValue_Int32(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_PCARepairID)]).ToString();
                        repi.responsibility = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Responsibility)]);
                        repi.returnSign = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_ReturnSign)]);
                        repi.site = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Site)]);
                        repi.subDefect = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_SubDefect)]);
                        repi.testStation = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCBRepair.fn_StationID)]);
                        repi.trackingStatus = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_TrackingStatus)]);
                        repi.type = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Type)]);
                        repi.udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Udt)]);
                        repi.uncover = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Uncover)]);
                        repi.vendorCT = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_VendorCT)]);
                        repi.versionA = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_VersionA)]);
                        repi.versionB = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_VersionB)]);
                        repi.mtaID = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_MTAID)]);
                        repi.location = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, PCBRepair_DefectInfo.fn_Location)]);
                        
                        repi.defectCodeDesc = DfctRepository.TransToDesc(repi.defectCodeID);
                        repi.causeDesc = DfciRepository.TransToDesc(IMES.FisObject.Common.Defect.DefectInfo.DefectInfoType.SaCause, repi.cause);
                        repi.testStationDesc = SttRepository.TransToDesc(repi.testStation);

                        ret.Add(repi);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMES.DataModel.MBInfo GetMBInfo(string MBId)
        {
            try
            {
                IMES.DataModel.MBInfo ret = new IMES.DataModel.MBInfo();
                IMB mb = this.Find(MBId);
                if (mb != null)
                {
                    if (mb.ModelObj != null)
                    {
                        ret._111LevelId = mb.ModelObj.Pn;
                        ret.MB_CODEId = mb.ModelObj.Mbcode;
                        ret.family = mb.ModelObj.Family;
                    }
                    ret.custVersion = mb.CUSTVER;
                    ret.dateCode = mb.DateCode;
                    ret.id = mb.Sn;
                    ret.iecVersion = mb.IECVER;
                    ret.SMTMOId = mb.SMTMO;
                    ret.ecr = mb.ECR;
                    if (mb.MBStatus != null)
                    {
                        ret.testStation = mb.MBStatus.Station;// SttRepository.TransToDesc(mb.MBStatus.Station);
                        ret.line = mb.MBStatus.Line;
                    }
                    return ret;
                }
                else
                    return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 检查输入的cpuVendorSn是否已经和MB绑定
        /// 用于CombineMB CPU之前检查CPU是否已与其他MB绑定;
        /// 检查PCB表的CVSN栏位如果等于输入的cpuVendorSn存在返回和它绑定的MBSNO
        /// 不存在返回""
        /// select top 1 PCBNo from PCB where CVSN=@CVSN
        /// </summary>
        /// <param name="cpuVendorSn"></param>
        /// <returns></returns>
        public string IsUsedCvsn(string cpuVendorSn) 
        {
            string result = "";
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB cond = new PCB();
                        cond.CVSN = cpuVendorSn;
                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), "TOP 1", new List<string>() { PCB.fn_PCBNo }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[PCB.fn_CVSN].Value = cpuVendorSn;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            result = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]);
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IMB GetMBByMAC(string mac)
        {
            SqlDataReader sqlR = null;
            try
            {
                SqlTransactionManager.Begin();

                IMB ret = null;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB cond = new PCB();
                        cond.MAC = mac;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[PCB.fn_MAC].Value = mac;
                //using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                try
                {
                    sqlR = SqlHelper.ExecuteReader_OnTrans(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MB(
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));
                        //ret.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                        //ret.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                        //ret.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                        //ret.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                        //ret.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                        //ret.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                        //ret.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                        //ret.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                        //ret.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                        addMBRemaingPropertyValue(ret, sqlR, sqlCtx);

                        ((MB)ret).Tracker.Clear();
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

        public IList<IMB> GetMBListByMAC(string mac)
        {
            try
            {
                IList<IMB> ret = new List<IMB>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB cond = new PCB();
                        cond.MAC = mac;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), cond, null, null);
                    }
                }
                sqlCtx.Params[PCB.fn_MAC].Value = mac;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MB item = new MB(
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                        GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                        GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));
                        //item.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                        //item.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                        //item.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                        //item.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                        //item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                        //item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                        //item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                        //item.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                        //item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                        addMBRemaingPropertyValue(item, sqlR, sqlCtx);
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

        public IList<IProductPart> GetPCBPartsByPartNoAndValue(string partNo, string val)
        {
            try
            {
                IList<IProductPart> ret = new List<IProductPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCB_Part cond = new _Schema.PCB_Part();
                        cond.partNo = partNo;
                        cond.partSn = val;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCB_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PCB_Part.fn_partNo].Value = partNo;
                sqlCtx.Params[_Schema.PCB_Part.fn_partSn].Value = val;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IProductPart item = new ProductPart(GetValue_Int32(sqlR, sqlCtx.Indexes[PCB_Part.fn_id]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partNo]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_pcbno]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_station]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partType]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_custmerPn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_iecpn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partSn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_editor]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB_Part.fn_cdt]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB_Part.fn_udt]));
                        ((ProductPart)item).BomNodeType = GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_bomNodeType]);
                        ((ProductPart)item).CheckItemType = GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_checkItemType]);
                        ((ProductPart)item).Tracker.Clear();
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

        public IList<IProductPart> GetPCBPartsByPartSn(string partSn)
        {
            try
            {
                IList<IProductPart> ret = new List<IProductPart>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCB_Part cond = new _Schema.PCB_Part();
                        cond.partSn = partSn;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCB_Part), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PCB_Part.fn_partSn].Value = partSn;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IProductPart item = new ProductPart(GetValue_Int32(sqlR, sqlCtx.Indexes[PCB_Part.fn_id]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partNo]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_pcbno]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_station]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partType]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_custmerPn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_iecpn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_partSn]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_editor]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB_Part.fn_cdt]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB_Part.fn_udt]));
                        ((ProductPart)item).BomNodeType = GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_bomNodeType]);
                        ((ProductPart)item).CheckItemType = GetValue_Str(sqlR, sqlCtx.Indexes[PCB_Part.fn_checkItemType]);
                        ((ProductPart)item).Tracker.Clear();
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

        public IList<IMES.FisObject.PCA.MB.MBInfo> GetPCBInfoByTypeValue(string infoType, string infoValue)
        {
            try
            {
                IList<IMES.FisObject.PCA.MB.MBInfo> ret = new List<IMES.FisObject.PCA.MB.MBInfo>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBInfo cond = new PCBInfo();
                        cond.InfoType = infoType;
                        cond.InfoValue = infoValue;
                        sqlCtx = Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBInfo), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBInfo.fn_InfoType].Value = infoType;
                sqlCtx.Params[PCBInfo.fn_InfoValue].Value = infoValue;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.PCA.MB.MBInfo mbinfo = new IMES.FisObject.PCA.MB.MBInfo(GetValue_Int32(sqlR, sqlCtx.Indexes[PCBInfo.fn_ID]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_PCBID]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_InfoType]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_InfoValue]),
                                                    GetValue_Str(sqlR, sqlCtx.Indexes[PCBInfo.fn_Editor]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBInfo.fn_Cdt]),
                                                    GetValue_DateTime(sqlR, sqlCtx.Indexes[PCBInfo.fn_Udt]));

                        mbinfo.Tracker.Clear();
                        ret.Add(mbinfo);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateUUID(string mac, DateTime currentTime, DateTime currentUTCTime)
        {
            try
            {
                SqlParameter[] paramsArray = new SqlParameter[3];

                paramsArray[0] = new SqlParameter("@MAC_Address", SqlDbType.VarChar);
                paramsArray[0].Value = mac;
                paramsArray[1] = new SqlParameter("@CurrentTime", SqlDbType.DateTime);
                paramsArray[1].Value = currentTime;
                paramsArray[2] = new SqlParameter("@CurrentUTCTime", SqlDbType.DateTime);
                paramsArray[2].Value = currentUTCTime;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, CommandType.StoredProcedure, "IMES_GetUUID", paramsArray))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        return GetValue_Str(sqlR, 0);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return "";
        }

        public IList<string> FilterMBNOList(IList<string> mbNOList, string stationId)
        {
            try
            {
                List<string> ret = new List<string>();

                if (mbNOList != null && mbNOList.Count > 0)
                {
                    IList<string> batch = new List<string>();
                    int i = 0;
                    foreach (string str in mbNOList)
                    {
                        batch.Add(str);
                        if ((i + 1) % batchSQLCnt == 0 || i == mbNOList.Count - 1)
                        {
                            ret.AddRange(FilterMBNOList_Inner(batch, stationId));
                            batch.Clear();
                        }
                        i++;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveBatch(IList<IMB> items)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    IList<string> batch = new List<string>();
                    int i = 0;
                    foreach (IMB entry in items)
                    {
                        batch.Add(entry.Sn);
                        if ((i + 1) % batchSQLCnt == 0 || i == items.Count - 1)
                        {
                            //PersistDeleteMBLogByPCBID_Batch(batch);
                            //PersistDeleteMBStatus_Batch(batch);
                            PersistDeleteMB_Batch(batch);
                            batch.Clear();
                        }
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMBStatusBatch(IList<MBStatus> mbstts)
        {
            try
            {
                if (mbstts != null && mbstts.Count > 0)
                {
                    IList<MBStatus> batch = new List<MBStatus>();
                    int i = 0;
                    foreach (MBStatus entry in mbstts)
                    {
                        batch.Add(entry);
                        if ((i + 1) % batchSQLCnt == 0 || i == mbstts.Count - 1)
                        {
                            UpdateMBStatusBatch_Inner(batch);
                            batch.Clear();
                        }
                        entry.Tracker.Clear();
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddMBLogBatch(IList<MBLog> mblogs)
        {
            try
            {
                if (mblogs != null && mblogs.Count > 0)
                {
                    IList<MBLog> batch = new List<MBLog>();
                    int i = 0;
                    foreach (MBLog entry in mblogs)
                    {
                        batch.Add(entry);
                        if ((i + 1) % batchSQLCnt == 0 || i == mblogs.Count - 1)
                        {
                            AddMBLogBatch_Inner(batch);
                            batch.Clear();
                        }
                        entry.Tracker.Clear();
                        i++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetTheNewestStationFromPCBLog(string pcbno)
        {
            //select Station from IMES_PCA..PCBLog where PCBNo='' (Cdt最大的)
            try
            {
                string ret = null;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBLog cond = new PCBLog();
                        cond.PCBID = pcbno;
                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBLog), null, new List<string>(){PCBLog.fn_StationID}, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderByDesc, _Schema.PCBLog.fn_Cdt);
                    }
                }
                sqlCtx.Params[PCBLog.fn_PCBID].Value = pcbno;
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Str(sqlR, sqlCtx.Indexes[PCBLog.fn_StationID]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<RptPcaRepInfo> GetRptPcaRepByTpAndSnoID(string tp, string snoId)
        {
            try
            {
                IList<RptPcaRepInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Rpt_PCARep cond = new mtns::Rpt_PCARep();
                        cond.tp = tp;
                        cond.snoId = snoId;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Rpt_PCARep>(tk, null, null, new ConditionCollection<mtns::Rpt_PCARep>(new EqualCondition<mtns::Rpt_PCARep>(cond)), mtns::Rpt_PCARep.fn_cdt);
                    }
                }
                sqlCtx.Param(mtns::Rpt_PCARep.fn_tp).Value = tp;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_snoId).Value = snoId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Rpt_PCARep, RptPcaRepInfo, RptPcaRepInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistRptPcaRepBySnoIdAndTpAndRemark(string snoId, string tp, string remark)
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
                        _Metas.Rpt_PCARep cond = new _Metas.Rpt_PCARep();
                        cond.snoId = snoId;
                        cond.tp = tp;
                        cond.remark = remark;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Rpt_PCARep>(tk, "COUNT", new string[] { _Metas.Rpt_PCARep.fn_id }, new ConditionCollection<_Metas.Rpt_PCARep>(new EqualCondition<_Metas.Rpt_PCARep>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Rpt_PCARep.fn_snoId).Value = snoId;
                sqlCtx.Param(_Metas.Rpt_PCARep.fn_tp).Value = tp;
                sqlCtx.Param(_Metas.Rpt_PCARep.fn_remark).Value = remark;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void InsertRptPcaRep(RptPcaRepInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Rpt_PCARep>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Rpt_PCARep, RptPcaRepInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Rpt_PCARep.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddMBTest(MBTestDef mbTest)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Mb_Test>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Mb_Test, MBTestDef>(sqlCtx, mbTest);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Mb_Test.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Mb_Test.fn_udt).Value = cmDt;

                mbTest.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteMBTest(string code, string family, bool type)
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
                        mtns::Mb_Test cond = new mtns::Mb_Test();
                        cond.code = code;
                        cond.family = family;
                        cond.type = true;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Mb_Test>(tk, new ConditionCollection<mtns::Mb_Test>(new EqualCondition<mtns::Mb_Test>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Mb_Test.fn_code).Value = code;
                sqlCtx.Param(mtns::Mb_Test.fn_family).Value = family;
                sqlCtx.Param(mtns::Mb_Test.fn_type).Value = type;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MBTestDef> GetMBTestbyFamily(string family)
        {
            try
            {
                IList<MBTestDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mb_Test cond = new mtns::Mb_Test();
                        cond.family = family;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mb_Test>(tk, null, null, new ConditionCollection<mtns::Mb_Test>(new EqualCondition<mtns::Mb_Test>(cond)), mtns::Mb_Test.fn_code);
                    }
                }
                sqlCtx.Param(mtns::Mb_Test.fn_family).Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mb_Test, MBTestDef, MBTestDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePCBStatus(PCBStatusInfo setValue, PCBStatusInfo condition)
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
                        Pcbstatus cond = FuncNew.SetColumnFromField<Pcbstatus, PCBStatusInfo>(condition);

                        Pcbstatus setv = FuncNew.SetColumnFromField<Pcbstatus, PCBStatusInfo>(setValue);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pcbstatus>(new SetValueCollection<Pcbstatus>(new CommonSetValue<Pcbstatus>(setv)), new ConditionCollection<Pcbstatus>(new EqualCondition<Pcbstatus>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcbstatus, PCBStatusInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pcbstatus, PCBStatusInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pcbstatus.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MBTestDef> GetMBTestByCodeFamilyAndType(string code, string family, bool type)
        {
            try
            {
                IList<MBTestDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mb_Test cond = new mtns::Mb_Test();
                        cond.code = code;
                        cond.family = family;
                        cond.type = true;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mb_Test>(tk, null, null, new ConditionCollection<mtns::Mb_Test>(new EqualCondition<mtns::Mb_Test>(cond)), mtns::Mb_Test.fn_code);
                    }
                }
                sqlCtx.Param(mtns::Mb_Test.fn_code).Value = code;
                sqlCtx.Param(mtns::Mb_Test.fn_family).Value = family;
                sqlCtx.Param(mtns::Mb_Test.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mb_Test, MBTestDef, MBTestDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MBCFGDef> GetAllMBCFGLst()
        {
            try
            {
                IList<MBCFGDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Mbcfg>(tk, Mbcfg.fn_mbcode);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Mbcfg, MBCFGDef, MBCFGDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddMBCFG(MBCFGDef mbcfgDef)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Mbcfg>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Mbcfg, MBCFGDef>(sqlCtx, mbcfgDef);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Mbcfg.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Mbcfg.fn_udt).Value = cmDt;

                mbcfgDef.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteMBCFG(int id)
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
                        mtns::Mbcfg cond = new mtns::Mbcfg();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Mbcfg>(tk, new ConditionCollection<mtns::Mbcfg>(new EqualCondition<mtns::Mbcfg>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Mbcfg.fn_id).Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMBCFG(MBCFGDef mbcfgDef, string mbCode, string series, string type)
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
                        Mbcfg cond = new Mbcfg();
                        cond.mbcode = mbCode;
                        cond.series = series;
                        cond.tp = type;
                        Mbcfg setv = FuncNew.SetColumnFromField<Mbcfg, MBCFGDef>(mbcfgDef, Mbcfg.fn_id, Mbcfg.fn_cdt);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<Mbcfg>(tk, new SetValueCollection<Mbcfg>(new CommonSetValue<Mbcfg>(setv)), new ConditionCollection<Mbcfg>(new EqualCondition<Mbcfg>(cond)));
                    }
                }
                sqlCtx.Param(Mbcfg.fn_mbcode).Value = mbCode;
                sqlCtx.Param(Mbcfg.fn_series).Value = series;
                sqlCtx.Param(Mbcfg.fn_tp).Value = type;

                sqlCtx = FuncNew.SetColumnFromField<Mbcfg, MBCFGDef>(sqlCtx, mbcfgDef, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Mbcfg.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MBCFGDef> GetMBCFGByCodeSeriesAndType(string mbCode, string series, string type)
        {
            try
            {
                IList<MBCFGDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mbcfg cond = new mtns::Mbcfg();
                        cond.mbcode = mbCode;
                        cond.series = series;
                        cond.tp = type;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mbcfg>(tk, null, null, new ConditionCollection<mtns::Mbcfg>(new EqualCondition<mtns::Mbcfg>(cond)), mtns::Mbcfg.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::Mbcfg.fn_mbcode).Value = mbCode;
                sqlCtx.Param(mtns::Mbcfg.fn_series).Value = series;
                sqlCtx.Param(mtns::Mbcfg.fn_tp).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mbcfg, MBCFGDef, MBCFGDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ITCNDDefectCheckDef> GetAllITCNDDefectChecks()
        {
            try
            {
                IList<ITCNDDefectCheckDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Maintain_ITCNDefect_Check>(tk, Maintain_ITCNDefect_Check.fn_family);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Maintain_ITCNDefect_Check, ITCNDDefectCheckDef, ITCNDDefectCheckDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddITCNDDefectCheck(ITCNDDefectCheckDef item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Maintain_ITCNDefect_Check>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Maintain_ITCNDefect_Check, ITCNDDefectCheckDef>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Maintain_ITCNDefect_Check.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveITCNDDefectCheck(string family)
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
                        mtns::Maintain_ITCNDefect_Check cond = new mtns::Maintain_ITCNDefect_Check();
                        cond.family = family;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Maintain_ITCNDefect_Check>(tk, new ConditionCollection<mtns::Maintain_ITCNDefect_Check>(new EqualCondition<mtns::Maintain_ITCNDefect_Check>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Maintain_ITCNDefect_Check.fn_family).Value = family;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveITCNDDefectCheckbyFamilyAndCode(string family, string code)
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
                        mtns::Maintain_ITCNDefect_Check cond = new mtns::Maintain_ITCNDefect_Check();
                        cond.family = family;
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Maintain_ITCNDefect_Check>(tk, new ConditionCollection<mtns::Maintain_ITCNDefect_Check>(new EqualCondition<mtns::Maintain_ITCNDefect_Check>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Maintain_ITCNDefect_Check.fn_family).Value = family;
                sqlCtx.Param(mtns::Maintain_ITCNDefect_Check.fn_code).Value = code;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPCB(string pcbNo)
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
                        Pcb cond = new Pcb();
                        cond.pcbno = pcbNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcb>(tk, "COUNT", new string[] { Pcb.fn_pcbno }, new ConditionCollection<Pcb>(new EqualCondition<Pcb>(cond)));
                    }
                }
                sqlCtx.Param(Pcb.fn_pcbno).Value = pcbNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                        ret = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Repair> GetPcbRepairList(string pcbNo, string station)
        {
            try
            {
                IList<Repair> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbrepair cond = new _Metas.Pcbrepair();
                        cond.pcbno = pcbNo;
                        cond.station = station;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbrepair>(tk, null, null, new ConditionCollection<_Metas.Pcbrepair>(new EqualCondition<_Metas.Pcbrepair>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcbrepair.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbrepair.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<Repair>();
                        while (sqlR.Read())
                        {
                            Repair item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbrepair, Repair>(item, sqlR, sqlCtx);
                            item.Status = (Repair.RepairStatus)Enum.Parse(typeof(Repair.RepairStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbrepair.fn_status)).ToString());
                            item.TestLogID = g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbrepair.fn_testLogID)).ToString();
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

        public DateTime GetMaxCdtFromPCBRepair(string pcbNo, string station)
        {
            try
            {
                DateTime ret = DateTime.MinValue;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbrepair cond = new _Metas.Pcbrepair();
                        cond.pcbno = pcbNo;
                        cond.station = station;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbrepair>(tk, "MAX", new string[] { _Metas.Pcbrepair.fn_cdt }, new ConditionCollection<_Metas.Pcbrepair>(new EqualCondition<_Metas.Pcbrepair>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcbrepair.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbrepair.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_DateTime(sqlR, sqlCtx.Indexes("MAX"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MBStatus> GetPcbStatusList(string pcbNo, string station)
        {
            try
            {
                IList<MBStatus> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbstatus cond = new _Metas.Pcbstatus();
                        cond.pcbno = pcbNo;
                        cond.station = station;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbstatus>(tk, null, null, new ConditionCollection<_Metas.Pcbstatus>(new EqualCondition<_Metas.Pcbstatus>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcbstatus.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbstatus.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcbstatus, MBStatus, MBStatus>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ITCNDDefectCheckDef> GetITCNDDefectChecks(string mbSno, string type)
        {
            try
            {
                IList<ITCNDDefectCheckDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Maintain_ITCNDefect_Check cond = new _Metas.Maintain_ITCNDefect_Check();
                        cond.code = mbSno;

                        _Metas.Maintain_ITCNDefect_Check cond2 = new _Metas.Maintain_ITCNDefect_Check();
                        cond2.type = type;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Maintain_ITCNDefect_Check>(tk, null, null, new ConditionCollection<_Metas.Maintain_ITCNDefect_Check>(
                            new EqualCondition<_Metas.Maintain_ITCNDefect_Check>(cond, null, "LEFT({0},2)"),
                            new EqualCondition<_Metas.Maintain_ITCNDefect_Check>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_code).Value = mbSno;
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Maintain_ITCNDefect_Check, ITCNDDefectCheckDef, ITCNDDefectCheckDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ITCNDDefectCheckDef> GetITCNDDefectChecks_NotCut(string mbSno, string type)
        {
            try
            {
                IList<ITCNDDefectCheckDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Maintain_ITCNDefect_Check cond = new _Metas.Maintain_ITCNDefect_Check();
                        cond.code = mbSno;

                        _Metas.Maintain_ITCNDefect_Check cond2 = new _Metas.Maintain_ITCNDefect_Check();
                        cond2.type = type;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Maintain_ITCNDefect_Check>(tk, null, null, new ConditionCollection<_Metas.Maintain_ITCNDefect_Check>(
                            new EqualCondition<_Metas.Maintain_ITCNDefect_Check>(cond),
                            new EqualCondition<_Metas.Maintain_ITCNDefect_Check>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_code).Value = mbSno;
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Maintain_ITCNDefect_Check, ITCNDDefectCheckDef, ITCNDDefectCheckDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, string[] types, int status, string station, DateTime beginCdt)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        cond.status = status;
                        cond.station = station;

                        _Metas.Pcbtestlog cond2 = new _Metas.Pcbtestlog();
                        cond2.type = "[INSET]";

                        _Metas.Pcbtestlog cond3 = new _Metas.Pcbtestlog();
                        cond3.cdt = beginCdt;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond),
                            new InSetCondition<_Metas.Pcbtestlog>(cond2),
                            new GreaterCondition<_Metas.Pcbtestlog>(cond3)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_status).Value = status;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_station).Value = station;
                sqlCtx.Param(g.DecG(_Metas.Pcbtestlog.fn_cdt)).Value = beginCdt;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pcbtestlog.fn_type), g.ConvertInSet(types));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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

        public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, string[] types, string station, DateTime beginCdt)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        cond.station = station;

                        _Metas.Pcbtestlog cond2 = new _Metas.Pcbtestlog();
                        cond2.type = "[INSET]";

                        _Metas.Pcbtestlog cond3 = new _Metas.Pcbtestlog();
                        cond3.cdt = beginCdt;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond),
                            new InSetCondition<_Metas.Pcbtestlog>(cond2),
                            new GreaterCondition<_Metas.Pcbtestlog>(cond3)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_station).Value = station;
                sqlCtx.Param(g.DecG(_Metas.Pcbtestlog.fn_cdt)).Value = beginCdt;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pcbtestlog.fn_type), g.ConvertInSet(types));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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
        public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, string station )
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        cond.station = station;

                        //_Metas.Pcbtestlog cond2 = new _Metas.Pcbtestlog();
                        //cond2.type = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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
        public IList<MBTestDef> GetMBTestList(string mbSno, bool type)
        {
            try
            {
                IList<MBTestDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Mb_Test cond = new _Metas.Mb_Test();
                        cond.code = mbSno;

                        _Metas.Mb_Test cond2 = new _Metas.Mb_Test();
                        cond2.type = true;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Mb_Test>(tk, null, null, new ConditionCollection<_Metas.Mb_Test>(
                            new EqualCondition<_Metas.Mb_Test>(cond, "RTRIM({0})", "LEFT({0},2)"),
                            new EqualCondition<_Metas.Mb_Test>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.Mb_Test.fn_code).Value = mbSno;
                sqlCtx.Param(_Metas.Mb_Test.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Mb_Test, MBTestDef, MBTestDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MBTestDef> GetMBTestList_NotCut(string mbSno, bool type)
        {
            try
            {
                IList<MBTestDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Mb_Test cond = new _Metas.Mb_Test();
                        cond.code = mbSno;

                        _Metas.Mb_Test cond2 = new _Metas.Mb_Test();
                        cond2.type = true;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Mb_Test>(tk, null, null, new ConditionCollection<_Metas.Mb_Test>(
                            new EqualCondition<_Metas.Mb_Test>(cond, "RTRIM({0})"),
                            new EqualCondition<_Metas.Mb_Test>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.Mb_Test.fn_code).Value = mbSno;
                sqlCtx.Param(_Metas.Mb_Test.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Mb_Test, MBTestDef, MBTestDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DateTime GetMaxCdtFromPCBLog(string pcbNo)
        {
            try
            {
                DateTime ret = DateTime.MinValue;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcblog cond = new _Metas.Pcblog();
                        cond.pcbno = pcbNo;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcblog>(tk, "MAX", new string[] { _Metas.Pcblog.fn_cdt }, new ConditionCollection<_Metas.Pcblog>(new EqualCondition<_Metas.Pcblog>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcblog.fn_pcbno).Value = pcbNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_DateTime(sqlR, sqlCtx.Indexes("MAX"));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ITCNDDefectCheckDef> GetITCNDDefectChecks(string mbSno)
        {
            try
            {
                IList<ITCNDDefectCheckDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Maintain_ITCNDefect_Check cond = new _Metas.Maintain_ITCNDefect_Check();
                        cond.code = mbSno;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Maintain_ITCNDefect_Check>(tk, null, null, new ConditionCollection<_Metas.Maintain_ITCNDefect_Check>(
                            new EqualCondition<_Metas.Maintain_ITCNDefect_Check>(cond, null, "LEFT({0},2)")));
                    }
                }
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_code).Value = mbSno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Maintain_ITCNDefect_Check, ITCNDDefectCheckDef, ITCNDDefectCheckDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ITCNDDefectCheckDef> GetITCNDDefectChecks_NotCut(string mbSno)
        {
            try
            {
                IList<ITCNDDefectCheckDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Maintain_ITCNDefect_Check cond = new _Metas.Maintain_ITCNDefect_Check();
                        cond.code = mbSno;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Maintain_ITCNDefect_Check>(tk, null, null, new ConditionCollection<_Metas.Maintain_ITCNDefect_Check>(
                            new EqualCondition<_Metas.Maintain_ITCNDefect_Check>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_code).Value = mbSno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Maintain_ITCNDefect_Check, ITCNDDefectCheckDef, ITCNDDefectCheckDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, int status, string station, DateTime beginCdt)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        cond.status = status;
                        cond.station = station;

                        _Metas.Pcbtestlog cond2 = new _Metas.Pcbtestlog();
                        cond2.cdt = beginCdt;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond),
                             new GreaterCondition<_Metas.Pcbtestlog>(cond2)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_status).Value = status;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_station).Value = station;
                sqlCtx.Param(g.DecG(_Metas.Pcbtestlog.fn_cdt)).Value = beginCdt;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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

        public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, string station, DateTime beginCdt)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        cond.station = station;

                        _Metas.Pcbtestlog cond2 = new _Metas.Pcbtestlog();
                        cond2.cdt = beginCdt;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond),
                             new GreaterCondition<_Metas.Pcbtestlog>(cond2)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_station).Value = station;
                sqlCtx.Param(g.DecG(_Metas.Pcbtestlog.fn_cdt)).Value = beginCdt;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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

        public IList<IMES.FisObject.PCA.MB.MBInfo> GetMbInfoListByInfoTypeAndPcbNoList(string infoType, string[] pcbNos)
        {
            try
            {
                IList<IMES.FisObject.PCA.MB.MBInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbinfo cond = new _Metas.Pcbinfo();
                        cond.infoType = infoType;

                        _Metas.Pcbinfo cond2 = new _Metas.Pcbinfo();
                        cond2.pcbno = "[INSET]";

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbinfo>(tk, null, null, new ConditionCollection<_Metas.Pcbinfo>(
                            new EqualCondition<_Metas.Pcbinfo>(cond),
                            new InSetCondition<_Metas.Pcbinfo>(cond2)));
                    }
                }
                sqlCtx.Param(_Metas.Pcbinfo.fn_infoType).Value = infoType;
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pcbinfo.fn_pcbno), g.ConvertInSet(pcbNos));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcbinfo, IMES.FisObject.PCA.MB.MBInfo, IMES.FisObject.PCA.MB.MBInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMtaMarkByRepairId(int repairId, string mark)
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
                        Mta_Mark cond = new Mta_Mark();
                        cond.rep_Id = repairId;
                        Mta_Mark setv = new Mta_Mark();
                        setv.mark = mark;
                        sqlCtx = FuncNew.GetConditionedUpdate<Mta_Mark>(tk, new SetValueCollection<Mta_Mark>(new CommonSetValue<Mta_Mark>(setv)), new ConditionCollection<Mta_Mark>(
                            new AnyCondition<Mta_Mark>(cond, string.Format("{0} IN (SELECT {1} FROM {2} WHERE {3}={4})", "{0}", mtns.Pcbrepair_Defectinfo.fn_id, ToolsNew.GetTableName(typeof(mtns.Pcbrepair_Defectinfo)), mtns.Pcbrepair_Defectinfo.fn_pcarepairid, "{1}"))));
                    }
                }
                sqlCtx.Param(g.DecAny(Mta_Mark.fn_rep_Id)).Value = repairId;
                sqlCtx.Param(g.DecSV(Mta_Mark.fn_mark)).Value = mark;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertMtaMarkInfo(MtaMarkInfo item)
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
                        sqlCtx = FuncNew.GetCommonInsert<Mta_Mark>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Mta_Mark, MtaMarkInfo>(sqlCtx, item);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MbCodeAndMdlInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix)
        {
            try
            {
                IList<MbCodeAndMdlInfo> ret = new List<MbCodeAndMdlInfo>();

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = mbType;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = mdlType;
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        _Metas.PartInfo cond4 = new _Metas.PartInfo();
                        cond4.infoValue = "%" + mdlPostfix;
                        tf3.Conditions.Add(new LikeCondition<_Metas.PartInfo>(cond4, "UPPER({0})"));
                        tf3.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, g.DecAliasInner("t2", _Metas.PartInfo.fn_infoValue));

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = mbType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = mdlType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)).Value = "%" + mdlPostfix+"%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MbCodeAndMdlInfo item = new MbCodeAndMdlInfo();
                            item.mbCode = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)));
                            item.mdl = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)));
                            item.displayName = item.mbCode + ' ' + item.mdl;
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

        public IList<string> GetPartNoListByInfo(string bomNodeType, string mbType, string mbCode)
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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = mbType;
                        cond2.infoValue = mbCode;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_partNo);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, g.DecAliasInner("t2", _Metas.PartInfo.fn_partNo));

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = mbType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)).Value = mbCode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_partNo)));
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

        public MbCodeAndMdlInfo GetMbCodeAndMdlInfoByPnoForMB(string pno)
        {
            try
            {
                MbCodeAndMdlInfo ret = null;

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
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = "MB";
                        cond.partNo = pno;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = "MB";
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = "MDL";
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        tf3.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "TOP 1", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_bomNodeType)).Value = cond.bomNodeType;
                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = cond2.infoType;
                        sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = cond3.infoType;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_partNo)).Value = pno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new MbCodeAndMdlInfo();
                        ret.mbCode = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)));
                        ret.mdl = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)));
                        ret.displayName = ret.mbCode + ' ' + ret.mdl;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ExistPCBRepair(string mbSno, int status)
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
                        _Metas.Pcbrepair cond = new _Metas.Pcbrepair();
                        cond.pcbno = mbSno;
                        cond.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbrepair>(tk, "COUNT", new string[] { _Metas.Pcbrepair.fn_id }, new ConditionCollection<_Metas.Pcbrepair>(new EqualCondition<_Metas.Pcbrepair>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcbrepair.fn_pcbno).Value = mbSno;
                sqlCtx.Param(_Metas.Pcbrepair.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<IMES.DataModel.BorrowLog> GetBorrowLogBySno(string sn, string status)
        {
            try
            {
                IList<IMES.DataModel.BorrowLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.BorrowLog cond = new _Metas.BorrowLog();
                        cond.sn = sn;
                        cond.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.BorrowLog>(tk, null, null, new ConditionCollection<_Metas.BorrowLog>(
                            new EqualCondition<_Metas.BorrowLog>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.BorrowLog.fn_sn).Value = sn;
                sqlCtx.Param(_Metas.BorrowLog.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.BorrowLog, IMES.DataModel.BorrowLog, IMES.DataModel.BorrowLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateBorrowLog(IMES.DataModel.BorrowLog item, string statusCondition)
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
                        _Metas.BorrowLog cond = new _Metas.BorrowLog();
                        cond.status = statusCondition;
                        cond.sn = item.Sn;

                        _Metas.BorrowLog setv = new _Metas.BorrowLog();
                        setv.returner = item.Returner;
                        setv.acceptor = item.Acceptor;
                        setv.status = item.Status;
                        setv.rdate = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.BorrowLog>(tk, new SetValueCollection<_Metas.BorrowLog>(new CommonSetValue<_Metas.BorrowLog>(setv)), new ConditionCollection<_Metas.BorrowLog>(new EqualCondition<_Metas.BorrowLog>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.BorrowLog.fn_status).Value = statusCondition;
                sqlCtx.Param(_Metas.BorrowLog.fn_sn).Value = item.Sn;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.BorrowLog.fn_rdate)).Value = cmDt;
                sqlCtx.Param(g.DecSV(_Metas.BorrowLog.fn_status)).Value = item.Status;
                sqlCtx.Param(g.DecSV(_Metas.BorrowLog.fn_acceptor)).Value = item.Acceptor;
                sqlCtx.Param(g.DecSV(_Metas.BorrowLog.fn_returner)).Value = item.Returner;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MbCodeAndMdlInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix, string vgaType, string vgaValue)
        {
            try
            {
                IList<MbCodeAndMdlInfo> ret = new List<MbCodeAndMdlInfo>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields tf3 = null;
                ITableAndFields tf4 = null;
                ITableAndFields tf5 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond2 = new _Metas.PartInfo();
                        cond2.infoType = mbType;
                        tf2.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = mdlType;
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        _Metas.PartInfo cond4 = new _Metas.PartInfo();
                        cond4.infoValue = "%" + mdlPostfix;
                        tf3.Conditions.Add(new LikeCondition<_Metas.PartInfo>(cond4, "UPPER({0})"));
                        tf3.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tf4 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond5 = new _Metas.PartInfo();
                        cond5.infoType = vgaType;
                        cond5.infoValue = vgaValue;
                        tf4.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond5));
                        tf4.ClearToGetFieldNames();

                        tf5 = new TableAndFields<_Metas.Smtmo>();
                        _Metas.Smtmo cond6 = new _Metas.Smtmo();
                        cond6.printQty = 0;
                        tf5.Conditions.Add(new AnyCondition<_Metas.Smtmo>(cond6, string.Format("{0}<t5.{1}", "{0}", _Metas.Smtmo.fn_qty)));
                        tf5.SubDBCalalog = _Schema.SqlHelper.DB_PCA;
                        tf5.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2, tf3, tf4, tf5 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf2, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf4, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.Smtmo>(tf1, Part_NEW.fn_partNo, tf5, _Metas.Smtmo.fn_iecpartno));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, g.DecAliasInner("t2", _Metas.PartInfo.fn_infoValue));

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];
                tf4 = tafa[3];
                tf5 = tafa[4];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoType)).Value = mbType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = mdlType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)).Value = "%" + mdlPostfix+"%";
                sqlCtx.Param(g.DecAlias(tf4.Alias, _Metas.PartInfo.fn_infoType)).Value = vgaType;
                sqlCtx.Param(g.DecAlias(tf4.Alias, _Metas.PartInfo.fn_infoValue)).Value = vgaValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MbCodeAndMdlInfo item = new MbCodeAndMdlInfo();
                            item.mbCode = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.PartInfo.fn_infoValue)));
                            item.mdl = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)));
                            item.displayName = item.mbCode + ' ' + item.mdl;
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

        public void UpdateRptPcaRepInfo(RptPcaRepInfo setValue, RptPcaRepInfo condition)
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
                Rpt_PCARep cond = FuncNew.SetColumnFromField<Rpt_PCARep, RptPcaRepInfo>(condition);
                Rpt_PCARep setv = FuncNew.SetColumnFromField<Rpt_PCARep, RptPcaRepInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Rpt_PCARep>(new SetValueCollection<Rpt_PCARep>(new CommonSetValue<Rpt_PCARep>(setv)), new ConditionCollection<Rpt_PCARep>(new EqualCondition<Rpt_PCARep>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Rpt_PCARep, RptPcaRepInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Rpt_PCARep, RptPcaRepInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Rpt_PCARep.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMtaMarkInfo(MtaMarkInfo setValue, MtaMarkInfo condition)
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
                Mta_Mark cond = FuncNew.SetColumnFromField<Mta_Mark, MtaMarkInfo>(condition);
                Mta_Mark setv = FuncNew.SetColumnFromField<Mta_Mark, MtaMarkInfo>(setValue);

                sqlCtx = FuncNew.GetConditionedUpdate<Mta_Mark>(new SetValueCollection<Mta_Mark>(new CommonSetValue<Mta_Mark>(setv)), new ConditionCollection<Mta_Mark>(new EqualCondition<Mta_Mark>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Mta_Mark, MtaMarkInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Mta_Mark, MtaMarkInfo>(sqlCtx, setValue, true);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistPcbRepairWithDefectOfCertainCausePrefix(string causePrefix, string mbSno)
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
                        tf1 = new TableAndFields<Pcbrepair>();
                        Pcbrepair cond = new Pcbrepair();
                        cond.pcbno = mbSno;
                        tf1.Conditions.Add(new EqualCondition<Pcbrepair>(cond));
                        tf1.AddRangeToGetFieldNames(Pcbrepair.fn_id);

                        tf2 = new TableAndFields<_Metas.Pcbrepair_Defectinfo>();
                        _Metas.Pcbrepair_Defectinfo cond2 = new _Metas.Pcbrepair_Defectinfo();
                        cond2.cause = causePrefix;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Pcbrepair_Defectinfo>(cond2, "LEFT({0},2)"));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Pcbrepair, Pcbrepair_Defectinfo>(tf1, Pcbrepair.fn_id, tf2, _Metas.Pcbrepair_Defectinfo.fn_pcarepairid));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Pcbrepair.fn_pcbno)).Value = mbSno;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Pcbrepair_Defectinfo.fn_cause)).Value = causePrefix;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<int> GetPcbRepairDefectInfoIdListWithDefect(string mbSno)
        {
            try
            {
                IList<int> ret = null;

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
                        tf1 = new TableAndFields<Pcbrepair>();
                        Pcbrepair cond = new Pcbrepair();
                        cond.pcbno = mbSno;
                        tf1.Conditions.Add(new EqualCondition<Pcbrepair>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.Pcbrepair_Defectinfo>();
                        tf2.AddRangeToGetFieldNames(Pcbrepair_Defectinfo.fn_id);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Pcbrepair, Pcbrepair_Defectinfo>(tf1, Pcbrepair.fn_id, tf2, _Metas.Pcbrepair_Defectinfo.fn_pcarepairid));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts, "t2." + _Metas.Pcbrepair_Defectinfo.fn_id);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Pcbrepair.fn_pcbno)).Value = mbSno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<int>();
                        while (sqlR.Read())
                        {
                            int item = g.GetValue_Int32(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.Pcbrepair_Defectinfo.fn_id)));
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

        public IList<MBLog> GetMBLog(string pcbno, string station, int status)
        {
            try
            {
                IList<MBLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcblog cond = new _Metas.Pcblog();
                        cond.pcbno = pcbno;
                        cond.station = station;
                        cond.status = status;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcblog>(tk, null, null, new ConditionCollection<Pcblog>(new EqualCondition<Pcblog>(cond)), Pcblog.fn_cdt);
                    }
                }
                sqlCtx.Param(_Metas.Pcblog.fn_pcbno).Value = pcbno;
                sqlCtx.Param(_Metas.Pcblog.fn_station).Value = station;
                sqlCtx.Param(_Metas.Pcblog.fn_status).Value = status;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcblog, MBLog, MBLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MBLog> GetMBLog(string pcbno, string station)
        {
            try
            {
                IList<MBLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcblog cond = new _Metas.Pcblog();
                        cond.pcbno = pcbno;
                        cond.station = station;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcblog>(tk, null, null, new ConditionCollection<Pcblog>(new EqualCondition<Pcblog>(cond)), Pcblog.fn_cdt);
                    }
                }
                sqlCtx.Param(_Metas.Pcblog.fn_pcbno).Value = pcbno;
                sqlCtx.Param(_Metas.Pcblog.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcblog, MBLog, MBLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MBCodeDef GetMBCode(string mbCode)
        {
            try
            {
                MBCodeDef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Mbcode cond = new Mbcode();
                        cond.mbcode = mbCode;
                        sqlCtx = FuncNew.GetConditionedSelect<Mbcode>(tk, "TOP 1", null, new ConditionCollection<Mbcode>(
                            new EqualCondition<Mbcode>(cond)));
                    }
                }
                sqlCtx.Param(Mbcode.fn_mbcode).Value = mbCode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Mbcode, MBCodeDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<int> GetQtyListFromPcaIctCountByCdtAndPdLine(DateTime cdt, string pdLine)
        {
            try
            {
                IList<int> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pcaictcount cond = new Pcaictcount();
                        cond.cdt = cdt;
                        cond.pdLine = pdLine;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcaictcount>(tk, null, new string[] { Pcaictcount.fn_qty }, new ConditionCollection<Pcaictcount>(new EqualCondition<Pcaictcount>(cond)), Pcaictcount.fn_qty);
                    }
                }
                sqlCtx.Param(Pcaictcount.fn_cdt).Value = new DateTime(cdt.Year, cdt.Month, cdt.Day, cdt.Hour, cdt.Minute, cdt.Second, cdt.Millisecond);
                sqlCtx.Param(Pcaictcount.fn_pdLine).Value = pdLine;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<int>();
                        while(sqlR.Read())
                        {
                            int item = g.GetValue_Int32(sqlR, sqlCtx.Indexes(Pcaictcount.fn_qty));
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

        public void UpdateQtyFromPcaIctCountByCdtAndPdLine(int qty, DateTime cdt, string pdLine)
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
                        Pcaictcount cond = new Pcaictcount();
                        cond.cdt = cdt;
                        cond.pdLine = pdLine;

                        Pcaictcount setv = new Pcaictcount();
                        setv.qty = qty;

                        sqlCtx = FuncNew.GetConditionedUpdate<Pcaictcount>(tk, new SetValueCollection<Pcaictcount>(new CommonSetValue<Pcaictcount>(setv)), new ConditionCollection<Pcaictcount>(new EqualCondition<Pcaictcount>(cond)));
                    }
                }
                sqlCtx.Param(Pcaictcount.fn_cdt).Value = new DateTime(cdt.Year, cdt.Month, cdt.Day, cdt.Hour, cdt.Minute, cdt.Second, cdt.Millisecond);
                sqlCtx.Param(Pcaictcount.fn_pdLine).Value = pdLine;
                sqlCtx.Param(g.DecSV(Pcaictcount.fn_qty)).Value = qty;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPcaIctCountInfo(PcaIctCountInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcaictcount>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pcaictcount, PcaIctCountInfo>(sqlCtx, item);

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MBCFGDef GetMBCFG(string mbCode, string type)
        {
            try
            {
                MBCFGDef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mbcfg cond = new mtns::Mbcfg();
                        cond.mbcode = mbCode;
                        cond.tp = type;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mbcfg>(tk, "TOP 1", null, new ConditionCollection<mtns::Mbcfg>(new EqualCondition<mtns::Mbcfg>(cond)), mtns::Mbcfg.fn_udt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(mtns::Mbcfg.fn_mbcode).Value = mbCode;
                sqlCtx.Param(mtns::Mbcfg.fn_tp).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mbcfg, MBCFGDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// PCBLog.ID 
        /// Condition：PCBNo=MBSno and Status=0 Order By Cdt Desc
        /// </summary>
        /// <param name="productId">pcbNo</param>
        /// <returns>MBLog</returns>
        public MBLog GetLatestFailLog(string pcbNo)
        {
            try
            {
                MBLog ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcblog cond = new _Metas.Pcblog();
                        cond.pcbno = pcbNo;
                        cond.status = 0;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcblog>(tk, "TOP 1", null, new ConditionCollection<Pcblog>(new EqualCondition<Pcblog>(cond)), Pcblog.fn_cdt + FuncNew.DescendOrder);
                        sqlCtx.Param(_Metas.Pcblog.fn_status).Value = cond.status; 
                    }
                }
                sqlCtx.Param(_Metas.Pcblog.fn_pcbno).Value = pcbNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcblog, MBLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        }

        public MBLog GetLatestFailLogRegardlessStatus(string pcbNo)
        {
            try
            {
                MBLog ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcblog cond = new _Metas.Pcblog();
                        cond.pcbno = pcbNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcblog>(tk, "TOP 1", null, new ConditionCollection<Pcblog>(new EqualCondition<Pcblog>(cond)), Pcblog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcblog.fn_pcbno).Value = pcbNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcblog, MBLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcbtestlog>(tk, null, null, new ConditionCollection<Pcbtestlog>(new EqualCondition<Pcbtestlog>(cond)), Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    //ret = FuncNew.SetFieldFromColumn<_Metas.Pcbtestlog, TestLog, TestLog>(ret, sqlR, sqlCtx);
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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

        public IList<PcaTestCheckInfo> GetPcaTestCheckInfoListByCode(string mbSno)
        {
            try
            {
                IList<PcaTestCheckInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcatest_Check cond = new _Metas.Pcatest_Check();
                        cond.code = mbSno;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcatest_Check>(tk, null, null, new ConditionCollection<Pcatest_Check>(new EqualCondition<Pcatest_Check>(cond, null, "LEFT({0},2)")));
                    }
                }
                sqlCtx.Param(_Metas.Pcatest_Check.fn_code).Value = mbSno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcatest_Check, PcaTestCheckInfo, PcaTestCheckInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcaTestCheckInfo> GetPcaTestCheckInfoListByCode_NotCut(string mbSno)
        {
            try
            {
                IList<PcaTestCheckInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcatest_Check cond = new _Metas.Pcatest_Check();
                        cond.code = mbSno;
                        sqlCtx = FuncNew.GetConditionedSelect<Pcatest_Check>(tk, null, null, new ConditionCollection<Pcatest_Check>(new EqualCondition<Pcatest_Check>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcatest_Check.fn_code).Value = mbSno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcatest_Check, PcaTestCheckInfo, PcaTestCheckInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcaTestCheckInfo> GetPcaTestCheckInfoListByCode(PcaTestCheckInfo condition)
        {
            try
            {
                IList<PcaTestCheckInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pcatest_Check cond = mtns::FuncNew.SetColumnFromField<mtns::Pcatest_Check, PcaTestCheckInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pcatest_Check>(null, null, new mtns::ConditionCollection<mtns::Pcatest_Check>(new mtns::EqualCondition<mtns::Pcatest_Check>(cond)), mtns::Pcatest_Check.fn_id);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcatest_Check, PcaTestCheckInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pcatest_Check, PcaTestCheckInfo, PcaTestCheckInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertFruDetInfo(FruDetInfo newFruDet)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<FruDet>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<FruDet, FruDetInfo>(sqlCtx, newFruDet);

                newFruDet.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePcbStatuses(MBStatus setValue, string[] pcbIds)
        {
            try
            {
                UpdatePcbStatuses(setValue, pcbIds, MBStatusEnum.NULL);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void UpdatePcbStatuses(MBStatus setValue, string[] pcbIds, MBStatusEnum status)
        {
            try
            {
                if (pcbIds != null)
                {
                    foreach (string pcbId in pcbIds)
                    {
                        UpdatePcbStatuses_Inner(setValue, status, pcbId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdatePcbStatuses_Inner(MBStatus setValue, MBStatusEnum status, string pcbId)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Pcbstatus cond = new Pcbstatus();
                        cond.pcbno = pcbId;
                        Pcbstatus setv = mtns::FuncNew.SetColumnFromField<Pcbstatus, MBStatus>(setValue);
                        setv.udt = DateTime.Now;
                        if (status == MBStatusEnum.NULL)
                            setv.status = int.MinValue;
                        else
                            setv.status = Convert.ToInt32(status);
                        sqlCtx = mtns::FuncNew.GetConditionedUpdate<Pcbstatus>(tk,new mtns::SetValueCollection<Pcbstatus>(new mtns::CommonSetValue<Pcbstatus>(setv)), new mtns::ConditionCollection<Pcbstatus>(new mtns::EqualCondition<Pcbstatus>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pcbstatus.fn_pcbno).Value = pcbId;
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcbstatus, MBStatus>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Pcbstatus.fn_udt)).Value = cmDt;
                if (status != MBStatusEnum.NULL)
                    sqlCtx.Param(g.DecSV(mtns::Pcbstatus.fn_status)).Value = Convert.ToInt32(status);
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddMBLogs(MBLog[] mbLogs)
        {
            try
            {
                if (mbLogs != null)
                {
                    foreach (MBLog mbLog in mbLogs)
                    {
                        this.PersistInsertMBLog(mbLog);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddMBInfoes(IList<IMES.FisObject.PCA.MB.MBInfo> mbInfoes)
        {
            try
            {
                if (mbInfoes != null)
                {
                    foreach (IMES.FisObject.PCA.MB.MBInfo mbInfo in mbInfoes)
                    {
                        this.PersistInsertMBInfo(mbInfo);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ITCNDDefectCheckDef CheckExistByFamilyAndCode(string family, string code)
        {
            try
            {
                ITCNDDefectCheckDef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Maintain_ITCNDefect_Check cond = new _Metas.Maintain_ITCNDefect_Check();
                        cond.family = family;
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Maintain_ITCNDefect_Check>(tk, "TOP 1", null, new ConditionCollection<_Metas.Maintain_ITCNDefect_Check>(
                            new EqualCondition<_Metas.Maintain_ITCNDefect_Check>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_family).Value = family;
                sqlCtx.Param(_Metas.Maintain_ITCNDefect_Check.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Maintain_ITCNDefect_Check, ITCNDDefectCheckDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcaTestCheckInfo> CheckSATestCheckRuleExist(string code)
        {
            try
            {
                IList<PcaTestCheckInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcatest_Check cond = new _Metas.Pcatest_Check();
                        cond.code = code;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcatest_Check>(tk, null, null, new ConditionCollection<_Metas.Pcatest_Check>(
                            new EqualCondition<_Metas.Pcatest_Check>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcatest_Check.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<_Metas.Pcatest_Check, PcaTestCheckInfo, PcaTestCheckInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveSATestCheckRuleItem(int id)
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
                        mtns::Pcatest_Check cond = new mtns::Pcatest_Check();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Pcatest_Check>(tk, new ConditionCollection<mtns::Pcatest_Check>(new EqualCondition<mtns::Pcatest_Check>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Pcatest_Check.fn_id).Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddSATestCheckRuleItem(PcaTestCheckInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcatest_Check>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pcatest_Check, PcaTestCheckInfo>(sqlCtx, item);

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTestCheckRuleItem(PcaTestCheckInfo item, int id)
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
                Pcatest_Check cond = new Pcatest_Check();
                cond.id = id;
                Pcatest_Check setv = mtns::FuncNew.SetColumnFromField<Pcatest_Check, PcaTestCheckInfo>(item, Pcatest_Check.fn_id);
                setv.udt = DateTime.Now;
                sqlCtx = mtns::FuncNew.GetConditionedUpdate<Pcatest_Check>(new mtns::SetValueCollection<Pcatest_Check>(new mtns::CommonSetValue<Pcatest_Check>(setv)), new mtns::ConditionCollection<Pcatest_Check>(new mtns::EqualCondition<Pcatest_Check>(cond)));
                //    }
                //}
                sqlCtx.Param(mtns::Pcatest_Check.fn_id).Value = id;
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcatest_Check, PcaTestCheckInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Pcatest_Check.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcaTestCheckInfo> GetAllSATestCheckRuleItems()
        {
            try
            {
                IList<PcaTestCheckInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Pcatest_Check>(tk, Pcatest_Check.fn_id);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Pcatest_Check, PcaTestCheckInfo, PcaTestCheckInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePCBLotInfo(PcblotInfo setValue, PcblotInfo condition)
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
                mtns::Pcblot cond = mtns::FuncNew.SetColumnFromField<mtns::Pcblot, PcblotInfo>(condition);
                mtns::Pcblot setv = mtns::FuncNew.SetColumnFromField<mtns::Pcblot, PcblotInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Pcblot>(new mtns::SetValueCollection<mtns::Pcblot>(new mtns::CommonSetValue<mtns::Pcblot>(setv)), new mtns::ConditionCollection<mtns::Pcblot>(new mtns::EqualCondition<mtns::Pcblot>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcblot, PcblotInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcblot, PcblotInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Pcblot.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateLotInfo(LotInfo setValue, LotInfo condition)
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
                mtns::Lot cond = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(condition);
                mtns::Lot setv = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Lot>(new mtns::SetValueCollection<mtns::Lot>(new mtns::CommonSetValue<mtns::Lot>(setv)), new mtns::ConditionCollection<mtns::Lot>(new mtns::EqualCondition<mtns::Lot>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Lot.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateLotInfoForDecQty(LotInfo setValue, LotInfo condition)
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
                mtns::Lot cond = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(condition);
                mtns::Lot setv = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(setValue, Lot.fn_qty);
                setv.udt = DateTime.Now;
                mtns::Lot setv2 = new Lot();
                setv2.qty = setValue.qty;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Lot>(new mtns::SetValueCollection<mtns::Lot>(new mtns::CommonSetValue<mtns::Lot>(setv), new mtns.ForDecSetValue<mtns::Lot>(setv2)), new mtns::ConditionCollection<mtns::Lot>(new mtns::EqualCondition<mtns::Lot>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Lot.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecDec(mtns::Lot.fn_qty)).Value = setValue.qty;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcblotInfo> GetPcblotInfoList(PcblotInfo condition)
        {
            try
            {
                IList<PcblotInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pcblot cond = mtns::FuncNew.SetColumnFromField<mtns::Pcblot, PcblotInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pcblot>(null, null, new mtns::ConditionCollection<mtns::Pcblot>(new mtns::EqualCondition<mtns::Pcblot>(cond)), mtns::Pcblot.fn_cdt + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcblot, PcblotInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pcblot, PcblotInfo, PcblotInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<LotInfo> GetlotInfoList(LotInfo condition)
        {
            try
            {
                IList<LotInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Lot cond = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Lot>(null, null, new mtns::ConditionCollection<mtns::Lot>(new mtns::EqualCondition<mtns::Lot>(cond)), mtns::Lot.fn_lotNo);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Lot, LotInfo, LotInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertLotInfo(LotInfo item)
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
                        sqlCtx = FuncNew.GetCommonInsert<Lot>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Lot, LotInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Lot.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Lot.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateLotInfoForIncQty(LotInfo setValue, LotInfo condition)
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
                mtns::Lot cond = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(condition);
                mtns::Lot setv = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(setValue, Lot.fn_qty);
                setv.udt = DateTime.Now;
                mtns::Lot setv2 = new Lot();
                setv2.qty = setValue.qty;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Lot>(new mtns::SetValueCollection<mtns::Lot>(new mtns::CommonSetValue<mtns::Lot>(setv), new mtns.ForIncSetValue<mtns::Lot>(setv2)), new mtns::ConditionCollection<mtns::Lot>(new mtns::EqualCondition<mtns::Lot>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Lot, LotInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Lot.fn_udt)).Value = cmDt;
                sqlCtx.Param(g.DecInc(mtns::Lot.fn_qty)).Value = setValue.qty;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPCBLotInfo(PcblotInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcblot>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pcblot, PcblotInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Pcblot.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Pcblot.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<LotSettingInfo> GetLotSettingInfoList(LotSettingInfo condition)
        {
            try
            {
                IList<LotSettingInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::LotSetting cond = mtns::FuncNew.SetColumnFromField<mtns::LotSetting, LotSettingInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::LotSetting>(null, null, new mtns::ConditionCollection<mtns::LotSetting>(new mtns::EqualCondition<mtns::LotSetting>(cond)), mtns::LotSetting.fn_line);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::LotSetting, LotSettingInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::LotSetting, LotSettingInfo, LotSettingInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertLotSettingInfo(LotSettingInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<LotSetting>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<LotSetting, LotSettingInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::LotSetting.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::LotSetting.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLotSettingInfo(LotSettingInfo condition)
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
                LotSetting cond = FuncNew.SetColumnFromField<LotSetting, LotSettingInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<LotSetting>(new ConditionCollection<LotSetting>(new EqualCondition<LotSetting>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<LotSetting, LotSettingInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, int status, string station)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        cond.status = status;
                        cond.station = station;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_status).Value = status;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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

        public IList<TestLog> GetPCBTestLogListFromPCBTestLogByType(string pcbNo, int status, string type)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        cond.status = status;
                        cond.type = type;
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_status).Value = status;
                sqlCtx.Param(_Metas.Pcbtestlog.fn_type).Value = type;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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

        public void UpdateLotSettingInfo(LotSettingInfo setValue, LotSettingInfo condition)
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
                mtns::LotSetting cond = mtns::FuncNew.SetColumnFromField<mtns::LotSetting, LotSettingInfo>(condition);
                mtns::LotSetting setv = mtns::FuncNew.SetColumnFromField<mtns::LotSetting, LotSettingInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::LotSetting>(new mtns::SetValueCollection<mtns::LotSetting>(new mtns::CommonSetValue<mtns::LotSetting>(setv)), new mtns::ConditionCollection<mtns::LotSetting>(new mtns::EqualCondition<mtns::LotSetting>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::LotSetting, LotSettingInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::LotSetting, LotSettingInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::LotSetting.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPcblotcheckInfo(PcblotcheckInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcblotcheck>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pcblotcheck, PcblotcheckInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Pcblotcheck.fn_cdt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPcbNoAndCheckStatusList(string lotNo, string status)
        {
            try
            {
                DataTable ret = null;

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
                        tf1 = new TableAndFields<Pcblot>();
                        Pcblot cond = new Pcblot();
                        cond.lotNo = lotNo;
                        cond.status = status;
                        tf1.Conditions.Add(new EqualCondition<Pcblot>(cond));
                        tf1.AddRangeToGetFieldNames(Pcblot.fn_pcbno);

                        tf2 = new TableAndFields<Pcblotcheck>();
                        tf2.AddRangeToGetFuncedFieldNames(new string[][] { new string[] { Pcblotcheck.fn_status, string.Format("ISNULL(t2.{0},'0') AS Checked", Pcblotcheck.fn_status) } });

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new mtns.TableConnectionItem<Pcblot, Pcblotcheck>(tf1, Pcblot.fn_lotNo, tf2, _Metas.Pcblotcheck.fn_lotNo),
                            new mtns.TableConnectionItem<Pcblot, Pcblotcheck>(tf1, Pcblot.fn_pcbno, tf2, _Metas.Pcblotcheck.fn_pcbno)
                            );

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, null, tafa, tblCnnts, "t1." + _Metas.Pcblot.fn_pcbno);

                        var arr = sqlCtx.Sentence.Split(new string[]{"AND","WHERE","ON",","}, StringSplitOptions.None);
                        sqlCtx.Sentence = string.Format("{0},{1},{2} LEFT JOIN {3} ON {4} AND {5} WHERE {6} AND {7}", arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7]);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Pcblot.fn_lotNo)).Value = lotNo;
                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Pcblotcheck.fn_status)).Value = status;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPCBLotCheckFromPCBLot(string pcbNo, string editor, PcblotInfo condition)
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
                Pcblot cond = FuncNew.SetColumnFromField<Pcblot, PcblotInfo>(condition);

                sqlCtx = FuncNew.GetConditionedForBackupInsert<Pcblot, Pcblotcheck>(
                    new string[][]{
                                new string[]{Pcblot.fn_lotNo,Pcblotcheck.fn_lotNo},
                                new string[]{string.Format("@{0}$2",Pcblotcheck.fn_pcbno),Pcblotcheck.fn_pcbno},
                                new string[]{"'0'",Pcblotcheck.fn_status},
                                new string[]{string.Format("@{0}",Pcblotcheck.fn_editor),Pcblotcheck.fn_editor},
                                new string[]{"GETDATE()",_Metas.Pcblotcheck.fn_cdt}
                            },
                    new ConditionCollection<Pcblot>(
                        new EqualCondition<Pcblot>(cond)));
                sqlCtx.AddParam(Pcblotcheck.fn_pcbno + "$2", new SqlParameter(string.Format("@{0}$2", Pcblotcheck.fn_pcbno), ToolsNew.GetDBFieldType<Pcblotcheck>(Pcblotcheck.fn_pcbno)));
                sqlCtx.AddParam(Pcblotcheck.fn_editor, new SqlParameter(string.Format("@{0}", Pcblotcheck.fn_editor), ToolsNew.GetDBFieldType<Pcblotcheck>(Pcblotcheck.fn_editor)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcblot, PcblotInfo>(sqlCtx, condition);

                sqlCtx.Param(Pcblotcheck.fn_pcbno + "$2").Value = pcbNo;
                sqlCtx.Param(Pcblotcheck.fn_editor).Value = editor;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetCountOfPcblotCheck(PcblotcheckInfo condition)
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
                mtns::Pcblotcheck cond = mtns::FuncNew.SetColumnFromField<mtns::Pcblotcheck, PcblotcheckInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pcblotcheck>("COUNT", new string[] { mtns::Pcblotcheck.fn_id }, new mtns::ConditionCollection<mtns::Pcblotcheck>(new mtns::EqualCondition<mtns::Pcblotcheck>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcblotcheck, PcblotcheckInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public IList<string> GetStationListFromPcbStatus(string pcbNo)
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
                        tf1 = new TableAndFields<Pcbstatus>();
                        Pcbstatus cond = new Pcbstatus();
                        cond.pcbno = pcbNo;
                        tf1.Conditions.Add(new EqualCondition<Pcbstatus>(cond));
                        tf1.AddRangeToGetFieldNames(Pcbstatus.fn_station);

                        tf2 = new TableAndFields<_Metas.Station>();
                        tf2.SubDBCalalog = SqlHelper.DB_GetData;
                        tf2.AddRangeToGetFieldNames(_Metas.Station.fn_descr);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Pcbstatus, _Metas.Station>(tf1, Pcbstatus.fn_station, tf2, _Metas.Station.fn_station));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, Pcbstatus.fn_station);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Pcbstatus.fn_pcbno)).Value = pcbNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = string.Format("{0}  {1}",
                                g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Pcbstatus.fn_station))),
                                g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.Station.fn_descr))));
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

        public IList<string> GetLineListFromPcbStatus(string pcbNo)
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
                        tf1 = new TableAndFields<Pcbstatus>();
                        Pcbstatus cond = new Pcbstatus();
                        cond.pcbno = pcbNo;
                        tf1.Conditions.Add(new EqualCondition<Pcbstatus>(cond));
                        tf1.AddRangeToGetFieldNames(Pcbstatus.fn_line);

                        tf2 = new TableAndFields<_Metas.Line>();
                        tf2.SubDBCalalog = SqlHelper.DB_GetData;
                        tf2.AddRangeToGetFieldNames(_Metas.Line.fn_descr);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new TableConnectionItem<Pcbstatus, _Metas.Line>(tf1, Pcbstatus.fn_line, tf2, _Metas.Line.fn_line));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, Pcbstatus.fn_line);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Pcbstatus.fn_pcbno)).Value = pcbNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = string.Format("{0}  {1}",
                                g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, _Metas.Pcbstatus.fn_line))),
                                g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.Line.fn_descr))));
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

        public IList<LotInfo> GetLotList(string[] statuses, int oqcTimeSpanDays, string pdLine, string mbCode, int lotQty)
        {
            try
            {
                IList<LotInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Lot cond = new Lot();
                        cond.status = "[INSET]";

                        Lot cond2 = new Lot();
                        cond2.line = pdLine;
                        cond2.mbcode = mbCode;

                        Lot cond3 = new Lot();
                        cond3.qty = lotQty;

                        Lot cond4 = new Lot();
                        cond4.cdt = DateTime.Now;

                        sqlCtx = mtns::FuncNew.GetConditionedSelect<Lot>(null, null, new mtns::ConditionCollection<Lot>(
                            new mtns::InSetCondition<Lot>(cond),
                            new mtns::EqualCondition<Lot>(cond2),
                            new mtns::SmallerCondition<Lot>(cond3),
                            new mtns::AnyCondition<Lot>(cond4, string.Format("DATEDIFF(DAY,{0},GETDATE())<=@{1}", "{0}", Lot.fn_cdt))
                            ), Lot.fn_cdt);

                        sqlCtx.AddParam(Lot.fn_cdt, new SqlParameter(string.Format("@{0}", Lot.fn_cdt), SqlDbType.Int));
                    }
                }
                sqlCtx.Param(Lot.fn_cdt).Value = oqcTimeSpanDays;
                sqlCtx.Param(Lot.fn_line).Value = pdLine;
                sqlCtx.Param(Lot.fn_mbcode).Value = mbCode;
                sqlCtx.Param(g.DecS(Lot.fn_qty)).Value = lotQty;

                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(Lot.fn_status), g.ConvertInSet(statuses));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Lot, LotInfo, LotInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcboqcrepairInfo> GetPcboqcrepairInfoList(PcboqcrepairInfo condition)
        {
            try
            {
                IList<PcboqcrepairInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pcboqcrepair cond = mtns::FuncNew.SetColumnFromField<mtns::Pcboqcrepair, PcboqcrepairInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pcboqcrepair>(null, null, new mtns::ConditionCollection<mtns::Pcboqcrepair>(new mtns::EqualCondition<mtns::Pcboqcrepair>(cond)), mtns::Pcboqcrepair.fn_cdt);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcboqcrepair, PcboqcrepairInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pcboqcrepair, PcboqcrepairInfo, PcboqcrepairInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcboqcrepairInfo> GetPcboqcrepairInfoListOrderByCdtDesc(PcboqcrepairInfo condition)
        {
            try
            {
                IList<PcboqcrepairInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pcboqcrepair cond = mtns::FuncNew.SetColumnFromField<mtns::Pcboqcrepair, PcboqcrepairInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pcboqcrepair>(null, null, new mtns::ConditionCollection<mtns::Pcboqcrepair>(new mtns::EqualCondition<mtns::Pcboqcrepair>(cond)), mtns::Pcboqcrepair.fn_cdt + FuncNew.DescendOrder);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcboqcrepair, PcboqcrepairInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pcboqcrepair, PcboqcrepairInfo, PcboqcrepairInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<Pcboqcrepair_DefectinfoInfo> GetPcboqcrepairDefectinfoInfoList(Pcboqcrepair_DefectinfoInfo condition)
        {
            try
            {
                IList<Pcboqcrepair_DefectinfoInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pcboqcrepair_Defectinfo cond = mtns::FuncNew.SetColumnFromField<mtns::Pcboqcrepair_Defectinfo, Pcboqcrepair_DefectinfoInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pcboqcrepair_Defectinfo>(null, null, new mtns::ConditionCollection<mtns::Pcboqcrepair_Defectinfo>(new mtns::EqualCondition<mtns::Pcboqcrepair_Defectinfo>(cond)), mtns::Pcboqcrepair_Defectinfo.fn_cdt);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcboqcrepair_Defectinfo, Pcboqcrepair_DefectinfoInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pcboqcrepair_Defectinfo, Pcboqcrepair_DefectinfoInfo, Pcboqcrepair_DefectinfoInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePcboqcrepairInfo(PcboqcrepairInfo setValue, PcboqcrepairInfo condition)
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
                Pcboqcrepair cond = FuncNew.SetColumnFromField<Pcboqcrepair, PcboqcrepairInfo>(condition);
                Pcboqcrepair setv = FuncNew.SetColumnFromField<Pcboqcrepair, PcboqcrepairInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Pcboqcrepair>(new SetValueCollection<Pcboqcrepair>(new CommonSetValue<Pcboqcrepair>(setv)), new ConditionCollection<Pcboqcrepair>(new EqualCondition<Pcboqcrepair>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcboqcrepair, PcboqcrepairInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pcboqcrepair, PcboqcrepairInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Pcboqcrepair.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePcboqcrepairInfo(PcboqcrepairInfo condition)
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
                Pcboqcrepair cond = FuncNew.SetColumnFromField<Pcboqcrepair, PcboqcrepairInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Pcboqcrepair>(new ConditionCollection<Pcboqcrepair>(new EqualCondition<Pcboqcrepair>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcboqcrepair, PcboqcrepairInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPcboqcrepairInfo(PcboqcrepairInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcboqcrepair>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pcboqcrepair, PcboqcrepairInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Pcboqcrepair.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Pcboqcrepair.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertPcboqcrepairDefectinfo(Pcboqcrepair_DefectinfoInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcboqcrepair_Defectinfo>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pcboqcrepair_Defectinfo, Pcboqcrepair_DefectinfoInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Pcboqcrepair_Defectinfo.fn_cdt).Value = cmDt;
                //sqlCtx.Param(mtns::Pcboqcrepair_Defectinfo.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePcboqcrepairDefectinfo(Pcboqcrepair_DefectinfoInfo condition)
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
                Pcboqcrepair_Defectinfo cond = FuncNew.SetColumnFromField<Pcboqcrepair_Defectinfo, Pcboqcrepair_DefectinfoInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Pcboqcrepair_Defectinfo>(new ConditionCollection<Pcboqcrepair_Defectinfo>(new EqualCondition<Pcboqcrepair_Defectinfo>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcboqcrepair_Defectinfo, Pcboqcrepair_DefectinfoInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SMTLineDef> GetSMTLineList()
        {
            try
            {
                IList<SMTLineDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = mtns::FuncNew.GetCommonSelect<mtns::Smtline>(tk, mtns::Smtline.fn_line);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Smtline, SMTLineDef, SMTLineDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveSMTLine(SMTLineDef condition)
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
                Smtline cond = FuncNew.SetColumnFromField<Smtline, SMTLineDef>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Smtline>(new ConditionCollection<Smtline>(new EqualCondition<Smtline>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Smtline, SMTLineDef>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SMTLineDef> GetExistSMTLine(SMTLineDef condition)
        {
            try
            {
                IList<SMTLineDef> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Smtline cond = mtns::FuncNew.SetColumnFromField<mtns::Smtline, SMTLineDef>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Smtline>(null, null, new mtns::ConditionCollection<mtns::Smtline>(new mtns::EqualCondition<mtns::Smtline>(cond)), mtns::Smtline.fn_line);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Smtline, SMTLineDef>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Smtline, SMTLineDef, SMTLineDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddSMTLine(SMTLineDef item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Smtline>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Smtline, SMTLineDef>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Smtline.fn_cdt).Value = cmDt;
                sqlCtx.Param(Smtline.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ChangeSMTLine(SMTLineDef setValue, SMTLineDef condition)
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
                Smtline cond = FuncNew.SetColumnFromField<Smtline, SMTLineDef>(condition);
                Smtline setv = FuncNew.SetColumnFromField<Smtline, SMTLineDef>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Smtline>(new SetValueCollection<Smtline>(new CommonSetValue<Smtline>(setv)), new ConditionCollection<Smtline>(new EqualCondition<Smtline>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Smtline, SMTLineDef>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Smtline, SMTLineDef>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Smtline.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetLineList()
        {
            try
            {
                DataTable ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Dept>(tk, null, new string[] { _Metas.Dept.fn_remark, _Metas.Dept.fn_line }, new ConditionCollection<_Metas.Dept>(), _Metas.Dept.fn_section, string.Format("SUBSTRING({0},4,1)", _Metas.Dept.fn_line), string.Format("SUBSTRING({0},3,1)", _Metas.Dept.fn_line));
                    }
                }
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetSectionList(DeptInfo condition)
        {
            try
            {
                IList<string> ret = new List<string>();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Dept cond = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Dept>("DISTINCT", new string[]{mtns::Dept.fn_section}, new mtns::ConditionCollection<mtns::Dept>(new mtns::EqualCondition<mtns::Dept>(cond)), mtns::Dept.fn_section);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Dept.fn_section)));
                        }
                    }
                }
                if (!ret.Contains("SMT1A"))
                    ret.Add("SMT1A");
                if (!ret.Contains("SMT1B"))
                    ret.Add("SMT1B");
                return (from item in ret orderby item select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeptInfo> GetSectionList(DeptInfo eqCondition, DeptInfo likeCondition)
        {
            try
            {
                IList<DeptInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Dept cond = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(eqCondition);
                mtns::Dept cond2 = mtns.FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(likeCondition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Dept>(null, null, new mtns::ConditionCollection<mtns::Dept>(new mtns::EqualCondition<mtns::Dept>(cond), new LikeCondition<mtns::Dept>(cond2)), mtns::Dept.fn_dept, mtns::Dept.fn_section, mtns.Dept.fn_line, mtns.Dept.fn_fisline);
                var sqlCtx2 = FuncNew.GetConditionedSelect<mtns::Dept>(null, new string[] { mtns::Dept.fn_dept }, new ConditionCollection<mtns::Dept>(new LikeCondition<mtns::Dept>(cond2)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx2, likeCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Dept, DeptInfo, DeptInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeptInfo> GetDeptInfoList()
        {
            try
            {
                IList<DeptInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = mtns::FuncNew.GetCommonSelect<mtns::Dept>(tk, mtns::Dept.fn_section, string.Format("SUBSTRING({0},4,1)", mtns::Dept.fn_line), string.Format("SUBSTRING({0},3,1)", mtns::Dept.fn_line));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Dept, DeptInfo, DeptInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DeptInfo> GetDeptInfoList(DeptInfo condition)
        {
            try
            {
                IList<DeptInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Dept cond = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Dept>(null, null, new mtns::ConditionCollection<mtns::Dept>(new mtns::EqualCondition<mtns::Dept>(cond)), mtns::Dept.fn_dept, mtns::Dept.fn_section, mtns.Dept.fn_line, mtns.Dept.fn_fisline);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Dept, DeptInfo, DeptInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDeptInfo(DeptInfo condition)
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
                mtns::Dept cond = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<mtns::Dept>(new ConditionCollection<mtns::Dept>(new EqualCondition<mtns::Dept>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddDeptInfo(DeptInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Dept>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Dept.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Dept.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDeptInfo(DeptInfo setValue, DeptInfo condition)
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
                mtns::Dept cond = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(condition);
                mtns::Dept setv = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<mtns::Dept>(new SetValueCollection<mtns::Dept>(new CommonSetValue<mtns::Dept>(setv)), new ConditionCollection<mtns::Dept>(new EqualCondition<mtns::Dept>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Dept.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDeptList()
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Dept>("DISTINCT", new string[]{mtns::Dept.fn_dept}, new mtns::ConditionCollection<mtns::Dept>(), mtns::Dept.fn_dept);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Dept.fn_dept)));
                        }
                    }
                }
                if (!ret.Contains("SMT"))
                    ret.Add("SMT");
                return (from item in ret orderby item select item).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddFamilyMbInfo(FamilyMbInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Family_MB>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Family_MB.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Family_MB.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteFamilyMbInfo(FamilyMbInfo condition)
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
                mtns::Family_MB cond = FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<mtns::Family_MB>(new ConditionCollection<mtns::Family_MB>(new EqualCondition<mtns::Family_MB>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ModifyFamilyMbInfo(FamilyMbInfo setValue, FamilyMbInfo condition)
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
                mtns::Family_MB cond = FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(condition);
                mtns::Family_MB setv = FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<mtns::Family_MB>(new SetValueCollection<mtns::Family_MB>(new CommonSetValue<mtns::Family_MB>(setv)), new ConditionCollection<mtns::Family_MB>(new EqualCondition<mtns::Family_MB>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Family_MB.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<FamilyMbInfo> GetFamilyMbInfoList(FamilyMbInfo condition)
        {
            try
            {
                IList<FamilyMbInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Family_MB cond = mtns::FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Family_MB>(null, null, new mtns::ConditionCollection<mtns::Family_MB>(new mtns::EqualCondition<mtns::Family_MB>(cond)), mtns::Family_MB.fn_mb);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Family_MB, FamilyMbInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Family_MB, FamilyMbInfo, FamilyMbInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetFamilyListFromFamilyMbByLike(string familyPrefix)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Family_MB cond = new mtns::Family_MB();
                        cond.family = familyPrefix + "%";
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Family_MB>(tk, "DISTINCT", new string[]{mtns::Family_MB.fn_family}, new mtns::ConditionCollection<mtns::Family_MB>(
                            new mtns::LikeCondition<mtns::Family_MB>(cond)
                            ), 
                            mtns::Family_MB.fn_family);
                    }
                }
                sqlCtx.Param(_Metas.Family_MB.fn_family).Value = familyPrefix + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            ret.Add(g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Family_MB.fn_family)));
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

		public IList<string> GetFamilyListFromFamilyMb()//ByLike(string familyPrefix)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        //mtns::Family_MB cond = new mtns::Family_MB();
                        //cond.family = familyPrefix + "%";
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Family_MB>(tk, "DISTINCT", new string[]{mtns::Family_MB.fn_family}, new mtns::ConditionCollection<mtns::Family_MB>(
                            //new mtns::LikeCondition<mtns::Family_MB>(cond)
                            ), 
                            mtns::Family_MB.fn_family);
                    }
                }
                //sqlCtx.Param(_Metas.Family_MB.fn_family).Value = familyPrefix + "%";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            ret.Add(g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.Family_MB.fn_family)));
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

        public IList<SmttimeInfo> GetSMTTimeInfoList(DateTime queryDate)
        {
            try
            {
                IList<SmttimeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Smttime cond = new mtns::Smttime();
                        cond.date = queryDate;
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Smttime>(tk, null, null, new mtns::ConditionCollection<mtns::Smttime>(
                            new mtns::EqualCondition<mtns::Smttime>(cond, "DATENAME(yyyy,{0})", "DATENAME(yyyy,{0})"),
                            new mtns::EqualCondition<mtns::Smttime>(cond, "DATENAME(mm,{0})", "DATENAME(mm,{0})"),
                            new mtns::EqualCondition<mtns::Smttime>(cond, "DATENAME(dd,{0})", "DATENAME(dd,{0})")
                            ), string.Format("SUBSTRING({0},4,1)", mtns::Smttime.fn_line), string.Format("SUBSTRING({0},3,1)", mtns::Smttime.fn_line));
                    }
                }
                sqlCtx.Param(mtns::Smttime.fn_date).Value = queryDate;
                sqlCtx.Param(mtns::Smttime.fn_date + "$1").Value = queryDate;
                sqlCtx.Param(mtns::Smttime.fn_date + "$2").Value = queryDate;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Smttime, SmttimeInfo, SmttimeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddSMTTimeInfo(SmttimeInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<_Metas.Smttime>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<_Metas.Smttime, IMES.DataModel.SmttimeInfo>(sqlCtx, item);

                sqlCtx.Param(_Metas.Smttime.fn_cdt).Value = cmDt;
                sqlCtx.Param(_Metas.Smttime.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSMTTimeInfo(SmttimeInfo condition)
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
                Smttime cond = FuncNew.SetColumnFromField<Smttime, SmttimeInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Smttime>(new ConditionCollection<Smttime>(new EqualCondition<Smttime>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Smttime, SmttimeInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistSMTTimeInfo(SmttimeInfo condition)
        {
            try
            {
                bool ret = false;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Smttime cond = mtns::FuncNew.SetColumnFromField<mtns::Smttime, SmttimeInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Smttime>("COUNT", new string[] { mtns.Smttime.fn_id }, new mtns::ConditionCollection<mtns::Smttime>(new mtns::EqualCondition<mtns::Smttime>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Smttime, SmttimeInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
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

        public void UpdateSMTTimeInfo(SmttimeInfo setValue, SmttimeInfo condition)
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
                mtns::Smttime cond = mtns::FuncNew.SetColumnFromField<mtns::Smttime, SmttimeInfo>(condition);
                mtns::Smttime setv = mtns::FuncNew.SetColumnFromField<mtns::Smttime, SmttimeInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Smttime>(new mtns::SetValueCollection<mtns::Smttime>(new mtns::CommonSetValue<mtns::Smttime>(setv)), new mtns::ConditionCollection<mtns::Smttime>(new mtns::EqualCondition<mtns::Smttime>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Smttime, SmttimeInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Smttime, SmttimeInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Smttime.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SMTLineDef> GetSMTLineInfoListByLineList(IList<string> lineList)
        {
            try
            {
                IList<SMTLineDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Smtline cond = new Smtline();
                        cond.line = "[INSET]";
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Smtline>(tk, null, null, new mtns::ConditionCollection<mtns::Smtline>(new mtns::InSetCondition<mtns::Smtline>(cond)), mtns::Smtline.fn_line);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Smtline.fn_line), g.ConvertInSet(new List<string>(lineList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Smtline, SMTLineDef, SMTLineDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateAlarmWithSpecifiedDefectForSA(AlarmSettingInfo alarmSetting)
        {
            int ret = -1;
            try
            {
                var sCurTime = "curTime";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "INSERT {0}({26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37}) " +
                                           "SELECT 'SA', DATEADD(HOUR, -@{20}, @{19}), @{19}, @{23}, a.{6}, " +
                                                  "a.{7}, d.{18} AS Family, b.{11}  AS Defect, 'ALM2' AS ReasonCode, " +
                                                  "'Defect: '+b.{11}+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.{8}))+' >= '+ STR(@{22}) AS Reason, 'Created', @{19} " +
                                           "FROM {1} a " +
                                           "LEFT OUTER JOIN {2} b " +
                                           "ON a.{8}=b.{12} " +
                                           "INNER JOIN {3} c " +
                                           "ON a.{9}=c.{15} " +
                                           "INNER JOIN {4}..{5} d " +
                                           "ON c.{16}=d.{17} " +
                                           "WHERE a.{10}=0 " +
                                           "AND DATEDIFF(HOUR, a.{13}, @{19})<=@{20} " +
                                           "AND a.{7}=@{24} " +
                                           "AND b.{14}<>1 " +
                                           "AND charindex(RTRIM(b.{11})+',',@{21}) > 0 " +
                                           "AND d.{18}=@{25} " +
                                           "GROUP BY a.{6}, a.{7}, d.{18}, b.{11} " +
                                           "HAVING COUNT(a.{8})>=@{22}; SELECT @@IDENTITY; ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Alarm)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback_Defectinfo)),
                                                                        ToolsNew.GetTableName(typeof(mtns.Pcb)),
                                                                        _Schema.SqlHelper.DB_GetData,
                                                                        ToolsNew.GetTableName(typeof(mtns.Part_NEW)),
                                                                        Pcbtestlogback.fn_line,
                                                                        Pcbtestlogback.fn_station,
                                                                        Pcbtestlogback.fn_id,
                                                                        Pcbtestlogback.fn_pcbno,
                                                                        Pcbtestlogback.fn_status,
                                                                        Pcbtestlogback_Defectinfo.fn_defectCodeID,
                                                                        Pcbtestlogback_Defectinfo.fn_pcbtestlogbackid,
                                                                        Pcbtestlogback.fn_cdt,
                                                                        ProductTestLogBack_DefectInfo.fn_triggerAlarm,
                                                                        mtns.Pcb.fn_pcbno,
                                                                        mtns.Pcb.fn_pcbmodelid,
                                                                        mtns.Part_NEW.fn_partNo,
                                                                        mtns.Part_NEW.fn_descr,
                                                                        sCurTime,
                                                                        AlarmSetting.fn_period,
                                                                        AlarmSetting.fn_defects,
                                                                        AlarmSetting.fn_defectQty,
                                                                        AlarmSetting.fn_id,
                                                                        AlarmSetting.fn_station,
                                                                        AlarmSetting.fn_family,
                                                                        Alarm.fn_stage,
                                                                        Alarm.fn_startTime,
                                                                        Alarm.fn_endTime,
                                                                        Alarm.fn_alarmSettingID,
                                                                        Alarm.fn_line,
                                                                        Alarm.fn_station,
                                                                        Alarm.fn_family,
                                                                        Alarm.fn_defect,
                                                                        Alarm.fn_reasonCode,
                                                                        Alarm.fn_reason,
                                                                        Alarm.fn_status,
                                                                        Alarm.fn_cdt
                                                                        );

                        sqlCtx.AddParam(mtns.AlarmSetting.fn_station, new SqlParameter("@" + mtns.AlarmSetting.fn_station, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_station)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_family, new SqlParameter("@" + mtns.AlarmSetting.fn_family, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_family)));
                        sqlCtx.AddParam(sCurTime, new SqlParameter("@" + sCurTime, SqlDbType.DateTime));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_period, new SqlParameter("@" + mtns.AlarmSetting.fn_period, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_period)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_defects, new SqlParameter("@" + mtns.AlarmSetting.fn_defects, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_defects)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_defectQty, new SqlParameter("@" + mtns.AlarmSetting.fn_defectQty, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_defectQty)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_id, new SqlParameter("@" + mtns.AlarmSetting.fn_id, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_id)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(_Metas.AlarmSetting.fn_station).Value = alarmSetting.Station;
                sqlCtx.Param(_Metas.AlarmSetting.fn_family).Value = alarmSetting.Family;
                if (alarmSetting.CurrTime == DateTime.MinValue)
                {
                    DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                    alarmSetting.CurrTime = cmDt;
                }
                sqlCtx.Param(sCurTime).Value = alarmSetting.CurrTime;
                sqlCtx.Param(_Metas.AlarmSetting.fn_period).Value = alarmSetting.Period;
                sqlCtx.Param(_Metas.AlarmSetting.fn_defects).Value = alarmSetting.Defects;
                sqlCtx.Param(_Metas.AlarmSetting.fn_defectQty).Value = alarmSetting.DefectQty;
                sqlCtx.Param(_Metas.AlarmSetting.fn_id).Value = alarmSetting.Id;

                //_Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                ret = _Schema.SqlHelper.ExecuteScalarForAquireIdInsertWithTry(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void  UpdateForCreateAlarmWithDefectForSA(AlarmSettingInfo alarmSetting,int alarm_id)
        {
            try
            {
                var sCurTime = "curTime";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "UPDATE {0} " +
                                           "SET {6}=1 " +
                                           "WHERE {7} IN " +
                                           "( " +
                                               "SELECT a.{7} " +
                                               "FROM {1} b " +
                                               "LEFT OUTER JOIN {2} c " +
                                               "ON b.{10}=c.{15} " +
                                               "AND b.{11}=c.{16} " +
                                               "LEFT OUTER JOIN {0} a " +
                                               "ON a.{8}=c.{17} " +
                                               "INNER JOIN {3} d " +
                                               "ON d.{21}=c.{18} " +
                                               "INNER JOIN {4}..{5} e " +
                                               "ON d.{22}=e.{23} " +
                                               "WHERE b.{12}={27} " +
                                               "AND c.{19}=0 " +
                                               "AND c.{20}>=DATEADD(HOUR, -@{26}, @{25}) " +
                                               "AND c.{20}<=@{25} " +
                                               "AND a.{9}=b.{13} " +
                                               "AND e.{24}=b.{14} " +
                                           ") ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Pcbtestlogback_Defectinfo)),  //0
                                                                        ToolsNew.GetTableName(typeof(Alarm)),                       //1
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback)),              //2
                                                                        ToolsNew.GetTableName(typeof(mtns.Pcb)),                    //3
                                                                        _Schema.SqlHelper.DB_GetData,                               //4
                                                                        ToolsNew.GetTableName(typeof(mtns.Part_NEW)),               //5
                                                                        Pcbtestlogback_Defectinfo.fn_triggerAlarm,                  //6
                                                                        Pcbtestlogback_Defectinfo.fn_id,                            //7
                                                                        Pcbtestlogback_Defectinfo.fn_pcbtestlogbackid,              //8
                                                                        Pcbtestlogback_Defectinfo.fn_defectCodeID,                  //9
                                                                        Alarm.fn_line,                                              //10
                                                                        Alarm.fn_station,                                           //11
                                                                        Alarm.fn_id,                                                //12
                                                                        Alarm.fn_defect,                                            //13
                                                                        Alarm.fn_family,                                            //14
                                                                        Pcbtestlogback.fn_line,                                     //15
                                                                        Pcbtestlogback.fn_station,                                  //16
                                                                        Pcbtestlogback.fn_id,                                       //17
                                                                        Pcbtestlogback.fn_pcbno,                                    //18
                                                                        Pcbtestlogback.fn_status,                                   //19
                                                                        Pcbtestlogback.fn_cdt,                                      //20
                                                                        mtns.Pcb.fn_pcbno,                                          //21
                                                                        mtns.Pcb.fn_pcbmodelid,                                     //22
                                                                        mtns.Part_NEW.fn_partNo,                                    //23
                                                                        mtns.Part_NEW.fn_descr,                                     //24
                                                                        sCurTime,                                                   //25
                                                                        AlarmSetting.fn_period,                                     //26
                                                                        alarm_id                                                    //27
                                                                        );

                        sqlCtx.AddParam(sCurTime, new SqlParameter("@" + sCurTime, SqlDbType.DateTime));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_period, new SqlParameter("@" + mtns.AlarmSetting.fn_period, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_period)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                if (alarmSetting.CurrTime == DateTime.MinValue)
                {
                    DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                    alarmSetting.CurrTime = cmDt;
                }
                sqlCtx.Param(sCurTime).Value = alarmSetting.CurrTime;
                sqlCtx.Param(_Metas.AlarmSetting.fn_period).Value = alarmSetting.Period;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateAlarmWithExcludedDefectForSA(AlarmSettingInfo alarmSetting)
        {
            int ret = -1;
            try
            {
                var sCurTime = "curTime";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "INSERT {0}({26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37}) " +
                                           "SELECT 'SA', DATEADD(HOUR, -@{20}, @{19}), @{19}, @{23}, a.{6}, " +
                                                  "a.{7}, d.{18} AS Family, b.{11}  AS Defect, 'ALM2' AS ReasonCode, " +
                                                  "'Defect: '+b.{11}+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.{8}))+' >= '+ STR(@{22}) AS Reason, 'Created', @{19} " +
                                           "FROM {1} a " +
                                           "LEFT OUTER JOIN {2} b " +
                                           "ON a.{8}=b.{12} " +
                                           "INNER JOIN {3} c " +
                                           "ON a.{9}=c.{15} " +
                                           "INNER JOIN {4}..{5} d " +
                                           "ON c.{16}=d.{17} " +
                                           "WHERE a.{10}=0 " +
                                           "AND DATEDIFF(HOUR, a.{13}, @{19})<=@{20} " +
                                           "AND a.{7}=@{24} " +
                                           "AND b.{14}<>1 " +
                                           "AND charindex(RTRIM(b.{11})+',',@{21})=0 " +
                                           "AND d.{18}=@{25} " +
                                           "GROUP BY a.{6}, a.{7}, d.{18}, b.{11} " +
                                           "HAVING COUNT(a.{8})>=@{22}; SELECT @@IDENTITY; ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Alarm)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback_Defectinfo)),
                                                                        ToolsNew.GetTableName(typeof(mtns.Pcb)),
                                                                        _Schema.SqlHelper.DB_GetData,
                                                                        ToolsNew.GetTableName(typeof(mtns.Part_NEW)),
                                                                        Pcbtestlogback.fn_line,
                                                                        Pcbtestlogback.fn_station,
                                                                        Pcbtestlogback.fn_id,
                                                                        Pcbtestlogback.fn_pcbno,
                                                                        Pcbtestlogback.fn_status,
                                                                        Pcbtestlogback_Defectinfo.fn_defectCodeID,
                                                                        Pcbtestlogback_Defectinfo.fn_pcbtestlogbackid,
                                                                        Pcbtestlogback.fn_cdt,
                                                                        ProductTestLogBack_DefectInfo.fn_triggerAlarm,
                                                                        mtns.Pcb.fn_pcbno,
                                                                        mtns.Pcb.fn_pcbmodelid,
                                                                        mtns.Part_NEW.fn_partNo,
                                                                        mtns.Part_NEW.fn_descr,
                                                                        sCurTime,
                                                                        AlarmSetting.fn_period,
                                                                        AlarmSetting.fn_defects,
                                                                        AlarmSetting.fn_defectQty,
                                                                        AlarmSetting.fn_id,
                                                                        AlarmSetting.fn_station,
                                                                        AlarmSetting.fn_family,
                                                                        Alarm.fn_stage,
                                                                        Alarm.fn_startTime,
                                                                        Alarm.fn_endTime,
                                                                        Alarm.fn_alarmSettingID,
                                                                        Alarm.fn_line,
                                                                        Alarm.fn_station,
                                                                        Alarm.fn_family,
                                                                        Alarm.fn_defect,
                                                                        Alarm.fn_reasonCode,
                                                                        Alarm.fn_reason,
                                                                        Alarm.fn_status,
                                                                        Alarm.fn_cdt
                                                                        );

                        sqlCtx.AddParam(mtns.AlarmSetting.fn_station, new SqlParameter("@" + mtns.AlarmSetting.fn_station, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_station)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_family, new SqlParameter("@" + mtns.AlarmSetting.fn_family, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_family)));
                        sqlCtx.AddParam(sCurTime, new SqlParameter("@" + sCurTime, SqlDbType.DateTime));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_period, new SqlParameter("@" + mtns.AlarmSetting.fn_period, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_period)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_defects, new SqlParameter("@" + mtns.AlarmSetting.fn_defects, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_defects)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_defectQty, new SqlParameter("@" + mtns.AlarmSetting.fn_defectQty, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_defectQty)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_id, new SqlParameter("@" + mtns.AlarmSetting.fn_id, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_id)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(_Metas.AlarmSetting.fn_station).Value = alarmSetting.Station;
                sqlCtx.Param(_Metas.AlarmSetting.fn_family).Value = alarmSetting.Family;
                if (alarmSetting.CurrTime == DateTime.MinValue)
                {
                    DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                    alarmSetting.CurrTime = cmDt;
                }
                sqlCtx.Param(sCurTime).Value = alarmSetting.CurrTime;
                sqlCtx.Param(_Metas.AlarmSetting.fn_period).Value = alarmSetting.Period;
                sqlCtx.Param(_Metas.AlarmSetting.fn_defects).Value = alarmSetting.Defects;
                sqlCtx.Param(_Metas.AlarmSetting.fn_defectQty).Value = alarmSetting.DefectQty;
                sqlCtx.Param(_Metas.AlarmSetting.fn_id).Value = alarmSetting.Id;

                //_Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                ret = _Schema.SqlHelper.ExecuteScalarForAquireIdInsertWithTry(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateAlarmWithAllDefectForSA(AlarmSettingInfo alarmSetting)
        {
            int ret = -1;
            try
            {
                var sCurTime = "curTime";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "INSERT {0}({26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37}) " +
                                           "SELECT 'SA', DATEADD(HOUR, -@{20}, @{19}), @{19}, @{23}, a.{6}, " +
                                                  "a.{7}, d.{18} AS Family, b.{11}  AS Defect, 'ALM2' AS ReasonCode, " +
                                                  "'Defect: '+b.{11}+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.{8}))+' >= '+ STR(@{22}) AS Reason, 'Created', @{19} " +
                                           "FROM {1} a " +
                                           "LEFT OUTER JOIN {2} b " +
                                           "ON a.{8}=b.{12} " +
                                           "INNER JOIN {3} c " +
                                           "ON a.{9}=c.{15} " +
                                           "INNER JOIN {4}..{5} d " +
                                           "ON c.{16}=d.{17} " +
                                           "WHERE a.{10}=0 " +
                                           "AND DATEDIFF(HOUR, a.{13}, @{19})<=@{20} " +
                                           "AND a.{7}=@{24} " +
                                           "AND b.{14}<>1 " +
                                           //"AND b.{11}+',' LIKE @{21} " +
                                           "AND d.{18}=@{25} " +
                                           "GROUP BY a.{6}, a.{7}, d.{18}, b.{11} " +
                                           "HAVING COUNT(a.{8})>=@{22}; SELECT @@IDENTITY; ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Alarm)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback_Defectinfo)),
                                                                        ToolsNew.GetTableName(typeof(mtns.Pcb)),
                                                                        _Schema.SqlHelper.DB_GetData,
                                                                        ToolsNew.GetTableName(typeof(mtns.Part_NEW)),
                                                                        Pcbtestlogback.fn_line,
                                                                        Pcbtestlogback.fn_station,
                                                                        Pcbtestlogback.fn_id,
                                                                        Pcbtestlogback.fn_pcbno,
                                                                        Pcbtestlogback.fn_status,
                                                                        Pcbtestlogback_Defectinfo.fn_defectCodeID,
                                                                        Pcbtestlogback_Defectinfo.fn_pcbtestlogbackid,
                                                                        Pcbtestlogback.fn_cdt,
                                                                        ProductTestLogBack_DefectInfo.fn_triggerAlarm,
                                                                        mtns.Pcb.fn_pcbno,
                                                                        mtns.Pcb.fn_pcbmodelid,
                                                                        mtns.Part_NEW.fn_partNo,
                                                                        mtns.Part_NEW.fn_descr,
                                                                        sCurTime,
                                                                        AlarmSetting.fn_period,
                                                                        "",//AlarmSetting.fn_defects,
                                                                        AlarmSetting.fn_defectQty,
                                                                        AlarmSetting.fn_id,
                                                                        AlarmSetting.fn_station,
                                                                        AlarmSetting.fn_family,
                                                                        Alarm.fn_stage,
                                                                        Alarm.fn_startTime,
                                                                        Alarm.fn_endTime,
                                                                        Alarm.fn_alarmSettingID,
                                                                        Alarm.fn_line,
                                                                        Alarm.fn_station,
                                                                        Alarm.fn_family,
                                                                        Alarm.fn_defect,
                                                                        Alarm.fn_reasonCode,
                                                                        Alarm.fn_reason,
                                                                        Alarm.fn_status,
                                                                        Alarm.fn_cdt
                                                                        );

                        sqlCtx.AddParam(mtns.AlarmSetting.fn_station, new SqlParameter("@" + mtns.AlarmSetting.fn_station, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_station)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_family, new SqlParameter("@" + mtns.AlarmSetting.fn_family, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_family)));
                        sqlCtx.AddParam(sCurTime, new SqlParameter("@" + sCurTime, SqlDbType.DateTime));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_period, new SqlParameter("@" + mtns.AlarmSetting.fn_period, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_period)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_defects, new SqlParameter("@" + mtns.AlarmSetting.fn_defects, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_defects)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_defectQty, new SqlParameter("@" + mtns.AlarmSetting.fn_defectQty, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_defectQty)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_id, new SqlParameter("@" + mtns.AlarmSetting.fn_id, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_id)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(_Metas.AlarmSetting.fn_station).Value = alarmSetting.Station;
                sqlCtx.Param(_Metas.AlarmSetting.fn_family).Value = alarmSetting.Family;
                if (alarmSetting.CurrTime == DateTime.MinValue)
                {
                    DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                    alarmSetting.CurrTime = cmDt;
                }
                sqlCtx.Param(sCurTime).Value = alarmSetting.CurrTime;
                sqlCtx.Param(_Metas.AlarmSetting.fn_period).Value = alarmSetting.Period;
                sqlCtx.Param(_Metas.AlarmSetting.fn_defects).Value = alarmSetting.Defects;
                sqlCtx.Param(_Metas.AlarmSetting.fn_defectQty).Value = alarmSetting.DefectQty;
                sqlCtx.Param(_Metas.AlarmSetting.fn_id).Value = alarmSetting.Id;

                //_Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                ret = _Schema.SqlHelper.ExecuteScalarForAquireIdInsertWithTry(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateAlarmWithYieldForSA(AlarmSettingInfo alarmSetting)
        {
            int ret = -1;
            try
            {
                var sCurTime = "curTime";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "SELECT a.{5}, COUNT(a.{6}) AS Total INTO #temp " +
                                            "FROM {1} a, {2} b, {3}..{4} c " +
                                            "WHERE DATEDIFF(HOUR, a.{7}, @{14})<=@{15} " +
                                            "AND a.{8}=@{16} " +
                                            "AND a.{9}=b.{10} " +
                                            "AND b.{11}=c.{12} " +
                                            "AND c.{13}=@{17} " +
                                            "GROUP BY a.{5} " +
                                            "HAVING COUNT(a.{6})>=@{18} " +

                                            "INSERT {0} ({21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32}) " +
                                            "SELECT 'SA', DATEADD(HOUR, -@{15}, @{14}), @{14}, @{20}, a.{5}, " +
                                            "b.{8}, d.{13}, '' AS Defect, 'ALM1' AS ReasonCode, " +
                                            "CONVERT(VARCHAR,COUNT(b.{6}))+' / '+ CONVERT(VARCHAR,a.Total)+' < '+ CONVERT(VARCHAR,@{19}) +'%' AS Reason, 'Created', @{14} " +
                                            "FROM #temp a, {1} b, {2} c, {3}..{4} d " +
                                            "WHERE a.{5}=b.{5} " +
                                            "AND DATEDIFF(HOUR, b.{7}, @{14})<=@{15} " +
                                            "AND b.{8}=@{16} " +
                                            "AND b.{9}=c.{10} " +
                                            "AND c.{11}=d.{12} " +
                                            "AND d.{13}=@{17} " +
                                            "And b.Status=1 " +
                                            "GROUP BY a.{5}, b.{8}, d.{13}, a.Total " +
                                            "HAVING COUNT(b.{6})*100<@{19}*a.Total " +

                                            "DROP TABLE #temp ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Alarm)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback)),
                                                                        ToolsNew.GetTableName(typeof(mtns.Pcb)),
                                                                        _Schema.SqlHelper.DB_GetData,
                                                                        ToolsNew.GetTableName(typeof(mtns.Part_NEW)),
                                                                        Pcbtestlogback.fn_line,
                                                                        Pcbtestlogback.fn_id,
                                                                        Pcbtestlogback.fn_cdt,
                                                                        Pcbtestlogback.fn_station,
                                                                        Pcbtestlogback.fn_pcbno,
                                                                        mtns.Pcb.fn_pcbno,
                                                                        mtns.Pcb.fn_pcbmodelid,
                                                                        mtns.Part_NEW.fn_partNo,
                                                                        mtns.Part_NEW.fn_descr,
                                                                        sCurTime,
                                                                        AlarmSetting.fn_period,
                                                                        AlarmSetting.fn_station,
                                                                        AlarmSetting.fn_family,
                                                                        AlarmSetting.fn_minQty,
                                                                        AlarmSetting.fn_yieldRate,
                                                                        AlarmSetting.fn_id,
                                                                        Alarm.fn_stage,
                                                                        Alarm.fn_startTime,
                                                                        Alarm.fn_endTime,
                                                                        Alarm.fn_alarmSettingID,
                                                                        Alarm.fn_line,
                                                                        Alarm.fn_station,
                                                                        Alarm.fn_family,
                                                                        Alarm.fn_defect,
                                                                        Alarm.fn_reasonCode,
                                                                        Alarm.fn_reason,
                                                                        Alarm.fn_status,
                                                                        Alarm.fn_cdt
                                                                        );
                        sqlCtx.AddParam(sCurTime, new SqlParameter("@" + sCurTime, SqlDbType.DateTime));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_period, new SqlParameter("@" + mtns.AlarmSetting.fn_period, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_period)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_station, new SqlParameter("@" + mtns.AlarmSetting.fn_station, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_station)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_family, new SqlParameter("@" + mtns.AlarmSetting.fn_family, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_family)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_minQty, new SqlParameter("@" + mtns.AlarmSetting.fn_minQty, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_minQty)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_yieldRate, new SqlParameter("@" + mtns.AlarmSetting.fn_yieldRate, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_yieldRate)));
                        sqlCtx.AddParam(mtns.AlarmSetting.fn_id, new SqlParameter("@" + mtns.AlarmSetting.fn_id, ToolsNew.GetDBFieldType<mtns.AlarmSetting>(mtns.AlarmSetting.fn_id)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                if (alarmSetting.CurrTime == DateTime.MinValue)
                {
                    DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                    alarmSetting.CurrTime = cmDt;
                }
                sqlCtx.Param(sCurTime).Value = alarmSetting.CurrTime;
                sqlCtx.Param(_Metas.AlarmSetting.fn_period).Value = alarmSetting.Period;
                sqlCtx.Param(_Metas.AlarmSetting.fn_station).Value = alarmSetting.Station;
                sqlCtx.Param(_Metas.AlarmSetting.fn_family).Value = alarmSetting.Family;
                sqlCtx.Param(_Metas.AlarmSetting.fn_minQty).Value = alarmSetting.MinQty;
                sqlCtx.Param(_Metas.AlarmSetting.fn_yieldRate).Value = alarmSetting.YieldRate;
                sqlCtx.Param(_Metas.AlarmSetting.fn_id).Value = alarmSetting.Id;

                //_Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                ret = _Schema.SqlHelper.ExecuteScalarForAquireIdInsertWithTry(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePcbRepair(Repair setValue, Repair condition)
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
                Pcbrepair cond = FuncNew.SetColumnFromField<Pcbrepair, Repair>(condition);
                Pcbrepair setv = FuncNew.SetColumnFromField<Pcbrepair, Repair>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Pcbrepair>(new SetValueCollection<Pcbrepair>(new CommonSetValue<Pcbrepair>(setv)), new ConditionCollection<Pcbrepair>(new EqualCondition<Pcbrepair>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcbrepair, Repair>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pcbrepair, Repair>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Pcbrepair.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPcbRepair(Repair item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcbrepair>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Pcbrepair, Repair>(sqlCtx, item);
                sqlCtx.Param(_Metas.Pcbrepair.fn_status).Value =  Convert.ToInt32(item.Status);
                if (item.TestLogID != null)
                    sqlCtx.Param(_Metas.Pcbrepair.fn_testLogID).Value = Convert.ToInt32(item.TestLogID);

                item.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPcbRepairDefect(RepairDefect item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Pcbrepair_Defectinfo>(tk);
                    }
                }
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn__4M_).Value = item._4M;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_action).Value = item.Action;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_cause).Value = item.Cause;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_cdt).Value = item.Cdt;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_component).Value = item.Component;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_cover).Value = item.Cover;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_defectCode).Value = item.DefectCodeID;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_distribution).Value = item.Distribution;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_editor).Value = item.Editor;
 
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_isManual).Value = item.IsManual;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_location).Value = item.Location;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_majorPart).Value = item.MajorPart;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_manufacture).Value = item.Manufacture;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_mark).Value = item.Mark;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_mtaid).Value = item.MTAID;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_newPart).Value = item.NewPart;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_newPartDateCode).Value = item.NewPartDateCode;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_newPartSno).Value = item.NewPartSno;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_obligation).Value = item.Obligation;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_oldPart).Value = item.OldPart;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_oldPartSno).Value = item.OldPartSno;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_partType).Value = item.PartType;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_pcarepairid).Value = item.RepairID;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_piastation).Value = item.PIAStation;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_remark).Value = item.Remark;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_responsibility).Value = item.Responsibility;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_returnSign).Value = item.ReturnSign;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_side).Value = item.Side;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_site).Value = item.Site;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_subDefect).Value = item.SubDefect;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_trackingStatus).Value = item.TrackingStatus;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_type).Value = item.Type;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_udt).Value = item.Udt;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_uncover).Value = item.Uncover;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_vendorCT).Value = item.VendorCT;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_versionA).Value = item.VersionA;
                sqlCtx.Param(mtns::Pcbrepair_Defectinfo.fn_versionB).Value = item.VersionB;
                item.ID = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePcb(PcbEntityInfo setValue, PcbEntityInfo condition)
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
                Pcb cond = FuncNew.SetColumnFromField<Pcb, PcbEntityInfo>(condition);
                Pcb setv = FuncNew.SetColumnFromField<Pcb, PcbEntityInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Pcb>(new SetValueCollection<Pcb>(new CommonSetValue<Pcb>(setv)), new ConditionCollection<Pcb>(new EqualCondition<Pcb>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcb, PcbEntityInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pcb, PcbEntityInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Pcb.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePcbInfo(fomb.MBInfo setValue, fomb.MBInfo condition)
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
                Pcbinfo cond = FuncNew.SetColumnFromField<Pcbinfo, fomb.MBInfo>(condition);
                Pcbinfo setv = FuncNew.SetColumnFromField<Pcbinfo, fomb.MBInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Pcbinfo>(new SetValueCollection<Pcbinfo>(new CommonSetValue<Pcbinfo>(setv)), new ConditionCollection<Pcbinfo>(new EqualCondition<Pcbinfo>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcbinfo, fomb.MBInfo>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pcbinfo, fomb.MBInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Pcbinfo.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePcbStatus(MBStatus setValue, MBStatus condition)
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
                Pcbstatus cond = FuncNew.SetColumnFromField<Pcbstatus, MBStatus>(condition);
                Pcbstatus setv = FuncNew.SetColumnFromField<Pcbstatus, MBStatus>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = FuncNew.GetConditionedUpdate<Pcbstatus>(new SetValueCollection<Pcbstatus>(new CommonSetValue<Pcbstatus>(setv)), new ConditionCollection<Pcbstatus>(new EqualCondition<Pcbstatus>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Pcbstatus, MBStatus>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<Pcbstatus, MBStatus>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.Pcbstatus.fn_udt)).Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<TestLog> GetPCBTestLogInfo(TestLog eqCondition, TestLog notNullCondition)
        {
            try
            {
                IList<TestLog> ret = null;

                if (eqCondition == null)
                    eqCondition = new TestLog();
                if (notNullCondition == null)
                    notNullCondition = new TestLog();

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {

                _Metas.Pcbtestlog cond = FuncNew.SetColumnFromField<_Metas.Pcbtestlog, TestLog>(eqCondition);
                _Metas.Pcbtestlog cond2 = FuncNew.SetColumnFromField<_Metas.Pcbtestlog, TestLog>(notNullCondition);

                if (eqCondition.Status != TestLog.TestLogStatus.Fail)
                {
                    cond.status = 1;
                }

                sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(null, null, new ConditionCollection<_Metas.Pcbtestlog>(new EqualCondition<_Metas.Pcbtestlog>(cond), new NotEqualCondition<_Metas.Pcbtestlog>(cond2, "ISNULL({0},'')")), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                var sqlCtx2 = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(null, new string[] { _Metas.Pcbtestlog.fn_id }, new ConditionCollection<_Metas.Pcbtestlog>(new NotEqualCondition<_Metas.Pcbtestlog>(cond2, "ISNULL({0},'')")));

                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<_Metas.Pcbtestlog, TestLog>(sqlCtx, eqCondition);
                sqlCtx2 = FuncNew.SetColumnFromField<_Metas.Pcbtestlog, TestLog>(sqlCtx2, notNullCondition);
                sqlCtx.OverrideParams(sqlCtx2);

                if (eqCondition.Status != TestLog.TestLogStatus.Fail)
                {
                    sqlCtx.Param(_Metas.Pcbtestlog.fn_status).Value = cond.status;
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pcbtestlog, TestLog, TestLog>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetRemarkAndLineFromDept(DeptInfo condition)
        {
            try
            {
                DataTable ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Dept cond = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Dept>("DISTINCT", new string[] { mtns::Dept.fn_remark, mtns::Dept.fn_line }, new mtns::ConditionCollection<mtns::Dept>(new mtns::EqualCondition<mtns::Dept>(cond)), mtns::Dept.fn_line);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Dept, DeptInfo>(sqlCtx, condition);

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PcbEntityInfo> GetPCBWithAlarm(AlarmInfo condition)
        {
            IList<PcbEntityInfo> ret = null;
            try
            {
                var id = "ID";
                var st = "StartTime";
                var et = "EndTime";

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "SELECT pcb.{4},pcb.{5},pcb.{6},pcb.{7},pcb.{8},pcb.{9},pcb.{10},pcb.{11},pcb.{12},pcb.{13},pcb.{14},pcb.{15},pcb.{16},pcb.{17},pcb.{32} FROM {0} a,{1} p,{2} pdi,{3} pcb " +
                                            "WHERE a.{18}=@{29} " +
                                            "AND a.{19}=p.{21} " +
                                            "AND a.{20}=p.{22} " +
                                            "AND p.{23}=1 " +
                                            "AND (p.{24}>@{30} AND p.{24}<=@{31}) " +
                                            "AND p.{25}=pdi.{27} " +
                                            "AND pdi.{28}=1 " +
                                            "AND p.{26}=pcb.{13} ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Alarm)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback)),
                                                                        ToolsNew.GetTableName(typeof(Pcbtestlogback_Defectinfo)),
                                                                        ToolsNew.GetTableName(typeof(mtns.Pcb)),
                                                                        mtns.Pcb.fn_cdt,
                                                                        mtns.Pcb.fn_custsn,
                                                                        mtns.Pcb.fn_custver,
                                                                        mtns.Pcb.fn_cvsn,
                                                                        mtns.Pcb.fn_dateCode,
                                                                        mtns.Pcb.fn_ecr,
                                                                        mtns.Pcb.fn_iecver,
                                                                        mtns.Pcb.fn_mac,
                                                                        mtns.Pcb.fn_pcbmodelid,
                                                                        mtns.Pcb.fn_pcbno,
                                                                        mtns.Pcb.fn_shipMode,
                                                                        mtns.Pcb.fn_smtmo,
                                                                        mtns.Pcb.fn_state,
                                                                        mtns.Pcb.fn_udt,
                                                                        Alarm.fn_id,
                                                                        Alarm.fn_line,
                                                                        Alarm.fn_station,
                                                                        Pcbtestlogback.fn_line,
                                                                        Pcbtestlogback.fn_station,
                                                                        Pcbtestlogback.fn_status,
                                                                        Pcbtestlogback.fn_cdt,
                                                                        Pcbtestlogback.fn_id,
                                                                        Pcbtestlogback.fn_pcbno,
                                                                        Pcbtestlogback_Defectinfo.fn_pcbtestlogbackid,
                                                                        Pcbtestlogback_Defectinfo.fn_triggerAlarm,
                                                                        id,
                                                                        st,
                                                                        et,
                                                                        mtns.Pcb.fn_uuid
                                                                        );

                        sqlCtx.AddParam(id, new SqlParameter("@" + id, ToolsNew.GetDBFieldType<Alarm>(Alarm.fn_id)));
                        sqlCtx.AddParam(st, new SqlParameter("@" + st, ToolsNew.GetDBFieldType<Pcbtestlogback>(Pcbtestlogback.fn_cdt)));
                        sqlCtx.AddParam(et, new SqlParameter("@" + et, ToolsNew.GetDBFieldType<Pcbtestlogback>(Pcbtestlogback.fn_cdt)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(id).Value = condition.Id;
                sqlCtx.Param(st).Value = condition.StartTime;
                sqlCtx.Param(et).Value = condition.EndTime;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<PcbEntityInfo>();
                        while (sqlR.Read())
                        {
                            PcbEntityInfo item = new PcbEntityInfo();
                            item.pcbno = GetValue_Str(sqlR, 9);
                            item.smtmo = GetValue_Str(sqlR, 11);
                            item.custsn = GetValue_Str(sqlR, 1);
                            item.pcbmodelid = GetValue_Str(sqlR, 8);
                            item.dateCode = GetValue_Str(sqlR, 4);
                            item.mac = GetValue_Str(sqlR, 7);
                            item.uuid = GetValue_Str(sqlR, 14);
                            item.ecr = GetValue_Str(sqlR, 5);
                            item.iecver = GetValue_Str(sqlR, 6);
                            item.custver = GetValue_Str(sqlR, 2);
                            item.cvsn = GetValue_Str(sqlR, 3);
                            item.udt = GetValue_DateTime(sqlR, 13);
                            item.cdt = GetValue_DateTime(sqlR, 0);
                            item.shipMode = GetValue_Str(sqlR, 10);
                            item.state = GetValue_Str(sqlR, 12);
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

        public IList<fomb.MBInfo> GetPcbInfoByCondition(IMES.FisObject.PCA.MB.MBInfo condition)
        {
            try
            {
                IList<fomb.MBInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::Pcbinfo cond = mtns::FuncNew.SetColumnFromField<mtns::Pcbinfo, fomb.MBInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Pcbinfo>(null, null, new mtns::ConditionCollection<mtns::Pcbinfo>(new mtns::EqualCondition<mtns::Pcbinfo>(cond)), mtns::Pcbinfo.fn_pcbno);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Pcbinfo, fomb.MBInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Pcbinfo, fomb.MBInfo, fomb.MBInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPcbNoFromPcbRepairAndPcbRepairDefectInfo(string pcbNo, string remarkLike)
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
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = "SELECT {2} FROM {0} " +
                                            "WHERE {2}=@{2} " +
                                            "AND {3}<>'10' " +
                                            "AND {3}<>'19' " +
                                            "AND {4}=1 " +
                                            "UNION " +
                                            "SELECT {2} FROM {0} a, {1} b " +
                                            "WHERE a.{2}=@{2} " +
                                            "AND a.{3}='10' " +
                                            "AND a.{4}=1 " +
                                            "AND a.{5}=b.{6} " +
                                            "AND UPPER(b.{7}) LIKE @{7}";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(Pcbrepair)),
                                                                        ToolsNew.GetTableName(typeof(Pcbrepair_Defectinfo)),
                                                                        Pcbrepair.fn_pcbno,
                                                                        Pcbrepair.fn_station,
                                                                        Pcbrepair.fn_status,
                                                                        Pcbrepair.fn_id,
                                                                        Pcbrepair_Defectinfo.fn_pcarepairid,
                                                                        Pcbrepair_Defectinfo.fn_remark
                                                                        );
                        sqlCtx.AddParam(mtns.Pcbrepair.fn_pcbno, new SqlParameter("@" + mtns.Pcbrepair.fn_pcbno, ToolsNew.GetDBFieldType<mtns.Pcbrepair>(mtns.Pcbrepair.fn_pcbno)));
                        sqlCtx.AddParam(mtns.Pcbrepair_Defectinfo.fn_remark, new SqlParameter("@" + mtns.Pcbrepair_Defectinfo.fn_remark, ToolsNew.GetDBFieldType<mtns.Pcbrepair_Defectinfo>(mtns.Pcbrepair_Defectinfo.fn_remark)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param(_Metas.Pcbrepair.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(_Metas.Pcbrepair_Defectinfo.fn_remark).Value = "%" + remarkLike + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, 0);
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

        #endregion

        #region . Inners .

        private IList<string> FilterMBNOList_Inner(IList<string> mbNOList, string stationId)
        {
            try
            {
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
                        tf1.Table = typeof(PCB);
                        PCB insetCond = new PCB();
                        insetCond.PCBNo = "INSET";
                        tf1.inSetcond = insetCond;
                        tf1.ToGetFieldNames.Add(PCB.fn_PCBNo);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(PCBStatus);
                        PCBStatus neqCond = new PCBStatus();
                        neqCond.StationID = stationId;
                        tf2.notEqualcond = neqCond;
                        tf2.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, PCB.fn_PCBNo, tf2, PCBStatus.fn_PCBID);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                sqlCtx.Params[Func.DecAlias(tf2.alias, PCBStatus.fn_StationID)].Value = stationId;
                string Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf1.alias, Func.DecInSet(PCB.fn_PCBNo)), Func.ConvertInSet(mbNOList));

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string item = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, PCB.fn_PCBNo)]);
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

        private void PersistInsertMB(IMB mb)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCB.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCB.fn_CUSTVER].Value = mb.CUSTVER;
                sqlCtx.Params[PCB.fn_CVSN].Value = mb.CVSN;
                sqlCtx.Params[PCB.fn_DateCode].Value = mb.DateCode;
                sqlCtx.Params[PCB.fn_ECR].Value = mb.ECR;
                sqlCtx.Params[PCB.fn_IECVER].Value = mb.IECVER;
                sqlCtx.Params[PCB.fn_MAC].Value = mb.MAC;
                sqlCtx.Params[PCB.fn_SMTMOID].Value = mb.SMTMO;
                sqlCtx.Params[PCB.fn_PCBModelID].Value = mb.Model;
                sqlCtx.Params[PCB.fn_PCBNo].Value = mb.Sn;
                sqlCtx.Params[PCB.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCB.fn_UUID].Value = mb.UUID;
                sqlCtx.Params[PCB.fn_CUSTSN].Value = mb.CustSn;
                addMBSQLParameter(mb, sqlCtx);
                //sqlCtx.Params[Pcb.fn_state].Value = string.IsNullOrEmpty(mb.State) ? "" : mb.State;
                //sqlCtx.Params[Pcb.fn_shipMode].Value = string.IsNullOrEmpty(mb.ShipMode)?"":mb.ShipMode;

                //sqlCtx.Params[Pcb.fn_cartonWeight].Value = mb.CartonWeight;
                //sqlCtx.Params[Pcb.fn_unitWeight].Value = mb.UnitWeight;
                //sqlCtx.Params[Pcb.fn_cartonSN].Value = mb.CartonSN;
                //sqlCtx.Params[Pcb.fn_deliveryNo].Value = mb.DeliveryNo;
                //sqlCtx.Params[Pcb.fn_palletNo].Value = mb.PalletNo;
                //sqlCtx.Params[Pcb.fn_qcStatus].Value = mb.QCStatus;
                //sqlCtx.Params[Pcb.fn_pizzaID].Value = mb.PizzaID;        
               
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMB(IMB mb)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCB.fn_CUSTVER].Value = mb.CUSTVER;
                sqlCtx.Params[PCB.fn_CVSN].Value = mb.CVSN;
                sqlCtx.Params[PCB.fn_DateCode].Value = mb.DateCode;
                sqlCtx.Params[PCB.fn_ECR].Value = mb.ECR;
                sqlCtx.Params[PCB.fn_IECVER].Value = mb.IECVER;
                sqlCtx.Params[PCB.fn_MAC].Value = mb.MAC;
                sqlCtx.Params[PCB.fn_SMTMOID].Value = mb.SMTMO;
                sqlCtx.Params[PCB.fn_PCBModelID].Value = mb.Model;
                sqlCtx.Params[PCB.fn_PCBNo].Value = mb.Sn;
                sqlCtx.Params[PCB.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCB.fn_UUID].Value = mb.UUID;
                sqlCtx.Params[PCB.fn_CUSTSN].Value = mb.CustSn;

                addMBSQLParameter(mb, sqlCtx);
                //sqlCtx.Params[Pcb.fn_state].Value = string.IsNullOrEmpty(mb.State) ? "" : mb.State;
                //sqlCtx.Params[Pcb.fn_shipMode].Value = string.IsNullOrEmpty(mb.ShipMode) ? "" : mb.ShipMode;

                //sqlCtx.Params[Pcb.fn_cartonWeight].Value = mb.CartonWeight;
                //sqlCtx.Params[Pcb.fn_unitWeight].Value = mb.UnitWeight;
                //sqlCtx.Params[Pcb.fn_cartonSN].Value = mb.CartonSN;
                //sqlCtx.Params[Pcb.fn_deliveryNo].Value = mb.DeliveryNo;
                //sqlCtx.Params[Pcb.fn_palletNo].Value = mb.PalletNo;
                //sqlCtx.Params[Pcb.fn_qcStatus].Value = mb.QCStatus;
                //sqlCtx.Params[Pcb.fn_pizzaID].Value = mb.PizzaID; 
       
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMB(IMB mb)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB));
                    }
                }
                sqlCtx.Params[PCB.fn_PCBNo].Value = mb.Sn;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertMB_ForReplace(IMB mb)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCB.fn_Cdt].Value = mb.Cdt;
                sqlCtx.Params[PCB.fn_CUSTVER].Value = mb.CUSTVER;
                sqlCtx.Params[PCB.fn_CVSN].Value = mb.CVSN;
                sqlCtx.Params[PCB.fn_DateCode].Value = mb.DateCode;
                sqlCtx.Params[PCB.fn_ECR].Value = mb.ECR;
                sqlCtx.Params[PCB.fn_IECVER].Value = mb.IECVER;
                sqlCtx.Params[PCB.fn_MAC].Value = mb.MAC;
                sqlCtx.Params[PCB.fn_SMTMOID].Value = mb.SMTMO;
                sqlCtx.Params[PCB.fn_PCBModelID].Value = mb.Model;
                sqlCtx.Params[PCB.fn_PCBNo].Value = mb.Sn;
                sqlCtx.Params[PCB.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCB.fn_UUID].Value = mb.UUID;
                sqlCtx.Params[PCB.fn_CUSTSN].Value = mb.CustSn;
                addMBSQLParameter(mb, sqlCtx);

                //sqlCtx.Params[Pcb.fn_state].Value = string.IsNullOrEmpty(mb.State) ? "" : mb.State;
                //sqlCtx.Params[Pcb.fn_shipMode].Value = string.IsNullOrEmpty(mb.ShipMode) ? "" : mb.ShipMode;

                //sqlCtx.Params[Pcb.fn_cartonWeight].Value = mb.CartonWeight;
                //sqlCtx.Params[Pcb.fn_unitWeight].Value = mb.UnitWeight;
                //sqlCtx.Params[Pcb.fn_cartonSN].Value = mb.CartonSN;
                //sqlCtx.Params[Pcb.fn_deliveryNo].Value = mb.DeliveryNo;
                //sqlCtx.Params[Pcb.fn_palletNo].Value = mb.PalletNo;
                //sqlCtx.Params[Pcb.fn_qcStatus].Value = mb.QCStatus;
                //sqlCtx.Params[Pcb.fn_pizzaID].Value = mb.PizzaID; 
       
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMBStatus(MBStatus mbStatus)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus));
                    }
                }
                sqlCtx.Params[PCBStatus.fn_PCBID].Value = mbStatus.Pcbid;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMBLogByPCBID(string pcbid)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBLog cond = new PCBLog();
                        cond.PCBID = pcbid;
                        sqlCtx = Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBLog), cond, null, null);
                    }
                }
                sqlCtx.Params[PCBLog.fn_PCBID].Value = pcbid;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMB(string startSn, string endSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB betweenCond = new PCB();
                        betweenCond.PCBNo = string.Format("{0} AND {1}", startSn, endSn);
                        sqlCtx = Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), null, betweenCond, null);
                    }
                }
                sqlCtx.Params[Func.DecBeg(PCB.fn_PCBNo)].Value = startSn;
                sqlCtx.Params[Func.DecEnd(PCB.fn_PCBNo)].Value = endSn;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMBStatus(string startSn, string endSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBStatus betweenCond = new PCBStatus();
                        betweenCond.PCBID = string.Format("{0} AND {1}", startSn, endSn);
                        sqlCtx = Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus), null, betweenCond, null);
                    }
                }
                sqlCtx.Params[Func.DecBeg(PCBStatus.fn_PCBID)].Value = startSn;
                sqlCtx.Params[Func.DecEnd(PCBStatus.fn_PCBID)].Value = endSn;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMB_Batch(IList<string> items)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCB insetCond = new PCB();
                        insetCond.PCBNo = "INSET";
                        sqlCtx = Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), null, null, insetCond);
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(Func.DecInSet(PCB.fn_PCBNo), Func.ConvertInSet(items));
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        //private void PersistDeleteMBStatus_Batch(IList<string> items)
        //{
        //    try
        //    {
        //        SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                PCBLog insetCond = new PCBLog();
        //                insetCond.PCBID = "INSET";
        //                sqlCtx = Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBLog), null, null, insetCond);
        //            }
        //        }
        //        string Sentence = sqlCtx.Sentence.Replace(Func.DecInSet(PCBLog.fn_PCBID), Func.ConvertInSet(items));
        //        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private void PersistDeleteMBLogByPCBID_Batch(IList<string> items)
        //{
        //    try
        //    {
        //        SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                PCBStatus insetCond = new PCBStatus();
        //                insetCond.PCBID = "INSET";
        //                sqlCtx = Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus), null, null, insetCond);
        //            }
        //        }
        //        string Sentence = sqlCtx.Sentence.Replace(Func.DecInSet(PCBStatus.fn_PCBID), Func.ConvertInSet(items));
        //        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #region . inserts .

        private void CheckAndInsertSubs(IMB item, StateTracker tracker)
        {
            //persist MBRepair
            IList<Repair> lstRep = (IList<Repair>)item.GetType().GetField("_repair", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstRep != null && lstRep.Count > 0)//(item.Repairs.Count > 0)
            {
                foreach (Repair repi in lstRep)//item.Repairs)
                {
                    if (tracker.GetState(repi) == DataRowState.Added)
                    {
                        repi.Sn = item.Sn;
                        this.PersistInsertRepair(repi);

                        IList<RepairDefect> lstDfct = (IList<RepairDefect>)repi.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(repi);
                        if (lstDfct != null && lstDfct.Count > 0)//(repi.Defects != null && repi.Defects.Count > 0)
                        {
                            foreach (RepairDefect repDfcti in lstDfct)//repi.Defects)
                            {
                                repDfcti.RepairID = repi.ID;
                                if (tracker.GetState(repDfcti) == DataRowState.Added)
                                {
                                    this.PersistInsertRepairDefect(repDfcti);

                                    if (repDfcti.MTAMark != null)
                                    {
                                        repDfcti.MTAMark.rep_Id = repDfcti.ID;//2012-02-02  itc207031     Modify ITC-1360-0213
                                        this.InsertMtaMarkInfo(repDfcti.MTAMark);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //persist MBInfo
            IList<IMES.FisObject.PCA.MB.MBInfo> lstMbInfo = (IList<IMES.FisObject.PCA.MB.MBInfo>)item.GetType().GetField("_info", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMbInfo != null && lstMbInfo.Count > 0)//(item.MBInfos.Count > 0)
            {
                foreach (IMES.FisObject.PCA.MB.MBInfo mbii in lstMbInfo)//item.MBInfos)
                {
                    if (tracker.GetState(mbii) == DataRowState.Added)
                    {
                        mbii.PCBID = item.Sn;
                        this.PersistInsertMBInfo(mbii);
                    }
                }
            }

            //persist MBPart
            IList<IProductPart> lstMbPart = (IList<IProductPart>)item.GetType().GetField("_part", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMbPart != null && lstMbPart.Count > 0)//(item.MBParts.Count > 0)
            {
                foreach (ProductPart mbprt in lstMbPart)//item.MBParts)
                {
                    if (tracker.GetState(mbprt) == DataRowState.Added)
                    {
                        mbprt.ProductID = item.Sn;
                        this.PersistInsertMBPart(mbprt);
                    }
                }
            }

            //persist MBLog
            IList<MBLog> lstMbLg = (IList<MBLog>)item.GetType().GetField("_logs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMbLg != null && lstMbLg.Count > 0)//(item.MBLogs.Count > 0)
            {
                foreach (MBLog mbLog in lstMbLg)//item.MBLogs)
                {
                    if (tracker.GetState(mbLog) == DataRowState.Added)
                    {
                        mbLog.PCBID = item.Sn;
                        this.PersistInsertMBLog(mbLog);
                    }
                }
            }

            //persist MBTestLog
            IList<TestLog> lstTstLg = (IList<TestLog>)item.GetType().GetField("_testLogs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstTstLg != null && lstTstLg.Count > 0)//(item.TestLogs.Count > 0)
            {
                foreach (TestLog testLog in lstTstLg)//item.TestLogs)
                {
                    if (tracker.GetState(testLog) == DataRowState.Added)
                    {
                        testLog.Sn = item.Sn;
                        this.PersistInsertTestLog(testLog);
                        //this.PersistInsertTestLogBack(testLog);

                        IList<TestLogDefect> lstTstLgDfct = (IList<TestLogDefect>)testLog.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(testLog);
                        if (lstTstLgDfct != null && lstTstLgDfct.Count > 0)//(testLog.Defects != null && testLog.Defects.Count > 0)
                        {
                            foreach (TestLogDefect testLogDfcti in lstTstLgDfct)//testLog.Defects)
                            {
                                testLogDfcti.TestLogID = testLog.ID;
                                if (tracker.GetState(testLogDfcti) == DataRowState.Added)
                                {
                                    this.PersistInsertTestLogDefect(testLogDfcti);
                                   // this.PersistInsertTestLogDefectBack(testLogDfcti);
                                }
                            }
                        }
                    }
                }
            }

            IList<ProductAttribute> lstPrdAttr = (IList<ProductAttribute>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPrdAttr != null && lstPrdAttr.Count > 0)
            {
                foreach (ProductAttribute pcbAttr in lstPrdAttr)
                {
                    if (tracker.GetState(pcbAttr) == DataRowState.Added)
                    {
                        pcbAttr.ProductId = item.Sn;
                        this.PersistInsertPCBAttribute(pcbAttr);
                    }
                }
            }

            IList<ProductAttributeLog> lstPrdAttrLg = (IList<ProductAttributeLog>)item.GetType().GetField("_attributeLogs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPrdAttrLg != null && lstPrdAttrLg.Count > 0)
            {
                foreach (ProductAttributeLog pcbAttrLg in lstPrdAttrLg)
                {
                    if (tracker.GetState(pcbAttrLg) == DataRowState.Added)
                    {
                        pcbAttrLg.ProductId = item.Sn;
                        this.PersistInsertPCBAttributeLog(pcbAttrLg);
                    }
                }
            }

            //persist MBRptRepair
            IList<MBRptRepair> lstRptRep = (IList<MBRptRepair>)item.GetType().GetField("_rptRepair", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstRptRep != null && lstRptRep.Count > 0)
            {
                foreach (MBRptRepair mbrptrep in lstRptRep)
                {
                    if (tracker.GetState(mbrptrep) == DataRowState.Added)
                    {
                        mbrptrep.MBSn = item.Sn;
                        this.PersistInsertMBRptRepair(mbrptrep);
                    }
                }
            }
        }

        private void PersistInsertMBStatus(string pcbNo, MBStatus status)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBStatus.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCBStatus.fn_PCBID].Value = pcbNo;
                sqlCtx.Params[PCBStatus.fn_Editor].Value = status.Editor;
                sqlCtx.Params[PCBStatus.fn_LineID].Value = status.Line;
                sqlCtx.Params[PCBStatus.fn_StationID].Value = status.Station;
                sqlCtx.Params[PCBStatus.fn_Status].Value = status.Status;
                sqlCtx.Params[PCBStatus.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCBStatus.fn_TestFailCount].Value = status.TestFailCount;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertRepair(Repair rep)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBRepair));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBRepair.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCBRepair.fn_Editor].Value = rep.Editor;
                //sqlCtx.Params[PCBRepair.fn_ID].Value = rep.ID;
                sqlCtx.Params[PCBRepair.fn_LineID].Value = rep.LineID;
                sqlCtx.Params[PCBRepair.fn_PCBModelID].Value = rep.Model;
                sqlCtx.Params[PCBRepair.fn_PCBID].Value = rep.Sn;
                sqlCtx.Params[PCBRepair.fn_StationID].Value = rep.Station;
                sqlCtx.Params[PCBRepair.fn_Status].Value = Convert.ToInt32(rep.Status);
                //sqlCtx.Params[PCBRepair.fn_ReturnID].Value = rep.ReturnID;
                sqlCtx.Params[PCBRepair.fn_Type].Value = rep.Type;
                sqlCtx.Params[PCBRepair.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCBRepair.fn_TestLogID].Value = ToInt32WithNull(rep.TestLogID);
                sqlCtx.Params[PCBRepair.fn_logID].Value = rep.LogId;
                rep.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertRepairDefect(RepairDefect repDefect)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBRepair_DefectInfo));

                        //sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_].ParameterName = Func.ClearRectBrace(sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_].ParameterName);
                        //sqlCtx.Params.Add(Func.ClearRectBrace(PCBRepair_DefectInfo.fn__4M_), sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_]);
                        //sqlCtx.Params.Remove(PCBRepair_DefectInfo.fn__4M_);
                        //sqlCtx.Sentence = sqlCtx.Sentence.Replace("@" + PCBRepair_DefectInfo.fn__4M_, "@" + Func.ClearRectBrace(PCBRepair_DefectInfo.fn__4M_));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_].Value = repDefect._4M;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Action].Value = repDefect.Action;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Cause].Value = repDefect.Cause;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Component].Value = repDefect.Component;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Cover].Value = repDefect.Cover;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_DefectCodeID].Value = repDefect.DefectCodeID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Distribution].Value = repDefect.Distribution;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Editor].Value = repDefect.Editor;
                //sqlCtx.Params[PCBRepair_DefectInfo.fn_ID].Value = repDefect.ID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_IsManual].Value = Convert.ToInt32(repDefect.IsManual);
                sqlCtx.Params[PCBRepair_DefectInfo.fn_MajorPart].Value = repDefect.MajorPart;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Manufacture].Value = repDefect.Manufacture;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Side].Value = repDefect.Side;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Mark].Value = repDefect.Mark;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_NewPart].Value = repDefect.NewPart;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_NewPartSno].Value = repDefect.NewPartSno;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_NewPartDateCode].Value = repDefect.NewPartDateCode;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Obligation].Value = repDefect.Obligation;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_OldPart].Value = repDefect.OldPart;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_OldPartSno].Value = repDefect.OldPartSno;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_PartType].Value = repDefect.PartType;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_PIAStation].Value = repDefect.PIAStation;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Remark].Value = repDefect.Remark;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_PCARepairID].Value = repDefect.RepairID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Responsibility].Value = repDefect.Responsibility;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_ReturnSign].Value = repDefect.ReturnSign;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Site].Value = repDefect.Site;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_SubDefect].Value = repDefect.SubDefect;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_TrackingStatus].Value = repDefect.TrackingStatus;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Type].Value = repDefect.Type;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Uncover].Value = repDefect.Uncover;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_VendorCT].Value = repDefect.VendorCT;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_VersionA].Value = repDefect.VersionA;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_VersionB].Value = repDefect.VersionB;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_MTAID].Value = repDefect.MTAID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Location].Value = repDefect.Location;
                repDefect.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertMBInfo(IMES.FisObject.PCA.MB.MBInfo mbii)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBInfo));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCBInfo.fn_Editor].Value = mbii.Editor;
                //sqlCtx.Params[PCBInfo.fn_ID].Value = mbii.ID;
                sqlCtx.Params[PCBInfo.fn_InfoType].Value = mbii.InfoType;
                sqlCtx.Params[PCBInfo.fn_InfoValue].Value = mbii.InfoValue;
                sqlCtx.Params[PCBInfo.fn_PCBID].Value = mbii.PCBID;
                sqlCtx.Params[PCBInfo.fn_Udt].Value = cmDt;
                mbii.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertMBPart(ProductPart mbprt)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB_Part));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCB_Part.fn_cdt].Value = cmDt;
                sqlCtx.Params[PCB_Part.fn_editor].Value = mbprt.Editor;
                //sqlCtx.Params[PCB_Part.fn_id].Value = mbprt.ID;
                sqlCtx.Params[PCB_Part.fn_partNo].Value = mbprt.PartID;
                sqlCtx.Params[PCB_Part.fn_pcbno].Value = mbprt.ProductID;
                sqlCtx.Params[PCB_Part.fn_udt].Value = cmDt;
                sqlCtx.Params[PCB_Part.fn_partSn].Value = mbprt.PartSn;
                sqlCtx.Params[PCB_Part.fn_station].Value = mbprt.Station;
                sqlCtx.Params[PCB_Part.fn_iecpn].Value = mbprt.Iecpn;
                sqlCtx.Params[PCB_Part.fn_custmerPn].Value = mbprt.CustomerPn;
                sqlCtx.Params[PCB_Part.fn_partType].Value = mbprt.PartType;
                sqlCtx.Params[PCB_Part.fn_bomNodeType].Value = mbprt.BomNodeType;
                sqlCtx.Params[PCB_Part.fn_checkItemType].Value = mbprt.CheckItemType;

                mbprt.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertMBLog(MBLog mblog)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBLog));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCBLog.fn_Editor].Value = mblog.Editor;
                //sqlCtx.Params[PCBLog.fn_ID].Value = mblog.ID;
                sqlCtx.Params[PCBLog.fn_LineID].Value = mblog.LineID;
                sqlCtx.Params[PCBLog.fn_PCBModelID].Value = mblog.Model;
                sqlCtx.Params[PCBLog.fn_PCBID].Value = mblog.PCBID;
                sqlCtx.Params[PCBLog.fn_StationID].Value = mblog.StationID;
                sqlCtx.Params[PCBLog.fn_Status].Value = mblog.Status;
                mblog.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertTestLog(TestLog testLog)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBTestLog));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBTestLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCBTestLog.fn_Editor].Value = testLog.Editor;
                sqlCtx.Params[PCBTestLog.fn_FixtureID].Value = testLog.FixtureId;
                //sqlCtx.Params[PCBTestLog.fn_ID].Value = testLog.ID;
                sqlCtx.Params[PCBTestLog.fn_LineID].Value = testLog.Line;
                sqlCtx.Params[PCBTestLog.fn_PCBID].Value = testLog.Sn;
                sqlCtx.Params[PCBTestLog.fn_StationID].Value = testLog.Station;
                sqlCtx.Params[PCBTestLog.fn_Status].Value = Convert.ToInt32(testLog.Status);
                sqlCtx.Params[PCBTestLog.fn_Type].Value = testLog.Type;
                sqlCtx.Params[PCBTestLog.fn_JoinID].Value = testLog.JoinID;
                sqlCtx.Params[PCBTestLog.fn_ActionName].Value = testLog.ActionName;
                sqlCtx.Params[PCBTestLog.fn_ErrorCode].Value = testLog.ErrorCode;
                sqlCtx.Params[PCBTestLog.fn_Descr].Value = testLog.Descr;
                sqlCtx.Params[PCBTestLog.fn_remark].Value = testLog.Remark;
                testLog.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertTestLogBack(TestLog testLog)
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
                        sqlCtx = FuncNew.GetCommonInsert<_Metas.Pcbtestlogback>(tk);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_id).Value = testLog.ID;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_line).Value = testLog.Line;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_pcbno).Value = testLog.Sn;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_station).Value = testLog.Station;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_status).Value = Convert.ToInt32(testLog.Status);
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_type).Value = testLog.Type;

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_cdt).Value = cmDt;
                //sqlCtx.Param(_Metas.Pcbtestlogback.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertTestLogDefect(TestLogDefect testLogDfcti)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBTestLog_DefectInfo));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_Cdt].Value = cmDt;
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_DefectCodeID].Value = testLogDfcti.DefectCode;
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_Editor].Value = testLogDfcti.Editor;
                //sqlCtx.Params[PCBTestLog_DefectInfo.fn_ID].Value = testLogDfcti.ID;
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_PCBTestLogID].Value = testLogDfcti.TestLogID;
                testLogDfcti.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertTestLogDefectBack(TestLogDefect testLogDfcti)
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
                        sqlCtx = FuncNew.GetCommonInsert<_Metas.Pcbtestlogback_Defectinfo>(tk);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_defectCodeID).Value = testLogDfcti.DefectCode;
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_id).Value = testLogDfcti.ID;
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_pcbtestlogbackid).Value = testLogDfcti.TestLogID;
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_triggerAlarm).Value = false;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_cdt).Value = cmDt;
                //sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertPCBAttribute(ProductAttribute item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttr));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBAttr.fn_AttrName].Value = item.AttributeName;
                sqlCtx.Params[_Schema.PCBAttr.fn_AttrValue].Value = item.AttributeValue;
                sqlCtx.Params[_Schema.PCBAttr.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PCBAttr.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PCBAttr.fn_PCBNo].Value = item.ProductId;
                sqlCtx.Params[_Schema.PCBAttr.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertPCBAttributeLog(ProductAttributeLog item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttrLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBAttrLog.fn_AttrName].Value = item.AttributeName;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_AttrNewValue].Value = item.NewValue;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_AttrOldValue].Value = item.OldValue;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_Descr].Value = item.Descr;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_PCBModelID].Value = item.Model;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_PCBNo].Value = item.ProductId;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_Station].Value = item.Station;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertMBRptRepair(MBRptRepair item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<mtns::Rpt_PCARep>(tk);
                    }
                }
                sqlCtx.Param(mtns::Rpt_PCARep.fn_mark).Value = item.Mark;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_remark).Value = item.Remark;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_snoId).Value = item.MBSn;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_status).Value = item.Status;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_tp).Value = item.Tp;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_username).Value = item.UserName;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Rpt_PCARep.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Rpt_PCARep.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region . updates .

        private void CheckAndUpdateSubs(IMB item, StateTracker tracker)
        {
            IList<Repair> lstRep = (IList<Repair>)item.GetType().GetField("_repair", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstRep != null && lstRep.Count > 0)//(item.Repairs.Count > 0)
            {
                IList<Repair> iLstToDelRep = new List<Repair>();
                foreach (Repair repi in lstRep)//item.Repairs)
                {
                    if (tracker.GetState(repi) == DataRowState.Modified)
                    {
                        this.PersistUpdateRepair(repi);

                        IList<RepairDefect> lstDfct = (IList<RepairDefect>)repi.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(repi);
                        if (lstDfct != null && lstDfct.Count > 0)//(repi.Defects != null && repi.Defects.Count > 0)
                        {
                            IList<RepairDefect> iLstToDel = new List<RepairDefect>();
                            foreach (RepairDefect repDfcti in lstDfct)//repi.Defects)
                            {
                                if (tracker.GetState(repDfcti) == DataRowState.Modified)
                                {
                                    this.PersistUpdateRepairDefect(repDfcti);
                                }
                                else if (tracker.GetState(repDfcti) == DataRowState.Added)
                                {
                                    repDfcti.RepairID = repi.ID;
                                    this.PersistInsertRepairDefect(repDfcti);

                                    if (repDfcti.MTAMark != null)
                                    {
                                        repDfcti.MTAMark.rep_Id = repDfcti.ID;//2012-02-02  itc207031     Modify ITC-1360-0213
                                        this.InsertMtaMarkInfo(repDfcti.MTAMark);
                                    }
                                }
                                else if (tracker.GetState(repDfcti) == DataRowState.Deleted)
                                {
                                    this.PersistDeleteRepairDefect(repDfcti);
                                    iLstToDel.Add(repDfcti);
                                }
                            }
                            foreach (RepairDefect rdfct in iLstToDel)
                            {
                                lstDfct.Remove(rdfct);
                            }
                        }
                    }
                    else if (tracker.GetState(repi) == DataRowState.Deleted)
                    {
                        IList<RepairDefect> lstDfct = (IList<RepairDefect>)repi.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(repi);
                        if (lstDfct != null && lstDfct.Count > 0)
                        {
                            IList<RepairDefect> iLstToDel = new List<RepairDefect>();
                            foreach (RepairDefect repDfcti in lstDfct)
                            {
                                this.PersistDeleteRepairDefect(repDfcti);
                                iLstToDel.Add(repDfcti);
                            }
                            foreach (RepairDefect rdfct in iLstToDel)
                            {
                                lstDfct.Remove(rdfct);
                            }
                        }
                        this.PersistDeleteMBRepair(repi);
                        iLstToDelRep.Add(repi);
                    }
                }
                foreach (Repair rp in iLstToDelRep)
                {
                    lstRep.Remove(rp);
                }
            }

            //persist MBInfo
            IList<IMES.FisObject.PCA.MB.MBInfo> lstMbInfo = (IList<IMES.FisObject.PCA.MB.MBInfo>)item.GetType().GetField("_info", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMbInfo != null && lstMbInfo.Count > 0)//(item.MBInfos.Count > 0)
            {
                foreach (IMES.FisObject.PCA.MB.MBInfo mbii in lstMbInfo)//item.MBInfos)
                {
                    if (tracker.GetState(mbii) == DataRowState.Modified)
                    {
                        this.PersistUpdateMBInfo(mbii);
                    }
                }
            }

            //persist MBPart
            IList<IProductPart> lstMbPart = (IList<IProductPart>)item.GetType().GetField("_part", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMbPart != null && lstMbPart.Count > 0)//(item.MBParts.Count > 0)
            {
                IList<IProductPart> iLstToDel = new List<IProductPart>();
                foreach (ProductPart mbprt in lstMbPart)//item.MBParts)
                {
                    if (tracker.GetState(mbprt) == DataRowState.Modified)
                    {
                        this.PersistUpdateMBPart(mbprt);
                    }
                    else if (tracker.GetState(mbprt) == DataRowState.Deleted)
                    {
                        this.PersistDeleteMBPart(mbprt);
                        iLstToDel.Add(mbprt);
                    }
                }
                foreach (IProductPart prdprt in iLstToDel)
                {
                    lstMbPart.Remove(prdprt);
                }
            }

            //persist MBLog
            IList<MBLog> lstMbLg = (IList<MBLog>)item.GetType().GetField("_logs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstMbLg != null && lstMbLg.Count > 0)//(item.MBLogs.Count > 0)
            {
                foreach (MBLog mbLog in lstMbLg)//item.MBLogs)
                {
                    if (tracker.GetState(mbLog) == DataRowState.Modified)
                    {
                        this.PersistUpdateMBLog(mbLog);
                    }
                }
            }

            //persist MBTestLog
            IList<TestLog> lstTstLg = (IList<TestLog>)item.GetType().GetField("_testLogs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstTstLg != null && lstTstLg.Count > 0)//(item.TestLogs.Count > 0)
            {
                foreach (TestLog testLog in lstTstLg)//item.TestLogs)
                {
                    if (tracker.GetState(testLog) == DataRowState.Modified)
                    {
                        this.PersistUpdateTestLog(testLog);
                        //this.PersistUpdateTestLogBack(testLog);

                        IList<TestLogDefect> lstTstLgDfct = (IList<TestLogDefect>)testLog.GetType().GetField("_defects", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(testLog);
                        if (lstTstLgDfct != null && lstTstLgDfct.Count > 0)//(testLog.Defects != null && testLog.Defects.Count > 0)
                        {
                            IList<TestLogDefect> iLstToDel = new List<TestLogDefect>();
                            foreach (TestLogDefect testLogDfcti in lstTstLgDfct)//testLog.Defects)
                            {
                                if (tracker.GetState(testLogDfcti) == DataRowState.Modified)
                                {
                                    this.PersistUpdateTestLogDefect(testLogDfcti);
                                    //this.PersistUpdateTestLogDefectBack(testLogDfcti);
                                }
                                else if (tracker.GetState(testLogDfcti) == DataRowState.Added)
                                {
                                    testLogDfcti.TestLogID = testLog.ID;
                                    this.PersistInsertTestLogDefect(testLogDfcti);
                                    //this.PersistInsertTestLogDefectBack(testLogDfcti);
                                }
                                else if (tracker.GetState(testLogDfcti) == DataRowState.Deleted)
                                {
                                    this.PersistDeleteTestLogDefect(testLogDfcti);
                                    //this.PersistDeleteTestLogDefectBack(testLogDfcti);
                                    iLstToDel.Add(testLogDfcti);
                                }
                            }
                            foreach (TestLogDefect tld in iLstToDel)
                            {
                                lstTstLgDfct.Remove(tld);
                            }
                        }
                    }
                }
            }

            //persist PCBAttr
            IList<IMES.FisObject.FA.Product.ProductAttribute> lstPCBAttr = (IList<IMES.FisObject.FA.Product.ProductAttribute>)item.GetType().GetField("_attributes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPCBAttr != null && lstPCBAttr.Count > 0)
            {
                foreach (IMES.FisObject.FA.Product.ProductAttribute pcba in lstPCBAttr)
                {
                    if (tracker.GetState(pcba) == DataRowState.Modified)
                    {
                        this.PersistUpdatePCBAttribute(pcba);
                    }
                }
            }

            //persist PCBAttrLog
            IList<IMES.FisObject.FA.Product.ProductAttributeLog> lstPCBAttrLg = (IList<IMES.FisObject.FA.Product.ProductAttributeLog>)item.GetType().GetField("_attributeLogs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
            if (lstPCBAttrLg != null && lstPCBAttrLg.Count > 0)
            {
                foreach (IMES.FisObject.FA.Product.ProductAttributeLog pcbal in lstPCBAttrLg)
                {
                    if (tracker.GetState(pcbal) == DataRowState.Modified)
                    {
                        this.PersistUpdatePCBAttributeLog(pcbal);
                    }
                }
            }
        }

        private void PersistUpdateMBStatus(string pcbNo, MBStatus status)
        {
            try
            {
                string line = status.Line;
                if (line != null)
                    line = line.Trim();

                if (string.IsNullOrEmpty(line))
                    PersistUpdateMBStatusWithoutLine(pcbNo, status);
                else
                    PersistUpdateMBStatusWithLine(pcbNo, status);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMBStatusWithLine(string pcbNo, MBStatus status)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBStatus.fn_Editor].Value = status.Editor;
                sqlCtx.Params[PCBStatus.fn_LineID].Value = status.Line;
                sqlCtx.Params[PCBStatus.fn_PCBID].Value = pcbNo;
                sqlCtx.Params[PCBStatus.fn_StationID].Value = status.Station;
                sqlCtx.Params[PCBStatus.fn_Status].Value = status.Status;
                sqlCtx.Params[PCBStatus.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCBStatus.fn_TestFailCount].Value = status.TestFailCount;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMBStatusWithoutLine(string pcbNo, MBStatus status)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        PCBStatus cond = new PCBStatus();
                        cond.PCBID = pcbNo;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus), null, new List<string>() { PCBStatus.fn_LineID, PCBStatus.fn_PCBID }, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[PCBStatus.fn_PCBID].Value = pcbNo;

                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(PCBStatus.fn_Editor)].Value = status.Editor;
                //sqlCtx.Params[_Schema.Func.DecSV(PCBStatus.fn_LineID)].Value = status.Line;
                sqlCtx.Params[_Schema.Func.DecSV(PCBStatus.fn_StationID)].Value = status.Station;
                sqlCtx.Params[_Schema.Func.DecSV(PCBStatus.fn_Status)].Value = status.Status;
                sqlCtx.Params[_Schema.Func.DecSV(PCBStatus.fn_Udt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(PCBStatus.fn_TestFailCount)].Value = status.TestFailCount;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateRepair(Repair rep)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBRepair));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBRepair.fn_Editor].Value = rep.Editor;
                sqlCtx.Params[PCBRepair.fn_ID].Value = rep.ID;
                sqlCtx.Params[PCBRepair.fn_LineID].Value = rep.LineID;
                sqlCtx.Params[PCBRepair.fn_PCBID].Value = rep.Sn;
                sqlCtx.Params[PCBRepair.fn_PCBModelID].Value = rep.Model;
                sqlCtx.Params[PCBRepair.fn_StationID].Value = rep.Station;
                sqlCtx.Params[PCBRepair.fn_Status].Value = Convert.ToInt32(rep.Status);
                //sqlCtx.Params[PCBRepair.fn_ReturnID].Value = rep.ReturnID;
                sqlCtx.Params[PCBRepair.fn_Type].Value = rep.Type;
                sqlCtx.Params[PCBRepair.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCBRepair.fn_TestLogID].Value = ToInt32WithNull(rep.TestLogID);
                sqlCtx.Params[PCBRepair.fn_logID].Value = rep.LogId;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateRepairDefect(RepairDefect repDefect)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBRepair_DefectInfo));

                        //sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_].ParameterName = Func.ClearRectBrace(sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_].ParameterName);
                        //sqlCtx.Params.Add(Func.ClearRectBrace(PCBRepair_DefectInfo.fn__4M_), sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_]);
                        //sqlCtx.Params.Remove(PCBRepair_DefectInfo.fn__4M_);
                        //sqlCtx.Sentence = sqlCtx.Sentence.Replace("@" + PCBRepair_DefectInfo.fn__4M_, "@" + Func.ClearRectBrace(PCBRepair_DefectInfo.fn__4M_));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBRepair_DefectInfo.fn__4M_].Value = repDefect._4M;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Action].Value = repDefect.Action;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Cause].Value = repDefect.Cause;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Component].Value = repDefect.Component;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Cover].Value = repDefect.Cover;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_DefectCodeID].Value = repDefect.DefectCodeID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Distribution].Value = repDefect.Distribution;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Editor].Value = repDefect.Editor;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_ID].Value = repDefect.ID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_IsManual].Value = Convert.ToInt32(repDefect.IsManual);
                sqlCtx.Params[PCBRepair_DefectInfo.fn_MajorPart].Value = repDefect.MajorPart;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Manufacture].Value = repDefect.Manufacture;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Side].Value = repDefect.Side;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Mark].Value = repDefect.Mark;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_NewPart].Value = repDefect.NewPart;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_NewPartSno].Value = repDefect.NewPartSno;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_NewPartDateCode].Value = repDefect.NewPartDateCode;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Obligation].Value = repDefect.Obligation;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_OldPart].Value = repDefect.OldPart;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_OldPartSno].Value = repDefect.OldPartSno;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_PartType].Value = repDefect.PartType;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_PCARepairID].Value = repDefect.RepairID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_PIAStation].Value = repDefect.PIAStation;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Remark].Value = repDefect.Remark;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Responsibility].Value = repDefect.Responsibility;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_ReturnSign].Value = repDefect.ReturnSign;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Site].Value = repDefect.Site;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_SubDefect].Value = repDefect.SubDefect;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_TrackingStatus].Value = repDefect.TrackingStatus;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Type].Value = repDefect.Type;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Udt].Value = cmDt;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Uncover].Value = repDefect.Uncover;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_VendorCT].Value = repDefect.VendorCT;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_VersionA].Value = repDefect.VersionA;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_VersionB].Value = repDefect.VersionB;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_MTAID].Value = repDefect.MTAID;
                sqlCtx.Params[PCBRepair_DefectInfo.fn_Location].Value = repDefect.Location;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteRepairDefect(RepairDefect repDefect)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBRepair_DefectInfo));
                    }
                }
                sqlCtx.Params[_Schema.PCBRepair_DefectInfo.fn_ID].Value = repDefect.ID;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void PersistUpdateMBInfo(IMES.FisObject.PCA.MB.MBInfo mbii)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBInfo));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCBInfo.fn_Editor].Value = mbii.Editor;
                sqlCtx.Params[PCBInfo.fn_ID].Value = mbii.ID;
                sqlCtx.Params[PCBInfo.fn_InfoType].Value = mbii.InfoType;
                sqlCtx.Params[PCBInfo.fn_InfoValue].Value = mbii.InfoValue;
                sqlCtx.Params[PCBInfo.fn_PCBID].Value = mbii.PCBID;
                sqlCtx.Params[PCBInfo.fn_Udt].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMBPart(ProductPart mbprt)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB_Part));
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[PCB_Part.fn_udt].Value = cmDt;
                sqlCtx.Params[PCB_Part.fn_editor].Value = mbprt.Editor;
                sqlCtx.Params[PCB_Part.fn_id].Value = mbprt.ID;
                sqlCtx.Params[PCB_Part.fn_partNo].Value = mbprt.PartID;
                sqlCtx.Params[PCB_Part.fn_pcbno].Value = mbprt.ProductID;
                sqlCtx.Params[PCB_Part.fn_udt].Value = cmDt;
                sqlCtx.Params[PCB_Part.fn_partSn].Value = mbprt.PartSn;
                sqlCtx.Params[PCB_Part.fn_station].Value = mbprt.Station;
                sqlCtx.Params[PCB_Part.fn_iecpn].Value = mbprt.Iecpn;
                sqlCtx.Params[PCB_Part.fn_custmerPn].Value = mbprt.CustomerPn;
                sqlCtx.Params[PCB_Part.fn_partType].Value = mbprt.PartType;
                sqlCtx.Params[PCB_Part.fn_bomNodeType].Value = mbprt.BomNodeType;
                sqlCtx.Params[PCB_Part.fn_checkItemType].Value = mbprt.CheckItemType;

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMBPart(ProductPart mbprt)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCB_Part));
                    }
                }
                sqlCtx.Params[_Schema.PCB_Part.fn_id].Value = mbprt.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateMBLog(MBLog mblog)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBLog));
                    }
                }
                sqlCtx.Params[PCBLog.fn_Editor].Value = mblog.Editor;
                sqlCtx.Params[PCBLog.fn_ID].Value = mblog.ID;
                sqlCtx.Params[PCBLog.fn_LineID].Value = mblog.LineID;
                sqlCtx.Params[PCBLog.fn_PCBID].Value = mblog.PCBID;
                sqlCtx.Params[PCBLog.fn_PCBModelID].Value = mblog.Model;
                sqlCtx.Params[PCBLog.fn_StationID].Value = mblog.StationID;
                sqlCtx.Params[PCBLog.fn_Status].Value = mblog.Status;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateTestLog(TestLog testLog)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBTestLog));
                    }
                }
                sqlCtx.Params[PCBTestLog.fn_Editor].Value = testLog.Editor;
                sqlCtx.Params[PCBTestLog.fn_FixtureID].Value = testLog.FixtureId;
                sqlCtx.Params[PCBTestLog.fn_ID].Value = testLog.ID;
                sqlCtx.Params[PCBTestLog.fn_LineID].Value = testLog.Line;
                sqlCtx.Params[PCBTestLog.fn_PCBID].Value = testLog.Sn;
                sqlCtx.Params[PCBTestLog.fn_StationID].Value = testLog.Station;
                sqlCtx.Params[PCBTestLog.fn_Status].Value = Convert.ToInt32(testLog.Status);
                sqlCtx.Params[PCBTestLog.fn_Type].Value = testLog.Type;
                sqlCtx.Params[PCBTestLog.fn_JoinID].Value = testLog.JoinID;
                sqlCtx.Params[PCBTestLog.fn_ActionName].Value = testLog.ActionName;
                sqlCtx.Params[PCBTestLog.fn_Descr].Value = testLog.Descr;
                sqlCtx.Params[PCBTestLog.fn_ErrorCode].Value = testLog.ErrorCode;
                sqlCtx.Params[PCBTestLog.fn_remark].Value = testLog.Remark;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateTestLogBack(TestLog testLog)
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
                        sqlCtx = FuncNew.GetCommonUpdate<_Metas.Pcbtestlogback>(tk);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_line).Value = testLog.Line;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_pcbno).Value = testLog.Sn;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_station).Value = testLog.Station;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_status).Value = Convert.ToInt32(testLog.Status);
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_type).Value = testLog.Type;
                sqlCtx.Param(_Metas.Pcbtestlogback.fn_id).Value = testLog.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateTestLogDefect(TestLogDefect testLogDfcti)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBTestLog_DefectInfo));
                    }
                }
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_DefectCodeID].Value = testLogDfcti.DefectCode;
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_Editor].Value = testLogDfcti.Editor;
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_ID].Value = testLogDfcti.ID;
                sqlCtx.Params[PCBTestLog_DefectInfo.fn_PCBTestLogID].Value = testLogDfcti.TestLogID;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void PersistUpdateTestLogDefectBack(TestLogDefect testLogDfcti)
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
                        Pcbtestlogback_Defectinfo setv = new Pcbtestlogback_Defectinfo();
                        setv.defectCodeID = "defectCodeID";
                        setv.pcbtestlogbackid = 88;
                        Pcbtestlogback_Defectinfo cond = new Pcbtestlogback_Defectinfo();
                        cond.id = 88;
                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.Pcbtestlogback_Defectinfo>(tk, new SetValueCollection<Pcbtestlogback_Defectinfo>(new CommonSetValue<Pcbtestlogback_Defectinfo>(setv)), new ConditionCollection<Pcbtestlogback_Defectinfo>(new EqualCondition<Pcbtestlogback_Defectinfo>(cond)));
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_defectCodeID).Value = testLogDfcti.DefectCode;
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_pcbtestlogbackid).Value = testLogDfcti.TestLogID;
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_id).Value = testLogDfcti.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteTestLogDefect(TestLogDefect testLogDfcti)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBTestLog_DefectInfo));
                    }
                }
                sqlCtx.Params[_Schema.PCBTestLog_DefectInfo.fn_ID].Value = testLogDfcti.ID;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void PersistDeleteTestLogDefectBack(TestLogDefect testLogDfcti)
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
                        sqlCtx = FuncNew.GetCommonDelete<_Metas.Pcbtestlogback_Defectinfo>(tk);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlogback_Defectinfo.fn_id).Value = testLogDfcti.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePCBAttribute(ProductAttribute pcba)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBAttr cond = new _Schema.PCBAttr();
                        cond.PCBNo = pcba.ProductId;
                        cond.AttrName = pcba.AttributeName;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttr), new List<string>() { _Schema.PCBAttr.fn_AttrValue, _Schema.PCBAttr.fn_Editor}, null, null,null, cond, null,null,null,null,null,null,null);
                    }
                }
                sqlCtx.Params[_Schema.PCBAttr.fn_AttrName].Value = pcba.AttributeName;
                sqlCtx.Params[_Schema.PCBAttr.fn_PCBNo].Value = pcba.ProductId;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBAttr.fn_AttrValue)].Value = pcba.AttributeValue;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBAttr.fn_Editor)].Value = pcba.Editor;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBAttr.fn_Udt)].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePCBAttributeLog(ProductAttributeLog pcbal)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttrLog));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBAttrLog.fn_AttrName].Value = pcbal.AttributeName;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_AttrNewValue].Value = pcbal.NewValue;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_AttrOldValue].Value = pcbal.OldValue;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_Descr].Value = pcbal.Descr;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_Editor].Value = pcbal.Editor;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_ID].Value = pcbal.ID;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_PCBModelID].Value = pcbal.Model;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_PCBNo].Value = pcbal.ProductId;
                sqlCtx.Params[_Schema.PCBAttrLog.fn_Station].Value = pcbal.Station;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteMBRepair(Repair rep)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBRepair));
                    }
                }
                sqlCtx.Params[_Schema.PCBRepair.fn_ID].Value = rep.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void UpdateMBStatusBatch_Inner(IList<MBStatus> items)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    _Schema.SQLContextCollection sqlCtxSet = new _Schema.SQLContextCollection();

                    int i = 0;
                    foreach (MBStatus entry in items)
                    {
                        _Schema.SQLContext sqlCtx = ComposeForUpdateMBStatusBatch(entry);
                        sqlCtxSet.AddOne(i, sqlCtx);
                        i++;
                    }
                    _Schema.SQLContext sqlCtxBatch = sqlCtxSet.MergeToOneNonQuery();
                    _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtxBatch.Sentence, sqlCtxBatch.Params.Values.ToArray<SqlParameter>());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private SQLContext ComposeForUpdateMBStatusBatch(MBStatus status)
        {
            SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    sqlCtx = Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBStatus));
                }
            }
            DateTime cmDt = SqlHelper.GetDateTime();
            sqlCtx.Params[PCBStatus.fn_Editor].Value = status.Editor;
            sqlCtx.Params[PCBStatus.fn_LineID].Value = status.Line;
            sqlCtx.Params[PCBStatus.fn_PCBID].Value = status.Pcbid;
            sqlCtx.Params[PCBStatus.fn_StationID].Value = status.Station;
            sqlCtx.Params[PCBStatus.fn_Status].Value = status.Status;
            sqlCtx.Params[PCBStatus.fn_Udt].Value = cmDt;
            sqlCtx.Params[PCBStatus.fn_TestFailCount].Value = status.TestFailCount;
            return sqlCtx;
        }

        private void AddMBLogBatch_Inner(IList<MBLog> mblogs)
        {
            try
            {
                if (mblogs != null && mblogs.Count > 0)
                {
                    _Schema.SQLContextCollection sqlCtxSet = new _Schema.SQLContextCollection();

                    int i = 0;
                    foreach (MBLog entry in mblogs)
                    {
                        _Schema.SQLContext sqlCtx = ComposeForAddMBLogBatch(entry);
                        sqlCtxSet.AddOne(i, sqlCtx);
                        i++;
                    }
                    _Schema.SQLContext sqlCtxBatch = sqlCtxSet.MergeToOneNonQuery();
                    _Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtxBatch.Sentence, sqlCtxBatch.Params.Values.ToArray<SqlParameter>());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private SQLContext ComposeForAddMBLogBatch(MBLog mblog)
        {
            SQLContext sqlCtx = null;
            lock (MethodBase.GetCurrentMethod())
            {
                if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                {
                    sqlCtx = Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCBLog));
                }
            }
            DateTime cmDt = SqlHelper.GetDateTime();
            sqlCtx.Params[PCBLog.fn_Cdt].Value = cmDt;
            sqlCtx.Params[PCBLog.fn_Editor].Value = mblog.Editor;
            //sqlCtx.Params[PCBLog.fn_ID].Value = mblog.ID;
            sqlCtx.Params[PCBLog.fn_LineID].Value = mblog.LineID;
            sqlCtx.Params[PCBLog.fn_PCBModelID].Value = mblog.Model;
            sqlCtx.Params[PCBLog.fn_PCBID].Value = mblog.PCBID;
            sqlCtx.Params[PCBLog.fn_StationID].Value = mblog.StationID;
            sqlCtx.Params[PCBLog.fn_Status].Value = mblog.Status;
            return sqlCtx;
        }

        private void ReplaceMBSn_MODismantleLog(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MODismantleLog cond = new _Schema.MODismantleLog();
                        cond.PCBNo = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MODismantleLog), new List<string>() { _Schema.MODismantleLog.fn_PCBNo}, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                //DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.MODismantleLog.fn_PCBNo].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MODismantleLog.fn_PCBNo)].Value = newSn;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.MODismantleLog.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        //private void ReplaceMBSn_PCB(string oldSn, string newSn)
        //{
        //    try
        //    {
        //        SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.PCB cond = new _Schema.PCB();
        //                cond.PCBNo = oldSn;
        //                sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCB), new List<string>() { _Schema.PCB.fn_PCBNo }, null, null, null, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        DateTime cmDt = SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.PCB.fn_PCBNo].Value = oldSn;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCB.fn_PCBNo)].Value = newSn;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCB.fn_Udt)].Value = cmDt;
        //        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void ReplaceMBSn_PCBInfo(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBInfo cond = new _Schema.PCBInfo();
                        cond.PCBID = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBInfo), new List<string>() { _Schema.PCBInfo.fn_PCBID }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBInfo.fn_PCBID].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBInfo.fn_PCBID)].Value = newSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBInfo.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_PCBLog(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBLog cond = new _Schema.PCBLog();
                        cond.PCBID = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBLog), new List<string>() { _Schema.PCBLog.fn_PCBID }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                //DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBLog.fn_PCBID].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBLog.fn_PCBID)].Value = newSn;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBLog.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_PCBRepair(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBRepair cond = new _Schema.PCBRepair();
                        cond.PCBID = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBRepair), new List<string>() { _Schema.PCBRepair.fn_PCBID }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBRepair.fn_PCBID].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBRepair.fn_PCBID)].Value = newSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBRepair.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_PCBStatus(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBStatus cond = new _Schema.PCBStatus();
                        cond.PCBID = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBStatus), new List<string>() { _Schema.PCBStatus.fn_PCBID }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBStatus.fn_PCBID].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBStatus.fn_PCBID)].Value = newSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBStatus.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_PCBTestLog(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBTestLog cond = new _Schema.PCBTestLog();
                        cond.PCBID = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBTestLog), new List<string>() { _Schema.PCBTestLog.fn_PCBID }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                //DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBTestLog.fn_PCBID].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBTestLog.fn_PCBID)].Value = newSn;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBTestLog.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_PCB_Part(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCB_Part cond = new _Schema.PCB_Part();
                        cond.pcbno = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCB_Part), new List<string>() { _Schema.PCB_Part.fn_pcbno }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCB_Part.fn_pcbno].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCB_Part.fn_pcbno)].Value = newSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCB_Part.fn_udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_SnoLog3D(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.SnoLog3D cond = new _Schema.SnoLog3D();
                        cond.PCBID = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.SnoLog3D), new List<string>() { _Schema.SnoLog3D.fn_PCBID }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                //DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.SnoLog3D.fn_PCBID].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.SnoLog3D.fn_PCBID)].Value = newSn;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.SnoLog3D.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_Attr(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBAttr cond = new _Schema.PCBAttr();
                        cond.PCBNo = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttr), new List<string>() { _Schema.PCBAttr.fn_PCBNo }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBAttr.fn_PCBNo].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBAttr.fn_PCBNo)].Value = newSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBAttr.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReplaceMBSn_AttrLog(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCBAttrLog cond = new _Schema.PCBAttrLog();
                        cond.PCBNo = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCBAttrLog), new List<string>() { _Schema.PCBAttrLog.fn_PCBNo }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                //DateTime cmDt = SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PCBAttrLog.fn_PCBNo].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBAttrLog.fn_PCBNo)].Value = newSn;
                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.PCBAttrLog.fn_Udt)].Value = cmDt;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        //private void ReplaceMBSn_TransferToFISList(string oldSn, string newSn)
        //{
        //    try
        //    {
        //        SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.TransferToFISList cond = new _Schema.TransferToFISList();
        //                cond.PCBNo = oldSn;
        //                sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TransferToFISList), new List<string>() { _Schema.TransferToFISList.fn_PCBNo }, null, null, null, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        DateTime cmDt = SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.TransferToFISList.fn_PCBNo].Value = oldSn;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.TransferToFISList.fn_PCBNo)].Value = newSn;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.TransferToFISList.fn_Udt)].Value = cmDt;
        //        SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void ReplaceMBSn_TestBoxDataLog(string oldSn, string newSn)
        {
            try
            {
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.TestBoxDataLog cond = new _Schema.TestBoxDataLog();
                        cond.PCBNo = oldSn;
                        sqlCtx = Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.TestBoxDataLog), new List<string>() { _Schema.TestBoxDataLog.fn_PCBNo }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.TestBoxDataLog.fn_PCBNo].Value = oldSn;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.TestBoxDataLog.fn_PCBNo)].Value = newSn;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetPCBInfoValue(string pcbno, string infotype)
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
                        tf1 = new TableAndFields<Pcb>();
                        Pcb cond = new Pcb();
                        cond.pcbno = pcbno;
                        tf1.Conditions.Add(new EqualCondition<Pcb>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.Pcbinfo>();
                        _Metas.Pcbinfo cond2 = new _Metas.Pcbinfo();
                        cond2.infoType = infotype;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Pcbinfo>(cond2));
                        tf2.AddRangeToGetFieldNames(_Metas.Pcbinfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        mtns.TableConnectionCollection tblCnnts = new mtns.TableConnectionCollection(
                            new mtns.TableConnectionItem<Pcb, _Metas.Pcbinfo>(tf1, Pcb.fn_pcbno, tf2, _Metas.Pcbinfo.fn_pcbno));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "TOP 1", tafa, tblCnnts, "t2." + _Metas.Pcbinfo.fn_cdt);
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.Pcb.fn_pcbno)).Value = pcbno;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Pcbinfo.fn_infoType)).Value = infotype;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf2.Alias, _Metas.Pcbinfo.fn_infoValue)));
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

        #region . Defered  .

        public void DeleteMBSectionDefered(IUnitOfWork uow, string startSn, string endSn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), startSn, endSn);
        }

        public void SetMaxMBNODefered(IUnitOfWork uow, string mbCode, string mbType, IMB maxMO)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbCode, mbType, maxMO);
        }

        public void RemoveBatchDefered(IUnitOfWork uow, IList<IMB> items)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), items);
        }

        public void UpdateMBStatusBatchDefered(IUnitOfWork uow, IList<MBStatus> mbstts)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbstts);
        }

        public void AddMBLogBatchDefered(IUnitOfWork uow, IList<MBLog> mblogs)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mblogs);
        }

        /// <summary>
        /// Replace Old MB Data with New MB Sno, defered version
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        /// <param name="oldSn">oldSn</param>
        /// <param name="newSn">newSn</param>
        /// <remarks>
        /// 将下列各表中的Old MB Sno 对应记录的PCBNo 栏位Update 为New MB Sno
        /// IMES_PCA..MODismantleLog
        /// IMES_PCA..PCB
        /// IMES_PCA..PCBInfo
        /// IMES_PCA..PCBLog
        /// IMES_PCA..PCBRepair
        /// IMES_PCA..PCBStatus
        /// IMES_PCA..PCBTestLog
        /// IMES_PCA..PCB_Part
        /// IMES_PCA..SnoLog3D
        /// IMES_PCA..TransferToFISList
        /// </remarks>
        public void ReplaceMBSnDefered(IUnitOfWork uow, string oldSn, string newSn)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), oldSn, newSn);
        }

        public void InsertRptPcaRepDefered(IUnitOfWork uow, RptPcaRepInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddMBTestDefered(IUnitOfWork uow, MBTestDef mbTest)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbTest);
        }

        public void DeleteMBTestDefered(IUnitOfWork uow, string code, string family, bool type)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), code, family, type);
        }

        public void UpdatePCBStatusDefered(IUnitOfWork uow, PCBStatusInfo setValue, PCBStatusInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddMBCFGDefered(IUnitOfWork uow, MBCFGDef mbcfgDef)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbcfgDef);
        }

        public void DeleteMBCFGDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void UpdateMBCFGDefered(IUnitOfWork uow, MBCFGDef mbcfgDef, string mbCode, string series, string type)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbcfgDef, mbCode, series, type);
        }

        public void AddITCNDDefectCheckDefered(IUnitOfWork uow, ITCNDDefectCheckDef item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void RemoveITCNDDefectCheckDefered(IUnitOfWork uow, string family)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), family);
        }

        public void RemoveITCNDDefectCheckbyFamilyAndCodeDefered(IUnitOfWork uow, string family, string code)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), family, code);
        }

        public void UpdateMtaMarkByRepairIdDefered(IUnitOfWork uow, int repairId, string mark)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), repairId, mark);
        }

        public void InsertMtaMarkInfoDefered(IUnitOfWork uow, MtaMarkInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateBorrowLogDefered(IUnitOfWork uow, IMES.DataModel.BorrowLog item, string statusCondition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, statusCondition);
        }

        public void UpdateRptPcaRepInfoDefered(IUnitOfWork uow, RptPcaRepInfo setValue, RptPcaRepInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateMtaMarkInfoDefered(IUnitOfWork uow, MtaMarkInfo setValue, MtaMarkInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateQtyFromPcaIctCountByCdtAndPdLineDefered(IUnitOfWork uow, int qty, DateTime cdt, string pdLine)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), qty, cdt, pdLine);
        }

        public void InsertPcaIctCountInfoDefered(IUnitOfWork uow, PcaIctCountInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertFruDetInfoDefered(IUnitOfWork uow, FruDetInfo newFruDet)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), newFruDet);
        }

        public void UpdatePcbStatusesDefered(IUnitOfWork uow, MBStatus setValue, string[] pcbIds)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, pcbIds);
        }

        public void AddMBLogsDefered(IUnitOfWork uow, MBLog[] mbLogs)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbLogs);
        }

        public void AddMBInfoesDefered(IUnitOfWork uow, IList<IMES.FisObject.PCA.MB.MBInfo> mbInfoes)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbInfoes);
        }

        public void RemoveSATestCheckRuleItemDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddSATestCheckRuleItemDefered(IUnitOfWork uow, PcaTestCheckInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateTestCheckRuleItemDefered(IUnitOfWork uow, PcaTestCheckInfo item, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, id);
        }

        public void UpdatePCBLotInfoDefered(IUnitOfWork uow, PcblotInfo setValue, PcblotInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateLotInfoDefered(IUnitOfWork uow, LotInfo setValue, LotInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdateLotInfoForDecQtyDefered(IUnitOfWork uow, LotInfo setValue, LotInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void InsertLotInfoDefered(IUnitOfWork uow, LotInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateLotInfoForIncQtyDefered(IUnitOfWork uow, LotInfo setValue, LotInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void InsertPCBLotInfoDefered(IUnitOfWork uow, PcblotInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertLotSettingInfoDefered(IUnitOfWork uow, LotSettingInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteLotSettingInfoDefered(IUnitOfWork uow, LotSettingInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void UpdateLotSettingInfoDefered(IUnitOfWork uow, LotSettingInfo setValue, LotSettingInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void InsertPcblotcheckInfoDefered(IUnitOfWork uow, PcblotcheckInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertPCBLotCheckFromPCBLotDefered(IUnitOfWork uow, string pcbNo, string editor, PcblotInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pcbNo, editor, condition);
        }

        public void UpdatePcboqcrepairInfoDefered(IUnitOfWork uow, PcboqcrepairInfo setValue, PcboqcrepairInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeletePcboqcrepairInfoDefered(IUnitOfWork uow, PcboqcrepairInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void InsertPcboqcrepairInfoDefered(IUnitOfWork uow, PcboqcrepairInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertPcboqcrepairDefectinfoDefered(IUnitOfWork uow, Pcboqcrepair_DefectinfoInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePcboqcrepairDefectinfoDefered(IUnitOfWork uow, Pcboqcrepair_DefectinfoInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void RemoveSMTLineDefered(IUnitOfWork uow, SMTLineDef condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void AddSMTLineDefered(IUnitOfWork uow, SMTLineDef item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void ChangeSMTLineDefered(IUnitOfWork uow, SMTLineDef setValue, SMTLineDef condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void DeleteDeptInfoDefered(IUnitOfWork uow, DeptInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void AddDeptInfoDefered(IUnitOfWork uow, DeptInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateDeptInfoDefered(IUnitOfWork uow, DeptInfo setValue, DeptInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddFamilyMbInfoDefered(IUnitOfWork uow, FamilyMbInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteFamilyMbInfoDefered(IUnitOfWork uow, FamilyMbInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void ModifyFamilyMbInfoDefered(IUnitOfWork uow, FamilyMbInfo setValue, FamilyMbInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddSMTTimeInfoDefered(IUnitOfWork uow, SmttimeInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteSMTTimeInfoDefered(IUnitOfWork uow, SmttimeInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void UpdateSMTTimeInfoDefered(IUnitOfWork uow, SmttimeInfo setValue, SmttimeInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void CreateAlarmWithSpecifiedDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), alarmSetting);
        }

        public void UpdateForCreateAlarmWithDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), alarmSetting);
        }

        public void CreateAlarmWithExcludedDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), alarmSetting);
        }

        public void CreateAlarmWithAllDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), alarmSetting);
        }

        public void CreateAlarmWithYieldForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), alarmSetting);
        }

        public void UpdatePcbRepairDefered(IUnitOfWork uow, Repair setValue, Repair condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void AddPcbRepairDefered(IUnitOfWork uow, Repair item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPcbRepairDefectDefered(IUnitOfWork uow, RepairDefect item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdatePcbDefered(IUnitOfWork uow, PcbEntityInfo setValue, PcbEntityInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdatePcbInfoDefered(IUnitOfWork uow, IMES.FisObject.PCA.MB.MBInfo setValue, IMES.FisObject.PCA.MB.MBInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        public void UpdatePcbStatusDefered(IUnitOfWork uow, MBStatus setValue, MBStatus condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        #endregion

        #region add new interface
        public IList<MbCodeAndMdlInfo> GetMbCodeAndMdlInfoList(string bomNodeType,
                                                                                              string partType,
                                                                                              string mbType,
                                                                                              string mdlType,
                                                                                              string mdlPostfix)
        {
            try
            {
                IList<MbCodeAndMdlInfo> ret = new List<MbCodeAndMdlInfo>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();

                        sqlCtx.Sentence = @"SELECT DISTINCT b.InfoValue as [MB Code], 
                                                           c.InfoValue as [MDL],
                                                           b.InfoValue + ' ' + c.InfoValue as [DisplayName]
	                                            FROM Part a, 
	                                                 PartInfo b, 
	                                                 PartInfo c
	                                            WHERE a.BomNodeType = @BomNodeType
		                                            AND a.PartNo = b.PartNo
		                                            AND a.PartNo = c.PartNo
                                                    AND a.PartType = @PartType 
		                                            AND b.InfoType = @mbType
		                                            AND c.InfoType = @mdlType
		                                            AND Upper(c.InfoValue) LIKE @mdlPostfix
	                                            order by b.InfoValue";
                        sqlCtx.AddParam("BomNodeType", new SqlParameter("@BomNodeType", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));
                        sqlCtx.AddParam("mbType", new SqlParameter("@mbType", SqlDbType.VarChar));
                        sqlCtx.AddParam("mdlType", new SqlParameter("@mdlType", SqlDbType.VarChar));
                        sqlCtx.AddParam("mdlPostfix", new SqlParameter("@mdlPostfix", SqlDbType.VarChar));  
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("BomNodeType").Value = bomNodeType;
                sqlCtx.Param("PartType").Value = partType;
                sqlCtx.Param("mbType").Value = mbType;
                sqlCtx.Param("mdlType").Value = mdlType;
                sqlCtx.Param("mdlPostfix").Value = "%" + mdlPostfix+"%" ;               

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MbCodeAndMdlInfo item = new MbCodeAndMdlInfo();
                            item.mbCode = g.GetValue_Str(sqlR, 0);
                            item.mdl = g.GetValue_Str(sqlR, 1);
                            item.displayName = g.GetValue_Str(sqlR, 2);
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

        public int GetMbRepairCountByLocationAndStation(string pcbNo,
                                                                                 string station,
                                                                                 string location)
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

                        sqlCtx.Sentence = @"select count(1) as qty
                                                            from PCBRepair a 
                                                            inner join PCBRepair_DefectInfo b on a.ID= b.PCARepairID
                                                            where a.PCBNo=@PCBNo and
                                                                  a.Station=@Station and
                                                                  b.Location=@Location";
                        sqlCtx.AddParam("PCBNo", new SqlParameter("@PCBNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Location", new SqlParameter("@Location", SqlDbType.VarChar));
                       
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("PCBNo").Value = pcbNo;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("Location").Value = location;
             
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {

                   
                    if (sqlR != null & sqlR.Read())
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


       public IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, DateTime beginCdt)
        {
            try
            {
                IList<TestLog> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Pcbtestlog cond = new _Metas.Pcbtestlog();
                        cond.pcbno = pcbNo;
                        
                        _Metas.Pcbtestlog cond2 = new _Metas.Pcbtestlog();
                        cond2.cdt = beginCdt;

                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.Pcbtestlog>(tk, null, null, new ConditionCollection<_Metas.Pcbtestlog>(
                            new EqualCondition<_Metas.Pcbtestlog>(cond),
                             new GreaterCondition<_Metas.Pcbtestlog>(cond2)), _Metas.Pcbtestlog.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(_Metas.Pcbtestlog.fn_pcbno).Value = pcbNo;
                sqlCtx.Param(g.DecG(_Metas.Pcbtestlog.fn_cdt)).Value = beginCdt;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<TestLog>();
                        while (sqlR.Read())
                        {
                            TestLog item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<_Metas.Pcbtestlog, TestLog>(item, sqlR, sqlCtx);
                            item.Status = (TestLog.TestLogStatus)Enum.Parse(typeof(TestLog.TestLogStatus), g.GetValue_Int32(sqlR, sqlCtx.Indexes(_Metas.Pcbtestlog.fn_status)).ToString());
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


       public IList<IMB> GetChildMBFromParentMB(string pcbNo)
       {
           try
           {
               IList<IMB> ret = new List<IMB>();

               SQLContext sqlCtx = null;
               lock (MethodBase.GetCurrentMethod())
               {
                   if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                   {
                       PCB cond = new PCB();
                       cond.PCBNo = pcbNo;
                       sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(PCB), null, null, null, cond,
                                                                         null, null, null, null, null, null);
                       sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PCB.fn_PCBNo);
                       
                   }
               }
               sqlCtx.Params[PCB.fn_PCBNo].Value = pcbNo.Substring(0, 5) + "[1-9]" + pcbNo.Substring(6, 4);

               using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, 
                                                                                                    CommandType.Text, 
                                                                                                    sqlCtx.Sentence,
                                                                                                    sqlCtx.Params.Values.ToArray<SqlParameter>()))
               {
                   if (sqlR != null)
                   {
                       while (sqlR.Read())
                       {
                           IMB item = new MB(
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBNo]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_SMTMOID]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTSN]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_PCBModelID]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_DateCode]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_MAC]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_UUID]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ECR]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_IECVER]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CUSTVER]),
                               GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_CVSN]),
                               GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Udt]),
                               GetValue_DateTime(sqlR, sqlCtx.Indexes[PCB.fn_Cdt]));
                               //item.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
                               //item.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

                               //item.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
                               //item.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
                               //item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
                               //item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
                               //item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
                               //item.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
                               //item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
                                addMBRemaingPropertyValue(item, sqlR, sqlCtx);
                           ((MB)item).Tracker.Clear();
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


       public void UpdatePCBStatusByMultiMB(IList<string> pcbNoList,
                                                           string station,
                                                           int status,
                                                           string line,
                                                           string editor)
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
                       sqlCtx.Sentence = @"update  PCBStatus
                                                            set  Station=@Station,
                                                                   Status=@Status, 
                                                                   Line =@Line,                                                                      
                                                                    Editor = @Editor,
                                                                    Udt = GETDATE()
                                                                where   PCBNo in ('{0}') ";
                       sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                       sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.Int));
                       sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));         
                     
                       sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }
               sqlCtx.Param("Station").Value = station;
               sqlCtx.Param("Status").Value = status;
               sqlCtx.Param("Line").Value = line;              

               sqlCtx.Param("Editor").Value = editor;

               string pcbNoStr = string.Join("','", pcbNoList.ToArray());
               string sqlStr = string.Format(sqlCtx.Sentence, pcbNoStr);
               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                CommandType.Text,
                                                                               sqlStr,
                                                                                sqlCtx.Params);

           }
           catch (Exception)
           {
               throw;
           }

       }

       public void UpdatePCBStatusByMultiMBDefered(IUnitOfWork uow,
                                                           IList<string> pcbNoList,
                                                           string station,
                                                           int status,
                                                           string line,
                                                           string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     pcbNoList,
                                                      station,
                                                      status,
                                                      line,
                                                      editor);
       }

       public void UpdatePCBPreStation(IList<TbProductStatus> pcbStatusList)
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
                       sqlCtx.Sentence = @"MERGE INTO PCBStatusEx as T
                                                             Using (
		                                                                select a.ProductID as PCBNo,  a.Station, a.Status,  a.Line,
                                                                                   a.TestFailCount, a.Editor, a.Udt
		                                                                from @TbProductStatus a
	                                                                ) as S
	                                                         ON T.PCBNo = S.PCBNo 
	                                                         WHEN NOT MATCHED THEN
		                                                            insert (PCBNo, PreStation, PreStatus, PreLine, PreTestFailCount, PreEditor, PreUdt)
		                                                            values (S.PCBNo, S.Station, S.[Status], S.Line, S.TestFailCount, S.Editor, S.Udt)
	                                                         WHEN MATCHED THEN
		                                                            update set PreStation=S.Station, PreStatus=S.[Status], PreLine=S.Line, 
		                                                                             PreTestFailCount=S.TestFailCount, PreEditor=S.Editor, PreUdt=S.Udt;";
                       SqlParameter para = new SqlParameter("@TbProductStatus", SqlDbType.Structured);
                       para.TypeName = "TbProductStatus";
                       para.Direction = ParameterDirection.Input;
                       sqlCtx.AddParam("TbProductStatus", para);
                     
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               DataTable dt = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable<TbProductStatus>(pcbStatusList);

               sqlCtx.Param("TbProductStatus").Value = dt;
               

               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                  CommandType.Text,
                                                                                  sqlCtx.Sentence,
                                                                                  sqlCtx.Params);
           }
           catch (Exception)
           {
               throw;
           }
       }
       public void UpdatePCBPreStationDefered(IUnitOfWork uow, IList<TbProductStatus> pcbStatusList)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     pcbStatusList);

       }

       public void WritePCBLogByMultiMB(IList<string> pcbNoList,
                                                           string station,
                                                           int status,
                                                           string line,
                                                           string editor)
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
                       sqlCtx.Sentence = @"insert PCBLog(PCBNo, PCBModel, Station, Status, Line, Editor, Cdt)
                                                        select a.PCBNo, a.PCBModelID, @Station, @Status, 
                                                                   (case when @Line='' then c.Line else @Line end), @Editor, GETDATE() 
                                                         from PCB a
                                                         inner join PCBStatus c on a.PCBNo = c.PCBNo   
                                                         inner join @PCBNoList b on a.PCBNo = b.data  ";
                       SqlParameter para = new SqlParameter("@PCBNoList", SqlDbType.Structured);
                       para.TypeName = "TbStringList";
                       para.Direction = ParameterDirection.Input;
                       sqlCtx.AddParam("PCBNoList", para);
                       sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                       sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.Int));
                       sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));                      
                       sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               DataTable dt = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable(pcbNoList);

               sqlCtx.Param("PCBNoList").Value = dt;
               sqlCtx.Param("Station").Value = station;
               sqlCtx.Param("Status").Value = status;
               sqlCtx.Param("Line").Value = string.IsNullOrEmpty(line) ? string.Empty : line.Trim();
              
               sqlCtx.Param("Editor").Value = editor;

               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                  CommandType.Text,
                                                                                  sqlCtx.Sentence,
                                                                                  sqlCtx.Params);
           }
           catch (Exception)
           {
               throw;
           }
       }

       public void WritePCBLogByMultiMBDefered(IUnitOfWork uow,
                                                           IList<string> pcbNoList,
                                                           string station,
                                                           int status,
                                                           string line,
                                                           string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                      pcbNoList,
                                                       station,
                                                          status,
                                                          line,
                                                          editor);
       }

       public void UpdateRCTO146MBbyMultiMB(IList<string> pcbNoList,
                                                       string skuModel,
                                                       string pizzaID,
                                                       decimal cartonWeight,
                                                       string cartonSN,
                                                       string deliveryNo,
                                                       string palletNo,
                                                       string shipMode,
                                                        string editor)
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
                       sqlCtx.Sentence = @"update  PCB
                                                            set  SkuModel=@SkuModel,
                                                                   PizzaID=@PizzaID,
                                                                   CartonWeight=@CartonWeight,
                                                                    CartonSN=@CartonSN,
                                                                    PalletNo=@PalletNo,
                                                                    DeliveryNo=@DeliveryNo,
                                                                    ShipMode=@ShipMode,                                                                  
                                                                    Udt = GETDATE()
                                                                where   PCBNo in ('{0}') ";
                       sqlCtx.AddParam("SkuModel", new SqlParameter("@SkuModel", SqlDbType.VarChar));
                       sqlCtx.AddParam("PizzaID", new SqlParameter("@PizzaID", SqlDbType.VarChar));
                       sqlCtx.AddParam("CartonWeight", new SqlParameter("@CartonWeight", SqlDbType.Decimal));
                       sqlCtx.AddParam("CartonSN", new SqlParameter("@CartonSN", SqlDbType.VarChar));
                       sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));

                       sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                       sqlCtx.AddParam("ShipMode", new SqlParameter("@ShipMode", SqlDbType.VarChar));

                      //sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }
               sqlCtx.Param("SkuModel").Value = skuModel;
               sqlCtx.Param("PizzaID").Value = pizzaID;
               sqlCtx.Param("CartonWeight").Value = cartonWeight;
               sqlCtx.Param("CartonSN").Value = cartonSN;
               sqlCtx.Param("PalletNo").Value = palletNo;

               sqlCtx.Param("DeliveryNo").Value = deliveryNo;
               sqlCtx.Param("ShipMode").Value = shipMode;


               //sqlCtx.Param("Editor").Value = editor;
               string pcbNoStr= string.Join("','",pcbNoList.ToArray());
               string sqlStr = string.Format(sqlCtx.Sentence, pcbNoStr);
               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                CommandType.Text,
                                                                               sqlStr,
                                                                                sqlCtx.Params);
               
           }
           catch (Exception)
           {
               throw;
           }
       }

       public void UpdateRCTO146MBbyMultiMBDefered(IUnitOfWork uow,
                                                       IList<string> pcbNoList,
                                                        string skuModel,
                                                        string pizzaID,
                                                        decimal cartonWeight,
                                                        string cartonSN,
                                                        string deliveryNo,
                                                        string palletNo,
                                                        string shipMode,
                                                        string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), 
                                                     pcbNoList,
                                                     skuModel,
                                                     pizzaID,
                                                     cartonWeight,
                                                     cartonSN,
                                                     deliveryNo,
                                                     palletNo,
                                                    shipMode,
                                                    editor);
       }
       //Combine Carton With Pallet
       public void UpdatePalletNobyCaronSn(IList<string> cartonSNList,
                                                            string palletNo,
                                                            string editor)
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
                       sqlCtx.Sentence = @"update  PCB
                                                            set  PalletNo=@PalletNo,
                                                                   Udt = GETDATE()
                                                                where  CartonSN in ('{0}') ";
                       
                       sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                       sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }
               
               sqlCtx.Param("PalletNo").Value = palletNo;

               sqlCtx.Param("Editor").Value = editor;
               string cartonSNStr = string.Join("','", cartonSNList.ToArray());
               string sqlStr = string.Format(sqlCtx.Sentence, cartonSNStr);
               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                CommandType.Text,
                                                                               sqlStr,
                                                                                sqlCtx.Params);

           }
           catch (Exception)
           {
               throw;
           }

       }

       public void UpdatePalletNobyCaronSnDefered(IUnitOfWork uow,
                                                                        IList<string> cartonSNList,
                                                                        string palletNo,
                                                                        string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     cartonSNList,
                                                     palletNo,
                                                     editor);
       }

       //Get Combined Pallet CartonQty and DeliveryNo By Carton
      
       public IList<string> GetDeliveryNoByCartonSN(IList<string> cartonSNList)
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
                       sqlCtx.Sentence = @"select distinct DeliveryNo
                                                                 from    PCB
                                                                 where  CartonSN in ('{0}') ";
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               string cartonSNStr = string.Join("','", cartonSNList.ToArray());
               string sqlStr = string.Format(sqlCtx.Sentence, cartonSNStr);

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                           CommandType.Text,
                                                                                                                           sqlStr,
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

       public IList<CombinedPalletCarton> GetCartonQtywithCombinedPallet(string deliveryNo)
       {
           try
           {
               IList<CombinedPalletCarton> ret = new List<CombinedPalletCarton>();

               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @"select PalletNo, count(distinct CartonSN) as Qty
                                                        from    PCB
                                                        where  DeliveryNo =@DeliveryNo and
                                                                    PalletNo != ''
                                                        group by PalletNo";

                       sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("DeliveryNo").Value = deliveryNo;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                           CommandType.Text,
                                                                                                                           sqlCtx.Sentence,
                                                                                                                           sqlCtx.Params))
               {
                   if (sqlR != null)
                   {
                       while (sqlR.Read())
                       {

                           ret.Add(SQLData.ToObject<CombinedPalletCarton>(sqlR));
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

       public IList<TbProductStatus> GetMBStatus(IList<string> mbSnList)
       {
           try
           {
               IList<TbProductStatus> ret = new List<TbProductStatus>();
               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @"select PCBNo as ProductID, Station, Status, Line, TestFailCount, 
                                                                    Editor, Cdt, Udt
                                                         from PCBStatus a
                                                         inner join @MBSNList b on a.PCBNo=b.data ";
                       SqlParameter para = new SqlParameter("@MBSNList", SqlDbType.Structured);
                       para.TypeName = "TbStringList";
                       para.Direction = ParameterDirection.Input;
                       sqlCtx.AddParam("MBSNList", para);

                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }


               DataTable dt = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable(mbSnList);

               sqlCtx.Param("MBSNList").Value = dt;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                           CommandType.Text,
                                                                                                                           sqlCtx.Sentence,
                                                                                                                           sqlCtx.Params))
               {
                   while (sqlR != null && sqlR.Read())
                   {
                       TbProductStatus item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<TbProductStatus>(sqlR);

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
       public int GetCombinedMBQtyWithDeliveryNo(string deliveryNo)
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
                       sqlCtx.Sentence = @"select count(distinct PCBNo) as Qty
                                                                 from    PCB
                                                                 where  DeliveryNo=@DeliveryNo ";

                       sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("DeliveryNo").Value = deliveryNo;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
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

       public IList<RCTO146MBInfo> GetRCTO146MBByCartonSN(IList<string> cartonSNList)
       {
           try
           {
               IList<RCTO146MBInfo> ret = new List<RCTO146MBInfo>();

               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @" select a.PCBNo, a.PalletNo, a.DeliveryNo, a.ShipMode, a.SkuModel,
                                                                    b.Station,b.Status,b.Line,b.TestFailCount, b.Editor,
                                                                    b.Udt
                                                             from PCB a, PCBStatus b
                                                             where a.PCBNo = b.PCBNo and
                                                                   a.CartonSN in ('{0}')  ";
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               string cartonSNStr = string.Join("','", cartonSNList.ToArray());
               string sqlStr = string.Format(sqlCtx.Sentence, cartonSNStr);

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                           CommandType.Text,
                                                                                                                          sqlStr,
                                                                                                                           sqlCtx.Params))
               {
                   if (sqlR != null)
                   {
                       while (sqlR.Read())
                       {
                           ret.Add(_Schema.SQLData.ToObject < RCTO146MBInfo>(sqlR));
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

       public IList<RCTO146MBInfo> GetRCTO146MBByPalletNo(string palletNo)
       {
           try
           {
               IList<RCTO146MBInfo> ret = new List<RCTO146MBInfo>();

               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @" select a.PCBNo, a.PalletNo, a.DeliveryNo, a.ShipMode, a.SkuModel,
                                                                    b.Station,b.Status,b.Line,b.TestFailCount, b.Editor,
                                                                    b.Udt
                                                             from PCB a, PCBStatus b
                                                             where a.PCBNo = b.PCBNo and
                                                                   a.PalletNo=@PalletNo ";

                       sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));

                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("PalletNo").Value = palletNo;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                           CommandType.Text,
                                                                                                                         sqlCtx.Sentence,
                                                                                                                           sqlCtx.Params))
               {
                   if (sqlR != null)
                   {
                       while (sqlR.Read())
                       {
                           ret.Add(_Schema.SQLData.ToObject<RCTO146MBInfo>(sqlR));
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

    
       public void CheckDnQtyAndUpdateDnStatus(string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode)
       {
           int dnQty = DeliveryRep.GetDeliveryQtyOnTrans(deliveryNo,"00");
           int combinedQty = getCombinedQtyByDnOnTrans(deliveryNo);
           int remainingQty =dnQty - combinedQty;
           if (remainingQty < needCombinedQty)
           {
               if (string.IsNullOrEmpty(errorCode))
               {
                   throw new Exception("DeliveryNo:" + deliveryNo + " is not enough remaining Qty:" + remainingQty.ToString());
               }
               else
               {
                   throw new FisException(errorCode, new string[] { deliveryNo, remainingQty.ToString() }); 
               }
           }
           else if (remainingQty == needCombinedQty)
           {
               DeliveryRep.UpdateDeliveryStatus(deliveryNo, fullCombinedStatus);
           }
       }

       public void CheckDnQtyAndUpdateDnStatusDefered(IUnitOfWork uow, string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     deliveryNo,
                                                     needCombinedQty,
                                                     fullCombinedStatus,
                                                     errorCode);
       }



       //Unpack By Carton /Delivery
       public void UnpackRCTO146MBbyCatonSN(string cartonSN,
                                                                 string editor)
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
                       sqlCtx.Sentence = @"update  PCB
                                                            set  SkuModel='',
                                                                   PizzaID='',
                                                                   CartonWeight=0,
                                                                    CartonSN='',
                                                                    PalletNo='',
                                                                    DeliveryNo='',                                                                  
                                                                    Udt = GETDATE()
                                                                where   CartonSN= @CartonSN ";
                     
                       sqlCtx.AddParam("CartonSN", new SqlParameter("@CartonSN", SqlDbType.VarChar));
                       


                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }
              
               sqlCtx.Param("CartonSN").Value = cartonSN;             
               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                CommandType.Text,
                                                                               sqlCtx.Sentence,
                                                                                sqlCtx.Params);

           }
           catch (Exception)
           {
               throw;
           }
       }

       public void UnpackRCTO146MBbyCatonSNDefered(IUnitOfWork uow,
                                                                          string cartonSN,
                                                                           string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     cartonSN,
                                                     editor);
       }

       public void UnpackRCTO146MBbyDeliveryNo(string deliveryNo,
                                                                string editor)
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
                       sqlCtx.Sentence = @"update  PCB
                                                            set  SkuModel='',
                                                                   PizzaID='',
                                                                   CartonWeight=0,
                                                                    CartonSN='',
                                                                    PalletNo='',
                                                                    DeliveryNo='',                                                                  
                                                                    Udt = GETDATE()
                                                                where   DeliveryNo= @DeliveryNo ";

                       sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));



                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("DeliveryNo").Value = deliveryNo;
               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                CommandType.Text,
                                                                               sqlCtx.Sentence,
                                                                                sqlCtx.Params);

           }
           catch (Exception)
           {
               throw;
           }
       }

       public void UnpackRCTO146MBbyDeliveryNoDefered(IUnitOfWork uow,
                                                                          string deliveryNo,
                                                                           string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     deliveryNo,
                                                     editor);
       }


       public IList<RCTO146MBInfo> GetRCTO146MBByDeliveryNo(string deliveryNo)
       {
           try
           {
               IList<RCTO146MBInfo> ret = new List<RCTO146MBInfo>();

               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @" select a.PCBNo, a.PalletNo, a.DeliveryNo, a.ShipMode, a.SkuModel,
                                                                    b.Station,b.Status,b.Line,b.TestFailCount, b.Editor,
                                                                    b.Udt
                                                             from PCB a, PCBStatus b
                                                             where a.PCBNo = b.PCBNo and
                                                                   a.DeliveryNo=@DeliveryNo ";

                       sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));

                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("DeliveryNo").Value = deliveryNo;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                           CommandType.Text,
                                                                                                                         sqlCtx.Sentence,
                                                                                                                           sqlCtx.Params))
               {
                   if (sqlR != null)
                   {
                       while (sqlR.Read())
                       {
                           ret.Add(_Schema.SQLData.ToObject<RCTO146MBInfo>(sqlR));
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
        #endregion

     

       #region Private Check Delivery Qty
       
       private int getCombinedQtyByDnOnTrans(string deliveryNo)
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
                       sqlCtx.Sentence = @"select COUNT(PCBNo) as Qty 
                                                        from PCB 
                                                        where DeliveryNo=@DeliveryNo";

                       sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));

                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("DeliveryNo").Value = deliveryNo;


               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PCA,
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
       #region private add item into IMB
       private void addMBRemaingPropertyValue(IMB item, SqlDataReader sqlR, SQLContext sqlCtx)
       {
           item.State = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_State]);
           item.ShipMode = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_ShipMode]);

           item.CartonWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_cartonWeight]);
           item.UnitWeight = GetValue_Decimal(sqlR, sqlCtx.Indexes[PCB.fn_unitWeight]);
           item.CartonSN = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_cartonSN]);
           item.DeliveryNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_deliveryNo]);
           item.PalletNo = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_palletNo]);
           item.QCStatus = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_qcStatus]);
           item.PizzaID = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_pizzaID]);
           item.SkuModel = GetValue_Str(sqlR, sqlCtx.Indexes[PCB.fn_skuModel]);

       }

       private void addMBSQLParameter(IMB mb, SQLContext sqlCtx)
       {
           sqlCtx.Params[Pcb.fn_state].Value = string.IsNullOrEmpty(mb.State) ? "" : mb.State;
           sqlCtx.Params[Pcb.fn_shipMode].Value = string.IsNullOrEmpty(mb.ShipMode) ? "" : mb.ShipMode;

           sqlCtx.Params[Pcb.fn_cartonWeight].Value = mb.CartonWeight;
           sqlCtx.Params[Pcb.fn_unitWeight].Value = mb.UnitWeight;
           sqlCtx.Params[Pcb.fn_cartonSN].Value = mb.CartonSN;
           sqlCtx.Params[Pcb.fn_deliveryNo].Value = mb.DeliveryNo;
           sqlCtx.Params[Pcb.fn_palletNo].Value = mb.PalletNo;
           sqlCtx.Params[Pcb.fn_qcStatus].Value = mb.QCStatus;
           sqlCtx.Params[Pcb.fn_pizzaID].Value = mb.PizzaID;
           sqlCtx.Params[Pcb.fn_skuModel].Value = mb.SkuModel;

       }
        #endregion

       #region for Pilot Run MO Check
       public bool ExistsPCBInfoAndLogStation(string infoType, string infoValue, IList<string> stationList, int status)
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
                       sqlCtx.Sentence = @"select top 1 a.PCBNo, a.Status 
                                                        from PCBLog a, 
                                                             PCBInfo b,
                                                             @StationList c 
                                                        where a.PCBNo = b.PCBNo and 
                                                             b.InfoType=@InfoType and 
                                                             b.InfoValue=@InfoValue and 
                                                             a.Station =c.data   and
                                                             a.Status =@Status";

                       sqlCtx.AddParam("InfoType", new SqlParameter("@InfoType", SqlDbType.VarChar));
                       sqlCtx.AddParam("InfoValue", new SqlParameter("@InfoValue", SqlDbType.VarChar));
                       sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.Int));
                       SqlParameter para1 = new SqlParameter("@StationList", SqlDbType.Structured);
                       para1.TypeName = "TbStringList";
                       para1.Direction = ParameterDirection.Input;
                       sqlCtx.AddParam("StationList", para1);
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("InfoType").Value = infoType;
               sqlCtx.Param("InfoValue").Value = infoValue;
               sqlCtx.Param("Status").Value = status;

               DataTable dt1 = _Schema.SQLData.ToDataTable(stationList);
               sqlCtx.Param("StationList").Value = dt1;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA,
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

       public void RemovePCBInfosByType(string pcbNo, IList<string> itemTypes)
       {
           MethodBase mthObj = MethodBase.GetCurrentMethod();
           int tk = mthObj.MetadataToken;
           SQLContextNew sqlCtx = null;
           lock (MethodBase.GetCurrentMethod())
           {
               if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
               {
                   _Metas.Pcbinfo eqCond = new _Metas.Pcbinfo();
                   eqCond.pcbno = pcbNo;
                   _Metas.Pcbinfo insetCond = new _Metas.Pcbinfo();
                   insetCond.infoType = "INSET";
                   sqlCtx = _Metas.FuncNew.GetConditionedDelete<_Metas.Pcbinfo>(tk, new _Metas.ConditionCollection<_Metas.Pcbinfo>(
                                                                                                          new _Metas.EqualCondition<_Metas.Pcbinfo>(eqCond),
                                                                                                          new _Metas.InSetCondition<_Metas.Pcbinfo>(insetCond)));
               }
           }
           sqlCtx.Param(_Metas.Pcbinfo.fn_pcbno).Value = pcbNo;
           string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Pcbinfo.fn_infoType),
                                                                             g.ConvertInSet(itemTypes));
           _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA,
                                                                     CommandType.Text,
                                                                     Sentence,
                                                                     sqlCtx.Params);
       }

       public void RemovePCBInfosByTypeDefered(IUnitOfWork uow, string pcbNo, IList<string> itemTypes)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pcbNo, itemTypes);
       }
       #endregion

       #region ASUS CheckSN
       public void ExistsCustomSnThrowError(string pcbNo, string customSn)
       {
           try
           {
               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               _Schema.SQLContext sqlCtx = null;
               lock (mthObj)
               {
                   if (!_Schema.Func.PeerTheSQL(tk, out sqlCtx))
                   {
                       _Schema.PCB cond = new _Schema.PCB();
                       cond.CUSTSN = customSn;
                       sqlCtx = _Schema.Func.GetConditionedFuncSelect(tk, typeof(_Schema.PCB), null, new List<string> { _Schema.PCB.fn_PCBNo },
                                                                                                   cond, null, null, null, null, null, null, null);
                       sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH(NOLOCK) WHERE");
                   }
               }
               sqlCtx.Params[_Schema.PCB.fn_CUSTSN].Value = customSn;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                   CommandType.Text,
                                                                                                                   sqlCtx.Sentence,
                                                                                                                   sqlCtx.Params.Values.ToArray<SqlParameter>()))
               {
                   while (sqlR != null && sqlR.Read())
                   {
                       string id = GetValue_Str(sqlR, 0);
                       if (id != pcbNo)
                       {
                           throw new FisException("CHK1223", new string[] { customSn });
                       }
                   }
               }

           }
           catch (Exception)
           {
               throw;
           }
       }

       public void ExistsCustomSnThrowErrorDefered(IUnitOfWork uow, string pcbNo, string customSn)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), pcbNo, customSn);
       }
       #endregion

       #region PCB_Part backup && delete & insert
       public IList<PCBPartInfo> GetPCBPartByPCBNos(IList<string> pcbNoList)
       {
           try
           {
               IList<PCBPartInfo> ret = new List<PCBPartInfo>();
               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       PCBPartInfo condition = new PCBPartInfo {  PCBNo=string.Empty };
                       Pcb_Part incond = FuncNew.SetColumnFromField<Pcb_Part, PCBPartInfo>(condition);

                       sqlCtx = FuncNew.GetConditionedSelect<Pcb_Part>(tk, null, null,
                                                                                          new ConditionCollection<Pcb_Part>(new InSetCondition<Pcb_Part>(incond)));

                   }
               }

               string sentence = sqlCtx.Sentence.Replace(g.DecInSet(Pcb_Part.fn_pcbno), g.ConvertInSet(pcbNoList));
               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                            CommandType.Text,
                                                                                                                            sentence,
                                                                                                                            sqlCtx.Params))
               {

                   ret = FuncNew.SetFieldFromColumn<Pcb_Part, PCBPartInfo, PCBPartInfo>(ret, sqlR, sqlCtx);
               }

               return ret;
           }
           catch (Exception)
           {
               throw;
           }
       }
       public IList<PCBPartInfo> GetPCBPartByPartSnList(IList<string> partSnList)
       {
           try
           {
               IList<PCBPartInfo> ret = new List<PCBPartInfo>();
               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;     
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       PCBPartInfo condition = new PCBPartInfo { PartSn = string.Empty };
                       Pcb_Part incond = FuncNew.SetColumnFromField<Pcb_Part, PCBPartInfo>(condition);

                       sqlCtx = FuncNew.GetConditionedSelect<Pcb_Part>(tk,null, null,
                                                                                          new ConditionCollection<Pcb_Part>(new InSetCondition<Pcb_Part>(incond)));

                   }
               }

               string sentence = sqlCtx.Sentence.Replace(g.DecInSet(Pcb_Part.fn_partSn), g.ConvertInSet(partSnList));  
               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                            CommandType.Text,
                                                                                                                            sentence,
                                                                                                                            sqlCtx.Params))
               {

                   ret = FuncNew.SetFieldFromColumn<Pcb_Part, PCBPartInfo, PCBPartInfo>(ret, sqlR, sqlCtx);
               }

               return ret;
           }
           catch (Exception)
           {
               throw;
           }
       }
       public void BackupPCBPart(IList<UnpackPCBPartInfo> unpackPCBInfoList)
       {
           try
           {
               _Schema.BulkCopyHelper.BulkCopyToDatabase(unpackPCBInfoList, _Schema.SqlHelper.ConnectionString_PCA, "UnpackPCB_Part");
           }
           catch (Exception)
           {
               throw;
           }
       }
       public void BackupPCBPartDefered(IUnitOfWork uow, IList<UnpackPCBPartInfo> unpackPCBInfoList)
       {
           Action deferAction = () => { BackupPCBPart(unpackPCBInfoList); };
           AddOneInvokeBody(uow, deferAction);
       }
       public void RemovePCBPartByIDs(IList<int> idList)
       {
           try
           {
               MethodBase mthObj = MethodBase.GetCurrentMethod();
               SQLContextNew sqlCtx = null;
               int tk = mthObj.MetadataToken;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       PCBPartInfo condition = new PCBPartInfo { ID = 0 };
                       Pcb_Part incond = FuncNew.SetColumnFromField<Pcb_Part, PCBPartInfo>(condition);

                       sqlCtx = FuncNew.GetConditionedDelete<Pcb_Part>(tk, new ConditionCollection<Pcb_Part>(new InSetCondition<Pcb_Part>(incond)));
                   }
               }
               string sentence = sqlCtx.Sentence.Replace(g.DecInSet(Pcb_Part.fn_id), g.ConvertInSet(idList));
               
               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                              CommandType.Text,
                                                                              sentence,
                                                                              sqlCtx.Params);

           }
           catch (Exception)
           {
               throw;
           }
       }
       public void RemovePCBPartByIDsDefered(IUnitOfWork uow, IList<int> idList)
       {
           Action deferAction = () => { RemovePCBPartByIDs(idList); };
           AddOneInvokeBody(uow, deferAction);
       }

       public void RemovePCBPartByPCBNos(IList<string> pcbNoList)
       {
           try
           {
               MethodBase mthObj = MethodBase.GetCurrentMethod();
               SQLContextNew sqlCtx = null;
               int tk = mthObj.MetadataToken;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       PCBPartInfo condition = new PCBPartInfo {  PCBNo = string.Empty };
                       Pcb_Part incond = FuncNew.SetColumnFromField<Pcb_Part, PCBPartInfo>(condition);

                       sqlCtx = FuncNew.GetConditionedDelete<Pcb_Part>(tk, new ConditionCollection<Pcb_Part>(new InSetCondition<Pcb_Part>(incond)));
                   }
               }
               string sentence = sqlCtx.Sentence.Replace(g.DecInSet(Pcb_Part.fn_pcbno), g.ConvertInSet(pcbNoList));

               _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                              CommandType.Text,
                                                                              sentence,
                                                                              sqlCtx.Params);

           }
           catch (Exception)
           {
               throw;
           }

       }
       public void RemovePCBPartByPCBNosDefered(IUnitOfWork uow, IList<string> pcbNoList)
       {
           Action deferAction = () => { RemovePCBPartByPCBNos(pcbNoList); };
           AddOneInvokeBody(uow, deferAction);
       }

       public void InsertPCBPart(IList<PCBPartInfo> pcbPartList)
       {
           try
           {
               _Schema.BulkCopyHelper.BulkCopyToDatabase(pcbPartList, _Schema.SqlHelper.ConnectionString_PCA, "PCB_Part");
           }
           catch (Exception)
           {
               throw;
           }
       }

       public void InsertPCBPartDefered(IUnitOfWork uow, IList<PCBPartInfo> pcbPartList)
       {
           Action deferAction = () => { InsertPCBPart(pcbPartList); };
           AddOneInvokeBody(uow, deferAction);
       }

       public IList<PCBStatusExInfo> GetPCBPreStation(IList<string> pcbNoList)
       {
           try
           {
               IList<PCBStatusExInfo> ret = new List<PCBStatusExInfo>();
               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       sqlCtx = new SQLContextNew();
                       sqlCtx.Sentence = @"select PCBNo, PreStation, PreStatus, PreLine, PreTestFailCount, 
                                                                       PreEditor, PreUdt
                                                                from PCBStatusEx a
                                                                inner join @PCBNoList b on a.PCBNo=b.data ";
                       SqlParameter para = new SqlParameter("@PCBNoList", SqlDbType.Structured);
                       para.TypeName = "TbStringList";
                       para.Direction = ParameterDirection.Input;
                       sqlCtx.AddParam("PCBNoList", para);

                       SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               DataTable dt = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable(pcbNoList);

               sqlCtx.Param("PCBNoList").Value = dt;

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA,
                                                                                                                           CommandType.Text,
                                                                                                                           sqlCtx.Sentence,
                                                                                                                           sqlCtx.Params))
               {
                   while (sqlR != null && sqlR.Read())
                   {
                       PCBStatusExInfo item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<PCBStatusExInfo>(sqlR);                   
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
       #endregion
    }
}
