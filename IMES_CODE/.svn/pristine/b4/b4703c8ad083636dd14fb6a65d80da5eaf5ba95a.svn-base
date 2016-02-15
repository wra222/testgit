using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;

using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;

using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.PAK
{
    public class PalletTypeRepository : BaseRepository<PalletType>  , IPalletTypeRepository
    {
        #region Overrides of BaseRepository<PalletType>

        protected override void PersistNewItem(PalletType item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsert(item); 
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(PalletType item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified )
                {                   
                        this.PersistUpdate(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(PalletType item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistRemove(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<PalletType>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override PalletType Find(object key)
        {
            try
            {
                PalletType ret = null;
               
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet,Editor, Cdt, Udt,
                                                                    isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ID=@ID";
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ID").Value = (int)key;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = SQLData.ToObject<PalletType>(sqlR);
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
        public override IList<PalletType> FindAll()
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();
               
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet, Editor, Cdt, Udt,
                                                                         isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType";
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
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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
        public override void Add(PalletType item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        /// <param name="uow"></param>
        public override void Remove(PalletType item, IUnitOfWork uow)
        {
            base.Remove(item, uow); 
            
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(PalletType item, IUnitOfWork uow)
        {
            base.Update(item, uow);

        }



        #endregion

        #region . Inners .
       
        private void PersistInsert(PalletType item)
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
                        sqlCtx.Sentence = @"Insert PalletType(ShipWay, RegId, Type, StdPltFullQty, MaxQty, 
                                                                                      MinQty, Code, PltWeight, MinusPltWeight,Editor, 
                                                                                      CheckCode,ChepPallet, Cdt, Udt,
                                                                                       PalletLayer,OceanType)
                                                                    Values(@ShipWay, @RegId, @Type, @StdPltFullQty, @MaxQty, 
                                                                           @MinQty, @Code, @PltWeight, @MinusPltWeight,@Editor, 
                                                                           @CheckCode,@ChepPallet, GETDATE(), GETDATE(),
                                                                            @PalletLayer,@OceanType)";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        sqlCtx.AddParam("Type", new SqlParameter("@Type", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("MaxQty", new SqlParameter("@MaxQty", SqlDbType.Int));

                        sqlCtx.AddParam("MinQty", new SqlParameter("@MinQty", SqlDbType.Int));
                        sqlCtx.AddParam("Code", new SqlParameter("@Code", SqlDbType.VarChar));
                        sqlCtx.AddParam("PltWeight", new SqlParameter("@PltWeight", SqlDbType.Decimal));
                        sqlCtx.AddParam("MinusPltWeight", new SqlParameter("@MinusPltWeight", SqlDbType.VarChar));
                        sqlCtx.AddParam("CheckCode", new SqlParameter("@CheckCode", SqlDbType.VarChar));

                        sqlCtx.AddParam("ChepPallet", new SqlParameter("@ChepPallet", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletLayer", new SqlParameter("@PalletLayer", SqlDbType.Int));
                        sqlCtx.AddParam("OceanType", new SqlParameter("@OceanType", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = item.ShipWay;
                sqlCtx.Param("RegId").Value = item.RegId;
                sqlCtx.Param("Type").Value = item.Type;
                sqlCtx.Param("StdPltFullQty").Value = item.StdPltFullQty;
                sqlCtx.Param("MaxQty").Value = item.MaxQty;

                sqlCtx.Param("MinQty").Value = item.MinQty;
                sqlCtx.Param("Code").Value = item.Code;
                sqlCtx.Param("PltWeight").Value = item.PltWeight;
                sqlCtx.Param("MinusPltWeight").Value = item.MinusPltWeight;
                sqlCtx.Param("CheckCode").Value = item.CheckCode;

                sqlCtx.Param("ChepPallet").Value = item.ChepPallet;
                sqlCtx.Param("Editor").Value = item.Editor;
                sqlCtx.Param("PalletLayer").Value = item.PalletLayer;
                sqlCtx.Param("OceanType").Value = item.OceanType;

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
      
        

        private void PersistUpdate(PalletType item)
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
                        sqlCtx.Sentence = @"Update PalletType
                                                            set ShipWay=@ShipWay, RegId=@RegId, Type=@Type, StdPltFullQty=@StdPltFullQty, MaxQty=@MaxQty, 
                                                                  MinQty=@MinQty, Code=@Code, PltWeight=@PltWeight, MinusPltWeight=@MinusPltWeight,
                                                                  CheckCode=@CheckCode,ChepPallet=@ChepPallet,Editor=@Editor,Udt=GETDATE(),
                                                                    PalletLayer=@PalletLayer,OceanType=@OceanType
                                                             where ID=@ID";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        sqlCtx.AddParam("Type", new SqlParameter("@Type", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("MaxQty", new SqlParameter("@MaxQty", SqlDbType.Int));

                        sqlCtx.AddParam("MinQty", new SqlParameter("@MinQty", SqlDbType.Int));
                        sqlCtx.AddParam("Code", new SqlParameter("@Code", SqlDbType.VarChar));
                        sqlCtx.AddParam("PltWeight", new SqlParameter("@PltWeight", SqlDbType.Decimal));
                        sqlCtx.AddParam("MinusPltWeight", new SqlParameter("@MinusPltWeight", SqlDbType.VarChar));
                        sqlCtx.AddParam("CheckCode", new SqlParameter("@CheckCode", SqlDbType.VarChar));

                        sqlCtx.AddParam("ChepPallet", new SqlParameter("@ChepPallet", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletLayer", new SqlParameter("@PalletLayer", SqlDbType.Int));
                        sqlCtx.AddParam("OceanType", new SqlParameter("@OceanType", SqlDbType.VarChar));

                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = item.ShipWay;
                sqlCtx.Param("RegId").Value = item.RegId;
                sqlCtx.Param("Type").Value = item.Type;
                sqlCtx.Param("StdPltFullQty").Value = item.StdPltFullQty;
                sqlCtx.Param("MaxQty").Value = item.MaxQty;

                sqlCtx.Param("MinQty").Value = item.MinQty;
                sqlCtx.Param("Code").Value = item.Code;
                sqlCtx.Param("PltWeight").Value = item.PltWeight;
                sqlCtx.Param("MinusPltWeight").Value = item.MinusPltWeight;
                sqlCtx.Param("CheckCode").Value = item.CheckCode;

                sqlCtx.Param("ChepPallet").Value = item.ChepPallet;
                sqlCtx.Param("Editor").Value = item.Editor;
                sqlCtx.Param("PalletLayer").Value = item.PalletLayer;
                sqlCtx.Param("OceanType").Value = item.OceanType;

                sqlCtx.Param("ID").Value = item.ID;


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


        private void PersistRemove(PalletType item)
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
                        sqlCtx.Sentence = @"Delete from PalletType
                                                            where ID=@ID";
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("ID").Value = item.ID;

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

        #region implement  IPalletTypeRepository

        public void RemovePalletType(int id)
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
                        sqlCtx.Sentence = @"Delete from PalletType
                                                            where ID=@ID";
                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("ID").Value = id;

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

        public void RemovePalletTypeDefered(IUnitOfWork uow, int id)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id);
        }

        public IList<PalletType> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int maxQty, int minQty)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                      
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                      @MaxQty between MinQty and MaxQty 
                                                                Union
                                                                select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                     
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                      @MinQty between MinQty and MaxQty
                                                                union
                                                                select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                     
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                     MaxQty between  @MinQty and @MaxQty  and
                                                                    MinQty between  @MinQty and @MaxQty";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("MaxQty", new SqlParameter("@MaxQty", SqlDbType.Int));
                        sqlCtx.AddParam("MinQty", new SqlParameter("@MinQty", SqlDbType.Int));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;
                sqlCtx.Param("StdPltFullQty").Value = stdPltFullQty;
                sqlCtx.Param("MaxQty").Value = maxQty;
                sqlCtx.Param("MinQty").Value = minQty;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int palletLayer, int maxQty, int minQty)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                      
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                        PalletLayer=@PalletLayer        and 
                                                                      @MaxQty between MinQty and MaxQty 
                                                                Union
                                                                select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                     
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                       PalletLayer=@PalletLayer        and  
                                                                      @MinQty between MinQty and MaxQty
                                                                union
                                                                select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                     
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                      PalletLayer=@PalletLayer        and
                                                                     MaxQty between  @MinQty and @MaxQty  and
                                                                    MinQty between  @MinQty and @MaxQty";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletLayer", new SqlParameter("@PalletLayer", SqlDbType.Int));
                        sqlCtx.AddParam("MaxQty", new SqlParameter("@MaxQty", SqlDbType.Int));
                        sqlCtx.AddParam("MinQty", new SqlParameter("@MinQty", SqlDbType.Int));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;
                sqlCtx.Param("StdPltFullQty").Value = stdPltFullQty;
                sqlCtx.Param("PalletLayer").Value = palletLayer;
                sqlCtx.Param("MaxQty").Value = maxQty;
                sqlCtx.Param("MinQty").Value = minQty;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int palletLayer, string oceanType, int maxQty, int minQty)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                      
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                       PalletLayer=@PalletLayer        and 
                                                                       OceanType=@OceanType        and  
                                                                      @MaxQty between MinQty and MaxQty 
                                                                Union
                                                                select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                     
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                       PalletLayer=@PalletLayer        and
                                                                       OceanType=@OceanType        and     
                                                                      @MinQty between MinQty and MaxQty
                                                                union
                                                                select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                     
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                      PalletLayer=@PalletLayer        and
                                                                      OceanType=@OceanType        and    
                                                                     MaxQty between  @MinQty and @MaxQty  and
                                                                    MinQty between  @MinQty and @MaxQty";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletLayer", new SqlParameter("@PalletLayer", SqlDbType.Int));
                        sqlCtx.AddParam("OceanType", new SqlParameter("@OceanType", SqlDbType.VarChar));
                        sqlCtx.AddParam("MaxQty", new SqlParameter("@MaxQty", SqlDbType.Int));
                        sqlCtx.AddParam("MinQty", new SqlParameter("@MinQty", SqlDbType.Int));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;
                sqlCtx.Param("StdPltFullQty").Value = stdPltFullQty;
                sqlCtx.Param("PalletLayer").Value = palletLayer;
                sqlCtx.Param("OceanType").Value = oceanType;
                sqlCtx.Param("MaxQty").Value = maxQty;
                sqlCtx.Param("MinQty").Value = minQty;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> GetPalletType(string shipWay, string regId, string stdPltFullQty, int palletLayer, string oceanType, int qty)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet, Editor, Cdt, Udt,
                                                                        isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                          RegId = @RegId   and                                                                     
                                                                         StdPltFullQty = @StdPltFullQty and
                                                                         PalletLayer=@PalletLayer        and
                                                                         OceanType=@OceanType        and 
                                                                        @Qty between MinQty and MaxQty ";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Type", new SqlParameter("@Type", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletLayer", new SqlParameter("@PalletLayer", SqlDbType.Int));
                        sqlCtx.AddParam("OceanType", new SqlParameter("@OceanType", SqlDbType.VarChar));
                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;
                //sqlCtx.Param("Type").Value = type;
                sqlCtx.Param("StdPltFullQty").Value = stdPltFullQty;
                sqlCtx.Param("PalletLayer").Value = palletLayer;
                sqlCtx.Param("OceanType").Value = oceanType;
                sqlCtx.Param("Qty").Value = qty;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> GetPalletType(string shipWay, string regId, string stdPltFullQty, int palletLayer, int qty)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet, Editor, Cdt, Udt,
                                                                        isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                          RegId = @RegId   and                                                                     
                                                                         StdPltFullQty = @StdPltFullQty and
                                                                         PalletLayer=@PalletLayer        and 
                                                                        @Qty between MinQty and MaxQty ";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Type", new SqlParameter("@Type", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("PalletLayer", new SqlParameter("@PalletLayer", SqlDbType.Int));

                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));


                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;
                //sqlCtx.Param("Type").Value = type;
                sqlCtx.Param("StdPltFullQty").Value = stdPltFullQty;
                sqlCtx.Param("PalletLayer").Value = palletLayer;
                sqlCtx.Param("Qty").Value = qty;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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
        public IList<PalletType> GetPalletType(string shipWay, string regId,  string stdPltFullQty, int qty)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet, Editor, Cdt, Udt,
                                                                        isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and                                                                     
                                                                      StdPltFullQty = @StdPltFullQty and
                                                                      @Qty between MinQty and MaxQty ";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Type", new SqlParameter("@Type", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        sqlCtx.AddParam("Qty", new SqlParameter("@Qty", SqlDbType.Int));

                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;
                //sqlCtx.Param("Type").Value = type;
                sqlCtx.Param("StdPltFullQty").Value = stdPltFullQty;
                sqlCtx.Param("Qty").Value = qty;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> GetPalletType(string shipWay, string regId, string stdPltFullQty)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet,Editor, Cdt, Udt,
                                                                        isnull(PalletLayer,0) as PalletLayer, isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId   and
                                                                      StdPltFullQty = @StdPltFullQty  ";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        //sqlCtx.AddParam("Type", new SqlParameter("@Type", SqlDbType.VarChar));
                        sqlCtx.AddParam("StdPltFullQty", new SqlParameter("@StdPltFullQty", SqlDbType.VarChar));
                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;
                //sqlCtx.Param("Type").Value = type;
                sqlCtx.Param("StdPltFullQty").Value = stdPltFullQty;                


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> GetPalletType(string shipWay, string regId)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet, Editor, Cdt, Udt,
                                                                        isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay and
                                                                      RegId = @RegId ";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));
                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;
                sqlCtx.Param("RegId").Value = regId;              


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> GetPalletType(string shipWay)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet, Editor, Cdt, Udt,
                                                                        isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where ShipWay=@ShipWay ";
                        sqlCtx.AddParam("ShipWay", new SqlParameter("@ShipWay", SqlDbType.VarChar));
                      
                        

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("ShipWay").Value = shipWay;                      


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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

        public IList<PalletType> GetPalletTypeByRegId(string regId)
        {
            try
            {
                IList<PalletType> ret = new List<PalletType>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select ID, ShipWay, RegId, Type, StdPltFullQty, 
                                                                       MaxQty, MinQty, Code, PltWeight, MinusPltWeight, 
                                                                       CheckCode,ChepPallet, Editor, Cdt, Udt,
                                                                        isnull(PalletLayer,0) as PalletLayer,isnull(OceanType,'') as OceanType
                                                                from PalletType
                                                                where RegId=@RegId ";
                        sqlCtx.AddParam("RegId", new SqlParameter("@RegId", SqlDbType.VarChar));



                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("RegId").Value = regId;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PalletType item = SQLData.ToObject<PalletType>(sqlR);
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
        
        public IList<string> GetShipWay()
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
                        sqlCtx.Sentence = @"select distinct ShipWay
                                                                from PalletType
                                                                order by ShipWay ";
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

       public  IList<string> GetRegId()
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
                        sqlCtx.Sentence = @"select distinct RegId
                                                                from PalletType
                                                                order by RegId";
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
    }
}
