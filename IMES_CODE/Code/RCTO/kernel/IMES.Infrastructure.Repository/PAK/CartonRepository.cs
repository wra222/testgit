using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.FisObjectBase;
using CartonNs=IMES.FisObject.PAK.Carton;
using IMES.Infrastructure.Util;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Metas;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;


namespace IMES.Infrastructure.Repository.PAK
{
    public class CartonRepository : BaseRepository<CartonNs.Carton>, CartonNs.ICartonRepository
    {
        #region Overrides of BaseRepository<Carton>

        protected override void PersistNewItem(CartonNs.Carton item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertCarton(item);

                    CartonNs.CartonStatus cartonStatus = (CartonNs.CartonStatus)item.GetType().GetField("_cartonStatus", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    //if (cartonStatus != null && cartonStatus.Tracker.GetState(cartonStatus)== DataRowState.Added)
                    //    this.PersistInsertCartonStatus(item.CartonSN, cartonStatus);
                    if (cartonStatus != null)
                    {
                        switch (cartonStatus.Tracker.GetState(cartonStatus))
                        {
                            case DataRowState.Added:
                                this.PersistInsertCartonStatus(item.CartonSN, cartonStatus);
                                break;
                            case DataRowState.Modified:
                                this.PersistUpdateCartonStatus(item.CartonSN, cartonStatus);
                                break;
                            default:
                                break;
                        }
                    }

                    IList<CartonNs.CartonInfo> cartonInfos = (IList<CartonNs.CartonInfo>)item.GetType().GetField("_cartonInfoList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    //if (cartonInfos != null && cartonInfos.Count > 0)
                    //{
                    //    foreach (CartonNs.CartonInfo info in cartonInfos)
                    //    {
                    //        if (info.Tracker.GetState(info) == DataRowState.Added)
                    //        {
                    //            this.PersistInsertCartonInfo(item.CartonSN, info);
                    //        }
                    //    }
                    //}
                    if (cartonInfos != null && cartonInfos.Count > 0)
                    {
                        foreach (CartonNs.CartonInfo info in cartonInfos)
                        {
                            switch (info.Tracker.GetState(info))
                            {
                                case DataRowState.Added:
                                    this.PersistInsertCartonInfo(item.CartonSN, info);
                                    break;
                                case DataRowState.Modified:
                                    this.PersistUpdateCartonInfo(item.CartonSN, info);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    IList<CartonNs.CartonLog> cartonLogs = (IList<CartonNs.CartonLog>)item.GetType().GetField("_cartonLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (cartonLogs != null && cartonLogs.Count > 0)
                    {
                        foreach (CartonNs.CartonLog info in cartonLogs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertCartonLog(item.CartonSN, info);
                            }
                        }
                    }


                    IList<CartonNs.CartonQCLog> cartonQCLogs = (IList<CartonNs.CartonQCLog>)item.GetType().GetField("_cartonQCLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (cartonQCLogs != null && cartonQCLogs.Count > 0)
                    {
                        foreach (CartonNs.CartonQCLog info in cartonQCLogs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertCartonQCLog(item.CartonSN, info);
                            }
                        }
                    }

                    IList<CartonNs.DeliveryCarton> deliveryCartons = (IList<CartonNs.DeliveryCarton>)item.GetType().GetField("_deliveryCartonList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (deliveryCartons != null && deliveryCartons.Count > 0)
                    {
                        foreach (CartonNs.DeliveryCarton info in deliveryCartons)
                        {
                            switch (info.Tracker.GetState(info))
                            {
                                case DataRowState.Added:
                                    this.PersistInsertDeliveryCarton(item.CartonSN, info);
                                    break;
                                case DataRowState.Modified:
                                    this.PersistUpdateDeliveryCarton(item.CartonSN, info);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(CartonNs.Carton item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified || item.GetRelationTableState() == DataRowState.Modified)
                {
                    if (tracker.GetState(item) == DataRowState.Modified)
                    {
                        this.PersistUpdateCarton(item);
                    }

                    CartonNs.CartonStatus cartonStatus = (CartonNs.CartonStatus)item.GetType().GetField("_cartonStatus", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (cartonStatus != null )
                    {   switch (cartonStatus.Tracker.GetState(cartonStatus))
                        {    
                        case DataRowState.Added:
                                    this.PersistInsertCartonStatus(item.CartonSN, cartonStatus);
                                    break;
                        case DataRowState.Modified:
                                this.PersistUpdateCartonStatus(item.CartonSN, cartonStatus);
                                break;
                        default:
                                break;
                        }
                    }

                    IList<CartonNs.CartonInfo> cartonInfos = (IList<CartonNs.CartonInfo>)item.GetType().GetField("_cartonInfoList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (cartonInfos != null && cartonInfos.Count > 0)
                    {
                        foreach (CartonNs.CartonInfo info in cartonInfos)
                        {
                             switch (info.Tracker.GetState(info)) 
                            {
                                 case DataRowState.Added:
                                        this.PersistInsertCartonInfo(item.CartonSN, info);
                                        break;
                                 case DataRowState.Modified:
                                        this.PersistUpdateCartonInfo(item.CartonSN, info);
                                        break;
                                 default:
                                        break;
                            }
                        }
                    }

                    IList<CartonNs.DeliveryCarton> deliveryCartons = (IList<CartonNs.DeliveryCarton>)item.GetType().GetField("_deliveryCartonList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (deliveryCartons != null && deliveryCartons.Count > 0)
                    {
                        foreach (CartonNs.DeliveryCarton info in deliveryCartons)
                        {
                            switch (info.Tracker.GetState(info))
                            {
                                case DataRowState.Added:
                                    this.PersistInsertDeliveryCarton(item.CartonSN, info);
                                    break;
                                case DataRowState.Modified:
                                    this.PersistUpdateDeliveryCarton(item.CartonSN, info);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }


                    IList<CartonNs.CartonLog> cartonLogs = (IList<CartonNs.CartonLog>)item.GetType().GetField("_cartonLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (cartonLogs != null && cartonLogs.Count > 0)
                    {
                        foreach (CartonNs.CartonLog info in cartonLogs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertCartonLog(item.CartonSN, info);
                            }
                        }
                    }


                    IList<CartonNs.CartonQCLog> cartonQCLogs = (IList<CartonNs.CartonQCLog>)item.GetType().GetField("_cartonQCLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (cartonQCLogs != null && cartonQCLogs.Count > 0)
                    {
                        foreach (CartonNs.CartonQCLog info in cartonQCLogs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertCartonQCLog(item.CartonSN, info);
                            }
                        }
                    }

                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(CartonNs.Carton item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {

                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Carton>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override CartonNs.Carton Find(object key)
        {
            try
            {
                CartonNs.Carton ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select CartonSN, PalletNo, Model, BoxId, PAQCStatus, 
                                                                        Weight, DNQty, Qty, FullQty, Status, 
                                                                        PreStation, PreStationStatus, Editor, Cdt, Udt 
                                                                from Carton
                                                                where CartonSN= @cartonSN";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonSN").Value = (string)key;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObjectWithAttribute<CartonNs.Carton>(sqlR);
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
        public override IList<CartonNs.Carton> FindAll()
        {
            try
            {
                IList<CartonNs.Carton> ret = new List<CartonNs.Carton>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select CartonSN, PalletNo, Model, BoxId, PAQCStatus, 
                                                                        Weight, DNQty, Qty, FullQty, Status, 
                                                                        PreStation, PreStationStatus, Editor, Cdt, Udt 
                                                                from Carton";
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        CartonNs.Carton item = SQLData.ToObjectWithAttribute<CartonNs.Carton>(sqlR);
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
        public override void Add(CartonNs.Carton item, IUnitOfWork uow)
        {
            base.Add(item, uow);           
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(CartonNs.Carton item, IUnitOfWork uow)
        {
            //base.Remove(item, uow); 
            throw new Exception("Not Allow Delete Data");
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(CartonNs.Carton item, IUnitOfWork uow)
        {
            base.Update(item, uow);
          
        }



        #endregion

        #region . Inners .
        #region insert table
        private void PersistInsertCarton(CartonNs.Carton item)
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
                        sqlCtx.Sentence = @"insert Carton(CartonSN, PalletNo, Model, BoxId, PAQCStatus, 
                                                                                  Weight, DNQty, Qty, FullQty, Status, 
                                                                                  PreStation, PreStationStatus, UnPackPalletNo, Editor, Cdt, 
                                                                                  Udt)
                                                                    values(@cartonSN, @palletNo, @model, @boxId, @paqcStatus, 
                                                                            @weight, @dnQty, @qty, @fullQty, @status, 
                                                                            @preStation, @preStationStatus,@UnPackPalletNo, @editor, GETDATE(), 
                                                                            GETDATE()) ";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("palletNo", new SqlParameter("@palletNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("boxId", new SqlParameter("@boxId", SqlDbType.VarChar));
                        sqlCtx.AddParam("paqcStatus", new SqlParameter("@paqcStatus", SqlDbType.VarChar));

                        sqlCtx.AddParam("weight", new SqlParameter("@weight", SqlDbType.Decimal));
                        sqlCtx.AddParam("dnQty", new SqlParameter("@dnQty", SqlDbType.Int));
                        sqlCtx.AddParam("qty", new SqlParameter("@qty", SqlDbType.Int));
                        sqlCtx.AddParam("fullQty", new SqlParameter("@fullQty", SqlDbType.Int));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));

                        sqlCtx.AddParam("preStation", new SqlParameter("@preStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("preStationStatus", new SqlParameter("@preStationStatus", SqlDbType.Int));
                        sqlCtx.AddParam("UnPackPalletNo", new SqlParameter("@UnPackPalletNo", SqlDbType.VarChar));
                        
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonSN").Value = item.CartonSN;
                sqlCtx.Param("palletNo").Value = item.PalletNo;
                sqlCtx.Param("model").Value = item.Model;
                sqlCtx.Param("boxId").Value = item.BoxId;
                sqlCtx.Param("paqcStatus").Value = item.PAQCStatus;

                sqlCtx.Param("weight").Value = item.Weight;
                sqlCtx.Param("dnQty").Value = item.DNQty;
                sqlCtx.Param("qty").Value = item.Qty;
                sqlCtx.Param("fullQty").Value = item.FullQty;
                sqlCtx.Param("status").Value = item.Status.ToString();

                sqlCtx.Param("preStation").Value = item.PreStation;
                sqlCtx.Param("preStationStatus").Value = item.PreStationStatus;
                sqlCtx.Param("UnPackPalletNo").Value = item.UnPackPalletNo;
                
                sqlCtx.Param("editor").Value = item.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void PersistInsertCartonStatus(string cartonSN, CartonNs.CartonStatus item)
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
                        sqlCtx.Sentence = @"insert CartonStatus( CartonNo, Station, Status, Line, Editor, 
				                                                                              Cdt, Udt)
                                                                            values(	@cartonNo, @station, @status, @line, @editor,
                                                                                         GETDATE(),GETDATE()) ";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("station", new SqlParameter("@station", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.Int));
                        sqlCtx.AddParam("line", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));
                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = cartonSN;
                sqlCtx.Param("station").Value = item.Station;
                sqlCtx.Param("status").Value = item.Status;
                sqlCtx.Param("line").Value = item.Line;
                sqlCtx.Param("editor").Value = item.Editor;
                item.CartonNo = cartonSN;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertCartonInfo(string cartonSN, CartonNs.CartonInfo item)
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
                        sqlCtx.Sentence = @"insert CartonInfo(CartonNo, InfoType, InfoValue, Editor, Cdt, 
                                                                                          Udt)
                                                                        values(@cartonNo, @infoType, @infoValue, @editor, GETDATE(), 
                                                                               GETDATE())";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("infoType", new SqlParameter("@infoType", SqlDbType.VarChar));
                       sqlCtx.AddParam("infoValue", new SqlParameter("@infoValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = cartonSN;
                sqlCtx.Param("infoType").Value = item.InfoType;
                sqlCtx.Param("infoValue").Value = item.InfoValue;
                sqlCtx.Param("editor").Value = item.Editor;
                item.CartonNo = cartonSN;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistInsertCartonLog(string cartonSN, CartonNs.CartonLog item)
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
                        sqlCtx.Sentence = @"insert CartonLog(CartonNo, Station, Status, Line, Editor, 
                                                                                        Cdt)
                                                                        values(@cartonNo, @station, @status,@line,@editor, 
                                                                                      GETDATE())";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("station", new SqlParameter("@station", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("line", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = cartonSN;
                sqlCtx.Param("station").Value = item.Station;
                sqlCtx.Param("status").Value = item.Status;
                sqlCtx.Param("line").Value = item.Line;
                sqlCtx.Param("editor").Value = item.Editor;
                item.CartonNo = cartonSN;

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
        private void PersistInsertCartonQCLog(string cartonSN, CartonNs.CartonQCLog item)
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
                        sqlCtx.Sentence = @"insert CartonQCLog(CartonSN, Model, Line, Type, Status, 
                                                                                              Remark, Editor, Cdt)
                                                                        values(@cartonSN, @model, @line,@type,@status,
                                                                                    @remark,@editor, GETDATE())";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("line", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("type", new SqlParameter("@type", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("remark", new SqlParameter("@remark", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonSN").Value = cartonSN;
                sqlCtx.Param("model").Value = item.Model;
                sqlCtx.Param("line").Value = item.Line;
                sqlCtx.Param("type").Value = item.Type;
                sqlCtx.Param("status").Value = (new CartonNs.CastCartonQCStatus()).ConvertTo(item.Status,typeof(string));
                sqlCtx.Param("remark").Value = item.Remark;
                sqlCtx.Param("editor").Value = item.Editor;
                item.CartonSN = cartonSN;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void PersistInsertDeliveryCarton(string cartonSN, CartonNs.DeliveryCarton item)
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
                        sqlCtx.Sentence = @"insert Delivery_Carton(DeliveryNo, CartonSN, Model, Qty, AssignQty, 
                                                                                                  Editor, Cdt, Udt)
                                                                        values(@deliveryNo, @cartonSN, @model, @qty,@assignQty,
                                                                                    @editor, GETDATE(), getdate())";
                        sqlCtx.AddParam("deliveryNo", new SqlParameter("@deliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("qty", new SqlParameter("@qty", SqlDbType.Int));
                        sqlCtx.AddParam("assignQty", new SqlParameter("@assignQty", SqlDbType.Int));
                        //sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("deliveryNo").Value = item.DeliveryNo;
                sqlCtx.Param("cartonSN").Value = cartonSN;
                sqlCtx.Param("model").Value = item.Model;
                sqlCtx.Param("qty").Value = item.Qty;
                sqlCtx.Param("assignQty").Value = item.AssignQty;
                //sqlCtx.Param("status").Value = item.Status.ToString();            
                sqlCtx.Param("editor").Value = item.Editor;
                item.CartonSN = cartonSN;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
                item.Tracker.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region update table
        private void PersistUpdateCarton(CartonNs.Carton item)
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
                        sqlCtx.Sentence = @"Update Carton
                                                            set PalletNo=@palletNo, Model=@model, BoxId=@boxId, PAQCStatus=@paqcStatus, 
                                                                                  Weight=@weight, DNQty=@dnQty, Qty=@qty, FullQty=@fullQty, Status=@status, 
                                                                                  PreStation=@preStation, PreStationStatus=@preStationStatus,UnPackPalletNo=@UnPackPalletNo, Editor=@editor, 
                                                                                    Udt =getdate()
                                                             where CartonSN=@cartonSN";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("palletNo", new SqlParameter("@palletNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("boxId", new SqlParameter("@boxId", SqlDbType.VarChar));
                        sqlCtx.AddParam("paqcStatus", new SqlParameter("@paqcStatus", SqlDbType.VarChar));

                        sqlCtx.AddParam("weight", new SqlParameter("@weight", SqlDbType.Decimal));
                        sqlCtx.AddParam("dnQty", new SqlParameter("@dnQty", SqlDbType.Int));
                        sqlCtx.AddParam("qty", new SqlParameter("@qty", SqlDbType.Int));
                        sqlCtx.AddParam("fullQty", new SqlParameter("@fullQty", SqlDbType.Int));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));

                        sqlCtx.AddParam("preStation", new SqlParameter("@preStation", SqlDbType.VarChar));
                        sqlCtx.AddParam("preStationStatus", new SqlParameter("@preStationStatus", SqlDbType.Int));
                        sqlCtx.AddParam("UnPackPalletNo", new SqlParameter("@UnPackPalletNo", SqlDbType.VarChar));
                        
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonSN").Value = item.CartonSN;
                sqlCtx.Param("palletNo").Value = item.PalletNo;
                sqlCtx.Param("model").Value = item.Model;
                sqlCtx.Param("boxId").Value = item.BoxId;
                sqlCtx.Param("paqcStatus").Value = item.PAQCStatus;

                sqlCtx.Param("weight").Value = item.Weight;
                sqlCtx.Param("dnQty").Value = item.DNQty;
                sqlCtx.Param("qty").Value = item.Qty;
                sqlCtx.Param("fullQty").Value = item.FullQty;
                sqlCtx.Param("status").Value = item.Status.ToString();

                sqlCtx.Param("preStation").Value = item.PreStation;
                sqlCtx.Param("preStationStatus").Value = item.PreStationStatus;
                sqlCtx.Param("UnPackPalletNo").Value = item.UnPackPalletNo;

                sqlCtx.Param("editor").Value = item.Editor;
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
        private void PersistUpdateCartonStatus(string cartonSN, CartonNs.CartonStatus item)
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
                        sqlCtx.Sentence = @"Update CartonStatus
                                                             set        Station=@station, 
                                                                          Status=@status, 
                                                                          Line = @line, 
                                                                          Editor=@editor, 
				                                                             Udt = getdate()
                                                            where CartonNo=@cartonNo";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("station", new SqlParameter("@station", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.Int));
                        sqlCtx.AddParam("line", new SqlParameter("@line", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = cartonSN;
                sqlCtx.Param("station").Value = item.Station;
                sqlCtx.Param("status").Value = item.Status;
                sqlCtx.Param("line").Value = item.Line;
                sqlCtx.Param("editor").Value = item.Editor;
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
        private void PersistUpdateCartonInfo(string cartonSN, CartonNs.CartonInfo item)
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
                        sqlCtx.Sentence = @"Update CartonInfo
                                                            set   InfoValue=@infoValue, 
                                                                    Editor=@editor,  
                                                                    Udt =getdate()
                                                            where CartonNo= @cartonNo and
                                                                       InfoType=@infoType";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("infoType", new SqlParameter("@infoType", SqlDbType.VarChar));
                        sqlCtx.AddParam("infoValue", new SqlParameter("@infoValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = cartonSN;
                sqlCtx.Param("infoType").Value = item.InfoType;
                sqlCtx.Param("infoValue").Value = item.InfoValue;
                sqlCtx.Param("editor").Value = item.Editor;               
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
        private void PersistUpdateDeliveryCarton(string cartonSN, CartonNs.DeliveryCarton item)
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
                        sqlCtx.Sentence = @"Update Delivery_Carton
                                                                  set   Model=@model,
                                                                          Qty =@qty, 
                                                                          AssignQty =@assignQty, 
                                                                         Editor=@editor,
                                                                         Udt=getdate()
                                                             where DeliveryNo =  @deliveryNo and
                                                                        CartonSN   =  @cartonSN ";
                        sqlCtx.AddParam("deliveryNo", new SqlParameter("@deliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("qty", new SqlParameter("@qty", SqlDbType.Int));
                        sqlCtx.AddParam("assignQty", new SqlParameter("@assignQty", SqlDbType.Int));
                        //sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("deliveryNo").Value = item.DeliveryNo;
                sqlCtx.Param("cartonSN").Value = cartonSN;
                sqlCtx.Param("model").Value = item.Model;
                sqlCtx.Param("qty").Value = item.Qty;
                sqlCtx.Param("assignQty").Value = item.AssignQty;
                //sqlCtx.Param("status").Value = item.Status.ToString();
                sqlCtx.Param("editor").Value = item.Editor;
               

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
        #endregion


      
        #endregion

        #region implementation ICartonRepository

        #region Fill Data
        public void FillCurrentStation(CartonNs.Carton carton)
        {
            try
            {
                CartonNs.CartonStatus ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select CartonNo, Station, Status, Line, Editor, 
                                                                       Cdt, Udt
                                                                from CartonStatus
                                                                where CartonNo = @cartonNo";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = carton.CartonSN;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObject<CartonNs.CartonStatus>(sqlR);
                        ret.Tracker.Clear();

                    }

                }
                carton.GetType().GetField("_cartonStatus", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillCartonInfo(CartonNs.Carton carton)
        {
            try
            {
                IList<CartonNs.CartonInfo> ret = new List< IMES.FisObject.PAK.Carton.CartonInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, CartonNo, InfoType, InfoValue, Editor, Cdt, Udt
                                                              from CartonInfo
                                                              where CartonNo= @cartonNo ";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = carton.CartonSN;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        CartonNs.CartonInfo item = SQLData.ToObject<CartonNs.CartonInfo>(sqlR);
                        item.Tracker.Clear();
                        ret.Add(item);

                    }

                }
                carton.GetType().GetField("_cartonInfoList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillCartonQCLog(CartonNs.Carton carton)
        {
            try
            {
                IList<CartonNs.CartonQCLog> ret = new List<CartonNs.CartonQCLog>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, CartonSN, Model, Line, Type, Status, 
                                                                        Remark, Editor, Cdt 
                                                                 from CartonQCLog
                                                                 where  CartonSN = @cartonSN ";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonSN").Value = carton.CartonSN;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        CartonNs.CartonQCLog item = SQLData.ToObjectWithAttribute<CartonNs.CartonQCLog>(sqlR);
                        item.Tracker.Clear();
                        ret.Add(item);

                    }

                }
                carton.GetType().GetField("_cartonQCLogList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }


        public void FillCartonLog(CartonNs.Carton carton)
        {
            try
            {
                IList<CartonNs.CartonLog> ret = new List<CartonNs.CartonLog>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, CartonNo, Station, Status, Line, 
                                                                        Editor, Cdt
                                                                 from CartonLog
                                                                 where CartonNo = @cartonNo ";
                        sqlCtx.AddParam("cartonNo", new SqlParameter("@cartonNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonNo").Value = carton.CartonSN;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                   while(sqlR != null && sqlR.Read())
                    {
                        CartonNs.CartonLog item = SQLData.ToObject<CartonNs.CartonLog>(sqlR);
                        item.Tracker.Clear();
                        ret.Add(item);

                    }

                }
                carton.GetType().GetField("_cartonLogList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillDeliveryCarton(CartonNs.Carton carton)
        {
            try
            {
                IList<CartonNs.DeliveryCarton> ret = new List<CartonNs.DeliveryCarton>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, DeliveryNo, CartonSN, Model, Qty, 
                                                                         AssignQty, Editor, Cdt, Udt
                                                                 from Delivery_Carton
                                                                 where CartonSN =@cartonSN ";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonSN").Value = carton.CartonSN;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        CartonNs.DeliveryCarton item = SQLData.ToObject<CartonNs.DeliveryCarton>(sqlR);
                        item.Tracker.Clear();
                        ret.Add(item);

                    }

                }
                carton.GetType().GetField("_deliveryCartonList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillCartonProduct(CartonNs.Carton carton)
        {
             try
            {
                IList<CartonNs.CartonProduct> ret = new List<CartonNs.CartonProduct>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select b.ID, b.DeliveryNo, b.CartonSN, b.ProductID, b.Remark, 
                                                                       b.Editor, b.Cdt, b.Udt
                                                                 from  Carton_Product b
                                                                 where b.CartonSN =@cartonSN ";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("cartonSN").Value = carton.CartonSN;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        CartonNs.CartonProduct item = SQLData.ToReadOnlyObject<CartonNs.CartonProduct>(sqlR);
                        item.Tracker.Clear();
                        ret.Add(item);
                    }

                }
                carton.GetType().GetField("_cartonProductList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(carton, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region implement interface
        public IList<AvailableDelivery> GetAvailableDNList(string model, string dnStatus, int offsetShipdate)
        {
            try
            {
                IList<AvailableDelivery> ret = new List<AvailableDelivery>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"with temp
//                                                            as ( select  top 1 a.DeliveryNo,
//			                                                             a1.PalletNo, 
//			                                                             a.Model, 
//			                                                             b.QtyPerCarton as QtyPerCarton,                
//			                                                             b.CartonQty as DNCartonQty,
//			                                                             c1.CartonQty,   
//			                                                             a.Qty as DNQty,
//			                                                             b.ShipmentNo,				                                                            
//			                                                             a.ShipDate,
//			                                                             b.PalletType as PackCategory,
//			                                                             c1.RemainQty,
//			                                                             a1.DeviceQty as Qty,   
//			                                                             0 as Level                                                                
//	                                                            from Delivery a WITH (updlock)
//	                                                            inner join Delivery_Pallet a1 on a1.DeliveryNo = a.DeliveryNo
//	                                                            inner join DeliveryEx b on  a.DeliveryNo = b.DeliveryNo	 
//	                                                            cross apply dbo.fn_UnBindDNQtyByCartonPallet(a.DeliveryNo,a1.PalletNo, a1.DeviceQty, b.QtyPerCarton, a1.DeliveryQty) c1                                                                    
//	                                                            where a.Model=@model and
//		                                                              a.ShipDate >= @shipdate and
//		                                                              a.Status=@status and 
//		                                                              c1.RemainCartonQty>0 and
//		                                                              c1.RemainQty>0
//	                                                            order by  a.ShipDate, b.PalletType ,a1.PalletNo,a.DeliveryNo
//                                                            union all
//                                                            select  a.DeliveryNo,
//		                                                            b.PalletNo,
//		                                                            a.Model, 
//		                                                            b.QtyPerCarton,                
//		                                                            b.CartonQty as DNCartonQty,
//		                                                            c1.CartonQty,  
//		                                                            a.Qty as DNQty ,
//		                                                            f.ShipmentNo,
//		                                                            a.ShipDate,
//		                                                            f.PalletType as PackCategory,
//		                                                            c1.RemainQty,
//		                                                            a1.DeviceQty as Qty,
//		                                                            b.Level+1 as  Level                                                               
//                                                            from  temp b
//                                                            inner join DeliveryEx f on  f.DeliveryNo != b.DeliveryNo and
//							                                                            f.ShipmentNo = b.ShipmentNo                                                                 
//                                                            inner join Delivery a  WITH (updlock) on a.DeliveryNo =f.DeliveryNo
//                                                            inner join Delivery_Pallet a1 on a1.DeliveryNo = f.DeliveryNo and a1.PalletNo =b.PalletNo
//                                                            cross apply dbo.fn_UnBindDNQtyByCartonPallet(a.DeliveryNo,a1.PalletNo, a1.DeviceQty, f.QtyPerCarton, a1.DeliveryQty) c1                                                                    
//                                                            where a.Model=@model and
//	                                                              a.ShipDate >= @shipdate and
//	                                                              a.Status=@status and 
//	                                                              c1.RemainCartonQty>0     and
//	                                                              c1.RemainQty>0 and
//	                                                              b.Level =0                                                                                                       
//                                                            )
//                                                            select * from temp  
//                                                            order by  ShipDate,  PackCategory ,DeliveryNo  ";
                        sqlCtx.Sentence = @"select * from [dbo].[fn_GetAvailableDNList](@model,@status,@shipdate) 
                                                            order by DeliveryNo";

                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("shipdate", new SqlParameter("@shipdate", SqlDbType.DateTime));
                        //sqlCtx.AddParam("qtyPerCarton", new SqlParameter("@qtyPerCarton", SqlDbType.Int));



                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("model").Value = model;
                sqlCtx.Param("status").Value = dnStatus;
                DateTime now = DateTime.Now;
                DateTime shipdate = new DateTime(now.Year, now.Month, now.Day);
                shipdate = shipdate.AddDays(offsetShipdate);


                sqlCtx.Param("shipdate").Value = shipdate;
                //sqlCtx.Param("qtyPerCarton").Value = qtyPerCarton;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlCtx.Sentence,
                                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        AvailableDelivery item = SQLData.ToObject<AvailableDelivery>(sqlR);
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
        
        public IList<AvailableDelivery> GetAvailableDNListWithSingle(string model, string dnStatus, int offsetShipdate)
        {
            try
            {
                IList<AvailableDelivery> ret = new List<AvailableDelivery>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"with temp
//                                                             as ( select  top 1 a.DeliveryNo,
//                                                                             a1.PalletNo, 
//                                                                             a.Model, 
//                                                                             b.QtyPerCarton as QtyPerCarton,                
//                                                                             b.CartonQty as DNCartonQty,
//                                                                             c1.CartonQty,                                                                               
//                                                                             a.Qty as DNQty,
//                                                                             b.ShipmentNo,				                                                            
//                                                                             a.ShipDate,
//                                                                             b.PalletType as PackCategory,
//                                                                             c1.RemainQty,
//                                                                             a.Qty,   
//                                                                             0 as Level                                                                
//                                                                    from Delivery a WITH (updlock)
//                                                                    inner join Delivery_Pallet a1 on a1.DeliveryNo = a.DeliveryNo
//                                                                    inner join DeliveryEx b on  a.DeliveryNo = b.DeliveryNo                                                                   
//                                                                    cross apply dbo.fn_UnBindDNQtyByCartonPallet(a.DeliveryNo,a1.PalletNo, a.Qty, b.QtyPerCarton, a1.DeliveryQty) c1                                                                    
//                                                                    where a.Model=@model and
//                                                                          a.ShipDate >= @shipdate and
//                                                                          a.Status=@status and 
//                                                                          c1.RemainCartonQty>0 and
//                                                                          c1.RemainQty>0 
//                                                                    order by  a.ShipDate,b.PalletType,a1.PalletNo,a.DeliveryNo                                                                                                                                         
//                                                             )
//                                                             select * from temp ";
                        sqlCtx.Sentence = "select * from [dbo].[fn_GetAvailableSingleDN](@model,@status,@shipdate)";
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("shipdate", new SqlParameter("@shipdate", SqlDbType.DateTime));
                        //sqlCtx.AddParam("qtyPerCarton", new SqlParameter("@qtyPerCarton", SqlDbType.Int));



                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("model").Value = model;
                sqlCtx.Param("status").Value = dnStatus;
                DateTime now = DateTime.Now;
                DateTime shipdate = new DateTime(now.Year, now.Month, now.Day);
                shipdate = shipdate.AddDays(offsetShipdate);


                sqlCtx.Param("shipdate").Value = shipdate;
                //sqlCtx.Param("qtyPerCarton").Value = qtyPerCarton;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlCtx.Sentence,
                                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        AvailableDelivery item = SQLData.ToObject<AvailableDelivery>(sqlR);
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

        public IList<AvailableDelivery> GetAvailableDNListByPo(string model, string poNo, string dnStatus, int offsetShipdate)
        {
            try
            {
                IList<AvailableDelivery> ret = new List<AvailableDelivery>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                      
                        sqlCtx.Sentence = @"select * from [dbo].[fn_GetAvailableDNListByPo](@model,@poNo,@status,@shipdate) 
                                                            order by DeliveryNo";
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("poNo", new SqlParameter("@poNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("shipdate", new SqlParameter("@shipdate", SqlDbType.DateTime));
                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("model").Value = model;
                sqlCtx.Param("poNo").Value = poNo;
                sqlCtx.Param("status").Value = dnStatus;
                DateTime now = DateTime.Now;
                DateTime shipdate = new DateTime(now.Year, now.Month, now.Day);
                shipdate = shipdate.AddDays(offsetShipdate);


                sqlCtx.Param("shipdate").Value = shipdate;
                //sqlCtx.Param("qtyPerCarton").Value = qtyPerCarton;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlCtx.Sentence,
                                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        AvailableDelivery item = SQLData.ToObject<AvailableDelivery>(sqlR);
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


        public IList<AvailableDelivery> GetAvailableDNListWithSingleByPo(string model,string poNo, string dnStatus, int offsetShipdate)
        {
            try
            {
                IList<AvailableDelivery> ret = new List<AvailableDelivery>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = "select * from [dbo].[fn_GetAvailableSingleDNByPo](@model,@poNo,@status,@shipdate)";
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("poNo", new SqlParameter("@poNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("shipdate", new SqlParameter("@shipdate", SqlDbType.DateTime));
                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("model").Value = model;
                sqlCtx.Param("poNo").Value = poNo;
                sqlCtx.Param("status").Value = dnStatus;
                DateTime now = DateTime.Now;
                DateTime shipdate = new DateTime(now.Year, now.Month, now.Day);
                shipdate = shipdate.AddDays(offsetShipdate);

                sqlCtx.Param("shipdate").Value = shipdate;
                //sqlCtx.Param("qtyPerCarton").Value = qtyPerCarton;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlCtx.Sentence,
                                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        AvailableDelivery item = SQLData.ToObject<AvailableDelivery>(sqlR);
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

        public CartonNs.DeliveryCarton BindDeliveryCarton(string cartonSN, AvailableDelivery availableDN, string editor)
        {
            try
            {
                CartonNs.DeliveryCarton ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"insert Delivery_Carton(DeliveryNo, CartonSN, Model, Qty, AssignQty, 
                                                                                                  Editor, Cdt, Udt)
                                                             Output Inserted.*
                                                             values(@deliveryNo, @cartonSN, @model, @qty,@assignQty,
                                                                        @editor, GETDATE(), getdate())";
                        sqlCtx.AddParam("deliveryNo", new SqlParameter("@deliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.AddParam("qty", new SqlParameter("@qty", SqlDbType.Int));
                        sqlCtx.AddParam("assignQty", new SqlParameter("@assignQty", SqlDbType.Int));
                        //sqlCtx.AddParam("status", new SqlParameter("@status", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("deliveryNo").Value = availableDN.DeliveryNo;
                sqlCtx.Param("cartonSN").Value = cartonSN;
                sqlCtx.Param("model").Value = availableDN.Model;
                sqlCtx.Param("qty").Value = availableDN.CartonQty;
                sqlCtx.Param("assignQty").Value = 0;
                //sqlCtx.Param("status").Value = CartonNs.DeliveryCartonState.Reserve;
                sqlCtx.Param("editor").Value =editor;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlCtx.Sentence,
                                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObject<CartonNs.DeliveryCarton>(sqlR);                    

                    }

                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
            
        }


//        public void unBindDeliveryCarton(string cartonSN)
//        {
//            try
//            {
//                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new SQLContextNew();
//                        //sqlCtx.Sentence = @"delete from Delivery_Carton where CartonSN=@cartonSN";         
//                        sqlCtx.Sentence = @"update Delivery_Carton
//                                                            set Status = 'UnPack'
//                                                            where CartonSN=@cartonSN";         
//                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                       
//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
              
//                sqlCtx.Param("cartonSN").Value = cartonSN;               

//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                 CommandType.Text,
//                                                                                 sqlCtx.Sentence,
//                                                                                 sqlCtx.Params);
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public void unBindDeliveryCartonDefered(IUnitOfWork uow, string cartonSN)
//        {
//            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), cartonSN);
//        }

        public void unBindCarton(string cartonSN,string editor)
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
                        sqlCtx.Sentence = @"update Carton
                                                            set Status = 'UnPack',
                                                                   Editor = @Editor,
                                                                    Udt = getdate()
                                                            where CartonSN=@cartonSN";  
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("cartonSN").Value = cartonSN;
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
        public void unBindCartonDefered(IUnitOfWork uow, string cartonSN,string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), cartonSN,editor);
        }

        public void unBindCartonByDn(string dn,string editor)
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
                        sqlCtx.Sentence = @"update Carton
                                                            set Status='UnPack',
                                                                   Editor=@Editor,
                                                                    Udt=getdate() 
                                                            from Carton a inner join 
                                                                 Delivery_Carton b on a.CartonSN = b.CartonSN
                                                            where a.Status in ('Full','Partial') and 
                                                                       b.DeliveryNo =@DeliveryNo ";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = dn;
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
        public void unBindCartonByDnDefered(IUnitOfWork uow, string dn,string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn,editor);
        }

        public string BindPalletWithDN(string deliveryNo)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"with cte as( select b.PalletNo, b.DeliveryQty,
			                                                                            c.CartonQty,
                                                                                        d.DNQty  
		                                                                        from Delivery_Pallet b
                                                                                cross apply dbo.fn_GetDNQty(b.PalletNo) d
                                                                                cross apply dbo.fn_GetCartonQtyByDeliveryPallet(b.DeliveryNo,b.PalletNo) c
	                                                                           where b.DeliveryNo =@deliveryNo)
                                                             select top 1 PalletNo 
                                                             from cte 
                                                             where  DeliveryQty> CartonQty and
                                                                         DNQty>0  
                                                             order by DNQty,CartonQty desc, PalletNo";
                        sqlCtx.AddParam("deliveryNo", new SqlParameter("@deliveryNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("deliveryNo").Value = deliveryNo;
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlCtx.Sentence,
                                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetString(0).Trim();
                    }

                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string BindPalletWithDN(IList<string> deliveryNoList)
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
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"with cte as( select b.PalletNo, a.DeliveryQty,
			                                                                            c.CartonQty,
                                                                                        d.DNQty,
                                                                                        a.DeliveryNo  
		                                                                        from Delivery_Pallet b 
                                                                                inner join Delivery_Pallet a on a.PalletNo = b.PalletNo
                                                                                cross apply dbo.fn_GetDNQty(b.PalletNo) d
                                                                                cross apply dbo.fn_GetCartonQtyByDeliveryPallet(a.DeliveryNo, b.PalletNo) c
	                                                                           where b.DeliveryNo =@deliveryNo and
                                                                                         a.DeliveryNo in ({0})            
                                                                                          )
                                                            select top 1 a.PalletNo
                                                                 from (select PalletNo, 
                                                                              CartonQty=sum(CartonQty),
                                                                              DNQty=count(1) 
                                                                           from cte 
                                                                            where  DeliveryQty> CartonQty and
                                                                                   DNQty >=@dnQty  
                                                                            group by PalletNo) a
                                                                where  a.DNQty>=@dnQty
                                                                order by a.CartonQty desc, PalletNo";
                        sqlCtx.AddParam("deliveryNo", new SqlParameter("@deliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("dnQty", new SqlParameter("@dnQty", SqlDbType.Int));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("deliveryNo").Value = deliveryNoList[0];
                sqlCtx.Param("dnQty").Value = deliveryNoList.Count;


                string inCondStr = "'" + string.Join("','", deliveryNoList.ToArray<string>()) + "'";

               string sqlStr = string.Format( sqlCtx.Sentence,inCondStr);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlStr,
                                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = sqlR.GetString(0).Trim();
                    }

                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BindCartonProduct(string cartonSN, string productId, string dn,string remark, string editor)
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
                        sqlCtx.Sentence = @"merge Carton_Product as target
                                                            using (select @CartonSN ,
                                                                         @ProductID) as source(CartonSN, ProductID)
                                                            on (target.CartonSN =  source.CartonSN and
                                                                target.ProductID = source.ProductID)
                                                            when MATCHED then
                                                                 update set DeliveryNo =@DeliveryNo, 
                                                                            Remark = @Remark,
                                                                            Editor=@Editor,
                                                                            Udt=getdate()
                                                            WHEN NOT MATCHED then              
                                                                 insert( CartonSN, ProductID, DeliveryNo,Remark, Editor, 
                                                                                        Cdt, Udt)
                                                                 values(@CartonSN, @ProductID, @DeliveryNo,@Remark,@Editor,
                                                                         GETDATE(), getdate());";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("CartonSN", new SqlParameter("@CartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("ProductID", new SqlParameter("@ProductID", SqlDbType.VarChar));
                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DeliveryNo").Value = dn;
                sqlCtx.Param("CartonSN").Value = cartonSN;
                sqlCtx.Param("ProductID").Value = productId;
                sqlCtx.Param("Remark").Value = remark;              
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
        public void BindCartonProductDefered(IUnitOfWork uow, string cartonSN, string productId, string dn, string remark, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), cartonSN,productId,dn,remark,editor);
        }
        public void RollBackAssignCarton(string cartonSN,string editor)
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
                        sqlCtx.Sentence = @"update Carton
                                                            set Status = 'Abort',
                                                                   Editor=@Editor,
                                                                    Udt = getdate() 
                                                            where CartonSN=@cartonSN and
                                                                        Status = 'Reserve' ";
                        sqlCtx.AddParam("cartonSN", new SqlParameter("@cartonSN", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("cartonSN").Value = cartonSN;
                sqlCtx.Param("Editor").Value = cartonSN;
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
        public void RollBackAssignCartonDefered(IUnitOfWork uow, string cartonSN,string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), cartonSN,editor);
        }


        public IList<string> GetReserveCartonByProdId(string productId)
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
                        sqlCtx.Sentence = @"select  a.CartonSN
                                                                 from  Carton a  
                                                                 inner join Carton_Product b on a.CartonSN=b.CartonSN
                                                                 where  a.Status='Reserve' and  
                                                                             b.ProductID =@ProductID ";
                        sqlCtx.AddParam("ProductID", new SqlParameter("@ProductID", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ProductID").Value = productId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {                        
                        ret.Add(sqlR.GetString(0).Trim());
                    }

                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IList<string> GetCartonSNListByPalletNo(string palletNo, bool includeReserveStatus)
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
                        sqlCtx.Sentence = @"select CartonSN
                                                            from Carton 
                                                            where PalletNo=@PalletNo and
                                                                  Status in ({0}) ";
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;
                string sqlStr="";
                if (includeReserveStatus)
                {
                    sqlStr = string.Format(sqlCtx.Sentence, "'Reserve','Full','Partial'");
                }
                else
                {
                    sqlStr = string.Format(sqlCtx.Sentence, "'Full','Partial'");
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlStr,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(sqlR.GetString(0).Trim());
                    }

                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetCartonSNListByDeliveryNo(string deliveryNo, bool includeReserveStatus)
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
                        sqlCtx.Sentence = @"select distinct a.CartonSN
                                                            from Carton a 
                                                            inner join Delivery_Carton b on a.CartonSN = b.CartonSN    
                                                            where b.DeliveryNo =@DeliveryNo and
                                                                       a.Status in ({0}) ";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DeliveryNo").Value = deliveryNo;
                string sqlStr = "";
                if (includeReserveStatus)
                {
                    sqlStr = string.Format(sqlCtx.Sentence, "'Reserve','Full','Partial'");
                }
                else
                {
                    sqlStr = string.Format(sqlCtx.Sentence, "'Full','Partial'");
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlStr,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(sqlR.GetString(0).Trim());
                    }

                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveEdiPackingData(string boxId)
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
                        sqlCtx.Sentence = @"delete from PAK_PackkingData where BOX_ID=@BOX_ID";
                        sqlCtx.AddParam("BOX_ID", new SqlParameter("@BOX_ID", SqlDbType.VarChar));
                       

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("BOX_ID").Value = boxId;
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
        public void RemoveEdiPackingDataDefered(IUnitOfWork uow, string boxId)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), boxId);
        }
        public void RemoveEdiODMSession(IList<string> custSNList)
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
                        sqlCtx.Sentence = @"delete from  PAKODMSESSION where SERIAL_NUM in ({0})";
                       
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                string cond = "'" + string.Join("','", custSNList.ToArray()) + "'";
                string sqlStr = string.Format(sqlCtx.Sentence, cond);
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_HP_EDI,
                                                                                                                                            CommandType.Text,
                                                                                                                                           sqlStr,
                                                                                                                                            sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }

        }
        public void RemoveEdiODMSessionDefered(IUnitOfWork uow, IList<string> custSNList)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), custSNList);
        }

        public void ClearSnoIdInShipBoxDet(string cartonSN, string editor)
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
                                                                Editor=@Editor,
                                                                Udt =GETDATE()
                                                            where  SnoId=@SnoId ";
                        sqlCtx.AddParam("SnoId", new SqlParameter("@SnoId", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("SnoId").Value = cartonSN;
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
        public void ClearSnoIdInShipBoxDetDefered(IUnitOfWork uow, string cartonSN, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), cartonSN, editor);
        }

        public IList<CartonProductInfo> GetCartonProductByExcludeDN(string dn)
        {
            try
            {
                IList<CartonProductInfo> ret = new List<CartonProductInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.CartonSN , c.DeliveryNo, c.ProductID, a.Model, a.BoxId, a.PalletNo, a.Qty, a.FullQty, a.Status 
                                                                from Carton a 
                                                                inner join Delivery_Carton b on a.CartonSN = b.CartonSN 
                                                                inner join Carton_Product c on b.CartonSN = c.CartonSN 
                                                                where a.Status in ('Full','Partial')       and 
                                                                      b.DeliveryNo =@DeliveryNo     and
                                                                      c.DeliveryNo != @DeliveryNo ";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("DeliveryNo").Value = dn;
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        CartonProductInfo item = SQLData.ToObject<CartonProductInfo>(sqlR);
                        //SQLData.TrimStringProperties(item);
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

        public IList<IMES.FisObject.PAK.DN.Delivery> GetDeliveryByNoList(IList<string> dnNoList)
        {
            try
            {
                IList<IMES.FisObject.PAK.DN.Delivery> ret = new List<IMES.FisObject.PAK.DN.Delivery>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select DeliveryNo, ShipmentNo, PoNo, Model, ShipDate, Qty, Status, Editor, Cdt, Udt
                                                            from Delivery 
                                                            where DeliveryNo in({0}) ";
                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                string dnNoStr = "'" + string.Join("','", dnNoList.ToArray()) + "'";
                string sqlStr = string.Format(sqlCtx.Sentence, dnNoStr);


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlStr,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        IMES.FisObject.PAK.DN.Delivery item = SQLData.ToObject<IMES.FisObject.PAK.DN.Delivery>(sqlR);
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


        public void UpdateCartonPreStationByDn(string dn, string editor)
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
                        sqlCtx.Sentence = @"update Carton
                                                            set PreStation=c.Station,
                                                                    PreStationStatus=c.Status,
                                                                   Editor=@Editor,
                                                                    Udt=getdate() 
                                                            from Carton a
                                                            inner join CartonStatus c on  a.CartonSN = c.CartonNo
                                                             inner join Delivery_Carton b on a.CartonSN = b.CartonSN
                                                            where a.Status in ('Full','Partial') and 
                                                                       b.DeliveryNo =@DeliveryNo ";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = dn;
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
        public void UpdateCartonPreStationByDnDefered(IUnitOfWork uow, string dn, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn, editor);
        }

        public void UpdateCartonStatusByDn(string dn,string station, int status, string line, string editor)
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
                        sqlCtx.Sentence = @"update CartonStatus
                                                            set   Station=@Station,
                                                                    Status=@Status,
                                                                    Line=@Line,
                                                                    Editor=@Editor,
                                                                    Udt=getdate() 
                                                            from Carton a
                                                            inner join CartonStatus c on  a.CartonSN = c.CartonNo
                                                             inner join Delivery_Carton b on a.CartonSN = b.CartonSN
                                                            where a.Status in ('Full','Partial') and 
                                                                       b.DeliveryNo =@DeliveryNo ";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.Int));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = dn;
                sqlCtx.Param("Editor").Value = editor;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("Line").Value = line;
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
        public void UpdateCartonStatusByDnDefered(IUnitOfWork uow, string dn,string station, int status, string line, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn,station,status,line,editor);
        }

       
        
        public  void InsertCartonLogByDn(string dn, string station, int status, string line, string editor)
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
                        sqlCtx.Sentence = @"insert CartonLog(CartonNo, Station, Status, Line, Editor, Cdt)
                                                            select a.CartonSN,@Station,@Status,@Line,@Editor,GETDATE() 
                                                            from Carton a
                                                            inner join CartonStatus c on  a.CartonSN = c.CartonNo
                                                             inner join Delivery_Carton b on a.CartonSN = b.CartonSN
                                                            where a.Status in ('Full','Partial') and 
                                                                       b.DeliveryNo =@DeliveryNo ";
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.Int));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("DeliveryNo").Value = dn;
                sqlCtx.Param("Editor").Value = editor;
                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("Line").Value = line;
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
        public void InsertCartonLogByDnDefered(IUnitOfWork uow, string dn, string station, int status, string line, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), dn, station, status, line, editor);
        }

        public CartonNs.Carton GetCartonByBoxId(string boxId)
        {
            try
            {
                CartonNs.Carton ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select top 1 CartonSN, PalletNo, Model, BoxId, PAQCStatus, 
                                                                        Weight, DNQty, Qty, FullQty, Status, 
                                                                        PreStation, PreStationStatus,UnPackPalletNo, Editor, Cdt, 
                                                                        Udt 
                                                                from Carton
                                                                where BoxId= @BoxId and
                                                                            Status in('Partial','Full')
                                                                order by Udt desc";
                        sqlCtx.AddParam("BoxId", new SqlParameter("@BoxId", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("BoxId").Value = boxId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObjectWithAttribute<CartonNs.Carton>(sqlR);
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


        public CartonNs.Carton GetCartonByPalletNo(string palletNo)
        {
            try
            {
                CartonNs.Carton ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select top 1 CartonSN, PalletNo, Model, BoxId, PAQCStatus, 
                                                                        Weight, DNQty, Qty, FullQty, Status, 
                                                                        PreStation, PreStationStatus, UnPackPalletNo, Editor, Cdt,
                                                                        Udt 
                                                                from Carton
                                                                where PalletNo= @PalletNo and
                                                                            Status in('Partial','Full')
                                                                order by Udt desc";
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
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObjectWithAttribute<CartonNs.Carton>(sqlR);
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

        public int GetAssignedPalletCartonQty(string palletNo)
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
                        sqlCtx.Sentence = @"select count(distinct CartonSN) as Qty 
                                                                from Carton
                                                                where PalletNo= @PalletNo and
                                                                            Status in('Partial','Full') ";
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

//        public CartonNs.CartonProduct UpdateCartonAbortStatus(string productId, string newCartonSN,string editor)
//        {
//            try
//            {
//                CartonNs.CartonProduct ret = null;

//                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"update Carton           
//                                                            set  Status='Abort',
//                                                                    Editor = @Editor,
//                                                                    Udt =getdate()
//                                                            from Carton a
//                                                            inner join CartonProduct b on b.CartonSN = a.CartonSN
//                                                            where a.CartonSN !=@CartonSN and
//                                                                       a.Status = 'Reserve' and 
//                                                                       b.ProductId = @ProductId
//
//                                                            update Delivery_Carton           
//                                                            set  Status='Abort'
//                                                                   Editor = @Editor,
//                                                                    Udt =getdate()
//                                                            from Delivery_Carton a
//                                                            inner join CartonProduct b on b.CartonSN = a.CartonSN
//                                                            where a.CartonSN !=@CartonSN and
//                                                                       a.Status = 'Reserve' and
//                                                                       b.ProductId = @ProductId
//
//                                                            update CartonProduct
//                                                            set Remark='Abort'
//                                                                  Editor = @Editor,
//                                                                  Udt =getdate()
//                                                            output deleted.*  
//                                                            where CartonSN !=@CartonSN and
//                                                                       ProductId = @ProductId";

//                        sqlCtx.AddParam("CartonSN", new SqlParameter("@CartonSN", SqlDbType.VarChar));
//                        sqlCtx.AddParam("ProductId", new SqlParameter("@ProductId", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("CartonSN").Value = newCartonSN;
//                sqlCtx.Param("ProductId").Value = productId;
//                sqlCtx.Param("Editor").Value = editor;

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                                             CommandType.Text,
//                                                                                                                                              sqlCtx.Sentence,
//                                                                                                                                              sqlCtx.Params))
//                {
//                    if (sqlR != null && sqlR.Read())
//                    {
//                        ret = SQLData.ToReadOnlyObject<CartonNs.CartonProduct>(sqlR);
//                        ret.Tracker.Clear();
//                    }
//                }
//                return ret;

              
//            }
//            catch (Exception)
//            {
//                throw;
//            }
        //        }

        public IList<AssignedDeliveryPalletInfo> GetAssignedDeliveryPalletCartonQty(string palletNo, bool includeReserveStatus)
        {
            try
            {
                IList<AssignedDeliveryPalletInfo> ret = new List<AssignedDeliveryPalletInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.PalletNo, b.DeliveryNo, count(1) as CartonQty,
                                                                       ISNull(SUM(b.Qty),0) as DeviceQty 
                                                            from Carton a 
                                                            inner join Delivery_Carton b on a.CartonSN = b.CartonSN    
                                                            where a.PalletNo =@PalletNo and
                                                                       a.Status in ({0}) 
                                                            group by a.PalletNo, b.DeliveryNo";
                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("PalletNo").Value = palletNo;
                string sqlStr = "";
                if (includeReserveStatus)
                {
                    sqlStr = string.Format(sqlCtx.Sentence, "'Reserve','Full','Partial'");
                }
                else
                {
                    sqlStr = string.Format(sqlCtx.Sentence, "'Full','Partial'");
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlStr,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            AssignedDeliveryPalletInfo item = SQLData.ToObject < AssignedDeliveryPalletInfo>(sqlR);
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
        #endregion
    }
}
