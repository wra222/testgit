using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.UnitOfWork;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using IMES.Infrastructure.Util;
using IMES.DataModel;
using schema=IMES.Infrastructure.Repository._Schema;
using mtns = IMES.Infrastructure.Repository._Metas;


namespace IMES.Infrastructure.Repository.PAK
{
    /// <summary>
    /// 数据访问与持久化类: PalletWeight相关
    /// </summary>
    public class PalletWeightRepository : BaseRepository<PalletWeight>, IPalletWeightRepository
    {
        #region Overrides of BaseRepository<PalletWeight>

        protected override void PersistNewItem(PalletWeight item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertPalletWeight(item);

                    //this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(PalletWeight item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdatePalletWeight(item);

                    //this.CheckAndInsertSubs(item, tracker);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(PalletWeight item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeletePalletWeight(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<PalletWeight>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override PalletWeight Find(object key)
        {
            try
            {
                PalletWeight ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PalletWeight cond = new _Schema.PalletWeight();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PalletWeight.fn_ID].Value = (int)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new PalletWeight();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Editor]);
                        ret.FamilyID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Family]);
                        ret.Id = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_ID]);
                        ret.Qty = GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Qty]);
                        ret.Region = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Region]);
                        ret.Tolerance = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Tolerance]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Udt]);
                        ret.Weight = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.PalletWeight.fn_Weight]);
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
        public override IList<PalletWeight> FindAll()
        {
            try
            {
                IList<PalletWeight> ret = new List<PalletWeight>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, null))
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

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        /// <param name="uow"></param>
        public override void Add(PalletWeight item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(PalletWeight item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(PalletWeight item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region IPalletWeightRepository Members

        public IList<PalletWeight> GetPltWeight(string family, string region, short qty)
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
                        cond.Region = region;
                        cond.Qty = qty;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PalletWeight), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PalletWeight.fn_Family].Value = family;
                sqlCtx.Params[_Schema.PalletWeight.fn_Region].Value = region;
                sqlCtx.Params[_Schema.PalletWeight.fn_Qty].Value = qty;
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
            catch(Exception)
            {
                throw;
            }
        }

        public decimal GetWeightOfFRUFISToSAPWeight(string shipment)
        {
            //select Weight from IMES_PAK..FRUFISToSAPWeight where Shipment=''
            try
            {
                decimal ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUFISToSAPWeight cond = new _Schema.FRUFISToSAPWeight();
                        cond.Shipment = shipment;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUFISToSAPWeight), null, new List<string>() { _Schema.FRUFISToSAPWeight.fn_Weight }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Shipment].Value = shipment;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Decimal(sqlR, sqlCtx.Indexes[_Schema.FRUFISToSAPWeight.fn_Weight]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddOrModifyFRUFISToSAPWeight(IMES.DataModel.FRUFISToSAPWeight item)
        {
            try
            {
                SqlTransactionManager.Begin();

                if (PeekFRUFISToSAPWeight(item))
                    UpdateFRUFISToSAPWeight(item);
                else
                    InsertFRUFISToSAPWeight(item);

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

        private bool PeekFRUFISToSAPWeight(IMES.DataModel.FRUFISToSAPWeight item)
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
                        _Schema.FRUFISToSAPWeight cond = new _Schema.FRUFISToSAPWeight();
                        cond.Shipment = item.Shipment;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUFISToSAPWeight), "COUNT", new List<string>() { _Schema.FRUFISToSAPWeight.fn_Shipment }, cond, null, null, null, null, null, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");
                    }
                }
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Shipment].Value = item.Shipment;
                //using (SqlDataReader 

                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
                //)
                //{
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                        ret = cnt > 0 ? true : false;
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

        private void UpdateFRUFISToSAPWeight(IMES.DataModel.FRUFISToSAPWeight item)
        {
            //update IMES_PAK..FRUFISToSAPWeight Set Weight='',Cdt=getdate() where Shipment=''
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.FRUFISToSAPWeight cond = new _Schema.FRUFISToSAPWeight();
                        cond.Shipment = item.Shipment;
                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUFISToSAPWeight), new List<string>() { _Schema.FRUFISToSAPWeight.fn_Cdt, _Schema.FRUFISToSAPWeight.fn_Status, _Schema.FRUFISToSAPWeight.fn_Type, _Schema.FRUFISToSAPWeight.fn_Weight }, null, null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Shipment].Value = item.Shipment;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUFISToSAPWeight.fn_Cdt)].Value = cmDt;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUFISToSAPWeight.fn_Status)].Value = item.Status;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUFISToSAPWeight.fn_Type)].Value = item.Type;
                sqlCtx.Params[_Schema.Func.DecSV(_Schema.FRUFISToSAPWeight.fn_Weight)].Value = item.Weight;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertFRUFISToSAPWeight(IMES.DataModel.FRUFISToSAPWeight item)
        {
            //insert into IMES_PAK..FRUWeightLog (SN,Weight,Line,Station,Editor,Cdt) Values('','','','','',getdate())
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.FRUFISToSAPWeight));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Shipment].Value = item.Shipment;
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Status].Value = item.Status;
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Type].Value = item.Type;
                sqlCtx.Params[_Schema.FRUFISToSAPWeight.fn_Weight].Value = item.Weight;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PalletWeightInfo> GetPltWeightByCondition(PalletWeightInfo condition)
        {
            try
            {
                IList<PalletWeightInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::PalletWeight_NEW cond = mtns::FuncNew.SetColumnFromField<mtns::PalletWeight_NEW, PalletWeightInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::PalletWeight_NEW>(null, null, new mtns::ConditionCollection<mtns::PalletWeight_NEW>(new mtns::EqualCondition<mtns::PalletWeight_NEW>(cond)), mtns::PalletWeight_NEW.fn_cdt);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PalletWeight_NEW, PalletWeightInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::PalletWeight_NEW, PalletWeightInfo, PalletWeightInfo>(ret, sqlR, sqlCtx);
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

        private void PersistInsertPalletWeight(PalletWeight item)
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
                sqlCtx.Params[_Schema.PalletWeight.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PalletWeight.fn_Family].Value = item.FamilyID;
                //sqlCtx.Params[_Schema.PalletWeight.fn_ID].Value = item.Id;
                sqlCtx.Params[_Schema.PalletWeight.fn_Qty].Value = item.Qty;
                sqlCtx.Params[_Schema.PalletWeight.fn_Region].Value = item.Region;
                sqlCtx.Params[_Schema.PalletWeight.fn_Tolerance].Value = item.Tolerance;
                sqlCtx.Params[_Schema.PalletWeight.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletWeight.fn_Weight].Value = item.Weight;
                item.Id = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdatePalletWeight(PalletWeight item)
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
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PalletWeight.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PalletWeight.fn_Family].Value = item.FamilyID;
                sqlCtx.Params[_Schema.PalletWeight.fn_ID].Value = item.Id;
                sqlCtx.Params[_Schema.PalletWeight.fn_Qty].Value = item.Qty;
                sqlCtx.Params[_Schema.PalletWeight.fn_Region].Value = item.Region;
                sqlCtx.Params[_Schema.PalletWeight.fn_Tolerance].Value = item.Tolerance;
                sqlCtx.Params[_Schema.PalletWeight.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PalletWeight.fn_Weight].Value = item.Weight;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeletePalletWeight(PalletWeight item)
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
                sqlCtx.Params[_Schema.PalletWeight.fn_ID].Value = item.Id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Defered

        public void AddOrModifyFRUWeightLogDefered(IUnitOfWork uow, IMES.DataModel.FRUFISToSAPWeight item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        #endregion

        #region FISTOSAP_WEIGHT table
        public IList<FisToSapWeightDef> ExistsFisToSapWeightByShipment(string DBconnectionStr,IList<string> shipmentList)
        {
            try
            {
                IList<FisToSapWeightDef> ret = new List<FisToSapWeightDef>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!mtns.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new mtns.SQLContextNew();
                        sqlCtx.Sentence = @" select [DN/Shipment] as Shipment,
                                                                        Status as ShipType,
                                                                        KG as Weight,
                                                                        SendStatus as SendStatus 
                                                              from FISTOSAP_WEIGHT
                                                             where [DN/Shipment] in (select data from @data)
                                                             order by Shipment, Cdt desc";
                        SqlParameter para1 = new SqlParameter("@data", SqlDbType.Structured);
                        para1.TypeName = "TbStringList";
                        sqlCtx.AddParam("ShipmentList", para1);
                        mtns.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipmentList").Value = schema.SQLData.ToDataTable(shipmentList);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(DBconnectionStr,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null )
                    {
                        while (sqlR.Read())
                        {
                            FisToSapWeightDef item = _Schema.SQLData.ToObject<FisToSapWeightDef>(sqlR);
                            //_Schema.SQLData.TrimStringProperties(item);
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
        public void InsertFisToSapWeight(string DBconnectionStr,FisToSapWeightDef shipWeight)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!mtns.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new mtns.SQLContextNew();
                        sqlCtx.Sentence = @"insert FISTOSAP_WEIGHT([DN/Shipment], Status, KG, Cdt)
                                                            values(@Shipment, @ShipType, @Weight, GETDATE())";
                        sqlCtx.AddParam("Shipment", new SqlParameter("@Shipment", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipType", new SqlParameter("@ShipType", SqlDbType.VarChar));
                        sqlCtx.AddParam("Weight", new SqlParameter("@Weight", SqlDbType.Decimal));
                        

                        mtns.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Shipment").Value = shipWeight.Shipment;
                sqlCtx.Param("ShipType").Value = shipWeight.ShipType;
                sqlCtx.Param("Weight").Value = shipWeight.Weight;
                _Schema.SqlHelper.ExecuteNonQuery(DBconnectionStr,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertFisToSapWeightDefered(IUnitOfWork uow, string DBconnectionStr,FisToSapWeightDef shipWeight)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), DBconnectionStr, shipWeight);
        }
        #endregion
    }
}
