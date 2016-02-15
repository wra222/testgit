using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Material;
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
    public class MaterialBoxRepository: BaseRepository<MaterialBox>, IMaterialBoxRepository
    {
        private static Metas.GetValueClass g = new Metas.GetValueClass();

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
       
        #region Overrides of BaseRepository<MaterialBox>

        protected override void PersistNewItem(MaterialBox item)
        {
                StateTracker tracker = item.Tracker;
                try
                {
                    if (tracker.GetState(item) == DataRowState.Added)
                    {
                        if (!string.IsNullOrEmpty(item.LotNo) && !string.IsNullOrEmpty(item.MaterialType))
                        {
                            int qty=0;
                            if (existMaterialLot(item.MaterialType, item.LotNo,out qty))
                            {
                                this.PersistUpdateMaterialLotByLotMaterType(new MaterialLot() {
                                     LotNo=item.LotNo,
                                     MaterialType=item.MaterialType,
                                     Qty=qty+item.Qty,
                                     Editor=item.Editor
                                    
                                });
                            }
                            else
                            {                             
                                 this.PersistInsertMaterialLot(new MaterialLot() {
                                     LotNo=item.LotNo,
                                     MaterialType=item.MaterialType,
                                      SpecNo=item.SpecNo,
                                       Status=item.Status,
                                     Qty=item.Qty,
                                     Editor=item.Editor
                                     //Cdt=DateTime.Now,
                                    // Udt=DateTime.Now
                                });

                            }
                        }
                        this.PersistInsertMaterialBox(item);

                        //Add MaterialBoxAttr 
                        IList<MaterialBoxAttr> materialBoxAttrs = (IList<MaterialBoxAttr>)item.GetType().GetField("_materialBoxAttrList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                        if (materialBoxAttrs != null && materialBoxAttrs.Count > 0)
                        {
                            foreach (MaterialBoxAttr info in materialBoxAttrs)
                            {
                                if (info.Tracker.GetState(info) == DataRowState.Added)
                                {
                                    this.PersistInsertMaterialBoxAttr(info);
                                }
                                else if (info.Tracker.GetState(info) == DataRowState.Modified)
                                {
                                    this.PersistUpdateMaterialBoxAttr(info);
                                }
                            }
                        }
                        //Add MaterialBoxAtrrLog
                        IList<MaterialBoxAttrLog> materialBoxAttrLogs = (IList<MaterialBoxAttrLog>)item.GetType().GetField("_materialBoxAttrLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                        if (materialBoxAttrLogs != null && materialBoxAttrLogs.Count > 0)
                        {
                            foreach (MaterialBoxAttrLog info in materialBoxAttrLogs)
                            {
                                if (info.Tracker.GetState(info) == DataRowState.Added)
                                {
                                    this.PersistInsertMaterialBoxAttrLog(info);
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

        protected override void PersistUpdatedItem(MaterialBox item)
        {
            StateTracker tracker = item.Tracker;
            try
            {

                if (tracker.GetState(item) == DataRowState.Modified || tracker.GetState(item.SubTrackerName) == DataRowState.Modified)
                {
                    if (tracker.GetState(item) == DataRowState.Modified)
                    {
                        //string lotNo = "";
                        //int qty=this.getMaterialBoxQty(item.BoxId, out lotNo);
                        //int lotQty=0;
                        //if (!string.IsNullOrEmpty(lotNo) &&
                        //     qty != item.Qty && 
                        //     lotNo == item.LotNo &&
                        //     this.existMaterialLot(item.MaterialType, lotNo, out lotQty))
                        //{
                        //    this.PersistUpdateMaterialLotByLotMaterType(new MaterialLot()
                        //    {
                        //        LotNo = item.LotNo,
                        //        MaterialType = item.MaterialType,
                        //        Qty = item.Qty-qty + item.Qty,
                        //        Editor = item.Editor                                
                        //    });
                        //}                      
                        this.PersistUpdateMaterialBox(item);
                    }

                    //Add MaterialBoxAttr 
                    IList<MaterialBoxAttr> materialBoxAttrs = (IList<MaterialBoxAttr>)item.GetType().GetField("_materialBoxAttrList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (materialBoxAttrs != null && materialBoxAttrs.Count > 0)
                    {
                        foreach (MaterialBoxAttr info in materialBoxAttrs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertMaterialBoxAttr(info);
                            }
                            else if (info.Tracker.GetState(info) == DataRowState.Modified)
                            {
                                this.PersistUpdateMaterialBoxAttr(info);
                            }
                        }
                    }
                    //Add MaterialBoxAtrrLog
                    IList<MaterialBoxAttrLog> materialBoxAttrLogs = (IList<MaterialBoxAttrLog>)item.GetType().GetField("_materialBoxAttrLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (materialBoxAttrLogs != null && materialBoxAttrLogs.Count > 0)
                    {
                        foreach (MaterialBoxAttrLog info in materialBoxAttrLogs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertMaterialBoxAttrLog(info);
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

        protected override void PersistDeletedItem(MaterialBox item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteMaterialBox(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<MaterialBox>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override MaterialBox Find(object key)
        {
            try
            {
                MaterialBox ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                #region mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select BoxId, MaterialType, LotNo, SpecNo, FeedType, 
//                                                                       Revision, DateCode, Supplier, PartNo, Qty, 
//                                                                       Status, Comment, Editor, Cdt, Udt
//                                                               from MaterialBox
//                                                              where BoxId=@BoxId";
//                        sqlCtx.AddParam("BoxId", new SqlParameter("@BoxId", SqlDbType.VarChar));
//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("BoxId").Value = (string)key;

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    if (sqlR != null && sqlR.Read())
//                    {
//                        ret = SQLData.ToObject<MaterialBox>(sqlR);
//                        ret.Tracker.Clear();

//                    }
//                }
                //                return ret;
                #endregion

                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.MaterialBox cond = new _Metas.MaterialBox();
                        cond.boxId = (string)key;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBox>(tk, null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.EqualCondition<_Metas.MaterialBox>(cond)));
                    }
                }
                sqlCtx.Param(Metas.MaterialBox.fn_boxId).Value = (string)key;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBox, MaterialBox>(ret, sqlR, sqlCtx);
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
        public override IList<MaterialBox> FindAll()
        {
            try
            {
                IList<MaterialBox> ret = new List<MaterialBox>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                #region Mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select BoxId, MaterialType, LotNo, SpecNo, FeedType, 
//                                                                       Revision, DateCode, Supplier, PartNo, Qty, 
//                                                                       Status, Comment, Editor, Cdt, Udt
//                                                               from MaterialBox
//                                                              order by BoxId";

//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }               

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    while (sqlR != null && sqlR.Read())
//                    {
//                        MaterialBox item  = SQLData.ToObject<MaterialBox>(sqlR);
//                        item.Tracker.Clear();
//                        ret.Add(item);
//                    }
//                }
                //                return ret;
                #endregion
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        sqlCtx = Metas.FuncNew.GetCommonSelect<_Metas.MaterialBox>(tk, Metas.MaterialBox.fn_boxId);
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBox, MaterialBox, MaterialBox>(ret, sqlR, sqlCtx);
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
        public override void Add(MaterialBox item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(MaterialBox item, IUnitOfWork uow)
        {
            base.Remove(item, uow); 
            
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(MaterialBox item, IUnitOfWork uow) 
        {
            base.Update(item, uow);            

        }

        #endregion

        #region . Inners .

        #region insert table
       
         private void PersistInsertMaterialBox(MaterialBox item)
         {
             try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 #region mark SQL
                 //                 lock (mthObj)
//                 {
//                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                     {
//                         sqlCtx = new Metas.SQLContextNew();
//                         sqlCtx.Sentence = @"insert MaterialBox(BoxId, MaterialType, LotNo, SpecNo, FeedType, 
//                                                                                           Revision, DateCode, Supplier, PartNo, Qty, 
//                                                                                           Status, Comment, Editor, Cdt, Udt)
//                                                                        select a.*
//                                                                        from (MERGE INTO MaterialLot as Target 
//                                                                              Using (select @LotNo,@MaterialType)
//                                                                                     as Source (LotNo, MaterialType)
//                                                                              ON Target.LotNo = Source.LotNo and
//                                                                                     Target.MaterialType = Source.MaterialType   
//                                                                            WHEN NOT MATCHED THEN
//                                                                                insert (LotNo, MaterialType, SpecNo, Qty, Status, Editor, Cdt, Udt)		 		
//                                                                                values( @LotNo, @MaterialType, @SpecNo, @Qty, @Status, @Editor, getdate(), getdate())
//                                                                            WHEN MATCHED THEN
//                                                                                 update set Qty= Qty+@Qty,
//                                                                                             Editor =@Editor,
//                                                                                             Udt=getdate()
//                                                                             OUTPUT @BoxId as BoxId, @MaterialType as MaterialType, @LotNo as LotNo, @SpecNo as SpecNo, @FeedType as FeedType,
//                                                                                               @Revision as Revision, @DateCode as DateCode, @Supplier as Supplier, @PartNo as PartNo, @Qty as Qty, 
//                                                                                                @Status as Status,@Comment as Comment, @Editor as Editor,getdate() as Cdt,getdate() as Udt)a;  ";
//                         sqlCtx.AddParam("BoxId", new SqlParameter("@BoxId", SqlDbType.VarChar));
//                         sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                         sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("SpecNo", new SqlParameter("@SpecNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("FeedType", new SqlParameter("@FeedType", SqlDbType.VarChar));

//                         sqlCtx.AddParam("Revision", new SqlParameter("@Revision", SqlDbType.VarChar));
//                         sqlCtx.AddParam("DateCode", new SqlParameter("@DateCode", SqlDbType.VarChar));
//                         sqlCtx.AddParam("Supplier", new SqlParameter("@Supplier", SqlDbType.VarChar));
//                         sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));

//                         sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
//                         sqlCtx.AddParam("Comment", new SqlParameter("@Comment", SqlDbType.VarChar));


//                         sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

//                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                     }
//                 }
//                 sqlCtx.Param("BoxId").Value = item.BoxId;
//                 sqlCtx.Param("MaterialType").Value = item.MaterialType;
//                 sqlCtx.Param("LotNo").Value = item.LotNo;
//                 sqlCtx.Param("SpecNo").Value = item.SpecNo;
//                 sqlCtx.Param("FeedType").Value = item.FeedType;

//                 sqlCtx.Param("Revision").Value = item.Revision;
//                 sqlCtx.Param("DateCode").Value = item.DateCode;
//                 sqlCtx.Param("Supplier").Value = item.Supplier;
//                 sqlCtx.Param("PartNo").Value = item.PartNo;
//                 sqlCtx.Param("Qty").Value = item.Qty;

//                 sqlCtx.Param("Status").Value = item.Status;
//                 sqlCtx.Param("Comment").Value = item.Comment;
                 
//                 sqlCtx.Param("Editor").Value = item.Editor;
//                 _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                  CommandType.Text,
//                                                                                  sqlCtx.Sentence,
//                                                                                  sqlCtx.Params);
                 //                 item.Tracker.Clear();
                 #endregion
                 lock (mthObj)
                 {
                     if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         sqlCtx = _Metas.FuncNew.GetCommonInsert<_Metas.MaterialBox>(tk);

                     }
                 }
                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(sqlCtx, item);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(_Metas.MaterialBox.fn_udt).Value = cmDt;
                 sqlCtx.Param(_Metas.MaterialBox.fn_cdt).Value = cmDt;
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

         private void PersistUpdateMaterialBox(MaterialBox item)
         {
             try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 lock (mthObj)
                 {

                     _Metas.MaterialBox cond = new _Metas.MaterialBox();
                     cond.boxId = item.BoxId;

                     _Metas.MaterialBox setv = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(item, _Metas.MaterialBox.fn_boxId);
                     setv.udt = DateTime.Now;

                     sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.MaterialBox>(new _Metas.SetValueCollection<_Metas.MaterialBox>(new _Metas.CommonSetValue<_Metas.MaterialBox>(setv)),
                                                                                                                                 new _Metas.ConditionCollection<_Metas.MaterialBox>(new _Metas.EqualCondition<_Metas.MaterialBox>(cond)));

                 }

                 sqlCtx.Param(_Metas.MaterialBox.fn_boxId).Value = item.BoxId;

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(sqlCtx, item, true);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(g.DecSV(_Metas.MaterialBox.fn_udt)).Value = cmDt;

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

         private void PersistDeleteMaterialBox(MaterialBox item)
         {
              try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         sqlCtx = new Metas.SQLContextNew();
                         sqlCtx.Sentence = @"update  MaterialLot
                                                                set Qty= case when a.Qty-b.Qty >0 then
                                                                                   a.Qty-b.Qty
                                                                              else
                                                                                  0
                                                                         end,
                                                                     Editor=@Editor,
                                                                     Udt=GETDATE()     
                                                                from MaterialLot a
                                                                inner join MaterialBox b on a.LotNo = b.LotNo
                                                                where b.BoxId =@BoxId

                                                             delete  MaterialBox
                                                             where BoxId =@BoxId

                                                             delete MaterialBoxAttr 
                                                              where BoxId =@BoxId

                                                              delete MaterialBoxAttrLog 
                                                              where BoxId =@BoxId";

                         sqlCtx.AddParam("BoxId", new SqlParameter("@BoxId", SqlDbType.VarChar));  
                         sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                     }
                 }
                 sqlCtx.Param("BoxId").Value = item.BoxId;
                 sqlCtx.Param("Editor").Value = item.Editor;
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

         private void PersistUpdateMaterialBoxAttr(MaterialBoxAttr item)
         {
             try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 lock (mthObj)
                 {

                     _Metas.MaterialBoxAttr cond = new _Metas.MaterialBoxAttr();
                     cond.boxId = item.BoxId;
                     cond.attrName = item.AttrName;
                     _Metas.MaterialBoxAttr setv = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBoxAttr, MaterialBoxAttr>(item, _Metas.MaterialBoxAttr.fn_boxId, _Metas.MaterialBoxAttr.fn_attrName);
                     setv.udt = DateTime.Now;

                     sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.MaterialBoxAttr>(new _Metas.SetValueCollection<_Metas.MaterialBoxAttr>(new _Metas.CommonSetValue<_Metas.MaterialBoxAttr>(setv)),
                                                                                                                                       new _Metas.ConditionCollection<_Metas.MaterialBoxAttr>(new _Metas.EqualCondition<_Metas.MaterialBoxAttr>(cond)));

                 }

                 sqlCtx.Param(_Metas.MaterialBoxAttr.fn_boxId).Value = item.BoxId;
                 sqlCtx.Param(_Metas.MaterialBoxAttr.fn_attrName).Value = item.AttrName;

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBoxAttr, MaterialBoxAttr>(sqlCtx, item, true);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(g.DecSV(_Metas.Material.fn_udt)).Value = cmDt;

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

         private void PersistInsertMaterialBoxAttr(MaterialBoxAttr item)
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
                         sqlCtx = _Metas.FuncNew.GetCommonInsert<_Metas.MaterialBoxAttr>(tk);

                     }
                 }
                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBoxAttr, MaterialBoxAttr>(sqlCtx, item);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(_Metas.MaterialBoxAttr.fn_udt).Value = cmDt;
                 sqlCtx.Param(_Metas.MaterialBoxAttr.fn_cdt).Value = cmDt;
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

         private void PersistInsertMaterialBoxAttrLog(MaterialBoxAttrLog item)
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
                         sqlCtx = _Metas.FuncNew.GetAquireIdInsert<_Metas.MaterialBoxAttrLog>(tk);

                     }
                 }
                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBoxAttrLog, MaterialBoxAttrLog>(sqlCtx, item);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(_Metas.MaterialBoxAttrLog.fn_cdt).Value = cmDt;
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

         private void PersistUpdateMaterialLotByLotMaterType(MaterialLot item)
         {
             try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 lock (mthObj)
                 {
                     _Metas.MaterialLot cond = new _Metas.MaterialLot();
                     cond.lotNo = item.LotNo;
                     cond.materialType = item.MaterialType;
                     _Metas.MaterialLot setv = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialLot, MaterialLot>(item, _Metas.MaterialLot.fn_lotNo, 
                                                                                                                                                                                _Metas.MaterialLot.fn_materialType, 
                                                                                                                                                                                _Metas.MaterialLot.fn_specNo, 
                                                                                                                                                                                _Metas.MaterialLot.fn_status);
                     setv.udt = DateTime.Now;

                     sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.MaterialLot>( new _Metas.SetValueCollection<_Metas.MaterialLot>(new _Metas.CommonSetValue<_Metas.MaterialLot>(setv)),
                                                                                                                                 new _Metas.ConditionCollection<_Metas.MaterialLot>(new _Metas.EqualCondition<_Metas.MaterialLot>(cond)));

                 }

                 sqlCtx.Param(_Metas.MaterialLot.fn_lotNo).Value = item.LotNo;
                 sqlCtx.Param(_Metas.MaterialLot.fn_materialType).Value = item.MaterialType;

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialLot, MaterialLot>(sqlCtx, item, true);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(g.DecSV(_Metas.MaterialLot.fn_udt)).Value = cmDt;

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

         private void PersistInsertMaterialLot(MaterialLot item)
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
                         sqlCtx = _Metas.FuncNew.GetCommonInsert<_Metas.MaterialLot>(tk);

                     }
                 }
                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialLot, MaterialLot>(sqlCtx, item);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(_Metas.MaterialLot.fn_udt).Value = cmDt;
                 sqlCtx.Param(_Metas.MaterialLot.fn_cdt).Value = cmDt;
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

         private bool existMaterialLot(string materialType, string lotNo, out int qty)
         {
             try
             {
                 qty = 0;
                 bool ret = false;
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialLot cond = new _Metas.MaterialLot();
                         cond.lotNo = lotNo;
                         cond.materialType = materialType;

                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialLot>(tk, null, new string[]{_Metas.MaterialLot.fn_qty},
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialLot>(new Metas.EqualCondition<_Metas.MaterialLot>(cond)));
                     }
                 }
                 sqlCtx.Param(Metas.MaterialLot.fn_lotNo).Value = lotNo;
                 sqlCtx.Param(Metas.MaterialLot.fn_materialType).Value = materialType;



                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     if (sqlR != null && sqlR.Read())
                     {
                         qty=sqlR.GetInt32(0);
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

         private int getMaterialBoxQty(string boxId, out string lotNo)
         {
             try
             {
                 lotNo = "";
                 int ret = 0;                 
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialBox cond = new _Metas.MaterialBox();
                         cond.boxId = boxId;


                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBox>(tk, null, new string[] { _Metas.MaterialBox.fn_qty,_Metas.MaterialBox.fn_lotNo },
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.EqualCondition<_Metas.MaterialBox>(cond)));
                     }
                 }
                 sqlCtx.Param(Metas.MaterialBox.fn_boxId).Value = boxId;

                  using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     if (sqlR != null && sqlR.Read())
                     {
                         ret = sqlR.GetInt32(0);
                         lotNo = sqlR.GetString(1).ToString();
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

         #region FillData
         public void FillMaterialLot(MaterialBox materialBox)
         {
             try
             {
                 MaterialLot ret = null;
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 #region mark SQL Statement
                 //                 lock (mthObj)
//                 {
//                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                     {
//                         sqlCtx = new Metas.SQLContextNew();
//                         sqlCtx.Sentence = @"select LotNo, MaterialType, SpecNo, Qty, Status, Editor, Cdt, Udt
//                                                                from MaterialLot
//                                                                where LotNo=@LotNo and
//                                                                           MaterialType =@MaterialType";
//                         sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                     }
//                 }
//                 sqlCtx.Param("LotNo").Value = materialBox.LotNo;
//                 sqlCtx.Param("MaterialType").Value = materialBox.MaterialType;

//                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                             CommandType.Text,
//                                                                                                                             sqlCtx.Sentence,
//                                                                                                                             sqlCtx.Params))
//                 {
//                     if (sqlR != null && sqlR.Read())
//                     {
//                         ret = SQLData.ToObject<MaterialLot>(sqlR);
//                         ret.Tracker.Clear();
//                     }

                 //                 }
                 #endregion

                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialLot cond = new _Metas.MaterialLot();
                         cond.lotNo = materialBox.LotNo;
                         cond.materialType = materialBox.MaterialType;

                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialLot>(tk, null, null,
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialLot>(new Metas.EqualCondition<_Metas.MaterialLot>(cond)),
                                                                                                          _Metas.MaterialLot.fn_udt + Metas.FuncNew.DescendOrder);
                     }
                 }
                 sqlCtx.Param(Metas.MaterialLot.fn_lotNo).Value = materialBox.LotNo;
                 sqlCtx.Param(Metas.MaterialLot.fn_materialType).Value = materialBox.MaterialType;



                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialLot, MaterialLot>(ret, sqlR, sqlCtx);
                 }                 
                 materialBox.GetType().GetField("_materialLot", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(materialBox, ret);

             }
             catch (Exception)
             {
                 throw;
             }
         }

         public void FillMaterialBoxAttr(MaterialBox materialBox)
         {
             try
             {
                 IList<MaterialBoxAttr> ret = new List<MaterialBoxAttr>();

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialBoxAttr cond = new _Metas.MaterialBoxAttr();
                         cond.boxId = materialBox.BoxId;
                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBoxAttr>(tk, null, null,
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialBoxAttr>(new Metas.EqualCondition<_Metas.MaterialBoxAttr>(cond)),
                                                                                                          Metas.MaterialBoxAttr.fn_attrName);
                     }
                 }
                 sqlCtx.Param(Metas.MaterialBoxAttr.fn_boxId).Value = materialBox.BoxId;


                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBoxAttr, MaterialBoxAttr, MaterialBoxAttr>(ret, sqlR, sqlCtx);
                 }
                 materialBox.GetType().GetField("_materialBoxAttrList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(materialBox, ret);
             }
             catch (Exception)
             {
                 throw;
             }
         }

         public void FillMaterialBoxAttrLog(MaterialBox materialBox)
         {
             try
             {
                 IList<MaterialBoxAttrLog> ret = new List<MaterialBoxAttrLog>();

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialBoxAttrLog cond = new _Metas.MaterialBoxAttrLog();
                         cond.boxId = materialBox.BoxId;
                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBoxAttrLog>(tk, null, null,
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialBoxAttrLog>(new Metas.EqualCondition<_Metas.MaterialBoxAttrLog>(cond)),
                                                                                                          Metas.MaterialBoxAttrLog.fn_cdt);
                     }
                 }
                 sqlCtx.Param(Metas.MaterialBoxAttrLog.fn_boxId).Value = materialBox.BoxId;


                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBoxAttrLog, MaterialBoxAttrLog, MaterialBoxAttrLog>(ret, sqlR, sqlCtx);
                 }
                 materialBox.GetType().GetField("_materialBoxAttrLogList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(materialBox, ret);
             }
             catch (Exception)
             {
                 throw;
             }
         }
       
         #endregion

        #region implement IMaterialBoxRepository
         public IList<MaterialBox> GetMaterialBoxByLot(string materialType, string lotNo)
         {
             try
             {
                 IList<MaterialBox> ret = new List<MaterialBox>();
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 #region mark SQL
//                 lock (mthObj)
//                 {
//                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                     {
//                         sqlCtx = new Metas.SQLContextNew();
//                         sqlCtx.Sentence = @"select BoxId, MaterialType, LotNo, SpecNo, FeedType, 
//                                                                       Revision, DateCode, Supplier, PartNo, Qty, 
//                                                                       Status, Comment, Editor, Cdt, Udt
//                                                               from MaterialBox
//                                                               where  LotNo=@LotNo and
//                                                                           MaterialType =@MaterialType 
//                                                              order by BoxId";
//                         sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                     }
//                 }
//                 sqlCtx.Param("LotNo").Value = lotNo;
//                 sqlCtx.Param("MaterialType").Value = materialType;
//                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                             CommandType.Text,
//                                                                                                                             sqlCtx.Sentence,
//                                                                                                                             sqlCtx.Params))
//                 {
//                     while (sqlR != null && sqlR.Read())
//                     {
//                         MaterialBox item = SQLData.ToObject<MaterialBox>(sqlR);
//                         item.Tracker.Clear();
//                         ret.Add(item);
//                     }
//                 }
                 //                 return ret;
                 #endregion

                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialBox cond = new _Metas.MaterialBox();
                         cond.materialType = materialType;
                         cond.lotNo = lotNo;
                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBox>(tk, null, null,
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.EqualCondition<_Metas.MaterialBox>(cond)),
                                                                                                          Metas.MaterialBox.fn_boxId);
                     }
                 }
                 sqlCtx.Param(Metas.MaterialBox.fn_materialType).Value = materialType;
                 sqlCtx.Param(Metas.MaterialBox.fn_lotNo).Value = lotNo;



                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBox, MaterialBox, MaterialBox>(ret, sqlR, sqlCtx);
                 }
                 return ret;
             }
             catch (Exception)
             {
                 throw;
             }
         }
         public IList<MaterialBox> GetMaterialBoxBySpec(string materialType, string specNo)
         {
             try
             {
                 IList<MaterialBox> ret = new List<MaterialBox>();
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 #region mark SQL
                 //                 lock (mthObj)
//                 {
//                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                     {
//                         sqlCtx = new Metas.SQLContextNew();
//                         sqlCtx.Sentence = @"select BoxId, MaterialType, LotNo, SpecNo, FeedType, 
//                                                                       Revision, DateCode, Supplier, PartNo, Qty, 
//                                                                       Status, Comment, Editor, Cdt, Udt
//                                                               from MaterialBox
//                                                               where  SpecNo=@SpecNo and
//                                                                           MaterialType =@MaterialType  
//                                                              order by BoxId";
//                         sqlCtx.AddParam("SpecNo", new SqlParameter("@SpecNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                     }
//                 }
//                 sqlCtx.Param("SpecNo").Value = specNo;
//                 sqlCtx.Param("MaterialType").Value = materialType;
//                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                             CommandType.Text,
//                                                                                                                             sqlCtx.Sentence,
//                                                                                                                             sqlCtx.Params))
//                 {
//                     while (sqlR != null && sqlR.Read())
//                     {
//                         MaterialBox item = SQLData.ToObject<MaterialBox>(sqlR);
//                         item.Tracker.Clear();
//                         ret.Add(item);
//                     }
//                 }
                 //                 return ret;
                 #endregion
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialBox cond = new _Metas.MaterialBox();
                         cond.materialType = materialType;
                         cond.specNo = specNo;
                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBox>(tk, null, null,
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.EqualCondition<_Metas.MaterialBox>(cond)),
                                                                                                          Metas.MaterialBox.fn_boxId);
                     }
                 }
                 sqlCtx.Param(Metas.MaterialBox.fn_materialType).Value = materialType;
                 sqlCtx.Param(Metas.MaterialBox.fn_specNo).Value = specNo;



                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBox, MaterialBox, MaterialBox>(ret, sqlR, sqlCtx);
                 }
                 return ret;

             }
             catch (Exception)
             {
                 throw;
             }
         }
         public MaterialLot GetMaterialLot(string materialType, string lotNo)
         {
             try
             {
                 MaterialLot ret = null;
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 #region Mark SQL
                 //                 lock (mthObj)
//                 {
//                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                     {
//                         sqlCtx = new Metas.SQLContextNew();
//                         sqlCtx.Sentence = @"select LotNo, MaterialType, SpecNo, Qty, Status, Editor, Cdt, Udt
//                                                                from MaterialLot
//                                                                where LotNo=@LotNo and
//                                                                           MaterialType =@MaterialType  ";
//                         sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                     }
//                 }
//                 sqlCtx.Param("LotNo").Value = lotNo;
//                 sqlCtx.Param("MaterialType").Value = materialType;


//                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                             CommandType.Text,
//                                                                                                                             sqlCtx.Sentence,
//                                                                                                                             sqlCtx.Params))
//                 {
//                     if (sqlR != null && sqlR.Read())
//                     {
//                         ret = SQLData.ToObject<MaterialLot>(sqlR);
//                         ret.Tracker.Clear();
//                     }

//                 }
                 //                 return ret;
                 #endregion

                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialLot cond = new _Metas.MaterialLot();                         
                         cond.lotNo = lotNo;
                         cond.materialType = materialType;

                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialLot>(tk, null, null,
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialLot>(new Metas.EqualCondition<_Metas.MaterialLot>(cond)));
                     }
                 }
                 sqlCtx.Param(Metas.MaterialLot.fn_lotNo).Value = lotNo;
                 sqlCtx.Param(Metas.MaterialLot.fn_materialType).Value = materialType;



                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialLot, MaterialLot>(ret, sqlR, sqlCtx);
                 }
                 return ret;
             }
             catch (Exception)
             {
                 throw;
             }
         }
         public IList<MaterialLot> GetMaterialLotBySpec(string materialType, string specNo)
         {
             try
             {
                 IList<MaterialLot> ret = new List<MaterialLot>();
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 #region markSQL
                 //                 lock (mthObj)
//                 {
//                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                     {
//                         sqlCtx = new Metas.SQLContextNew();
//                         sqlCtx.Sentence = @"select LotNo, MaterialType, SpecNo, Qty, Status, Editor, Cdt, Udt
//                                                                from MaterialLot
//                                                                where SpecNo=@SpecNo and
//                                                                           MaterialType =@MaterialType ";
//                         sqlCtx.AddParam("SpecNo", new SqlParameter("@SpecNo", SqlDbType.VarChar));
//                         sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                     }
//                 }
//                 sqlCtx.Param("SpecNo").Value = specNo;
//                 sqlCtx.Param("MaterialType").Value = materialType;

//                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                             CommandType.Text,
//                                                                                                                             sqlCtx.Sentence,
//                                                                                                                             sqlCtx.Params))
//                 {
//                     while (sqlR != null && sqlR.Read())
//                     {
//                         MaterialLot item = SQLData.ToObject<MaterialLot>(sqlR);
//                         item.Tracker.Clear();
//                         ret.Add(item);
//                     }

//                 }
                 //                 return ret;
                 #endregion
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialLot cond = new _Metas.MaterialLot();
                         cond.specNo = specNo;
                         cond.materialType = materialType;

                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialLot>(tk, null, null,
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialLot>(new Metas.EqualCondition<_Metas.MaterialLot>(cond)));
                     }
                 }
                 sqlCtx.Param(Metas.MaterialLot.fn_specNo).Value = specNo;
                 sqlCtx.Param(Metas.MaterialLot.fn_materialType).Value = materialType;



                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialLot, MaterialLot, MaterialLot>(ret, sqlR, sqlCtx);
                 }
                 return ret;

             }
             catch (Exception)
             {
                 throw;
             }
         }


         public IList<MaterialBox> GetMaterialBox(MaterialBox condition)
         {
             try
             {

                 IList<MaterialBox> ret = new List<MaterialBox>();

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     _Metas.MaterialBox cond = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(condition, _Metas.MaterialBox.fn_boxWeight);
                     sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBox>(null, null,
                                                                                                        new Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.EqualCondition<_Metas.MaterialBox>(cond)),
                                                                                                        Metas.MaterialBox.fn_boxId);

                 }

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(sqlCtx, condition);


                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBox, MaterialBox, MaterialBox>(ret, sqlR, sqlCtx);
                 }
                 return ret;
             }
             catch (Exception)
             {
                 throw;
             }
         }

         public IList<MaterialBox> GetMaterialBoxbyMultiBoxId(IList<string> boxIdList)
         {
             try
             {

                 IList<MaterialBox> ret = new List<MaterialBox>();

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     _Metas.MaterialBox cond = new _Metas.MaterialBox();
                     cond.boxId = "[INSET]";

                     sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBox>(tk, null, null,
                                                                                                        new Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.InSetCondition<_Metas.MaterialBox>(cond)),
                                                                                                        Metas.MaterialBox.fn_boxId);

                 }

                 string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.MaterialBox.fn_boxId), g.ConvertInSet(boxIdList));


                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                     Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialBox, MaterialBox, MaterialBox>(ret, sqlR, sqlCtx);
                 }
                 return ret;
             }
             catch (Exception)
             {
                 throw;
             }
         }

         public IList<string> GetMaterialBoxIdByAttribute(string attrName, string attrValue)
         {
             try
             {
                 IList<string> ret = new List<string>();

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialBoxAttr cond = new _Metas.MaterialBoxAttr();
                         cond.attrName = attrName;
                         cond.attrValue = attrValue;
                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBoxAttr>(tk, "Distinct", new string[] { _Metas.MaterialBoxAttr.fn_boxId },
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialBoxAttr>(new Metas.EqualCondition<_Metas.MaterialBoxAttr>(cond)));
                     }
                 }
                 sqlCtx.Param(Metas.MaterialBoxAttr.fn_attrName).Value = attrName;
                 sqlCtx.Param(Metas.MaterialBoxAttr.fn_attrValue).Value = attrValue;


                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
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

         public IList<CombinedPalletCarton> GetCartonQtywithCombinedPallet(string deliveryNo)
         {
             try
             {
                 IList<CombinedPalletCarton> ret = new List<CombinedPalletCarton>();

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         sqlCtx = new Metas.SQLContextNew();
                         sqlCtx.Sentence = @"select PalletNo, count(distinct CartonSN) as Qty
                                                        from    MaterialBox
                                                        where  DeliveryNo =@DeliveryNo and                                                                 
                                                                    PalletNo != ''
                                                        group by PalletNo";

                         sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                     }
                 }

                 sqlCtx.Param("DeliveryNo").Value = deliveryNo;

                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
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

         public int GetCombinedMaterialBoxQty(string materialType, string cartonSn, string deliveryNo, string palletNo)
         {
             try
             {
                 int ret = 0;

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 MaterialBox condititon = new MaterialBox()
                 {
                     MaterialType = materialType,
                     CartonSN = cartonSn,
                     DeliveryNo = deliveryNo,
                     PalletNo = palletNo
                 };


                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         _Metas.MaterialBox cond = new _Metas.MaterialBox();
                         cond.materialType = materialType;
                         cond.cartonSN = cartonSn;
                         cond.deliveryNo = deliveryNo;
                         cond.palletNo = palletNo;
                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialBox>("sum", new string[] { _Metas.MaterialBox.fn_qty },
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.EqualCondition<_Metas.MaterialBox>(cond)));
                     }
                 }

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(sqlCtx, condititon);

                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     if (sqlR != null && sqlR.Read())
                     {
                         if (!sqlR.IsDBNull(0))
                         {
                             ret = sqlR.GetInt32(0);
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

         public void UpdateDnPalletStatusbyMultiBoxId(IList<string> boxIdList,
                                                                     string deliveryNo,
                                                                     string cartonSN,
                                                                     string palletNo,
                                                                     string status,
                                                                     string line,
                                                                      string editor)
         {
             try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 MaterialBox item = new MaterialBox()
                 {
                      DeliveryNo = deliveryNo,
                       CartonSN =cartonSN,
                        PalletNo =palletNo,
                     Status = status,
                      Line =line,
                     Editor = editor
                 };

                 lock (mthObj)
                 {

                     _Metas.MaterialBox cond = new _Metas.MaterialBox();
                     cond.boxId = "[INSET]";

                     _Metas.MaterialBox setv = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(item, _Metas.MaterialBox.fn_boxId,_Metas.MaterialBox.fn_boxWeight);
                     setv.udt = DateTime.Now;

                     sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.MaterialBox>(new _Metas.SetValueCollection<_Metas.MaterialBox>(new _Metas.CommonSetValue<_Metas.MaterialBox>(setv)),
                                                                                                                                  new _Metas.ConditionCollection<_Metas.MaterialBox>(new Metas.InSetCondition<_Metas.MaterialBox>(cond)));

                 }

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialBox, MaterialBox>(sqlCtx, item, true);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(g.DecSV(_Metas.Material.fn_udt)).Value = cmDt;

                 string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.MaterialBox.fn_boxId), g.ConvertInSet(boxIdList));

                 _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                  CommandType.Text,
                                                                                 Sentence,
                                                                                  sqlCtx.Params);
             }
             catch (Exception)
             {
                 throw;
             }
         }

         public void UpdateDnPalletStatusbyMultiBoxIdDefered(IUnitOfWork uow,
                                                                     IList<string> boxIdList,
                                                                     string deliveryNo,
                                                                     string cartonSN,
                                                                     string palletNo,
                                                                     string status,
                                                                     string line,
                                                                      string editor)
         {
             AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                            boxIdList,
                                            deliveryNo,
                                            cartonSN,
                                            palletNo,
                                            status,
                                            line,
                                            editor);
         }


         public void CheckDnQtyAndUpdateDnStatus(string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode)
         {
             int dnQty = DeliveryRep.GetDeliveryQtyOnTrans(deliveryNo,"00");
             int combinedQty = getCombinedQtyByDnOnTrans(deliveryNo);
             int remainingQty = dnQty - combinedQty;
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
        #endregion

         #region Private Check Delivery Qty
        
         private int getCombinedQtyByDnOnTrans(string deliveryNo)
         {
             try
             {
                 int ret = 0;

                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;
                 lock (mthObj)
                 {
                     if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                     {
                         sqlCtx = new Metas.SQLContextNew();
                         sqlCtx.Sentence = @"select isnull(sum(isnull(Qty,0)),0) as Qty 
                                                        from MaterialBox 
                                                        where DeliveryNo=@DeliveryNo";

                         sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));

                         Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
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
    }

    
}
