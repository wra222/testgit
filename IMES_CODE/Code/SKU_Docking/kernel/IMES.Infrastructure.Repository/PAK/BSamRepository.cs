using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.FisObjectBase;
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Util;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Metas;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;


namespace IMES.Infrastructure.Repository.PAK
{
    public class BSamRepository : BaseRepository<BSamLocation>, IBSamRepository
    {
        #region Overrides of BaseRepository<BSamLocation>

        protected override void PersistNewItem(BSamLocation item)
        {
            StateTracker tracker = item .Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                  
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(BSamLocation item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    //this.PersistUpdateBSamLoc(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(BSamLocation item)
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

        #region Implementation of IRepository<BSamLocation>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override BSamLocation Find(object key)
        {
            try
            {
                BSamLocation ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select LocationId, Model, Qty, RemainQty, FullQty, 
                                                                       FullCartonQty, HoldInput, HoldOutput, Editor, Cdt, 
                                                                       Udt
                                                                from BSamLocation
                                                            where LocationId=@LocationId";
                        sqlCtx.AddParam("LocationId", new SqlParameter("@LocationId", SqlDbType.VarChar));
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("LocationId").Value = (string) key;
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                       ret = SQLData.ToReadOnlyObject<BSamLocation>(sqlR);
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
        public override IList<BSamLocation> FindAll()
        {
            try
            {
                IList<BSamLocation> ret = new List<BSamLocation>() ;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select LocationId, Model, Qty, RemainQty, FullQty, 
                                                                       FullCartonQty, HoldInput, HoldOutput, Editor, Cdt, 
                                                                       Udt
                                                                from BSamLocation";                      
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
                        BSamLocation item = SQLData.ToReadOnlyObject<BSamLocation>(sqlR);
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
        public override void Add(BSamLocation item, IUnitOfWork uow)
        {
            //Don't need insert location
            //base.Add(item, uow);
            throw new Exception("Not Allow insert data");
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(BSamLocation item, IUnitOfWork uow)
        {
             //Don't need remove location
            //base.Remove(item, uow);
            throw new Exception("Not Allow Delete Data");
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(BSamLocation item, IUnitOfWork uow)
        {
            //base.Update(item, uow);
            throw new Exception("Not Allow Update Data");
        }


       
        #endregion

        #region . Inners .
//        private void PersistUpdateBSamLoc(BSamLocation item)
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
//                        sqlCtx.Sentence = @"update BSamLocation
//                                                            set Model=case when (Qty+@Qty)>0 then @Model
//                                                                                       else  ''
//                                                                              end ,
//                                                                Qty= case when (Qty+@Qty)<0 then 0
//                                                                                  when  (Qty+@Qty) >@FullQty then @FullQty                              
//                                                                         else   Qty+@Qty
//                                                                         end ,
//                                                                RemainQty = case when (Qty+@Qty)<0 then @FullQty 
//                                                                                                when  (Qty+@Qty) >@FullQty then 0 
//                                                                                                 else   @FullQty- Qty-@Qty
//                                                                                                  end ,  
//                                                                FullQty = case when (Qty+@Qty)>0 then @FullQtyPerCarton*FullCartonQty
//                                                                                else  0
//                                                                                 end,
//                                                                --FullCartonQty = @FullCartonQty,
//                                                                HoldInput = @HoldInput,
//                                                                HoldOutput = @HoldOutput,
//                                                                Editor =@Editor,
//                                                                Udt=GETDATE()
//                                                            where LocationId =  @LocationId";
//                        sqlCtx.AddParam("LocationId", new SqlParameter("@LocationId", SqlDbType.Int));
//                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
//                        //sqlCtx.AddParam("RemainQty", new SqlParameter("@RemainQty", SqlDbType.Int));
//                        //sqlCtx.AddParam("FullQty", new SqlParameter("@FullQty", SqlDbType.Int));
//                        //sqlCtx.AddParam("FullCartonQty", new SqlParameter("@FullCartonQty", SqlDbType.Int));
//                        sqlCtx.AddParam("FullQtyPerCarton", new SqlParameter("@FullQtyPerCarton", SqlDbType.Int));
//                        sqlCtx.AddParam("HoldInput", new SqlParameter("@HoldInput", SqlDbType.VarChar));
//                        sqlCtx.AddParam("HoldOutput", new SqlParameter("@HoldOutput", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                       
//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("LocationId").Value = item.LocationId;
//                sqlCtx.Param("Model").Value = item.Model;
//                sqlCtx.Param("Qty").Value = item.MovingCount;
//                //sqlCtx.Param("RemainQty").Value = item.RemainQty;
//                //sqlCtx.Param("FullQty").Value = item.FullQty;
//                //sqlCtx.Param("FullCartonQty").Value = item.FullCartonQty;
//                sqlCtx.Param("HoldInput").Value = item.HoldInput;
//                sqlCtx.Param("HoldOutput").Value = item.HoldOutput;
//                sqlCtx.Param("Editor").Value = item.Editor;
              
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
        #endregion

        #region implement IBSamRepository
        public BSamLocation GetMoveInLoc(string model)
        {
             try
            {
                BSamLocation ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"select top 1 LocationId, Model, Qty, RemainQty, FullQty, 
//                                                                       FullCartonQty, HoldInput, HoldOutput, Editor, Cdt, 
//                                                                       Udt
//                                                               from BSamLocation
//                                                                where (exists( select LocationId 
//				                                                                        from BSamLocation 
//				                                                                        where Model =@Model and
//	               	                                                                          RemainQty>0) and
//	                                                                            Model =@Model and
//	                                                                            RemainQty>0) and
//	                                                                    HoldInput ='N' or
//	                                                                    Model='' 
//                                                                order by Model desc,RemainQty,LocationId";
                        sqlCtx.Sentence = @"select top 1 LocationId, Model, Qty, RemainQty, FullQty, 
                                                                       FullCartonQty, HoldInput, HoldOutput, Editor, Cdt, 
                                                                       Udt
                                                               from BSamLocation
                                                                where (Model =@Model and
	                                                                            RemainQty>0 and
                                                                              HoldInput ='N')  or
	                                                                       (  Model=''            and
                                                                               HoldInput ='N')   
                                                                order by Model desc, RemainQty, LocationId";
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                       SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Model").Value = model;
                
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToReadOnlyObject<BSamLocation>(sqlR);
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

        public void MoveInLoc(string model, int fullQtyPerCarton, string editor, BSamLocation loc)
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
                        sqlCtx.Sentence = @"update BSamLocation
                                                                set Model = case when Model='' then @bindModel
                                                                                 else Model
                                                                            end,				 
                                                                    Qty= Qty+1,
                                                                    RemainQty = (case when Model='' then FullCartonQty *@fullQtyPerCarton
                                                                                      else RemainQty
                                                                                 end)-1,
                                                                    FullQty = case when Model='' then FullCartonQty *@fullQtyPerCarton
                                                                                 else FullQty
                                                                              end, 
                                                                    Editor = @editor,
                                                                    Udt =getdate()
                                                                output INSERTED.*	
                                                                where LocationId=@locationId and
                                                                           (Model='' or
	                                                                         FullQty > Qty)";
                       
                        sqlCtx.AddParam("bindModel", new SqlParameter("@bindModel", SqlDbType.VarChar));
                        sqlCtx.AddParam("fullQtyPerCarton", new SqlParameter("@fullQtyPerCarton", SqlDbType.Int));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("locationId", new SqlParameter("@locationId", SqlDbType.VarChar));



                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("locationId").Value = loc.LocationId;
                sqlCtx.Param("bindModel").Value = model;
                sqlCtx.Param("fullQtyPerCarton").Value = fullQtyPerCarton;
                sqlCtx.Param("editor").Value = editor;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                             CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                              sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        SQLData.ToReadOnlyObject<BSamLocation>(sqlR, ref loc);                        
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void MoveInLocDefered(IUnitOfWork uow, string model, int fullQtyPerCarton, string editor, BSamLocation loc)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model, fullQtyPerCarton, editor, loc);
        }

        public BSamLocation GetAndAssignMoveInLoc(string model, int fullCartonQty, string editor)
        {
            try
            {
                BSamLocation ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"with cte as (select top 1 LocationId, Model, Qty, RemainQty, FullQty, 
                                                                                               FullCartonQty, HoldInput, HoldOutput, Editor, Cdt, 
                                                                                               Udt
                                                                                       from BSamLocation
                                                                                        where (Model =@Model and
	                                                                                                RemainQty>0 and
                                                                                                    HoldInput ='N')  or
	                                                                                              (Model=''            and
                                                                                                    HoldInput ='N')   
                                                                                    order by Model desc, RemainQty, LocationId)                                                                                      
                                                                            update cte           
                                                                            set  Model=(case Model 
                                                                                                 when '' then @Model
                                                                                                 else
                                                                                                         Model
                                                                                                  end),            
                                                                                     RemainQty = (case Model 
                                                                                                             when '' then @fullCartonQty * FullCartonQty
                                                                                                             else RemainQty
                                                                                                             end)-1,
                                                                                     Qty= Qty+1,
                                                                                     FullQty = (case Model 
                                                                                                     when '' then @fullCartonQty * FullCartonQty
                                                                                                     else FullQty
                                                                                                     end),
                                                                                     Editor=@Editor,
                                                                                     Udt = getdate()            
                                                                             output inserted.*;";
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("fullCartonQty", new SqlParameter("@fullCartonQty", SqlDbType.Int));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Model").Value = model;
                sqlCtx.Param("fullCartonQty").Value = fullCartonQty;
                 sqlCtx.Param("Editor").Value = editor;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                             CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                              sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToReadOnlyObject<BSamLocation>(sqlR);
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
        public void RollbackMoveInLoc(string locationId, int qty, string editor)
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
                        sqlCtx.Sentence = @"Update BSamLocation
                                                            set  Qty=case when Qty-@qty>0 then Qty-@qty
                                                                                    else 0
                                                                           end,
                                                                   Model =  case when Qty-@qty>0 then Model
                                                                                    else ''
                                                                                    end,
                                                                    RemainQty = case when Qty-@qty>0 then RemainQty+@qty
                                                                                           else 0
                                                                                           end,
                                                                    FullQty =case when Qty-@qty>0 then FullQty
                                                                                    else 0
                                                                                    end,
                                                                     Editor=@editor,
                                                                     Udt = getdate() 
                                                            where LocationId=@locationId";
                        sqlCtx.AddParam("locationId", new SqlParameter("@locationId", SqlDbType.VarChar));
                        sqlCtx.AddParam("qty", new SqlParameter("@qty", SqlDbType.Int));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("locationId").Value = locationId;
                sqlCtx.Param("qty").Value = qty;
                sqlCtx.Param("editor").Value = editor;

                 SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params);
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RollbackMoveInLocDefered(IUnitOfWork uow, string locationId, int qty, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), locationId,qty, editor);
        }

        public void MoveOutLoc(string locationId,string editor)
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
                        sqlCtx.Sentence = @"update BSamLocation
                                                                set Model = case when Qty >1 then Model
                                                                                             else ''
                                                                                        end,				 
                                                                    Qty= Qty-1,
                                                                    RemainQty =case when Qty >1 then RemainQty+1
                                                                                                   else 0
                                                                                           end,
                                                                    FullQty = case when Qty >1 then FullQty
                                                                                             else 0
                                                                                      end, 
                                                                    Editor = @editor,
                                                                    Udt =getdate()                                                           
                                                                where LocationId=@locationId and
                                                                           Qty > 0";

                        sqlCtx.AddParam("locationId", new SqlParameter("@locationId", SqlDbType.VarChar));
                        sqlCtx.AddParam("editor", new SqlParameter("@editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("locationId").Value = locationId;
                sqlCtx.Param("editor").Value = editor;


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

        public void MoveOutLocDefered(IUnitOfWork uow,string locationId, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), locationId, editor);
        }


        public void UpdateHoldInLocation(IList<string> Ids, bool isHold, string editor)
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
                        sqlCtx.Sentence = @"update BSamLocation
                                                            set HoldInput =@HoldInput,
                                                                  Udt =GETDATE(),
                                                                    Editor =@Editor       
                                                            from BSamLocation a 
                                                            inner join @Ids b on a.LocationId = b.data";

                        SqlParameter para = new SqlParameter("@Ids", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("Ids", para);    

                        sqlCtx.AddParam("HoldInput", new SqlParameter("@HoldInput", SqlDbType.VarChar));
                        
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
             
                sqlCtx.Param("Ids").Value = SQLData.ToDataTable<string>(Ids);
                sqlCtx.Param("HoldInput").Value = isHold?"Y":"N";
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

        public void UpdateHoldOutLocation(IList<string> Ids, bool isHold, string editor)
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
                        sqlCtx.Sentence = @"update BSamLocation
                                                            set HoldOutput =@HoldInput,
                                                                  Udt =GETDATE(),
                                                                    Editor =@Editor       
                                                            from BSamLocation a 
                                                            inner join @Ids b on a.LocationId = b.data";

                        SqlParameter para = new SqlParameter("@Ids", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        sqlCtx.AddParam("Ids", para);

                        sqlCtx.AddParam("HoldInput", new SqlParameter("@HoldInput", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Ids").Value = SQLData.ToDataTable<string>(Ids);
                sqlCtx.Param("HoldInput").Value = isHold ? "Y" : "N";
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

     

        public BSamModel GetBSamModel(string a_Part_Model)
        {
            try
            {
                BSamModel ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select A_Part_Model, C_Part_Model, HP_A_Part, HP_C_SKU, QtyPerCarton, 
                                                                      Editor, Cdt, Udt
                                                                from BSamModel
                                                            where A_Part_Model=@a_Part_Model";
                        sqlCtx.AddParam("a_Part_Model", new SqlParameter("@a_Part_Model", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("a_Part_Model").Value = a_Part_Model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToReadOnlyObject<BSamModel>(sqlR);
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

        public void UpdateBSamModelQtyPerCarton(string a_Part_Model, int qtyPerCarton, string editor)
        {
            try
            {
                BSamModel ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @" update BSamModel
                                                                     set QtyPerCarton = @QtyPerCarton,
                                                                         Editor =@Editor,
                                                                         Udt =getdate()
                                                                     where   A_Part_Model =@A_Part_Model";
                        sqlCtx.AddParam("A_Part_Model", new SqlParameter("@A_Part_Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("QtyPerCarton", new SqlParameter("@QtyPerCarton", SqlDbType.Int));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("A_Part_Model").Value = a_Part_Model;
                sqlCtx.Param("Editor").Value = editor;
                sqlCtx.Param("QtyPerCarton").Value = qtyPerCarton;

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

        public void UpdateBSamModelQtyPerCartonDefered(IUnitOfWork uow, string a_Part_Model, int qtyPerCarton, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), a_Part_Model, qtyPerCarton, editor);
        }


        public IList<BSamLocation> GetBSamLocation(BSamLocationQueryType type,string model)
        {
            try
            {
                IList<BSamLocation> ret = new List<BSamLocation>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select LocationId, Model, Qty, RemainQty, FullQty, 
                                                                       FullCartonQty, HoldInput, HoldOutput, Editor, Cdt, 
                                                                       Udt
                                                                from BSamLocation
                                                                {0}
                                                               order by LocationId";
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                string condition="";
                switch (type)
                {
                    case BSamLocationQueryType.Empty:
                        condition = "where Model='' ";
                        break;
                    case BSamLocationQueryType.Full:
                        condition = "where Qty>0 and RemainQty=0 ";
                        break;
                    case BSamLocationQueryType.Partial:
                        condition = "where Qty>0 and RemainQty!=0 ";
                        break;
                    case BSamLocationQueryType.Occupy:
                        condition = "where Qty>0 ";
                        break;
                    case BSamLocationQueryType.Hold:
                         condition = "where HoldInput='Y' or HoldOutput='Y' ";
                        break;
                    case BSamLocationQueryType.HoldIn:
                         condition = "where HoldInput='Y' ";
                        break;
                    case BSamLocationQueryType.HoldOut:
                         condition = "where HoldOutput='Y' ";
                        break;
                    case BSamLocationQueryType.Model:
                        condition = "where Model='"+model.Trim()+"'";
                        break;
                    default:
                        break;
                }
                string sqlStr = string.Format(sqlCtx.Sentence, condition);
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                           sqlStr,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        BSamLocation item = SQLData.ToReadOnlyObject<BSamLocation>(sqlR);
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


        public IList<string> GetAllBSamModel()
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
                        sqlCtx.Sentence = @"select distinct A_Part_Model
                                                            from BSamModel";
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
                        ret.Add(sqlR.GetString(0));
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

        #region Implement IBSamRepository method for BSamLocation Access
        public void FillProductIDInLoc(BSamLocation loc)
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
                        sqlCtx.Sentence = @"select ProductID
                                                                from ProductAttr
                                                            where AttrName='CartonLocation' and
                                                                       AttrValue=@locationId";
                        sqlCtx.AddParam("locationId", new SqlParameter("@locationId", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("locationId").Value = loc.LocationId.Trim();

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(sqlR.GetString(0));                        
                    }

                }
                loc.GetType().GetField("_productIDList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(loc, ret);
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
