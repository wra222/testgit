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
    public class MaterialRepository: BaseRepository<Material>, IMaterialRepository
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
        #region Overrides of BaseRepository<MaterialStatus>

        protected override void PersistNewItem(Material item)
        { 
                StateTracker tracker = item.Tracker;
                try
                {
                    if (tracker.GetState(item) == DataRowState.Added)
                    {
                        this.PersistInsertMaterial(item);

                        //Add Material Log
                        IList<MaterialLog> materialLogs = (IList<MaterialLog>)item.GetType().GetField("_materialLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                        if (materialLogs != null && materialLogs.Count > 0)
                        {
                            foreach (MaterialLog info in materialLogs)
                            {
                                if (info.Tracker.GetState(info) == DataRowState.Added)
                                {
                                    this.PersistInsertMaterilaLog(info);
                                }
                            }
                        }

                        //Add MaterialAttr 
                        IList<MaterialAttr> materialAttrs = (IList<MaterialAttr>)item.GetType().GetField("_materialAttrList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                        if (materialAttrs != null && materialAttrs.Count > 0)
                        {
                            foreach (MaterialAttr info in materialAttrs)
                            {
                                if (info.Tracker.GetState(info) == DataRowState.Added)
                                {
                                    this.PersistInsertMaterialAttr(info);
                                }
                                else if (info.Tracker.GetState(info) == DataRowState.Modified)
                                {
                                    this.PersistUpdateMaterialAttr(info);
                                }
                            }
                        }
                        //Add MaterialAtrrLog
                        IList<MaterialAttrLog> materialAttrLogs = (IList<MaterialAttrLog>)item.GetType().GetField("_materialAttrLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                        if (materialAttrLogs != null && materialAttrLogs.Count > 0)
                        {
                            foreach (MaterialAttrLog info in materialAttrLogs)
                            {
                                if (info.Tracker.GetState(info) == DataRowState.Added)
                                {
                                    this.PersistInsertMaterialAttrLog(info);
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

        protected override void PersistUpdatedItem(Material item)
        {
            StateTracker tracker = item.Tracker;
            try
            {

                if (tracker.GetState(item) == DataRowState.Modified || tracker.GetState(item.SubTrackerName) == DataRowState.Modified)
                {
                    if (tracker.GetState(item) == DataRowState.Modified)
                    {
                        this.PersistUpdateMaterial(item);
                    }

                    //Add Material Log
                    IList<MaterialLog> materialLogs = (IList<MaterialLog>)item.GetType().GetField("_materialLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (materialLogs != null && materialLogs.Count > 0)
                    {
                        foreach (MaterialLog info in materialLogs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertMaterilaLog(info);
                            }
                        }
                    }

                    //Add MaterialAttr 
                    IList<MaterialAttr> materialAttrs = (IList<MaterialAttr>)item.GetType().GetField("_materialAttrList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (materialAttrs != null && materialAttrs.Count > 0)
                    {
                        foreach (MaterialAttr info in materialAttrs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertMaterialAttr(info);
                            }
                            else if (info.Tracker.GetState(info) == DataRowState.Modified)
                            {
                                this.PersistUpdateMaterialAttr(info);
                            }
                        }
                    }
                    //Add MaterialAtrrLog
                    IList<MaterialAttrLog> materialAttrLogs = (IList<MaterialAttrLog>)item.GetType().GetField("_materialAttrLogList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item);
                    if (materialAttrLogs != null && materialAttrLogs.Count > 0)
                    {
                        foreach (MaterialAttrLog info in materialAttrLogs)
                        {
                            if (info.Tracker.GetState(info) == DataRowState.Added)
                            {
                                this.PersistInsertMaterialAttrLog(info);
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

        protected override void PersistDeletedItem(Material item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteMaterial(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<Material>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override Material Find(object key)
        {
            try
            {
                Material ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                #region Mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select MaterialCT, MaterialType, LotNo, Stage, Line, 
//                                                                       PreStatus, Status, Editor, Cdt, Udt
//                                                                from Material
//                                                                where MaterialCT =@MaterialCT";
//                        sqlCtx.AddParam("MaterialCT", new SqlParameter("@MaterialCT", SqlDbType.VarChar));
//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("MaterialCT").Value = (string)key;

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    if (sqlR != null && sqlR.Read())
//                    {
//                        ret = SQLData.ToObject<Material>(sqlR);
//                        ret.Tracker.Clear();

//                    }
                //                }
                #endregion               
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Material cond = new _Metas.Material();
                        cond.materialCT = (string)key;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.Material>(tk, null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.Material>(new Metas.EqualCondition<_Metas.Material>(cond)));
                    }
                }
                sqlCtx.Param(Metas.Material.fn_materialCT).Value = (string)key;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.Material, Material>(ret, sqlR, sqlCtx);
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
        public override IList<Material> FindAll()
        {
            try
            {
                IList<Material> ret = new List<Material>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                #region Mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select MaterialCT, MaterialType, LotNo, Stage, Line, 
//                                                                       PreStatus, Status, Editor, Cdt, Udt
//                                                                from Material
//                                                             order by MaterialCT";

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
//                        Material item = SQLData.ToObject<Material>(sqlR);
//                        item.Tracker.Clear();
//                        ret.Add(item);

//                    }
                //                }
                #endregion

                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {                        
                        sqlCtx = Metas.FuncNew.GetCommonSelect<_Metas.Material>(tk,Metas.Material.fn_materialCT);
                    }
                }          
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.Material, Material, Material>(ret, sqlR, sqlCtx);
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
        public override void Add(Material item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(Material item, IUnitOfWork uow)
        {
            //base.Remove(item, uow); 
            throw new Exception("Not Allow Delete Data");
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(Material item, IUnitOfWork uow) 
        {
            base.Update(item, uow);

        }

        #endregion

        #region . Inners .

        private void PersistInsertMaterial(Material item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                #region mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"insert Material(MaterialCT, MaterialType, LotNo, Stage, Line, 
//                                                                                    PreStatus, Status, Editor, Cdt, Udt)
//                                                                    values(@MaterialCT, @MaterialType, @LotNo, @Stage, @Line, 
//                                                                           @PreStatus, @Status, @Editor, GETDATE(), getdate())";
//                        sqlCtx.AddParam("MaterialCT", new SqlParameter("@MaterialCT", SqlDbType.VarChar));
//                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

//                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));


//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("MaterialCT").Value = item.MaterialCT;
//                sqlCtx.Param("MaterialType").Value = item.MaterialType;
//                sqlCtx.Param("LotNo").Value = item.LotNo;
//                sqlCtx.Param("Stage").Value = item.Stage;
//                sqlCtx.Param("Line").Value = item.Line;

//                sqlCtx.Param("PreStatus").Value = item.PreStatus;
//                sqlCtx.Param("Status").Value = item.Status;

//                sqlCtx.Param("Editor").Value = item.Editor;
//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                 CommandType.Text,
//                                                                                 sqlCtx.Sentence,
//                                                                                 sqlCtx.Params);
//                item.Tracker.Clear();
#endregion

                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = _Metas.FuncNew.GetCommonInsert<_Metas.Material>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.Material.fn_udt).Value = cmDt;
                sqlCtx.Param(_Metas.Material.fn_cdt).Value = cmDt;

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

        private void PersistUpdateMaterial(Material item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                #region Mark SQL
                //lock (mthObj)
                //{

//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"Update Material
//                                                                    set MaterialType=@MaterialType, 
//                                                                            LotNo=@LotNo, 
//                                                                            Stage=@Stage, 
//                                                                            Line=@Line, 
//                                                                            PreStatus=@PreStatus, 
//                                                                            Status=@Status, 
//                                                                            Editor=@Editor, 
//                                                                            Udt =GETDATE()
//                                                                    where MaterialCT = @MaterialCT";
//                        sqlCtx.AddParam("MaterialCT", new SqlParameter("@MaterialCT", SqlDbType.VarChar));
//                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

//                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));


//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("MaterialCT").Value = item.MaterialCT;
//                sqlCtx.Param("MaterialType").Value = item.MaterialType;
//                sqlCtx.Param("LotNo").Value = item.LotNo;
//                sqlCtx.Param("Stage").Value = item.Stage;
//                sqlCtx.Param("Line").Value = item.Line;

//                sqlCtx.Param("PreStatus").Value = item.PreStatus;
//                sqlCtx.Param("Status").Value = item.Status;

//                sqlCtx.Param("Editor").Value = item.Editor;
//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                 CommandType.Text,
//                                                                                 sqlCtx.Sentence,
//                                                                                 sqlCtx.Params);
//                item.Tracker.Clear();
#endregion

                lock (mthObj)
                {
                    //if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    //{
                        _Metas.Material cond = new _Metas.Material();
                        cond.materialCT = item.MaterialCT;
                        _Metas.Material setv = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(item, _Metas.Material.fn_materialCT);
                        setv.udt = DateTime.Now;

                        sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.Material>(new _Metas.SetValueCollection<_Metas.Material>(new _Metas.CommonSetValue<_Metas.Material>(setv)),
                                                                                                                              new _Metas.ConditionCollection<_Metas.Material>(new _Metas.EqualCondition<_Metas.Material>(cond)));
                    //}
                }

                sqlCtx.Param(_Metas.Material.fn_materialCT).Value = item.MaterialCT;

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(sqlCtx, item, true);
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

        private void PersistDeleteMaterial(Material item)
        {
            throw new Exception("Not Allow Delete Data");
        }

        private void PersistInsertMaterilaLog(MaterialLog item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                #region mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @" insert MaterialLog(MaterialCT, Action, Stage, Line, PreStatus, 
//				                                                                            Status, Comment, Editor, Cdt)
//                                                                         values(@MaterialCT, @Action, @Stage, @Line, @PreStatus, 
//		                                                                        @Status, @Comment,@Editor, GETDATE()) ";
//                        sqlCtx.AddParam("MaterialCT", new SqlParameter("@MaterialCT", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Action", new SqlParameter("@Action", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
//                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));

//                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Comment", new SqlParameter("@Comment", SqlDbType.VarChar));

//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("MaterialCT").Value = item.MaterialCT;
//                sqlCtx.Param("Action").Value = item.Action;                
//                sqlCtx.Param("Stage").Value = item.Stage;
//                sqlCtx.Param("Line").Value = item.Line;
//                sqlCtx.Param("PreStatus").Value = item.PreStatus;

//                sqlCtx.Param("Status").Value = item.Status;
//                sqlCtx.Param("Comment").Value = item.Comment;

//                sqlCtx.Param("Editor").Value = item.Editor;
//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                 CommandType.Text,
//                                                                                 sqlCtx.Sentence,
//                                                                                 sqlCtx.Params);
                //                item.Tracker.Clear();
                #endregion

                lock (mthObj)
                {
                    if (!_Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                       
                        sqlCtx = _Metas.FuncNew.GetAquireIdInsert<_Metas.MaterialLog>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialLog, MaterialLog>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.MaterialLog.fn_cdt).Value = cmDt;

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

        private void PersistUpdateMaterialAttr(MaterialAttr item)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;               

                lock (mthObj)
                {
                    
                    _Metas.MaterialAttr cond = new _Metas.MaterialAttr();
                    cond.materialCT = item.MaterialCT;
                    cond.attrName = item.AttrName;
                    _Metas.MaterialAttr setv = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialAttr, MaterialAttr>(item, _Metas.MaterialAttr.fn_materialCT,_Metas.MaterialAttr.fn_attrName);
                    setv.udt = DateTime.Now;

                    sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.MaterialAttr>(new _Metas.SetValueCollection<_Metas.MaterialAttr>(new _Metas.CommonSetValue<_Metas.MaterialAttr>(setv)),
                                                                                                                                new _Metas.ConditionCollection<_Metas.MaterialAttr>(new _Metas.EqualCondition<_Metas.MaterialAttr>(cond)));
                  
                }

                sqlCtx.Param(_Metas.MaterialAttr.fn_materialCT).Value = item.MaterialCT;
                sqlCtx.Param(_Metas.MaterialAttr.fn_attrName).Value = item.AttrName;

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialAttr, MaterialAttr>(sqlCtx, item, true);
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

        private void PersistInsertMaterialAttr(MaterialAttr item)
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
                        sqlCtx = _Metas.FuncNew.GetCommonInsert<_Metas.MaterialAttr>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialAttr, MaterialAttr>(sqlCtx, item);
                
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.MaterialAttr.fn_udt).Value = cmDt;
                sqlCtx.Param(_Metas.MaterialAttr.fn_cdt).Value = cmDt;

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

        private void PersistInsertMaterialAttrLog(MaterialAttrLog item)
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
                        sqlCtx = _Metas.FuncNew.GetAquireIdInsert<_Metas.MaterialAttrLog>(tk);

                    }
                }
                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.MaterialAttrLog, MaterialAttrLog>(sqlCtx, item);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(_Metas.MaterialAttrLog.fn_cdt).Value = cmDt;
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


        #region FillData
        public void FillMaterialLot(Material material)
        {
            try
            {
                MaterialLot ret =null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;

                #region mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select LotNo, MaterialType, SpecNo, Qty, Status, Editor, Cdt, Udt
//                                                                from MaterialLot
//                                                                where LotNo=@LotNo and
//                                                                           MaterialType=@MaterialType ";
//                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("LotNo").Value = material.LotNo;
//                sqlCtx.Param("MaterialType").Value = material.MaterialType;
//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    if (sqlR != null && sqlR.Read())
//                    {
//                       ret = SQLData.ToObject<MaterialLot>(sqlR);
//                       ret.Tracker.Clear();
//                    }

                //                }
                #endregion

                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.MaterialLot cond = new _Metas.MaterialLot();
                        cond.lotNo = material.LotNo;
                        cond.materialType = material.MaterialType;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialLot>(tk, null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.MaterialLot>(new Metas.EqualCondition<_Metas.MaterialLot>(cond)));
                    }
                }
                sqlCtx.Param(Metas.MaterialLot.fn_lotNo).Value = material.LotNo;
                sqlCtx.Param(Metas.MaterialLot.fn_materialType).Value = material.MaterialType;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialLot, MaterialLot>(ret, sqlR, sqlCtx);
                }

                material.GetType().GetField("_materialLot", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(material, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void FillMaterialLog(Material material)
        {
            try
            {
                IList<MaterialLog> ret = new List<MaterialLog>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;

                #region mark SQL
                //                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select ID, MaterialCT, Action, Stage, Line, 
//                                                                       PreStatus, Status, Comment, Editor, Cdt
//                                                                from MaterialLog
//                                                                where MaterialCT=@MaterialCT ";
//                        sqlCtx.AddParam("MaterialCT", new SqlParameter("@MaterialCT", SqlDbType.VarChar));
//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("MaterialCT").Value = material.MaterialCT;

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    while (sqlR != null && sqlR.Read())
//                    {
//                        MaterialLog item = SQLData.ToObject<MaterialLog>(sqlR);
//                        item.Tracker.Clear();
//                        ret.Add(item);

//                    }

                //                }
                #endregion

                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.MaterialLog cond = new _Metas.MaterialLog();
                        cond.materialCT = material.MaterialCT;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialLog>(tk, null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.MaterialLog>(new Metas.EqualCondition<_Metas.MaterialLog>(cond)),
                                                                                                         Metas.MaterialLog.fn_cdt);
                    }
                }
                sqlCtx.Param(Metas.MaterialLog.fn_materialCT).Value = material.MaterialCT;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialLog, MaterialLog, MaterialLog>(ret, sqlR, sqlCtx);
                }
                material.GetType().GetField("_materialLogList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(material, ret);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void FillMaterialAttr(Material material){
            try
            {
                IList<MaterialAttr> ret = new List<MaterialAttr>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;                
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.MaterialAttr cond = new _Metas.MaterialAttr();
                        cond.materialCT = material.MaterialCT;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialAttr>(tk, null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.MaterialAttr>(new Metas.EqualCondition<_Metas.MaterialAttr>(cond)),
                                                                                                         Metas.MaterialAttr.fn_attrName);
                    }
                }
                sqlCtx.Param(Metas.MaterialAttr.fn_materialCT).Value = material.MaterialCT;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialAttr, MaterialAttr, MaterialAttr>(ret, sqlR, sqlCtx);
                }
                material.GetType().GetField("_materialAttrList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(material, ret);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillMaterialAttrLog(Material material)
        {
            try
            {
                IList<MaterialAttrLog> ret = new List<MaterialAttrLog>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.MaterialAttrLog cond = new _Metas.MaterialAttrLog();
                        cond.materialCT = material.MaterialCT;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialAttrLog>(tk, null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.MaterialAttrLog>(new Metas.EqualCondition<_Metas.MaterialAttrLog>(cond)),
                                                                                                         Metas.MaterialAttrLog.fn_cdt);
                    }
                }
                sqlCtx.Param(Metas.MaterialAttrLog.fn_materialCT).Value = material.MaterialCT;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.MaterialAttrLog, MaterialAttrLog, MaterialAttrLog>(ret, sqlR, sqlCtx);
                }
                material.GetType().GetField("_materialAttrLogList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(material, ret);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region implement IMaterialRepository
        public void AddMultiMaterialCT(IList<string> materialCTList,
                                             string materialType,
                                              string lotNo,
                                              string stage,
                                              string line,
                                              string preStatus,
                                              string curStatus,
                                              string editor)
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
                        sqlCtx.Sentence = @"insert Material(MaterialCT, MaterialType, LotNo, Stage, Line, 
                                                                                    PreStatus, Status, Editor, Cdt, Udt)                
                                                                    select data, @MaterialType, @LotNo, @Stage, @Line, 
                                                                           @PreStatus, @Status, @Editor, GETDATE(), getdate()
                                                                     from @MaterialCTList";
                        
                        SqlParameter para = new SqlParameter("@MaterialCTList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("MaterialCTList", para);

                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));


                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialCTList").Value = SQLData.ToDataTable<string>(materialCTList);
                sqlCtx.Param("MaterialType").Value = materialType;
                sqlCtx.Param("LotNo").Value = lotNo;
                sqlCtx.Param("Stage").Value = stage;
                sqlCtx.Param("Line").Value = line;

                sqlCtx.Param("PreStatus").Value = preStatus;
                sqlCtx.Param("Status").Value = curStatus;

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

        public void AddMultiMaterialCTDefered(IUnitOfWork uow,
                                                            IList<string> materialCTList,
                                                           string materialType,
                                                            string lotNo,
                                                            string stage,
                                                            string line,
                                                            string preStatus,
                                                            string curStatus,
                                                            string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), 
                                            materialCTList, 
                                            materialType, 
                                            lotNo, 
                                            stage, 
                                            line, 
                                            preStatus,
                                            curStatus,
                                            editor);
        }


        public void AddMultiMaterialCT(IList<string> materialCTList,
                                             string materialType,
                                              string lotNo,
                                              string stage,
                                              string line,
                                              string preStatus,
                                              string curStatus,
                                              string model,
                                              string deliveryNo,
                                              string palletNo,
                                              string cartonSN,
                                              string pizzaId,
                                              string shipMode,  
                                              string editor)
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
//                        sqlCtx.Sentence = @"insert Material(MaterialCT, MaterialType, LotNo, Stage, Line, 
//                                                                                    PreStatus, Status, Model, DeliveryNo,PalletNo,
//                                                                                    CartonSN, PizzaID,ShipMode, Editor, Cdt, 
//                                                                                    Udt)                
//                                                                    select data, @MaterialType, @LotNo, @Stage, @Line, 
//                                                                                        @PreStatus, @Status,  @Model, @DeliveryNo,@PalletNo,
//                                                                                        @CartonSN,@PizzaID,@ShipMode,@Editor, GETDATE(), 
//                                                                                        getdate()
//                                                                     from @MaterialCTList";
                        sqlCtx.Sentence = @"merge Material as T
                                                        using (select data as MaterialCT, @MaterialType as MaterialType, @LotNo as LotNo , @Stage as Stage, @Line as Line, 
                                                                 @PreStatus as PreStatus, @Status as Status , @Model as Model, @DeliveryNo as DeliveryNo ,@PalletNo as PalletNo,
                                                                @CartonSN as CartonSN,@PizzaID as PizzaID,@ShipMode as ShipMode,@Editor as Editor
                                                               from @MaterialCTList) as S
                                                        on(T.MaterialCT = S.MaterialCT)
                                                        When MATCHED  Then       
                                                             Update 
	                                                          Set MaterialType= S.MaterialType,
		                                                          LotNo=S.LotNo,
		                                                          Stage=S.Stage,
		                                                          Line=S.Line,
		                                                          PreStatus=S.PreStatus,
		                                                          Status=S.Status,	
                                                                  Model=S.Model,
                                                                  DeliveryNo=S.DeliveryNo,
                                                                  PalletNo= S.PalletNo,
                                                                 CartonSN= S.CartonSN,	 
                                                                 PizzaID= S.PizzaID,
                                                                 ShipMode= S.ShipMode, 	
		                                                          Udt = getdate(),
		                                                          Editor = S.Editor 
                                                        When NOT MATCHED  Then
	                                                        insert (MaterialCT, MaterialType, LotNo, Stage, Line, 
                                                                                  PreStatus, Status, Model, DeliveryNo,PalletNo,
                                                                                   CartonSN, PizzaID,ShipMode, Editor, Cdt,  
                                                                                       Udt)                
                                                            Values(S.MaterialCT, S.MaterialType, S.LotNo, S.Stage, S.Line, 
                                                                        S.PreStatus, S.Status, S.Model, S.DeliveryNo, S.PalletNo,
                                                                         S.CartonSN, S.PizzaID,S.ShipMode, S.Editor, getdate(), getdate());";

                        SqlParameter para = new SqlParameter("@MaterialCTList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("MaterialCTList", para);

                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));

                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));

                        sqlCtx.AddParam("PalletNo", new SqlParameter("@PalletNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("CartonSN", new SqlParameter("@CartonSN", SqlDbType.VarChar));

                        sqlCtx.AddParam("PizzaID", new SqlParameter("@PizzaID", SqlDbType.VarChar));
                        sqlCtx.AddParam("ShipMode", new SqlParameter("@ShipMode", SqlDbType.VarChar));


                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialCTList").Value = SQLData.ToDataTable<string>(materialCTList);
                sqlCtx.Param("MaterialType").Value = materialType;
                sqlCtx.Param("LotNo").Value = lotNo;
                sqlCtx.Param("Stage").Value = stage;
                sqlCtx.Param("Line").Value = line;

                sqlCtx.Param("PreStatus").Value = preStatus;
                sqlCtx.Param("Status").Value = curStatus;

                sqlCtx.Param("Model").Value = model;
                sqlCtx.Param("DeliveryNo").Value = deliveryNo;
                sqlCtx.Param("PalletNo").Value = palletNo;
                sqlCtx.Param("CartonSN").Value = cartonSN;


                sqlCtx.Param("PizzaID").Value = pizzaId;
                sqlCtx.Param("ShipMode").Value = shipMode;

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

        public void AddMultiMaterialCTDefered(IUnitOfWork uow,
                                                        IList<string> materialCTList,
                                                        string materialType,
                                                         string lotNo,
                                                         string stage,
                                                          string line,
                                                            string preStatus,
                                                            string curStatus,
                                                             string model,
                                                             string deliveryNo,
                                                             string palletNo,
                                                             string cartonSN,
                                                            string pizzaId,
                                                             string shipMode,
                                                             string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                           materialCTList,
                                           materialType,
                                           lotNo,
                                           stage,
                                           line,
                                           preStatus,
                                           curStatus,
                                           model,
                                           deliveryNo,
                                           palletNo,
                                           cartonSN,
                                           pizzaId,
                                           shipMode,
                                           editor);

        }


        public void AddMultiMaterialLog(IList<string> materialCTList,
                                                           string action,
                                                             string stage,
                                                             string line,
                                                             string preStatus,
                                                             string curStatus,
                                                             string comment,
                                                             string editor)
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

                        sqlCtx.Sentence = @"insert MaterialLog(MaterialCT, Action, Stage, Line, PreStatus, 
				                                                                            Status,Comment, Editor, Cdt)                
                                                                    select data,  @Action, @Stage, @Line,@PreStatus, 
                                                                            @Status, @Comment, @Editor,  getdate()
                                                                     from @MaterialCTList";

                        SqlParameter para = new SqlParameter("@MaterialCTList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("MaterialCTList", para);
                       
                        sqlCtx.AddParam("Action", new SqlParameter("@Action", SqlDbType.VarChar));
                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("Comment", new SqlParameter("@Comment", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialCTList").Value = SQLData.ToDataTable<string>(materialCTList);
                sqlCtx.Param("Action").Value = action;
                sqlCtx.Param("Stage").Value = stage;
                sqlCtx.Param("Line").Value = line;

                sqlCtx.Param("PreStatus").Value = preStatus;
                sqlCtx.Param("Status").Value = curStatus;
                sqlCtx.Param("Comment").Value = comment;

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

        public void AddMultiMaterialLogDefered(IUnitOfWork uow,
                                                                        IList<string> materialCTList,
                                                                        string action,
                                                                        string stage,
                                                                        string line,
                                                                        string preStatus,
                                                                        string curStatus,
                                                                        string comment,
                                                                        string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                            materialCTList,
                                            action,
                                            stage,
                                            line,
                                            preStatus,
                                            curStatus,
                                            comment,
                                            editor);
        }

        public void UpdateMultiMaterialStatus(IList<string> materialCTList,
                                                          string preStatus,
                                                          string curStatus,
                                                           string editor)
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
                        sqlCtx.Sentence = @"update Material
                                                                set PreStatus=@PreStatus,
                                                                      Status = @Status,
                                                                      Editor = @Editor,
                                                                      Udt = GETDATE()
                                                             from Material a 
                                                             inner join @MaterialCTList b on a.MaterialCT = b.data ";

                        SqlParameter para = new SqlParameter("@MaterialCTList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("MaterialCTList", para);

                        //sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        //sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));


                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialCTList").Value = SQLData.ToDataTable<string>(materialCTList);
                //sqlCtx.Param("MaterialType").Value = materialType;
                //sqlCtx.Param("LotNo").Value = lotNo;
                //sqlCtx.Param("Stage").Value = stage;
                //sqlCtx.Param("Line").Value = line;

                sqlCtx.Param("PreStatus").Value = preStatus;
                sqlCtx.Param("Status").Value = curStatus;

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

        public void UpdateMultiMaterialStatusDefered(IUnitOfWork uow,
                                                                       IList<string> materialCTList,
                                                                       string preStatus,
                                                                       string curStatus,
                                                                       string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                            materialCTList,                                           
                                            preStatus,
                                            curStatus,                                            
                                            editor);
        }

        public void UpdateMultiMaterialCurStatus(IList<string> materialCTList,
                                                                string curStatus,
                                                                string editor)
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
                        sqlCtx.Sentence = @"update Material
                                                                set PreStatus=Status,
                                                                      Status = @Status,
                                                                      Editor = @Editor,
                                                                      Udt = GETDATE()
                                                             from Material a 
                                                             inner join @MaterialCTList b on a.MaterialCT = b.data ";

                        SqlParameter para = new SqlParameter("@MaterialCTList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("MaterialCTList", para);

                        //sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        //sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

                        //sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));


                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialCTList").Value = SQLData.ToDataTable<string>(materialCTList);
                //sqlCtx.Param("MaterialType").Value = materialType;
                //sqlCtx.Param("LotNo").Value = lotNo;
                //sqlCtx.Param("Stage").Value = stage;
                //sqlCtx.Param("Line").Value = line;

                //sqlCtx.Param("PreStatus").Value = preStatus;
                sqlCtx.Param("Status").Value = curStatus;

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

        public void UpdateMultiMaterialCurStatusDefered(IUnitOfWork uow,
                                                                            IList<string> materialCTList,
                                                                           string curStatus,
                                                                          string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                            materialCTList,                                          
                                            curStatus,
                                            editor);
        }


        public void AddMultiMaterialCurStatusLog(IList<string> materialCTList,
                                                           string action,
                                                             string stage,
                                                             string line,                                                           
                                                             string curStatus,
                                                             string comment,
                                                             string editor)
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

                        sqlCtx.Sentence = @"insert MaterialLog(MaterialCT, Action, Stage, Line, PreStatus, 
				                                                                            Status,Comment, Editor, Cdt)                
                                                                    select data,  @Action, @Stage, @Line, a.PreStatus, 
                                                                            @Status, @Comment, @Editor,  getdate()
                                                                     from  Material a, @MaterialCTList b
                                                                    where a.MaterialCT = b.data";

                        SqlParameter para = new SqlParameter("@MaterialCTList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("MaterialCTList", para);

                        sqlCtx.AddParam("Action", new SqlParameter("@Action", SqlDbType.VarChar));
                        sqlCtx.AddParam("Stage", new SqlParameter("@Stage", SqlDbType.VarChar));
                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));

                        sqlCtx.AddParam("PreStatus", new SqlParameter("@PreStatus", SqlDbType.VarChar));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("Comment", new SqlParameter("@Comment", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("MaterialCTList").Value = SQLData.ToDataTable<string>(materialCTList);
                sqlCtx.Param("Action").Value = action;
                sqlCtx.Param("Stage").Value = stage;
                sqlCtx.Param("Line").Value = line;

                //sqlCtx.Param("PreStatus").Value = PreStatus;
                sqlCtx.Param("Status").Value = curStatus;
                sqlCtx.Param("Comment").Value = comment;

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

        public void AddMultiMaterialCurStatusLogDefered(IUnitOfWork uow,
                                                                        IList<string> materialCTList,
                                                                        string action,
                                                                        string stage,
                                                                        string line,                                                                        
                                                                        string curStatus,
                                                                        string comment,
                                                                        string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                            materialCTList,
                                            action,
                                            stage,
                                            line,                                          
                                            curStatus,
                                            comment,
                                            editor);
        }


        public void UpdateDnPalletbyMultiCartonSN(IList<string> cartonSNList, 
                                                                            string deliveryNo, 
                                                                            string palletNo,
                                                                            string preStatus, 
                                                                            string status,
                                                                            string line,
                                                                            string editor)
        {
            try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 Material item = new Material()
                 {
                      DeliveryNo=deliveryNo,
                      PalletNo=palletNo,
                      PreStatus =preStatus,
                     Status = status,
                     Line =line,
                     Editor = editor
                 };

                 lock (mthObj)
                 {

                     _Metas.Material cond = new _Metas.Material();
                     cond.cartonSN = "[INSET]";

                     _Metas.Material setv = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(item, _Metas.Material.fn_cartonSN);
                     setv.udt = DateTime.Now;

                     sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.Material>(new _Metas.SetValueCollection<_Metas.Material>(new _Metas.CommonSetValue<_Metas.Material>(setv)),
                                                                                                                                  new _Metas.ConditionCollection<_Metas.Material>(new Metas.InSetCondition<_Metas.Material>(cond)));

                 }                 

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(sqlCtx, item, true);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(g.DecSV(_Metas.Material.fn_udt)).Value = cmDt;

                 string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Material.fn_cartonSN), g.ConvertInSet(cartonSNList));

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


        public void UpdateDnPalletbyMultiCartonSNDefered(IUnitOfWork uow, 
                                                                                    IList<string> cartonSNList, 
                                                                                    string deliveryNo, 
                                                                                    string palletNo, 
                                                                                    string preStatus, 
                                                                                    string status,
                                                                                    string line, 
                                                                                    string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                            cartonSNList,
                                            deliveryNo,
                                            palletNo,
                                            preStatus, 
                                            status,
                                            line,
                                            editor);
        }

        public void UpdateDnPalletbyMultiCT(IList<string> materialCTList,
                                                              string deliveryNo,
                                                              string palletNo,
                                                              string cartonSN,
                                                              string preStatus,
                                                              string status,
                                                               string line,
                                                               string editor)
        {
            try
             {
                 MethodBase mthObj = MethodBase.GetCurrentMethod();
                 int tk = mthObj.MetadataToken;
                 Metas.SQLContextNew sqlCtx = null;

                 Material item = new Material()
                 {
                      DeliveryNo=deliveryNo,
                      PalletNo=palletNo,
                      CartonSN =cartonSN,
                      PreStatus =preStatus,
                     Status = status,
                     Line =line,
                     Editor = editor
                 };

                 lock (mthObj)
                 {

                     _Metas.Material cond = new _Metas.Material();
                     cond.materialCT = "[INSET]";

                     _Metas.Material setv = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(item, _Metas.Material.fn_materialCT);
                     setv.udt = DateTime.Now;

                     sqlCtx = _Metas.FuncNew.GetConditionedUpdate<_Metas.Material>(new _Metas.SetValueCollection<_Metas.Material>(new _Metas.CommonSetValue<_Metas.Material>(setv)),
                                                                                                                                  new _Metas.ConditionCollection<_Metas.Material>(new Metas.InSetCondition<_Metas.Material>(cond)));

                 }                 

                 sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(sqlCtx, item, true);
                 DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                 sqlCtx.Param(g.DecSV(_Metas.Material.fn_udt)).Value = cmDt;

                 string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Material.fn_materialCT), g.ConvertInSet(materialCTList));

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
        public void UpdateDnPalletbyMultiCTDefered(IUnitOfWork uow,
                                                                            IList<string> materialCTList,
                                                                            string deliveryNo,
                                                                            string palletNo,
                                                                            string cartonSN,
                                                                            string preStatus,
                                                                            string status,
                                                                             string line,
                                                                            string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                           materialCTList,
                                           deliveryNo,
                                           palletNo,
                                           cartonSN,
                                           preStatus,
                                           status,
                                           line,
                                           editor);
        }

       #endregion

       #region Query function
        public IList<Material> GetMaterialByType(string materialType)
        {
            try
            {
                #region mark SQLStatement
                //                IList<Material> ret = new List<Material>();
//                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                Metas.SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select MaterialCT, MaterialType, LotNo, Stage, Line, 
//                                                                       PreStatus, Status, Editor, Cdt, Udt
//                                                                from Material
//                                                              where MaterialType = @MaterialType
//                                                             order by MaterialCT";
//                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }

//                sqlCtx.Param("MaterialType").Value = materialType;

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    while (sqlR != null && sqlR.Read())
//                    {
//                        Material item = SQLData.ToObject<Material>(sqlR);
//                        item.Tracker.Clear();
//                        ret.Add(item);

//                    }
//                }
                //                return ret;
                #endregion

                IList<Material> ret = new List<Material>();

               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               Metas.SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       _Metas.Material cond = new _Metas.Material();
                       cond.materialType = materialType;                      
                       sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.Material>(tk, null, null, 
                                                                                                        new Metas.ConditionCollection<_Metas.Material>(new Metas.EqualCondition<_Metas.Material>(cond)), 
                                                                                                        Metas.Material.fn_materialCT);
                   }
               }
               sqlCtx.Param(Metas.Material.fn_materialType).Value = materialType;


               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, 
                                                                                                                    CommandType.Text, 
                                                                                                                    sqlCtx.Sentence, 
                                                                                                                    sqlCtx.Params))
               {
                   ret = Metas.FuncNew.SetFieldFromColumn<_Metas.Material, Material,Material>(ret, sqlR, sqlCtx);
               }
               return ret;
           }
           catch (Exception)
           {
               throw;
           }           
        }

        public IList<Material> GetMaterialByLotNo(string materialType, string lotNo)
        {
            try
            {
                #region mark SQL
                //                IList<Material> ret = new List<Material>();
//                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                Metas.SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select MaterialCT, MaterialType, LotNo, Stage, Line, 
//                                                                       PreStatus, Status, Editor, Cdt, Udt
//                                                                from Material
//                                                              where MaterialType = @MaterialType       and 
//                                                                         LotNo = @LotNo
//                                                             order by MaterialCT";
//                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }

//                sqlCtx.Param("MaterialType").Value = materialType;
//                sqlCtx.Param("LotNo").Value = lotNo;

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    while (sqlR != null && sqlR.Read())
//                    {
//                        Material item = SQLData.ToObject<Material>(sqlR);
//                        item.Tracker.Clear();
//                        ret.Add(item);

//                    }
//                }
                //                return ret;
                #endregion
                IList<Material> ret = new List<Material>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Material cond = new _Metas.Material();
                        cond.materialType = materialType;
                        cond.lotNo = lotNo;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.Material>(tk, null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.Material>(new Metas.EqualCondition<_Metas.Material>(cond)),
                                                                                                         Metas.Material.fn_materialCT);
                    }
                }
                sqlCtx.Param(Metas.Material.fn_materialType).Value = materialType;
                sqlCtx.Param(Metas.Material.fn_lotNo).Value = materialType;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.Material, Material, Material>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IList<Material> GetMaterial(Material condition)
        {
            try
            {
                
                IList<Material> ret = new List<Material>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.Material cond = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(condition, _Metas.Material.fn_cartonWeight, _Metas.Material.fn_unitWeight);
                      sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.Material>(null, null,
                                                                                                         new Metas.ConditionCollection<_Metas.Material>(new Metas.EqualCondition<_Metas.Material>(cond)),
                                                                                                         Metas.Material.fn_materialCT);
                   
                }

                sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(sqlCtx, condition);


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                     CommandType.Text,
                                                                                                                     sqlCtx.Sentence,
                                                                                                                     sqlCtx.Params))
                {
                    ret = Metas.FuncNew.SetFieldFromColumn<_Metas.Material, Material, Material>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }
       public int GetCombinedMaterialLotQty(string materialType,string lotNo)
        {
            try
            {
               int ret = 0;
               #region mark SQL
               //                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                Metas.SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new Metas.SQLContextNew();
//                        sqlCtx.Sentence = @"select count(1) as Qty
//                                                              from Material
//                                                              where MaterialType = @MaterialType  and 
//                                                                         LotNo = @LotNo";
//                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
//                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
//                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }

//                sqlCtx.Param("MaterialType").Value = materialType;
               //                sqlCtx.Param("LotNo").Value = lotNo;
               #endregion

               MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        _Metas.Material cond = new _Metas.Material();
                        cond.materialType = materialType;
                        cond.lotNo = lotNo;
                        sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.Material>(tk, "COUNT", new string[]{ _Metas.Material.fn_materialCT},
                                                                                                         new Metas.ConditionCollection<_Metas.Material>(new Metas.EqualCondition<_Metas.Material>(cond)),
                                                                                                        null);
                    }
                }
                sqlCtx.Param(Metas.Material.fn_materialType).Value = materialType;
                sqlCtx.Param(Metas.Material.fn_lotNo).Value = lotNo;

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

       public IList<MaterialLotQtyInfo> GetMaterialLotQtyGroupStatus(string materialType, string lotNo)
        {
            try
            {
                IList<MaterialLotQtyInfo> ret = new List<MaterialLotQtyInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                Metas.SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new Metas.SQLContextNew();
                        sqlCtx.Sentence = @"select @LotNo as LotNo, Status, count(1) as Qty
                                                                from Material
                                                              where MaterialType = @MaterialType  and
                                                                            LotNo = @LotNo
                                                             Group By Status";

                        sqlCtx.AddParam("LotNo", new SqlParameter("@LotNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("MaterialType", new SqlParameter("@MaterialType", SqlDbType.VarChar));
                        Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("MaterialType").Value = materialType;
                sqlCtx.Param("LotNo").Value = lotNo;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        MaterialLotQtyInfo item = SQLData.ToObject<MaterialLotQtyInfo>(sqlR);                       
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


       public IList<Material> GetMaterialByMultiCT(IList<string> materialCTList)
       {
           try
           {

               IList<Material> ret = new List<Material>();

               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               Metas.SQLContextNew sqlCtx = null;
               lock (mthObj)
               {
                   _Metas.Material cond = new _Metas.Material();
                   cond.materialCT = "[INSET]";
                   
                   sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.Material>(tk,null, null,
                                                                                                      new Metas.ConditionCollection<_Metas.Material>(new Metas.InSetCondition<_Metas.Material>(cond)),
                                                                                                      Metas.Material.fn_materialCT);

               }

               string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(_Metas.Material.fn_materialCT), g.ConvertInSet(materialCTList));


               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                    CommandType.Text,
                                                                                                                   Sentence,
                                                                                                                    sqlCtx.Params))
               {
                   ret = Metas.FuncNew.SetFieldFromColumn<_Metas.Material, Material, Material>(ret, sqlR, sqlCtx);
               }
               return ret;
           }
           catch (Exception)
           {
               throw;
           }

       }

       public IList<string> GetMaterialCTbyAttribute(string attrName, string attrValue)
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
                         _Metas.MaterialAttr cond = new _Metas.MaterialAttr();
                         cond.attrName = attrName;
                         cond.attrValue = attrValue;
                         sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.MaterialAttr>(tk, "Distinct", new string[] { _Metas.MaterialAttr.fn_materialCT},
                                                                                                          new Metas.ConditionCollection<_Metas.MaterialAttr>(new Metas.EqualCondition<_Metas.MaterialAttr>(cond)));
                     }
                 }
                 sqlCtx.Param(Metas.MaterialAttr.fn_attrName).Value = attrName;
                 sqlCtx.Param(Metas.MaterialAttr.fn_attrValue).Value = attrValue;


                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                      CommandType.Text,
                                                                                                                      sqlCtx.Sentence,
                                                                                                                      sqlCtx.Params))
                 {
                     while (sqlR.Read())
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


       public int GetCombinedMaterialQty(string materialType, string cartonSn, string deliveryNo, string palletNo)
       {
           try
           {
               int ret = 0;

               MethodBase mthObj = MethodBase.GetCurrentMethod();
               int tk = mthObj.MetadataToken;
               Metas.SQLContextNew sqlCtx = null;

               Material condititon = new Material()
               {
                    MaterialType =materialType,
                    CartonSN =cartonSn,
                    DeliveryNo =deliveryNo,
                    PalletNo = palletNo
               };
               

               lock (mthObj)
               {
                   if (!Metas.SQLCache.PeerTheSQL(tk, out sqlCtx))
                   {
                       _Metas.Material cond = new _Metas.Material();
                       cond.materialType = materialType;
                       cond.cartonSN = cartonSn;
                       cond.deliveryNo = deliveryNo;
                       cond.palletNo = palletNo;
                       sqlCtx = Metas.FuncNew.GetConditionedSelect<_Metas.Material>( "Distinct", new string[] { _Metas.Material.fn_materialCT },
                                                                                                        new Metas.ConditionCollection<_Metas.Material>(new Metas.EqualCondition<_Metas.Material>(cond)));
                   }
               }

               sqlCtx = _Metas.FuncNew.SetColumnFromField<_Metas.Material, Material>(sqlCtx, condititon);

               using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                    CommandType.Text,
                                                                                                                    sqlCtx.Sentence,
                                                                                                                    sqlCtx.Params))
               {
                   if (sqlR != null )
                   {
                       while(sqlR.Read())
                       {
                       ret++;
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
                                                        from    Material
                                                        where  DeliveryNo =@DeliveryNo and
                                                                   CartonSN!='' and
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

       //Unpack By Carton /Delivery
       public void UnpackByCatonSN(string cartonSN,                                                    
                                                    string status,
                                                    string editor)
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
                       sqlCtx.Sentence = @"update  Material
                                                            set  PizzaID='',
                                                                   CartonWeight=0,
                                                                   CartonSN='',
                                                                   PalletNo='',
                                                                   DeliveryNo='',
                                                                   PreStatus  =Status,
                                                                   Status= @Status,  
                                                                   Editor =@Editor,                                                               
                                                                    Udt = GETDATE()
                                                                where   CartonSN= @CartonSN ";

                       sqlCtx.AddParam("CartonSN", new SqlParameter("@CartonSN", SqlDbType.VarChar));
                       sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                       sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                       Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("CartonSN").Value = cartonSN;
               sqlCtx.Param("Status").Value = status;
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

       public void UnpackByCatonSNDefered(IUnitOfWork uow,
                                                        string cartonSN,
                                                        string status,
                                                        string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     cartonSN,
                                                     status,
                                                     editor);
       }

       public void UnpackByDeliveryNo(string deliveryNo,
                                               string status,
                                               string editor)
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
                       sqlCtx.Sentence = @"update  Material
                                                            set  PizzaID='',
                                                                   CartonWeight=0,
                                                                   CartonSN='',
                                                                   PalletNo='',
                                                                   DeliveryNo='',
                                                                   PreStatus  =Status,
                                                                   Status= @Status,  
                                                                   Editor =@Editor,                                                               
                                                                    Udt = GETDATE()
                                                                where   DeliveryNo= @DeliveryNo ";

                       sqlCtx.AddParam("DeliveryNo", new SqlParameter("@DeliveryNo", SqlDbType.VarChar));
                       sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                       sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                       Metas.SQLCache.InsertIntoCache(tk, sqlCtx);
                   }
               }

               sqlCtx.Param("DeliveryNo").Value = deliveryNo;
               sqlCtx.Param("Status").Value = status;
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

       public void UnpackByDeliveryNoDefered(IUnitOfWork uow,
                                                           string deliveryNo,
                                                           string status,
                                                           string editor)
       {
           AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),
                                                     deliveryNo,
                                                     status,
                                                     editor);
       }
       #endregion

       #region Private Check Delivery Qty
       private int getDeliveryQtyOnTrans(string deliveryNo)
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
                       sqlCtx.Sentence = @"select  a.Qty
                                                         from Delivery a WITH (INDEX=Delivery_PK,ROWLOCK,UPDLOCK)
                                                          where a.DeliveryNo =@DeliveryNo";

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
                       sqlCtx.Sentence = @"select COUNT(MaterialCT) as Qty 
                                                        from Material
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
