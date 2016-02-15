using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Location;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: Line相关
    /// </summary>
    public class LocaionRepository : BaseRepository<fons.FloorAreaLoc>, fons.ILocationRepository
    {
        private static mtns::GetValueClass g = new mtns::GetValueClass();

         #region Overrides of BaseRepository<FloorAreaLoc>

        protected override void PersistNewItem(fons.FloorAreaLoc item)
        {
            throw new Exception("Not support Insert");
        }

        protected override void PersistUpdatedItem(fons.FloorAreaLoc item)
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

                        _Metas.FloorAreaLoc cond = new _Metas.FloorAreaLoc();
                        cond.locID = item.LocID;
                        _Metas.FloorAreaLoc setv = FuncNew.SetColumnFromField<_Metas.FloorAreaLoc, fons.FloorAreaLoc>(item, _Metas.FloorAreaLoc.fn_locID, _Metas.FloorAreaLoc.fn_floor, _Metas.FloorAreaLoc.fn_area, _Metas.FloorAreaLoc.fn_loc);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<_Metas.FloorAreaLoc>(new SetValueCollection<_Metas.FloorAreaLoc>(new CommonSetValue<_Metas.FloorAreaLoc>(setv)),
                                                                                                                  new ConditionCollection<_Metas.FloorAreaLoc>(new EqualCondition<_Metas.FloorAreaLoc>(cond)));

                    }
                }

                sqlCtx.Param(_Metas.FloorAreaLoc.fn_locID).Value = item.LocID;
                
                sqlCtx = FuncNew.SetColumnFromField<_Metas.FloorAreaLoc, fons.FloorAreaLoc>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(_Metas.FloorAreaLoc.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void PersistDeletedItem(fons.FloorAreaLoc item)
        {
            throw new Exception("Not Support Delete");
        }

        #endregion

        #region Implementation of IRepository<fons.FloorAreaLoc>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override fons.FloorAreaLoc Find(object key)
        {
            try
            {
                fons.FloorAreaLoc ret = null;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;              
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                     if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {

                        _Metas.FloorAreaLoc cond = new _Metas.FloorAreaLoc();
                        cond.locID = (string)key;                       
                        sqlCtx = FuncNew.GetConditionedSelect<_Metas.FloorAreaLoc>(null, null,
                                                                                      new ConditionCollection<_Metas.FloorAreaLoc>(new EqualCondition<_Metas.FloorAreaLoc>(cond)));

                     }
                }
                sqlCtx.Param(_Metas.FloorAreaLoc.fn_locID).Value = (string)key;
                

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.FloorAreaLoc, fons.FloorAreaLoc>(ret, sqlR, sqlCtx);
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
        public override IList<fons.FloorAreaLoc> FindAll()
        {
            try
            {
                IList<fons.FloorAreaLoc> ret = new List<fons.FloorAreaLoc>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
               SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<_Metas.FloorAreaLoc>(tk, _Metas.FloorAreaLoc.fn_locID);
                    }
                }
               
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.FloorAreaLoc, fons.FloorAreaLoc, fons.FloorAreaLoc>(ret, sqlR, sqlCtx);
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
        public override void Add(fons.FloorAreaLoc item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(fons.FloorAreaLoc item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(fons.FloorAreaLoc item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of ILocationRepository

        private string assignLoc(string floor, string area, string category, string model, string unit, string editor, int fullQty, string palletType)
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
                        sqlCtx.Sentence = @"with cte as (select top 1 LocID,Model,Unit,RemainQty,Qty,FullLocQty,
                                                                                              Status,Editor,Udt, Remark
                                                                                       from FloorAreaLoc WITH (INDEX=Idx_FloorAreaLoc_AssignLoc,ROWLOCK,UPDLOCK)
                                                                                        where  Floor=@Floor and
                                                                                                   Area=@Area and
                                                                                                   Unit in (@Unit,'')    and
                                                                                                   Category=@Category and 
                                                                                                   Model in (@Model , '') and
	                                                                                               Status in ('Empty','Partial') and
                                                                                                   HoldInput ='N'	                                                                                              
                                                                                    order by Qty desc, Loc)                                                                                      
                                                                            update cte           
                                                                            set  Model=(case isnull( cte.Model,'') 
                                                                                                 when '' then @Model
                                                                                                 else
                                                                                                          cte.Model
                                                                                                  end), 
                                                                                    Unit=(case isnull(Unit,'') 
                                                                                                 when '' then @Unit
                                                                                                 else
                                                                                                         Unit
                                                                                                  end),             
                                                                                     RemainQty = (case  cte.Status 
                                                                                                             when 'Empty' then @FullQty
                                                                                                             else  cte.RemainQty
                                                                                                             end)-1,
                                                                                     Qty= (case cte.Status 
                                                                                                     when 'Empty' then 0
                                                                                                     else Qty
                                                                                                     end)+1,
                                                                                     FullLocQty = (case cte.Status 
                                                                                                     when 'Empty' then @FullQty
                                                                                                     else  cte.FullLocQty
                                                                                                     end),
                                                                                     Status = (case  
                                                                                                    when cte.Status='Empty' and @FullQty=1  then 'Full'
                                                                                                    when cte.Status='Partial' and cte.RemainQty < 2 then 'Full'  
                                                                                                     else  'Partial'
                                                                                                     end),
                                                                                    Remark=(case isnull( cte.Remark,'')
                                                                                                    when '' then @Remark
                                                                                                    else  cte.Remark
                                                                                                    end),  
                                                                                     Editor=@Editor,
                                                                                     Udt = getdate()            
                                                                             output inserted.LocID;";
                        sqlCtx.AddParam("Floor", new SqlParameter("@Floor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Area", new SqlParameter("@Area", SqlDbType.VarChar));
                        sqlCtx.AddParam("Category", new SqlParameter("@Category", SqlDbType.VarChar));
                        sqlCtx.AddParam("Unit", new SqlParameter("@Unit", SqlDbType.VarChar));
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("FullQty", new SqlParameter("@FullQty", SqlDbType.Int));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Floor").Value = floor;
                sqlCtx.Param("Area").Value = area;
                sqlCtx.Param("Category").Value = category;
                sqlCtx.Param("Unit").Value = unit;
                sqlCtx.Param("Model").Value = model;
                sqlCtx.Param("FullQty").Value = fullQty;
                sqlCtx.Param("Editor").Value = editor;
                sqlCtx.Param("Remark").Value = palletType;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                             CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                              sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = ((string)sqlR[0]).Trim();
                       
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string assignEmptyLoc( fons.FloorAreaLoc srcLoc, string floor,  string area, string editor)
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
                        sqlCtx.Sentence = @"with cte as (select top 1 LocID,Model,Unit,RemainQty,Qty,FullLocQty,
                                                                                              Status,Editor,Udt, Remark
                                                                                       from FloorAreaLoc WITH (INDEX=Idx_FloorAreaLoc_AssignLoc,ROWLOCK,UPDLOCK)
                                                                                        where  Floor=@Floor and
                                                                                                   Area=@Area and
                                                                                                   Unit =''    and
                                                                                                   Category=@Category and 
                                                                                                   Model ='' and
	                                                                                               Status = 'Empty' and
                                                                                                   HoldInput ='N'	                                                                                              
                                                                                    order by Loc)                                                                                      
                                                                            update cte           
                                                                            set  Model=@Model,  
                                                                                  Unit =@Unit,           
                                                                                  RemainQty =@RemainQty,
                                                                                  Qty= @Qty,
                                                                                  FullLocQty = @FullLocQty,
                                                                                  Status = @Status,
                                                                                  Remark=@Remark,    
                                                                                     Editor=@Editor,
                                                                                     Udt = getdate()            
                                                                             output inserted.LocID;";
                        sqlCtx.AddParam("Floor", new SqlParameter("@Floor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Area", new SqlParameter("@Area", SqlDbType.VarChar));
                        sqlCtx.AddParam("Category", new SqlParameter("@Category", SqlDbType.VarChar));
                        sqlCtx.AddParam("Unit", new SqlParameter("@Unit", SqlDbType.VarChar));
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("FullLocQty", new SqlParameter("@FullLocQty", SqlDbType.Int));
                        sqlCtx.AddParam("RemainQty", new SqlParameter("@RemainQty", SqlDbType.Int));
                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));
                        sqlCtx.AddParam("Status", new SqlParameter("@Status", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Floor").Value = floor;
                sqlCtx.Param("Area").Value = area;
                sqlCtx.Param("Category").Value = srcLoc.Category;
                sqlCtx.Param("Unit").Value = srcLoc.Unit;
                sqlCtx.Param("Model").Value = srcLoc.Model;
                sqlCtx.Param("FullLocQty").Value = srcLoc.FullLocQty;
                sqlCtx.Param("RemainQty").Value = srcLoc.RemainQty;
                sqlCtx.Param("Qty").Value = srcLoc.Qty;
                sqlCtx.Param("Status").Value = srcLoc.Status;
                sqlCtx.Param("Editor").Value = editor;
                sqlCtx.Param("Remark").Value = srcLoc.Remark;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                             CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                              sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = ((string)sqlR[0]).Trim();

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void locMoveOutbyProduct(IList<string> productIDList, string attrName, string editor)
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
                        sqlCtx.Sentence = @"with cte(LocID,Qty)
                                                            as(	select a.AttrValue, COUNT(1) as Qty
	                                                            from ProductAttr a
	                                                            inner join @ProductList b on a.ProductID = b.data
	                                                            where a.AttrName=@AttrName and
		                                                              a.AttrValue!=''
	                                                            group by a.AttrValue)

                                                            update FloorAreaLoc
                                                            set Qty=case when a.Qty - b.Qty<=0 then 0
                                                                         else a.Qty - b.Qty end ,
                                                                RemainQty =case when a.Qty - b.Qty<=0 then 0
                                                                                else a.RemainQty+b.Qty end,
                                                                FullLocQty= case when a.Qty - b.Qty<=0 then 0                                                                                             
                                                                                 else a.FullLocQty end,
                                                                Status =  case when a.Qty - b.Qty<=0 then 'Empty'
                                                                                        when a.Qty - b.Qty>=a.FullLocQty then 'Full'
                                                                                         else 'Partial' end,
                                                                Model  = case when a.Qty - b.Qty<=0 then ''
                                                                                 else a.Model end,
                                                                Remark = case when a.Qty - b.Qty<=0 then ''
                                                                                 else a.Remark end,
                                                                Unit=case when a.Qty - b.Qty<=0 then ''
                                                                                 else a.Unit end,
                                                                Editor =@Editor,
                                                                Udt=GETDATE()
                                                            from FloorAreaLoc a
                                                            inner join cte b on a.LocID = b.LocID;    ";
                        SqlParameter para = new SqlParameter("@ProductList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        para.Direction = ParameterDirection.Input;
                        sqlCtx.AddParam("ProductList", para);
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ProductList").Value = _Schema.SQLData.ToDataTable(productIDList);
                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("Editor").Value = editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
                                                                                              CommandType.Text,
                                                                                               sqlCtx.Sentence,
                                                                                               sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void locMoveOutbyLocID(string locID, string attrName, string editor)
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
                        sqlCtx.Sentence = @"with cte(LocID,Qty)
                                                            as(	select @LocID, COUNT(1) as Qty
	                                                            from ProductAttr a	                                                           
	                                                            where a.AttrName=@AttrName and
		                                                              a.AttrValue =@LocID)

                                                            update FloorAreaLoc
                                                            set Qty=case when a.Qty - b.Qty<=0 then 0
                                                                         else a.Qty - b.Qty end ,
                                                                RemainQty =case when a.Qty - b.Qty<=0 then 0
                                                                                else a.RemainQty+b.Qty end,
                                                                FullLocQty= case when a.Qty - b.Qty<=0 then 0
                                                                                 else a.FullLocQty end,
                                                                Status =  case when a.Qty - b.Qty<=0 then 'Empty'
                                                                                       when a.Qty - b.Qty>=a.FullLocQty then 'Full'
                                                                                 else 'Partial' end,
                                                                Model  = case when a.Qty - b.Qty<=0 then ''
                                                                                 else a.Model end,
                                                                Remark  = case when a.Qty - b.Qty<=0 then ''
                                                                                 else a.Remark end,
                                                                Unit =case when a.Qty - b.Qty<=0 then ''
                                                                                 else a.Unit end,
                                                                Editor =@Editor,
                                                                Udt=GETDATE()
                                                            from FloorAreaLoc a
                                                            inner join cte b on a.LocID = b.LocID;    ";

                        sqlCtx.AddParam("LocID", new SqlParameter("@LocID", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("LocID").Value = locID;
                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("Editor").Value = editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
                                                                                              CommandType.Text,
                                                                                               sqlCtx.Sentence,
                                                                                               sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region disable code
//        private string updateProductAttr(string productID, string attrName, string value, string editor)
//        {
//            try
//            {
//                string ret = null;
//                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"merge ProductAttr as T
//                                                        using (select @ProductID as ProductID, 
//                                                                     @AttrName as AttrName,
//                                                                     @AttrValue as AttrValue,
//                                                                     @Editor    as Editor) as S
//                                                        on (T.ProductID =S.ProductID and
//                                                           T.AttrName = S.AttrName)
//                                                        WHEN MATCHED Then
//                                                           Update Set AttrValue=S.AttrValue,
//                                                                      Editor =S.Editor,
//                                                                      Udt=getdate()
//                                                        WHEN Not MATCHED Then
//                                                          INSERT (ProductID,AttrName, AttrValue, Editor, Cdt, Udt) 
//                                                          VALUES (S.ProductID, S.AttrName, S.AttrValue, S.Editor,getdate() ,getdate())             
//                                                       OUTPUT Deleted.AttrValue;";
//                        sqlCtx.AddParam("ProductID", new SqlParameter("@ProductID", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));  
//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("ProductID").Value = productID;
//                sqlCtx.Param("AttrName").Value = attrName;
//                sqlCtx.Param("AttrValue").Value = value;
//                sqlCtx.Param("Editor").Value = editor;          


//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                                             CommandType.Text,
//                                                                                                                                              sqlCtx.Sentence,
//                                                                                                                                              sqlCtx.Params))
//                {
//                    if (sqlR != null && sqlR.Read())
//                    {
//                        if (sqlR.IsDBNull(0))
//                        {
//                            ret = null;
//                        }
//                        else
//                        {
//                            ret = ((string)sqlR[0]).Trim();
//                        }

//                    }
//                }
//                return ret;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        private void insertProductAttrLog(string productID, string model, string station,
//                                                            string attrName,  string value, string oldValue, string editor)
//        {
//            try
//            {
//               MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"insert ProductAttrLog(ProductID, Model, Station, AttrName, AttrOldValue, 
//					                                                                       AttrNewValue, Descr, Editor, Cdt)
//                                                        Values(@ProductID, @Model, @Station, @AttrName, @AttrOldValue, 
//					                                                @AttrNewValue, '', @Editor, GETDATE())";
//                        sqlCtx.AddParam("ProductID", new SqlParameter("@ProductID", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrOldValue", new SqlParameter("@AttrOldValue", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrNewValue", new SqlParameter("@AttrNewValue", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("ProductID").Value = productID;
//                sqlCtx.Param("Model").Value = model;
//                sqlCtx.Param("Station").Value = station;
//                sqlCtx.Param("AttrName").Value = attrName;
//                sqlCtx.Param("AttrOldValue").Value = oldValue;
//                sqlCtx.Param("AttrNewValue").Value = value;
//                sqlCtx.Param("Editor").Value = editor;


//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
//                                                                                              CommandType.Text,
//                                                                                               sqlCtx.Sentence,
//                                                                                               sqlCtx.Params);
               
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        private void updateProductListAttr(IList<string> productIDList, string attrName, string value, string editor)
//        {
//            try
//            {
//               MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {
//                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"merge ProductAttr as T
//                                                        using (select a.data as ProductID, 
//                                                                     @AttrName as AttrName,
//                                                                     @AttrValue as AttrValue,
//                                                                     @Editor    as Editor
//                                                                   from @ProductList a) as S
//                                                        on (T.ProductID =S.ProductID and
//                                                           T.AttrName = S.AttrName)
//                                                        WHEN MATCHED Then
//                                                           Update Set AttrValue=S.AttrValue,
//                                                                      Editor =S.Editor,
//                                                                      Udt=getdate()
//                                                        WHEN Not MATCHED Then
//                                                          INSERT (ProductID,AttrName, AttrValue, Editor, Cdt, Udt) 
//                                                          VALUES (S.ProductID, S.AttrName, S.AttrValue, S.Editor,getdate() ,getdate());";
                        
//                        SqlParameter para = new SqlParameter("@ProductList", SqlDbType.Structured);
//                        para.TypeName = "TbStringList";
//                        para.Direction = ParameterDirection.Input;
//                        sqlCtx.AddParam("ProductList", para);

//                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }

//                sqlCtx.Param("ProductList").Value = _Schema.SQLData.ToDataTable(productIDList); 
//                sqlCtx.Param("AttrName").Value = attrName;
//                sqlCtx.Param("AttrValue").Value = value;
//                sqlCtx.Param("Editor").Value = editor;


//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK,
//                                                                                                                                             CommandType.Text,
//                                                                                                                                              sqlCtx.Sentence,
//                                                                                                                                              sqlCtx.Params);
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        private void insertProductListAttrLog(IList<string> productIDList, string station,string attrName, string value, string oldValue, string editor)
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
//                        sqlCtx.Sentence = @"insert ProductAttrLog(ProductID, Model, Station, AttrName, AttrOldValue, 
//					                                                                       AttrNewValue, Descr, Editor, Cdt)
//                                                        select a.data, b.Model, @Station, @AttrName, @AttrOldValue, 
//					                                                @AttrNewValue, '', @Editor, GETDATE()
//                                                        from @ProductList a 
//                                                        inner join Product b on a.data = b.ProductID";
//                        SqlParameter para = new SqlParameter("@ProductList", SqlDbType.Structured);
//                        para.TypeName = "TbStringList";
//                        para.Direction = ParameterDirection.Input;
//                        sqlCtx.AddParam("ProductList", para);
                       
//                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrOldValue", new SqlParameter("@AttrOldValue", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrNewValue", new SqlParameter("@AttrNewValue", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
//                sqlCtx.Param("ProductList").Value = _Schema.SQLData.ToDataTable(productIDList);               
//                sqlCtx.Param("Station").Value = station;
//                sqlCtx.Param("AttrName").Value = attrName;
//                sqlCtx.Param("AttrOldValue").Value = oldValue;
//                sqlCtx.Param("AttrNewValue").Value = value;
//                sqlCtx.Param("Editor").Value = editor;


//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
//                                                                                              CommandType.Text,
//                                                                                               sqlCtx.Sentence,
//                                                                                               sqlCtx.Params);

//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }


//        private void clearProductAttr(string attrName, string value, string station, string editor)
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
//                        sqlCtx.Sentence = @"declare @attr TbAttribute
//                                                        update ProductAttr
//                                                        set AttrValue=''
//                                                        output deleted.ProductID, deleted.AttrValue
//                                                        into @attr(Name,Value)
//                                                        where AttrName =@AttrName and
//                                                              AttrValue =@AttrValue
//                                                              
//                                                        insert ProductAttrLog(ProductID, Model, Station, AttrName, 
//                                                                              AttrOldValue, AttrNewValue, Descr, Editor, Cdt)    
//                                                        select a.Name,b.Model,@Station,@AttrName,a.Value,'','',@Editor,GETDATE()
//                                                          from  @attr a
//                                                          inner join Product b on b.ProductID =a.Name";                      
                       
//                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
//                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }
               
//                sqlCtx.Param("Station").Value = station;
//                sqlCtx.Param("AttrName").Value = attrName;
//                sqlCtx.Param("AttrValue").Value = value;
             
//                sqlCtx.Param("Editor").Value = editor;


//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
//                                                                                              CommandType.Text,
//                                                                                               sqlCtx.Sentence,
//                                                                                               sqlCtx.Params);

//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

       

//        private void clearProductAttrByProduct(IList<string> productIDList, string attrName, string station, string editor)
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
//                        sqlCtx.Sentence = @"declare @attr TbAttribute
//                                                        update ProductAttr
//                                                        set AttrValue=''
//                                                        from  ProductAttr a
//                                                        inner join @ProductList b on a.ProductID = b.data
//                                                        output deleted.ProductID, deleted.AttrValue
//                                                        into @attr(Name,Value)
//                                                        where a.AttrName =@AttrName 
//                                                              
//                                                        insert ProductAttrLog(ProductID, Model, Station, AttrName, 
//                                                                              AttrOldValue, AttrNewValue, Descr, Editor, Cdt)    
//                                                        select a.Name,b.Model,@Station,@AttrName,a.Value,'','',@Editor,GETDATE()
//                                                          from  @attr a
//                                                          inner join Product b on b.ProductID =a.Name";

//                        SqlParameter para = new SqlParameter("@ProductList", SqlDbType.Structured);
//                        para.TypeName = "TbStringList";
//                        para.Direction = ParameterDirection.Input;
//                        sqlCtx.AddParam("ProductList", para);

//                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));                       
//                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
//                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }

//                sqlCtx.Param("Station").Value = station;
//                sqlCtx.Param("AttrName").Value = attrName;
//                sqlCtx.Param("ProductList").Value = _Schema.SQLData.ToDataTable(productIDList);  

//                sqlCtx.Param("Editor").Value = editor;


//                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
//                                                                                              CommandType.Text,
//                                                                                               sqlCtx.Sentence,
//                                                                                               sqlCtx.Params);

//            }
//            catch (Exception)
//            {
//                throw;
//            }
        //        }
        #endregion

        public void AssignLocAndMoveIn(string productID, string productAttrName, string floor,
                                                string area, string category, string model, string unit, string editor, int fullQty,
                                                string station, string palletType)
        {
            string loc = assignLoc(floor, area, category, model, unit, editor, fullQty,palletType);
            if (string.IsNullOrEmpty(loc))
            {
                throw new FisException("CQCHK50028", new List<string> { floor, area,model,category,unit});
            }

            UpdateProductListAttr(new List<string> { productID }, productAttrName, loc, station, editor);
            //string oldValue = updateProductAttr(productID, productAttrName, loc, editor);
            //if (!string.IsNullOrEmpty(oldValue))
            //{
            //    insertProductAttrLog(productID, model, station, productAttrName, loc, oldValue, editor);
            //}
        }

        public void AssignLocAndMoveInDefered(IUnitOfWork uow, string productID, string productAttrName, string floor,
                                                            string area, string category, string model, string unit, string editor, int fullQty,
                                                           string station,string palletType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), productID, productAttrName, floor,
                                            area, category, model, unit, editor, fullQty, station, palletType);
        }

        

        public void LocMoveOut(fons.FloorAreaLoc loc, string productAttrName, string station, string editor)
        {
            //reset location
            //loc.Model = "";
            //loc.Qty = 0;
            //loc.RemainQty = 0;
            //loc.FullLocQty = 0;
            //loc.Status = "Empty";
            //loc.Remark = "";
            //PersistUpdatedItem(loc);
            locMoveOutbyLocID(loc.LocID, productAttrName, editor);
            //clearProductAttr(productAttrName, loc.LocID, station, editor);  
            UpdateProductAttr(productAttrName, loc.LocID, "", station, editor);

        }

        public void LocMoveOutDefered(IUnitOfWork uow, fons.FloorAreaLoc loc, string productAttrName, string station, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), loc, productAttrName,station,editor);
        }

        public IList<fons.FloorAreaLoc> GetLocation(fons.FloorAreaLoc condition)
        {
            try
            {
                IList<fons.FloorAreaLoc> ret = new List<fons.FloorAreaLoc>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    _Metas.FloorAreaLoc cond = FuncNew.SetColumnFromField<_Metas.FloorAreaLoc, fons.FloorAreaLoc>(condition);
                     sqlCtx = FuncNew.GetConditionedSelect<_Metas.FloorAreaLoc>(null, null,
                                                                                      new ConditionCollection<_Metas.FloorAreaLoc>(new EqualCondition<_Metas.FloorAreaLoc>(cond)));

                 
                }
                sqlCtx = FuncNew.SetColumnFromField<_Metas.FloorAreaLoc, fons.FloorAreaLoc>(sqlCtx, condition);


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<_Metas.FloorAreaLoc, fons.FloorAreaLoc, fons.FloorAreaLoc>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LocMoveOutByProductIDList(IList<string> productIDList, string productAttrName, string station, string editor)
        {
            locMoveOutbyProduct(productIDList, productAttrName, editor);
            //clearProductAttrByProduct(productIDList, productAttrName, station, editor);
            UpdateProductListAttr(productIDList, productAttrName, "", station, editor);
        }
        public void LocMoveOutByProductIDListDefered(IUnitOfWork uow, IList<string> productIDList, string productAttrName, string station, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), productIDList, productAttrName, station,editor);
        }


        public void TransferToLoc(IList<string> productIDList, string productAttrName, fons.FloorAreaLoc srcLoc, string floor, string area,
                                                string editor, string station)
        {
            string loc = assignEmptyLoc(srcLoc, floor, area, editor);           
            if (string.IsNullOrEmpty(loc))
            {
                throw new FisException("CQCHK50028", new List<string> { floor, area, srcLoc.Model, srcLoc.Category, srcLoc.Unit });
            }

             //updateProductListAttr(productIDList, productAttrName, loc, editor);

             //insertProductListAttrLog(productIDList, station, productAttrName, loc, srcLoc.LocID, editor);

             UpdateProductListAttr(productIDList, productAttrName, loc, station, editor);
           

        }
        public void TransferToLocDefered(IUnitOfWork uow, IList<string> productIDList, string productAttrName, fons.FloorAreaLoc srcLoc, string floor,
                                                            string area, string editor, string station)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), productIDList, productAttrName,srcLoc, floor,
                                            area, editor,station);
        }

        public IList<string> GetProductIDListByLoc(string productAttrName, string locID)
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
                        sqlCtx.Sentence = @"Select ProductID from ProductAttr where AttrName=@AttrName and AttrValue=@AttrValue";
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
                       
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("AttrName").Value = productAttrName;
                sqlCtx.Param("AttrValue").Value = locID;
               


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                             CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                              sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while(sqlR.Read())
                        {
                            ret.Add(((string)sqlR[0]).Trim());
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

        public void UpdateProductAttr(string attrName, string oldValue, string newValue, string station, string editor)
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
                        sqlCtx.Sentence = @"declare @attr TbAttribute
                                                        update ProductAttr
                                                        set AttrValue=@NewAttrValue
                                                        output deleted.ProductID, deleted.AttrValue
                                                        into @attr(Name,Value)
                                                        where AttrName =@AttrName and
                                                              AttrValue =@AttrValue
                                                              
                                                        insert ProductAttrLog(ProductID, Model, Station, AttrName, 
                                                                              AttrOldValue, AttrNewValue, Descr, Editor, Cdt)    
                                                        select a.Name,b.Model,@Station,@AttrName,a.Value,@NewAttrValue,'',@Editor,GETDATE()
                                                          from  @attr a
                                                          inner join Product b on b.ProductID =a.Name
                                                          where a.Value is not null";

                        sqlCtx.AddParam("NewAttrValue", new SqlParameter("@NewAttrValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Station").Value = station;
                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("AttrValue").Value = oldValue;
                sqlCtx.Param("NewAttrValue").Value = newValue;
                sqlCtx.Param("Editor").Value = editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_FA,
                                                                                              CommandType.Text,
                                                                                               sqlCtx.Sentence,
                                                                                               sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProductAttrDefered(IUnitOfWork uow, string attrName, string oldValue, string newValue, string station, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), attrName, oldValue, newValue, station,editor );
        }


        public  void UpdateProductListAttr(IList<string> productIDList, string attrName, string value, string station, string editor)
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
                        sqlCtx.Sentence = @"declare @attr TbAttribute;
                                                        merge ProductAttr as T
                                                        using (select a.data as ProductID, 
                                                                     @AttrName as AttrName,
                                                                     @AttrValue as AttrValue,
                                                                     @Editor    as Editor
                                                                   from @ProductList a) as S
                                                        on (T.ProductID =S.ProductID and
                                                           T.AttrName = S.AttrName)
                                                        WHEN MATCHED Then
                                                           Update Set AttrValue=S.AttrValue,
                                                                      Editor =S.Editor,
                                                                      Udt=getdate()
                                                        WHEN Not MATCHED Then
                                                          INSERT (ProductID,AttrName, AttrValue, Editor, Cdt, Udt) 
                                                          VALUES (S.ProductID, S.AttrName, S.AttrValue, S.Editor,getdate() ,getdate())
                                                        output inserted.ProductID, deleted.AttrValue
                                                        into @attr(Name,Value);
                                                        insert ProductAttrLog(ProductID, Model, Station, AttrName, 
                                                                              AttrOldValue, AttrNewValue, Descr, Editor, Cdt)    
                                                        select a.Name,b.Model,@Station,@AttrName,a.Value,@AttrValue,'',@Editor,GETDATE()
                                                          from  @attr a
                                                          inner join Product b on b.ProductID =a.Name
                                                         where a.Value is not null";

                        SqlParameter para = new SqlParameter("@ProductList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        para.Direction = ParameterDirection.Input;
                        sqlCtx.AddParam("ProductList", para);

                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        sqlCtx.AddParam("AttrValue", new SqlParameter("@AttrValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("ProductList").Value = _Schema.SQLData.ToDataTable(productIDList);
                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("AttrValue").Value = value;
                sqlCtx.Param("Station").Value = station;
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
        public void UpdateProductListAttrDefered(IUnitOfWork uow, IList<string> productIDList, string attrName, string value, string station, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), productIDList, attrName, value, station, editor);
        }

        public  IList<AttributeInfo> GetProductListAttr(IList<string> productIDList, string attrName)
        {
            try
            {
                IList<AttributeInfo> ret = new List<AttributeInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"Select a.ProductID as Name , a.AttrValue as Value 
                                                         from ProductAttr a 
                                                         inner join   @ProductList b on a.ProductID = b.data        
                                                         where AttrName=@AttrName 
                                                        order by  a.ProductID, a.Udt desc";
                        
                        sqlCtx.AddParam("AttrName", new SqlParameter("@AttrName", SqlDbType.VarChar));
                        SqlParameter para = new SqlParameter("@ProductList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";
                        para.Direction = ParameterDirection.Input;
                        sqlCtx.AddParam("ProductList", para);

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("AttrName").Value = attrName;
                sqlCtx.Param("ProductList").Value = _Schema.SQLData.ToDataTable(productIDList); 



                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                                             CommandType.Text,
                                                                                                                                              sqlCtx.Sentence,
                                                                                                                                              sqlCtx.Params))
                {
                    if (sqlR != null  )
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(_Schema.SQLData.ToObject<AttributeInfo>(sqlR));
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

       
    }
}
